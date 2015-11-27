// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// The exception that is throw when encoding an object which doesn't have data.
    /// </summary>
    public class Asn1EmptyDataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of Asn1EmptyDataException exception by the default error message.
        /// </summary>
        public Asn1EmptyDataException()
        {

        }

        /// <summary>
        /// Initializes a new instance of Asn1EmptyDataException exception by the given error message.
        /// </summary>
        public Asn1EmptyDataException(string str)
            : base(str)
        {

        }
    }
}
