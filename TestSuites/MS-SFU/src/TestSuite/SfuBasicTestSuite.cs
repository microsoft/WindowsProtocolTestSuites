// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.SfuProtocol.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;
using System;
using System.Linq;

namespace Microsoft.Protocols.TestSuites.SfuProtocol.TestSuites
{
    public class SfuBasicTestSuite : TestClassBase
    {
        protected SfuTestConfig config;

        protected SfuFunctionalClient client;

        protected override void TestInitialize()
        {
            base.TestInitialize();

            config = new SfuTestConfig(BaseTestSite);
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }

        protected void Initialize(ServiceInfo service1Instance)
        {
            client = new SfuFunctionalClient(BaseTestSite, service1Instance, config.SutAddress, config.Port, config.Transport, config.Timeout);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Authenticate with SUT.");

            client.Authenticate();
        }

        protected void S4U2Self(UserInfo user, out KerberosTgsResponse tgsRsp)
        {
            KerberosTgsResponse krbTgsRsp = null;

            client.S4U2Self(user, response =>
            {
                BaseTestSite.Assert.IsInstanceOfType(response, typeof(KerberosTgsResponse), "SUT should reply with TGS_REP.");

                krbTgsRsp = response as KerberosTgsResponse;

                // Verify the response
                BaseTestSite.Assert.AreEqual(client.Service1.ServicePrincipalName, KerberosUtility.PrincipalName2String(krbTgsRsp.Response.ticket.sname), "The sname field in ticket should contain the user name requested to indicate that SUT supports S4U.");

                BaseTestSite.Assert.AreEqual(client.Service1.Realm, krbTgsRsp.Response.ticket.realm.Value, "The realm field in ticket should be equal to realm1.");

                if (!String.IsNullOrEmpty(client.Service1.Salt))
                {
                    krbTgsRsp.DecryptTicket(client.Service1.Password, client.Service1.Salt);

                    bool cnameEqual = ((long)user.UserType == krbTgsRsp.TicketEncPart.cname.name_type.Value) && (user.UserName == KerberosUtility.PrincipalName2String(krbTgsRsp.TicketEncPart.cname));

                    BaseTestSite.Assert.IsTrue(cnameEqual, "The cname field in ticket should contain the user name requested to indicate that SUT supports S4U.");

                    BaseTestSite.Assert.AreEqual(client.Service1.Realm, krbTgsRsp.TicketEncPart.crealm.Value, "The crealm field in ticket should be equal to realm1.");

                    var flags = (EncTicketFlags)(KerberosUtility.ConvertFlags2Int(krbTgsRsp.TicketEncPart.flags.ByteArrayValue));

                    bool forwardable = flags.HasFlag(EncTicketFlags.FORWARDABLE);

                    if (user.DelegationNotAllowed)
                    {
                        BaseTestSite.Assert.IsFalse(forwardable, "Forwardable MUST be set to false.");
                    }
                    else
                    {
                        if (client.Service1.TrustedToAuthenticationForDelegation)
                        {
                            BaseTestSite.Assert.IsTrue(forwardable, "Forwardable MUST be set to true.");
                        }
                        else
                        {
                            BaseTestSite.Assert.IsFalse(forwardable, "Forwardable MUST be set to false.");
                        }

                    }
                }
            });

            tgsRsp = krbTgsRsp;
        }

