// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

package rdpsutcontrol.agent;

import rdpsutcontrol.message.*;

import java.io.File;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.Properties;
import java.awt.*;
import java.awt.image.BufferedImage;

import javax.imageio.ImageIO;

public class SUTControl
{
    private Properties config;

    public SUTControl(Properties _config)
    {
        config = _config;
    }

    public SUT_Control_Response_Message ProcessCommand(SUT_Control_Request_Message request)
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
                    if (connectPayload.type == RDP_Connect_Payload_Type.PARAMETERS_STRUCT &&
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
                case RDPSUTControl_CommandId.TOUCH_EVENT_SINGLE:
                    // TODO
                    break;
                case RDPSUTControl_CommandId.TOUCH_EVENT_MULTIPLE:
                    // TODO
                    break;
                case RDPSUTControl_CommandId.TOUCH_EVENT_DISMISS_HOVERING_CONTACT:
                    // TODO
                    break;
                case RDPSUTControl_CommandId.DISPLAY_UPDATE_RESOLUTION:
                    // TODO
                    break;
                case RDPSUTControl_CommandId.DISPLAY_UPDATE_MONITORS:
                    // TODO
                    break;
                case RDPSUTControl_CommandId.DISPLAY_FULLSCREEN:
                    // TODO
                    break;
                default:
                    errorMessage = "CommandID is not supported :"+request.commandId;
                    break;
            }
        }
        catch (Exception e)
        {
            errorMessage = "Process command "+ request.commandId +" failed with exception :"+e.getMessage();
        }

        SUT_Control_Response_Message response = new SUT_Control_Response_Message(request.testsuiteId, request.commandId, request.caseName, request.requestId, resultCode, errorMessage, payload);
        return response;
    }

    public int Start_RDP_Connection(RDP_Connection_Configure_Parameters parameters)
    {
        String cmd;

        if (parameters.connectApproach == 0x0000 && parameters.screenType == 0x0000)
        {
            cmd = config.getProperty("Negotiate");
        }
        else if (parameters.connectApproach == 0x0000 && parameters.screenType == 0x0001)
        {
            cmd = config.getProperty("NegotiateFullScreen");
        }
        else if (parameters.connectApproach == 0x0001 && parameters.screenType == 0x0000)
        {
            cmd = config.getProperty("DirectCredSSP");
        }
        else if (parameters.connectApproach == 0x0001 && parameters.screenType == 0x0001)
        {
            cmd = config.getProperty("DirectCredSSPFullScreen");
        }
        else
        {
            // Negotiate is the default one
            cmd = config.getProperty("Negotiate");
        }

        try
        {
            cmd = cmd.replace("{{address}}", parameters.address);
            cmd = cmd.replace("{{port}}", String.valueOf(parameters.port));

            System.out.println(cmd);
            Runtime.getRuntime().exec(cmd);
            return 1;
        }
        catch (Exception e)
        {
            return -1;
        }
    }

    public int Stop_RDP_Connection()
    {
        String cmd;
        cmd = config.getProperty("StopRDP");

        try
        {
            System.out.println(cmd);
            Runtime.getRuntime().exec(cmd);
            return 1;
        }
        catch (Exception e)
        {
            return -1;
        }
    }

    public byte[] Take_Screen_Shot()
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
        catch (Exception e)
        {
            System.out.println(e.getMessage());
            return null;
        }
    }
}
