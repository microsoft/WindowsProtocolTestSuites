// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

package rdpsutcontrol.message;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.charset.Charset;

public class SUT_Control_Response_Message
{
    public short messageType;
    public short testsuiteId;
    public short commandId;

    public int caseNameLength;
    public String caseName;
    private byte[] encodedCaseName;

    public short requestId;

    public int resultCode;

    private int errorMessageLength;
    private String errorMessage;
    private byte[] encodedErrorMessage;

    public int payloadLength;
    public byte[] payload;

    public String GetErrorMessage()
    {
        return errorMessage;
    }

    public void SetErrorMessage(String errorMessage)
    {
        this.errorMessage = errorMessage;
        if(this.errorMessage != null)
        {
            this.encodedErrorMessage = Charset.forName("UTF-8").encode(this.errorMessage).array();
            this.errorMessageLength = encodedErrorMessage.length;
        }
    }

    public void SetCaseName(String caseName) {
        this.caseName = caseName;
        if(this.caseName != null) {
            this.encodedCaseName = Charset.forName("UTF-8").encode(this.caseName).array();
            this.caseNameLength = this.encodedCaseName.length;
        }
    }

    public SUT_Control_Response_Message(short testsuitId, short commandId, String caseName, short requestId, int resultCode, String errorMessage, byte[] payload)
    {
        this.messageType = SUTControl_MessageType.SUT_CONTROL_RESPONSE;
        this.commandId = commandId;

        this.caseNameLength = 0;
        this.caseName = caseName;

        if (this.caseName != null)
        {
            this.encodedCaseName = Charset.forName("UTF-8").encode(this.caseName).array();
            this.caseNameLength = this.encodedCaseName.length;
        }

        this.requestId = requestId;
        this.resultCode = resultCode;
        this.errorMessage = errorMessage;
        this.payload = payload;

        if(this.errorMessage != null)
        {
            this.encodedErrorMessage = Charset.forName("UTF-8").encode(this.errorMessage).array();
            this.errorMessageLength = encodedErrorMessage.length;
        }

        if(this.payload != null)
        {
            this.payloadLength = payload.length;
        }
    }

    public byte[] Encode()
    {
        int length = 24 + errorMessageLength + payloadLength + this.caseNameLength;
        ByteBuffer buffer = ByteBuffer.allocate(length);
        buffer.order(ByteOrder.LITTLE_ENDIAN);

        buffer.putShort(messageType);
        buffer.putShort(testsuiteId);
        buffer.putShort(commandId);

        buffer.putInt(this.caseNameLength);
        if ((this.caseName != null) && (this.caseNameLength > 0))
        {
            buffer.put(this.encodedCaseName);
        }

        buffer.putShort(requestId);

        buffer.putInt(resultCode);

        buffer.putInt(errorMessageLength);
        if(encodedErrorMessage != null)
        {
            buffer.put(encodedErrorMessage);
        }

        buffer.putInt(payloadLength);
        if(payload != null)
        {
            buffer.put(payload);
        }

        return buffer.array();
    }
}
