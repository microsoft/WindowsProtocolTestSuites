// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;

    /// <summary>
    /// Verify the server role requirements of MS-PCHC.
    /// </summary>
    public partial class PchcServerAdapter
    {
        /// <summary>
        /// Capture the Transport RS
        /// </summary>
        /// <param name="method">The request method.</param>
        /// <param name="uri">The uri which the client use to connenct the hosted cache.</param>
        private void ValidateTransport(
            string method,
            string uri)
        {
            // Capture MS-PCHC R3
            Site.CaptureRequirementIfAreEqual<string>(
                "POST".ToLower(),
                method.ToLower(),
                3,
                "[Transport]The server sends the response message as payload of the HTTP response.");

            // Capture MS-PCHC R4 and R69
            string expect = string.Format(
                "https://{0}:{1}/{2}",
                this.GetProperty("Environment.HostedCacheServer.MachineName"),
                PCHCPROTOCOLPORT,
                PCHCRESOURCE);

            Site.CaptureRequirementIfAreEqual<string>(
                expect.ToLower(),
                uri.ToLower(),
                4,
                @"[In Transport] The URL on which the server MUST listen is 
                https://:<port number>/C574AC30-5794-4AEE-B1BB-6651C5315029.");

            Site.CaptureRequirementIfAreEqual<string>(
                expect.ToLower(),
                uri.ToLower(),
                69,
                @"[In Initialization] The following initialization of the hosted cache MUST be performed:
                The hosted cache MUST be initialized by starting to listen for incoming HTTP requests on the
                URL specified in section 2.1.");
        }

        /// <summary>
        /// Capture the ResponseMessage RS.
        /// </summary>
        /// <param name="responseMsg">A ResponseMessage message of an INITIAL_OFFER_MESSAGE.</param>
        private void ValidateInitialOfferMessageResponse(ResponseMessage responseMsg)
        {
            // Capture MS-PCHC R55
            uint expect = sizeof(ResponseCode);
            uint actual = responseMsg.TransportHeader.Size;
            Site.CaptureRequirementIfAreEqual<uint>(
                expect,
                actual,
                55,
                @"[In Transport Header] The transport adds the following header[Size (4 bytes):  Total message size
                in bytes, excluding this field] in front of any response protocol message.");

            // Capture MS-PCHC R56
            Site.Log.Add(
               LogEntryKind.Debug,
               "Verify MS-PCHC_R56, Record responseMsg.ResponseCode Type:{0}",
               responseMsg.ResponseCode.ToString());

            // The received message is the same type as ResponseMessage, so this can be directly validated
            Site.CaptureRequirement(
                56,
                @"[In Response Code] Each response message contains a response code, as specified below
                [Transportheader (4 bytes), ResponseCode (1 byte)].");

            // Capture MS-PCHC R58
            Site.CaptureRequirementIfAreEqual<uint>(
                1,
                sizeof(ResponseCode),
                58,
                @"[In Response Code] ResponseCode (1 byte):  A code that indicates the server 
                response to the client request message.");

            // Capture MS-PCHC R61
            Site.CaptureRequirementIfIsTrue(
                responseMsg.ResponseCode == ResponseCode.INTERESTED
                || responseMsg.ResponseCode == ResponseCode.OK,
                61,
                @"[In Response Code] In an INITIAL_OFFER_MESSAGE (section 2.2.1.3),
                the response code MUST be either OK or INTERESTED.");

            // Capture MS-PCHC R131
            Site.CaptureRequirementIfIsTrue(
                responseMsg.ResponseCode == ResponseCode.INTERESTED
                || responseMsg.ResponseCode == ResponseCode.OK,
                131,
                @"[In Response Message] Response messages are sent in response to the following request messages:
                INITIAL_OFFER_MESSAGE, section 2.2.1.3, SEGMENT_INFO_MESSAGE, section 2.2.1.4.");
        }

        /// <summary>
        /// Capture the ResponseMessage RS.
        /// </summary>
        /// <param name="responseMsg">A ResponseMessage message of an INITIAL_OFFER_MESSAGE.</param>
        private void ValidateSegmentInfoResponse(ResponseMessage responseMsg)
        {
            // Capture MS-PCHC R55
            uint expect = sizeof(ResponseCode);
            uint actual = responseMsg.TransportHeader.Size;
            Site.CaptureRequirementIfAreEqual<uint>(
                expect,
                actual,
                55,
                @"[In Transport Header] The transport adds the following header[Size (4 bytes):  Total message size
                in bytes, excluding this field] in front of any response protocol message.");

            // Capture MS-PCHC R56
            Site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCHC_R56, Record responseMsg.ResponseCode Type:{0}",
                responseMsg.ResponseCode.ToString());

            // The received message is the same type as ResponseMessage, so this can be directly validated
            Site.CaptureRequirement(
                56,
               @"[In Response Code] Each response message contains a response code, as specified below
                [Transportheader (4 bytes), ResponseCode (1 byte)].");

            // Capture MS-PCHC R58
            Site.CaptureRequirementIfAreEqual<uint>(
                1,
                sizeof(ResponseCode),
                58,
                @"[In Response Code] ResponseCode (1 byte):  A code that indicates the server 
                response to the client request message.");

            // Capture MS-PCHC R62
            Site.CaptureRequirementIfAreEqual<ResponseCode>(
                ResponseCode.OK,
                responseMsg.ResponseCode,
                62,
                @"[In Response Code] In a SEGMENT_INFO_MESSAGE (section 2.2.1.4), the response code MUST be OK.");

            Site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCHC_R72, Record responseMsg Type:{0}",
                responseMsg.ToString());

            // Capture MS-PCHC R72
            // The received message is the same type as ResponseMessage, so this can be directly validated
            Site.CaptureRequirement(
                72,
                @"[In INITIAL_OFFER_MESSAGE Request Received] The hosted cache MUST respond with a 
                correctly formatted response message, as specified in section 2.2.2.");
        }

        /// <summary>
        /// Captrue the hosted cache initialization RS.
        /// </summary>
        /// <param name="port">The port which the hosted cache will listening.</param>
        private void ValidateServerInitialization(int port)
        {
            string platformOsVersion = this.GetProperty("Environment.HostedCacheServer.OSVersion");

            bool isWindows = false;

            if (platformOsVersion.ToLower().Equals(ServerRoleOSVersion.Win2K8.ToString().ToLower())
                || platformOsVersion.ToLower().Equals(ServerRoleOSVersion.Win2K8R2.ToString().ToLower())
                || platformOsVersion.ToLower().Equals(ServerRoleOSVersion.Win7.ToString().ToLower())
                || platformOsVersion.ToLower().Equals(ServerRoleOSVersion.WinVista.ToString().ToLower()))
            {
                isWindows = true;
            }

            // Capture MS-PCHC_R5 and 1005 
            string isR5Implemented = this.GetProperty("PCHC.SHOULDMAY.R5Implemented");
            string isR6Implemented = this.GetProperty("PCHC.SHOULDMAY.R6Implemented");

            if (isWindows)
            {
                Site.CaptureRequirementIfAreEqual(
                    PCHCPROTOCOLPORT,
                    port,
                    1005,
                    @"[In Transport] In windows implementation, the port number used is 443.");

                // Capture MS-PCHC R2005
                Site.CaptureRequirementIfAreEqual<int>(
                    PCHCPROTOCOLPORT,
                    port,
                    2005,
                    @"[In Transport] <2> Section 2.1: In a Windows implementation, the hosted cache 
                    listens on port 443 by default.");

                if (null == isR5Implemented)
                {
                    Site.Properties.Add("PCHC.SHOULDMAY.R5Implemented", bool.TrueString);
                    isR5Implemented = bool.TrueString;
                }

                // MS-PCHC_R1006 is blocked by technical document issue about an administrator 
                // can specify a different legal port number.
                if (bool.Parse(this.GetProperty("PCHC.IsTDI.66078.Fixed")))
                {
                    Site.CaptureRequirementIfAreNotEqual(
                        PCHCPROTOCOLPORT,
                        port,
                        1006,
                        @"[In Transport,the port number used is 443] In a Windows implementation , higher-layer action 
                        such as an administrator doesn't  specify a different legal port number by default.");
                }

                if (null == isR6Implemented)
                {
                    Site.Properties.Add("PCHC.SHOULDMAY.R6Implemented", bool.FalseString);
                    isR5Implemented = bool.FalseString;
                }
            }

            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify the port number, The actual port numbert is {0}.",
                port.ToString());

            if (null != isR5Implemented)
            {
                Site.CaptureRequirementIfAreEqual(
                    bool.Parse(isR5Implemented),
                    port == PCHCPROTOCOLPORT,
                    5,
                    @"[In Transport] The port number used SHOULD be 443.");
            }

            // MS-PCHC_R1006 is blocked by technical document issue about an administrator 
            // can specify a different legal port number.
            if (null != isR6Implemented)
            {
                if (bool.Parse(this.GetProperty("PCHC.IsTDI.66078.Fixed")))
                {
                    Site.CaptureRequirementIfAreEqual<bool>(
                        bool.Parse(isR6Implemented),
                        port == PCHCPROTOCOLPORT,
                        6,
                        @" [In Transport,the port number used is 443] A higher-layer action such as an administrator
                        MAY specify a different legal port number.");
                }
            }
        }

        /// <summary>
        /// Validate the SEGMENT_INFO_MESSAGE related RS
        /// </summary>
        /// <param name="segmentInfoMessage">The send message SEGMENT_INFO_MESSAGE</param>
        private void ValidateSegmentInfoMessage(SEGMENT_INFO_MESSAGE segmentInfoMessage)
        {
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R87, Validate this by confirm the ContentTag in the send SEGMENT_INFO_MESSAGE is not null.
                Record SEGMENT_INFO_MESSAGE.ContentTag is not null or not, 0 represents not null and 1 represents null,
                Record the actual value is {0}",
                (segmentInfoMessage.ContentTag != null).ToString());

            // Capture MS-PCHC R87
            this.Site.CaptureRequirementIfIsNotNull(
                segmentInfoMessage.ContentTag,
                87,
                @"[In SEGMENT_INFO_MESSAGE Request Received] Regardless of whether an INITIAL_OFFER_MESSAGE
                has previously been received from this client, the hosted cache MUST perform the following actions:
                The ContentTag is described in the SEGMENT_INFO_MESSAGE request.");
        }
    }
}
