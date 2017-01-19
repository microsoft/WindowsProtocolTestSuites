// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.DirectoryServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using ProtocolMessageStructures;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security
{
    class Utilities
    {
        #region GlobalVariables

        public const int IDENTIFIER_AUTHORITY_VALUES = 6;

        public static IntPtr? PolicyHandle = IntPtr.Zero;

        public static string ValisSid = "S-1-5-21-10-20-40";

        public static string TestSid = "S-1-5-21-10-30-40";

        #endregion GlobalVariables

        #region GetSid
        /// <summary>
        /// Initializes the SID of the Domain.
        /// </summary> 

        public static _RPC_SID[] GetSid(string sid)
        {
            _RPC_SID[] sidToInitialize = new _RPC_SID[1];
            int index = 1;
            string sidString = string.Empty;
            if ((sid == "TrustObject1") || (sid == "CollisionObject"))
                sidString = ValisSid;
            else
                sidString = TestSid;

            char[] delimiter = new char[1];
            delimiter[0] = '-';
            string[] SubAuthorities = sidString.Split(delimiter);

            sidToInitialize[0].Revision = Convert.ToByte(SubAuthorities[index++]);
            sidToInitialize[0].IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
            sidToInitialize[0].IdentifierAuthority.Value = new byte[IDENTIFIER_AUTHORITY_VALUES];

            sidToInitialize[0].IdentifierAuthority.Value[0] = (byte)Value_Values.NULL_SID_AUTHORITY;
            sidToInitialize[0].IdentifierAuthority.Value[1] = (byte)Value_Values.NULL_SID_AUTHORITY;
            sidToInitialize[0].IdentifierAuthority.Value[2] = (byte)Value_Values.NULL_SID_AUTHORITY;
            sidToInitialize[0].IdentifierAuthority.Value[3] = (byte)Value_Values.NULL_SID_AUTHORITY;
            sidToInitialize[0].IdentifierAuthority.Value[4] = (byte)Value_Values.NULL_SID_AUTHORITY;
            sidToInitialize[0].IdentifierAuthority.Value[5] = Convert.ToByte(SubAuthorities[index++]);

            sidToInitialize[0].SubAuthorityCount = Convert.ToByte(SubAuthorities.Length - index);
            sidToInitialize[0].SubAuthority = new uint[sidToInitialize[0].SubAuthorityCount];
            for (int i = 0; i < (SubAuthorities.Length - index); i++)
            {
                sidToInitialize[0].SubAuthority[i] = Convert.ToUInt32(SubAuthorities[i + index]);
            }
            return sidToInitialize;
        }
        #endregion

        #region GetTheDomainName
        public static int GetTheDomainName(string strCheckNameExists, ref _RPC_UNICODE_STRING[] DomainName)
        {
            int intCount = 0;
            string[] arrNameString = new string[10];
            string[] arrNamesArray = new string[10];


            arrNamesArray[0] = strCheckNameExists;
            //converting a string to a ushort array
            for (int index = 0; index <= intCount; index++)
            {
                arrNameString[index] = arrNamesArray[index];

                char[] nameArray = new char[arrNameString[index].Length];
                nameArray = arrNameString[index].ToCharArray();

                DomainName[index].Buffer = new ushort[nameArray.Length];
                Array.Copy(nameArray, DomainName[index].Buffer, nameArray.Length);
            }
            return 0;
        }
        #endregion

        #region ConvertingushortArraytoString
        public static string ConversionfromushortArraytoString(ushort[] ushrtarr)
        {
            string convertedstring = null;
            char[] charArray1 = new char[ushrtarr.Length];

            for (int i = 0; i < charArray1.Length; i++)
            {
                charArray1[i] = (char)ushrtarr[i];

            }
            StringBuilder s = new StringBuilder();
            s.Append(charArray1);
            convertedstring = s.ToString();
            return convertedstring;
        }
        #endregion

        #region DomainInformation
        public static _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX[] DomainInformation(string testDomainName, int trustDirection)
        {
            _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX[] DomainInformation = new _LSAPR_TRUSTED_DOMAIN_INFORMATION_EX[1];
            _RPC_UNICODE_STRING[] DomainName = new _RPC_UNICODE_STRING[1];

            GetTheDomainName(testDomainName, ref DomainName);

            DomainName[0].Length = (ushort)(DomainName[0].Buffer.Length * 2);
            DomainName[0].MaximumLength = (ushort)(2 + DomainName[0].Length);

            DomainInformation[0].Name = new _RPC_UNICODE_STRING();
            DomainInformation[0].Name = DomainName[0];
            DomainInformation[0].FlatName = DomainName[0];
            DomainInformation[0].Sid = Utilities.GetSid(testDomainName);
            DomainInformation[0].TrustAttributes = MS_ADTS_SecurityRequirementsValidator.TrustAttributes;
            DomainInformation[0].TrustDirection = (uint)trustDirection;
            DomainInformation[0].TrustType = TrustType_Values.ActiveDirectory;

            return DomainInformation;
        }
        #endregion DomainInformation

        #region AuthInformation
        public static _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[] AuthInformation()
        {
            _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[] authInformation = new _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[1];
            authInformation[0].IncomingAuthInfos = 0;
            authInformation[0].OutgoingAuthInfos = 0;
            authInformation[0].IncomingAuthenticationInformation = new _LSAPR_AUTH_INFORMATION[1];
            authInformation[0].IncomingPreviousAuthenticationInformation = new _LSAPR_AUTH_INFORMATION[1];
            authInformation[0].OutgoingAuthenticationInformation = new _LSAPR_AUTH_INFORMATION[1];
            authInformation[0].OutgoingPreviousAuthenticationInformation = new _LSAPR_AUTH_INFORMATION[1];

            return authInformation;
        }
        #endregion AuthInformation

        #region RemoveAccessRights
        public static void RemoveAccessRights(string dn,
                                              string targetUser,
                                              string Domain,
                                              string authUsername,
                                              string authPassword,
                                              ActiveDirectoryRights accessRight,
                                              AccessControlType controlType)
        {
            DirectoryEntry entry = new DirectoryEntry(dn, authUsername, authPassword, AuthenticationTypes.Secure);
            ActiveDirectorySecurity sd = entry.ObjectSecurity;
            int retryCount = 0;
            IdentityReference acctSID;
            retryRemoveAccessRight:
            try
            {
                NTAccount accountName = new NTAccount(Domain, targetUser);
                acctSID = accountName.Translate(typeof(SecurityIdentifier));
            }
            catch (IdentityNotMappedException e)
            {
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    string.Format("Translate account name error: {0}", e.Message));
                TestClassBase.BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    string.Format("Wait 1 minute and retry:{0}", retryCount));
                retryCount++;
                System.Threading.Thread.Sleep(60000);
                if (retryCount < 10) goto retryRemoveAccessRight;
                else throw;
            }
            ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(new SecurityIdentifier(acctSID.Value), accessRight, controlType);
            sd.AddAccessRule(myRule);
            entry.ObjectSecurity.AddAccessRule(myRule);
            entry.CommitChanges();
        }
        #endregion RemoveAccessRights

        #region LsarOpenPolicy2
        public static IntPtr LsarOpenPolicy2(LsaClient lsadAdapterObj, string strServerName, ACCESS_MASK MAXIMUM_ALLOWED)
        {

            _LSAPR_OBJECT_ATTRIBUTES objectAttributes = new _LSAPR_OBJECT_ATTRIBUTES();
            objectAttributes.RootDirectory = null;
            ACCESS_MASK uintAccessMask = (ACCESS_MASK)MAXIMUM_ALLOWED;

            NtStatus uintMethodStatus = lsadAdapterObj.LsarOpenPolicy2(
                strServerName,
                objectAttributes,
                uintAccessMask,
                out PolicyHandle);

            return PolicyHandle.Value;
        }
        #endregion LsarOpenPolicy2

        #region Parse Domain DnsName
        public static string ParseDomainName(string DomainDnsName)
        {
            string[] temp = DomainDnsName.Split('.');
            string parsedDomainName = "";
            foreach (string item in temp)
            {
                parsedDomainName += "DC=" + item + ",";
            }
            return parsedDomainName.Substring(0, parsedDomainName.Length - 1);
        }
        #endregion Parse Domain DnsName
    }
}
