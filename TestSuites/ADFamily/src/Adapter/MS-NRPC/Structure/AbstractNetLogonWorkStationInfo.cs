// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;
    using Microsoft.Protocols.TestTools.StackSdk.PrintService.Rprn;

    /// <summary>
    /// Abstraction of  struct NETLOGON_WORKSTATION_INFO.
    /// </summary>
    public struct AbstractNetLogonWorkStationInfo
    {
        /// <summary>
        /// Whether the value of field Dummy1 is NULL.
        /// </summary>
        public bool IsDummy1Null;

        /// <summary>
        /// Whether the value of field Dummy2 is NULL.
        /// </summary>
        public bool IsDummy2Null;

        /// <summary>
        /// Whether the value of field Dummy3 is NULL.
        /// </summary>
        public bool IsDummy3Null;

        /// <summary>
        /// Whether the value of field Dummy4 is NULL.
        /// </summary>
        public bool IsDummy4Null;

        /// <summary>
        /// Whether the value of field OsVersion is NULL.
        /// </summary>
        public bool IsOsVersionNull;

        /// <summary>
        /// Whether the value of field OsName is NULL.
        /// </summary>
        public bool IsOsNameNull;

        /// <summary>
        /// Whether the DummyString3 contain 0 for the Length field, 0 for the MaximumLength field, and NULL 
        /// for the Buffer field or not.
        /// </summary>
        public bool IsDummyString3Null;

        /// <summary>
        /// Whether the DummyString4 contain 0 for the Length field, 0 for the MaximumLength field, and NULL 
        /// for the Buffer field or not.
        /// </summary>
        public bool IsDummyString4Null;

        /// <summary>
        /// A set of bit flags specifying workstation behavior.
        /// </summary>
        public uint WorkStationFlags;

        /// <summary>
        /// Whether the DummyLong2 is zero.
        /// </summary>
        public bool IsDummyLong2Zero;

        /// <summary>
        /// Whether the DummyLong3 is zero.
        /// </summary>
        public bool IsDummyLong3Zero;

        /// <summary>
        /// Whether the DummyLong4 IS zero.
        /// </summary>
        public bool IsDummyLong4Zero;

        /// <summary>
        /// Indicate the Client OS Type.
        /// </summary>
        public OS_TYPE ClientOSType;
    }
}