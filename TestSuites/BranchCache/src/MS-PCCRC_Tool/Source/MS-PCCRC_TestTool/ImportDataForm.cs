// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MS_PCCRC_TestTool
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
    using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

    public partial class ImportDataForm : Form
    {
        private const int BLOCKBYTECOUNT = 0x10000;
        private const int SEGMENTBLOCKCOUNT = 512;
        private const string HOHODK_APPEND_STRING = "MS_P2P_CACHING\0";

        private string smb2FilePathPattern = @"^\\\\(?<Server>[^\\]+)\\(?<SharedFolder>[^\\]+)\\(?<FileName>[^\\]+)$";
        private string httpFilePathPattern = @"^http://(?<Server>[^/]+)/(?<FileName>[^/]+)$";

        private RichTextBoxLogger logger;

        public ImportDataForm()
        {
            InitializeComponent();
        }

        private void ImportDataForm_Load(object sender, EventArgs e)
        {
            branchCacheVersionComboBox.Items.Add(BranchCacheVersion.V1);
            branchCacheVersionComboBox.Items.Add(BranchCacheVersion.V2);
            branchCacheVersionComboBox.SelectedIndex = 0;

            operationModeComboBox.Items.Add(OperationMode.RemoteHashVerification);
            operationModeComboBox.Items.Add(OperationMode.LocalHashGeneration);
            operationModeComboBox.SelectedIndex = 0;

            transportComboBox.Items.Add(ContentInformationTransport.PCCRTP);
            transportComboBox.Items.Add(ContentInformationTransport.SMB2);
            transportComboBox.SelectedIndex = 0;

            hashAlgorithmComboBox.Items.Add(dwHashAlgo_Values.SHA256);
            hashAlgorithmComboBox.Items.Add(dwHashAlgo_Values.SHA384);
            hashAlgorithmComboBox.Items.Add(dwHashAlgo_Values.SHA512);
            hashAlgorithmComboBox.SelectedIndex = 0;

            smb2AuthenticationComboBox.Items.Add(SecurityPackageType.Negotiate);
            smb2AuthenticationComboBox.Items.Add(SecurityPackageType.Kerberos);
            smb2AuthenticationComboBox.Items.Add(SecurityPackageType.Ntlm);
            smb2AuthenticationComboBox.SelectedIndex = 0;

            logger = new RichTextBoxLogger(logRichTextBox);
        }

        private void branchCacheVersionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetUIStatus();
        }

        private void operationModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetUIStatus();
        }

        private void transportComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetUIStatus();
        }

        private void SetUIStatus()
        {
            if (branchCacheVersionComboBox.SelectedItem == null || operationModeComboBox.SelectedItem == null || transportComboBox.SelectedItem == null)
                return;

            var version = (BranchCacheVersion)branchCacheVersionComboBox.SelectedItem;
            var operationMode = (OperationMode)operationModeComboBox.SelectedItem;
            var transport = (ContentInformationTransport)transportComboBox.SelectedItem;

            transportComboBox.Enabled = true;
            hashAlgorithmComboBox.Enabled = true;
            filePathBrowseButton.Enabled = true;
            smb2AuthenticationComboBox.Enabled = true;
            domainNameTextBox.Enabled = true;
            userNameTextBox.Enabled = true;
            userPasswordTextBox.Enabled = true;

            if (operationMode == OperationMode.LocalHashGeneration)
            {
                transportComboBox.Enabled = false;
                smb2AuthenticationComboBox.Enabled = false;
                domainNameTextBox.Enabled = false;
                userNameTextBox.Enabled = false;
                userPasswordTextBox.Enabled = false;

                if (version == BranchCacheVersion.V2)
                {
                    hashAlgorithmComboBox.Enabled = false;
                }
            }
            else
            {
                hashAlgorithmComboBox.Enabled = false;

                if (transport == ContentInformationTransport.PCCRTP)
                {
                    filePathBrowseButton.Enabled = false;
                    smb2AuthenticationComboBox.Enabled = false;
                    domainNameTextBox.Enabled = false;
                    userNameTextBox.Enabled = false;
                    userPasswordTextBox.Enabled = false;
                }
            }
        }

        private void filePathBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFlieDlg = new OpenFileDialog();
            openFlieDlg.Filter = "All files (*.*)|*.*";
            openFlieDlg.RestoreDirectory = true;
            if (openFlieDlg.ShowDialog() == DialogResult.OK)
            {
                this.filePathTextBox.Text = openFlieDlg.FileName;
            }
        }

        private void executeButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckInput())
                    return;

                logger.Clear();

                #region Read settings from UI

                var version = (BranchCacheVersion)branchCacheVersionComboBox.SelectedItem;
                var operationMode = (OperationMode)operationModeComboBox.SelectedItem;
                var transport = (ContentInformationTransport)transportComboBox.SelectedItem;

                var serverSecret = serverSecretTextBox.Text;
                var filePath = filePathTextBox.Text;

                var hashAlgoValue = (dwHashAlgo_Values)hashAlgorithmComboBox.SelectedItem;
                HashAlgorithm hashAlgorithm;
                HMAC hmacAlgorithm;
                int hashBlockSize;

                string server = null;
                string file = null;
                string sharedFolder = null;
                Match filePathMatch = null;
                switch (transport)
                {
                    case ContentInformationTransport.PCCRTP:
                        filePathMatch = Regex.Match(filePath, httpFilePathPattern);
                        server = filePathMatch.Groups["Server"].Value;
                        file = filePathMatch.Groups["FileName"].Value;
                        break;
                    case ContentInformationTransport.SMB2:
                        filePathMatch = Regex.Match(filePath, smb2FilePathPattern);
                        server = filePathMatch.Groups["Server"].Value;
                        sharedFolder = filePathMatch.Groups["SharedFolder"].Value;
                        file = filePathMatch.Groups["FileName"].Value;
                        break;
                    default:
                        throw new NotImplementedException();
                }


                SecurityPackageType securityPackageType = (SecurityPackageType)smb2AuthenticationComboBox.SelectedItem;
                
                string domainName = domainNameTextBox.Text;
                string userName = userNameTextBox.Text;
                string userPassword = userPasswordTextBox.Text;

                #endregion

                var timeout = TimeSpan.FromSeconds(60);

                byte[] content;
                byte[] contentInformation;

                Content_Information_Data_Structure contentInformationStructure = new Content_Information_Data_Structure();
                Content_Information_Data_Structure_V2 contentInformationStructureV2 = new Content_Information_Data_Structure_V2();

                #region Read content and content information

                if (operationMode == OperationMode.RemoteHashVerification)
                {
                    switch (transport)
                    {
                        case ContentInformationTransport.PCCRTP:
                            PccrtpClient pccrtpClient = new PccrtpClient();
                            PccrtpRequest pccrtpRequest = pccrtpClient.CreatePccrtpRequest(
                                server,
                                80,
                                file,
                                version);
                            PccrtpResponse pccrtpResponse = pccrtpClient.SendHttpRequest(
                                HttpVersionType.HttpVersion11,
                                pccrtpRequest,
                                (int)timeout.TotalMilliseconds);

                            if (pccrtpResponse.HttpResponse.ContentEncoding == "peerdist")
                            {
                                contentInformation = pccrtpResponse.PayloadData;

                                content = Utility.DownloadHTTPFile(server, file);
                            }
                            else
                            {
                                content = pccrtpResponse.PayloadData;

                                Thread.Sleep(5000); // Wait for hash generation

                                pccrtpResponse = pccrtpClient.SendHttpRequest(
                                    HttpVersionType.HttpVersion11,
                                    pccrtpRequest,
                                    (int)timeout.TotalMilliseconds);

                                contentInformation = pccrtpResponse.PayloadData;
                            }

                            break;

                        case ContentInformationTransport.SMB2:
                            using (Smb2ClientTransport smb2Client = new Smb2ClientTransport(timeout))
                            {
                                smb2Client.OpenFile(
                                    server,
                                    sharedFolder,
                                    file,
                                    securityPackageType,
                                    domainName,
                                    userName,
                                    userPassword,
                                    AccessMask.GENERIC_READ);

                                content = smb2Client.ReadAllBytes();

                                Thread.Sleep(5000); // Wait for hash generation

                                HASH_HEADER hashHeader;
                                smb2Client.ReadHash(
                                    SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                                    version == BranchCacheVersion.V1 ? SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_1 : SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_2,
                                    version == BranchCacheVersion.V1 ? SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_HASH_BASED : SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_FILE_BASED,
                                    0,
                                    uint.MaxValue,
                                    out hashHeader,
                                    out contentInformation);
                            }

                            break;

                        default:
                            throw new NotImplementedException();
                    }

                    switch (version)
                    {
                        case BranchCacheVersion.V1:
                            contentInformationStructure = PccrcUtility.ParseContentInformation(contentInformation);
                            break;
                        case BranchCacheVersion.V2:
                            contentInformationStructureV2 = PccrcUtility.ParseContentInformationV2(contentInformation);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    content = File.ReadAllBytes(filePath);
                }

                #endregion

                #region Calculate hash and execute verification

                switch (version)
                {
                    case BranchCacheVersion.V1:

                        if (operationMode == OperationMode.RemoteHashVerification)
                            PccrcUtility.GetHashAlgorithm(contentInformationStructure.dwHashAlgo, out hashAlgorithm, out hmacAlgorithm, out hashBlockSize);
                        else
                            PccrcUtility.GetHashAlgorithm(hashAlgoValue, out hashAlgorithm, out hmacAlgorithm, out hashBlockSize);
                        hmacAlgorithm.Key = hashAlgorithm.ComputeHash(Encoding.Unicode.GetBytes(serverSecret));

                        logger.LogInfo(
                            "Ks = Hash(ServerSecret): {0}",
                            Utility.ToHexString(hmacAlgorithm.Key));

                        logger.NewLine();

                        int blockTotalCount = content.Length / BLOCKBYTECOUNT;
                        if (content.Length > BLOCKBYTECOUNT * blockTotalCount)
                        {
                            blockTotalCount = blockTotalCount + 1;
                        }

                        int segmentCount = blockTotalCount / SEGMENTBLOCKCOUNT;
                        if (blockTotalCount > SEGMENTBLOCKCOUNT * segmentCount)
                        {
                            segmentCount = segmentCount + 1;
                        }

                        for (int segmentIndex = 0; segmentIndex < segmentCount; segmentIndex++)
                        {
                            logger.LogInfo("Segment{0}", segmentIndex);
                            logger.NewLine();
                            logger.Indent();

                            List<byte> blockHashList = new List<byte>();

                            List<byte> tempList = new List<byte>();

                            int blockCount = (segmentIndex == segmentCount - 1) ? (blockTotalCount % SEGMENTBLOCKCOUNT) : (SEGMENTBLOCKCOUNT);

                            for (int blockIndex = 0; blockIndex < blockCount; blockIndex++)
                            {
                                logger.LogInfo(
                                    "Block{0} Offset {1} Length {2}",
                                    blockIndex,
                                    BLOCKBYTECOUNT * SEGMENTBLOCKCOUNT * segmentIndex + BLOCKBYTECOUNT * blockIndex,
                                    BLOCKBYTECOUNT);
                                logger.NewLine();
                                logger.Indent();

                                var block = content.Skip(BLOCKBYTECOUNT * SEGMENTBLOCKCOUNT * segmentIndex + BLOCKBYTECOUNT * blockIndex).Take(BLOCKBYTECOUNT).ToArray();

                                byte[] blockHash = hashAlgorithm.ComputeHash(block);

                                logger.LogInfo("BlockHash{0} = Hash(Block): {1}", blockIndex, Utility.ToHexString(blockHash));

                                if (operationMode == OperationMode.RemoteHashVerification &&
                                    !blockHash.SequenceEqual(contentInformationStructure.blocks[segmentIndex].BlockHashes.Skip(blockIndex * hashBlockSize).Take(hashBlockSize)))
                                {
                                    logger.LogError("Server Returned: {0}", Utility.ToHexString(contentInformationStructure.blocks[segmentIndex].BlockHashes.Skip(blockIndex * hashBlockSize).Take(hashBlockSize).ToArray()));
                                }

                                blockHashList.AddRange(blockHash);

                                logger.Unindent();
                                logger.NewLine();
                            }

                            byte[] hod = hashAlgorithm.ComputeHash(blockHashList.ToArray());

                            logger.LogInfo(
                                "HoD = Hash(BlockHash0 + BlockHash1 + ... + BlockHashN): {0}",
                                Utility.ToHexString(hod));

                            if (operationMode == OperationMode.RemoteHashVerification &&
                                !hod.SequenceEqual(contentInformationStructure.segments[segmentIndex].SegmentHashOfData))
                            {
                                logger.LogError("Server Returned: {0}", Utility.ToHexString(contentInformationStructure.segments[segmentIndex].SegmentHashOfData));
                            }

                            logger.NewLine();

                            byte[] kp = hmacAlgorithm.ComputeHash(hod);

                            logger.LogInfo(
                                "Kp = HMAC(Ks, HoD): {0}",
                                Utility.ToHexString(kp));

                            if (operationMode == OperationMode.RemoteHashVerification &&
                                !kp.SequenceEqual(contentInformationStructure.segments[segmentIndex].SegmentSecret))
                            {
                                logger.LogError("Server Returned: {0}", Utility.ToHexString(contentInformationStructure.segments[segmentIndex].SegmentSecret));
                            }

                            logger.NewLine();

                            tempList.AddRange(hod);
                            tempList.AddRange(Encoding.Unicode.GetBytes(HOHODK_APPEND_STRING));

                            byte[] hoHoDK = hashAlgorithm.ComputeHash(tempList.ToArray());

                            logger.LogInfo(
                                "hoHoDK = HMAC(HoD + \"MS_P2P_CACHING\"): {0}",
                                Utility.ToHexString(hoHoDK));

                            logger.NewLine();

                            logger.Unindent();
                        }
                        break;

                    case BranchCacheVersion.V2:

                        PccrcUtility.GetHashAlgorithm(dwHashAlgoV2_Values.TRUNCATED_SHA512, out hashAlgorithm, out hmacAlgorithm);
                        hmacAlgorithm.Key = hashAlgorithm.ComputeHash(Encoding.Unicode.GetBytes(serverSecret)).Take(32).ToArray();

                        logger.LogInfo(
                            "Ks = Hash(ServerSecret): {0}",
                            Utility.ToHexString(hmacAlgorithm.Key));

                        logger.NewLine();

                        int segmentLength = BLOCKBYTECOUNT;
                        int chunkCount = 1;

                        if (operationMode == OperationMode.RemoteHashVerification)
                            chunkCount = contentInformationStructureV2.chunks.Length;

                        int segmentOffset = 0;
                        for (int chunkIndex = 0; chunkIndex < chunkCount; chunkIndex++)
                        {
                            logger.LogInfo("Chunk{0}", chunkIndex);
                            logger.NewLine();
                            logger.Indent();

                            segmentCount = content.Length / segmentLength;
                            if (content.Length > segmentCount * segmentLength)
                                segmentCount++;

                            if (operationMode == OperationMode.RemoteHashVerification)
                                segmentCount = contentInformationStructureV2.chunks[chunkIndex].chunkData.Length;

                            for (int segmentIndex = 0; segmentIndex < segmentCount; ++segmentIndex)
                            {
                                logger.LogInfo(
                                    "Segment{0} Offset {1} Length {2}",
                                    segmentIndex,
                                    segmentOffset,
                                    BLOCKBYTECOUNT);
                                logger.NewLine();
                                logger.Indent();

                                if (operationMode == OperationMode.RemoteHashVerification)
                                    segmentLength = (int)contentInformationStructureV2.chunks[chunkIndex].chunkData[segmentIndex].cbSegment;

                                List<byte> tempList = new List<byte>();

                                var segment = content.Skip(segmentOffset).Take(segmentLength).ToArray();

                                segmentOffset += segmentLength;

                                //TRANCATED_SHA_512
                                byte[] hod = hashAlgorithm.ComputeHash(segment).Take(32).ToArray();

                                logger.LogInfo(
                                    "HoD = Hash(Segment): {0}",
                                    Utility.ToHexString(hod));

                                if (operationMode == OperationMode.RemoteHashVerification &&
                                    !hod.SequenceEqual(contentInformationStructureV2.chunks[chunkIndex].chunkData[segmentIndex].SegmentHashOfData))
                                {
                                    logger.LogError("Server Returned: {0}", Utility.ToHexString(contentInformationStructureV2.chunks[chunkIndex].chunkData[segmentIndex].SegmentHashOfData));
                                }

                                logger.NewLine();

                                byte[] kp = hmacAlgorithm.ComputeHash(hod).Take(32).ToArray();

                                logger.LogInfo(
                                    "Kp = HMAC(Ks, HoD): {0}",
                                    Utility.ToHexString(kp));

                                if (operationMode == OperationMode.RemoteHashVerification &&
                                    !kp.SequenceEqual(contentInformationStructureV2.chunks[chunkIndex].chunkData[segmentIndex].SegmentSecret))
                                {
                                    logger.LogError("Server Returned: {0}", Utility.ToHexString(contentInformationStructureV2.chunks[chunkIndex].chunkData[segmentIndex].SegmentSecret));
                                }

                                logger.NewLine();

                                tempList.AddRange(hod);
                                tempList.AddRange(Encoding.Unicode.GetBytes(HOHODK_APPEND_STRING));

                                byte[] hoHoDK = hashAlgorithm.ComputeHash(tempList.ToArray());

                                logger.LogInfo(
                                    "hoHoDK = HMAC(HoD + \"MS_P2P_CACHING\"): {0}",
                                    Utility.ToHexString(hoHoDK));

                                logger.NewLine();

                                logger.Unindent();
                            }
                        }

                        break;

                    default:
                        throw new NotImplementedException();
                }

                if (operationMode == OperationMode.RemoteHashVerification)
                {
                    if (logger.HasError)
                    {
                        Utility.ShowMessageBox("Hash verification error found!", MessageBoxIcon.Error);
                    }
                    else
                    {
                        Utility.ShowMessageBox("Hash verification passed!", MessageBoxIcon.Information);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message + "\r\n\r\n" + ex.StackTrace, MessageBoxIcon.Error);
            }
        }

        private bool CheckInput()
        {
            var operationMode = (OperationMode)operationModeComboBox.SelectedItem;
            var transport = (ContentInformationTransport)transportComboBox.SelectedItem;

            if (string.IsNullOrWhiteSpace(filePathTextBox.Text))
            {
                Utility.ShowMessageBox(
                   "Please specify the file path.",
                   MessageBoxIcon.Error);
                return false;
            }

            if (operationMode == OperationMode.RemoteHashVerification &&
                transport == ContentInformationTransport.SMB2 &&
                !Regex.IsMatch(filePathTextBox.Text.ToLower(), smb2FilePathPattern))
            {
                Utility.ShowMessageBox(
                   @"The file path is not having correct format: \\Server\SharedFolder\FileName.ext",
                   MessageBoxIcon.Error);
                return false;
            }

            if (operationMode == OperationMode.RemoteHashVerification &&
                transport == ContentInformationTransport.PCCRTP &&
                !Regex.IsMatch(filePathTextBox.Text.ToLower(), httpFilePathPattern))
            {
                Utility.ShowMessageBox(
                   @"The file path is not having correct format: http://Server/FileName.ext",
                   MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(serverSecretTextBox.Text))
            {
                Utility.ShowMessageBox(
                   "Please specify the server secret.",
                   MessageBoxIcon.Error);
                return false;
            }

            if (operationMode == OperationMode.RemoteHashVerification &&
                transport == ContentInformationTransport.SMB2 &&
                string.IsNullOrWhiteSpace(userNameTextBox.Text))
            {
                Utility.ShowMessageBox(
                   "Please specify user name.",
                   MessageBoxIcon.Error);
                return false;
            }

            if (operationMode == OperationMode.RemoteHashVerification &&
                transport == ContentInformationTransport.SMB2 && 
                string.IsNullOrWhiteSpace(userNameTextBox.Text))
            {
                Utility.ShowMessageBox(
                   "Please specify password.",
                   MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void saveLogButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (logRichTextBox.TextLength == 0)
                {
                    Utility.ShowMessageBox(
                       "Log not available.",
                       MessageBoxIcon.Error);

                    return;
                }

                SaveFileDialog saveFlieDlg = new SaveFileDialog();
                saveFlieDlg.Filter = "Rich Text Files (*.rtf)|*.rtf";
                saveFlieDlg.RestoreDirectory = true;
                if (saveFlieDlg.ShowDialog() == DialogResult.OK)
                {
                    logRichTextBox.SaveFile(saveFlieDlg.FileName);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message + "\r\n\r\n" + ex.StackTrace, MessageBoxIcon.Error);
            }
        }
    }

    public enum OperationMode
    {
        RemoteHashVerification,
        LocalHashGeneration,
    }
}
