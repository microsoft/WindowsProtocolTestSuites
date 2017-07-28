// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdprfx;

namespace Microsoft.Protocols.TestSuites.Rdpei
{
    public partial class RdpeiSUTControlAdapter : ManagedAdapterBase, IRdpeiSUTControlAdapter
    {
        #region Variables

        private ITestSite site;
        private IRdprfxAdapter rdprfxAdapter;
        private IRdpbcgrAdapter rdpbcgrAdapter;
        //The MaxRequestSize field of Multifragment Update Capability Set. Just for test.
        private uint maxRequestSize = 0x50002A;

        #endregion

        #region IAdapter Methods

        public override void Initialize(ITestSite testSite)
        {
            this.site = testSite;
            this.rdprfxAdapter = (IRdprfxAdapter)Site.GetAdapter(typeof(IRdprfxAdapter));
            this.rdprfxAdapter.Initialize(testSite);
            this.rdpbcgrAdapter = (IRdpbcgrAdapter)Site.GetAdapter(typeof(IRdpbcgrAdapter));
            RdpeiUtility.Initialized(this.site);
        }

        public override void Reset()
        {
            this.rdprfxAdapter.Reset();
        }

        public new TestTools.ITestSite Site
        {
            get { return site; }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.rdprfxAdapter.Dispose();
        }

        #endregion

        #region IRdpeiSUTControlAdapter Methods

        /// <summary>
        /// This method is used to trigger one touch event on the client.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Touch the screen of the client.\r\n\r\n" +
                    "2.Enter a return value, using a positive value for a successful operation or a negative value for a failed operation.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        public int TriggerOneTouchEventOnClient(string caseName)
        {
            this.rdprfxAdapter.Accept(rdpbcgrAdapter.ServerStack, rdpbcgrAdapter.SessionContext);
            this.rdprfxAdapter.ReceiveAndCheckClientCoreData();
            TS_RFX_ICAP[] supportedRfxCaps;
            this.rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out supportedRfxCaps);
            
            RdpeiUtility.SendInstruction(RdpeiSUTControlData.OneTouchEventControlData);

            return 1;
        }

        /// <summary>
        /// This method is used to trigger continuous touch events on the client.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Touch the screen several times to trigger touch events (at least touch 5 times).\r\n\r\n" +
                    "2.Enter a return value, using a positive value for a successful operation or a negative value for a failed operation.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        public int TriggerContinuousTouchEventOnClient(string caseName)
        {
            this.rdprfxAdapter.Accept(rdpbcgrAdapter.ServerStack, rdpbcgrAdapter.SessionContext);
            this.rdprfxAdapter.ReceiveAndCheckClientCoreData();
            TS_RFX_ICAP[] supportedRfxCaps;
            this.rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out supportedRfxCaps);

            RdpeiUtility.SendInstruction(RdpeiSUTControlData.ContinuousTouchEventControlData);

            return 1;
        }

        /// <summary>
        /// This method is used to trigger multitouch events on the client.
        /// </summary>
        /// <param name="contactCount">The number of multitouch contacts.</param>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.Touch the screen of the client with multiple touch points, the number of touch points is specified in the parameter contactCout.\r\n\r\n" +
                    "2.Enter a return value, using a positive value for a successful operation or a negative value for a failed operation.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        public int TriggerMultiTouchEventOnClient(string caseName, ushort contactCount)
        {
            this.rdprfxAdapter.Accept(rdpbcgrAdapter.ServerStack, rdpbcgrAdapter.SessionContext);
            this.rdprfxAdapter.ReceiveAndCheckClientCoreData();
            TS_RFX_ICAP[] supportedRfxCaps;
            this.rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out supportedRfxCaps);

            RdpeiUtility.SendInstruction(RdpeiSUTControlData.MultitouchEventControlData(contactCount));

            return 1;
        }

        /// <summary>
        /// This method is only used by managed adapter. This method is used to trigger touch events at specified position. 
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        public int TriggerPositionSpecifiedTouchEventOnClient(string caseName)
        {
            this.rdprfxAdapter.Accept(rdpbcgrAdapter.ServerStack, rdpbcgrAdapter.SessionContext);
            this.rdprfxAdapter.ReceiveAndCheckClientCoreData();
            TS_RFX_ICAP[] supportedRfxCaps;
            this.rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out supportedRfxCaps);

            RdpeiUtility.SendInstruction(RdpeiSUTControlData.PositionSpecifiedTouchEventControlData);

            return 1;
        }

        /// <summary>
        /// This method is used to trigger the RDPINPUT_DISMISS_HOVERING_CONTACT_PDU message.
        /// </summary>
        /// <returns>Negative values indicate the operation is failed, otherwise, successful.</returns>
        [MethodHelp("1.If your device supports proximity, trigger the RDPINPUT_DISMISS_HOVERING_CONTACT_PDU message on client, and enter a positive return value. \r\n\r\n" +
                    "2.If your device does not support proximity, enter a negative return value.\r\n\r\n" +
                    "3.To pass the value to the test case, click the Succeed button. Or, to end the test case, enter a message into the Failure Message dialog box and then click Fail.")]
        public int TriggerDismissHoveringContactPduOnClient(string caseName)
        {
            this.rdprfxAdapter.Accept(rdpbcgrAdapter.ServerStack, rdpbcgrAdapter.SessionContext);
            this.rdprfxAdapter.ReceiveAndCheckClientCoreData();
            TS_RFX_ICAP[] supportedRfxCaps;
            this.rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out supportedRfxCaps);

            RdpeiUtility.SendInstruction(RdpeiSUTControlData.DismissHoveringContactPduControlData);

            ushort btnLeft = (ushort)(rdpbcgrAdapter.CapabilitySetting.DesktopWidth - 160);
            ushort btnTop = (ushort)(rdpbcgrAdapter.CapabilitySetting.DesktopHeight - 120);
            RdpeiUtility.AddButton("Exit", btnLeft, btnTop);
            return 1;
        }

        #endregion
    }
}
