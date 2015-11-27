// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// The exception that is thrown when encoding an undefined enumerated object.
    /// </summary>
    public class Asn1UndefinedEnumeratedData : Asn1Exception
    {
        /// <summary>
        /// Initializes a new instance of Asn1UndefinedEnumeratedData exception by the default error message.
        /// </summary>
        public Asn1UndefinedEnumeratedData()
        {

        }

        /// <summary>
        /// Initializes a new instance of Asn1UndefinedEnumeratedData exception by the given error message.
        /// </summary>
        public Asn1UndefinedEnumeratedData(string str)
            : base(str)
        {

        }
    }
}
