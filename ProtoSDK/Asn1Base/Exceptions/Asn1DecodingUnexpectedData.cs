// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// The exception that is thrown when an unexpected data is read from the buffer during decoding process.
    /// </summary>
    public class Asn1DecodingUnexpectedData : Asn1Exception
    {
        /// <summary>
        /// Initializes a new instance of Asn1DecodingUnexpectedData exception by the default error message.
        /// </summary>
        public Asn1DecodingUnexpectedData()
        {

        }

        /// <summary>
        /// Initializes a new instance of Asn1DecodingUnexpectedData exception by the given error message.
        /// </summary>
        public Asn1DecodingUnexpectedData(string str)
            : base(str)
        {

        }
    }
}
