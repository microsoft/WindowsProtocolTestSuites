// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.PrintService.Rprn
{
    /// <summary>
    /// The Product Suite Flags are implementation-specific values 
    /// for the product suites that are available on the operating system (OS).
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum RprnProductSuiteFlags : ushort
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Microsoft Small Business Server was once installed on the system, 
        /// but it might have been upgraded to another version of Windows.
        /// </summary>
        VER_SUITE_SMALLBUSINESS = 0x0001,

        /// <summary>
        /// Windows NT Server 4.0 Enterprise Edition, Windows 2000 Advanced Server, 
        /// Windows Server 2003 Enterprise Edition, or Windows Server 2008 Enterprise 
        /// is installed.
        /// </summary>
        VER_SUITE_ENTERPRISE = 0x0002,

        /// <summary>
        /// Microsoft BackOffice components are installed.
        /// </summary>
        VER_SUITE_BACKOFFICE = 0x0004,

        /// <summary>
        /// Terminal Services is installed. If VER_SUITE_TERMINAL is set but 
        /// VER_SUITE_SINGLEUSERTS is not set, the system is running in 
        /// application server mode.
        /// </summary>
        VER_SUITE_TERMINAL = 0x0010,

        /// <summary>
        /// Microsoft Small Business Server is installed with the restrictive 
        /// client license in force.
        /// </summary>
        VER_SUITE_SMALLBUSINESS_RESTRICTED = 0x0020,

        /// <summary>
        /// Windows XP Embedded is installed.
        /// </summary>
        VER_SUITE_EMBEDDEDNT = 0x0040,

        /// <summary>
        /// Windows 2000 Datacenter Server, Windows Server 2003 Datacenter Edition, 
        /// or Windows Server 2008 Datacenter is installed.
        /// </summary>
        VER_SUITE_DATACENTER = 0x0080,

        /// <summary>
        /// Remote Desktop is supported, but only one interactive session. 
        /// This value is set unless the system is running in application server mode.
        /// </summary>
        VER_SUITE_SINGLEUSERTS = 0x0100,

        /// <summary>
        /// Windows XP Home Edition, Windows Vista Home Basic, 
        /// or Windows Vista Home Premium is installed.
        /// </summary>
        VER_SUITE_PERSONAL = 0x0200,

        /// <summary>
        /// Windows Server 2003 Web Edition is installed.
        /// </summary>
        VER_SUITE_BLADE = 0x0400,

        /// <summary>
        /// Windows Storage Server 2003 R2 or Windows Storage Server 2003 is installed.
        /// </summary>
        VER_SUITE_STORAGE_SERVER = 0x2000,

        /// <summary>
        /// Windows Server 2003 Compute Cluster Edition is installed.
        /// </summary>
        VER_SUITE_COMPUTE_SERVER = 0x4000,

        /// <summary>
        /// Windows Home Server is installed.
        /// </summary>
        VER_SUITE_WH_SERVER = 0x8000,

    }
}
