// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
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
        }

        public bool CreateDirectory(string uncSharePath, string directoryName)
        {
            if (!ConnectToShare(uncSharePath, out uint treeId))
            {
                return false;
            }
            uint createStatus = client.Create(treeId, directoryName, CreateOptions_Values.FILE_DIRECTORY_FILE, out FILEID fileId, out Smb2CreateContextResponse[] serverCreateContexts);
            DisconnectToShare(treeId, fileId);
            return createStatus == Smb2Status.STATUS_SUCCESS;
        }

        public void DeleteDirectory(string uncSharePath, string directoryName)
        {
            if (!ConnectToShare(uncSharePath, out uint treeId))
            {
                return;
            }
            client.Create(treeId, directoryName, CreateOptions_Values.FILE_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE, out FILEID fileId, out Smb2CreateContextResponse[] serverCreateContexts);
            DisconnectToShare(treeId, fileId);
        }

        public bool CreateFile(string uncSharePath, string fileName, string content)
        {
            if (!ConnectToShare(uncSharePath, out uint treeId))
            {
                return false;
            }
            uint createStatus = client.Create(treeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out FILEID fileId, out Smb2CreateContextResponse[] serverCreateContexts);
            client.Write(treeId, fileId, content);
            DisconnectToShare(treeId, fileId);
            return createStatus == Smb2Status.STATUS_SUCCESS;
        }

        public void DeleteFile(string uncSharePath, string fileName)
        {
            if (!ConnectToShare(uncSharePath, out uint treeId))
            {
                return;
            }
            client.Create(treeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE, out FILEID fileId, out Smb2CreateContextResponse[] serverCreateContexts);
            DisconnectToShare(treeId, fileId);
        }

        public void CopyFile(string uncSharePath, string fileName)
        {
            if (!ConnectToShare(uncSharePath, out uint treeId))
            {
                return;
            }

            string[] filePath = fileName.Split(new char[] { '\\' });
            // file Path contains wildcard, so get the files by search pattern.
            string[] files = System.IO.Directory.GetFiles(filePath[0], filePath[1]);

            foreach (var file in files)
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    // file contains path of the orginal file, the pure file name needs to be extract it and then copy it to the destination share.
                    var paths = file.Split(new char[] { '\\' });
                    var pureFileName = paths[paths.Length - 1];
                    byte[] buf = new byte[fs.Length];
                    fs.Read(buf, 0, (int)fs.Length);
                    client.Create(treeId, pureFileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out FILEID fileId, out Smb2CreateContextResponse[] serverCreateContexts);
                    client.Write(treeId, fileId, buf);
                    client.Close(treeId, fileId);
                }
            }

            DisconnectToShare(treeId, FILEID.Zero);
        }

        public bool CheckIfShareIsAvailable(string share)
        {
            return ConnectToShare(share, out uint treeId);
        }

        private bool ConnectToShare(string uncSharePath, out uint treeId)
        {
            treeId = 0;
            client = new Smb2FunctionalClient(testConfig.Timeout, testConfig, Site);

            string serverName = Smb2Utility.GetServerName(uncSharePath);
            var serverIPs = Dns.GetHostEntry(serverName).AddressList;
            if (serverIPs == null || serverIPs.Length == 0)
            {
                return false;
            }

            client.ConnectToServer(Smb2TransportType.Tcp, serverName, serverIPs[0]);
            uint status = client.Negotiate(
                Smb2Utility.GetDialects(testConfig.MaxSmbVersionSupported),
                true);
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                return false;
            }
            status = client.SessionSetup(testConfig.DefaultSecurityPackage, serverName, testConfig.AccountCredential, false);
            if (status != Smb2Status.STATUS_SUCCESS)
            {
                return false;
            }

            client.TreeConnect(uncSharePath, out treeId);
            return status == Smb2Status.STATUS_SUCCESS;
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
