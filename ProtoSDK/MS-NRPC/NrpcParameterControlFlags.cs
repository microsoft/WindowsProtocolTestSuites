// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// A set of bit flags that contain information pertaining 
    /// to the logon validation processing.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum NrpcParameterControlFlags : uint
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        ///A:<para/>
        ///Clear text passwords can be transmitted for this logon identity.
        ///</summary>
        ClearTextPasswordsCanBeTransmitted = 0x2,

        ///<summary>
        ///B:<para/>
        ///Update the logon statistics for this account upon successful logon.
        ///</summary>
        UpdateLogonStatistics = 0x4,

        ///<summary>
        ///C:<para/>
        ///Return the user parameter list for this account upon successful logon.
        ///</summary>
        ReturnUserParameterList = 0x8,

        ///<summary>
        ///D:<para/>
        ///Do not attempt to log this account on as a guest upon logon failure.
        ///</summary>
        DoNotAttemptToLogonAsGuest = 0x10,

        ///<summary>
        ///E:<para/>
        ///Allow this account to log on with the domain controller account.
        ///</summary>
        AllowToLogonWithDcAccount = 0x20,

        ///<summary>
        ///F:<para/>
        ///Return the password expiration date and time upon successful logon.
        ///</summary>
        ReturnPasswordExpirationDateTime = 0x40,

        ///<summary>
        ///G:<para/>
        ///Send a client challenge upon logon request.
        ///</summary>
        SendClientChallenge = 0x80,

        ///<summary>
        ///H:<para/>
        ///Attempt logon as a guest for this account only.
        ///</summary>
        AttemptLogonAsGuest = 0x100,

        ///<summary>
        ///I:<para/>
        ///Return the profile path upon successful logon.
        ///</summary>
        ReturnProfilePath = 0x200,

        ///<summary>
        ///J:<para/>
        ///Attempt logon to the specified domain only.
        ///</summary>
        AttemptLogonToDomainOnly = 0x400,

        ///<summary>
        ///K:<para/>
        ///Allow this account to log on with the computer account.
        ///</summary>
        AllowLogonWithComputerAccount = 0x800,

        ///<summary>
        ///L:<para/>
        ///Disable allowing fallback to guest account for this account.
        ///</summary>
        DisableAllowingFallbackToGuest = 0x1000,

        ///<summary>
        ///M:<para/>
        ///Force the logon of this account as a guest if the password is incorrect.
        ///</summary>
        ForceLogonAsGuestIfPasswordIsIncorrect = 0x2000,

        ///<summary>
        ///N:<para/>
        ///This account has supplied a clear text password.
        ///</summary>
        SuppliedClearTextPassword = 0x4000,

        ///<summary>
        ///O:<para/>
        ///Allow NTLMv1 authentication ([MS-NLMP]) when only NTLMv2 ([NTLM]) is allowed.
        ///</summary>
        ForceAllowNtlmV1 = 0x10000,

        ///<summary>
        ///P:<para/>
        ///Use subauthentication ([MS-APDS] section 3.1.5.2.1).
        ///</summary>
        UseSubauthentication = 0x100000,
        
        ///<summary>
        ///Q:<para/>
        ///Encode the subauthentication package identifier. Bits Q–X are used to encode the integer value of the subauthentication package identifier (this is in little-endian order).
        ///</summary>
        EncodeSubauthenticationPackageIdentifierQ = 0x1000000,

        ///<summary>
        ///R:<para/>
        ///Encode the subauthentication package identifier. Bits Q–X are used to encode the integer value of the subauthentication package identifier (this is in little-endian order).
        ///</summary>
        EncodeSubauthenticationPackageIdentifierR = 0x2000000,

        ///<summary>
        ///S:<para/>
        ///Encode the subauthentication package identifier. Bits Q–X are used to encode the integer value of the subauthentication package identifier (this is in little-endian order).
        ///</summary>
        EncodeSubauthenticationPackageIdentifierS = 0x4000000,

        ///<summary>
        ///T:<para/>
        ///Encode the subauthentication package identifier. Bits Q–X are used to encode the integer value of the subauthentication package identifier (this is in little-endian order).
        ///</summary>
        EncodeSubauthenticationPackageIdentifierT = 0x8000000,

        ///<summary>
        ///U:<para/>
        ///Encode the subauthentication package identifier. Bits Q–X are used to encode the integer value of the subauthentication package identifier (this is in little-endian order).
        ///</summary>
        EncodeSubauthenticationPackageIdentifierU = 0x10000000,

        ///<summary>
        ///V:<para/>
        ///Encode the subauthentication package identifier. Bits Q–X are used to encode the integer value of the subauthentication package identifier (this is in little-endian order).
        ///</summary>
        EncodeSubauthenticationPackageIdentifierV = 0x20000000,

        ///<summary>
        ///W:<para/>
        ///Encode the subauthentication package identifier. Bits Q–X are used to encode the integer value of the subauthentication package identifier (this is in little-endian order).
        ///</summary>
        EncodeSubauthenticationPackageIdentifierW = 0x40000000,

        ///<summary>
        ///X:<para/>
        ///Encode the subauthentication package identifier. Bits Q–X are used to encode the integer value of the subauthentication package identifier (this is in little-endian order).
        ///</summary>
        EncodeSubauthenticationPackageIdentifierX = 0x80000000
    }
}
