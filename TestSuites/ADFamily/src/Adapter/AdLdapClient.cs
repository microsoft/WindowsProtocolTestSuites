// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.Protocols;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Text;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Common
{
    /// <summary>
    /// Provides definition for the interface methods
    /// </summary>
    public class AdLdapClient
    {
        #region Variables

        public AdtsLdapClient ldapClientStack;
        string ldapErrorCode;
        string windowsErrorCode;
        TimeSpan timeout = new TimeSpan(0, 5, 0);
        int transportBufferSize = 8192;

        /// <summary>
        /// </summary>
        // all operated objects
        public Stack<string> testedObjects = new Stack<string>();
        // all objects in active directory currently
        Dictionary<string, bool> objectsInAd = new Dictionary<string, bool>();

        #endregion

        #region Adapter Instance

        static AdLdapClient _adapter = null;

        /// <summary>
        /// Static instance for AD LDAP Adapter
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static AdLdapClient Instance()
        {
            if (_adapter == null)
            {
                _adapter = new AdLdapClient();
            }
            return _adapter;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This method is to extract error messages from ldap error responses
        /// </summary>
        /// <param name="errorMessage">messages returned by ldap error response</param>
        /// <returns>error message extracted</returns>
        protected string ErrorToString(string errorMessage)
        {
            string result = null;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    int errorCode = Convert.ToInt32(
                        errorMessage.Split(':')[0].Substring(4),
                        16);

                    result = Enum.GetName(typeof(WindowsErrorCode), errorCode);
                }
                catch
                {

                }
            }
            if (null == result)
            {
                return "UnKnownError";
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Try to parse a Filter from a filter string
        /// </summary>
        /// <param name="filterString">the filter string</param>
        /// <returns>return parsing result</returns>
        protected Filter ParseFilterString(string filterString)
        {
            Stack<Filter> filterStack = new Stack<Filter>();
            Stack<int> braceletIndex = new Stack<int>();

            for (int i = 0; i < filterString.Length; i++)
            {
                if (filterString[i] == '(')
                {
                    braceletIndex.Push(i);
                }

                if (filterString[i] == ')')
                {
                    int left = braceletIndex.Pop();
                    if (filterString[left + 1] == '&')
                    {
                        Filter[] filters = new Filter[filterStack.Count];
                        for (int j = 0; filterStack.Count != 0; j++)
                        {
                            filters[j] = filterStack.Pop();
                        }
                        Filter filter = new Filter();
                        filter.SetData(Filter.and, new Asn1SetOf<Filter>(filters));
                        //filter.Set_and(new _SetOfFilter(filters));
                        filterStack.Push(filter);
                    }
                    else if (filterString[left + 1] == '|')
                    {
                        Filter[] filters = new Filter[filterStack.Count];
                        for (int j = 0; filterStack.Count != 0; j++)
                        {
                            filters[j] = filterStack.Pop();
                        }
                        Filter filter = new Filter();
                        filter.SetData(Filter.or, new Asn1SetOf<Filter>(filters));
                        //filter.Set_or(new _SetOfFilter(filters));
                        filterStack.Push(filter);
                    }
                    else
                    {
                        filterStack.Push(
                            ParseFilterBinaryOperation(filterString.Substring(left + 1, i - left - 1))
                            );
                    }
                }
            }

            if (filterStack.Count > 1)
            {
                throw new Exception(string.Format("Filter {0} is mal-formated.", filterString));
            }

            return filterStack.Pop();
        }

        /// <summary>
        /// This method encodes binary operations in filters from string to Filter: (attribute)(operation)(value)
        /// And it also support wrapping not: !(), (!())
        /// </summary>
        /// <param name="filterExpression">filter expression to be parsed</param>
        /// <returns>returns the encoding result</returns>
        protected Filter ParseFilterBinaryOperation(string filterExpression)
        {
            Filter filter = new Filter();

            // remove bracelets
            // assume the expression contains:
            // (a) no bracelet: A=V
            // (b) only 1 pair of bracelet: !(A=V), (A=V)
            filterExpression.Trim('(', ')');
            string attribute, value;
            bool wrapNot = false;

            // (1) not: filterExpression = "!()"
            if (filterExpression.Contains("!"))
            {
                filterExpression.Trim('!');
                wrapNot = true;
            }

            // (2) greater or equal: filterExpression = "(A>=V)"
            if (filterExpression.Contains(">="))
            {
                string[] str = filterExpression.Split(new string[] { ">=" }, 2, StringSplitOptions.RemoveEmptyEntries);
                attribute = str[0];
                value = str[1];
                //filter.Set_greaterOrEqual(
                //    new AttributeValueAssertion(
                //        new AttributeDescription(attribute),
                //        new AssertionValue(value)));
                filter.SetData(Filter.greaterOrEqual, new AttributeValueAssertion(new AttributeDescription(attribute), new AssertionValue(value)));
            }
            // (3) less or equal: filterExpression = "(A<=V)"
            else if (filterExpression.Contains("<="))
            {
                string[] str = filterExpression.Split(new string[] { "<=" }, 2, StringSplitOptions.RemoveEmptyEntries);
                attribute = str[0];
                value = str[1];
                //filter.Set_lessOrEqual(
                //    new AttributeValueAssertion(
                //        new AttributeDescription(attribute),
                //        new AssertionValue(value)));
                filter.SetData(Filter.lessOrEqual, new AttributeValueAssertion(new AttributeDescription(attribute), new AssertionValue(value)));
            }
            // (4) approximate match: filterExpression = "A~=V"
            else if (filterExpression.Contains("~="))
            {
                string[] str = filterExpression.Split(new string[] { "~=" }, 2, StringSplitOptions.RemoveEmptyEntries);
                attribute = str[0];
                value = str[1];
                //filter.Set_approxMatch(
                //    new AttributeValueAssertion(
                //        new AttributeDescription(attribute),
                //        new AssertionValue(value)));
                filter.SetData(Filter.approxMatch, new AttributeValueAssertion(new AttributeDescription(attribute), new AssertionValue(value)));
            }
            // (10) extensible match: filter = "(A:1.2.840.113556.1.4.xxxx:=V)"
            //     [MS-ADTS] section 3.1.1.3.1.3.1 Search Filters
            //     Active Directory exposes extensible match rules that are defined in section 3.1.1.3.4.4, namely
            //     (a) LDAP_MATCHING_RULE_BIT_AND
            //     (b) LDAP_MATCHING_RULE_BIT_OR
            //     (c) LDAP_MATCHING_RULE_TRANSITIVE_EVAL
            //     (d) LDAP_MATCHING_RULE_DN_WITH_DATA
            //     Other than these rules, the rules that Active Directory uses for comparing values (for example, 
            //     comparing two String(Unicode) attributes for equality or ordering) are not exposed as extensible
            //     match rules. These comparison rules are documented for each syntax type in section 3.1.1.2.2.4. 
            else if (filterExpression.Contains(":")
                && filterExpression.Contains(":="))
            {
                string[] matchingRule = filterExpression.Split(new string[] { ":", ":=" }, 3, StringSplitOptions.RemoveEmptyEntries);
                if (matchingRule.Length != 3)
                {
                    throw new Exception(string.Format("The matching rule format is wrong: {0}", filterExpression));
                }
                string attr = matchingRule[0].Trim();
                string rule = matchingRule[1].Trim();
                string val = matchingRule[2].Trim();

                MatchingRuleAssertion matchingRuleAssertion = new MatchingRuleAssertion();
                matchingRuleAssertion.matchingRule = new MatchingRuleId(rule);
                matchingRuleAssertion.type = new AttributeDescription(attr);
                matchingRuleAssertion.matchValue = new AssertionValue(val);
                //filter.Set_extensibleMatch(matchingRuleAssertion);
                filter.SetData(Filter.extensibleMatch, matchingRuleAssertion);
            }
            // (5) present: filterExpression = "A=*"
            else if (filterExpression.EndsWith("=*"))
            {
                string[] str = filterExpression.Split(new string[] { "=" }, 2, StringSplitOptions.RemoveEmptyEntries);
                attribute = str[0];
                //filter.Set_present(
                //    new AttributeDescription(attribute));
                filter.SetData(Filter.present, new AttributeDescription(attribute));
            }
            else if (filterExpression.Contains("="))
            {
                string[] str = filterExpression.Split(new string[] { "=" }, 2, StringSplitOptions.RemoveEmptyEntries);
                attribute = str[0];
                value = str[1];

                // (6) substrings: filter = "(A=*V2)", "(A=V1*)", "(A=V1*V2)", "(A=V1*V2*...*V3)"
                if (value.Contains("*"))
                {
                    string[] values = value.Split('*');

                    SubstringFilter_substrings_element[] elements;

                    if (values.Any(x => string.IsNullOrEmpty(x)))
                    {
                        elements = new SubstringFilter_substrings_element[1];
                        elements[0] = new SubstringFilter_substrings_element();

                        if (filterExpression.EndsWith("*"))
                            elements[0].SetData(SubstringFilter_substrings_element.initial, new LDAPString(values[0]));
                        //elements[0].Set_initial(new LDAPString(values[0]));
                        else if (filterExpression.StartsWith("*"))
                            elements[0].SetData(SubstringFilter_substrings_element.final, new LDAPString(values[0]));
                            //elements[0].Set_final(new LDAPString(values[0]));
                    }
                    else
                    {
                        elements = new SubstringFilter_substrings_element[value.Length];
                        for (int i = 0; i < values.Length; i++)
                        {
                            elements[i] = new SubstringFilter_substrings_element();
                        }

                        //elements[0].Set_initial(new LDAPString(values.First()));
                        elements[0].SetData(SubstringFilter_substrings_element.initial, new LDAPString(values.First()));
                        //elements[values.Length - 1].Set_final(new LDAPString(values.Last()));
                        elements[values.Length - 1].SetData(SubstringFilter_substrings_element.final, new LDAPString(values.Last()));
                        for (int i = 1; i < values.Length - 1; i++)
                        {
                            //elements[i].Set_any(new LDAPString(values[i]));
                            elements[i].SetData(SubstringFilter_substrings_element.any, new LDAPString(values[i]));
                        }
                    }
                    //filter.Set_substrings(
                    //        new SubstringFilter(
                    //            new AttributeDescription(attribute),
                    //            new _SeqOfSubstringFilter_substrings_element(elements)));
                    
                    filter.SetData(Filter.substrings, new SubstringFilter(
                                new AttributeDescription(attribute),
                                new Asn1SequenceOf<SubstringFilter_substrings_element>(elements)));
                }
                // (7) equality match: filterExpression = "(A=V)"
                else
                {
                    //filter.Set_equalityMatch(
                    //    new AttributeValueAssertion(
                    //        new AttributeDescription(attribute),
                    //        new AssertionValue(value)));
                    filter.SetData(Filter.equalityMatch, new AttributeValueAssertion(
                            new AttributeDescription(attribute),
                            new AssertionValue(value)));
                }
            }
            else
            {
                throw new Exception(string.Format("Filter operation not supported, filter: {0}.", filterExpression));
            }

            if (wrapNot)
            {
                Filter notFilter = new Filter();
                //notFilter.Set_not(filter);
                notFilter.SetData(Filter.not, filter);
                return notFilter;
            }

            return filter;
        }

        /// <summary>
        /// This method gets attribute values for a particular attribute given the search result entry
        /// </summary>
        /// <param name="entryPacket">The search result entry packet</param>
        /// <param name="attributeName">The attribute name to look for</param>
        /// <returns>returns the attribute values</returns>
        public string[] GetAttributeValuesInString(
            AdtsSearchResultEntryPacket entryPacket,
            string attributeName)
        {
            string[] result = null;

            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry searchResultEntry
                = (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)entryPacket.GetInnerRequestOrResponse();

            foreach (PartialAttributeList_element attribute in searchResultEntry.attributes.Elements)
            {
                if (Encoding.ASCII.GetString(attribute.type.ByteArrayValue).Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    AttributeValue[] values = attribute.vals.Elements;
                    if (values.Length == 0) return null;

                    result = new string[values.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        result[i] = Encoding.ASCII.GetString(values[i].ByteArrayValue);
                    }
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// This method gets attribute values for a particular attribute given the search result entry
        /// </summary>
        /// <param name="entryPacket">The search result entry packet</param>
        /// <param name="attributeName">The attribute name to look for</param>
        /// <returns>returns the attribute values</returns>
        public byte[][] GetAttributeValuesInBytes(
            AdtsSearchResultEntryPacket entryPacket,
            string attributeName)
        {
            byte[][] result = null;

            Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry searchResultEntry
                = (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)entryPacket.GetInnerRequestOrResponse();

            foreach (PartialAttributeList_element attribute in searchResultEntry.attributes.Elements)
            {
                if (Encoding.ASCII.GetString(attribute.type.ByteArrayValue).Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    AttributeValue[] values = attribute.vals.Elements;
                    result = new byte[values.Length][];
                    for (int i = 0; i < values.Length; i++)
                    {
                        result[i] = values[i].ByteArrayValue;
                    }
                    break;
                }
            }

            return result;
        }

        #endregion

        #region LDAP Binding

        /// <summary>
        /// Anonymous Binding:
        /// Sends an LDAP anonymous bind request to LDAP server.
        /// </summary>
        /// <param name="serverName">This parameter represents the FQDN or the NetBios Name of the LDAP server</param>
        /// <param name="portNumber">LDAP port number</param>
        /// <returns>return ldapClientStack</returns>
        public string AnonymousBinding(
            IPAddress serverIP,
            int portNumber,
            bool isWindows = true)
        {
            SocketTransportConfig transportConfig = new SocketTransportConfig();
            transportConfig.RemoteIpAddress = serverIP;
            transportConfig.RemoteIpPort = portNumber;
            transportConfig.BufferSize = transportBufferSize;
            transportConfig.Type = StackTransportType.Tcp;
            transportConfig.Role = Role.Client;

            ldapClientStack = new AdtsLdapClient(AdtsLdapVersion.V3, transportConfig);
            ldapClientStack.Connect();
            AdtsBindRequestPacket bindRequest = ldapClientStack.CreateSimpleBindRequest(null, String.Empty);
            ldapClientStack.SendPacket(bindRequest);
            AdtsLdapPacket response = ldapClientStack.ExpectPacket(timeout);
            AdtsBindResponsePacket bindResponse = (AdtsBindResponsePacket)response;
            ldapErrorCode = Enum.GetName(
                    typeof(ResultCode),
                    ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.BindResponse)
                    bindResponse.GetInnerRequestOrResponse()).resultCode.Value);

            // if the ldap server is windows, format the error code
            if (isWindows)
            {
                if (ldapErrorCode.Equals(ResultCode.Success.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return ldapErrorCode + "_STATUS_SUCCESS";
                }
                else
                {
                    windowsErrorCode = ErrorToString(
                        Encoding.ASCII.GetString(
                        ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.BindResponse)
                        bindResponse.GetInnerRequestOrResponse()).errorMessage.ByteArrayValue));

                    return ldapErrorCode + "_" + windowsErrorCode;
                }
            }
            // if the ldap server is non windows, return ldap error code straightly
            else
            {
                return ldapErrorCode;
            }
        }

        /// <summary>
        /// ConnectAndBind:
        /// Sends an LDAP bind request to LDAP Server using the current credentials.
        /// </summary>
        /// <param name="serverName">This parameter represents the NetBios name of the ldap server</param>
        /// <param name="portNumber">LDAP Port Number</param>
        /// <param name="userName">Username of the Domain Controller</param>
        /// <param name="password">Password of the Domain Controller</param>
        /// <param name="domainName">Netbios domain name of the domain, excluding "com net org" part</param>
        /// <param name="authenticationType">The type of the Authentication mechanisms</param>
        /// <returns>return ldapClientStack</returns>
        public string ConnectAndBind(
            string serverName,
            IPAddress serverIP,
            int portNumber,
            string userName, // to provide credentials
            string password,
            string domainName,
            AuthType authenticationType,
            bool isWindows = true)
        {
            SocketTransportConfig transportConfig = new SocketTransportConfig();
            transportConfig.RemoteIpAddress = serverIP;
            transportConfig.RemoteIpPort = portNumber;
            transportConfig.BufferSize = transportBufferSize;
            transportConfig.Type = StackTransportType.Tcp;
            transportConfig.Role = Role.Client;

            ldapClientStack = new AdtsLdapClient(AdtsLdapVersion.V3, transportConfig);
            ldapClientStack.Connect();

            // Using GSS-SPNEGO as its authentication protocol
            SecurityPackageType securityType;
            switch (authenticationType)
            {
                case AuthType.Ntlm:
                    securityType = SecurityPackageType.Ntlm;
                    break;
                case AuthType.Kerberos:
                default:
                    securityType = SecurityPackageType.Kerberos;
                    break;
            }
            AccountCredential transportCredential = new AccountCredential(
                domainName,
                userName,
                password);

            ClientSecurityContextAttribute contextAttributes = ClientSecurityContextAttribute.Connection;
            SspiClientSecurityContext securityContext = new SspiClientSecurityContext(
                securityType,
                transportCredential,
                String.Concat("LDAP/", serverName),
                contextAttributes,
                SecurityTargetDataRepresentation.SecurityNetworkDrep);
            securityContext.Initialize(null);

            // [MS-ADTS] Section 5.1.1.1.2 SASL Authentication: sasl bind
            // NTLM authentication requires two SaslBind requests
            // First Sasl Bind Request:
            AdtsBindRequestPacket bindRequest = ldapClientStack.CreateSaslBindRequest(securityContext, true);
            ldapClientStack.SendPacket(bindRequest);
            AdtsLdapPacket response = ldapClientStack.ExpectPacket(timeout);
            AdtsBindResponsePacket bindResponse = (AdtsBindResponsePacket)response;
            int bindStatus = (int)((BindResponse)bindResponse.GetInnerRequestOrResponse()).resultCode.Value;

            // Second Sasl Bind Request:
            if (bindStatus == LDAPResult_resultCode.saslBindInProgress)
            {
                securityContext.Initialize(((BindResponse)bindResponse.GetInnerRequestOrResponse()).serverSaslCreds.ByteArrayValue);
                bindRequest = ldapClientStack.CreateSaslBindRequest(securityContext, true);
                ldapClientStack.SendPacket(bindRequest);
                response = ldapClientStack.ExpectPacket(timeout);
                bindResponse = (AdtsBindResponsePacket)response;
                bindStatus = (int)((BindResponse)bindResponse.GetInnerRequestOrResponse()).resultCode.Value;
            }

            ldapErrorCode = Enum.GetName(
                    typeof(ResultCode), bindStatus);

            // if the ldap server is windows, format the error code
            if (isWindows)
            {
                if (bindStatus == LDAPResult_resultCode.success)
                {
                    return ldapErrorCode + "_STATUS_SUCCESS";
                }
                else
                {
                    windowsErrorCode = ErrorToString(
                        Encoding.ASCII.GetString(
                        ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.BindResponse)
                        bindResponse.GetInnerRequestOrResponse()).errorMessage.ByteArrayValue));

                    return ldapErrorCode + "_" + windowsErrorCode;
                }
            }
            // if the ldap server is non windows, return ldap error code straightly
            else
            {
                return ldapErrorCode;
            }
        }

        /// <summary>
        /// ConnectAndBindEx method connects to the LDAP Server over UDP.
        /// </summary>
        /// <param name="serverIP">This parameter represents the IP Address of the ldap server</param>
        /// <param name="portNumber">LDAP Port Number</param>
        /// <param name="stackTransportType">The type of the stack transport</param>
        /// <returns>return ldapClientStack</returns>
        public string ConnectAndBindEx(
            IPAddress serverIP,
            int portNumber,
            StackTransportType stackTransportType,
            bool isWindows = true)
        {
            SocketTransportConfig transportConfig = new SocketTransportConfig();
            transportConfig.Role = Role.Client;
            transportConfig.MaxConnections = 1;
            transportConfig.BufferSize = transportBufferSize;
            transportConfig.RemoteIpAddress = serverIP;
            transportConfig.RemoteIpPort = portNumber;
            transportConfig.Type = stackTransportType;
            // For UDP bind
            if (transportConfig.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                transportConfig.LocalIpAddress = IPAddress.Any;
            }
            else if (transportConfig.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                transportConfig.LocalIpAddress = IPAddress.IPv6Any;
            }

            ldapClientStack = new AdtsLdapClient(AdtsLdapVersion.V3, transportConfig);
            ldapClientStack.Connect();

            if (null == ldapClientStack)
            {
                if (isWindows)
                    ldapErrorCode = ResultCode.Other.ToString() + "_STATUS_FAILED";
                else
                    ldapErrorCode = ResultCode.Other.ToString();
            }
            else
            {
                if (isWindows)
                    ldapErrorCode = ResultCode.Success.ToString() + "_STATUS_SUCCESS";
                else
                    ldapErrorCode = ResultCode.Success.ToString();
            }

            return ldapErrorCode;
        }

        #endregion

        #region LDAP Unbind

        /// <summary>
        /// Unbind method sends an LDAP unbind request to LDAP Server.
        /// </summary>
        public void Unbind()
        {
            if (null == ldapClientStack)
            {
                return;
            }

            AdtsUnbindRequestPacket unbindRequest = ldapClientStack.CreateUnbindRequest();
            ldapClientStack.SendPacket(unbindRequest);

            ldapClientStack.Disconnect();

            if (ldapClientStack != null)
            {
                ldapClientStack.Dispose();
                ldapClientStack = null;
            }
        }

        #endregion

        #region LDAP Add

        /// <summary>
        /// [MS-ADTS] Section 3.1.1.5.2 Add Operation
        /// AddObject method adds an object to the Active Directory.
        /// </summary>
        /// <param name="objectDn">The distinguishedName of the new object in the directory</param>
        /// <param name="attributes">This parameter contains the list of attributes and their values of the object to be added</param>
        /// <param name="controls">This parameter specifies the extended controls of this operation</param>
        /// <returns>Result code of the add response</returns>
        public string AddObject(
            string objectDn,
            List<DirectoryAttribute> attributes,
            DirectoryControl[] controls,
            bool isWindows = true)
        {
            // if ldapClientStack is null, a connection to the server is not established and endpoint is not binded
            if (null == ldapClientStack)
            {
                throw new Exception(string.Format("A connection to the ldap server is not established and binded."));
            }
            else
            {
                string distinguishedName = objectDn;
                if (distinguishedName != null) distinguishedName = distinguishedName.Trim();

                // this object brings trouble for env
                if (distinguishedName.ToLower(CultureInfo.InvariantCulture)
                    .Contains("CN=Password Settings Container,".ToLower(CultureInfo.InvariantCulture)))
                    return "Success_STATUS_SUCCESS";
                else if (distinguishedName.ToLower(CultureInfo.InvariantCulture)
                    .Contains("CN=newappnc,".ToLower(CultureInfo.InvariantCulture)))
                    return "Success_STATUS_SUCCESS";

                testedObjects.Push(string.Format("Add:{0}", distinguishedName));
                try
                {
                    objectsInAd.Add(distinguishedName.ToLower(CultureInfo.InvariantCulture), true);
                }
                catch
                {
                    // throw new Exception(string.Format("objectDn {0} already exists in active directory", distinguishedName));
                }

                #region attributes

                // translate object attributes from DirectoryAttribute to KeyValuePair<string, string[]> which is supported by LDAP SDK
                KeyValuePair<string, string[]>[] objAttributes = new KeyValuePair<string, string[]>[attributes.Count];
                for (int i = 0; i < attributes.Count; i++)
                {
                    string name = attributes[i].Name;
                    string[] value;

                    // if attribute name contains ':', it is not a .net format DirectoryAttribute
                    if (name.Contains(':'))
                    {
                        name = attributes[i].Name.Split(':')[0].Trim();
                        value = attributes[i].Name.Split(':')[1].Trim().Split(';');
                        for (int j = 0; j < value.Length; j++)
                        {
                            value[j] = value[j].Trim();
                        }

                        // if the adding attribute is objectGUID, convert each specified GUID to a byte array
                        // [MS-ADTS] section 3.1.1.5.2 Add Operation - Constraints
                        // The requester is allowed to specify the objectGUID if the following five conditions are all satisfied:
                        // (1) The fSpecifyGUIDOnAdd heuristic is true in the dSHeuristics attribute (see section 6.1.1.2.4.1.2).
                        // (2) The requester has the Add-GUID control access right (section 5.1.3.2.1) on the NC root of the NC where the object is being added.
                        // (3) The requester-specified objectGUID is not currently in use in the forest.
                        // (4) Active Directory is operating as AD DS.
                        // (5) The requester-specified objectGUID is not the NULL GUID.
                        if (String.Equals(name, "objectGUID", StringComparison.InvariantCultureIgnoreCase))
                        {
                            // Orignially, should not specify more than 1 value for the objectGUID attribute
                            // For the reason of some negative cases, will not check in adapter
                            for (int j = 0; j < value.Length; j++)
                            {
                                value[j] = Encoding.ASCII.GetString((new Guid(value[j])).ToByteArray());
                            }
                        }
                    }
                    // if attribute name does not contain ':', it is a .net format DirectoryAttribute
                    else
                    {
                        value = (string[])attributes[i].GetValues(typeof(string));
                    }

                    objAttributes[i] = new KeyValuePair<string, string[]>(name, value);
                }

                #endregion

                #region send packet and extended controls

                // Create add request and send packet
                AdtsAddRequestPacket addRequest = ldapClientStack.CreateAddRequest(
                    distinguishedName,
                    objAttributes);

                if (controls != null)
                {
                    foreach (DirectoryControl control in controls)
                    {
                        ldapClientStack.AddDirectoryControls(addRequest, control);
                    }
                }
                ldapClientStack.SendPacket(addRequest);

                #endregion

                #region receive packet

                // Expect response packet
                AdtsLdapPacket response = ldapClientStack.ExpectPacket(timeout);
                if (null == response)
                {
                    throw new Exception(string.Format("Response not received within time for the add request."));
                }
                if (!(response is AdtsAddResponsePacket))
                {
                    throw new Exception(string.Format("The response is not an ldap add response."));
                }

                #endregion

                #region return

                AdtsAddResponsePacket addResponse = (AdtsAddResponsePacket)response;
                ldapErrorCode = Enum.GetName(
                    typeof(ResultCode),
                    ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.AddResponse)
                    addResponse.GetInnerRequestOrResponse()).resultCode.Value);

                // if the ldap server is windows, format the error code
                if (isWindows)
                {
                    if (ldapErrorCode.Equals(ResultCode.Success.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return ldapErrorCode + "_STATUS_SUCCESS";
                    }
                    else
                    {
                        windowsErrorCode = ErrorToString(
                            Encoding.ASCII.GetString(
                            ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.AddResponse)
                            addResponse.GetInnerRequestOrResponse()).errorMessage.ByteArrayValue));

                        // [MS-ADTS] Section 3.1.1.3.1.2.1 Naming Attributes
                        // The server will reject an attempt to create such a non-uniquely named object with the error 
                        // entryAlreadyExists / <unrestricted>. This requirement for unique AttributeValues guarantees 
                        // the uniqueness of canonical names.
                        if (ldapErrorCode.Equals(ResultCode.EntryAlreadyExists.ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            return "EntryAlreadyExists_UnKnownError";
                        }

                        return ldapErrorCode + "_" + windowsErrorCode;
                    }
                }
                // if the ldap server is non windows, return ldap error code straightly
                else
                {
                    return ldapErrorCode;
                }

                #endregion
            }
        }

        #endregion

        #region LDAP Modify

        /// <summary>
        /// [MS-ADTS] Section 3.1.1.5.3 Modify Operation
        /// ModifyObject method modifies an existing object in the Active Directory.
        /// </summary>
        /// <param name="objectDn">The distinguished name of the object to be modified</param>
        /// <param name="attributes">This parameter lists out the attributes and their values of the object to be modified</param>
        /// <param name="controls">This parameter specifies the extended controls of this operation</param>
        /// <returns>Result code of the modify response</returns>
        public string ModifyObject(
            string objectDn,
            List<DirectoryAttributeModification> attributes,
            DirectoryControl[] controls,
            bool isWindows = true)
        {
            // if ldapClientStack is null, a connection to the server is not established and endpoint is not binded
            if (null == ldapClientStack)
            {
                throw new Exception(string.Format("A connection to the ldap server is not established and binded."));
            }
            else
            {
                string distinguishedName = objectDn;
                if (distinguishedName != null) distinguishedName = distinguishedName.Trim();

                testedObjects.Push(string.Format("Modify:{0}", distinguishedName));

                #region attributes

                DirectoryAttributeModification[] modification = (DirectoryAttributeModification[])attributes.ToArray();

                #endregion

                #region send packet and extended controls

                // Create modify request and send packet
                AdtsModifyRequestPacket modifyRequest = ldapClientStack.CreateModifyRequest(distinguishedName, modification);
                // [MS-ADTS] section 3.1.1.3.4.1.14
                // The LDAP_SERVER_SHOW_DELETED_OID control is used with an LDAP operation to specify
                // that tombstones and deleted-objects should be visible to the operation.
                ldapClientStack.AddDirectoryControls(modifyRequest, new ShowDeletedControl());
                if (controls != null)
                {
                    foreach (DirectoryControl control in controls)
                    {
                        ldapClientStack.AddDirectoryControls(modifyRequest, control);
                    }
                }

                ldapClientStack.SendPacket(modifyRequest);

                #endregion

                #region receive packet

                // Expect response packet
                AdtsLdapPacket response = ldapClientStack.ExpectPacket(timeout);
                if (null == response)
                {
                    throw new Exception(string.Format("Response not received within time for the modify request."));
                }
                if (!(response is AdtsModifyResponsePacket))
                {
                    throw new Exception(string.Format("The response is not an ldap modify response."));
                }

                #endregion

                #region return

                AdtsModifyResponsePacket modifyResponse = (AdtsModifyResponsePacket)response;
                ldapErrorCode = Enum.GetName(
                    typeof(ResultCode),
                    ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.ModifyResponse)
                    modifyResponse.GetInnerRequestOrResponse()).resultCode.Value);

                // if the ldap server is windows, format the error code
                if (isWindows)
                {
                    if (ldapErrorCode.Equals(ResultCode.Success.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return ldapErrorCode + "_STATUS_SUCCESS";
                    }
                    else
                    {
                        windowsErrorCode = ErrorToString(
                            Encoding.ASCII.GetString(
                            ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.ModifyResponse)
                            modifyResponse.GetInnerRequestOrResponse()).errorMessage.ByteArrayValue));

                        return ldapErrorCode + "_" + windowsErrorCode;
                    }
                }
                // if the ldap server is non windows, return ldap error code straightly
                else
                {
                    return ldapErrorCode;
                }

                #endregion
            }
        }


        /// <summary>
        /// [MS-ADTS] section 3.1.1.5.3.7 Undelete Operation
        /// UndeleteModifyObject method undeletes a deleted object in the Active Directory. 
        /// This method uses an extendedcontrol named ShowDeletedControl to find the deleted object DN.
        /// </summary>
        /// <param name="objectDn">The distinguished name of the object to be undeleted.</param>
        /// <param name="newDn">The new distinguished name of the object to be undeleted.</param>
        /// <param name="controls">This parameter specifies the extended controls of this operation</param> 
        /// <returns>Result code of the modify response.</returns>
        public string UndeleteModifyObject(
            string objectDn,
            string newDn,
            DirectoryControl[] controls,
            bool isWindows = true)
        {
            List<DirectoryAttributeModification> attributes = new List<DirectoryAttributeModification>();

            // [MS-ADTS] section 3.1.1.5.3.7 Undelete Operation
            // The undelete operation is identified by the presence of the following attribute LDAPMods (both MUST be present):
            // (1) REMOVE isDeleted attribute
            // (2) REPLACE distinguishedName attribute with a new value
            DirectoryAttributeModification attribute1 = new DirectoryAttributeModification();
            attribute1.Name = "isDeleted";
            attribute1.Operation = DirectoryAttributeOperation.Delete;
            attributes.Add(attribute1);

            DirectoryAttributeModification attribute2 = new DirectoryAttributeModification();
            attribute2.Name = "distinguishedName";
            attribute2.Operation = DirectoryAttributeOperation.Replace;
            attribute2.Add(newDn);
            attributes.Add(attribute2);

            return ModifyObject(objectDn, attributes, controls, isWindows);
        }

        #endregion

        #region LDAP Modify DN

        /// <summary>
        /// IntraDomainModifyDn method renames or moves an object within a Domain.
        /// </summary>
        /// <param name="objectDn">The distinguished name of the object to be moved/renamed.</param>
        /// <param name="newParentDn">The distinguished name of the object's parent to which this object will become child </param>
        /// <param name="newObjectName">The new distinguished name of the object to be moved/renamed.</param>
        /// <param name="deleteOldRDN">Boolean variable specifying whether the old RDN value should be retained."true" if old object is to be deleted</param>
        /// <returns>Result code of the ModifyDn response.</returns>
        // Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public string IntraDomainModifyDn(
            string objectDn,
            string newParentDn,
            string newObjectName,
            bool deleteOldRDN,
            DirectoryControl[] controls,
            bool isWindows = true)
        {
            // if ldapClientStack is null, a connection to the server is not established and endpoint is not binded
            if (null == ldapClientStack)
            {
                throw new Exception(string.Format("A connection to the ldap server is not established and binded."));
            }
            else
            {
                string distinguishedName = objectDn;
                if (distinguishedName != null) distinguishedName = distinguishedName.Trim();

                string newObjectDN = String.Concat(newObjectName, ',', newParentDn);
                testedObjects.Push(string.Format("IntraDomainModifyDn:{0}", newObjectDN));
                try
                {
                    // add new DN
                    objectsInAd.Add(newObjectDN.ToLower(CultureInfo.InvariantCulture), true);
                }
                catch
                {
                    // throw new Exception(string.Format("ObjectDn {0} already exists in active directory", newObjectDN));
                }
                if (distinguishedName != null)
                {
                    try
                    {
                        // remove old DN
                        objectsInAd.Remove(distinguishedName.ToLower(CultureInfo.InvariantCulture));
                    }
                    catch
                    {
                        throw new Exception(string.Format("No ObjectDn {0} found in active directory", distinguishedName));
                    }
                }

                #region send packet and extended controls

                // Create modify request and send packet
                AdtsModifyDnRequestPacket modifyDnRequest = ldapClientStack.CreateModifyDnRequest(
                    distinguishedName,
                    newObjectName,
                    newParentDn,
                    deleteOldRDN);

                // [MS-ADTS] section 3.1.1.5.4.2 Cross Domain Move
                // The Modify DN LDAP request must have LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID control to indicate 
                // that the requester intends to perform a cross-domain move operation.
                // [MS-ADTS] section 3.1.1.3.4.1.14 LDAP_SERVER_SHOW_DELETED_OID
                // The LDAP_SERVER_SHOW_DELETED_OID control is used with an LDAP operation to specify
                // that tombstones and deleted-objects should be visible to the operation.
                // [MS-ADTS] section 3.1.1.3.4.1.26 LDAP_SERVER_SHOW_RECYCLED_OID
                // The LDAP_SERVER_SHOW_RECYCLED_OID control is used with an LDAP operation to specify that 
                // tombstones, deleted-objects, and recycled-objects should be visible to the operation. 
                ldapClientStack.AddDirectoryControls(
                    modifyDnRequest,
                    new ShowDeletedControl());
                if (controls != null)
                {
                    foreach (DirectoryControl control in controls)
                    {
                        ldapClientStack.AddDirectoryControls(modifyDnRequest, control);
                    }
                }

                ldapClientStack.SendPacket(modifyDnRequest);

                #endregion

                #region receive packet

                // Expect response packet
                AdtsLdapPacket response = ldapClientStack.ExpectPacket(timeout);
                if (null == response)
                {
                    throw new Exception(string.Format("Response not received within time for the intra domain modify dn request."));
                }
                if (!(response is AdtsModifyDnResponsePacket))
                {
                    throw new Exception(string.Format("The response is not an ldap intra domain modify dn response."));
                }
                AdtsModifyDnResponsePacket modifyDnResponse = (AdtsModifyDnResponsePacket)response;

                #endregion

                #region return

                ldapErrorCode = Enum.GetName(
                    typeof(ResultCode),
                    ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.ModifyDNResponse)
                    modifyDnResponse.GetInnerRequestOrResponse()).resultCode.Value);

                // if the ldap server is windows, format the error code
                if (isWindows)
                {
                    if (ldapErrorCode.Equals(ResultCode.Success.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return ldapErrorCode + "_STATUS_SUCCESS";
                    }
                    else
                    {
                        windowsErrorCode = ErrorToString(
                            Encoding.ASCII.GetString(
                            ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.ModifyDNResponse)
                            modifyDnResponse.GetInnerRequestOrResponse()).errorMessage.ByteArrayValue));

                        return ldapErrorCode + "_" + windowsErrorCode;
                    }
                }
                // if the ldap server is non windows, return ldap error code straightly
                else
                {
                    return ldapErrorCode;
                }

                #endregion
            }
        }

        /// <summary>
        /// CrossDomainModifyDn method renames or moves an object across the Domains.
        /// </summary>
        /// <param name="objectDn">The distinguished name of an object to be moved/renamed.</param>
        /// <param name="newParentDn">The distinguished name of an object's parent to which this object will become child </param>
        /// <param name="newObjectName">The new distinguished name of an object to be moved/renamed.</param>
        /// <param name="deleteOldRDN"> Boolean value that says whether the old RDN value should be retained. </param>
        /// <param name="targetDomainControllerName">The name of new Domain Controller to which the object is to be moved/renamed.</param>
        /// <param name="controls">This parameter specifies the extended controls of this operation</param> 
        /// <returns>Result code of the ModifyDn response.</returns>
        public string CrossDomainModifyDN(
            string objectDn,
            string newParentDn,
            string newObjectName,
            bool deleteOldRDN,
            string targetDomainControllerName,
            DirectoryControl[] controls,
            bool isWindows = true)
        {
            // if ldapClientStack is null, a connection to the server is not established and endpoint is not binded
            if (null == ldapClientStack)
            {
                throw new Exception(string.Format("A connection to the ldap server is not established and binded."));
            }
            else
            {
                string distinguishedName = objectDn;
                if (distinguishedName != null) distinguishedName = distinguishedName.Trim();

                string newObjectDN = String.Concat(newObjectName, ',', newParentDn);
                testedObjects.Push(string.Format("CrossDomainModifyDn:{0}", newObjectDN));
                try
                {
                    // add new DN
                    objectsInAd.Add(newObjectDN.ToLower(CultureInfo.InvariantCulture), true);
                }
                catch
                {
                    // throw new Exception(string.Format("ObjectDn {0} already exist in active directory", newObjectDN));
                }
                try
                {
                    // remove old DN
                    if (distinguishedName != null)
                    {
                        objectsInAd.Remove(distinguishedName.ToLower(CultureInfo.InvariantCulture));
                    }
                }
                catch
                {
                    throw new Exception(string.Format("No ObjectDn {0} found in active directory", distinguishedName));
                }

                #region send packet and extended controls

                // Create modify request and send packet
                AdtsModifyDnRequestPacket modifyDnRequest = ldapClientStack.CreateModifyDnRequest(
                    distinguishedName,
                    newObjectName,
                    newParentDn,
                    deleteOldRDN);
                // [MS-ADTS] section 3.1.1.5.4.2 Cross Domain Move
                // The Modify DN LDAP request must have LDAP_SERVER_CROSSDOM_MOVE_TARGET_OID control to indicate 
                // that the requester intends to perform a cross-domain move operation.
                // [MS-ADTS] section 3.1.1.3.4.1.14 LDAP_SERVER_SHOW_DELETED_OID
                // The LDAP_SERVER_SHOW_DELETED_OID control is used with an LDAP operation to specify
                // that tombstones and deleted-objects should be visible to the operation.
                // [MS-ADTS] section 3.1.1.3.4.1.26 LDAP_SERVER_SHOW_RECYCLED_OID
                // The LDAP_SERVER_SHOW_RECYCLED_OID control is used with an LDAP operation to specify that 
                // tombstones, deleted-objects, and recycled-objects should be visible to the operation. 
                ldapClientStack.AddDirectoryControls(modifyDnRequest,
                    new CrossDomainMoveControl(targetDomainControllerName),
                    new ShowDeletedControl(),
                    new DirectoryControl(
                        ExtendedControl.LDAP_SERVER_SHOW_RECYCLED_OID,
                        null,
                        false,
                        true));
                if (controls != null)
                {
                    foreach (DirectoryControl control in controls)
                    {
                        ldapClientStack.AddDirectoryControls(modifyDnRequest, control);
                    }
                }

                ldapClientStack.SendPacket(modifyDnRequest);

                #endregion

                #region receive packet

                // Expect response packet
                AdtsLdapPacket response = ldapClientStack.ExpectPacket(timeout);
                if (null == response)
                {
                    throw new Exception(string.Format("Response not received within time for the cross domain modify dn request."));
                }
                if (!(response is AdtsModifyDnResponsePacket))
                {
                    throw new Exception(string.Format("The response is not an ldap cross domain modify dn response."));
                }
                AdtsModifyDnResponsePacket modifyDnResponse = (AdtsModifyDnResponsePacket)response;

                #endregion

                #region return

                ldapErrorCode = Enum.GetName(
                    typeof(ResultCode),
                    ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.ModifyDNResponse)
                    modifyDnResponse.GetInnerRequestOrResponse()).resultCode.Value);

                // if the ldap server is windows, format the error code
                if (isWindows)
                {
                    if (ldapErrorCode.Equals(ResultCode.Success.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return ldapErrorCode + "_STATUS_SUCCESS";
                    }
                    else
                    {
                        windowsErrorCode = ErrorToString(
                            Encoding.ASCII.GetString(
                            ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.ModifyDNResponse)
                            modifyDnResponse.GetInnerRequestOrResponse()).errorMessage.ByteArrayValue));

                        return ldapErrorCode + "_" + windowsErrorCode;
                    }
                }
                // if the ldap server is non windows, return ldap error code straightly
                else
                {
                    return ldapErrorCode;
                }

                #endregion
            }
        }

        #endregion

        #region LDAP Search

        /// <summary>
        /// SearchObject method searches for objects in the Active Directory.
        /// </summary>
        /// <param name="objectDn">The distinguished name of an object to be searched</param>
        /// <param name="searchScope">This parameter specifies the scope of the search to be performed</param>
        /// <param name="filterString">This parameter specifies the search filter in string format</param>
        /// <param name="attributesToReturn">This parameter specifies the list of attributes to be returned from search operation</param>
        /// <param name="controls">This parameter specifies the extended controls of this operation</param>
        /// <param name="searchResultEntries">This parameter is the list of AdtsSearchResultEntryPacket returned from the server</param>
        /// <returns>Result code of the search response</returns>
        public string SearchObject(
            string objectDn,
            System.DirectoryServices.Protocols.SearchScope searchScope,
            string filterString,
            string[] attributesToReturn,
            DirectoryControl[] controls,
            out ICollection<AdtsSearchResultEntryPacket> searchResultEntries,
            bool isWindows = true)
        {
            searchResultEntries = new List<AdtsSearchResultEntryPacket>();

            // if ldapClientStack is null, a connection to the server is not established and endpoint is not binded
            if (null == ldapClientStack)
            {
                throw new Exception(string.Format("A connection to the ldap server is not established and binded."));
            }
            else
            {
                string distinguishedName = objectDn;
                if (distinguishedName != null) distinguishedName = distinguishedName.Trim();

                #region filter

                Filter filter = ParseFilterString(filterString);

                #endregion

                #region send packet and extended controls

                // Create search request and send packet
                AdtsSearchRequestPacket searchRequest = ldapClientStack.CreateSearchRequest(
                    distinguishedName,
                    //No size limit for search response
                    0,
                    //No time limit of search, in seconds, before DC returns an timeLimitExceeded error.     
                    0,
                    searchScope,
                    System.DirectoryServices.Protocols.DereferenceAlias.Never,
                    filter,
                    false,
                    attributesToReturn);

                if (null != controls)
                {
                    foreach (DirectoryControl control in controls)
                    {
                        ldapClientStack.AddDirectoryControls(searchRequest, control);
                    }
                }
                ldapClientStack.SendPacket(searchRequest);

                #endregion

                #region receive packet

                // Expect response packet
                Stack<AdtsLdapPacket> searchResultList = new Stack<AdtsLdapPacket>();
                while (true)
                {
                    AdtsLdapPacket response = ldapClientStack.ExpectPacket(timeout);
                    if (response is AdtsSearchResultEntryPacket)
                    {
                        searchResultList.Push(response);
                    }
                    else if (response is AdtsSearchResultDonePacket)
                    {
                        searchResultList.Push(response);
                        break;
                    }
                }

                AdtsSearchResultDonePacket searchResultDonePacket = (AdtsSearchResultDonePacket)searchResultList.Pop();
                while (searchResultList.Count > 0)
                {
                    searchResultEntries.Add((AdtsSearchResultEntryPacket)searchResultList.Pop());
                }

                #endregion

                #region return

                ldapErrorCode = Enum.GetName(
                    typeof(ResultCode),
                    ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultDone)
                    searchResultDonePacket.GetInnerRequestOrResponse()).resultCode.Value);

                // if the ldap server is windows, format the error code
                if (isWindows)
                {
                    if (ldapErrorCode.Equals(ResultCode.Success.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return ldapErrorCode + "_STATUS_SUCCESS";
                    }
                    else
                    {
                        windowsErrorCode = ErrorToString(
                            Encoding.ASCII.GetString(
                            ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultDone)
                            searchResultDonePacket.GetInnerRequestOrResponse()).errorMessage.ByteArrayValue));

                        return ldapErrorCode + "_" + windowsErrorCode;
                    }
                }
                // if the ldap server is non windows, return ldap error code straightly
                else
                {
                    return ldapErrorCode;
                }

                #endregion
            }
        }

        #endregion

        #region LDAP Delete

        /// <summary>
        /// [MS-ADTS] section 3.1.1.5.5 Delete Operation
        /// The delete operation results in the transformation of an existing-object in the directory tree into some form of deleted object.
        /// </summary>
        /// <param name="objectDn">The distinguishedName of the object in the directory to be deleted</param>
        /// <param name="controls">This parameter specifies the extended controls of this operation</param>
        /// <returns>Result code of the add response</returns>
        public string DeleteObject(
            string objectDn,
            DirectoryControl[] controls,
            bool isWindows = true)
        {
            // if ldapClientStack is null, a connection to the server is not established and endpoint is not binded
            if (null == ldapClientStack)
            {
                throw new Exception(string.Format("A connection to the ldap server is not established and binded."));
            }
            else
            {
                string distinguishedName = objectDn;
                if (distinguishedName != null) distinguishedName = distinguishedName.Trim();

                // this object brings trouble for env
                if (distinguishedName.ToLower(CultureInfo.InvariantCulture)
                    .Contains("CN=Password Settings Container,".ToLower(CultureInfo.InvariantCulture)))
                    return "Success_STATUS_SUCCESS";
                else if (distinguishedName.ToLower(CultureInfo.InvariantCulture)
                    .Contains("CN=newappnc,".ToLower(CultureInfo.InvariantCulture)))
                    return "Success_STATUS_SUCCESS";

                testedObjects.Push(string.Format("Delete:{0}", distinguishedName));
                try
                {
                    objectsInAd.Remove(distinguishedName.ToLower(CultureInfo.InvariantCulture));
                }
                catch
                {
                    throw new Exception(string.Format("ObjectDn {0} not found in active directory", distinguishedName));
                }

                #region send packet and controls

                // Create add request and send packet
                AdtsDelRequestPacket deleteRequest = ldapClientStack.CreateDelRequest(
                    distinguishedName);

                if (controls != null)
                {
                    foreach (DirectoryControl control in controls)
                    {
                        ldapClientStack.AddDirectoryControls(deleteRequest, control);
                    }
                }
                ldapClientStack.SendPacket(deleteRequest);

                #endregion

                #region receive packet

                // Expect response packet
                AdtsLdapPacket response = ldapClientStack.ExpectPacket(timeout);
                if (null == response)
                {
                    throw new Exception(string.Format("Response not received within time for the delete request."));
                }
                if (!(response is AdtsDelResponsePacket))
                {
                    throw new Exception(string.Format("The response is not an ldap delete response."));
                }

                #endregion

                #region return

                AdtsDelResponsePacket deleteResponse = (AdtsDelResponsePacket)response;
                ldapErrorCode = Enum.GetName(
                    typeof(ResultCode),
                    ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.DelResponse)
                    deleteResponse.GetInnerRequestOrResponse()).resultCode.Value);

                // if the ldap server is windows, format the error code
                if (isWindows)
                {
                    if (ldapErrorCode.Equals(ResultCode.Success.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return ldapErrorCode + "_STATUS_SUCCESS";
                    }
                    else
                    {
                        windowsErrorCode = ErrorToString(
                            Encoding.ASCII.GetString(
                            ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.DelResponse)
                            deleteResponse.GetInnerRequestOrResponse()).errorMessage.ByteArrayValue));

                        // [MS-ADTS] section 3.1.1.3.1.2.1 Naming Attributes
                        // The server will reject an attempt to create such a non-uniquely named object with the error 
                        // entryAlreadyExists / <unrestricted>. This requirement for unique AttributeValues guarantees 
                        // the uniqueness of canonical names.
                        if (ldapErrorCode.Equals(ResultCode.EntryAlreadyExists.ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            return "EntryAlreadyExists_UnKnownError";
                        }

                        return ldapErrorCode + "_" + windowsErrorCode;
                    }
                }
                // if the ldap server is non windows, return ldap error code straightly
                else
                {
                    return ldapErrorCode;
                }

                #endregion
            }
        }

        #endregion

        #region LDAP Extended Controls

        /// <summary>
        /// Performs PageWise Search
        /// [MS-ADTS] section 3.1.1.3.4.1.1 LDAP_PAGED_RESULT_OID_STRING
        /// This control, which is used as both a request control and a response control, is documented in [RFC2696].
        /// </summary>
        /// <param name="pageSize">The number of entries per page</param>
        /// <param name="objectDn">The distinguished name of an object to be searched</param>
        /// <param name="searchScope">This parameter specifies the scope of the search to be performed</param>
        /// <param name="filterString">This parameter specifies the names of the search filter and their matching rules and values</param>
        /// <param name="attributesToReturn">This parameter specifies the list of attributes to be returned from search operation</param>
        /// <param name="searchResultEntries">This parameter is the list of AdtsSearchResultEntryPacket returned from the server</param>
        /// <returns>Result code of the search response</returns>
        public string PageRequestControl(
            int pageSize,
            string objectDn,
            System.DirectoryServices.Protocols.SearchScope searchScope,
            string filterString,
            string[] attributesToReturn,
            out ICollection<AdtsSearchResultEntryPacket> searchResultEntries,
            bool isWindows = true)
        {
            PageResultRequestControl pageRequest = new PageResultRequestControl(pageSize);
            DirectoryControl[] controls = new DirectoryControl[] { pageRequest };
            return SearchObject(
                objectDn,
                searchScope,
                filterString,
                attributesToReturn,
                controls,
                out searchResultEntries,
                isWindows);
        }

        /// <summary>
        /// Returns Result code of the DirectorySync ExtendedControl Operation
        /// [MS-ADTS] section 3.1.1.3.4.1.3 LDAP_SERVER_DIRSYNC_OID
        /// The LDAP_SERVER_DIRSYNC_OID control is used with an LDAP search operation to retrieve the changes 
        /// made to objects since a previous search with an LDAP_SERVER_DIRSYNC_OID control was performed.
        /// </summary>
        /// <param name="syncOptions">Enum defined in System.DirectoryServices.Protocols namespace</param>
        /// <param name="objectDn">The distinguished name of an object to be searched</param>
        /// <param name="searchScope">This parameter specifies the scope of the search to be performed</param>
        /// <param name="filterString">This parameter specifies the names of the search filter and their matching rules and values</param>
        /// <param name="attributesToReturn">This parameter specifies the list of attributes to be returned from search operation</param>
        /// <param name="searchResultEntries">This parameter is the list of AdtsSearchResultEntryPacket returned from the server</param>
        /// <returns>Result code of the search response</returns>
        /// Disable CA1011, it is require more derived type in this method 
        /// Disable warning CA1500 the value of the parameter serverHostName is always the same.
        [SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public string DirectorySyncRequestControl(
            System.DirectoryServices.Protocols.DirectorySynchronizationOptions syncOptions,
            string objectDn,
            System.DirectoryServices.Protocols.SearchScope searchScope,
            string filterString,
            string[] attributesToReturn,
            out ICollection<AdtsSearchResultEntryPacket> searchResultEntries,
            bool isWindows = true)
        {
            DirSyncRequestControl dirSyncRequest = new DirSyncRequestControl(null, syncOptions);
            DirectoryControl[] controls = new DirectoryControl[] { dirSyncRequest };
            return SearchObject(
                objectDn,
                searchScope,
                filterString,
                attributesToReturn,
                controls,
                out searchResultEntries,
                isWindows);
        }

        /// <summary>
        /// Returns Result Code of RangeOption Extended Control
        /// [MS-ADTS] section 3.1.1.3.4.1.10 LDAP_SERVER_RANGE_OPTION_OID
        /// The presence of this OID indicates that the DC supports range retrieval of multivalued attributes.
        /// </summary>
        /// <returns>Result code of the search response.</returns>
        /// <param name="objectDn">The distinguished name of an object to be searched</param>
        /// <param name="searchScope">This parameter specifies the scope of the search to be performed</param>
        /// <param name="filterString">This parameter specifies the names of the search filter and their matching rules and values</param>
        /// <param name="attributesToReturn">This parameter specifies the list of attributes to be returned from search operation</param>
        /// <param name="searchResultEntries">This parameter is the list of AdtsSearchResultEntryPacket returned from the server</param>
        /// <returns>Result code of the search response</returns>
        public string RangeOptionControl(
            string objectDn,
            System.DirectoryServices.Protocols.SearchScope searchScope,
            string filterString,
            string[] attributesToReturn,
            out ICollection<AdtsSearchResultEntryPacket> searchResultEntries,
            bool isWindows = true)
        {
            DirectoryControl rangeOption = new DirectoryControl(ExtendedControl.LDAP_SERVER_RANGE_OPTION_OID, null, false, true);
            DirectoryControl[] controls = new DirectoryControl[] { rangeOption };
            return SearchObject(
                objectDn,
                searchScope,
                filterString,
                attributesToReturn,
                controls,
                out searchResultEntries,
                isWindows);
        }

        /// <summary>
        /// Extracts Different parts of Security Descriptor
        /// [MS-ADTS] section 3.1.1.3.4.1.11 LDAP_SERVER_SD_FLAGS_OID
        /// The LDAP_SERVER_SD_FLAGS_OID control is used with an LDAP Search request to control the portion of a Windows Security Descriptor to retrieve.
        /// </summary>
        /// <param name="securityMasks">Enum Defined in System.DirectoryServices.Protocols namespace</param>
        /// <param name="objectDn">The distinguished name of an object to be searched</param>
        /// <param name="searchScope">This parameter specifies the scope of the search to be performed</param>
        /// <param name="filterString">This parameter specifies the names of the search filter and their matching rules and values</param>
        /// <param name="attributesToReturn">This parameter specifies the list of attributes to be returned from search operation</param>
        /// <param name="searchResultEntries">This parameter is the list of AdtsSearchResultEntryPacket returned from the server</param>
        /// <returns>Result code of the search response</returns>
        public string SecurityDescriptorFlagsControl(
            System.DirectoryServices.Protocols.SecurityMasks securityMasks,
            string objectDn,
            System.DirectoryServices.Protocols.SearchScope searchScope,
            string filterString,
            string[] attributesToReturn,
            out ICollection<AdtsSearchResultEntryPacket> searchResultEntries,
            bool isWindows = true)
        {
            SecurityDescriptorFlagControl sdControl = new SecurityDescriptorFlagControl(securityMasks);
            DirectoryControl[] controls = new DirectoryControl[] { sdControl };
            return SearchObject(
                objectDn,
                searchScope,
                filterString,
                attributesToReturn,
                controls,
                out searchResultEntries,
                isWindows);
        }

        /// <summary>
        /// Delete Tree Control method deletes a tree of objects from the Active Directory.
        /// [MS-ADTS] section 3.1.1.3.4.1.15 LDAP_SERVER_TREE_DELETE_OID
        /// The LDAP_SERVER_TREE_DELETE_OID control is used with an LDAP delete operation to cause the server 
        /// to recursively delete the entire subtree of objects located underneath the object specified in the delete operation.
        /// </summary>
        /// <param name="objectDn">The distinguished name of a root object to be deleted.</param>
        /// <returns>Result code of the delete response.</returns>
        public string DeleteTreeControl(
            string objectDn,
            bool isWindows = true)
        {
            TreeDeleteControl tdControl = new TreeDeleteControl();
            DirectoryControl[] controls = new DirectoryControl[] { tdControl };
            return DeleteObject(
                objectDn,
                controls,
                isWindows);
        }

        /// <summary>
        /// Performs VirtualListView Operation
        /// [MS-ADTS] section 3.1.1.3.4.1.17 LDAP_CONTROL_VLVREQUEST and LDAP_CONTROL_VLVRESPONSE
        /// The LDAP_CONTROL_VLVREQUEST control is used with an LDAP search operation to retrieve a subset of the objects that satisfy the search request. 
        /// </summary>
        /// <param name="sortAttribute">The attribute name to be sorted for the search objects</param>
        /// <param name="isReverseOrder">If sort is in reverse order</param>
        /// <param name="before">number of objects before the target object</param>
        /// <param name="after">number of objects after the target objects</param>
        /// <param name="valueToSearch">The objects to be searched. Example:adts*,normaluser</param>
        /// <param name="objectDn">The distinguished name of an object to be searched</param>
        /// <param name="searchScope">This parameter specifies the scope of the search to be performed</param>
        /// <param name="filterString">This parameter specifies the names of the search filter and their matching rules and values</param>
        /// <param name="attributesToReturn">This parameter specifies the list of attributes to be returned from search operation</param>
        /// <param name="searchResultEntries">This parameter is the list of AdtsSearchResultEntryPacket returned from the server</param>
        /// <returns>Result code of the search response</returns>
        public string VirtualListViewControl(
            string sortAttribute,
            bool isReverseOrder,
            int before,
            int after,
            string valueToSearch,
            string objectDn,
            System.DirectoryServices.Protocols.SearchScope searchScope,
            string filterString,
            string[] attributesToReturn,
            out ICollection<AdtsSearchResultEntryPacket> searchResultEntries,
            bool isWindows = true)
        {
            SortRequestControl sortRequest = new SortRequestControl(sortAttribute, isReverseOrder);
            VlvRequestControl vlvRequest = new VlvRequestControl(before, after, valueToSearch);
            DirectoryControl[] controls = new DirectoryControl[] { sortRequest, vlvRequest };
            return SearchObject(
                objectDn,
                searchScope,
                filterString,
                attributesToReturn,
                controls,
                out searchResultEntries,
                isWindows);
        }

        /// <summary>
        /// Performs Attribute Scoped Query
        /// [MS-ADTS] section 3.1.1.3.4.1.18 LDAP_SERVER_ASQ_OID
        /// The LDAP_SERVER_ASQ_OID control is used with an LDAP search operation. When this control is used,
        /// the search is not performed against the object specified in the search, or the objects located 
        /// underneath that object, but rather against the set of objects named by an attribute of Object(DS-DN) 
        /// syntax that is located on the object specified by the base DN of the search request
        /// </summary>
        /// <param name="attributeName">The name of the attribute on which AttributeScoped Query needs to be performed</param>
        /// <param name="objectDn">The distinguished name of an object to be searched</param>
        /// <param name="searchScope">This parameter specifies the scope of the search to be performed</param>
        /// <param name="filterString">This parameter specifies the names of the search filter and their matching rules and values</param>
        /// <param name="attributesToReturn">This parameter specifies the list of attributes to be returned from search operation</param>
        /// <param name="searchResultEntries">This parameter is the list of AdtsSearchResultEntryPacket returned from the server</param>
        /// <returns>Result code of the search response</returns>
        public string ASQQuery(
            string attributeName,
            string objectDn,
            System.DirectoryServices.Protocols.SearchScope searchScope,
            string filterString,
            string[] attributesToReturn,
            out ICollection<AdtsSearchResultEntryPacket> searchResultEntries,
            bool isWindows = true)
        {
            AsqRequestControl asqRequest = new AsqRequestControl(attributeName);
            DirectoryControl[] controls = new DirectoryControl[] { asqRequest };
            return SearchObject(
                objectDn,
                searchScope,
                filterString,
                attributesToReturn,
                controls,
                out searchResultEntries,
                isWindows);
        }

        #endregion

        #region LDAP Extended Operations

        /// <summary>
        /// Performs an LDAP Extended Operation
        /// </summary>
        /// <param name="requestName">OID of the extended operation</param>
        /// <param name="requestValue">null</param>
        /// <returns>Result code of the extended operations response.</returns>
        public Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.ExtendedResponse ExtendedOperations(
            string requestName,
            byte[] requestValue)
        {
            // if ldapClientStack is null, a connection to the server is not established and endpoint is not binded
            if (null == ldapClientStack)
            {
                throw new Exception(string.Format("A connection to the ldap server is not established and binded."));
            }
            else
            {
                #region send and receive

                // Create extended operation request and send packet
                AdtsExtendedRequestPacket extendedRequest = ldapClientStack.CreateExtendedRequest(
                    requestName,
                    requestValue);
                ldapClientStack.SendPacket(extendedRequest);

                // Receive extended operation response
                AdtsLdapPacket response = ldapClientStack.ExpectPacket(timeout);
                if (null == response)
                {
                    throw new Exception(string.Format("Response not received within time for the delete request."));
                }
                if (!(response is AdtsExtendedResponsePacket))
                {
                    throw new Exception(string.Format("The response is not an ldap delete response."));
                }

                #endregion

                #region return

                AdtsExtendedResponsePacket extendedResponse = (AdtsExtendedResponsePacket)response;
                return ((Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.ExtendedResponse)
                    extendedResponse.GetInnerRequestOrResponse());

                #endregion
            }
        }

        #endregion

        #region LDAP Policies

        /// <summary>
        /// The method is used to update the value of ldap polices
        /// [MS-ADTS] section 3.1.1.3.4.6 LDAP Policies
        /// The maximum number of objects that are returned in a single search result, independent of how 
        /// large each returned object is. To perform a search where the result might exceed this number of 
        /// objects, the client must specify the paged search control.
        /// </summary>
        /// <param name="policies">The polices to be updated</param>
        /// <param name="newValues">The new values of these polices</param>
        /// <returns>Result code of the modify ldap policies response.</returns>
        public string ModifyLdapPolices(
            string serverName,
            string namingContext,
            LdapPolicy[] policies,
            string[] newValues,
            bool isWindows = true)
        {
            // if ldapClientStack is null, a connection to the server is not established and endpoint is not binded
            if (null == ldapClientStack)
            {
                throw new Exception(string.Format("A connection to the ldap server is not established and binded."));
            }
            else
            {
                // [MS-ADTS] section 3.1.1.3.4.6 LDAP Policies
                // There can be multiple queryPolicy objects in a forest. A DC determines the queryPolicy object that contains its 
                // policies according to the following logic:
                // (1) If the queryPolicyObject attribute is present on the DC's nTDSDSA object, the DC uses the queryPolicy object referenced by it.
                // (2) Otherwise, if the queryPolicyObject attribute is present on the nTDSSiteSettings object for the site to which the 
                //     DC belongs, the DC uses the queryPolicy object referenced by it.
                // (3) Otherwise, the DC uses the queryPolicy object whose DN is "CN=Default Query Policy,CN=Query-Policies" relative to 
                //     the nTDSService object (for example, "CN=Default Query Policy, CN=Query-Policies, CN=Directory Service, CN=Windows NT, CN=Services" 
                //     relative to the root of the config NC).
                string queryPolicyDn = string.Concat(
                    "CN=Default Query Policy,CN=Query-Policies,CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration,",
                    namingContext);

                ICollection<AdtsSearchResultEntryPacket> searchResultEntries;
                string[] attributeValues;

                #region (1) Get queryPolicyObject attribute from nTDSDSA object

                string nTDSDSADn = string.Format(
                    "CN=NTDS Settings,CN={0},CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,{1}",
                    serverName,
                    namingContext);

                string result = SearchObject(
                    nTDSDSADn,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "(objectclass=nTDSDSA)",
                    new string[] { "queryPolicyObject" },
                    null,
                    out searchResultEntries);

                // searchResultEntries should only contain 1 entry packet
                if (result.Contains("Success")
                    || searchResultEntries.Count == 1)
                {
                    attributeValues = GetAttributeValuesInString(searchResultEntries.ElementAt(0), "queryPolicyObject");
                    // attributeValues should only contain no more than 1 attribute value
                    if (null != attributeValues)
                    {
                        queryPolicyDn = attributeValues[0];
                    }
                }

                #endregion

                #region (2) Get queryPolicyObject attribute from nTDSSiteSettings object

                string nTDSSiteSettingsDn = string.Format(
                    "CN=NTDS Site Settings,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,{0}",
                    namingContext);
                result = SearchObject(
                    nTDSSiteSettingsDn,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "(objectclass=nTDSSiteSettings)",
                    new string[] { "queryPolicyObject" },
                    null,
                    out searchResultEntries);

                // searchResultEntries should only contain 1 entry packet
                if (result.Contains("Success")
                    || searchResultEntries.Count == 1)
                {
                    attributeValues = GetAttributeValuesInString(searchResultEntries.ElementAt(0), "queryPolicyObject");
                    // attributeValues should only contain no more than 1 attribute value
                    if (null != attributeValues)
                    {
                        queryPolicyDn = attributeValues[0];
                    }
                }

                #endregion

                #region Get lDAPAdminLimits Attribute from the queryPolicy object

                result = SearchObject(
                    queryPolicyDn,
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "(objectclass=queryPolicy)",
                    new string[] { "lDAPAdminLimits" },
                    null,
                    out searchResultEntries);

                // searchResultEntries should only contain 1 entry packet
                Dictionary<LdapPolicy, string> attrDictionary = new Dictionary<LdapPolicy, string>();
                if (result.Contains("Success")
                    || searchResultEntries.Count == 1)
                {
                    attributeValues = GetAttributeValuesInString(searchResultEntries.ElementAt(0), "lDAPAdminLimits");
                    // attributeValues should contain more than 1 attribute value
                    if (null != attributeValues)
                    {
                        foreach (string attributeValue in attributeValues)
                        {
                            string[] elements = attributeValue.Split('=');
                            attrDictionary.Add((LdapPolicy)Enum.Parse(typeof(LdapPolicy), elements[0], true), elements[1]);
                        }
                    }
                }

                #endregion

                #region Modify lDAPAdminLimits Attribute from the queryPolicy object

                List<DirectoryAttributeModification> attributes = new List<DirectoryAttributeModification>();
                DirectoryAttributeModification attr = new DirectoryAttributeModification();
                attr.Name = "lDAPAdminLimits";
                attr.Operation = DirectoryAttributeOperation.Replace;
                // replace all the policies with their new values
                for (int i = 0; i < policies.Length; i++)
                {
                    attrDictionary[policies[i]] = newValues[i];
                }
                // form the new query policy object lDAPAdminLimits attribute value
                for (int i = 0; i < attrDictionary.Count; i++)
                {
                    attr.Add(
                        string.Concat(
                        Enum.GetName(typeof(LdapPolicy), attrDictionary.ElementAt(i).Key),
                        "=",
                        attrDictionary.ElementAt(i).Value));
                }
                attributes.Add(attr);
                result = ModifyObject(
                    queryPolicyDn,
                    attributes,
                    null
                    );

                return result;

                #endregion
            }
        }

        #endregion

        #region LDAP rootDSE Modify Operations

        /// <summary>
        /// Search RootDSE
        /// </summary>
        /// <param name="rootDSEValues">The rootDSE attributes retreived</param>
        /// <param name="isWindows">whether the server is windows or not</param>
        public string SearchRootDSEValues(
            out Dictionary<string, string> rootDSEValues,
            bool isWindows = true)
        {
            string result = string.Empty;

            #region Generate RootDSE Attribute Dictionary

            rootDSEValues = new Dictionary<string, string>();
            rootDSEValues.Add(RootDSEAttribute.configurationNamingContext, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.currentTime, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.defaultNamingContext, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.dNSHostName, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.dsSchemaAttrCount, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.dsSchemaClassCount, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.dsSchemaPrefixCount, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.dsServiceName, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.highestCommittedUSN, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.isGlobalCatalogReady, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.isSynchronized, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.ldapServiceName, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.namingContexts, string.Empty);
            // netlogon: ldap search for this attribute will not return searchResultEntries, resolved directly as LDAP ping
            // rootDSErootDSEValues.Add(RootDSEAttribute.netlogon, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.pendingPropagations, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.rootDomainNamingContext, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.schemaNamingContext, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.serverName, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.subschemaSubentry, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.supportedCapabilities, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.supportedControl, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.supportedLDAPPolicies, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.supportedLDAPVersion, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.supportedSASLMechanisms, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.domainControllerFunctionality, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.domainFunctionality, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.forestFunctionality, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.msDS_ReplAllInboundNeighbors, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.msDS_ReplAllOutboundNeighbors, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.msDS_ReplConnectionFailures, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.msDS_ReplLinkFailures, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.msDS_ReplPendingOps, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.msDS_ReplQueueStatistics, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.msDS_TopQuotaUsage, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.supportedConfigurableSettings, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.supportedExtension, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.validFSMOs, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.dsaVersionString, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.msDS_PortLDAP, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.msDS_PortSSL, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.msDS_PrincipalName, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.serviceAccountInfo, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.spnRegistrationResult, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.tokenGroups, string.Empty);
            rootDSEValues.Add(RootDSEAttribute.usnAtRifm, string.Empty);

            #endregion

            // if ldapClientStack is null, a connection to the server is not established and endpoint is not binded
            if (null == ldapClientStack)
            {
                throw new Exception(string.Format("A connection to the ldap server is not established and binded."));
            }
            else
            {
                string[] attributesToReturn = rootDSEValues.Keys.ToArray();

                // [MS-ADTS] section 3.1.1.3.2 rootDSE Attributes
                // When the attribute is operational, the server returns the attribute only when it is explicitly requested.
                ICollection<AdtsSearchResultEntryPacket> searchResultEntries = new List<AdtsSearchResultEntryPacket>();
                result = SearchObject(
                    null,
                    System.DirectoryServices.Protocols.SearchScope.Base,
                    "(objectClass=*)",
                    attributesToReturn,
                    null,
                    out searchResultEntries,
                    isWindows);

                if (result.Contains(Enum.GetName(typeof(ResultCode), ResultCode.Success))
                    || searchResultEntries.Count >= 1)
                {
                    foreach (AdtsSearchResultEntryPacket searchResultEntry in searchResultEntries)
                    {
                        foreach (string attributeName in attributesToReturn)
                        {
                            string[] attributeValues = GetAttributeValuesInString(searchResultEntry, attributeName);
                            if (null != attributeValues)
                            {
                                string attributeValue;
                                if (attributeValues.Length > 1)
                                {
                                    attributeValue = string.Join(";", attributeValues);
                                }
                                else
                                {
                                    attributeValue = attributeValues[0];
                                }
                                rootDSEValues[attributeName] = attributeValue;
                            }
                            else
                            {
                                rootDSEValues[attributeName] = null;
                            }
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}