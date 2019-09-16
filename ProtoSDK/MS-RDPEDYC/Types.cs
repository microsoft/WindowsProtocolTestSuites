// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc.Utility;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc
{
    #region Basic Types
    /// <summary>
    /// RDPEDYC specific exception thrown when mismatched PDU is unmarshaled.
    /// </summary>
    public class RdpedycPduMismatchException : Exception
    {
        /// <summary>
        /// Construct an instance with message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public RdpedycPduMismatchException(string message)
            : base(message)
        {
        }
    }

    public class ConstLength
    {
        //According to section 3.1.5.1.4 of MS-RDPEDYC,
        //If the total uncompressed length of the message exceeds 1,590 bytes, 
        //the DYNVC_DATA_FIRST_COMPRESSED (section 2.2.3.3) PDU is sent as the first data PDU, 
        //followed by DYNVC_DATA_COMPRESSED (section 2.2.3.4) PDUs until all the data has been sent.
        public const uint MAX_UNCOMPRESSED_DATA_LENGTH = 1590;

        //According to section 2.2.3.3 of MS-RDPEDYC, 
        //the max length of Data filed of DYNVC_DATA_FIRST_COMPRESSED is:
        // 1600(max chunk len) -1(descriptor) - 1(header)-1(cmd,sp, cbid)- 1( channelid) -2 (length) = 1594.
        public const uint MAX_FIRST_COMPRESSED_DATA_LENGTH = 1594;

        //According to section 2.2.3.4 of MS-RDPEDYC, 
        //the max length of Data filed of DYNVC_DATA_COMPRESSED is:
        // 1600(max chunk len) -1(descriptor) - 1(header)-1(cmd,sp, cbid)- 1( channelid) = 1596.
        public const uint MAX_COMPRESSED_DATA_LENGTH = 1596;

        /// <summary>
        /// Max length of a Data PDU
        /// </summary>
        public const int MAX_CHUNK_LEN = 1600;

    }

    /// <summary>
    /// Header bitmask fields
    /// </summary>
    public class Header
    {
        private const byte cmdBitmask = 0xF0;
        private const byte spBitmask = 0x0C;
        private const byte cbChIdBitmask = 0x03;

        /// <summary>
        ///  Indicates the PDU type and MUST be set to one of the following values.
        ///  0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09
        /// </summary>
        public Cmd_Values Cmd { get; set; }
        /// <summary>
        ///  The value and meaning depend on the Cmd field.
        /// This field meaning can be Len, Sp, Pri, etc, depends on the Cmd field.
        /// </summary>
        public int Sp { get; set; }
        /// <summary>
        ///  Indicates the length of the ChannelId field.
        /// </summary>
        public cbChId_Values CbChannelId { get; set; }

        /// <summary>
        /// Raw data of the header fields.
        /// </summary>
        public byte RawData
        {
            get
            {
                // Always re-caculate the latest result.
                return (byte)(((int)Cmd << 4) | (Sp << 2) | (int)CbChannelId);
            }
        }

        /// <summary>
        /// Unmarshal constructor.
        /// </summary>
        /// <param name="data">raw data to be unmarshaled.</param>
        public Header(byte data)
        {
            Cmd = (Cmd_Values)((data & cmdBitmask) >> 4);
            Sp = (int)((data & spBitmask) >> 2);
            CbChannelId = (cbChId_Values)(data & cbChIdBitmask);
        }

        /// <summary>
        /// Marshal constructor
        /// </summary>
        /// <param name="cmd">Cmd field</param>
        /// <param name="sp">SP/Pri/Len field</param>
        /// <param name="cbChId">cbChId field</param>
        public Header(Cmd_Values cmd, int sp, cbChId_Values cbChId)
        {
            this.Cmd = cmd;
            this.Sp = sp;
            this.CbChannelId = cbChId;
        }

    }

    public enum Cmd_Values : int
    {

        /// <summary>
        ///  The message contained in the optionalFields field is
        ///  a Create Request PDU or a Create Response PDU.
        /// </summary>
        Create = 0x01,

        /// <summary>
        ///  The message contained in the optionalFields field is
        ///  a Data First PDU.
        /// </summary>
        FirstData = 0x02,

        /// <summary>
        ///  The message contained in the optionalFields field is
        ///  a Data PDU.
        /// </summary>
        Data = 0x03,

        /// <summary>
        ///  The message contained in the optionalFields field is
        ///  a Close Request PDU or a Close Response PDU.
        /// </summary>
        Close = 0x04,

        /// <summary>
        ///  The message contained in the optionalFields field is
        ///  a Capability Request PDU or a Capabilities Response
        ///  PDU.
        /// </summary>
        Capability = 0x05,

        /// <summary>
        /// The message contained in the optionalFields field is 
        /// a Data First Compressed PDU (section 2.2.3.3).
        /// </summary>
        FirstDataCompressed = 0x06,

        /// <summary>
        /// The message contained in the optionalFields field is 
        /// a Data Compressed PDU (section 2.2.3.4).
        /// </summary>
        DataCompressed = 0x07,

        /// <summary>
        /// The message contained in the optionalFields field is 
        /// a Soft-Sync Request PDU (section 2.2.5.1).
        /// </summary>
        SoftSyncReq = 0x08,

        /// <summary>
        /// The message contained in the optionalFields field is 
        /// a Soft-Sync Response PDU (section 2.2.5.2).
        /// </summary>
        SoftSyncRes = 0x09,

        /// <summary>
        /// Not supported by the protocol. Only apply to marshaling runtimes.        
        /// </summary>
        Unknown = 0x0F
    }

    public enum cbChId_Values : int
    {

        /// <summary>
        ///  The ChannelId is 1 byte wide.
        /// </summary>
        OneByte = 0x00,

        /// <summary>
        ///  The ChannelId is 2 bytes wide.
        /// </summary>
        TwoBytes = 0x01,

        /// <summary>
        ///  The ChannelId is 4 bytes wide.
        /// </summary>
        FourBytes = 0x02,

        /// <summary>
        ///  The ChannelId is 4 bytes wide.
        /// </summary>
        Invalid = 0x03
    }

    public enum Len_Values : int
    {

        /// <summary>
        ///  Length field length is 1 byte.
        /// </summary>
        OneByte = 0x0,

        /// <summary>
        ///  Length field length is 2 bytes.
        /// </summary>
        TwoBytes = 0x1,

        /// <summary>
        ///  Length field length is 4 bytes.
        /// </summary>
        FourBytes = 0x2,

        /// <summary>
        ///  The ChannelId is 4 bytes wide.
        /// </summary>
        Invalid = 0x3,
    }

    public enum DynamicVC_TransportType : uint
    {
        /// <summary>
        /// use static virtual channel
        /// </summary>
        RDP_TCP = 0x0,

        /// <summary>
        /// use reliable UDP transport
        /// </summary>
        RDP_UDP_Reliable = 0x1,

        /// <summary>
        /// use lossy UDP transport
        /// </summary>
        RDP_UDP_Lossy = 0x2,
    }

    /// <summary>
    /// DYNVC_CAPS version
    /// </summary>
    public enum DYNVC_CAPS_Version : ushort
    {
        VERSION1 = 0x0001,
        VERSION2 = 0x0002,
        VERSION3 = 0x0003,
    }

    #endregion

    #region MS-RDPEDYC PDUs

    public abstract class DynamicVCPDU : BasePDU
    {
        public virtual Header HeaderBits { get; set; }

        public byte[] RawData { get; set; }

        public virtual uint ChannelId { get; set; }

        protected abstract Cmd_Values Cmd { get; }

        public DynamicVCPDU()
        {
            // Unknown PDU doesn't support header.
            if (Cmd != Cmd_Values.Unknown)
            {
                HeaderBits = new Header(Cmd, 0, 0);
            }
        }

        #region Encoding Members

        public override void Encode(PduMarshaler marshaler)
        {
            if (Cmd != Cmd_Values.Unknown)
            {
                marshaler.WriteByte(HeaderBits.RawData);
            }

            DoMarshal(marshaler);

            SetRawData(true, marshaler);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            HeaderBits = new Header(marshaler.ReadByte());

            Cmd_Values c = HeaderBits.Cmd;

            // TODO: Check this logic
            // Check in the special case that the Cmd bit field just
            // equals to Cmd_Values.Unknown
            if (c == Cmd_Values.Unknown)
            {
                return false;
            }

            try
            {
                if (c == Cmd)
                {
                    DoUnmarshal(marshaler);
                    SetRawData(false, marshaler);
                    return true;
                }
            }
            catch (OverflowException)
            {
                marshaler.Reset();
            }
            catch (RdpedycPduMismatchException)
            {
                marshaler.Reset();
            }

            return false;
        }

        protected abstract void DoUnmarshal(PduMarshaler marshaler);

        protected abstract void DoMarshal(PduMarshaler marshaler);

        #endregion

        #region Private and Protected Methods

        protected void ReadChannelId(PduMarshaler marshaler)
        {
            uint res = 0;
            switch (HeaderBits.CbChannelId)
            {
                case cbChId_Values.OneByte:
                    res = Convert.ToUInt32(marshaler.ReadByte());
                    break;
                case cbChId_Values.TwoBytes:
                    res = Convert.ToUInt32(marshaler.ReadUInt16());
                    break;
                case cbChId_Values.FourBytes:
                    res = Convert.ToUInt32(marshaler.ReadUInt32());
                    break;
                case cbChId_Values.Invalid:
                default:
                    //TODO: handle errors.
                    break;
            }

            //TODO: handle errors.
            ChannelId = res;
        }

        protected void WriteChannelId(PduMarshaler marshaler)
        {
            //TODO: Refine this method
            switch (HeaderBits.CbChannelId)
            {
                case cbChId_Values.OneByte:
                    marshaler.WriteByte(Convert.ToByte(ChannelId));
                    break;
                case cbChId_Values.TwoBytes:
                    marshaler.WriteUInt16(Convert.ToUInt16(ChannelId));
                    break;
                case cbChId_Values.FourBytes:
                    marshaler.WriteUInt32(Convert.ToUInt32(ChannelId));
                    break;
                case cbChId_Values.Invalid:
                default:
                    DynamicVCException.Throw("chChId is invalid.");
                    break;
            }
        }

        protected void UpdateCbChannelId()
        {
            // TODO: check this logic
            if (ChannelId <= Byte.MaxValue)
            {
                HeaderBits.CbChannelId = cbChId_Values.OneByte;
            }
            else if (ChannelId <= UInt16.MaxValue)
            {
                HeaderBits.CbChannelId = cbChId_Values.TwoBytes;
            }
            else if (ChannelId <= UInt32.MaxValue)
            {
                HeaderBits.CbChannelId = cbChId_Values.FourBytes;
            }
            else
            {
                DynamicVCException.Throw("ChannelId is valid.");
            }
        }

        protected uint ReadLength(PduMarshaler marshaler)
        {
            uint res = 0;
            switch ((Len_Values)HeaderBits.Sp)
            {
                case Len_Values.OneByte:
                    res = Convert.ToUInt32(marshaler.ReadByte());
                    break;
                case Len_Values.TwoBytes:
                    res = Convert.ToUInt32(marshaler.ReadUInt16());
                    break;
                case Len_Values.FourBytes:
                    res = Convert.ToUInt32(marshaler.ReadUInt32());
                    break;
                case Len_Values.Invalid:
                default:
                    //TODO: handle errors.
                    break;
            }

            //TODO: handle errors.
            return res;
        }

        protected void WriteLength(PduMarshaler marshaler, uint Length)
        {
            //TODO: Refine this method
            switch ((Len_Values)HeaderBits.Sp)
            {
                case Len_Values.OneByte:
                    marshaler.WriteByte(Convert.ToByte(Length));
                    break;
                case Len_Values.TwoBytes:
                    marshaler.WriteUInt16(Convert.ToUInt16(Length));
                    break;
                case Len_Values.FourBytes:
                    marshaler.WriteUInt32(Convert.ToUInt32(Length));
                    break;
                case Len_Values.Invalid:
                default:
                    DynamicVCException.Throw("Len is invalid.");
                    break;
            }
        }

        protected void UpdateLengthOfLength(uint length)
        {
            // TODO: check this logic
            if (length <= Byte.MaxValue)
            {
                HeaderBits.Sp = (int)Len_Values.OneByte;
            }
            else if (length <= UInt16.MaxValue)
            {
                HeaderBits.Sp = (int)Len_Values.TwoBytes;
            }
            else if (length <= UInt32.MaxValue)
            {
                HeaderBits.Sp = (int)Len_Values.FourBytes;
            }
            else
            {
                DynamicVCException.Throw("The field Length is too long.");
            }
        }

        private void SetRawData(bool marshaling, PduMarshaler marshaler)
        {
            byte[] data = marshaler.RawData;

            if ((null == data) || (data.Length <= 0))
            {
                if (marshaling)
                {
                    DynamicVCException.Throw("The PDU object was not marshaled successfully.");
                }
                else
                {
                    DynamicVCException.Throw("The PDU object was not unmarshaled successfully.");
                }
            }
            RawData = data;
        }

        #endregion
    }

    public class UnknownDynamicVCPDU : DynamicVCPDU
    {
        public override Header HeaderBits
        {
            get
            {
                DynamicVCException.Throw("UnknownDynamicVCPDU doesn't support valid header fields.");
                return null;
            }
            set
            {
                DynamicVCException.Throw("UnknownDynamicVCPDU doesn't support valid header fields.");
            }
        }

        public override uint ChannelId
        {
            get
            {
                DynamicVCException.Throw("UnknownDynamicVCPDU doesn't support channel ID.");
                return 0;
            }
            set
            {
                DynamicVCException.Throw("UnknownDynamicVCPDU doesn't support  channel ID.");
            }
        }

        public UnknownDynamicVCPDU()
        {
        }

        public UnknownDynamicVCPDU(byte[] data)
        {
            RawData = data;
        }

        protected override Cmd_Values Cmd
        {
            get
            {
                return Cmd_Values.Unknown;
            }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            marshaler.WriteBytes(RawData);
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
        }
    }

    public abstract class RequestDvcPDU : DynamicVCPDU
    {
    }

    public abstract class ResponseDvcPDU : DynamicVCPDU
    {
    }

    #region Initializing Dynamic Virtual Channels

    public class CapsVer1ReqDvcPdu : RequestDvcPDU
    {
        public byte Pad;
        public ushort Version;

        public override uint ChannelId
        {
            get
            {
                DynamicVCException.Throw("CapsVer1ReqDvcPdu doesn't support channel ID.");
                return 0;
            }
            set
            {
                DynamicVCException.Throw("CapsVer1ReqDvcPdu doesn't support  channel ID.");
            }
        }

        public CapsVer1ReqDvcPdu()
        {
            HeaderBits = new Header(Cmd_Values.Capability, 0x00, cbChId_Values.OneByte);

            Pad = 0x00;
            Version = 0x0001;
        }

        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.Capability; }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            marshaler.WriteByte(Pad);
            marshaler.WriteUInt16(Version);
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            Pad = marshaler.ReadByte();
            Version = marshaler.ReadUInt16();

            if (Version != (ushort)DYNVC_CAPS_Version.VERSION1)
            {
                throw new RdpedycPduMismatchException($"Version should be {DYNVC_CAPS_Version.VERSION1}!");
            }
        }
    }

    public class CapsVer2ReqDvcPdu : RequestDvcPDU
    {
        public byte Pad;
        public ushort Version;
        public ushort PriorityCharge0;
        public ushort PriorityCharge1;
        public ushort PriorityCharge2;
        public ushort PriorityCharge3;

        public override uint ChannelId
        {
            get
            {
                DynamicVCException.Throw("CapsVer2ReqDvcPdu doesn't support channel ID.");
                return 0;
            }
            set
            {
                DynamicVCException.Throw("CapsVer2ReqDvcPdu doesn't support  channel ID.");
            }
        }

        public CapsVer2ReqDvcPdu()
        {
            HeaderBits = new Header(Cmd_Values.Capability, 0x00, cbChId_Values.OneByte);

            Pad = 0x00;
            Version = 0x0002;
        }

        public CapsVer2ReqDvcPdu(
            ushort priorityCharge0,
            ushort priorityCharge1,
            ushort priorityCharge2,
            ushort priorityCharge3
            )
        {
            HeaderBits = new Header(Cmd_Values.Capability, 0x00, cbChId_Values.OneByte);

            Pad = 0x00;
            Version = 0x0002;

            this.PriorityCharge0 = priorityCharge0;
            this.PriorityCharge1 = priorityCharge1;
            this.PriorityCharge2 = priorityCharge2;
            this.PriorityCharge3 = priorityCharge3;
        }

        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.Capability; }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            marshaler.WriteByte(Pad);
            marshaler.WriteUInt16(Version);
            marshaler.WriteUInt16(PriorityCharge0);
            marshaler.WriteUInt16(PriorityCharge1);
            marshaler.WriteUInt16(PriorityCharge2);
            marshaler.WriteUInt16(PriorityCharge3);
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            Pad = marshaler.ReadByte();
            Version = marshaler.ReadUInt16();

            if (Version != (ushort)DYNVC_CAPS_Version.VERSION2)
            {
                throw new RdpedycPduMismatchException($"Version should be {DYNVC_CAPS_Version.VERSION2}!");
            }

            PriorityCharge0 = marshaler.ReadUInt16();
            PriorityCharge1 = marshaler.ReadUInt16();
            PriorityCharge2 = marshaler.ReadUInt16();
            PriorityCharge3 = marshaler.ReadUInt16();
        }
    }

    public class CapsVer3ReqDvcPdu : RequestDvcPDU
    {
        public byte Pad;
        public ushort Version;
        public ushort PriorityCharge0;
        public ushort PriorityCharge1;
        public ushort PriorityCharge2;
        public ushort PriorityCharge3;

        public override uint ChannelId
        {
            get
            {
                DynamicVCException.Throw("CapsVer2ReqDvcPdu doesn't support channel ID.");
                return 0;
            }
            set
            {
                DynamicVCException.Throw("CapsVer2ReqDvcPdu doesn't support  channel ID.");
            }
        }

        public CapsVer3ReqDvcPdu()
        {
            HeaderBits = new Header(Cmd_Values.Capability, 0x00, cbChId_Values.OneByte);

            Pad = 0x00;
            Version = 0x0003;
        }

        public CapsVer3ReqDvcPdu(
            ushort priorityCharge0,
            ushort priorityCharge1,
            ushort priorityCharge2,
            ushort priorityCharge3
            )
        {
            HeaderBits = new Header(Cmd_Values.Capability, 0x00, cbChId_Values.OneByte);

            Pad = 0x00;
            Version = 0x0003;

            this.PriorityCharge0 = priorityCharge0;
            this.PriorityCharge1 = priorityCharge1;
            this.PriorityCharge2 = priorityCharge2;
            this.PriorityCharge3 = priorityCharge3;
        }

        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.Capability; }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            marshaler.WriteByte(Pad);
            marshaler.WriteUInt16(Version);
            marshaler.WriteUInt16(PriorityCharge0);
            marshaler.WriteUInt16(PriorityCharge1);
            marshaler.WriteUInt16(PriorityCharge2);
            marshaler.WriteUInt16(PriorityCharge3);
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            Pad = marshaler.ReadByte();
            Version = marshaler.ReadUInt16();

            if (Version != (ushort)DYNVC_CAPS_Version.VERSION3)
            {
                throw new RdpedycPduMismatchException($"Version should be {DYNVC_CAPS_Version.VERSION3}!");
            }

            PriorityCharge0 = marshaler.ReadUInt16();
            PriorityCharge1 = marshaler.ReadUInt16();
            PriorityCharge2 = marshaler.ReadUInt16();
            PriorityCharge3 = marshaler.ReadUInt16();
        }
    }

    public class CapsRespDvcPdu : ResponseDvcPDU
    {
        public byte Pad;
        public ushort Version;

        public override uint ChannelId
        {
            get
            {
                DynamicVCException.Throw("CapsRespDvcPdu doesn't support channel ID.");
                return 0;
            }
            set
            {
                DynamicVCException.Throw("CapsRespDvcPdu doesn't support  channel ID.");
            }
        }

        public CapsRespDvcPdu()
        {
            HeaderBits = new Header(Cmd_Values.Capability, 0x00, cbChId_Values.OneByte);
            Pad = 0x00;
        }

        public CapsRespDvcPdu(ushort version)
        {
            HeaderBits = new Header(Cmd_Values.Capability, 0x00, cbChId_Values.OneByte);
            Pad = 0x00;
            Version = version;
        }

        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.Capability; }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            marshaler.WriteByte(Pad);
            marshaler.WriteUInt16(Version);
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            Pad = marshaler.ReadByte();
            Version = marshaler.ReadUInt16();
        }
    }

    #endregion

    #region Opening Dynamic Virtual Channel PDUs
    /// <summary>
    /// The DYNVC_CREATE_REQ (section 2.2.2.1) PDU is sent by the DVC server manager to the DVC client manager to request that a channel be opened.
    /// The header of DYNVC_CREATE_REQ contains fields: cbId, Pri and Cmd 
    /// </summary>
    public class CreateReqDvcPdu : RequestDvcPDU
    {
        public string ChannelName;

        public CreateReqDvcPdu()
        {
        }

        public CreateReqDvcPdu(int priority, uint channelId, string channelName)
        {
            HeaderBits = new Header(Cmd, priority, cbChId_Values.Invalid);

            this.ChannelId = channelId;
            this.ChannelName = channelName;

            UpdateCbChannelId();
        }

        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.Create; }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            WriteChannelId(marshaler);
            marshaler.WriteASCIIString(ChannelName);
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            ReadChannelId(marshaler);
            ChannelName = marshaler.ReadASCIIString();
        }
    }

    /// <summary>
    /// The DYNVC_CREATE_RSP (section 2.2.2.2) PDU is sent by the DVC client manager to indicate the status of the client DVC create operation.
    /// The header of DYNVC_CREATE_REQ contains fields: cbId, Sp and Cmd 
    /// </summary>
    public class CreateRespDvcPdu : ResponseDvcPDU
    {
        // A 32-bit, signed integer that specifies the NTSTATUS code that indicates success or 
        // failure of the client dynamic virtual channel creation. NTSTATUS codes are specified 
        // in [MS-ERREF] section 2.3. A zero or positive value indicates success; a negative value
        // indicates failure.
        public int CreationStatus { set; get; }

        public CreateRespDvcPdu()
        {
        }

        public CreateRespDvcPdu(uint channelId, int creationStatus)
        {
            HeaderBits = new Header(Cmd_Values.Create, 0x00, cbChId_Values.Invalid);

            this.ChannelId = channelId;
            this.CreationStatus = creationStatus;

            UpdateCbChannelId();
        }

        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.Create; }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            WriteChannelId(marshaler);
            marshaler.WriteInt32(CreationStatus);
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            ReadChannelId(marshaler);
            CreationStatus = marshaler.ReadInt32();
        }
    }

    #endregion

    #region Sending and Receiving Data PDUs

    public abstract class DataDvcBasePdu : DynamicVCPDU
    {
        public byte[] Data { get; set; }

        /// <summary>
        /// Compute the Non Data size.
        /// </summary>
        /// <param name="isSp">Indicates the 3-4 bits are Sp or Len</param>
        /// <returns></returns>
        public int NonDataSize(bool isSp)
        {
            // Header length = 1;
            int len = 1;

            // cbChId indicates the length of the ChannelId field.
            switch (this.HeaderBits.CbChannelId)
            {
                case cbChId_Values.OneByte:
                    len += 1;
                    break;
                case cbChId_Values.TwoBytes:
                    len += 2;
                    break;
                case cbChId_Values.FourBytes:
                    len += 4;
                    break;
                case cbChId_Values.Invalid:
                default:
                    DynamicVCException.Throw("The length of the ChannelId field is invalid.");
                    break;
            }

            if (!isSp)
            {
                // If the 3-4 bit is Len, this value indicates the length of Length field.
                // Length field is applied to DYNVC_DATA_FIRST and DYNVC_DATA_FIRST_COMPRESSED structures.
                switch ((Len_Values)this.HeaderBits.Sp)
                {
                    case Len_Values.OneByte:
                        len += 1;
                        break;
                    case Len_Values.TwoBytes:
                        len += 2;
                        break;
                    case Len_Values.FourBytes:
                        len += 4;
                        break;
                    case Len_Values.Invalid:
                    default:
                        DynamicVCException.Throw("The length of Length field is invalid.");
                        break;
                }
            }

            return len;
        }
    }

    public class DataFirstDvcPdu : DataDvcBasePdu
    {
        public uint Length { get; set; }

        public DataFirstDvcPdu()
        {
        }

        public DataFirstDvcPdu(uint channelId, uint length, byte[] data)
        {
            this.ChannelId = channelId;
            this.Length = length;
            this.Data = data;

            UpdateCbChannelId();
            UpdateLengthOfLength(length);
        }

        public int GetNonDataSize()
        {
            return NonDataSize(false);
        }

        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.FirstData; }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            WriteChannelId(marshaler);
            WriteLength(marshaler, this.Length);
            marshaler.WriteBytes(this.Data);
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            ReadChannelId(marshaler);
            Length = ReadLength(marshaler);
            Data = marshaler.ReadToEnd();
        }
    }

    public class DataDvcPdu : DataDvcBasePdu
    {
        public DataDvcPdu()
        {
        }

        public DataDvcPdu(uint channelId, byte[] data)
        {
            this.ChannelId = channelId;
            this.Data = data;

            UpdateCbChannelId();
        }

        public int GetNonDataSize()
        {
            return NonDataSize(true);
        }

        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.Data; }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            WriteChannelId(marshaler);
            marshaler.WriteBytes(Data);
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            ReadChannelId(marshaler);
            this.Data = marshaler.ReadToEnd();
        }
    }

    /// <summary>
    /// The DYNVC_DATA_FIRST_COMPRESSED PDU is used to send the first block of data of a fragmented message 
    /// when the data block is compressed.
    /// </summary>
    public class DataFirstCompressedDvcPdu : DataDvcBasePdu
    {
        public uint Length { get; set; }

        public DataFirstCompressedDvcPdu() { }

        /// <summary>
        /// Construct the DataFirstCompressedDvcPdu
        /// </summary>
        /// <param name="channelId">channelId</param>
        /// <param name="length">length for the total uncompressed data length</param>
        /// <param name="data">The co</param>
        public DataFirstCompressedDvcPdu(uint channelId, uint length, byte[] data)
        {
            this.ChannelId = channelId;
            this.Length = length;

            this.Data = data;

            UpdateCbChannelId();
            UpdateLengthOfLength(length);
        }

        public int GetNonDataSize()
        {
            return NonDataSize(false);
        }

        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.FirstDataCompressed; }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            WriteChannelId(marshaler);
            WriteLength(marshaler, this.Length);
            marshaler.WriteBytes(this.Data);
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            ReadChannelId(marshaler);
            Length = ReadLength(marshaler);
            Data = marshaler.ReadToEnd();
        }

    }

    /// <summary>
    /// Types of RDP_SEGMENTED_DATA: Single or Mutipart.
    /// </summary>
    public enum DescriptorTypes : byte
    {
        /// <summary>
        /// Specifies whether the RDP_SEGMENTED_DATA structure wraps a single segment. The segmentCount, 
        /// uncompressedSize, and segmentArray fields MUST NOT be present, and the segment field MUST be present.
        /// </summary>
        SINGLE = 0xE0,
        /// <summary>
        /// Specifies whether the RDP_SEGMENTED_DATA structure wraps multiple segments. The segmentCount,  
        /// uncompressedSize, and segmentArray fields MUST be present, and the segment field MUST NOT be present. 
        /// </summary>
        MULTIPART = 0xE1
    }

    public enum SEGMENT_PART_SISE : uint
    {
        MAX_PACKET_COMPR_TYPE_RDP8_SEGMENT_PART_SIZE = 65535,  // EGFX: Maximum number of uncompressed bytes in a single segment is 65535
        MAX_PACKET_COMPR_TYPE_RDP8_LITE_SEGMENT_PART_SIZE = 8192,  //EDYC: Maximum number of uncompressed bytes in a single segment: 8,192 for DYNVC_DATA_FIRST_COMPRESSED
        MAX_PACKET_COMPR_TYPE_RDP8_MATCH_DISTANCE = 2500000, //EGFX: Maximum match distance / minimum history size: 2,500,000 bytes.
        MAX_PACKET_COMPR_TYPE_RDP8_LITE_MATCH_DISTANCE = 8192 //EDYC: Maximum match distance / minimum history size: 8,192 bytes instead of 2,500,000 bytes.
    }

    public enum PACKET_COMPR_FLAG : byte
    {
        PACKET_COMPR_TYPE_RDP8 = 0x04,
        PACKET_COMPR_TYPE_LITE = 0x06, //DYNVC_DATA_FIRST_COMPRESSED 
        PACKET_COMPRESSED = 0x20
    }
    /// <summary>
    /// The DYNVC_DATA_COMPRESSED PDU is used to send both single messages and blocks of fragmented messages 
    /// when the data block is compressed.
    /// </summary>
    public class DataCompressedDvcPdu : DataDvcBasePdu
    {
        public DataCompressedDvcPdu() { }

        public DataCompressedDvcPdu(uint channelId, byte[] data)
        {
            HeaderBits.Cmd = Cmd_Values.DataCompressed;
            HeaderBits.Sp = 0x00;

            this.ChannelId = channelId;
            this.Data = data;
            UpdateCbChannelId();
        }

        public int GetNonDataSize()
        {
            return NonDataSize(true);
        }

        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.DataCompressed; }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            WriteChannelId(marshaler);
            marshaler.WriteBytes(Data);
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            ReadChannelId(marshaler);
            this.Data = marshaler.ReadToEnd();
        }
    }
    #endregion

    #region Closing Dynamic Virtual Channel PDUs

    public class CloseDvcPdu : DynamicVCPDU
    {
        public CloseDvcPdu()
        {
        }

        public CloseDvcPdu(uint channelId)
        {
            HeaderBits.Cmd = Cmd_Values.Close;
            HeaderBits.Sp = 0x00;
            ChannelId = channelId;

            UpdateCbChannelId();
        }

        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.Close; }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            WriteChannelId(marshaler);
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            ReadChannelId(marshaler);
        }
    }

    #endregion

    #region Soft-Sync
    /// <summary>
    /// A DYNVC_SOFT_SYNC_REQUEST PDU is sent by a DVC server manager over the DRDYNVC static virtual channel 
    /// on the main RDP connection.
    /// </summary>
    public class SoftSyncReqDvcPDU : RequestDvcPDU
    {
        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.SoftSyncReq; }
        }
        public byte Pad;
        public UInt32 Length;
        public SoftSyncReqFlags_Value Flags;
        public ushort NumberOfTunnels;
        public SoftSyncChannelList[] SoftSyncChannelLists;
        public override uint ChannelId
        {
            get
            {
                DynamicVCException.Throw("CapsVer1ReqDvcPdu doesn't support channel ID.");
                return 0;
            }
            set
            {
                DynamicVCException.Throw("CapsVer1ReqDvcPdu doesn't support  channel ID.");
            }
        }

        public SoftSyncReqDvcPDU() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public SoftSyncReqDvcPDU(SoftSyncReqFlags_Value flags, ushort numberOfTunnels, SoftSyncChannelList[] softSyncChannelLists)
        {
            HeaderBits = new Header(Cmd_Values.SoftSyncReq, 0x00, 0x00);
            this.Pad = 0x00;
            this.Flags = flags;
            this.NumberOfTunnels = numberOfTunnels;
            this.SoftSyncChannelLists = softSyncChannelLists;

            // Section 2.2.5.1, Length (4 bytes): A 32-bit, unsigned integer indicating the total size, in bytes, of the Length, Flags, NumberOfTunnels, and SoftSyncChannelLists fields.

            this.Length = 8;

            if (flags.HasFlag(SoftSyncReqFlags_Value.SOFT_SYNC_CHANNEL_LIST_PRESENT))
            {
                if (softSyncChannelLists == null)
                {
                    DynamicVCException.Throw("Should have one or more Soft-Sync Channel Lists.");
                }

                foreach (var channel in softSyncChannelLists)
                {
                    this.Length += (uint)channel.GetSize();
                }
            }
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            marshaler.WriteByte(Pad);
            marshaler.WriteUInt32(Length);
            marshaler.WriteUInt16((ushort)Flags);
            marshaler.WriteUInt16((ushort)NumberOfTunnels);
            if (SoftSyncChannelLists != null)
            {
                foreach (var li in SoftSyncChannelLists)
                {
                    marshaler.WriteBytes(li.Encode());
                }
            }
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            this.Pad = marshaler.ReadByte();
            this.Length = marshaler.ReadUInt32();
            this.Flags = (SoftSyncReqFlags_Value)marshaler.ReadUInt16();
            this.NumberOfTunnels = marshaler.ReadUInt16();
            List<SoftSyncChannelList> list = new List<SoftSyncChannelList>();

            for (int i = 0; i < NumberOfTunnels; ++i)
            {
                SoftSyncChannelList channel = new SoftSyncChannelList((TunnelType_Value)marshaler.ReadUInt32(), marshaler.ReadUInt16());

                List<uint> Ids = new List<uint>();
                for (int k = 0; k < channel.NumberOfDVCs; ++k)
                {
                    Ids.Add(marshaler.ReadUInt32());
                }
                channel.ListOfDVCIds = Ids.ToArray();
                list.Add(channel);
            }
            this.SoftSyncChannelLists = list.ToArray();
        }

    }

    /// <summary>
    /// A 16-bit, unsigned integer that specifies the contents of this PDU.
    /// </summary>
    public enum SoftSyncReqFlags_Value : ushort
    {
        /// <summary>
        /// Indicates that no more data will be sent over TCP for the specified DVCs. This flag MUST be set.
        /// </summary>
        SOFT_SYNC_TCP_FLUSHED = 0x01,

        /// <summary>
        /// Indicates that one or more Soft-Sync Channel Lists (section 3.1.5.1.1) are present in this PDU.
        /// </summary>
        SOFT_SYNC_CHANNEL_LIST_PRESENT = 0x02
    }

    /// <summary>
    /// One or more DYNVC_SOFT_SYNC_CHANNEL_LISTs are contained in a Soft-Sync Request PDU to indicate 
    /// which dynamic virtual channels have data written by the server on the specified multitransport tunnel
    /// </summary>
    public class SoftSyncChannelList
    {
        /// <summary>
        /// Indicates the target tunnel type for the transport switch.
        /// </summary>
        public TunnelType_Value TunnelType;

        /// <summary>
        /// A 16-bit, unsigned integer indicating the number of DVCs that will have data written by the server manager on this tunnel.
        /// </summary>
        public ushort NumberOfDVCs;

        /// <summary>
        /// One or more 32-bit, unsigned integers, as indicated by the NumberOfDVCs field, 
        /// containing the channel ID of each DVC that will have data written by the server manager on this tunnel.
        /// </summary>
        public uint[] ListOfDVCIds;

        /// <summary>
        /// Encode method.
        /// </summary>
        /// <returns>Binary value of SoftSyncChannelList</returns>
        public byte[] Encode()
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(TypeMarshal.ToBytes<TunnelType_Value>(TunnelType));
            buffer.AddRange(TypeMarshal.ToBytes<ushort>(NumberOfDVCs));
            foreach (uint i in ListOfDVCIds)
            {
                buffer.AddRange(TypeMarshal.ToBytes<uint>(i));
            }
            return buffer.ToArray();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SoftSyncChannelList(TunnelType_Value tunnelType, ushort numberOfDVCs, List<uint> listOfIds = null)
        {
            this.TunnelType = tunnelType;
            this.NumberOfDVCs = numberOfDVCs;
            if (listOfIds != null)
                this.ListOfDVCIds = listOfIds.ToArray();
        }

        public uint GetSize()
        {
            // TunnelType: 4 bytes; NumberOfDVCs: 2 bytes; ListOfDVCIds: NumberOfDVCs * 4 bytes.
            return (uint)(4 + 2 + NumberOfDVCs * 4);
        }

        public SoftSyncChannelList() { }

    }

    /// <summary>
    /// Indicates the target tunnel type for the transport switch
    /// </summary>
    public enum TunnelType_Value : uint
    {
        /// <summary>
        /// RDP-UDP Forward Error Correction (FEC) multitransport tunnel ([MS-RDPEMT] section 1.3).
        /// </summary>
        TUNNELTYPE_UDPFECR = 0x00000001,

        /// <summary>
        /// RDP-UDP FEC lossy multitransport tunnel ([MS-RDPEMT] section 1.3).
        /// </summary>
        TUNNELTYPE_UDPFECL = 0x00000003
    }

    /// <summary>
    /// A DYNVC_SOFT_SYNC_RESPONSE PDU is sent by a DVC client manager over the DRDYNVC static virtual channel 
    /// on the main RDP connection in response to a Soft-Sync Request PDU (section 2.2.5.1). 
    /// </summary>
    public class SoftSyncResDvcPdu : ResponseDvcPDU
    {
        protected override Cmd_Values Cmd
        {
            get { return Cmd_Values.SoftSyncRes; }
        }

        public byte Pad;
        public uint NumberOfTunnels;
        public TunnelType_Value[] TunnelsToSwitch;
        public override uint ChannelId
        {
            get
            {
                DynamicVCException.Throw("CapsVer1ReqDvcPdu doesn't support channel ID.");
                return 0;
            }
            set
            {
                DynamicVCException.Throw("CapsVer1ReqDvcPdu doesn't support  channel ID.");
            }
        }

        public SoftSyncResDvcPdu() { }

        public SoftSyncResDvcPdu(uint numberOfTunnels, TunnelType_Value[] tunnelsToSwitch)
        {
            HeaderBits = new Header(Cmd_Values.SoftSyncRes, 0x00, 0x00);
            Pad = 0x00;
            NumberOfTunnels = numberOfTunnels;
            TunnelsToSwitch = tunnelsToSwitch;
        }

        protected override void DoMarshal(PduMarshaler marshaler)
        {
            marshaler.WriteByte(Pad);
            marshaler.WriteUInt32(NumberOfTunnels);
            foreach (var tunnel in TunnelsToSwitch)
            {
                marshaler.WriteBytes(TypeMarshal.ToBytes<TunnelType_Value>(tunnel));
            }
        }

        protected override void DoUnmarshal(PduMarshaler marshaler)
        {
            this.Pad = marshaler.ReadByte();
            this.NumberOfTunnels = marshaler.ReadUInt32();
            List<TunnelType_Value> tunnelsToSwitchList = new List<TunnelType_Value>();
            for (int i = 0; i < NumberOfTunnels; ++i)
            {
                tunnelsToSwitchList.Add((TunnelType_Value)marshaler.ReadUInt32());
            }
            this.TunnelsToSwitch = tunnelsToSwitchList.ToArray();
        }
    }
    #endregion 
    #endregion
}
