// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestManager.BranchCachePlugin
{
    #region public class

    /// <summary>
    /// Indicate network related information: SUTIplist and LocalIpList
    /// </summary>
    public class NetworkInfo
    {
        public List<IPAddress> SUTIpList { get; set; }
        public List<IPAddress> LocalIpList { get; set; }
    }

    /// <summary>
    /// Indicate share related branchcache information: Sharename and shareHashGeneration
    /// </summary>
    public class ShareInfo
    {
        public string ShareName { get; set; }
        public ShareHashGeneration shareHashGeneration { get; set; }
    }

    /// <summary>
    /// Indicate branchcache version supported information
    /// </summary>
    public class VersionInfo
    {
        public BranchCacheVersion branchCacheVersion { get; set; }
    }

    #endregion

    /// <summary>
    /// BranchCache detector used to detect branch cache related information
    /// </summary>
    public class BranchCacheDetector
    {
        #region fields and properties

        private Queue<ulong> ioctlRequestMessageIds = new Queue<ulong>();
        private DetectionInfo detectionInfo = new DetectionInfo();
        private const string defaultShare = "FileShare";
        private const int defaultTimeoutInSeconds = 60;
        public Logger logWriter = null;
        public string ContentServerName { get; private set; }
        public string HostedCacheServerName { get; private set; }
        public IPAddress SUTIpAddress { get; private set; }
        public SecurityPackageType SecurityPackageType { get; private set; }
        public AccountCredential Credential { get; private set; }
        public List<Smb2Client> ClientList = null;

        #endregion

        #region public methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Enanle the log while detector start</param>
        public BranchCacheDetector(Logger logger)
        {
            logWriter = logger;
            logWriter.AddLineToLog(LogLevel.Information);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Enable the log</param>
        /// <param name="contentServerName">The ContentServer name</param>
        /// <param name="hostedCacheServerName">The HostedCacheServer name</param>
        /// <param name="accountCredential">Account credential</param>
        /// <param name="securityPackageType">security package: NTLM, Negotiate, kerbrose</param>
        public BranchCacheDetector(Logger logger, string contentServerName, string hostedCacheServerName, AccountCredential accountCredential, SecurityPackageType securityPackageType = SecurityPackageType.Negotiate)
        {
            ContentServerName = contentServerName;
            HostedCacheServerName = hostedCacheServerName;
            Credential = accountCredential;
            SecurityPackageType = securityPackageType;

            logWriter = logger;
            logWriter.AddLog(LogLevel.Information, string.Format("ContentServerName: {0}", ContentServerName));
            logWriter.AddLog(LogLevel.Information, string.Format("HostedCacheServerName: {0}", HostedCacheServerName));
            logWriter.AddLog(LogLevel.Information, string.Format("DomainName: {0}", Credential.DomainName));
            logWriter.AddLog(LogLevel.Information, string.Format("UserName: {0}", Credential.AccountName));
            logWriter.AddLog(LogLevel.Information, string.Format("UserPassword: {0}", Credential.Password));
            logWriter.AddLog(LogLevel.Information, string.Format("SecurityPackageType: {0}", SecurityPackageType.ToString()));
            logWriter.AddLineToLog(LogLevel.Information);
        }

        /// <summary>
        /// Ping target SUT
        /// </summary>
        /// <returns></returns>
        public NetworkInfo PingTargetSUT(string targetSUT)
        {
            NetworkInfo networkInfo = new NetworkInfo();
            IPAddress[] addList = Dns.GetHostAddresses(targetSUT);

            if (null == addList)
            {
                logWriter.AddLog(LogLevel.Error, string.Format("Failed to get IPAddress from SUT name {0}.", targetSUT));
            }

            networkInfo.SUTIpList = new List<IPAddress>();
            logWriter.AddLog(LogLevel.Information, "IP address returned from Dns.GetHostAddresses:");
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
                logWriter.AddLog(LogLevel.Error, string.Format("No available IP address on target SUT {0}.", targetSUT));
            }

            if (!DetermineSUTIPAddress(networkInfo.SUTIpList.ToArray()))
            {
                networkInfo.SUTIpList = new List<IPAddress>();
            }

            return networkInfo;
        }

        /// <summary>
        /// Get local network information
        /// </summary>
        /// <param name="info">The detection information</param>
        /// <returns></returns>
        public NetworkInfo FetchLocalNetworkInfo(DetectionInfo info)
        {
            LogIPConfig();

            NetworkInfo networkInfo = info.ContentServerNetworkInformation;

            #region Get local IP list

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
                            Smb2Client smb2Client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds));
                            AddToClientList(smb2Client);
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

            if (networkInfo.LocalIpList.Count == 0)
            {
                logWriter.AddLog(LogLevel.Error, "No available local IP address");
            }

            #endregion

            return networkInfo;

        }

        /// <summary>
        /// Check whether user can log on with the credential
        /// </summary>
        /// <param name="info">The detection information</param>
        public void CheckUsernamePassword(DetectionInfo info)
        {
            Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds));
            AddToClientList(client);
            ulong messageId;
            ulong sessionId;
            Guid clientGuid;
            NEGOTIATE_Response negotiateResp;
            if (!UserLogon(info, client, out messageId, out sessionId, out clientGuid, out negotiateResp))
                return;

            try
            {
                Packet_Header header;
                LOGOFF_Response logoffResponse;
                client.LogOff(1, 1, Packet_Header_Flags_Values.FLAGS_SIGNED, messageId++, sessionId, out header, out logoffResponse);

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

        /// <summary>
        /// Get platform information
        /// </summary>
        /// <param name="info">The detection information</param>
        public void FetchPlatform(DetectionInfo info)
        {
            // Set the default values
            info.SUTPlatform = Platform.NonWindows;

            try
            {
                ManagementObjectCollection resultCollection = QueryWmiObject(info.TargetSUT, "SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject result in resultCollection)
                {
                    info.SUTPlatform = ConvertToPlatform(result["Caption"].ToString());
                    logWriter.AddLog(LogLevel.Information, "Platform: " + info.SUTPlatform);
                    break;
                }
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Information, "Detect platform failure. Reason：" + ex.Message);
            }
        }

        /// <summary>
        /// Get share information
        /// </summary>
        /// <param name="info">The detection information</param>
        /// <returns></returns>
        public ShareInfo FetchShareInfo(DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detect Share Info =====");
            logWriter.AddLog(LogLevel.Information, "Share name: " + defaultShare);

            Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds));
            AddToClientList(client);
            Packet_Header header;
            Guid clientGuid;
            NEGOTIATE_Response negotiateResp;
            ulong messageId = 0;
            ulong sessionId = 0;
            uint treeId = 0;
            try
            {
                UserLogon(info, client, out messageId, out sessionId, out clientGuid, out negotiateResp);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, Detector.LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Information);
                logWriter.AddLog(LogLevel.Error, string.Format("User log on failed: {0}", ex.Message));
            }

            detectionInfo.ResetDetectResult();

            #region TreeConnect

            TREE_CONNECT_Response treeConnectResp;
            string uncSharePath = Smb2Utility.GetUncPath(info.ContentServerName, defaultShare);
            client.TreeConnect(
                1,
                1,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId++,
                sessionId,
                uncSharePath,
                out treeId,
                out header,
                out treeConnectResp);

            if (header.Status != Smb2Status.STATUS_SUCCESS)
            {
                LogFailedStatus("TREECONNECT", header.Status);
                throw new Exception("TREECONNECT failed with " + Smb2Status.GetStatusCode(header.Status));
            }

            ShareInfo shareInfo = new ShareInfo();
            shareInfo.ShareName = uncSharePath;
            shareInfo.shareHashGeneration = ShareHashGeneration.NotEnabled;
            if (treeConnectResp.ShareFlags.HasFlag(ShareFlags_Values.SHAREFLAG_ENABLE_HASH_V1))
            {
                shareInfo.shareHashGeneration = ShareHashGeneration.V1Enabled;
            }
            if (treeConnectResp.ShareFlags.HasFlag(ShareFlags_Values.SHAREFLAG_ENABLE_HASH_V2))
            {
                shareInfo.shareHashGeneration |= ShareHashGeneration.V2Enabled;
            }
            #endregion

            try
            {
                LOGOFF_Response logoffResponse;
                client.LogOff(1, 1, Packet_Header_Flags_Values.FLAGS_SIGNED, messageId++, sessionId, out header, out logoffResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("LOGOFF", header.Status);
                }
            }
            catch (Exception e)
            {
                logWriter.AddLog(LogLevel.Information, "Exception in Cleanup: " + e.Message);
            }

            return shareInfo;
        }

        /// <summary>
        /// Get branchcache version supported information, which version SUT supports depends on the IOCTL_READ_HASH response code
        /// </summary>
        /// <param name="info">The detection information</param>
        /// <returns></returns>
        public VersionInfo FetchVersionInfo(DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detect Version Info =====");

            Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds));
            AddToClientList(client);
            Packet_Header header;
            Guid clientGuid;
            NEGOTIATE_Response negotiateResp;
            ulong messageId = 1;
            ulong sessionId = 0;
            uint treeId = 0;
            try
            {
                UserLogon(info, client, out messageId, out sessionId, out clientGuid, out negotiateResp);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Warning, "Failed", false, Detector.LogStyle.StepFailed);
                logWriter.AddLineToLog(LogLevel.Information);
                logWriter.AddLog(LogLevel.Error, string.Format("User log on failed: {0}", ex.Message));
            }

            detectionInfo.ResetDetectResult();

            #region TreeConnect

            TREE_CONNECT_Response treeConnectResp;
            string uncSharePath = Smb2Utility.GetUncPath(info.ContentServerName, defaultShare);
            client.TreeConnect(
                1,
                1,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId++,
                sessionId,
                uncSharePath,
                out treeId,
                out header,
                out treeConnectResp);

            if (header.Status != Smb2Status.STATUS_SUCCESS)
            {
                LogFailedStatus("TREECONNECT", header.Status);
                throw new Exception("TREECONNECT failed with " + Smb2Status.GetStatusCode(header.Status));
            }
            #endregion

            CREATE_Response createResp;
            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            VersionInfo versionInfo = new VersionInfo();
            versionInfo.branchCacheVersion = BranchCacheVersion.NotSupported;
            string fileName = "MultipleBlocks.txt";
            client.Create(
                1,
                1,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId++,
                sessionId,
                treeId,
                fileName,
                AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE,
                ShareAccess_Values.NONE,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                CreateDisposition_Values.FILE_OPEN_IF,
                File_Attributes.NONE,
                ImpersonationLevel_Values.Impersonation,
                SecurityFlags_Values.NONE,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                null,
                out fileId,
                out serverCreateContexts,
                out header,
                out createResp);

            HASH_HEADER hashHeader;
            byte[] hashData = null;

            // Trigger to generate Content Information V1
            uint status = 0;
            status = ReadHash(
                client,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId++,
                treeId,
                sessionId,
                fileId,
                SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_1,
                SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_HASH_BASED,
                0,
                uint.MaxValue,
                out hashHeader,
                out hashData);

            // Retrieve Content Information V1
            status = ReadHash(
                client,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId++,
                treeId,
                sessionId,
                fileId,
                SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_1,
                SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_HASH_BASED,
                0,
                uint.MaxValue,
                out hashHeader,
                out hashData);

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                LogFailedStatus("READ_HASH_V1", header.Status);
            }
            else
            {
                versionInfo.branchCacheVersion = BranchCacheVersion.BranchCacheVersion1;
            }

            // Trigger to generate Content Information V2
            status = ReadHash(
                client,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId++,
                treeId,
                sessionId,
                fileId,
                SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_2,
                SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_FILE_BASED,
                0,
                uint.MaxValue,
                out hashHeader,
                out hashData);

            status = ReadHash(
                client,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId++,
                treeId,
                sessionId,
                fileId,
                SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_2,
                SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_FILE_BASED,
                0,
                uint.MaxValue,
                out hashHeader,
                out hashData);

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                LogFailedStatus("READ_HASH_V2", header.Status);
            }
            else
            {
                versionInfo.branchCacheVersion |= BranchCacheVersion.BranchCacheVersion2;
            }

            try
            {
                LOGOFF_Response logoffResponse;
                client.LogOff(1, 1, Packet_Header_Flags_Values.FLAGS_SIGNED, messageId++, sessionId, out header, out logoffResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("LOGOFF", header.Status);
                }
            }
            catch (Exception e)
            {
                logWriter.AddLog(LogLevel.Information, "Exception in Cleanup: " + e.Message);
            }

            return versionInfo;
        }

        #endregion

        #region private methods

        private Platform ConvertToPlatform(string platform)
        {
            if (platform.Contains("Windows Server 2012 R2"))
                return Platform.WindowsServer2012R2;
            else if (platform.Contains("Windows Server 2012"))
                return Platform.WindowsServer2012;
            else if (platform.Contains("Windows Server 2008 R2"))
                return Platform.WindowsServer2008R2;
            else if (platform.Contains("Windows Serer 2008"))
                return Platform.WindowsServer2008;
            else
                return Platform.NonWindows;
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

        private void LogFailedStatus(string operation, uint status)
        {
            logWriter.AddLog(LogLevel.Information, string.Format(operation + " failed, status: {0}", Smb2Status.GetStatusCode(status)));
        }

        private bool DetermineSUTIPAddress(IPAddress[] ips)
        {
            bool canConnect = false;
            foreach (var ip in ips)
            {
                try
                {
                    Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds));
                    AddToClientList(client);
                    client.ConnectOverTCP(ip);
                    canConnect = true;
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
                logWriter.AddLog(LogLevel.Error, string.Format("Can not connect to {0}.\r\nPlease check Target SUT.", ContentServerName));
            }

            return canConnect;
        }

        private void LogIPConfig()
        {
            logWriter.AddLog(LogLevel.Information, "ipconfig /all");
            string result = logWriter.RunCmdAndGetOutput("ipconfig /all");
            logWriter.AddLog(LogLevel.Information, result);
        }

        private bool UserLogon(DetectionInfo info, Smb2Client client, out ulong messageId, out ulong sessionId, out Guid clientGuid, out NEGOTIATE_Response negotiateResp)
        {
            messageId = 0;
            sessionId = 0;
            client.ConnectOverTCP(Dns.GetHostAddresses(info.ContentServerName)[0]);

            #region Negotiate

            DialectRevision selectedDialect;
            byte[] gssToken;
            Packet_Header header;
            clientGuid = Guid.NewGuid();

            client.Negotiate(
                1,
                1,
                Packet_Header_Flags_Values.NONE,
                messageId++,
                new DialectRevision[] { DialectRevision.Smb30 },
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
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
                    Smb2Utility.GetCifsServicePrincipalName(ContentServerName),
                    ClientSecurityContextAttribute.None,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);

            // Server GSS token is used only for Negotiate authentication when enabled
            if (SecurityPackageType == SecurityPackageType.Negotiate)
                sspiClientGss.Initialize(gssToken);
            else
                sspiClientGss.Initialize(null);

            do
            {
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
            client.GenerateCryptoKeys(sessionId, sessionKey, true, false, null, false);

            #endregion

            return true;
        }

        private uint ReadHash(
            Smb2Client client,
            Packet_Header_Flags_Values headerFlags,
            ulong messageId,
            uint treeId,
            ulong sessionId,
            FILEID fileId,
            SRV_READ_HASH_Request_HashType_Values hashType,
            SRV_READ_HASH_Request_HashVersion_Values hashVersion,
            SRV_READ_HASH_Request_HashRetrievalType_Values hashRetrievalType,
            ulong offset,
            uint length,
            out HASH_HEADER hashHeader,
            out byte[] hashData)
        {
            hashHeader = new HASH_HEADER();
            hashData = null;

            SRV_READ_HASH_Request readHashRequest = new SRV_READ_HASH_Request();
            readHashRequest.HashType = hashType;
            readHashRequest.HashVersion = hashVersion;
            readHashRequest.HashRetrievalType = hashRetrievalType;
            readHashRequest.Offset = offset;
            readHashRequest.Length = length;

            byte[] requestInput = TypeMarshal.ToBytes(readHashRequest);
            byte[] responseOutput;

            uint status;
            SendIoctlPayload(client, CtlCode_Values.FSCTL_SRV_READ_HASH, requestInput, Packet_Header_Flags_Values.FLAGS_SIGNED, messageId, treeId, sessionId, fileId);
            ExpectIoctlPayload(client, out status, out responseOutput);

            if (status != Smb2Status.STATUS_SUCCESS)
                return status;

            byte[] hashHeaderDataBuffer = null;
            switch (hashRetrievalType)
            {
                case SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_HASH_BASED:
                    hashHeaderDataBuffer = TypeMarshal.ToStruct<SRV_HASH_RETRIEVE_HASH_BASED>(responseOutput).Buffer;
                    break;

                case SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_FILE_BASED:
                    hashHeaderDataBuffer = TypeMarshal.ToStruct<SRV_HASH_RETRIEVE_FILE_BASED>(responseOutput).Buffer;
                    break;

                default:
                    throw new NotImplementedException(hashRetrievalType.ToString());
            }

            int hashHeaderLength = 0;
            hashHeader = TypeMarshal.ToStruct<HASH_HEADER>(hashHeaderDataBuffer, ref hashHeaderLength);
            hashData = hashHeaderDataBuffer.Skip(hashHeaderLength).ToArray();

            return status;
        }

        private void SendIoctlPayload(
            Smb2Client client,
            CtlCode_Values code,
            byte[] payload,
            Packet_Header_Flags_Values headerFlags,
            ulong messageId,
            uint treeId,
            ulong sessionId,
            FILEID fileId)
        {
            if (client == null)
            {
                throw new InvalidOperationException("The transport is not connected.");
            }

            if (payload == null)
            {
                throw new ArgumentNullException("payload");
            }

            var request = new Smb2IOCtlRequestPacket();

            request.Header.CreditCharge = 1;
            request.Header.Command = Smb2Command.IOCTL;
            request.Header.CreditRequestResponse = 1;
            request.Header.Flags = headerFlags;
            request.Header.MessageId = messageId;
            request.Header.TreeId = treeId;
            request.Header.SessionId = sessionId;

            request.PayLoad.CtlCode = code;

            if (code == CtlCode_Values.FSCTL_DFS_GET_REFERRALS || code == CtlCode_Values.FSCTL_DFS_GET_REFERRALS_EX)
            {
                request.PayLoad.FileId = FILEID.Invalid;
            }
            else
            {
                request.PayLoad.FileId = fileId;
            }

            if (payload.Length > 0)
            {
                request.PayLoad.InputOffset = request.BufferOffset;
                request.PayLoad.InputCount = (ushort)payload.Length;
                request.Buffer = payload;
            }

            request.PayLoad.MaxInputResponse = 0;
            request.PayLoad.MaxOutputResponse = 4096;
            request.PayLoad.Flags = IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL;

            ioctlRequestMessageIds.Enqueue(request.Header.MessageId);
            client.SendPacket(request);
        }

        private void ExpectIoctlPayload(Smb2Client client, out uint status, out byte[] payload)
        {
            if (client == null)
            {
                throw new InvalidOperationException("The transport is not connected.");
            }
            Smb2IOCtlResponsePacket response = client.ExpectPacket<Smb2IOCtlResponsePacket>(ioctlRequestMessageIds.Dequeue());

            payload = null;
            if (response.PayLoad.OutputCount > 0)
            {
                payload = response.Buffer.Skip((int)(response.PayLoad.OutputOffset - response.BufferOffset)).Take((int)response.PayLoad.OutputCount).ToArray();
            }

            status = response.Header.Status;
        }

        private void AddToClientList(Smb2Client client)
        {
            if (this.ClientList == null)
            {
                this.ClientList = new List<Smb2Client>();
            }
            else
            {
                this.ClientList.Add(client);
            }
        }

        #endregion
    }
}
