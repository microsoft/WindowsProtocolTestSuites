// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Linq;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    Segmentation ::= BIT STRING
    {
    begin (0),
    end (1)
    } (SIZE (2))
    */
    public class Segmentation : Asn1Object 
    {
        public static readonly int begin = 0;
        public static readonly int end = 1;

        public bool[] Value;

        public Segmentation()
        {
            Value = new bool[0];
        }

        public Segmentation(bool[] data)
        {
            Value = data;
        }

        protected override bool VerifyConstraints()
        {
            return Value.Length == 2;
        }

        public override void PerEncode(IAsn1PerEncodingBuffer buffer)
        {
            buffer.WriteBit(Value[0]);
            buffer.WriteBit(Value[1]);
        }

        public override void PerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            Value = buffer.ReadBits(2);
        }

        public override bool Equals(object obj)
        {
            // If parameter is null or cannot be cast to Asn1Integer return false.
            Segmentation p = obj as Segmentation;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match.
            return this.Value.SequenceEqual(p.Value);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
