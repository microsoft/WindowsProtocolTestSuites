// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat
{
    using System;
    using System.Text;

    using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
    using Microsoft.Protocols.TestTools.StackSdk.Dtyp;

    /// <summary>
    /// The utility class for the project MS-LSAT_ServerAdapter.
    /// </summary>
    public static class LsatUtilities
    {
        #region Field

        #region Const field

        /// <summary>
        /// Max count of well known security principal. 
        /// </summary>
        public const int WellknownSecurityPrincipalMaxCount = 27;

        /// <summary>
        /// Max count of RID.
        /// </summary>
        public const int RidMaxCount = 38;

        /// <summary>
        /// Max user count.
        /// </summary>
        public const int TestsuiteMaxUserCount = 4;

        /// <summary>
        /// Value of LookupSidS1.
        /// </summary>
        public const int LookupSidS1 = 1;

        /// <summary>
        /// Value of LookupSidS2.
        /// </summary>
        public const int LookupSidS2 = 2;

        /// <summary>
        /// Value of Invalid.
        /// </summary>
        public const uint Invalid = 1;

        /// <summary>
        /// Value of Success.
        /// </summary>
        public const uint Success = 2;

        /// <summary>
        /// Value of SomeNotMapped.
        /// </summary>
        public const uint SomeNotMapped = 3;

        /// <summary>
        /// Value of NoneMapped.
        /// </summary>
        public const uint NoneMapped = 4;

        /// <summary>
        /// Value of InvalidLookupLevel.
        /// </summary>
        public const uint InvalidLookupLevel = 25;

        /// <summary>
        /// Value of InvalidRootDirectory.
        /// </summary>
        public const uint InvalidRootDirectory = 25;

        /// <summary>
        /// Value of FlagValueZero.
        /// </summary>
        public const uint FlagValueZero = 0x00000000;

        /// <summary>
        /// Value of StatusSuccess.
        /// </summary>
        public const uint StatusSuccess = 0x00000000;

        /// <summary>
        /// Value of StatusSomeNotMapped.
        /// </summary>
        public const uint StatusSomeNotMapped = 0x00000107;

        /// <summary>
        /// Value of StatusNoneMapped.
        /// </summary>
        public const uint StatusNoneMapped = 0xC0000073;

        /// <summary>
        /// Value of StatusInvalidParameter.
        /// </summary>
        public const uint StatusInvalidParameter = 0xC000000D;

        /// <summary>
        /// Value of StatusAccessDenied.
        /// </summary>
        public const uint StatusAccessDenied = 0xC0000022;

        /// <summary>
        /// Value of StatusInvalidServerState.
        /// </summary>
        public const uint StatusInvalidServerState = 0xC00000DC;

        /// <summary>
        /// Value of MsbNotSet.
        /// </summary>
        public const uint MsbNotSet = 0x00000000;

        /// <summary>
        /// Identifier authority count.
        /// </summary>
        private const uint IdentifierAuthorityCount = 6;

        /// <summary>
        /// Subauthority start position.
        /// </summary>
        private const int SubauthorityStartPosition = 8;

        /// <summary>
        /// Built in container max count.
        /// </summary>
        private const int BuiltinContainerMaxCount = 21;

        /// <summary>
        /// Domain relative max count.
        /// </summary>
        private const int DomainRelativeMaxCount = 17;

        /// <summary>
        /// Relative ID position.
        /// </summary>
        private const int RelativeIdPosition = 24;

        /// <summary>
        /// Subsuthority size.
        /// </summary>
        private const int SubauthoritySize = 4;

        /// <summary>
        /// Invalid revision.
        /// </summary>
        private const byte InvalidRevision = 100;

        /// <summary>
        /// Invalid sub authority.
        /// </summary>
        private const uint InvalidSubauthority = 1357;

        /// <summary>
        /// Look up names.
        /// </summary>
        private const uint LookupNames = 0x00000800;

        /// <summary>
        /// Status invalid handle.
        /// </summary>
        private const uint StatusInvalidHandle = 0xC0000008;

        /// <summary>
        /// Status unknown.
        /// </summary>
        private const uint StatusUnknown = 0xFFFFFFFF;

        /// <summary>
        /// MSB set.
        /// </summary>
        private const uint MsbSet = 0x80000000;

        #endregion

        #region Static filed

        /// <summary>
        /// Set of Names which are not present in the domain
        /// </summary>
        public static string[] NonExistingAccount = { @"moskjkshr@hdhh.com", "kujssarkhh", @"hdhsgjgs\bcD", @"kkx.com\hjshshs" };

        /// <summary>
        /// Sid entries.
        /// </summary>
        private static uint sidEntries;

        /// <summary>
        /// Account names.
        /// </summary>
        private static string[] accountNames;

        /// <summary>
        /// Name value.
        /// </summary>
        private static string[] nameString;

        /// <summary>
        /// Names value.
        /// </summary>
        private static string[] namesArray;

        /// <summary>
        /// Sids value.
        /// </summary>
        private static _LSAPR_SID_INFORMATION[] arrayOfSids = new _LSAPR_SID_INFORMATION[TestsuiteMaxUserCount];

        /// <summary>
        /// Well known security principals.
        /// </summary>
        private static string[] wellknownSecurityPrincipals;

        /// <summary>
        /// Builtin relative domain names.
        /// </summary>
        private static string[] builtinRelativeDomainNames;

        /// <summary>
        /// Name validity.
        /// </summary>
        private static uint namesValidity;

        /// <summary>
        /// SID validity.
        /// </summary>
        private static uint sidsValidity;

        /// <summary>
        /// Well known SIDs.
        /// </summary>
        private static string[] wellknownSIDs;

        /// <summary>
        /// Well knows relative ID.
        /// </summary>
        private static int[] wellknownRelativeID;

        /// <summary>
        /// Well known SIDs.
        /// </summary>
        private static string[] wellknownSidStrings;

        #endregion

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets wellknownSecurityPrincipals.
        /// </summary>
        internal static string[] WellknownSecurityPrincipals
        {
            get { return wellknownSecurityPrincipals; }
            set { wellknownSecurityPrincipals = value; }
        }

        /// <summary>
        /// Gets or sets builtinRelativeDomainNames.
        /// </summary>
        internal static string[] BuiltinRelativeDomainNames
        {
            get { return builtinRelativeDomainNames; }
            set { builtinRelativeDomainNames = value; }
        }

        /// <summary>
        /// Gets or sets namesValidity.
        /// </summary>
        internal static uint NamesValidity
        {
            get { return namesValidity; }
            set { namesValidity = value; }
        }

        /// <summary>
        /// Gets or sets sidsValidity.
        /// </summary>
        internal static uint SidsValidity
        {
            get { return sidsValidity; }
            set { sidsValidity = value; }
        }

        /// <summary>
        /// Gets or sets wellknownSIDs.
        /// </summary>
        internal static string[] WellknownSIDs
        {
            get { return wellknownSIDs; }
            set { wellknownSIDs = value; }
        }

        /// <summary>
        /// Gets or sets wellknownRelativeID.
        /// </summary>
        internal static int[] WellknownRelativeID
        {
            get { return wellknownRelativeID; }
            set { wellknownRelativeID = value; }
        }

        /// <summary>
        /// Gets or sets wellknownSidStrings.
        /// </summary>
        internal static string[] WellknownSidStrings
        {
            get { return wellknownSidStrings; }
            set { wellknownSidStrings = value; }
        }

        #endregion

        #region InitializeAccountNames

        /// <summary>
        /// Method to read the Domain User Account Names from ptfconfig file
        /// and convert them into RPC_UNICODE_STRING format.
        /// </summary>
        public static void InitializeAccountNames(string domainAdministratorName)
        {
            int counter = 0;
            accountNames = new string[TestsuiteMaxUserCount];
            nameString = new string[TestsuiteMaxUserCount];
            LsatAdapter.RpcAccountNames = new _RPC_UNICODE_STRING[TestsuiteMaxUserCount];

            for (counter = 0; counter < TestsuiteMaxUserCount; counter++)
            {
                accountNames[counter] = domainAdministratorName;
                nameString[counter] = accountNames[counter];
                char[] nameArray = new char[nameString[counter].Length];
                nameArray = nameString[counter].ToCharArray();
                LsatAdapter.RpcAccountNames[counter].Buffer = new ushort[nameArray.Length];
                Array.Copy(nameArray, LsatAdapter.RpcAccountNames[counter].Buffer, nameArray.Length);
                LsatAdapter.RpcAccountNames[counter].Length =
                    (ushort)(2 * LsatAdapter.RpcAccountNames[counter].Buffer.Length);
                LsatAdapter.RpcAccountNames[counter].MaximumLength =
                    (ushort)(LsatAdapter.RpcAccountNames[counter].Length + 2);
            }
        }

        #endregion

        #region InitializeWellknownPrincipalNames

        /// <summary>
        /// Method to read the Wellknown Principal Names from ptfconfig file
        /// which are given in Section 7.1.1.2.6 of ADTS document, and convert them into RPC_UNICODE_STRING format.
        /// </summary>
        /// <param name="numberOfUsers">Contains the number of well known users.</param>
        /// <param name="securityPrincipals">The security principals of well known users.</param>
        /// <param name="rpcNameBuffer">The RPC name.</param>
        public static void InitializeWellknownPrincipalNames(
            uint numberOfUsers, 
            string[] securityPrincipals, 
            ref _RPC_UNICODE_STRING[] rpcNameBuffer)
        {
            rpcNameBuffer = new _RPC_UNICODE_STRING[numberOfUsers];

            nameString = new string[numberOfUsers];

            for (int index = 0; index < numberOfUsers; index++)
            {
                nameString[index] = securityPrincipals[index];
                char[] nameArray = new char[nameString[index].Length];
                nameArray = nameString[index].ToCharArray();
                rpcNameBuffer[index].Buffer = new ushort[nameArray.Length];
                Array.Copy(nameArray, rpcNameBuffer[index].Buffer, nameArray.Length);
                rpcNameBuffer[index].Length = (ushort)(2 * rpcNameBuffer[index].Buffer.Length);
                rpcNameBuffer[index].MaximumLength = (ushort)(rpcNameBuffer[index].Length + 2);
            }
        }

        #endregion

        #region ConvertSidsToString

        /// <summary>
        /// Method to convert the Sids into string format.
        /// </summary>
        public static void ConvertSidsToString()
        {
            WellknownSidStrings = new string[WellknownSecurityPrincipalMaxCount];
            StringBuilder sidString = new StringBuilder();

            for (int counter = 0; counter < WellknownSecurityPrincipalMaxCount; counter++)
            {
                if (LsatAdapter.TranslatedWellknownSids.Value.Sids[counter].DomainIndex != -1)
                {
                    _RPC_SID counterSid = LsatAdapter.TranslatedWellknownSids.Value.Sids[counter].Sid[0];

                    sidString.Append("S-");
                    sidString.Append(counterSid.Revision);
                    sidString.Append("-");
                    sidString.Append(Convert.ToInt32(
                        counterSid.IdentifierAuthority.Value[IdentifierAuthorityCount - 1]));

                    for (int index = 0; index < counterSid.SubAuthorityCount; index++)
                    {
                        sidString.Append("-");
                        sidString.Append(counterSid.SubAuthority[index]);
                    }

                    WellknownSidStrings[counter] = Convert.ToString(sidString);
                    sidString.Remove(0, sidString.Length);
                }
            }
        }

        #endregion

        #region ConvertUnicodesToString

        /// <summary>
        ///  Method to convert the unicode array into string format.
        /// </summary>
        /// <param name="unicodeValue">Unicode value.</param>
        /// <returns>String value.</returns>
        public static string ConvertUnicodesToString(ushort[] unicodeValue)
        {
            string stringValue = string.Empty;
            for (int i = 0; i < unicodeValue.Length; i++)
            {
                byte[] bytesValue = new byte[2];
                bytesValue[0] = (byte)unicodeValue[i];
                bytesValue[1] = (byte)(unicodeValue[i] >> 8);
                stringValue += Encoding.Unicode.GetString(bytesValue);
            }

            return stringValue;
        }

        #endregion

        #region InitializeWellKnownSecuritySIDs

        /// <summary>
        /// WellknownSecurityPrincipals and WellknownSIDs are given in the Section 6.1.1.2.6 of ADTS protocol.
        /// BuiltinRelativeDomainNames and WellknownRelativeID are given in the Section 6.1.1.4.12 and Section 6.1.1.6 of ADTS protocol.
        /// </summary>
        public static void InitializeWellKnownSecuritySIDs()
        {
            WellknownSecurityPrincipals = new string[WellknownSecurityPrincipalMaxCount]{
            "Anonymous Logon","Authenticated Users","Batch","Console Logon","Creator Group","Creator Owner","Dialup","Digest Authentication",
            "Enterprise Domain Controllers","Everyone","Interactive","IUSR","Local Service","Network","Network Service","NTLM Authentication",
            "Other Organization","Owner Rights","Proxy","Remote Interactive Logon","Restricted","SChannel Authentication","Self","Service",
            "System","Terminal Server User","This Organization"};
            WellknownSIDs = new string[WellknownSecurityPrincipalMaxCount]{
            "S-1-5-7","S-1-5-11","S-1-5-3","S-1-2-1","S-1-3-1","S-1-3-0","S-1-5-1","S-1-5-64-21","S-1-5-9","S-1-1-0","S-1-5-4","S-1-5-17",
            "S-1-5-19","S-1-5-2","S-1-5-20","S-1-5-64-10","S-1-5-1000","S-1-3-4","S-1-5-8","S-1-5-14","S-1-5-12","S-1-5-64-14",
            "S-1-5-10","S-1-5-6","S-1-5-18","S-1-5-13","S-1-5-15"};
            BuiltinRelativeDomainNames = new string[RidMaxCount]{
            "Account Operators","Administrators","Backup Operators","Certificate Service DCOM Access","Cryptographic Operators",
            "Distributed COM Users","Event Log Readers","Guests","IIS_IUSRS","Incoming Forest Trust Builders","Network Configuration Operators",
            "Performance Log Users","Performance Monitor Users","Pre-Windows 2000 operating system Compatible Access","Print Operators","Remote Desktop Users",
            "Replicator","Server Operators","Terminal Server License Servers","Users","Windows Authorization Access Group","Administrator","Guest","krbtgt",
            "Cert Publishers","Domain Admins","Domain Computers","Domain Controllers","Domain Guests","Domain Users","Enterprise Admins","Group Policy Creator Owners",
            "RAS and IAS Servers","Read-Only Domain Controllers","Enterprise Read-Only Domain Controllers","Schema Admins","Allowed RODC Password Replication Group",
            "Denied RODC Password Replication Group"};
            WellknownRelativeID = new int[RidMaxCount]{
            548,544,551,574,569,562,573,546,568,557,556,559,558,554,550,555,552,549,561,545,560,500,501,502,517,512,515,516,514,513,519,520,553,521,498,518,571,572};
        }

        #endregion

        #region GetSids

        /// <summary>
        /// Method to store SIDs in an array which are returned from 
        /// the translatedSids parameter of LsarLookupNames3 method.
        /// </summary>
        public static void GetSids()
        {
            int indexOfNames = 0, indexOfSids = 0;

            while (LsatAdapter.MappedCounts > 0)
            {
                if (LsatAdapter.TranslatedSids3.Value.Sids[indexOfNames].Sid == null)
                {
                    indexOfNames++;
                }
                else
                {
                    arrayOfSids[indexOfSids].Sid = new _RPC_SID[1];
                    arrayOfSids[indexOfSids].Sid[0].Revision =
                        LsatAdapter.TranslatedSids3.Value.Sids[indexOfNames].Sid[0].Revision;
                    arrayOfSids[indexOfSids].Sid[0].IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
                    arrayOfSids[indexOfSids].Sid[0].IdentifierAuthority.Value = new byte[IdentifierAuthorityCount];

                    for (int index = 0; index < IdentifierAuthorityCount; index++)
                    {
                        arrayOfSids[indexOfSids].Sid[0].IdentifierAuthority.Value[index] =
                            LsatAdapter.TranslatedSids3.Value.Sids[indexOfNames].Sid[0].IdentifierAuthority.Value[index];
                    }

                    arrayOfSids[indexOfSids].Sid[0].SubAuthorityCount =
                        LsatAdapter.TranslatedSids3.Value.Sids[indexOfNames].Sid[0].SubAuthorityCount;
                    arrayOfSids[indexOfSids].Sid[0].SubAuthority = new uint[arrayOfSids[indexOfSids].Sid[0].SubAuthorityCount];

                    for (int index = 0; index < arrayOfSids[indexOfSids].Sid[0].SubAuthorityCount; index++)
                    {
                        arrayOfSids[indexOfSids].Sid[0].SubAuthority[index] =
                            LsatAdapter.TranslatedSids3.Value.Sids[indexOfNames].Sid[0].SubAuthority[index];
                    }

                    LsatAdapter.MappedCounts--;
                    indexOfNames++;
                    indexOfSids++;
                }
            }
        }

        #endregion

        #region SetNamesParameter

        /// <summary>
        /// Method to set the names of LsarLookupNames, LsarLookupNames2, LsarLookupNames3 and LsarLookupNames4 methods.
        /// </summary>
        public static void SetNamesParameter()
        {
            namesArray = new string[TestsuiteMaxUserCount];

            if (LsatAdapter.IsUserPrincipalSupports)
            {
                if (!LsatAdapter.NameOfDomain.Contains(@"."))
                {
                    // If domain name doesn't contain the domain suffix, ".com" is as default
                    namesArray[0] = string.Format("{0}" + "@" + "{1}" + ".com", accountNames[0], LsatAdapter.NameOfDomain);
                }
                else
                {
                    namesArray[0] = string.Format("{0}" + "@" + "{1}", accountNames[0], LsatAdapter.NameOfDomain);
                }
            }
            else
            {
                namesArray[0] = accountNames[0];
            }

            if (!LsatAdapter.NameOfDomain.Contains(@"."))
            {
                // If domain name doesn't contain the domain suffix, ".com" is as default
                namesArray[1] = string.Format("{0}" + @".com\" + "{1}", LsatAdapter.NameOfDomain, accountNames[1]);
            }
            else
            {
                namesArray[1] = string.Format("{0}" + @"\" + "{1}", LsatAdapter.NameOfDomain, accountNames[1]);
            }

            namesArray[2] = accountNames[2];

            if (!LsatAdapter.AreSidsInitialized)
            {
                namesArray[3] = accountNames[3].ToUpper();
            }
            else
            {
                namesArray[3] = LsatAdapter.NameOfDomain.ToUpper();
            }

            switch (NamesValidity)
            {
                case SomeNotMapped:

                    for (int index = TestsuiteMaxUserCount - 2; index < TestsuiteMaxUserCount; index++)
                    {
                        namesArray[index] = NonExistingAccount[index];
                    }

                    break;

                case NoneMapped:
                    for (int index = 0; index < TestsuiteMaxUserCount; index++)
                    {
                        namesArray[index] = NonExistingAccount[index];
                    }

                    break;

                default:

                    break;
            }
        }

        #endregion

        #region InitializeLookupNames

        /// <summary>
        /// Method to initialize the Names field of LsarLookupNames4 Message in RPC_UNICODE_STRING format.
        /// </summary>
        public static void InitializeLookupNames()
        {
            LsatAdapter.LsaNames = new _LSA_UNICODE_STRING[TestsuiteMaxUserCount];

            SetNamesParameter();

            for (int index = 0; index < TestsuiteMaxUserCount; index++)
            {
                nameString[index] = namesArray[index];

                char[] nameArray = new char[nameString[index].Length];
                nameArray = nameString[index].ToCharArray();

                LsatAdapter.LsaNames[index].Buffer = new ushort[nameArray.Length];
                Array.Copy(nameArray, LsatAdapter.LsaNames[index].Buffer, nameArray.Length);
                LsatAdapter.LsaNames[index].Length = (ushort)(2 * LsatAdapter.LsaNames[index].Buffer.Length);
                LsatAdapter.LsaNames[index].MaximumLength = (ushort)(LsatAdapter.LsaNames[index].Length + 2);
            }
        }

        #endregion

        #region InitializeNames

        /// <summary>
        /// Method to initialize the Names field of LsarLookupNames3, 
        /// LsarlookupNames2 and LsarLookupNames Messages in RPC_UNICODE_STRING format.
        /// </summary>
        public static void InitializeNames()
        {
            SetNamesParameter();

            LsatAdapter.RpcNames = new _RPC_UNICODE_STRING[TestsuiteMaxUserCount];

            for (int index = 0; index < TestsuiteMaxUserCount; index++)
            {
                nameString[index] = namesArray[index];
                char[] nameArray = new char[nameString[index].Length];
                nameArray = nameString[index].ToCharArray();
                LsatAdapter.RpcNames[index].Buffer = new ushort[nameArray.Length];
                Array.Copy(nameArray, LsatAdapter.RpcNames[index].Buffer, nameArray.Length);

                if (NamesValidity == Invalid)
                {
                    LsatAdapter.RpcNames[index].Length =
                        (ushort)((2 * LsatAdapter.RpcNames[index].Buffer.Length) + 1);
                }
                else
                {
                    LsatAdapter.RpcNames[index].Length =
                        (ushort)(2 * LsatAdapter.RpcNames[index].Buffer.Length);
                }

                LsatAdapter.RpcNames[index].MaximumLength =
                    (ushort)(LsatAdapter.RpcNames[index].Length + 2);
            }
        }

        #endregion

        #region InitializeSids

        /// <summary>
        /// Method to initialize the Sids field of LsarLookupSids2 and LsarLookupSids Messages in RPC_SID format.
        /// </summary>
        public static void InitializeSids()
        {
            int indexOfSids = 0;
            LsatAdapter.SidEnumBuff[0].Entries = TestsuiteMaxUserCount;
            sidEntries = TestsuiteMaxUserCount;
            LsatAdapter.SidEnumBuff[0].SidInfo = new _LSAPR_SID_INFORMATION[TestsuiteMaxUserCount];

            while (sidEntries > 0)
            {
                LsatAdapter.SidEnumBuff[0].SidInfo[indexOfSids].Sid = new _RPC_SID[1];

                if (SidsValidity == Invalid)
                {
                    LsatAdapter.SidEnumBuff[0].SidInfo[indexOfSids].Sid[0].Revision = InvalidRevision;
                }
                else
                {
                    LsatAdapter.SidEnumBuff[0].SidInfo[indexOfSids].Sid[0].Revision = 
                        arrayOfSids[indexOfSids].Sid[0].Revision;
                }

                LsatAdapter.SidEnumBuff[0].SidInfo[indexOfSids].Sid[0].IdentifierAuthority = 
                    new _RPC_SID_IDENTIFIER_AUTHORITY();
                LsatAdapter.SidEnumBuff[0].SidInfo[indexOfSids].Sid[0].IdentifierAuthority.Value = 
                    new byte[IdentifierAuthorityCount];

                for (int index = 0; index < IdentifierAuthorityCount; index++)
                {
                    LsatAdapter.SidEnumBuff[0].SidInfo[indexOfSids].Sid[0].IdentifierAuthority.Value[index] = 
                        arrayOfSids[indexOfSids].Sid[0].IdentifierAuthority.Value[index];
                }

                LsatAdapter.SidEnumBuff[0].SidInfo[indexOfSids].Sid[0].SubAuthorityCount = 
                    arrayOfSids[indexOfSids].Sid[0].SubAuthorityCount;
                LsatAdapter.SidEnumBuff[0].SidInfo[indexOfSids].Sid[0].SubAuthority = 
                    new uint[arrayOfSids[indexOfSids].Sid[0].SubAuthorityCount];

                for (int index = 0; index < arrayOfSids[indexOfSids].Sid[0].SubAuthorityCount; index++)
                {
                    LsatAdapter.SidEnumBuff[0].SidInfo[indexOfSids].Sid[0].SubAuthority[index] = 
                        arrayOfSids[indexOfSids].Sid[0].SubAuthority[index];
                }

                sidEntries--;
                indexOfSids++;
            }

            if (SidsValidity == SomeNotMapped)
            {
                for (int index = TestsuiteMaxUserCount - 2; index < TestsuiteMaxUserCount; index++)
                {
                    for (uint counter = 0;
                        counter < LsatAdapter.SidEnumBuff[0].SidInfo[index].Sid[0].SubAuthorityCount;
                        counter++)
                    {
                        LsatAdapter.SidEnumBuff[0].SidInfo[index].Sid[0].SubAuthority[counter] = 
                            InvalidSubauthority + counter;
                    }
                }
            }
            else if (SidsValidity == NoneMapped)
            {
                for (int index = 0; index < TestsuiteMaxUserCount; index++)
                {
                    for (uint counter = 0;
                        counter < LsatAdapter.SidEnumBuff[0].SidInfo[index].Sid[0].SubAuthorityCount; 
                        counter++)
                    {
                        LsatAdapter.SidEnumBuff[0].SidInfo[index].Sid[0].SubAuthority[counter] = 
                            InvalidSubauthority + counter;
                    }
                }
            }
        }

        #endregion

        #region AreTranslatedNamesEqual

        /// <summary>
        /// Method to check whether the names sent to LsarLookupNames3 
        /// and names returned by LsarLookupSids2 are equal or not.
        /// </summary>
        /// <param name="lookupType">An indicator for specifies the type of the translated name 
        /// structure to be checked with.</param>
        /// <returns>Returns true if the names are equal else false.</returns>
        public static bool AreTranslatedNamesEqual(int lookupType)
        {
            ushort[] accountNameBuff;

            for (int index = 0; index < TestsuiteMaxUserCount; index++)
            {
                accountNameBuff = new ushort[accountNames[index].Length];
                int counter = 0;

                if (lookupType == LookupSidS2)
                {
                    foreach (char character in accountNames[index])
                    {
                        accountNameBuff[counter] = (ushort)character;

                        if (accountNameBuff[counter] != LsatAdapter.TranslatedNames2.Value.Names[index].Name.Buffer[counter])
                        {
                            return false;
                        }

                        counter++;
                    }
                }
                else if (lookupType == LookupSidS1)
                {
                    foreach (char character in accountNames[index])
                    {
                        accountNameBuff[counter] = (ushort)character;

                        if (accountNameBuff[counter] != LsatAdapter.TranslatedNames1.Value.Names[index].Name.Buffer[counter])
                        {
                            return false;
                        }
                        
                        counter++;
                    }
                }
            }

            return true;
        }

        #endregion

        #region MakeSidRequirement

        /// <summary>
        /// Method to build the requirement based on security principal name and security principal sid.
        /// </summary>
        /// <param name="securityPrincipalName">The value of security principal name.</param>
        /// <param name="securityPrincipalSid">The value of security principal sid.</param>
        /// <returns>Returns the format result.</returns>
        public static string MakeSidRequirement(string securityPrincipalName, string securityPrincipalSid)
        {
            return string.Format(
                "For the {0} WellKnownSecurityPrincipal the objectSid attribute value is {1}.", 
                securityPrincipalName, 
                securityPrincipalSid);
        }

        #endregion

        #region MakeRIDRequirement

        /// <summary>
        /// Method to build the requirement based on security principal name and security principal sid.
        /// </summary>
        /// <param name="securityPrincipalName">The value of security principal.</param>
        /// <param name="relativeID">The value of the relative identifier of the 
        /// security principal with respect to its domain.</param>
        /// <returns>Returns the format result.</returns>
        public static string MakeRIDRequirement(string securityPrincipalName, int relativeID)
        {
            return string.Format("The RID attribute of {0} Group Object must be {1}.", securityPrincipalName, relativeID);
        }

        #endregion
    }
}
