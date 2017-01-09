// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// The update class to managed object added by tests.
    /// </summary>
    public class AddObjectUpdate : LdapBaseUpdate, IUpdate
    {
        /// <summary>
        /// server
        /// </summary>
        EnvironmentConfig.Machine dsServerType;

        /// <summary>
        /// target object
        /// </summary>
        string objectDN;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dcType"></param>
        /// <param name="objDN"></param>
        public AddObjectUpdate(EnvironmentConfig.Machine dcType, string objDN)
        {
            dsServerType = dcType;
            objectDN = objDN;
        }

        /// <summary>
        /// delete object if exists
        /// </summary>
        /// <returns></returns>
        public bool Invoke()
        {
            //Assume the object has been added in external.
            return true;
        }

        /// <summary>
        /// delete object created by tests
        /// </summary>
        /// <returns></returns>
        public bool Revert()
        {
            return op();
        }

        /// <summary>
        /// try delete object
        /// </summary>
        /// <returns></returns>
        bool op()
        {
            DsServer dc = (DsServer)EnvironmentConfig.MachineStore[dsServerType];
            if (!LdapUtility.IsObjectExist(dc, objectDN))
                return true;

            for (int i = 0; i < 2; i++)
            {
                try
                {
                    System.DirectoryServices.Protocols.ResultCode rCode = ldapAdapter.DeleteObject(dc, objectDN);
                    if (rCode == System.DirectoryServices.Protocols.ResultCode.Success)
                        return true;
                }
                catch
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            return false;
        }
    }
}
