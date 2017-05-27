// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Opn.Runtime.Monitoring;
using Microsoft.Opn.Runtime.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.MessageAnalyzer
{
    public class Message : ComplexType
    {
        #region variables
        // Private variables
        internal const string SOURCE_ADDRESS_SELECTOR_NAME = "Source";
        internal const string DESTINATION_ADDRESS_SELECTOR_NAME = "Destination";
        internal const string FRAME_NUMBER_SELECTOR_NAME = "MessageNumber";

        private readonly Dictionary<string, ISelector> fieldSelectors = null;

        private readonly IMonitor monitor;
        private readonly MessageId messageId;
        private string source;
        private string destination;
        private uint frameNumber = 0;

        private uint protocolLevel = 0;
        #endregion variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageId">message ID</param>
        /// <param name="monitor">monitor of the environment</param>
        internal Message(MessageId messageId, IMonitor monitor)
            : this(monitor.GetMessageValue(messageId), monitor)
        {
        }

        internal Message(MessageValue messageValue, IMonitor monitor)
            : base(messageValue)
        {
            this.messageId = messageValue.__Id;
            this.monitor = monitor;
            this.fieldSelectors = new Dictionary<string, ISelector>();

            // constructor the selector
            fieldSelectors.Add(SOURCE_ADDRESS_SELECTOR_NAME,
                monitor.CreateSearchSelector(SOURCE_ADDRESS_SELECTOR_NAME, null, 0,
                    SearchSelectorFlags.IncludeAnnotations |
                    SearchSelectorFlags.IncludeFields |
                    SearchSelectorFlags.IncludeProperties |
                    SearchSelectorFlags.StopAtFirst));
            fieldSelectors.Add(DESTINATION_ADDRESS_SELECTOR_NAME,
                monitor.CreateSearchSelector(DESTINATION_ADDRESS_SELECTOR_NAME, null, 0,
                    SearchSelectorFlags.IncludeAnnotations |
                    SearchSelectorFlags.IncludeFields |
                    SearchSelectorFlags.IncludeProperties |
                    SearchSelectorFlags.StopAtFirst));
            fieldSelectors.Add(FRAME_NUMBER_SELECTOR_NAME,
                monitor.CreateSearchSelector(FRAME_NUMBER_SELECTOR_NAME, null, 0,
                    SearchSelectorFlags.IncludeAnnotations |
                    SearchSelectorFlags.StopAtFirst));

        }

        #endregion Constructor

        #region Property

        /// <summary>
        /// Source of this message
        /// </summary>
        public string Source
        {
            get
            {
                if (source == null)
                {
                    source = this.GetMessageSelectorValue(SOURCE_ADDRESS_SELECTOR_NAME);
                }
                return source;
            }
        }

        /// <summary>
        /// get message id
        /// </summary>
        public MessageId MessageId
        {
            get
            {
                return messageId;
            }
        }

        /// <summary>
        /// Destination of this message
        /// </summary>
        public string Destination
        {
            get
            {
                if (destination == null)
                {
                    destination = this.GetMessageSelectorValue(DESTINATION_ADDRESS_SELECTOR_NAME);
                }
                return destination;
            }
        }

        /// <summary>
        /// Get the Model name of a reference type 
        /// </summary>
        public string ModuleName
        {
            get
            {
                return ((MessageValue)referenceValue).__ModuleName;
            }
        }

        public OpnValueTree MessageValueInfoTree
        {
            get
            {
                return monitor.GetMessageValueInfoTree(messageId);
            }
        }

        /// <summary>
        /// Get Message Number/frame number of a message
        /// </summary>
        /// <returns></returns>
        public uint MessageNumber
        {
            get
            {
                if (frameNumber != 0)
                {
                    // return cached frame number
                    return frameNumber;
                }

                string strValue = GetMessageSelectorValue(FRAME_NUMBER_SELECTOR_NAME);
                if (uint.TryParse(strValue, out frameNumber))
                {
                    return frameNumber;
                }
                return 0;
            }
        }

        public string SummaryLine
        {
            get
            {
                String propertyString = monitor.GetMessageDisplayData(messageId, monitor.SummarySelector).Values[0].ToString();
                return propertyString;
            }
        }

        public uint ProtocolLevel { get { return protocolLevel; } internal set { protocolLevel = value; } }
        #endregion Property

        #region Public methods

        /// <summary>
        /// Get child messages
        /// </summary>
        /// <returns>A collection of child messages</returns>
        public ICollection<Message> GetChildMessages()
        {            
            MessageIds childMessageIds = monitor.GetChildMessages(messageId);
            if (childMessageIds != null)
            {
                ICollection<Message> messageList = new List<Message>();
                foreach (MessageId childId in childMessageIds)
                {
                    messageList.Add(new Message(childId, monitor));
                }
                return messageList;
                
            }
            return null;
        }

        internal MessageIds GetChildMessageIds()
        {
            return monitor.GetChildMessages(messageId);
        }

        /// <summary>
        /// Get message of a reference type 
        /// </summary>
        public string MessageType
        {
            get
            {
                return ((MessageValue)referenceValue).__ReferenceTypeName;
            }
        }

        /// <summary>
        /// check message is operation or not
        /// </summary>
        /// <returns></returns>
        public bool IsOperation()
        {
            ITypeInfo msgType = monitor.GetMessageType(messageId);
            if (msgType == null)
                return false;
            return msgType.IsOperation;
        }

        /// <summary>
        /// Get Diagnoses
        /// </summary>
        /// <param name="onlyTopLevelDiagnosisMessages"> whether to parse all diagnoses</param>
        /// <returns></returns>
        public MessageDiagnosis[] GetDiagnoses(bool onlyTopLevelDiagnosisMessages = true)
        {
            if (onlyTopLevelDiagnosisMessages)
            {
                return monitor.GetMessageData(messageId).Diagnoses;
            }
            else
            {
                DiagnosisType diagType = monitor.GetDiagnosisTypes(messageId);

                if (diagType != DiagnosisType.None)
                {
                    // Get diagnosis from current message and lower level message 
                    return monitor.GetMessageDataWithAllDiagnoses(messageId).Diagnoses;
                }
                return new MessageDiagnosis[0];
            }
        }

        /// <summary>
        /// Get property of a message using property name
        /// </summary>
        /// <param name="name">property name</param>
        /// <returns></returns>
        public String GetProperty(string name)
        {
            String propertyString = null;
            List<IPropertyInfo> propertyInfos = monitor.GetMessageType(messageId).AllProperties.ToList();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.Name.ToLower().Equals(name.ToLower()))
                {
                    propertyString = monitor.GetMessageDisplayData(messageId, propertyInfo).Values[0].ToString();
                    break;
                }
            }
            return propertyString;
        }

        #endregion Public methods

        #region private method;

        private string GetMessageSelectorValue(string fieldName)
        {
            if (!fieldSelectors.ContainsKey(fieldName) || messageId == null)
                return string.Empty;

            MessageDataRow dataRow = this.monitor.GetMessageDisplayData((MessageId)messageId, fieldSelectors[fieldName]);
            if (dataRow.Values == null || dataRow.Values[0] == null)
                return string.Empty;

            return dataRow.Values[0].ToString();
        }

        #endregion private method;
        
    }
}
