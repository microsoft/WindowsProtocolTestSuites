// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Opn.Runtime.Messaging;
using Microsoft.Opn.Runtime.Monitoring;
using Microsoft.Opn.Runtime.Utilities;
using Microsoft.Opn.Runtime.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;

namespace Microsoft.Protocols.TestTools.MessageAnalyzer
{
    public class CaptureFileSession : Session
    {
        #region variables
        private bool controlThreadOn;
        private AutoResetEvent controlThreadStopEvent = new AutoResetEvent(false);
        private Thread monitorWorkControlThread;

        private readonly bool catchAllMessages;
        private readonly bool isDisablePersistence;
        private FramesStorage frameStorage = null;
        #endregion

        #region property

        public ICaptureJournal FileCaptureJournal
        {
            get
            {
                return this.captureJournal;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="monitor"></param>
        /// <param name="catchAllMessages">
        /// False by default and not recommanded to set as true because of the performance.
        /// So far, it is only used in Grouping test scenario
        /// </param>
        internal CaptureFileSession(IMonitor monitor, string filePath, bool isDisablePersistence, bool catchAllMessages)
            : base(monitor)
        {
            this.catchAllMessages = catchAllMessages;
            this.isDisablePersistence = isDisablePersistence;

            // Only use a default query, Since the query of CaptureJournal of LiveCapture will be updated when start.
            QueryBuilder qb = monitor.CreateQueryBuilder();
            qb.FromCompleted();
            qb.FromAborted();
            qb.FromStalled();
            Query query = qb.Compile();
            captureJournal = monitor.CreateCaptureJournal(query);
            viewJournal = captureJournal.CreateView(monitor.CreateQueryBuilder().Compile());
            captureJournal.DisablePersistence = isDisablePersistence;

            // Create and initial capture session for capturJournal
            CaptureSession.CreateCaptureSession(monitor, captureJournal, false);
            IMessageReader messageReader = CreateMessageReader(filePath);
            captureJournal.CaptureSession.AddReader(messageReader);

            // Create Monitor work control thread
            monitorWorkControlThread = new Thread(new ThreadStart(MonitorWorkControl));
            monitorWorkControlThread.IsBackground = true;

            if (this.catchAllMessages)
                frameStorage = new FramesStorage();

            // Create Monitor work control thread
            monitorWorkControlThread = new Thread(new ThreadStart(MonitorWorkControl));
            monitorWorkControlThread.IsBackground = true;
        }

        #endregion Constructor

        #region Override methods

        /// <summary>
        /// Start the capture
        /// </summary>
        /// <param name="filter">A filter string</param>
        /// <param name="selectedModels">A List of strings for top level models, if this parameter is not null, capture only give messages on the level of these models</param>
        /// <param name="parse">Whether parsing the message during capturing</param>
        /// <param name="clearExistingMessages">Whether clear existing messages in the capture lists of capture</param>
        /// <param name="onlytoplevelframe">Whether return top level message or not</param>
        public override void Start(string filter = null, bool parse = true, bool clearExistingMessages = false)
        {
            // set event
            if (catchAllMessages)
            {
                captureJournal.MessagePersisted += OnMessagePersisted;
            }

            //Start the capture
            base.Start(filter, parse, clearExistingMessages);

            //Start Monitor work control thread
            controlThreadOn = true;
            monitorWorkControlThread.Start();
        }

        public override void Stop()
        {
            // Close Monitor work control thread
            monitorWorkControlThread.Abort();
            // Wait until thread stoped
            controlThreadStopEvent.WaitOne();
            controlThreadOn = false;

            base.Stop();

            if (catchAllMessages)
            {
                captureJournal.MessagePersisted -= OnMessagePersisted;
            }
        }

        protected override void Dispose(bool disposing)
        {            
            if (!disposed)
            {
                base.Dispose(disposing);
                // Stop monitor work control thread
                if (monitorWorkControlThread != null && monitorWorkControlThread.IsAlive)
                    monitorWorkControlThread.Abort();

                disposed = true;
            }
        }

        #endregion Override methods

        #region Public methods

        /// <summary>
        /// Wait until pasring finished
        /// </summary>
        public void WaitUntilParsingComplete()
        {
            if (!controlThreadOn)
            {
                // If Stop() function has been called, throw a exception
                throw new InvalidOperationException("WaitUntilParsingComplete() function should be called before Stop() function.");
            }

            // Wait for finish signal
            controlThreadStopEvent.WaitOne();
            // Set the finish signal again, so that it can be used in Stop()
            controlThreadStopEvent.Set();
        }

        /// <summary>
        /// Used only when catchAllMessages is true.
        /// Only for Grouping test
        /// </summary>
        /// <returns></returns>
        public List<Message> GetAllMessageList()
        {
            if (catchAllMessages)
                return frameStorage.AllMessages;
            return null;
        }

        /// <summary>
        /// Used only when catchAllMessages is true.
        /// Only for Grouping test
        /// </summary>
        /// <returns></returns>
        public List<Message> GetTopProtocolLevelMessages()
        {
            if (catchAllMessages)
                return frameStorage.TopProtocolLevelMessages;
            return null;
        }
        #endregion Public methods

        #region private methods
        /// <summary>
        /// Create messageReader accroding to file extension
        /// </summary>
        /// <param name="CaptureFile">Capture file path</param>
        /// <returns></returns>
        IMessageReader CreateMessageReader(string CaptureFile)
        {
            IMessageReader reader = null;
            //Create and Initialize Message Meta data loader catalog 
            MessageMetadataLoaderCatalog messageMetadataLoaderCatalog = new MessageMetadataLoaderCatalog();
            messageMetadataLoaderCatalog.Initialize(monitor.Settings.ExtensionLoadPath);

            if (PersistUtils.IsUnparsedProjectFile(CaptureFile))
            {
                //If capture file is an uncompressed project file
                reader = FileMessageReader.CreateReaderFromProjectFile(CaptureFile, messageMetadataLoaderCatalog);
            }
            else if (PersistUtils.IsCompressedUnparsedProjectFile(CaptureFile) || PersistUtils.IsCompressedProjectFile(CaptureFile))
            {
                //If capture file is a compressed project file
                reader = FileMessageReader.CreateReaderFromCompressedFile(CaptureFile, messageMetadataLoaderCatalog);
            }
            else
            {
                //other file type, such as netmon capture file
                FileReaderCatalog fileReaderCatalog = new FileReaderCatalog();
                fileReaderCatalog.Initialize(monitor.Settings.ExtensionLoadPath);
                IFileLoader fileLoader = fileReaderCatalog.GetRegisteredFileLoaders(CaptureFile).First();
                reader = fileLoader.OpenFile(CaptureFile, fileLoader.CreateDefaultConfig(null));
            }
            return reader;
        }

        /// <summary>
        /// Function for monitor work control thread.
        /// </summary>
        private void MonitorWorkControl()
        {
            try
            {
                while (monitor.Work(TimeSpan.FromMilliseconds(monitorWorkInterval)))
                {
                    // Sleep one milliseconds, so the other thread can be scheduled if necessary
                    Thread.Sleep(1);
                }
            }
            catch (ThreadAbortException)
            {
                // Catch the ThreadAbortException. 
            }
            finally
            {
                controlThreadStopEvent.Set();
            }
        }

        /// <summary>
        /// This function is called when message persisted. All messages in the Message Store will be persisted.
        /// This function is used when catchAllMessages is true. It is for group testing scenario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="messageValue"></param>
        protected void OnMessagePersisted(object sender, MessageValue messageValue)
        {
            lock (frameStorage)
            {
                frameStorage.AddMessage(messageValue, monitor);
            }
        }
        #endregion
    }

    /// <summary>
    /// In this structure, message is stored group by Frame Number.
    /// 
    /// When adding message into this storage, it should calculate the protocol level.
    /// Protocol level is the level of protocol stack. For example, in below protocol stack,
    /// CapFile message with protocol level 0;
    /// Ethernet message with protocol level 1;
    /// IPv4 message with protocol level 2;
    /// IPv4
    ///   + Ethernet
    ///     + CapFile
    ///     
    /// When message persisted, the message will be inserted to its related frame number message bucket.
    /// If the message dost not have children, set its protocol level to ZERO and insert into 
    /// stored message dictionary directly.
    /// If the message has children, then find out its children in the stored message dictionary.
    /// If one of its children already in the stored message dictionary, set the protocol level as
    /// its child + 1 and insert into stored message dictionary.
    /// If none of its children already in the stored message dictionary, insert into un-stored
    /// message dictionary.
    /// 
    /// For the messages in un-stored message dictionary, it will have the chance to move to stored
    /// message dictionary when new message persisted. If the new message persisted is the child of its children,
    /// it will be moved from un-stored message dictionary to stored message dictionary and set protocol level.
    /// </summary>
    internal class FramesStorage
    {
        private SortedDictionary<uint, FrameMessagesBucket> frames;

        public List<Message> AllMessages
        {
            get
            {
                List<Message> messages = new List<Message>();
                foreach(var item in frames)
                {
                    messages.AddRange(item.Value.AllMessages);
                }

                return messages;
            }
        }

        public List<Message> TopProtocolLevelMessages
        {
            get
            {
                List<Message> messages = new List<Message>();
                foreach (var item in frames)
                {
                    messages.AddRange(item.Value.TopProtocolLevelMessages);
                }

                return messages;
            }
        }

        internal FramesStorage()
        {
            frames = new SortedDictionary<uint, FrameMessagesBucket>();
        }

        internal void AddMessage(MessageValue messageValue, IMonitor monitor)
        {
            Message message = new Message(messageValue, monitor);
            uint messageNumber = message.MessageNumber;

            if (!frames.ContainsKey(messageNumber))
            {
                frames.Add(messageNumber, new FrameMessagesBucket(messageNumber));
            }

            frames[messageNumber].AddMessage(message);
        }
    }

    internal class FrameMessagesBucket
    {
        #region Fields
        // dictionary to store the final result
        private SortedDictionary<MessageId, Message> storedMessages;
        // direction to store the message which its children is not coming
        private Dictionary<MessageId, Message> notStoredMessages;

        private List<Message> topProtocolLevelMessage;
        private readonly uint frameNumber;
        #endregion

        #region Properties
        internal List<Message> AllMessages { get { return storedMessages.Values.ToList<Message>(); } }
        internal List<Message> TopProtocolLevelMessages { get { return topProtocolLevelMessage; } }
        #endregion

        internal FrameMessagesBucket(uint frameNumber)
        {
            this.frameNumber = frameNumber;
            storedMessages = new SortedDictionary<MessageId, Message>();
            topProtocolLevelMessage = new List<Message>();
            notStoredMessages = new Dictionary<MessageId, Message>();
        }

        internal void AddMessage(Message message)
        {
            Contract.Assert(message.MessageNumber == this.frameNumber);

            if (this.storedMessages.ContainsKey(message.MessageId) ||
                this.notStoredMessages.ContainsKey(message.MessageId))
            {
                // check whether the message has been add before
                return;
            }

            // check whether message contains child
            bool shouldAddInStoredMessageList = false;
            var childMessageIds = message.GetChildMessageIds();
            if (childMessageIds == null || childMessageIds.Count() == 0)
            {
                // protocol level is 0
                message.ProtocolLevel = 0;
                shouldAddInStoredMessageList = true;
            }
            else
            {
                foreach (var childMessageId in childMessageIds)
                {
                    if (this.storedMessages.ContainsKey(childMessageId))
                    {
                        message.ProtocolLevel = this.storedMessages[childMessageId].ProtocolLevel + 1;
                        shouldAddInStoredMessageList = true;
                        break;
                    }
                }
            }

            // should be add to stored message list
            if (shouldAddInStoredMessageList)
            {
                AddTopLevelMessageList(message);
                storedMessages.Add(message.MessageId, message);
                MoveNotStoredMessageToStoredMessageList(message);
                return;
            }

            // for those message whose child is not persisted yet, its protocol level cannot be determined
            // store them in the not stored message list temporary
            storedMessages.Add(message.MessageId, message);
        }

        private void AddTopLevelMessageList(Message message)
        {
            if (this.topProtocolLevelMessage.Count == 0
                || this.topProtocolLevelMessage.First().ProtocolLevel == message.ProtocolLevel)
            {
                // add directly
                topProtocolLevelMessage.Add(message);
            }
            else if (this.topProtocolLevelMessage.First().ProtocolLevel < message.ProtocolLevel)
            {
                this.topProtocolLevelMessage.Clear();
                this.topProtocolLevelMessage.Add(message);
            }
        }

        private void MoveNotStoredMessageToStoredMessageList(Message newComingMessage)
        {
            List<MessageId> removedList = new List<MessageId>();

            foreach(var item in notStoredMessages)
            {
                MessageIds childMessageIds = item.Value.GetChildMessageIds();
                if (childMessageIds.Contains(newComingMessage.MessageId))
                {
                    // move to stored message list
                    item.Value.ProtocolLevel = newComingMessage.ProtocolLevel + 1;

                    removedList.Add(item.Key);
                    AddTopLevelMessageList(item.Value);
                    storedMessages.Add(item.Key, item.Value);

                    // move the message whose children contains the message
                    MoveNotStoredMessageToStoredMessageList(item.Value);
                }
            }

            foreach (var messageId in removedList)
                notStoredMessages.Remove(messageId);
        }
    }
}
