// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// NDR data representation format label, defined in [C706] section 14.1.
    /// </summary>
    public struct DataRepresentationFormatLabel
    {
        /// <summary>
        /// NDR data representation format, including integer representation, 
        /// character representation and floating-point representation.
        /// </summary>
        public RpceDataRepresentationFormat dataRepFormat;

        /// <summary>
        /// reserved.
        /// </summary>
        public ushort reserved;
    }


    /// <summary>
    /// p_syntax_id_t, defined in [C706] section 12.6.3.1.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct p_syntax_id_t
    {
        /// <summary>
        /// interface identifier UUID.
        /// </summary>
        [FieldOffset(0)]
        public Guid if_uuid;

        /// <summary>
        /// interface identifier version.
        /// </summary>
        [FieldOffset(16)]
        public uint if_version;

        ///
        /// The struct is described in [C706] 12.6.3 as following:
        ///
        /// typedef   struct   {
        ///              uuid_t   if_uuid;
        ///              u_int32   if_version;
        ///           } p_syntax_id_t;
        ///
        /// For abstract syntax, if_uuid is set to the interface UUID, and if_version is
        /// set to the interface version.
        /// For transfer syntax, these are set to the UUID and version created for the
        /// data representation.The major version is encoded in the 16 least significant
        /// bits of if_version and the minor version in the 16 most significant bits.
        ///
        /// So the memory layout is manually defined here to directly get the major and
        /// minor version.
        ///

        /// <summary>
        /// interface identifier version major.
        /// the 16 least significant bits of if_version.
        /// </summary>
        [FieldOffset(16)]
        public ushort if_vers_major;

        /// <summary>
        /// interface identifier version minor.
        /// the 16 most significant bits of if_version.
        /// </summary>
        [FieldOffset(18)]
        public ushort if_vers_minor;
    }


    /// <summary>
    /// p_cont_elem_t, defined in [C706] section 12.6.3.1.
    /// </summary>
    public struct p_cont_elem_t
    {
        /// <summary>
        /// p_cont_id
        /// </summary>
        public ushort p_cont_id;

        /// <summary>
        /// number of items
        /// </summary>
        public byte n_transfer_syn;

        /// <summary>
        /// alignment pad, m.b.z.
        /// </summary>
        public byte reserved;

        /// <summary>
        /// abstract syntax
        /// </summary>
        public p_syntax_id_t abstract_syntax;

        /// <summary>
        /// transfer syntax list
        /// </summary>
        public p_syntax_id_t[] transfer_syntaxes;
    }


    /// <summary>
    /// p_cont_list_t, defined in [C706] section 12.6.3.1.
    /// </summary>
    public struct p_cont_list_t
    {
        /// <summary>
        /// number of items
        /// </summary>
        public byte n_context_elem;

        /// <summary>
        /// alignment pad, m.b.z.
        /// </summary>
        public byte reserved;

        /// <summary>
        /// alignment pad, m.b.z.
        /// </summary>
        public ushort reserved2;

        /// <summary>
        /// p_cont_elem list
        /// </summary>
        public p_cont_elem_t[] p_cont_elem;
    }


    /// <summary>
    /// auth_verifier_co_t, defined in [C706] section 13.2.6.1 and [MS-RPCE] section 2.2.2.11.
    /// </summary>
    public struct auth_verifier_co_t
    {
        /// <summary>
        /// restore 4 byte alignment<para/>
        /// align(4)
        /// </summary>
        public byte[] auth_pad;

        /// <summary>
        /// which authent service
        /// </summary>
        public byte auth_type;

        /// <summary>
        /// which level within service
        /// </summary>
        public byte auth_level;

        /// <summary>
        /// authent pad length
        /// </summary>
        public byte auth_pad_length;

        /// <summary>
        /// reserved, m.b.z.
        /// </summary>
        public byte auth_reserved;

        /// <summary>
        /// auth context id
        /// </summary>
        public uint auth_context_id;

        /// <summary>
        /// credentials
        /// </summary>
        public byte[] auth_value;
    }


    /// <summary>
    /// version_t, defined in [C706] section 12.6.3.1 and [MS-RPCE] section 2.2.1.1.5.
    /// </summary>
    public struct version_t
    {
        /// <summary>
        /// major
        /// </summary>
        public byte major;

        /// <summary>
        /// minor
        /// </summary>
        public byte minor;
    }


    /// <summary>
    /// p_rt_versions_supported_t, defined in [C706] section 12.6.3.1 
    /// and [MS-RPCE] section 2.2.1.1.6.
    /// </summary>
    public struct p_rt_versions_supported_t
    {
        /// <summary>
        /// number of protocols
        /// </summary>
        public byte n_protocols;

        /// <summary>
        /// protocols list
        /// </summary>
        public version_t[] p_protocols;
    }


    /// <summary>
    /// p_reject_reason_t, defined in [C706] section 12.6.3.1 and [MS-RPCE] section 2.2.2.5.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum p_reject_reason_t : ushort
    {
        /// <summary>
        /// If the reason for the error does not fit any of 
        /// the other reasons specified in this section, 
        /// then this rejection code SHOULD be used.
        /// </summary>
        REASON_NOT_SPECIFIED = 0,

        /// <summary>
        /// Not used.<para/>
        /// Client implementations of these extensions SHOULD treat 
        /// this rejection code in the same manner as LOCAL_LIMIT_EXCEEDED.
        /// </summary>
        TEMPORARY_CONGESTION = 1,

        /// <summary>
        /// The server could not complete the request due to lack of resources.
        /// </summary>
        LOCAL_LIMIT_EXCEEDED = 2,

        /// <summary>
        /// Not used.
        /// </summary>
        CALLED_PADDR_UNKNOWN = 3,

        /// <summary>
        /// The server detected a protocol error while processing an rpc_bind 
        /// or rpc_alter_context PDU.
        /// </summary>
        PROTOCOL_VERSION_NOT_SUPPORTED = 4,

        /// <summary>
        /// Not used.
        /// </summary>
        DEFAULT_CONTEXT_NOT_SUPPORTED = 5,

        /// <summary>
        /// Not used.
        /// </summary>
        USER_DATA_NOT_READABLE = 6,

        /// <summary>
        /// Not used.
        /// </summary>
        NO_PSAP_AVAILABLE = 7,

        /// <summary>
        /// Authentication type requested by client is not recognized by server.
        /// </summary>
        authentication_type_not_recognized = 8,

        /// <summary>
        /// This rejection code is used for security-related errors 
        /// not covered by other rejection codes.
        /// </summary>
        invalid_checksum = 9,
    }


    /// <summary>
    /// p_cont_def_result_t, defined in [C706] section 12.6.3.1 and [MS-RPCE] section 2.2.2.4.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum p_cont_def_result_t : ushort
    {
        /// <summary>
        /// acceptance
        /// </summary>
        acceptance = 0,

        /// <summary>
        /// user rejection
        /// </summary>
        user_rejection = 1,

        /// <summary>
        /// provider rejection
        /// </summary>
        provider_rejection = 2,

        /// <summary>
        /// negotiate ack
        /// </summary>
        negotiate_ack = 3,
    }


    /// <summary>
    /// p_provider_reason_t, defined in [C706] section 12.6.3.1.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum p_provider_reason_t : ushort
    {
        /// <summary>
        /// reason not specified
        /// </summary>
        reason_not_specified = 0,

        /// <summary>
        /// abstract syntax not supported
        /// </summary>
        abstract_syntax_not_supported = 1,

        /// <summary>
        /// proposed transfer syntaxes not supported
        /// </summary>
        proposed_transfer_syntaxes_not_supported = 2,

        /// <summary>
        /// local limit exceeded
        /// </summary>
        local_limit_exceeded = 3,
    }


    /// <summary>
    /// p_result_t, defined in [C706] section 12.6.3.1.
    /// </summary>
    public struct p_result_t
    {
        /// <summary>
        /// result
        /// </summary>
        public p_cont_def_result_t result;

        /// <summary>
        /// only relevant if result != acceptance
        /// </summary>
        public p_provider_reason_t reason;

        /// <summary>
        /// transfer syntax selected 0 if result not accepted
        /// </summary>
        public p_syntax_id_t transfer_syntax;
    }


    /// <summary>
    /// p_result_list_t, defined in [C706] section 12.6.3.1.
    /// </summary>
    public struct p_result_list_t
    {
        /// <summary>
        /// count
        /// </summary>
        public byte n_results;

        /// <summary>
        /// alignment pad, m.b.z.
        /// </summary>
        public byte reserved;

        /// <summary>
        /// alignment pad, m.b.z.
        /// </summary>
        public ushort reserved2;

        /// <summary>
        /// results list
        /// </summary>
        public p_result_t[] p_results;
    }


    /// <summary>
    /// port_any_t, defined in [C706] section 12.6.3.1.
    /// </summary>
    public struct port_any_t
    {
        /// <summary>
        /// length
        /// </summary>
        public ushort length;

        /// <summary>
        /// port string spec
        /// </summary>
        public byte[] port_spec;
    }

    /// <summary>
    /// The format for RPCE response
    /// </summary>
    public struct RpceResponseFormat
    {
        /// <summary>
        /// Parameter list
        /// </summary>
        public Int3264[] paramList;

        /// <summary>
        /// Operator number
        /// </summary>
        public ushort opnum;
    }

    /// <summary>
    /// The exception will be thrown when RPCE connect is disconnected.
    /// </summary>
    public class RpceDisconnectedException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public RpceDisconnectedException(string message)
            : base(message)
        {
        }
    }
}
