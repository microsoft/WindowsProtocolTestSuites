// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using LdapV2 = Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2;
using LdapV3 = Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    public class AdtsDelRequestPacket : AdtsLdapPacket
    {
        /// <summary>
        /// Gets inner request or response that's contained in the ldap message.
        /// </summary>
        /// <returns>The inner request or response.</returns>
        public override Asn1Object GetInnerRequestOrResponse()
        {
            if (this.ldapMessagev2 != null)
            {
                return (LdapV2.DelRequest)this.ldapMessagev2.protocolOp.GetData();
            }
            else
            {
                return (LdapV3.DelRequest)this.ldapMessagev3.protocolOp.GetData();
            }
        }
    }
}