// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// Encoder class which creates LDAP requests/responses.
    /// </summary>
    internal abstract class AdtsLdapEncoder
    {
        #region Requests
        /// <summary>
        /// Creates a BindRequest with simple bind for Active Directory Domain Services(AD DS).
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="username">User name which doesn't include domain prefix.</param>
        /// <param name="password">Password of user.</param>
        /// <param name="domainNetbiosName">NetBIOS domain name(with suffix like ".com" removed).</param>
        /// <returns>The packet that contains the request.</returns>
        internal abstract AdtsBindRequestPacket CreateSimpleBindRequest(
            AdtsLdapContext context,
            string username,
            string password,
            string domainNetbiosName);


        /// <summary>
        /// Creates a BindRequest with simple bind.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="name">
        /// The name field of BindRequest, see TD Section 5.1.1.1.1 for full list of legal names.
        /// </param>
        /// <param name="password">The password credential of the object.</param>
        /// <returns>The packet that contains the request.</returns>
        internal abstract AdtsBindRequestPacket CreateSimpleBindRequest(
            AdtsLdapContext context,
            string name,
            string password);


        /// <summary>
        /// Creates a BindRequest with SASL bind. This method is for LDAP v3 only.
        /// Note that for GSS-SPNEGO with NTLM, two rounds of bind requests is required.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="mechanism">Authentication mechanism used, e.g., GSS-SPNEGO, etc.</param>
        /// <param name="credential">The credential to be sent, it can be calculated with SSPI.</param>
        /// <returns>The packet that contains the request.</returns>
        internal abstract AdtsBindRequestPacket CreateSaslBindRequest(
            AdtsLdapContext context,
            string mechanism,
            byte[] credential);


        /// <summary>
        /// Creates a sicily package discovery BindRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <returns>The sicily package discovery BindRequest.</returns>
        internal abstract AdtsBindRequestPacket CreateSicilyPackageDiscoveryBindRequest(AdtsLdapContext context);


        /// <summary>
        /// Creates a sicily negotiate BindRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="authName">Authentication name of the request.</param>
        /// <param name="credential">The credential to be sent, it can be calculated with SSPI.</param>
        /// <returns>The packet that contains the request.</returns>
        internal abstract AdtsBindRequestPacket CreateSicilyNegotiateBindRequest(
            AdtsLdapContext context,
            string authName,
            byte[] credential);


        /// <summary>
        /// Creates a sicily response BindRequest packet. Usually it's the last packet of authentication during
        /// the bind process.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="credential">The credential to be sent, it can be calculated with SSPI.</param>
        /// <returns>The packet that contains the request.</returns>
        internal abstract AdtsBindRequestPacket CreateSicilyResponseBindRequest(
            AdtsLdapContext context,
            byte[] credential);


        /// <summary>
        /// Creates an AddRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="objectDn">The DN of the object to be added.</param>
        /// <param name="attributes">Attributes to be set.</param>
        /// <returns>The packet that contains the request.</returns>
        internal abstract AdtsAddRequestPacket CreateAddRequest(
            AdtsLdapContext context,
            string objectDn,
            params KeyValuePair<string, string[]>[] attributes);


        /// <summary>
        /// Creates a DelRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="objectDn">The DN of the object to be deleted.</param>
        /// <returns>The packet that contains the request.</returns>
        internal abstract AdtsDelRequestPacket CreateDelRequest(AdtsLdapContext context, string objectDn);


        /// <summary>
        /// Creates an AbandonRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="messageId">The ID of message to be abandoned.</param>
        /// <returns>The packet that contains the request.</returns>
        internal abstract AdtsAbandonRequestPacket CreateAbandonRequest(AdtsLdapContext context, long messageId);


        /// <summary>
        /// Creates a CompareRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="objectDn">The DN of the object to be compared.</param>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <param name="attributeValue">The value of the attribute.</param>
        /// <returns>The packet that contains the request.</returns>
        internal abstract AdtsCompareRequestPacket CreateCompareRequest(
            AdtsLdapContext context,
            string objectDn,
            string attributeName,
            string attributeValue);


        /// <summary>
        /// Creates an ExtendedRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="requestName">The request name of the extended operation.</param>
        /// <param name="requestValue">The request value of the extended operation.</param>
        /// <returns>The packet that contains the request.</returns>
        internal abstract AdtsExtendedRequestPacket CreateExtendedRequest(
            AdtsLdapContext context,
            string requestName,
            byte[] requestValue);


        /// <summary>
        /// Creates an UnbindRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <returns>The packet that contains the request.</returns>
        internal abstract AdtsUnbindRequestPacket CreateUnbindRequest(AdtsLdapContext context);


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
        internal abstract AdtsModifyDnRequestPacket CreateModifyDnRequest(
            AdtsLdapContext context,
            string oldDn,
            string newRdn,
            string newParentDn,
            bool delOldRdn);


        /// <summary>
        /// Creates a ModifyRequest packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="objectDn">The DN of object to be modified.</param>
        /// <param name="modificationList">Modification list of attributes.</param>
        /// <returns>The packet that contains the request.</returns>
        internal abstract AdtsModifyRequestPacket CreateModifyRequest(
            AdtsLdapContext context,
            string objectDn,
            params DirectoryAttributeModification[] modificationList);


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
        internal abstract AdtsSearchRequestPacket CreateSearchRequest(
            AdtsLdapContext context,
            string dn,
            long sizeLimit,
            long timeLimit,
            SearchScope scope,
            DereferenceAlias dereferenceAliases,
            Asn1Choice filter,
            bool typesOnly,
            params string[] attributes);

        #endregion Requests

        #region Responses

        /// <summary>
        /// Creates a BindResponse for normal bindings and SASL bindings.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN. Required, but can be an empty string.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional and for LDAP v3 only.</param>
        /// <param name="serverCredentials">Server credentials, optional for normal bind.</param>
        /// <returns>The packet that contains the response.</returns>
        internal abstract AdtsBindResponsePacket CreateBindResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral,
            byte[] serverCredentials);


        /// <summary>
        /// Creates a SicilyBindResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="serverCredentials">Server credentials, optional for normal and sicily bind.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <returns>The packet that contains the response.</returns>
        internal abstract AdtsSicilyBindResponsePacket CreateSicilyBindResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            byte[] serverCredentials,
            string errorMessage);


        /// <summary>
        /// Creates an AddResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        internal abstract AdtsAddResponsePacket CreateAddResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral);


        /// <summary>
        /// Creates a DelResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        internal abstract AdtsDelResponsePacket CreateDelResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral);


        /// <summary>
        /// Creates a ModifyResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        internal abstract AdtsModifyResponsePacket CreateModifyResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral);


        /// <summary>
        /// Creates a ModifyDnResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        internal abstract AdtsModifyDnResponsePacket CreateModifyDnResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral);


        /// <summary>
        /// Creates a CompareResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        internal abstract AdtsCompareResponsePacket CreateCompareResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral);


        /// <summary>
        /// Creates an ExtendedResponse packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional. Used for LDAP v3 only.</param>
        /// <returns>The packet that contains the response.</returns>
        internal abstract AdtsExtendedResponsePacket CreateExtendedResponse(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral);


        /// <summary>
        /// Creates a SearchResultEntry packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="attributes">The attributes and values that are contained in the entry.</param>
        /// <returns>The packet that contains the response.</returns>
        internal abstract AdtsSearchResultEntryPacket CreateSearchedResultEntry(
            AdtsLdapContext context,
            string matchedDn,
            params KeyValuePair<string, string[]>[] attributes);


        /// <summary>
        /// Creates a SearchResultReference. For LDAP v3 only.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="referenceUrls">The referenced URLs.</param>
        /// <returns>The packet that contains the response.</returns>
        internal abstract AdtsSearchResultReferencePacket CreateSearchResultReference(
            AdtsLdapContext context,
            string[] referenceUrls);


        /// <summary>
        /// Creates a SearchResultDone packet.
        /// </summary>
        /// <param name="context">The user context which contains message ID.</param>
        /// <param name="resultCode">Result code of previous request, as specified in RFC 2251.</param>
        /// <param name="matchedDn">Matched DN.</param>
        /// <param name="errorMessage">Error message for result code. Required.</param>
        /// <param name="referral">Referral. Optional.</param>
        /// <returns>The packet that contains the response.</returns>
        internal abstract AdtsSearchResultDonePacket CreateSearchResultDone(
            AdtsLdapContext context,
            ResultCode resultCode,
            string matchedDn,
            string errorMessage,
            string[] referral);
        #endregion Responses
    }
}