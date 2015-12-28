// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory
{
    /// <summary>
    /// Load claims
    /// </summary>
    public class ClaimsLoader
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dc">DC name or address</param>
        /// <param name="domain">domain DNS name</param>
        /// <param name="user">user name for LDAP connection</param>
        /// <param name="password">password of user</param>
        public ClaimsLoader(string dc, string domain, string user, string password)
        {
            DomainController = dc;
            DomainDNSName = domain;
            UserName = user;
            Password = password;
        }

        /// <summary>
        /// user name for connection
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// password for user
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// DNS name of the domain
        /// </summary>
        public string DomainDNSName
        {
            get
            {
                return domainDNS;
            }
            set
            {
                domainDNS = value;
                domainNC = dnsNameToDN(value);
            }
        }

        /// <summary>
        /// machine name of the DC
        /// </summary>
        public string DomainController
        {
            get;
            set;
        }

        /// <summary>
        /// domain DNS name
        /// </summary>
        string domainDNS;

        /// <summary>
        /// domain NC Distinguished Name
        /// </summary>
        string domainNC;

        /// <summary>
        /// Get Claims for provided principal without encode
        /// </summary>
        /// <param name="principal">Distinguished Name of the principal</param>
        /// <param name="principalClass">class of principal, user or device</param>
        /// <param name="source">claim source type, AD or Certificate</param>
        /// <returns>a list of CLAIMS_ARRAY for the principal</returns>
        public List<CLAIMS_ARRAY> GetClaimsForPrincipalWithoutEncode(string principal, ClaimsPrincipalClass principalClass, ClaimsSource source)
        {
            List<CLAIMS_ARRAY> ret = new List<CLAIMS_ARRAY>();

            #region get principal object
            using (DirectoryEntry princ = new DirectoryEntry("LDAP://" + principal,
                domainDNS + "\\" + UserName, Password, AuthenticationTypes.Secure))
            {

            #endregion
                #region AD source
                if (source.HasFlag(ClaimsSource.AD))
                {
                    CLAIMS_ARRAY ad = getADSoucredClaims(princ, principalClass);
                    ret.Add(ad);
                    //Constructed claims use the CLAIMS_SOURCE_TYPE_AD source type
                    ad = getConstructedClaims(princ, principalClass);
                    ret.Add(ad);
                }
                #endregion


                #region certificate source
                //not implemented
                #endregion

                return ret;
            }
        }

        /// <summary>
        /// validate claim definition
        /// </summary>
        /// <param name="root">claim to validate</param>
        /// <returns>true if it's OK</returns>
        bool validateClaimDefinition(DirectoryEntry claim)
        {

            CLAIM_TYPE valueType = getClaimValueType(claim.Properties[ConstValue.distinguishedname].Value.ToString(), DomainController);

            PropertyValueCollection sType = claim.Properties[ConstValue.msDSClaimSourceType];
            if (sType == null || sType.Value == null)
                return false;

            switch ((ClaimsSourceType)Enum.Parse(typeof(ClaimsSourceType), sType.Value.ToString(), true))
            {
                case ClaimsSourceType.AD:
                    {
                        #region ad sourced claim
                        PropertyValueCollection attrSrc = claim.Properties[ConstValue.msDSClaimAttributeSource];
                        if (attrSrc == null || attrSrc.Value == null)
                            return false;

                        string srcDN = attrSrc.Value.ToString();

                        using (DirectoryEntry srcEntry = new DirectoryEntry("LDAP://" + srcDN))
                        {

                            int syntax = (int)srcEntry.Properties["oMSyntax"].Value;
                            bool syntaxPassed = false;
                            switch (syntax)
                            {
                                case 127:
                                    if (valueType == CLAIM_TYPE.CLAIM_TYPE_STRING)
                                        syntaxPassed = true;
                                    break;
                                case 6:
                                    if (valueType == CLAIM_TYPE.CLAIM_TYPE_UINT64)
                                        syntaxPassed = true;
                                    break;
                                case 1:
                                    if (valueType == CLAIM_TYPE.CLAIM_TYPE_BOOLEAN)
                                        syntaxPassed = true;
                                    break;
                                case 2:
                                    if (valueType == CLAIM_TYPE.CLAIM_TYPE_INT64)
                                        syntaxPassed = true;
                                    break;
                                case 10:
                                    if (valueType == CLAIM_TYPE.CLAIM_TYPE_INT64)
                                        syntaxPassed = true;
                                    break;
                                case 64:
                                    if (valueType == CLAIM_TYPE.CLAIM_TYPE_STRING)
                                        syntaxPassed = true;
                                    break;
                                case 66:
                                    if (valueType == CLAIM_TYPE.CLAIM_TYPE_STRING)
                                        syntaxPassed = true;
                                    break;
                                case 65:
                                    if (valueType == CLAIM_TYPE.CLAIM_TYPE_INT64)
                                        syntaxPassed = true;
                                    break;
                                default:
                                    break;


                            }
                            if (syntaxPassed)
                                return true;

                        #endregion
                        }


                        break;
                    }
                case ClaimsSourceType.Certificate:
                    {
                        #region certificate sourced claim
                        PropertyValueCollection attrSrc = claim.Properties[ConstValue.msDSClaimAttributeSource];
                        PropertyValueCollection source = claim.Properties[ConstValue.msDsClaimSource];
                        if ((valueType == CLAIM_TYPE.CLAIM_TYPE_BOOLEAN) && ((attrSrc == null) || (attrSrc.Value == null))
                            && (source != null) && (source.Value != null))
                        {
                            return true;
                        }
                        #endregion
                    }
                    break;
                case ClaimsSourceType.TransformPolicy:
                    {
                        #region transform policy
                        PropertyValueCollection attrSrc = claim.Properties[ConstValue.msDSClaimAttributeSource];
                        PropertyValueCollection source = claim.Properties[ConstValue.msDsClaimSource];
                        if (((attrSrc == null) || (attrSrc.Value == null))
                            && ((source == null) || (source.Value == null)))
                        {
                            return true;
                        }
                        #endregion
                    }
                    break;
                case ClaimsSourceType.Constructed:
                    {
                        #region Constructed
                        //Constructed claims are generated dynamically according to a claim-specific algorithm
                        PropertyValueCollection attrSrc = claim.Properties[ConstValue.msDSClaimAttributeSource];
                        PropertyValueCollection source = claim.Properties[ConstValue.msDsClaimSource];
                        if (((attrSrc == null) || (attrSrc.Value == null))
                            && ((source == null) || (source.Value == null)))
                        {
                            return true;
                        }
                        #endregion
                    }
                    break;
                default:
                    return false;
            }


            return false;
        }

        /// <summary>
        /// read msDS-ClaimValueType of a claim from DC
        /// </summary>
        /// <param name="dn">Distinguished Name of claim</param>
        /// <param name="server">DC name or address</param>
        /// <returns>CLAIM_TYPE</returns>
        CLAIM_TYPE getClaimValueType(string dn, string server)
        {
            using (System.DirectoryServices.Protocols.LdapConnection con = new System.DirectoryServices.Protocols.LdapConnection(server))
            {
                System.DirectoryServices.Protocols.SearchRequest req = new System.DirectoryServices.Protocols.SearchRequest(
                    dn,
                    "(objectclass=*)",
                     System.DirectoryServices.Protocols.SearchScope.Base,
                     new string[] { ConstValue.msDSClaimValueType });

                System.DirectoryServices.Protocols.SearchResponse res = (System.DirectoryServices.Protocols.SearchResponse)con.SendRequest(req);

                object o = res.Entries[0].Attributes[ConstValue.msDSClaimValueType][0];

                return (CLAIM_TYPE)Enum.Parse(typeof(CLAIM_TYPE), o.ToString());
            }
        }

        /// <summary>
        /// get AD sourced claims for a principal
        /// </summary>
        /// <param name="principal">the target principal</param>
        /// <param name="principalClass">user or device</param>
        /// <returns>a CLAIMS_ARRAY contains claims of the principal</returns>
        CLAIMS_ARRAY getADSoucredClaims(DirectoryEntry principal, ClaimsPrincipalClass principalClass)
        {
            CLAIMS_ARRAY ret = new CLAIMS_ARRAY();

            using (DirectoryEntry root = new DirectoryEntry(ConstValue.claimTypesPath + domainNC))
            {

                List<CLAIM_ENTRY> claims = new List<CLAIM_ENTRY>();

                DirectoryEntries children = root.Children;

                foreach (DirectoryEntry de in children)
                {
                    //source type should be AD
                    if ((!de.Properties.Contains(ConstValue.msDSClaimSourceType))
                        || ((ClaimsSourceType)Enum.Parse(typeof(ClaimsSourceType), de.Properties[ConstValue.msDSClaimSourceType].Value.ToString(), true) != ClaimsSourceType.AD)
                        || (!de.Properties.Contains(ConstValue.msDSClaimTypeAppliesToClass)))
                        continue;

                    //should applies to this principal class
                    bool classMatched = false;
                    foreach (object str in de.Properties[ConstValue.msDSClaimTypeAppliesToClass])
                    {
                        string tmp = str.ToString();
                        if (tmp.ToLower() == (ConstValue.userRDN + domainNC.ToLower()) && principalClass == ClaimsPrincipalClass.User)
                        {
                            classMatched = true;
                            break;
                        }

                        if ((tmp.ToLower() == (ConstValue.computerRDN + domainNC).ToLower()) && principalClass == ClaimsPrincipalClass.Device)
                        {
                            classMatched = true;
                            break;
                        }
                    }
                    if (!classMatched)
                        continue;

                    //validate claim definition
                    if (!validateClaimDefinition(de))
                        continue;

                    //create CLAIM_ENTRY record
                    CLAIM_ENTRY claim;
                    claim.Id = de.Properties["cn"].Value.ToString();
                    claim.Type = getClaimValueType(de.Properties[ConstValue.distinguishedname].Value.ToString(), DomainController);
                    claim.Values = new CLAIM_ENTRY_VALUE_UNION();
                    using (DirectoryEntry source = new DirectoryEntry("LDAP://" + de.Properties[ConstValue.msDSClaimAttributeSource].Value.ToString()))
                    {
                        PropertyValueCollection values = principal.Properties[source.Properties["ldapdisplayName"].Value.ToString()];

                        //parse values
                        switch (claim.Type)
                        {
                            case CLAIM_TYPE.CLAIM_TYPE_STRING:
                                claim.Values.Struct3 = new CLAIM_TYPE_VALUE_LPWSTR();
                                claim.Values.Struct3.ValueCount = (uint)values.Count;
                                claim.Values.Struct3.StringValues = new string[values.Count];
                                for (int i = 0; i < values.Count; i++)
                                {
                                    claim.Values.Struct3.StringValues[i] = values[i].ToString();
                                }
                                break;
                            case CLAIM_TYPE.CLAIM_TYPE_INT64:
                                claim.Values.Struct1 = new CLAIM_TYPE_VALUE_INT64();
                                claim.Values.Struct1.ValueCount = (uint)values.Count;
                                claim.Values.Struct1.Int64Values = new long[values.Count];
                                for (int i = 0; i < values.Count; i++)
                                {
                                    claim.Values.Struct1.Int64Values[i] = (long)values[i];
                                }
                                break;
                            case CLAIM_TYPE.CLAIM_TYPE_UINT64:
                                claim.Values.Struct2 = new CLAIM_TYPE_VALUE_UINT64();
                                claim.Values.Struct2.ValueCount = (uint)values.Count;
                                claim.Values.Struct2.Uint64Values = new ulong[values.Count];
                                for (int i = 0; i < values.Count; i++)
                                {
                                    claim.Values.Struct2.Uint64Values[i] = (ulong)values[i];
                                }
                                break;
                            case CLAIM_TYPE.CLAIM_TYPE_BOOLEAN:
                                claim.Values.Struct4 = new CLAIM_TYPE_VALUE_BOOL();
                                claim.Values.Struct4.ValueCount = (uint)values.Count;
                                claim.Values.Struct4.BooleanValues = new bool[values.Count];
                                for (int i = 0; i < values.Count; i++)
                                {
                                    claim.Values.Struct4.BooleanValues[i] = (bool)values[i];
                                }
                                break;
                        }
                        claims.Add(claim);

                        ret.ulClaimsCount = (uint)claims.Count;
                        ret.ClaimEntries = claims.ToArray();
                        ret.usClaimsSourceType = 1;

                    }
                }
            }
            return ret;

        }

        /// <summary>
        /// get Constructed claims for a principal
        /// </summary>
        /// <param name="principal">the target principal</param>
        /// <param name="principalClass">user or device</param>
        /// <returns>a CLAIMS_ARRAY contains claims of the principal</returns>
        CLAIMS_ARRAY getConstructedClaims(DirectoryEntry principal, ClaimsPrincipalClass principalClass)
        {
            CLAIMS_ARRAY ret = new CLAIMS_ARRAY();

            DirectoryEntry root = new DirectoryEntry(ConstValue.claimTypesPath + domainNC);

            List<CLAIM_ENTRY> claims = new List<CLAIM_ENTRY>();

            DirectoryEntries children = root.Children;
            foreach (DirectoryEntry de in children)
            {
                //source type should be Constructed
                if ((de.Properties[ConstValue.msDSClaimSourceType] == null)
                    || ((ClaimsSourceType)Enum.Parse(typeof(ClaimsSourceType), de.Properties[ConstValue.msDSClaimSourceType].Value.ToString(), true) != ClaimsSourceType.Constructed)
                    || (de.Properties[ConstValue.msDSClaimTypeAppliesToClass] == null))
                    continue;

                //should applies to this principal class
                bool classMatched = false;
                foreach (object str in de.Properties[ConstValue.msDSClaimTypeAppliesToClass])
                {
                    string tmp = str.ToString();
                    if (tmp.ToLower() == (ConstValue.userRDN + domainNC.ToLower()) && principalClass == ClaimsPrincipalClass.User)
                    {
                        classMatched = true;
                        break;
                    }

                    if ((tmp.ToLower() == (ConstValue.computerRDN + domainNC).ToLower()) && principalClass == ClaimsPrincipalClass.Device)
                    {
                        classMatched = true;
                        break;
                    }
                }
                if (!classMatched)
                    continue;

                //validate claim definition
                if (!validateClaimDefinition(de))
                    continue;

                if (de.Properties[ConstValue.name] == null || de.Properties[ConstValue.name].Value == null)
                    continue;

                //Currently only the AuthenticationSilo claim is supported
                if (de.Properties[ConstValue.name].Value.ToString().Equals(ConstValue.authSiloClaimName, StringComparison.OrdinalIgnoreCase))
                {
                    CLAIM_ENTRY? claim = getAuthSiloClaim(principal);
                    if (claim.HasValue)
                    {
                        claims.Add(claim.Value);
                    }
                    break;
                }
            }
            ret.ulClaimsCount = (uint)claims.Count;
            ret.ClaimEntries = claims.ToArray();
            ret.usClaimsSourceType = 1;

            return ret;
        }

        /// <summary>
        /// get constructed claim for the specified principal
        /// </summary>
        /// <param name="principal">target principal</param>
        /// <returns>a CLAIM_ENTRY if the specified principal is a member of an authentication silo</returns>
        CLAIM_ENTRY? getAuthSiloClaim(DirectoryEntry principal)
        {
            //AuthSiloClaim is not issued until the domain functional level is at DS_BEHAVIOR_WIN2012R2 or higher.
            using (DirectoryEntry root = new DirectoryEntry("LDAP://" + domainNC))
            {

                if (root.Properties[ConstValue.msDSBehaviorVersion] == null)
                {
                    return null;
                }

                if ((root.Properties[ConstValue.msDSBehaviorVersion] == null)
                    || (int.Parse(root.Properties[ConstValue.msDSBehaviorVersion].Value.ToString()) < ConstValue.WinSvr2012R2))
                {
                    return null;
                }

                if ((principal.Properties[ConstValue.msDSAssignedAuthNPolicySilo] == null)
                    || (principal.Properties[ConstValue.msDSAssignedAuthNPolicySilo].Value == null))
                {
                    return null;
                }

                //Check if user is assigned to an enforced silo.
                using (DirectoryEntry assignedSilo = new DirectoryEntry("LDAP://" + principal.Properties[ConstValue.msDSAssignedAuthNPolicySilo].Value.ToString()))
                {
                    if ((assignedSilo.Properties[ConstValue.msDSAuthNPolicySiloEnforced] == null)
                        || (!bool.Parse(assignedSilo.Properties[ConstValue.msDSAuthNPolicySiloEnforced].Value.ToString())))
                    {
                        return null;
                    }

                    if (assignedSilo.Properties[ConstValue.msDSAuthNPolicySiloMembers] == null)
                    {
                        return null;
                    }

                    //Check if silo is configured with the user as a member.
                    bool memberOfSilo = false;
                    string dn = principal.Properties[ConstValue.distinguishedname].Value.ToString();
                    foreach (var siloMember in assignedSilo.Properties[ConstValue.msDSAuthNPolicySiloMembers])
                    {
                        if (siloMember.Equals(dn))
                        {
                            memberOfSilo = true;
                            break;
                        }
                    }

                    if (memberOfSilo == false)
                    {
                        return null;
                    }

                    //Fill in the claim details and return the claim.
                    CLAIM_ENTRY claim = new CLAIM_ENTRY();
                    claim.Id = ConstValue.authSiloClaimName;
                    claim.Type = CLAIM_TYPE.CLAIM_TYPE_STRING;
                    claim.Values.Struct3 = new CLAIM_TYPE_VALUE_LPWSTR();
                    claim.Values.Struct3.ValueCount = 1;
                    claim.Values.Struct3.StringValues = new string[] { assignedSilo.Properties[ConstValue.name].Value.ToString() };
                    return claim;
                }
            }
        }

        /// <summary>
        /// convert DNS name to Distinguished Name format
        /// </summary>
        /// <param name="dns">dns name</param>
        /// <returns>Distinguished Name</returns>
        string dnsNameToDN(string dns)
        {
            string[] tmps = dns.ToLower().Replace(" ", "").Split(new string[] { ",", "." }, StringSplitOptions.RemoveEmptyEntries);
            string ret = "";
            foreach (string s in tmps)
            {
                ret += "DC=";
                ret += s;
                ret += ",";
            }

            return ret.Remove(ret.Length - 1);
        }
    }
}
