// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

package RDPSUTControl;
import java.io.File;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.awt.*;
import java.awt.image.BufferedImage;

import javax.imageio.ImageIO;

import RDPSUTControlMessage.*;

public class SUTControl {
	public static SUT_Control_Response_Message ProcessCommand(SUT_Control_Request_Message request)
	{
		int resultCode = 1;
		String errorMessage = null;
		byte[] payload = null;
		try
		{
			switch(request.commandId)
			{
				case RDPSUTControl_CommandId.START_RDP_CONNECTION:
					RDP_Connection_PayLoad connectPayload = new RDP_Connection_PayLoad();
					connectPayload.Decode(request.payload);
					if(connectPayload.type == RDP_Connect_Payload_Type.PARAMETERS_STRUCT &&
							Start_RDP_Connection(connectPayload.rdpConnectionConfig) > 0)
					{
						resultCode = SUTControl_ResultCode.SUCCESS;
					}
					else if (connectPayload.type == RDP_Connect_Payload_Type.RDP_FILE)
					{
						errorMessage = "Free RDP cannot use .rdp file";
					}
					else
					{
						errorMessage = "Start RDP Connection failed!";
					}
					break;
				case RDPSUTControl_CommandId.CLOSE_RDP_CONNECTION:
					if(Stop_RDP_Connection() > 0)
					{
						resultCode = SUTControl_ResultCode.SUCCESS;
					}
					else
					{
						errorMessage = "Stop RDP Connection failed!";
					}
					break;
				case RDPSUTControl_CommandId.SCREEN_SHOT:
					payload = Take_Screen_Shot();
					if(payload != null)
					{
						resultCode = SUTControl_ResultCode.SUCCESS;
					}
					else
					{
						errorMessage = "Take screen shot failed!";
					}
					break;
					
					//Add more command support here
					
				default:
					errorMessage = "CommandID is not supported :"+request.commandId;
					break;
			}
		}
		catch(Exception e)
		{
			errorMessage = "Process command "+ request.commandId +" failed with exception :"+e.getMessage();
		}
		
		SUT_Control_Response_Message response = new SUT_Control_Response_Message(request.testsuiteId, request.commandId, request.caseName, request.requestId, resultCode, errorMessage, payload);
		return response;
	}
		
	public static int Start_RDP_Connection(RDP_Connection_Configure_Parameters parameters)
	{
		String parameterStr = " --rfx -T RDPClient";
		parameterStr += " " + parameters.address +":"+parameters.port;
		if(parameters.screenType == 0x0001)
		{
			parameterStr += " -f";
		}
		else
		{			
		}
		try
		{
			System.out.println("xfreerdp "+parameterStr);
			Runtime.getRuntime().exec("xfreerdp "+parameterStr);		
			return 1;
		}
		catch(Exception e)
		{
			return -1;
		}
	}
	public static int Stop_RDP_Connection()
	{
	
		try
		{
			// Do nothing. FreeRDP close the connection when server disconnect connection
			// you can do something here to kill or close RDP client too
			return 1;
		}
		catch(Exception e)
		{
			return -1;
		}
	}
	
	
	public static byte[] Take_Screen_Shot()
	{
		try
		{
			// using import to take screen shot, please install it, or using other tool instead
			// Java's screenshot method (Robot.createScreenCapture) is not works well on Ubuntu.
			Runtime.getRuntime().exec("import -window RDPClient /home/pettest/ttt.bmp");
			
			// wait the capture done and the graphic file created
			// Need to set a timeout here, preventing endless wait if import tool error
			while(!new File("/home/pettest/ttt.bmp").exists())
			{
				Thread.sleep(100);
			}
			File imageFile = new File("/home/pettest/ttt.bmp");
			BufferedImage screenShot = ImageIO.read(imageFile);
	        int bufferSize = screenShot.getWidth()* screenShot.getHeight()*3 + 8;
	        ByteBuffer buffer = ByteBuffer.allocate(bufferSize);
	        buffer.order(ByteOrder.LITTLE_ENDIAN);
	        
	        buffer.putInt(screenShot.getWidth());
	        buffer.putInt(screenShot.getHeight());
	        for(int j=0; j<screenShot.getHeight(); j++ )
	        {
	        	for(int i=0; i < screenShot.getWidth(); i++)
	        	{
	        		int RGB = screenShot.getRGB(i, j);
	        		Color col = new Color(RGB);
	        		buffer.put((byte)col.getRed());
	        		buffer.put((byte)col.getGreen());
	        		buffer.put((byte)col.getBlue());
	        	}
	        }
	        return buffer.array();

		}
		catch(Exception e)
		{
			System.out.println(e.getMessage());
			return null;
		}
	}

}
