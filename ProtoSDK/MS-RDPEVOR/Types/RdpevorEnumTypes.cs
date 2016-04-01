// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpevor
{
    /// <summary>
    /// The value of this integer indicates the type of message following the header. 
    /// </summary>
    public enum PacketTypeValues: uint
    {
        /// <summary>
        /// Indicates that this message should be interpreted as a TSMM_PRESENTATION_REQUEST structure.
        /// </summary>
        TSMM_PACKET_TYPE_PRESENTATION_REQUEST = 1,

        /// <summary>
        /// Indicates that this message should be interpreted as a TSMM_PRESENTATION_RESPONSE structure.
        /// </summary>
        TSMM_PACKET_TYPE_PRESENTATION_RESPONSE = 2,

        /// <summary>
        /// Indicates that this message should be interpreted as a TSMM_CLIENT_NOTIFICATION structure.
        /// </summary>
        TSMM_PACKET_TYPE_CLIENT_NOTIFICATION = 3,

        /// <summary>
        /// Indicates that this message should be interpreted as a TSMM_VIDEO_DATA structure.
        /// </summary>
        TSMM_PACKET_TYPE_VIDEO_DATA = 4
    }

    /// <summary>
    /// The protocol version of RDPEVOR.
    /// </summary>
    public enum RdpevorVersionValues : byte
    {
        /// <summary>
        /// In RDP8, this MUST be set to 0x01.
        /// </summary>
        RDP8 = 0x01,

        /// <summary>
        /// Invalide value.
        /// </summary>
        InvalidValue = 0x02
    }

    /// <summary>
    /// A number that identifies which operation the client is to perform.
    /// </summary>
    public enum CommandValues : byte
    {
        /// <summary>
        /// Command to start presentation.
        /// </summary>
        Start = 0x01,

        /// <summary>
        /// Command to stop presentation.
        /// </summary>
        Stop = 0x02,

        /// <summary>
        /// Invalid command, used for negative test.
        /// </summary>
        InvalidCommand = 0x04
    }

    /// <summary>
    /// UINT8. A number that identifies which notification type the client is sending.
    /// </summary>
    public enum NotificationTypeValues:byte
    {
        /// <summary>
        /// This message SHOULD be sent whenever the client detects missing or out-of-order packets.
        /// </summary>
        NetworkError = 0x01,

        /// <summary>
        /// This message MUST be sent whenever the client cannot decode incoming frames fast enough.
        /// </summary>
        FrameRateOverride = 0x02
    }

    /// <summary>
    /// UINT32. A number that identifies which operation to execute on the server.  This number is a bitmask.
    /// </summary>
    [Flags]
    public enum FrameRateOverride_FlagsValues : uint
    {
        /// <summary>
        /// This message MUST be sent whenever the client cannot decode incoming frames fast enough.  
        /// DesiredFrameRate MUST be set to the number of frames the client can decode per second.  
        /// This flag is mutually exclusive with Unrestricted Frame Rate (0x2).
        /// </summary>
        OverrideFrameRate = 0x1,

        /// <summary>
        /// This message SHOULD be sent whenever the client can decode all frames sent from the server 
        /// and there still exists spare resources to decode more frames.  
        /// The server will send as many frames as it can in response.  
        /// DesiredFrameRate will be ignored and SHOULD be set to zero.
        /// </summary>
        UnrestrictedFrameRate = 0x2
    }

    /// <summary>
    /// UINT8. The bits of this integer indicate attributes of this message. 
    /// </summary>
    [Flags]
    public enum TsmmVideoData_FlagsValues : byte
    {
        None = 0,

        /// <summary>
        /// Indicates that this message has a valid hnsTimestamp field.
        /// </summary>
        TSMM_VIDEO_DATA_FLAG_HAS_TIMESTAMPS = 0x01,

        /// <summary>
        /// Indicates that the sample contained in this message is part of a keyframe.
        /// </summary>
        TSMM_VIDEO_DATA_FLAG_KEYFRAME = 0x02,

        /// <summary>
        /// Invalid flag for negative test
        /// </summary>
        TSMM_VIDEO_DATA_FLAG_NEW_FRAMERATE = 0x04
    }

    /// <summary>
    /// This field identifies the Media Foundation video subtype of the video stream.
    /// </summary>
    public enum VideoSubtype
    {
        MFVideoFormat_RGB32, 
        MFVideoFormat_IYUV,  
        MFVideoFormat_H264
    }

    /// <summary>
    /// The negative types for test.
    /// </summary>
    public enum RdpevorNegativeType
    {
        /// <summary>
        /// Indicates a positive test, all fields should be set to valid value.
        /// </summary>
        None,

        /// <summary>
        /// Set the packet length of Presentation Request to an invalid value.
        /// </summary>
        PresentationRequest_InvalidPacketLength,

        /// <summary>
        /// Set the version field of Presentation Request to an invalid value.
        /// </summary>
        PresentationRequest_InvalidVersion,

        /// <summary>
        /// Set the packet length of TSMM_VIDEO_DATA to an invalid value.
        /// </summary>
        VideoData_InvalidPacketLength,

        /// <summary>
        /// Set the version field of TSMM_VIDEO_DATA to an invalid value.
        /// </summary>
        VideoData_InvalidVersion,
    }
}
