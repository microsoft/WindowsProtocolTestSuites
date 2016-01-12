// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.ExtendedLogging;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite
{
    /// <summary>
    /// Global settings of the whole test suite.
    /// </summary>
    [TestClass]
    public class GlobalSettings
    {
        protected static KerberosUtility.DumpMessageEventHandler DumpMessageHandler;

        /// <summary>
        /// This function will be called when loading assembly
        /// </summary>
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            SubscribeLogger();
        }

        /// <summary>
        /// This function will be called when unloading assembly
        /// </summary>
        [AssemblyCleanup]
        public static void Cleanup()
        {
            UnsubscribeLogger();
        }

        protected static void SubscribeLogger()
        {
            DumpMessageHandler = new KerberosUtility.DumpMessageEventHandler
                (
                    (string messageName,
                        string messageDescription,
                        KerberosUtility.DumpLevel dumpLevel,
                        byte[] payload) =>
                    {
                        if (Enum.IsDefined(typeof(DumpLevel), (int)dumpLevel))
                        {
                            ExtendedLogger.DumpMessage(messageName,
                                (DumpLevel)dumpLevel,
                                messageDescription,
                                payload);
                        }
                    }
                );
            KerberosUtility.DumpMessage += DumpMessageHandler;
        }

        protected static void UnsubscribeLogger()
        {
            KerberosUtility.DumpMessage -= DumpMessageHandler;
        }
    }
}
