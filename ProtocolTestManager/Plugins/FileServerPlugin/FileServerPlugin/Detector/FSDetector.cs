// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;

namespace Microsoft.Protocols.TestManager.FileServerPlugin
{
    /// <summary>
    /// IP address info for both driver and SUT
    /// </summary>
    public class NetworkInfo
    {
        public List<IPAddress> SUTIpList { get; set; }
        public List<IPAddress> LocalIpList { get; set; }
    }

    /// <summary>
    /// Basic info of SMB2 protocol
    /// </summary>
    public class Smb2Info
    {
        public DialectRevision MaxSupportedDialectRevision { get; set; }
        public Capabilities_Values SupportedCapabilities { get; set; }
        public EncryptionAlgorithm SelectedCipherID { get; set; }

        public bool IsRequireMessageSigning { get; set; }

        public CompressionAlgorithm[] SupportedCompressionAlgorithms { get; set; }
    }

    /// <summary>
    /// All info of shares in SUT
    /// </summary>
    public class ShareInfo
    {
        public string ShareName { get; set; }
        public ShareType_Values ShareType { get; set; }
        public Share_Capabilities_Values ShareCapabilities { get; set; }
        public ShareFlags_Values ShareFlags { get; set; }
    }

    /// <summary>
    /// FileServer detector, used to detect server capabilities.
    /// </summary>
    public partial class FSDetector
    {
        #region Properties

        public IPAddress SUTIpAddress { get; private set; }
        public SecurityPackageType SecurityPackageType { get; private set; }
        public AccountCredential Credential { get; private set; }

        #endregion

        #region Fields
        private Logger logWriter = null;
        private const string defautBasicShare = "SMBBasic";
        private const string vhdName = "plugin.vhdx";
        private const string fileNameSuffix = ":SharedVirtualDisk";
        private const int defaultTimeoutInSeconds = 20;
        private const string testSuiteName = "FileServer";

        // Get Service principal name may take a long time, so just get it once and save it.
        private string sutName = null;
        private bool alreadyGotDnsName = false;
        #endregion

        public string SUTName
        {
            get
            {
                IPAddress address;
                if (!alreadyGotDnsName
                    && !IPAddress.TryParse(sutName, out address)) // If the sutName is an IP address, no need to query DNS to get the entry
                {
                    try
                    {
                        sutName = Dns.GetHostEntry(sutName).HostName;
                        alreadyGotDnsName = true;
                    }
                    catch
                    {
                        logWriter.AddLog(LogLevel.Information, "GetHostEntry failed");
                    }
                }

                return sutName;
            }
        }
        public FSDetector
            (Logger logger,
            string targetSUT,
            AccountCredential accountCredential,
            SecurityPackageType securityPackageType)
        {
            sutName = targetSUT;
            Credential = accountCredential;
            SecurityPackageType = securityPackageType;

            logWriter = logger;
            logWriter.AddLog(LogLevel.Information, string.Format("SutName: {0}", sutName));
            logWriter.AddLog(LogLevel.Information, string.Format("DomainName: {0}", Credential.DomainName));
            logWriter.AddLog(LogLevel.Information, string.Format("UserName: {0}", Credential.AccountName));
            logWriter.AddLog(LogLevel.Information, string.Format("UserPassword: {0}", Credential.Password));
            logWriter.AddLog(LogLevel.Information, string.Format("SecurityPackageType: {0}", SecurityPackageType.ToString()));
            logWriter.AddLineToLog(LogLevel.Information);
        }

