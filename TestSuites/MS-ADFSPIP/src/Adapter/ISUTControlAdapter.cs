// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    public interface ISUTControlAdapter : IAdapter
    {
        [MethodHelp("Trigger the proxy client to create a new set of published settings on a relying party trust")]
        bool TriggerPublishApplication(out string errMsg);

        [MethodHelp("Trigger the proxy client to create a new NonClaims set of published settings on a relying party trust")]
        bool TriggerPublishNonClaimsApp(out string errMsg);

        [MethodHelp("Trigger the proxy client to remove an existing set of published settings on a relying party trust")]
        bool TriggerRemoveApplication(out string errMsg);

        [MethodHelp("Trigger the proxy client to install or re-install the application proxy service")]
        bool TriggerInstallApplicationProxy(out string errMsg);

        [MethodHelp("Check whether the proxy client is correctly deployed")]
        bool IsApplicationProxyConfigured();

        [MethodHelp("Copy the Proxy trust certificate file with private key to this path and reply the password")]
        void TriggerExportCertificate();

        [MethodHelp("Clear proxy trust certificate cache if it impact new deployment")]
        void TriggerCleanCertificate();

        [MethodHelp("Uninstall proxy")]
        void TriggerProxyUninstall();

        [MethodHelp("Install proxy with these parameters")]
        void TriggerProxyInstall();
    }
}
