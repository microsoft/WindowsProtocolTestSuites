// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp
{
    public class CongestionControl
    {
        /// <summary>
        /// This method is used to update congestion window
        /// There is a space in the receiver-advertised window for this datagram and the Congestion Control logic permits transmission of a datagram.
        /// </summary>
        /// <param name="windowSize"></param>
        /// <param name="congestionOccured"></param>
        public void UpdateCongestionWindowSize(ref ushort windowSize, bool congestionOccured)
        {
            // TODO: Should Implement a real congrestion algorithm
            // Currently, no change for windowSize;
        }
    }
}
