// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    public class ClientClaimsInfo : PacInfoBuffer
    {
        public PAC_CLIENT_CLAIMS_INFO NativeClaimsSetMetadata;
        public CLAIMS_SET NativeClaimSet;
        internal override void DecodeBuffer(byte[] buffer, int index, int count)
        {
            NativeClaimsSetMetadata = PacUtility.NdrUnmarshal<PAC_CLIENT_CLAIMS_INFO>(
                buffer,
                index,
                count,
                FormatString.OffsetClientClaim, false, 4);

            byte[] decompressed = null;
            int decompressedLen = -1;
            if (NativeClaimsSetMetadata.Claims.usCompressionFormat != CLAIMS_COMPRESSION_FORMAT.COMPRESSION_FORMAT_NONE)
            {
                uint err = ClaimsCompression.Decompress(NativeClaimsSetMetadata.Claims.usCompressionFormat,
                    NativeClaimsSetMetadata.Claims.ClaimsSet,
                    (int)NativeClaimsSetMetadata.Claims.ulUncompressedClaimsSetSize,
                    out decompressed);
                if (err != 0)
                {
                    throw new Exception("Failed to decompress CLAIMS_SET data, error code is :" + err);
                }
                decompressedLen = decompressed.Length;
            }
            else
            {
                decompressed = NativeClaimsSetMetadata.Claims.ClaimsSet;
                decompressedLen = (int)NativeClaimsSetMetadata.Claims.ulClaimsSetSize;
            }
            NativeClaimSet = PacUtility.NdrUnmarshal<CLAIMS_SET>(
                decompressed,
                0,
                decompressedLen,
                FormatString.OffsetClaimSet, false, 4);

        }

        internal override byte[] EncodeBuffer()
        {
            using (SafeIntPtr ptr = TypeMarshal.ToIntPtr(NativeClaimsSetMetadata))
            {
                return PacUtility.NdrMarshal(ptr, FormatString.OffsetClientClaim);
            }
        }

        internal override int CalculateSize()
        {
            return EncodeBuffer().Length;
        }

        internal override PAC_INFO_BUFFER_Type_Values GetBufferInfoType()
        {
            return PAC_INFO_BUFFER_Type_Values.ClientClaimsInformation;
        }
    }
}