        protected void S4U2Proxy(bool resourceBased, ServiceInfo service2, KerberosTgsResponse tgsRsp1)
        {
            client.S4U2Proxy(resourceBased, service2, tgsRsp1.Response.ticket, response =>
            {
                var flags1 = (EncTicketFlags)(KerberosUtility.ConvertFlags2Int(tgsRsp1.TicketEncPart.flags.ByteArrayValue));

                bool forwardable1 = flags1.HasFlag(EncTicketFlags.FORWARDABLE);

                var adWin2kPac1 = Utilities.GetAdWin2KPac(tgsRsp1);

                BaseTestSite.Assert.IsNotNull(adWin2kPac1, "There should be an AdWin2KPac in ticket.");

                var validationInfos = adWin2kPac1.Pac.PacInfoBuffers.Where(buffer => buffer.GetType() == typeof(KerbValidationInfo));

                bool validationInfoFound = validationInfos.Count() == 1;

                BaseTestSite.Assert.IsTrue(validationInfoFound, "There should be a KerbValidationInfo in ticket.");

                var validationInfo = validationInfos.First() as KerbValidationInfo;

                // USER_NOT_DELEGATED
                bool userNotDelegated = (validationInfo.NativeKerbValidationInfo.UserAccountControl & (UInt32)USER_ACCOUNT_CONTROL.USER_NOT_DELEGATED) != 0;

                if (!forwardable1)
                {
                    if (!resourceBased)
                    {
                        if (userNotDelegated)
                        {
                            // Check error code STATUS_NO_MATCH.
                            BaseTestSite.Assert.IsInstanceOfType(response, typeof(KerberosKrbError), "SUT should reply with KRB_ERR.");

                            var krbErr = response as KerberosKrbError;

                            BaseTestSite.Assert.AreEqual(krbErr.ErrorCode, KRB_ERROR_CODE.KDC_ERR_BADOPTION, "SUT should reply with KRB-ERR-BADOPTION.");

                            var status = Utilities.GetExtendedStatus(krbErr);

                            BaseTestSite.Assert.AreEqual((UInt32)NtStatus.STATUS_NO_MATCH, status, "STATUS_NO_MATCH.");

                            return;
                        }
                        else
                        {
                            // Check error code STATUS_NOT_SUPPORTED.
                            BaseTestSite.Assert.IsInstanceOfType(response, typeof(KerberosKrbError), "SUT should reply with KRB_ERR.");

                            var krbErr = response as KerberosKrbError;

                            BaseTestSite.Assert.AreEqual(krbErr.ErrorCode, KRB_ERROR_CODE.KDC_ERR_BADOPTION, "SUT should reply with KRB-ERR-BADOPTION.");

                            var status = Utilities.GetExtendedStatus(krbErr);

                            BaseTestSite.Assert.AreEqual((UInt32)NtStatus.STATUS_NOT_SUPPORTED, status, "STATUS_NOT_SUPPORTED.");

                            return;
                        }
                    }
                    else
                    {
                        if (userNotDelegated)
                        {
                            // Check error code STATUS_ACCOUNT_RESTRICTION.
                            BaseTestSite.Assert.IsInstanceOfType(response, typeof(KerberosKrbError), "SUT should reply with KRB_ERR.");

                            var krbErr = response as KerberosKrbError;

                            BaseTestSite.Assert.AreEqual(krbErr.ErrorCode, KRB_ERROR_CODE.KDC_ERR_BADOPTION, "SUT should reply with KRB-ERR-BADOPTION.");

                            var status = Utilities.GetExtendedStatus(krbErr);

                            BaseTestSite.Assert.AreEqual((UInt32)NtStatus.STATUS_ACCOUNT_RESTRICTION, status, "STATUS_ACCOUNT_RESTRICTION.");

                            return;
                        }
                    }
                }

                BaseTestSite.Assert.IsInstanceOfType(response, typeof(KerberosTgsResponse), "SUT should reply with TGS_REP.");

                var tgsRep = response as KerberosTgsResponse;

                // Verify the response
                BaseTestSite.Assert.AreEqual(service2.ServicePrincipalName, KerberosUtility.PrincipalName2String(tgsRep.Response.ticket.sname), "The sname field in ticket should contain the name of service2.");

                BaseTestSite.Assert.AreEqual(service2.Realm, tgsRep.Response.ticket.realm.Value, "The realm field in ticket should contain the realm of service2.");

                if (!String.IsNullOrEmpty(service2.Password) && !String.IsNullOrEmpty(service2.Salt))
                {
                    tgsRep.DecryptTicket(service2.Password, service2.Salt);

                    bool cnameEqual = (tgsRsp1.TicketEncPart.cname.name_type.Value == tgsRep.TicketEncPart.cname.name_type.Value) && (KerberosUtility.PrincipalName2String(tgsRsp1.TicketEncPart.cname) == KerberosUtility.PrincipalName2String(tgsRep.TicketEncPart.cname));

                    BaseTestSite.Assert.IsTrue(cnameEqual, "The cname field in ticket should contain the cname of additional ticket.");

                    BaseTestSite.Assert.AreEqual(tgsRsp1.TicketEncPart.crealm.Value, tgsRep.TicketEncPart.crealm.Value, "The crealm field in ticket should contain the crealm of additional ticket.");

                    var flags = (EncTicketFlags)(KerberosUtility.ConvertFlags2Int(tgsRep.TicketEncPart.flags.ByteArrayValue));

                    bool forwardable = flags.HasFlag(EncTicketFlags.FORWARDABLE);

                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Forwardable is set to {0}.", forwardable);

                    var adWin2kPac = Utilities.GetAdWin2KPac(tgsRep);

                    BaseTestSite.Assert.IsNotNull(adWin2kPac, "There should be an AdWin2KPac in ticket.");

                    var s4uDelegationInfo = adWin2kPac.Pac.PacInfoBuffers.Where(buffer => buffer is S4uDelegationInfo).FirstOrDefault() as S4uDelegationInfo;

                    BaseTestSite.Assert.IsNotNull(s4uDelegationInfo, "There should be a S4U_DELEGATION_INFO in AdWin2KPac.");

                    BaseTestSite.Log.Add(LogEntryKind.Debug, "S4U2proxyTarget: {0}.", Utilities.Parse_RPC_UnicodeString(s4uDelegationInfo.NativeS4uDelegationInfo.S4U2proxyTarget));

                    BaseTestSite.Log.Add(LogEntryKind.Debug, "TransitedListSize: {0}.", s4uDelegationInfo.NativeS4uDelegationInfo.TransitedListSize);

                    BaseTestSite.Log.Add(LogEntryKind.Debug, "S4UTransitedServices: {0}.", String.Join(Environment.NewLine, s4uDelegationInfo.NativeS4uDelegationInfo.S4UTransitedServices.Select(element => Utilities.Parse_RPC_UnicodeString(element)).ToArray()));

                }
            });
        }
    }
}
