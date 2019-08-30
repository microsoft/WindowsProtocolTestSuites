// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Net;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System.Management;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;

namespace Microsoft.Protocols.TestManager.WSPServerPlugin
{

    public class WSPDetector : IDisposable
    {
        public IPAddress SUTIpAddress { get; private set; }
        public AccountCredential Credential { get; private set; }

        public Logger logWriter;

        #region Variables
        private WspClient wspClient = null;           
      
        private string sutName = null;
        private const int defaultTimeoutInSeconds = 20;
    
        private bool alreadyGotDnsName = false;
        #endregion Variables

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
       
             
        public WSPDetector
            (Logger logger,
            DetectionInfo info
            )
        {
            sutName = info.ServerComputerName;            

            logWriter = logger;
            logWriter.AddLog(LogLevel.Information, string.Format("DomainName: {0}", info.DomainName));
            logWriter.AddLog(LogLevel.Information, string.Format("ServerComputerName: {0}", sutName));
            logWriter.AddLog(LogLevel.Information, string.Format("UserName: {0}", info.ServerUserName));
            logWriter.AddLog(LogLevel.Information, string.Format("UserPassword: {0}", info.ServerUserPassword));

            logWriter.AddLineToLog(LogLevel.Information);
        }
        public bool DetectSUTConnection(ref DetectionInfo detectionInfo)
        {
            DetectorUtil.WriteLog("===== Detect Target SUT Name=====", true, LogStyle.Default);

            try
            {
                detectionInfo.ClientName = Dns.GetHostName();

                IPAddress address;
                //Detect SUT IP address by SUT name
                //If SUT name is an ip address, skip to resolve, use the ip address directly
                if (IPAddress.TryParse(detectionInfo.ServerComputerName, out address))
                {
                    this.SUTIpAddress = address;
                    DetectorUtil.WriteLog("Finished", true, LogStyle.StepPassed);
                    return true;
                }
                else //DNS resolve the SUT IP address by SUT name
                {
                    IPAddress[] addList = Dns.GetHostAddresses(detectionInfo.ServerComputerName);

                    if (null == addList)
                    {
                        DetectorUtil.WriteLog(string.Format("The SUT name {0} cannot be resolved.", detectionInfo.ServerComputerName), true, LogStyle.Error);
                        DetectorUtil.WriteLog("Failed", true, LogStyle.StepFailed);
                        return false;
                    }
                    else
                    {
                        this.SUTIpAddress = addList[0];
                        DetectorUtil.WriteLog(string.Format("The SUT name {0} can be resolved as :", addList.ToString()), true, LogStyle.Default);
                        DetectorUtil.WriteLog("Finished", true, LogStyle.StepPassed);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                DetectorUtil.WriteLog(ex.Message, true, LogStyle.StepFailed);
                DetectorUtil.WriteLog("Failed", true, LogStyle.StepFailed);
                return false;
            }
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
                        Packet_Header_Flags_Values.NONE,
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
            DialectRevision[] reqDialects = new DialectRevision[] { DialectRevision.Smb2002, DialectRevision.Smb30, DialectRevision.Smb302 };
          
            byte[] gssToken;
            Packet_Header header;
            clientGuid = Guid.NewGuid();
            logWriter.AddLog(LogLevel.Information, "Client sends multi-protocol Negotiate to server");
            MultiProtocolNegotiate(
                client,
                1,
                1,
                Packet_Header_Flags_Values.NONE,
                messageId++,
                reqDialects,
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
                    SecurityPackageType.Negotiate,
                     new AccountCredential(info.DomainName, info.ServerUserName, info.ServerUserPassword),
                    Smb2Utility.GetCifsServicePrincipalName(SUTName),
                    ClientSecurityContextAttribute.None,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);

            // Server GSS token is used only for Negotiate authentication when enabled            
            sspiClientGss.Initialize(gssToken);           

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
                false, 
                encryptionRequired,
                null,
                false);

            #endregion
        }
        public uint MultiProtocolNegotiate(
           Smb2Client client,
           ushort creditCharge,
           ushort creditRequest,
           Packet_Header_Flags_Values flags,
           ulong messageId,
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

            return status;
        }
        public bool FetchPlatformInfo(ref DetectionInfo info)
        {
            string osArchitecture = "64-bit";
            string buildNum = "14393";

            try
            {
                ManagementObjectCollection resultCollection = QueryWmiObject(
                    info.ServerComputerName,
                    "SELECT * FROM Win32_OperatingSystem");

                foreach (ManagementBaseObject result in resultCollection)
                {
                    foreach (var prop in result.Properties)
                    {
                        osArchitecture = result["OSArchitecture"].ToString();
                        buildNum = result["BuildNumber"].ToString();
                        break;
                    }
                }
                info.ServerOffset = String.CompareOrdinal(osArchitecture, "64-bit") == 0 ? "64" : "32";
                info.ServerVersion = GetOSVersion(buildNum, osArchitecture, info.IsWDSInstalled).ToString();
            }
            catch (Exception ex)
            {                
                logWriter.AddLog(LogLevel.Information, $"Detect OS Version failed with  Reason: {ex.Message}");
                return false;   
            }

            try
            {
                ManagementObjectCollection resultCollection = QueryWmiObject(
                    info.ClientName,
                    "SELECT * FROM Win32_OperatingSystem");

                foreach (ManagementBaseObject result in resultCollection)
                {
                    foreach (var prop in result.Properties)
                    {
                        osArchitecture = result["OSArchitecture"].ToString();
                        buildNum = result["BuildNumber"].ToString();
                        break;
                    }
                }
                info.ClientOffset = String.CompareOrdinal(osArchitecture, "64-bit") == 0 ? "64" : "32";
                info.ClientVersion = GetOSVersion(buildNum, osArchitecture, false).ToString();
            }
            catch (Exception ex)
            {
                logWriter.AddLog(LogLevel.Information, $"Detect OS Version failed with  Reason: {ex.Message}");
                return false;
            }
            return true;
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
        private long GetOSVersion(string buildNum, string osArchitecture, bool isWindowsSearch40Installed)
        {
            long osversion = 0;
            long wspVersion = 0;
            try
            {
                long.TryParse(buildNum, out osversion);
            }
            catch (Exception e)
            {
                logWriter.AddLog(LogLevel.Error, $"Get OS version failed with error: {e.Message}");
            }

            if (osversion >= 7601)//32 bit for Windows 7 and Windows Server 2008 R2
            {
                wspVersion = 0x00000700;
            }
            else if (osversion >= 6002 && isWindowsSearch40Installed) //32 bit for Windows Vista (6002) or Windows Server 2008 (6003) with Windows Search 4.0 installed
            {
                wspVersion = 0x00000109;
            }
            else //32-bit Windows Server 2008 operating system, or 32-bit Windows Vista operating system.
            {
                wspVersion = 0x00000102;
            }

            if (String.CompareOrdinal(osArchitecture, "64-bit") == 0)
            {
                wspVersion |= 0x10000;
            }

            return wspVersion;
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

       

        private void LogIPConfig()
        {
            logWriter.AddLog(LogLevel.Information, "ipconfig /all");
            string result = logWriter.RunCmdAndGetOutput("ipconfig /all");
            logWriter.AddLog(LogLevel.Information, result);
        }

        public bool FetchShareInfo(ref DetectionInfo info)
        {           
            bool result = false;            
           
            // Try to connect the share which is input by the user in the "Target Share" field of Auto-Detection page.
            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                try
                {
                    logWriter.AddLog(LogLevel.Information, string.Format("Try to connect share {0}.", info.SharedPath));
                    ConnectToShare(info.SharedPath, info, client, out messageId, out sessionId, out treeId);
                    result = true;
                }
                catch(Exception ex)
                {                    
                    // Show error to user.
                    logWriter.AddLog(LogLevel.Error, $"Did not find the share on SUT. Failed with error: ${ex.Message}. ");
                }
            }            

            return result;
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

            logWriter.AddLog(LogLevel.Information, "Client sends TreeConnect to server");

            client.TreeConnect(
                1,
                1,
                Packet_Header_Flags_Values.NONE,
                messageId++,
                sessionId,
                sharename,
                out treeId,
                out header,
                out treeConnectResp);

            if (header.Status != Smb2Status.STATUS_SUCCESS)
            {
                LogFailedStatus("TREECONNECT", header.Status);
                throw new Exception("TREECONNECT failed with " + Smb2Status.GetStatusCode(header.Status));
            }

            try
            {

                TREE_DISCONNECT_Response treeDisconnectResponse;
                client.TreeDisconnect(
                    1,
                    1,
                    Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    out header,
                    out treeDisconnectResponse);

                LOGOFF_Response logoffResponse;
                client.LogOff(1, 1, Packet_Header_Flags_Values.NONE, messageId++, sessionId, out header, out logoffResponse);
            }
            catch (Exception ex)
            {
                treeId = 0;
                logWriter.AddLog(LogLevel.Information, string.Format("TREECONNECT failed with: " + ex.Message));
               
            }
            #endregion
        }        

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (wspClient != null)
            {
                wspClient.Dispose();
                wspClient = null;
            }
        }
    }
}
