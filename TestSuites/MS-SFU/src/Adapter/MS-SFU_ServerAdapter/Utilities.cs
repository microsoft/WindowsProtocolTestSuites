// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using System;
using System.Linq;

namespace Microsoft.Protocols.TestSuites.SfuProtocol.Adapter
{
    /// <summary>
    /// MS-SFU utilities.
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// Parse _RPC_UNICODE_STRING to string.
        /// </summary>
        /// <param name="rpcUnicodeString">_RPC_UNICODE_STRING instance.</param>
        /// <returns>String value of input.</returns>
        public static string Parse_RPC_UnicodeString(_RPC_UNICODE_STRING rpcUnicodeString)
        {
            return String.Concat(rpcUnicodeString.Buffer.Take(rpcUnicodeString.Length).Select(code => Char.ConvertFromUtf32(code)).ToArray());
        }

        /// <summary>
        /// Get AdWin2KPac from KerberosTgsResponse.
        /// </summary>
        /// <param name="tgsRsp">The KerberosTgsResponse where to get AdWin2KPac.</param>
        /// <returns>AdWin2KPac instance.</returns>
        public static AdWin2KPac GetAdWin2KPac(KerberosTgsResponse tgsRsp)
        {
            var elements = tgsRsp.TicketEncPart.authorization_data.Elements;

            var result = elements
                            .Where(element => element.ad_type.Value == (long)AuthorizationData_elementType.AD_IF_RELEVANT)
                            .Select(element => AdIfRelevent.Parse(element))
                            .SelectMany(element =>
                            {
                                return element.Elements
                                                .Where(innerElement => innerElement is AdWin2KPac)
                                                .Select(innerElement => innerElement as AdWin2KPac);
                            });

            return result.First();
        }

        /// <summary>
        /// Get the extended status of KerberosKrbError.
        /// </summary>
        /// <param name="krbErr">The KerberosKrbError where to get extended status.</param>
        /// <returns>The extended status if available.</returns>
        public static uint? GetExtendedStatus(KerberosKrbError krbErr)
        {
            var buffer = new Asn1DecodingBuffer(krbErr.KrbError.e_data.ByteArrayValue);

            var eData = new KERB_ERROR_DATA();

            eData.BerDecode(buffer);

            if (eData.data_type.Value != (long)KERB_ERR_TYPE.KERB_ERR_TYPE_EXTENDED)
            {
                return null;
            }

            var extErr = TypeMarshal.ToStruct<KERB_EXT_ERROR>(eData.data_value.ByteArrayValue);

            return extErr.status;
        }
    }
}
