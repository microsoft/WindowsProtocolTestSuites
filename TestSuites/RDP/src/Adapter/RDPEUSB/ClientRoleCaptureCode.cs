// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb;

namespace Microsoft.Protocols.TestSuites.Rdpeusb
{
    /// <summary>
    /// The helper class for verifying requirements.
    /// </summary>
    public class ReqCapturer
    {
        /// <summary>
        /// Indicates if verifications need to be done.
        /// </summary>
        public static bool NeedVerify { get; set; }

        /// <summary>
        /// The associated test site.
        /// </summary>
        public static ITestSite Site { get; set; }

        public static void VerifyResponse(EusbPdu request, EusbPdu responseToBeVerified)
        {
            if (!NeedVerify)
            {
                return;
            }

            if (responseToBeVerified is EusbIoControlCompletionPdu)
            {
                Site.Assert.IsInstanceOfType(
                    request,
                    typeof(EusbRegisterRequestCallbackPdu),
                    "The request must be type of EusbRegisterRequestCallbackPdu"
                    );
            }
        }

        /// <summary>
        /// This method is used to verify the IOCONTROL_COMPLETION PDU.
        /// </summary>
        /// <param name="responsePdu">The PDU from the client.</param>
        /// <param name="requestId">The RequestId in the request PDU.</param>
        /// <param name="outputBufferSize">The OutputBufferSize in the request PDU.</param>
        /// <param name="requestCompletion">A unique InterfaceID to be set in the Register Request Callback Message.</param>
        public static void VerifyIoControlCompletion(EusbIoControlCompletionPdu responsePdu, uint requestId, uint outputBufferSize, uint requestCompletion)
        {
            Site.Assert.AreEqual<uint>(
                requestCompletion,
                responsePdu.InterfaceId,
                "Expect that the InterfaceId in the response PDU equals the RequestCompletion field of the REGISTER_REQUEST_CALLBACK PDU. The actual value is 0x{0:x8}.",
                responsePdu.InterfaceId);

            Site.Assert.AreEqual<Mask_Values>(
                Mask_Values.STREAM_ID_PROXY,
                responsePdu.Mask,
                "Expect that the Mask in the response PDU is STREAM_ID_PROXY.");

            Site.Assert.AreEqual<FunctionId_Values>(
                FunctionId_Values.IOCONTROL_COMPLETION,
                (FunctionId_Values)responsePdu.FunctionId,
                "Expect that the FunctionId in the response PDU is CHANNEL_CREATED. The actual value is 0x{0:x8}.",
                responsePdu.FunctionId);

            Site.Assert.AreEqual<uint>(
                requestId,
                responsePdu.RequestId,
                "Expect that the RequestId in the response PDU equals the RequestId in the request PDU. The actual value is 0x{0:x8}.",
                requestId);

            if (responsePdu.HResult == (uint)HRESULT_FROM_WIN32.ERROR_SUCCESS)
            {
                Site.Assert.AreEqual<uint>(
                    outputBufferSize,
                    responsePdu.OutputBufferSize,
                    "Expect that the OutputBufferSize in the response PDU equals the OutputBufferSize in the request PDU. The actual value is 0x{0:x8}.",
                    responsePdu.OutputBufferSize);

                Site.Assert.AreEqual<uint>(
                    outputBufferSize,
                    (uint)responsePdu.OutputBuffer.Length,
                    "Expect that the length of OutputBuffer in the response PDU equals the OutputBufferSize in the request PDU. The actual value is 0x{0:x8}.",
                    (uint)responsePdu.OutputBuffer.Length);
            }
            else if (responsePdu.HResult == (uint)HRESULT_FROM_WIN32.ERROR_INSUFFICIENT_BUFFER)
            {
                Site.Assert.AreEqual<uint>(
                    outputBufferSize,
                    responsePdu.OutputBufferSize,
                    "Expect that the OutputBufferSize in the response PDU equals the OutputBufferSize in the request PDU. The actual value is 0x{0:x8}.",
                    responsePdu.OutputBufferSize);

                Site.Assert.IsTrue(
                    responsePdu.Information > outputBufferSize,
                    "Expect that the Information in the response PDU is bigger than the OutputBufferSize in the request PDU when returning ERROR_INSUFFICIENT_BUFFER. The actual value is 0x{0:x8}.",
                    responsePdu.Information);

                Site.Assert.AreEqual<uint>(
                    responsePdu.Information,
                    (uint)responsePdu.OutputBuffer.Length,
                    "Expect that the length of OutputBuffer in the response PDU equals the Information in the response PDU. The actual value is 0x{0:x8}.",
                    (uint)responsePdu.OutputBuffer.Length);
            }
            else
            {
                Site.Assert.AreEqual<uint>(
                    0,
                    responsePdu.OutputBufferSize,
                    "Expect that the OutputBufferSize in the response PDU is zero when returning other errors. The actual value is 0x{0:x8}.",
                    responsePdu.OutputBufferSize);

                Site.Assert.AreEqual<uint>(
                    0,
                    (uint)responsePdu.OutputBuffer.Length,
                    "Expect that the OutputBuffer in the response PDU is empty. The actual value is 0x{0:x8}.",
                    (uint)responsePdu.OutputBuffer.Length);
            }

        }

