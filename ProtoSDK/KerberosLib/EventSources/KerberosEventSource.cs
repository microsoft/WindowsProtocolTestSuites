// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System.Diagnostics.Tracing;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.EventSources
{
    [EventSource(Name = "Microsoft-WindowsProtocolsTestSuite-Kerberos")]
    internal sealed class KerberosEventSource : EventSource
    {
        public KerberosEventSource()
            : base(EventSourceSettings.EtwSelfDescribingEventFormat)
        {
            
        }

        public static KerberosEventSource Log { get; } = 
            new KerberosEventSource();

        #region ClientSecurityContext

        [Event(1)]
        public void ClientSecurityContext_Construction_I_Started
            (string serverName, string domainName, string accountName, string accountType, string kdcAddress, int kdcPort, string transportType,
                string contextAttribute, string oidPkt, string salt) => 
            WriteEvent(1, serverName, domainName, accountName, accountType, kdcAddress, kdcPort, transportType,
                        contextAttribute, oidPkt, salt);

        [Event(2)]
        public void ClientSecurityContext_Construction_I_Completed
            () => WriteEvent(2);

        [Event(100)]
        public void ClientSecurityContext_InitializationWithServerTokenStarted(byte[] serverToken) =>
            WriteEvent(100, serverToken);

        [Event(101)]
        public void ClientSecurityContext_InitializationWithServerTokenCompleted() =>
            WriteEvent(101);

        [Event(102)]
        public void ClientSecurityContext_GetApResponseFromTokenStarted(byte[] token, string gssToken) =>
            WriteEvent(102, token, gssToken);

        [Event(103)]
        public void ClientSecurityContext_GetApResponseFromTokenCompleted(byte[] token, string encryptType,
                byte[] cipherData, byte[] sessionKey, byte[] clearText) =>
            WriteEvent(103, token, encryptType, cipherData, sessionKey, clearText);

        [Event(104)]
        public void ClientSecurityContext_InitializationWithoutServerTokenStarted() =>
            WriteEvent(104);

        [Event(105)]
        public void ClientSecurityContext_InitializationWithoutServerTokenCompleted() =>
            WriteEvent(105);

        [Event(106)]
        public void ClientSecurityContext_GetTGTCachedTokenStarted(AccountCredential inputCredential, string inputServerPrincipleName) =>
            WriteEvent(106, inputCredential, inputServerPrincipleName);

        #endregion
    }
}
