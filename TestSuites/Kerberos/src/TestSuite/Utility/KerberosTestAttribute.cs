// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite
{
    [Flags]
    public enum Feature
    {
        Default = 0x1,
        FAST = 0x2,
        Kile = 0x4,
        Pac = 0x8,
        Claim = 0x10,
        Silo = 0x20,
        RC4 = 0x40
    }

    public enum ApplicationServer
    {
        Smb2,
        Http,
        Ldap,
        All
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class KerberosTestAttribute : Attribute
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

    /// <summary>
    /// Used to specify this test method is running against which application server.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ApplicationServerAttribute : KerberosTestAttribute
    {
        ApplicationServer app;

        public ApplicationServerAttribute(ApplicationServer appServer)
        {
            app = appServer;
        }

        public ApplicationServer AppServer
        {
            get
            {
                return app;
            }
        }

        public override void Logging()
        {
            if (Site == null)
                return;
            Site.Log.Add(LogEntryKind.Comment, "This test case support running against {0} server", app);
        }
    }


    /// <summary>
    /// Used to specify covered features of a test method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class FeatureAttribute : KerberosTestAttribute
    {
        Feature kerberosFeature;

        public FeatureAttribute(Feature feature)
        {
            kerberosFeature = feature;
        }

        public Feature Feature
        {
            get
            {
                return kerberosFeature;
            }
        }

        public override void Logging()
        {
            if (Site == null)
                return;
            Site.Log.Add(LogEntryKind.Comment, "This test case need {0} support.", kerberosFeature);
        }
    }

}
