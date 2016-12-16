// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Opn.Runtime.Monitoring;
using Microsoft.Opn.Runtime.Utilities;
using Microsoft.Opn.Runtime.Values;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestTools.MessageAnalyzer
{
    public abstract class Session : IDisposable
    {
        #region variables

        // Monitor work internal (milliseconds)
        protected uint monitorWorkInterval = 150;

        protected readonly IMonitor monitor;
        protected ICaptureJournal captureJournal = null;
        protected IViewJournal viewJournal = null;
        // A message list which contains all captured messages
        protected SortedDictionary<MessageId, Message> capturedMessageList = new SortedDictionary<MessageId, Message>();
        // Track whether Dispose has been called.
        protected bool disposed = false;

        private bool isUsingSingleProcessor = false;

        #endregion variables

        #region Property

        public MessageIds CapturedMessageIds
        {
            get
            {
                return new MessageIds(capturedMessageList.Keys);
            }
        }

        /// <summary>
        /// Get captured messages
        /// </summary>
        /// <returns></returns>
        public virtual IList<Message> CapturedMessagesList
        {
            get
            {
                // If selectOnly is false, return all captured messages
                return capturedMessageList.Values.ToList<Message>();
            }
        }

        public virtual IList<Message> SortedCapturedMessagesList
        {
            get
            {
                return SortMessageList(this.CapturedMessagesList);
            }
        }

        /// <summary>
        /// Return messages list which also contains operations's children
        /// Used by Test Suite Team
        /// </summary>
        public virtual IList<Message> SortedMessagesWithOperationChildrenList
        {
            get
            {
                IList<Message> messageList = this.CapturedMessagesList;
                foreach(var item in this.capturedMessageList)
                {
                    if (item.Value.IsOperation())
                    {
                        foreach (var child in item.Value.GetChildMessages())
                            messageList.Add(child);
                    }
                }

                return SortMessageList(messageList);
            }
        }

        /// <summary>
        /// Interval of monitor work (Milliseconds)
        /// </summary>
        public uint MonitorWorkInterval
        {
            get
            {
                return monitorWorkInterval;
            }
            set
            {
                monitorWorkInterval = value;
            }
        }

        /// <summary>
        /// Property used for performance workaround
        /// </summary>
        public bool IsUsingSingleProcessor
        {
            get
            {
                return isUsingSingleProcessor;
            }
            set
            {
                isUsingSingleProcessor = value;
            }
        }

        #endregion Property

        #region Abstract methods

        /// <summary>
        /// Start the capture
        /// </summary>
        /// <param name="filter">A filter string</param>
        /// <param name="parse">Whether parsing the message during capturing</param>
        /// <param name="clearExistingMessages">Whether clear existing messages in the capture lists of capture</param>
        public virtual void Start(string filter = null, bool parse = true, bool clearExistingMessages = false)
        {
            // clear
            if (clearExistingMessages)
            {
                ClearExistingMessage();
            }

            // set event
            viewJournal.ContentChanged += OnViewJournalContentChanged;

            // Create and Set query
            QueryBuilder queryBuilder = monitor.CreateQueryBuilder();
            queryBuilder.FromCompleted();
            queryBuilder.FromAborted();
            queryBuilder.FromStalled();

            if (filter != null)
                queryBuilder.WhereFilter(filter);

            if (!parse)
                captureJournal.DisableParsing = true;
            captureJournal.Query = queryBuilder.Compile();

            // Start Session and Journal
            captureJournal.Start(clearExistingMessages);
            captureJournal.CaptureSession.Start();
            viewJournal.Start(clearExistingMessages);
        }

        /// <summary>
        /// Stop the capture
        /// </summary>
        public virtual void Stop()
        {
            // Stop capture session first
            captureJournal.CaptureSession.Stop();
            // Stop captureJournal and viewJournal
            captureJournal.Stop();
            viewJournal.Stop();

            viewJournal.ContentChanged -= OnViewJournalContentChanged;
        }

        #endregion Abstract methods

        #region Public methods

        /// <summary>
        /// Save captured message into a file.
        /// </summary>
        /// <param name="filepath">File path to save captured messages</param>
        /// <param name="selectedOnly">Indicate save only selected messages or all messages</param>
        /// <returns></returns>
        public virtual bool SaveCapturedMessages(string filepath)
        {
            // Create and initialize a FileWriterCatalog
            FileWriterCatalog writerCatalog = new FileWriterCatalog();
            writerCatalog.Initialize(monitor.Settings.ExtensionLoadPath);
            if (PersistUtils.IsPersistTypeSupported(writerCatalog, filepath))
            {
                MessageIds capturedMessageids = CapturedMessageIds;
                // Save All messages
                if (capturedMessageids.Count > 0)
                {
                    PersistUtils.Persist(captureJournal, writerCatalog, filepath, capturedMessageids);
                    return true;
                }
            }
            return false;
        }

        #region Dispose
        /// <summary>
        /// Dispose this Capture object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (captureJournal != null && captureJournal.DisableParsing)
                {
                    captureJournal.DisableParsing = false;
                }

                if (disposing)
                {
                    //Dispose view journal if existed
                    if (viewJournal != null)
                    {
                        viewJournal.Dispose();
                        viewJournal = null;
                    }

                    // Dispose capture journal if existed
                    if (captureJournal != null)
                    {
                        captureJournal.Release();
                        captureJournal.Dispose();
                        captureJournal = null;
                    }
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Use C# destructor syntax for finalization code
        /// </summary>
        ~Session()
        {
            Dispose(false);
        }

        #endregion Dispose

        #endregion Public methods

        #region protected methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monitor"></param>
        protected Session(IMonitor monitor)
        {
            this.monitor = monitor;
        }

        /// <summary>
        /// Clear collections in this capture
        /// </summary>
        protected void ClearExistingMessage()
        {
            capturedMessageList.Clear();
        }

        /// <summary>
        /// This function is used for event MessageArrived of viewJournal
        /// This event can give a right order of arrived messages, 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        protected void OnViewJournalContentChanged(object sender, JournalContentChange[] journalContentChanges)
        {
            foreach (var change in journalContentChanges)
            {
                // delete message
                if (change.DeletedMessages != null)
                {
                    foreach (INode node in change.DeletedMessages)
                    {
                        // Add arrived message id into a queue. Used to record right message order.
                        MessageId messageID = (MessageId)node.NodeId;
                        this.capturedMessageList.Remove(messageID);
                    }
                }

                if (change.AddedMessages != null)
                {
                    foreach (INode node in change.AddedMessages)
                    {
                        // Add arrived message id into a queue. Used to record right message order.
                        MessageId messageId = (MessageId)node.NodeId;
                        Message message = new Message(messageId, monitor);
                        // Add arrived message id into a queue. Used to record right message order.
                        this.capturedMessageList.Add(messageId, message);
                    }
                }
            }
        }

        /// <summary>
        /// Sort the message by using the MessageNumber
        /// </summary>
        /// <param name="messageList"></param>
        /// <returns></returns>
        private IList<Message> SortMessageList(IList<Message> messageList)
        {
            if (messageList == null || messageList.Count == 0)
            {
                return messageList;
            }

            return messageList.OrderBy<Message, uint>(message => message.MessageNumber).ToList<Message>();
        }

        #endregion Protected methods
    }
}
