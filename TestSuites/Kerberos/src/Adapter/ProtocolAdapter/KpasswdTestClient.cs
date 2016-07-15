// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocol.TestSuites.Kerberos.Adapter
{
    public class KpasswdTestClient : KpasswdClient
    {
        private ITestSite testSite;

        /// <summary>
        /// Construct a kpassword test client
        /// </summary>
        /// <param name="kdcAddress">The IP address of the KDC.</param>
        /// <param name="kdcPort">The port of the KDC for Kpassword.</param>
        /// <param name="transportType">Whether the transport is TCP or UDP transport.</param>
        /// <param name="ticket">The ticket authorized to change password</param>
        public KpasswdTestClient(string kdcAddress, int kdcPort, TransportType transportType, KerberosTicket ticket)
            : base(kdcAddress, kdcPort, transportType)
        {
            testSite = TestClassBase.BaseTestSite;
            this.Context.Ticket = ticket;
        }

        /// <summary>
        /// Create a Kpassword Request
        /// </summary>
        /// <param name="newPwd">The new password to change</param>
        /// <returns></returns>
        public KpasswordRequest CreateKpasswordRequest(string newPwd)
        {
            //Generate the subkey, which will be used to encrypt/decrypt KRB-PRIV message
            EncryptionKey subkey = KerberosUtility.MakeKey((EncryptionType)this.Context.Ticket.SessionKey.keytype.Value, "Password01!", "This is a salt");
            Context.Subkey = subkey;

            Authenticator authenticator = CreateAuthenticator(this.Context.Ticket, null, subkey);
            KpasswordRequest request = new KpasswordRequest(this.Context.Ticket, authenticator, newPwd);
            return request;
        }

        public KpasswordRequest CreateAuthErrorKpasswordRequest(string newPwd)
        {
            //Generate the subkey, which will be used to encrypt/decrypt KRB-PRIV message
            EncryptionKey subkey = KerberosUtility.MakeKey((EncryptionType)this.Context.Ticket.SessionKey.keytype.Value, "Password01!", "This is a salt");
            Context.Subkey = subkey;

            Authenticator authenticator = CreateAuthenticator(this.Context.Ticket, null, subkey);
            KpasswordRequest request = new KpasswordRequest(this.Context.Ticket, authenticator, newPwd, true);
            return request;
        }

        /// <summary>
        /// Create and send Kpassword Request
        /// </summary>
        /// <param name="newPwd">The new password to change</param>
        public void SendKpasswordRequest(string newPwd)
        {
            KpasswordRequest pwdRequest = this.CreateKpasswordRequest(newPwd);
            this.SendPdu(pwdRequest);
            this.testSite.Log.Add(LogEntryKind.Comment, "Send Kpassword request.");
        }

        /// <summary>
        /// Create and send malformed Kpassword Request
        /// </summary>
        /// <param name="newPwd">The new password to change</param>
        public void SendMalformedKpasswordRequest(string newPwd)
        {
            KpasswordRequest pwdRequest = this.CreateKpasswordRequest(newPwd);
            //Change the protocol version number(hex constant 0x0001) to be 0xff80 
            pwdRequest.version = KerberosUtility.ConvertEndian(0xff80);
            this.SendPdu(pwdRequest);
            this.testSite.Log.Add(LogEntryKind.Comment, "Send malformed Kpassword request.");
        }

        /// <summary>
        /// Create and send Auth Error Kpassword Request
        /// </summary>
        /// <param name="newPwd">The new password to change</param>
        public void SendAuthErrorKpasswordRequest(string newPwd)
        {
            KpasswordRequest pwdRequest = this.CreateAuthErrorKpasswordRequest(newPwd);
            this.SendPdu(pwdRequest);
            this.testSite.Log.Add(LogEntryKind.Comment, "Send Auth Error Kpassword request.");
        }

        /// <summary>
        /// Recieve a Kpassword response
        /// </summary>
        /// <returns></returns>
        public KpasswordResponse ExpectKpasswordResponse()
        {
            KerberosPdu responsePdu = this.ExpectPdu(KerberosConstValue.TIMEOUT_DEFAULT, typeof(KerberosAsResponse));
            this.testSite.Assert.IsNotNull(responsePdu, "Response Pdu should not be null.");
            if (responsePdu is KerberosKrbError)
            {
                KerberosKrbError error = responsePdu as KerberosKrbError;
                this.testSite.Log.Add(LogEntryKind.Comment, "ERROR CODE: {0}", error.ErrorCode.ToString());
            }
            this.testSite.Assert.IsInstanceOfType(responsePdu, typeof(KpasswordResponse), "Response type mismatches");

            KpasswordResponse response = responsePdu as KpasswordResponse;
            this.testSite.Log.Add(LogEntryKind.Comment, "Recieve Kpassword response.");

            //User the subkey to decrypt the KRB-PRIV
            response.DecryptKrbPriv(Context.Subkey);
            return response;
        }

        private Authenticator CreateAuthenticator(
                KerberosTicket ticket,
                AuthorizationData data,
                EncryptionKey subkey
                )
        {
            Authenticator plaintextAuthenticator = new Authenticator();
            plaintextAuthenticator.authenticator_vno = new Asn1Integer(KerberosConstValue.KERBEROSV5);
            plaintextAuthenticator.crealm = ticket.Ticket.realm;
            plaintextAuthenticator.cusec = new Microseconds(0);
            plaintextAuthenticator.ctime = KerberosUtility.CurrentKerberosTime;
            plaintextAuthenticator.seq_number = new Protocols.TestTools.StackSdk.Security.KerberosLib.KerbUInt32(0);
            plaintextAuthenticator.cname = ticket.TicketOwner;
            plaintextAuthenticator.subkey = subkey;
            plaintextAuthenticator.authorization_data = data;
            return plaintextAuthenticator;
        }

        public override void Connect()
        {
            testSite.Log.Add(LogEntryKind.Comment, "Setup {0} transport connection with KDC({1}:{2}).", transportType, kdcAddress, kdcPort);
            base.Connect();
        }

        public override void DisConnect()
        {
            testSite.Log.Add(LogEntryKind.Comment, "Setup {0} transport connection with KDC({1}:{2}).", transportType, kdcAddress, kdcPort);
            base.DisConnect();
        }

        public ushort GetResultCode(KpasswordResponse response)
        {
            byte[] resultCodeBytes = ArrayUtility.SubArray<byte>(response.priv_enc_part.user_data.ByteArrayValue, 0, sizeof(ushort));
            Array.Reverse(resultCodeBytes);
            return BitConverter.ToUInt16(resultCodeBytes, 0);
        }
    }
}