        public NetworkInfo DetectSUTConnection()
        {
            NetworkInfo networkInfo = new NetworkInfo();
            IPAddress address;
            //Detect SUT IP address by SUT name
            //If SUT name is an ip address, skip to resolve, use the ip address directly
            try
            {

                if (IPAddress.TryParse(sutName, out address))
                {
                    networkInfo.SUTIpList = new List<IPAddress>();
                    networkInfo.SUTIpList.Add(address);
                }
                else //DNS resolve the SUT IP address by SUT name
                {
                    IPAddress[] addList = Dns.GetHostAddresses(sutName);

                    if (null == addList)
                    {
                        logWriter.AddLog(LogLevel.Error, string.Format("The SUT name {0} is incorrect.", SUTName));
                    }

                    networkInfo.SUTIpList = new List<IPAddress>();
                    logWriter.AddLog(LogLevel.Information, "IP addresses returned from Dns.GetHostAddresses:");
                    foreach (var item in addList)
                    {
                        logWriter.AddLog(LogLevel.Information, item.ToString());
                        if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            networkInfo.SUTIpList.Add(item);
                        }
                    }

                    if (networkInfo.SUTIpList.Count == 0)
                    {
                        logWriter.AddLog(LogLevel.Error, string.Format("No available IP address resolved for target SUT {0}.", SUTName));
                    }
                }
                DetermineSUTIPAddress(networkInfo.SUTIpList.ToArray());

                return networkInfo;
            }
            catch
            {
                logWriter.AddLog(LogLevel.Error, string.Format("Detect Target SUT connection failed with SUT name: {0}.", SUTName));
                return null;
            }
        }

