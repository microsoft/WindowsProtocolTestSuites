// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Spcifies the constraints of Asn1Ojbects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class Asn1Constraint : Asn1Attribute
    {
        
    }
}
