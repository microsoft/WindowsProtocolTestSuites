// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.Protocols;
using System.Globalization;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap
{
    /// <summary>
    /// replace .Net LDAP AddRequest
    /// </summary>
    public class ManagedModifyDNRequest : System.DirectoryServices.Protocols.ModifyDNRequest
    {
        /// <summary>
        /// allowed constructor
        /// </summary>
        /// <param name="distinguishedName">old DN of object</param>
        /// <param name="newParentDistinguishedName">DN of new parent for object</param>
        /// <param name="newName">new RDN of object</param>
        public ManagedModifyDNRequest(string distinguishedName, string newParentDistinguishedName, string newName)
        {
            if (distinguishedName == null)
            {
                throw new Exception("Null DistinguishedName is not allowed");
            }
            if (newParentDistinguishedName == null)
            {
                throw new Exception("Null new parent DistinguishedName is not allowed");
            }
            if (newName == null)
            {
                throw new Exception("Null new RDN is not allowed");
            }
            string key = ("cn=" + newName + "," + newParentDistinguishedName).ToLower(CultureInfo.InvariantCulture);
            if (!EnvironmentConfig.Inst.TmpObjectDict.ContainsKey(key))
                EnvironmentConfig.Inst.TmpObjectDict.Add(key, true);
            base.NewName = newName;
            base.NewParentDistinguishedName = newParentDistinguishedName;
            base.DistinguishedName = distinguishedName;
        }

        /// <summary>
        /// not allow default constructor
        /// </summary>
        private ManagedModifyDNRequest()
        {
        }
    }

    /// <summary>
    /// replace .Net LDAP ModifyDNRequest
    /// </summary>
    public class ManagedAddRequest : System.DirectoryServices.Protocols.AddRequest
    {
        /// <summary>
        /// not allow default constructor
        /// </summary>
        private ManagedAddRequest()
        {
        }

        /// <summary>
        /// allowed constructor
        /// </summary>
        /// <param name="distinguishedName">object DN</param>
        /// <param name="attributes">attributes of object</param>
        public ManagedAddRequest(string distinguishedName, params DirectoryAttribute[] attributes)
        {
            if (distinguishedName == null)
            {
                throw new Exception("Null DistinguishedName is not allowed");
            }
            string key = distinguishedName.ToLower(CultureInfo.InvariantCulture);
            if (!EnvironmentConfig.Inst.TmpObjectDict.ContainsKey(key))
                EnvironmentConfig.Inst.TmpObjectDict.Add(distinguishedName.ToLower(CultureInfo.InvariantCulture), true);
            base.DistinguishedName = distinguishedName;
            if (attributes != null)
            {
                for (int i = 0; i < attributes.Length; i++)
                {
                    base.Attributes.Add(attributes[i]);
                }
            }
        }

        /// <summary>
        /// allowed constructor
        /// </summary>
        /// <param name="distinguishedName">object DN</param>
        /// <param name="objectClass">objectClass of object</param>
        public ManagedAddRequest(string distinguishedName, string objectClass)
        {
            if (distinguishedName == null)
            {
                throw new Exception("Null DistinguishedName is not allowed");
            }
            if (objectClass == null)
            {
                throw new Exception("Null object class is not allowed");
            }
            string key = distinguishedName.ToLower(CultureInfo.InvariantCulture);
            if (!EnvironmentConfig.Inst.TmpObjectDict.ContainsKey(key))
                EnvironmentConfig.Inst.TmpObjectDict.Add(distinguishedName.ToLower(CultureInfo.InvariantCulture), true);
            base.DistinguishedName = distinguishedName;
            base.Attributes.Add(new DirectoryAttribute("objectclass", objectClass));
        }
    }


    public class EnvironmentConfig
    {
        public static ServerVersion ServerVer
        {
            get;
            set;
        }
                
        ~EnvironmentConfig()
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter("tmp.txt"))
            {
                Dictionary<string, bool>.Enumerator enumer = TmpObjectDict.GetEnumerator();
                while (enumer.MoveNext())
                {
                    writer.WriteLine(enumer.Current.Key);
                }

            }
        }

        public static EnvironmentConfig Inst = new EnvironmentConfig();

        //SHOULD DELETE!
        public Dictionary<string, bool> TmpObjectDict = new Dictionary<string, bool>();
    }
}
