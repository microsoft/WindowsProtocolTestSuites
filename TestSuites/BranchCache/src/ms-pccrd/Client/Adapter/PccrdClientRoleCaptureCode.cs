// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrd
{
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.SoapMessage;

    /// <summary>
    /// A implementation of IPccrdClientAdapter.
    /// </summary>
    public partial class PccrdClientAdapter
    {
        /// <summary>
        /// Capture requirements related with SOAP header in Probe message.
        /// </summary>
        /// <param name="header">SOAP header.</param>
        private void CaptureHeaderRequirements(WsdHeader header)
        {
            // Since the message parsed by stack layer properly, capture the requirement directly.
            Site.CaptureRequirement(
                171,
                @"[The schema definition of Header of Probe message is:]<s:Header ... >
                <a:Action ... >
                http://schemas.xmlsoap.org/ws/2005/04/discovery/Probe
                </a:Action>
                <a:MessageID ... >xs:anyURI</a:MessageID>
                [<a:ReplyTo ... >endpoint-reference</a:ReplyTo>]?
                <a:To ... >xs:anyURI</a:To>
                ...
                </s:Header>");

            if (null != header.ReplyTo)
            {
                // Since the message parsed by stack layer properly, capture the requirement directly.
                Site.CaptureRequirement(
                    172,
                    "If [ReplyTo is] included in the header, it [ReplyTo] MUST be of type a:EndpointReferenceType [WS-Addressing].");
            }

            // Add debug info.
            Site.Log.Add(TestTools.LogEntryKind.Debug, "To: {0}", header.To.Value);
            bool isR173Verified = string.Equals(
                header.To.Value,
                "urn:schemas-xmlsoap-org:ws:2005:04:discovery",
                System.StringComparison.OrdinalIgnoreCase);
            Site.CaptureRequirementIfIsTrue(
                isR173Verified,
                173,
                "/s:Envelope/s:Header/a:To MUST be \"urn:schemas-xmlsoap-org:ws:2005:04:discovery\" [RFC 2141].");
        }

        /// <summary>
        /// Capture requirements related to Probe structure
        /// </summary>
        /// <param name="probe">Probe structure</param>
        private void CaptureProbeRequirements(ProbeType probe)
        {
            // Since the message parsed by stack layer properly, capture the requirement directly.
            Site.CaptureRequirement(
                24,
                "[In Probe] The Discovery Protocol uses a WSD Probe with the <Types> and <Scopes> elements in the WSD message.");
            
            // Since the message parsed by stack layer properly, capture the requirement directly.
            Site.CaptureRequirement(
                25,
                @"[The schema definition of Probe is:] <wsd:Probe>
                  <wsd:Types>
                    PeerDist:PeerDistData
                  </wsd:Types>
                  <wsd:Scopes MatchBy=""http://schemas.xmlsoap.org/ws/2005/04/discovery/strcmp0"">
                0200000000000000000000000000000000000000000000000000000000000000000
                0000000000000000000000000000000000000000000000000000000000000
                  </wsd:Scopes>
                </wsd:Probe>");

            // Since the probe message is received succesfully, the reuqirement can be captured directly
            Site.CaptureRequirement(
                175,
                @"Probe message MUST be sent using the following assignments:
                • DISCOVERY_PORT: port 3702 [IANA]
                • IPv4 multicast address: 239.255.255.250
                • IPv6 multicast address: FF02::C (link-local scope)");

            this.CaptureClientRoleScopesElementRequirements(probe.Scopes);
        }

        /// <summary>
        /// Capture requirements related to Scope structure
        /// </summary>
        /// <param name="scopes">Scope structure</param>
        private void CaptureClientRoleScopesElementRequirements(ScopesType scopes)
        {
            // Since the message parsed by stack layer properly, capture the requirement directly.
            Site.CaptureRequirement(
                53,
                @"[In Scopes] When specified in the request message, the peer MUST populate this value[Scopes]
                to indicate the segments it is searching for.");

            // Assume R174 is satisfied
            bool isR174Verified = true;
            foreach (string scope in scopes.Text)
            {
                // Add debug info.
                Site.Log.Add(TestTools.LogEntryKind.Debug, "scope: {0}", scope);
                if (null == scope)
                {
                    // The scope is not absolute uri, hence R174 is not satisfied
                    isR174Verified = false;
                    break;
                }
            }

            Site.CaptureRequirementIfIsTrue(
                isR174Verified,
                174,
                "Scopes MUST be a list of absolute URIs.");

            // "http://schemas.xmlsoap.org/ws/2005/04/discovery/strcmp0" is the case-sensitive
            // string comparison rule defined in Ws-Discovery
            Site.CaptureRequirementIfAreEqual<string>(
                "http://schemas.xmlsoap.org/ws/2005/04/discovery/strcmp0",
                scopes.MatchBy,
                57,
                @"[In Scopes] The Discovery Protocol uses the case-sensitive string comparison rule defined
                as part of the WSD standard schemas.");
        }
    }
}