        /// <summary>
        /// This method is used to verify the URB_COMPLETION PDU.
        /// </summary>
        /// <param name="responsePdu">The PDU from the client.</param>
        /// <param name="tsUrb">The TS_URB in the request.</param>
        /// <param name="requestCompletion">A unique InterfaceID to be set in the Register Request Callback Message.</param>
        public static void VerifyUrbCompletion(EusbUrbCompletionPdu responsePdu, TS_URB tsUrb, uint requestCompletion)
        {
            Site.Assert.AreEqual<uint>(
                requestCompletion,
                responsePdu.InterfaceId,
                "Expect that the InterfaceId in the response PDU equals the RequestCompletion field of the REGISTER_REQUEST_CALLBACK PDU. The actual value is 0x{0:x8}.",
                responsePdu.InterfaceId);

            Site.Assert.AreEqual<Mask_Values>(
                Mask_Values.STREAM_ID_PROXY,
                responsePdu.Mask,
                "Expect that the Mask in the response PDU is STREAM_ID_PROXY.");

            Site.Assert.AreEqual<FunctionId_Values>(
                FunctionId_Values.URB_COMPLETION,
                (FunctionId_Values)responsePdu.FunctionId,
                "Expect that the FunctionId in the response PDU is CHANNEL_CREATED. The actual value is 0x{0:x8}.",
                responsePdu.FunctionId);

            Site.Assert.AreEqual<uint>(
                tsUrb.Header.RequestId,
                responsePdu.RequestId,
                "Expect that the RequestId in the response PDU equals the RequestId in the request PDU. The actual value is 0x{0:x8}.",
                responsePdu.RequestId);
        }

