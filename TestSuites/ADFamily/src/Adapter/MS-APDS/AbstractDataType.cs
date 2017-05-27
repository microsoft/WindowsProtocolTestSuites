// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Apds
{
    #region Enum data types for request & response data structures

    /// <summary>
    /// It indicates whether the account and password are valid
    /// </summary>
    public enum AccountInformation
    {
        /// <summary>
        /// The account and password are correct
        /// </summary>
        Valid,
        /// <summary>
        /// The account sent in request is not found in the DC
        /// </summary>
        AccountNotExist,
        /// <summary>
        /// The password sent in request does not match the one on DC
        /// </summary>
        WrongPassword,
        /// <summary>
        /// The account is a managed service account
        /// </summary>
        ManagedServiceAccount
    }

    /// <summary>
    /// It indicates whether the resource DC is blocked
    /// </summary>
    public enum ResourceDCBlocked
    {
        /// <summary>
        /// ResourceDCBlocked variable is set to false
        /// </summary>
        NotBlocked,
        /// <summary>
        /// ResourceDCBlocked == TRUE, and the NTLM server's name is not equal to any of the DCBlockExceptions server names
        /// </summary>
        BlockedAndNoException,
        /// <summary>
        /// ResourceDCBlocked == TRUE, and the NTLM server's name is equal to one of the DCBlockExceptions server names
        /// </summary>
        BlockedAndIsException
    }

    /// <summary>
    /// It indicates the fields that should be ignored by DC in DIGEST_VALIDATION_REQ 
    /// </summary>
    public struct IgnoredFields
    {
        /// <summary>
        /// Reserved3 field
        /// </summary>
        public int reserved3;
        /// <summary>
        /// Reserved4 field
        /// </summary>
        public int reserved4;
        /// <summary>
        /// Initialize method
        /// </summary>
        public void Initialize()
        {

            reserved3 = 0;
            reserved4 = 0;
        }
    }

    /// <summary>
    /// Indicates the status codes  
    /// </summary>
    /// Disable warning CA1028 because System.Int32. cannot match the enumeration design accroding to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Status : uint
    {
        /// <summary>
        /// DC returns STATUS_SUCCESS
        /// </summary>
        Success = 0x00000000,
        /// <summary>
        /// DC returns STATUS_INVALID_INFO_CLASS
        /// </summary>
        InvalidInfo = 0xc0000003,
        /// <summary>
        /// DC returns STATUS_LOGON_FAILURE
        /// </summary>
        LogonFailure = 0xC000006D,
        /// <summary>
        /// DC returns error STATUS_NO_SUCH_USER
        /// </summary>
        NoSuchUser = 0xC0000064,
        /// <summary>
        /// DC returns STATUS_NTLM_BLOCKED mentioned in MS-ERREF
        /// </summary>
        NTLMBlocked = 0xC0000418,
        /// <summary>
        /// DC returns STATUS_AUTHENTICATION_FIREWALL_FAILED mentioned in MS-ERREF
        /// </summary>
        AuthenticationFirewallFailed = 0xC0000413,
        /// <summary>
        /// DC returns STATUS_WRONG_PASSWORD
        /// </summary>
        WrongPassword = 0xC000006A,
        /// <summary>
        /// DC returns SEC_E_QOP_NOT_SUPPORTED
        /// </summary>
        NotSupported = 0x8009030A,
        /// <summary>
        /// DC returns STATUS_ACCOUNT_RESTRICTION
        /// </summary>
        AccountRestriction = 0xC000006E
    }    

    /// <summary>
    /// A 32-bit unsigned integer that defines the digest validation
    /// message type. This member MUST be set to 0x0000001A.
    /// </summary>
    /// Disable warning CA1008 because according to actual Model design, Zero value are not usful for every enumeration.
    /// Disable warning CA1028 because System.Int32. cannot match the enumeration design accroding to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum MessageType : uint
    {
        /// <summary>
        /// Request value.
        /// </summary>
        V1 = 0x0000001a,

        /// <summary>
        /// Response value.
        /// </summary>
        V2 = 0x0000000a,
    }

    /// <summary>
    /// A 16-bit unsigned integer that defines the version of
    /// the digest validation protocol. The protocol version
    /// defined in this document is 1 (the value of this member
    /// MUST be 0x0001).
    /// </summary>
    /// Disable warning CA1008 because according to actual Model design, Zero value are not usful for every enumeration.
    /// Disable warning CA1028 because System.Int32. cannot match the enumeration design accroding to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum Version : ushort
    {

        /// <summary>
        ///  Version 1
        /// </summary>
        V1 = 0x0001,
    }

    /// <summary>
    /// A 16-bit unsigned integer field reserved for future
    /// use. The value of this member MUST be 0, and MUST
    /// be ignored on receipt.
    /// </summary>
    /// Disable warning CA1028 because System.Int32. cannot match the enumeration design accroding to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Reserved1 : ushort
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// A 16-bit unsigned integer field reserved for future
    /// use. The value of this member MUST be 0, and MUST
    /// be ignored on receipt.
    /// </summary>
    /// Disable warning CA1028 because System.Int32. cannot match the enumeration design accroding to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Reserved3 : ushort
    {
        /// <summary>
        ///  Default value.
        /// </summary>
        V1 = 0,
        /// <summary>
        /// Possible value.
        /// </summary>
        V2 = 1,
    }

    /// <summary>
    /// A 32-bit unsigned integer field from response.
    /// The value of this member MUST be 0, and MUST
    /// be ignored on receipt.
    /// </summary>
    /// Disable warning CA1028 because System.Int32. cannot match the enumeration design accroding to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Resp_Reserved3 : uint
    {
        /// <summary>
        ///  Default value.
        /// </summary>
        V1 = 0,
        /// <summary>
        /// Possible value.
        /// </summary>
        V2 = 1,
    }

    /// <summary>
    /// A 16-bit unsigned integer field reserved for future
    /// use. The value of this member MUST be 0, and MUST
    /// be ignored on receipt. This field is missing in the TD.
    /// </summary>
    /// Disable warning CA1028 because System.Int32. cannot match the enumeration design accroding to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Reserved4 : ushort
    {
        /// <summary>
        ///  Default value.
        /// </summary>
        V1 = 0,
        /// <summary>
        /// Possible value.
        /// </summary>
        V2 = 1,
    }

    /// <summary>
    /// An unused 64-bit unsigned integer. The value of this
    ///  member MUST be 0, and MUST be ignored on receipt.
    /// </summary>
    /// Disable warning CA1028 because System.Int32. cannot match the enumeration design accroding to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Pad1 : ulong
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    #endregion

    #region Other Enums used by test suite

    /// <summary>
    /// Indicates the SUT OS being tested.
    /// </summary>
    public enum OSVersion
    {
        NONWINDOWS   = 0,
        WINSVR2008   = 1,
        WINSVR2008R2 = 2,
        WINSVR2012   = 3,
        WINSVR2012R2 = 4,
        OTHER        = 5
    }

    #endregion
}