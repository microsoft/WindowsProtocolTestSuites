// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    Control ::= SEQUENCE {
                controlType             LDAPOID,
                criticality             BOOLEAN DEFAULT FALSE,
                controlValue            OCTET STRING OPTIONAL }
    */
    public class Control : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPOID controlType { get; set; }

        [Asn1Field(1, Optional = true)]
        public Asn1Boolean criticality { get; set; }

        [Asn1Field(2)]
        public Asn1OctetString controlValue { get; set; }

        public Control()
        {
            this.controlType = null;
            this.criticality = null;
            this.controlValue = null;
        }

        public Control(
         LDAPOID controlType,
         Asn1Boolean criticality,
         Asn1OctetString controlValue)
        {
            this.controlType = controlType;
            this.criticality = criticality;
            this.controlValue = controlValue;
        }
    }
}

