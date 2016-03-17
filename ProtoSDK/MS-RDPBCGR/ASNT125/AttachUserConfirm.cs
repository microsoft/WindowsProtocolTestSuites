// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    AttachUserConfirm ::= [APPLICATION 11] IMPLICIT SEQUENCE
    {
        result Result,
        initiator UserId OPTIONAL
    }
    */
    [Asn1Tag(Asn1TagType.Application, 11)]
    public class AttachUserConfirm : Asn1Sequence
    {
        [Asn1Field(0)]
        public Result result { get; set; }
        
        [Asn1Field(1, Optional = true)]
        public UserId initiator { get; set; }
        
        public AttachUserConfirm()
        {
            this.result = null;
            this.initiator = null;
        }
        
        public AttachUserConfirm(
         Result result,
         UserId initiator)
        {
            this.result = result;
            this.initiator = initiator;
        }
    }
}

