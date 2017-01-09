// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr
{
    /// <summary>
    /// this enum describes DRS_OPTIONS bits supported when uses IDL_DRSGetReplInfo to query neighbors
    /// </summary>
    public enum RepNbrOptionToDra : uint
    {
        SYNC_ON_STARTUP = DRS_OPTIONS.DRS_INIT_SYNC,

        DO_SCHEDULED_SYNCS = DRS_OPTIONS.DRS_PER_SYNC,

        WRITEABLE = DRS_OPTIONS.DRS_WRIT_REP,

        USE_ASYNC_INTERSIE_TRANSPORT = DRS_OPTIONS.DRS_MAIL_REP,

        IGNORE_CHANGE_NOTIFICATIONS = DRS_OPTIONS.DRS_DISABLE_AUTO_SYNC,

        DISABLE_SCHEDULED_SYNC = DRS_OPTIONS.DRS_DISABLE_PERIODIC_SYNC,

        FULL_SYNC_IN_PROGRESS = DRS_OPTIONS.DRS_FULL_SYNC_IN_PROGRESS,

        FULL_SYNC_NEXT_PACKET = DRS_OPTIONS.DRS_FULL_SYNC_PACKET,

        COMPRESS_CHANGES = DRS_OPTIONS.DRS_USE_COMPRESSION,

        NO_CHANGE_NOTIFICATIONS = DRS_OPTIONS.DRS_NEVER_NOTIFY,

        NEVER_SYNCED = DRS_OPTIONS.DRS_NEVER_SYNCED,

        TWO_WAY_SYNC = DRS_OPTIONS.DRS_TWOWAY_SYNC,

        NONGC_RO_REPLICA = DRS_OPTIONS.DRS_NONGC_RO_REP,

        PARTIAL_ATTRIBUTED_SET = DRS_OPTIONS.DRS_SYNC_PAS,

        PREEMPED = DRS_OPTIONS.DRS_PREEMPTED,

        SELECT_SECRETS = DRS_OPTIONS.DRS_SPECIAL_SECRET_PROCESSING,

        GCSPN = DRS_OPTIONS.DRS_REF_GCSPN
    }
}
