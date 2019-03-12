// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdp.Rdpefs;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    [TestClass]
    public partial class RdpbcgrTestSuite : RdpTestClassBase
    {
        #region Variables
        // Max allowed Desktop width
        private const ushort MaxDesktopWidth = 8192;
        // Max allowed Desktop Height
        private const ushort MaxDesktopHeight = 8192;
        // Desktop width used for Bitmap cap test
        private const ushort DesktopWidthForCapTest = 1024;
        // Desktop height used for Bitmap cap test
        private const ushort DesktopHeightForCapTest = 768;
        // Max allowed VCChunkSize value
        private const uint MaxVCChunkSize = 16256;
        // Large value of bitmapCodecCount
        private const byte LargeBitmapCodecCount = 10;
        // Large value of maxRequestSize
        private const uint LargeMaxRequestSize = 0x00400000;
        // Max Width and Height for pointer (large pointer not supported)
        private const ushort MaxPointerWidth = 32;
        private const ushort MaxPointerHeight = 32;
        // Pointer Width and Height for test
        private const ushort PointerWidth = 16;
        private const ushort PointerOddWidth = 31;
        private const ushort PointerHeight = 16;
        // pointer with and height for test pointer update
        private const ushort PointerWidthForUpdate = 30;
        private const ushort PointerHeightForUpdate = 20;
        // Point defined for testing pointer
        private TS_POINT16 PointerPos = new TS_POINT16(64, 64);
        private TS_POINT16 PointerPos2 = new TS_POINT16(128, 128);
        // Move step of Pointer.
        private const int PointerMoveStep = 10;
        // Random value generator
        private Random random = new Random();

        private IRdpefsAdapter rdpefsAdapter = null;
        #endregion Variables

        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            RdpTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            RdpTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialize and Cleanup       
        protected override void TestInitialize()
        {
            base.TestInitialize();

            // Turn on verification
            this.rdpbcgrAdapter.TurnVerificationOff(false);
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to close all RDP connections for clean up.");
            int iResult = this.sutControlAdapter.TriggerClientDisconnectAll(this.TestContext.TestName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "The result of TriggerClientDisconnectAll is {0}.", iResult);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Stop RDP listening.");
            this.rdpbcgrAdapter.StopRDPListening();

            if (this.rdpefsAdapter != null)
                this.rdpefsAdapter.Dispose();
        }
        #endregion


        /// <summary>
        /// Delete capability set from a capset collection according to capset id
        /// </summary>
        /// <param name="capSetCollection">Cap set collection</param>
        /// <param name="capType">Cap type</param>
        private void DeleteCapset(Collection<ITsCapsSet> capSetCollection, capabilitySetType_Values capType)
        {
            if (capSetCollection == null || capSetCollection.Count == 0)
            {
                return;
            }
            int i = 0;
            for (i = 0; i < capSetCollection.Count; i++)
            {
                if (capSetCollection[i].GetCapabilityType() == capType)
                {
                    capSetCollection.RemoveAt(i);
                    return;
                }
            }
            
        }

        /// <summary>
        /// Return a reasonable failureCode according to the receving Client_X_224_Connection_Request_Pdu.
        /// </summary>
        /// <param name="pdu">the Client_X_224_Connection_Request_Pdu</param>
        /// <returns></returns>
        private failureCode_Values GetFailureCode_RDP_NEG_FAILURE(Client_X_224_Connection_Request_Pdu pdu)
        {
            failureCode_Values result = failureCode_Values.NO_FAILURE;
            if (pdu == null)
                return result;

            bool has_PROTOCOL_RDP_FLAG = pdu.rdpNegData.requestedProtocols.HasFlag(requestedProtocols_Values.PROTOCOL_RDP_FLAG);
            bool has_PROTOCOL_SSL_FLAG = pdu.rdpNegData.requestedProtocols.HasFlag(requestedProtocols_Values.PROTOCOL_RDP_FLAG);
            bool has_PROTOCOL_HYBRID_FLAG = pdu.rdpNegData.requestedProtocols.HasFlag(requestedProtocols_Values.PROTOCOL_HYBRID_FLAG);
            bool has_PROTOCOL_HYBRID_EX = pdu.rdpNegData.requestedProtocols.HasFlag(requestedProtocols_Values.PROTOCOL_HYBRID_EX);

            if (!has_PROTOCOL_HYBRID_EX && !has_PROTOCOL_HYBRID_FLAG && has_PROTOCOL_SSL_FLAG)
            {
                // 0000 | 0001
                result = failureCode_Values.SSL_REQUIRED_BY_SERVER;
            }
            else if (!has_PROTOCOL_HYBRID_EX && !has_PROTOCOL_HYBRID_FLAG)
            {
                // 0010 | 0011
                result = failureCode_Values.HYBRID_REQUIRED_BY_SERVER;
            }
            else if (!has_PROTOCOL_SSL_FLAG)
            {
                // 1000 | 0100 | 1101 | 1100 | 1001 | 0101 | 1100
                result = failureCode_Values.SSL_WITH_USER_AUTH_REQUIRED_BY_SERVER;
            }
            else if (!has_PROTOCOL_RDP_FLAG)
            {
                // 1010 | 0110 | 1110
                result = failureCode_Values.SSL_CERT_NOT_ON_SERVER;
            }
            else
            {
                // 1011 | 0111 | 1111
                result = failureCode_Values.INCONSISTENT_FLAGS;
            }

            return result;
        }
    
        private void SendStaticVirtualChannelTraffics(StaticVirtualChannel_InvalidType invalidType)
        {
            if (this.rdpefsAdapter == null)
            {
                this.rdpefsAdapter = (IRdpefsAdapter)this.TestSite.GetAdapter(typeof(IRdpefsAdapter));
                this.rdpefsAdapter.Reset();
                this.rdpefsAdapter.AttachRdpbcgrAdapter(this.rdpbcgrAdapter);
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to sending ref traffics.");
            this.rdpefsAdapter.GenerateStaticVirtualChannelTraffics(invalidType);
        }
    }
}
;