// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPMSetBindingsOut message contains a response to a CPMSetBindingsIn message.
    /// </summary>
    public class CPMSetBindingsOut : IWspOutMessage
    {
        public IWspInMessage Request { get; set; }

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            var header = new WspMessageHeader();

            header.FromBytes(buffer);

            Header = header;
        }

        public void ToBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
