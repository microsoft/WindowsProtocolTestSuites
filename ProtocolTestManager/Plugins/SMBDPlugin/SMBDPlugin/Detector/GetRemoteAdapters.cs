// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Net;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SMBDDetector
    {

        public bool GetRemoteAdapters()
        {
            DetectorUtil.WriteLog("Get the remote adapters...");

            bool result = false;

            var ipList = Dns.GetHostAddresses(DetectionInfo.SUTName);


            foreach (var ip in ipList)
            {

            }

            if (result)
            {
                DetectorUtil.WriteLog("Finished", false, LogStyle.StepPassed);
                return true;
            }
            else
            {
                DetectorUtil.WriteLog("Failed", false, LogStyle.StepFailed);
                return false;
            }
        }

        private IPAddress[] GetIPAdressOfSut()
        {
            try
            {
                return Dns.GetHostAddresses(DetectionInfo.SUTName);
            }
            catch (Exception ex)
            {
                DetectorUtil.WriteLog(String.Format("Cannot get SUT IP addresses: {0}.", ex));
                return new IPAddress[0];
            }
        }

        private bool GetRemoteNetworkInterfaceInformation(IPAddress ip)
        {
            try
            {
                var smb2Client = new Smb2Client(new TimeSpan(0, 0, 10));

                smb2Client.ConnectOverTCP(ip, IPAddress.Parse(DetectionInfo.ServerNonRdmaNICIPAddress));

                ulong messageId = 0;

                var clientId = Guid.NewGuid();

                DialectRevision selectedDialect;

                byte[] gssToken;

                Packet_Header packetHeader;

                NEGOTIATE_Response resonse;

                uint status;

                status = smb2Client.Negotiate(
                    1,
                    1,
                    Packet_Header_Flags_Values.NONE,
                    messageId,
                    new DialectRevision[] { DialectRevision.Smb30 },
                    SecurityMode_Values.NONE,
                    Capabilities_Values.NONE,
                    clientId,
                    out selectedDialect,
                    out gssToken,
                    out packetHeader,
                    out resonse
                    );

                if (status != Smb2Status.STATUS_SUCCESS)
                {
                    return false;
                }

                messageId++;

                var sspiClientGss = new SspiClientSecurityContext(
                    DetectionInfo.Authentication,
                    new AccountCredential(DetectionInfo.DomainName, DetectionInfo.UserName, DetectionInfo.Password),
                    Smb2Utility.GetCifsServicePrincipalName(DetectionInfo.SUTName),
                    ClientSecurityContextAttribute.None,
                    SecurityTargetDataRepresentation.SecurityNativeDrep
                    );


                if (DetectionInfo.Authentication == SecurityPackageType.Negotiate)
                {
                    sspiClientGss.Initialize(gssToken);
                }
                else
                {
                    sspiClientGss.Initialize(null);
                }

                ulong sessionId = 0;

                SESSION_SETUP_Response sessionSetupResponse;

                status = smb2Client.SessionSetup(
                    1,
                    1,
                    Packet_Header_Flags_Values.NONE,
                    messageId,
                    sessionId,
                    SESSION_SETUP_Request_Flags.NONE,
                    SESSION_SETUP_Request_SecurityMode_Values.NONE,
                    SESSION_SETUP_Request_Capabilities_Values.NONE,
                    0,
                    sspiClientGss.Token,
                    out sessionId,
                    out gssToken,
                    out packetHeader,
                    out sessionSetupResponse
                    );

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}