        /// <summary>
        /// This method is used to verify the URB_COMPLETION_NO_DATA PDU.
        /// </summary>
        /// <param name="responsePdu">The PDU from the client.</param>
        /// <param name="tsUrb">The TS_URB in the request.</param>
        /// <param name="isTransferInRequest">This specify if the request is TRANSFER_IN_REQUEST or TRANSFER_OUT_REQUEST.</param>
        /// <param name="requestCompletion">A unique InterfaceID to be set in the Register Request Callback Message.</param>
        public static void VerifyUrbCompletionNoData(EusbUrbCompletionNoDataPdu responsePdu, TS_URB tsUrb, bool isTransferInRequest, uint requestCompletion)
        {
            Site.Assert.AreEqual<uint>(
                requestCompletion,
                responsePdu.InterfaceId,
                "Expect that the InterfaceId in the response PDU equals the RequestCompletion field of the REGISTER_REQUEST_CALLBACK PDU. The actual value is 0x{0:x8}.",
                responsePdu.InterfaceId);

            Site.Assert.AreEqual<Mask_Values>(
                Mask_Values.STREAM_ID_PROXY,
                responsePdu.Mask,
                "Expect that the Mask in the response PDU is STREAM_ID_PROXY.");

            Site.Assert.AreEqual<FunctionId_Values>(
                FunctionId_Values.URB_COMPLETION_NO_DATA,
                (FunctionId_Values)responsePdu.FunctionId,
                "Expect that the FunctionId in the response PDU is CHANNEL_CREATED. The actual value is 0x{0:x8}.",
                responsePdu.FunctionId);

            Site.Assert.AreEqual<uint>(
                tsUrb.Header.RequestId,
                responsePdu.RequestId,
                "Expect that the RequestId in the response PDU equals the RequestId in the request PDU. The actual value is 0x{0:x8}.",
                responsePdu.RequestId);

            if (isTransferInRequest)
            {
                #region Verify Response For TRANSFER_IN_REQUEST
                Site.Assert.AreEqual<uint>(
                    0,
                    responsePdu.OutputBufferSize,
                    "Expect that the OutputBufferSize in the response PDU is zero. The actual value is 0x{0:x8}.",
                    responsePdu.OutputBufferSize);

                if (tsUrb is TS_URB_SELECT_CONFIGURATION)
                {
                    #region Verify TS_URB_SELECT_CONFIGURATION_RESULT
                    Site.Log.Add(LogEntryKind.Debug,
                        "Expect the TsUrbResult is TS_URB_SELECT_CONFIGURATION_RESULT when the TsUrb in the request is TS_URB_SELECT_CONFIGURATION.");
                    TS_URB_SELECT_CONFIGURATION_RESULT urb = new TS_URB_SELECT_CONFIGURATION_RESULT();

                    if (!PduMarshaler.Unmarshal(responsePdu.TsUrbResult, urb))
                    {
                        // TsUrbResult can not be unmarshaled to TS_URB_SELECT_CONFIGURATION_RESULT
                        TS_URB_UNKNOWN unknowUrb = new TS_URB_UNKNOWN();
                        Site.Assume.IsTrue(PduMarshaler.Unmarshal(responsePdu.TsUrbResult, unknowUrb),
                            "Marshaling the data to an unknown PDU MUST succeed.");

                        Site.Log.Add(LogEntryKind.CheckFailed,
                            "The TsUrbResult is not valid TS_URB_SELECT_CONFIGURATION_RESULT. The data is:\r\n{0}", unknowUrb.ToString());
                    }
                    else
                    {

                        Site.Log.Add(LogEntryKind.CheckSucceeded, "The TsUrbResult is expected TS_URB_SELECT_CONFIGURATION_RESULT.");
                    }
                    #endregion
                }
                else if (tsUrb is TS_URB_SELECT_INTERFACE)
                {
                    #region Verify TS_URB_SELECT_INTERFACE_RESULT
                    Site.Log.Add(LogEntryKind.Debug,
                        "Expect the TsUrbResult is TS_URB_SELECT_INTERFACE_RESULT when the TsUrb in the request is TS_URB_SELECT_INTERFACE.");
                    TS_URB_SELECT_INTERFACE_RESULT urb = new TS_URB_SELECT_INTERFACE_RESULT();

                    if (!PduMarshaler.Unmarshal(responsePdu.TsUrbResult, urb))
                    {
                        // TsUrbResult can not be unmarshaled to TS_URB_SELECT_INTERFACE_RESULT
                        TS_URB_UNKNOWN unknowUrb = new TS_URB_UNKNOWN();
                        Site.Assume.IsTrue(PduMarshaler.Unmarshal(responsePdu.TsUrbResult, unknowUrb),
                            "Marshaling the data to an unknown PDU MUST succeed.");

                        Site.Log.Add(LogEntryKind.CheckFailed,
                            "The TsUrbResult is not valid TS_URB_SELECT_INTERFACE_RESULT. The data is:\r\n{0}", unknowUrb.ToString());
                    }
                    else
                    {

                        Site.Log.Add(LogEntryKind.CheckSucceeded, "The TsUrbResult is expected TS_URB_SELECT_INTERFACE_RESULT.");
                    }
                    #endregion
                }
                else if (tsUrb is TS_URB_GET_CURRENT_FRAME_NUMBER)
                {
                    #region Verify TS_URB_GET_CURRENT_FRAME_NUMBER_RESULT
                    Site.Log.Add(LogEntryKind.Debug,
                        "Expect the TsUrbResult is TS_URB_GET_CURRENT_FRAME_NUMBER_RESULT when the TsUrb in the request is TS_URB_GET_CURRENT_FRAME_NUMBER.");
                    TS_URB_GET_CURRENT_FRAME_NUMBER_RESULT urb = new TS_URB_GET_CURRENT_FRAME_NUMBER_RESULT();

                    if (!PduMarshaler.Unmarshal(responsePdu.TsUrbResult, urb))
                    {
                        // TsUrbResult can not be unmarshaled to TS_URB_GET_CURRENT_FRAME_NUMBER_RESULT
                        TS_URB_UNKNOWN unknowUrb = new TS_URB_UNKNOWN();
                        Site.Assume.IsTrue(PduMarshaler.Unmarshal(responsePdu.TsUrbResult, unknowUrb),
                            "Marshaling the data to an unknown PDU MUST succeed.");

                        Site.Log.Add(LogEntryKind.CheckFailed,
                            "The TsUrbResult is not valid TS_URB_GET_CURRENT_FRAME_NUMBER_RESULT. The data is:\r\n{0}", unknowUrb.ToString());
                    }
                    else
                    {

                        Site.Log.Add(LogEntryKind.CheckSucceeded, "The TsUrbResult is expected TS_URB_GET_CURRENT_FRAME_NUMBER_RESULT.");
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region Verify Response For TRANSFER_OUT_REQUEST
                Site.Assert.AreNotEqual<uint>(
                    0,
                    responsePdu.OutputBufferSize,
                    "Expect that the OutputBufferSize in the response PDU is not zero. The actual value is 0x{0:x8}.",
                    responsePdu.OutputBufferSize);
                #endregion
            }

            #region Verify TS_URB_ISOCH_TRANSFER_RESULT
            if (tsUrb is TS_URB_ISOCH_TRANSFER)
            {
                Site.Log.Add(LogEntryKind.Debug,
                    "Expect the TsUrbResult is TS_URB_ISOCH_TRANSFER_RESULT when the TsUrb in the request is TS_URB_ISOCH_TRANSFER.");
                TS_URB_ISOCH_TRANSFER_RESULT urb = new TS_URB_ISOCH_TRANSFER_RESULT();

                if (!PduMarshaler.Unmarshal(responsePdu.TsUrbResult, urb))
                {
                    // TsUrbResult can not be unmarshaled to TS_URB_ISOCH_TRANSFER_RESULT
                    TS_URB_UNKNOWN unknowUrb = new TS_URB_UNKNOWN();
                    Site.Assume.IsTrue(PduMarshaler.Unmarshal(responsePdu.TsUrbResult, unknowUrb),
                        "Marshaling the data to an unknown PDU MUST succeed.");

                    Site.Log.Add(LogEntryKind.CheckFailed,
                        "The TsUrbResult is not valid TS_URB_ISOCH_TRANSFER_RESULT. The data is:\r\n{0}", unknowUrb.ToString());
                }
                else
                {

                    Site.Log.Add(LogEntryKind.CheckSucceeded, "Expect that the TsUrbResult is TS_URB_ISOCH_TRANSFER_RESULT.");
                }
            }
            #endregion
        }
    }
}
