// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using System;

namespace Microsoft.Protocols.TestSuites.SfuProtocol.Adapter
{
    /// <summary>
    /// SFU functional client.
    /// </summary>
    public class SfuFunctionalClient
    {
        private ITestSite BaseTestSite;

        private KerberosTestClient client;

        public ServiceInfo Service1 { get; private set; }

        private TimeSpan timeout;

        private EncryptionKey asRepSessionKey;
        private KerberosTicket asRepTicket;
        private EncryptionType asRepEncrytType;

        /// <summary>
        /// Constructor of SfuFunctionalClient.
        /// </summary>
        /// <param name="site">The test site.</param>
        /// <param name="service1Info">Information of service1.</param>
        /// <param name="sutAddress">Address of SUT.</param>
        /// <param name="port">Port number</param>
        /// <param name="transport">Transport to be used.</param>
        /// <param name="timeoutForKerberos">Time out for Kerberos.</param>
        public SfuFunctionalClient(ITestSite site,ServiceInfo service1Info, string sutAddress, int port, TransportType transport, TimeSpan timeoutForKerberos)
        {
            BaseTestSite = site;

            Service1 = service1Info;

            timeout = timeoutForKerberos;

            client = new KerberosTestClient(Service1.Realm, Service1.UserName, Service1.Password, Service1.UserType, sutAddress, port, transport, KerberosConstValue.OidPkt.MSKerberosToken);
        }

        /// <summary>
        /// Authenticate with SUT.
        /// </summary>
        public void Authenticate()
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

        /// <summary>
        /// S4U2Self with SUT.
        /// </summary>
        /// <param name="user">Information of user.</param>
        /// <param name="checker">Response checker.</param>
        public void S4U2Self(UserInfo user, ResponseChecker checker)
        {
            BaseTestSite.Log.Add(LogEntryKind.Debug, $"S4U2Self: {Service1.ServicePrincipalName}, {user.UserName}, {user.UserType}.");

            var options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;

            var paForUserEnc = new PA_FOR_USER_ENC(
                new PrincipalName(new KerbInt32((long)user.UserType), new Asn1SequenceOf<KerberosString>(new KerberosString[] { new KerberosString(user.UserName) })),
                new TestTools.StackSdk.Security.KerberosLib.Realm(Service1.Realm)
                );

            paForUserEnc.UpdateChecksum(client.Context.SessionKey.keyvalue.ByteArrayValue);

            var paForUser = new PA_FOR_USER(paForUserEnc);

            var seqOfPaData = new Asn1SequenceOf<PA_DATA>(new PA_DATA[] { paForUser });

            client.SendTgsRequest(
                        Service1.ServicePrincipalName,
                        options,
                        seqOfPaData
                        );

            var response = client.ExpectPdu(timeout);

            checker(response);
        }

        /// <summary>
        /// S4U2Proxy with SUT.
        /// </summary>
        /// <param name="isConstrained">Whether to use constrained delegation.</param>
        /// <param name="service2">Information of service2.</param>
        /// <param name="additionalTicket">Additional ticket.</param>
        /// <param name="checker">The response checker.</param>
        public void S4U2Proxy(bool isConstrained, ServiceInfo service2, Ticket additionalTicket, ResponseChecker checker)
        {
            var pacOptionsValue = PacOptions.None;

            if (isConstrained)
            {
                pacOptionsValue = PacOptions.ResourceBasedConstrainedDelegation;
            }

            var pacOptions = new PaPacOptions(pacOptionsValue);


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
                        service2.ServicePrincipalName,
                        options,
                        seqOfPaData,
                        additionalTicket
                        );

            var response = client.ExpectPdu(timeout);

            checker(response);
        }

    }
}
