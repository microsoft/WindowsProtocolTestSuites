// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Samr
{
    /// <summary>
    /// Utility class that contains SAMR RPC constants.
    /// </summary>
    public static class SamrUtility
    {
        /// <summary>
        /// SAMR RPC interface UUID.
        /// </summary>
        public static readonly Guid SAMR_RPC_INTERFACE_UUID = new Guid("12345778-1234-ABCD-EF00-0123456789AC");

        /// <summary>
        /// SAMR RPC interface major version.
        /// </summary>
        public const ushort SAMR_RPC_INTERFACE_MAJOR_VERSION = 1;

        /// <summary>
        /// SAMR RPC interface minor version.
        /// </summary>
        public const ushort SAMR_RPC_INTERFACE_MINOR_VERSION = 0;

        /// <summary>
        /// SAMR RPC namedpipe well-known endpoint
        /// </summary>
        public const string SAMR_RPC_OVER_NP_WELLKNOWN_ENDPOINT = "\\PIPE\\samr";

        /// <summary>
        /// function query dynamic tcp endpoint of SAMR
        /// </summary>
        /// <param name="serverName">
        /// the serverName that is binded
        /// </param>
        /// <returns>
        /// dynamic tcp endpoint of SAMR
        /// </returns>
        public static ushort[] QuerySamrTcpEndpoint(string serverName)
        {
            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }
            return RpceEndpointMapper.QueryDynamicTcpEndpointByInterface(
                serverName,
                SAMR_RPC_INTERFACE_UUID,
                SAMR_RPC_INTERFACE_MAJOR_VERSION,
                SAMR_RPC_INTERFACE_MINOR_VERSION);
        }


        #region Create Samr Request Messages upon opnum
        /// <summary>
        /// Creates an instance of request stub upon opnum received
        /// </summary>
        /// <param name="opnum"> opnum received</param>
        /// <returns>an instance of request stub.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        internal static SamrRequestStub CreateSamrRequestStub(SamrMethodOpnums opnum)
        {
            SamrRequestStub requestStub = null;
            switch (opnum)
            {
                case SamrMethodOpnums.SamrConnect:
                    requestStub = new SamrConnectRequest();
                    break;

                case SamrMethodOpnums.SamrCloseHandle:
                    requestStub = new SamrCloseHandleRequest();
                    break;

                case SamrMethodOpnums.SamrSetSecurityObject:
                    requestStub = new SamrCloseHandleRequest();
                    break;

                case SamrMethodOpnums.SamrQuerySecurityObject:
                    requestStub = new SamrQuerySecurityObjectRequest();
                    break;

                case SamrMethodOpnums.Opnum4NotUsedOnWire:
                    requestStub = new Opnum4NotUsedOnWireRequest();
                    break;

                case SamrMethodOpnums.SamrLookupDomainInSamServer:
                    requestStub = new SamrLookupDomainInSamServerRequest();
                    break;

                case SamrMethodOpnums.SamrEnumerateDomainsInSamServer:
                    requestStub = new SamrEnumerateDomainsInSamServerRequest();
                    break;

                case SamrMethodOpnums.SamrOpenDomain:
                    requestStub = new SamrOpenDomainRequest();
                    break;

                case SamrMethodOpnums.SamrQueryInformationDomain:
                    requestStub = new SamrQueryInformationDomainRequest();
                    break;

                case SamrMethodOpnums.SamrSetInformationDomain:
                    requestStub = new SamrSetInformationDomainRequest();
                    break;

                case SamrMethodOpnums.SamrCreateGroupInDomain:
                    requestStub = new SamrCreateGroupInDomainRequest();
                    break;

                case SamrMethodOpnums.SamrEnumerateGroupsInDomain:
                    requestStub = new SamrEnumerateGroupsInDomainRequest();
                    break;

                case SamrMethodOpnums.SamrCreateUserInDomain:
                    requestStub = new SamrCreateUserInDomainRequest();
                    break;

                case SamrMethodOpnums.SamrEnumerateUsersInDomain:
                    requestStub = new SamrEnumerateUsersInDomainRequest();
                    break;

                case SamrMethodOpnums.SamrCreateAliasInDomain:
                    requestStub = new SamrCreateAliasInDomainRequest();
                    break;

                case SamrMethodOpnums.SamrEnumerateAliasesInDomain:
                    requestStub = new SamrEnumerateAliasesInDomainRequest();
                    break;

                case SamrMethodOpnums.SamrGetAliasMembership:
                    requestStub = new SamrGetAliasMembershipRequest();
                    break;

                case SamrMethodOpnums.SamrLookupNamesInDomain:
                    requestStub = new SamrLookupNamesInDomainRequest();
                    break;

                case SamrMethodOpnums.SamrLookupIdsInDomain:
                    requestStub = new SamrLookupIdsInDomainRequest();
                    break;

                case SamrMethodOpnums.SamrOpenGroup:
                    requestStub = new SamrOpenGroupRequest();
                    break;

                case SamrMethodOpnums.SamrQueryInformationGroup:
                    requestStub = new SamrQueryInformationGroupRequest();
                    break;

                case SamrMethodOpnums.SamrSetInformationGroup:
                    requestStub = new SamrSetInformationGroupRequest();
                    break;

                case SamrMethodOpnums.SamrAddMemberToGroup:
                    requestStub = new SamrAddMemberToGroupRequest();
                    break;

                case SamrMethodOpnums.SamrDeleteGroup:
                    requestStub = new SamrDeleteGroupRequest();
                    break;

                case SamrMethodOpnums.SamrRemoveMemberFromGroup:
                    requestStub = new SamrRemoveMemberFromGroupRequest();
                    break;

                case SamrMethodOpnums.SamrGetMembersInGroup:
                    requestStub = new SamrGetMembersInGroupRequest();
                    break;

                case SamrMethodOpnums.SamrSetMemberAttributesOfGroup:
                    requestStub = new SamrSetMemberAttributesOfGroupRequest();
                    break;

                case SamrMethodOpnums.SamrOpenAlias:
                    requestStub = new SamrOpenAliasRequest();
                    break;

                case SamrMethodOpnums.SamrQueryInformationAlias:
                    requestStub = new SamrQueryInformationAliasRequest();
                    break;

                case SamrMethodOpnums.SamrSetInformationAlias:
                    requestStub = new SamrSetInformationAliasRequest();
                    break;

                case SamrMethodOpnums.SamrDeleteAlias:
                    requestStub = new SamrDeleteAliasRequest();
                    break;

                case SamrMethodOpnums.SamrAddMemberToAlias:
                    requestStub = new SamrAddMemberToAliasRequest();
                    break;

                case SamrMethodOpnums.SamrRemoveMemberFromAlias:
                    requestStub = new SamrRemoveMemberFromAliasRequest();
                    break;

                case SamrMethodOpnums.SamrGetMembersInAlias:
                    requestStub = new SamrGetMembersInAliasRequest();
                    break;

                case SamrMethodOpnums.SamrOpenUser:
                    requestStub = new SamrOpenUserRequest();
                    break;

                case SamrMethodOpnums.SamrDeleteUser:
                    requestStub = new SamrDeleteUserRequest();
                    break;

                case SamrMethodOpnums.SamrQueryInformationUser:
                    requestStub = new SamrQueryInformationUserRequest();
                    break;

                case SamrMethodOpnums.SamrSetInformationUser:
                    requestStub = new SamrSetInformationUserRequest();
                    break;

                case SamrMethodOpnums.SamrChangePasswordUser:
                    requestStub = new SamrChangePasswordUserRequest();
                    break;

                case SamrMethodOpnums.SamrGetGroupsForUser:
                    requestStub = new SamrGetGroupsForUserRequest();
                    break;

                case SamrMethodOpnums.SamrQueryDisplayInformation:
                    requestStub = new SamrQueryDisplayInformationRequest();
                    break;

                case SamrMethodOpnums.SamrGetDisplayEnumerationIndex:
                    requestStub = new SamrGetDisplayEnumerationIndexRequest();
                    break;

                case SamrMethodOpnums.Opnum42NotUsedOnWire:
                    requestStub = new Opnum42NotUsedOnWireRequest();
                    break;

                case SamrMethodOpnums.Opnum43NotUsedOnWire:
                    requestStub = new Opnum43NotUsedOnWireRequest();
                    break;

                case SamrMethodOpnums.SamrGetUserDomainPasswordInformation:
                    requestStub = new SamrGetUserDomainPasswordInformationRequest();
                    break;

                case SamrMethodOpnums.SamrRemoveMemberFromForeignDomain:
                    requestStub = new SamrRemoveMemberFromForeignDomainRequest();
                    break;

                case SamrMethodOpnums.SamrQueryInformationDomain2:
                    requestStub = new SamrQueryInformationDomain2Request();
                    break;

                case SamrMethodOpnums.SamrQueryInformationUser2:
                    requestStub = new SamrQueryInformationUser2Request();
                    break;

                case SamrMethodOpnums.SamrQueryDisplayInformation2:
                    requestStub = new SamrQueryDisplayInformation2Request();
                    break;

                case SamrMethodOpnums.SamrGetDisplayEnumerationIndex2:
                    requestStub = new SamrGetDisplayEnumerationIndex2Request();
                    break;

                case SamrMethodOpnums.SamrCreateUser2InDomain:
                    requestStub = new SamrCreateUser2InDomainRequest();
                    break;

                case SamrMethodOpnums.SamrQueryDisplayInformation3:
                    requestStub = new SamrQueryDisplayInformation3Request();
                    break;

                case SamrMethodOpnums.SamrAddMultipleMembersToAlias:
                    requestStub = new SamrAddMultipleMembersToAliasRequest();
                    break;

                case SamrMethodOpnums.SamrRemoveMultipleMembersFromAlias:
                    requestStub = new SamrRemoveMultipleMembersFromAliasRequest();
                    break;

                case SamrMethodOpnums.SamrOemChangePasswordUser2:
                    requestStub = new SamrOemChangePasswordUser2Request();
                    break;

                case SamrMethodOpnums.SamrUnicodeChangePasswordUser2:
                    requestStub = new SamrUnicodeChangePasswordUser2Request();
                    break;

                case SamrMethodOpnums.SamrGetDomainPasswordInformation:
                    requestStub = new SamrGetDomainPasswordInformationRequest();
                    break;

                case SamrMethodOpnums.SamrConnect2:
                    requestStub = new SamrConnect2Request();
                    break;

                case SamrMethodOpnums.SamrSetInformationUser2:
                    requestStub = new SamrSetInformationUser2Request();
                    break;

                case SamrMethodOpnums.Opnum59NotUsedOnWire:
                    requestStub = new Opnum59NotUsedOnWireRequest();
                    break;

                case SamrMethodOpnums.Opnum60NotUsedOnWire:
                    requestStub = new Opnum60NotUsedOnWireRequest();
                    break;

                case SamrMethodOpnums.Opnum61NotUsedOnWire:
                    requestStub = new Opnum61NotUsedOnWireRequest();
                    break;

                case SamrMethodOpnums.SamrConnect4:
                    requestStub = new SamrConnect4Request();
                    break;

                case SamrMethodOpnums.Opnum63NotUsedOnWire:
                    requestStub = new Opnum63NotUsedOnWireRequest();
                    break;

                case SamrMethodOpnums.SamrConnect5:
                    requestStub = new SamrConnect5Request();
                    break;

                case SamrMethodOpnums.SamrRidToSid:
                    requestStub = new SamrRidToSidRequest();
                    break;

                case SamrMethodOpnums.SamrSetDSRMPassword:
                    requestStub = new SamrSetDSRMPasswordRequest();
                    break;

                case SamrMethodOpnums.SamrValidatePassword:
                    requestStub = new SamrValidatePasswordRequest();
                    break;

                case SamrMethodOpnums.Opnum68NotUsedOnWire:
                    requestStub = new Opnum68NotUsedOnWireRequest();
                    break;

                case SamrMethodOpnums.Opnum69NotUsedOnWire:
                    requestStub = new Opnum69NotUsedOnWireRequest();
                    break;

                default:
                    throw new InvalidOperationException("Unknown opnum encountered");
            }
            return requestStub;
        }

        #endregion
    }
}
