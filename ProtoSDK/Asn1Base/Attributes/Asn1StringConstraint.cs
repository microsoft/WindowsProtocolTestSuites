// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Spcifies the constraints of an STRING.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class Asn1StringConstraint : Asn1SizeConstraint
    {
        /// <summary>
        /// Gets or sets a string or a regex which contains all the available characters for the STRING type.
        /// </summary>
        public string PermittedCharSet { get; set; }

        /// <summary>
        /// Initializes a new instance of the Asn1StringConstraint class that doesn't have constraints.
        /// </summary>
        public Asn1StringConstraint()
        {
            PermittedCharSet = null;
        }
    }
}
