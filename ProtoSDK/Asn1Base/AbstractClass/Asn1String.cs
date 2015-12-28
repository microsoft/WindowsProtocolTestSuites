// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Contains some common fields and properties for String types.
    /// </summary>
    /// <remarks>
    /// All ASN.1 Strings except for BIT STRING must be drived from this class.
    /// </remarks>
    public abstract class Asn1String : Asn1Object
    {
        /// <summary>
        /// Stores the data of the object.
        /// </summary>
        private string data;

        /// <summary>
        /// Gets or sets to the data of this object.
        /// </summary>
        public virtual string Value
        {
            get
            {
                return data;
            }
            set
            {
                this.data = value;
            }
        }

        #region overrode methods from System.Object

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if obj has same data with this instance. False if not.</returns>
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Asn1String return false.
            Asn1String p = obj as Asn1String;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match.
            return this.Value == p.Value;
        }

        /// <summary>
        /// Returns the hash code of the instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        #endregion overrode methods from System.Object
    }
}
