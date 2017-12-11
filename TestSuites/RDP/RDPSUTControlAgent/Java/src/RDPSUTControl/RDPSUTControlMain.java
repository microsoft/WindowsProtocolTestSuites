// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

package RDPSUTControl;

public class RDPSUTControlMain {

	public static void main(String[] args) {
		try
		{
			RDPSUTControlListener listenThread = new RDPSUTControlListener(4488);
			listenThread.start();
			System.out.println("RDPSUTControl Listener started!");
		}
		catch(Exception e)
		{
		
		}
				

	}

}
