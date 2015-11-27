// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// interpreter_opt_flags in header of a RPC procedure.<para/>
    /// http://msdn.microsoft.com/en-us/library/aa378707(VS.85).aspx
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum RpceInterpreterOptFlags2 : byte
    {
        /// <summary>
        /// indicates whether new correlation descriptors are used in the format strings.
        /// </summary>
        HasNewCorrDesc = 0x01,

        /// <summary>
        /// set when the routine needs the correlation check on client.
        /// </summary>
        ClientCorrCheck = 0x02,

        /// <summary>
        /// set when the routine needs the correlation check on server.
        /// </summary>
        ServerCorrCheck = 0x04,

        /// <summary>
        /// indicate that the routine uses the notify feature as defined by the [notify] attribute.
        /// </summary>
        HasNotify = 0x08,

        /// <summary>
        /// indicate that the routine uses the notify feature as defined by the [notify_flag] attribute.
        /// </summary>
        HasNotify2 = 0x10,

        /// <summary>
        /// return value is complex.
        /// </summary>
        HasComplexReturn = 0x20,

        /// <summary>
        /// there's conformance range.
        /// </summary>
        HasConformanceRange = 0x40,

        /// <summary>
        /// there's big by value parameter.
        /// </summary>
        HasBigByValueParam = 0x80,
    }
}
