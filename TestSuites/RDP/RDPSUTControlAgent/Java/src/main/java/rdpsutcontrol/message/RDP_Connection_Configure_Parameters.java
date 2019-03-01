// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

package rdpsutcontrol.message;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.charset.Charset;

public class RDP_Connection_Configure_Parameters
{
    public short port;
    public short screenType;
    public short desktopWidth;
    public short desktopHeight;
    public short connectApproach;
    public short AddressLength;
    public String address;

    public boolean Decode(byte[] data)
    {
        try
        {
            ByteBuffer buffer = ByteBuffer.wrap(data);
            buffer.order(ByteOrder.LITTLE_ENDIAN);
            this.port = buffer.getShort();
            this.screenType = buffer.getShort();
            this.desktopWidth = buffer.getShort();
            this.desktopHeight = buffer.getShort();
            this.connectApproach = buffer.getShort();

            this.AddressLength = buffer.getShort();
            byte[] addArray = new byte[AddressLength];
            buffer.get(addArray);
            this.address = Charset.forName("UTF-8").decode(ByteBuffer.wrap(addArray)).toString();

            return true;
        }
        catch(Exception e)
        {
            return false;
        }
    }
}
