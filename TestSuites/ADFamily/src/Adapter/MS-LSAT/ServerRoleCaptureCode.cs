// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat
{
    using System;

    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
    using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

    /// <summary>
    /// Capture Server related requirements.
    /// </summary>
    public partial class LsatAdapter : ADCommonServerAdapter, ILsatAdapter
    {
        #region Verify requirements related with WellKnowSecurityPrincipal in MS-ADTS protocol

        /// <summary>
        /// Verify requirements related with WellKnowSecurityPrincipal in MS-ADTS protocol.
        /// </summary>
        private void VerifyADTSRelatedRequirements1()
        {
            if (MappedCounts > 0)
            {
                for (int index = 0; index < LsatUtilities.WellknownSecurityPrincipalMaxCount; index++)
                {
                    if (LsatUtilities.WellknownSIDs[index] == LsatUtilities.WellknownSidStrings[index])
                    {
                        #region MS-ADTS_R1001 to MS-ADTS_R1027

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-ADTS_R10{0:D2}, the actual value of the objectSid attribute of 
                            the {1} WellKnowSecurityPrincipal is: {2}.",
                            index + 1,
                            LsatUtilities.WellknownSecurityPrincipals[index],
                            LsatUtilities.WellknownSIDs[index]);
                        
                        // Verify MS-ADTS requirement: MS-ADTS_R1001 to MS-ADTS_R1026
                        LsatTestSite.CaptureRequirement(
                            "MS-ADTS",
                            1001 + index,
                            LsatUtilities.MakeSidRequirement(
                                LsatUtilities.WellknownSecurityPrincipals[index],
                                LsatUtilities.WellknownSIDs[index]));

                        #endregion
                    }
                }
            }
        }

        #endregion

        #region Verify requirements related with Group Object in MS-ADTS protocol

        /// <summary>
        /// Verify requirements related with Group Object in MS-ADTS protocol.
        /// </summary>
        private void VerifyADTSRelatedRequirements2()
        {
            if (MappedCounts > 0)
            {
                for (int index = 0; index < LsatUtilities.RidMaxCount; index++)
                {
                    if (this.translatedSids1.Value.Sids[index].RelativeId == LsatUtilities.WellknownRelativeID[index])
                    {
                        #region MS-ADTS_R1028 to MS-ADTS_R1065

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-ADTS_R10{0:D2}, the actual value of the RID  attribute of 
                            the {1} Group Object is: {2}.",
                            28 + index,
                            LsatUtilities.BuiltinRelativeDomainNames[index],
                            LsatUtilities.WellknownRelativeID[index]);

                        // Verify MS-ADTS requirement: MS-ADTS_R1027 to MS-ADTS_R1062
                        LsatTestSite.CaptureRequirement(
                            "MS-ADTS",
                            1028 + index,
                            LsatUtilities.MakeRIDRequirement(
                                LsatUtilities.BuiltinRelativeDomainNames[index],
                                LsatUtilities.WellknownRelativeID[index]));

                        #endregion
                    }
                }
            } // End of if (mappedCount > 0)
        }

        #endregion

        #region Verify requirements related with LsarGetUserName interface

        /// <summary>
        /// Verify requirements related with LsarGetUserName interface.
        /// </summary>
        /// <param name="principalName">The security principal name.</param>
        /// <param name="netbiosName">The domain NetBIOS name.</param>
        private void VerifyLsarGetUserName(
            string principalName,
            string netbiosName)
        {
            string expectedNetbiosName = string.Empty;
            string actualNetbiosName = string.Empty;

            #region MS-LSAT_R914

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-LSAT_R914, the actual value of the domain NetBIOS name is: {0}.",
                netbiosName);

            expectedNetbiosName = NameOfDomain.Split(new char[] { '.' })[0].ToUpper();
            actualNetbiosName = netbiosName.ToUpper();

            if (!string.IsNullOrEmpty(netbiosName))
            {
                // Verify MS-LSAT requirement: MS-LSAT_R914
                LsatTestSite.CaptureRequirementIfAreEqual(
                    expectedNetbiosName,
                    actualNetbiosName,
                    914,
                    @"[In LsarGetUserName (Opnum 45)]After the security principal is located, 
                    the RPC server MUST return the domain NetBIOS name in the DomainName parameter 
                    if DomainName is not NULL.");
            }

            #endregion

            #region MS-LSAT_R189

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-LSAT_R189, the actual value of the security principal name is: {0}.",
                principalName);

            // Verify MS-LSAT requirement: MS-LSAT_R189
            LsatTestSite.CaptureRequirementIfIsTrue(
                this.currentUserName.ToUpper().Equals(principalName.ToUpper()),
                "MS-LSAT",
                189,
                @"In 'UserName ' parameter of LsarGetUserName interface of  LSAT Remote Protocol: On return, contains 
                the name of the security principal making the call. This MUST be of the form samAccountName (logon 
                name used to support clients and servers running older versions of the operating system");

            #endregion

            #region MS-LSAT_R191

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-LSAT_R191, the actual value of the domain NetBIOS name is: {0}.",
                netbiosName);

            // Verify MS-LSAT requirement: MS-LSAT_R191
            LsatTestSite.CaptureRequirementIfAreEqual(
                expectedNetbiosName,
                actualNetbiosName,
                191,
                @"In 'DomainName' parameter of LsarGetUserName interface of  LSAT Remote Protocol: On return, 
                it contains the domain name of the security principal invoking the method. 
                This value MUST contain the NetBIOS name.");

            #endregion

            #region MS-LSAT_R913

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-LSAT_R913, the actual value of the security principal name is: {0}.",
                principalName);

            // Verify MS-LSAT requirement: MS-LSAT_R913
            LsatTestSite.CaptureRequirementIfIsTrue(
                this.currentUserName.ToUpper().Equals(principalName.ToUpper()),
                "MS-LSAT",
                913,
                @"[In LsarGetUserName (Opnum 45)]After the security principal is located, 
                the RPC server MUST return the security principal name in the UserName parameter.");

            #endregion

            #region MS-LSAT_R915

            // Verify MS-LSAT requirement: MS-LSAT_R915
            // If the program goes to here, it inidicates that the return value of invoking method LsarGetUserName 
            // is STATUS_SUCCESS, the security principal name in the UserName parameter and the domain NetBIOS name 
            // in the DomainName parameter are returned when DomainName is not NULL. 
            // So the MS-LSAT_R915 can be directly verified in this case.
            LsatTestSite.CaptureRequirement(
                915,
                @"[In LsarGetUserName (Opnum 45)]The return value MUST be set to STATUS_SUCCESS in this case.");

            #endregion
        }

        #endregion

        #region Verify requirements related with LsarOpenPolicy interface

        /// <summary>
        /// Verify requirements related with LsarOpenPolicy interface.
        /// </summary>
        /// <param name="rootDirectory">Contains Null value or Non-Null Value.</param>
        private void VerifyLsarOpenPolicy(RootDirectory rootDirectory)
        {
            if (rootDirectory == RootDirectory.Null
                    && (this.desiredAccess & InvalidDesiredAccess) == 0)
            {
                #region MS-LSAT_R5

                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    "Verify MS-LSAT_R5, the actual return value of the LsarOpenPolicy is: 0x{0:X8}",
                    this.methodStatus);

                // Verify MS-LSAT requirement: MS-LSAT_R5
                LsatTestSite.CaptureRequirementIfAreEqual(
                    LsatUtilities.StatusSuccess,
                    this.methodStatus,
                    "MS-LSAT",
                    5,
                    @"LSAT Remote Protocol MUST use '\\PIPE\\lsarpc' as the RPC endpoint when using RPC over SMB.");

                #endregion

                #region MS-LSAT_R10

                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    "Verify MS-LSAT_R10, the actual return value of the LsarOpenPolicy is: 0x{0:X8}",
                    this.methodStatus);

                // Verify MS-LSAT requirement: MS-LSAT_R10
                LsatTestSite.CaptureRequirementIfAreEqual(
                    LsatUtilities.StatusSuccess,
                    this.methodStatus,
                    "MS-LSAT",
                    10,
                    @"LSAT Remote Protocol for the RPC interface MUST use the UUID 
                    as (12345778-1234-ABCD-EF00-0123456789AB). ");

                #endregion
            }
        }

        #endregion

        #region Verify requirements related with LsarLookupNames4 interface

        /// <summary>
        /// Verify requirements related with LsarLookupNames4 interface.
        /// </summary>
        /// <param name="translatedSids">Specifies a set of translated SIDs.</param>
        private void VerifyLsarLookupNames4(_LSAPR_TRANSLATED_SIDS_EX2? translatedSids)
        {
            switch (LsatUtilities.NamesValidity)
            {
                #region Success

                case LsatUtilities.Success:

                    #region MS-LSAT_R198

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R198, the actual return value of the LsarLookupNames4 is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R198
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusSuccess,
                        this.methodStatus,
                        "MS-LSAT",
                        198,
                        @"In Names parameter of the LsarLookupNames4 interface of  LSAT Remote Protocol: server 
                        MUST support User principal names like  user_name@example.example.com.");

                    #endregion

                    #region MS-LSAT_R199

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R199, the actual return value of the LsarLookupNames4 is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R199
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusSuccess,
                        this.methodStatus,
                        "MS-LSAT",
                        199,
                        @"In 'Names' parameter of the LsarLookupNames4 interface of  LSAT Remote Protocol: server 
                        MUST support Fully qualified account names based on either DNS  or NetBIOS names like 
                        example.example.com\\user_name or example\\user_name.");

                    #endregion

                    #region MS-LSAT_R200

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R200, the actual return value of the LsarLookupNames4 is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R200
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusSuccess,
                        this.methodStatus,
                        "MS-LSAT",
                        200,
                        @"In 'Names' parameter of the LsarLookupNames4 interface:  Server MUST support Unqualified 
                        or isolated names like user_name.");

                    #endregion

                    #region MS-LSAT_R201

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R201, the actual return value of the LsarLookupNames4 is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R201
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusSuccess,
                        this.methodStatus,
                        "MS-LSAT",
                        201,
                        @"In LsarLookupNames4, the Names parameter MUST NOT be case sensitive.");

                    #endregion

                    #region MS-LSAT_R224

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R224, the actual return value of the LsarLookupNames4 is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R224
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusSuccess,
                        this.methodStatus,
                        "MS-LSAT",
                        224,
                        @"In LsarLookupNames4 interface:  The RPC server MUST check each element in the Names parameter
                        for validity, as described for the RPC_UNICODE_STRING structure in [MS-DTYP] section 2.3.9.");

                    #endregion

                    #region MS-LSAT_R247

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R247, the actual value of the DomainIndex field of 
                        the TranslatedSids entry is: {0}.",
                        translatedSids.Value.Sids[0].DomainIndex.ToString());

                    // Verify MS-LSAT requirement: MS-LSAT_R247
                    LsatTestSite.CaptureRequirementIfIsTrue(
                        translatedSids.Value.Sids[0].DomainIndex >= 0,
                        "MS-LSAT",
                        247,
                        @"In LsarLookupNames4 interface: For a successful search, DomainIndex field of TranslatedSids 
                        entry MUST be updated with Index of the domain in the ReferencedDomains parameter.");

                    #endregion

                    break;

                #endregion

                #region None_Mapped

                case LsatUtilities.NoneMapped:

                    #region MS-LSAT_R248

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R248, the actual value of the Use field of TranslatedSids entry is: {0}.",
                        translatedSids.Value.Sids[0].Use.ToString());

                    // Verify MS-LSAT requirement: MS-LSAT_R248
                    LsatTestSite.CaptureRequirementIfIsTrue(
                        translatedSids.Value.Sids[0].Use == _SID_NAME_USE.SidTypeUnknown,
                        "MS-LSAT",
                        248,
                        @"In LsarLookupNames4 interface: If a match cannot be found for a name, Use field of 
                        TranslatedSids entry MUST be updated with SidTypeUnknown.");

                    #endregion

                    #region MS-LSAT_R249

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R249, the Sid field of TranslatedSids entry is {0} null.",
                        translatedSids.Value.Sids[0].Sid == null ? string.Empty : "not");

                    // Verify MS-LSAT requirement: MS-LSAT_R249
                    LsatTestSite.CaptureRequirementIfIsTrue(
                        translatedSids.Value.Sids[0].Sid == null,
                        "MS-LSAT",
                        249,
                        @"In LsarLookupNames4 interface: If a match cannot be found for a name, Sid field of 
                        TranslatedSids entry MUST be updated with NULL.");

                    #endregion

                    #region MS-LSAT_R250

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R250, the actual value of the Flags field of TranslatedSids entry is: {0}.",
                        translatedSids.Value.Sids[0].Flags.ToString());

                    // Verify MS-LSAT requirement: MS-LSAT_R250
                    LsatTestSite.CaptureRequirementIfIsTrue(
                        translatedSids.Value.Sids[0].Flags == LsatUtilities.FlagValueZero,
                        "MS-LSAT",
                        250,
                        @"In LsarLookupNames4 interface: If a match cannot be found for a name, Flags field of 
                        TranslatedSids entry MUST be updated with 0x00000000.");

                    #endregion

                    #region MS-LSAT_R251

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R251, the actual value of the DomainIndex field of 
                        TranslatedSids entry is: {0}.",
                        translatedSids.Value.Sids[0].DomainIndex.ToString());

                    // Verify MS-LSAT requirement: MS-LSAT_R251
                    LsatTestSite.CaptureRequirementIfIsTrue(
                        translatedSids.Value.Sids[0].DomainIndex == -1,
                        "MS-LSAT",
                        251,
                        @"In LsarLookupNames4 interface: If a match cannot be found for a name, DomainIndex field of 
                        TranslatedSids entry MUST be updated with -1.");

                    #endregion

                    break;

                #endregion

                #region default

                default:

                    break;

                #endregion
            }
        }

        #endregion

        #region Verify requirements related with Names parameter in LsarLookupNames3 interface

        /// <summary>
        /// Verify requirements related with Names parameter in LsarLookupNames3 interface.
        /// </summary>
        /// <param name="optionOfLookup">Specifies the flag value whether the MSB is set or not.</param>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        /// <param name="translateSids">An output parameter specifies whether translated sid 
        /// is Valid or Invalid.</param>
        /// <param name="mapCount">An output parameter specifies whether mapped count is Valid or Invalid.</param>
        /// <returns>Returns NoneMapped if none of the secPrincipalNames are translated into their SID form, 
        /// else return Success.</returns>
        private ErrorStatus VerifyLsarLookupNames3Names(
            LookUpOption optionOfLookup,
            LookUpLevel levelOfLookup,
            out TranslatedSid translateSids,
            out MappedCount mapCount)
        {
            if (this.desiredAccess == PolicyLookupNames
                    && levelOfLookup != LookUpLevel.Invalid
                    && !(levelOfLookup != LookUpLevel.LookUpWKSTA
                    && optionOfLookup == LookUpOption.MSBSet))
            {
                if (LsatUtilities.NamesValidity == LsatUtilities.NoneMapped)
                {
                    if (!this.isDomainController)
                    {
                        translateSids = TranslatedSid.Invalid;
                        mapCount = MappedCount.Invalid;

                        return ErrorStatus.NoneMapped;
                    }
                }

                switch (LsatUtilities.NamesValidity)
                {
                    #region Invalid

                    case LsatUtilities.Invalid:

                        #region MS-LSAT_R284

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R284, the actual return value of the LsarLookupNames3 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R284
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusInvalidParameter,
                            this.methodStatus,
                            "MS-LSAT",
                            284,
                            @"In LsarLookupNames3 interface: If validation fails while checking  each element in the 
                            Names parameter by the RPC server, then it  MUST return 
                            LsatUtilities.STATUS_INVALID_PARAMETER.");

                        #endregion

                        break;

                    #endregion

                    #region Success

                    case LsatUtilities.Success:

                        #region MS-LSAT_R32

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R32, the actual return value of the LsarLookupNames3 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R32
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            32,
                            @"In 'ACCESS_MASK' field of the structure ACCESS_MASK of LSAT Remote Protocol: If this value 
                            is (0x80000000) POLICY_LOOKUP_NAMES , then Access to translate names and SIDs is granted to 
                            the client (getting the correct handle from OpenPolicy() method).");

                        #endregion

                        if (this.referencedDomains.Value.Entries != 0)
                        {
                            #region MS-LSAT_R34

                            // Add the debug information
                            Site.Log.Add(
                                LogEntryKind.Debug,
                                @"Verify MS-LSAT_R34, the Domains field of LSAPR_REFERENCED_DOMAIN_LIST structure 
                                is {0} null.",
                                this.referencedDomains.Value.Domains == null ? string.Empty : "not");

                            // Verify MS-LSAT requirement: MS-LSAT_R34
                            LsatTestSite.CaptureRequirementIfIsTrue(
                                this.referencedDomains.Value.Domains != null,
                                "MS-LSAT",
                                34,
                                @"In 'Domains' field of the structure LSAPR_REFERENCED_DOMAIN_LIST of LSAT Remote 
                                Protocol: Contains a set of structures that identify domains. If the Entries parameter 
                                in this structure is not 0, then this parameter MUST be non-NULL. ");

                            #endregion
                        }

                        #region MS-LSAT_R159

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R159, the actual return value of the LsarOpenPolicy2 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R159
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            159,
                            @"In 'DesiredAccess' parameter of LsarOpenPolicy2 interface of  LSAT Remote Protocol: 
                            An ACCESS_MASK value that specifies the requested access rights that MUST be granted 
                            on the returned PolicyHandle, if the request is successful.");

                        #endregion

                        #region MS-LSAT_R168

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R168, the actual return value of the LsarOpenPolicy2 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R168
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            168,
                            @"In LsarOpenPolicy2 interface of  LSAT Remote Protocol: The context created by the 
                            implementation that is referenced by PolicyHandle on return MUST contain the access 
                            granted by the server implementation as a result of validating the DesiredAccess.");

                        #endregion

                        if (IsUserPrincipalSupports)
                        {
                            #region MS-LSAT_R259

                            // Add the debug information
                            Site.Log.Add(
                                LogEntryKind.Debug,
                                @"Verify MS-LSAT_R259, the actual return value of the LsarLookupNames3 is: 0x{0:X8}.",
                                this.methodStatus);

                            // Verify MS-LSAT requirement: MS-LSAT_R259
                            LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                                LsatUtilities.StatusSuccess,
                                this.methodStatus,
                                "MS-LSAT",
                                259,
                                @"In Names parameter of the LsarLookupNames3 interface of  LSAT Remote Protocol: server 
                                MUST support User principal names like  user_name@example.example.com.");

                            #endregion
                        }

                        #region MS-LSAT_R260

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R260, the actual return value of the LsarLookupNames3 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R260
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            260,
                            @"In 'Names' parameter of the LsarLookupNames3 interface of  LSAT Remote Protocol: server 
                            MUST support Fully qualified account names based on either DNS  or NetBIOS names like 
                            example.example.com\\user_name or example\\user_name.");

                        #endregion

                        #region MS-LSAT_R261

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R261, the actual return value of the LsarLookupNames3 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R261
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            261,
                            @"In 'Names' parameter of the LsarLookupNames3 interface:  Server MUST support Unqualified 
                            or isolated names like user_name.");

                        #endregion

                        #region MS-LSAT_R262

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R262, the actual return value of the LsarLookupNames3 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R262
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            262,
                            @"In LsarLookupNames3, the Names parameter MUST NOT be case sensitive.");

                        #endregion

                        #region MS-LSAT_R283

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R283, the actual return value of the LsarLookupNames3 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R283
                        LsatTestSite.CaptureRequirementIfAreNotEqual<uint>(
                            LsatUtilities.StatusInvalidParameter,
                            this.methodStatus,
                            "MS-LSAT",
                            283,
                            @"In LsarLookupNames3 interface:  The RPC server MUST check each element in the Names 
                            parameter for validity, as described for the RPC_UNICODE_STRING structure 
                            in [MS-DTYP] section 2.3.9.");

                        #endregion

                        #region MS-LSAT_R306

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R306, the actual value of the DomainIndex field of TranslatedSids entry 
                            is: {0}.",
                            TranslatedSids3.Value.Sids[0].DomainIndex.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R306
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            TranslatedSids3.Value.Sids[0].DomainIndex >= 0,
                            "MS-LSAT",
                            306,
                            @"In LsarLookupNames3 interface: For a successful search, DomainIndex field of TranslatedSids 
                            entry MUST be updated with Index of the domain in the ReferencedDomains parameter.");

                        #endregion

                        #region MS-LSAT_R68

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R68, the actual value of the DomainIndex field 
                            of LSA_TRANSLATED_SID_EX2 structure is: {0},
                            and the Domains field of LSAPR_REFERENCED_DOMAIN_LIST structure is {1} null.",
                            TranslatedSids3.Value.Sids[0].DomainIndex,
                            this.referencedDomains.Value.Domains == null ? string.Empty : "not");

                        // Verify MS-LSAT requirement: MS-LSAT_R68
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            TranslatedSids3.Value.Sids[0].DomainIndex >= 0 
                                && this.referencedDomains.Value.Domains != null,
                            "MS-LSAT",
                            68,
                            @"The LSA_TRANSLATED_SID_EX2 structure contains information about a security principal after 
                            translation it has been translated into a SID. This structure MUST always be accompanied  
                            by an LSAPR_REFERENCED_DOMAIN_LIST structure when DomainIndex is not -1.");

                        #endregion

                        #region MS-LSAT_R312

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R312, the actual return value of the LsarLookupNames3 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R312
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            312,
                            @"In LsarLookupNames3 interface: The return value MUST be set to STATUS_SUCCESS if all names 
                            are translated correctly.");

                        #endregion

                        break;

                    #endregion

                    #region None_Mapped

                    case LsatUtilities.NoneMapped:

                        #region MS-LSAT_R72

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R72, the actual value of the DomainIndex field 
                            of LSAPR_TRANSLATED_SID_EX2 structure is: {0}.",
                            TranslatedSids3.Value.Sids[0].DomainIndex.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R72
                        LsatTestSite.CaptureRequirementIfIsFalse(
                            TranslatedSids3.Value.Sids[0].DomainIndex < -1,
                            "MS-LSAT",
                            72,
                            @"In 'DomainIndex' field of the structure LSAPR_TRANSLATED_SID_EX2  of LSAT Remote Protocol:
                            Contains the index of the domain in which the security principal is in. No negative values 
                            must be returned by this structure except -1.");

                        #endregion

                        #region MS-LSAT_R71

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R71, the actual value of the DomainIndex field 
                            of LSAPR_TRANSLATED_SID_EX2 structure is: {0},
                            and the Domains field of LSAPR_REFERENCED_DOMAIN_LIST structure is {1} null.",
                            TranslatedSids3.Value.Sids[0].DomainIndex,
                            this.referencedDomains.Value.Domains == null ? string.Empty : "not");

                        // Verify MS-LSAT requirement: MS-LSAT_R71
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            TranslatedSids3.Value.Sids[0].DomainIndex == -1 
                                && this.referencedDomains.Value.Domains == null,
                            "MS-LSAT",
                            71,
                            @"In 'DomainIndex' field of the structure LSAPR_TRANSLATED_SID_EX2  of LSAT Remote Protocol:
                            Contains the index of the domain in which the security principal is in. This value of -1 
                            MUST be used in structure in to specify that there are no corresponding domains.");

                        #endregion

                        #region MS-LSAT_R307

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R307, the actual value of the Use field of TranslatedSids entry is: {0}.",
                            TranslatedSids3.Value.Sids[0].Use.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R307
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            TranslatedSids3.Value.Sids[0].Use == _SID_NAME_USE.SidTypeUnknown,
                            "MS-LSAT",
                            307,
                            @"In LsarLookupNames3 interface: If a match cannot be found for a name, Use field 
                            of TranslatedSids entry MUST be updated with SidTypeUnknown.");

                        #endregion

                        #region MS-LSAT_R308

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R308, the Sid field of TranslatedSids entry is {0} null.",
                            TranslatedSids3.Value.Sids[0].Sid == null ? string.Empty : "not");

                        // Verify MS-LSAT requirement: MS-LSAT_R308
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            TranslatedSids3.Value.Sids[0].Sid == null,
                            "MS-LSAT",
                            308,
                            @"In LsarLookupNames3 interface: If a match cannot be found for a name, Sid field 
                            of TranslatedSids entry MUST be updated with NULL.");

                        #endregion

                        #region MS-LSAT_R309

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R309, the actual value of the Flags field of TranslatedSids entry is: {0}.",
                            TranslatedSids3.Value.Sids[0].Flags.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R309
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            (uint)TranslatedSids3.Value.Sids[0].Flags == LsatUtilities.FlagValueZero,
                            "MS-LSAT",
                            309,
                            @"In LsarLookupNames3 interface: If a match cannot be found for a name, Flags field of 
                            TranslatedSids entry MUST be updated with 0x00000000.");

                        #endregion

                        #region MS-LSAT_R310

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R310, the actual value of the DomainIndex field of TranslatedSids entry 
                            is: {0}.",
                            TranslatedSids3.Value.Sids[0].DomainIndex.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R310
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            TranslatedSids3.Value.Sids[0].DomainIndex == -1,
                            "MS-LSAT",
                            310,
                            @"If a match cannot be found for a name, DomainIndex field of TranslatedSids entry MUST 
                            be updated with -1.");

                        #endregion

                        break;

                    #endregion

                    #region default

                    default:

                        break;

                    #endregion
                }
            }

            translateSids = TranslatedSid.Valid;
            mapCount = MappedCount.Valid;

            return ErrorStatus.Success;
        }

        #endregion

        #region Verify requirements related with LsarLookupNames3 Message

        /// <summary>
        /// Verify requirements related with LsarLookupNames3 Message.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupNames3Message(LookUpLevel levelOfLookup)
        {
            if (!this.isDomainController)
            {
                if (levelOfLookup == LookUpLevel.Invalid)
                {
                    #region MS-LSAT_R316

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R316, the actual return value of the LsarLookupNames3 is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R316
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusInvalidParameter,
                        this.methodStatus,
                        "MS-LSAT",
                        316,
                        @"In LSAT Remote Protocol the LsarLookupNames3 message MUST be valid 
                        on non-domain controller machines.");

                    #endregion
                }
            }
            else
            {
                if (levelOfLookup == LookUpLevel.Invalid)
                {
                    #region MS-LSAT_R629

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R629, the actual return value of the LsarLookupNames3 is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R629
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusInvalidParameter,
                        this.methodStatus,
                        "MS-LSAT",
                        629,
                        @"In LSAT Remote Protocol the LsarLookupNames3 message MUST be valid 
                        on domain controllers machines.");

                    #endregion
                }
            }
        }

        #endregion

        #region Verify requirements related with TranslatedSids structure in LsarLookupNames3 interface

        /// <summary>
        /// Verify requirements related with TranslatedSids structure in LsarLookupNames3 interface.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupNames3TranslatedSids(LookUpLevel levelOfLookup)
        {
            if ((this.methodStatus == LsatUtilities.StatusInvalidParameter
                    || this.methodStatus == LsatUtilities.StatusAccessDenied)
                    && levelOfLookup == LookUpLevel.LookUpWKSTA)
            {
                #region MS-LSAT-R315

                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-LSAT_R315, the ReferencedDomains is {0} null,and the Sids field 
                    in TranslatedSids structure is {1} null.",
                    this.referencedDomains == null ? string.Empty : "not",
                    TranslatedSids3.Value.Sids == null ? string.Empty : "not");

                // Verify MS-LSAT requirement: MS-LSAT_R315
                LsatTestSite.CaptureRequirementIfIsTrue(
                    this.referencedDomains == null && TranslatedSids3.Value.Sids == null,
                    "MS-LSAT",
                    315,
                    @"In LsarLookupNames3 interface:If LookupLevel is LsapLookupWksta, and the return code can be 
                    identified as an error value (that is, less than 0) other than STATUS_NONE_MAPPED, ReferencedDomains 
                    and the Sids field in the TranslatedSids structure MUST NOT be returned.");

                #endregion
            }
        }

        #endregion

        #region Verify requirements related with Names parameter in LsarLookupNames2 interface

        /// <summary>
        /// Verify requirements related with Names parameter in LsarLookupNames2 interface.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupNames2Names(LookUpLevel levelOfLookup)
        {
            if (this.count > 0
                    && this.desiredAccess == PolicyLookupNames
                    && levelOfLookup != LookUpLevel.Invalid)
            {
                switch (LsatUtilities.NamesValidity)
                {
                    #region Invalid

                    case LsatUtilities.Invalid:

                        #region MS-LSAT_R345

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R345, the actual return value of the LsarLookupNames2 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R345
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusInvalidParameter,
                            this.methodStatus,
                            "MS-LSAT",
                            345,
                            @"In LsarLookupNames2 interface: If validation fails while checking  each element 
                            in the Names parameter by the RPC server, then it  MUST return 
                            LsatUtilities.STATUS_INVALID_PARAMETER.");

                        #endregion

                        break;

                    #endregion

                    #region Success

                    case LsatUtilities.Success:

                        #region MS-LSAT_R320

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R320, the actual return value of the LsarLookupNames2 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R320
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            320,
                            @"In Names parameter of the LsarLookupNames2 interface of  LSAT Remote Protocol: server MUST 
                            support User principal names like  user_name@example.example.com.");

                        #endregion

                        #region MS-LSAT_R321

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R321, the actual return value of the LsarLookupNames2 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R321
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            321,
                            @"In 'Names' parameter of the LsarLookupNames2 interface of  LSAT Remote Protocol: server 
                            MUST support Fully qualified account names based on either DNS  or NetBIOS names like 
                            example.example.com\\user_name or example\\user_name.");

                        #endregion

                        #region MS-LSAT_R322

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R322, the actual return value of the LsarLookupNames2 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R322
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            322,
                            @"In 'Names' parameter of the LsarLookupNames2 interface:  Server MUST support Unqualified 
                            or isolated names like user_name.");

                        #endregion

                        #region MS-LSAT_R323

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R323, the actual return value of the LsarLookupNames2 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R323
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            323,
                            @"In LsarLookupNames2, the Names parameter MUST NOT be case sensitive.");

                        #endregion

                        if (this.translatedSids2.Value.Sids[0].Use != _SID_NAME_USE.SidTypeDomain
                                && this.translatedSids2.Value.Sids[0].Flags !=
                                   Flags_Values.NameMatchingLastDatabaseView)
                        {
                            #region MS-LSAT_R380

                            // Add the debug information
                            Site.Log.Add(
                                LogEntryKind.Debug,
                                @"Verify MS-LSAT_R380, the actual value of the RelativeId parameter 
                                in the structure TranslatedSids is: {0}.",
                                this.translatedSids2.Value.Sids[0].RelativeId.ToString());

                            // Verify MS-LSAT requirement: MS-LSAT_R380
                            LsatTestSite.CaptureRequirementIfAreNotEqual<uint>(
                                0xFFFFFFFF,
                                this.translatedSids2.Value.Sids[0].RelativeId,
                                "MS-LSAT",
                                380,
                                @"In LsarLookupNames2 of LSAT Remote Protocol, the RelativeId parameter in the output 
                                structure of TranslatedSids MUST be computed as the last SubAuthority 
                                in the RPC_SID structure ([MS-DTYP] section 2.4.8) if the translated SID 
                                is not of type SidTypeDomain, and if the Flags field does not contain 0x00000004.");

                            #endregion
                        }

                        for (int index = 0; index < this.count; index++)
                        {
                            if (this.translatedSids2.Value.Sids[index].Use == _SID_NAME_USE.SidTypeDomain
                                    || this.translatedSids2.Value.Sids[index].Flags == 
                                       Flags_Values.NameMatchingLastDatabaseView)
                            {
                                #region MS-LSAT_R381

                                // Add the debug information
                                Site.Log.Add(
                                    LogEntryKind.Debug,
                                    @"Verify MS-LSAT_R381, the actual value of the RelativeId parameter 
                                    in the structure TranslatedSids is: {0}.",
                                    this.translatedSids2.Value.Sids[0].RelativeId.ToString());

                                // Verify MS-LSAT requirement: MS-LSAT_R381
                                LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                                    0xFFFFFFFF,
                                    this.translatedSids2.Value.Sids[index].RelativeId,
                                    "MS-LSAT",
                                    381,
                                    @"In LsarLookupNames2 of LSAT Remote Protocol, If the translated SID is of type 
                                    SidTypeDomain or the Flags field contains 0x00000004, 
                                    RelativeId MUST be set to 0xFFFFFFFF.");

                                #endregion

                                break;
                            }
                        }

                        #region MS-LSAT_R344

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R344, the actual return value of the LsarLookupNames2 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R344
                        LsatTestSite.CaptureRequirementIfAreNotEqual<uint>(
                            LsatUtilities.StatusInvalidParameter,
                            this.methodStatus,
                            "MS-LSAT",
                            344,
                            @"In LsarLookupNames2 interface:  The RPC server MUST check each element in the Names 
                            parameter for validity, as described for the RPC_UNICODE_STRING structure 
                            in [MS-DTYP] section 2.3.9.");

                        #endregion

                        #region MS-LSAT_R367

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R367, the actual value of the DomainIndex field 
                            of TranslatedSids entry is: {0}.",
                            this.translatedSids2.Value.Sids[0].DomainIndex.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R367
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            this.translatedSids2.Value.Sids[0].DomainIndex >= 0,
                            "MS-LSAT",
                            367,
                            @"In LsarLookupNames2 interface: For a successful search, DomainIndex field of 
                            TranslatedSids entry MUST be updated with Index of the domain 
                            in the ReferencedDomains parameter.");

                        #endregion

                        #region MS-LSAT_R61

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R61, the actual value of the DomainIndex field 
                            of LSA_TRANSLATED_SID_EX structure is: {0},
                            and the Domains field of LSAPR_REFERENCED_DOMAIN_LIST structure is {1} null.",
                            this.translatedSids2.Value.Sids[0].DomainIndex,
                            this.referencedDomains.Value.Domains == null ? string.Empty : "not");

                        // Verify MS-LSAT requirement: MS-LSAT_R61
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            this.translatedSids2.Value.Sids[0].DomainIndex >= 0 
                                && this.referencedDomains.Value.Domains != null,
                            "MS-LSAT",
                            61,
                            @"The LSA_TRANSLATED_SID_EX structure contains information about a security principal after 
                            translation it has been translated into a SID. This structure MUST always be accompanied  
                            by an LSAPR_REFERENCED_DOMAIN_LIST structure when DomainIndex is not -1.");

                        #endregion

                        #region MS-LSAT_R373

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R373, the actual return value of the LsarLookupNames2 is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R373
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            373,
                            @"In LsarLookupNames2 interface: The return value MUST be set to STATUS_SUCCESS if all names 
                            are translated correctly.");

                        #endregion

                        break;

                    #endregion

                    #region None_Mapped

                    case LsatUtilities.NoneMapped:

                        #region MS-LSAT_R63

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R63, the actual value of the DomainIndex field 
                            of LSAPR_TRANSLATED_SID_EX structure is: {0}.",
                            this.translatedSids2.Value.Sids[0].DomainIndex.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R63
                        LsatTestSite.CaptureRequirementIfIsFalse(
                            this.translatedSids2.Value.Sids[0].DomainIndex < -1,
                            "MS-LSAT",
                            63,
                            @"In 'DomainIndex' field of the structure LSAPR_TRANSLATED_SID_EX of LSAT Remote Protocol: 
                            Contains the index of the domain in which the security principal is in. No negative values 
                            must be returned by this structure except -1.");

                        #endregion

                        #region MS-LSAT_R62

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R62, the actual value of the DomainIndex field 
                            of LSAPR_TRANSLATED_SID_EX structure is: {0},
                            and the Domains field of LSAPR_REFERENCED_DOMAIN_LIST structure is {1} null.",
                            this.translatedSids2.Value.Sids[0].DomainIndex,
                            this.referencedDomains.Value.Domains == null ? string.Empty : "not");

                        // Verify MS-LSAT requirement: MS-LSAT_R62
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            this.translatedSids2.Value.Sids[0].DomainIndex == -1 
                                && this.referencedDomains.Value.Domains == null,
                            "MS-LSAT",
                            62,
                            @"In 'DomainIndex' field of the structure LSAPR_TRANSLATED_SID_EX of LSAT Remote Protocol: 
                            Contains the index of the domain in which the security principal is in. This value of -1 
                            MUST be used to specify that there are no corresponding domains.");

                        #endregion

                        #region MS-LSAT_R368

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R368, the actual value of the Use field of TranslatedSids entry is: {0}.",
                            this.translatedSids2.Value.Sids[0].Use.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R368
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            this.translatedSids2.Value.Sids[0].Use == _SID_NAME_USE.SidTypeUnknown,
                            "MS-LSAT",
                            368,
                            @"In LsarLookupNames2 interface: If a match cannot be found for a name, Use field of 
                            TranslatedSids entry MUST be updated with SidTypeUnknown.");

                        #endregion

                        #region MS-LSAT_R369

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R369, the actual value of the RelativeId parameter 
                            of TranslatedSids entry is: {0}.",
                            this.translatedSids2.Value.Sids[0].RelativeId.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R369
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            this.translatedSids2.Value.Sids[0].RelativeId == 0,
                            "MS-LSAT",
                            369,
                            @"In LsarLookupNames2 interface: If a match cannot be found for a name, Sid field of 
                            TranslatedSids entry MUST be updated with NULL.");

                        #endregion

                        #region MS-LSAT_R370

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R370, the actual value of the Flags field 
                            of TranslatedSids entry is: {0}.",
                            this.translatedSids2.Value.Sids[0].Flags.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R370
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            (uint)this.translatedSids2.Value.Sids[0].Flags == LsatUtilities.FlagValueZero,
                            "MS-LSAT",
                            370,
                            @"In LsarLookupNames2 interface: If a match cannot be found for a name, Flags field of 
                            TranslatedSids entry MUST be updated with 0x00000000.");

                        #endregion

                        #region MS-LSAT_R371

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R371, the actual value of the DomainIndex field 
                            of TranslatedSids entry is: {0}.",
                            this.translatedSids2.Value.Sids[0].DomainIndex.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R371
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            this.translatedSids2.Value.Sids[0].DomainIndex == -1,
                            "MS-LSAT",
                            371,
                            @"In LsarLookupNames2 interface: If a match cannot be found for a name, Flags field of 
                            TranslatedSids entry MUST be updated with 0x00000000.");

                        #endregion

                        break;

                    #endregion

                    #region default

                    default:
                        break;

                    #endregion
                }
            }
        }

        #endregion

        #region Verify requirements related with TranslatedSids structure in LsarLookupNames2 interface

        /// <summary>
        /// Verify requirements related with TranslatedSids structure in LsarLookupNames2 interface.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupNames2TranslatedSids(LookUpLevel levelOfLookup)
        {
            if ((this.methodStatus == LsatUtilities.StatusInvalidParameter
                    || this.methodStatus == LsatUtilities.StatusAccessDenied)
                    && levelOfLookup == LookUpLevel.LookUpWKSTA)
            {
                #region MS-LSAT-R376

                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-LSAT_R376, the ReferencedDomains is {0} null,
                    and the Sids field in the TranslatedSids structure is {1} null.",
                    this.referencedDomains == null ? string.Empty : "not",
                    this.translatedSids2.Value.Sids == null ? string.Empty : "not");

                // Verify MS-LSAT requirement: MS-LSAT_R376
                LsatTestSite.CaptureRequirementIfIsTrue(
                    this.referencedDomains == null && this.translatedSids2.Value.Sids == null,
                    "MS-LSAT",
                    376,
                    @"In LsarLookupNames2 interface: If LookupLevel is LsapLookupWksta, and the return code can be 
                    identified as an error value (that is, less than 0) other than STATUS_NONE_MAPPED, 
                    ReferencedDomains and the Sids field in the TranslatedSids structure MUST NOT be returned.");

                #endregion
            }
        }

        #endregion

        #region Verify requirements related with LsarLookupNames2 Message

        /// <summary>
        /// Verify requirements related with LsarLookupNames2 Message.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupNames2Message(LookUpLevel levelOfLookup)
        {
            if (!this.isDomainController)
            {
                if (levelOfLookup == LookUpLevel.Invalid)
                {
                    #region MS-LSAT_R377

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R377, the actual return value of the LsarLookupNames2 is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R377
                    LsatTestSite.CaptureRequirementIfAreEqual(
                        LsatUtilities.StatusInvalidParameter,
                        this.methodStatus,
                        "MS-LSAT",
                        377,
                        @"In LSAT Remote Protocol the LsarLookupNames2 message MUST be valid 
                        on non-domain controller machines.");

                    #endregion
                }
            }
            else
            {
                if (levelOfLookup == LookUpLevel.Invalid)
                {
                    #region MS-LSAT_R630

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R630, the actual return value of the LsarLookupNames2 is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R630
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusInvalidParameter,
                        this.methodStatus,
                        "MS-LSAT",
                        630,
                        @"In LSAT Remote Protocol the LsarLookupNames2 message MUST be valid 
                        on domain controllers machines.");

                    #endregion
                }
            }
        }

        #endregion

        #region Verify requirements related with Names parameter in LsarLookupNames interface

        /// <summary>
        /// Verify requirements related with Names parameter in LsarLookupNames interface.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupNamesNames(LookUpLevel levelOfLookup)
        {
            if (this.count > 0
                    && this.desiredAccess == PolicyLookupNames
                    && levelOfLookup != LookUpLevel.Invalid)
            {
                switch (LsatUtilities.NamesValidity)
                {
                    #region Invalid

                    case LsatUtilities.Invalid:

                        #region MS-LSAT_R402

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R402, the actual return value of the LsarLookupNames is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R402
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusInvalidParameter,
                            this.methodStatus,
                            "MS-LSAT",
                            402,
                            @"In LsarLookupNames interface: If validation fails while checking  each element in the Names
                            parameter by the RPC server, then it  MUST return LsatUtilities.STATUS_INVALID_PARAMETER.");

                        #endregion

                        break;

                    #endregion

                    #region Success

                    case LsatUtilities.Success:

                        if (this.policyHandle != IntPtr.Zero)
                        {
                            #region MS-LSAT_R171

                            // Add the debug information
                            Site.Log.Add(
                                LogEntryKind.Debug,
                                @"Verify MS-LSAT_R171, the actual return value of the LsarOpenPolicy is: 0x{0:X8}.",
                                this.methodStatus);

                            // Verify MS-LSAT requirement: MS-LSAT_R171
                            LsatTestSite.CaptureRequirementIfAreNotEqual<uint>(
                                LsatUtilities.StatusAccessDenied,
                                this.methodStatus,
                                "MS-LSAT",
                                171,
                                @"In 'DesiredAccess' parameter of LsarOpenPolicy interface of  LSAT Remote Protocol: 
                                An ACCESS_MASK value that specifies the requested access rights that MUST be granted 
                                on the returned PolicyHandle, if the request is successful.");

                            #endregion

                            #region MS-LSAT_R180

                            // Add the debug information
                            Site.Log.Add(
                                LogEntryKind.Debug,
                                @"Verify MS-LSAT_R180, the actual return value of the LsarOpenPolicy is: 0x{0:X8}.",
                                this.methodStatus);

                            // Verify MS-LSAT requirement: MS-LSAT_R180
                            LsatTestSite.CaptureRequirementIfAreNotEqual<uint>(
                                LsatUtilities.StatusAccessDenied,
                                this.methodStatus,
                                "MS-LSAT",
                                180,
                                @"In LsarOpenPolicy interface of  LSAT Remote Protocol: The context created by the 
                                implementation that is referenced by PolicyHandle on return MUST contain the access 
                                granted by the server implementation as a result of validating the DesiredAccess.");

                            #endregion
                        }

                        #region MS-LSAT_R387

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R387, the actual return value of the LsarLookupNames is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R387
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            387,
                            @"In Names parameter of the LsarLookupNames interface of  LSAT Remote Protocol: server MUST
                            support User principal names like  user_name@example.example.com.");

                        #endregion

                        #region MS-LSAT_R388

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R388, the actual return value of the LsarLookupNames is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R388
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            388,
                            @"In 'Names' parameter of the LsarLookupNames interface of  LSAT Remote Protocol: server 
                            MUST support Fully qualified account names based on either DNS  or NetBIOS names like 
                            example.example.com\\user_name or example\\user_name.");

                        #endregion

                        #region MS-LSAT_R389

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R389, the actual return value of the LsarLookupNames is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R389
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            389,
                            @"In 'Names' parameter of the LsarLookupNames interface:  Server MUST support Unqualified 
                            or isolated names like user_name.");

                        #endregion

                        #region MS-LSAT_R390

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R390, the actual return value of the LsarLookupNames is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R390
                        LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                            LsatUtilities.StatusSuccess,
                            this.methodStatus,
                            "MS-LSAT",
                            390,
                            @"In LsarLookupNames, the Names parameter MUST NOT be case sensitive.");

                        #endregion

                        #region MS-LSAT_R401

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R401, the actual return value of the LsarLookupNames is: 0x{0:X8}.",
                            this.methodStatus);

                        // Verify MS-LSAT requirement: MS-LSAT_R401
                        LsatTestSite.CaptureRequirementIfAreNotEqual<uint>(
                            LsatUtilities.StatusInvalidParameter,
                            this.methodStatus,
                            "MS-LSAT",
                            401,
                            @"In LsarLookupNames interface:  The RPC server MUST check each element in the 
                            Names parameter for validity, as described for the RPC_UNICODE_STRING structure 
                            in [MS-DTYP] section 2.3.9.");

                        #endregion

                        #region MS-LSAT_R424

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R424, the actual value of the DomainIndex field 
                            of TranslatedSids entry is: {0}.",
                            this.translatedSids1.Value.Sids[0].DomainIndex.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R424
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            this.translatedSids1.Value.Sids[0].DomainIndex >= 0,
                            "MS-LSAT",
                            424,
                            @"In LsarLookupNames interface: For a successful search, DomainIndex field of TranslatedSids 
                            entry MUST be updated with Index of the domain in the ReferencedDomains parameter.");

                        #endregion

                        #region MS-LSAT_R37

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R37, the actual value of the DomainIndex field 
                            of LSAPR_TRANSLATED_SID structure is: {0},
                            and the Domains field of LSAPR_REFERENCED_DOMAIN_LIST structure is {1} null.",
                            this.translatedSids1.Value.Sids[0].DomainIndex,
                            this.referencedDomains.Value.Domains == null ? string.Empty : "not");

                        // Verify MS-LSAT requirement: MS-LSAT_R37
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            this.translatedSids1.Value.Sids[0].DomainIndex >= 0 
                                && this.referencedDomains.Value.Domains != null,
                            "MS-LSAT",
                            37,
                            @"The LSA_TRANSLATED_SID structure contains information about a security principal after 
                            translation from a name to a SID. This structure MUST always be accompanied by an 
                            LSAPR_REFERENCED_DOMAIN_LIST structure when DomainIndex is not -1.");

                        #endregion

                        break;

                    #endregion

                    #region None_Mapped

                    case LsatUtilities.NoneMapped:

                        #region MS-LSAT_R39

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R39, the actual value of the DomainIndex field 
                            of LSA_TRANSLATED_SID structure is: {0}.",
                            this.translatedSids1.Value.Sids[0].DomainIndex.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R39
                        LsatTestSite.CaptureRequirementIfIsFalse(
                            this.translatedSids1.Value.Sids[0].DomainIndex < -1,
                            "MS-LSAT",
                            39,
                            @"In 'DomainIndex' field of the structure LSA_TRANSLATED_SID of LSAT Remote Protocol: 
                            Contains the index of the domain in which the security principal is in. 
                            No negative values MUST be returned by this structure except -1.");

                        #endregion

                        #region MS-LSAT_R38

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R38, the actual value of the DomainIndex field 
                            of LSA_TRANSLATED_SID structure is: {0},
                            and the Domains field of LSAPR_REFERENCED_DOMAIN_LIST structure is {1} null.",
                            this.translatedSids1.Value.Sids[0].DomainIndex,
                            this.referencedDomains.Value.Domains == null ? string.Empty : "not");

                        // Verify MS-LSAT requirement: MS-LSAT_R38
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            this.translatedSids1.Value.Sids[0].DomainIndex == -1 
                                && this.referencedDomains.Value.Domains == null,
                            "MS-LSAT",
                            38,
                            @"In 'DomainIndex' field of the structure LSA_TRANSLATED_SID of LSAT Remote Protocol: 
                            Contains the index of the domain in which the security principal is in. 
                            The value of -1 MUST be used to specify that there are no corresponding domains. ");

                        #endregion

                        #region MS-LSAT_R425

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R425, the actual value of the Use field of TranslatedSids entry is: {0}.",
                            this.translatedSids1.Value.Sids[0].Use.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R425
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            this.translatedSids1.Value.Sids[0].Use == _SID_NAME_USE.SidTypeUnknown,
                            "MS-LSAT",
                            425,
                            @"In LsarLookupNames interface: If a match cannot be found for a name, Use field of 
                            TranslatedSids entry MUST be updated with SidTypeUnknown.");

                        #endregion

                        #region MS-LSAT_R426

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R426, the actual value of the RelativeID field 
                            of TranslatedSids entry is: {0}.",
                            this.translatedSids1.Value.Sids[0].RelativeId.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R426
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            this.translatedSids1.Value.Sids[0].RelativeId == 0,
                            "MS-LSAT",
                            426,
                            @"In LsarLookupNames interface: If a match cannot be found for a name, Sid field of 
                            TranslatedSids entry MUST be updated with NULL.");

                        #endregion

                        #region MS-LSAT_R428

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R428, the actual value of the DomainIndex field 
                            of TranslatedSids entry is: {0}.",
                            this.translatedSids1.Value.Sids[0].DomainIndex.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R428
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            this.translatedSids1.Value.Sids[0].DomainIndex == -1,
                            "MS-LSAT",
                            428,
                            @"In LsarLookupNames interface: If a match cannot be found for a name, DomainIndex field of 
                            TranslatedSids entry MUST be updated with -1.");

                        #endregion

                        break;

                    #endregion

                    #region default

                    default:

                        break;

                    #endregion
                }
            }
        }

        #endregion

        #region Verify requirements related with TranslatedSids structure in LsarLookupNames interface

        /// <summary>
        /// Verify requirements related with TranslatedSids structure in LsarLookupNames interface.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupNamesTranslatedSids(LookUpLevel levelOfLookup)
        {
            if ((this.methodStatus == LsatUtilities.StatusInvalidParameter
                    || this.methodStatus == LsatUtilities.StatusAccessDenied)
                    && levelOfLookup == LookUpLevel.LookUpWKSTA)
            {
                #region MS-LSAT-R433

                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-LSAT_R433, the ReferencedDomains is {0} null,
                    and the Sids field of TranslatedSids structure is {1} null.",
                    this.referencedDomains == null ? string.Empty : "not",
                    this.translatedSids1.Value.Sids == null ? string.Empty : "not");

                // Verify MS-LSAT requirement: MS-LSAT_R433
                LsatTestSite.CaptureRequirementIfIsTrue(
                    this.referencedDomains == null && this.translatedSids1.Value.Sids == null,
                    "MS-LSAT",
                    433,
                    @"In LsarLookupNames interface:If LookupLevel is LsapLookupWksta, and the return code can be 
                    identified as an error value (that is, less than 0) other than STATUS_NONE_MAPPED, 
                    ReferencedDomains and the Sids field in the TranslatedSids structure MUST NOT be returned.");

                #endregion
            }
        }

        #endregion

        #region Verify requirements related with LsarLookupNames Message

        /// <summary>
        /// Verify requirements related with LsarLookupNames Message.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookUpNamesMessage(LookUpLevel levelOfLookup)
        {
            if (!this.isDomainController)
            {
                if (levelOfLookup == LookUpLevel.Invalid)
                {
                    #region MS-LSAT_R434

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R434, the actual return value of the LsarLookupNames is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R434
                    LsatTestSite.CaptureRequirementIfAreEqual(
                        LsatUtilities.StatusInvalidParameter,
                        this.methodStatus,
                        "MS-LSAT",
                        434,
                        @"In LSAT Remote Protocol the LsarLookupNames message MUST be valid 
                        on non-domain controller machines.");

                    #endregion
                }
            }
            else
            {
                if (levelOfLookup == LookUpLevel.Invalid)
                {
                    #region MS-LSAT_R631

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R631, the actual return value of the LsarLookupNames is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R631
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusInvalidParameter,
                        this.methodStatus,
                        "MS-LSAT",
                        631,
                        @"In LSAT Remote Protocol the LsarLookupNames message MUST be valid 
                        on domain controllers machines.");

                    #endregion
                }
            }
        }

        #endregion

        #region Verify requirements related with parameters in LsarLookupSids2 interface

        /// <summary>
        /// Verify requirements related with parameters in LsarLookupSids2 interface.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupSids2Parameters(LookUpLevel levelOfLookup)
        {
            if (this.policyHandle != IntPtr.Zero && levelOfLookup != LookUpLevel.Invalid)
            {
                if (LsatUtilities.SidsValidity != LsatUtilities.Invalid
                        && this.desiredAccess == PolicyLookupNames)
                {
                    if (this.countOfSids == MappedCounts
                            && levelOfLookup != LookUpLevel.Invalid)
                    {
                        if (this.countOfSids > 0)
                        {
                            #region MS-LSAT_R504

                            // Add the debug information
                            Site.Log.Add(
                                LogEntryKind.Debug,
                                @"Verify MS-LSAT_R504, the Sid field of LSAPR_REFERENCED_DOMAIN_LIST structure 
                                is {0} null.",
                                this.referencedDomains.Value.Domains[0].Sid == null ? string.Empty : "not");

                            // Verify MS-LSAT requirement: MS-LSAT_R504
                            LsatTestSite.CaptureRequirementIfIsTrue(
                                this.referencedDomains.Value.Domains[0].Sid != null,
                                "MS-LSAT",
                                504,
                                @"In 'ReferencedDomains' parameter of The LsarLookupSids2 interface of  LSAT Remote 
                                Protocol: contains the domain information for the domain to which 
                                each security principals belongs.");

                            #endregion

                            #region MS-LSAT_R505

                            // Add the debug information
                            Site.Log.Add(
                                LogEntryKind.Debug,
                                @"Verify MS-LSAT_R505, the actual return value of the LsarLookupSids2 is: 0x{0:X8}.",
                                this.methodStatus);

                            // Verify MS-LSAT requirement: MS-LSAT_R505
                            LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                                LsatUtilities.StatusSuccess,
                                this.methodStatus,
                                "MS-LSAT",
                                505,
                                @"In 'TranslatedNames' parameter of The LsarLookupSids2 interface of  LSAT Remote 
                                Protocol: contains the corresponding name form for security principal SIDs 
                                in the SidEnumBuffer parameter.");

                            #endregion

                            #region MS-LSAT_R549

                            // Add the debug information
                            Site.Log.Add(
                                LogEntryKind.Debug,
                                @"Verify MS-LSAT_R549, the actual value of the DomainIndex field 
                                of TranslatedNames entry is: {0}.",
                                TranslatedNames2.Value.Names[0].DomainIndex.ToString());

                            // Verify MS-LSAT requirement: MS-LSAT_R549
                            LsatTestSite.CaptureRequirementIfIsTrue(
                                TranslatedNames2.Value.Names[0].DomainIndex >= 0,
                                "MS-LSAT",
                                549,
                                @"In LsarLookupSids2 interface of  LSAT Remote Protocol: For a successful search, 
                                DomainIndex field of TranslatedNames entry MUST be updated with Index of the domain 
                                in the ReferencedDomains parameter.");

                            #endregion

                            #region MS-LSAT_R54

                            // Add the debug information
                            Site.Log.Add(
                                LogEntryKind.Debug,
                                @"Verify MS-LSAT_R54, the actual value of the DomainIndex field 
                                of TLSAPR_TRANSLATED_NAME_EX structure is: {0},
                                and the Domains field of LSAPR_REFERENCED_DOMAIN_LIST structure is {1} null.",
                                TranslatedNames2.Value.Names[0].DomainIndex,
                                this.referencedDomains.Value.Domains == null ? string.Empty : "not");

                            // Verify MS-LSAT requirement: MS-LSAT_R54
                            LsatTestSite.CaptureRequirementIfIsTrue(
                                TranslatedNames2.Value.Names[0].DomainIndex >= 0 
                                    && this.referencedDomains.Value.Domains != null,
                                "MS-LSAT",
                                54,
                                @"The LSAPR_TRANSLATED_NAME_EX structure contains information about a security principal, 
                                along with the human-readable identifier for that security principal. This structure 
                                when DomainIndex is not -1, which contains the domain information 
                                for the security principals.");

                            #endregion

                            #region MS-LSAT_R561

                            // Add the debug information
                            Site.Log.Add(
                                LogEntryKind.Debug,
                                @"Verify MS-LSAT_R561, the actual return value of translating all SIDs is: {0}.",
                                LsatUtilities.AreTranslatedNamesEqual(LsatUtilities.LookupSidS2).ToString());

                            // Verify MS-LSAT requirement: MS-LSAT_R561
                            LsatTestSite.CaptureRequirementIfIsTrue(
                                LsatUtilities.AreTranslatedNamesEqual(LsatUtilities.LookupSidS2),
                                "MS-LSAT",
                                561,
                                @"In LsarLookupSids2 interface of  LSAT Remote Protocol: If all SIDs are translated 
                                correctly then the return value MUST be set to STATUS_SUCCESS.");

                            #endregion
                        }
                    }
                    else if (MappedCounts == 0
                                 && this.countOfSids > 0
                                 && levelOfLookup != LookUpLevel.Invalid)
                    {
                        #region MS-LSAT_56

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R56, the actual value of the DomainIndex field 
                            of LSAPR_TRANSLATED_NAME_EX structure is: {0}.",
                            TranslatedNames2.Value.Names[0].DomainIndex.ToString());

                        // Verify MS-LSAT requirement: MS-LSAT_R56
                        LsatTestSite.CaptureRequirementIfIsFalse(
                            TranslatedNames2.Value.Names[0].DomainIndex < -1,
                            "MS-LSAT",
                            56,
                            @"In 'DomainIndex' field of the structure LSAPR_TRANSLATED_NAME_EX  of LSAT Remote Protocol:
                            Contains the index of the domain in which the security principal is in. No negative values 
                            must be used by this structure except -1.");

                        #endregion

                        #region MS-LSAT_55

                        // Add the debug information
                        Site.Log.Add(
                            LogEntryKind.Debug,
                            @"Verify MS-LSAT_R55, the actual value of the DomainIndex field 
                            of LSAPR_TRANSLATED_NAME_EX structure is: {0},
                            and the Domains field of LSAPR_REFERENCED_DOMAIN_LIST structure is {1} null.",
                            TranslatedNames2.Value.Names[0].DomainIndex.ToString(),
                            this.referencedDomains.Value.Domains == null ? string.Empty : "not");

                        // Verify MS-LSAT requirement: MS-LSAT_R55
                        LsatTestSite.CaptureRequirementIfIsTrue(
                            TranslatedNames2.Value.Names[0].DomainIndex == -1 
                                && this.referencedDomains.Value.Domains == null,
                            "MS-LSAT",
                            55,
                            @"In 'DomainIndex' field of the structure LSAPR_TRANSLATED_NAME_EX  of LSAT Remote Protocol: 
                            Contains the index of the domain in which the security principal is in. This value of -1 
                            MUST be used to specify that there are no corresponding domains.");

                        #endregion
                    }
                }
            }
        }

        #endregion

        #region Verify requirements related with TranslatedNames structure in LsarLookupSids2 interface

        /// <summary>
        /// Verify requirements related with TranslatedNames structure in LsarLookupSids2 interface.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupSids2TranslatedNames(LookUpLevel levelOfLookup)
        {
            if ((this.methodStatus == LsatUtilities.StatusInvalidParameter
                    || this.methodStatus == LsatUtilities.StatusAccessDenied)
                    && levelOfLookup == LookUpLevel.LookUpWKSTA)
            {
                #region MS-LSAT-R564

                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-LSAT_R564, the referencedDomains is {0} null,
                    and the Names field of LSAPR_TRANSLATED_NAMES_EX structure is {0} null.",
                    this.referencedDomains == null ? string.Empty : "not",
                    TranslatedNames2.Value.Names == null ? string.Empty : "not");

                // Verify MS-LSAT requirement: MS-LSAT_R564
                LsatTestSite.CaptureRequirementIfIsTrue(
                    this.referencedDomains == null && TranslatedNames2.Value.Names == null,
                    "MS-LSAT",
                    564,
                    @"In LsarLookupSids2 interface of  LSAT Remote Protocol: If LookupLevel is LsapLookupWksta, 
                    and the return code can be identified as an error value other than STATUS_NONE_MAPPED, 
                    ReferencedDomains and the Names fields in the TranslatedNames structure MUST NOT be returned.");

                #endregion
            }
        }
      
        #endregion

        #region Verify requirements related with LsarLookupSids2 Message

        /// <summary>
        /// Verify requirements related with LsarLookupSids2 Message.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupSids2Message(LookUpLevel levelOfLookup)
        {
            if (!this.isDomainController)
            {
                if (levelOfLookup != LookUpLevel.LookUpWKSTA)
                {
                    #region MS-LSAT_R521

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R521, the actual return value of the LsarLookupSids2 is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R521
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusInvalidParameter,
                        this.methodStatus,
                        "MS-LSAT",
                        521,
                        @"In LsarLookupSids2 interface of  LSAT Remote Protocol: LookupLevel values other than 
                        LsapLookupWksta are not valid if the RPC server is not a domain controller.");

                    #endregion
                }

                if (levelOfLookup == LookUpLevel.Invalid)
                {
                    #region MS-LSAT_R565

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R565, the actual return value of the LsarLookupSids2 is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R565
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusInvalidParameter,
                        this.methodStatus,
                        "MS-LSAT",
                        565,
                        @"In LsarLookupSids2 interface of  LSAT Remote Protocol: This message is valid 
                        on non-domain controller machines");

                    #endregion
                }
            }
            else
            {
                if (levelOfLookup == LookUpLevel.Invalid)
                {
                    #region MS-LSAT_R632

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R632, the actual return value of the LsarLookupSids2 is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R632
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusInvalidParameter,
                        this.methodStatus,
                        "MS-LSAT",
                        632,
                        @"In LsarLookupSids2 interface of  LSAT Remote Protocol: This message is valid 
                        on domain controller machines");

                    #endregion
                }
            }
        }

        #endregion

        #region Verify requirements related with parameters in LsarLookupSids interface

        /// <summary>
        /// Verify requirements related with parameters in LsarLookupSids interface.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupSidsParameters(LookUpLevel levelOfLookup)
        {
            if (LsatUtilities.SidsValidity != LsatUtilities.Invalid
                    && this.countOfSids == MappedCounts
                    && this.desiredAccess == PolicyLookupNames
                    && levelOfLookup != LookUpLevel.Invalid)
            {
                if (this.countOfSids > 0)
                {
                    #region MS-LSAT_R567

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R567, the Domains field of LSAPR_REFERENCED_DOMAIN_LIST structure 
                        is {0} null.",
                        this.referencedDomains.Value.Domains == null ? string.Empty : "not");

                    // Verify MS-LSAT requirement: MS-LSAT_R567
                    LsatTestSite.CaptureRequirementIfIsTrue(
                        this.referencedDomains.Value.Domains != null,
                        "MS-LSAT",
                        567,
                        @"In 'ReferencedDomains' parameter of The LsarLookupSids interface of  LSAT Remote Protocol:  
                        contains the domain information for the domain to which each security principals belongs.");

                    #endregion

                    #region MS-LSAT_R568

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R568, the Buffer field of LSAPR_TRANSLATED_NAMES structure is {0} null.",
                        TranslatedNames1.Value.Names[0].Name.Buffer == null ? string.Empty : "not");

                    // Verify MS-LSAT requirement: MS-LSAT_R568
                    LsatTestSite.CaptureRequirementIfIsTrue(
                        TranslatedNames1.Value.Names[0].Name.Buffer != null,
                        "MS-LSAT",
                        568,
                        @"In 'TranslatedNames' parameter of The LsarLookupSids interface of  LSAT Remote Protocol:  
                        contains the corresponding name form for security principal SIDs 
                        in the SidEnumBuffer parameter.");

                    #endregion

                    #region MS-LSAT_R608

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R608, the actual value of the DomainIndex field of TranslateNames entry 
                        is: {0}.",
                        TranslatedNames1.Value.Names[0].DomainIndex.ToString());

                    // Verify MS-LSAT requirement: MS-LSAT_R608
                    LsatTestSite.CaptureRequirementIfIsTrue(
                        TranslatedNames1.Value.Names[0].DomainIndex >= 0,
                        "MS-LSAT",
                        608,
                        @"In LsarLookupSids interface of  LSAT Remote Protocol: For a successful search, DomainIndex 
                        field of TranslatedNames entry MUST be updated with Index of the domain 
                        in the ReferencedDomains parameter.");

                    #endregion

                    #region MS-LSAT_R48

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R48, the actual value of the DomainIndex field of TranslateNames entry is: {0},
                        and the Domains field of LSAPR_REFERENCED_DOMAIN_LIST strucure is {1} null.",
                        TranslatedNames1.Value.Names[0].DomainIndex.ToString(),
                        this.referencedDomains.Value.Domains == null ? string.Empty : "not");

                    // Verify MS-LSAT requirement: MS-LSAT_R48
                    LsatTestSite.CaptureRequirementIfIsTrue(
                        TranslatedNames1.Value.Names[0].DomainIndex >= 0 && this.referencedDomains.Value.Domains != null,
                        "MS-LSAT",
                        48,
                        @"The LSAPR_TRANSLATED_NAME structure contains information about a security principal, 
                        along with the human-readable identifier for that security principal. This structure 
                        when DomainIndex is not -1, which contains the domain information for the security principals.");

                    #endregion

                    #region MS-LSAT_R620

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R620, the actual return value of translating SIDs is: {0}.",
                        LsatUtilities.AreTranslatedNamesEqual(LsatUtilities.LookupSidS1).ToString());

                    // Verify MS-LSAT requirement: MS-LSAT_R620
                    LsatTestSite.CaptureRequirementIfIsTrue(
                        LsatUtilities.AreTranslatedNamesEqual(LsatUtilities.LookupSidS1),
                        "MS-LSAT",
                        620,
                        @"In LsarLookupSids interface of  LSAT Remote Protocol: If all SIDs are translated correctly 
                        then the return value MUST be set to STATUS_SUCCESS.");

                    #endregion
                }
            }
            else if (LsatUtilities.SidsValidity != LsatUtilities.Invalid
                         && this.desiredAccess == PolicyLookupNames
                         && MappedCounts == 0
                         && this.countOfSids > 0
                         && levelOfLookup != LookUpLevel.Invalid)
            {
                #region MS-LSAT_R50

                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-LSAT_R50, the actual value of the DomainIndex field 
                    of LSAPR_TRANSLATED_NAME structrue is: {0}.",
                    TranslatedNames1.Value.Names[0].DomainIndex.ToString());

                // Verify MS-LSAT requirement: MS-LSAT_R50
                LsatTestSite.CaptureRequirementIfIsFalse(
                    TranslatedNames1.Value.Names[0].DomainIndex < -1,
                    "MS-LSAT",
                    50,
                    @"In 'DomainIndex' field of the structure LSAPR_TRANSLATED_NAME of LSAT Remote Protocol: No negative 
                    values MUST be used by this structure except -1.");

                #endregion

                #region MS-LSAT_R49

                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-LSAT_R49, the actual value of the DomainIndex field 
                    of LSAPR_TRANSLATED_NAME structure is: {0},
                    and the Domains field of LSAPR_REFERENCED_DOMAIN_LIST strucure is {1} null.",
                    TranslatedNames1.Value.Names[0].DomainIndex.ToString(),
                    this.referencedDomains.Value.Domains == null ? string.Empty : "not");

                // Verify MS-LSAT requirement: MS-LSAT_R49
                LsatTestSite.CaptureRequirementIfIsTrue(
                    TranslatedNames1.Value.Names[0].DomainIndex == -1 && this.referencedDomains.Value.Domains == null,
                    "MS-LSAT",
                    49,
                    @"In 'DomainIndex' field of the structure LSAPR_TRANSLATED_NAME of LSAT Remote Protocol: Contains the 
                    index of the domain in which the security principal is in. The value of -1 MUST be used to specify 
                    that there are no corresponding domains.");

                #endregion
            }
        }

        #endregion

        #region Verify requirements related with TranslatedNames structure in LsarLookupSids interface

        /// <summary>
        /// Verify requirements related with TranslatedNames structure in LsarLookupSids interface.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupSidsTranslatedNames(LookUpLevel levelOfLookup)
        {
            if ((this.methodStatus == LsatUtilities.StatusInvalidParameter
                    || this.methodStatus == LsatUtilities.StatusAccessDenied)
                    && levelOfLookup == LookUpLevel.LookUpWKSTA)
            {
                #region MS-LSAT-R623

                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-LSAT_R623, the ReferencedDomains is {0} null,
                    and the Names field in the TranslatedNames structure is {1} null.",
                    this.referencedDomains == null ? string.Empty : "not",
                    TranslatedNames1.Value.Names == null ? string.Empty : "not");

                // Verify MS-LSAT requirement: MS-LSAT_R623
                LsatTestSite.CaptureRequirementIfIsTrue(
                    this.referencedDomains == null && TranslatedNames1.Value.Names == null,
                    "MS-LSAT",
                    623,
                    @"In LsarLookupSids interface of  LSAT Remote Protocol: If LookupLevel is LsapLookupWksta, and the 
                    return code can be identified as an error value other than STATUS_NONE_MAPPED, ReferencedDomains and 
                    the Names fields in the TranslatedNames structure MUST NOT be returned.");

                #endregion
            }
        }

        #endregion

        #region Verify requirements related with LsarLookupSids Message

        /// <summary>
        /// Verify requirements related with LsarLookupSids Message.
        /// </summary>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        private void VerifyLsarLookupSidsMessage(LookUpLevel levelOfLookup)
        {
            if (!this.isDomainController)
            {
                if (levelOfLookup != LookUpLevel.LookUpWKSTA)
                {
                    #region MS-LSAT_R580

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R580, the actual return value of the LsarLookupSids is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R580
                    LsatTestSite.CaptureRequirementIfAreNotEqual<uint>(
                        LsatUtilities.StatusSuccess,
                        this.methodStatus,
                        "MS-LSAT",
                        580,
                        @"In LsarLookupSids interface of  LSAT Remote Protocol: LookupLevel values other than 
                        LsapLookupWksta are not valid if the RPC server is not a domain controller.");

                    #endregion
                }

                if (levelOfLookup == LookUpLevel.Invalid)
                {
                    #region MS-LSAT_R624

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R624, the actual return value of the LsarLookupSids is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R624
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusInvalidParameter,
                        this.methodStatus,
                        "MS-LSAT",
                        624,
                        @"In LsarLookupSids interface of  LSAT Remote Protocol: This message is valid 
                        on non-domain controller machines.");

                    #endregion
                }
            }
            else
            {
                if (levelOfLookup == LookUpLevel.Invalid)
                {
                    #region MS-LSAT_R633

                    // Add the debug information
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        @"Verify MS-LSAT_R633, the actual return value of the LsarLookupSids is: 0x{0:X8}.",
                        this.methodStatus);

                    // Verify MS-LSAT requirement: MS-LSAT_R633
                    LsatTestSite.CaptureRequirementIfAreEqual<uint>(
                        LsatUtilities.StatusInvalidParameter,
                        this.methodStatus,
                        "MS-LSAT",
                        633,
                        @"In LsarLookupSids interface of  LSAT Remote Protocol: This message is valid 
                        on domain controller machines.");

                    #endregion
                }
            }
        }

        #endregion
    }
}