// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery
{
    /// <summary>
    /// Wsdiscovery protocol strings.
    /// </summary>
    internal class ProtocolStrings
    {
        /// <summary>
        /// The namespace for WS-Discovery
        /// </summary>
        public const string Namespace = "http://schemas.xmlsoap.org/ws/2005/04/discovery";

        /// <summary>
        /// The namespace for WS-Addressing
        /// </summary>
        public const string WsaNamespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing";

        /// <summary>
        /// The name of ReplyTo element
        /// </summary>
        public const string ReplyToElement = "ReplyTo";

        /// <summary>
        /// The name of EndpointReference element
        /// </summary>
        public const string EprElement = "EndpointReference";

        /// <summary>
        /// The defalut prefix of namespace
        /// </summary>
        public const string DefaultPrefix = "d";

        /// <summary>
        /// The name of Hello element
        /// </summary>
        public const string HelloElement = "Hello";

        /// <summary>
        /// The name of Bye element
        /// </summary>
        public const string ByeElement = "Bye";

        /// <summary>
        /// The name of Probe element
        /// </summary>
        public const string ProbeElement = "Probe";

        /// <summary>
        /// The name of ProbeMatch element
        /// </summary>
        public const string ProbeMatchElement = "ProbeMatch";

        /// <summary>
        /// The name of ProbeMatches element
        /// </summary>
        public const string ProbeMatchesElement = "ProbeMatches";

        /// <summary>
        /// The name of Resolve element
        /// </summary>
        public const string ResolveElement = "Resolve";

        /// <summary>
        /// The name of ResolveMatch element
        /// </summary>
        public const string ResolveMatchElement = "ResolveMatch";

        /// <summary>
        /// The name of ResolveMatches element
        /// </summary>
        public const string ResolveMatchesElement = "ResolveMatches";

        /// <summary>
        /// The name of Types element
        /// </summary>
        public const string TypesElement = "Types";

        /// <summary>
        /// The name of Scopes element
        /// </summary>
        public const string ScopesElement = "Scopes";

        /// <summary>
        /// The name of MatchBy attribute
        /// </summary>
        public const string MatchByAttribute = "MatchBy";

        /// <summary>
        /// The name of XAddrs element
        /// </summary>
        public const string XAddrsElement = "XAddrs";

        /// <summary>
        /// The name of AppSequence element
        /// </summary>
        public const string AppSequenceElement = "AppSequence";

        /// <summary>
        /// The namespace for Hello action
        /// </summary>
        public const string HelloAction = "http://schemas.xmlsoap.org/ws/2005/04/discovery/Hello";

        /// <summary>
        /// The namespace for Bye action
        /// </summary>
        public const string ByeAction = "http://schemas.xmlsoap.org/ws/2005/04/discovery/Bye";

        /// <summary>
        /// The namespace for Probe action
        /// </summary>
        public const string ProbeAction = "http://schemas.xmlsoap.org/ws/2005/04/discovery/Probe";

        /// <summary>
        /// The namespace for Resolve action
        /// </summary>
        public const string ResolveAction = "http://schemas.xmlsoap.org/ws/2005/04/discovery/Resolve";

        /// <summary>
        /// The namespace for ProbeMatch action
        /// </summary>
        public const string ProbeMatchesAction = "http://schemas.xmlsoap.org/ws/2005/04/discovery/ProbeMatches";

        /// <summary>
        /// The namespace for ResolveMatch action
        /// </summary>
        public const string ResolveMatchesAction = "http://schemas.xmlsoap.org/ws/2005/04/discovery/ResolveMatches";

        /// <summary>
        /// The namespace for rfc2396 matching
        /// </summary>
        public const string ScopeMatchByPrefix = "http://schemas.xmlsoap.org/ws/2005/04/discovery/rfc2396";

        /// <summary>
        /// The namespace for strcmp0 matching
        /// </summary>
        public const string ScopeMatchByExact = "http://schemas.xmlsoap.org/ws/2005/04/discovery/strcmp0";

        /// <summary>
        /// The namespace for uuid matching
        /// </summary>
        public const string ScopeMatchByUuid = "http://schemas.xmlsoap.org/ws/2005/04/discovery/uuid";

        /// <summary>
        /// The namespace for ldap matching
        /// </summary>
        public const string ScopeMatchByLdap = "http://schemas.xmlsoap.org/ws/2005/04/discovery/ldap";

        /// <summary>
        /// The discovery address
        /// </summary>
        public const string DiscoveryAddress = "urn:schemas-xmlsoap-org:ws:2005:04:discovery";

        /// <summary>
        /// The Ipv4 multicast address
        /// </summary>
        public const string MulticastAddressIPv4 = "239.255.255.250";

        /// <summary>
        /// The Ipv6 multicast address
        /// </summary>
        public const string MulticastAddressIPv6 = "FF02::C";

        /// <summary>
        /// The multicast port
        /// </summary>
        public const int MulticastPort = 3702;

        /// <summary>
        /// The udp scheme
        /// </summary>
        public const string UdpScheme = "soap.udp";

        /// <summary>
        /// The anonymous Uri for WsAddressing approved in August 2004
        /// </summary>
        public const string WSAddressingAugust2004AnonymousUri = "http://schemas.xmlsoap.org/ws/2004/08/addressing/role/anonymous";

        /// <summary>
        /// The none Uri for WsAddressing approved in August 2004
        /// </summary>
        public const string WSAddressingAugust2004NoneUri = "http://www.w3.org/2005/08/addressing/none";
    }
}
