using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.Pac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.SFUProtocol
{
    [TestClass]
    public class SFU_BasicTestSuite : TestClassBase
    {
        private string realm1;
        private string service1User;
        private string service1Pass;
        private string sutAddress;
        private int port;
        private TransportType transport;
        private string service1;
        private string service2User;
        private KerberosTestClient client;
        private string service2;
        private string service1Salt;
        private string service2Pass;
        private string service2Salt;
        private string delegatedUser;
        private PrincipalType delegatedUserType;
        private EncryptionKey asRepSessionKey;
        private KerberosTicket asRepTicket;
        private EncryptionType asRepEncrytType;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Initialize(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Cleanup();
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();

            // SUT related configuration
            sutAddress = GetProperty("SutHostName").ParseIPAddress().ToString();
            transport = (TransportType)Enum.Parse(typeof(TransportType), GetProperty("Transport"));
            port = Int32.Parse(GetProperty("RemoteMachinePort"));

            // Realm1 related configuration
            realm1 = GetProperty("Realm1");
            delegatedUser = GetProperty("DelegatedUserName");
            delegatedUserType = (PrincipalType)Enum.Parse(typeof(PrincipalType), GetProperty("DelegatedUserType"));

            // Service1 related configuration
            service1 = GetProperty("Service1FQDN");
            service1User = GetProperty("Service1UserName");
            service1Pass = GetProperty("Service1Password");
            service1Salt = GetProperty("Service1Salt", false);


            client = new KerberosTestClient(realm1, service1User, service1Pass, KerberosAccountType.User, sutAddress, port, transport, KerberosConstValue.OidPkt.MSKerberosToken);

        }

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

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }

        [TestCategory("BVT")]
        [TestCategory("SingleRealm")]
        [TestMethod]
        public void BVT_SingleRealm_S4U2Self_UsingUserName()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Authenticate with SUT.");
            Authenticate();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Self to get ticket to service1 for user2.");
            S4U2Self();
        }

        [TestCategory("BVT")]
        [TestCategory("SingleRealm")]
        [TestMethod]
        public void BVT_SingleRealm_S4U2Proxy_UsingUserName()
        {

            // Service2 related configuration
            service2 = GetProperty("Service2FQDN");
            service2User = GetProperty("Service2UserName");
            service2Pass = GetProperty("Service2Password", false);
            service2Salt = GetProperty("Service2Salt", false);


            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Authenticate with SUT.");
            Authenticate();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Self to get ticket to service1 for user2.");
            var tgsRep = S4U2Self();

            tgsRep.DecryptTicket(service1Pass, service1Salt);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Proxy to get ticket to service2 for user2.");
            S4U2Proxy(tgsRep.Response.ticket, tgsRep.TicketEncPart);

            return;
        }

        private void S4U2Proxy(Ticket ticket, EncTicketPart encTicketPart)
        {
            var pacOptions = new PaPacOptions(PacOptions.ResourceBasedConstrainedDelegation);


            var options = KdcOptions.CNAMEINADDLTKT | KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;


            var seqOfPaData = new Asn1SequenceOf<PA_DATA>(
                new PA_DATA[]
                {
                    pacOptions.Data
                }
            );

            client.Context.SessionKey = asRepSessionKey;
            client.Context.Ticket = asRepTicket;
            client.Context.SelectedEType = asRepEncrytType;

            client.SendTgsRequest(
                        service2,
                        options,
                        seqOfPaData,
                        ticket
                        );

            var tgsRep = client.ExpectTgsResponse();


            BaseTestSite.Assert.IsNotNull(tgsRep, "SUT should reply with TGS_REP.");

            // Verify the response
            BaseTestSite.Assert.AreEqual(service2, KerberosUtility.PrincipalName2String(tgsRep.Response.ticket.sname), "The sname field in ticket should contain the name of service2.");

            BaseTestSite.Assert.AreEqual(realm1, tgsRep.Response.ticket.realm.Value, "The realm field in ticket should contain the realm of service2.");

            if (!String.IsNullOrEmpty(service2Salt) && !String.IsNullOrEmpty(service2Salt))
            {
                tgsRep.DecryptTicket(service2Pass, service2Salt);

                bool cnameEqual = (encTicketPart.cname.name_type.Value == tgsRep.TicketEncPart.cname.name_type.Value) && (KerberosUtility.PrincipalName2String(encTicketPart.cname) == KerberosUtility.PrincipalName2String(tgsRep.TicketEncPart.cname));

                BaseTestSite.Assert.IsTrue(cnameEqual, "The cname field in ticket should contain the cname of additional ticket.");

                BaseTestSite.Assert.AreEqual(encTicketPart.crealm.Value, tgsRep.TicketEncPart.crealm.Value, "The crealm field in ticket should contain the crealm of additional ticket.");

                var flags = (EncTicketFlags)(KerberosUtility.ConvertFlags2Int(tgsRep.TicketEncPart.flags.ByteArrayValue));

                bool fowardable = flags.HasFlag(EncTicketFlags.FORWARDABLE);

                BaseTestSite.Assert.IsTrue(fowardable, "Forwardable should be set.");

                var adWin2kPac = FindOneInAuthData<AdWin2KPac>(tgsRep.TicketEncPart.authorization_data.Elements);

                BaseTestSite.Assert.IsNotNull(adWin2kPac, "There should be an AdWin2KPac in ticket.");

                var s4uDelegationInfo = adWin2kPac.Pac.PacInfoBuffers.Where(buffer => buffer is S4uDelegationInfo).FirstOrDefault() as S4uDelegationInfo;

                BaseTestSite.Assert.IsNotNull(s4uDelegationInfo, "There should be a S4U_DELEGATION_INFO in AdWin2KPac.");

                BaseTestSite.Log.Add(LogEntryKind.Debug, "S4U2proxyTarget: {0}.", Parse_RPC_UnicodeString(s4uDelegationInfo.NativeS4uDelegationInfo.S4U2proxyTarget));

                BaseTestSite.Log.Add(LogEntryKind.Debug, "TransitedListSize: {0}.", s4uDelegationInfo.NativeS4uDelegationInfo.TransitedListSize);

                BaseTestSite.Log.Add(LogEntryKind.Debug, "S4UTransitedServices: {0}.", String.Join(Environment.NewLine, s4uDelegationInfo.NativeS4uDelegationInfo.S4UTransitedServices.Select(element => Parse_RPC_UnicodeString(element)).ToArray()));

            }

        }

        private string Parse_RPC_UnicodeString(_RPC_UNICODE_STRING rpcUnicodeString)
        {
            return String.Concat(rpcUnicodeString.Buffer.Take(rpcUnicodeString.Length).Select(code => Char.ConvertFromUtf32(code)).ToArray());
        }

        private static T FindOneInAuthData<T>(AuthorizationDataElement[] elements) where T : class, IAuthDataElement
        {
            T element = null;
            if (typeof(T) == typeof(AdIfRelevent)) throw new NotSupportedException("Search for AdIfRelevent is not supported.");
            LinkedList<IAuthDataElement> authDataList = new LinkedList<IAuthDataElement>();
            AdIfRelevent rootNode = new AdIfRelevent(new AD_IF_RELEVANT(elements));
            authDataList.AddLast(rootNode);
            while (element == null && authDataList.Count > 0)
            {
                var node = authDataList.Last.Value;
                authDataList.RemoveLast();
                if (node is AdIfRelevent)
                {
                    var adIfRelevent = node as AdIfRelevent;
                    foreach (var authData in adIfRelevent.Elements)
                    {
                        authDataList.AddLast(authData);
                    }
                }
                else if (node is T)
                {
                    element = node as T;
                }
            }
            return element;
        }

        private KerberosTgsResponse S4U2Self()
        {


            var options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;

            var paForUserEnc = new PA_FOR_USER_ENC(
                new PrincipalName(new KerbInt32((long)delegatedUserType), new Asn1SequenceOf<KerberosString>(new KerberosString[] { new KerberosString(delegatedUser) })),
                new TestTools.StackSdk.Security.KerberosLib.Realm(realm1)
                );

            paForUserEnc.UpdateChecksum(client.Context.SessionKey.keyvalue.ByteArrayValue);

            var paForUser = new PA_FOR_USER(paForUserEnc);

            var seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paForUser });

            client.SendTgsRequest(
                        service1,
                        options,
                        seqOfPaData
                        );

            var tgsRep = client.ExpectTgsResponse();

            BaseTestSite.Assert.IsNotNull(tgsRep, "SUT should reply with TGS_REP.");

            // Verify the response
            BaseTestSite.Assert.AreEqual(service1, KerberosUtility.PrincipalName2String(tgsRep.Response.ticket.sname), "The sname field in ticket should contain the user name requested to indicate that SUT supports S4U.");

            BaseTestSite.Assert.AreEqual(realm1, tgsRep.Response.ticket.realm.Value, "The realm field in ticket should be equal to realm1.");

            if (!String.IsNullOrEmpty(service1Salt))
            {
                tgsRep.DecryptTicket(service1Pass, service1Salt);

                bool cnameEqual = ((long)delegatedUserType == tgsRep.TicketEncPart.cname.name_type.Value) && (delegatedUser == KerberosUtility.PrincipalName2String(tgsRep.TicketEncPart.cname));

                BaseTestSite.Assert.IsTrue(cnameEqual, "The cname field in ticket should contain the user name requested to indicate that SUT supports S4U.");

                BaseTestSite.Assert.AreEqual(realm1, tgsRep.TicketEncPart.crealm.Value, "The crealm field in ticket should be equal to realm1.");


                var flags = (EncTicketFlags)(KerberosUtility.ConvertFlags2Int(tgsRep.TicketEncPart.flags.ByteArrayValue));

                bool fowardable = flags.HasFlag(EncTicketFlags.FORWARDABLE);

                BaseTestSite.Log.Add(LogEntryKind.Debug, "Forwardable is set to {0}", fowardable);

            }

            return tgsRep;
        }

        private void Authenticate()
        {

            var options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;

            client.SendAsRequest(options, null);

            METHOD_DATA methodData;
            var krbError = client.ExpectPreauthRequiredError(out methodData);

            BaseTestSite.Assert.IsNotNull(krbError, "SUT should reply with preauth required error.");


            string timeStamp = KerberosUtility.CurrentKerberosTime.Value;
            PaEncTimeStamp paEncTimeStamp = new PaEncTimeStamp(
                timeStamp,
                0,
                client.Context.SelectedEType,
                client.Context.CName.Password,
                client.Context.CName.Salt
                );
            PaPacRequest paPacRequest = new PaPacRequest(true);
            var seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paEncTimeStamp.Data, paPacRequest.Data });


            client.SendAsRequest(options, seqOfPaData);

            var asRep = client.ExpectAsResponse();

            BaseTestSite.Assert.IsNotNull(asRep, "SUT should reply with AS_REP.");

            asRepSessionKey = client.Context.SessionKey;
            asRepTicket = client.Context.Ticket;
            asRepEncrytType = client.Context.SelectedEType;

        }
    }
}
