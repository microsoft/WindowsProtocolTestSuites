// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.PrintService.Rprn
{
    /// <summary>
    ///  The OS_TYPE enumeration specifies information about
    ///  the operating system (OS) type for use with Server
    ///  Handle Key Values.
    /// </summary>
    //  <remarks>
    //   MS-RPRN\682158a5-b093-400b-8dd9-b31f3fdd04d5.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum OS_TYPE : byte
    {

        /// <summary>
        ///  The OS is a windows_nt workstation. This value indicates
        ///  windows_2000_professional, windows_xp, or windows_vistawindows_vista,
        ///  or windows_7.
        /// </summary>
        VER_NT_WORKSTATION = 1,

        /// <summary>
        ///  The OS is a windows_ntdomain controller. This value indicates
        ///  windows_2000_server, windows_server_2003, or windows_server_2008windows_server_2008,
        ///  or windows_server_7.
        /// </summary>
        VER_NT_DOMAIN_CONTROLLER = 2,

        /// <summary>
        ///  The OS is a windows_nt server. This value indicates windows_2000_server,
        ///  windows_server_2003, or windows_server_2008windows_server_2008,
        ///  or windows_server_7. A server that is also a domain
        ///  controller is reported as VER_NT_DOMAIN_CONTROLLER,
        ///  not VER_NT_SERVER.
        /// </summary>
        VER_NT_SERVER = 3,
    }

    /// <summary>
    ///  The OSVERSIONINFO structure specifies operating system
    ///  (OS) version information for use with Server Handle
    ///  Key Values.
    /// </summary>
    //  <remarks>
    //   MS-RPRN\5942648e-b54f-4e22-a0e2-2d000e084b23.xml
    //  </remarks>
    public partial struct OSVERSIONINFO
    {

        /// <summary>
        ///  The size of the OSVERSIONINFO structure in bytes.
        /// </summary>
        public uint dwOSVersionInfoSize;

        /// <summary>
        ///  The major OS version. See dwMajorVersion in SPLCLIENT_INFO
        ///  Members for details.
        /// </summary>
        public uint dwMajorVersion;

        /// <summary>
        ///  The minor OS version. See dwMinorVersion in SPLCLIENT_INFO
        ///  Members (section) for details.
        /// </summary>
        public uint dwMinorVersion;

        /// <summary>
        ///  The build number of the OS.
        /// </summary>
        public uint dwBuildNumber;

        /// <summary>
        ///  The OS platform. See wProcessorArchitecture in SPLCLIENT_INFO
        ///  Members (section) for details.
        /// </summary>
        public uint dwPlatformId;

        /// <summary>
        ///  A maintenance string for Microsoft Product Support Services
        ///  (PSS) use.
        /// </summary>
        [StaticSize(128, StaticSizeMode.Elements)]
        public ushort[] szCSDVersion;
    }

    /// <summary>
    ///  The OSVERSIONINFOEX structure specifies extended operating
    ///  system (OS) version information for use with Server
    ///  Handle Key Values.
    /// </summary>
    //  <remarks>
    //   MS-RPRN\cd8e00e8-4c44-4e4f-98d3-0db33d75d06d.xml
    //  </remarks>
    public partial struct OSVERSIONINFOEX
    {

        /// <summary>
        ///  An OSVERSIONINFO structure, which specifies basic OS
        ///  version information.
        /// </summary>
        public OSVERSIONINFO OSVersionInfo;

        /// <summary>
        ///  The major version number of the latest Service Pack
        ///  installed on the system. For example, for Service Pack
        ///  3, the major version number is 3. If no Service Pack
        ///  has been installed, the value is zero.
        /// </summary>
        public ushort wServicePackMajor;

        /// <summary>
        ///  The minor version number of the latest Service Pack
        ///  installed on the system. For example, for Service Pack
        ///  3, the minor version number is 0.
        /// </summary>
        public ushort wServicePackMinor;

        /// <summary>
        ///  A value that identifies the product suites available
        ///  on the system, consisting of Product Suite Flags.
        /// </summary>
        public ushort wSuiteMask;

        /// <summary>
        ///  Additional information about the OS, which MUST be an
        ///  OS_TYPE enumeration value.
        /// </summary>
        public byte wProductType;

        /// <summary>
        ///  A field that SHOULD be initialized to zero when sent
        ///  and MUST be ignored on receipt.
        /// </summary>
        public byte wReserved;
    }
}
