// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    DomainMCSPDU ::= CHOICE
    {
        plumbDomainIndication PlumbDomainIndication,
        erectDomainRequest ErectDomainRequest,
        mergeChannelsRequest MergeChannelsRequest,
        mergeChannelsConfirm MergeChannelsConfirm,
        purgeChannelsIndication PurgeChannelsIndication,
        mergeTokensRequest MergeTokensRequest,
        mergeTokensConfirm MergeTokensConfirm,
        purgeTokensIndication PurgeTokensIndication,
        disconnectProviderUltimatum DisconnectProviderUltimatum,
        rejectMCSPDUUltimatum RejectMCSPDUUltimatum,
        attachUserRequest AttachUserRequest,
        attachUserConfirm AttachUserConfirm,
        detachUserRequest DetachUserRequest,
        detachUserIndication DetachUserIndication,
        channelJoinRequest ChannelJoinRequest,
        channelJoinConfirm ChannelJoinConfirm,
        channelLeaveRequest ChannelLeaveRequest,
        channelConveneRequest ChannelConveneRequest,
        channelConveneConfirm ChannelConveneConfirm,
        channelDisbandRequest ChannelDisbandRequest,
        channelDisbandIndication ChannelDisbandIndication,
        channelAdmitRequest ChannelAdmitRequest,
        channelAdmitIndication ChannelAdmitIndication,
        channelExpelRequest ChannelExpelRequest,
        channelExpelIndication ChannelExpelIndication,
        sendDataRequest SendDataRequest,
        sendDataIndication SendDataIndication,
        uniformSendDataRequest UniformSendDataRequest,
        uniformSendDataIndication UniformSendDataIndication,
        tokenGrabRequest TokenGrabRequest,
        tokenGrabConfirm TokenGrabConfirm,
        tokenInhibitRequest TokenInhibitRequest,
        tokenInhibitConfirm TokenInhibitConfirm,
        tokenGiveRequest TokenGiveRequest,
        tokenGiveIndication TokenGiveIndication,
        tokenGiveResponse TokenGiveResponse,
        tokenGiveConfirm TokenGiveConfirm,
        tokenPleaseRequest TokenPleaseRequest,
        tokenPleaseIndication TokenPleaseIndication,
        tokenReleaseRequest TokenReleaseRequest,
        tokenReleaseConfirm TokenReleaseConfirm,
        tokenTestRequest TokenTestRequest,
        tokenTestConfirm TokenTestConfirm
    }
    */
    public class DomainMCSPDU : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long plumbDomainIndication = 0;
        [Asn1ChoiceElement(plumbDomainIndication)]
        protected PlumbDomainIndication field0 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long erectDomainRequest = 1;
        [Asn1ChoiceElement(erectDomainRequest)]
        protected ErectDomainRequest field1 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long mergeChannelsRequest = 2;
        [Asn1ChoiceElement(mergeChannelsRequest)]
        protected MergeChannelsRequest field2 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long mergeChannelsConfirm = 3;
        [Asn1ChoiceElement(mergeChannelsConfirm)]
        protected MergeChannelsConfirm field3 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long purgeChannelsIndication = 4;
        [Asn1ChoiceElement(purgeChannelsIndication)]
        protected PurgeChannelsIndication field4 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long mergeTokensRequest = 5;
        [Asn1ChoiceElement(mergeTokensRequest)]
        protected MergeTokensRequest field5 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long mergeTokensConfirm = 6;
        [Asn1ChoiceElement(mergeTokensConfirm)]
        protected MergeTokensConfirm field6 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long purgeTokensIndication = 7;
        [Asn1ChoiceElement(purgeTokensIndication)]
        protected PurgeTokensIndication field7 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long disconnectProviderUltimatum = 8;
        [Asn1ChoiceElement(disconnectProviderUltimatum)]
        protected DisconnectProviderUltimatum field8 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long rejectMCSPDUUltimatum = 9;
        [Asn1ChoiceElement(rejectMCSPDUUltimatum)]
        protected RejectMCSPDUUltimatum field9 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long attachUserRequest = 10;
        [Asn1ChoiceElement(attachUserRequest)]
        protected AttachUserRequest field10 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long attachUserConfirm = 11;
        [Asn1ChoiceElement(attachUserConfirm)]
        protected AttachUserConfirm field11 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long detachUserRequest = 12;
        [Asn1ChoiceElement(detachUserRequest)]
        protected DetachUserRequest field12 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long detachUserIndication = 13;
        [Asn1ChoiceElement(detachUserIndication)]
        protected DetachUserIndication field13 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long channelJoinRequest = 14;
        [Asn1ChoiceElement(channelJoinRequest)]
        protected ChannelJoinRequest field14 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long channelJoinConfirm = 15;
        [Asn1ChoiceElement(channelJoinConfirm)]
        protected ChannelJoinConfirm field15 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long channelLeaveRequest = 16;
        [Asn1ChoiceElement(channelLeaveRequest)]
        protected ChannelLeaveRequest field16 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long channelConveneRequest = 17;
        [Asn1ChoiceElement(channelConveneRequest)]
        protected ChannelConveneRequest field17 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long channelConveneConfirm = 18;
        [Asn1ChoiceElement(channelConveneConfirm)]
        protected ChannelConveneConfirm field18 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long channelDisbandRequest = 19;
        [Asn1ChoiceElement(channelDisbandRequest)]
        protected ChannelDisbandRequest field19 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long channelDisbandIndication = 20;
        [Asn1ChoiceElement(channelDisbandIndication)]
        protected ChannelDisbandIndication field20 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long channelAdmitRequest = 21;
        [Asn1ChoiceElement(channelAdmitRequest)]
        protected ChannelAdmitRequest field21 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long channelAdmitIndication = 22;
        [Asn1ChoiceElement(channelAdmitIndication)]
        protected ChannelAdmitIndication field22 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long channelExpelRequest = 23;
        [Asn1ChoiceElement(channelExpelRequest)]
        protected ChannelExpelRequest field23 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long channelExpelIndication = 24;
        [Asn1ChoiceElement(channelExpelIndication)]
        protected ChannelExpelIndication field24 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long sendDataRequest = 25;
        [Asn1ChoiceElement(sendDataRequest)]
        protected SendDataRequest field25 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long sendDataIndication = 26;
        [Asn1ChoiceElement(sendDataIndication)]
        protected SendDataIndication field26 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long uniformSendDataRequest = 27;
        [Asn1ChoiceElement(uniformSendDataRequest)]
        protected UniformSendDataRequest field27 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long uniformSendDataIndication = 28;
        [Asn1ChoiceElement(uniformSendDataIndication)]
        protected UniformSendDataIndication field28 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenGrabRequest = 29;
        [Asn1ChoiceElement(tokenGrabRequest)]
        protected TokenGrabRequest field29 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenGrabConfirm = 30;
        [Asn1ChoiceElement(tokenGrabConfirm)]
        protected TokenGrabConfirm field30 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenInhibitRequest = 31;
        [Asn1ChoiceElement(tokenInhibitRequest)]
        protected TokenInhibitRequest field31 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenInhibitConfirm = 32;
        [Asn1ChoiceElement(tokenInhibitConfirm)]
        protected TokenInhibitConfirm field32 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenGiveRequest = 33;
        [Asn1ChoiceElement(tokenGiveRequest)]
        protected TokenGiveRequest field33 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenGiveIndication = 34;
        [Asn1ChoiceElement(tokenGiveIndication)]
        protected TokenGiveIndication field34 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenGiveResponse = 35;
        [Asn1ChoiceElement(tokenGiveResponse)]
        protected TokenGiveResponse field35 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenGiveConfirm = 36;
        [Asn1ChoiceElement(tokenGiveConfirm)]
        protected TokenGiveConfirm field36 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenPleaseRequest = 37;
        [Asn1ChoiceElement(tokenPleaseRequest)]
        protected TokenPleaseRequest field37 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenPleaseIndication = 38;
        [Asn1ChoiceElement(tokenPleaseIndication)]
        protected TokenPleaseIndication field38 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenReleaseRequest = 39;
        [Asn1ChoiceElement(tokenReleaseRequest)]
        protected TokenReleaseRequest field39 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenReleaseConfirm = 40;
        [Asn1ChoiceElement(tokenReleaseConfirm)]
        protected TokenReleaseConfirm field40 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenTestRequest = 41;
        [Asn1ChoiceElement(tokenTestRequest)]
        protected TokenTestRequest field41 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long tokenTestConfirm = 42;
        [Asn1ChoiceElement(tokenTestConfirm)]
        protected TokenTestConfirm field42 { get; set; }
        
        public DomainMCSPDU()
            : base()
        {
        }
        
        public DomainMCSPDU(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }

        public string ElemName
        {
            get
            {
                switch (SelectedChoice)
                {
                    case channelJoinConfirm:
                        {
                            return "channelJoinConfirm";
                        }
                    case sendDataIndication:
                        {
                            return "sendDataIndication";
                        }
                    case attachUserConfirm:
                        {
                            return "attachUserConfirm";
                        }
                    case disconnectProviderUltimatum:
                        {
                            return "disconnectProviderUltimatum";
                        }
                    case erectDomainRequest:
                        {
                            return "erectDomainRequest";
                        }
                    case attachUserRequest:
                        {
                            return "attachUserRequest";
                        }
                    case channelJoinRequest:
                        {
                            return "channelJoinRequest";
                        }
                    case sendDataRequest:
                        {
                            return "sendDataRequest";
                        }
                    default:
                        {
                            return "Not used in RDP Test Suite.";
                        }
                }
            }


            /*// Erect Domain Request PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_ERECT_DOMAIN_REQUEST:
                    pdu = DecodeMcsErectDomainRequestPDU(data);
                    break;

                // Attach User Request PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_ATTACH_USER_REQUEST:
                    pdu = DecodeMcsAttachUserRequestPDU(data);
                    break;

                // Channel Join Request PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_CHANNEL_JOIN_REQUEST:
                    pdu = DecodeMcsChannelJoinRequestPDU(data);
                    break;

                // Send Data Request PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_SEND_DATA_REQUEST:
                    pdu = DecodeMcsSendDataRequestPDU(serverSessionContext, data, domainPdu);
                    break;

                // Disconnect Provider Ultimatum PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_DISCONNECT_PROVIDER_ULTIMATUM:
                    pdu = DecodeDisconnectProviderUltimatumPDU(data);
                    break;

                default:
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
             * */
        }
    }
}

