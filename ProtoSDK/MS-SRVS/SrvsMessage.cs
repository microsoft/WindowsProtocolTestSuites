// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Srvs
{
    public enum SRVS_OPNUM : ushort
    {
        NetrShareEnum = 15,
        NetrShareGetInfo = 16,
        NetrShareSetInfo = 17
    }

    /// <summary>
    /// Specifies the information level of the data. 
    /// </summary>
    public enum SHARE_ENUM_STRUCT_LEVEL : uint
    {
        /// <summary>
        /// SHARE_INFO_0_CONTAINER
        /// </summary>
        Level0 = 0,

        /// <summary>
        /// SHARE_INFO_1_CONTAINER
        /// </summary>
        Level1 = 1,

        /// <summary>
        /// SHARE_INFO_2_CONTAINER
        /// </summary>
        Level2 = 2,

        /// <summary>
        /// SHARE_INFO_501_CONTAINER
        /// </summary>
        Level501 = 501,

        /// <summary>
        /// SHARE_INFO_502_CONTAINER
        /// </summary>
        Level502 = 502,

        /// <summary>
        /// SHARE_INFO_503_CONTAINER
        /// </summary>
        Level503 = 503
    }

    /// <summary>
    /// The SHARE_INFO_0_CONTAINER structure contains a value that indicates the number of entries that 
    /// the NetrShareEnum method returns and a pointer to the buffer that contains the entries.
    /// </summary>
    public struct SHARE_INFO_0_CONTAINER
    {
        /// <summary>
        /// The number of entries returned by the method.
        /// </summary>
        public uint EntriesRead;

        /// <summary>
        ///  A pointer to the SHARE_INFO_1 entries returned by the
        ///  method.
        /// </summary>
        [Size("EntriesRead")]
        public SHARE_INFO_0[] Buffer;
    }

    /// <summary>
    /// The SHARE_INFO_1_CONTAINER structure contains a value that indicates the number of entries that the NetrShareEnum 
    /// method returns and a pointer to the buffer that contains the entries.
    /// </summary>
    public struct SHARE_INFO_1_CONTAINER
    {
        /// <summary>
        /// The number of entries returned by the method.
        /// </summary>
        public uint EntriesRead;

        /// <summary>
        /// A pointer to the SHARE_INFO_1 entries returned by the method.
        /// </summary>
        [Size("EntriesRead")]
        public SHARE_INFO_1[] Buffer;
    }

    /// <summary>
    /// The SHARE_ENUM_UNION union contains information about shares. 
    /// It is used in the definition of the SHARE_ENUM_STRUCT structure.
    /// </summary>
    [Union("System.Int32")]
    public struct SHARE_ENUM_UNION
    {

        [Indirect()]
        [Case("0")]
        public SHARE_INFO_0_CONTAINER? Level0;

        [Indirect()]
        [Case("1")]
        public SHARE_INFO_1_CONTAINER? Level1;
    }

    /// <summary>
    /// The SHARE_ENUM_STRUCT structure specifies the information level that the client requests in the NetrShareEnum method
    /// and encapsulates the SHARE_ENUM_UNION union that receives the entries enumerated by the server.
    /// </summary>
    public struct SHARE_ENUM_STRUCT
    {
        /// Specifies the information level of the data. This parameter MUST have one of the following values.
        public uint Level;

        /// Contains a share information container whose type is specified by the Level parameter as the 
        /// preceding table shows. The enumerated share entries are returned in this member.
        [Switch("Level")]
        public SHARE_ENUM_UNION ShareInfo;
    }

    /// <summary>
    /// The SHARE_INFO union contains information about a share.
    /// </summary>
    [Union("System.Int32")]
    public struct SHARE_INFO
    {
        [Indirect()]
        [Case("0")]
        public SHARE_INFO_0? ShareInfo0;

        [Indirect()]
        [Case("1")]
        public SHARE_INFO_1? ShareInfo1;

        [Indirect()]
        [Case("502")]
        public SHARE_INFO_502_I? ShareInfo502;

    }

    /// <summary>
    /// The SHARE_INFO_0 structure contains the name of the shared resource. 
    /// </summary>
    public struct SHARE_INFO_0
    {
        /// <summary>
        /// A pointer to a null-terminated Unicode UTF-16 string that specifies the name of a shared resource.
        /// The server MUST ignore this member when processing the NetrShareSetInfo method
        /// </summary>
        [String(StringEncoding.Unicode)]
        public string shi1_netname;
    }

    /// <summary>
    /// The SHARE_INFO_1 structure contains information about the shared resource, 
    /// including the name and type of the resource and a comment associated with the resource. 
    /// </summary>
    public struct SHARE_INFO_1
    {
        /// <summary>
        /// A pointer to a null-terminated Unicode UTF-16 string that specifies the name of a shared resource.
        /// The server MUST ignore this member when processing the NetrShareSetInfo method
        /// </summary>
        [String(StringEncoding.Unicode)]
        public string shi1_netname;

        /// <summary>
        /// Specifies a DWORD value that indicates the type of share.
        /// </summary>
        public uint shi1_type;

        /// <summary>
        /// A pointer to a null-terminated Unicode UTF-16 string that specifies an optional comment about the shared resource.
        /// </summary>
        [String(StringEncoding.Unicode)]
        public string shi1_remark;
    }

    /// <summary>
    /// The SHARE_INFO_502_I structure contains information about the shared resource, 
    /// including the name of the resource, type, and permissions, the number of connections, and other pertinent information.
    /// </summary>
    public struct SHARE_INFO_502_I
    {
        /// <summary>
        /// A pointer to a null-terminated Unicode UTF-16 string that specifies the name of a shared resource.
        /// The server MUST ignore this member when processing the NetrShareSetInfo method
        /// </summary>
        [String(StringEncoding.Unicode)]
        public string shi502_netname;

        /// <summary>
        /// Specifies a DWORD value that indicates the type of share.
        /// </summary>
        public uint shi502_type;

        /// <summary>
        /// A pointer to a null-terminated Unicode UTF-16 string that specifies an optional comment about the shared resource.
        /// </summary>
        [String(StringEncoding.Unicode)]
        public string shi502_remark;

        /// <summary>
        /// This field is not used. The server MUST ignore the value of this parameter on receipt.
        /// </summary>
        public uint shi502_permissions;

        /// <summary>
        /// Specifies a DWORD value that indicates the maximum number of concurrent connections that the shared resource can accommodate. 
        /// If the value that is specified by shi502_max_uses is 0xFFFFFFFF, the maximum number of connections MUST be unlimited.
        /// </summary>
        public uint shi502_max_uses;

        /// <summary>
        /// Specifies a DWORD value that indicates the number of current connections to the resource. 
        /// The server MUST ignore this member on receipt.
        /// </summary>
        public uint shi502_current_users;

        /// <summary>
        /// A pointer to a null-terminated Unicode UTF-16 string that contains the local path for the shared resource. 
        /// For disks, shi502_path is the path that is being shared. For print queues, shi502_path is the name of the print queue that is being shared.
        /// For communication devices, shi502_path is the name of the communication device that is being shared. For interprocess communications (IPC),
        /// shi502_path is the name of the interprocess communication that is being shared.
        /// The server MUST ignore this member when processing the NetrShareSetInfo method.
        /// </summary>
        [String(StringEncoding.Unicode)]
        public string shi502_path;

        /// <summary>
        /// This field is not used. The client MUST send a NULL (zero-length) string and the server MUST ignore the value of this parameter on receipt.
        /// </summary>
        [String(StringEncoding.Unicode)]
        public string shi502_passwd;

        /// <summary>
        /// The length of the security descriptor that is being passed in the shi502_security_descriptor member.
        /// </summary>
        public uint shi502_reserved;

        /// <summary>
        /// Specifies the SECURITY_DESCRIPTOR, as described in [MS-DTYP] section 2.4.6, that is associated with this share.
        /// </summary>
        [Size("shi502_reserved")]
        public byte[] shi502_security_descriptor;
    }
}
