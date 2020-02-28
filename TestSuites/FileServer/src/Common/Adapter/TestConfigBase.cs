// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    public class TestConfigBase
    {
        /// <summary>
        /// Unsupported Ioctl Codes' list
        /// </summary>
        private List<CtlCode_Values> UnsupportedIoCtlCodesList;

        /// <summary>
        /// Not supported Create Contexts' list
        /// </summary>
        private List<CreateContextTypeValue> UnsupportedCreateContextsList;

        public DialectRevision MaxSmbVersionSupported;

        public DialectRevision MaxSmbVersionClientSupported;

        public bool SendSignedRequest;

        public List<string> ActiveTDIs;

        public readonly int WriteBufferLengthInKb;

        #region Properties

        public Smb2TransportType UnderlyingTransport
        {
            get
            {
                return ParsePropertyToEnum<Smb2TransportType>(GetProperty("UnderlyingTransport"), "UnderlyingTransport");
            }
        }

        public AccountCredential AccountCredential
        {
            get
            {
                return new AccountCredential(DomainName, UserName, UserPassword);
            }
        }

        public AccountCredential NonAdminAccountCredential
        {
            get
            {
                return new AccountCredential(DomainName, NonAdminUserName, UserPassword);
            }
        }

        public AccountCredential GuestAccountCredential
        {
            get
            {
                return new AccountCredential(DomainName, GuestUserName, UserPassword);
            }
        }

        public SecureString SecurePassword
        {
            get
            {
                SecureString secStr = new SecureString();
                foreach (char c in UserPassword)
                {
                    secStr.AppendChar(c);
                }
                return secStr;
            }
        }

        public SecurityPackageType DefaultSecurityPackage
        {
            get
            {
                return ParsePropertyToEnum<SecurityPackageType>(GetProperty("SupportedSecurityPackage"), "SupportedSecurityPackage");
            }
        }

        public RpceAuthenticationLevel DefaultRpceAuthenticationLevel
        {
            get
            {
                return ParsePropertyToEnum<RpceAuthenticationLevel>(GetProperty("RpceAuthenticationLevel"), "RpceAuthenticationLevel");
            }
        }

        public bool IsSMB1NegotiateEnabled
        {
            get
            {
                return Boolean.Parse(GetProperty("IsSMB1NegotiateEnabled"));
            }
        }

        public bool IsLeasingSupported
        {
            get
            {
                return Boolean.Parse(GetProperty("IsLeasingSupported"));
            }
        }

        public bool IsDirectoryLeasingSupported
        {
            get
            {
                return Boolean.Parse(GetProperty("IsDirectoryLeasingSupported"));
            }
        }

        public bool IsEncryptionSupported
        {
            get
            {
                return Boolean.Parse(GetProperty("IsEncryptionSupported"));
            }
        }

        public bool IsMultiChannelCapable
        {
            get
            {
                return Boolean.Parse(GetProperty("IsMultiChannelCapable"));
            }
        }

        public bool IsPersistentHandlesSupported
        {
            get
            {
                return Boolean.Parse(GetProperty("IsPersistentHandlesSupported"));
            }
        }

        public bool IsMultiCreditSupported
        {
            get
            {
                return Boolean.Parse(GetProperty("IsMultiCreditSupported"));
            }
        }

        public bool DisableVerifySignature
        {
            get
            {
                return Boolean.Parse(GetProperty("DisableVerifySignature"));
            }
        }

        public bool IsGlobalEncryptDataEnabled
        {
            get
            {
                return Boolean.Parse(GetProperty("IsGlobalEncryptDataEnabled"));
            }
        }

        public bool IsGlobalRejectUnencryptedAccessEnabled
        {
            get
            {
                return Boolean.Parse(GetProperty("IsGlobalRejectUnencryptedAccessEnabled"));
            }
        }

        public DialectRevision[] RequestDialects
        {
            get
            {
                return Smb2Utility.GetDialects(MaxSmbVersionClientSupported);
            }
        }

        public ITestSite Site
        {
            get;
            set;
        }

        #region Network Configuration
        public IPAddress ClientNic1IPAddress
        {
            get
            {
                var result = GetProperty("ClientNic1IPAddress").ParseIPAddress();
                Site.Assume.IsTrue(result != IPAddress.None, "ClientNic1IPAddress should be a valid IP address or a resolvable host name!");
                return result;
            }
        }
        public IPAddress ClientNic2IPAddress
        {
            get
            {
                var result = GetProperty("ClientNic2IPAddress", false).ParseSecondaryIPAddress();
                Site.Assume.IsTrue(result != IPAddress.None, "ClientNic2IPAddress should be a valid IP address or a resolvable host name with at least two IP addresses!");
                return result;
            }
        }
        #endregion

        #region Share and File Configuration
        public string BasicFileShare
        {
            get
            {
                return GetProperty("BasicFileShare");
            }
        }

        public string EncryptedFileShare
        {
            get
            {
                return GetProperty("EncryptedFileShare");
            }
        }

        public string CAShareName
        {
            get
            {
                return GetProperty("CAShareName");
            }
        }

        #endregion


        #region Global Test Configuration
        /// <summary>
        /// Timeout in seconds for SMB2 connection over transport
        /// </summary>
        public TimeSpan Timeout
        {
            get
            {
                return TimeSpan.FromSeconds(int.Parse(GetProperty("Timeout")));
            }
        }

        public TimeSpan LongerTimeout
        {
            get
            {
                return TimeSpan.FromSeconds(int.Parse(GetProperty("LongerTimeout")));
            }
        }

        public TimeSpan RetryInterval
        {
            get
            {
                return TimeSpan.FromSeconds(int.Parse(GetProperty("RetryInterval")));
            }
        }

        public Platform Platform
        {
            get
            {
                return ParsePropertyToEnum<Platform>(GetProperty("Platform"), "Platform");
            }
        }
        #endregion

        #region SUT Common Configuration
        public string SutComputerName
        {
            get
            {
                return GetProperty("SutComputerName");
            }
        }

        public IPAddress SutIPAddress
        {
            get
            {
                var result = GetProperty("SutIPAddress").ParseIPAddress();
                Site.Assume.IsTrue(result != IPAddress.None, "SutIPAddress should be a valid IP address or a resolvable host name!");
                return result;
            }
        }

        public string DomainName
        {
            get
            {
                // Allow DomainName being null
                return GetProperty("DomainName", false);
            }
        }

        public string UserName
        {
            get
            {
                return GetProperty("AdminUserName");
            }
        }

        public string NonAdminUserName
        {
            get
            {
                return GetProperty("NonAdminUserName");
            }
        }

        public string GuestUserName
        {
            get
            {
                return GetProperty("GuestUserName");
            }
        }

        public string UserPassword
        {
            get
            {
                return GetProperty("PasswordForAllUsers");
            }
        }

        public bool UseServerGssToken
        {
            get
            {
                return bool.Parse(GetProperty("UseServerGssToken"));
            }
        }

        public uint MaxResiliencyTimeoutInSecond
        {
            get
            {
                return uint.Parse(GetProperty("MaxResiliencyTimeout")) / 1000;
            }
        }

        public bool IsWindowsPlatform
        {
            get
            {
                return this.Platform != Platform.NonWindows;
            }
        }

        public string CAShareServerName
        {
            get
            {
                return GetProperty("CAShareServerName");
            }
        }

        public IPAddress CAShareServerIP
        {
            get
            {
                var result = CAShareServerName.ParseIPAddress();
                Site.Assume.IsTrue(result != IPAddress.None, "CAShareServerIP should be a valid IP address or a resolvable host name!");
                return result;
            }
        }

        public bool IsServerSigningRequired
        {
            get
            {
                return Boolean.Parse(GetProperty("IsRequireMessageSigning"));
            }
        }

        public List<EncryptionAlgorithm> SupportedEncryptionAlgorithmList;

        public List<CompressionAlgorithm> SupportedCompressionAlgorithmList;

        #endregion

        #endregion

        public TestConfigBase(ITestSite site)
        {
            Site = site;

            WriteBufferLengthInKb = DEFAULT_WRITE_BUFFER_SIZE_IN_KB;

            string activeTDIsProperty = GetProperty("ActiveTDIs", false);
            if (!string.IsNullOrEmpty(activeTDIsProperty))
            {
                ActiveTDIs = new List<string>(activeTDIsProperty.Split(';'));
            }
            else
            {
                ActiveTDIs = new List<string>();
            }

            MaxSmbVersionSupported = ParsePropertyToEnum<DialectRevision>(GetProperty("MaxSmbVersionSupported"), "MaxSmbVersionSupported");

            MaxSmbVersionClientSupported = ParsePropertyToEnum<DialectRevision>(GetProperty("MaxSmbVersionClientSupported"), "MaxSmbVersionClientSupported");

            SupportedEncryptionAlgorithmList = ParsePropertyToList<EncryptionAlgorithm>("SupportedEncryptionAlgorithms");

            SupportedCompressionAlgorithmList = ParsePropertyToList<CompressionAlgorithm>("SupportedCompressionAlgorithms");

            SendSignedRequest = Boolean.Parse(GetProperty("SendSignedRequest"));

            if (!SendSignedRequest && MaxSmbVersionSupported == DialectRevision.Smb311)
            {
                Site.Assume.Fail("The config \"SendSignedRequest\" should not be false if \"MaxSmbVersionSupported\" is \"Smb311\".");
            }
        }

        public bool IsIoCtlCodeSupported(CtlCode_Values ioCtlCode)
        {
            if (UnsupportedIoCtlCodesList == null)
                UnsupportedIoCtlCodesList = ParsePropertyToList<CtlCode_Values>("UnsupportedIoCtlCodes");
            return !UnsupportedIoCtlCodesList.Contains(ioCtlCode);
        }

        public bool IsCreateContextSupported(CreateContextTypeValue createContext)
        {
            if (UnsupportedCreateContextsList == null)
                UnsupportedCreateContextsList = ParsePropertyToList<CreateContextTypeValue>("UnsupportedCreateContexts");
            return !UnsupportedCreateContextsList.Contains(createContext);
        }

        /// <summary>
        /// Get property value from grouped ptf config.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="checkNullOrEmpty">Check if the property is null or the value is empty.</param>
        /// <returns>The value of the property.</returns>
        public string GetProperty(string groupName, string propertyName, bool checkNullOrEmpty = true)
        {
            string propertyValue = Site.Properties[groupName + "." + propertyName];

            if (checkNullOrEmpty && string.IsNullOrEmpty(propertyValue))
            {
                if (propertyValue == null)
                {
                    Site.Assert.Inconclusive("The property {0} does not existed.", propertyName);
                }
                else
                {
                    Site.Assert.Inconclusive("The value of {0} is empty.", propertyName);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get property value from grouped ptf config.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="checkNullOrEmpty">Check if the property is null or the value is empty.</param>
        /// <returns>The value of the property.</returns>
        private string GetProperty(string propertyName, bool checkNullOrEmpty = true)
        {
            return GetProperty("Common", propertyName, checkNullOrEmpty);
        }

        protected List<T> ParsePropertyToList<T>(string property, string groupName = "Common") where T : struct
        {
            List<T> list = new List<T>();
            string propertyValue = GetProperty(groupName, property, false);
            if (string.IsNullOrEmpty(propertyValue)) return list;

            string[] values = propertyValue.Split(';');
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    list.Add(ParsePropertyToEnum<T>(value, property));
                }
            }

            return list;
        }

        protected T ParsePropertyToEnum<T>(string propertyValue, string propertyName) where T : struct
        {
            T result;
            if (!Enum.TryParse(propertyValue, out result))
            {
                // Should fail the case if value is not filled correctly
                Site.Assume.Fail("{0} is not a valid value of {1}.", propertyValue, propertyName);
            }
            return result;
        }

        public bool IsCapabilitySupported(NEGOTIATE_Response_Capabilities_Values capability)
        {
            switch (capability)
            {
                case NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING:
                    return this.IsLeasingSupported;
                case NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LARGE_MTU:
                    return this.IsMultiCreditSupported;
                case NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL:
                    return this.IsMultiChannelCapable;
                case NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES:
                    return this.IsPersistentHandlesSupported;
                case NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING:
                    return this.IsDirectoryLeasingSupported;
                case NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION:
                    return this.IsEncryptionSupported;
                default:
                    throw new Exception("Capability not supported");
            }
        }

        #region Const

        private const int DEFAULT_WRITE_BUFFER_SIZE_IN_KB = 32;

        #endregion

        #region Check SMB2 Server Capabilities
        public void CheckNegotiateCapabilities(NEGOTIATE_Response_Capabilities_Values capValue, NEGOTIATE_Response response)
        {
            if (!response.Capabilities.HasFlag(capValue))
            {
                Site.Assert.Inconclusive("This test case is not applicable due to following capability is not supported: {0}", (Capabilities_Values)capValue);
            }
        }

        /// <summary>
        /// Check whether actual dialect is bigger than expected dialect or not.
        /// </summary>
        /// <param name="expectedMinimalDialect">Expected minimal dialect.</param>
        /// <param name="response">Negotiate response header.</param>
        public void CheckNegotiateDialect(DialectRevision expectedMinimalDialect, NEGOTIATE_Response response)
        {
            if (response.DialectRevision < expectedMinimalDialect)
            {
                Site.Assert.Inconclusive(
                    "This test case is not applicable due to that actual dialect {0} is less than expected minimal dialect {1}",
                    (DialectRevision)response.DialectRevision,
                    (DialectRevision)expectedMinimalDialect);
            }
        }

        public void CheckIOCTL(params CtlCode_Values[] values)
        {
            foreach (CtlCode_Values value in values)
            {
                if (!IsIoCtlCodeSupported(value))
                {
                    Site.Assert.Inconclusive("Test case is applicable in servers that implement IOCTL {0}.", value);
                }
            }
        }

        public void CheckDialect(DialectRevision dialect)
        {
            if (MaxSmbVersionSupported < dialect)
            {
                Site.Assert.Inconclusive("Test case is applicable in servers that implement dialect {0}.", dialect);
            }
            if (MaxSmbVersionClientSupported < dialect)
            {
                Site.Assert.Inconclusive("Test case is applicable when the ptf property value of MaxSmbVersionClientSupported is larger than or equal to {0}.", dialect);
            }
        }

        public void CheckPlatform(Platform platform)
        {
            if (Platform < platform)
            {
                Site.Assert.Inconclusive("The case is applicable in {0}.", platform);
            }
        }

        public void CheckCapabilities(NEGOTIATE_Response_Capabilities_Values capabilities)
        {
            foreach (NEGOTIATE_Response_Capabilities_Values capability in Enum.GetValues(typeof(NEGOTIATE_Response_Capabilities_Values)))
            {
                if (capability != NEGOTIATE_Response_Capabilities_Values.NONE //Every capability contains NONE value. Skip checking it.
                    && capabilities.HasFlag(capability)
                    && !IsCapabilitySupported(capability))
                {
                    Site.Assert.Inconclusive("Test case is applicable in servers that support {0}.", capability);
                }
            }
        }

        public void CheckCreateContext(params CreateContextTypeValue[] values)
        {
            foreach (var value in values)
            {
                if (!IsCreateContextSupported(value))
                {
                    Site.Assert.Inconclusive("Test case is applicable in servers that implement Create Context {0}", value);
                }
            }
        }

        public void CheckSigning()
        {
            if (!SendSignedRequest)
            {
                Site.Assert.Inconclusive("Test case is applicable for signed request.");
            }
        }

        /// <summary>
        /// Check values of IsGlobalEncryptDataEnabled and IsGlobalRejectUnencryptedAccessEnabled for those part encryption required test cases
        /// Such as encrypt on one channel, encrypt at first and unencrypt later
        /// </summary>
        /// <param name="selectedDialect"></param>
        public void CheckServerEncrypt(DialectRevision selectedDialect = DialectRevision.Smb2Unknown)
        {
            if (this.IsGlobalEncryptDataEnabled)
            {
                if (selectedDialect == DialectRevision.Smb2Unknown)
                {
                    selectedDialect = MaxSmbVersionSupported < MaxSmbVersionClientSupported ? MaxSmbVersionSupported : MaxSmbVersionClientSupported;
                }
                if (selectedDialect < DialectRevision.Smb30)
                {
                    if (this.IsGlobalRejectUnencryptedAccessEnabled)
                    {
                        Site.Assert.Inconclusive("Test case is not applicable when dialect is less than SMB 3.0, both IsGlobalEncryptDataEnabled and IsGlobalRejectUnencryptedAccessEnabled set to true.");
                    }
                }
                else
                {
                    Site.Assert.Inconclusive("Test case is not applicable when dialect is SMB 3.0 or later and IsGlobalEncryptDataEnabled set to true.");
                }
            }
        }

        public void CheckEncryptionAlgorithm(EncryptionAlgorithm cipherId)
        {
            if (!SupportedEncryptionAlgorithmList.Contains(cipherId))
            {
                Site.Assert.Inconclusive("Test case is applicable for the server that supports {0}", cipherId);
            }
        }

        public bool IsCompressionSupported()
        {
            if (SupportedCompressionAlgorithmList.Count == 0
                || (SupportedCompressionAlgorithmList.Count == 1 && SupportedCompressionAlgorithmList[0] == CompressionAlgorithm.NONE))
            {
                return false;
            }

            // Windows 10 v1809 operating system and prior, Windows Server v1809 operating system and prior, and Windows Server 2019 and prior do not support compression.
            if (IsWindowsPlatform)
            {
                return Platform >= Platform.WindowsServerV1903; 
            }

            return true;
        }

        public void CheckCompressionAlgorithm(CompressionAlgorithm? compressionAlgorithm = null)
        {
            if (SupportedCompressionAlgorithmList.Count == 0
                || (SupportedCompressionAlgorithmList.Count == 1 && SupportedCompressionAlgorithmList[0] == CompressionAlgorithm.NONE))
            {
                Site.Assert.Inconclusive("SUT does not support compression!");
            }

            if (compressionAlgorithm != null)
            {
                if (!SupportedCompressionAlgorithmList.Contains(compressionAlgorithm.Value))
                {
                    Site.Assert.Inconclusive("The specified compression algorithm {0} is not supported by SUT!", compressionAlgorithm);
                }
            }
        }

        public void CheckNegotiateContext<T>(T request, Smb2NegotiateResponsePacket response)
        {
            if (response.PayLoad.DialectRevision != DialectRevision.Smb311)
            {
                return;
            }

            if (request is Smb2NegotiateRequestPacket)
            {
                Smb2NegotiateRequestPacket smb2Request = request as Smb2NegotiateRequestPacket;
                // 3.3.5.4: if Dialect is "3.1.1" Then the server MUST build a NegotiateContextList for its negotiate response and check Including below:
                // 1. The server MUST add an SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context to the response's NegotiateContextList.
                if (response.NegotiateContext_PREAUTH == null)
                {
                    Site.Assert.Fail("The server MUST add an SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context to the response's NegotiateContextList.");
                }

                // 2. HashAlgorithmCount MUST be set to 1
                Site.Assert.AreEqual<int>(1, response.NegotiateContext_PREAUTH.Value.HashAlgorithmCount, "The response's SMB2_PREAUTH_INTEGRITY_CAPABILITIES.HashAlgorithmCount MUST be set to 1");

                // 3. SMB2_PREAUTH_INTEGRITY_CAPABILITIES Salt buffer length same as SaltLength
                Site.Assert.AreEqual<int>(response.NegotiateContext_PREAUTH.Value.SaltLength, response.NegotiateContext_PREAUTH.Value.Salt.Length, "The response's SMB2_PREAUTH_INTEGRITY_CAPABILITIES Salt buffer length must same as SaltLength");

                // 4. If client haven't send a negotiate context the server should not response except SMB2_PREAUTH_INTEGRITY_CAPABILITIES

                if ((smb2Request.NegotiateContext_ENCRYPTION == null) && (response.NegotiateContext_ENCRYPTION != null))
                {
                    Site.Assert.Fail("The server Should not response a SMB2_ENCRYPTION_CAPABILITIES as it's not sent in request.");
                }
                if ((smb2Request.NegotiateContext_COMPRESSION == null) && (response.NegotiateContext_COMPRESSION != null))
                {
                    Site.Assert.Fail("The server Should not response a SMB2_COMPRESSION_CAPABILITIES as it's not sent in request.");
                }

                if (response.NegotiateContext_ENCRYPTION != null)
                {
                    Site.Assert.AreEqual<int>(1, response.NegotiateContext_ENCRYPTION.Value.CipherCount, "The response's SMB2_ENCRYPTION_CAPABILITIES.CipherCount MUST be set to 1");
                }
                if (response.NegotiateContext_COMPRESSION != null)
                {
                    Site.Assert.AreEqual<int>(1, response.NegotiateContext_COMPRESSION.Value.CompressionAlgorithmCount, "The response's SMB2_COMPRESSION_CAPABILITIES.CompressionAlgorithmCount MUST be set to 1");
                }
            }
            else if (request is SmbNegotiateRequestPacket)
            {
                Site.Assert.IsNull(response.NegotiateContext_ENCRYPTION, "The server Should not response a SMB2_ENCRYPTION_CAPABILITIES as request is SmbNegotiateRequestPacket.");
                Site.Assert.IsNull(response.NegotiateContext_COMPRESSION, "The server Should not response a SMB2_COMPRESSION_CAPABILITIES as request is SmbNegotiateRequestPacket.");
            }
        }
        #endregion
    }

    public enum Platform
    {
        /// <summary>
        /// Non Windows implementation
        /// </summary>
        NonWindows = 0x00000000,

        /// <summary>
        /// Windows Server 2008
        /// </summary>
        WindowsServer2008 = 0x10000002,

        /// <summary>
        /// Windows Server 2008 R2
        /// </summary>
        WindowsServer2008R2 = 0x10000004,

        /// <summary>
        /// Windows Server 2012
        /// </summary>
        WindowsServer2012 = 0x10000006,

        /// <summary>
        /// Windows Server 2012 R2
        /// </summary>
        WindowsServer2012R2 = 0x10000007,

        /// <summary>
        /// Windows Server 2016
        /// </summary>
        WindowsServer2016 = 0x10000008,

        /// <summary>
        /// Windows Server v1709
        /// </summary>
        WindowsServerV1709 = 0x10000009,

        /// <summary>
        /// Windows Server v1803
        /// </summary>
        WindowsServerV1803 = 0x1000000A,

        /// <summary>
        /// Windows Server 2019 
        /// </summary>
        WindowsServer2019 = 0x1000000B,

        /// <summary>
        /// Windows Server v1903 
        /// </summary>
        WindowsServerV1903 = 0x1000000C,
    }
}
