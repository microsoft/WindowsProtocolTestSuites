// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

package rdpsutcontrol.agent;

public class App
{
    public static void main(String[] args)
    {
        try
        {
            Listener listenThread = new Listener(4488);
            listenThread.start();
            System.out.println("RDPSUTControl Listener started!");
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }
}
