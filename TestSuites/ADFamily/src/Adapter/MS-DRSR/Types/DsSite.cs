// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// A class to load information of a site
    /// </summary>
    public class DsSite
    {
        /// <summary>
        /// DistinguishedName of this site
        /// </summary>
        public string DN { get; set; }

        /// <summary>
        /// Common name of this site
        /// </summary>
        public string CN { get; set; }

        /// <summary>
        /// objectGuid of this site object
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Domains where this site belongs to 
        /// </summary>
        public DsDomain[] Domains { get; set; }

        /// <summary>
        /// Site cost
        /// </summary>
        public uint Cost { get; set; }

        /// <summary>
        /// Servers in this site
        /// </summary>
        public DsServer[] Servers { get; set; }
    }
}
