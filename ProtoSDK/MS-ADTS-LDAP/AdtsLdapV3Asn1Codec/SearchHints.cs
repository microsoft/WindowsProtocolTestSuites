// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{

    public class SearchHints : Asn1SequenceOf<SearchHints_element>
    {
        public SearchHints()
            : base()
        {
            this.Elements = null;
        }

        public SearchHints(int numRecords)
            : base()
        {
            this.Elements = new SearchHints_element[numRecords];
        }

        public SearchHints(SearchHints_element[] val)
            : base(val)
        {
        }
    }
}
