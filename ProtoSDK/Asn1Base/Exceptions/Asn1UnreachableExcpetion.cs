// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// The exception that is thrown when the program reaches an unreachable state.
    /// </summary>
    /// <remarks>
    /// If the exception is thrown, there must be some design error.
    /// </remarks>
    public class Asn1UnreachableExcpetion : Asn1Exception
    {
        /// <summary>
        /// Initializes a new instance of Asn1UnreachableExcpetion exception by the default error message.
        /// </summary>
        public Asn1UnreachableExcpetion()
        {

        }

        /// <summary>
        /// Initializes a new instance of Asn1UnreachableExcpetion exception by the given error message.
        /// </summary>
        public Asn1UnreachableExcpetion(string str)
            : base(str)
        {

        }
    }
}
