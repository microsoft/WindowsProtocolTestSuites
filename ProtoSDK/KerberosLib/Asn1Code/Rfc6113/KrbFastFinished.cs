// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth 
{
    /*
    KrbFastFinished ::= SEQUENCE {
          timestamp       [0] KerberosTime,
          usec            [1] Microseconds,
              -- timestamp and usec represent the time on the KDC when
              -- the reply was generated.
          crealm          [2] Realm,
          cname           [3] PrincipalName,
              -- Contains the client realm and the client name.
          ticket-checksum [4] Checksum,
              -- checksum of the ticket in the KDC-REP  using the armor
              -- and the key usage is KEY_USAGE_FAST_FINISH.
              -- The checksum type is the required checksum type
              -- of the armor key.
          ...
      }
    */
    public class KrbFastFinished : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerberosTime timestamp { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Microseconds usec { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public Realm crealm { get; set; }
        
        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 3)]
        public PrincipalName cname { get; set; }
        
        [Asn1Field(4), Asn1Tag(Asn1TagType.Context, 4)]
        public Checksum ticket_checksum { get; set; }
        
        public KrbFastFinished()
        {
            this.timestamp = null;
            this.usec = null;
            this.crealm = null;
            this.cname = null;
            this.ticket_checksum = null;
        }
        
        public KrbFastFinished(
         KerberosTime param0,
         Microseconds param1,
         Realm param2,
         PrincipalName param3,
         Checksum param4)
        {
            this.timestamp = param0;
            this.usec = param1;
            this.crealm = param2;
            this.cname = param3;
            this.ticket_checksum = param4;
        }
    }
}

