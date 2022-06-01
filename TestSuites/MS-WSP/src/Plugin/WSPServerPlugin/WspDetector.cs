// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Microsoft.Protocols.TestManager.WSPServerPlugin
{
    public class WspDetector : IDisposable
    {
        public IPAddress ServerIpAddress { get; private set; }

        public DetectLogger logWriter;

        #region Variables
        private string serverName = null;

        private bool alreadyGotDnsName = false;
        #endregion Variables

        public string ServerName
        {
            get
            {
                if (!alreadyGotDnsName && !IPAddress.TryParse(serverName, out _)) // If the serverName is an IP address, no need to query DNS to get the entry
                {
                    try
                    {
                        serverName = Dns.GetHostEntry(serverName).HostName;
                        alreadyGotDnsName = true;
                    }
                    catch
                    {
                        logWriter.AddLog(DetectLogLevel.Information, "GetHostEntry failed");
                    }
                }

                return serverName;
            }
        }

        public WspDetector(DetectLogger logger, DetectionInfo info)
        {
            serverName = info.ServerComputerName;

            logWriter = logger;
            logWriter.AddLog(DetectLogLevel.Information, $"{nameof(info.ServerComputerName)}: {serverName}");
            logWriter.AddLog(DetectLogLevel.Information, $"{nameof(info.DomainName)}: {info.DomainName}");
            logWriter.AddLog(DetectLogLevel.Information, $"{nameof(info.UserName)}: {info.UserName}");
            logWriter.AddLog(DetectLogLevel.Information, $"User{nameof(info.Password)}: {info.Password}");
            logWriter.AddLog(DetectLogLevel.Information, $"{nameof(info.SupportedSecurityPackage)}: {info.SupportedSecurityPackage}");
            logWriter.AddLog(DetectLogLevel.Information, $"{nameof(info.UseServerGssToken)}: {info.UseServerGssToken}");
        }

        public bool DetectServerConnection(ref DetectionInfo detectionInfo)
        {
            logWriter.AddLog(DetectLogLevel.Information, "===== Detect Target Server Name =====");

            try
            {
                detectionInfo.ClientName = Dns.GetHostName();

                //Detect server IP address by server name
                //If server name is an ip address, skip to resolve, use the ip address directly
                if (IPAddress.TryParse(detectionInfo.ServerComputerName, out IPAddress address))
                {
                    this.ServerIpAddress = address;
                    logWriter.AddLog(DetectLogLevel.Information, "The Server name is an IP address.");

                    return true;
                }
                else //DNS resolve the server IP address by server name
                {
                    IPAddress[] addList = Dns.GetHostAddresses(detectionInfo.ServerComputerName);

                    if (!addList.Any())
                    {
                        logWriter.AddLog(DetectLogLevel.Error, $"The Server name {detectionInfo.ServerComputerName} cannot be resolved.");

                        return false;
                    }

                    this.ServerIpAddress = addList[0];
                    logWriter.AddLog(DetectLogLevel.Information, $"The Server name {detectionInfo.ServerComputerName} can be resolved as {addList[0]}.");

                    if (detectionInfo.ServerComputerName == null)
                    {
                        detectionInfo.ServerComputerName = addList[0].ToString();
                    }

                    return true;
                }
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.HostNotFound)
            {
                logWriter.AddLog(DetectLogLevel.Error, $"The Server name {detectionInfo.ServerComputerName} cannot be resolved.");
                logWriter.AddLog(DetectLogLevel.Error, ex.Message);

                return false;
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Error, ex.Message);

                return false;
            }
        }

        public bool CheckUserLogon(DetectionInfo info)
        {
            using var client = new Smb2Client(info.SMB2ClientTimeout);

            ulong messageId;
            ulong sessionId;
            Packet_Header_Flags_Values defaultFlags;
            NEGOTIATE_Response negotiateResp;
            bool treeConnectSigningRequired = false;
            UserLogon(info, client, out messageId, out sessionId, out defaultFlags, out negotiateResp, out treeConnectSigningRequired);

            try
            {
                Packet_Header header;
                LOGOFF_Response logoffResponse;

                logWriter.AddLog(DetectLogLevel.Information, "Client sends TreeConnect to server");
                client.TreeConnect(
                    1,
                    1,
                    treeConnectSigningRequired ? defaultFlags | Packet_Header_Flags_Values.FLAGS_SIGNED : defaultFlags,
                    messageId++,
                    sessionId,
                    $"\\\\{ServerName}\\{info.ShareName}",
                    out var treeId,
                    out header,
                    out _);
                ThrowIfFailedStatus(nameof(Smb2Client.TreeConnect), header.Status);

                logWriter.AddLog(DetectLogLevel.Information, "Client sends TreeDisonnect to server");
                client.TreeDisconnect(
                    1,
                    1,
                    defaultFlags,
                    messageId++,
                    sessionId,
                    treeId,
                    out header,
                    out _);
                ThrowIfFailedStatus(nameof(Smb2Client.TreeDisconnect), header.Status);

                logWriter.AddLog(DetectLogLevel.Information, "Client sends LogOff to server");
                client.LogOff(
                    1,
                    1,
                    defaultFlags,
                    messageId++,
                    sessionId,
                    out header,
                    out logoffResponse);
                ThrowIfFailedStatus(nameof(Smb2Client.LogOff), header.Status);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Error, ex.ToString());
                return false;
            }

            return true;
        }

        public void UserLogon(
            DetectionInfo info,
            Smb2Client client,
            out ulong messageId,
            out ulong sessionId,
            out Packet_Header_Flags_Values defaultFlags,
            out NEGOTIATE_Response negotiateResp,
            out bool treeConnectSigningRequired)
        {
            messageId = 0;
            sessionId = 0;

            logWriter.AddLog(DetectLogLevel.Information, "Client connects to server");
            client.ConnectOverTCP(ServerIpAddress);

            defaultFlags = Packet_Header_Flags_Values.NONE;
            var validDialects = new DialectRevision[]
            {
                DialectRevision.Smb2002,
                DialectRevision.Smb21,
                DialectRevision.Smb30,
                DialectRevision.Smb302,
                DialectRevision.Smb311
            };

            logWriter.AddLog(DetectLogLevel.Information, "Client sends Negotiate to server");
            var preauthIntegrityHashIDs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
            var encryptionAlgorithms = new EncryptionAlgorithm[] { EncryptionAlgorithm.ENCRYPTION_AES128_GCM, EncryptionAlgorithm.ENCRYPTION_AES128_CCM };
            var status = client.Negotiate(
                    creditCharge: 1,
                    creditRequest: 1,
                    flags: defaultFlags,
                    messageId: messageId++,
                    // Will negotiate highest dialect server supports  
                    dialects: validDialects,
                    securityMode: SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    capabilities: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_ENCRYPTION | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_LARGE_MTU,
                    clientGuid: Guid.NewGuid(),
                    out var selectedDialect,
                    out var serverGssToken,
                    out Packet_Header _,
                    out negotiateResp,
                    preauthHashAlgs: preauthIntegrityHashIDs,
                    encryptionAlgs: encryptionAlgorithms);
            ThrowIfFailedStatus(nameof(Smb2Client.Negotiate), status);

            var sessionSiginingRequired = negotiateResp.SecurityMode.HasFlag(NEGOTIATE_Response_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);
            if (sessionSiginingRequired)
            {
                defaultFlags |= Packet_Header_Flags_Values.FLAGS_SIGNED;
            }

            var usedSecurityPackageType = (SecurityPackageType)Enum.Parse(typeof(SecurityPackageType), info.SupportedSecurityPackage);
            var sspiClientGss = new SspiClientSecurityContext(
                usedSecurityPackageType,
                new AccountCredential(info.DomainName, info.UserName, info.Password),
                Smb2Utility.GetCifsServicePrincipalName(ServerName),
                ClientSecurityContextAttribute.None,
                SecurityTargetDataRepresentation.SecurityNativeDrep);

            if (usedSecurityPackageType == SecurityPackageType.Negotiate && info.UseServerGssToken)
            {
                sspiClientGss.Initialize(serverGssToken);
            }
            else
            {
                sspiClientGss.Initialize(null);
            }

            do
            {
                logWriter.AddLog(DetectLogLevel.Information, "Client sends SessionSetup to server");
                status = client.SessionSetup(
                    creditCharge: 1,
                    creditRequest: 1,
                    flags: Packet_Header_Flags_Values.NONE,
                    messageId: messageId++,
                    sessionId: sessionId,
                    sessionSetupFlags: SESSION_SETUP_Request_Flags.NONE,
                    securityMode: SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    capabilities: SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                    previousSessionId: 0,
                    clientGssToken: sspiClientGss.Token,
                    out sessionId,
                    out serverGssToken,
                    out _,
                    out _);

                if ((status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || status == Smb2Status.STATUS_SUCCESS) &&
                    serverGssToken != null && serverGssToken.Length > 0)
                {
                    sspiClientGss.Initialize(serverGssToken);
                }
            } while (status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED);
            ThrowIfFailedStatus(nameof(Smb2Client.SessionSetup), status);

            treeConnectSigningRequired = sessionSiginingRequired || (selectedDialect >= DialectRevision.Smb311);
            client.GenerateCryptoKeys(
                sessionId,
                sspiClientGss.SessionKey,
                treeConnectSigningRequired,
                false);
        }

        public bool FetchPlatformInfo(ref DetectionInfo info)
        {
            // Get client computer OS version and offset
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    ManagementObjectCollection resultCollection = QueryWmiObject(info.ClientName, "SELECT * FROM Win32_OperatingSystem");

                    var osArchitecture = string.Empty;
                    var version = string.Empty;
                    foreach (ManagementBaseObject result in resultCollection)
                    {
                        osArchitecture = result["OSArchitecture"].ToString();
                        version = result["Version"].ToString();
                        break;
                    }
                    info.ClientOffset = string.CompareOrdinal(osArchitecture.Substring(0, 2), "64") == 0 ? "64" : "32";
                    info.ClientVersion = GetOSVersion(version, osArchitecture, info.IsWDSInstalled).ToString();
                }

                info.LcidValue = CultureInfo.CurrentCulture.LCID.ToString();
                info.LanguageLocale = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

                logWriter.AddLog(DetectLogLevel.Information, "Detect Client OS Version finished successfully.");
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Error, $"Detect Client OS Version failed with Reason: {ex.Message}");
                return false;
            }

            // Get server OS version and offset
            try
            {
                CheckWspService(ref info);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Error, $"Check WSP Service failed with Reason: {ex.Message}");
                return false;
            }

            return true;
        }

        [SupportedOSPlatform("windows")]
        private ManagementObjectCollection QueryWmiObject(string machineName, string queryString)
        {
            var options = new ConnectionOptions() { Timeout = new TimeSpan(0, 0, 5) };
            var ms = new ManagementScope("\\\\" + machineName + "\\root\\cimv2", options);
            ms.Connect();

            var query = new ObjectQuery(queryString);
            var searcher = new ManagementObjectSearcher(ms, query);

            return searcher.Get();
        }

        private long GetOSVersion(string version, string osArchitecture, bool isWindowsSearch40Installed)
        {
            Version osVersion = default;
            try
            {
                osVersion = Version.Parse(version);
                osVersion = new Version(osVersion.Major, osVersion.Minor);
            }
            catch (Exception ex)
            {
                logWriter.AddLog(DetectLogLevel.Error, $"Get OS version failed with error: {ex.Message}");
            }

            long wspVersion;
            if (osVersion >= new Version(6, 1)) // 32 bit for Windows 7 and Windows Server 2008 R2
            {
                wspVersion = 0x00000700;
            }
            else if (osVersion >= new Version(6, 0) && isWindowsSearch40Installed) // 32 bit for Windows Vista (6002) or Windows Server 2008 (6003) with Windows Search 4.0 installed
            {
                wspVersion = 0x00000109;
            }
            else // 32-bit Windows Server 2008 operating system, or 32-bit Windows Vista operating system.
            {
                wspVersion = 0x00000102;
            }

            if (string.CompareOrdinal(osArchitecture, "64-bit") == 0)
            {
                wspVersion |= 0x10000;
            }

            return wspVersion;
        }

        public void CheckWspService(ref DetectionInfo info)
        {
            var parameter = DetectionHelper.BuildParameter();

            MessageBuilder builder = new MessageBuilder(parameter);

            using var client = new WspClient();

            client.Sender = new RequestSender(
                info.ServerComputerName,
                info.UserName,
                info.DomainName,
                info.Password,
                info.SupportedSecurityPackage,
                info.UseServerGssToken,
                info.SMB2ClientTimeout);

            uint clientversion = 0;
            uint.TryParse(info.ClientVersion, out clientversion);

            string serverName = info.ServerComputerName;
            var connectInMessage = builder.GetCPMConnectIn(
                clientversion,
                1, // isRemote
                info.UserName,
                info.ClientName,
                info.ServerComputerName,
                info.CatalogName,
                info.LanguageLocale);

            // Send CPMConnectIn Message
            logWriter.AddLog(DetectLogLevel.Information, "WSP clietnt sends CPMConnectIn to server");
            client.SendCPMConnectIn(
                connectInMessage._iClientVersion,
                connectInMessage._fClientIsRemote,
                connectInMessage.MachineName,
                connectInMessage.UserName,
                connectInMessage.PropertySet1,
                connectInMessage.PropertySet2,
                connectInMessage.aPropertySets,
                connectInMessage.cPropSets,
                connectInMessage.cExtPropSet);

            client.ExpectMessage(out CPMConnectOut connectOutMessage);

            uint msgId = (uint)connectOutMessage.Header._msg;
            uint msgStatus = connectOutMessage.Header._status;

            if (msgStatus != (uint)WspErrorCode.SUCCESS)
            {
                logWriter.AddLog(DetectLogLevel.Error, $"CPMConnectOutResponse failed with {msgStatus}.");
            }

            if ((msgId == (uint)MessageType.CPMConnectOut) && (msgStatus == (uint)WspErrorCode.SUCCESS))
            {
                info.ServerVersion = connectOutMessage._serverVersion.ToString();

                if ((connectOutMessage._serverVersion & WspConsts.Is64bitVersion) != 0)
                {
                    info.ServerOffset = "64";
                }
                else
                {
                    info.ServerOffset = "32";
                }

                logWriter.AddLog(DetectLogLevel.Information, $"ServerVersion returned from CPMConnectOut message is: {info.ServerVersion}.");
            }

            uint? cursor = null;
            try
            {
                var createQueryInMessage = builder.GetCPMCreateQueryIn(
                    info.QueryPath,
                    info.QueryText,
                    WspConsts.System_Search_Contents,
                    false);

                // Send CPMCreateQueryIn Message
                logWriter.AddLog(DetectLogLevel.Information, "WSP clietnt sends CPMCreateQueryIn to server");
                client.SendCPMCreateQueryIn(
                    createQueryInMessage.ColumnSet,
                    createQueryInMessage.RestrictionArray,
                    createQueryInMessage.SortSet,
                    createQueryInMessage.CCategorizationSet,
                    createQueryInMessage.RowSetProperties,
                    createQueryInMessage.PidMapper,
                    createQueryInMessage.GroupArray,
                    createQueryInMessage.Lcid);

                client.ExpectMessage(out CPMCreateQueryOut createQueryOutMessage);

                msgStatus = createQueryOutMessage.Header._status;

                if (msgStatus != (uint)WspErrorCode.SUCCESS)
                {
                    logWriter.AddLog(DetectLogLevel.Error, $"CPMCreateQueryOutResponse failed with {msgStatus}.");
                }

                if (msgStatus == (uint)WspErrorCode.SUCCESS)
                {
                    cursor = createQueryOutMessage.aCursors[0];
                }
            }
            finally
            {
                if (cursor.HasValue)
                {
                    var freeCursorInMessage = builder.GetCPMFreeCursorIn(cursor.Value);

                    // Send CPMFreeCursorIn Message
                    logWriter.AddLog(DetectLogLevel.Information, "WSP clietnt sends CPMFreeCursorIn to server");
                    client.Send(freeCursorInMessage);

                    client.ExpectMessage(out CPMFreeCursorOut freeCursorOutMessage);

                    msgStatus = freeCursorOutMessage.Header._status;

                    if (msgStatus != (uint)WspErrorCode.SUCCESS)
                    {
                        logWriter.AddLog(DetectLogLevel.Error, $"CPMFreeCursorOutResponse failed with {msgStatus}.");
                    }
                }

                var disconnectMessage = builder.GetCPMDisconnect();

                // Send CPMDisconnect Message
                logWriter.AddLog(DetectLogLevel.Information, "WSP clietnt sends CPMDisconnect to server");
                client.Sender.SendMessage(disconnectMessage, out _);
            }
        }

        private void ThrowIfFailedStatus(string operation, uint status)
        {
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                logWriter.AddLog(DetectLogLevel.Error, $"{operation} failed, status: {Smb2Status.GetStatusCode(status)}");
                throw new Exception($"{operation} failed with {Smb2Status.GetStatusCode(status)}");
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {

        }
    }
}
