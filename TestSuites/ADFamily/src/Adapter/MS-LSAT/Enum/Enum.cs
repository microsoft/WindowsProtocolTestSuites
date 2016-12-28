// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat
{
    /// <summary>
    /// This enumeration defines the possible values for the handle returned from the actions 
    /// OpenPolicy2 or OpenPolicy and Close.
    /// </summary>
    public enum Handle
    {
        /// <summary>
        /// Valid handle.
        /// </summary>
        Valid,

        /// <summary>
        /// Invalid handle.
        /// </summary>
        Invalid,

        /// <summary>
        /// The handle is null.
        /// </summary>
        Null
    }

    /// <summary>
    /// This enumeration defines the possible error states returned from the actions.
    /// </summary>
    public enum ErrorStatus
    {
        /// <summary>
        /// Access was denied.
        /// </summary>
        AccessDenied,

        /// <summary>
        /// Invalid parameter.
        /// </summary>
        InvalidParameter,

        /// <summary>
        /// Invalid handle.
        /// </summary>
        InvalidHandle,

        /// <summary>
        /// Some not mapped.
        /// </summary>
        SomeNotMapped,

        /// <summary>
        /// None mapped.
        /// </summary>
        NoneMapped,

        /// <summary>
        /// Invalid server state.
        /// </summary>
        InvalidServerState,

        /// <summary>
        /// Success state.
        /// </summary>
        Success,

        /// <summary>
        /// Unknown state.
        /// </summary>
        Unknown
    }

    /// <summary>
    /// This enumeration defines the possible values of root directory field passed in 
    /// OpenPolicy2 or OpenPolicy methods.
    /// </summary>
    public enum RootDirectory
    {
        /// <summary>
        /// The root directory field is null.
        /// </summary>
        Null,

        /// <summary>
        /// The root directory field is non-null.
        /// </summary>
        NonNull
    }

    /// <summary>
    /// This enumeration defines the requesters anonymous access settings.
    /// </summary>
    public enum AnonymousAccess
    {
        /// <summary>
        /// Disable the requesters anonymous access.
        /// </summary>
        Disable,

        /// <summary>
        /// Enable the requesters anonymous access.
        /// </summary>
        Enable
    }

    /// <summary>
    /// This enumeration defines whether the server is domain controller or non domain controller.
    /// </summary>
    public enum ProtocolServerConfig
    {
        /// <summary>
        /// The server is domain controller.
        /// </summary>
        DomainController,

        /// <summary>
        /// The server is non domain controller.
        /// </summary>
        NonDomainController
    }

    /// <summary>
    /// This enumeration defines the possible values for user name returned from the action GetUserName.
    /// </summary>
    public enum Name
    {
        /// <summary>
        /// Valid user name.
        /// </summary>
        valid,

        /// <summary>
        /// Invalid user name.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines the possible values for lookup levels.
    /// </summary>
    public enum LookUpLevel
    {
        /// <summary>
        /// None lookup.
        /// </summary>
        None,

        /// <summary>
        /// Look up WKSTA.
        /// </summary>
        LookUpWKSTA = 1,

        /// <summary>
        /// Look up PDC.
        /// </summary>
        LookUpPDC,

        /// <summary>
        /// Look up TDL.
        /// </summary>
        LookUpTDL,

        /// <summary>
        /// Look up GC.
        /// </summary>
        LookUpGC,

        /// <summary>
        /// Look up forest referral.
        /// </summary>
        LookUpXForestReferral,

        /// <summary>
        /// Look up forest resolve.
        /// </summary>
        LookUpXForestResolve,

        /// <summary>
        /// Look up RODCReferralToFullDC.
        /// </summary>
        LookUpRODCReferralToFullDC,

        /// <summary>
        /// Invalid level.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines the possible values for lookup options.
    /// </summary>
    public enum LookUpOption
    {
        /// <summary>
        /// The value of lookup options is MSBSet.
        /// </summary>
        MSBSet,

        /// <summary>
        /// The value of lookup options is MSBNotSet.
        /// </summary>
        MSBNotSet
    }

    /// <summary>
    /// This enumeration defines the possible values for translated sid returned from LookupNames methods.
    /// </summary>
    public enum TranslatedSid
    {
        /// <summary>
        /// Valid translated sid.
        /// </summary>
        Valid,

        /// <summary>
        /// Invalid translated sid.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines the possible values for translated name returned from LookupSids methods.
    /// </summary>
    public enum TranstlatedNames
    {
        /// <summary>
        /// Valid translated name.
        /// </summary>
        Valid,

        /// <summary>
        /// Invalid translated name.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration is for validating the LSA handle passed in for actions WinLookupNames2 and LookupSids3.
    /// </summary>
    public enum LSAHandle
    {
        /// <summary>
        /// Valid LSA handle.
        /// </summary>
        Valid,

        /// <summary>
        /// Invalid LSA handle.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration defines the possible values for mapped count returned from the 
    /// LookupNames and LookupSids actions.
    /// </summary>
    public enum MappedCount
    {
        /// <summary>
        /// Valid mapped count.
        /// </summary>
        Valid,

        /// <summary>
        /// Invalid mapped count.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// This enumeration is for checking authentication of user in action GetUserName.
    /// </summary>
    public enum User
    {
        /// <summary>
        /// Authentication of user is authenticated.
        /// </summary>
        Authenticated,

        /// <summary>
        /// Authentication of user is not authenticated.
        /// </summary>
        NotAuthenticated
    }
}
