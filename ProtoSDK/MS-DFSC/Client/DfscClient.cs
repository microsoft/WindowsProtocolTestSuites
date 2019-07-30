// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;


namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Dfsc
{
    /// <summary>
    /// DfscClient is the exposed interface for testing Dfs server.
    /// API packet API and raw API.
    /// </summary>
    public sealed class DfscClient : IDisposable
    {
        #region Fields

        private FileServiceClientTransport transport;
        private bool disposed;

        #endregion


        #region Property

        /// <summary>
        /// To detect whether there are packets cached in the queue of Transport.
        /// Usually, it should be called after Disconnect to assure all events occurred in transport
        /// have been handled.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The transport is not connected.</exception>
        public bool IsDataAvailable
        {
            get
            {
                if (this.transport == null)
                {
                    throw new InvalidOperationException("The transport is not connected.");
                }

                return this.transport.IsDataAvailable;
            }
        }

        #endregion


        #region Constructor & Dispose

        /// <summary>
        /// Constructor
        /// </summary>
        /// <exception cref="System.ArgumentNullException">transport parameter is null</exception>
        public DfscClient()
        {
        }


        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                    if (this.transport != null)
                    {
                        this.transport.Dispose();
                        this.transport = null;
                    }
                }

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~DfscClient()
        {
            Dispose(false);
        }

        #endregion

        #region Packet Api

        /// <summary>
        /// Create DfsReferralRequest packet
        /// </summary>
        /// <param name="maxReferralLevel">
        /// A 16-bit integer that indicates the highest DFS referral version understood by the client.
        /// </param>
        /// <param name="fileName">
        /// Specify the UNC path or zero length string to be resolved.
        /// This parameter should not be null-terminated. This packet api adds null character automatically.
        /// </param>
        /// <exception cref="ArgumentException">fileName is not a valid string for DFS referral</exception>
        public DfscReferralRequestPacket CreateDfscReferralRequestPacket(ushort maxReferralLevel, string fileName)
        {

            /* Remove first backslash from fileName
             * To create valid string for the RequestFileName field of the REQ_DFS_REFERRAL_EX structure
             * Example:
             * "\\contoso.com\share\filepath" becomes "\contoso.com\share\filepath"
            */
      
            if (fileName.Length > 0 && fileName.StartsWith(@"\\") )
            {
                fileName = fileName.Substring(1);
            }
    
            fileName += '\0';
       

            //Create REQ_GET_DFS_REFERRAL structure and packet
            REQ_GET_DFS_REFERRAL referralRequest = new REQ_GET_DFS_REFERRAL();
            referralRequest.MaxReferralLevel = maxReferralLevel;
            referralRequest.RequestFileName = Encoding.Unicode.GetBytes(fileName);

            DfscReferralRequestPacket packet = new DfscReferralRequestPacket();
            packet.ReferralRequest = referralRequest;

            return packet;
        }

        /// <summary>
        /// Create DfsReferralRequest packet
        /// </summary>
        /// <param name="maxReferralLevel">
        /// A 16-bit integer that indicates the highest DFS referral version understood by the client.
        /// </param>
        /// <param name="fileName">
        /// Specifies the UNC path or zero length string to be resolved.
        /// This parameter should not be null-terminated. parameter length precedes parameter in structure to indicate bounds.
        /// 
        /// </param>
        /// <param name="flag">
        /// A 16 bit integer which indicates if client site name has been provided.
        /// "0x1": Site name included, "0x0": Site name is not included.
        /// </param>
        /// <param name="siteName">
        /// A string which specifies the client computer's Active Directory Domain Service site name.
        /// This parameter should not be null-terminated. parameter length precedes parameter in structure to indicate bounds.
        /// </param>
        /// <exception cref="ArgumentException">fileName is not a valid string for DFS referral</exception>
        /// 
        public DfscReferralRequestEXPacket CreateDfscReferralRequestPacketEX(ushort maxReferralLevel, string fileName, 
                                                                                    REQ_GET_DFS_REFERRAL_RequestFlags flag, string siteName = null)
        {
           
            /* Remove first backslash from fileName
             * To create valid string for the RequestFileName field of the REQ_DFS_REFERRAL_EX structure
             * Example:
             * "\\contoso.com\share\filepath" becomes "\contoso.com\share\filepath"
            */
            if (fileName.Length > 0 && fileName.StartsWith(@"\\"))
            {
                fileName = fileName.Substring(1);
            }

            fileName += '\0';

            //Build REQ_DFS_REFERRAL_EX structure
            REQ_GET_DFS_REFERRAL_EX DFSRequestEX = new REQ_GET_DFS_REFERRAL_EX();
            DFSRequestEX.MaxReferralLevel = maxReferralLevel;
            DFSRequestEX.RequestFlags = flag;

            REQ_GET_DFS_REFERRAL_RequestData requestData = new REQ_GET_DFS_REFERRAL_RequestData();
            requestData.RequestFileName = Encoding.Unicode.GetBytes(fileName);
            requestData.RequestFileNameLength = (ushort)requestData.RequestFileName.Length;

            DFSRequestEX.RequestDataLength = (uint)(requestData.RequestFileNameLength + 2);

            if (DFSRequestEX.RequestFlags == REQ_GET_DFS_REFERRAL_RequestFlags.SiteName)
            {
                if (siteName == null)
                {
                    throw new ArgumentNullException("siteName is null when flag is set");
                }

                requestData.SiteName = Encoding.Unicode.GetBytes(siteName);
                requestData.SiteNameLength = (ushort)requestData.SiteName.Length;
                DFSRequestEX.RequestDataLength += (uint)(requestData.SiteNameLength + 2);
            }

            DFSRequestEX.RequestData = requestData;

            //Build REQ_GET_DFS_REFERRAL_EX packet
            DfscReferralRequestEXPacket packet = new DfscReferralRequestEXPacket();
            packet.ReferralRequest = DFSRequestEX;

            return packet;

        }
        
        #endregion


        #region Raw Api

        /// <summary>
        /// Set up connection with server.
        /// Including 4 steps: 1. Tcp connection 2. Negotiation 3. SessionSetup 4. TreeConnect in order
        /// </summary>
        /// <param name="server">server name of ip address</param>
        /// <param name="client">client name of ip address</param>
        /// <param name="domain">user's domain</param>
        /// <param name="userName">user's name</param>
        /// <param name="password">user's password</param>
        /// <param name="timeout">The pending time to get server's response in step 2, 3 or 4</param>
        /// <exception cref="System.Net.ProtocolViolationException">Fail to set up connection with server</exception>
        public void Connect(
            string server,
            string client,
            string domain,
            string userName,
            string password,
            TimeSpan timeout,
            SecurityPackageType securityPackage,
            bool useServerToken,
            bool transportPreferredSMB)
        {
            if (transportPreferredSMB)
            {
                transport = new SmbClientTransport();
            }
            else
            {
                transport = new Smb2ClientTransport();
            }

            transport.Connect(server, client, domain, userName, password, timeout, securityPackage, useServerToken);
        } 

        /// <summary>
        /// Encode the packet to the specified transport protocol's payload. Then Send the request to server.
        /// </summary>
        /// <param name="packet">
        /// The REQ_GET_DFS_REFERRAL packet to be sent. 
        /// </param>
        /// <param name="EXpacket">
        /// The REQ_GET_DFS_REFERRAL_EX packet to be sent.
        /// MUST be null if transport is not SMB2 or SMB3
        /// </param>
        /// <exception cref="System.ArgumentNullException">The packet to be sent is null.</exception>
        /// <exception cref="System.InvalidOperationException">The transport is not connected.</exception>
        public void SendPacket(DfscReferralRequestEXPacket EXpacket = null, DfscReferralRequestPacket packet = null)
        {
            if (packet == null && EXpacket == null)
            {
                throw new ArgumentNullException("packet is null");
            } 
            else if (packet == null)
            {
                this.transport.SendDfscPayload(EXpacket.ToBytes(), true);
            }
            else
            {
                this.transport.SendDfscPayload(packet.ToBytes(), false);
            }
        }


        /// <summary>
        /// Encode the packet to the specified transport protocol's payload. Then Send the request to server.
        /// </summary>
        /// <param name="payload">The DFSC payload in byte array</param>
        /// <param name="isEX">
        /// Boolean which indicates whether payload contains DFS_GET_REFERRAL_EX message
        /// </param>
        /// <exception cref="System.ArgumentNullException">The packet to be sent is null.</exception>
        /// <exception cref="System.InvalidOperationException">The transport is not connected.</exception>
        public void SendBytes(byte[] payload, bool isEX)
        {
            if (payload == null)
            {
                throw new ArgumentNullException("payload is null");
            }
            if (isEX)
            {
                this.transport.SendDfscPayload(payload, true);
            }
            else
            {
                this.transport.SendDfscPayload(payload, false);
            }
        }


        /// <summary>
        /// Wait for the Dfs response packet from server.
        /// </summary>
        /// <param name="timeout">the pending time to get server's response</param>
        /// <param name="ntStatus">the status of the dfs referral response from the server.</param>
        /// <exception cref="System.InvalidOperationException">The transport is not connected.</exception>
        public DfscReferralResponsePacket ExpectPacket(TimeSpan timeout, out uint ntStatus)
        {
            byte[] payload;

            this.transport.ExpectDfscPayload(timeout, out ntStatus, out payload);
            DfscReferralResponsePacket packet = new DfscReferralResponsePacket(ntStatus, payload);
            return packet;
        }


        /// <summary>
        /// Disconnect from server.
        /// Including 3 steps: 1. TreeDisconnect 2. Logoff 3. Tcp disconnection in order.
        /// </summary>
        /// <param name="timeout">The pending time to get server's response in step 1 or 2</param>
        /// <exception cref="System.Net.ProtocolViolationException">Fail to disconnect from server</exception>
        /// <exception cref="System.InvalidOperationException">The transport is not connected</exception>
        public void Disconnect(TimeSpan timeout)
        {
            if (transport != null)
            {
                this.transport.Disconnect(timeout);
            }
        }

        /// <summary>
        /// Retrieves the DFS Referrals from the RESP_GET_DFS_REFERRALS packet
        /// </summary>
        /// <typeparam name="T">The DFS referral response version expected to be returned</typeparam>
        /// <param name="dfscReferralResponse">The packet to retrieve the referrals</param>
        /// <param name="expectedVersion">The expected version of the referral responses</param>
        /// <returns>Returns the first DFS referral in the response packet</returns>
        public T[] RetrieveReferralEntries<T>(DfscReferralResponsePacket dfscReferralResponse, int expectedVersion)
            where T : struct
        {

            switch ((ReferralEntryType_Values)dfscReferralResponse.VersionNumber)
            {
                case ReferralEntryType_Values.DFS_REFERRAL_V1:
                    DFS_REFERRAL_V1[] referralEntries_V1 = (DFS_REFERRAL_V1[])dfscReferralResponse.ReferralResponse.ReferralEntries;
                    foreach (DFS_REFERRAL_V1 entry in referralEntries_V1)
                    {
                        entry.ToString();
                    }

                    return (T[])(object)referralEntries_V1;

                case ReferralEntryType_Values.DFS_REFERRAL_V2:
                    DFS_REFERRAL_V2[] referralEntries_V2 = (DFS_REFERRAL_V2[])dfscReferralResponse.ReferralResponse.ReferralEntries;
                    foreach (DFS_REFERRAL_V2 entry in referralEntries_V2)
                    {
                        entry.ToString();
                    }
                    return (T[])(object)referralEntries_V2;

                case ReferralEntryType_Values.DFS_REFERRAL_V3:
                    expectedVersion++; //To automatically pass V4 site.Assertion
                    goto case ReferralEntryType_Values.DFS_REFERRAL_V4;

                case ReferralEntryType_Values.DFS_REFERRAL_V4:
                    if (dfscReferralResponse.IsNameListReferral)
                    {
                        DFS_REFERRAL_V3V4_NameListReferral[] referralEntries_V3V4_NameListReferral = (DFS_REFERRAL_V3V4_NameListReferral[])dfscReferralResponse.ReferralResponse.ReferralEntries;
                        foreach (DFS_REFERRAL_V3V4_NameListReferral entry in referralEntries_V3V4_NameListReferral)
                        {
                            entry.ToString();
                        }
                        return (T[])(object)referralEntries_V3V4_NameListReferral;
                    }
                    else
                    {
                        DFS_REFERRAL_V3V4_NonNameListReferral[] referralEntries_V3V4_NonNameListReferral = (DFS_REFERRAL_V3V4_NonNameListReferral[])dfscReferralResponse.ReferralResponse.ReferralEntries;
                        foreach (DFS_REFERRAL_V3V4_NonNameListReferral entry in referralEntries_V3V4_NonNameListReferral)
                        {
                            entry.ToString();
                        }
                        return (T[])(object)referralEntries_V3V4_NonNameListReferral;
                    }

                default:
                    throw new InvalidOperationException("The version number of Referral Entry is not correct.");

            }
        }

        /// <summary>
        /// Retrieves the DFS Referrals from the RESP_GET_DFS_REFERRALS packet
        /// </summary>
        /// <typeparam name="T">The DFS referral response version expected to be returned</typeparam>
        /// <param name="dfscReferralResponse">The packet to retrieve the referrals</param>
        /// <param name="expectedVersion">The expected version of the referral responses</param>
        /// <returns>Returns the first DFS referral in the response packet</returns>
        public T RetrieveReferralEntriesFirstTarget<T>(DfscReferralResponsePacket dfscReferralResponse, int expectedVersion)
            where T : struct
        {

            return (T)(RetrieveReferralEntries<T>(dfscReferralResponse, expectedVersion))[0];
        }

        /// <summary>
        /// Sends a REQ_GET_DFS_REFERRAL or REQ_GET_DFS_REFERRAL_EX to the server and captures the RESP_GET_DFS_REFERRAL response.
        /// </summary>
        /// <param name="packetEX">Optional REQ_GET_DFS_REFERRAL_EX packet to be sent to the server</param>
        /// <param name="packet">Optional REQ_GET_DFS_REFERRAL packet to be sent to the server</param>
        /// <exception cref="ArgumentNullException">There must be one non null request packet.</exception>
        /// <returns></returns>
        public DfscReferralResponsePacket SendAndRecieveDFSCReferralMessages(out uint status, TimeSpan timeout, DfscReferralRequestEXPacket packetEX = null, DfscReferralRequestPacket packet = null)
        {
            SendPacket(packetEX, packet);

            return ExpectPacket(timeout, out status);
        }

        #endregion
    }
}