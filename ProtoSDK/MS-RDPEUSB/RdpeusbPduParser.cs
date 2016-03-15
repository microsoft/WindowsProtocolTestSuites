// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb
{
    /// <summary>
    /// This abstract class is used as the interface of the parser of the MS-RDPEUSB data PDUs.
    /// </summary>
    public abstract class RdpeusbPduParser
    {
        public virtual EusbPdu ParsePdu(byte[] data)
        {
            EusbPdu pdu = new EusbUnknownPdu();
            PduMarshaler.Unmarshal(data, pdu);
            return pdu;
        }
    }    

    
}
