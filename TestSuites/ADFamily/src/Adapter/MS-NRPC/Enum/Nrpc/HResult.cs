// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// Various NRPC API error codes.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32",
        Justification = "EnumStorageShouldBeInt32")]
    public enum HRESULT : uint
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        ERROR_SUCCESS = 0x00000000,

        /// <summary>
        /// Access is denied.
        /// </summary>
        ERROR_ACCESS_DENIED = 0x00000005,

        /// <summary>
        /// The request is not supported.
        /// </summary>
        ERROR_NOT_SUPPORTED = 0x00000032,

        /// <summary>
        /// The parameter is incorrect.
        /// </summary>
        ERROR_INVALID_PARAMETER = 0x00000057,

        /// <summary>
        /// The system call level is not correct.
        /// </summary>
        ERROR_INVALID_LEVEL = 0x0000007C,

        /// <summary>
        /// Invalid flags.
        /// </summary>
        ERROR_INVALID_FLAGS = 0x000003EC,

        /// <summary>
        /// The specified account does not exist.
        /// </summary>
        ERROR_NO_SUCH_USER = 0x00000525,

        /// <summary>
        /// The specified domain either does not exist or could not be contacted.
        /// </summary>
        ERROR_NO_SUCH_DOMAIN = 0x0000054B,

        /// <summary>
        /// There are currently no logon servers available to service the logon request.
        /// </summary>
        ERROR_NO_LOGON_SERVERS = 0x0000051F,

        /// <summary>
        /// The format of the specified computer name is invalid.
        /// </summary>
        ERROR_INVALID_COMPUTERNAME = 0x000004BA,

        /// <summary>
        /// The format of the specified domain name is invalid.
        /// </summary>
        ERROR_INVALID_DOMAINNAME = 0x000004BC,

        /// <summary>
        /// This operation is only allowed for the PDC of the domain.
        /// </summary>
        ERROR_INVALID_DOMAIN_ROLE = 0x0000054A,

        /// <summary>
        /// 0XFFFFF could not be found in MS-ERREF.
        /// </summary>
        ERROR_FAILURE = 0XFFFFF,

        /// <summary>
        /// The trust relationship between this workstation and the primary domain failed.
        /// </summary>
        ERROR_TRUSTED_RELATIONSHIP_FAILURE = 0x000006FD,

        /// <summary>
        /// The service has not been started.
        /// </summary>
        ERROR_SERVICE_NOT_ACTIVE = 0x00000426,

        /// <summary>
        /// A security package-specific error occurred.
        /// </summary>
        RPC_S_SEC_PKG_ERROR = 0x00000721,

        /// <summary>
        /// {Not Implemented} The requested operation is not implemented.
        /// </summary>
        STATUS_NOT_IMPLEMENTED = 0xc0000002,

        /// <summary>
        /// {Invalid Parameter} The specified information class is not a valid information class for the specified 
        /// object.
        /// </summary>
        STATUS_INVALID_INFO_CLASS = 0xC0000003,

        /// <summary>
        /// An invalid parameter was passed to a service or function.
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// {Access Denied} A process has requested access to an object but has not been granted those access rights.
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022,

        /// <summary>
        /// The directory service is busy.
        /// </summary>
        STATUS_DS_BUSY = 0xC00002A5,

        /// <summary>
        /// The specified account does not exist.
        /// </summary>
        STATUS_NO_SUCH_USER = 0xC0000064,

        /// <summary>
        /// The request is not supported.
        /// </summary>
        STATUS_NOT_SUPPORTED = 0xC00000BB,

        /// <summary>
        /// Indicates a name that was specified as a remote computer name is syntactically invalid.
        /// </summary>
        STATUS_INVALID_COMPUTER_NAME = 0xC0000122,

        /// <summary>
        /// The workstation does not have a trust secret for the primary domain in the local LSA database.
        /// </summary>
        STATUS_NO_TRUST_LSA_SECRET = 0xC000018A,

        /// <summary>
        /// When trying to update a password, this return status indicates that the value provided as 
        /// the current password is not correct.
        /// </summary>
        STATUS_WRONG_PASSWORD = 0xC000006A,

        /// <summary>
        /// The specified domain did not exist.
        /// </summary>
        STATUS_NO_SUCH_DOMAIN = 0xC00000DF,

        /// <summary>
        /// The SAM database on the Windows Server does not have a computer account for this 
        /// workstation trust relationship.
        /// </summary>
        STATUS_NO_TRUST_SAM_ACCOUNT = 0xC000018B,

        /// <summary>
        /// {Incorrect System Call Level} An invalid level was passed into the specified system call.
        /// </summary>
        STATUS_INVALID_LEVEL = 0xC0000148,

        /// <summary>
        /// The logon request failed because the trust relationship between the primary domain and 
        /// the trusted domain failed.
        /// </summary>
        STATUS_TRUSTED_DOMAIN_FAILURE = 0xC000018C,

        /// <summary>
        /// The system detected a possible attempt to compromise security. 
        /// Ensure that you can contact the server that authenticated you.
        /// </summary>
        STATUS_DOWNGRADE_DETECTED = 0xc0000388,

        /// <summary>
        /// An attempt was made to logon, but the NetLogon service was not started.
        /// </summary>
        STATUS_NETLOGON_NOT_STARTED = 0xC0000192,

        /// <summary>
        /// Could not find the domain controller for this domain.
        /// </summary>
        NERR_DCNotFound = 0x00000995,

        /// <summary>
        /// This operation is allowed only on the PDC of the domain.
        /// </summary>
        NERR_NotPrimary = 0x000008B2,

        /// <summary>
        /// Unspecified error.
        /// </summary>
        E_FAIL = 0x80004005,

        /// <summary>
        /// The token supplied to the function is invalid.
        /// </summary>
        SEC_E_INVALID_TOKEN = 0x80090308,

        /// <summary>
        /// The message or signature supplied for verification has been altered.
        /// </summary>
        SEC_E_MESSAGE_ALTERED = 0x8009030F,

        /// <summary>
        /// Represent any non-zero error code. It is only used when no accurate error code is given. 
        /// </summary>
        ERROR_NON_ZERO = 0xFFFFFFFF,

        /// <summary>
        /// Invalid Check Sum
        /// </summary>
        Invalid_Checksum = 0x09,

        /// <summary>
        /// No site name is available for this machine.
        /// </summary>
        ERROR_NO_SITENAME = 0x0000077F,

        /// <summary>
        /// Indicate the request is out of sequence.
        /// </summary>
        SEC_E_OUT_OF_SEQUENCE = 1825,
    }
}
