// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

package rdpsutcontrol.agent;

import rdpsutcontrol.message.*;

import java.net.*;
import java.util.Properties;
import java.io.*;

public class Listener extends Thread
{
    public boolean ThreadRunning = true;
    public static int bufferLength = 20000;
    private ServerSocket serverSocket;
    private SUTControl sutControl;

    public Listener(int port, Properties config) throws IOException
    {
        sutControl = new SUTControl(config);
        serverSocket = new ServerSocket(port);
        serverSocket.setSoTimeout(10000);
        ThreadRunning = true;
    }

    public void run()
    {
        while(ThreadRunning)
        {
            try
            {
                Socket server = serverSocket.accept();

                DataInputStream in = new DataInputStream(server.getInputStream());

                byte[] buffer = new byte[bufferLength];
                in.read(buffer);

                SUT_Control_Request_Message request = new SUT_Control_Request_Message();
                request.Decode(buffer);

                SUT_Control_Response_Message response = sutControl.ProcessCommand(request);
                DataOutputStream out = new DataOutputStream(server.getOutputStream());
                out.write(response.Encode());

                server.close();
            }
            catch(SocketTimeoutException s)
            {
                // Time out when accept a socket, continue!
            }
            catch(IOException e)
            {
                e.printStackTrace();
                break;
            }
        }
    }
}
