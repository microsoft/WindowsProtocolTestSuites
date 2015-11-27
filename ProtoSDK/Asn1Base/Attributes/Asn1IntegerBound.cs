// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Spcifies the bounds of an INTEGER.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class Asn1IntegerBound : Asn1Attribute
    {
        private long max, min;

        /// <summary>
        /// Initializes a new instance of the Asn1IntegerBound class that doesn't have max and min bounds.
        /// </summary>
        public Asn1IntegerBound()
        {
            HasMax = false;
            HasMin = false;
        }

        /// <summary>
        /// Gets a bool indicates whether an max bound is defined.
        /// </summary>
        public bool HasMax { get; private set; }

        /// <summary>
        /// Gets a bool indicates whether a min bound is defined.
        /// </summary>
        public bool HasMin { get; private set; }

        /// <summary>
        /// Gets or sets the max bound of an INTEGER.
        /// </summary>
        public long Max
        {
            get
            {
                return max;
            }
            set
            {
                max = value;
                HasMax = true;
            }
        }

        /// <summary>
        /// Gets or sets the min bound of an INTEGER.
        /// </summary>
        public long Min
        {
            get
            {
                return min;
            }
            set
            {
                min = value;
                HasMin = true;
            }
        }
    }
}
