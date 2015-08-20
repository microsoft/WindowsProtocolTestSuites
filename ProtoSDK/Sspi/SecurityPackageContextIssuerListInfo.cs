// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// The CRYPT_INTEGER_BLOB structure contains an arbitrary array of bytes. The structure definition includes 
    /// aliases appropriate to the various functions that use it.
    /// http://msdn.microsoft.com/en-us/library/aa381414(v=VS.85).aspx
    /// </summary>
    public struct SecurityPackageContextIssuerListInfo
    {
        /// <summary>
        /// IssuerList data.
        /// </summary>
        public byte[][] Issuers;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="issuerList">issuer list</param>
        internal SecurityPackageContextIssuerListInfo(SspiSecurityPackageContextIssuerListInfo issuerList)
        {
            List<byte[]> issuers = new List<byte[]>();

            for (int i = 0; i < issuerList.cIssuers; i++)
            {
                CryptoApiBlob blob = (CryptoApiBlob)Marshal.PtrToStructure(issuerList.aIssuers, typeof(CryptoApiBlob));

                issuers.Add(blob.GetData());
            }
            this.Issuers = issuers.ToArray();
        }
    }
}
