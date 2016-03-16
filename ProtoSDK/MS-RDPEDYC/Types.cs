// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc.Utility;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc
{
    #region Basic Types

    /// <summary>
    /// Header bitmask fields
    /// </summary>
    public class Header
    {
        private const byte cmdBitmask = 0xF0;
        private const byte spBitmask = 0x0C;
        private const byte cbChIdBitmask = 0x03;

        /// <summary>
        ///  Indicates the PDU type and MUST be set to one of the
        ///  following values.
        ///  0x01, 0x02, 0x03, 0x04, 0x05  
        /// </summary>
        public Cmd_Values Cmd { get; set; }
        /// <summary>
        ///  The value and meaning depend on the Cmd field.
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
        /// <param name="sp">SP field</param>
        /// <param name="cbChId">cbChId field</param>
        public Header(Cmd_Values cmd, int sp, cbChId_Values cbChId)
        {
            this.Cmd = cmd;
            this.Sp = sp;
            this.CbChannelId = cbChId;
        }
    }

    public enum Version_Values : ushort
    {
        /// <summary>
        ///  Version level one is supported.
        /// </summary>
        V1 = 0x0001,

        /// <summary>
        ///  Version level two is supported.
        /// </summary>
        V2 = 0x0002,
    }

    public enum Cmd_Values : int {
        
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
        /// Not supported by the protocol. Only apply to marshalinhg
        /// runtines.
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
        ///  This seems a TDI. should be invalid?
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
        ///  This seems a TDI. should be invalid?
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

        public byte[] RawData { get; protected set; }

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
        }

        public CapsVer2ReqDvcPdu(
            ushort priorityCharge0,
            ushort priorityCharge1,
            ushort priorityCharge2,
            ushort priorityCharge3
            )
        {
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
        }

        public CapsVer3ReqDvcPdu(
            ushort priorityCharge0,
            ushort priorityCharge1,
            ushort priorityCharge2,
            ushort priorityCharge3
            )
        {
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
        }

        public CapsRespDvcPdu(ushort version)
        {
            this.Version = version;
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
            HeaderBits = new Header(Cmd, 0, cbChId_Values.Invalid);

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
            int channelIdSize = 0;
            int lengthSize = 0;
            const int headerSize = 1;

            // TODO: refine
            switch (this.HeaderBits.CbChannelId)
            {
                case cbChId_Values.OneByte:
                    channelIdSize = 1;
                    break;
                case cbChId_Values.TwoBytes:
                    channelIdSize = 2;
                    break;
                case cbChId_Values.FourBytes:
                    channelIdSize = 4;
                    break;
                case cbChId_Values.Invalid:
                default:
                    DynamicVCException.Throw("channelIdSize is invalid.");
                    break;
            }

            switch ((Len_Values)this.HeaderBits.Sp)
            {
                case Len_Values.OneByte:
                    lengthSize = 1;
                    break;
                case Len_Values.TwoBytes:
                    lengthSize = 2;
                    break;
                case Len_Values.FourBytes:
                    lengthSize = 4;
                    break;
                case Len_Values.Invalid:
                default:
                    DynamicVCException.Throw("lengthSize is invalid.");
                    break;
            }

            return (headerSize + channelIdSize + lengthSize);
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
            int channelIdSize = 0;
            const int headerSize = 1;

            // TODO: refine
            switch (this.HeaderBits.CbChannelId)
            {
                case cbChId_Values.OneByte:
                    channelIdSize = 1;
                    break;
                case cbChId_Values.TwoBytes:
                    channelIdSize = 2;
                    break;
                case cbChId_Values.FourBytes:
                    channelIdSize = 4;
                    break;
                case cbChId_Values.Invalid:
                default:
                    DynamicVCException.Throw("channelIdSize is invalid.");
                    break;
            }

            return (headerSize + channelIdSize);
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

    #endregion

    #region Closing Dynamic Virtual Channel PDUs

    public class CloseDvcPdu : DynamicVCPDU
    {
        public CloseDvcPdu()
        {
        }

        public CloseDvcPdu(uint channelId)
        {
            this.ChannelId = channelId;
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


    #endregion
}
