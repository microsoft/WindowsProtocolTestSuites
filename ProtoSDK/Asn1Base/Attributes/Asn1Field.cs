// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Indicates that a field/property corresponds to one of the fields in ASN.1 definition for SEQUENCE and SET.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class Asn1Field : Asn1Attribute
    {
        private int index;
        private bool optional;

        /// <summary>
        /// Initializes a new instance of the Asn1Field class with a given index.
        /// </summary>
        /// <param name="Index"></param>
        public Asn1Field(int Index)
        {
            this.index = Index;
            optional = false;
        }

        /// <summary>
        /// Gets the index of the field.
        /// </summary>
        public int Index
        {
            get
            {
                return index;
            }
        }

        /// <summary>
        /// Specifies whether the field is optional.
        /// </summary>
        public bool Optional
        {
            get
            {
                return optional;
            }
            set
            {
                this.optional = value;
            }
        }
    }
}
