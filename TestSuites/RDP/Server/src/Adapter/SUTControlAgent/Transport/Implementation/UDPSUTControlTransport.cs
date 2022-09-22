// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.Protocols.TestSuites.Rdp.SUTControlAgent.Message;

namespace Microsoft.Protocols.TestSuites.Rdp.SUTControlAgent.Transport
{
    /// <summary>
    /// UDP transport
    /// </summary>
    public class UDPSUTControlTransport : ISUTControltransport
    {
        IPEndPoint remoteEP;
        UdpClient client = null;

        /// <summary>
        /// Connect to the server
        /// </summary>
        /// <param name="timeout">Time out</param>
        /// <param name="remoteEP">RemoteEP</param>
        /// <returns></returns>
        public bool Connect(TimeSpan timeout, IPEndPoint remoteEP)
        {
            this.remoteEP = remoteEP;
            DateTime End = DateTime.Now + timeout;

            do
            {
                try
                {
                    client = new UdpClient();
                    client.Connect(remoteEP);
                    return true;
                }
                catch (Exception)
                {
                }
            }
            while (DateTime.Now < End);
            return false;
        }

        /// <summary>
        /// Disconnect
        /// </summary>
        public void Disconnect()
        {

            if (client != null)
            {
                client.Close();
                client = null;
            }
        }

        /// <summary>
        /// Send SUT Control Request Message
        /// </summary>
        /// <param name="sutControlRequest">Request message</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool SendSUTControlRequestMessage(SUTControlRequestMessage sutControlRequest)
        {
            byte[] rawData = sutControlRequest.Encode();
            try
            {
                client.Send(rawData, rawData.Length);
                return true;
            }
            catch
            {

            }
            return false;
        }


        /// <summary>
        /// Expect a SUT control response message
        /// </summary>
        /// <param name="timeout">Time out</param>
        /// <param name="requestId">RequestId should be in the response message</param>
        /// <returns>SUT Control response message</returns>
        public SUTControlResponseMessage ExpectSUTControlResponseMessage(TimeSpan timeout, ushort requestId)
        {
            DateTime End = DateTime.Now + timeout;
            SUTControlResponseMessage responseMessage = new SUTControlResponseMessage();
            while (DateTime.Now < End)
            {
                IPEndPoint endpointReceived = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = client.Receive(ref endpointReceived);

                if (buffer != null &&
                    remoteEP.Equals(endpointReceived))
                {
                    int index = 0;
                    if (responseMessage.Decode(buffer, ref index))
                    {
                        if (responseMessage.requestId == requestId)
                        {
                            return responseMessage;
                        }
                    }
                }

                System.Threading.Thread.Sleep(100);
            }
            return null;
        }
    }
}
