using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib
{
    public class Consts
    {
        internal const int MAX_TOKEN_SIZE = 12288;

        internal const uint SEC_E_OK = 0;
        internal const uint SEC_I_CONTINUE_NEEDED = 0x00090312;
        internal const uint SEC_I_COMPLETE_NEEDED = 0x00090313;
        internal const uint SEC_I_COMPLETE_AND_CONTINUE = 0x00090314;
        internal const uint SEC_I_MESSAGE_FRAGMENT = 0x00090364;

        internal const uint SECPKG_CRED_INBOUND = 1;
        internal const uint SECPKG_CRED_OUTBOUND = 2;

        // Security Context Attributes: defined in sspi.h
        // reference: http://msdn.microsoft.com/en-us/library/aa379326(VS.85).aspx
        internal const uint SECPKG_ATTR_SIZES = 0;
        internal const uint SECPKG_ATTR_NAMES = 1;
        internal const uint SECPKG_ATTR_LIFESPAN = 2;
        internal const uint SECPKG_ATTR_DCE_INFO = 3;
        internal const uint SECPKG_ATTR_STREAM_SIZES = 4;
        internal const uint SECPKG_ATTR_KEY_INFO = 5;
        internal const uint SECPKG_ATTR_AUTHORITY = 6;
        internal const uint SECPKG_ATTR_PROTO_INFO = 7;
        internal const uint SECPKG_ATTR_PASSWORD_EXPIRY = 8;
        internal const uint SECPKG_ATTR_SESSION_KEY = 9;
        internal const uint SECPKG_ATTR_PACKAGE_INFO = 10;
        internal const uint SECPKG_ATTR_USER_FLAGS = 11;
        internal const uint SECPKG_ATTR_NEGOTIATION_INFO = 12;
        internal const uint SECPKG_ATTR_NATIVE_NAMES = 13;
        internal const uint SECPKG_ATTR_FLAGS = 14;
        // These attributes exist only in Win XP and greater
        internal const uint SECPKG_ATTR_USE_VALIDATED = 15;
        internal const uint SECPKG_ATTR_CREDENTIAL_NAME = 16;
        internal const uint SECPKG_ATTR_TARGET_INFORMATION = 17;
        internal const uint SECPKG_ATTR_ACCESS_TOKEN = 18;
        // These attributes exist only in Win2K3 and greater
        internal const uint SECPKG_ATTR_TARGET = 19;
        internal const uint SECPKG_ATTR_AUTHENTICATION_ID = 20;
        // These attributes exist only in Win2K3SP1 and greater
        internal const uint SECPKG_ATTR_LOGOFF_TIME = 21;
        //Defined in schannel.h
        internal const uint SECPKG_ATTR_REMOTE_CERT_CONTEXT = 0x53;   // returns PCCERT_CONTEXT
        internal const uint SECPKG_ATTR_LOCAL_CERT_CONTEXT = 0x54;   // returns PCCERT_CONTEXT
        internal const uint SECPKG_ATTR_ROOT_STORE = 0x55;   // returns HCERTCONTEXT to the root store
        internal const uint SECPKG_ATTR_ISSUER_LIST_EX = 0x59;   // returns SecPkgContext_IssuerListInfoEx
        internal const uint SECPKG_ATTR_CONNECTION_INFO = 0x5a;   // returns SecPkgContext_ConnectionInfo
        internal const uint SECPKG_ATTR_EAP_KEY_BLOCK = 0x5b;   // returns SecPkgContext_EapKeyBlock
        internal const uint SECPKG_ATTR_APP_DATA = 0x5e;  // sets/returns SecPkgContext_SessionAppData

        // Specifies the version number of SecBufferDesc.
        internal const int SECBUFFER_VERSION = 0;
        //Schannel version
        internal const uint SCHANNEL_CRED_VERSION = 0x00000004;

        //identifiers for schannel, defined in Schannel.h please see 
        //http://msdn.microsoft.com/en-us/library/aa379810(VS.85).aspx
        //Private Communications Technology 1.0 server side. (Obsolete.)
        internal const uint SP_PROT_PCT1_SERVER = 0x00000001;
        //Private Communications Technology 1.0 client side. (Obsolete.)
        internal const uint SP_PROT_PCT1_CLIENT = 0x00000002;
        //Secure Sockets Layer 2.0 server side. Superseded by SP_PROT_TLS1_SERVER.
        internal const uint SP_PROT_SSL2_SERVER = 0x00000004;
        //Secure Sockets Layer 2.0 client side. Superseded by SP_PROT_TLS1_CLIENT.
        internal const uint SP_PROT_SSL2_CLIENT = 0x00000008;
        //Secure Sockets Layer 3.0 server side.
        internal const uint SP_PROT_SSL3_SERVER = 0x00000010;
        //Secure Sockets Layer 3.0 client side.
        internal const uint SP_PROT_SSL3_CLIENT = 0x00000020;
        //Transport Layer Security 1.0 server side.
        internal const uint SP_PROT_TLS1_SERVER = 0x00000040;
        //Transport Layer Security 1.0 client side.
        internal const uint SP_PROT_TLS1_CLIENT = 0x00000080;
        internal const uint SP_PROT_UNI_SERVER = 0x40000000;
        internal const uint SP_PROT_UNI_CLIENT = 0x80000000;
        internal const uint SP_PROT_CLIENTS = SP_PROT_PCT1_CLIENT
            | SP_PROT_SSL2_CLIENT
            | SP_PROT_SSL3_CLIENT
            | SP_PROT_UNI_CLIENT
            | SP_PROT_TLS1_CLIENT;
        internal const uint SP_PROT_SERVERS = SP_PROT_PCT1_SERVER
            | SP_PROT_SSL2_SERVER
            | SP_PROT_SSL3_SERVER
            | SP_PROT_UNI_SERVER
            | SP_PROT_TLS1_SERVER;
        //DTLS 1.0 server side
        internal const uint SP_PROT_DTLS_SERVER = 0x00010000;
        //DTLS 1.0 client side
        internal const uint SP_PROT_DTLS_CLIENT = 0x00020000;

        //dwFlag values of Schannel_Cred.
        internal const uint SCH_CRED_MANUAL_CRED_VALIDATION = 0x00000008;
        internal const uint SCH_CRED_NO_DEFAULT_CREDS = 0x00000010;
        //Identity of Unicode in SEC_WINNT_AUTH_IDENTITY.
        internal const int SEC_WINNT_AUTH_IDENTITY_UNICODE = 0x2;
    }
}
