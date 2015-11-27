// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /// <summary>
    /// Specify the account type of cName
    /// </summary>
    public enum KerberosAccountType : byte
    {
        /// <summary>
        /// User account
        /// </summary>
        User = 0,

        /// <summary>
        /// Computer account
        /// </summary>
        Device = 1,
    }


    /// <summary>
    /// The padata type represent which padata value will be contained in request.
    /// </summary>
    public enum PaDataType : int
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// DER encoding of AP-REQ
        /// </summary>
        PA_TGS_REQ = 1,

        /// <summary>
        /// DER encoding of PA-ENC-TIMESTAMP
        /// </summary>
        PA_ENC_TIMESTAMP = 2,

        /// <summary>
        /// salt (not ASN.1 encoded)
        /// </summary>
        PA_PW_SALT = 3,

        /// <summary>
        /// DER encoding of ETYPE-INFO
        /// </summary>
        PA_ETYPE_INFO = 11,

        /// <summary>
        /// DER encoding of PA_PK_AS_REQ_OLD
        /// </summary>
        PA_PK_AS_REQ_OLD = 14,

        /// <summary>
        /// DER encoding of PA_PK_AS_REP_OLD
        /// </summary>
        PA_PK_AS_REP_OLD = 15,

        /// <summary>
        /// DER encoding of PA_PK_AS_REQ
        /// </summary>
        PA_PK_AS_REQ = 16,

        /// <summary>
        /// DER encoding of PA_PK_AS_REP
        /// </summary>
        PA_PK_AS_REP = 17,

        /// <summary>
        /// DER encoding of ETYPE-INFO2
        /// </summary>
        PA_ETYPE_INFO2 = 19,

        /// <summary>
        /// DER encoding of PA_SVR_REFERRAL_DATA
        /// </summary>
        PA_SVR_REFERRAL_INFO = 20,

        /// <summary>
        /// DER encoding of PA_PAC_REQUEST
        /// </summary>
        PA_PAC_REQUEST = 128,

        /// <summary>
        /// PA_FOR_USER of SFU
        /// </summary>
        PA_SFU_PA_FOR_USER = 129,

        /// <summary>
        /// PA_S4U_X509_USER of SFU
        /// </summary>
        PA_SFU_PA_S4U_X509_USER = 130,

        /// <summary>
        /// PA_DATA_CHEKSUM in as-request
        /// </summary>
        PA_DATA_CHEKSUM = 132,


        /// <summary>
        /// PA-FX-COOKIE Stateless cookie that is not tied to a specific KDC.
        /// </summary>
        PA_FX_COOKIE = 133,

        /// <summary>
        /// Fast
        /// </summary>
        PA_FX_FAST = 136,

        /// <summary>
        /// PA_FX_ERROR
        /// </summary>
        PA_FX_ERROR = 137,

        /// <summary>
        /// Encrypted PA-ENC-TS-ENC
        /// </summary>
        PA_ENCRYPTED_CHALLENGE = 138,

        /// <summary>
        /// supported encryption types
        /// </summary>
        PA_SUPPORTED_ENCTYPES = 165,

        /// <summary>
        /// Pac option
        /// </summary>
        PA_PAC_OPTION = 167,

        /// <summary>
        /// Invalid padata type
        /// </summary>
        UnKnow = 10000,

        /// <summary>
        /// DER encoding of PA_PK_AS_REQ_OLD
        /// </summary>
        PA_PK_AS_REQ_WINDOWS_OLD = 15,

        /// <summary>
        /// DER encoding of PA_PK_AS_REP_OLD
        /// </summary>
        PA_PK_AS_REP_WINDOWS_OLD = 15,
    }


    /// <summary>
    /// Key usage number from rfc 4120,in 
    /// http://www.apps.ietf.org/rfc/rfc4120.html#sec-7.5.1
    /// </summary>
    public enum KeyUsageNumber : int
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// AS-REQ PA-ENC-TIMESTAMP padata timestamp, encrypted with the client key (Section 5.2.7.2) 
        /// </summary>
        PA_ENC_TIMESTAMP = 1,

        /// <summary>
        /// AS-REP Ticket and TGS-REP Ticket (includes TGS session 
        /// key or application session key), encrypted with the service key (Section 5.3)
        /// </summary>
        AS_REP_TicketAndTGS_REP_Ticket = 2,

        /// <summary>
        /// AS-REP encrypted part (includes TGS session key or
        /// application session key), encrypted with the client key (Section 5.4.2) 
        /// </summary>
        AS_REP_ENCRYPTEDPART = 3,

        /// <summary>
        /// TGS-REQ KDC-REQ-BODY AuthorizationData, encrypted with 
        /// the TGS session key (Section 5.4.1)
        /// </summary>
        TGS_REQ_KDC_REQ_BODY_AuthorizationData = 4,

        /// <summary>
        /// TGS-REQ KDC-REQ-BODY AuthorizationData, encrypted with 
        /// the TGS authenticator subkey (Section 5.4.1) 
        /// </summary>
        TGS_REQ_KDC_REQ_BODY_AuthorizatorData = 5,

        /// <summary>
        /// TGS-REQ PA-TGS-REQ padata AP-REQ Authenticator cksum,
        /// keyed with the TGS session key (Section 5.5.1) 7. TGS-REQ PA-TGS-REQ padata
        /// AP-REQ Authenticator (includes TGS authenticator subkey), encrypted with 
        /// the TGS session key (Section 5.5.1) 
        /// </summary>
        TGS_REQ_PA_TGS_REQ_adataOR_AP_REQ_Authenticator_cksum = 6,

        /// <summary>
        /// TGS-REQ PA-TGS-REQ padata AP-REQ Authenticator (includes TGS authenticator subkey), 
        /// encrypted with the TGS session key (Section 5.5.1) 
        /// </summary>
        TG_REQ_PA_TGS_REQ_padataOR_AP_REQ_Authenticator = 7,

        /// <summary>
        /// TGS-REP encrypted part (includes application sessionkey),
        /// encrypted with the TGS session key (Section 5.4.2)
        /// </summary>
        TGS_REP_encrypted_part = 8,

        /// <summary>
        /// TGS-REP encrypted part (includes application session key),
        /// encrypted with the TGS authenticator subkey(Section 5.4.2)
        /// </summary>
        TGS_REP_encrypted_part_subkey = 9,

        /// <summary>
        /// AP-REQ Authenticator checksum in TGS request.
        /// </summary>
        AP_REQ_Authenticator_Checksum = 10,

        /// <summary>
        /// AP-REQ Authenticator.
        /// </summary>
        AP_REQ_Authenticator = 11,

        /// <summary>
        /// AP-REP encrypted part.
        /// </summary>
        AP_REP_EncAPRepPart = 12,

        /// <summary>
        /// KRB-PRIV encrypted part.
        /// </summary>
        KRB_PRIV_EncPart = 13,

        /// <summary>
        /// KRB-CRED encrypted part.
        /// </summary>
        KRB_CRED_EncPart = 14,

        /// <summary>
        /// KRB-SAFE cksum, keyed with a key chosen by the application (Section 5.6.1)
        /// </summary>
        KRB_SAFE_Checksum = 15,

        /// <summary>
        /// KrbFastArmoredReq Checksum.([RFC6113] Section 5.4.2)
        /// </summary>
        FAST_REQ_CHECKSUM = 50,

        /// <summary>
        /// KrbFastArmoredReq encrypted part.([RFC6113] Section 5.4.2)
        /// </summary>
        FAST_ENC = 51,

        /// <summary>
        /// KrbFastArmoredRep encrypted part.([RFC6113] Section 5.4.2)
        /// </summary>
        FAST_REP = 52,

        /// <summary>
        /// KrbFastFinished ticket-checksum part.([RFC6113] Section 5.4.3)
        /// </summary>
        FAST_FINISHED = 53,
        /// <summary>
        /// EncryptedChallenge encrypted part.([RFC6113] Section 5.4.6)
        /// </summary>
        ENC_CHALLENGE_CLIENT = 54
    }

    /// <summary>
    /// usage number for token.
    /// </summary>
    public enum TokenKeyUsage : int
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// usage number for wrap token for [rfc4757].
        /// </summary>
        USAGE_WRAP = 13,

        /// <summary>
        /// usage number for mic token for [rfc4757].
        /// </summary>
        USAGE_MIC = 15,

        /// <summary>
        /// usage number for wrap and mic token for [rfc4121].
        /// </summary>
        KG_USAGE_ACCEPTOR_SEAL = 22,

        /// <summary>
        /// usage number for wrap and mic token for [rfc4121].
        /// </summary>
        KG_USAGE_ACCEPTOR_SIGN = 23,

        /// <summary>
        /// usage number for wrap and mic token for [rfc4121].
        /// </summary>
        KG_USAGE_INITIATOR_SEAL = 24,

        /// <summary>
        /// usage number for wrap and mic token for [rfc4121].
        /// </summary>
        KG_USAGE_INITIATOR_SIGN = 25,
    }


    /// <summary>
    /// KILE Message Type
    /// </summary>
    public enum MsgType : int
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// This signifies KRB_AS_REQ message Type
        /// </summary>
        KRB_AS_REQ = 10,

        /// <summary>
        /// This signifies KRB_AS_RESP message Type
        /// </summary>
        KRB_AS_RESP = 11,

        /// <summary>
        /// This signifies KRB_TGS_REQ message Type
        /// </summary>
        KRB_TGS_REQ = 12,

        /// <summary>
        /// This signifies KRB_TGS_RESP message Type
        /// </summary>
        KRB_TGS_RESP = 13,

        /// <summary>
        /// This signifies KRB_AP_REQ message Type
        /// </summary>
        KRB_AP_REQ = 14,

        /// <summary>
        /// This signifies KRB_AP_RESP message Type
        /// </summary>
        KRB_AP_RESP = 15,

        /// <summary>
        /// This signifies KRB_SAFE message Type
        /// </summary>
        KRB_SAFE = 20,

        /// <summary>
        /// This signifies KRB_PRIV message Type
        /// </summary>
        KRB_PRIV = 21,

        /// <summary>
        /// This signifies KRB_CRED message Type
        /// </summary>
        KRB_CRED = 22,

        /// <summary>
        /// This signifies KRB_ERROR message Type
        /// </summary>
        KRB_ERROR = 30
    }


    /// <summary>
    /// EncTicket Flags, can be found in rfc 4120. Section 5.3 Tickets
    /// http://www.ietf.org/rfc/rfc4120.txt
    /// </summary>
    [Flags]
    public enum EncTicketFlags : uint
    {
        /// <summary>
        /// Reserved for future expansion of this field.
        /// </summary>
        RESERVED = (uint)1 << 31,

        /// <summary>
        /// The FORWARDABLE flag is normally only interpreted by the TGS, and can be ignored by end servers.
        /// When set, this flag tells the ticket-granting server that it is OK to issue a new TGT 
        /// with a different network address based on the presented ticket.
        /// </summary>
        FORWARDABLE = (uint)1 << 30,

        /// <summary>
        /// When set, this flag indicates that the ticket has either been forwarded TGT.
        /// </summary>
        FORWARDED =  (uint)1 << 29,

        /// <summary>
        /// The PROXIABLE flag is normally only interpreted by the TGS, and can be ignored by end servers. 
        /// The PROXIABLE flag has an interpretation identical to that of the FORWARDABLE flag, 
        /// except that the PROXIABLE flag tells the ticket-granting server that only non-TGTs may be issued with different network addresses.
        /// </summary>
        PROXIABLE = (uint)1 << 28,

        /// <summary>
        /// When set, this flag indicates that a ticket is a proxy.
        /// </summary>
        PROXY = (uint)1 << 27,

        /// <summary>
        /// The MAY-POSTDATE flag is normally only interpreted by the TGS, and can be ignored by end servers. 
        /// This flag tells the ticket-granting server that a post-dated ticket MAY be issued based on this TGT.
        /// </summary>
        MAY_POSTDATE = (uint)1 << 26,

        /// <summary>
        /// This flag indicates that this ticket has been postdated. 
        /// The end-service can check the authtime field to see when the original authentication occurred.
        /// </summary>
        POSTDATED = (uint)1 << 25,

        /// <summary>
        /// This flag indicates that a ticket is invalid, and it must be validated by the KDC before use. 
        /// Application servers must reject tickets which have this flag set.
        /// </summary>
        INVALID = (uint)1 << 24,

        /// <summary>
        /// The RENEWABLE flag is normally only interpreted by the TGS, and can usually be ignored by end servers 
        /// (some particularly careful servers MAY disallow renewable tickets). A renewable ticket can be used 
        /// to obtain a replacement ticket that expires at a later date.
        /// </summary>
        RENEWABLE = (uint)1 << 23,

        /// <summary>
        /// This flag indicates that this ticket was issued using the AS protocol, and not issued based on a TGT.
        /// </summary>
        INITIAL = (uint)1 << 22,

        /// <summary>
        /// This flag indicates that during initial authentication, the client was authenticated by the KDC before a ticket was issued. 
        /// The strength of the pre-authentication method is not indicated, but is acceptable to the KDC.
        /// </summary>
        PRE_AUTHENT = (uint)1 << 21,

        /// <summary>
        /// This flag indicates that the protocol employed for initial authentication required the use of hardware expected to 
        /// be possessed solely by the named client. The hardware authentication method is and the strength of the method is not indicated.
        /// </summary>
        HW_AUTHENT = (uint)1 << 20,

        /// <summary>
        /// This flag indicates that the KDC for policy-checked the realm has checked the transited field against a realm-defined policy for trusted certifiers. 
        /// If this flag is reset (0), then the application server must check the transited field itself, and if unable to do so, it must reject the authentication.
        /// If the flag is set (1), then the application server MAY skip its own validation of the transited field, relying on the validation performed by the KDC. 
        /// At its option the application server MAY still apply its own validation based on a separate policy for acceptance.
        /// This flag is new since RFC 1510.
        /// </summary>
        TRANSITED_POLICY_CHECKED = (uint)1 << 19,

        /// <summary>
        /// This flag indicates that the server (not the client) specified in the ticket has been determined by policy of the realm to be a suitable recipient of delegation.
        /// A client can use the presence of this flag to help it decide whether to delegate credentials (either grant a proxy or a forwarded TGT) to this server.
        /// The client is free to ignore the value of this flag. When setting this flag, an administrator should consider the security and placement of the server
        /// on which the service will run, as well as whether the service requires the use of delegated credentials. 
        /// This flag is new since RFC 1510.
        /// </summary>
        OK_AS_DELEGATE = (uint)1 << 18
    }


    /// <summary>
    /// KRB FLAGS, can find in rfc 4120
    /// http://www.ietf.org/rfc/rfc4120.txt
    /// </summary>
    [Flags]
    public enum KdcOptions : uint
    {
        /// <summary>
        /// This signifies Validate KRB Flag
        /// </summary>
        VALIDATE = 0x00000001,

        /// <summary>
        /// This signifies RENEW KRB Flag
        /// </summary>
        RENEW = 0x00000002,

        /// <summary>
        /// This signifies UNUSED29 KRB Flag
        /// </summary>
        UNUSED29 = 0x00000004,

        /// <summary>
        /// This signifies  ENCTKTINSKEY KRB Flag
        /// </summary>
        ENCTKTINSKEY = 0x00000008,

        /// <summary>
        /// This signifies RENEWABLEOK KRB Flag
        /// </summary>
        RENEWABLEOK = 0x00000010,

        /// <summary>
        /// This signifies DISABLETRANSITEDCHECK Flag
        /// </summary>
        DISABLETRANSITEDCHECK = 0x00000020,

        /// <summary>
        /// This signifies  UNUSED16 Flag
        /// </summary>
        UNUSED16 = 0x0000FFC0,

        /// <summary>
        /// This signifies CANONICALIZE Flag
        /// </summary>
        CANONICALIZE = 0x00010000,

        /// <summary>
        /// This signifies CNAMEINADDLTKT Flag
        /// </summary>
        CNAMEINADDLTKT = 0x00020000,

        /// <summary>
        /// This signifies OK_AS_DELEGATE KRB Flag
        /// </summary>
        OK_AS_DELEGATE = 0x00040000,

        /// <summary>
        /// This signifies UNUSED12 KRB Flag
        /// </summary>
        UNUSED12 = 0x00080000,

        /// <summary>
        /// This signifies OPTHARDWAREAUTH KRB Flag
        /// </summary>
        OPTHARDWAREAUTH = 0x00100000,

        /// <summary>
        /// This signifies UNUSED10 KRB Flag
        /// </summary>
        PREAUTHENT = 0x00200000,

        /// <summary>
        /// This signifies UNUSED9 KRB Flag
        /// </summary>
        INITIAL = 0x00400000,

        /// <summary>
        /// This signifies RENEWABLE KRB Flag
        /// </summary>
        RENEWABLE = 0x00800000,

        /// <summary>
        /// This signifies UNUSED7 KRB Flag
        /// </summary>
        UNUSED7 = 0x01000000,

        /// <summary>
        /// This signifies POSTDATED KRB Flag
        /// </summary>
        POSTDATED = 0x02000000,

        /// <summary>
        /// This signifies ALLOWPOSTDATE KRB Flag
        /// </summary>
        ALLOWPOSTDATE = 0x04000000,

        /// <summary>
        /// This signifies PROXY KRB Flag
        /// </summary>
        PROXY = 0x08000000,

        /// <summary>
        /// This signifies PROXIABLE KRB Flag
        /// </summary>
        PROXIABLE = 0x10000000,

        /// <summary>
        /// This signifies FORWARDED KRB Flag
        /// </summary>
        FORWARDED = 0x20000000,

        /// <summary>
        /// This signifies FORWARDABLE KRB Flag
        /// </summary>
        FORWARDABLE = 0x40000000,

        /// <summary>
        /// This signifies RESERVED KRB Flag
        /// </summary>
        RESERVED = 0x80000000,
    }


    /// <summary>
    /// Represents the valid error messages associated with KILE.
    /// Reference rfc 4120  section 7.5.9. Error Codes
    /// http://www.ietf.org/rfc/rfc4120.txt
    /// </summary>
    public enum KRB_ERROR_CODE : int
    {
        /// <summary>
        ///  No error
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Client's entry in database has expired
        /// </summary>
        KDC_ERR_NAME_EXP = 1,

        /// <summary>
        /// Server's entry in database has expired
        /// </summary>

        KDC_ERR_SERVICE_EXP = 2,
        /// <summary>
        /// Requested protocol version    number not supported
        /// </summary>

        KDC_ERR_BAD_PVNO = 3,
        /// <summary>
        ///  Client's key encrypted in old master key
        /// </summary>

        KDC_ERR_C_OLD_MAST_KVNO = 4,
        /// <summary>
        /// Server's key encrypted in old master key
        /// </summary>

        KDC_ERR_S_OLD_MAST_KVNO = 5,
        /// <summary>
        ///  Client not found in Kerberos database
        /// </summary>

        KDC_ERR_C_PRINCIPAL_UNKNOWN = 6,
        /// <summary>
        /// Server not found in Kerberos database
        /// </summary>

        KDC_ERR_S_PRINCIPAL_UNKNOWN = 7,
        /// <summary>
        /// Multiple principal entries in database
        /// </summary>

        KDC_ERR_PRINCIPAL_NOT_UNIQUE = 8,
        /// <summary>
        /// The client or server has a null key
        /// </summary>

        KDC_ERR_NULL_KEY = 9,
        /// <summary>
        /// Ticket not eligible for postdating
        /// </summary>

        KDC_ERR_CANNOT_POSTDATE = 10,
        /// <summary>
        ///  Requested starttime is later than endtime
        /// </summary>
        KDC_ERR_NEVER_VALID = 11,

        /// <summary>
        ///  KDC policy rejects request
        /// </summary>
        KDC_ERR_POLICY = 12,

        /// <summary>
        ///  KDC cannot accommodate  requested option
        /// </summary>
        KDC_ERR_BADOPTION = 13,

        /// <summary>
        ///  KDC has no support for encryption type
        /// </summary>
        KDC_ERR_ETYPE_NOTSUPP = 14,

        /// <summary>
        /// KDC has no support for checksum type
        /// </summary>
        KDC_ERR_SUMTYPE_NOSUPP = 15,

        /// <summary>
        ///  KDC has no support for padata type
        /// </summary>
        KDC_ERR_PADATA_TYPE_NOSUPP = 16,

        /// <summary>
        /// KDC has no support for transited type
        /// </summary>
        KDC_ERR_TRTYPE_NOSUPP = 17,

        /// <summary>
        /// Clients credentials have   been revoked
        /// </summary>
        KDC_ERR_CLIENT_REVOKED = 18,

        /// <summary>
        /// Credentials for server have  been revoked
        /// </summary>
        KDC_ERR_SERVICE_REVOKED = 19,

        /// <summary>
        ///  TGT has been revoked
        /// </summary>
        KDC_ERR_TGT_REVOKED = 20,

        /// <summary>
        /// Client not yet valid; try again later
        /// </summary>
        KDC_ERR_CLIENT_NOTYET = 21,

        /// <summary>
        ///  Server not yet valid; try again later
        /// </summary>
        KDC_ERR_SERVICE_NOTYET = 22,

        /// <summary>
        /// Password has expired;change password to reset
        /// </summary>
        KDC_ERR_KEY_EXPIRED = 23,

        /// <summary>
        /// Pre-authentication information was invalid
        /// </summary>
        KDC_ERR_PREAUTH_FAILED = 24,

        /// <summary>
        /// Additional pre-authentication required
        /// </summary>
        KDC_ERR_PREAUTH_REQUIRED = 25,

        /// <summary>
        /// Requested server and ticket don't match
        /// </summary>
        KDC_ERR_SERVER_NOMATCH = 26,

        /// <summary>
        ///  Server principal valid for user2user only
        /// </summary>
        KDC_ERR_MUST_USE_USER2USER = 27,

        /// <summary>
        /// KDC Policy rejects transited path
        /// </summary>
        KDC_ERR_PATH_NOT_ACCEPTED = 28,

        /// <summary>
        /// A service is not available
        /// </summary>
        KDC_ERR_SVC_UNAVAILABLE = 29,

        /// <summary>
        /// Integrity check on decrypted field failed
        /// </summary>
        KRB_AP_ERR_BAD_INTEGRITY = 31,

        /// <summary>
        /// Ticket expired
        /// </summary>
        KRB_AP_ERR_TKT_EXPIRED = 32,

        /// <summary>
        /// Ticket not yet valid
        /// </summary>
        KRB_AP_ERR_TKT_NYV = 33,

        /// <summary>
        ///  Request is a replay
        /// </summary>
        KRB_AP_ERR_REPEAT = 34,

        /// <summary>
        /// The ticket isn't for us
        /// </summary>
        KRB_AP_ERR_NOT_US = 35,

        /// <summary>
        /// Ticket and authenticator don't match
        /// </summary>
        KRB_AP_ERR_BADMATCH = 36,

        /// <summary>
        /// Clock skew too great
        /// </summary>
        KRB_AP_ERR_SKEW = 37,

        /// <summary>
        /// Incorrect net address
        /// </summary>
        KRB_AP_ERR_BADADDR = 38,

        /// <summary>
        /// Protocol version mismatch
        /// </summary>
        KRB_AP_ERR_BADVERSION = 39,

        /// <summary>
        /// Invalid msg type
        /// </summary>
        KRB_AP_ERR_MSG_TYPE = 40,

        /// <summary>
        /// Message stream modified
        /// </summary>
        KRB_AP_ERR_MODIFIED = 41,

        /// <summary>
        /// Message out of order
        /// </summary>
        KRB_AP_ERR_BADORDER = 42,

        /// <summary>
        ///  Specified version of key is not available
        /// </summary>
        KRB_AP_ERR_BADKEYVER = 44,

        /// <summary>
        /// Service key not available
        /// </summary>
        KRB_AP_ERR_NOKEY = 45,

        /// <summary>
        ///  Mutual authentication  failed
        /// </summary>
        KRB_AP_ERR_MUT_FAIL = 46,

        /// <summary>
        ///  Incorrect message direction
        /// </summary>
        KRB_AP_ERR_BADDIRECTION = 47,

        /// <summary>
        /// Alternative authentication method required
        /// </summary>
        KRB_AP_ERR_METHOD = 48,

        /// <summary>
        /// Incorrect sequence number  in message
        /// </summary>
        KRB_AP_ERR_BADSEQ = 49,

        /// <summary>
        /// Inappropriate type of checksum in message
        /// </summary>
        KRB_AP_ERR_INAPP_CKSUM = 50,

        /// <summary>
        ///  Policy rejects transited  path
        /// </summary>
        KRB_AP_PATH_NOT_ACCEPTED = 51,

        /// <summary>
        ///  Response too big for UDP    retry with TCP
        /// </summary>
        KRB_ERR_RESPONSE_TOO_BIG = 52,

        /// <summary>
        /// Generic error (description in e-text)
        /// </summary>
        KRB_ERR_GENERIC = 60,

        /// <summary>
        /// Field is too long for this implementation
        /// </summary>
        KRB_ERR_FIELD_TOOLONG = 61,

        /// <summary>
        ///  Reserved for PKINIT
        /// </summary>
        KDC_ERROR_CLIENT_NOT_TRUSTED = 62,

        /// <summary>
        ///  Reserved for PKINIT
        /// </summary>
        KDC_ERROR_KDC_NOT_TRUSTED = 63,

        /// <summary>
        /// Reserved for PKINIT
        /// </summary>
        KDC_ERROR_INVALID_SIG = 64,

        /// <summary>
        ///   Reserved for PKINIT
        /// </summary>
        KDC_ERR_KEY_TOO_WEAK = 65,

        /// <summary>
        ///  Reserved for PKINIT
        /// </summary>
        KDC_ERR_CERTIFICATE_MISMATCH = 66,

        /// <summary>
        ///  No TGT available to validate USER-TO-USER
        /// </summary>
        KRB_AP_ERR_NO_TGT = 67,

        /// <summary>
        /// Reserved for future use
        /// </summary>
        KDC_ERR_WRONG_REALM = 68,

        /// <summary>
        /// Ticket must be for USER-TO-USER
        /// </summary>
        KRB_AP_ERR_USER_TO_USER_REQUIRED = 69,

        /// <summary>
        /// Reserved for PKINIT
        /// </summary>
        KDC_ERR_CANT_VERIFY_CERTIFICATE = 70,

        /// <summary>
        /// Reserved for PKINIT
        /// </summary>
        KDC_ERR_INVALID_CERTIFICATE = 71,

        /// <summary>
        /// Reserved for PKINIT
        /// </summary>
        KDC_ERR_REVOKED_CERTIFICATE = 72,

        /// <summary>
        /// KILE will return a zero-length message whenever it receives a message that is either not well-formed or not supported.
        /// </summary>
        ZERO_LENGTH_MESSAGE
    }


    /// <summary>
    /// Represents different modes associated with the KRB_PRIV message.
    /// </summary>
    public enum KRB_PRIV_REQUEST
    {
        /// <summary>
        /// Represents a KRB_PRIV message with timestamp 
        /// </summary>
        KrbPrivWithTimeStamp,

        /// <summary>
        /// Represents a KRB_PRIV message with sequencenumber 
        /// </summary>
        KrbPrivWithSequenceNumber,
    }

    /// <summary>
    /// Checksum algorithm type for Wrap and GetMic token.
    /// </summary>
    public enum SGN_ALG : ushort
    {
        /// <summary>
        /// DES MAC MD5 algorithm in rfc[1964].
        /// </summary>
        DES_MAC_MD5 = 0x0000,

        /// <summary>
        /// MD2.5 algorithm in rfc[1964].
        /// </summary>
        MD2_5 = 0x0100,

        /// <summary>
        /// DES-MAC algorithm in rfc[1964].
        /// </summary>
        DES_MAC = 0x0200,

        /// <summary>
        /// HMAC algorithm in rfc[4757].
        /// </summary>
        HMAC = 0x1100,
    }

    /// <summary>
    /// Seal flag in Wrap and GetMic token.
    /// </summary>
    public enum SEAL_ALG : ushort
    {
        /// <summary>
        /// DES encryption.
        /// </summary>
        DES = 0x0000,

        /// <summary>
        /// RC4 encryption.
        /// </summary>
        RC4 = 0x1000,

        /// <summary>
        /// No encryption.
        /// </summary>
        NONE = 0xffff,
    }

    /// <summary>
    /// AuthorizationData_element type
    /// this type value can be find in rfc 4120
    /// </summary>
    public enum AuthorizationData_elementType : int
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  AD-IF-RELEVANT is (1).
        /// </summary>
        AD_IF_RELEVANT = 1,
        /// <summary>
        /// The ad-type for AD-KDC-ISSUED is (4).
        /// </summary>
        AD_KDC_ISSUED = 4,
        /// <summary>
        /// The ad-type for AD-AND-OR is (5).
        /// </summary>
        AD_AND_OR = 5,
        /// <summary>
        /// The ad-type for AD-MANDATORY-FOR-KDC is (8).
        /// </summary>
        AD_MANDATORY_FOR_KDC = 8,
        /// <summary>
        /// The ad-type of AD-WIN2K-PAC
        /// </summary>
        AD_WIN2K_PAC = 128,
        /// <summary>
        /// type KERB_AUTH_DATA_TOKEN_RESTRICTIONS (141), containing the KERB-AD-RESTRICTION-ENTRY structure (
        /// </summary>
        KERB_AUTH_DATA_TOKEN_RESTRICTIONS = 141,
        /// <summary>
        /// The Authorization Data Type AD-AUTH-DATA-AP-OPTIONS has an ad-type 
        /// of 143 and ad-data of KERB_AP_OPTIONS_CBT (0x4000)
        /// </summary>
        AD_AUTH_DATA_AP_OPTIONS = 143,

        /// <summary>
        /// The Authorization Data Type AD-fx-fast-armor has an ad-type 
        /// of 71 and null ad-data.
        /// </summary>
        AD_FX_FAST_ARMOR = 71,

        /// <summary>
        /// The Authorization Data Type AD-fx-fast-used has an ad-type 
        /// of 72 and null ad-data.
        /// </summary>
        AD_FX_FAST_USED = 72
    }

    /// <summary>
    /// The principal type of Principal, can be found in rfc 4120
    /// http://www.ietf.org/rfc/rfc4120.txt
    /// </summary>
    public enum PrincipalType : int
    {
        /// <summary>
        /// Name type not known
        /// </summary>
        NT_UNKNOWN = 0,
        /// <summary>
        /// Just the name of the principal as in DCE,or for users
        /// </summary>
        NT_PRINCIPAL = 1,
        /// <summary>
        /// Service and other unique instance (krbtgt)
        /// </summary>
        NT_SRV_INST = 2,
        /// <summary>
        /// Service with host name as instance (telnet, rcommands)
        /// </summary>
        NT_SRV_HST = 3,
        /// <summary>
        ///  Service with host as remaining components
        /// </summary>
        NT_SRV_XHST = 4,
        /// <summary>
        /// Unique ID
        /// </summary>
        NT_UID = 5,
        /// <summary>
        ///  Encoded X.509 Distinguished name [RFC2253]
        /// </summary>
        NT_X500_PRINCIPAL = 6,
        /// <summary>
        /// Name in form of SMTP email name(e.g., user@example.com)
        /// </summary>
        NT_SMTP_NAME = 7,
        /// <summary>
        /// Enterprise name - may be mapped to principal name
        /// </summary>
        NT_ENTERPRISE = 10
    }

    /// <summary>
    /// The APOptions,can find in rfc 4120.
    /// http://www.ietf.org/rfc/rfc4120.txt
    /// </summary>
    [Flags]
    public enum ApOptions : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Reserved for future expansion of this field.
        /// </summary>
        Reserved = 0x80000000,

        /// <summary>
        /// The USE-SESSION-KEY option indicates that the ticket the client is presenting to a
        /// server is encrypted in the session key from the server's TGT.  When this option is not
        /// specified, the ticket is encrypted in the server's secret key.
        /// </summary>
        UseSessionKey = 0x40000000,

        /// <summary>
        /// The MUTUAL-REQUIRED option tells the server that the client requires mutual
        /// authentication, and that it must respond with a KRB_AP_REP message.
        /// </summary>
        MutualRequired = 0x20000000,
    }

    /// <summary>
    /// The PA PAC Options
    /// </summary>
    [Flags]
    public enum PacOptions : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Reserved for future expansion of this field.
        /// </summary>
        Claims = 0x80000000,

        /// <summary>
        /// The USE-SESSION-KEY option indicates that the ticket the client is presenting to a
        /// server is encrypted in the session key from the server's TGT.  When this option is not
        /// specified, the ticket is encrypted in the server's secret key.
        /// </summary>
        BranchAware = 0x40000000,

        /// <summary>
        /// The MUTUAL-REQUIRED option tells the server that the client requires mutual
        /// authentication, and that it must respond with a KRB_AP_REP message.
        /// </summary>
        ForwardToFullDc = 0x20000000,
    }

    /// <summary>
    /// The transport type.
    /// </summary>
    public enum TransportType
    {
        TCP,
        UDP,
    }

    /// <summary>
    /// The ip type.
    /// </summary>
    public enum IpType
    {
        Ipv4,
        Ipv6,
    }

    /// <summary>
    /// The checksum "Flags" field is used to convey service options or extension negotiation information.
    /// It is defined in [RFC 4121] section 4.1.1.1.
    /// </summary>
    [Flags]
    public enum ChecksumFlags : int
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// When GSS_C_DELEG_FLAG is set, the DlgOpt, Dlgth, and Deleg fields of the checksum data MUST 
        /// immediately follow the Flags field. 
        /// </summary>
        GSS_C_DELEG_FLAG = 0x00000001,

        /// <summary>
        /// MutualAuthentication.
        /// </summary>
        GSS_C_MUTUAL_FLAG = 0x00000002,

        /// <summary>
        /// ReplayDetect.
        /// </summary>
        GSS_C_REPLAY_FLAG = 0x00000004,

        /// <summary>
        /// SequenceDetect.
        /// </summary>
        GSS_C_SEQUENCE_FLAG = 0x00000008,

        /// <summary>
        /// Confidentiality.
        /// </summary>
        GSS_C_CONF_FLAG = 0x00000010,

        /// <summary>
        /// Integrity.
        /// </summary>
        GSS_C_INTEG_FLAG = 0x00000020,

        /// <summary>
        /// This flag was added for use with Microsoft's implementation of Distributed Computing Environment
        /// Remote Procedure Call (DCE RPC), which initially expected three legs of authentication. 
        /// </summary>
        GSS_C_DCE_STYLE = 0x00001000,

        /// <summary>
        /// This flag allows the client to indicate to the server that it should only allow the server application
        /// to identify the client by name and ID, but not to impersonate the client.
        /// </summary>
        GSS_C_IDENTIFY_FLAG = 0x00002000,

        /// <summary>
        /// Setting this flag indicates that the client wants to be informed of extended error information.
        /// </summary>
        GSS_C_EXTENDED_ERROR_FLAG = 0x00004000,
    }

    /// <summary>
    /// The address type ,can find in
    /// http://www.ietf.org/rfc/rfc4120.txt
    /// </summary>
    public enum AddressType : int
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// ipv4
        /// </summary>
        IPv4 = 2,

        /// <summary>
        /// Directional
        /// </summary>
        Directional = 3,

        /// <summary>
        ///CHAOSnet addresses
        /// </summary>
        ChaosNet = 5,

        /// <summary>
        /// Xerox Network Services (XNS) addresses
        /// </summary>
        XNS = 6,

        /// <summary>
        /// ISO addresses 
        /// </summary>
        ISO = 7,

        /// <summary>
        /// DECnet Phase IV addresses
        /// </summary>
        DECNET_Phase_IV = 12,

        /// <summary>
        /// AppleTalk Datagram Delivery Protocol (DDP) addresses
        /// </summary>
        AppleTalk_DDP = 16,

        /// <summary>
        /// net bios address
        /// </summary>
        NetBios = 20,

        /// <summary>
        /// ipv6
        /// </summary>
        IPv6 = 24,
    }

    /// <summary>
    /// Token ID in GSSAPI.
    /// </summary>
    public enum TOK_ID : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Mic token ID in [rfc1964] and [rfc4757].
        /// </summary>
        Mic1964_4757 = 0x0101,

        /// <summary>
        /// Wrap token ID in [rfc1964] and [rfc4757].
        /// </summary>
        Wrap1964_4757 = 0x0201,

        /// <summary>
        /// Mic token ID in [rfc4121].
        /// </summary>
        Mic4121 = 0x0404,

        /// <summary>
        /// Wrap token ID in [rfc4121].
        /// </summary>
        Wrap4121 = 0x0504,

        /// <summary>
        /// KRB_AP_REQ token ID.
        /// </summary>
        KRB_AP_REQ = 0x0100,

        /// <summary>
        /// KRB_AP_REP token ID.
        /// </summary>
        KRB_AP_REP = 0x0200,

        /// <summary>
        /// KRB_ERROR token ID.
        /// </summary>
        KRB_ERROR = 0x0300,
    }

    /// <summary>
    /// A 32-bit unsigned integer indicating the token information type.
    /// </summary>
    public enum LSAP_TOKEN_INFO_INTEGRITY_Flags : uint
    {
        /// <summary>
        /// Full token.
        /// </summary>
        FULL_TOKEN = 0x00000000,

        /// <summary>
        /// User Account Control (UAC) restricted token.
        /// </summary>
        UAC_TOKEN = 0x00000001,
    }

    /// <summary>
    /// A 32-bit unsigned integer indicating the integrity level of the calling process.
    /// </summary>
    public enum LSAP_TOKEN_INFO_INTEGRITY_TokenIL : uint
    {
        /// <summary>
        /// Untrusted.
        /// </summary>
        Untrusted = 0x00000000,

        /// <summary>
        /// Low.
        /// </summary>
        Low = 0x00001000,

        /// <summary>
        /// Medium.
        /// </summary>
        Medium = 0x00002000,

        /// <summary>
        /// High.
        /// </summary>
        High = 0x00003000,

        /// <summary>
        /// System.
        /// </summary>
        System = 0x00004000,

        /// <summary>
        /// Protected process.
        /// </summary>
        Protected = 0x00005000,
    }

    public enum KrbFastArmorType
    {
        FX_FAST_ARMOR_AP_REQUEST = 1
    }

    /// <summary>
    /// The data in the msDS-SupportedEncryptionTypes attribute([MS-ADA2] section 2.402).
    /// </summary>
    [Flags]
    public enum SupportedEncryptionTypes : uint
    {
        DES_CBC_CRC = 0x00000001,
        DES_CBC_MD5 = 0x00000002,
        RC4_HMAC = 0x00000004,
        AES128_CTS_HMAC_SHA1_96 = 0x00000008,
        AES256_CTS_HMAC_SHA1_96 = 0x00000010,

        FAST_Supported = 0x00010000,
        CompoundIdentity_Supported = 0x00020000,
        Claims_Supported = 0x00040000,
        ResSIDCompression_Disabled = 0x00080000
    }

    [Flags]
    public enum FastOptionFlags : uint
    {
        None = 0x00000000,
        Hide_Client_Names = 0x40000000
    }

    /// <summary>
    /// AP request authenticator checksum structure.
    /// </summary>
    public struct AuthCheckSum
    {
        public int Lgth;
        public byte[] Bnd; // 16 bytes
        public int Flags;
        public short DlgOpt;
        public short Dlgth;
        public byte[] Deleg;
        public byte[] Exts;
    }

    /// <summary>
    /// Principal
    /// </summary>
    public class Principal
    {
        /// <summary>
        /// Type of Principal
        /// </summary>
        public KerberosAccountType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Realm of this principal
        /// </summary>
        public Realm Realm
        {
            get;
            set;
        }

        /// <summary>
        /// Principal name
        /// </summary>
        public PrincipalName Name
        {
            get;
            set;
        }

        /// <summary>
        /// Password of principal
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Salt of principal
        /// </summary>
        public string Salt
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type of Principal</param>
        /// <param name="name">Principal name</param>
        /// <param name="password">Password of principal</param>
        public Principal(KerberosAccountType type, Realm realm, PrincipalName name, string password, string salt)
        {
            Type = type;
            Realm = realm;
            Name = name;
            Password = password;
            Salt = salt;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type of Principal</param>
        /// <param name="name">Principal name</param>
        /// <param name="password">Password of principal</param>
        /// <param name="salt">Salt of principal</param>
        public Principal(KerberosAccountType type, string realm, string name, string password, string salt)
        {
            Type = type;
            Realm = new Realm(realm);
            Name = new PrincipalName(new KerbInt32((int)PrincipalType.NT_PRINCIPAL), KerberosUtility.String2SeqKerbString(name));
            Password = password;
            Salt = salt;
        }
    }

    /// <summary>
    /// Kerberos ticket
    /// </summary>
    public class KerberosTicket
    {
        /// <summary>
        /// Ticket
        /// </summary>
        public Ticket Ticket
        {
            get;
            set;
        }

        /// <summary>
        /// Owner of ticket
        /// </summary>
        public PrincipalName TicketOwner
        {
            get;
            set;
        }

        /// <summary>
        /// Session key in ticket
        /// </summary>
        public EncryptionKey SessionKey
        {
            get;
            set;
        }

        /// <summary>
        /// Create a Kerberos ticket
        /// </summary>
        /// <param name="ticket">Ticket</param>
        /// <param name="ticketOwner">Owner of ticket</param>
        /// <param name="sessionKey">Session key in ticket</param>
        public KerberosTicket(Ticket ticket, PrincipalName ticketOwner, EncryptionKey sessionKey)
        {
            this.Ticket = ticket;
            this.TicketOwner = ticketOwner;
            this.SessionKey = sessionKey;
        }
    }

    /// <summary>
    /// Error types in KKDCP
    /// </summary>
    public enum KKDCPError
    {
        STATUS_SUCCESS,
        STATUS_AUTHENTICATION_FIREWALL_FAILED,
        STATUS_NO_LOGON_SERVERS
    }

    /// <summary>
    /// Error types for Kpassword
    /// </summary>
    public enum KpasswdError : ushort
    {
        /// <summary>
        /// request succeeds
        /// </summary>
        KRB5_KPASSWD_SUCCESS,

        /// <summary>
        /// request fails due to being malformed
        /// </summary>
        KRB5_KPASSWD_MALFORMED,

        /// <summary>
        /// request fails due to "hard" error in processing the request
        /// </summary>
        KRB5_KPASSWD_HARDERROR,

        /// <summary>
        /// request fails due to an error in authentication processing
        /// </summary>
        KRB5_KPASSWD_AUTHERROR,

        /// <summary>
        /// request fails due to a "soft" error in processing the request
        /// </summary>
        KRB5_KPASSWD_SOFTERROR,

        /// <summary>
        /// requestor not authorized
        /// </summary>
        KRB5_KPASSWD_ACCESSDENIED,

        /// <summary>
        /// protocol version unsupported, not used in windows
        /// </summary>
        KRB5_KPASSWD_BAD_VERSION,

        /// <summary>
        /// initial flag required, not used in windows
        /// </summary>
        KRB5_KPASSWD_INITIAL_FLAG_NEEDED,

        /// <summary>
        /// request fails for some other reason
        /// </summary>
        KRB5_KPASSWD_OTHER_ERRORS = 0xFFFF
    }
}
