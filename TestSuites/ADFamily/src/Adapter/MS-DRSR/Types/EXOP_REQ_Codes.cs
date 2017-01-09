// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// Request codes for extended operation of IDL_DRSGetNCChanges.
    /// </summary>
    public enum EXOP_REQ_Codes:uint
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// The requested extended operation for a FSMO role owner transfer.
        /// </summary>
        EXOP_FSMO_REQ_ROLE = 0x00000001,

        /// <summary>
        /// The requested extended operation for a RID allocation from the RID Master FSMO role owner.
        /// </summary>
        EXOP_FSMO_REQ_RID_ALLOC = 0x00000002,

        /// <summary>
        /// The requested extended operation for transfer of the RID Master FSMO role.
        /// </summary>
        EXOP_FSMO_RID_REQ_ROLE = 0x00000003,

        /// <summary>
        /// The requested extended operation for transfer of the PDC FSMO role. 
        /// </summary>
        EXOP_FSMO_REQ_PDC = 0x00000004,

        /// <summary>
        /// The requested extended operation to request the server to request an extended operation role transfer from the client.
        /// </summary>
        EXOP_FSMO_ABANDON_ROLE = 0x00000005,

        /// <summary>
        /// The requested extended operation to replicate changes from a single object.
        /// </summary>
        EXOP_REPL_OBJ = 0x00000006,

        /// <summary>
        /// The requested extended operation to replicate changes form a single object with secrets. 
        /// </summary>
        EXOP_REPL_SECRETS = 0x00000007

    }
}
