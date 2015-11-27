// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RFC_*** flags.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum RpceCoPfcFlags : byte
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// First fragment
        /// </summary>
        PFC_FIRST_FRAG = 0x01,

        /// <summary>
        /// Last fragment
        /// </summary>
        PFC_LAST_FRAG = 0x02,

        /// <summary>
        /// Cancel was pending at sender
        /// For PDU types bind, bind_ack, alter_context, and alter_context_resp, 
        /// this flag MUST be interpreted as PFC_SUPPORT_HEADER_SIGN. 
        /// For the remaining PDU types, this flag MUST be interpreted as PFC_PENDING_CANCEL.
        /// </summary>
        PFC_PENDING_CANCEL = 0x04,

        /// <summary>
        /// supports header sign<para/>
        /// For PDU types bind, bind_ack, alter_context, and alter_context_resp, 
        /// this flag MUST be interpreted as PFC_SUPPORT_HEADER_SIGN. 
        /// For the remaining PDU types, this flag MUST be interpreted as PFC_PENDING_CANCEL.
        /// </summary>
        PFC_SUPPORT_HEADER_SIGN = 0x04,

        /// <summary>
        /// Reserved.
        /// </summary>
        PFC_RESERVED_1 = 0x08,

        /// <summary>
        /// supports concurrent multiplexing
        /// of a single connection.
        /// </summary>
        PFC_CONC_MPX = 0x10,

        /// <summary>
        /// only meaningful on 'fault' packet;
        /// if true, guaranteed call did not
        /// execute.
        /// </summary>
        PFC_DID_NOT_EXECUTE = 0x20,

        /// <summary>
        /// 'maybe' call semantics requested<para/>
        /// Windows ignores the PFC_MAYBE flag when it is present in a PDU.
        /// </summary>
        PFC_MAYBE = 0x40,

        /// <summary>
        /// if true, a non-nil object UUID
        /// was specified in the handle, and
        /// is present in the optional object
        /// field. If false, the object field
        /// is omitted.
        /// </summary>
        PFC_OBJECT_UUID = 0x80,
    }
}
