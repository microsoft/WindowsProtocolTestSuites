// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Asn1OctetString = Microsoft.Protocols.TestTools.StackSdk.Asn1.Asn1OctetString;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Encode different types to a byte stream.
    /// </summary>
    public class RdpbcgrEncoder
    {
        /// <summary>
        /// Encode a structure to a byte list.
        /// </summary>
        /// <param name="buffer">The buffer list to contain the structure. 
        /// This argument cannot be null. It may throw ArgumentNullException if it is null.</param>
        /// <param name="structure">The structure to be added to buffer list.
        /// This argument cannot be null. It may throw ArgumentNullException if it is null.</param>
        internal static void EncodeStructure(List<byte> buffer, object structure)
        {
            byte[] structBuffer = RdpbcgrUtility.StructToBytes(structure);
            buffer.AddRange(structBuffer);
        }


        /// <summary>
        /// Encode a byte array to a byte list.
        /// </summary>
        /// <param name="buffer">The buffer list to contain the bytes.
        /// This argument cannot be null. It may throw ArgumentNullException if it is null.</param>
        /// <param name="byteArray">The bytes to be added to buffer list. 
        /// This argument can be null. If it is null, the method will encode nothing.</param>
        internal static void EncodeBytes(List<byte> buffer, byte[] byteArray)
        {
            if (byteArray != null)
            {
                buffer.AddRange(byteArray);
            }
        }

        internal static uint CalculateUnicodeStringEncodingSize(string unicodeString, bool isZeroTerminited)
        {
            uint result = (uint)unicodeString.Length * 2;
            if (isZeroTerminited)
            {
                result += 2;
            }
            return result;
        }

        /// <summary>
        /// Encode a Unicode string to a byte list.
        /// </summary>
        /// <param name="buffer">The buffer list to contain the string.
        /// This argument cannot be null. It may throw ArgumentNullException if it is null.</param>
        /// <param name="uniString">The string to be added to buffer list.
        /// This argument can be null. If it is null, bytesNumber of zero will be appended to the buffer.</param>
        /// <param name="bytesNumber">The number of bytes to be appended to the buffer. 
        /// If the uniString length is longer than bytesNumber, then it will be truncated.
        /// If the uniString length is shorter than bytesNumber, then zero will be padded.</param>
        internal static void EncodeUnicodeString(List<byte> buffer, string uniString, uint bytesNumber)
        {
            if (bytesNumber != 0)
            {
                byte[] stringBuffer = new byte[bytesNumber];
                if (uniString != null)
                {
                    byte[] uniBuffer = Encoding.Unicode.GetBytes(uniString);

                    if (uniBuffer.Length < (bytesNumber - 1))
                    {
                        uniBuffer.CopyTo(stringBuffer, 0);
                    }
                    else
                    {
                        // leave the last two bytes for null-terminator.
                        Array.Copy(uniBuffer, stringBuffer, bytesNumber - 2);
                    }
                }

                buffer.AddRange(stringBuffer);
            }
        }


        /// <summary>
        /// Encode an ANSI string to a byte list.
        /// </summary>
        /// <param name="buffer">The buffer list to contain the string.
        /// This argument cannot be null. It may throw ArgumentNullException if it is null.</param>
        /// <param name="ansiString">The string to be added to buffer list.
        /// This argument can be null. If it is null, bytesNumber of zero will be appended to the buffer.</param>
        /// <param name="bytesNumber">The number of bytes to be appended to the buffer. 
        /// If the ansiString length is longer than bytesNumber, then it will be truncated.
        /// If the ansiString length is shorter than bytesNumber, then zero will be padded.</param>
        internal static void EncodeAnsiString(List<byte> buffer, string ansiString, uint bytesNumber)
        {
            if (bytesNumber != 0)
            {
                byte[] stringBuffer = new byte[bytesNumber];
                if (ansiString != null)
                {
                    byte[] ansiBuffer = Encoding.ASCII.GetBytes(ansiString);

                    if (ansiString.Length < bytesNumber)
                    {
                        ansiBuffer.CopyTo(stringBuffer, 0);
                    }
                    else
                    {
                        // leave the last byte for null-terminator.
                        Array.Copy(ansiBuffer, stringBuffer, bytesNumber - 1);
                    }
                }

                buffer.AddRange(stringBuffer);
            }
        }


        /// <summary>
        /// Encode DomainMCSPDU to a byte list.
        /// </summary>
        /// <param name="buffer">The buffer list to contain the PDU.
        /// This argument cannot be null. It may throw ArgumentNullException if it is null.</param>
        /// <param name="domainMcsPdu">The PDU to be encoded.
        /// This argument can be null. If it is null, the method will encode nothing.</param>
        internal static void EncodeDomainMcsPdu(List<byte> buffer, DomainMCSPDU domainMcsPdu)
        {
            if (domainMcsPdu != null)
            {
                Asn1PerEncodingBuffer perEncodeBuffer = new Asn1PerEncodingBuffer(true);
                domainMcsPdu.PerEncode(perEncodeBuffer);
                buffer.AddRange(perEncodeBuffer.ByteArrayData);
            }
        }


        /// <summary>
        /// Encode security header and its body to a byte list.
        /// </summary>
        /// <param name="buffer">The buffer list to contain the PDU.
        /// This argument cannot be null. It may throw ArgumentNullException if it is null.</param>
        /// <param name="securityHeader">The security header to be encoded.
        /// This argument can be null. If it is null, the method will encode dataBody only. 
        /// Then it is the same as EncodeBytes.</param>
        /// <param name="dataBody">The data following the security header.
        /// This argument can be null. If it is null, the method will encode securityHeader only.</param>
        /// <param name="context">The context used to encrypt the data body.
        /// If the securityHeader is the type of TS_SECURITY_HEADER, then this argument can be null.
        /// Otherwise, this argument can not be null, it will not encrypt the dataBody.</param>
        internal static void EncodeSecurityData(List<byte> buffer,
                                                TS_SECURITY_HEADER securityHeader,
                                                byte[] dataBody,
                                                RdpbcgrClientContext context)
        {
            if (securityHeader != null)       // have a security header
            {
                EncodeStructure(buffer, (ushort)securityHeader.flags);
                EncodeStructure(buffer, securityHeader.flagsHi);

                if (securityHeader.GetType() == typeof(TS_SECURITY_HEADER1))   // non-fips security header
                {
                    if (dataBody != null && context != null)
                    {
                        TS_SECURITY_HEADER1 nonFipsHeader = securityHeader as TS_SECURITY_HEADER1;
                        byte[] dataSignature = null;

                        bool isSalted = (nonFipsHeader.flags & TS_SECURITY_HEADER_flags_Values.SEC_SECURE_CHECKSUM)
                                         == TS_SECURITY_HEADER_flags_Values.SEC_SECURE_CHECKSUM;
                        context.Encrypt(dataBody, isSalted, out dataBody, out dataSignature);

                        // If the data signature has not been set, generate it.
                        // Otherwise, keep the old value.
                        if (nonFipsHeader.dataSignature == null)
                        {
                            nonFipsHeader.dataSignature = dataSignature;
                        }

                        EncodeBytes(buffer, nonFipsHeader.dataSignature);
                    }
                }
                else if (securityHeader.GetType() == typeof(TS_SECURITY_HEADER2))   // fips security header
                {
                    if (dataBody != null && context != null)
                    {
                        TS_SECURITY_HEADER2 fipsHeader = securityHeader as TS_SECURITY_HEADER2;
                        byte[] dataSignature = null;
                        EncodeStructure(buffer, (ushort)fipsHeader.length);
                        EncodeStructure(buffer, fipsHeader.version);

                        // If the padlen equals 0, calculate it.
                        // Otherwise, keep the old value.
                        if (fipsHeader.padlen == 0)
                        {
                            fipsHeader.padlen = (byte)(ConstValue.TRIPLE_DES_PAD
                                              - (dataBody.Length % ConstValue.TRIPLE_DES_PAD));
                        }

                        EncodeStructure(buffer, fipsHeader.padlen);
                        context.Encrypt(dataBody, false, out dataBody, out dataSignature);

                        // If the data signature has not been set, generate it.
                        // Otherwise, keep the old value.
                        if (fipsHeader.dataSignature == null)
                        {
                            fipsHeader.dataSignature = dataSignature;
                        }

                        EncodeBytes(buffer, fipsHeader.dataSignature);
                    }
                }
                // else do not do encryption                              
            }

            EncodeBytes(buffer, dataBody);
        }


        internal static void EncodeSecurityData(List<byte> buffer,
                                                TS_SECURITY_HEADER securityHeader,
                                                byte[] dataBody,
                                                RdpbcgrServerSessionContext context)
        {
            if (securityHeader != null)       // have a security header
            {
                EncodeStructure(buffer, (ushort)securityHeader.flags);
                EncodeStructure(buffer, securityHeader.flagsHi);

                if (securityHeader.GetType() == typeof(TS_SECURITY_HEADER1))   // non-fips security header
                {
                    if (dataBody != null && context != null)
                    {
                        TS_SECURITY_HEADER1 nonFipsHeader = securityHeader as TS_SECURITY_HEADER1;
                        byte[] dataSignature = null;

                        bool isSalted = (nonFipsHeader.flags & TS_SECURITY_HEADER_flags_Values.SEC_SECURE_CHECKSUM)
                                         == TS_SECURITY_HEADER_flags_Values.SEC_SECURE_CHECKSUM;
                        context.ServerEncrypt(dataBody, isSalted, out dataBody, out dataSignature);

                        // If the data signature has not been set, generate it.
                        // Otherwise, keep the old value.
                        if (nonFipsHeader.dataSignature == null)
                        {
                            nonFipsHeader.dataSignature = dataSignature;
                        }

                        EncodeBytes(buffer, nonFipsHeader.dataSignature);
                    }
                }
                else if (securityHeader.GetType() == typeof(TS_SECURITY_HEADER2))   // fips security header
                {
                    if (dataBody != null && context != null)
                    {
                        TS_SECURITY_HEADER2 fipsHeader = securityHeader as TS_SECURITY_HEADER2;
                        byte[] dataSignature = null;
                        EncodeStructure(buffer, (ushort)fipsHeader.length);
                        EncodeStructure(buffer, fipsHeader.version);

                        // If the padlen equals 0, calculate it.
                        // Otherwise, keep the old value.
                        if (fipsHeader.padlen == 0)
                        {
                            fipsHeader.padlen = (byte)(ConstValue.TRIPLE_DES_PAD
                                              - (dataBody.Length % ConstValue.TRIPLE_DES_PAD));
                        }

                        EncodeStructure(buffer, fipsHeader.padlen);
                        context.ServerEncrypt(dataBody, false, out dataBody, out dataSignature);

                        // If the data signature has not been set, generate it.
                        // Otherwise, keep the old value.
                        if (fipsHeader.dataSignature == null)
                        {
                            fipsHeader.dataSignature = dataSignature;
                        }

                        EncodeBytes(buffer, fipsHeader.dataSignature);
                    }
                }
            }

            EncodeBytes(buffer, dataBody);
        }


        /// <summary>
        /// Encode slow path pdu to a byte list.
        /// </summary>
        /// <param name="sendBuffer">The buffer list to contain the PDU.
        /// This argument cannot be null. It may throw ArgumentNullException if it is null.</param>
        /// <param name="commonHeader">The common header of the PDU including tpktHeader, X224, security header,
        /// user channel Id and I/O channel Id.</param>
        /// <param name="dataBody">The data following the common header.
        /// This argument can be null. If it is null, the method will encode commonHeader only.</param>
        /// <param name="context">The context used to encrypt the data body.
        /// If the securityHeader is the type of TS_SECURITY_HEADER, then this argument can be null.
        /// Otherwise, this argument can not be null, the dataBody will not be encrypted.</param>
        public static void EncodeSlowPathPdu(List<byte> sendBuffer,
                                               SlowPathPduCommonHeader commonHeader,
                                               byte[] dataBody,
                                               RdpbcgrClientContext context)
        {
            EncodeStructure(sendBuffer, commonHeader.tpktHeader);
            EncodeStructure(sendBuffer, commonHeader.x224Data);

            List<byte> securityBuffer = new List<byte>();
            EncodeSecurityData(securityBuffer, commonHeader.securityHeader, dataBody, context);

            SendDataRequest securityExchange = new SendDataRequest(new UserId(commonHeader.initiator),
                                                                   new ChannelId(commonHeader.channelId),
                                                                   new DataPriority(ConstValue.SEND_DATA_REQUEST_PRIORITY),
                                                                   ConstValue.SEND_DATA_REQUEST_SEGMENTATION,
                                                                   new Asn1OctetString(securityBuffer.ToArray()));
            DomainMCSPDU mcsDomain = new DomainMCSPDU(DomainMCSPDU.sendDataRequest, securityExchange);
            EncodeDomainMcsPdu(sendBuffer, mcsDomain);
        }

        internal static void EncodeSlowPathPdu(List<byte> sendBuffer,
                                               SlowPathPduCommonHeader commonHeader,
                                               byte[] dataBody,
                                               RdpbcgrServerSessionContext context)
        {
            EncodeStructure(sendBuffer, commonHeader.tpktHeader);
            EncodeStructure(sendBuffer, commonHeader.x224Data);

            List<byte> securityBuffer = new List<byte>();
            EncodeSecurityData(securityBuffer, commonHeader.securityHeader, dataBody, context);

            SendDataIndication securityExchange = new SendDataIndication(new UserId(commonHeader.initiator),
                                                                   new ChannelId(commonHeader.channelId),
                                                                   new DataPriority(ConstValue.SEND_DATA_REQUEST_PRIORITY),
                                                                   ConstValue.SEND_DATA_REQUEST_SEGMENTATION,
                                                                   new Asn1OctetString(securityBuffer.ToArray()));
            DomainMCSPDU mcsDomain = new DomainMCSPDU(DomainMCSPDU.sendDataIndication, securityExchange);
            EncodeDomainMcsPdu(sendBuffer, mcsDomain);
        }

        /// <summary>
        /// Encode NetWork Detection Request
        /// </summary>
        /// <param name="sendBuffer">The buffer list to contain the encoded PDU.</param>
        /// <param name="networkDetectionRequest">The Network Detect Request want to be encoded</param>
        internal static void EncodeNetworkDetectionRequest(List<byte> sendBuffer, NETWORK_DETECTION_REQUEST networkDetectionRequest, bool isSubHeader = false)
        {
            if (!isSubHeader)
            {
                EncodeStructure(sendBuffer, networkDetectionRequest.headerLength);
                EncodeStructure(sendBuffer, (byte)networkDetectionRequest.headerTypeId);
            }
            EncodeStructure(sendBuffer, networkDetectionRequest.sequenceNumber);
            EncodeStructure(sendBuffer, (ushort)networkDetectionRequest.requestType);

            AUTO_DETECT_REQUEST_TYPE requestType = networkDetectionRequest.requestType;

            if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_IN_CONNECTTIME || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_AFTER_CONNECTTIME)
            {
                //RDP_RTT_REQUEST

            }
            else if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_IN_CONNECTTIME || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_RELIABLEUDP || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_LOSSYUDP)
            {
                //RDP_BW_START                
            }
            else if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_PAYLOAD)
            {
                //RDP_BW_PAYLOAD
                RDP_BW_PAYLOAD bwPayload = (RDP_BW_PAYLOAD)networkDetectionRequest;
                EncodeStructure(sendBuffer, bwPayload.payloadLength);
                EncodeBytes(sendBuffer, bwPayload.payload);

            }
            else if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_IN_CONNECTTIME || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_RELIABLEUDP || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_LOSSYUDP)
            {
                //RDP_BW_STOP
                if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_IN_CONNECTTIME)
                {
                    RDP_BW_STOP bwStop = (RDP_BW_STOP)networkDetectionRequest;
                    EncodeStructure(sendBuffer, bwStop.payloadLength);
                    EncodeBytes(sendBuffer, bwStop.payload);
                }
            }
            else if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BANDWIDTH_AVERAGERTT || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_AVERAGERTT || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT)
            {
                //RDP_NETCHAR_RESULT
                RDP_NETCHAR_RESULT netCharResult = (RDP_NETCHAR_RESULT)networkDetectionRequest;
                if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BANDWIDTH_AVERAGERTT)
                {
                    EncodeStructure(sendBuffer, netCharResult.bandwidth);
                    EncodeStructure(sendBuffer, netCharResult.averageRTT);
                }
                else if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_AVERAGERTT)
                {
                    EncodeStructure(sendBuffer, netCharResult.baseRTT);
                    EncodeStructure(sendBuffer, netCharResult.averageRTT);
                }
                else if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT)
                {
                    EncodeStructure(sendBuffer, netCharResult.baseRTT);
                    EncodeStructure(sendBuffer, netCharResult.bandwidth);
                    EncodeStructure(sendBuffer, netCharResult.averageRTT);
                }
            }
        }

        /// <summary>
        /// Encode NETWORK_DETECTION_RESPONSE
        /// </summary>
        /// <param name="sendBuffer"></param>
        /// <param name="networkDetectionResponse"></param>
        /// <param name="isSubHeader"></param>
        internal static void EncodeNetworkDetectionResponse(List<byte> sendBuffer, NETWORK_DETECTION_RESPONSE networkDetectionResponse, bool isSubHeader = false)
        {
            if (!isSubHeader)
            {
                EncodeStructure(sendBuffer, networkDetectionResponse.headerLength);
                EncodeStructure(sendBuffer, (byte)networkDetectionResponse.headerTypeId);
            }
            EncodeStructure(sendBuffer, networkDetectionResponse.sequenceNumber);
            EncodeStructure(sendBuffer, (ushort)networkDetectionResponse.responseType);

            if (networkDetectionResponse.responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_RTT_RESPONSE)
            {
                //RDP_RTT_RESPONSE
            }
            else if (networkDetectionResponse.responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_AFTER_CONNECT ||
                networkDetectionResponse.responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_DURING_CONNECT)
            {
                RDP_BW_RESULTS bwResult = (RDP_BW_RESULTS)networkDetectionResponse;
                EncodeStructure(sendBuffer, bwResult.timeDelta);
                EncodeStructure(sendBuffer, bwResult.byteCount);
            }
            else if (networkDetectionResponse.responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_NETCHAR_SYNC)
            {
                RDP_NETCHAR_SYNC sync = (RDP_NETCHAR_SYNC)networkDetectionResponse;
                EncodeStructure(sendBuffer, sync.bandwidth);
                EncodeStructure(sendBuffer, sync.rtt);
            }
        }
    }
}
