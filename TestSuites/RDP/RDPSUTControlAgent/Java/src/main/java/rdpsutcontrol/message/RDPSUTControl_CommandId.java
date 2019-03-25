// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

package rdpsutcontrol.message;

public class RDPSUTControl_CommandId
{
    public static final short START_RDP_CONNECTION = 0x0001;
    public static final short CLOSE_RDP_CONNECTION = 0x0002;
    public static final short AUTO_RECONNECT = 0x0003;
    public static final short BASIC_INPUT = 0x0004;
    public static final short SCREEN_SHOT = 0x0005;
    public static final short TOUCH_EVENT_SINGLE = 0x0101;
    public static final short TOUCH_EVENT_MULTIPLE = 0x0102;
    public static final short TOUCH_EVENT_DISMISS_HOVERING_CONTACT = 0x0103;
    public static final short DISPLAY_UPDATE_RESOLUTION = 0x0201;
    public static final short DISPLAY_UPDATE_MONITORS = 0x0202;
    public static final short DISPLAY_FULLSCREEN = 0x0203;
}
