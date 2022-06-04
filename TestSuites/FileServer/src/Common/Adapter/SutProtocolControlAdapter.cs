// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.IO;
using System.Net;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    /// <summary>
    /// A managed protocol adapter
    /// </summary>
    public class SutProtocolControlAdapter : ManagedAdapterBase, ISutProtocolControlAdapter
    {
        private TestConfigBase testConfig;

        private Smb2FunctionalClient client;

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            testConfig = new TestConfigBase(Site);
            TestTools.StackSdk.Security.KerberosLib.KerberosContext.KDCComputerName = testConfig.DCServerName;
            TestTools.StackSdk.Security.KerberosLib.KerberosContext.KDCPort = testConfig.KDCPort;
        }

        public bool CreateDirectory(string uncSharePath, string directoryName)
        {
            uint createStatus = 0;
            try
            {
                if (!ConnectToShare(uncSharePath, out uint treeId))
                {
                    return false;
                }

                createStatus = client.Create(treeId, directoryName,
                    CreateOptions_Values.FILE_DIRECTORY_FILE,
                    out FILEID fileId,
                    out Smb2CreateContextResponse[] serverCreateContexts,
                    checker: (header, response) => { });
                DisconnectToShare(treeId, fileId);
            }
            catch (Exception ex)
            {
                Site.Log.Add(LogEntryKind.Debug, $"Exception thrown when CreateDirectory: {ex}");
            }
            return createStatus == Smb2Status.STATUS_SUCCESS;
        }

        public void DeleteDirectory(string uncSharePath, string directoryName)
        {
            try
            {
                if (!ConnectToShare(uncSharePath, out uint treeId))
                {
                    return;
                }

                client.Create(treeId, directoryName,
                    CreateOptions_Values.FILE_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    out FILEID fileId,
                    out Smb2CreateContextResponse[] serverCreateContexts,
                    checker: (header, response) => { });
                DisconnectToShare(treeId, fileId);
            }
            catch (Exception ex)
            {
                Site.Log.Add(LogEntryKind.Debug, $"Exception thrown when DeleteDirectory: {ex}");
            }
        }

        public bool CreateFile(string uncSharePath, string fileName, string content)
        {

            uint createStatus = 0;
            try
            {
                if (!ConnectToShare(uncSharePath, out uint treeId))
                {
                    return false;
                }

                createStatus = client.Create(treeId, fileName,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                    out FILEID fileId,
                    out Smb2CreateContextResponse[] serverCreateContexts,
                    checker: (header, response) => { });
                if (createStatus == Smb2Status.STATUS_SUCCESS)
                {
                    createStatus = client.Write(treeId, fileId, content, checker: (header, response) => { });
                }
                DisconnectToShare(treeId, fileId);
            }
            catch (Exception ex)
            {
                Site.Log.Add(LogEntryKind.Debug, $"Exception thrown when CreateFile: {ex}");
            }
            return createStatus == Smb2Status.STATUS_SUCCESS;
        }

        public void DeleteFile(string uncSharePath, string fileName)
        {
            try
            {
                if (!ConnectToShare(uncSharePath, out uint treeId))
                {
                    return;
                }

                client.Create(treeId, fileName,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    out FILEID fileId,
                    out Smb2CreateContextResponse[] serverCreateContexts,
                    checker: (header, response) => { });
                DisconnectToShare(treeId, fileId);
            }
            catch (Exception ex)
            {
                Site.Log.Add(LogEntryKind.Debug, $"Exception thrown when DeleteFile: {ex}");
            }
        }

        public void CopyFile(string uncSharePath, string fileName)
        {
            try
            {
                if (!ConnectToShare(uncSharePath, out uint treeId))
                {
                    return;
                }

                // fileName contains a file path and a search pattern. e.g. "Data\*.*"
                string[] filePath = fileName.Split(new char[] { '\\' });
                // fileName contains wildcard, so get the files by search pattern.
                string[] files = Directory.GetFiles(filePath[0], filePath[1]);

                foreach (var file in files)
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        // "file" contains path of the orginal file, the pure file name needs to be extracted and then the file can be copied to the destination share.
                        var pureFileName = Path.GetFileName(file);
                        byte[] buf = new byte[fs.Length];
                        fs.Read(buf, 0, (int)fs.Length);
                        client.Create(treeId, pureFileName,
                            CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                            out FILEID fileId,
                            out Smb2CreateContextResponse[] serverCreateContexts,
                            checker: (header, response) => { });
                        client.Write(treeId, fileId, buf);
                        client.Close(treeId, fileId);
                    }
                }

                DisconnectToShare(treeId, FILEID.Zero);
            }
            catch (Exception ex)
            {
                Site.Log.Add(LogEntryKind.Debug, $"Exception thrown when CopyFile: {ex}");
            }
        }

        public bool CheckIfShareIsAvailable(string share)
        {
            return ConnectToShare(share, out uint treeId);
        }

        private bool ConnectToShare(string uncSharePath, out uint treeId)
        {
            treeId = 0;

            string serverName = Smb2Utility.GetServerName(uncSharePath);
            var serverIPs = Dns.GetHostAddresses(serverName);
            if (serverIPs == null || serverIPs.Length == 0)
            {
                return false;
            }

            uint status = 0;
            foreach (var ip in serverIPs) // Try to connect to each IP since not all of them can be connected successfully.
            {
                try
                {
                    client = new Smb2FunctionalClient(testConfig.Timeout, testConfig, Site);
                    client.ConnectToServer(testConfig.UnderlyingTransport, serverName, ip);
                    status = client.Negotiate(
                        Smb2Utility.GetDialects(testConfig.MaxSmbVersionSupported),
                        true,
                        checker: (header, response) => { });
                    if (status != Smb2Status.STATUS_SUCCESS)
                    {
                        continue;
                    }
                    status = client.SessionSetup(testConfig.DefaultSecurityPackage, serverName, testConfig.AccountCredential, false, checker: (header, response) => { });
                    if (status != Smb2Status.STATUS_SUCCESS)
                    {
                        client.Disconnect();
                        continue;
                    }

                    status = client.TreeConnect(uncSharePath, out treeId, checker: (header, response) => { });
                    if (status == Smb2Status.STATUS_SUCCESS)
                    {
                        return true;
                    }
                    else
                    {
                        client.LogOff();
                        client.Disconnect();
                    }
                }
                catch
                {
                    // Catch the exceptions thrown in Negotiage/SessionSetup/TreeConnect
                    // Try next IP
                }
            }

            return false;
        }

        private void DisconnectToShare(uint treeId, FILEID fileId)
        {
            if (fileId.Persistent != 0 || fileId.Volatile != 0)
            {
                client.Close(treeId, fileId);
            }
            client.TreeDisconnect(treeId);
            client.LogOff();
            client.Disconnect();
        }
    }
}
