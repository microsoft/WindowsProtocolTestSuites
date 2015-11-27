// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 CANCEL Response packet is sent by the server to indicate that an 
    /// SMB2 CANCEL Request was processed.
    /// </summary>
    public class Smb2CancelResponsePacket : Smb2StandardPacket<CANCEL_Response>
    {
    }

    // And empty structure to handle expection for cancel request which is not mentioned in TD.
    public partial struct CANCEL_Response
    {
    }
}
