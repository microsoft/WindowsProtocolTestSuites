// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// the authenticate packet of nlmp
    /// </summary>
    public class NlmpAuthenticatePacket : NlmpPacket
    {
        /// <summary>
        /// 2.2.1.3 AUTHENTICATE_MESSAGE The AUTHENTICATE_MESSAGE defines an NTLM authenticate message that is sent 
        /// from the client to the server after the CHALLENGE_MESSAGE (section 2.2.1.2) is processed by the client.
        /// </summary>
        private AUTHENTICATE_MESSAGE payload;

        /// <summary>
        /// 2.2.1.3 AUTHENTICATE_MESSAGE The AUTHENTICATE_MESSAGE defines an NTLM authenticate message that is sent 
        /// from the client to the server after the CHALLENGE_MESSAGE (section 2.2.1.2) is processed by the client.
        /// </summary>
        public AUTHENTICATE_MESSAGE Payload
        {
            get
            {
                return this.payload;
            }
            set
            {
                this.payload = value;
            }
        }


        /// <summary>
        /// the length of authenticate message
        /// </summary>
        protected override int PayLoadLength
        {
            get
            {
                return NlmpUtility.AUTHENTICATE_MESSAGE_CONST_SIZE
                    + payload.DomainNameFields.Len
                    + payload.EncryptedRandomSessionKeyFields.Len
                    + payload.LmChallengeResponseFields.Len
                    + payload.NtChallengeResponseFields.Len
                    + payload.UserNameFields.Len
                    + payload.WorkstationFields.Len;
            }
        }


        /// <summary>
        /// default empty constructor
        /// </summary>
        public NlmpAuthenticatePacket()
        {
            this.header.MessageType = MessageType_Values.AUTHENTICATE;
            this.payload.MIC = new byte[NlmpUtility.AUTHENTICATE_MESSAGE_MIC_CONST_SIZE];
        }


        /// <summary>
        /// copy constructor.
        /// </summary>
        /// <param name="stackPacket">the source packet</param>
        /// <exception cref="ArgumentNullException">the stackPacket.payload.MIC must not be null</exception>
        /// <exception cref="ArgumentException">the stackPacket.payload.MIC.Length is wrong</exception>
        public NlmpAuthenticatePacket(
            NlmpAuthenticatePacket stackPacket
            )
            : base(stackPacket)
        {
            this.payload = stackPacket.payload;

            if (stackPacket.payload.MIC == null)
            {
                throw new ArgumentNullException("stackPacket", "the stackPacket.payload.MIC must not be null");
            }

            if (stackPacket.payload.MIC.Length != NlmpUtility.AUTHENTICATE_MESSAGE_MIC_CONST_SIZE)
            {
                throw new ArgumentException(
                    string.Format("the stackPacket.payload.MIC.Length must be {0}, actually {1}",
                    NlmpUtility.AUTHENTICATE_MESSAGE_MIC_CONST_SIZE,
                    stackPacket.payload.MIC.Length),
                    "stackPacket");
            }

            this.payload.MIC = new byte[stackPacket.payload.MIC.Length];
            Array.Copy(stackPacket.payload.MIC, this.payload.MIC, stackPacket.payload.MIC.Length);

            if (stackPacket.payload.DomainName != null)
            {
                this.payload.DomainName = new byte[stackPacket.payload.DomainName.Length];
                Array.Copy(
                    stackPacket.payload.DomainName,
                    this.payload.DomainName, stackPacket.payload.DomainName.Length);
            }

            if (stackPacket.payload.UserName != null)
            {
                this.payload.UserName = new byte[stackPacket.payload.UserName.Length];
                Array.Copy(
                    stackPacket.payload.UserName, this.payload.UserName, stackPacket.payload.UserName.Length);
            }

            if (stackPacket.payload.Workstation != null)
            {
                this.payload.Workstation = new byte[stackPacket.payload.Workstation.Length];
                Array.Copy(
                    stackPacket.payload.Workstation,
                    this.payload.Workstation, stackPacket.payload.Workstation.Length);
            }

            if (stackPacket.payload.LmChallengeResponse != null)
            {
                this.payload.LmChallengeResponse = new byte[stackPacket.payload.LmChallengeResponse.Length];
                Array.Copy(
                    stackPacket.payload.LmChallengeResponse,
                    this.payload.LmChallengeResponse, stackPacket.payload.LmChallengeResponse.Length);
            }

            if (stackPacket.payload.NtChallengeResponse != null)
            {
                this.payload.NtChallengeResponse = new byte[stackPacket.payload.NtChallengeResponse.Length];
                Array.Copy(
                    stackPacket.payload.NtChallengeResponse,
                    this.payload.NtChallengeResponse, stackPacket.payload.NtChallengeResponse.Length);
            }

            if (stackPacket.payload.EncryptedRandomSessionKey != null)
            {
                this.payload.EncryptedRandomSessionKey =
                    new byte[stackPacket.payload.EncryptedRandomSessionKey.Length];
                Array.Copy(
                    stackPacket.payload.EncryptedRandomSessionKey,
                    this.payload.EncryptedRandomSessionKey, stackPacket.payload.EncryptedRandomSessionKey.Length);
            }
        }


        /// <summary>
        /// decode packet from bytes
        /// </summary>
        /// <param name="packetBytes">the bytes contain packet</param>
        public NlmpAuthenticatePacket(
            byte[] packetBytes
            )
            : base(packetBytes)
        {
        }


        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>return the clone of this packet</returns>
        public override StackPacket Clone()
        {
            return new NlmpAuthenticatePacket(this);
        }


        /// <summary>
        /// write the payload to bytes. For all sub class to marshal itself.
        /// </summary>
        /// <returns>the bytes of struct</returns>
        protected override byte[] WriteStructToBytes()
        {
            byte[] bytes = NlmpUtility.StructGetBytes(payload);

            if (PayLoadLength != bytes.Length)
            {
                throw new InvalidOperationException("the payload length is not equal to the marshal size!");
            }

            return bytes;
        }


        /// <summary>
        /// read struct from bytes. All sub class override this to unmarshal itself.
        /// </summary>
        /// <param name="start">the start to read bytes</param>
        /// <param name="packetBytes">the bytes of struct</param>
        /// <returns>the read result, if success, return true.</returns>
        protected override bool ReadStructFromBytes(
            byte[] packetBytes, 
            int start
            )
        {
            AUTHENTICATE_MESSAGE authenticate = new AUTHENTICATE_MESSAGE();

            authenticate.LmChallengeResponseFields = NlmpUtility.BytesToStruct<MESSAGE_FIELDS>(
                packetBytes, ref start);

            authenticate.NtChallengeResponseFields = NlmpUtility.BytesToStruct<MESSAGE_FIELDS>(
                packetBytes, ref start);

            authenticate.DomainNameFields = NlmpUtility.BytesToStruct<MESSAGE_FIELDS>(
                packetBytes, ref start);

            authenticate.UserNameFields = NlmpUtility.BytesToStruct<MESSAGE_FIELDS>(
                packetBytes, ref start);

            authenticate.WorkstationFields = NlmpUtility.BytesToStruct<MESSAGE_FIELDS>(
                packetBytes, ref start);

            authenticate.EncryptedRandomSessionKeyFields = NlmpUtility.BytesToStruct<MESSAGE_FIELDS>(
                packetBytes, ref start);

            authenticate.NegotiateFlags = (NegotiateTypes)NlmpUtility.BytesToStruct<uint>(
                packetBytes, ref start);

            authenticate.Version = NlmpUtility.BytesToStruct<VERSION>(
                packetBytes, ref start);

            authenticate.MIC = NlmpUtility.ReadBytes(
                packetBytes, ref start, NlmpUtility.AUTHENTICATE_MESSAGE_MIC_CONST_SIZE);

            int currentIndex = 0;
            while (currentIndex != start)
            {
                currentIndex = start;
                if (authenticate.DomainNameFields.Len != 0 && authenticate.DomainNameFields.BufferOffset == start)
                {
                    authenticate.DomainName = NlmpUtility.ReadBytes(
                        packetBytes, ref start, authenticate.DomainNameFields.Len);
                    continue;
                }
                else if (authenticate.EncryptedRandomSessionKeyFields.Len != 0 
                    && authenticate.EncryptedRandomSessionKeyFields.BufferOffset == start)
                {
                    authenticate.EncryptedRandomSessionKey = NlmpUtility.ReadBytes(
                        packetBytes, ref start, authenticate.EncryptedRandomSessionKeyFields.Len);
                    continue;
                }
                else if (authenticate.LmChallengeResponseFields.Len != 0 
                    && authenticate.LmChallengeResponseFields.BufferOffset == start)
                {
                    authenticate.LmChallengeResponse = NlmpUtility.ReadBytes(
                        packetBytes, ref start, authenticate.LmChallengeResponseFields.Len);
                    continue;
                }
                else if (authenticate.NtChallengeResponseFields.Len != 0 
                    && authenticate.NtChallengeResponseFields.BufferOffset == start)
                {
                    authenticate.NtChallengeResponse = NlmpUtility.ReadBytes(
                        packetBytes, ref start, authenticate.NtChallengeResponseFields.Len);
                    continue;
                }
                else if (authenticate.UserNameFields.Len != 0 && authenticate.UserNameFields.BufferOffset == start)
                {
                    authenticate.UserName = NlmpUtility.ReadBytes(
                        packetBytes, ref start, authenticate.UserNameFields.Len);
                    continue;
                }
                else if (authenticate.WorkstationFields.Len != 0 
                    && authenticate.WorkstationFields.BufferOffset == start)
                {
                    authenticate.Workstation = NlmpUtility.ReadBytes(
                        packetBytes, ref start, authenticate.WorkstationFields.Len);
                    continue;
                }
                else
                {
                    break;
                }
            }

            this.payload = authenticate;
            
            return true;
        }


        /// <summary>
        /// set the version
        /// </summary>
        /// <param name="version">the new version</param>
        public override void SetVersion(
            VERSION version
            )
        {
            payload.Version = version;
        }


        /// <summary>
        /// set the negotiate flags
        /// </summary>
        /// <param name="negotiateFlags">the new flags</param>
        public override void SetNegotiateFlags(
            NegotiateTypes negotiateFlags
            )
        {
            payload.NegotiateFlags = negotiateFlags;
        }


        /// <summary>
        /// set the DomainName of payload
        /// </summary>
        /// <param name="domainName">the new payload value</param>
        public void SetDomainName(
            string domainName
            )
        {
            payload.DomainName = NlmpUtility.StringGetBytes(
                NlmpUtility.UpperCase(domainName), NlmpUtility.IsUnicode(this.payload.NegotiateFlags));
            payload.DomainNameFields.Len = (ushort)payload.DomainName.Length;
            payload.DomainNameFields.MaxLen = (ushort)payload.DomainName.Length;

            UpdateOffset();
        }


        /// <summary>
        /// set the UserName of payload
        /// </summary>
        /// <param name="userName">the new payload value</param>
        public void SetUserName(
            string userName
            )
        {
            payload.UserName = NlmpUtility.StringGetBytes(
                userName, NlmpUtility.IsUnicode(this.payload.NegotiateFlags));
            payload.UserNameFields.Len = (ushort)payload.UserName.Length;
            payload.UserNameFields.MaxLen = (ushort)payload.UserName.Length;

            UpdateOffset();
        }


        /// <summary>
        /// set the Workstation of payload
        /// </summary>
        /// <param name="workstation">the new payload value</param>
        public void SetWorkstation(
            string workstation
            )
        {
            payload.Workstation = NlmpUtility.StringGetBytes(
                workstation, NlmpUtility.IsUnicode(this.payload.NegotiateFlags));
            payload.WorkstationFields.Len = (ushort)payload.Workstation.Length;
            payload.WorkstationFields.MaxLen = (ushort)payload.Workstation.Length;

            UpdateOffset();
        }


        /// <summary>
        /// set the Workstation of payload
        /// </summary>
        /// <param name="lmChallengeResponse">the new payload value</param>
        public void SetLmChallengeResponse(
            byte[] lmChallengeResponse
            )
        {
            if (lmChallengeResponse == null)
            {
                return;
            }

            payload.LmChallengeResponseFields.Len = (ushort)lmChallengeResponse.Length;
            payload.LmChallengeResponseFields.MaxLen = (ushort)lmChallengeResponse.Length;
            payload.LmChallengeResponse = lmChallengeResponse;

            UpdateOffset();
        }


        /// <summary>
        /// set the Workstation of payload
        /// </summary>
        /// <param name="ntChallengeResponse">the new payload value</param>
        public void SetNtChallengeResponse(
            byte[] ntChallengeResponse
            )
        {
            if (ntChallengeResponse == null)
            {
                return;
            }

            payload.NtChallengeResponseFields.Len = (ushort)ntChallengeResponse.Length;
            payload.NtChallengeResponseFields.MaxLen = (ushort)ntChallengeResponse.Length;
            payload.NtChallengeResponse = ntChallengeResponse;

            UpdateOffset();
        }


        /// <summary>
        /// set the Workstation of payload
        /// </summary>
        /// <param name="encryptedRandomSessionKey">the new payload value</param>
        public void SetEncryptedRandomSessionKey(
            byte[] encryptedRandomSessionKey
            )
        {
            if (encryptedRandomSessionKey == null)
            {
                return;
            }

            payload.EncryptedRandomSessionKeyFields.Len = (ushort)encryptedRandomSessionKey.Length;
            payload.EncryptedRandomSessionKeyFields.MaxLen = (ushort)encryptedRandomSessionKey.Length;
            payload.EncryptedRandomSessionKey = encryptedRandomSessionKey;

            UpdateOffset();
        }


        /// <summary>
        /// update the offset of payload
        /// </summary>
        private void UpdateOffset()
        {
            payload.DomainNameFields.BufferOffset = (uint)(HeaderLength + NlmpUtility.AUTHENTICATE_MESSAGE_CONST_SIZE);

            payload.UserNameFields.BufferOffset = (uint)(
                HeaderLength + NlmpUtility.AUTHENTICATE_MESSAGE_CONST_SIZE + payload.DomainNameFields.Len);

            payload.WorkstationFields.BufferOffset = (uint)(
                HeaderLength + NlmpUtility.AUTHENTICATE_MESSAGE_CONST_SIZE + payload.DomainNameFields.Len 
                + payload.UserNameFields.Len);

            payload.LmChallengeResponseFields.BufferOffset = (uint)(
                HeaderLength + NlmpUtility.AUTHENTICATE_MESSAGE_CONST_SIZE + payload.DomainNameFields.Len 
                + payload.UserNameFields.Len + payload.WorkstationFields.Len);

            payload.NtChallengeResponseFields.BufferOffset = (uint)(
                HeaderLength + NlmpUtility.AUTHENTICATE_MESSAGE_CONST_SIZE + payload.DomainNameFields.Len 
                + payload.UserNameFields.Len + payload.WorkstationFields.Len + payload.LmChallengeResponseFields.Len);

            payload.EncryptedRandomSessionKeyFields.BufferOffset = (uint)(
                HeaderLength + NlmpUtility.AUTHENTICATE_MESSAGE_CONST_SIZE + payload.DomainNameFields.Len 
                + payload.UserNameFields.Len + payload.WorkstationFields.Len + payload.LmChallengeResponseFields.Len 
                + payload.NtChallengeResponseFields.Len);
        }
    }
}
