// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    BindRequest ::= [APPLICATION 0] SEQUENCE {
                version                 INTEGER (1 .. 127),
                name                    LDAPDN,
                authentication          AuthenticationChoice }
    */
    [Asn1Tag(Asn1TagType.Application, 0)]
    public class BindRequest : Asn1Sequence
    {
        [Asn1Field(0), Asn1IntegerBound(Min = 1, Max =127)]
        public Asn1Integer version { get; set; }
        
        [Asn1Field(1)]
        public LDAPDN name { get; set; }
        
        [Asn1Field(2)]
        public AuthenticationChoice authentication { get; set; }
        
        public BindRequest()
        {
            this.version = null;
            this.name = null;
            this.authentication = null;
        }
        
        public BindRequest(
         Asn1Integer version,
         LDAPDN name,
         AuthenticationChoice authentication)
        {
            this.version = version;
            this.name = name;
            this.authentication = authentication;
        }

        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            int allLen = 0;
            allLen += BerEncodeWithoutUnisersalTag(buffer);
            allLen += TagBerEncode(buffer, new Asn1Tag(Asn1TagType.Application, 0));
            return allLen;
        }

        public override int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag = true)
        {
            int headLen = 0;
            Asn1Tag appTag;
            headLen += TagBerDecode(buffer, out appTag);
            int valueLen;
            headLen += LengthBerDecode(buffer, out valueLen);

            int valueLenDecode = 0;
            version = new Asn1Integer(); 
            valueLenDecode += version.BerDecode(buffer);
            name = new LDAPDN(); 
            valueLenDecode += name.BerDecode(buffer);
            authentication = new AuthenticationChoice(); 
            valueLenDecode += authentication.BerDecode(buffer);
            if (valueLen != valueLenDecode)
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " LDAPResult.");
            }
            return headLen + valueLen;
        }
    }
}

