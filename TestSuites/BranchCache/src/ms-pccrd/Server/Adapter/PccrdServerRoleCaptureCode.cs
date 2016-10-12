// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrd
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Xml;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.SoapMessage;

    /// <summary>
    /// PCCRD server adapter implementation to test server endpoint.
    /// </summary>
    public partial class PccrdServerAdapter
    {
        /// <summary>
        /// Capture requirements related with SOAP header in ProbeMatch message.
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
        }

        /// <summary>
        /// Capture requirements for ProbeMatches strcture
        /// </summary>
        /// <param name="probeMatches">ProbeMatches structure</param>
        private void CaptureProbeMatchesRequirements(ProbeMatchesType probeMatches)
        {
            #region MS-PCCRD_R29

            // Since the message parsed by stack layer properly, capture the requirement directly.
            Site.CaptureRequirement(
                29,
                @"[The schema definition of ProbeMatch message is:]
                <wsd:ProbeMatches>
                  <wsd:ProbeMatch>
                    <wsa:EndpointReference>
                      <wsa:Address>
                              urn:uuid:87A89944-0230-43a5-AC4E-FAB1386C8E2C
                      </wsa:Address>
                    </wsa:EndpointReference>
                    <wsd:Types>
                      PeerDist:PeerDistData
                    </wsd:Types>
                    <wsd:Scopes>
                0200000000000000000000000000000000000000000000000000000000000000
                0000000000000000000000000000000000000000000000000000000000000000
                    </wsd:Scopes>
                    <wsd:XAddrs>
                      157.59.141.183:54321
                    </wsd:XAddrs>
                    <wsd:MetadataVersion>
                      1
                    </wsd:MetadataVersion>
                    <PeerDist:PeerDistData>
                      <PeerDist:BlockCount>
                        0000002A
                      </PeerDist:BlockCount>
                    </PeerDist:PeerDistData>
                  </wsd:ProbeMatch>
                </wsd:ProbeMatches>");

            #endregion

            foreach (ProbeMatchType match in probeMatches.ProbeMatch)
            {
                this.CaptureProbeMatchRequirements(match);
            }
        }

        /// <summary>
        /// Capture requirements for ProbeMatch structure
        /// </summary>
        /// <param name="probeMatch">ProbeMatch structure</param>
        private void CaptureProbeMatchRequirements(ProbeMatchType probeMatch)
        {
            this.CaptureAddressElementRequirements(probeMatch.EndpointReference.Address.Value);
            this.CaptureMetadataVersionRequirements(probeMatch.MetadataVersion);
            this.CaptureXAddrsElementRequirements(probeMatch.XAddrs);
            this.CaptureCustomizedAnyElementRequirements(probeMatch.Any);

            // Add debug info
            Site.Log.Add(LogEntryKind.Debug, "Types: {0}", probeMatch.Types);
            Site.CaptureRequirementIfIsNotNull(
                probeMatch.Types,
                49, 
                "[In Types] When [Types is] specified in the response, the value provides the Types that were found.");

            // Add debug info
            for (int i = 0; i < probeMatch.Scopes.Text.Length; i++)
            {
                Site.Log.Add(LogEntryKind.Debug, "Scopes[{0}]: {1}", i, probeMatch.Scopes.Text[i]);
            }

            Site.CaptureRequirementIfIsNotNull(
                probeMatch.Scopes.Text,
                54,
                "[In Scopes] When sending the response, the value provides the list of scopes present in the cache.");

            // Since Scopes and Types are verified, the following requirement can be verified directly.
            Site.CaptureRequirement(
                123,
                @"[In Receive Probe] The event processing involves checking the <Types> and <Scopes> elements to verify
                that the Probe was initiated by a peer.");
        }
        
        /// <summary>
        /// Capture requirements related to address field
        /// </summary>
        /// <param name="address">The remote endpoint address</param>
        private void CaptureAddressElementRequirements(string address)
        {
            bool isUri = Uri.IsWellFormedUriString(address, UriKind.RelativeOrAbsolute);
            bool isUuid = address.TrimStart().StartsWith("urn:uuid:", StringComparison.OrdinalIgnoreCase);
            string[] result = address.Trim().Split(new char[] { ':' });
            bool isGuid = false;
            if (result.Length > 1)
            {
                Guid guid = new Guid();
                isGuid = Guid.TryParse(result[2], out guid);
            }

            // Add debug info.
            Site.Log.Add(LogEntryKind.Debug, "Address: {0}", address);
            Site.Assert.IsTrue(isUri, "Address is{0}URI format.", isUri ? " " : " not ");
            Site.Assert.IsTrue(isUuid, "Address is{0}as a UUID scheme URI.", isUuid ? " " : " not ");
            Site.Assert.IsTrue(isGuid, "Address is{0}set to a GUID.", isGuid ? " " : " not ");

            // Since above three assertion passed, the following requirement can be verified directly.
            Site.CaptureRequirement(
                60,
                "[In Address] It [Address] MUST be set to a globally unique identifier (GUID) as a \"uuid:\" scheme URI.<2> ");
        }

        /// <summary>
        /// Capture requirements related to metadataVersion field
        /// </summary>
        /// <param name="data">The metadataVersion element</param>
        private void CaptureMetadataVersionRequirements(uint data)
        {
            #region MS-PCCRD_R147
            
            // add debug info.
            Site.Log.Add(LogEntryKind.Debug, "MetadataVersion:{0}", data);

            Site.CaptureRequirementIfAreEqual<uint>(
                1,
                data,
                147,
                "[The <MetadataVersion> element] MUST be set to 1.");
            
            #endregion
        }

        /// <summary>
        /// Capture requirements related to xAddrs field
        /// </summary>
        /// <param name="xAddrs">The xml element xAddrs</param>
        private void CaptureXAddrsElementRequirements(string xAddrs)
        {
            // ':' is the seperator
            string[] addrs = xAddrs.Trim().Split(new char[] { ':' });
            bool containIP = false;
            bool containPort = false;
            if (addrs.Length > 1)
            {
                IPAddress ip = IPAddress.Any;
                containIP = IPAddress.TryParse(addrs[0], out ip);
                int port = 0;
                containPort = int.TryParse(addrs[1], out port);
            }

            // Add debug info.
            Site.Log.Add(LogEntryKind.Debug, "XAddrs: {0}", xAddrs);

            Site.CaptureRequirementIfIsTrue(
                containIP && containPort,
                73,
                @"[In XAddrs] Each transport URI string MUST contain an address and port number
                that can be used for connection by a remote host.");
        }

        /// <summary>
        /// Capture requirements related to any nodes
        /// </summary>
        /// <param name="any">The xml element any</param>
        private void CaptureCustomizedAnyElementRequirements(XmlElement[] any)
        {
            bool hasChildren = any.Length > 0;
            Site.CaptureRequirementIfIsTrue(
                hasChildren,
                75,
                "[In Any] A custom XML section MUST be embedded in the <Any> element of the WSD message body.");

            XmlElement peer = null;
            bool isPeerDist = false;
            foreach (XmlElement ele in any)
            {
                if (ele.LocalName == "PeerDistData" && ele.NamespaceURI ==
                    "http://schemas.microsoft.com/p2p/2007/09/PeerDistributionDiscovery")
                {
                    if (ele.FirstChild.LocalName == "BlockCount" && ele.FirstChild.NamespaceURI ==
                            "http://schemas.microsoft.com/p2p/2007/09/PeerDistributionDiscovery")
                    {
                        isPeerDist = true;
                        peer = ele;
                    }

                    break;
                }
            }

            Site.CaptureRequirementIfIsTrue(
                isPeerDist,
                77,
                @"[In Any] The XML MUST be formatted as follows:
                <PeerDist:PeerDistData>
                   <PeerDist:BlockCount>
                      List of block counts
                   </PeerDist:BlockCount>
                </PeerDist:PeerDistData>");

            Site.Assert.IsNotNull(peer.FirstChild.InnerText, "BlockCount is {0}", peer.FirstChild.InnerText);
            
            string matchPattern = "[A-Fa-f0-9]*";
            Regex regex = new Regex(matchPattern);
            bool isR80Verified = regex.IsMatch(peer.FirstChild.InnerText);
            
            Site.CaptureRequirementIfIsTrue(
                isR80Verified,
                80,
                @"[In Any, BlockCount:] This element is set to the UTF-8-encoded string representation of the hexBinary 
                [XMLSCHEMA1.1/2] packed array of integers in network byte order.");
        }
    }
}

