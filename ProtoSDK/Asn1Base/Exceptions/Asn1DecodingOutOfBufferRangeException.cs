// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// The exception that is thrown when there is no enough data in a buffer during decoding process.
    /// </summary>
    public class Asn1DecodingOutOfBufferRangeException : Asn1Exception
    {
        /// <summary>
        /// Initializes a new instance of Asn1DecodingOutOfBufferRangeException exception by the default error message.
        /// </summary>
        public Asn1DecodingOutOfBufferRangeException()
        {

        }

        /// <summary>
        /// Initializes a new instance of Asn1DecodingOutOfBufferRangeException exception by the given error message.
        /// </summary>
        public Asn1DecodingOutOfBufferRangeException(string str)
            : base(str)
        {

        }
    }
}
