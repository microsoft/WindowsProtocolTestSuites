// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public class DRSRFailureTestClient : DRSRTestClient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.UInt32.Parse(System.String)")]
        public DRS_EXTENSIONS CreateCorrectDrsExtensionsForClient(bool isDc)
        {
            if (!isDc)

                return DRSClient.CreateDrsExtensions(false, TestTools.StackSdk.ActiveDirectory.Drsr.DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE,
                    Guid.Empty,
                    0,
                    0);
            else
            {
                DsServer sut = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];
                uint epo = 0;
                string got = LdapUtility.GetAttributeValueInString(sut, sut.NtdsDsaObjectName, "msDs-ReplicationEpoch");
                if (got != null)
                    epo = uint.Parse(got);
                return DRSClient.CreateDrsExtensions(true, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE,
                    ((DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1]).Site.Guid,
                    0,
                    epo);
            }
        }

        public DRS_MSG_QUERYSITESREQ CreateDRS_MSG_QUERYSITESREQ(string pwszFromSite, uint cToSites, string[] rgszToSites)
        {
            DRS_MSG_QUERYSITESREQ_V1 message = new DRS_MSG_QUERYSITESREQ_V1();
            message.pwszFromSite = pwszFromSite;
            message.cToSites = cToSites;
            message.rgszToSites = rgszToSites;
            message.dwFlags = dwFlags_Values.V1;
            DRS_MSG_QUERYSITESREQ request = new DRS_MSG_QUERYSITESREQ();
            request.V1 = message;
            return request;
        }


        public DRS_MSG_KCC_EXECUTE CreateExecuteKCCReq()
        {
            DRS_MSG_KCC_EXECUTE req = new DRS_MSG_KCC_EXECUTE();
            req.V1 = new DRS_MSG_KCC_EXECUTE_V1();
            req.V1.dwTaskID = (uint)dwTaskID_Values.V1;
            req.V1.dwFlags = (uint)DRS_MSG_KCC_EXECUTE_FLAGS.DS_KCC_FLAG_DAMPED;
            return req;
        }

        public DRS_MSG_REPVERIFYOBJ CreateDrsReplicaVerifyObjectsV1Request()
        {
            DRS_MSG_REPVERIFYOBJ req = new DRS_MSG_REPVERIFYOBJ();
            req.V1 = new DRS_MSG_REPVERIFYOBJ_V1();
            req.V1.pNC = DrsuapiClient.CreateDsName(null, Guid.Empty, null);
            req.V1.ulOptions = 0;
            req.V1.uuidDsaSrc = Guid.Empty;

            return req;
        }

        public DRS_MSG_GETCHGREQ CreateDrsGetNcChangesV8Request() {
            DRS_MSG_GETCHGREQ req = new DRS_MSG_GETCHGREQ();
            req.V8 = new DRS_MSG_GETCHGREQ_V8();
            req.V8.uuidDsaObjDest = Guid.Empty;
            req.V8.uuidInvocIdSrc = Guid.Empty;
            req.V8.pNC = DrsuapiClient.CreateDsName(null, Guid.Empty, null);
            req.V8.usnvecFrom = new USN_VECTOR();
            req.V8.pUpToDateVecDest = new UPTODATE_VECTOR_V1_EXT[1];
            req.V8.pUpToDateVecDest[0].dwVersion = UPTODATE_VECTOR_V1_EXT_dwVersion_Values.V1;
            req.V8.pUpToDateVecDest[0].cNumCursors = 1;
            req.V8.pUpToDateVecDest[0].rgCursors = new UPTODATE_CURSOR_V1[1];
            req.V8.pPartialAttrSet = null;
            req.V8.pPartialAttrSetEx = null;
            req.V8.PrefixTableDest = OIDUtility.CreatePrefixTable();

            return req;
        }

        public DRS_MSG_EXISTREQ CreateDrsGetObjectExistenceV1Request() {
            DRS_MSG_EXISTREQ req = new DRS_MSG_EXISTREQ();
            req.V1 = new DRS_MSG_EXISTREQ_V1();
            req.V1.guidStart = Guid.Empty;
            req.V1.pNC = DrsuapiClient.CreateDsName(null, Guid.Empty, null);
            req.V1.pUpToDateVecCommonV1 = new UPTODATE_VECTOR_V1_EXT[1];
            req.V1.pUpToDateVecCommonV1[0].cNumCursors = 1;
            req.V1.pUpToDateVecCommonV1[0].rgCursors = new UPTODATE_CURSOR_V1[1];
            req.V1.Md5Digest = new byte[16];
            return req;
        }

        public DRS_MSG_GETREPLINFO_REQ CreateDrsGetReplInfoV1Request() {
            DRS_MSG_GETREPLINFO_REQ req = new DRS_MSG_GETREPLINFO_REQ();
            req.V1 = new DRS_MSG_GETREPLINFO_REQ_V1();
            req.V1.uuidSourceDsaObjGuid = Guid.Empty;
            req.V1.pszObjectDN = "Dummy";
            return req;
        }

        public DRS_MSG_GETREPLINFO_REQ CreateDrsGetReplInfoV2Request() {
            DRS_MSG_GETREPLINFO_REQ req = new DRS_MSG_GETREPLINFO_REQ();
            req.V2 = new DRS_MSG_GETREPLINFO_REQ_V2();
            req.V2.uuidSourceDsaObjGuid = Guid.Empty;
            return req;
        }

        public DRS_MSG_REPSYNC CreateDrsReplicaSyncV1Request()
        {
            DRS_MSG_REPSYNC req = new DRS_MSG_REPSYNC();
            req.V1 = new DRS_MSG_REPSYNC_V1();
            req.V1.pNC = DrsuapiClient.CreateDsName(null, Guid.Empty, null);
            req.V1.uuidDsaSrc = Guid.Empty;
            return req;
        }

        public DRS_MSG_REPSYNC CreateDrsReplicaSyncV2Request()
        {
            DRS_MSG_REPSYNC req = new DRS_MSG_REPSYNC();
            req.V2 = new DRS_MSG_REPSYNC_V2();
            req.V2.pNC = DrsuapiClient.CreateDsName(null, Guid.Empty, null);
            req.V2.uuidDsaSrc = Guid.Empty;
            return req;
        }

        // <summary>
        // the function is used to create a DrsUpdateRef request
        // </summary>
        public DRS_MSG_UPDREFS CreateRequestForDrsUpdateRef(
                    EnvironmentConfig.Machine machine,
                    DrsUpdateRefs_Versions reqVer,
                    DsServer dest,
                    DRS_OPTIONS options,
                    NamingContext nc = NamingContext.ConfigNC)
        {
            string nc_name = null;
            Guid nc_guid = Guid.Empty;
            string nc_sid = null;
            DSNAME nc_obj;
            switch (nc)
            {
                case NamingContext.ConfigNC:
                    nc_obj = dest.Domain.ConfigNC;
                    break;
                case NamingContext.SchemaNC:
                    nc_obj = dest.Domain.SchemaNC;
                    break;
                case NamingContext.AppNC:
                    if (EnvironmentConfig.TestDS)
                    {
                        nc_obj = ((AddsDomain)dest.Domain).OtherNCs[0];
                    }
                    else
                    {
                        nc_obj = ((AdldsDomain)dest.Domain).AppNCs[0];
                    }
                    break;
                case NamingContext.DomainNC:
                    if (!EnvironmentConfig.TestDS)
                        nc_obj = new DSNAME();
                    nc_obj = ((AddsDomain)dest.Domain).DomainNC;
                    break;
                default:
                    nc_obj = new DSNAME();
                    break;
            }

            nc_name = LdapUtility.ConvertUshortArrayToString(nc_obj.StringName);
            nc_guid = nc_obj.Guid;
            nc_sid = convertSidToString(nc_obj.Sid);

            DRS_MSG_UPDREFS? req = CreateUpdateRefsRequest(
                reqVer,
                nc_name,
                nc_guid,
                nc_sid,
                dest.DsaNetworkAddress,
                dest.NtdsDsaObjectGuid,
                options);
            return (DRS_MSG_UPDREFS)req;
        }

        public DRS_MSG_REPMOD createDRS_MSG_REPMOD_Request(
                    EnvironmentConfig.Machine machine,
                    DsServer src,
                    DRS_OPTIONS replicaFlags,
                    DRS_MSG_REPMOD_FIELDS modifyFields,
                    DRS_OPTIONS options
                    )
        {
            #region generate the parameters
            string ncReplicaDistinguishedName = LdapUtility.ConvertUshortArrayToString(src.Domain.ConfigNC.StringName);
            Guid ncReplicaObjectGuid = src.Domain.ConfigNC.Guid;
            NT4SID ncReplicaObjectSid = src.Domain.ConfigNC.Sid;
            Guid sourceDsaGuid = src.NtdsDsaObjectGuid;
            string sourceDsaName = src.DsaNetworkAddress;

            #endregion

            REPLTIMES s = new REPLTIMES();
            s.rgTimes = new byte[84];
            for (int i = 0; i < s.rgTimes.Length; i++)
            {
                s.rgTimes[i] = 1;
            }

            string sid = convertSidToString(ncReplicaObjectSid);

            DRS_MSG_REPMOD? req = DRSClient.CreateReplicaModifyRequest(
                ncReplicaDistinguishedName,
                ncReplicaObjectGuid,
                sid,
                sourceDsaGuid,
                sourceDsaName,
                s,
                replicaFlags,
                modifyFields,
                options);
            return (DRS_MSG_REPMOD)req;
        }

        public DRS_MSG_VERIFYREQ CreateDrsVerifyNamesV1Request() {
            DRS_MSG_VERIFYREQ req = new DRS_MSG_VERIFYREQ();
            req.V1 = new DRS_MSG_VERIFYREQ_V1();
            req.V1.dwFlags = 0;
            req.V1.cNames = 1;
            req.V1.rpNames = new DSNAME[1][];
            req.V1.rpNames[0] = new DSNAME[] { DrsuapiClient.CreateDsName(null, Guid.Empty, null) };
            req.V1.RequiredAttrs = DrsuapiClient.CreateATTRBLOCK(null);
            req.V1.PrefixTable = OIDUtility.CreatePrefixTable();

            return req;
        }

        public DRS_MSG_REVMEMB_REQ CreateDrsGetMembershipsV1Request() {
            DRS_MSG_REVMEMB_REQ req = new DRS_MSG_REVMEMB_REQ();
            req.V1 = new DRS_MSG_REVMEMB_REQ_V1();
            req.V1.cDsNames = 1;
            req.V1.ppDsNames = new DSNAME[1][];
            req.V1.ppDsNames[0] = new DSNAME[] { DrsuapiClient.CreateDsName(null, Guid.Empty, null) };
            req.V1.pLimitingDomain = null;
            req.V1.OperationType = REVERSE_MEMBERSHIP_OPERATION_TYPE.GroupMembersTransitive;

            return req;
        }

        public DRS_MSG_CRACKREQ CreateDrsCrackNamesV1Request() {
            DRS_MSG_CRACKREQ req = new DRS_MSG_CRACKREQ();
            req.V1 = new DRS_MSG_CRACKREQ_V1();
            req.V1.cNames = 1;
            req.V1.rpNames = new string[1];
            req.V1.rpNames[0] = "Dummy";
            return req;
        }
    }
}
