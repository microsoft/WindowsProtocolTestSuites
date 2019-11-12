// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Signing
{
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class SigningAdapter : ModelManagedAdapterBase, ISigningAdapter
    {
        #region Fields

        private Smb2FunctionalClient testClient;
        private SigningConfig signingConfig;
        #endregion

        #region Initialization

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
        }
        public override void Reset()
        {
            base.Reset();

            if (testClient != null)
            {
                testClient.Disconnect();
                testClient = null;
            }
        }

        #endregion

        #region Events
        public event NegotiateResponseEventHandler NegotiateResponse;
        public event SessionSetupResponseEventHandler SessionSetupResponse;
        public event TreeConnectResponseEventHandler TreeConnectResponse;
        #endregion

        #region Actions
        public void ReadConfig(out SigningConfig c)
        {
            c = new SigningConfig
            {
                MaxSmbVersionSupported = ModelUtility.GetModelDialectRevision(testConfig.MaxSmbVersionSupported),
                IsServerSigningRequired = testConfig.IsServerSigningRequired,
            };
            if (testConfig.IsGlobalEncryptDataEnabled && c.MaxSmbVersionSupported >= ModelDialectRevision.Smb30)
            {
                Site.Assert.Inconclusive("This test case is not applicable due to IsGlobalEncryptDataEnabled is True");
            }
            signingConfig = c;
            Site.Log.Add(LogEntryKind.Debug, signingConfig.ToString());
        }

        public void NegotiateRequest(ModelDialectRevision maxSmbVersionClientSupported, SigningFlagType signingFlagType, SigningEnabledType signingEnabledType, SigningRequiredType signingRequiredType)
        {
            testClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            testClient.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress);

            DialectRevision[] dialects = Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(maxSmbVersionClientSupported));
            Packet_Header_Flags_Values headerFlags = (signingFlagType == SigningFlagType.SignedFlagSet) ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE;

            SigningEnabledType resSigningEnabledType = SigningEnabledType.SigningEnabledNotSet;
            SigningRequiredType resSigningRequiredType = SigningRequiredType.SigningRequiredNotSet;
            uint status = testClient.Negotiate(
                headerFlags,
                dialects,
                GetNegotiateSecurityMode(signingEnabledType, signingRequiredType),
                checker: (header, response) =>
                {
                    resSigningEnabledType =
                        response.SecurityMode.HasFlag(NEGOTIATE_Response_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED) ?
                        SigningEnabledType.SigningEnabledSet : SigningEnabledType.SigningEnabledNotSet;
                    resSigningRequiredType =
                        response.SecurityMode.HasFlag(NEGOTIATE_Response_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED) ?
                        SigningRequiredType.SigningRequiredSet : SigningRequiredType.SigningRequiredNotSet;
                });

            NegotiateResponse((ModelSmb2Status)status, resSigningEnabledType, resSigningRequiredType, signingConfig);
        }

        public void SessionSetupRequest(SigningFlagType signingFlagType, SigningEnabledType signingEnabledType, SigningRequiredType signingRequiredType, UserType userType)
        {
            SigningModelSessionId modelSessionId = SigningModelSessionId.ZeroSessionId;
            SessionFlags_Values sessionFlag = SessionFlags_Values.NONE;
            SigningFlagType responseSigningFlagType = SigningFlagType.SignedFlagNotSet;
            Packet_Header_Flags_Values headerFlags = (signingFlagType == SigningFlagType.SignedFlagSet) ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE;

            uint status = testClient.SessionSetup(
                headerFlags,
                testConfig.DefaultSecurityPackage,
                testConfig.SutComputerName,
                GetAccountCredential(userType),
                true,
                GetSessionSetupSecurityMode(signingEnabledType, signingRequiredType),
                checker: (header, response) =>
                {
                    modelSessionId = GetModelSessionId(header.SessionId);
                    responseSigningFlagType = GetSigningFlagType(header.Flags);
                    sessionFlag = response.SessionFlags;
                });

            SessionSetupResponse((ModelSmb2Status)status, modelSessionId, responseSigningFlagType, sessionFlag, signingConfig);
        }

        public void TreeConnectRequest(SigningFlagType signingFlagType)
        {
            uint treeId;
            SigningModelSessionId modelSessionId = SigningModelSessionId.ZeroSessionId;
            SigningFlagType responseSigningFlagType = SigningFlagType.SignedFlagNotSet;
            string sharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            Packet_Header_Flags_Values headerFlags = (signingFlagType == SigningFlagType.SignedFlagSet) ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE;

            // Inform SDK to disable/enable signing according to SigningFlagType.
            bool isEnableSigning = !(signingFlagType == SigningFlagType.SignedFlagNotSet);
            testClient.EnableSessionSigningAndEncryption(enableSigning: isEnableSigning, enableEncryption: false);
           
            uint status = testClient.TreeConnect(
                headerFlags,
                sharePath,
                out treeId,
                checker: (header, response) =>
                {
                    modelSessionId = GetModelSessionId(header.SessionId);
                    responseSigningFlagType = GetSigningFlagType(header.Flags);
                });

            TreeConnectResponse((ModelSmb2Status)status, modelSessionId, responseSigningFlagType);
        }
        #endregion

        #region Private Methods
        private AccountCredential GetAccountCredential(UserType userType)
        {
            AccountCredential accountCredential;
            switch (userType)
            {
                case UserType.Administrator:
                    accountCredential = testConfig.AccountCredential;
                    break;
                case UserType.Guest:
                    accountCredential = testConfig.GuestAccountCredential;
                    break;
                default:
                    throw new ArgumentException("The UserType is invalid", "userType");
            }

            return accountCredential;
        }

        private SigningModelSessionId GetModelSessionId(ulong sessionId)
        {
            return (sessionId == 0) ? SigningModelSessionId.ZeroSessionId : SigningModelSessionId.NonZeroSessionId;
        }

        private SecurityMode_Values GetNegotiateSecurityMode(SigningEnabledType signingEnabledType, SigningRequiredType signingRequiredType)
        {
            SecurityMode_Values securityMode = SecurityMode_Values.NONE;
            if (signingEnabledType == SigningEnabledType.SigningEnabledSet)
            {
                securityMode |= SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED;
            }
            if (signingRequiredType == SigningRequiredType.SigningRequiredSet)
            {
                securityMode |= SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED;
            }

            return securityMode;
        }

        private SESSION_SETUP_Request_SecurityMode_Values GetSessionSetupSecurityMode(SigningEnabledType signingEnabledType, SigningRequiredType signingRequiredType)
        {
            SESSION_SETUP_Request_SecurityMode_Values securityMode = SESSION_SETUP_Request_SecurityMode_Values.NONE;
            if (signingEnabledType == SigningEnabledType.SigningEnabledSet)
            {
                securityMode |= SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED;
            }
            if (signingRequiredType == SigningRequiredType.SigningRequiredSet)
            {
                securityMode |= SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED;
            }

            return securityMode;
        }

        private SigningFlagType GetSigningFlagType(Packet_Header_Flags_Values flags)
        {
            SigningFlagType signingFlag;

            if (flags.HasFlag(Packet_Header_Flags_Values.FLAGS_SIGNED))
            {
                signingFlag = SigningFlagType.SignedFlagSet;
            }
            else
            {
                signingFlag = SigningFlagType.SignedFlagNotSet;
            }

            return signingFlag;
        }
        #endregion
    }
}
