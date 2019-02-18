// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

package rdpsutcontrol.message;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.charset.Charset;

public class RDP_Connection_PayLoad
{
    public int type;
    public String rdpFileContent;
    public RDP_Connection_Configure_Parameters rdpConnectionConfig;

    public boolean Decode(byte[] data)
    {
        try
        {
            ByteBuffer buffer = ByteBuffer.wrap(data);
            buffer.order(ByteOrder.LITTLE_ENDIAN);
            this.type = buffer.getInt();
            
            byte[] remainData = new byte[data.length - 4];
            buffer.get(remainData);
            
            if (this.type == RDP_Connect_Payload_Type.RDP_FILE)
            {
                this.rdpFileContent = Charset.forName("UTF-8").decode(ByteBuffer.wrap(remainData)).toString();
            }
            else
            {
                this.rdpConnectionConfig = new RDP_Connection_Configure_Parameters();
                if(!this.rdpConnectionConfig.Decode(remainData))
                {
                    return false;
                }
            }
            return true;
        }
        catch(Exception e)
        {
            return false;
        }
    }
}
