// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// Data representation format used in RPCE.
    /// </summary>
    // RpceDataRepresentationFormat is not a flag enum, supress fxcop CA1027.
    [SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum RpceDataRepresentationFormat : ushort
    {
        /// <summary>
        /// IEEE, Big-endian and ASCII.
        /// </summary>
        IEEE_BigEndian_ASCII = 0x0000,

        /// <summary>
        /// IEEE, Little-endian and ASCII.
        /// </summary>
        IEEE_LittleEndian_ASCII = 0x0010,

        /// <summary>
        /// IEEE, Big-endian and EBCDIC.
        /// </summary>
        IEEE_BigEndian_EBCDIC = 0x0001,

        /// <summary>
        /// IEEE, Little-endian and EBCDID.
        /// </summary>
        IEEE_LittleEndian_EBCDIC = 0x0011,

        /// <summary>
        /// VAX, Big-endian and ASCII.
        /// </summary>
        VAX_BigEndian_ASCII = 0x0100,

        /// <summary>
        /// VAX, Little-endian and ASCII.
        /// </summary>
        VAX_LittleEndian_ASCII = 0x0110,

        /// <summary>
        /// VAX, Big-endian and EBCDIC.
        /// </summary>
        VAX_BigEndian_EBCDIC = 0x0101,

        /// <summary>
        /// VAX, Little-endian and EBCDIC.
        /// </summary>
        VAX_LittleEndian_EBCDIC = 0x0111,

        /// <summary>
        /// Cray, Big-endian and ASCII.
        /// </summary>
        Cray_BigEndian_ASCII = 0x0200,

        /// <summary>
        /// Cray, Little-endian and ASCII.
        /// </summary>
        Cray_LittleEndian_ASCII = 0x0210,

        /// <summary>
        /// Cray, Big-endian and EBCDIC.
        /// </summary>
        Cray_BigEndian_EBCDIC = 0x0201,

        /// <summary>
        /// Cray, Little-endian and EBCDIC.
        /// </summary>
        Cray_LittleEndian_EBCDIC = 0x0211,

        /// <summary>
        /// IBM, Big-endian and ASCII.
        /// </summary>
        IBM_BigEndian_ASCII = 0x0300,

        /// <summary>
        /// IBM, Little-endian and ASCII.
        /// </summary>
        IBM_LittleEndian_ASCII = 0x0310,

        /// <summary>
        /// IBM, Big-endian and EBCDIC.
        /// </summary>
        IBM_BigEndian_EBCDIC = 0x0301,

        /// <summary>
        /// IBM, Little-endian and EBCDIC.
        /// </summary>
        IBM_LittleEndian_EBCDIC = 0x0311,
    }
}
