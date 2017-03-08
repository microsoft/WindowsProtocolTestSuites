// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace Microsoft.Protocol.TestSuites.ADOD.Adapter.Util
{
    public class PingClient
    {
        private Ping pingSender;
        private string hostNameOrAddress;
        
        public PingClient(string hostNameOrAddr)
        {
            pingSender = new Ping();
            hostNameOrAddress = hostNameOrAddr;
        }

        public bool PingSuccess()
        {
            PingReply reply = pingSender.Send(hostNameOrAddress);
            if (reply.Status == IPStatus.Success) { return true; }
            else { return false; }
        }

        public bool PingFailure()
        {
            return !PingSuccess();
        }
    }
}
