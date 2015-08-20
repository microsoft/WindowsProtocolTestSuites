using System;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using StackSmb = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// MS-SMB test suite definition.
    /// </summary>
    [TestClass]
    public partial class TestSuite : TestClassBase
    {
        #region Private variable

        private const ushort SmbTreeConnectAndxFlags = (ushort)(
            StackSmb.TreeConnectFlags.TREE_CONNECT_ANDX_DISCONNECT_TID
            | StackSmb.TreeConnectFlags.TREE_CONNECT_ANDX_EXTENDED_RESPONSE
            | StackSmb.TreeConnectFlags.TREE_CONNECT_ANDX_EXTENDED_SIGNATURES);

        private static SmbClient smbClientStack;
        private static string serverName = string.Empty;
        private static int serverPort = 0;
        private static string domainName = string.Empty;
        private static string userName = string.Empty;
        private static string password = string.Empty;
        private static IpVersion ipVersion = IpVersion.Ipv4;
        private static int bufferSize = 0;
        private static TimeSpan timeout = new TimeSpan();
        private static Platform sutOsVersion = Platform.Win2K8R2;

        #endregion

        #region Adapter Instances

        private ISmbAdapter ISMBAdapterInstance = null;
        private IServerSetupAdapter IServerSetupAdapterInstance = null;

        #endregion

        #region Class Initialization and Cleanup

        /// <summary>
        /// Use ClassInitialize to run code before running the first test in the class.
        /// </summary>
        /// <param name="context">testContext</param>
        [ClassInitialize]
        public static void ClassInitialize(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context)
        {
            PtfTestClassBase.Initialize(context);
            
            // The time out length is 2000 milliseconds.
            timeout = TimeSpan.FromMilliseconds(2000);
        }


        /// <summary>
        /// Use ClassCleanup to run code after all tests in a class have run.
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (null != smbClientStack)
            {
                smbClientStack.Dispose();
                smbClientStack = null;
            }
            PtfTestClassBase.Cleanup();
        }

        #endregion

        #region Test initialization

        /// <summary>
        /// Use TestInitialize to run code before running the test in the class.
        /// </summary>
        protected override void TestInitialize()
        {
            base.TestInitialize();

            this.ISMBAdapterInstance = Site.GetAdapter<ISmbAdapter>();

            this.IServerSetupAdapterInstance = Site.GetAdapter<IServerSetupAdapter>();

            smbClientStack = new SmbClient();

            serverName = Site.Properties["SutMachineName"];
            serverPort = int.Parse(Site.Properties["SutPort"]);
            string ip = Site.Properties["SmbTransportIpVersion"];

            switch (ip.ToLower())
            {
                case "ipv4":
                    ipVersion = IpVersion.Ipv4;
                    break;
                case "ipv6":
                    ipVersion = IpVersion.Ipv6;
                    break;
                default:
                    ipVersion = IpVersion.Any;
                    break;
            }

            bufferSize = int.Parse(Site.Properties["SmbTransportBufferSize"]);
            domainName = Site.Properties["SutLoginDomain"];
            userName = Site.Properties["SutLoginAdminUserName"];
            password = Site.Properties["SutLoginAdminPwd"];
            string ptfSutOs = Site.Properties["SutPlatformOS"].ToString();

            if (Enum.Parse(typeof(Platform), ptfSutOs, true) != null)
            {
                sutOsVersion = (Platform)Enum.Parse(typeof(Platform), ptfSutOs, true);
            }
            else
            {
                sutOsVersion = Platform.NonWindows;
            }
        }


        /// <summary>
        /// Use ClassCleanup to run code after a test case in a class have run.
        /// </summary>
        protected override void TestCleanup()
        {
            base.TestCleanup();
            if (null != smbClientStack)
            {
                smbClientStack.Dispose();
                smbClientStack = null;
            }
        }

        #endregion

        #region Help Methods

        /// Verify whether the two byte arrays are equal or not.
        /// </summary>
        /// <param name="b1">The First Byte Array</param>
        /// <param name="response2">The Second Array</param>
        private bool CompareArrayEquals(Array array1, Array array2)
        {
            if (array1 == null && array2 == null) return true;
            if ((array1 == null && array2 != null) || (array1 != null && array2 == null))
            { return false; }
            if (!Object.ReferenceEquals(array1.GetType(), array2.GetType()))
            { return false; }

            if (array1.GetLength(0) != array2.GetLength(0))
            { return false; }

            ValueType v1, v2;
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                v1 = (ValueType)array1.GetValue(i);
                v2 = (ValueType)array2.GetValue(i);
                if (!v1.Equals(v2)) return false;
            }
            return true;
        }


        /// <summary>
        /// Verify whether the two response are equal or not
        /// </summary>
        /// <param name="smbFsctlSrvCopyChunkPacket1"> The SmbNtTransFsctlSrvCopyChunkResponsePacket Response For First</param>
        /// <param name="smbFsctlSrvCopyChunkPacket2"> The SmbNtTransFsctlSrvCopyChunkResponsePacket Response For Second</param>
        private bool VerifyResponse(
            SmbNtTransFsctlSrvCopyChunkResponsePacket smbFsctlSrvCopyChunkPacket1,
            SmbNtTransFsctlSrvCopyChunkResponsePacket smbFsctlSrvCopyChunkPacket2)
        {
            return (smbFsctlSrvCopyChunkPacket1.IsSignRequired == smbFsctlSrvCopyChunkPacket2.IsSignRequired)
                && (smbFsctlSrvCopyChunkPacket1.NtTransData.ChunkBytesWritten ==
                smbFsctlSrvCopyChunkPacket2.NtTransData.ChunkBytesWritten)
                && (smbFsctlSrvCopyChunkPacket1.NtTransData.ChunksWritten ==
                smbFsctlSrvCopyChunkPacket2.NtTransData.ChunksWritten)
                && (smbFsctlSrvCopyChunkPacket1.NtTransData.TotalBytesWritten ==
                smbFsctlSrvCopyChunkPacket2.NtTransData.TotalBytesWritten)
                && (smbFsctlSrvCopyChunkPacket1.PacketType == smbFsctlSrvCopyChunkPacket2.PacketType)
                && (smbFsctlSrvCopyChunkPacket1.SmbData.ByteCount == smbFsctlSrvCopyChunkPacket2.SmbData.ByteCount)
                && CompareArrayEquals(
                smbFsctlSrvCopyChunkPacket1.SmbData.Data,
                smbFsctlSrvCopyChunkPacket2.SmbData.Data)
                && CompareArrayEquals(
                smbFsctlSrvCopyChunkPacket1.SmbData.Pad1,
                smbFsctlSrvCopyChunkPacket2.SmbData.Pad1)
                && CompareArrayEquals(
                smbFsctlSrvCopyChunkPacket1.SmbData.Pad2,
                smbFsctlSrvCopyChunkPacket2.SmbData.Pad2)
                && CompareArrayEquals(
                smbFsctlSrvCopyChunkPacket1.SmbData.Parameters,
                smbFsctlSrvCopyChunkPacket2.SmbData.Parameters)
                && (smbFsctlSrvCopyChunkPacket1.SmbParameters.DataCount ==
                smbFsctlSrvCopyChunkPacket2.SmbParameters.DataCount)
                && (smbFsctlSrvCopyChunkPacket1.SmbParameters.DataDisplacement ==
                smbFsctlSrvCopyChunkPacket2.SmbParameters.DataDisplacement)
                && (smbFsctlSrvCopyChunkPacket1.SmbParameters.DataOffset ==
                smbFsctlSrvCopyChunkPacket2.SmbParameters.DataOffset)
                && (smbFsctlSrvCopyChunkPacket1.SmbParameters.ParameterCount ==
                smbFsctlSrvCopyChunkPacket2.SmbParameters.ParameterCount)
                && (smbFsctlSrvCopyChunkPacket1.SmbParameters.ParameterDisplacement ==
                smbFsctlSrvCopyChunkPacket2.SmbParameters.ParameterDisplacement)
                && (smbFsctlSrvCopyChunkPacket1.SmbParameters.ParameterOffset ==
                smbFsctlSrvCopyChunkPacket2.SmbParameters.ParameterOffset)
                && CompareArrayEquals(
                smbFsctlSrvCopyChunkPacket1.SmbParameters.Reserved1,
                smbFsctlSrvCopyChunkPacket2.SmbParameters.Reserved1)
                && CompareArrayEquals(
                smbFsctlSrvCopyChunkPacket1.SmbParameters.Setup,
                smbFsctlSrvCopyChunkPacket2.SmbParameters.Setup)
                && (smbFsctlSrvCopyChunkPacket1.SmbParameters.SetupCount ==
                smbFsctlSrvCopyChunkPacket2.SmbParameters.SetupCount)
                && (smbFsctlSrvCopyChunkPacket1.SmbParameters.TotalDataCount ==
                smbFsctlSrvCopyChunkPacket2.SmbParameters.TotalDataCount)
                && (smbFsctlSrvCopyChunkPacket1.SmbParameters.TotalParameterCount ==
                smbFsctlSrvCopyChunkPacket2.SmbParameters.TotalParameterCount)
                && (smbFsctlSrvCopyChunkPacket1.SmbParameters.WordCount ==
                smbFsctlSrvCopyChunkPacket2.SmbParameters.WordCount);
        }


        /// <summary>
        /// Verify whether the two response are equal or not
        /// </summary>
        /// <param name="response"> The SmbNtCreateAndxResponsePacketResponse For First</param>
        /// <param name="response2"> The SmbNtCreateAndxResponsePacket Response For Second</param>
        private bool VerifyCreateResponse(
            SmbNtCreateAndxResponsePacket response,
            SmbNtCreateAndxResponsePacket response2)
        {
            return (response.IsSignRequired == response2.IsSignRequired)
                && (response.PacketType == response2.PacketType)
                && (response.SmbData.ByteCount == response2.SmbData.ByteCount)
                && (response.SmbParameters.AllocationSize == response2.SmbParameters.AllocationSize)
                && (response.SmbParameters.AndXCommand == response2.SmbParameters.AndXCommand)
                && (response.SmbParameters.AndXOffset == response2.SmbParameters.AndXOffset)
                && (response.SmbParameters.AndXReserved == response2.SmbParameters.AndXReserved)
                && (response.SmbParameters.CreateTime.Time == response2.SmbParameters.CreateTime.Time)
                && (response.SmbParameters.NMPipeStatus_or_FileStatusFlags ==
                response2.SmbParameters.NMPipeStatus_or_FileStatusFlags)
                && (response.SmbParameters.Directory == response2.SmbParameters.Directory)
                && (response.SmbParameters.EndOfFile == response2.SmbParameters.EndOfFile)
                && (response.SmbParameters.ExtFileAttributes == response2.SmbParameters.ExtFileAttributes)
                && CompareArrayEquals(response.SmbParameters.FileId, response2.SmbParameters.FileId)
                && (response.SmbParameters.ResourceType == response2.SmbParameters.ResourceType)
                && CompareArrayEquals(
                response.SmbParameters.GuestMaximalAccessRights,
                response2.SmbParameters.GuestMaximalAccessRights)
                && (response.SmbParameters.LastAccessTime.Time == response2.SmbParameters.LastAccessTime.Time)
                && (response.SmbParameters.LastChangeTime.Time == response2.SmbParameters.LastChangeTime.Time)
                && (response.SmbParameters.LastWriteTime.Time == response2.SmbParameters.LastWriteTime.Time)
                && CompareArrayEquals(
                response.SmbParameters.MaximalAccessRights,
                response2.SmbParameters.MaximalAccessRights)
                && (response.SmbParameters.OplockLevel == response2.SmbParameters.OplockLevel)
                && CompareArrayEquals(response.SmbParameters.VolumeGUID, response2.SmbParameters.VolumeGUID)
                && (response.SmbParameters.WordCount == response2.SmbParameters.WordCount);
        }


        /// <summary>
        /// Verify whether the two response are equal or not
        /// </summary>
        /// <param name="readAndxResponsePacket1"> The SmbReadAndxResponsePacket For First</param>
        /// <param name="readAndxResponsePacket2"> The SmbReadAndxResponsePacket Response For Second</param>
        private bool VerifyReadAndxResponse(SmbReadAndxResponsePacket readAndxResponsePacket1,
                                            SmbReadAndxResponsePacket readAndxResponsePacket2)
        {
            return (readAndxResponsePacket1.AndxPacket.IsSignRequired ==
                readAndxResponsePacket2.AndxPacket.IsSignRequired)
                && (readAndxResponsePacket1.AndxPacket.PacketBytes == readAndxResponsePacket2.AndxPacket.PacketBytes)
                && (readAndxResponsePacket1.AndxPacket.PacketType == readAndxResponsePacket2.AndxPacket.PacketType)
                && (readAndxResponsePacket1.IsSignRequired == readAndxResponsePacket2.IsSignRequired)
                && (readAndxResponsePacket1.PacketBytes.IsFixedSize == readAndxResponsePacket2.PacketBytes.IsFixedSize)
                && (readAndxResponsePacket1.PacketBytes.IsReadOnly == readAndxResponsePacket2.PacketBytes.IsReadOnly)
                && (readAndxResponsePacket1.PacketBytes.IsSynchronized ==
                readAndxResponsePacket2.PacketBytes.IsSynchronized)
                && (readAndxResponsePacket1.PacketBytes.Length == readAndxResponsePacket2.PacketBytes.Length)
                && (readAndxResponsePacket1.PacketBytes.LongLength == readAndxResponsePacket2.PacketBytes.LongLength)
                && (readAndxResponsePacket1.PacketBytes.Rank == readAndxResponsePacket2.PacketBytes.Rank)
                && (readAndxResponsePacket1.PacketBytes.SyncRoot == readAndxResponsePacket2.PacketBytes.SyncRoot)
                && (readAndxResponsePacket1.PacketType == readAndxResponsePacket2.PacketType)
                && (readAndxResponsePacket1.SmbData.ByteCount == readAndxResponsePacket2.SmbData.ByteCount)
                && (readAndxResponsePacket1.SmbData.Data == readAndxResponsePacket2.SmbData.Data)
                && (readAndxResponsePacket1.SmbData.Pad == readAndxResponsePacket2.SmbData.Pad)
                && (readAndxResponsePacket1.SmbParameters.AndXCommand ==
                readAndxResponsePacket2.SmbParameters.AndXCommand)
                && (readAndxResponsePacket1.SmbParameters.AndXOffset ==
                readAndxResponsePacket2.SmbParameters.AndXOffset)
                && (readAndxResponsePacket1.SmbParameters.DataOffset ==
                readAndxResponsePacket2.SmbParameters.DataOffset)
                && (readAndxResponsePacket1.SmbParameters.AndXReserved ==
                readAndxResponsePacket2.SmbParameters.AndXReserved)
                && (readAndxResponsePacket1.SmbParameters.Available == readAndxResponsePacket2.SmbParameters.Available)
                && (readAndxResponsePacket1.SmbParameters.DataCompactionMode ==
                readAndxResponsePacket2.SmbParameters.DataCompactionMode)
                && (readAndxResponsePacket1.SmbParameters.DataLength ==
                readAndxResponsePacket2.SmbParameters.DataLength)
                && (readAndxResponsePacket1.SmbParameters.DataOffset ==
                readAndxResponsePacket2.SmbParameters.DataOffset)
                && (readAndxResponsePacket1.SmbParameters.Reserved1 == readAndxResponsePacket2.SmbParameters.Reserved1)
                && (readAndxResponsePacket1.SmbParameters.Reserved2 == readAndxResponsePacket2.SmbParameters.Reserved2);
        }


        /// <summary>
        /// Verify whether the two response are equal or not 
        /// </summary>
        /// <param name="response"> The First Response </param>
        /// <param name="response2"> The Second Response </param>
        private bool VerifyTreeConnectResponse(
            SmbTreeConnectAndxResponsePacket response,
            SmbTreeConnectAndxResponsePacket response2)
        {
            return (response.IsSignRequired == response2.IsSignRequired)
                && (response.PacketType == response2.PacketType)
                && (response.SmbData.ByteCount == response2.SmbData.ByteCount)
                && CompareArrayEquals(response.SmbData.NativeFileSystem, response.SmbData.NativeFileSystem)
                && CompareArrayEquals(response.SmbData.Service, response.SmbData.Service)
                & (response.SmbParameters.AndXCommand == response2.SmbParameters.AndXCommand)
                && (response.SmbParameters.AndXOffset == response2.SmbParameters.AndXOffset)
                && (response.SmbParameters.AndXReserved == response2.SmbParameters.AndXReserved)
                && (response.SmbParameters.GuestMaximalShareAccessRights ==
                response2.SmbParameters.GuestMaximalShareAccessRights)
                && (response.SmbParameters.MaximalShareAccessRights == response2.SmbParameters.MaximalShareAccessRights)
                && (response.SmbParameters.OptionalSupport == response2.SmbParameters.OptionalSupport)
                && (response.SmbParameters.WordCount == response2.SmbParameters.WordCount);
        }


        /// <summary>
        /// Verify whether the two response are equal or not
        /// </summary>
        /// <param name="smbTrans2FindNext2ResponsePacket1"> The SmbTrans2FindNext2ResponsePacket For First</param>
        /// <param name="smbTrans2FindNext2ResponsePacket2"> The SmbTrans2FindNext2ResponsePacket Response For Second</param>
        private bool VerifySmbTrans2FindNext2ResponsePacket(
                 SmbTrans2FindNext2ResponsePacket smbTrans2FindNext2ResponsePacket1,
                 SmbTrans2FindNext2ResponsePacket smbTrans2FindNext2ResponsePacket2)
        {
            return (smbTrans2FindNext2ResponsePacket1.IsSignRequired == smbTrans2FindNext2ResponsePacket2.IsSignRequired)
                && CompareArrayEquals(
                smbTrans2FindNext2ResponsePacket1.PacketBytes,
                smbTrans2FindNext2ResponsePacket2.PacketBytes)
                && (smbTrans2FindNext2ResponsePacket1.PacketType ==
                smbTrans2FindNext2ResponsePacket2.PacketType)
                && (smbTrans2FindNext2ResponsePacket1.SmbData.ByteCount ==
                smbTrans2FindNext2ResponsePacket2.SmbData.ByteCount)
                && CompareArrayEquals(
                smbTrans2FindNext2ResponsePacket1.SmbData.Pad1,
                smbTrans2FindNext2ResponsePacket2.SmbData.Pad1)
                && CompareArrayEquals(
                smbTrans2FindNext2ResponsePacket1.SmbData.Pad2,
                smbTrans2FindNext2ResponsePacket2.SmbData.Pad2)
                && CompareArrayEquals(
                smbTrans2FindNext2ResponsePacket1.SmbData.Trans2_Data,
                smbTrans2FindNext2ResponsePacket2.SmbData.Trans2_Data)
                && CompareArrayEquals(
                smbTrans2FindNext2ResponsePacket1.SmbData.Trans2_Parameters,
                smbTrans2FindNext2ResponsePacket2.SmbData.Trans2_Parameters)
                && (smbTrans2FindNext2ResponsePacket1.SmbParameters.DataCount ==
                smbTrans2FindNext2ResponsePacket2.SmbParameters.DataCount)
                && (smbTrans2FindNext2ResponsePacket1.SmbParameters.DataDisplacement ==
                smbTrans2FindNext2ResponsePacket2.SmbParameters.DataDisplacement)
                && (smbTrans2FindNext2ResponsePacket1.SmbParameters.DataOffset ==
                smbTrans2FindNext2ResponsePacket2.SmbParameters.DataOffset)
                && (smbTrans2FindNext2ResponsePacket1.SmbParameters.ParameterCount ==
                smbTrans2FindNext2ResponsePacket2.SmbParameters.ParameterCount)
                && (smbTrans2FindNext2ResponsePacket1.SmbParameters.ParameterDisplacement ==
                smbTrans2FindNext2ResponsePacket2.SmbParameters.ParameterDisplacement)
                && (smbTrans2FindNext2ResponsePacket1.SmbParameters.ParameterOffset ==
                smbTrans2FindNext2ResponsePacket2.SmbParameters.ParameterOffset)
                && (smbTrans2FindNext2ResponsePacket1.SmbParameters.Reserved1 ==
                smbTrans2FindNext2ResponsePacket2.SmbParameters.Reserved1)
                && (smbTrans2FindNext2ResponsePacket1.SmbParameters.Reserved2 ==
                smbTrans2FindNext2ResponsePacket2.SmbParameters.Reserved2)
                && CompareArrayEquals(
                smbTrans2FindNext2ResponsePacket1.SmbParameters.Setup,
                smbTrans2FindNext2ResponsePacket2.SmbParameters.Setup)
                && (smbTrans2FindNext2ResponsePacket1.SmbParameters.SetupCount ==
                smbTrans2FindNext2ResponsePacket2.SmbParameters.SetupCount)
                && (smbTrans2FindNext2ResponsePacket1.SmbParameters.TotalDataCount ==
                smbTrans2FindNext2ResponsePacket2.SmbParameters.TotalDataCount)
                && (smbTrans2FindNext2ResponsePacket1.SmbParameters.TotalParameterCount ==
                smbTrans2FindNext2ResponsePacket2.SmbParameters.TotalParameterCount)
                && (smbTrans2FindNext2ResponsePacket1.SmbParameters.WordCount ==
                smbTrans2FindNext2ResponsePacket2.SmbParameters.WordCount);
        }


        /// <summary>
        /// Verify whether the two response are equal or not.
        /// </summary>
        /// <param name="response"> The first SmbNegotiateResponsePacket.</param>
        /// <param name="response2"> The second SmbNegotiateResponsePacket Response.</param>
        private bool VerifyNegotiateResponse(
            SmbNegotiateResponsePacket response,
            SmbNegotiateResponsePacket response2)
        {
            return (response.IsSignRequired == response2.IsSignRequired)
                && (response.PacketType == response2.PacketType)
                && (response.SmbData.ByteCount == response2.SmbData.ByteCount)
                && CompareArrayEquals(response.SmbData.SecurityBlob, response2.SmbData.SecurityBlob)
                && (response.SmbData.ServerGuid == response2.SmbData.ServerGuid)
                && (response.SmbParameters.Capabilities == response2.SmbParameters.Capabilities)
                && (response.SmbParameters.DialectIndex == response2.SmbParameters.DialectIndex)
                && (response.SmbParameters.EncryptionKeyLength == response2.SmbParameters.EncryptionKeyLength)
                && (response.SmbParameters.MaxBufferSize == response2.SmbParameters.MaxBufferSize)
                && (response.SmbParameters.MaxMpxCount == response2.SmbParameters.MaxMpxCount)
                && (response.SmbParameters.MaxNumberVcs == response2.SmbParameters.MaxNumberVcs)
                && (response.SmbParameters.MaxRawSize == response2.SmbParameters.MaxRawSize)
                && (response.SmbParameters.SecurityMode == response2.SmbParameters.SecurityMode)
                && (response.SmbParameters.ServerTimeZone == response2.SmbParameters.ServerTimeZone)
                && (response.SmbParameters.SessionKey == response2.SmbParameters.SessionKey)
                && (response.SmbParameters.WordCount == response2.SmbParameters.WordCount);
        }


        /// <summary>
        /// Verify whether the two NtTransQueryQuota response are equal or not.
        /// </summary>
        /// <param name="smbNtTransQueryQuotaPacket1"> The first NtTransQueryQuota response.</param>
        /// <param name="smbNtTransQueryQuotaPacket2"> The second NtTransQueryQuota response.</param>
        /// <returns></returns>
        private bool VerifyNtTransQueryQuotaResponse(
            SmbNtTransQueryQuotaResponsePacket smbNtTransQueryQuotaPacket1,
            SmbNtTransQueryQuotaResponsePacket smbNtTransQueryQuotaPacket2)
        {
            return (smbNtTransQueryQuotaPacket1.IsSignRequired == smbNtTransQueryQuotaPacket2.IsSignRequired)
                && (smbNtTransQueryQuotaPacket1.NtTransDataList.Count ==
                smbNtTransQueryQuotaPacket2.NtTransDataList.Count)
                && CompareArrayEquals(
                smbNtTransQueryQuotaPacket1.PacketBytes,
                smbNtTransQueryQuotaPacket2.PacketBytes)
                && (smbNtTransQueryQuotaPacket1.PacketType == smbNtTransQueryQuotaPacket2.PacketType)
                && CompareArrayEquals(
                smbNtTransQueryQuotaPacket1.SmbData.Pad1,
                smbNtTransQueryQuotaPacket2.SmbData.Pad1)
                && CompareArrayEquals(
                smbNtTransQueryQuotaPacket1.SmbData.Pad2,

                smbNtTransQueryQuotaPacket2.SmbData.Pad2)
                && (smbNtTransQueryQuotaPacket1.SmbParameters.DataDisplacement == smbNtTransQueryQuotaPacket2.SmbParameters.DataDisplacement)
                && (smbNtTransQueryQuotaPacket1.SmbParameters.DataOffset ==
                smbNtTransQueryQuotaPacket2.SmbParameters.DataOffset)
                && (smbNtTransQueryQuotaPacket1.SmbParameters.ParameterCount ==
                smbNtTransQueryQuotaPacket2.SmbParameters.ParameterCount)
                && (smbNtTransQueryQuotaPacket1.SmbParameters.ParameterDisplacement ==
                smbNtTransQueryQuotaPacket2.SmbParameters.ParameterDisplacement)
                && (smbNtTransQueryQuotaPacket1.SmbParameters.ParameterOffset ==
                smbNtTransQueryQuotaPacket2.SmbParameters.ParameterOffset)
                && CompareArrayEquals(
                smbNtTransQueryQuotaPacket1.SmbParameters.Reserved1,
                smbNtTransQueryQuotaPacket2.SmbParameters.Reserved1)
                && CompareArrayEquals(
                smbNtTransQueryQuotaPacket1.SmbParameters.Setup,
                smbNtTransQueryQuotaPacket2.SmbParameters.Setup)
                && (smbNtTransQueryQuotaPacket1.SmbParameters.SetupCount ==
                smbNtTransQueryQuotaPacket2.SmbParameters.SetupCount)
                && (smbNtTransQueryQuotaPacket1.SmbParameters.TotalParameterCount ==
                smbNtTransQueryQuotaPacket2.SmbParameters.TotalParameterCount)
                && (smbNtTransQueryQuotaPacket1.SmbParameters.WordCount ==
                smbNtTransQueryQuotaPacket2.SmbParameters.WordCount);
        }


        /// <summary>
        /// Verify whether the two response are equal or not.
        /// </summary>
        /// <param name="response"> The SmbTrans2SetFileInformationResponsePacket For First.</param>
        /// <param name="response2"> The SmbTrans2SetFileInformationResponsePacket Response For Second.</param>
        private bool VerifyisVerifyTrans2SetFileInformation(
            SmbTrans2SetFileInformationResponsePacket response,
            SmbTrans2SetFileInformationResponsePacket response2)
        {
            return (response.IsSignRequired == response2.IsSignRequired
                && CompareArrayEquals(response.PacketBytes, response2.PacketBytes)
                && (response.PacketType == response2.PacketType)
                && (response.SmbData.ByteCount == response2.SmbData.ByteCount)
                && response.SmbData.ByteCount == response2.SmbData.ByteCount)
                && CompareArrayEquals(response.SmbData.Pad1, response2.SmbData.Pad1)
                && CompareArrayEquals(response.SmbData.Pad2, response2.SmbData.Pad2)
                && CompareArrayEquals(response.SmbData.Trans2_Data, response2.SmbData.Trans2_Data)
                && CompareArrayEquals(response.SmbData.Trans2_Parameters, response2.SmbData.Trans2_Parameters)
                && (response.SmbParameters.DataCount == response2.SmbParameters.DataCount)
                && (response.SmbParameters.DataDisplacement == response2.SmbParameters.DataDisplacement)
                && (response.SmbParameters.DataOffset == response2.SmbParameters.DataOffset)
                && (response.SmbParameters.ParameterCount == response2.SmbParameters.ParameterCount)
                && (response.SmbParameters.ParameterDisplacement == response2.SmbParameters.ParameterDisplacement)
                && (response.SmbParameters.ParameterOffset == response2.SmbParameters.ParameterOffset)
                && (response.SmbParameters.Reserved1 == response2.SmbParameters.Reserved1)
                && (response.SmbParameters.Reserved2 == response2.SmbParameters.Reserved2)
                && CompareArrayEquals(response.SmbParameters.Setup, response2.SmbParameters.Setup)
                && (response.SmbParameters.SetupCount == response2.SmbParameters.SetupCount)
                && (response.SmbParameters.TotalDataCount == response2.SmbParameters.TotalDataCount)
                && (response.SmbParameters.TotalParameterCount == response2.SmbParameters.TotalParameterCount)
                && (response.SmbParameters.WordCount == response2.SmbParameters.WordCount);
        }

        #endregion
    }
}
