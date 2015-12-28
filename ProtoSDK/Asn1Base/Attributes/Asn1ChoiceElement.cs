// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Indicates that a field/property is one of the choices in a class derived from Asn1Choice.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
        AllowMultiple = false, Inherited = true)]
    public sealed class Asn1ChoiceElement : Asn1Attribute
    {
        private long index;

        /// <summary>
        /// Gets the index of the choice element.
        /// </summary>
        public long Index
        {
            get
            {
                return index;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Asn1ChoiceElement class with a given index.
        /// </summary>
        /// <param name="index"></param>
        public Asn1ChoiceElement(long index)
        {
            this.index = index;
        }
    }
}