        private void DetermineSUTIPAddress(IPAddress[] ips)
        {
            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                bool canConnect = false;

                foreach (var ip in ips)
                {
                    canConnect = true;
                    try
                    {
                        client.ConnectOverTCP(ip);
                    }
                    catch (Exception ex)
                    {
                        canConnect = false;
                        logWriter.AddLog(LogLevel.Information, string.Format("Connect to IP {0} failed, reason: {1}", ip, ex.Message));
                    }
                    if (canConnect)
                    {
                        this.SUTIpAddress = ip;
                        break;
                    }
                }

                if (!canConnect)
                {
                    logWriter.AddLog(LogLevel.Error, string.Format("Can not connect to {0}.\r\nPlease check Target SUT.", SUTName));
                }
            }
        }

        public NetworkInfo FetchLocalNetworkInfo(DetectionInfo info)
        {
            LogIPConfig();

            //Get the network information with SUTIpList
            NetworkInfo networkInfo = info.networkInfo;

            #region Get Local IP List

            networkInfo.LocalIpList = new List<IPAddress>();

            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (adapter.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                    || adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                    || adapter.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet)
                {
                    foreach (var ip in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            using (Smb2Client smb2Client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
                            {
                                try
                                {
                                    smb2Client.ConnectOverTCP(SUTIpAddress, ip.Address);
                                    networkInfo.LocalIpList.Add(ip.Address);
                                }
                                catch (Exception ex)
                                {
                                    logWriter.AddLog(
                                        LogLevel.Information,
                                        string.Format("Connect from client IP {0} to SUT IP {1} failed, reason: {2}", ip.Address, SUTIpAddress, ex.Message));
                                }
                            }
                        }
                    }
                }
            }

            if (networkInfo.LocalIpList.Count == 0)
            {
                logWriter.AddLog(LogLevel.Error, "No available local IP address");
            }

            #endregion

            return networkInfo;
        }

        private void LogIPConfig()
        {
            logWriter.AddLog(LogLevel.Information, "ipconfig /all");
            string result = logWriter.RunCmdAndGetOutput("ipconfig /all");
            logWriter.AddLog(LogLevel.Information, result);
        }

        /// <summary>
        /// This method will send ComNegotiate request before sending a Negotiate request to simulate windows client behaviour.
        /// If ComNegotiate failed, the Negotiate request will still be sent.      
        /// </summary>
        public uint MultiProtocolNegotiate(
            Smb2Client client,
            ushort creditCharge,
            ushort creditRequest,
            Packet_Header_Flags_Values flags,
            ref ulong messageId,
            DialectRevision[] dialects,
            SecurityMode_Values securityMode,
            Capabilities_Values capabilities,
            Guid clientGuid,
            out DialectRevision selectedDialect,
            out byte[] gssToken,
            out Packet_Header responseHeader,
            out NEGOTIATE_Response responsePayload)
        {
            uint status = client.MultiProtocolNegotiate(
                    new string[] { "SMB 2.002", "SMB 2.???" },
                    out selectedDialect,
                    out gssToken,
                    out responseHeader,
                    out responsePayload);

            if (responseHeader.Status != Smb2Status.STATUS_SUCCESS)
            {
                LogFailedStatus("ComNegotiate", responseHeader.Status);
            }

            // If server only supports Smb2002, no further SMB2 negotiate needed
            if (selectedDialect == DialectRevision.Smb2002)
            {
                return status;
            }

            PreauthIntegrityHashID[] preauthHashAlgs = null;
            EncryptionAlgorithm[] encryptionAlgs = null;

            // For back compatibility, if dialects contains SMB 3.11, preauthentication integrity context should be present.
            if (Array.IndexOf(dialects, DialectRevision.Smb311) >= 0)
            {
                preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
                encryptionAlgs = new EncryptionAlgorithm[] {
                EncryptionAlgorithm.ENCRYPTION_AES128_GCM,
                EncryptionAlgorithm.ENCRYPTION_AES128_CCM };
            }

            status = client.Negotiate(
                creditCharge,
                creditRequest,
                flags,
                messageId,
                dialects,
                securityMode,
                capabilities,
                clientGuid,
                out selectedDialect,
                out gssToken,
                out responseHeader,
                out responsePayload,
                0,
                preauthHashAlgs,
                encryptionAlgs);

            // SMB2 negotiate is consume the message id
            messageId++;

            return status;
        }

        public void CheckUsernamePassword(DetectionInfo info)
        {
            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                Guid clientGuid;
                NEGOTIATE_Response negotiateResp;
                bool encryptionRequired;
                UserLogon(info, client, out messageId, out sessionId, out clientGuid, out negotiateResp, out encryptionRequired);

                try
                {
                    Packet_Header header;
                    LOGOFF_Response logoffResponse;
                    client.LogOff(
                        1,
                        1,
                        info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                        messageId++,
                        sessionId,
                        out header,
                        out logoffResponse);

                    if (header.Status != Smb2Status.STATUS_SUCCESS)
                    {
                        LogFailedStatus("LOGOFF", header.Status);
                    }
                }
                catch (Exception e)
                {
                    // Swallow all exceptions when cleaning up.
                    logWriter.AddLog(LogLevel.Information, "Exception in Cleanup: " + e.Message);
                }
            }
        }

        public void UserLogon(
            DetectionInfo info,
            Smb2Client client,
            out ulong messageId,
            out ulong sessionId,
            out Guid clientGuid,
            out NEGOTIATE_Response negotiateResp,
            out bool encryptionRequired)
        {
            messageId = 1;
            sessionId = 0;
            logWriter.AddLog(LogLevel.Information, "Client connects to server");
            client.ConnectOverTCP(SUTIpAddress);

            #region Negotiate

            DialectRevision selectedDialect;
            byte[] gssToken;
            Packet_Header header;
            clientGuid = Guid.NewGuid();
            logWriter.AddLog(LogLevel.Information, "Client sends multi-protocol Negotiate to server");
            MultiProtocolNegotiate(
                client,
                1,
                1,
                Packet_Header_Flags_Values.NONE,
                ref messageId,
                info.requestDialect,
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU
                | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES | Capabilities_Values.GLOBAL_CAP_ENCRYPTION,
                clientGuid,
                out selectedDialect,
                out gssToken,
                out header,
                out negotiateResp);

            if (header.Status != Smb2Status.STATUS_SUCCESS)
            {
                LogFailedStatus("NEGOTIATE", header.Status);
                throw new Exception(string.Format("NEGOTIATE failed with {0}", Smb2Status.GetStatusCode(header.Status)));
            }

            #endregion

            #region Session Setup
            SESSION_SETUP_Response sessionSetupResp;

            SspiClientSecurityContext sspiClientGss =
                new SspiClientSecurityContext(
                    SecurityPackageType,
                    Credential,
                    Smb2Utility.GetCifsServicePrincipalName(SUTName),
                    ClientSecurityContextAttribute.None,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);

            // Server GSS token is used only for Negotiate authentication when enabled
            if (SecurityPackageType == SecurityPackageType.Negotiate)
                sspiClientGss.Initialize(gssToken);
            else
                sspiClientGss.Initialize(null);

            do
            {
                logWriter.AddLog(LogLevel.Information, "Client sends SessionSetup to server");
                client.SessionSetup(
                    1,
                    64,
                    Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    SESSION_SETUP_Request_Flags.NONE,
                    SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                    0,
                    sspiClientGss.Token,
                    out sessionId,
                    out gssToken,
                    out header,
                    out sessionSetupResp);

                if ((header.Status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || header.Status == Smb2Status.STATUS_SUCCESS) && gssToken != null && gssToken.Length > 0)
                {
                    sspiClientGss.Initialize(gssToken);
                }
            } while (header.Status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED);

            if (header.Status != Smb2Status.STATUS_SUCCESS)
            {
                LogFailedStatus("SESSIONSETUP", header.Status);
                throw new Exception(string.Format("SESSIONSETUP failed with {0}", Smb2Status.GetStatusCode(header.Status)));
            }

            byte[] sessionKey;
            sessionKey = sspiClientGss.SessionKey;
            encryptionRequired = sessionSetupResp.SessionFlags == SessionFlags_Values.SESSION_FLAG_ENCRYPT_DATA;
            client.GenerateCryptoKeys(
                sessionId,
                sessionKey,
                info.smb2Info.IsRequireMessageSigning, // Enable signing according to the configuration of SUT
                encryptionRequired,
                null,
                false);

            #endregion
        }

        public void FetchPlatformAndUseraccounts(ref DetectionInfo info)
        {
            // Set the default values
            info.platform = Platform.NonWindows;
            info.nonadminUserAccounts = new List<string>();
            try
            {
                FetchPlatformInfo(ref info);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Information, "Detect platform failure. Reason: " + ex.Message);
            }
            try
            {
                FetchNonAdminUseraccountInfo(ref info);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Information, "Detect useraccounts failure. Reason: " + ex.Message);
            }
        }

        public Smb2Info FetchSmb2Info(DetectionInfo info)
        {
            Smb2Info smb2Info = new Smb2Info();

            using (Smb2Client smb2Client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                logWriter.AddLog(LogLevel.Information, "Client connects to server");
                smb2Client.ConnectOverTCP(SUTIpAddress);

                DialectRevision selectedDialect;
                byte[] gssToken;
                Packet_Header responseHeader;
                NEGOTIATE_Response responsePayload;
                ulong messageId = 1;
                logWriter.AddLog(LogLevel.Information, "Client sends multi-protocol Negotiate to server");
                MultiProtocolNegotiate(
                    smb2Client,
                    0,
                    1,
                    Packet_Header_Flags_Values.NONE,
                    ref messageId,
                    info.requestDialect,
                    SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_ENCRYPTION | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                    Guid.NewGuid(),
                    out selectedDialect,
                    out gssToken,
                    out responseHeader,
                    out responsePayload);

                if (responseHeader.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("NEGOTIATE", responseHeader.Status);
                    throw new Exception(string.Format("NEGOTIATE failed with {0}", Smb2Status.GetStatusCode(responseHeader.Status)));
                }

                smb2Info.MaxSupportedDialectRevision = responsePayload.DialectRevision;
                smb2Info.SupportedCapabilities = (Capabilities_Values)responsePayload.Capabilities;
                smb2Info.SelectedCipherID = smb2Client.SelectedCipherID;
                smb2Info.IsRequireMessageSigning = responsePayload.SecurityMode.HasFlag(NEGOTIATE_Response_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);
            }

            FetchSmb2CompressionInfo(smb2Info);

            return smb2Info;
        }

        private void FetchSmb2CompressionInfo(Smb2Info smb2Info)
        {
            if (smb2Info.MaxSupportedDialectRevision < DialectRevision.Smb311)
            {
                logWriter.AddLog(LogLevel.Information, "SMB dialect less than 3.1.1 does not support compression.");
                smb2Info.SupportedCompressionAlgorithms = new CompressionAlgorithm[0];
                return;
            }

            var possibleCompressionAlogrithms = new CompressionAlgorithm[] { CompressionAlgorithm.LZ77, CompressionAlgorithm.LZ77Huffman, CompressionAlgorithm.LZNT1 };

            // Iterate all possible compression algorithm for Windows will only return only one supported compression algorithm in response.
            var result = possibleCompressionAlogrithms.Where(compressionAlgorithm =>
            {
                using (var client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
                {
                    client.ConnectOverTCP(SUTIpAddress);

                    DialectRevision selectedDialect;
                    byte[] gssToken;
                    Packet_Header responseHeader;
                    NEGOTIATE_Response responsePayload;

                    uint status = client.Negotiate(
                        0,
                        1,
                        Packet_Header_Flags_Values.NONE,
                        0,
                        new DialectRevision[] { DialectRevision.Smb311 },
                        SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                        Capabilities_Values.NONE,
                        Guid.NewGuid(),
                        out selectedDialect,
                        out gssToken,
                        out responseHeader,
                        out responsePayload,
                        preauthHashAlgs: new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 },
                        compressionAlgorithms: new CompressionAlgorithm[] { compressionAlgorithm }
                        );

                    if (status == Smb2Status.STATUS_SUCCESS && client.CompressionInfo.CompressionIds.Length == 1 && client.CompressionInfo.CompressionIds[0] == compressionAlgorithm)
                    {
                        logWriter.AddLog(LogLevel.Information, $"Compression algorithm: {compressionAlgorithm} is supported by SUT.");
                        return true;
                    }
                    else
                    {
                        logWriter.AddLog(LogLevel.Information, $"Compression algorithm: {compressionAlgorithm} is not supported by SUT.");
                        return false;
                    }
                }
            });

            smb2Info.SupportedCompressionAlgorithms = result.ToArray();
        }

        public ShareInfo[] FetchShareInfo(DetectionInfo info)
        {
            string[] shareList = null;

            try
            {
                shareList = ServerHelper.EnumShares(SUTName, Credential.AccountName, Credential.DomainName, Credential.Password);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Information, string.Format("EnumShares failed, reason: {0}", ex.Message));
            }


            if (shareList == null)
            {
                // EnumShares may fail because the SUT doesn't support SRVS.
                // Try to connect the share which is input by the user in the "Target Share" field of Auto-Detection page.
                using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
                {
                    ulong messageId;
                    ulong sessionId;
                    uint treeId;
                    try
                    {
                        logWriter.AddLog(LogLevel.Information, string.Format("Try to connect share {0}.", info.BasicShareName));
                        ConnectToShare(info.BasicShareName, info, client, out messageId, out sessionId, out treeId);
                        shareList = new string[] { info.BasicShareName };
                    }
                    catch
                    {
                        // Show error to user.
                        logWriter.AddLog(LogLevel.Error, "Did not find shares on SUT. Please check share setting and SUT password.");
                    }
                }
            }

            return RetrieveShareProperties(shareList, info);
        }

        private ShareInfo[] RetrieveShareProperties(string[] shareList, DetectionInfo info)
        {
            List<ShareInfo> shareInfoList = new List<ShareInfo>();
            string uncShare;

            foreach (var share in shareList)
            {
                using (Smb2Client smb2Client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
                {
                    Packet_Header header;
                    ulong messageId;
                    ulong sessionId;
                    Guid clientGuid;
                    uncShare = string.Format(@"\\{0}\{1}", SUTName, share);
                    try
                    {
                        NEGOTIATE_Response negotiateResp;
                        bool encryptionRequired = false;
                        UserLogon(info, smb2Client, out messageId, out sessionId, out clientGuid, out negotiateResp, out encryptionRequired);
                        uint treeId;
                        TREE_CONNECT_Response treeConnectResp;

                        if (info.smb2Info.MaxSupportedDialectRevision == DialectRevision.Smb311) // When dialect is 3.11, TreeConnect must be signed or encrypted.
                        {
                            smb2Client.EnableSessionSigningAndEncryption(sessionId, true, encryptionRequired);
                        }

                        logWriter.AddLog(LogLevel.Information, string.Format("Client sends TreeConnect to {0} to retrieve the share properties.", uncShare));
                        smb2Client.TreeConnect(
                            1,
                            1,
                            (info.smb2Info.IsRequireMessageSigning || info.smb2Info.MaxSupportedDialectRevision == DialectRevision.Smb311) ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                            messageId++,
                            sessionId,
                            uncShare,
                            out treeId,
                            out header,
                            out treeConnectResp);

                        if (header.Status != Smb2Status.STATUS_SUCCESS)
                            continue;

                        // When dialect is 3.11, for the messages other than TreeConnect, signing is not required.
                        // Set it back to the configuration of the SUT.
                        if (info.smb2Info.MaxSupportedDialectRevision == DialectRevision.Smb311)
                        {
                            smb2Client.EnableSessionSigningAndEncryption(sessionId, info.smb2Info.IsRequireMessageSigning, encryptionRequired);
                        }

                        ShareInfo shareInfo = new ShareInfo();

                        shareInfo.ShareName = share;
                        shareInfo.ShareCapabilities = treeConnectResp.Capabilities;
                        shareInfo.ShareFlags = treeConnectResp.ShareFlags;
                        shareInfo.ShareType = treeConnectResp.ShareType;

                        shareInfoList.Add(shareInfo);

                        TREE_DISCONNECT_Response treeDisconnectResponse;
                        smb2Client.TreeDisconnect(
                            1,
                            1,
                            info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                            messageId++,
                            sessionId,
                            treeId,
                            out header,
                            out treeDisconnectResponse);

                        LOGOFF_Response logoffResponse;
                        smb2Client.LogOff(1, 1, Packet_Header_Flags_Values.NONE, messageId++, sessionId, out header, out logoffResponse);
                    }
                    catch (Exception ex)
                    {
                        logWriter.AddLog(LogLevel.Information, string.Format("Exception when retrieving share properties: " + ex.Message));
                        // Swallow all exceptions when cleaning up.
                    }
                }
            }

            return shareInfoList.ToArray();
        }

        private void LogFailedStatus(string operation, uint status)
        {
            logWriter.AddLog(LogLevel.Information, string.Format(operation + " failed, status: {0}", Smb2Status.GetStatusCode(status)));
        }

        /// <summary>
        /// Negotiate, SessionSetup, TreeConnect
        /// </summary>
        /// <returns>Return true for success, false for failure</returns>
        private void ConnectToShare(
            string sharename,
            DetectionInfo info,
            Smb2Client client,
            out ulong messageId,
            out ulong sessionId,
            out uint treeId)
        {
            Packet_Header header;
            Guid clientGuid;
            NEGOTIATE_Response negotiateResp;
            bool encryptionRequired = false;
            UserLogon(info, client, out messageId, out sessionId, out clientGuid, out negotiateResp, out encryptionRequired);

            #region TreeConnect

            TREE_CONNECT_Response treeConnectResp;
            string uncSharePath = Smb2Utility.GetUncPath(info.targetSUT, sharename);
            logWriter.AddLog(LogLevel.Information, "Client sends TreeConnect to server");
            if (info.smb2Info.MaxSupportedDialectRevision == DialectRevision.Smb311) // When dialect is 3.11, TreeConnect must be signed or encrypted.
            {
                client.EnableSessionSigningAndEncryption(sessionId, true, encryptionRequired);
            }

            client.TreeConnect(
                1,
                1,
                (info.smb2Info.IsRequireMessageSigning || info.smb2Info.MaxSupportedDialectRevision == DialectRevision.Smb311) ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                messageId++,
                sessionId,
                uncSharePath,
                out treeId,
                out header,
                out treeConnectResp);

            // When dialect is 3.11, for the messages other than TreeConnect, signing is not required.
            // Set it back to the configuration of the SUT.
            if (info.smb2Info.MaxSupportedDialectRevision == DialectRevision.Smb311)
            {
                client.EnableSessionSigningAndEncryption(sessionId, info.smb2Info.IsRequireMessageSigning, encryptionRequired);
            }

            if (header.Status != Smb2Status.STATUS_SUCCESS)
            {
                LogFailedStatus("TREECONNECT", header.Status);
                throw new Exception("TREECONNECT failed with " + Smb2Status.GetStatusCode(header.Status));
            }
            #endregion
        }

        private void FetchPlatformInfo(ref DetectionInfo info)
        {
            ManagementObjectCollection resultCollection = QueryWmiObject(SUTName, "SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject result in resultCollection)
            {
                info.platform = ConvertPlatform(result["Version"].ToString(), result["BuildNumber"].ToString());
                logWriter.AddLog(LogLevel.Information, "Platform: " + info.platform);
                break;
            }
        }

        private Platform ConvertPlatform(string osVersion, string buildNumber)
        {
            if (osVersion.StartsWith("10.0."))
            {
                int build;
                if (!Int32.TryParse(buildNumber, out build))
                {
                    // build number is empty or not a number
                    return Platform.WindowsServer2016;
                }

                if (build < 16299)
                {
                    return Platform.WindowsServer2016;
                }
                else if (build < 17134)
                {
                    return Platform.WindowsServerV1709;
                }
                else if (build < 17763)
                {
                    return Platform.WindowsServerV1803;
                }
                else if (build < 18362)
                {
                    return Platform.WindowsServer2019;
                }
                else
                {
                    return Platform.WindowsServerV1903;
                }
            }
            else if (osVersion.StartsWith("6.3."))
                return Platform.WindowsServer2012R2;
            else if (osVersion.StartsWith("6.2."))
                return Platform.WindowsServer2012;
            else if (osVersion.StartsWith("6.1."))
                return Platform.WindowsServer2008R2;
            else if (osVersion.StartsWith("6.0."))
                return Platform.WindowsServer2008;
            else
                return Platform.NonWindows;
        }

        private void FetchNonAdminUseraccountInfo(ref DetectionInfo info)
        {
            // If SUT is in Domain, get the Domain nonadmin useraccounts
            // If SUT is in WORKGROUP, get the SUT nonadmin useraccounts
            ManagementObjectCollection resultCollection = QueryWmiObject(info.domainName, "SELECT * FROM Win32_UserAccount");
            foreach (ManagementObject result in resultCollection)
            {
                #region Filter out administrator accounts according to SID

                string accountSID = result["SID"].ToString();
                if (accountSID.StartsWith("S-1-5-21"))
                {
                    // System administrator
                    if (accountSID.EndsWith("500"))
                        continue;
                    // Domain administrator
                    if (accountSID.EndsWith("512"))
                        continue;
                    // Schema administrator
                    if (accountSID.EndsWith("518"))
                        continue;
                    // Enterprise Administrator
                    if (accountSID.EndsWith("519"))
                        continue;
                    // Group Policy Creator Owners
                    if (accountSID.EndsWith("520"))
                        continue;
                }
                // Administrators (built-in group)
                if (accountSID.StartsWith("S-1-5-32") && accountSID.EndsWith("544"))
                    continue;

                #endregion

                // TODO: Further investigation required. 
                //  Find a better to filter out admin accounts
                //  Or check SID to confirm that no admin account omitted

                string userAccount = result["Name"].ToString();
                // Put the "nonadmin" user name to the first as the default value
                if (userAccount.ToLower() == "nonadmin")
                {
                    info.nonadminUserAccounts.Insert(0, userAccount);
                    continue;
                }
                // If get the "guest" use name, set it in the result
                // Keep the "guest" in the nonadmin useraccounts
                if (userAccount.ToLower() == "guest")
                    info.guestUserAccount = userAccount;

                info.nonadminUserAccounts.Add(userAccount);
            }
            logWriter.AddLog(LogLevel.Information, "Nonadmin Useraccounts: ");
            foreach (string account in info.nonadminUserAccounts)
            {
                logWriter.AddLog(LogLevel.Information, "\t" + account);
            }
        }

        private ManagementObjectCollection QueryWmiObject(string machineName, string queryString)
        {
            ConnectionOptions options = new ConnectionOptions() { Timeout = new TimeSpan(0, 0, 5) };
            ManagementScope ms = new ManagementScope("\\\\" + machineName + "\\root\\cimv2", options);
            ms.Connect();
            ObjectQuery query = new ObjectQuery(queryString);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(ms, query);
            return searcher.Get();
        }
    }
}