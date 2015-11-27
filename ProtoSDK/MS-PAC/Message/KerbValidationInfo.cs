// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    /// <summary>
    ///  The KERB_VALIDATION_INFO structure defines the user's
    ///  logon and authorization information provided by the
    ///  DC. A ptr to the KERB_VALIDATION_INFO structure
    ///  is serialized into an array of bytes and then placed
    ///  after the   Buffers array of the topmost PACTYPE structure,
    ///  at the offset specified in the Offset field of the
    ///  corresponding PAC_INFO_BUFFER structure in the Buffers
    ///  array. The ulType field of the corresponding PAC_INFO_BUFFER
    ///  structure is set to 0x00000001.The KERB_VALIDATION_INFO
    ///  structure is a subset of the NETLOGON_VALIDATION_SAM_INFO4
    ///  structure ([MS-NRPC] section 2.2.1.4.13). It is a
    ///  subset due to historical reasons and to the use of
    ///  the common Active Directory to generate this information.
    ///  NTLM uses the NETLOGON_VALIDATION_SAM_INFO4 structure
    ///  in the context of the server to domain controller exchange,
    ///  as described in  [MS-APDS] section 3.1. Consequently,
    ///  the  KERB_VALIDATION_INFO structure includes NTLM-specific
    ///  fields. Fields that are common to the  KERB_VALIDATION_INFO
    ///  and the NETLOGON_VALIDATION_SAM_INFO4 structures, and
    ///  which are specific to the NTLM authentication operation,
    ///  are not used with Kerberos authentication.	The KERB_VALIDATION_INFO
    ///  structure is marshaled by RPC [MS-RPCE].
    /// </summary>
    public class KerbValidationInfo : PacInfoBuffer
    {
        /// <summary>
        /// The native KERB_VALIDATION_INFO object.
        /// </summary>
        public KERB_VALIDATION_INFO NativeKerbValidationInfo;


        /// <summary>
        /// Decode specified buffer from specified index, with specified count
        /// of bytes, into the instance of current class.
        /// </summary>
        /// <param name="buffer">The specified buffer.</param>
        /// <param name="index">The specified index from beginning of buffer.</param>
        /// <param name="count">The specified count of bytes to be decoded.</param>
        internal override void DecodeBuffer(byte[] buffer, int index, int count)
        {
            NativeKerbValidationInfo = PacUtility.NdrUnmarshal<KERB_VALIDATION_INFO>(
                buffer,
                index,
                count,
                FormatString.OffsetKerb);
        }


        /// <summary>
        /// Encode the instance of current class into byte array,
        /// according to TD specification.
        /// </summary>
        /// <returns>The encoded byte array</returns>
        internal override byte[] EncodeBuffer()
        {
            using (SafeIntPtr ptr = TypeMarshal.ToIntPtr(NativeKerbValidationInfo))
            {
                return PacUtility.NdrMarshal(ptr, FormatString.OffsetKerb);
            }
        }


        /// <summary>
        /// Calculate size of current instance's encoded buffer, in bytes.
        /// </summary>
        /// <returns>The size of current instance's encoded buffer, in bytes.</returns>
        internal override int CalculateSize()
        {
            return EncodeBuffer().Length;
        }


        /// <summary>
        /// Get the ulType of current instance's PAC_INFO_BUFFER.
        /// </summary>
        /// <returns>The ulType of current instance's PAC_INFO_BUFFER.</returns>
        internal override PAC_INFO_BUFFER_Type_Values GetBufferInfoType()
        {
            return PAC_INFO_BUFFER_Type_Values.LogonInformation;
        }
    }
}
