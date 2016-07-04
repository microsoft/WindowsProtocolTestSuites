// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Spcifies the bounds for size.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class Asn1SizeConstraint : Asn1Constraint
    {
        private long minSize, maxSize;

        /// <summary>
        /// Gets a bool indicates whether a minimal size constraint is defined.
        /// </summary>
        public bool HasMinSize
        {
            get; private set;
        }
        
        /// <summary>
        /// Gets a bool indicates whether a maximal size constraint is defined.
        /// </summary>
        public bool HasMaxSize
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the minimal size constraint.
        /// </summary>
        public long MinSize
        {
            get
            {
                if (!HasMinSize)
                {
                    throw new Asn1InvalidOperation("MinSize is not defined.");
                }
                return minSize;
            }
            set
            {
                minSize = value;
                HasMinSize = true;
            }
        }

        /// <summary>
        /// Gets or sets the maximal size constraint.
        /// </summary>
        public long MaxSize
        {
            get
            {
                if (!HasMaxSize)
                {
                    throw new Asn1InvalidOperation("MaxSize is not defined.");
                }
                return maxSize;
            }
            set
            {
                maxSize = value;
                HasMaxSize = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of class Asn1SizeConstraint without specifying any size constraints.
        /// </summary>
        public Asn1SizeConstraint()
        {
            HasMinSize = false;
            HasMaxSize = false;
        }
    }
}
