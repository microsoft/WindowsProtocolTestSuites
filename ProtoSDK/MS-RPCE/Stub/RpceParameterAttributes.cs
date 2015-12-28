// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// PARAM_ATTRIBUTES of a RPC procedure parameter.<para/>
    /// http://msdn.microsoft.com/en-us/library/aa374362(VS.85).aspx
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    internal enum RpceParameterAttributes : ushort
    {
        /// <summary>
        /// The MustSize bit is set only if the parameter must be sized.
        /// </summary>
        MustSize = 0x0001,

        /// <summary>
        /// The MustFree bit is set if the server must call the parameter's NdrFree* routine.
        /// </summary>
        MustFree = 0x0002,

        /// <summary>
        /// The parameter is a pipe handle.
        /// </summary>
        IsPipe = 0x0004,

        /// <summary>
        /// The parameter is an input.
        /// </summary>
        IsIn = 0x0008,

        /// <summary>
        /// The parameter is an output.
        /// </summary>
        IsOut = 0x0010,

        /// <summary>
        /// The parameter is to be returned.
        /// </summary>
        IsReturn = 0x0020,

        /// <summary>
        /// The IsBasetype bit is set for simple types that are being marshaled by 
        /// the main -Oif interpreter loop. In particular, a simple type with a 
        /// range attribute on it is not flagged as a base type in order to force the 
        /// range routine marshaling through dispatching using an FC_RANGE token.
        /// </summary>
        IsBaseType = 0x0040,

        /// <summary>
        /// The IsByValue bit is set for compound types being sent by value, but is 
        /// not set for simple types, regardless of whether the argument is a pointer. 
        /// The compound types for which it is set are structures, unions, transmit_as, 
        /// represent_as, wire_marshal and SAFEARRAY. In general, the bit was introduced 
        /// for the benefit of the main interpreter loop in the -Oicf interpreter, 
        /// to ensure the nonsimple arguments (referred to as compound type arguments) 
        /// are properly dereferenced. This bit was never used in previous versions of 
        /// the interpreter.
        /// </summary>
        IsByValue = 0x0080,

        /// <summary>
        /// The IsSimpleRef bit is set for a parameter that is a reference pointer to 
        /// anything other than another pointer, and which has no allocate attributes. 
        /// For such a type, the parameter description's type_offset field, except 
        /// for a reference pointer to a base type, provides the offset to the referent's 
        /// type; the reference pointer is simply skipped.
        /// </summary>
        IsSimpleRef = 0x0100,

        /// <summary>
        /// The IsDontCallFreeInst bit is set for certain represent_as parameters whose 
        /// free instance routines should not be called.
        /// </summary>
        IsDontCallFreeInst = 0x0200,

        /// <summary>
        /// Save for async finish.
        /// </summary>
        SaveForAsyncFinish = 0x0400,

        /// <summary>
        /// The parameter is partial ignore.
        /// </summary>
        IsPartialIgnore = 0x0800,

        /// <summary>
        /// The parameter is force allocate.
        /// </summary>
        IsForceAllocate = 0x1000,

        /// <summary>
        /// The ServerAllocSize bits are nonzero if the parameter is [out], [in], 
        /// or [in,out] pointer to pointer, or pointer to enum16, and will be 
        /// initialized on the server interpreter's stack, rather than using a 
        /// call to I_RpcAllocate. If nonzero, this value is multiplied by 8 to 
        /// get the number of bytes for the parameter. Note that doing so means 
        /// at least 8 bytes are always allocated for a pointer.
        /// </summary>
        ServerAllocSize = 0xe000,
    }
}
