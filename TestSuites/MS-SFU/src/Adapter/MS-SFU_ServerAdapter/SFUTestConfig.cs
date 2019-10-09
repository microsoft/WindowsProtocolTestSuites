// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.SFUProtocol.Adapter
{
    public class SfuTestConfig
    {
        private ITestSite Site;

        public SfuTestConfig(ITestSite site)
        {
            Site = site;
        }

        #region SUT related configuration
        public string SutAddress
        {
            get => GetProperty("SutHostName").ParseIPAddress().ToString();
        }

        public TransportType Transport
        {
            get => (TransportType)Enum.Parse(typeof(TransportType), GetProperty("Transport"));
        }

        public int Port
        {
            get => Int32.Parse(GetProperty("RemoteMachinePort"));
        }

        public TimeSpan Timeout
        {
            get => new TimeSpan(0, 0, Int32.Parse(GetProperty("Timeout")));
        }
        #endregion

        #region Realm1 related configuration
        public string Realm1
        {
            get => GetProperty("Realm1");
        }

        public UserInfo DelegatedUserInfo
        {
            get => new UserInfo
            {
                UserName = GetProperty("DelegatedUserName"),
                UserType = (PrincipalType)Enum.Parse(typeof(PrincipalType), GetProperty("DelegatedUserType")),
                DelegationNotAllowed = false,
            };
        }

        public UserInfo RestrictedUserInfo
        {
            get => new UserInfo
            {
                UserName = GetProperty("RestrictedUserName"),
                UserType = (PrincipalType)Enum.Parse(typeof(PrincipalType), GetProperty("RestrictedUserType")),
                DelegationNotAllowed = true,
            };
        }

        public ServiceInfo Service1aInfo
        {
            get => new ServiceInfo
            {
                Realm = Realm1,
                ServicePrincipalName = GetProperty("Service1aFQDN"),
                UserName = GetProperty("Service1aUserName"),
                UserType = (KerberosAccountType)Enum.Parse(typeof(KerberosAccountType), GetProperty("Service1aUserType")),
                Password = GetProperty("Service1aPassword"),
                Salt = GetProperty("Service1aSalt", false),
                TrustedToAuthenticationForDelegation = true,
                ServicesAllowedToSendForwardedTicketsTo = new string[] { GetProperty("Service2FQDN"), },
                ServicesAllowedToReceiveForwardedTicketsFrom = new string[] { },
            };
        }

        public ServiceInfo Service1bInfo
        {
            get => new ServiceInfo
            {
                Realm = Realm1,
                ServicePrincipalName = GetProperty("Service1bFQDN"),
                UserName = GetProperty("Service1bUserName"),
                UserType = (KerberosAccountType)Enum.Parse(typeof(KerberosAccountType), GetProperty("Service1bUserType")),
                Password = GetProperty("Service1bPassword"),
                Salt = GetProperty("Service1bSalt", false),
                TrustedToAuthenticationForDelegation = false,
                ServicesAllowedToSendForwardedTicketsTo = new string[] { },
                ServicesAllowedToReceiveForwardedTicketsFrom = new string[] { },
            };
        }

        public ServiceInfo Service2Info
        {
            get => new ServiceInfo
            {
                Realm = Realm1,
                ServicePrincipalName = GetProperty("Service2FQDN"),
                UserName = GetProperty("Service2UserName"),
                Password = GetProperty("Service2Password", false),
                Salt = GetProperty("Service2Salt", false),
                TrustedToAuthenticationForDelegation = false,
                ServicesAllowedToSendForwardedTicketsTo = new string[] { },
                ServicesAllowedToReceiveForwardedTicketsFrom = new string[] { GetProperty("Service1aFQDN"),  },
            };
        }
        #endregion

        private string GetProperty(string property, bool required = true)
        {
            string value = Site.Properties[property];
            if (required)
            {
                if (String.IsNullOrEmpty(value))
                {
                    Site.Assume.Inconclusive("Case is skipped due to property {0} is not provided.", property);
                }
            }
            return value;
        }

    }
}
