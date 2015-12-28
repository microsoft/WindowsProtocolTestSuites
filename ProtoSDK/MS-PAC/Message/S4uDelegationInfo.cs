// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    /// <summary>
    ///  The S4U_DELEGATION_INFO structure  lists the services
    ///  that have been delegated through this Kerberos client
    ///  and subsequent services or servers. The list is used
    ///  only in a  Service for User to Proxy (S4U2proxy) [MS-SFU]
    ///  request. This feature could be used multiple times
    ///  in succession from service to service, which is useful
    ///  for auditing purposes. The S4U_DELEGATION_INFO
    ///  structure is marshaled by RPC [MS-RPCE]. 
    /// </summary>
    public class S4uDelegationInfo : PacInfoBuffer
    {
        /// <summary>
        /// The native _S4U_DELEGATION_INFO object.
        /// </summary>
        public _S4U_DELEGATION_INFO NativeS4uDelegationInfo;


        /// <summary>
        /// Decode specified buffer from specified index, with specified count
        /// of bytes, into the instance of current class.
        /// </summary>
        /// <param name="buffer">The specified buffer.</param>
        /// <param name="index">The specified index from beginning of buffer.</param>
        /// <param name="count">The specified count of bytes to be decoded.</param>
        internal override void DecodeBuffer(byte[] buffer, int index, int count)
        {
            NativeS4uDelegationInfo = PacUtility.NdrUnmarshal<_S4U_DELEGATION_INFO>(
                buffer,
                index,
                count,
                FormatString.OffsetS4u);
        }


        /// <summary>
        /// Encode the instance of current class into byte array,
        /// according to TD specification.
        /// </summary>
        /// <returns>The encoded byte array</returns>
        internal override byte[] EncodeBuffer()
        {
            using (SafeIntPtr ptr = TypeMarshal.ToIntPtr(NativeS4uDelegationInfo))
            {
                return PacUtility.NdrMarshal(ptr, FormatString.OffsetS4u);
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
            return PAC_INFO_BUFFER_Type_Values.ConstrainedDelegationInformation;
        }
    }
}
