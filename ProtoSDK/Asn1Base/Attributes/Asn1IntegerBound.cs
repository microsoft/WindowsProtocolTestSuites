// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Spcifies the bounds of an INTEGER.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class Asn1IntegerBound : Asn1Constraint
    {
        private long max, min;
        private bool hasMax, hasMin;

        /// <summary>
        /// Initializes a new instance of the Asn1IntegerBound class that doesn't have max and min bounds.
        /// </summary>
        public Asn1IntegerBound()
        {
            hasMax = false;
            hasMin = false;
        }

        /// <summary>
        /// Gets a bool indicates whether an max bound is defined.
        /// </summary>
        public bool HasMax { get { return hasMax; } }

        /// <summary>
        /// Gets a bool indicates whether a min bound is defined.
        /// </summary>
        public bool HasMin { get { return hasMin; } }

        /// <summary>
        /// Gets or sets the max bound of an INTEGER.
        /// </summary>
        public long Max
        {
            get
            {
                if (!HasMax)
                {
                    throw new Asn1InvalidOperation("Max Bound is not set.");
                }
                return max;
            }
            set
            {
                max = value;
                hasMax = true;
            }
        }

        /// <summary>
        /// Gets or sets the min bound of an INTEGER.
        /// </summary>
        public long Min
        {
            get
            {
                if (!HasMin)
                {
                    throw new Asn1InvalidOperation("Min Bound is not set.");
                }
                return min;
            }
            set
            {
                min = value;
                hasMin = true;
            }
        }
    }
}
