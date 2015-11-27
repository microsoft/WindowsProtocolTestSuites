// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Dfsc;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.DFSC.TestSuite
{
    public class DFSCTestUtility
    {
        private ITestSite baseTestSite;
        private DFSCTestConfig testConfig;

        #region consts
        
        /// <summary>
        /// Const types for DFSC cases
        /// </summary>
        public static class Consts
        {
            /// <summary>
            /// Used for negative cases, stand for invalid path name.
            /// </summary>
            public const string InvalidComponent = "Invalid";
        }
        #endregion

        public DFSCTestUtility(ITestSite baseTestSite, DFSCTestConfig testConfig)
        {
            this.baseTestSite = baseTestSite;
            this.testConfig = testConfig;
        }

        #region General functions used by cases

        /// <summary>
        /// It's a general function used to connect server, create DFS referral packet and send packet.
        /// </summary>
        /// <param name="status">Returned status from server</param>
        /// <param name="client">Client instance</param>
        /// <param name="entryType">Version of DFS referral request</param>
        /// <param name="reqPath">The requested DFS Path to resolve</param>
        /// <param name="dcOrDFSServer">Server is DC or DFS server</param>
        /// <param name="isEx">The request is REQ_GET_DFS_REFERRAL_EX or REQ_GET_DFS_REFERRAL</param>
        /// <param name="containSiteName">If REQ_GET_DFS_REFERRAL_EX contains "SiteName" field</param>
        /// <returns>The response packet</returns>
        public DfscReferralResponsePacket SendAndReceiveDFSReferral(
            out uint status, 
            DfscClient client, 
            ReferralEntryType_Values entryType, 
            string reqPath, 
            bool dcOrDFSServer,
            bool isEx = false, 
            bool containSiteName = false)
        {
            string serverName;
            if (dcOrDFSServer)
            {
                serverName = testConfig.DCServerName;
            }
            else
            {
                serverName = testConfig.DFSServerName;
            }

            baseTestSite.Log.Add(LogEntryKind.Debug, "Server name is {0}", serverName);
            baseTestSite.Log.Add(LogEntryKind.Debug, "Request path is {0}", reqPath);
            if (isEx)
            {
                baseTestSite.Log.Add(LogEntryKind.Debug, "The message is extended.");
            }
            else
            {
                baseTestSite.Log.Add(LogEntryKind.Debug, "The message is NOT extended.");
            }
            if (containSiteName)
            {
                baseTestSite.Log.Add(LogEntryKind.Debug, "The structure contains SiteName.");
            }
            else
            {
                baseTestSite.Log.Add(LogEntryKind.Debug, "The structure does not contain SiteName.");
            }

            client.Connect(serverName, testConfig.ClientComputerName, testConfig.DomainName, testConfig.UserName, testConfig.UserPassword, testConfig.Timeout,
                testConfig.DefaultSecurityPackage, testConfig.UseServerGssToken, testConfig.TransportPreferredSMB);
            DfscReferralRequestPacket reqPacket = null;
            DfscReferralRequestEXPacket reqPacketEX = null;
            if (isEx)
            {
                if (containSiteName)
                {
                    reqPacketEX = client.CreateDfscReferralRequestPacketEX((ushort)entryType, reqPath, REQ_GET_DFS_REFERRAL_RequestFlags.SiteName, testConfig.SiteName);
                }
                else
                {
                    reqPacketEX = client.CreateDfscReferralRequestPacketEX((ushort)entryType, reqPath, REQ_GET_DFS_REFERRAL_RequestFlags.None);
                }
            }
            else
            {
                reqPacket = client.CreateDfscReferralRequestPacket((ushort)entryType, reqPath);
            }
            
            DfscReferralResponsePacket respPacket = client.SendAndRecieveDFSCReferralMessages(out status, testConfig.Timeout, reqPacketEX, reqPacket);
            return respPacket;
        }

        /// <summary>
        /// This function is only used to verify sysvol/root/link referral response.
        /// It can not verify the response of Domain/DC referral request.
        /// </summary>
        /// <param name="referralResponseType">Type of DFS referral response</param>
        /// <param name="entryType">Version of DFS referral</param>
        /// <param name="reqPath">The requested DFS Path to resolve</param>
        /// <param name="target">The resolved path</param>
        /// <param name="respPacket">Packet of DFS referral response</param>
        public void VerifyReferralResponse(
            ReferralResponseType referralResponseType, 
            ReferralEntryType_Values entryType, 
            string reqPath, 
            string target, 
            DfscReferralResponsePacket respPacket)
        {
            if (ReferralResponseType.DCReferralResponse == referralResponseType || ReferralResponseType.DomainReferralResponse == referralResponseType)
                return;

            baseTestSite.Assert.AreEqual((ushort)reqPath.Length * 2, respPacket.ReferralResponse.PathConsumed,
                "PathConsumed must be set to length in bytes of the DFS referral request path");

            if (ReferralEntryType_Values.DFS_REFERRAL_V1 == entryType)
            {
                // Section 3.2.5.5 Receiving a Root Referral Request or Link Referral Request
                // For a DFS referral version 1, the ReferralServers and StorageServers bits of the referral entry MUST be set to 1.
                // Section 3.3.5.4 Receiving a sysvol Referral Request
                // If the MaxReferralLevel field in the request is 1, the ReferralServers and StorageServers fields MUST be set to 1. 
                // Otherwise, the ReferralServers field MUST be set to 0 and the StorageServers field MUST be set to 1.
                baseTestSite.Assert.AreEqual(ReferralHeaderFlags.S, respPacket.ReferralResponse.ReferralHeaderFlags & ReferralHeaderFlags.S,
                    "For a DFS referral version 1, the StorageServers bit of the referral entry MUST be set to 1.");
                baseTestSite.Assert.AreEqual(ReferralHeaderFlags.R, respPacket.ReferralResponse.ReferralHeaderFlags & ReferralHeaderFlags.R,
                    "For a DFS referral version 1, the ReferralServers bit of the referral entry MUST be set to 1.");
            }
            else
            {
                // Section 3.2.5.5 Receiving a Root Referral Request or Link Referral Request
                // If DFS root targets are returned or if DFS link targets are returned, the StorageServers bit of the referral entry MUST be set to 1. 
                // In all other cases, it MUST be set to 0.
                // Section 3.3.5.4   Receiving a sysvol Referral Request
                // If the MaxReferralLevel field in the request is 1, the ReferralServers and StorageServers fields MUST be set to 1. 
                // Otherwise, the ReferralServers field MUST be set to 0 and the StorageServers field MUST be set to 1.

                // Check StorageServers bit
                if (ReferralResponseType.RootTarget == referralResponseType 
                 || ReferralResponseType.LinkTarget == referralResponseType
                 || ReferralResponseType.SysvolReferralResponse == referralResponseType)
                {
                    baseTestSite.Assert.AreEqual(ReferralHeaderFlags.S, respPacket.ReferralResponse.ReferralHeaderFlags & ReferralHeaderFlags.S,
                        "StorageServers bit of the referral entry MUST be set to 1.");
                }
                else
                {
                    baseTestSite.Assert.AreNotEqual(ReferralHeaderFlags.S, respPacket.ReferralResponse.ReferralHeaderFlags & ReferralHeaderFlags.S,
                        "StorageServers bit of the referral entry MUST be set to 0.");
                }

                // Check ReferralServers bit
                if (ReferralResponseType.RootTarget == referralResponseType 
                 || ReferralResponseType.Interlink == referralResponseType)
                {
                    baseTestSite.Assert.AreEqual(ReferralHeaderFlags.R, respPacket.ReferralResponse.ReferralHeaderFlags & ReferralHeaderFlags.R,
                        "ReferralServers bit of the referral entry MUST be set to 1.");
                }
                else
                {
                    baseTestSite.Assert.AreNotEqual(ReferralHeaderFlags.R, respPacket.ReferralResponse.ReferralHeaderFlags & ReferralHeaderFlags.R,
                        "ReferralServers bit of the referral entry MUST be set to 0.");
                }
            }

            uint timeToLive;
            bool containTarget = false;
            switch ((ReferralEntryType_Values)respPacket.VersionNumber)
            {
                case ReferralEntryType_Values.DFS_REFERRAL_V1:
                    DFS_REFERRAL_V1[] referralEntries_V1 = (DFS_REFERRAL_V1[])respPacket.ReferralResponse.ReferralEntries;
                    foreach (DFS_REFERRAL_V1 entry in referralEntries_V1)
                    {
                        baseTestSite.Assert.AreEqual((ushort)entryType, entry.VersionNumber, "VersionNumber must be set to " + entryType.ToString());
                        baseTestSite.Assert.AreEqual(0, entry.ReferralEntryFlags, "ReferralEntryFlags MUST be set to 0x0000 by the server and ignored on receipt by the client.");
                        if (ReferralResponseType.RootTarget == referralResponseType)
                        {
                            baseTestSite.Assert.AreEqual(0x0001, entry.ServerType, "The ServerType field MUST be set to 0x0001 if root targets are returned." +
                                "In all other cases, the ServerType field MUST be set to 0x0000.");
                        }
                        else
                        {
                            baseTestSite.Assert.AreEqual(0, entry.ServerType, "The ServerType field MUST be set to 0x0001 if root targets are returned." +
                                "In all other cases, the ServerType field MUST be set to 0x0000.");
                        }

                        baseTestSite.Log.Add(LogEntryKind.Debug, "ShareName is {0}", entry.ShareName);
                        if (!containTarget)
                            containTarget = target.Equals(entry.ShareName, StringComparison.OrdinalIgnoreCase);
                    }
                    break;

                case ReferralEntryType_Values.DFS_REFERRAL_V2:
                    DFS_REFERRAL_V2[] referralEntries_V2 = (DFS_REFERRAL_V2[])respPacket.ReferralResponse.ReferralEntries;
                    timeToLive = referralEntries_V2[0].TimeToLive;
                    foreach (DFS_REFERRAL_V2 entry in referralEntries_V2)
                    {
                        baseTestSite.Assert.AreEqual(timeToLive, entry.TimeToLive, "TimeToLive must be the same");
                        baseTestSite.Assert.AreEqual((ushort)entryType, entry.VersionNumber, "VersionNumber must be set to " + entryType.ToString());
                        baseTestSite.Assert.AreEqual(0, entry.ReferralEntryFlags, "ReferralEntryFlags MUST be set to 0x0000 by the server and ignored on receipt by the client.");
                        if (ReferralResponseType.RootTarget == referralResponseType)
                        {
                            baseTestSite.Assert.AreEqual(0x0001, entry.ServerType, "The ServerType field MUST be set to 0x0001 if root targets are returned." +
                                "In all other cases, the ServerType field MUST be set to 0x0000.");
                        }
                        else
                        {
                            baseTestSite.Assert.AreEqual(0, entry.ServerType, "The ServerType field MUST be set to 0x0001 if root targets are returned." +
                                "In all other cases, the ServerType field MUST be set to 0x0000.");
                        }
                        baseTestSite.Assert.IsTrue(reqPath.Equals(entry.DFSPath, StringComparison.OrdinalIgnoreCase), "DFSPath must be {0}, actual is {1}", reqPath, entry.DFSPath);
                        baseTestSite.Assert.IsTrue(reqPath.Equals(entry.DFSAlternatePath, StringComparison.OrdinalIgnoreCase), "DFSAlternatePath must be {0}, actual is {1}", reqPath, entry.DFSAlternatePath);
                        baseTestSite.Log.Add(LogEntryKind.Debug, "DFSTargetPath is {0}", entry.DFSTargetPath);

                        if (!containTarget)
                            containTarget = target.Equals(entry.DFSTargetPath, StringComparison.OrdinalIgnoreCase);
                        timeToLive = entry.TimeToLive;
                    }
                    break;

                case ReferralEntryType_Values.DFS_REFERRAL_V3:
                case ReferralEntryType_Values.DFS_REFERRAL_V4:

                    DFS_REFERRAL_V3V4_NonNameListReferral[] referralEntries_V3V4 = (DFS_REFERRAL_V3V4_NonNameListReferral[])respPacket.ReferralResponse.ReferralEntries;
                    timeToLive = referralEntries_V3V4[0].TimeToLive;

                    bool firstTarget = true;
                    foreach (DFS_REFERRAL_V3V4_NonNameListReferral entry in referralEntries_V3V4)
                    {
                        baseTestSite.Assert.AreEqual(timeToLive, entry.TimeToLive, "TimeToLive must be the same");
                        baseTestSite.Assert.AreEqual((ushort)entryType, entry.VersionNumber, "VersionNumber must be set to " + entryType.ToString());

                        if (ReferralResponseType.RootTarget == referralResponseType)
                        {
                            baseTestSite.Assert.AreEqual(0x0001, entry.ServerType, "The ServerType field MUST be set to 0x0001 if root targets are returned." +
                                "In all other cases, the ServerType field MUST be set to 0x0000.");
                        }
                        else
                        {
                            baseTestSite.Assert.AreEqual(0, entry.ServerType, "The ServerType field MUST be set to 0x0001 if root targets are returned." +
                                "In all other cases, the ServerType field MUST be set to 0x0000.");
                        }
                        baseTestSite.Assert.IsTrue(reqPath.Equals(entry.DFSPath, StringComparison.OrdinalIgnoreCase), "DFSPath must be {0}, actual is {1}", reqPath, entry.DFSPath);
                        baseTestSite.Assert.IsTrue(reqPath.Equals(entry.DFSAlternatePath, StringComparison.OrdinalIgnoreCase), "DFSAlternatePath must be {0}, actual is {1}", reqPath, entry.DFSAlternatePath);

                        baseTestSite.Log.Add(LogEntryKind.Debug, "DFSTargetPath is {0}", entry.DFSTargetPath);

                        if (!containTarget)
                        {
                            containTarget = target.Equals(entry.DFSTargetPath, StringComparison.OrdinalIgnoreCase);
                        }

                        timeToLive = entry.TimeToLive;

                        if (ReferralEntryType_Values.DFS_REFERRAL_V3 == (ReferralEntryType_Values)respPacket.VersionNumber)
                            continue; // skip TargetSetBoundary check for version 3.
                        if (firstTarget)
                        {
                            baseTestSite.Assert.AreEqual(ReferralEntryFlags_Values.T, entry.ReferralEntryFlags, "TargetSetBoundary MUST be set to 1 for the first target.");
                            firstTarget = false;
                        }
                        else
                        {
                            baseTestSite.Assert.AreEqual(ReferralEntryFlags_Values.None, entry.ReferralEntryFlags, "TargetSetBoundary MUST be set to 0 for other targets.");
                        }
                    }
                    break;

                default:
                    throw new InvalidOperationException("The version number of Referral Entry is not correct.");
            }

            baseTestSite.Assert.IsTrue(containTarget, "{0} response must contain {1}", Enum.GetName(typeof(ReferralResponseType), referralResponseType), target);
        }

        /// <summary>
        /// It's not allowed to send REQ_GET_DFS_REFERRAL_EX over SMB transport.
        /// </summary>
        public void CheckEXOverSMB()
        {
            if (testConfig.TransportPreferredSMB)
            {
                baseTestSite.Assert.Inconclusive("REQ_GET_DFS_REFERRAL_EX can not be sent over SMB");
            }
        }

        #endregion
    }
}

