// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Http;

namespace Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP
{
    public class StsService : HttpEndpointBase
    {
        public override void Initialize()
        {
            Urls.Add(Constraints.BackEndProxyTLSUrl);
            Urls.Add(Constraints.EstablishTrustUrl);
            Urls.Add(Constraints.GetSTSConfigurationUrl);
            Urls.Add(Constraints.ProxyTrustUrl);
            Urls.Add(Constraints.RelyingPartyTrustUrl);
            Urls.Add(Constraints.RenewTrustUrl);
            Urls.Add(Constraints.StoreUrl);
            Urls.Add(Constraints.FederationAuthUrl);
            base.Initialize();
        }
    }
}
