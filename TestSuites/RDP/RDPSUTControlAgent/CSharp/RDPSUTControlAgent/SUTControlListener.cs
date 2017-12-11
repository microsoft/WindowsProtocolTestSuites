// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RDPSUTControlAgent
{
    public class SUTControlListener
    {
        int bufferLength = 2000;
        ushort listenPort = 4488;
        TcpListener listenServer;
        Thread processThread;
        bool processThreadRuning;
        TimeSpan timeout = new TimeSpan(0, 0, 10);
        public SUTControlListener()
        {
            listenServer = new TcpListener(GetLocalIP(), listenPort);
            processThread = new Thread(new ThreadStart(ProcessControlCommandThread));
        }

        public void Start()
        {
            listenServer.Start();

            processThreadRuning = true;
            processThread.Start();
        }

        public void Stop()
        {
            processThreadRuning = false;
            
            listenServer.Stop();
        }


        private IPAddress GetLocalIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }

            return null;
        }

        private void ProcessControlCommandThread()
        {
            while (processThreadRuning)
            {
                try
                {
                    if (!listenServer.Pending())
                    {
                        Thread.Sleep(500);
                        continue;
                    }
                    TcpClient client = listenServer.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    SUT_Control_Request_Message requestMessage = new SUT_Control_Request_Message();
                    List<byte> receivedDataList = new List<byte>();
                    DateTime end = DateTime.Now + timeout;
                    byte[] buffer = new byte[bufferLength];
                    while (DateTime.Now < end)
                    {
                        
                        int readbyteNumber = 0;
                        do
                        {
                            readbyteNumber = stream.Read(buffer, 0, bufferLength);
                            if (readbyteNumber > 0)
                            {
                                byte[] tmpbuffer = new byte[readbyteNumber];
                                Array.Copy(buffer, tmpbuffer, readbyteNumber);
                                receivedDataList.AddRange(tmpbuffer);
                            }
                        }
                        while (readbyteNumber == bufferLength);
                        int index=0;
                        if (requestMessage.Decode(receivedDataList.ToArray(), ref index))
                        {
                            Console.WriteLine("Receive command: " + (RDPSUTControl_CommandId)requestMessage.commandId);
                            SUT_Control_Response_Message response= RDPClientControl.ProcessCommand(requestMessage);
                            byte[] sendData = response.Encode();
                            stream.Write(sendData, 0, sendData.Length);
                            break;
                        }
                    }

                    stream.Close();
                    client.Close();
                    // Run post operation if it is existed. used for auto-reconnect
                    if (RDPClientControl.PostOperation != null)
                    {
                        RDPClientControl.PostOperation();
                        RDPClientControl.PostOperation = null;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Exception found during listening:"+e.Message);
                }
            }
        }
    }
}
