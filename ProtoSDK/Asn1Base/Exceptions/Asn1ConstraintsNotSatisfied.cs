// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// The exception that is thrown when the constraints is not satisfied for an object.
    /// </summary>
    public class Asn1ConstraintsNotSatisfied : Asn1Exception
    {
        /// <summary>
        /// Initializes a new instance of Asn1ConstraintsNotSatisfied exception by the default error message.
        /// </summary>
        public Asn1ConstraintsNotSatisfied()
        {

        }

        /// <summary>
        /// Initializes a new instance of Asn1ConstraintsNotSatisfied exception by the given error message.
        /// </summary>
        public Asn1ConstraintsNotSatisfied(string str)
            : base(str)
        {

        }
    }
}