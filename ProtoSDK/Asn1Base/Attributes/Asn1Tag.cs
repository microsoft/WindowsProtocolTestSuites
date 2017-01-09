// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents a tag in ASN.1 definition. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class
        , AllowMultiple = true, Inherited = true)]
    public sealed class Asn1Tag : Asn1Attribute
    {
        private Asn1TagType type;
        private long val;
        private EncodingWay? encodingWay;

        public Asn1Tag()
        {
            this.type = 0;
            this.val = 0;
            this.encodingWay = 0;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1Tag class with a given tag.
        /// </summary>
        /// <param name="TagValue">The value of the given tag.</param>
        /// <param name="TagType">The type of the given tag.</param>
        public Asn1Tag(Asn1TagType TagType, long TagValue)
        {
            this.type = TagType;
            this.val = TagValue;
            this.encodingWay = null;
        }

        /// <summary>
        /// Gets the type of the tag.
        /// </summary>
        public Asn1TagType TagType
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        /// Gets the value of the tag.
        /// </summary>
        public long TagValue
        {
            get
            {
                return this.val;
            }
        }

        /// <summary>
        /// Gets the encoding way (construted or primitive) of the tag.
        /// </summary>
        public EncodingWay EncodingWay
        {
            get
            {
                if (encodingWay != null)
                {
                    return (EncodingWay)encodingWay;
                }
                else
                {
                    switch (this.type)
                    {
                        case Asn1TagType.Application:
                            {
                                return EncodingWay.Constructed;
                            }
                        case Asn1TagType.Context:
                            {
                                return EncodingWay.Constructed;
                            }
                        case Asn1TagType.Universal:
                            {
                                return EncodingWay.Primitive;
                            }
                        case Asn1TagType.Private:
                            {
                                throw new NotImplementedException();
                            }
                        default:
                            {
                                throw new NotImplementedException();
                            }
                    }

                }
            }
            set
            {
                encodingWay = value;
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

            // If parameter cannot be cast to Asn1Tag return false.
            Asn1Tag p = obj as Asn1Tag;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match.
            return this.TagType == p.TagType &&
                this.TagValue == p.TagValue && this.EncodingWay == p.EncodingWay;
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
