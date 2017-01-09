// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// delete replication source update
    /// </summary>
    public class DelReplicaSource : DrsrBaseUpdate, IUpdate
    {
        /// <summary>
        /// operation sut
        /// </summary>
        EnvironmentConfig.Machine sut;

        /// <summary>
        /// replication source
        /// </summary>
        EnvironmentConfig.Machine target;

        /// <summary>
        /// operation flags
        /// </summary>
        DRS_OPTIONS options;

        /// <summary>
        /// credential
        /// </summary>
        EnvironmentConfig.User user;

        /// <summary>
        /// do a KCC before delete
        /// </summary>
        bool doKCC = true;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="svr">sut</param>
        /// <param name="toDel">source</param>
        /// <param name="op">flags</param>
        /// <param name="cred">credential</param>
        public DelReplicaSource(EnvironmentConfig.Machine svr, EnvironmentConfig.Machine toDel, DRS_OPTIONS op, EnvironmentConfig.User cred, NamingContext nc = NamingContext.ConfigNC, bool needKCC = true)
        {
            sut = svr;
            target = toDel;
            if ((op & DRS_OPTIONS.DRS_LOCAL_ONLY) == 0)
                options = op | DRS_OPTIONS.DRS_REF_OK;
            else
                options = op;
            user = cred;
            doKCC = needKCC;
        }


        /// <summary>
        /// delete the replication source
        /// </summary>
        /// <returns>true if success</returns>
        public bool Invoke()
        {
            bool binded = false;
            try
            {
                if (0 != client.DrsBind(sut, user, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE))
                    return false;

                binded = true;
                if (doKCC)
                    client.DrsExecuteKCC(sut, false);
                client.DrsReplicaDel(sut, (DsServer)EnvironmentConfig.MachineStore[target], options);

                client.DrsUnbind(sut);
                return true;
            }
            catch
            {
                if (binded)
                    client.DrsUnbind(sut);
                return true;
            }
        }

        /// <summary>
        /// restore replication source
        /// </summary>
        /// <returns>true if success</returns>
        public bool Revert()
        {
            bool binded = false;
            try
            {
                if (0 != client.DrsBind(sut, user, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE))
                    return false;

                binded = true;
                bool ret = 0 == client.DrsExecuteKCC(sut, false);

                client.DrsUnbind(sut);
                return ret;
            }
            catch
            {
                if (binded)
                    client.DrsUnbind(sut);
                return false;
            }
        }
    }

    /// <summary>
    /// delete replication source update
    /// </summary>
    public class RecoverReplicaSource : DrsrBaseUpdate, IUpdate
    {
        /// <summary>
        /// operation sut
        /// </summary>
        EnvironmentConfig.Machine sut;

        /// <summary>
        /// credential
        /// </summary>
        EnvironmentConfig.User user;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="svr">sut</param>
        /// <param name="toDel">source</param>
        /// <param name="op">flags</param>
        /// <param name="cred">credential</param>
        public RecoverReplicaSource(EnvironmentConfig.Machine svr, EnvironmentConfig.User cred)
        {
            sut = svr;
            user = cred;
        }


        /// <summary>
        /// delete the replication source
        /// </summary>
        /// <returns>true if success</returns>
        public bool Invoke()
        {
            return true;
        }

        /// <summary>
        /// restore replication source
        /// </summary>
        /// <returns>true if success</returns>
        public bool Revert()
        {
            bool binded = false;
            try
            {
                if (0 != client.DrsBind(sut, user, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE))
                    return false;

                binded = true;
                bool ret = 0 == client.DrsExecuteKCC(sut, false);

                client.DrsUnbind(sut);
                return ret;
            }
            catch
            {
                if (binded)
                    client.DrsUnbind(sut);
                return false;
            }
        }
    }

    public class NeedRepSourceUpdate : DrsrBaseUpdate, IUpdate
    {
        EnvironmentConfig.Machine server;

        EnvironmentConfig.Machine src;

        NamingContext srcNC;

        public NeedRepSourceUpdate(EnvironmentConfig.Machine sut, EnvironmentConfig.Machine srcSvr, NamingContext nc)
        {
            server = sut;
            src = srcSvr;
            srcNC = nc;
        }

        public bool Invoke()
        {
            bool binded = false;
            try
            {
                if (client.DrsBind(server, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE) != 0)
                    return false;

                binded = true;

                ILdapAdapter ad = client.Site.GetAdapter<ILdapAdapter>();
                ad.Site = client.Site;
                REPS_FROM[] reps = ad.GetRepsFrom((DsServer)EnvironmentConfig.MachineStore[server], srcNC);
                if (reps == null)
                    return false;
                DsServer s = (DsServer)EnvironmentConfig.MachineStore[src];
                bool found = false;
                foreach (REPS_FROM rf in reps)
                {
                    if (rf.uuidDsaObj == s.NtdsDsaObjectGuid)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                    return true;

                if (0 == client.DrsReplicaAdd(server, DRS_MSG_REPADD_Versions.V1, s, DRS_OPTIONS.DRS_WRIT_REP, srcNC))
                    return true;

                return false;

            }
            catch
            {

                return false;
            }
            finally
            {
                if (binded)
                    client.DrsUnbind(server);
            }
        }

        public bool Revert()
        {
            return true;
        }
    }
}
