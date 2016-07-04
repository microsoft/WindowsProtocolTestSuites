// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc
{
    /*
    Key ::= CHOICE -- Identifier of a standard or non-standard object
    {
        object OBJECT IDENTIFIER,
        h221NonStandard H221NonStandardIdentifier
    }
    */
    public class Key : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long obj = 0;
        [Asn1ChoiceElement(obj)]
        protected Asn1ObjectIdentifier field0 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long h221NonStandard = 1;
        [Asn1ChoiceElement(h221NonStandard)]
        protected H221NonStandardIdentifier field1 { get; set; }
        
        public Key()
            : base()
        {
        }
        
        public Key(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }
    }
}

