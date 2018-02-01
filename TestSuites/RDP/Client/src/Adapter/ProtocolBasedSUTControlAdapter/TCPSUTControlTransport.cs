// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    /// <summary>
    /// TCP transport
    /// </summary>
    public class TCPSUTControlTransport : ISUTControltransport
    {
        TcpClient client = null;
        NetworkStream netDataStream = null;

        /// <summary>
        /// Connect to the server
        /// </summary>
        /// <param name="timeout">Time out</param>
        /// <param name="remoteEP">RemoteEP</param>
        /// <returns></returns>
        public bool Connect(TimeSpan timeout, IPEndPoint remoteEP)
        {
            DateTime End = DateTime.Now + timeout;

            do
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(remoteEP);
                    netDataStream = client.GetStream();
                    return true;
                }
                catch(Exception)
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
            if (netDataStream != null)
            {
                netDataStream.Close();
                netDataStream = null;
            }

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
        public bool SendSUTControlRequestMessage(SUT_Control_Request_Message sutControlRequest)
        {
            byte[] rawData = sutControlRequest.Encode();
            try
            {
                netDataStream.Write(rawData, 0, rawData.Length);
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
        public SUT_Control_Response_Message ExpectSUTControlResponseMessage(TimeSpan timeout, ushort requestId)
        {
            List<byte> receivedDataList = new List<byte>();
            int bufferLength = 2000;
            DateTime End = DateTime.Now + timeout;
            SUT_Control_Response_Message responseMessage = new SUT_Control_Response_Message();
            while (DateTime.Now < End)
            {
                byte[] buffer = new byte[bufferLength];

                int readbyteNumber = 0;
                do
                {
                    readbyteNumber = netDataStream.Read(buffer, 0, bufferLength);
                    if (readbyteNumber > 0)
                    {
                        byte[] tmpbuffer = new byte[readbyteNumber];
                        Array.Copy(buffer, tmpbuffer, readbyteNumber);
                        receivedDataList.AddRange(tmpbuffer);
                    }
                }
                while (readbyteNumber == bufferLength);

                int index = 0;
                if (responseMessage.Decode(receivedDataList.ToArray(), ref index))
                {
                    if (responseMessage.requestId == requestId)
                    {
                        return responseMessage;
                    }
                    else
                    {                        
                        receivedDataList.RemoveRange(0, index);
                    }
                }

                System.Threading.Thread.Sleep(100);
            }
            return null;
        }
    }
}
