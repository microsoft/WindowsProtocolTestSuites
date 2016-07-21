// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using System;
using System.Collections.Generic;
using MsLdap = System.DirectoryServices.Protocols;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// Encoder class which creates requests/responses for LDAP v2.
    /// </summary>
    internal class AdtsLdapV2Encoder : AdtsLdapEncoder
    {
        /// <summary>
        /// The LDAP v2 encoder uses this version.
        /// </summary>
        private const AdtsLdapVersion version = AdtsLdapVersion.V2;

        #region Helper methods

        /// <summary>
        /// Creates a BindRequestPacket with context and BindRequest.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="bindRequest">The BindRequest message.</param>
        /// <returns>The BindRequestPacket.</returns>
        private AdtsBindRequestPacket CreateBindRequestPacket(
            AdtsLdapContext context,
            BindRequest bindRequest)
        {
            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.bindRequest, bindRequest);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);

            AdtsBindRequestPacket packet = new AdtsBindRequestPacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates a _SetOfAttributeValue instance that contains a set of values.
        /// </summary>
        /// <param name="values">The values to be added.</param>
        /// <returns>The _SetOfAttributeValue instance. If values is null, an empty set will be returned.</returns>
        private Asn1SetOf<AttributeValue> CreateAttributeValueSet(string[] values)
        {
            int length = (values != null) ? values.Length : 0;
            AttributeValue[] attributeValueArray = new AttributeValue[length];
            for (int i = 0; i < length; i++)
            {
                attributeValueArray[i] = new AttributeValue(values[i]);
            }

            Asn1SetOf<AttributeValue> valueSet = new Asn1SetOf<AttributeValue>(attributeValueArray);

            return valueSet;
        }

        #endregion Helper methods

        #region Requests
        /// <summary>
        /// Creates a BindRequest with simple bind for Active Directory Domain Services(AD DS).
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="username">User name which doesn't include domain prefix.</param>
        /// <param name="password">Password of user.</param>
        /// <param name="domainNetbiosName">NetBIOS domain name(with suffix like ".com" removed).</param>
        /// <returns>The packet that contains the request.</returns>
        /// <exception cref="ArgumentNullException">Thrown when username is null.</exception>
        internal override AdtsBindRequestPacket CreateSimpleBindRequest(
            AdtsLdapContext context,
            string username,
            string password,
            string domainNetbiosName)
        {
            if (username == null)
            {
                throw new ArgumentNullException("username");
            }

            BindRequest_authentication authentication = new BindRequest_authentication();
            authentication.SetData(BindRequest_authentication.simple, new Asn1OctetString(password ?? string.Empty));

            string fullname = (domainNetbiosName != null) ? (domainNetbiosName + "\\" + username) : username;
            BindRequest bindRequest = new BindRequest(
                new Asn1Integer((long)version),
                new LDAPDN(fullname),
                authentication);

            return CreateBindRequestPacket(context, bindRequest);
        }


        /// <summary>
        /// Creates a BindRequest with simple bind.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="name">
        /// The name field of BindRequest, see TD Section 5.1.1.1.1 for full list of legal names.
        /// </param>
        /// <param name="password">The password credential of the object.</param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsBindRequestPacket CreateSimpleBindRequest(
            AdtsLdapContext context,
            string name,
            string password)
        {
            BindRequest_authentication authentication = new BindRequest_authentication();
            authentication.SetData(BindRequest_authentication.simple, new Asn1OctetString(password ?? string.Empty));

            BindRequest bindRequest = new BindRequest(
                new Asn1Integer((long)version),
                new LDAPDN(name ?? string.Empty),
                authentication);

            return CreateBindRequestPacket(context, bindRequest);
        }


        /// <summary>
        /// Creates a BindRequest with SASL bind. This method is for LDAP v3 only.
        /// Note that for GSS-SPNEGO with NTLM, two rounds of bind requests is required.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="mechanism">Authentication mechanism used, e.g., GSS-SPNEGO, etc.</param>
        /// <param name="credential">The credential to be sent, it can be calculated with SSPI.</param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsBindRequestPacket CreateSaslBindRequest(
            AdtsLdapContext context,
            string mechanism,
            byte[] credential)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Creates a sicily package discovery BindRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <returns>The sicily package discovery BindRequest.</returns>
        internal override AdtsBindRequestPacket CreateSicilyPackageDiscoveryBindRequest(AdtsLdapContext context)
        {
            BindRequest_authentication authentication = new BindRequest_authentication();
            authentication.SetData(BindRequest_authentication.sicilyPackageDiscovery, new Asn1OctetString(new byte[0]));

            BindRequest bindRequest = new BindRequest(
                new Asn1Integer((long)version),
                new LDAPDN(string.Empty),    // For Sicily package discovery, DN is not required.
                authentication);

            return CreateBindRequestPacket(context, bindRequest);
        }


        /// <summary>
        /// Creates a sicily BindRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="authName">Authentication name of the request.</param>
        /// <param name="credential">The credential to be sent, it can be calculated with SSPI.</param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsBindRequestPacket CreateSicilyNegotiateBindRequest(
            AdtsLdapContext context,
            string authName,
            byte[] credential)
        {
            BindRequest_authentication authentication = new BindRequest_authentication();
            authentication.SetData(BindRequest_authentication.sicilyNegotiate, new Asn1OctetString(credential ?? (new byte[0])));

            BindRequest bindRequest = new BindRequest(
                new Asn1Integer((long)version),
                new LDAPDN(authName ?? string.Empty),
                authentication);

            return CreateBindRequestPacket(context, bindRequest);
        }


        /// <summary>
        /// Creates a sicily response BindRequest packet. Usually it's the last packet of authentication during
        /// the bind process.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="credential">The credential to be sent, it can be calculated with SSPI.</param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsBindRequestPacket CreateSicilyResponseBindRequest(
            AdtsLdapContext context,
            byte[] credential)
        {
            BindRequest_authentication authentication = new BindRequest_authentication();
            authentication.SetData(BindRequest_authentication.sicilyResponse, new Asn1OctetString(credential ?? (new byte[0])));

            BindRequest bindRequest = new BindRequest(
                new Asn1Integer((long)version),
                new LDAPDN(string.Empty),
                authentication);

            return CreateBindRequestPacket(context, bindRequest);
        }


        /// <summary>
        /// Creates an AddRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="objectDn">The DN of the object to be added.</param>
        /// <param name="attributes">Attributes to be set.</param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsAddRequestPacket CreateAddRequest(
            AdtsLdapContext context,
            string objectDn,
            params KeyValuePair<string, string[]>[] attributes)
        {
            int length = (attributes != null) ? attributes.Length : 0;

            AddRequest_attrs_element[] addrequestAttrsArray = new AddRequest_attrs_element[length];
            for (int i = 0; i < length; i++)
            {
                addrequestAttrsArray[i] = new AddRequest_attrs_element(
                    new AttributeType(attributes[i].Key),
                    CreateAttributeValueSet(attributes[i].Value));
            }
            Asn1SequenceOf<AddRequest_attrs_element> attributeList = new Asn1SequenceOf<AddRequest_attrs_element>(addrequestAttrsArray);
            
            AddRequest addRequest = new AddRequest(
                new LDAPDN(objectDn ?? string.Empty),
                attributeList);

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.addRequest, addRequest);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);

            AdtsAddRequestPacket packet = new AdtsAddRequestPacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates a DelRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="objectDn">The DN of the object to be deleted.</param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsDelRequestPacket CreateDelRequest(AdtsLdapContext context, string objectDn)
        {
            DelRequest delRequest = new DelRequest(objectDn ?? string.Empty);

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.delRequest, delRequest);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            LDAPMessage messasge = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsDelRequestPacket packet = new AdtsDelRequestPacket();
            packet.ldapMessagev2 = messasge;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates an AbandonRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="messageId">The ID of message to be abandoned.</param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsAbandonRequestPacket CreateAbandonRequest(AdtsLdapContext context, long messageId)
        {
            AbandonRequest abandonRequest = new AbandonRequest(messageId);

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.abandonRequest, abandonRequest);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsAbandonRequestPacket packet = new AdtsAbandonRequestPacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates a CompareRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="objectDn">The DN of the object to be compared.</param>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <param name="attributeValue">The value of the attribute.</param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsCompareRequestPacket CreateCompareRequest(
            AdtsLdapContext context,
            string objectDn,
            string attributeName,
            string attributeValue)
        {
            CompareRequest compareRequest = new CompareRequest(
                new LDAPDN(objectDn ?? string.Empty),
                new AttributeValueAssertion(
                    new AttributeType(attributeName ?? string.Empty),
                    new AttributeValue(attributeValue ?? string.Empty)));

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.compareDNRequest, compareRequest);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsCompareRequestPacket packet = new AdtsCompareRequestPacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates an ExtendedRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="requestName">The request name of the extended operation.</param>
        /// <param name="requestValue">The request value of the extended operation.</param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsExtendedRequestPacket CreateExtendedRequest(
            AdtsLdapContext context,
            string requestName,
            byte[] requestValue)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Creates an UnbindRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsUnbindRequestPacket CreateUnbindRequest(AdtsLdapContext context)
        {
            UnbindRequest unbindRequest = new UnbindRequest();

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.unbindRequest, unbindRequest);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsUnbindRequestPacket packet = new AdtsUnbindRequestPacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates a ModifyDNRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="oldDn">The original DN to be modified.</param>
        /// <param name="newRdn">The new relative DN.</param>
        /// <param name="newParentDn">
        /// The new parent DN. For LDAP v3 only. Ignored when creating LDAP v2 requests.
        /// </param>
        /// <param name="delOldRdn">
        /// Whether to delete old RDN. For LDAP v3 only. Ignored when creating LDAP v2 requests.
        /// </param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsModifyDnRequestPacket CreateModifyDnRequest(
            AdtsLdapContext context,
            string oldDn,
            string newRdn,
            string newParentDn,
            bool delOldRdn)
        {
            ModifyDNRequest modifyDnRequest = new ModifyDNRequest(
                new LDAPDN(oldDn ?? string.Empty),
                new RelativeLDAPDN(newRdn ?? string.Empty),
                new Asn1Boolean(delOldRdn),
                new LDAPDN(newParentDn));

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.modifyDNRequest, modifyDnRequest);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsModifyDnRequestPacket packet = new AdtsModifyDnRequestPacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates a ModifyRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="objectDn">The DN of object to be modified.</param>
        /// <param name="modificationList">Modification list of attributes.</param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsModifyRequestPacket CreateModifyRequest(
            AdtsLdapContext context,
            string objectDn,
            params MsLdap.DirectoryAttributeModification[] modificationList)
        {
            int length = (modificationList != null) ? modificationList.Length : 0;

            ModifyRequest_modifications_element[] modifyRequestArray = new ModifyRequest_modifications_element[length];
            for (int i = 0; i < length; i++)
            {
                modifyRequestArray[i] = new ModifyRequest_modifications_element(
                    new ModifyRequest_modifications_element_operation((long)modificationList[i].Operation),
                    new ModifyRequest_modifications_element_modification(
                        new AttributeType(modificationList[i].Name),
                        CreateAttributeValueSet((string[])modificationList[i].GetValues(typeof(string)))));
            }
            Asn1SequenceOf<ModifyRequest_modifications_element> modificationSequence = new Asn1SequenceOf<ModifyRequest_modifications_element>(modifyRequestArray);
            
            ModifyRequest modifyRequest = new ModifyRequest(
                new LDAPDN(objectDn ?? string.Empty),
                modificationSequence);

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.modifyRequest, modifyRequest);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsModifyRequestPacket packet = new AdtsModifyRequestPacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates a SearchRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="dn">The DN to be searched.</param>
        /// <param name="sizeLimit">Size limit.</param>
        /// <param name="timeLimit">Time limit, in seconds.</param>
        /// <param name="scope">Search scope. Base, single level, or subtree.</param>
        /// <param name="dereferenceAliases">Dereference aliase options.</param>
        /// <param name="filter">Search filter.</param>
        /// <param name="typesOnly">
        /// Specifies whether the search returns only the attribute names without the attribute values.
        /// </param>
        /// <param name="attributes">The attributes to be retrieved.</param>
        /// <returns>The packet that contains the request.</returns>
        internal override AdtsSearchRequestPacket CreateSearchRequest(
            AdtsLdapContext context,
            string dn,
            long sizeLimit,
            long timeLimit,
            MsLdap.SearchScope scope,
            MsLdap.DereferenceAlias dereferenceAliases,
            Asn1Choice filter,
            bool typesOnly,
            params string[] attributes)
        {
            int length = (attributes != null) ? attributes.Length : 0;

            AttributeType[] attributeTypeArray = new AttributeType[length];
            for (int i = 0; i < length; i++)
            {
                attributeTypeArray[i] = new AttributeType(attributes[i]);
            }
            Asn1SequenceOf<AttributeType> attributeList = new Asn1SequenceOf<AttributeType>(attributeTypeArray);
            
            SearchRequest searchRequest = new SearchRequest(
                new LDAPDN(dn ?? string.Empty),
                new SearchRequest_scope((long)scope),
                new SearchRequest_derefAliases((long)dereferenceAliases),
                new Asn1Integer(sizeLimit),
                new Asn1Integer(timeLimit),
                new Asn1Boolean(typesOnly),
                (Filter)filter,
                attributeList);

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.searchRequest, searchRequest);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsSearchRequestPacket packet = new AdtsSearchRequestPacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }

        #endregion Requests

        #region Responses

        /// <summary>
        /// Creates a BindResponse for normal bindings, SASL bindings and sicily bindings.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN. Required, but can be an empty string.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional and for LDAP v3 only.</param>
        /// <param name="serverCredentials">Server credentials, optional for normal bind.</param>
        /// <returns>The packet that contains the response.</returns>
        internal override AdtsBindResponsePacket CreateBindResponse(
            AdtsLdapContext context,
            MsLdap.ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral,
            byte[] serverCredentials)
        {
            BindResponse bindResponse = new BindResponse(
                new LDAPResult_resultCode((long)resultCode),
                new LDAPDN(matchedDn ?? string.Empty),
                new LDAPString(errorMessage ?? string.Empty));

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.bindResponse, bindResponse);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsBindResponsePacket packet = new AdtsBindResponsePacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates a SicilyBindResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="serverCredentials">Server credentials, optional for normal and sicily bind.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <returns>The packet that contains the response.</returns>
        internal override AdtsSicilyBindResponsePacket CreateSicilyBindResponse(
            AdtsLdapContext context,
            MsLdap.ResultCode resultCode,
            byte[] serverCredentials,
            string errorMessage)
        {
            SicilyBindResponse sicilyResponse = new SicilyBindResponse(
                new SicilyBindResponse_resultCode((long)resultCode),
                new Asn1OctetString(serverCredentials ?? (new byte[0])),
                new LDAPString(errorMessage ?? string.Empty));

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.sicilyResponse, sicilyResponse);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsSicilyBindResponsePacket packet = new AdtsSicilyBindResponsePacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates an AddResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        internal override AdtsAddResponsePacket CreateAddResponse(
            AdtsLdapContext context,
            MsLdap.ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            AddResponse addResponse = new AddResponse(
                new LDAPResult_resultCode((long)resultCode),
                new LDAPDN(matchedDn ?? string.Empty),
                new LDAPString(errorMessage ?? string.Empty));

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.addResponse, addResponse);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsAddResponsePacket packet = new AdtsAddResponsePacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates a DelResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        internal override AdtsDelResponsePacket CreateDelResponse(
            AdtsLdapContext context,
            MsLdap.ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            DelResponse delResponse = new DelResponse(
                new LDAPResult_resultCode((long)resultCode),
                new LDAPDN(matchedDn ?? string.Empty),
                new LDAPString(errorMessage ?? string.Empty));

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.delResponse, delResponse);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsDelResponsePacket packet = new AdtsDelResponsePacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates a ModifyResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        internal override AdtsModifyResponsePacket CreateModifyResponse(
            AdtsLdapContext context,
            MsLdap.ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            ModifyResponse modifyResponse = new ModifyResponse(
                new LDAPResult_resultCode((long)resultCode),
                new LDAPDN(matchedDn ?? string.Empty),
                new LDAPString(errorMessage ?? string.Empty));

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.modifyResponse, modifyResponse);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsModifyResponsePacket packet = new AdtsModifyResponsePacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates a ModifyDnResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        internal override AdtsModifyDnResponsePacket CreateModifyDnResponse(
            AdtsLdapContext context,
            MsLdap.ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            ModifyDNResponse modifyDnResponse = new ModifyDNResponse(
                new LDAPResult_resultCode((long)resultCode),
                new LDAPDN(matchedDn ?? string.Empty),
                new LDAPString(errorMessage ?? string.Empty));

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.modifyDNResponse, modifyDnResponse);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsModifyDnResponsePacket packet = new AdtsModifyDnResponsePacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates a CompareResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        internal override AdtsCompareResponsePacket CreateCompareResponse(
            AdtsLdapContext context,
            MsLdap.ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            CompareResponse compareResponse = new CompareResponse(
                new LDAPResult_resultCode((long)resultCode),
                new LDAPDN(matchedDn ?? string.Empty),
                new LDAPString(errorMessage ?? string.Empty));

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.compareDNResponse, compareResponse);

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsCompareResponsePacket packet = new AdtsCompareResponsePacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates an ExtendedResponse packet. For LDAP v3 only.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        internal override AdtsExtendedResponsePacket CreateExtendedResponse(
            AdtsLdapContext context,
            MsLdap.ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Creates a SearchResultEntry packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="attributes">The attributes and values that are contained in the entry.</param>
        /// <returns>The packet that contains the response.</returns>
        internal override AdtsSearchResultEntryPacket CreateSearchedResultEntry(
            AdtsLdapContext context,
            string matchedDn,
            params KeyValuePair<string, string[]>[] attributes)
        {
            int length = (attributes != null) ? attributes.Length : 0;

            SearchResponse_entry_attributes_element[] attributesElementArray = new SearchResponse_entry_attributes_element[length];
            for (int i = 0; i < length; i++)
            {
                attributesElementArray[i] = new SearchResponse_entry_attributes_element(
                    new AttributeType(attributes[i].Key),
                    CreateAttributeValueSet(attributes[i].Value));
            }
            Asn1SequenceOf<SearchResponse_entry_attributes_element> attributesElements = new Asn1SequenceOf<SearchResponse_entry_attributes_element>(attributesElementArray);

            SearchResponse_entry entry = new SearchResponse_entry(
                new LDAPDN(matchedDn ?? string.Empty),
                attributesElements);

            LDAPMessage_protocolOp operation = new LDAPMessage_protocolOp();
            operation.SetData(LDAPMessage_protocolOp.searchResponse, new SearchResponse(SearchResponse.entry, entry));

            LDAPMessage message = new LDAPMessage(new MessageID(context.MessageId), operation);
            AdtsSearchResultEntryPacket packet = new AdtsSearchResultEntryPacket();
            packet.ldapMessagev2 = message;
            packet.messageId = context.MessageId;

            return packet;
        }


        /// <summary>
        /// Creates a SearchResultReference. For LDAP v3 only.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="referenceUrls">The referenced URLs.</param>
        /// <returns>The packet that contains the response.</returns>
        internal override AdtsSearchResultReferencePacket CreateSearchResultReference(
            AdtsLdapContext context,
            string[] referenceUrls)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Creates a SearchResultDone packet.  For LDAP v3 only.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional.</param>
        /// <returns>The packet that contains the response.</returns>
        internal override AdtsSearchResultDonePacket CreateSearchResultDone(
            AdtsLdapContext context,
            MsLdap.ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral)
        {
            throw new NotSupportedException();
        }
        #endregion Responses
    }
}
