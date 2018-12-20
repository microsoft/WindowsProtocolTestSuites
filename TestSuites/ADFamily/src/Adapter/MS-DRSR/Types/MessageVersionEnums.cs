// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// Enumeration to specify the request versions of IDL_DRSGetReplInfo.
    /// </summary>
    public enum DrsGetReplInfo_Versions : uint
    {
        /// <summary>
        /// V1 request.
        /// </summary>
        V1 = 1,

        /// <summary>
        /// V2 request.
        /// </summary>
        V2 = 2
    }


    /// <summary>
    /// Enumeration to specify the request versions of IDL_DRSReplicaSync.
    /// </summary>
    public enum DrsReplicaSync_Versions : uint
    {

        /// <summary>
        /// V1 request.
        /// </summary>
        V1 = 1,

        /// <summary>
        /// V2 request.
        /// </summary>
        V2 = 2
    }

    /// <summary>
    /// Enumeration to specify the request versions of IDL_DRSUpdateRefs.
    /// </summary>
    public enum DrsUpdateRefs_Versions : uint
    {

        /// <summary>
        /// V1 request.
        /// </summary>
        V1 = 1,

        /// <summary>
        /// V2 request.
        /// </summary>
        V2 = 2
    }

    /// <summary>
    /// Enumeration to specify the request versions of IDL_DRSGetNCChanges.
    /// </summary>
    public enum DrsGetNCChanges_Versions : uint
    {
        /// <summary>
        /// V4 request.
        /// </summary>
        V4 = 4,

        /// <summary>
        /// V5 request.
        /// </summary>
        V5 = 5,

        /// <summary>
        /// V7 request.
        /// </summary>
        V7 = 7,

        /// <summary>
        /// V8 request.
        /// </summary>
        V8 = 8,

        /// <summary>
        /// V10 request.
        /// </summary>
        V10 = 10,

        /// <summary>
        /// V11 request.
        /// </summary>
        V11 = 11
    }

    /// <summary>
    /// Enumeration to specify the request versions of IDL_DRSVerifyObjects.
    /// </summary>
    public enum DrsReplicaVerifyObjects_Versions : uint
    {
        /// <summary>
        /// V1 request.
        /// </summary>
        V1 = 1
    }

    /// <summary>
    /// Enumeration to specify the request versions of IDL_DRSGetObjectExistence.
    /// </summary>
    public enum DrsGetObjectExistence_Versions : uint
    {
        /// <summary>
        /// V1 request.
        /// </summary>
        V1 = 1
    }

    /// <summary>
    /// Versions of DRS_MSG_DCINFOREQ
    /// </summary>
    public enum DRS_MSG_DCINFOREQ_Versions : uint
    {
        /// <summary>
        /// V1 request
        /// </summary>
        V1 = 1
    }

    /// <summary>
    /// Versions of DRS_MSG_QUERYSITESBYCOST
    /// </summary>
    public enum DRS_MSG_QUERYSITESREQ_Versions : uint
    {
        /// <summary>
        /// V1 request
        /// </summary>
        V1 = 1
    }

    /// <summary>
    /// Versions of DRS_MSG_REPADD
    /// </summary>
    public enum DRS_MSG_REPADD_Versions : uint
    {
        /// <summary>
        /// V1 request
        /// </summary>
        V1 = 1,

        /// <summary>
        /// V2 request
        /// </summary>
        V2 = 2,

        /// <summary>
        /// V3 request
        /// </summary>
        V3 = 3
    }

    /// <summary>
    /// Versions of DRS_AddEntry_Versions
    /// </summary>
    public enum DRS_AddEntry_Versions : uint
    {
        /// <summary>
        /// V2 request
        /// </summary>
        V2 = 2,

        /// <summary>
        /// V3 request
        /// </summary>
        V3 = 3
    }

    /// <summary>
    /// Versions of IDL_DSAPrepareScript 
    /// </summary>
    public enum DSAPrepareScript_Versions : uint
    {
        /// <summary>
        /// V1 request
        /// </summary>
        V1 = 1
    }

    /// <summary>
    /// Versions of IDL_DSAExecuteScript 
    /// </summary>
    public enum DSAExecuteScript_Versions : uint
    {
        /// <summary>
        /// V1 request
        /// </summary>
        V1 = 1
    }

    public enum DRSInterDomainMove_Versions : uint
    {
        /// <summary>
        /// V1 request. This request version is obsolete.
        /// </summary>
        V1 = 1,

        /// <summary>
        /// V2 request.
        /// </summary>
        V2 = 2
    }
}
