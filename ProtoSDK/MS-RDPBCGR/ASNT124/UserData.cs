// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*UserData ::= SET OF SEQUENCE
    {
        key Key,
        value OCTET STRING OPTIONAL
    }
    */
    public class UserData : Asn1SetOf<UserDataElement>
    {
        public UserData()
            : base()
        {
            this.Elements = null;
        }

        public UserData(UserDataElement[] val)
            : base(val)
        {
        }
    }
   
    public class UserDataElement : Asn1Sequence
    {
        [Asn1Field(0)]
        public Key key { get; set; }
        
        [Asn1Field(1, Optional = true)]
        public Asn1OctetString value { get; set; }
        
        public UserDataElement()
        {
            this.key = null;
            this.value = null;
        }
        
        public UserDataElement(
         Key key,
         Asn1OctetString value)
        {
            this.key = key;
            this.value = value;
        }
    }
}

