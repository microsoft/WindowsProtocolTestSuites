// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib
{
    public class Consts
    {
        public const int MAX_TOKEN_SIZE = 12288;

        //Schannel version
        public const uint SCHANNEL_CRED_VERSION = 0x00000004;

        //dwFlag values of Schannel_Cred.
        public const uint SCH_CRED_MANUAL_CRED_VALIDATION = 0x00000008;
        public const uint SCH_CRED_NO_DEFAULT_CREDS = 0x00000010;
        //Identity of Unicode in SEC_WINNT_AUTH_IDENTITY.
        public const int SEC_WINNT_AUTH_IDENTITY_UNICODE = 0x2;

        // Specifies the version number of SecBufferDesc.
        public const int SECBUFFER_VERSION = 0;

        /// <summary>
        /// Unknown OID Int Array
        /// </summary>
        public static int[] UnknownOidInt = { 1, 2, 841, 48018, 1, 2, 2 };

        /// <summary>
        /// MsKerberos OID Int Array
        /// </summary>
        public static int[] MsKerbOidInt = { 1, 2, 840, 48018, 1, 2, 2 };

        /// <summary>
        /// Kerberos OID Int Array
        /// </summary>
        public static int[] KerbOidInt = { 1, 2, 840, 113554, 1, 2, 2 };

        /// <summary>
        /// Nlmp OID Int Array
        /// </summary>
        public static int[] NlmpOidInt = { 1, 3, 6, 1, 4, 1, 311, 2, 2, 10 };

        /// <summary>
        /// NegoEx OID Int Array
        /// </summary>
        public static int[] NegoExOidInt = { 1, 3, 6, 1, 4, 1, 311, 2, 2, 30 };

        /// <summary>
        /// SPNG OID Int Array
        /// </summary>
        public static int[] SpngOidInt = { 1, 3, 6, 1, 5, 5, 2 };
    }
}
