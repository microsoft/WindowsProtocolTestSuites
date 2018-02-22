// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 ERROR Response packet is sent by the server to respond to a request that has failed or encountered an error.
    /// </summary>
    public class Smb2ErrorResponsePacket : Smb2StandardPacket<ERROR_Response_packet>
    {
        /// <summary>
        /// Convert an Smb2ErrorResponsePacket to a byte array 
        /// </summary>
        /// <returns>The byte array</returns>
        public override byte[] ToBytes()
        {
            byte[] messageData = TypeMarshal.ToBytes(this.Header);
            messageData = messageData.Concat(TypeMarshal.ToBytes<ushort>(this.PayLoad.StructureSize)).ToArray();
            messageData = messageData.Concat(TypeMarshal.ToBytes<byte>(this.PayLoad.ErrorContextCount)).ToArray();
            messageData = messageData.Concat(TypeMarshal.ToBytes<byte>(this.PayLoad.Reserved)).ToArray();
            messageData = messageData.Concat(TypeMarshal.ToBytes<uint>(this.PayLoad.ByteCount)).ToArray();

            if (this.PayLoad.ErrorContextCount > 0) //smb311
            {
                byte[] temp = new byte[] { };
                for (int i = 0; i < this.PayLoad.ErrorContextCount; i++)
                {
                    temp = temp.Concat(TypeMarshal.ToBytes<uint>(this.PayLoad.ErrorContextErrorData[i].ErrorDataLength)).ToArray();
                    temp = temp.Concat(TypeMarshal.ToBytes<Error_Id>(this.PayLoad.ErrorContextErrorData[i].ErrorId)).ToArray();

                    switch (this.Header.Status)
                    {
                        case Smb2Status.STATUS_STOPPED_ON_SYMLINK:
                            temp = temp.Concat(TypeMarshal.ToBytes<Symbolic_Link_Error_Response>(this.PayLoad.ErrorContextErrorData[i].ErrorData.SymbolicLinkErrorResponse)).ToArray();
                            break;
                        case Smb2Status.STATUS_BAD_NETWORK_NAME:
                            if (this.PayLoad.ErrorContextErrorData[i].ErrorId == Error_Id.ERROR_ID_SHARE_REDIRECT)
                            {
                                temp = temp.Concat(TypeMarshal.ToBytes<Share_Redirect_Error_Context_Response>(this.PayLoad.ErrorContextErrorData[i].ErrorData.ShareRedirectErrorContextResponse)).ToArray();
                            }
                            break;
                        case Smb2Status.STATUS_BUFFER_TOO_SMALL:
                            temp = temp.Concat(this.PayLoad.ErrorContextErrorData[i].ErrorData.BufferTooSmallErrorResponse).ToArray();
                            break;
                    }
                }
                Smb2Utility.Align8(ref temp);
                messageData = messageData.Concat(temp).ToArray();
            }
            else
            {
                messageData = messageData.Concat(this.PayLoad.ErrorData).ToArray();
            }

            return messageData.ToArray();
        }

        /// <summary>
        /// Build a Smb2ErrorResponsePacket from a byte array
        /// </summary>
        /// <param name="data">The byte array</param>
        /// <param name="consumedLen">The consumed data length</param>
        /// <param name="expectedLen">The expected data length</param>
        internal override void FromBytes(byte[] data, out int consumedLen, out int expectedLen)
        {
            consumedLen = 0;
            this.Header = TypeMarshal.ToStruct<Packet_Header>(data, ref consumedLen);

            this.PayLoad.StructureSize = TypeMarshal.ToStruct<ushort>(data.Skip(consumedLen).Take(2).ToArray());
            consumedLen += 2;

            this.PayLoad.ErrorContextCount = TypeMarshal.ToStruct<byte>(data.Skip(consumedLen).Take(1).ToArray());
            consumedLen += 1;

            this.PayLoad.Reserved = TypeMarshal.ToStruct<byte>(data.Skip(consumedLen).Take(1).ToArray());
            consumedLen += 1;

            this.PayLoad.ByteCount = TypeMarshal.ToStruct<uint>(data.Skip(consumedLen).Take(4).ToArray());
            consumedLen += 4;

            if (this.PayLoad.ErrorContextCount > 0) //smb311
            {
                List<Error_Context> errorContextList = new List<Error_Context>();
                for (int i = 0; i < this.PayLoad.ErrorContextCount; i++)
                {
                    Error_Context tempContext = new Error_Context();
                    tempContext.ErrorDataLength = TypeMarshal.ToStruct<uint>(data.Skip(consumedLen).Take(4).ToArray());
                    consumedLen += 4;
                    tempContext.ErrorId = TypeMarshal.ToStruct<Error_Id>(data.Skip(consumedLen).Take(4).ToArray());
                    consumedLen += 4;
                    switch (this.Header.Status)
                    {
                        case Smb2Status.STATUS_STOPPED_ON_SYMLINK:
                            tempContext.ErrorData.SymbolicLinkErrorResponse = TypeMarshal.ToStruct<Symbolic_Link_Error_Response>(data.Skip(consumedLen).Take((int)tempContext.ErrorDataLength).ToArray());
                            break;
                        case Smb2Status.STATUS_BAD_NETWORK_NAME:
                            if (tempContext.ErrorId == Error_Id.ERROR_ID_SHARE_REDIRECT)
                            {
                                tempContext.ErrorData.ShareRedirectErrorContextResponse = TypeMarshal.ToStruct<Share_Redirect_Error_Context_Response>(data.Skip(consumedLen).Take((int)tempContext.ErrorDataLength).ToArray());
                                tempContext.ErrorData.ShareRedirectErrorContextResponse.ResourceName = data.Skip(consumedLen + (int)tempContext.ErrorData.ShareRedirectErrorContextResponse.ResourceNameOffset).Take((int)tempContext.ErrorData.ShareRedirectErrorContextResponse.ResourceNameLength).ToArray();

                                for (int j = 0; j < tempContext.ErrorData.ShareRedirectErrorContextResponse.IPAddrCount; j += 1)
                                {
                                    if (tempContext.ErrorData.ShareRedirectErrorContextResponse.IPAddrMoveList[j].Type == Move_Dst_IpAddr_Type.MOVE_DST_IPADDR_V4)
                                    {
                                        // If the value of the Type field is MOVE_DST_IPADDR_V4, this field is the IPv4Address field followed by Reserved2 fields.
                                        // Reserved2 (12 bytes): The client MUST set this to 0, and the server MUST ignore it on receipt
                                        for (int z = 4; z < 16; z += 1)
                                        {
                                            tempContext.ErrorData.ShareRedirectErrorContextResponse.IPAddrMoveList[j].IPv6Address[z] = 0;
                                        }
                                    }
                                }
                            }
                            break;
                        case Smb2Status.STATUS_BUFFER_TOO_SMALL:
                            tempContext.ErrorData.BufferTooSmallErrorResponse = data.Skip(consumedLen).Take((int)tempContext.ErrorDataLength).ToArray();
                            break;
                    }
                    consumedLen += (int)tempContext.ErrorDataLength;
                    errorContextList.Add(tempContext);
                }

                this.PayLoad.ErrorContextErrorData = errorContextList.ToArray();
            }
            else
            {
                // If the ByteCount field is zero then the server MUST supply an ErrorData field that is one byte in length, and SHOULD set that byte to zero
                int byteCountValue = (int)(this.PayLoad.ByteCount == 0 ? 1 : this.PayLoad.ByteCount);
                this.PayLoad.ErrorData = data.Skip(consumedLen).Take(byteCountValue).ToArray();
                consumedLen += byteCountValue;

            }
            expectedLen = 0;
        }
    }
}
