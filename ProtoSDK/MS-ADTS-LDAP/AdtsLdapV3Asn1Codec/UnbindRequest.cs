﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    UnbindRequest ::= [APPLICATION 2] NULL
    */
    public class UnbindRequest : Asn1Object
    {
        /// <summary>
        /// Encodes the object by BER.
        /// </summary>
        /// <param name="buffer">A buffer that stores the BER encoding result.</param>
        /// <param name="explicitTag">Indicates whether the tags should be encoded explicitly. In our Test Suites, it will always be true.</param>
        /// <returns>The length of the encoding result of this object.</returns>
        /// <exception cref="Asn1ConstraintsNotSatisfied">
        /// Thrown when the constraints are not satisfied before encoding.
        /// </exception>
        /// <remarks>Override this method in a user-defined class only if the procedure is not applicable in some special scenarios.</remarks>
        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            return 0;
        }

        /// <summary>
        /// Decodes the object by BER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a BER encoding result.</param>
        /// <returns>The number of the bytes consumed in the buffer to decode this object.</returns>
        /// <exception cref="Asn1ConstraintsNotSatisfied">
        /// Thrown when the constraints are not satisfied after decoding.
        /// </exception>
        /// <exception cref="Asn1DecodingUnexpectedData">
        /// Thrown when the data in the buffer can not be properly decoded.
        /// </exception>
        /// <remarks>Override this method in a user-defined class only if the procedure is not applicable in some special scenarios.</remarks>
        public override int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag)
        {
            return 0;
        }
    }
}
