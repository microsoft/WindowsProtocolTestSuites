// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Azod.TestSuite
{
     [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class AzodTestAttribute : Attribute
    {
        public static ITestSite Site
        {
            get;
            set;
        }

        public virtual void Logging()
        {
        }
    }
}

