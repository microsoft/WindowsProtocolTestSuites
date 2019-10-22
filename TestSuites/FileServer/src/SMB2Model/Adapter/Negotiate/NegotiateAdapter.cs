// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Negotiate
{
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class NegotiateAdapter : ModelManagedAdapterBase, INegotiateAdapter
    {
        #region Fields
        private Smb2Client smb2Client;
        private NegotiateServerConfig config;

        private ulong messageId;
        #endregion

        #region  Events
        public event NegotiateResponseEventHandler NegotiateResponse;
        #endregion

        #region Initialization

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
        }

        public override void Reset()
        {
            base.Reset();

            if (smb2Client != null)
            {
                smb2Client.Disconnect();
                smb2Client = null;
            }
        }

        #endregion

        #region Public Methods
        public void OnServerDisconnected()
        {
            smb2Client.Disconnected -= new Action(OnServerDisconnected);
        }
        #endregion

        #region Actions
        public void ReadConfig(out NegotiateServerConfig c)
        {
            config = new NegotiateServerConfig
            {
                MaxSmbVersionSupported = testConfig.MaxSmbVersionSupported < DialectRevision.Smb311? testConfig.MaxSmbVersionSupported : DialectRevision.Smb302,
            };

            c = config;
        }

        public void SetupConnection()
        {
            smb2Client = new Smb2Client(testConfig.Timeout);
            smb2Client.DisableVerifySignature = this.testConfig.DisableVerifySignature;

            switch (testConfig.UnderlyingTransport)
            {
                case Smb2TransportType.Tcp:
                    Site.Assert.IsTrue(
                        testConfig.SutIPAddress != null && testConfig.SutIPAddress != System.Net.IPAddress.None,
                        "Server IP should not be empty when transport type is TCP.");
                    Site.Log.Add(LogEntryKind.Debug, "Connect to server {0} over TCP", testConfig.SutIPAddress.ToString());
                    smb2Client.ConnectOverTCP(testConfig.SutIPAddress);
                    break;
                case Smb2TransportType.NetBios:
                    Site.Assert.IsFalse(string.IsNullOrEmpty(testConfig.SutComputerName), "Server name should not be null when transport type is NetBIOS.");
                    Site.Log.Add(LogEntryKind.Debug, "Connect to server {0} over NetBios", testConfig.SutComputerName);
                    smb2Client.ConnectOverNetbios(testConfig.SutComputerName);
                    break;
                default:
                    Site.Assert.Fail("The transport type is {0}, but currently only Tcp and NetBIOS are supported.", testConfig.UnderlyingTransport);
                    break;
            }
            smb2Client.Disconnected += new Action(OnServerDisconnected);

            messageId = 0;
        }

        public void ExpectDisconnect()
        {
            if (smb2Client != null)
            {
                smb2Client.Disconnect();
                smb2Client = null;
            }
        }

        public void ComNegotiateRequest(Sequence<string> dialects)
        {
            Packet_Header responseHeader = new Packet_Header();
            DialectRevision selectedDialect = DialectRevision.Smb2Unknown;
            NEGOTIATE_Response responsePayload = new NEGOTIATE_Response();
            byte[] smb2ClientGssToken;
            ModelSmb2Status status = ModelSmb2Status.STATUS_SUCCESS;

            try
            {
                status = (ModelSmb2Status)smb2Client.MultiProtocolNegotiate(dialects.ToArray(), out selectedDialect, out smb2ClientGssToken, out responseHeader, out responsePayload);
                if (status != ModelSmb2Status.STATUS_SUCCESS)
                {
                    selectedDialect = DialectRevision.Smb2Unknown;
                }
                this.NegotiateResponse(status, selectedDialect);
                if (selectedDialect == DialectRevision.Smb2Wildcard)
                {
                    messageId = 1;
                }
            }
            catch
            {
            }

        }

        public void NegotiateRequest(Sequence<DialectRevision> dialects)
        {
            Smb2NegotiateRequestPacket negotiateRequest;
            Smb2NegotiateResponsePacket negotiateResponse;
            DialectRevision selectedDialect = DialectRevision.Smb2Unknown;
            byte[] smb2ClientGssToken;
            ModelSmb2Status status = ModelSmb2Status.STATUS_SUCCESS;

            try
            {
                status = (ModelSmb2Status)smb2Client.Negotiate(0, 1, Packet_Header_Flags_Values.NONE, messageId++, dialects.ToArray(), SecurityMode_Values.NONE, Capabilities_Values.NONE, Guid.NewGuid(),
                    out selectedDialect, out smb2ClientGssToken, out negotiateRequest, out negotiateResponse);
                if (status != ModelSmb2Status.STATUS_SUCCESS)
                {
                    selectedDialect = DialectRevision.Smb2Unknown;
                }
                this.NegotiateResponse(status, selectedDialect);
                testConfig.CheckNegotiateContext(negotiateRequest, negotiateResponse);
            }
            catch
            {
            }
        }
        #endregion
    }
}
