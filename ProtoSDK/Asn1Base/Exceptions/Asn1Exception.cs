// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents errors that occur with this runtime.
    /// </summary>
    /// <remarks>
    /// All the exceptions thrown by this runtime should be an Asn1Exception.
    /// </remarks>
    public class Asn1Exception : Exception
    {
        /// <summary>
        /// Initializes a new instance of Asn1Exception exception by the default error message.
        /// </summary>
        public Asn1Exception()
        {

        }

        /// <summary>
        /// Initializes a new instance of Asn1Exception exception by the given error message.
        /// </summary>
        public Asn1Exception(string str)
            : base(str)
        {

        }
    }
}
