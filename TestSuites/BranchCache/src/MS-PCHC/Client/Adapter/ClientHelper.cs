// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;

    /// <summary>
    /// The helper is used to exchange the data struct in adapter and stack.
    /// </summary>
    public static class ClientHelper
    {
        /// <summary>
        /// Convert the segmentInforMessageStack struct defined in stack to adapter 
        /// </summary>
        /// <param name="segmentInforMessageStack">The segmentInforMessageStack message</param>
        /// <returns>Return the segmentInforMessageStack type defined in adapter</returns>
        public static SegmentInfoMessage ConvertFromStackForSegmentInfoMsg(SEGMENT_INFO_MESSAGE segmentInforMessageStack)
        {
            SegmentInfoMessage segmentInforMessage;
            segmentInforMessage.ConnectionInfo = ConvertFromStackForConnInfo(segmentInforMessageStack.ConnectionInfo);
            segmentInforMessage.ContentTag = segmentInforMessageStack.ContentTag;
            segmentInforMessage.MsgHeader = ConvertFromStackForMsgHeader(segmentInforMessageStack.MsgHeader);
            segmentInforMessage.SegmentInfo = segmentInforMessageStack.SegmentInfo;

            return segmentInforMessage;
        }

        /// <summary>
        /// Convert the segmentInforMessage struct defined in adapter to stack
        /// </summary>
        /// <param name="segmentInforMessage">The segmentInforMessage message</param>
        /// <returns>Return the segmentInforMessage type defined in stack</returns>
        public static SEGMENT_INFO_MESSAGE ConvertToStackForSegmentInfoMsg(SegmentInfoMessage segmentInforMessage)
        {
            SEGMENT_INFO_MESSAGE segmentInforMessageStack;
            segmentInforMessageStack.ConnectionInfo = ConvertToStackForConnInfo(segmentInforMessage.ConnectionInfo);
            segmentInforMessageStack.ContentTag = segmentInforMessage.ContentTag;
            segmentInforMessageStack.MsgHeader = ConvertToStackForMsgHeader(segmentInforMessage.MsgHeader);
            segmentInforMessageStack.SegmentInfo = segmentInforMessage.SegmentInfo;

            return segmentInforMessageStack;
        }

        /// <summary>
        /// Convert the initialOfferMessageStack struct defined in stack to adapter 
        /// </summary>
        /// <param name="initialOfferMessageStack">The initialOfferMessageStack message</param>
        /// <returns>Return the initialOfferMessageStack type defined in adapter</returns>
        public static InitialOfferMessage ConvertFromStackForInitialOfferMsg(INITIAL_OFFER_MESSAGE initialOfferMessageStack)
        {
            InitialOfferMessage initialOfferMessage;
            initialOfferMessage.ConnectionInfo = ConvertFromStackForConnInfo(initialOfferMessageStack.ConnectionInfo);
            initialOfferMessage.Hash = initialOfferMessageStack.Hash;
            initialOfferMessage.MsgHeader = ConvertFromStackForMsgHeader(initialOfferMessageStack.MsgHeader);

            return initialOfferMessage;
        }

        /// <summary>
        /// Convert the initialOfferMessage struct defined in adapter to stack
        /// </summary>
        /// <param name="initialOfferMessage">The initialOfferMessage message</param>
        /// <returns>Return the initialOfferMessage type defined in stack</returns>
        public static INITIAL_OFFER_MESSAGE ConvertToStackForInitialOfferMsg(InitialOfferMessage initialOfferMessage)
        {
            INITIAL_OFFER_MESSAGE initialOfferMessageStack;
            initialOfferMessageStack.ConnectionInfo = ConvertToStackForConnInfo(initialOfferMessage.ConnectionInfo);
            initialOfferMessageStack.Hash = initialOfferMessage.Hash;
            initialOfferMessageStack.MsgHeader = ConvertToStackForMsgHeader(initialOfferMessage.MsgHeader);

            return initialOfferMessageStack;
        }

        /// <summary>
        /// Convert the connectionInformationStack struct defined in stack to adapter 
        /// </summary>
        /// <param name="connectionInformationStack">The connectionInformationStack message</param>
        /// <returns>Return the connectionInformationStack type defined in adapter</returns>
        private static ConnectionInformation ConvertFromStackForConnInfo(CONNECTION_INFORMATION connectionInformationStack)
        {
            ConnectionInformation connectionInformation;
            connectionInformation.Padding = connectionInformationStack.Padding;
            connectionInformation.Port = connectionInformationStack.Port;

            return connectionInformation;
        }

        /// <summary>
        /// Convert the connectionInformation struct defined in adapter to stack
        /// </summary>
        /// <param name="connectionInformation">The connectionInformation message</param>
        /// <returns>Return the connectionInformation type defined in stack</returns>
        private static CONNECTION_INFORMATION ConvertToStackForConnInfo(ConnectionInformation connectionInformation)
        {
            CONNECTION_INFORMATION connectionInformationStack;
            connectionInformationStack.Padding = connectionInformation.Padding;
            connectionInformationStack.Port = connectionInformation.Port;

            return connectionInformationStack;
        }

        /// <summary>
        /// Convert the messageHeaderStack struct defined in stack to adapter 
        /// </summary>
        /// <param name="messageHeaderStack">The messageHeaderStack message</param>
        /// <returns>Return the messageHeaderStack type defined in adapter</returns>
        private static MessageHeader ConvertFromStackForMsgHeader(MESSAGE_HEADER messageHeaderStack)
        {
            MessageHeader messageHeader;
            messageHeader.MajorVersion = messageHeaderStack.MajorVersion;
            messageHeader.MinorVersion = messageHeaderStack.MinorVersion;
            messageHeader.MsgType = ConvertFromStackForPCHCMsgType(messageHeaderStack.MsgType);
            messageHeader.Padding = messageHeaderStack.Padding;

            return messageHeader;
        }

        /// <summary>
        /// Convert the messageHeader struct defined in adapter to stack
        /// </summary>
        /// <param name="messageHeader">The messageHeader message</param>
        /// <returns>Return the messageHeader type defined in stack</returns>
        private static MESSAGE_HEADER ConvertToStackForMsgHeader(MessageHeader messageHeader)
        {
            MESSAGE_HEADER messageHeaderStack;
            messageHeaderStack.MajorVersion = messageHeader.MajorVersion;
            messageHeaderStack.MinorVersion = messageHeader.MinorVersion;
            messageHeaderStack.MsgType = ConvertToStackForPCHCMsgType(messageHeader.MsgType);
            messageHeaderStack.Padding = messageHeader.Padding;

            return messageHeaderStack;
        }

        /// <summary>
        /// Convert the pchcMessageTypeStack struct defined in stack to adapter 
        /// </summary>
        /// <param name="pchcMessageTypeStack">The pchcMessageTypeStack message</param>
        /// <returns>Return the pchcMessageTypeStack type defined in adapter</returns>
        private static PCHCMessageType ConvertFromStackForPCHCMsgType(PCHC_MESSAGE_TYPE pchcMessageTypeStack)
        {
            PCHCMessageType pchcMessageType;
            pchcMessageType = (PCHCMessageType)pchcMessageTypeStack;

            return pchcMessageType;
        }

        /// <summary>
        /// Convert the pchcMessageType struct defined in adapter to stack
        /// </summary>
        /// <param name="pchcMessageType">The pchcMessageType message</param>
        /// <returns>Return the pchcMessageType type defined in stack</returns>
        private static PCHC_MESSAGE_TYPE ConvertToStackForPCHCMsgType(PCHCMessageType pchcMessageType)
        {
            PCHC_MESSAGE_TYPE pchcMessageTypeStack;
            pchcMessageTypeStack = (PCHC_MESSAGE_TYPE)pchcMessageType;

            return pchcMessageTypeStack;
        }
    }
}
