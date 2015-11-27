// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// The exception that is thrown when the user defined type is inconsistent.
    /// </summary>
    /// <remarks>
    /// For example, this exception will be throw if in a SEQUENCE structure,
    /// OptionalFlags and ContexTags have different lengths
    /// </remarks>
    public class Asn1UserDefinedTypeInconsistent : Asn1Exception
    {
        /// <summary>
        /// Initializes a new instance of Asn1UserDefinedTypeInconsistent exception by the default error message.
        /// </summary>
        public Asn1UserDefinedTypeInconsistent()
        {

        }

        /// <summary>
        /// Initializes a new instance of Asn1UserDefinedTypeInconsistent exception by the given error message.
        /// </summary>
        public Asn1UserDefinedTypeInconsistent(string str)
            : base(str)
        {

        }
    }
}
