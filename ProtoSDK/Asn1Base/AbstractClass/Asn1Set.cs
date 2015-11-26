// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Corresponds to the SET type in ASN.1 definition.
    /// </summary>
    /// <remarks>
    /// All the user defined SET type should be derived from this class.
    /// </remarks>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.Set, EncodingWay = EncodingWay.Constructed)]
    public abstract class Asn1Set : Asn1HeterogeneousComposition
    {

    }
}