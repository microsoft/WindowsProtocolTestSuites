// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa
{
    /// <summary>
    /// A class contains context information of a LSA session
    /// </summary>
    public class LsaServerSessionContext
    {
        private LsaRequestStub requestReceived;

        private RpceServerSessionContext underlyingSessionContext;
        
        private Dictionary<IntPtr, ACCESS_MASK> policyContext;

        private Dictionary<IntPtr, ACCESS_MASK> accountContext;

        private Dictionary<IntPtr, ACCESS_MASK> secretContext;

        private Dictionary<IntPtr, ACCESS_MASK> trustedDomainContext;

        /// <summary>
        /// The last received request.
        /// </summary>
        internal LsaRequestStub RequestReceived
        {
            get
            {
                return requestReceived;
            }
        }


        /// <summary>
        /// The policy handle and access mask dictionary.
        /// </summary>
        [CLSCompliant(false)]
        public ReadOnlyDictionary<IntPtr, ACCESS_MASK> PolicyContext
        {
            get
            {
                return new ReadOnlyDictionary<IntPtr,ACCESS_MASK>(policyContext);
            }
        }


        /// <summary>
        /// The account handle and access mask dictionary.
        /// </summary>
        [CLSCompliant(false)]
        public ReadOnlyDictionary<IntPtr, ACCESS_MASK> AccountContext
        {
            get
            {
                return new ReadOnlyDictionary<IntPtr, ACCESS_MASK>(accountContext);
            }
        }


        /// <summary>
        /// The secret handle and access mask dictionary.
        /// </summary>
        [CLSCompliant(false)]
        public ReadOnlyDictionary<IntPtr, ACCESS_MASK> SecretContext
        {
            get
            {
                return new ReadOnlyDictionary<IntPtr, ACCESS_MASK>(secretContext);
            }
        }


        /// <summary>
        /// The trusted domain handle and access mask dictionary.
        /// </summary>
        [CLSCompliant(false)]
        public ReadOnlyDictionary<IntPtr, ACCESS_MASK> TrustedDomainContext
        {
            get
            {
                return new ReadOnlyDictionary<IntPtr, ACCESS_MASK>(trustedDomainContext);
            }
        }


        /// <summary>
        /// The corresponding RPCE layer session context
        /// </summary>
        [CLSCompliant(false)]
        public RpceServerSessionContext RpceLayerSessionContext
        {
            get
            {
                return underlyingSessionContext;
            }
            internal set
            {
                underlyingSessionContext = value;
            }
        }


        /// <summary>
        /// Session key.
        /// </summary>
        public byte[] SessionKey
        {
            get
            {
                if (underlyingSessionContext != null
                    && underlyingSessionContext.FileServiceTransportOpen != null
                    && underlyingSessionContext.FileServiceTransportOpen.TreeConnect != null
                    && underlyingSessionContext.FileServiceTransportOpen.TreeConnect.Session != null)
                {
                    return underlyingSessionContext.FileServiceTransportOpen.TreeConnect.Session.SessionKey4Smb;
                }
                return null;
            }
        }


        /// <summary>
        /// Initialize a LSA server session context class.
        /// </summary>
        internal LsaServerSessionContext()
        {
            policyContext = new Dictionary<IntPtr, ACCESS_MASK>();
            accountContext = new Dictionary<IntPtr, ACCESS_MASK>();
            secretContext = new Dictionary<IntPtr, ACCESS_MASK>();
            trustedDomainContext = new Dictionary<IntPtr, ACCESS_MASK>();
        }


        /// <summary>
        ///  Update the session context after receiving a request from the client.
        /// </summary>
        /// <param name="messageReceived">The LSA request received</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        internal void UpdateSessionContextWithMessageReceived(LsaRequestStub messageReceived)
        {
            requestReceived = messageReceived;
        }


        /// <summary>
        ///  Update the session context before sending a response to the client.
        /// </summary>
        /// <param name="messageToSend">The LSA response to be sent</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        internal void UpdateSessionContextWithMessageSent(LsaResponseStub messageToSend)
        {
            switch (messageToSend.Opnum)
            {
                case LsaMethodOpnums.LsarClose:
                    HandleLsarCloseResponse(messageToSend);
                    break;
                case LsaMethodOpnums.LsarOpenPolicy:
                    HandleLsarOpenPolicyResponse(messageToSend);
                    break;
                case LsaMethodOpnums.LsarCreateAccount:
                    HandleLsarCreateAccountResponse(messageToSend);
                    break;
                case LsaMethodOpnums.LsarCreateTrustedDomain:
                    HandleLsarCreateTrustedDomainResponse(messageToSend);
                    break;
                case LsaMethodOpnums.LsarCreateSecret:
                    HandleLsarCreateSecretResponse(messageToSend);
                    break;
                case LsaMethodOpnums.LsarOpenAccount:
                    HandleLsarOpenAccountResponse(messageToSend);
                    break;
                case LsaMethodOpnums.LsarOpenTrustedDomain:
                    HandleLsarOpenTrustedDomainResponse(messageToSend);
                    break;
                case LsaMethodOpnums.LsarOpenSecret:
                    HandleLsarOpenSecretResponse(messageToSend);
                    break;
                case LsaMethodOpnums.LsarDeleteObject:
                    HandleLsarDeleteObjectResponse((LsarDeleteObjectResponse)messageToSend);
                    break;
                case LsaMethodOpnums.LsarOpenPolicy2:
                    HandleLsarOpenPolicy2Response(messageToSend);
                    break;
                case LsaMethodOpnums.LsarCreateTrustedDomainEx:
                    HandleLsarCreateTrustedDomainExResponse(messageToSend);
                    break;
                case LsaMethodOpnums.LsarOpenTrustedDomainByName:
                    HandleLsarOpenTrustedDomainByNameResponse(messageToSend);
                    break;
                case LsaMethodOpnums.LsarCreateTrustedDomainEx2:
                    HandleLsarCreateTrustedDomainEx2Response(messageToSend);
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Handle LsarClose response sent event.
        /// </summary>
        /// <param name="messageToSend">LsarCloseResponse</param>
        private void HandleLsarCloseResponse(LsaResponseStub messageToSend)
        {
            LsarCloseRequest request = requestReceived as LsarCloseRequest;
            LsarCloseResponse response = messageToSend as LsarCloseResponse;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    if (policyContext.ContainsKey(request.ObjectHandle.Value))
                    {
                        policyContext.Remove(request.ObjectHandle.Value);
                    }
                    else if (accountContext.ContainsKey(request.ObjectHandle.Value))
                    {
                        accountContext.Remove(request.ObjectHandle.Value);
                    }
                    else if (secretContext.ContainsKey(request.ObjectHandle.Value))
                    {
                        secretContext.Remove(request.ObjectHandle.Value);
                    }
                    else if (trustedDomainContext.ContainsKey(request.ObjectHandle.Value))
                    {
                        trustedDomainContext.Remove(request.ObjectHandle.Value);
                    }
                }
            }
        }


        /// <summary>
        /// Handle LsarOpenPolicy response event.
        /// </summary>
        /// <param name="messageToSend">LsarOpenPolicyResponse</param>
        private void HandleLsarOpenPolicyResponse(LsaResponseStub messageToSend)
        {
            LsarOpenPolicyRequest request = requestReceived as LsarOpenPolicyRequest;
            LsarOpenPolicyResponse response = messageToSend as LsarOpenPolicyResponse;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    policyContext.Add(response.PolicyHandle, request.DesiredAccess);
                }
            }
        }


        /// <summary>
        /// Handle LsarOpenPolicy2 response event.
        /// </summary>
        /// <param name="messageToSend">LsarOpenPolicy2Response</param>
        private void HandleLsarOpenPolicy2Response(LsaResponseStub messageToSend)
        {
            LsarOpenPolicy2Request request = requestReceived as LsarOpenPolicy2Request;
            LsarOpenPolicy2Response response = messageToSend as LsarOpenPolicy2Response;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    policyContext.Add(response.PolicyHandle, request.DesiredAccess);
                }
            }
        }


        /// <summary>
        /// Handle LsarCreateAccount response event.
        /// </summary>
        /// <param name="messageToSend">LsarCreateAccountResponse</param>
        private void HandleLsarCreateAccountResponse(LsaResponseStub messageToSend)
        {
            LsarCreateAccountRequest request = requestReceived as LsarCreateAccountRequest;
            LsarCreateAccountResponse response = messageToSend as LsarCreateAccountResponse;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    accountContext.Add(response.AccountHandle, request.DesiredAccess);
                }
            }
        }


        /// <summary>
        /// Handle LsarCreateTrustedDomain response event.
        /// </summary>
        /// <param name="messageToSend">LsarCreateTrustedDomainResponse</param>
        private void HandleLsarCreateTrustedDomainResponse(LsaResponseStub messageToSend)
        {
            LsarCreateTrustedDomainRequest request = requestReceived as LsarCreateTrustedDomainRequest;
            LsarCreateTrustedDomainResponse response = messageToSend as LsarCreateTrustedDomainResponse;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    trustedDomainContext.Add(response.TrustedDomainHandle, request.DesiredAccess);
                }
            }
        }


        /// <summary>
        /// Handle LsarCreateSecret response event.
        /// </summary>
        /// <param name="messageToSend">LsarCreateSecretResponse</param>
        private void HandleLsarCreateSecretResponse(LsaResponseStub messageToSend)
        {
            LsarCreateSecretRequest request = requestReceived as LsarCreateSecretRequest;
            LsarCreateSecretResponse response = messageToSend as LsarCreateSecretResponse;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    secretContext.Add(response.SecretHandle, request.DesiredAccess);
                }
            }
        }


        /// <summary>
        /// Handle LsarOpenAccount response event.
        /// </summary>
        /// <param name="messageToSend">LsarOpenAccountResponse</param>
        private void HandleLsarOpenAccountResponse(LsaResponseStub messageToSend)
        {
            LsarOpenAccountRequest request = requestReceived as LsarOpenAccountRequest;
            LsarOpenAccountResponse response = messageToSend as LsarOpenAccountResponse;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    accountContext.Add(response.AccountHandle, request.DesiredAccess);
                }
            }
        }


        /// <summary>
        /// Handle LsarOpenTrustedDomain response event.
        /// </summary>
        /// <param name="messageToSend">LsarOpenTrustedDomainResponse</param>
        private void HandleLsarOpenTrustedDomainResponse(LsaResponseStub messageToSend)
        {
            LsarOpenTrustedDomainRequest request = requestReceived as LsarOpenTrustedDomainRequest;
            LsarOpenTrustedDomainResponse response = messageToSend as LsarOpenTrustedDomainResponse;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    trustedDomainContext.Add(response.TrustedDomainHandle, request.DesiredAccess);
                }
            }
        }


        /// <summary>
        /// Handle LsarOpenSecret response event.
        /// </summary>
        /// <param name="messageToSend">LsarOpenSecretResponse</param>
        private void HandleLsarOpenSecretResponse(LsaResponseStub messageToSend)
        {
            LsarOpenSecretRequest request = requestReceived as LsarOpenSecretRequest;
            LsarOpenSecretResponse response = messageToSend as LsarOpenSecretResponse;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    secretContext.Add(response.SecretHandle, request.DesiredAccess);
                }
            }
        }


        /// <summary>
        /// Handle LsarCreateTrustedDomainEx response event.
        /// </summary>
        /// <param name="messageToSend">LsarCreateTrustedDomainExResponse</param>
        private void HandleLsarCreateTrustedDomainExResponse(LsaResponseStub messageToSend)
        {
            LsarCreateTrustedDomainExRequest request = requestReceived as LsarCreateTrustedDomainExRequest;
            LsarCreateTrustedDomainExResponse response = messageToSend as LsarCreateTrustedDomainExResponse;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    trustedDomainContext.Add(response.TrustedDomainHandle, request.DesiredAccess);
                }
            }
        }


        /// <summary>
        /// Handle LsarOpenTrustedDomainByName response event.
        /// </summary>
        /// <param name="messageToSend">LsarOpenTrustedDomainByNameResponse</param>
        private void HandleLsarOpenTrustedDomainByNameResponse(LsaResponseStub messageToSend)
        {
            LsarOpenTrustedDomainByNameRequest request = requestReceived as LsarOpenTrustedDomainByNameRequest;
            LsarOpenTrustedDomainByNameResponse response = messageToSend as LsarOpenTrustedDomainByNameResponse;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    trustedDomainContext.Add(response.TrustedDomainHandle, request.DesiredAccess);
                }
            }
        }


        /// <summary>
        /// Handle LsarCreateTrustedDomainEx2 response event.
        /// </summary>
        /// <param name="messageToSend">LsarCreateTrustedDomainEx2Response</param>
        private void HandleLsarCreateTrustedDomainEx2Response(LsaResponseStub messageToSend)
        {
            LsarCreateTrustedDomainEx2Request request = requestReceived as LsarCreateTrustedDomainEx2Request;
            LsarCreateTrustedDomainEx2Response response = messageToSend as LsarCreateTrustedDomainEx2Response;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    trustedDomainContext.Add(response.TrustedDomainHandle, request.DesiredAccess);
                }
            }
        }


        /// <summary>
        /// Handle LsarDeleteObject response event.
        /// </summary>
        /// <param name="messageToSend">LsarDeleteObjectResponse</param>
        private void HandleLsarDeleteObjectResponse(LsaResponseStub messageToSend)
        {
            LsarDeleteObjectRequest request = requestReceived as LsarDeleteObjectRequest;
            LsarDeleteObjectResponse response = messageToSend as LsarDeleteObjectResponse;

            if (request != null && response != null)
            {
                if (response.Status == NtStatus.STATUS_SUCCESS)
                {
                    if (accountContext.ContainsKey(request.ObjectHandle.Value))
                    {
                        accountContext.Remove(request.ObjectHandle.Value);
                    }
                    else if (secretContext.ContainsKey(request.ObjectHandle.Value))
                    {
                        secretContext.Remove(request.ObjectHandle.Value);
                    }
                    else if (trustedDomainContext.ContainsKey(request.ObjectHandle.Value))
                    {
                        trustedDomainContext.Remove(request.ObjectHandle.Value);
                    }
                }
            }
        }
    }
}
