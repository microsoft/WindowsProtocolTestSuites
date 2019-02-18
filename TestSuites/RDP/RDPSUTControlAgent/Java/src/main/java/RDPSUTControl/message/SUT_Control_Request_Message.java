// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

package rdpsutcontrol.message;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.charset.Charset;

public class SUT_Control_Request_Message
{
    public short messageType;
    public short testsuiteId;
    public short commandId;
    public int caseNameLength;
    public String caseName;
    public short requestId;

    public int helpMessageLength;
    public String helpMessage;

    public int payloadLength;
    public byte[] payload;

    public boolean Decode(byte[] data)
    {
        try
        {
            ByteBuffer buffer = ByteBuffer.wrap(data);
            buffer.order(ByteOrder.LITTLE_ENDIAN);
            messageType = buffer.getShort();
            testsuiteId = buffer.getShort();
            commandId = buffer.getShort();
            
            this.caseNameLength = buffer.getInt();
            if (this.caseNameLength > 0)
            {
                byte[] nameArrary = new byte[this.caseNameLength];
                buffer.get(nameArrary);
                this.caseName = Charset.forName("UTF-8").decode(ByteBuffer.wrap(nameArrary)).toString();
            }

            requestId = buffer.getShort();

            this.helpMessageLength = buffer.getInt();
            byte[] stringArray = new byte[helpMessageLength];
            buffer.get(stringArray);
            this.helpMessage = Charset.forName("UTF-8").decode(ByteBuffer.wrap(stringArray)).toString();

            this.payloadLength = buffer.getInt();
            this.payload = new byte[payloadLength];
            buffer.get(this.payload);

            return true;
        }
        catch(Exception e)
        {
            return false;
        }
    }
}
