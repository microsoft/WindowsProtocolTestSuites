// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    /// <summary>
    ///  The UPN_DNS_INFO structure contains the client's UPN
    ///  and DNS name. It is used to provide the UPN and DNS
    ///  name that corresponds to the client of the ticket.
    ///  The UPN_DNS_INFO structure is placed directly after
    ///  the Buffers array of the topmost PACTYPE structure,
    ///  at the offset specified in the Offset field
    ///  of the corresponding PAC_INFO_BUFFER structure
    ///  in the Buffers array. The UlType field of the corresponding
    ///  PAC_INFO_BUFFER is set to 0x0000000C.
    ///  </summary>
    public class UpnDnsInfo : PacInfoBuffer
    {
        /// <summary>
        /// The native UPN_DNS_INFO object.
        /// </summary>
        public UPN_DNS_INFO NativeUpnDnsInfo;

        /// <summary>
        /// TD doesn't mention this alignment,
        /// but Windows implementations align both UPN and DNS information to 8-byte boundary.
        /// </summary>
        private const int UpnDnsAlignUnit = 0x08;

        /// <summary>
        /// The UPN information.
        /// </summary>
        private string upn = string.Empty;

        ///<summary>
        /// The DNS information.
        /// </summary>
        private string dnsDomain = string.Empty;

        /// <summary>
        /// Gets or sets the UPN information.
        /// </summary>
        public string Upn
        {
            get
            {
                return upn;
            }
            set
            {
                upn = value;
                UpdateOffsetLength();
            }
        }

        /// <summary>
        /// Gets or sets the DNS domain information.
        /// </summary>
        public string DnsDomain
        {
            get
            {
                return dnsDomain;
            }
            set
            {
                dnsDomain = value;
                UpdateOffsetLength();
            }
        }


        /// <summary>
        /// Decode specified buffer from specified index, with specified count
        /// of bytes, into the instance of current class.
        /// </summary>
        /// <param name="buffer">The specified buffer.</param>
        /// <param name="index">The specified index from beginning of buffer.</param>
        /// <param name="count">The specified count of bytes to be decoded.</param>
        internal override void DecodeBuffer(byte[] buffer, int index, int count)
        {
            NativeUpnDnsInfo =
                PacUtility.MemoryToObject<UPN_DNS_INFO>(buffer, index, count);

            upn = ReadUtf16String(
                buffer,
                index + NativeUpnDnsInfo.UpnOffset,
                NativeUpnDnsInfo.UpnLength);

            dnsDomain = ReadUtf16String(
                buffer,
                index + NativeUpnDnsInfo.DnsDomainNameOffset,
                NativeUpnDnsInfo.DnsDomainNameLength);

            UpdateOffsetLength();
        }


        /// <summary>
        /// Encode the instance of current class into byte array,
        /// according to TD specification.
        /// </summary>
        /// <returns>The encoded byte array</returns>
        internal override byte[] EncodeBuffer()
        {
            byte[] header = PacUtility.ObjectToMemory(NativeUpnDnsInfo);

            int length = CalculateSize();
            byte[] result = new byte[length];

            // header
            Buffer.BlockCopy(header, 0, result, 0, header.Length);

            // upn content
            Buffer.BlockCopy(Encoding.Unicode.GetBytes(upn), 0, result,
                NativeUpnDnsInfo.UpnOffset, NativeUpnDnsInfo.UpnLength);
            // dns domain content
            Buffer.BlockCopy(Encoding.Unicode.GetBytes(dnsDomain), 0, result,
                NativeUpnDnsInfo.DnsDomainNameOffset, NativeUpnDnsInfo.DnsDomainNameLength);

            return result;
        }


        /// <summary>
        /// Calculate size of current instance's encoded buffer, in bytes.
        /// </summary>
        /// <returns>The size of current instance's encoded buffer, in bytes.</returns>
        internal override int CalculateSize()
        {
            // UpdateOffsetLength() method has ensured that DnsDomainName is placed at the end.
            return NativeUpnDnsInfo.DnsDomainNameOffset + NativeUpnDnsInfo.DnsDomainNameLength
                + (UpnDnsAlignUnit - NativeUpnDnsInfo.DnsDomainNameLength % UpnDnsAlignUnit) % UpnDnsAlignUnit;
        }


        /// <summary>
        /// Get the ulType of current instance's PAC_INFO_BUFFER.
        /// </summary>
        /// <returns>The ulType of current instance's PAC_INFO_BUFFER.</returns>
        internal override PAC_INFO_BUFFER_Type_Values GetBufferInfoType()
        {
            return PAC_INFO_BUFFER_Type_Values.UpnAndDnsInformation;
        }


        /// <summary>
        /// Update length and offset of UPN and DNS Domain.
        /// </summary>
        private void UpdateOffsetLength()
        {
            int upnOffset = Marshal.SizeOf(NativeUpnDnsInfo.GetType());
            NativeUpnDnsInfo.UpnOffset = (ushort)PacUtility.AlignTo(upnOffset, UpnDnsAlignUnit);
            NativeUpnDnsInfo.UpnLength = (ushort)(Encoding.Unicode.GetByteCount(Upn));

            int dnsOffset = NativeUpnDnsInfo.UpnOffset + NativeUpnDnsInfo.UpnLength;
            NativeUpnDnsInfo.DnsDomainNameOffset = (ushort)PacUtility.AlignTo(dnsOffset, UpnDnsAlignUnit);
            NativeUpnDnsInfo.DnsDomainNameLength = (ushort)(Encoding.Unicode.GetByteCount(DnsDomain));
        }


        /// <summary>
        /// Read an UTF16 string from specified buffer.
        /// </summary>
        /// <param name="buffer">The specified buffer.</param>
        /// <param name="index">The index from beginning of the buffer.</param>
        /// <param name="stringLength">The desired string length.</param>
        /// <returns>The read string.</returns>
        private static string ReadUtf16String(byte[] buffer, int index, int stringLength)
        {
            if (index < 0 || stringLength < 0 || index + stringLength > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(
                    "index",
                    string.Format("index < 0 || stringLength < 0 || index + stringLength > buffer.Length,\n"
                    + "index={0} stringLength={1} buffer.Length={2}",
                    index,
                    stringLength,
                    buffer.Length));
            }

            return Encoding.Unicode.GetString(buffer, index, stringLength);
        }
    }
}
