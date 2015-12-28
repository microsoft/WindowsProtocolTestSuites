// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KerberosFastResponse
    {
        public KrbFastResponse FastResponse
        {
            get;
            set;
        }

        public Asn1SequenceOf<PA_DATA> SeqPaData
        {
            get
            {
                return FastResponse.padata;
            }
        }

        public IPaData[] PaData { get; private set; }

        public KerberosFastResponse(Asn1SequenceOf<PA_DATA> seqPaData, EncryptionKey strengthenKey, KrbFastFinished finished, long nouce)
        {
            this.FastResponse = new KrbFastResponse(seqPaData, strengthenKey, finished, new KerbUInt32(nouce));
        }

        public KerberosFastResponse(KrbFastResponse fastResponse)
        {
            FastResponse = fastResponse;
            int len = fastResponse.padata.Elements.Length;
            PaData = new IPaData[len];
            for (int i = 0; i < len; i++)
            {
                PaData[i] = PaDataParser.ParseRepPaData(fastResponse.padata.Elements[i]);
            }
        }

    }
}
