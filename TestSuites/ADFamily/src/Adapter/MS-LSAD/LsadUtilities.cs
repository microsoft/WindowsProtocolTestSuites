// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Lsa;
    using Microsoft.Protocols.TestTools.StackSdk.Dtyp;

    /// <summary>
    /// Tool class.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = @"Disable
    warning CA1506 because it will affect the implementation of Adapter and Model codes if do any changes about
    maintainability.")]
   internal class LsadUtilities
    {
        #region Constants

        public const uint INVALID_ROOTDIRECTORY = 20;
        public const uint INITIALISED_HANDLE = 1;
        public const uint INVALID_HANDLE = 53;
        public const int ReturnSuccess = 0;
        public const int ReturnInvalidParameter = 1;
        public const int ReturnNotSupported = 2;
        public const int ReturnAccessDenied = 3;
       
        // For Audit Log Information Class
        public const uint maxLogSize = 8192;
        public const uint auditLogPercenrFull = 0;
        public const uint auditRetention = 8533315;
        public const uint auditLogshutdown = 0;
        public const uint timetoshutdown = 288342;

        // Variable for security descritor access check
        public const uint securityDsrcrAccessCheck = 0x0CE0E000;
        public const uint invalidDesiredAccess = 0x0CE0E000;

        // For PdInformation
        public const string PdName = "0";

        public const uint MaxTrustHandleAccess = 0xF20F007F;

        public const uint TRUST_ATTRIBUTE_NON_TRANSITIVE = 0x00000001,
                          TRUST_ATTRIBUTE_UPLEVEL_ONLY = 0x00000002,
                          TRUST_ATTRIBUTE_QUARANTINED_DOMAIN = 0x00000004,
                          TRUST_ATTRIBUTE_FOREST_TRANSITIVE = 0x00000008,
                          TRUST_ATTRIBUTE_CROSS_ORGANIZATION = 0x00000010,
                          TRUST_ATTRIBUTE_WITHIN_FOREST = 0x00000020,
                          TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL = 0x00000020,
                          TRUST_ATTRIBUTE_USES_RC4_ENCRYPTION = 0x00000080;

        public const uint KERB_ENCTYPE_DES_CBC_CRC = 0x00000001,
                          KERB_ENCTYPE_DES_CBC_MD5 = 0x00000002,
                          KERB_ENCTYPE_RC4_HMAC_MD5 = 0x00000004,
                          KERB_ENCTYPE_AES128_CTS_HMAC_SHA1_96 = 0x00000008,
                          KERB_ENCTYPE_AES256_CTS_HMAC_SHA1_96 = 0x00000010;

        public const string UserRight = "SeInteractiveLogonRight";
        public const string InvalidRight = "Invalid";
        public const string NoPrivilegeWithAccount = "SeTcbPrivilege";

        public const uint InValidHandle = 61;
        public const uint LUIDLowPart = 11;
        public const uint LUIDHighPart = 0;
        public const uint InvalidLUIDLowPart = 40;
        public const uint InvalidLUIDHighPart = 0;
        public const string Privilege1 = "SeAssignPrimaryTokenPrivilege";
        public const string Priv1 = "{0,21}";
        public const string Priv2 = "{0,77}";
        public const uint OpenAccAccess = 4061069327;
        public const string SecurDescriptor = "01-00-00-80-14-00-00-00-00-00-00-00-00-00-00-00-00-00-"
                                              + "00-00-01-02-00-00-00-00-00-05-20-00-00-00-20-02-00-00";

        public const uint QuerySecurityInfo = 1;

        public const int IDENTIFIER_AUTHORITY_VALUES = 6;
        public IntPtr? tempPolicyHandle = IntPtr.Zero;
        public IntPtr? tempSecretHandle = IntPtr.Zero;
        public string strCheckSecretName = string.Empty;
        public _RPC_SID[] objAccountSid = new _RPC_SID[1];
        public _LSAPR_OBJECT_ATTRIBUTES objectAttributes = new _LSAPR_OBJECT_ATTRIBUTES();

        #endregion Constants

        #region VariablesforPolicyObjectMethods

        public bool queryflag;
        public bool querydomainflag;
        private string domainBufferNametocompare = string.Empty;
        private string ReplicaSourceInfotocompare = string.Empty;
        private string ReplicaAccountInfotocompare = string.Empty;
        private string DnsNametocompare = string.Empty;
        private string DnsDomainNametocompare = string.Empty;
        private string ForestNametocompare = string.Empty;
        private string PdNametocompare = string.Empty;
        private string localaccdomname = string.Empty;

        #endregion VariablesforPolicyObjectMethods
       
        #region Methods

        #region PolicyObject

        #region ConversionfromStringtoushort

        /// <summary>
        /// This method is invoked to convert String to Array. 
        /// </summary>
        /// <param name="str">It is the string value which is to be converted</param>
        /// <returns>An array value, which come from a string value</returns>
        public ushort[] ConversionfromStringtoushortArray(string str)
        {
            char[] charArray = str.ToCharArray();
            ushort[] ushortArray = new ushort[charArray.Length];

            for (int i = 0; i < ushortArray.Length; i++)
            {
                ushortArray[i] = (ushort)charArray[i];
            }

            return ushortArray;
        }

        #endregion

        #region ConvertingushortArraytoString

        /// <summary>
        /// This method is invoked to convert Array to String. 
        /// </summary>
        /// <param name="ushrtarr">It is the array value which is to be converted</param>
        /// <returns>A string value, which come from an array value</returns>
        public string ConversionfromushortArraytoString(ushort[] ushrtarr)
        {
            string convertedstring = null;
            char[] charArray1 = new char[ushrtarr.Length];

            for (int i = 0; i < charArray1.Length; i++)
            {
                charArray1[i] = (char)ushrtarr[i];
            }

            StringBuilder s = new StringBuilder();
            s.Append(charArray1);
            convertedstring = s.ToString();

            return convertedstring;
        }

        #endregion        

        #region CreateEfsBlob

        #region CopymainBlobbyte

        /// <summary>
        /// This method is invoked to append an array to another.
        /// </summary>
        /// <param name="mainBlob">Append another array behind it</param>
        /// <param name="index">Actual number of element in mainBlob.</param>
        /// <param name="bytesTobeAppended">Append this array to another.</param>
        public void CopyBytes(byte[] mainBlob, ref int index, byte[] bytesTobeAppended)
        {
            if (mainBlob.Length - index >= bytesTobeAppended.Length)
            {
                for (int i = 0; i < bytesTobeAppended.Length; i++)
                {
                    mainBlob[i + index] = bytesTobeAppended[i];
                }

                index += bytesTobeAppended.Length;
            }
        }

        #endregion

        #region CopyKeyBlobbyte

        /// <summary>
        /// This method is invoked to append an array to another.
        /// </summary>
        /// <param name="keyBlob">Append another array behind it</param>
        /// <param name="index">Actual number of element in keyBlob</param>
        /// <param name="byteofKeytobeAdded">Append this array to another</param>
        public void CopyKeybytes(byte[] keyBlob, ref int index, byte[] byteofKeytobeAdded)
        {
            if (keyBlob.Length - index >= byteofKeytobeAdded.Length)
            {
                for (int i = 0; i < byteofKeytobeAdded.Length; i++)
                {
                    keyBlob[i + index] = byteofKeytobeAdded[i];
                }

                index += byteofKeytobeAdded.Length;
            }
        }

        #endregion

        /// <summary>
        /// This method is invoked to create an EfsBlob object.
        /// </summary>
        /// <returns>Return an EfsBlob object</returns>
        public byte[] CreateEfsblob()
        {
            uint reserved = 0x01000100;
            uint keyCount = LsadManagedAdapter.KeyCount;
            uint Length1 = LsadManagedAdapter.Length1;
            uint Length2 = LsadManagedAdapter.Length2;
            uint SIDOffset = LsadManagedAdapter.SIDOffset;
            uint Reserved1 = 0x02000000;
            uint CertificateLength = LsadManagedAdapter.CertificateLength;
            uint CertificateOffset = LsadManagedAdapter.CertificateOffset;
            ulong Reserved2 = 0x0000000000000000;
            uint Certificate = LsadManagedAdapter.Certificate;
            byte[] keyblob = new byte[(7 * 4) + 8];
            int index1 = 0;
            this.CopyKeybytes(keyblob, ref index1, BitConverter.GetBytes(Length1));
            this.CopyKeybytes(keyblob, ref index1, BitConverter.GetBytes(Length2));
            this.CopyKeybytes(keyblob, ref index1, BitConverter.GetBytes(SIDOffset));
            this.CopyKeybytes(keyblob, ref index1, BitConverter.GetBytes(Reserved1));
            this.CopyKeybytes(keyblob, ref index1, BitConverter.GetBytes(CertificateLength));
            this.CopyKeybytes(keyblob, ref index1, BitConverter.GetBytes(CertificateOffset));
            this.CopyKeybytes(keyblob, ref index1, BitConverter.GetBytes(Reserved2));
            this.CopyKeybytes(keyblob, ref index1, BitConverter.GetBytes(Certificate));
            byte[] mainBlob = new byte[LsadManagedAdapter.InfoLength];
            int index = 0;
            this.CopyBytes(mainBlob, ref index, BitConverter.GetBytes(reserved));
            this.CopyBytes(mainBlob, ref index, BitConverter.GetBytes(keyCount));
            this.CopyBytes(mainBlob, ref index, keyblob);

            return mainBlob;
        }

        #endregion

        #region Functions for validating QueryInformationPolicy/QueryInformationPolicy2

        #region validateAuditLogInfo

        /// <summary>
        /// This method is invoked to get the value of queryflag when the type of policy  
        /// infomation is PolicyAuditLogInformation.
        /// </summary>
        /// <param name="policyinfotoquery">Data that represents the policy being set.</param>
        /// <returns>Value of queryflag</returns>
        public bool ifQueryauditLogInfo(_LSAPR_POLICY_INFORMATION policyinfotoquery)
        {
            if ((policyinfotoquery.PolicyAuditLogInfo.AuditLogFullShutdownInProgress == auditLogshutdown) 
                      && (policyinfotoquery.PolicyAuditLogInfo.AuditLogPercentFull == auditLogPercenrFull) 
                      && (policyinfotoquery.PolicyAuditLogInfo.AuditRetentionPeriod.QuadPart == auditRetention) 
                      && (policyinfotoquery.PolicyAuditLogInfo.MaximumLogSize == maxLogSize) 
                      && (policyinfotoquery.PolicyAuditLogInfo.TimeToShutdown.QuadPart == timetoshutdown))
            {
                this.queryflag = true;
            }
            else
            {
                this.queryflag = false;
            }

            return this.queryflag;
        }

        #endregion validateAuditLogInfo

        #region validateAuditEventsclass

        /// <summary>
        /// This method is invoked to get the value of queryflag when the type of policy  
        /// infomation is PolicyAuditEventsInformation.
        /// </summary>
        /// <param name="policyinfotoquery">Data that represents the policy being set.</param>
        /// <returns>Value of queryflag</returns>
        public bool ifQueryauditeventsInfo(_LSAPR_POLICY_INFORMATION policyinfotoquery)
        {
            if (policyinfotoquery.PolicyAuditEventsInfo.AuditingMode == 
                LsadManagedAdapter.AuditingMode)
            {
                this.queryflag = true;
            }
            else
            {
                this.queryflag = false;
            }

            return this.queryflag;
        }

        #endregion validateAuditEventsclass

        #region validatePolicyPrimaryDomainInformation

        /// <summary>
        /// This method is invoked to get the value of queryflag when the type of policy  
        /// infomation is PolicyPrimaryDomainInformation.
        /// </summary>
        /// <param name="policyinfotoquery">Data that represents the policy being set.</param>
        /// <returns>Value of queryflag</returns>
        public bool ifQueryPrimarydomainInfo(_LSAPR_POLICY_INFORMATION policyinfotoquery)
        {
            this.domainBufferNametocompare =
                this.ConversionfromushortArraytoString(policyinfotoquery.PolicyPrimaryDomainInfo.Name.Buffer);

            _RPC_SID[] sidToCompare = this.GetSid(DomainSid.Valid, LsadManagedAdapter.Instance(TestClassBase.BaseTestSite).PrimaryDomainSID);

            if ((this.domainBufferNametocompare.ToLower() == LsadManagedAdapter.domain.ToLower())
                     && this.CheckTheSids(policyinfotoquery.PolicyPrimaryDomainInfo.Sid[0], sidToCompare[0]))
            {
                this.queryflag = true;
            }
            else
            {
                this.queryflag = false;
            }

            return this.queryflag;
        }

        #endregion validatePolicyPrimaryDomainInformation

        #region validateAccountdomInfo

        /// <summary>
        /// This method is invoked to get the value of queryflag when the type of policy  
        /// infomation is PolicyAccountDomainInformation.
        /// </summary>
        /// <param name="policyinfotoquery">Data that represents the policy being set.</param>
        /// <returns>Value of queryflag</returns>
        public bool ifQueryAccountdomInfo(_LSAPR_POLICY_INFORMATION policyinfotoquery)
        {
            policyinfotoquery.PolicyAccountDomainInfo.DomainName = new _RPC_UNICODE_STRING();
            policyinfotoquery.PolicyAccountDomainInfo.DomainSid = this.GetSid(DomainSid.Valid, LsadManagedAdapter.Instance(TestClassBase.BaseTestSite).PrimaryDomainSID);
            policyinfotoquery.PolicyAccountDomainInfo.DomainName.Buffer =
                this.ConversionfromStringtoushortArray(LsadManagedAdapter.domain);
            this.domainBufferNametocompare =
                this.ConversionfromushortArraytoString(policyinfotoquery.PolicyAccountDomainInfo.DomainName.Buffer);

            if (this.domainBufferNametocompare.ToLower() == LsadManagedAdapter.domain.ToLower())
            {
                this.queryflag = true;
            }
            else
            {
                this.queryflag = false;
            }

            if (!policyinfotoquery.PolicyAccountDomainInfo.DomainSid.Equals(null))
            {
                #region MS-LSAD_R67

                TestClassBase.BaseTestSite.CaptureRequirementIfAreEqual<bool>(
                    this.queryflag,
                    true,
                    "MS-LSAD",
                    67,
                    @"The ""DomainSid"" field of the LSAPR_POLICY_ACCOUNT_DOM_INFO structure MUST NOT be NULL");

                #endregion
            }

            return this.queryflag;
        }

        #endregion validateAccountdomInfo

        #region validateAccoundomainInfoforNONDC

        /// <summary>
        /// This method is invoked to get the value of queryflag when the type of policy  
        /// infomation is PolicyAccountDomainInformation.
        /// </summary>
        /// <param name="policyinfotoquery">Data that represents the policy being set.</param>
        /// <returns>Value of queryflag</returns>
        public bool ifQueryAccountdomInfofornondc(_LSAPR_POLICY_INFORMATION policyinfotoquery)
        {
            policyinfotoquery.PolicyAccountDomainInfo.DomainName = new _RPC_UNICODE_STRING();
            policyinfotoquery.PolicyAccountDomainInfo.DomainSid = this.GetSid(DomainSid.Valid, LsadManagedAdapter.GetAccountDomainSidNonDC);
            policyinfotoquery.PolicyAccountDomainInfo.DomainName.Buffer =
                this.ConversionfromStringtoushortArray(LsadManagedAdapter.domain);
            this.domainBufferNametocompare =
                this.ConversionfromushortArraytoString(policyinfotoquery.PolicyAccountDomainInfo.DomainName.Buffer);

            if (this.domainBufferNametocompare.ToLower() == LsadManagedAdapter.domain.ToLower())
            {
                this.queryflag = true;
            }
            else
            {
                this.queryflag = false;
            }

            return this.queryflag;
        }

        #endregion

        #region validatePolicyLsaServerRoleInformation

        /// <summary>
        /// This method is invoked to get the value of queryflag when the type of policy  
        /// infomation is PolicyLsaServerRoleInformation.
        /// </summary>
        /// <param name="policyinfotoquery">Data that represents the policy being set.</param>
        /// <returns>Value of queryflag</returns>
        public bool ifQueryLsaSrvrroleInfo(_LSAPR_POLICY_INFORMATION policyinfotoquery)
        {
            if ((policyinfotoquery.PolicyServerRoleInfo.LsaServerRole == 
                _POLICY_LSA_SERVER_ROLE.PolicyServerRolePrimary) 
                      || (policyinfotoquery.PolicyServerRoleInfo.LsaServerRole == 
                      _POLICY_LSA_SERVER_ROLE.PolicyServerRoleBackup))
            {
                this.queryflag = true;
            }
            else
            {
                this.queryflag = false;
            }

            return this.queryflag;
        }

        #endregion validatePolicyLsaServerRoleInformation

        #region validatePolicyReplicaSourceInformation

        /// <summary>
        /// This method is invoked to get the value of queryflag when the type of policy  
        /// infomation is PolicyReplicaSourceInformation.
        /// </summary>
        /// <param name="policyinfotoquery">Data that represents the policy being set.</param>
        /// <returns>Value of queryflag</returns>
        public bool ifQueryReplicaSrcInfo(_LSAPR_POLICY_INFORMATION policyinfotoquery)
        {
            policyinfotoquery.PolicyReplicaSourceInfo.ReplicaAccountName = new _RPC_UNICODE_STRING();
            policyinfotoquery.PolicyReplicaSourceInfo.ReplicaSource = new _RPC_UNICODE_STRING();
            policyinfotoquery.PolicyReplicaSourceInfo.ReplicaSource.Buffer =
                this.ConversionfromStringtoushortArray(LsadManagedAdapter.ReplicaAccountName);
            policyinfotoquery.PolicyReplicaSourceInfo.ReplicaAccountName.Buffer = 
                this.ConversionfromStringtoushortArray(LsadManagedAdapter.ReplicasourceName);
            this.ReplicaSourceInfotocompare = 
                this.ConversionfromushortArraytoString(policyinfotoquery.PolicyReplicaSourceInfo.ReplicaSource.Buffer);
            this.ReplicaAccountInfotocompare = 
                this.ConversionfromushortArraytoString(policyinfotoquery.PolicyReplicaSourceInfo.ReplicaAccountName.Buffer);

            if ((LsadManagedAdapter.ReplicaAccountName == this.ReplicaSourceInfotocompare)
                     && (LsadManagedAdapter.ReplicasourceName == this.ReplicaAccountInfotocompare))
            {
                this.queryflag = true;
            }
            else
            {
                this.queryflag = false;
            }

            return this.queryflag;
        }

        #endregion validatePolicyReplicaSourceInformation

        #region validateDnsDomainInfo

        /// <summary>
        /// This method is invoked to get the value of queryflag when the type of policy  
        /// infomation is PolicyDnsDomainInformation.
        /// </summary>
        /// <param name="policyinfotoquery">Data that represents the policy being set.</param>
        /// <returns>Value of queryflag</returns>
        public bool ifQueryDnsDomInfo(_LSAPR_POLICY_INFORMATION policyinfotoquery)
        {
            _RPC_SID[] sidToCompare = new _RPC_SID[1];
            sidToCompare = this.GetSid(DomainSid.Valid, LsadManagedAdapter.Instance(TestClassBase.BaseTestSite).PrimaryDomainSID);
            this.DnsNametocompare =
                this.ConversionfromushortArraytoString(policyinfotoquery.PolicyDnsDomainInfo.Name.Buffer);
            this.DnsDomainNametocompare =
                this.ConversionfromushortArraytoString(policyinfotoquery.PolicyDnsDomainInfo.DnsDomainName.Buffer);
            this.ForestNametocompare =
                this.ConversionfromushortArraytoString(policyinfotoquery.PolicyDnsDomainInfo.DnsForestName.Buffer);
            
            if ((this.DnsNametocompare.ToLower() == LsadManagedAdapter.domain.ToLower())
                     && (this.DnsDomainNametocompare.ToLower() == LsadManagedAdapter.fullDomain.ToLower())
                     && (this.ForestNametocompare.ToLower() == LsadManagedAdapter.fullDomain.ToLower())
                     && (Convert.ToString(policyinfotoquery.PolicyDnsDomainInfo.DomainGuid) == LsadManagedAdapter.DomainGUID)
                     && this.CheckTheSids(policyinfotoquery.PolicyDnsDomainInfo.Sid[0], sidToCompare[0]))
            {
                this.queryflag = true;
            }
            else
            {
                this.queryflag = false;
            }

            return this.queryflag;
        }

        #endregion validateDnsDomainInfo

        #region validateDnsDomainInfoInt

        /// <summary>
        /// This method is invoked to get the value of queryflag when the type of policy  
        /// infomation is PolicyDnsDomainInformationInt.
        /// </summary>
        /// <param name="policyinfotoquery">Data that represents the policy being set.</param>
        /// <returns>Value of queryflag</returns>
        public bool ifQueryDnsDomIntInfo(_LSAPR_POLICY_INFORMATION policyinfotoquery)
        {
            _RPC_SID[] sidToCompare = new _RPC_SID[1];
            sidToCompare = this.GetSid(DomainSid.Valid, LsadManagedAdapter.Instance(TestClassBase.BaseTestSite).PrimaryDomainSID);
            this.DnsNametocompare = this.ConversionfromushortArraytoString(policyinfotoquery.PolicyDnsDomainInfoInt.Name.Buffer);
            this.DnsDomainNametocompare =
                this.ConversionfromushortArraytoString(policyinfotoquery.PolicyDnsDomainInfoInt.DnsDomainName.Buffer);
            this.ForestNametocompare =
                this.ConversionfromushortArraytoString(policyinfotoquery.PolicyDnsDomainInfoInt.DnsForestName.Buffer);

            if ((this.DnsNametocompare.ToLower() == LsadManagedAdapter.domain.ToLower())
                     && (this.DnsDomainNametocompare.ToLower() == LsadManagedAdapter.fullDomain.ToLower())
                     && (this.ForestNametocompare.ToLower() == LsadManagedAdapter.fullDomain.ToLower())
                     && (Convert.ToString(policyinfotoquery.PolicyDnsDomainInfoInt.DomainGuid) == LsadManagedAdapter.DomainGUID)
                     && this.CheckTheSids(policyinfotoquery.PolicyDnsDomainInfoInt.Sid[0], sidToCompare[0]))
            {
                this.queryflag = true;
            }
            else
            {
                this.queryflag = false;
            }

            return this.queryflag;
        }

        #endregion validateDnsDomainInfoInt

        #region validateLocalAccountDomainInfo

        /// <summary>
        /// This method is invoked to get the value of queryflag when the type of policy  
        /// infomation is PolicyLocalAccountDomainInformation.
        /// </summary>
        /// <param name="policyinfotoquery">Data that represents the policy being set.</param>
        /// <returns>Value of queryflag</returns>
        public bool ifQueryLocalaccountInfo(_LSAPR_POLICY_INFORMATION policyinfotoquery)
        {
            _RPC_SID[] sidToCompare = new _RPC_SID[1];
            sidToCompare = this.GetSid(DomainSid.Valid, LsadManagedAdapter.GetLocalAccountDomainSid);
            policyinfotoquery.PolicyLocalAccountDomainInfo.DomainName = new _RPC_UNICODE_STRING();
            policyinfotoquery.PolicyLocalAccountDomainInfo.DomainName.Buffer =
                this.ConversionfromStringtoushortArray(LsadManagedAdapter.domain);
            this.localaccdomname =
                this.ConversionfromushortArraytoString(policyinfotoquery.PolicyLocalAccountDomainInfo.DomainName.Buffer);

            if ((LsadManagedAdapter.domain == this.localaccdomname)
                     || this.CheckTheSids(policyinfotoquery.PolicyLocalAccountDomainInfo.DomainSid[0], sidToCompare[0]))
            {
                this.queryflag = true;
            }
            else
            {
                this.queryflag = false;
            }

            return this.queryflag;
        }

        #endregion validateLocalAccountDomainInfo

        #region validatePdAccountInfo

        /// <summary>
        /// This method is invoked to get the value of queryflag when the type of policy  
        /// infomation is PolicyPdAccountInformation.
        /// </summary>
        /// <param name="policyinfotoquery">Data that represents the policy being set.</param>
        /// <returns>Value of queryflag</returns>
        public bool ifQueryPdaccountInfo(_LSAPR_POLICY_INFORMATION policyinfotoquery)
        {
            policyinfotoquery.PolicyPdAccountInfo.Name = new _RPC_UNICODE_STRING();
            policyinfotoquery.PolicyPdAccountInfo.Name.Buffer = this.ConversionfromStringtoushortArray(PdName);
            policyinfotoquery.PolicyPdAccountInfo.Name.Length =
                (ushort)(2 * policyinfotoquery.PolicyPdAccountInfo.Name.Buffer.Length);
            policyinfotoquery.PolicyPdAccountInfo.Name.MaximumLength =
                (ushort)((policyinfotoquery.PolicyPdAccountInfo.Name.Length)+2);
            this.PdNametocompare = this.ConversionfromushortArraytoString(policyinfotoquery.PolicyPdAccountInfo.Name.Buffer);

            if (this.PdNametocompare == PdName)
            {
                this.queryflag = true;
            }
            else
            {
                this.queryflag = false;
            }

            return this.queryflag;
        }

        #endregion validatePdAccountInfo

        #endregion Functions for validating QueryInformationPolicy/QueryInformationPolicy2

        #region Functions for validating QuerydomainInformationPolicy

        #region validateEfsBlobInfo

        /// <summary>
        /// This method is invoked to get the value of querydomainflag when the type of policy domain   
        /// infomation is PolicyDomainEfsInformation.
        /// </summary>
        /// <param name="querydomaininfo">Data representing policy being set.</param>
        /// <returns>Value of querydomainflag</returns>
        public bool validateEfsBlobInfoquery(_LSAPR_POLICY_DOMAIN_INFORMATION querydomaininfo)
        {
            if (querydomaininfo.PolicyDomainEfsInfo.InfoLength == LsadManagedAdapter.InfoLength)
            {
                this.querydomainflag = true;
            }
            else
            {
                this.querydomainflag = false;
            }

            return this.querydomainflag;
        }

        #endregion validateEfsBlobInfo

        #region validateKerberoseTicketInfo

        /// <summary>
        /// This method is invoked to get the value of querydomainflag when the type of policy domain   
        /// infomation is PolicyDomainKerberosTicketInformation.
        /// </summary>
        /// <param name="querydomaininfo">Data representing policy being set.</param>
        /// <returns>Value of querydomainflag</returns>
        public bool validateKerberoseticketInfoquery(_LSAPR_POLICY_DOMAIN_INFORMATION querydomaininfo)
        {
            if ((querydomaininfo.PolicyDomainKerbTicketInfo.AuthenticationOptions == LsadManagedAdapter.Authenticationoption) 
                      || (querydomaininfo.PolicyDomainKerbTicketInfo.MaxServiceTicketAge.QuadPart == LsadManagedAdapter.Maxservicequadpart) 
                      || (querydomaininfo.PolicyDomainKerbTicketInfo.MaxTicketAge.QuadPart == LsadManagedAdapter.Maxticketquadpart) 
                      || (querydomaininfo.PolicyDomainKerbTicketInfo.MaxRenewAge.QuadPart == LsadManagedAdapter.MaxRenewquadpart) 
                      || (querydomaininfo.PolicyDomainKerbTicketInfo.MaxClockSkew.QuadPart == LsadManagedAdapter.MaxClockskewquad))
            {
                this.querydomainflag = true;
            }
            else
            {
                this.querydomainflag = false;
            }

            return this.querydomainflag;
        }

        #endregion validateKerberoseTicketInfo

        #endregion Functions for validating QuerydomainInformationPolicy

        #endregion PolicyObject

        #region AccountObject

        // General methods used in account objects.
        #region SIDIdentifierAuthority

        /// <summary>
        /// Retrieves SID IdentifierAuthority from ptfconfig file
        /// </summary>
        /// <param name="objAccountSidForIdentify">A SID of the account</param>
        /// <returns>A SID of the account with IdentifierAuthority value not null</returns>
        public _RPC_SID[] SIDIdentifierAuthority(ref _RPC_SID[] objAccountSidForIdentify)
        {
            objAccountSidForIdentify[0].IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
            objAccountSidForIdentify[0].IdentifierAuthority.Value = new byte[6];

            for (int index = 0; index < LsadManagedAdapter.SIDCount; index++)
            {
                objAccountSidForIdentify[0].IdentifierAuthority.Value[index] = 0x00;
            }

            objAccountSidForIdentify[0].IdentifierAuthority.Value[5] = 0x01;

            return objAccountSidForIdentify;
        }

        #endregion SIDIdentifierAuthority

        #region SID

        /// <summary>
        /// Retrieves SID values from ptfconfig file
        /// </summary>
        /// <param name="Sid">A SID of the account</param>
        /// <returns>SID of the account</returns>
        public _RPC_SID[] SID(TypeOfSID Sid)
        {
            string sidType = string.Empty;

            ////SID SubAuthorityCount value.
            this.objAccountSid[0].SubAuthorityCount = LsadManagedAdapter.SIDCount;

            ////_RPC_SID.SubAuthority array size.
            this.objAccountSid[0].SubAuthority = new uint[LsadManagedAdapter.SID];

            if (Sid == TypeOfSID.NewSID)
            {
                sidType = LsadManagedAdapter.NewSID;
            }
            else if (Sid == TypeOfSID.UnKnownSID)
            {
                sidType = LsadManagedAdapter.UnKnownSID;
            }
            else if (Sid == TypeOfSID.ExistSID)
            {
                sidType = LsadManagedAdapter.ExistSID;
            }

            string strSid = sidType;
            uint[] intSID = new uint[(strSid.Length / 3) + 1];
            int getSID = 0;

            for (int SIDIndex = 0; SIDIndex < strSid.Length; SIDIndex++)
            {
                intSID[getSID] = Convert.ToByte(strSid.Substring(SIDIndex, 2));
                getSID++;
                SIDIndex = SIDIndex + 2;
            }

            this.objAccountSid[0].SubAuthority = intSID;

            return this.objAccountSid;
        }

        #endregion SID

        #region AddPrivilege

        /// <summary>
        /// AddPrivileges to Account
        /// </summary>
        /// <param name="Priv">Type of Privilege</param>
        /// <returns>A set of privileges that belong to an account.</returns>        
        public _LSAPR_PRIVILEGE_SET? AddPrivilege(PrivilegeType Priv)
        {
            _LSAPR_PRIVILEGE_SET privilegeSet = new _LSAPR_PRIVILEGE_SET();
            privilegeSet.PrivilegeCount = 1;
            privilegeSet.Control = 0x00000001;
            privilegeSet.Privilege = new _LSAPR_LUID_AND_ATTRIBUTES[1];
            privilegeSet.Privilege[0].Luid = new _LUID();

            if (Priv == PrivilegeType.Valid)
            {
                privilegeSet.Privilege[0].Luid.LowPart = Convert.ToByte(LUIDLowPart);
                privilegeSet.Privilege[0].Luid.HighPart = Convert.ToByte(LUIDHighPart);
                privilegeSet.Privilege[0].Attributes = 0x00000001;
            }
            else
            {
                privilegeSet.Privilege[0].Luid.LowPart = Convert.ToByte(InvalidLUIDLowPart);
                privilegeSet.Privilege[0].Luid.HighPart = Convert.ToByte(InvalidLUIDHighPart);
                privilegeSet.Privilege[0].Attributes = 0x00000000;
            }

            return privilegeSet;
        }

        #endregion AddPrivilege

        #region SecurityDescriptor

        /// <summary>
        /// Passing SecurityDescriptor
        /// </summary>
        /// <returns>SecurityDescriptor value</returns>
        public byte[] SecurityDescriptor()
        {
            string strSecrDescriptor = SecurDescriptor;
            byte[] byteSecrDescriptor = new byte[(strSecrDescriptor.Length / 3) + 1];
            int getSecDesc = 0;

            for (int SecDescIndex = 0; SecDescIndex < strSecrDescriptor.Length; SecDescIndex++)
            {
                byteSecrDescriptor[getSecDesc] = Convert.ToByte(strSecrDescriptor.Substring(SecDescIndex, 2), 16);
                getSecDesc++;
                SecDescIndex = SecDescIndex + 2;
            }

            return byteSecrDescriptor;
        }

        #endregion SecurityDescriptor

        #region AccountObject InvalidHandle

        /// <summary>        
        /// Passing Secret handle to Account objects.      
        /// </summary>
        /// <returns>An account invalid handle</returns>
        public IntPtr? AccountObjInvalidHandle()
        {
            IntPtr? objInvalidHandle = IntPtr.Zero;
            _RPC_UNICODE_STRING[] secretName = new _RPC_UNICODE_STRING[1];
            ACCESS_MASK uintAccessMaskInv = ACCESS_MASK.NONE;
            _LSAPR_OBJECT_ATTRIBUTES[] objectAttributesHandle = new _LSAPR_OBJECT_ATTRIBUTES[1];
            System.IntPtr? objPolicyHandle = IntPtr.Zero;

            secretName[0].Buffer = new ushort[] { Convert.ToByte(InValidHandle) };
            secretName[0].Length = (ushort)(2 * secretName[0].Buffer.Length);
            secretName[0].MaximumLength = (ushort)(secretName[0].Length + 2);
            uintAccessMaskInv = ACCESS_MASK.POLICY_CREATE_SECRET;
            LsadManagedAdapter.lsadClientStack.LsarOpenPolicy2(
                "sdfd",
                objectAttributesHandle[0],
                uintAccessMaskInv,
                out objPolicyHandle);

            uintAccessMaskInv = ACCESS_MASK.MAXIMUM_ALLOWED;
            LsadManagedAdapter.lsadClientStack.LsarOpenSecret(
                objPolicyHandle.Value,
                secretName[0],
                uintAccessMaskInv,
                out objInvalidHandle);

            if (objInvalidHandle == IntPtr.Zero)
            {
                LsadManagedAdapter.lsadClientStack.LsarCreateSecret(
                    objPolicyHandle.Value,
                    secretName[0],
                    uintAccessMaskInv,
                    out objInvalidHandle);
            }

            LsadManagedAdapter.lsadClientStack.LsarClose(ref objPolicyHandle);

            return objInvalidHandle;
        }

        #endregion AccountObject InvalidHandle

        #region AccountHandlecheck

        /// <summary>
        /// Checking whether AccountHandle is Exists or not
        /// </summary>
        /// <param name="objAccountSidForCheck">SID of the account</param>
        /// <returns>Return 0 if AccountHandle is exist
        /// Return others if AccountHandle is not exist</returns>
        public uint AccountHandleCheck(ref _RPC_SID[] objAccountSidForCheck)
        {
            System.IntPtr? objPolicyHandle = IntPtr.Zero;
            System.IntPtr? objAccountHandle = IntPtr.Zero;
            _LSAPR_OBJECT_ATTRIBUTES objectAttributesForCheck = new _LSAPR_OBJECT_ATTRIBUTES();
            this.SIDIdentifierAuthority(ref objAccountSidForCheck);
            ACCESS_MASK uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
            objAccountSidForCheck[0].Revision = 0x01;
            ushort[] systemName = this.ConversionfromStringtoushortArray(string.Empty);
            NtStatus uintMethod = LsadManagedAdapter.lsadClientStack.LsarOpenPolicy(
                systemName,
                objectAttributesForCheck,
                uintDesrAccess,
                out objPolicyHandle);

            uintMethod = LsadManagedAdapter.lsadClientStack.LsarOpenAccount(
                objPolicyHandle.Value,
                objAccountSidForCheck[0],
                uintDesrAccess,
                out objAccountHandle);

            if (uintMethod == 0)
            {
                LsadManagedAdapter.lsadClientStack.LsarDeleteObject(ref objAccountHandle);
            }

            return (uint)uintMethod;
        }

        #endregion AccountHandleCheck

        #region CreateExistAccount

        /// <summary>
        /// Creating Existing Account Handle
        /// </summary>
        /// <param name="uintOpenAccAccess">A bitmask specifying accesses to be
        /// granted to the newly created and opened account at this time</param>
        /// <returns>Value of uintOpenAccAccess after LsarCreateAccount()</returns>
        public ACCESS_MASK CreateExistAccount(ACCESS_MASK uintOpenAccAccess)
        {
            System.IntPtr? objPolicyHandle = IntPtr.Zero;
            System.IntPtr? objAccountHandle = IntPtr.Zero;
            _LSAPR_OBJECT_ATTRIBUTES[] objectAttributesCheck = new _LSAPR_OBJECT_ATTRIBUTES[1];
            _RPC_SID[] objAccountSidCheck = new _RPC_SID[1];

            ////SID SubAuthorityCount value.
            objAccountSidCheck[0].SubAuthorityCount = LsadManagedAdapter.SIDCount;
            ////_RPC_SID.SubAuthority array size.
            objAccountSidCheck[0].SubAuthority = new uint[LsadManagedAdapter.SID];

            objAccountSidCheck = this.SID(TypeOfSID.ExistSID);
            this.SIDIdentifierAuthority(ref objAccountSidCheck);
            ACCESS_MASK uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
            objAccountSidCheck[0].Revision = 0x01;
            NtStatus Handle = LsadManagedAdapter.lsadClientStack.LsarOpenPolicy(
                this.ConversionfromStringtoushortArray(string.Empty),
                objectAttributesCheck[0],
                uintDesrAccess,
                out objPolicyHandle);

            Handle = LsadManagedAdapter.lsadClientStack.LsarOpenAccount(
                objPolicyHandle.Value,
                objAccountSidCheck[0],
                uintDesrAccess,
                out objAccountHandle);

            if (Handle == (NtStatus)0xC0000034)
            {
                LsadManagedAdapter.lsadClientStack.LsarCreateAccount(
                    objPolicyHandle.Value,
                    objAccountSidCheck[0],
                    uintOpenAccAccess,
                    out objAccountHandle);
            }

            objAccountSidCheck = this.SID(TypeOfSID.UnKnownSID);
            this.SIDIdentifierAuthority(ref objAccountSidCheck);
            objAccountSidCheck[0].Revision = 0x01;
            Handle = LsadManagedAdapter.lsadClientStack.LsarOpenAccount(
                objPolicyHandle.Value,
                objAccountSidCheck[0],
                uintDesrAccess,
                out objAccountHandle);

            if (Handle == 0)
            {
                Handle = LsadManagedAdapter.lsadClientStack.LsarDeleteObject(ref objAccountHandle);
            }

            return uintOpenAccAccess;
        }

        #endregion CreateExistAccount

        #region DeleteExistAccount

        /// <summary>
        /// Deleting Existing Account Handle
        /// </summary>
        /// <returns>Return 0x00000000 if the method is successful
        /// Return 0xC0000022 if the caller does not have 
        /// the permissions to perform this operation.
        /// Return 0xC000000D if some of the parameters supplied 
        /// were invalid.
        /// Return 0xC0000034 if an account with this SID does not
        /// exist in the server's database.
        /// Return 0xC0000008 if PolicyHandle is not a valid handle </returns>
        public uint DeleteExistAccount()
        {
            System.IntPtr? objPolicyHandle = IntPtr.Zero;
            System.IntPtr? objAccountHandle = IntPtr.Zero;

            _LSAPR_OBJECT_ATTRIBUTES[] objectAttributesForDel = new _LSAPR_OBJECT_ATTRIBUTES[1];
            _RPC_SID[] objAccountSidForDel = new _RPC_SID[1];

            ////SID SubAuthorityCount value.
            objAccountSidForDel[0].SubAuthorityCount = LsadManagedAdapter.SIDCount;

            ////_RPC_SID.SubAuthority array size.
            objAccountSidForDel[0].SubAuthority = new uint[LsadManagedAdapter.SID];

            objAccountSidForDel = this.SID(TypeOfSID.NewSID);
            this.SIDIdentifierAuthority(ref objAccountSidForDel);
            ACCESS_MASK uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
            objAccountSidForDel[0].Revision = 0x01;
            NtStatus Handle = LsadManagedAdapter.lsadClientStack.LsarOpenPolicy(
                this.ConversionfromStringtoushortArray(string.Empty),
                objectAttributesForDel[0],
                uintDesrAccess,
                out objPolicyHandle);

            Handle = LsadManagedAdapter.lsadClientStack.LsarOpenAccount(
                objPolicyHandle.Value,
                objAccountSidForDel[0],
                uintDesrAccess,
                out objAccountHandle);

            if (Handle == 0)
            {
                Handle = LsadManagedAdapter.lsadClientStack.LsarDeleteObject(ref objAccountHandle);
            }

            return (uint)Handle;
        }

        #endregion DeleteExistAccount

        #region DeleteUnknownSID

        /// <summary>
        /// Deleting UnknownSID Account Handle
        /// </summary>
        /// <returns>Return 0x00000000 if the method is successful
        /// Return 0xC0000022 if the caller does not have 
        /// the permissions to perform this operation.
        /// Return 0xC000000D if some of the parameters supplied 
        /// were invalid.
        /// Return 0xC0000034 if an account with this SID does not
        /// exist in the server's database.
        /// Return 0xC0000008 if PolicyHandle is not a valid handle </returns>
        public uint DeleteUnknownSID()
        {
            System.IntPtr? objPolicyHandle = IntPtr.Zero;
            System.IntPtr? objAccountHandle = IntPtr.Zero;

            _LSAPR_OBJECT_ATTRIBUTES[] objectAttributesForDel = new _LSAPR_OBJECT_ATTRIBUTES[1];
            _RPC_SID[] objAccountSidForDel = new _RPC_SID[1];

            ////SID SubAuthorityCount value.
            objAccountSidForDel[0].SubAuthorityCount = LsadManagedAdapter.SIDCount;

            ////_RPC_SID.SubAuthority array size.
            objAccountSidForDel[0].SubAuthority = new uint[LsadManagedAdapter.SID];

            objAccountSidForDel = this.SID(TypeOfSID.UnKnownSID);
            this.SIDIdentifierAuthority(ref objAccountSidForDel);
            ACCESS_MASK uintDesrAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
            objAccountSidForDel[0].Revision = 0x01;
            ushort[] systemName = this.ConversionfromStringtoushortArray(string.Empty);
            NtStatus Handle = LsadManagedAdapter.lsadClientStack.LsarOpenPolicy(
                systemName,
                objectAttributesForDel[0],
                uintDesrAccess,
                out objPolicyHandle);

            Handle = LsadManagedAdapter.lsadClientStack.LsarOpenAccount(
                objPolicyHandle.Value,
                objAccountSidForDel[0],
                uintDesrAccess,
                out objAccountHandle);

            if (Handle == 0)
            {
                Handle = LsadManagedAdapter.lsadClientStack.LsarDeleteObject(ref objAccountHandle);
            }

            return (uint)Handle;
        }

        #endregion DeleteExistAccount

        #region AccountUserRight

        /// <summary>
        /// Construct UserRight in AccountObject .
        /// </summary>
        /// <param name="UserRightCheck">the array of user rights</param>
        /// <param name="addUserRight">the array of user right sets</param>
        /// <param name="count">the number of user rights. It should be equal to the number of the user rights</param>
        public void AddAccountright(string[] UserRightCheck, ref _LSAPR_USER_RIGHT_SET[] addUserRight, int count)
        {
            addUserRight[0].UserRights = new _RPC_UNICODE_STRING[count];
            string[] arrUserRight = new string[count];

            for (int i = 0; i < count; i++)
            {
                if (UserRightCheck[i] == Convert.ToString(TypeOfUserRight.ValidUserRight))
                {
                    arrUserRight[i] = UserRight;
                }
                else if (UserRightCheck[i] == Convert.ToString(TypeOfUserRight.InvalidUserRight))
                {
                    arrUserRight[i] = InvalidRight;
                }
                else
                {
                    arrUserRight[i] = UserRightCheck[i];
                }

                char[] nameArray = new char[arrUserRight[i].Length];
                nameArray = arrUserRight[i].ToCharArray();

                addUserRight[0].UserRights[i].Buffer = new ushort[nameArray.Length];
                Array.Copy(nameArray, addUserRight[0].UserRights[i].Buffer, nameArray.Length);
                addUserRight[0].UserRights[i].Length = (ushort)(2 * addUserRight[0].UserRights[i].Buffer.Length);
                addUserRight[0].UserRights[i].MaximumLength =
                    (ushort)((2 * addUserRight[0].UserRights[i].Buffer.Length) + 2);
            }
        }

        #endregion AccountUserRight

        #region EnumerateAccountsWithUserRight

        /// <summary>
        /// Passing User right from PTFConfig equal to the passed-in value.
        /// </summary>
        /// <param name="UserRightCheck">Type of user right</param>
        /// <param name="addUserRight">A set which storage account user right</param>
        /// <returns>A set which storage account user right with a new one</returns>
        public _RPC_UNICODE_STRING[] AccountsWithUserRight(
            string UserRightCheck,
            ref _RPC_UNICODE_STRING[] addUserRight)
        {
            int intCount = 0;
            string[] arrUserRight = new string[10];

            if (UserRightCheck == Convert.ToString(TypeOfUserRight.ValidUserRight))
            {
                arrUserRight[0] = UserRight;
            }
            else if (UserRightCheck == Convert.ToString(TypeOfUserRight.InvalidUserRight))
            {
                arrUserRight[0] = InvalidRight;
            }
            else if (UserRightCheck == Convert.ToString(TypeOfUserRight.NoPrivilegeWithAccount))
            {
                arrUserRight[0] = NoPrivilegeWithAccount;
            }

            for (int index = 0; index <= intCount; index++)
            {
                char[] nameArray = new char[arrUserRight[index].Length];
                nameArray = arrUserRight[index].ToCharArray();
                addUserRight[index].Buffer = new ushort[nameArray.Length];
                Array.Copy(nameArray, addUserRight[index].Buffer, nameArray.Length);
                addUserRight[index].Length = (ushort)(2 * addUserRight[0].Buffer.Length);
                addUserRight[index].MaximumLength = (ushort)((2 * addUserRight[0].Buffer.Length) + 2);
            }

            return addUserRight;
        }

        #endregion EnumerateAccountsWithUserRight

        #region InvalidParamUserRight

        /// <summary>
        /// Passing InvalidParameter to Account User right.
        /// </summary>
        /// <returns>A set which storage account user right with a new one</returns>
        public _RPC_UNICODE_STRING[] InvalidParamUserRight()
        {
            _RPC_UNICODE_STRING[] objUserRight = new _RPC_UNICODE_STRING[1];
            this.AccountsWithUserRight(Convert.ToString(TypeOfUserRight.ValidUserRight), ref objUserRight);
            objUserRight[0].Length = (ushort)(2 * objUserRight[0].Buffer.Length);
            objUserRight[0].MaximumLength = (ushort)(objUserRight[0].Length + 2);
            objUserRight[0].Length = (ushort)objUserRight[0].Buffer.Length;

            return objUserRight;
        }

        #endregion InvalidParamUserRight

        #region win2k8AccHandle

        /// <summary>
        /// Passing InvalidHandle to win2k8Account Privilege.
        /// </summary>
        /// <returns>A handle to the account object</returns>
        public IntPtr? win2k8AccHandle()
        {
            IntPtr? objPolicyHandle = IntPtr.Zero;
            IntPtr? objAccountHandle = IntPtr.Zero;
            ACCESS_MASK accountAcess = ACCESS_MASK.MAXIMUM_ALLOWED;
            this.objAccountSid[0].Revision = 0x1;
            LsadManagedAdapter.lsadClientStack.LsarOpenPolicy(
                this.ConversionfromStringtoushortArray(string.Empty),
                this.objectAttributes,
                accountAcess,
                out objPolicyHandle);

            LsadManagedAdapter.lsadClientStack.LsarCreateAccount(
                objPolicyHandle.Value,
                this.objAccountSid[0],
                accountAcess,
                out objAccountHandle);

            LsadManagedAdapter.lsadClientStack.LsarOpenAccount(
                objPolicyHandle.Value,
                this.objAccountSid[0],
                accountAcess,
                out objAccountHandle);

            return objAccountHandle;
        }

        #endregion win2k8AccHandle

        #endregion AccountObject

        #region SecretObject

        // General methods used in Secret objects.
        #region nameOfSecretObject

        /// <summary>
        /// Retrieves secret name from ptfconfig file.
        /// </summary>
        /// <param name="checkNameExists">Result of check name</param>
        /// <param name="SecretName">A set which storage secret name.</param>
        /// <returns>Return 0 means retrieves secret name from ptfconfig file success.</returns>
        public int nameOfSecretObject(ResOfNameChecked checkNameExists, ref _RPC_UNICODE_STRING[] SecretName)
        {
            int intCount = 0;
            string[] arrNameString = new string[10];
            string[] arrNamesArray = new string[10];

            switch (checkNameExists)
            {
                case ResOfNameChecked.Valid:
                    arrNamesArray[0] = LsadManagedAdapter.ValidName;
                    break;
                case ResOfNameChecked.InValid:
                    arrNamesArray[0] = LsadManagedAdapter.InValidName;
                    break;
                // Not used
                //case ResOfNameChecked.Already:
                //    arrNamesArray[0] = TestClassBase.BaseTestSite.Properties.Get("presentValidSecretName");
                //    break;
                case ResOfNameChecked.TooLong:
                    arrNamesArray[0] = LsadManagedAdapter.TooLongName;
                    break;
                case ResOfNameChecked.NotPresent:
                    arrNamesArray[0] = LsadManagedAdapter.NotPresentName;
                    break;
                case ResOfNameChecked.LocalSystem:
                    arrNamesArray[0] = LsadManagedAdapter.local_system;
                    break;
                case ResOfNameChecked.GlobalSecretName:
                    arrNamesArray[0] = LsadManagedAdapter.GlobalSecretName;
                    break;
            }

            this.strCheckSecretName = arrNamesArray[0];

            for (int index = 0; index <= intCount; index++)
            {
                arrNameString[index] = arrNamesArray[index];
                char[] nameArray = new char[arrNameString[index].Length];
                nameArray = arrNameString[index].ToCharArray();
                SecretName[index].Buffer = new ushort[nameArray.Length];
                Array.Copy(nameArray, SecretName[index].Buffer, nameArray.Length);
                SecretName[index].Length = (ushort)(2 * SecretName[index].Buffer.Length);
                SecretName[index].MaximumLength = (ushort)(SecretName[index].Length + 2);
            }

            return 0;
        }

        #endregion nameOfSecretObject

        #region inValidHandle

        /// <summary>
        /// Passing Account handle to secret object.
        /// </summary>
        /// <returns>Return 0 means pass account handle to secret object success.</returns>
        public int inValidHandle()
            {
            _LSAPR_OBJECT_ATTRIBUTES[] objectAttributesForCheck = new _LSAPR_OBJECT_ATTRIBUTES[1];
            _RPC_SID[] Accountsid = new _RPC_SID[1];

            Accountsid[0].SubAuthorityCount = LsadManagedAdapter.SIDCount;
            Accountsid[0].SubAuthority = new uint[LsadManagedAdapter.SID];
            Accountsid[0].SubAuthority[0] = Convert.ToByte(TestClassBase.BaseTestSite.Properties.Get("NewSID0"));
            Accountsid[0].SubAuthority[1] = Convert.ToByte(TestClassBase.BaseTestSite.Properties.Get("NewSID1"));
            Accountsid[0].SubAuthority[2] = Convert.ToByte(TestClassBase.BaseTestSite.Properties.Get("NewSID2"));
            Accountsid[0].SubAuthority[3] = Convert.ToByte(TestClassBase.BaseTestSite.Properties.Get("NewSID3"));

            Accountsid[0].IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
            Accountsid[0].IdentifierAuthority.Value = new byte[6];
            Accountsid[0].IdentifierAuthority.Value[0] = (byte)Value_Values.NULL_SID_AUTHORITY;
            Accountsid[0].IdentifierAuthority.Value[1] = (byte)Value_Values.NULL_SID_AUTHORITY;
            Accountsid[0].IdentifierAuthority.Value[2] = (byte)Value_Values.NULL_SID_AUTHORITY;
            Accountsid[0].IdentifierAuthority.Value[3] = (byte)Value_Values.NULL_SID_AUTHORITY;
            Accountsid[0].IdentifierAuthority.Value[4] = (byte)Value_Values.NULL_SID_AUTHORITY;
            Accountsid[0].IdentifierAuthority.Value[5] = (byte)Value_Values.WORLD_SID_AUTHORITY;

            ACCESS_MASK uintDesiredAccess, uintTempAccessMask;
            IntPtr? PolicyHandle2 = IntPtr.Zero;
            IntPtr? AccountHandle = IntPtr.Zero;

            uintDesiredAccess = ACCESS_MASK.MAXIMUM_ALLOWED;
            uintTempAccessMask = ACCESS_MASK.MAXIMUM_ALLOWED;

            objectAttributesForCheck[0].RootDirectory = null;
            Accountsid[0].Revision = 1;

            this.tempPolicyHandle = LsadManagedAdapter.PolicyHandle;
            this.tempSecretHandle = LsadManagedAdapter.objSecretHandle;
            LsadManagedAdapter.lsadClientStack.LsarOpenPolicy2(
                "string",
                objectAttributesForCheck[0],
                uintDesiredAccess,
                out PolicyHandle2);

            LsadManagedAdapter.lsadClientStack.LsarCreateAccount(
                PolicyHandle2.Value,
                Accountsid[0],
                uintTempAccessMask,
                out AccountHandle);

            if (AccountHandle == IntPtr.Zero)
            {
                LsadManagedAdapter.lsadClientStack.LsarOpenAccount(
                    PolicyHandle2.Value,
                    Accountsid[0],
                    uintTempAccessMask,
                    out AccountHandle);
            }

            LsadManagedAdapter.PolicyHandle = AccountHandle;
            LsadManagedAdapter.objSecretHandle = AccountHandle;
            LsadManagedAdapter.lsadClientStack.LsarClose(ref PolicyHandle2);

            return 0;
        }

        #endregion inValidHandle

        #region nameOfPrivilege

        /// <summary>
        /// Retrieves privilege name from ptfconfig file.
        /// </summary>
        /// <param name="checkPrivilegeExists">Result of check name</param>
        /// <param name="inputPrivilegeName">A set which storage privilege name</param>
        /// <returns>Return 0 means retrieves privilege name from ptfconfig file success.</returns>
        public int nameOfPrivilege(PrivilegeType checkPrivilegeExists, ref _RPC_UNICODE_STRING inputPrivilegeName)
        {
            string[] arrNameString = new string[10];
            string[] arrNamesArray = new string[10];
            int intCount = 0;

            switch (checkPrivilegeExists)
            {
                case PrivilegeType.Valid:
                    arrNamesArray[0] = LsadManagedAdapter.ValidPrivilegeName;
                    break;
                case PrivilegeType.InValid:
                    arrNamesArray[0] = LsadManagedAdapter.InValidPrivilegeName;
                    break;
                case PrivilegeType.NoSuchPrivilege:
                    arrNamesArray[0] = LsadManagedAdapter.NoSuchPrivilegeName;
                    break;
            }

            for (int index = 0; index <= intCount; index++)
            {
                arrNameString[index] = arrNamesArray[index];

                char[] nameArray = new char[arrNameString[index].Length];
                nameArray = arrNameString[index].ToCharArray();

                inputPrivilegeName.Buffer = new ushort[nameArray.Length];
                Array.Copy(nameArray, inputPrivilegeName.Buffer, nameArray.Length);
                inputPrivilegeName.Length = (ushort)(2 * inputPrivilegeName.Buffer.Length);
                inputPrivilegeName.MaximumLength = (ushort)(inputPrivilegeName.Length + 2);
            }

            return 0;
        }

        #endregion nameOfPrivilege

        #region valuesOfLUID

        /// <summary>
        /// Retrieves luid value from ptfconfig file.
        /// </summary>
        /// <param name="checkLUIDExists">Validating the passed in luid in action LookUpPrivilegeName</param>
        /// <param name="inputPrivilegeLUID">A set which storage LUID value.</param>
        /// <returns>Return 0 means Retrieves luid value from ptfconfig file successfully</returns>
        public int valuesOfLUID(PrivilegeLUID checkLUIDExists, ref _LUID[] inputPrivilegeLUID)
        {
            switch (checkLUIDExists)
            {
                case PrivilegeLUID.Valid:
                    inputPrivilegeLUID[0].HighPart = LsadManagedAdapter.ValidInputPrivilegeLUIDHighPart;

                    inputPrivilegeLUID[0].LowPart = LsadManagedAdapter.ValidInputPrivilegeLUIDLowPart;
                    break;
                case PrivilegeLUID.Invalid:
                    inputPrivilegeLUID[0].HighPart = 0;
                    inputPrivilegeLUID[0].LowPart = 37;
                    break;
                case PrivilegeLUID.NotPresentLuid:
                    inputPrivilegeLUID[0].HighPart = 0;
                    inputPrivilegeLUID[0].LowPart = 45;
                    break;
            }

            return 0;
        }

        #endregion valuesOfLUID

        #endregion SecretObject

        #region PrivilegeInformation

        /// <summary>
        /// Retrieves luid value from ptfconfig file.
        /// </summary>
        /// <param name="htPrivilegeNameNLUID">Add information to this parameter</param>
        /// <returns>Return 0 means retrieve LUID value from ptfconfig file success.</returns>
        public int privilegeInformation(ref Hashtable htPrivilegeNameNLUID)
        {
            htPrivilegeNameNLUID.Add("{0,3}", "SeAssignPrimaryTokenPrivilege");
            htPrivilegeNameNLUID.Add("{0,21}", "SeAuditPrivilege");
            htPrivilegeNameNLUID.Add("{0,17}", "SeBackupPrivilege");
            htPrivilegeNameNLUID.Add("{0,23}", "SeChangeNotifyPrivilege");
            htPrivilegeNameNLUID.Add("{0,30}", "SeCreateGlobalPrivilege");
            htPrivilegeNameNLUID.Add("{0,15}", "SeCreatePagefilePrivilege");
            htPrivilegeNameNLUID.Add("{0,16}", "SeCreatePermanentPrivilege");
            htPrivilegeNameNLUID.Add("{0,2}", "SeCreateTokenPrivilege");
            htPrivilegeNameNLUID.Add("{0,20}", "SeDebugPrivilege");
            htPrivilegeNameNLUID.Add("{0,27}", "SeEnableDelegationPrivilege");
            htPrivilegeNameNLUID.Add("{0,29}", "SeImpersonatePrivilege");
            htPrivilegeNameNLUID.Add("{0,14}", "SeIncreaseBasePriorityPrivilege");
            htPrivilegeNameNLUID.Add("{0,5}", "SeIncreaseQuotaPrivilege");
            htPrivilegeNameNLUID.Add("{0,10}", "SeLoadDriverPrivilege");
            htPrivilegeNameNLUID.Add("{0,4}", "SeLockMemoryPrivilege");
            htPrivilegeNameNLUID.Add("{0,6}", "SeMachineAccountPrivilege");
            htPrivilegeNameNLUID.Add("{0,28}", "SeManageVolumePrivilege");
            htPrivilegeNameNLUID.Add("{0,13}", "SeProfileSingleProcessPrivilege");
            htPrivilegeNameNLUID.Add("{0,24}", "SeRemoteShutdownPrivilege");
            htPrivilegeNameNLUID.Add("{0,18}", "SeRestorePrivilege");
            htPrivilegeNameNLUID.Add("{0,8}", "SeSecurityPrivilege");
            htPrivilegeNameNLUID.Add("{0,19}", "SeShutdownPrivilege");
            htPrivilegeNameNLUID.Add("{0,26}", "SeSyncAgentPrivilege");
            htPrivilegeNameNLUID.Add("{0,22}", "SeSystemEnvironment");
            htPrivilegeNameNLUID.Add("{0,11}", "SeSystemProfilePrivilege");
            htPrivilegeNameNLUID.Add("{0,12}", "SeSystemtimePrivilege");
            htPrivilegeNameNLUID.Add("{0,9}", "SeTakeOwnershipPrivilege");
            htPrivilegeNameNLUID.Add("{0,7}", "SeTcbPrivilege");
            htPrivilegeNameNLUID.Add("{0,25}", "SeUndockPrivilege");
            htPrivilegeNameNLUID.Add("{0,35}", "SeCreateSymbolicLinkPrivilege");
            htPrivilegeNameNLUID.Add("{0,33}", "SeIncreaseWorkingSetPrivilege");
            htPrivilegeNameNLUID.Add("{0,32}", "SeRelabelPrivilege");
            htPrivilegeNameNLUID.Add("{0,34}", "SeTimeZonePrivilege");
            htPrivilegeNameNLUID.Add("{0,31}", "SeTrustedCredManAccessPrivilege");

            return 0;
        }

        #endregion PrivilegeInformation

        #region TrustObjects

        #region InvalidHandleCreationAndDeletion

        /// <summary>        
        /// Passing Secret handle to trusted objects.      
        /// </summary>
        /// <param name="deleteCheck">This value check whether delete secret handle</param>
        /// <returns>A new invalid handle</returns>
        public IntPtr? CreateAnInvalidHandle(bool deleteCheck)
        {
            IntPtr? HandleForSecretobject = IntPtr.Zero;
            IntPtr? secretHandle = IntPtr.Zero;
            _RPC_UNICODE_STRING scrName = new _RPC_UNICODE_STRING();
            _LSAPR_OBJECT_ATTRIBUTES objectAttributesHandle = new _LSAPR_OBJECT_ATTRIBUTES();

            NtStatus uintMethodStatus = 0;

            ACCESS_MASK uintAccessMaskInv = ACCESS_MASK.MAXIMUM_ALLOWED;
            scrName.Buffer = new ushort[] { 'I', 'N', 'V', 'A', 'L', 'I', 'D', 'S', 'C', 'R', 'H', 'D', 'L' };
            scrName.Length = (ushort)(2 * scrName.Buffer.Length);
            scrName.MaximumLength = (ushort)(scrName.Length + 2);
            
            uintMethodStatus = LsadManagedAdapter.lsadClientStack.LsarOpenPolicy2(
                "sdfd",
                objectAttributesHandle,
                uintAccessMaskInv,
                out HandleForSecretobject);

            uintAccessMaskInv = ACCESS_MASK.MAXIMUM_ALLOWED;
            uintMethodStatus = LsadManagedAdapter.lsadClientStack.LsarOpenSecret(
                HandleForSecretobject.Value,
                scrName,
                uintAccessMaskInv,
                out secretHandle);

            if ((uint)uintMethodStatus != (uint)ErrorStatus.Success)
            {
                uintMethodStatus = LsadManagedAdapter.lsadClientStack.LsarCreateSecret(
                    HandleForSecretobject.Value,
                    scrName,
                    uintAccessMaskInv,
                    out secretHandle);
            }

            if (deleteCheck)
            {
                uintMethodStatus = LsadManagedAdapter.lsadClientStack.LsarDeleteObject(ref secretHandle);
            }

            uintMethodStatus = LsadManagedAdapter.lsadClientStack.LsarClose(ref HandleForSecretobject);

            return secretHandle;
        }

        #endregion

        #region GetSid

        /// <summary>
        /// Initializes the SID of the domain.
        /// </summary>
        /// <param name="sid">Result of validating in domain accountSid</param>
        /// <param name="sidString">Entry name is ptfconfig file</param>
        /// <returns>SID to initialize</returns>
        public _RPC_SID[] GetSid(DomainSid sid, string sidString)
        {
            _RPC_SID[] sidToInitialize = new _RPC_SID[1];
            int index = 1;

            if (sid == DomainSid.Invalid)
            {
                sidString = LsadManagedAdapter.InvalidSid;
            }
                        
            char[] delimiter = new char[1];
            delimiter[0] = '-';
            string[] SubAuthorities = sidString.Split(delimiter);

            sidToInitialize[0].Revision = Convert.ToByte(SubAuthorities[index++]);
            sidToInitialize[0].IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
            sidToInitialize[0].IdentifierAuthority.Value = new byte[IDENTIFIER_AUTHORITY_VALUES];

            sidToInitialize[0].IdentifierAuthority.Value[0] = (byte)Value_Values.NULL_SID_AUTHORITY;
            sidToInitialize[0].IdentifierAuthority.Value[1] = (byte)Value_Values.NULL_SID_AUTHORITY;
            sidToInitialize[0].IdentifierAuthority.Value[2] = (byte)Value_Values.NULL_SID_AUTHORITY;
            sidToInitialize[0].IdentifierAuthority.Value[3] = (byte)Value_Values.NULL_SID_AUTHORITY;
            sidToInitialize[0].IdentifierAuthority.Value[4] = (byte)Value_Values.NULL_SID_AUTHORITY;
            sidToInitialize[0].IdentifierAuthority.Value[5] = Convert.ToByte(SubAuthorities[index++]);

            sidToInitialize[0].SubAuthorityCount = Convert.ToByte(SubAuthorities.Length - index);
            sidToInitialize[0].SubAuthority = new uint[sidToInitialize[0].SubAuthorityCount];
            
            for (int i = 0; i < (SubAuthorities.Length - index); i++)
            {
                sidToInitialize[0].SubAuthority[i] = Convert.ToUInt32(SubAuthorities[i + index]);
            }

            return sidToInitialize;
        }

        #endregion

        #region GetTheDomainName

        /// <summary>
        /// Initialize the Domain Name.
        /// </summary>
        /// <param name="checkNameExists">Domain type which is to be checked.</param>
        /// <param name="DomainName">The name of the trusted domain to query</param>
        /// <returns>Return 0 means get domain name successfully</returns>
        public int GetTheDomainName(DomainType checkNameExists, ref _RPC_UNICODE_STRING[] DomainName)
        {
            int intCount = 0;
            string[] arrNameString = new string[10];
            string[] arrNamesArray = new string[10];

            switch (checkNameExists)
            {
                case DomainType.ValidDomainName:
                    arrNamesArray[0] = LsadManagedAdapter.ValidDomainName;
                    break;
                case DomainType.CurrentDomain:
                    arrNamesArray[0] = LsadManagedAdapter.domain;
                    break;
                case DomainType.NoDomainName:
                    arrNamesArray[0] = LsadManagedAdapter.NoDomainName;
                    break;
                case DomainType.DCName:
                    arrNamesArray[0] = LsadManagedAdapter.Instance(TestClassBase.BaseTestSite).PDCNetbiosName;
                    break;
                case DomainType.newDomainNameforSetDomainInfo:
                    arrNamesArray[0] = LsadManagedAdapter.NewDomainNameforSetdomainInfo;
                    break;
                case DomainType.SecretNameToCheck:
                    arrNamesArray[0] = LsadManagedAdapter.GlobalSecretName;
                    break;
            }

            ////convereting a string to a ushort array
            for (int index = 0; index <= intCount; index++)
            {
                arrNameString[index] = arrNamesArray[index];

                char[] nameArray = new char[arrNameString[index].Length];
                nameArray = arrNameString[index].ToCharArray();

                DomainName[index].Buffer = new ushort[nameArray.Length];
                Array.Copy(nameArray, DomainName[index].Buffer, nameArray.Length);
            }

            return 0;
        }

        #endregion

        #region GetAuthInformation

        /// <summary>
        /// Passing value to authentication information.
        /// </summary>
        /// <param name="numberOfEntries">Number of entries</param>
        /// <param name="direction">Type of direction</param>
        /// <returns>Return authentication information for direction</returns>
        public _LSAPR_AUTH_INFORMATION[] GetAuthInformation(uint numberOfEntries, DirectionType direction)
        {
            _LSAPR_AUTH_INFORMATION[] authInformation = new _LSAPR_AUTH_INFORMATION[numberOfEntries];

            for (uint i = 0; i < numberOfEntries; i++)
            {
                if (direction == DirectionType.Incoming)
                {
                    authInformation[i].AuthType = (AuthType_Values)LsadManagedAdapter.IncomingAuthType;
                        
                    authInformation[i].AuthInfo =
                        Encoding.Default.GetBytes(LsadManagedAdapter.IncomingAuthInfo);
                }
                else if (direction == DirectionType.IncomingPrev)
                {
                    authInformation[i].AuthType = (AuthType_Values)LsadManagedAdapter.IncomingAuthType;

                    authInformation[i].AuthInfo =
                        Encoding.Default.GetBytes(LsadManagedAdapter.PrevIncomingAuthInfo);
                }
                else if (direction == DirectionType.Outgoing)
                {
                    authInformation[i].AuthType = (AuthType_Values)LsadManagedAdapter.IncomingAuthType;
                        
                    authInformation[i].AuthInfo =
                        Encoding.Default.GetBytes(LsadManagedAdapter.OutgoingAuthInfo);
                }
                else
                {
                    authInformation[i].AuthType = (AuthType_Values)LsadManagedAdapter.IncomingAuthType;
                        
                    authInformation[i].AuthInfo =
                        Encoding.Default.GetBytes(LsadManagedAdapter.PrevOutgoingAuthInfo);
                }

                authInformation[i].AuthInfoLength = (uint)authInformation[i].AuthInfo.Length;
            }

            return authInformation;
        }

        #endregion

        #region CheckFortheValidityOfData

        /// <summary>
        /// Checking if the Data that we set is equal to the Data that we are querying
        /// </summary>
        /// <param name="domainInformation">Data we set</param>
        /// <param name="informationClass">Date we are querying</param>
        /// <returns>Return true if  Data that we set is equal to the Data that we are querying
        /// others return false.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = @"Disable
        warning CA1502 because it will affect the implementation of Adapter and Model codes if do any changes about
        maintainability.")]
        public bool CheckForValidityOfData(
            _LSAPR_TRUSTED_DOMAIN_INFO domainInformation,
            _TRUSTED_INFORMATION_CLASS informationClass)
        {
            if (informationClass == _TRUSTED_INFORMATION_CLASS.TrustedPosixOffsetInformation)
            {
                return LsadManagedAdapter.domainInformationThatWasSet.TrustedPosixOffsetInfo.Offset ==
                            domainInformation.TrustedPosixOffsetInfo.Offset;
            }
            else if (informationClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx)
            {
                #region ValidationForInformationEx

                return LsadManagedAdapter.domainInformationThatWasSet.TrustedDomainInfoEx.TrustAttributes ==
                             domainInformation.TrustedDomainInfoEx.TrustAttributes
                             && (LsadManagedAdapter.domainInformationThatWasSet.TrustedDomainInfoEx.TrustDirection ==
                                     domainInformation.TrustedDomainInfoEx.TrustDirection)
                             && (LsadManagedAdapter.domainInformationThatWasSet.TrustedDomainInfoEx.TrustType ==
                                     domainInformation.TrustedDomainInfoEx.TrustType);

                #endregion ValidationForInformationEx
            }
            else if (informationClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation)
            {
                #region ValidationForFullInfo

                return LsadManagedAdapter.domainInformationThatWasSet.TrustedFullInfo.PosixOffset.Offset ==
                             domainInformation.TrustedFullInfo.PosixOffset.Offset
                             && (LsadManagedAdapter.domainInformationThatWasSet.TrustedFullInfo.Information.TrustType ==
                                     domainInformation.TrustedFullInfo.Information.TrustType)
                             && (LsadManagedAdapter.domainInformationThatWasSet.TrustedFullInfo.Information.TrustDirection ==
                                     domainInformation.TrustedFullInfo.Information.TrustDirection)
                             && (LsadManagedAdapter.domainInformationThatWasSet.TrustedFullInfo.Information.TrustAttributes ==
                                     domainInformation.TrustedFullInfo.Information.TrustAttributes)
                             && (domainInformation.TrustedFullInfo.AuthInformation.IncomingAuthInfos == 0)
                             && (domainInformation.TrustedFullInfo.AuthInformation.OutgoingAuthInfos == 0)
                             && (domainInformation.TrustedFullInfo.AuthInformation.IncomingAuthenticationInformation == null)
                             && (domainInformation.TrustedFullInfo.AuthInformation.IncomingPreviousAuthenticationInformation == null)
                             && (domainInformation.TrustedFullInfo.AuthInformation.OutgoingAuthenticationInformation == null)
                             && (domainInformation.TrustedFullInfo.AuthInformation.OutgoingPreviousAuthenticationInformation == null);

                #endregion ValidationForFullInfo
            }
            else if (informationClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainSupportedEncryptionTypes)
            {
                #region ValidationForEncryptinonTypes

                return LsadManagedAdapter.domainInformationThatWasSet.TrustedDomainSETs.SupportedEncryptionTypes ==
                            domainInformation.TrustedDomainSETs.SupportedEncryptionTypes;

                #endregion ValidationForEncryptinonTypes
            }
            else if (informationClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainNameInformation)
            {
                #region ValidationForDomainNameInfo

                _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];
                this.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
                domainName[0].Length = (ushort)(2 * domainName[0].Buffer.Length);
                domainName[0].MaximumLength = (ushort)(2 + domainName[0].Length);

                if (this.CheckTheRpcStrings(domainInformation.TrustedDomainNameInfo.Name, domainName[0]))
                {
                    #region MS-LSAD_R943

                    TestClassBase.BaseTestSite.CaptureRequirementIfIsTrue(
                        this.CheckTheRpcStrings(domainInformation.TrustedDomainNameInfo.Name, domainName[0]),                        
                        "MS-LSAD",
                        943,
                        @"For LSAPR_TRUSTED_DOMAIN_NAME_INFO, Name MUST satisfy RPC_UNICODE_STRING validation.");

                    #endregion
                }

                return this.CheckTheRpcStrings(domainInformation.TrustedDomainNameInfo.Name, domainName[0]);

                #endregion ValidationForDomainNameInfo
            }
            else if (informationClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation2Internal)
            {
                #region ValidationForFullInfo2Internal

                _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];
                this.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
                domainName[0].Length = (ushort)(2 * domainName[0].Buffer.Length);
                domainName[0].MaximumLength = (ushort)(2 + domainName[0].Length);
                _RPC_SID[] trustSid = new _RPC_SID[1];
                trustSid = this.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);

                return domainInformation.TrustedFullInfo2.AuthInformation.IncomingAuthInfos == 0
                             && (domainInformation.TrustedFullInfo2.AuthInformation.OutgoingAuthInfos == 0)
                             && (domainInformation.TrustedFullInfo2.AuthInformation.IncomingAuthenticationInformation == null)
                             && (domainInformation.TrustedFullInfo2.AuthInformation.IncomingPreviousAuthenticationInformation == null)
                             && (domainInformation.TrustedFullInfo2.AuthInformation.OutgoingAuthenticationInformation == null)
                             && (domainInformation.TrustedFullInfo2.AuthInformation.OutgoingPreviousAuthenticationInformation == null)
                             && (domainInformation.TrustedFullInfo2.Information.ForestTrustInfo == null)
                             && (domainInformation.TrustedFullInfo2.Information.ForestTrustLength == 0)
                             && (domainInformation.TrustedFullInfo2.Information.TrustType ==
                                     (uint)LsadManagedAdapter.trustObjectCreateinformation.uintTrustType)
                             && (domainInformation.TrustedFullInfo2.Information.TrustDirection ==
                                     LsadManagedAdapter.trustObjectCreateinformation.uintTrustDir)
                             && (domainInformation.TrustedFullInfo2.Information.TrustAttributes ==
                                     LsadManagedAdapter.trustObjectCreateinformation.uintTrustAttr)
                             && this.CheckTheRpcStrings(domainInformation.TrustedFullInfo2.Information.Name, domainName[0])
                             && this.CheckTheSids(domainInformation.TrustedFullInfo2.Information.Sid[0], trustSid[0]);

                #endregion ValidationForFullInfo2Internal
            }
            else if (informationClass == _TRUSTED_INFORMATION_CLASS.TrustedPasswordInformation)
            {
                #region ValidationForPasswordInfo

                return this.CompareTwoCipherValueStructures(
                    LsadManagedAdapter.domainInformationThatWasSet.TrustedPasswordInfo.Password,
                    domainInformation.TrustedPasswordInfo.Password)
                            && this.CompareTwoCipherValueStructures(
                            LsadManagedAdapter.domainInformationThatWasSet.TrustedPasswordInfo.OldPassword,
                            domainInformation.TrustedPasswordInfo.OldPassword);

                #endregion ValidationForPasswordInfo
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CheckTheRpcStrings
        
        /// <summary>
        /// Check the RPC string are same or not.
        /// </summary>
        /// <param name="current">Current RPC strings</param>
        /// <param name="previous">Previous RPC strings</param>
        /// <returns>Return true if current collection of RPC string is the same as previous 
        /// collection RPC string</returns>
        public bool CheckTheRpcStrings(_RPC_UNICODE_STRING current, _RPC_UNICODE_STRING previous)
        {
            if ((current.Length == previous.Length) && (current.MaximumLength == previous.MaximumLength))
            {
                for (int i = 0; i < current.Buffer.Length; i++)
                {
                    if (current.Buffer[i] != previous.Buffer[i])
                    {
                        break;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CheckTheSids

        /// <summary>
        /// Judge whether sid in current sid queue and sid in previous queue are all same or not.
        /// </summary>
        /// <param name="Current">Current sid queue</param>
        /// <param name="previous">Previous sid queue</param>
        /// <returns>Return true if sid in current sid queue and sid in previous queue are all same,
        /// Others return false</returns>
        public bool CheckTheSids(_RPC_SID Current, _RPC_SID previous)
        {
            if ((Current.Revision == previous.Revision) && (Current.SubAuthorityCount == previous.SubAuthorityCount))
            {
                for (int j = 0; j < Current.SubAuthorityCount; j++)
                {
                    if (Current.SubAuthority[j] != previous.SubAuthority[j])
                    {
                        return false;
                    }
                }

                for (int k = 0; k < Current.IdentifierAuthority.Value.Length; k++)
                {
                    if (Current.IdentifierAuthority.Value[k] != previous.IdentifierAuthority.Value[k])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        #endregion

        #region CheckForestInformation

        /// <summary>
        /// Querying the information that we set to see if both are same, to be sure that set is success.
        /// </summary>
        /// <param name="previous">Previous LSA_FOREST_TRUST_RECORDS</param>
        /// <param name="current">Current LSA_FOREST_TRUST_RECORDS</param>
        /// <returns>Return true if previous and current are same, others return false</returns>
        public bool CheckForestInformation(
            _LSA_FOREST_TRUST_INFORMATION previous,
            _LSA_FOREST_TRUST_INFORMATION current)
        {
            #region MS-LSAD_R146

            if (current.RecordCount != 0)
            {
                TestClassBase.BaseTestSite.CaptureRequirementIfIsNotNull(
                    current.Entries,
                    "MS-LSAD",
                    146,
                    @"In LSA_FOREST_TRUST_INFORMATION structure under field 'Entries' : If the RecordCount field has 
                    a value other than 0, Entries field MUST NOT be NULL.");
            }

            #endregion MS-LSAD_R146

            bool CheckSuccess = false;

            if (previous.RecordCount == current.RecordCount)
            {
                for (int i = 0; i < previous.RecordCount; i++)
                {
                    for (int j = 0; j < previous.Entries[i].Length; j++)
                    {
                        if ((previous.Entries[i][j].Flags == current.Entries[i][j].Flags)
                             && (previous.Entries[i][j].ForestTrustType == current.Entries[i][j].ForestTrustType)
                             && this.CheckTheRpcStrings(
                             previous.Entries[i][j].ForestTrustData.DomainInfo.DnsName,
                             current.Entries[i][j].ForestTrustData.DomainInfo.DnsName)
                             && this.CheckTheRpcStrings(
                             previous.Entries[i][j].ForestTrustData.DomainInfo.NetbiosName,
                             current.Entries[i][j].ForestTrustData.DomainInfo.NetbiosName)
                             && this.CheckTheSids(
                             previous.Entries[i][j].ForestTrustData.DomainInfo.Sid[0],
                             current.Entries[i][j].ForestTrustData.DomainInfo.Sid[0]))
                        {
                            CheckSuccess = true;
                        }
                        else
                        {
                            CheckSuccess = false;
                        }
                    }
                }

                return CheckSuccess;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region InitializeInformationClass

        /// <summary>
        /// Initializing the Information Class that we are requesting 
        /// to query and check required access.
        /// Checking if it is a valid information class to be queried
        /// </summary>
        /// <param name="trustedInformation">Values for the trusted information type</param>
        /// <param name="trustedDomainInfo">values for trusted domain object
        /// information passed in for many trusted domain object actions</param>
        /// <param name="isItASetOperation">Whether it is a set option</param>
        /// <param name="domainInformation">A set which store domain information</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = @"Disable
        warning CA1502 because it will affect the implementation of Adapter and Model codes if do any changes about
        maintainability.")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = @"Disable
        warning CA1505 because it will affect the implementation of Adapter and Model codes if do any changes about
        maintainability.")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = @"Disable
        warning CA1506 because it will affect the implementation of Adapter and Model codes if do any changes about
        maintainability.")]
        public void InitializeInformationClass(
            TrustedInformationClass trustedInformation,
            TRUSTED_DOMAIN_INFORMATION_EX trustedDomainInfo,
            bool isItASetOperation,
            ref _LSAPR_TRUSTED_DOMAIN_INFO domainInformation)
        {
            int INVALIDPARAM = 1, SUCCESS = 0, INVALIDINFOCLASS = 2;

            _RPC_UNICODE_STRING[] domainName = new _RPC_UNICODE_STRING[1];

            #region InfoClasses_values

            switch (trustedInformation)
            {
                case TrustedInformationClass.TrustedDomainNameInformation:

                    #region TrustedDomainNameInformation

                    if (isItASetOperation)
                    {
                        this.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
                        domainInformation.TrustedDomainNameInfo.Name = domainName[0];
                        domainInformation.TrustedDomainNameInfo.Name.Length =
                            (ushort)(2 * domainInformation.TrustedDomainNameInfo.Name.Buffer.Length);

                        domainInformation.TrustedDomainNameInfo.Name.MaximumLength =
                            (ushort)(domainInformation.TrustedDomainNameInfo.Name.Length + 2);
                        LsadManagedAdapter.intCheckInfoClass = INVALIDPARAM;
                    }
                    else
                    {
                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                 & (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME) ==
                                       (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME)
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.TrustedDomainInformationBasic:

                    #region TrustedDomainInformationBasic

                    if (isItASetOperation)
                    {
                        this.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
                        domainInformation.TrustedDomainInfoBasic.Name = domainName[0];
                        domainInformation.TrustedDomainInfoBasic.Name.Length =
                            (ushort)(2 * domainInformation.TrustedDomainInfoBasic.Name.Buffer.Length);

                        domainInformation.TrustedDomainInfoBasic.Name.MaximumLength =
                            (ushort)(domainInformation.TrustedDomainInfoBasic.Name.Length + 2);

                        domainInformation.TrustedDomainInfoBasic.Sid = this.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);

                        LsadManagedAdapter.intCheckInfoClass = INVALIDINFOCLASS;
                        LsadManagedAdapter.invalidParamterCount++;
                    }
                    else
                    {
                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                 & (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME) ==
                                       (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME)
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }

                        LsadManagedAdapter.intCheckInfoClass = INVALIDPARAM;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.TrustedDomainInformationEx:

                    #region TrustedDomainInformationEx

                    if (isItASetOperation)
                    {
                        this.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
                        domainName[0].Length = (ushort)(2 * domainName[0].Buffer.Length);
                        domainName[0].MaximumLength = (ushort)(2 + domainName[0].Length);
                        domainInformation.TrustedDomainInfoEx.FlatName = domainName[0];
                        domainInformation.TrustedDomainInfoEx.Name = domainName[0];
                        domainInformation.TrustedDomainInfoEx.Sid = this.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);
                        domainInformation.TrustedDomainInfoEx.TrustAttributes = trustedDomainInfo.TrustAttr;
                        domainInformation.TrustedDomainInfoEx.TrustDirection = trustedDomainInfo.TrustDir;
                        domainInformation.TrustedDomainInfoEx.TrustType = (TrustType_Values)trustedDomainInfo.TrustType;

                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                  & (uint)ACCESS_MASK.TRUSTED_SET_POSIX) !=
                                        (uint)ACCESS_MASK.TRUSTED_SET_POSIX)
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }
                    else
                    {
                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                 & (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME) ==
                                       (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME)
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.TrustedDomainInformationEx2Internal:

                    #region TrustedDomainInformationEx2Internal

                    if (isItASetOperation)
                    {
                        this.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
                        domainName[0].Length = (ushort)(2 * domainName[0].Buffer.Length);
                        domainName[0].MaximumLength = (ushort)(2 + domainName[0].Length);
                        domainInformation.TrustedDomainInfoEx2.FlatName = domainName[0];
                        domainInformation.TrustedDomainInfoEx2.Name = domainName[0];
                        domainInformation.TrustedDomainInfoEx2.Sid = this.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);
                        domainInformation.TrustedDomainInfoEx2.ForestTrustLength = 0;
                        domainInformation.TrustedDomainInfoEx2.ForestTrustInfo = null;
                        domainInformation.TrustedDomainInfoEx2.TrustAttributes = trustedDomainInfo.TrustAttr;
                        domainInformation.TrustedDomainInfoEx2.TrustDirection = trustedDomainInfo.TrustDir;
                        domainInformation.TrustedDomainInfoEx2.TrustType = trustedDomainInfo.TrustType;
                        LsadManagedAdapter.intCheckInfoClass = INVALIDPARAM;
                    }
                    else
                    {
                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                 & (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME) ==
                                       (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME)
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }

                        LsadManagedAdapter.intCheckInfoClass = INVALIDPARAM;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.TrustedControllersInformation:

                    #region TrustedControllersInformation

                    if (isItASetOperation)
                    {
                        domainInformation.TrustedControllersInfo.Entries = 1;
                        this.GetTheDomainName(DomainType.DCName, ref domainName);
                        domainInformation.TrustedControllersInfo.Names = domainName;
                        domainInformation.TrustedControllersInfo.Names[0].Length =
                            (ushort)(2 * domainInformation.TrustedControllersInfo.Names[0].Buffer.Length);

                        domainInformation.TrustedControllersInfo.Names[0].MaximumLength =
                            (ushort)(domainInformation.TrustedControllersInfo.Names[0].Length + 2);

                        if (domainInformation.TrustedControllersInfo.Entries != 0)
                        {
                            #region MS-LSAD_R95

                            TestClassBase.BaseTestSite.CaptureRequirementIfIsNotNull(
                                domainInformation.TrustedControllersInfo.Names,
                                "MS-LSAD",
                                95,
                                @"In LSAPR_TRUSTED_CONTROLLERS_INFO structure under field 'Names' : If the Entries 
                                field has a value other than 0, Names field MUST NOT be NULL.");

                            #endregion
                        }

                        LsadManagedAdapter.intCheckInfoClass = INVALIDPARAM;
                    }
                    else
                    {
                        LsadManagedAdapter.intCheckInfoClass = INVALIDPARAM;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.TrustedPosixOffsetInformation:

                    #region TrustedPosixOffsetInformation

                    if (isItASetOperation)
                    {
                        domainInformation.TrustedPosixOffsetInfo.Offset = 1;

                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                  & (uint)ACCESS_MASK.TRUSTED_SET_POSIX) !=
                                        (uint)ACCESS_MASK.TRUSTED_SET_POSIX)
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }
                    else
                    {
                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                 & (uint)ACCESS_MASK.TRUSTED_QUERY_POSIX) ==
                                       (uint)ACCESS_MASK.TRUSTED_QUERY_POSIX)
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.TrustedDomainSupportedEncryptionTypes:

                    #region TrustedDomainSupportedEncryptionTypes

                    if (isItASetOperation)
                    {
                        domainInformation.TrustedDomainSETs.SupportedEncryptionTypes =
                            LsadUtilities.KERB_ENCTYPE_RC4_HMAC_MD5;

                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                  & (uint)ACCESS_MASK.TRUSTED_SET_POSIX) !=
                                        (uint)ACCESS_MASK.TRUSTED_SET_POSIX)
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }
                    else
                    {
                        if (((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess)
                                  & (uint)ACCESS_MASK.TRUSTED_SET_POSIX) !=
                                        (uint)ACCESS_MASK.TRUSTED_SET_POSIX)
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.TrustedPasswordInformation:

                    #region TrustedPasswordInformation

                    if (isItASetOperation)
                    {
                        _RPC_UNICODE_STRING currentPassword = new _RPC_UNICODE_STRING();
                        _RPC_UNICODE_STRING oldPassword = new _RPC_UNICODE_STRING();
                        _RPC_UNICODE_STRING outputCurrent = new _RPC_UNICODE_STRING();
                        _RPC_UNICODE_STRING outputOld = new _RPC_UNICODE_STRING();
                        byte[] keyToEncrypt = new byte[16];
                        string strPasswordValues = string.Empty;
                        keyToEncrypt = LsadManagedAdapter.lsadAdapter.SessionKey;
                        strPasswordValues = LsadManagedAdapter.NewPassword;

                        currentPassword = DtypUtility.ToRpcUnicodeString(strPasswordValues);
                        LsaUtility.EncryptSecret(currentPassword, keyToEncrypt, out outputCurrent);

                        strPasswordValues = LsadManagedAdapter.OldPassword;

                        oldPassword = DtypUtility.ToRpcUnicodeString(strPasswordValues);

                        LsaUtility.EncryptSecret(oldPassword, keyToEncrypt, out outputOld);

                        domainInformation.TrustedPasswordInfo.Password = new _LSAPR_CR_CIPHER_VALUE[1];
                        domainInformation.TrustedPasswordInfo.Password[0].Buffer = new byte[outputCurrent.Length];
                        
                        Buffer.BlockCopy(
                            outputCurrent.Buffer, 
                            0, 
                            domainInformation.TrustedPasswordInfo.Password[0].Buffer, 
                            0, 
                            outputCurrent.Length);

                        domainInformation.TrustedPasswordInfo.Password[0].Length =
                            (uint)domainInformation.TrustedPasswordInfo.Password[0].Buffer.Length;
                        domainInformation.TrustedPasswordInfo.Password[0].MaximumLength =
                            (uint)domainInformation.TrustedPasswordInfo.Password[0].Length;

                        domainInformation.TrustedPasswordInfo.OldPassword = new _LSAPR_CR_CIPHER_VALUE[1];
                        domainInformation.TrustedPasswordInfo.OldPassword[0].Buffer = new byte[outputOld.Length];
                        
                        Buffer.BlockCopy(
                            outputOld.Buffer, 
                            0, 
                            domainInformation.TrustedPasswordInfo.OldPassword[0].Buffer, 
                            0, 
                            outputOld.Length);

                        domainInformation.TrustedPasswordInfo.OldPassword[0].Length =
                            (uint)domainInformation.TrustedPasswordInfo.OldPassword[0].Buffer.Length;
                        domainInformation.TrustedPasswordInfo.OldPassword[0].MaximumLength =
                            (uint)domainInformation.TrustedPasswordInfo.OldPassword[0].Length;
                        LsadManagedAdapter.intCheckInfoClass = INVALIDPARAM;
                    }
                    else
                    {
                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                 & (uint)ACCESS_MASK.TRUSTED_QUERY_AUTH) ==
                                       (uint)ACCESS_MASK.TRUSTED_QUERY_AUTH)
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }

                        LsadManagedAdapter.intCheckInfoClass = INVALIDPARAM;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.TrustedDomainAuthInformation:

                    #region TrustedDomainAuthInformation

                    if (isItASetOperation)
                    {
                        domainInformation.TrustedAuthInfo.IncomingAuthInfos = LsadManagedAdapter.CountOfIncomingAuthInfos;
                            
                        domainInformation.TrustedAuthInfo.IncomingAuthenticationInformation = this.GetAuthInformation(
                            domainInformation.TrustedAuthInfo.IncomingAuthInfos,
                            DirectionType.Incoming);

                        domainInformation.TrustedAuthInfo.IncomingPreviousAuthenticationInformation = this.GetAuthInformation(
                            domainInformation.TrustedAuthInfo.IncomingAuthInfos,
                            DirectionType.IncomingPrev);

                        domainInformation.TrustedAuthInfo.OutgoingAuthInfos = LsadManagedAdapter.CountOfOutgoingAuthInfos;
                            
                        domainInformation.TrustedAuthInfo.OutgoingAuthenticationInformation = this.GetAuthInformation(
                            domainInformation.TrustedAuthInfo.OutgoingAuthInfos,
                            DirectionType.Outgoing);

                        domainInformation.TrustedAuthInfo.OutgoingPreviousAuthenticationInformation = this.GetAuthInformation(
                            domainInformation.TrustedAuthInfo.OutgoingAuthInfos,
                            DirectionType.OutgoingPrev);

                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                  & (uint)ACCESS_MASK.TRUSTED_SET_AUTH) !=
                                        (uint)ACCESS_MASK.TRUSTED_SET_AUTH)
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }
                    else
                    {
                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                 & (uint)ACCESS_MASK.TRUSTED_QUERY_AUTH) ==
                                       (uint)ACCESS_MASK.TRUSTED_QUERY_AUTH)
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }

                        LsadManagedAdapter.intCheckInfoClass = INVALIDINFOCLASS;
                        LsadManagedAdapter.invalidParamterCount++;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.TrustedDomainAuthInformationInternal:

                    #region TrustedDomainAuthInformationInternal

                    if (isItASetOperation)
                    {
                        _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[] authInformation = 
                            new _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[1];
                        authInformation[0].IncomingAuthInfos = 0;
                        authInformation[0].OutgoingAuthInfos = 0;
                        authInformation[0].IncomingAuthenticationInformation = new _LSAPR_AUTH_INFORMATION[1];
                        authInformation[0].IncomingPreviousAuthenticationInformation = new _LSAPR_AUTH_INFORMATION[1];
                        authInformation[0].OutgoingAuthenticationInformation = new _LSAPR_AUTH_INFORMATION[1];
                        authInformation[0].OutgoingPreviousAuthenticationInformation = new _LSAPR_AUTH_INFORMATION[1];

                        domainInformation.TrustedAuthInfoInternal = 
                            new _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION_INTERNAL();
                        
                        _LSAPR_AUTH_INFORMATION[] authInfos = new _LSAPR_AUTH_INFORMATION[1];
                        authInfos[0].AuthInfo = new byte[3] { 1, 2, 3 };
                        authInfos[0].AuthInfoLength = (uint)authInfos[0].AuthInfo.Length;
                        authInfos[0].AuthType = AuthType_Values.V1;
                        authInfos[0].LastUpdateTime = new _LARGE_INTEGER();
                        authInfos[0].LastUpdateTime.QuadPart = 0xcafebeef; 
                        domainInformation.TrustedAuthInfoInternal.AuthBlob = LsaUtility.CreateTrustedDomainAuthorizedBlob(
                            authInfos,
                            null,
                            authInfos,
                            null,
                            LsadManagedAdapter.lsadAdapter.SessionKey);

                        if (domainInformation.TrustedAuthInfoInternal.AuthBlob.AuthSize != 0)
                        {
                            #region MS-LSAD_R127

                            TestClassBase.BaseTestSite.CaptureRequirementIfIsNotNull(
                                domainInformation.TrustedAuthInfoInternal.AuthBlob,
                                "MS-LSAD",
                                127,
                                @"In LSAPR_TRUSTED_DOMAIN_AUTH_BLOB structure under field 'AuthBlob' : 
                                If the AuthSize field has a value other than 0, AuthBlob field MUST NOT be NULL.");

                            #endregion
                        }

                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                  & (uint)ACCESS_MASK.TRUSTED_SET_AUTH) !=
                                        (uint)ACCESS_MASK.TRUSTED_SET_AUTH)
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }
                    else
                    {
                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                 & (uint)ACCESS_MASK.TRUSTED_QUERY_AUTH) ==
                                       (uint)ACCESS_MASK.TRUSTED_QUERY_AUTH)
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }

                        LsadManagedAdapter.intCheckInfoClass = INVALIDINFOCLASS;
                        LsadManagedAdapter.invalidParamterCount++;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.TrustedDomainFullInformation:

                    #region TrustedDomainFullInformation

                    if (isItASetOperation)
                    {
                        domainInformation.TrustedFullInfo.AuthInformation.IncomingAuthInfos = LsadManagedAdapter.CountOfIncomingAuthInfos;
                            
                        domainInformation.TrustedFullInfo.AuthInformation.IncomingAuthenticationInformation = this.GetAuthInformation(
                            domainInformation.TrustedFullInfo.AuthInformation.IncomingAuthInfos,
                            DirectionType.Incoming);

                        domainInformation.TrustedFullInfo.AuthInformation.IncomingPreviousAuthenticationInformation = this.GetAuthInformation(
                            domainInformation.TrustedFullInfo.AuthInformation.IncomingAuthInfos,
                            DirectionType.IncomingPrev);

                        domainInformation.TrustedFullInfo.AuthInformation.OutgoingAuthInfos = LsadManagedAdapter.CountOfOutgoingAuthInfos;
                            
                        domainInformation.TrustedFullInfo.AuthInformation.OutgoingAuthenticationInformation = this.GetAuthInformation(
                            domainInformation.TrustedFullInfo.AuthInformation.OutgoingAuthInfos,
                            DirectionType.Outgoing);

                        domainInformation.TrustedFullInfo.AuthInformation.OutgoingPreviousAuthenticationInformation = this.GetAuthInformation(
                            domainInformation.TrustedFullInfo.AuthInformation.OutgoingAuthInfos,
                            DirectionType.OutgoingPrev);

                        this.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
                        domainName[0].Length = (ushort)(2 * domainName[0].Buffer.Length);
                        domainName[0].MaximumLength = (ushort)(2 + domainName[0].Length);
                        domainInformation.TrustedFullInfo.Information.FlatName = domainName[0];
                        domainInformation.TrustedFullInfo.Information.Name = domainName[0];
                        domainInformation.TrustedFullInfo.Information.Sid = this.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);
                        domainInformation.TrustedFullInfo.Information.TrustAttributes = trustedDomainInfo.TrustAttr;
                        domainInformation.TrustedFullInfo.Information.TrustDirection = trustedDomainInfo.TrustDir;
                        domainInformation.TrustedFullInfo.Information.TrustType = (TrustType_Values)trustedDomainInfo.TrustType;
                        domainInformation.TrustedFullInfo.PosixOffset.Offset = 10;

                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                  & ((uint)ACCESS_MASK.TRUSTED_SET_AUTH | (uint)ACCESS_MASK.TRUSTED_SET_POSIX)) !=
                                         ((uint)ACCESS_MASK.TRUSTED_SET_AUTH | (uint)ACCESS_MASK.TRUSTED_SET_POSIX))
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }
                    else
                    {
                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                 & (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME) ==
                                       (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME)
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.TrustedDomainFullInformationInternal:

                    #region TrustedDomainFullInformationInternal

                    if (isItASetOperation)
                    {
                        _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[] authInformation =
                            new _LSAPR_TRUSTED_DOMAIN_AUTH_INFORMATION[1];

                        authInformation[0].IncomingAuthInfos = 0;
                        authInformation[0].OutgoingAuthInfos = 0;
                        authInformation[0].IncomingAuthenticationInformation = new _LSAPR_AUTH_INFORMATION[1];
                        authInformation[0].IncomingPreviousAuthenticationInformation = new _LSAPR_AUTH_INFORMATION[1];
                        authInformation[0].OutgoingAuthenticationInformation = new _LSAPR_AUTH_INFORMATION[1];
                        authInformation[0].OutgoingPreviousAuthenticationInformation = new _LSAPR_AUTH_INFORMATION[1];

                        _LSAPR_AUTH_INFORMATION[] authInfos = new _LSAPR_AUTH_INFORMATION[1];
                        authInfos[0].AuthInfo = new byte[3] { 1, 2, 3 };
                        authInfos[0].AuthInfoLength = (uint)authInfos[0].AuthInfo.Length;
                        authInfos[0].AuthType = AuthType_Values.V1;
                        authInfos[0].LastUpdateTime = new _LARGE_INTEGER();
                        authInfos[0].LastUpdateTime.QuadPart = 0xcafebeef;
                        domainInformation.TrustedFullInfoInternal.AuthInformation.AuthBlob = LsaUtility.CreateTrustedDomainAuthorizedBlob(
                            authInfos,
                            null,
                            authInfos,
                            null,
                            LsadManagedAdapter.lsadAdapter.SessionKey);

                        this.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
                        domainInformation.TrustedFullInfoInternal.Information.FlatName = domainName[0];
                        domainInformation.TrustedFullInfoInternal.Information.FlatName.Length =
                            (ushort)(2 * domainName[0].Buffer.Length);

                        domainInformation.TrustedFullInfoInternal.Information.FlatName.MaximumLength =
                            (ushort)(2 + domainInformation.TrustedFullInfoInternal.Information.FlatName.Length);

                        this.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
                        domainInformation.TrustedFullInfoInternal.Information.Name = domainName[0];
                        domainInformation.TrustedFullInfoInternal.Information.Name.Length =
                            (ushort)(2 * domainName[0].Buffer.Length);

                        domainInformation.TrustedFullInfoInternal.Information.Name.MaximumLength =
                            (ushort)(2 + domainInformation.TrustedFullInfoInternal.Information.Name.Length);

                        domainInformation.TrustedFullInfoInternal.Information.Sid = this.GetSid(
                            DomainSid.Valid,
                            LsadManagedAdapter.ValidSid);

                        domainInformation.TrustedFullInfoInternal.Information.TrustAttributes = trustedDomainInfo.TrustAttr;
                        domainInformation.TrustedFullInfoInternal.Information.TrustDirection = trustedDomainInfo.TrustDir;
                        domainInformation.TrustedFullInfoInternal.Information.TrustType =
                            (TrustType_Values)trustedDomainInfo.TrustType;

                        domainInformation.TrustedFullInfoInternal.PosixOffset.Offset = 10;

                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                  & ((uint)ACCESS_MASK.TRUSTED_SET_AUTH | (uint)ACCESS_MASK.TRUSTED_SET_POSIX)) !=
                                        ((uint)ACCESS_MASK.TRUSTED_SET_AUTH | (uint)ACCESS_MASK.TRUSTED_SET_POSIX))
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }
                    else
                    {
                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                 & (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME) ==
                                       (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME)
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }

                        LsadManagedAdapter.intCheckInfoClass = INVALIDINFOCLASS;
                        LsadManagedAdapter.invalidParamterCount++;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.TrustedDomainFullInformation2Internal:

                    #region TrustedDomainFullInformation2Internal

                    if (isItASetOperation)
                    {
                        domainInformation.TrustedFullInfo2.AuthInformation.IncomingAuthInfos = LsadManagedAdapter.CountOfIncomingAuthInfos;
                            
                        domainInformation.TrustedFullInfo2.AuthInformation.IncomingAuthenticationInformation = this.GetAuthInformation(
                            domainInformation.TrustedFullInfo2.AuthInformation.IncomingAuthInfos,
                            DirectionType.Incoming);

                        domainInformation.TrustedFullInfo2.AuthInformation.IncomingPreviousAuthenticationInformation = this.GetAuthInformation(
                            domainInformation.TrustedFullInfo2.AuthInformation.IncomingAuthInfos,
                            DirectionType.IncomingPrev);

                        domainInformation.TrustedFullInfo2.AuthInformation.OutgoingAuthInfos = LsadManagedAdapter.CountOfOutgoingAuthInfos;
                            
                        domainInformation.TrustedFullInfo2.AuthInformation.OutgoingAuthenticationInformation = this.GetAuthInformation(
                            domainInformation.TrustedFullInfo2.AuthInformation.OutgoingAuthInfos,
                            DirectionType.Outgoing);

                        domainInformation.TrustedFullInfo2.AuthInformation.OutgoingPreviousAuthenticationInformation = this.GetAuthInformation(
                            domainInformation.TrustedFullInfo2.AuthInformation.OutgoingAuthInfos,
                            DirectionType.OutgoingPrev);

                        this.GetTheDomainName(DomainType.ValidDomainName, ref domainName);
                        domainName[0].Length = (ushort)(2 * domainName[0].Buffer.Length);
                        domainName[0].MaximumLength = (ushort)(2 + domainName[0].Length);
                        domainInformation.TrustedFullInfo2.Information.FlatName = domainName[0];
                        domainInformation.TrustedFullInfo2.Information.Name = domainName[0];

                        domainInformation.TrustedFullInfo2.Information.Sid = this.GetSid(DomainSid.Valid, LsadManagedAdapter.ValidSid);
                        domainInformation.TrustedFullInfo2.Information.ForestTrustLength = 0;
                        domainInformation.TrustedFullInfo2.Information.ForestTrustInfo = null;
                        domainInformation.TrustedFullInfo2.Information.TrustAttributes = trustedDomainInfo.TrustAttr;
                        domainInformation.TrustedFullInfo2.Information.TrustDirection = trustedDomainInfo.TrustDir;
                        domainInformation.TrustedFullInfo2.Information.TrustType = trustedDomainInfo.TrustType;
                        domainInformation.TrustedFullInfo2.PosixOffset.Offset = 10;

                        LsadManagedAdapter.intCheckInfoClass = INVALIDPARAM;
                    }
                    else
                    {
                        if ((LsadManagedAdapter.trustObjectCreateinformation.uintTdoDesiredAccess
                                 & (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME) ==
                                       (uint)ACCESS_MASK.TRUSTED_QUERY_DOMAIN_NAME)
                        {
                            LsadManagedAdapter.isAccessDenied = false;
                        }
                        else
                        {
                            LsadManagedAdapter.isAccessDenied = true;
                        }

                        LsadManagedAdapter.intCheckInfoClass = SUCCESS;
                    }

                    #endregion

                    break;
                case TrustedInformationClass.Invalid:
                    LsadManagedAdapter.intCheckInfoClass = INVALIDPARAM;
                    break;
            }

            #endregion
        }

        #endregion

        #region CheckTheDomain

        /// <summary>
        /// Checking to be sure if the TDO is created or not by enumerating all the TDOs
        /// </summary>
        /// <param name="sidToCheck">SID to check</param>
        /// <param name="nameToCheck">Name to check</param>
        /// <param name="policyHandle">An RPC context handle obtained from either 
        ///                            LsarOpenPolicy or LsarOpenPolicy2</param>
        /// <returns>Return true if the TDO is created by enumerating all the TDOs</returns>
        public bool CheckTheDomain(_RPC_SID sidToCheck, _RPC_UNICODE_STRING nameToCheck, IntPtr policyHandle)
        {
            _LSAPR_TRUSTED_ENUM_BUFFER? enumBuf = new _LSAPR_TRUSTED_ENUM_BUFFER();
            uint? enumerationContext = 0;
            uint preferredMaximumLength = 1000;
            LsadManagedAdapter.lsadClientStack.LsarEnumerateTrustedDomains(
                policyHandle,
                ref enumerationContext,
                out enumBuf,
                preferredMaximumLength);

            if (enumBuf.Value.EntriesRead != 0)
            {
                #region MS-LSAD_R83

                TestClassBase.BaseTestSite.CaptureRequirementIfIsNotNull(
                    enumBuf.Value.Information,
                    "MS-LSAD",
                    83,
                    @"In LSAPR_TRUSTED_ENUM_BUFFER structure under field 'Information' : If the EntriesRead field 
                    has a value other than 0, Information field MUST NOT be NULL.");

                #endregion MS-LSAD_R83
            }

            bool checkIfTrue = false;

            if (sidToCheck.SubAuthority != null)
            {
                #region CheckingIfSidExistsInTheReturnedBuffer

                for (uint i = 0; i < enumBuf.Value.EntriesRead; i++)
                {
                    if (this.CheckTheSids(enumBuf.Value.Information[i].Sid[0], sidToCheck))
                    {
                        checkIfTrue = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                #endregion

                return checkIfTrue;
            }
            else
            {
                #region CheckingIfNameExistsInTheReturnedBuffer

                for (uint i = 0; i < enumBuf.Value.EntriesRead; i++)
                {
                    if (this.CheckTheRpcStrings(enumBuf.Value.Information[i].Name, nameToCheck))
                    {
                        checkIfTrue = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                #endregion

                return checkIfTrue;
            }
        }

        #endregion

        #region CompareCipherValueStructures

        /// <summary>
        /// Check whether revious counted buffer of bytes and the current counted buffer of bytes are same or not.
        /// </summary>
        /// <param name="previous">The previous counted buffer of bytes containing a secret object</param>
        /// <param name="current">The current counted buffer of bytes containing a secret object</param>
        /// <returns>Return true if previous and current are same.</returns>
        public bool CompareTwoCipherValueStructures(_LSAPR_CR_CIPHER_VALUE[] previous, _LSAPR_CR_CIPHER_VALUE[] current)
        {
            bool check = false;

            if ((previous[0].Length == current[0].Length) && (previous[0].MaximumLength == current[0].MaximumLength))
            {
                for (int i = 0; i < previous[0].Length; i++)
                {
                    if (previous[0].Buffer[i] != current[0].Buffer[i])
                    {
                        check = false;
                        break;
                    }
                }

                return check;
            }
            else
            {
                return false;
            }
        }

        #endregion CompareCipherValueStructures

        #region CheckIfDataIsSame

        /// <summary>
        /// Check whether set information and query information are same of not.
        /// </summary>
        /// <param name="InfoClass">Type of trusted domain information</param>
        /// <param name="setInformation">Set information</param>
        /// <param name="queryInformation">Query information</param>
        /// <returns>Return true if set information and query iformation are same.</returns>
        public bool CheckIfDataIsSame(
            _TRUSTED_INFORMATION_CLASS InfoClass,
            _LSAPR_TRUSTED_DOMAIN_INFO setInformation,
            _LSAPR_TRUSTED_DOMAIN_INFO queryInformation)
        {
            if (InfoClass == _TRUSTED_INFORMATION_CLASS.TrustedPosixOffsetInformation)
            {
                return setInformation.TrustedPosixOffsetInfo.Offset == queryInformation.TrustedPosixOffsetInfo.Offset;
            }
            else if (InfoClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainInformationEx)
            {
                return setInformation.TrustedDomainInfoEx.TrustAttributes == queryInformation.TrustedDomainInfoEx.TrustAttributes
                             && (setInformation.TrustedDomainInfoEx.TrustDirection ==
                                     queryInformation.TrustedDomainInfoEx.TrustDirection)
                             && (setInformation.TrustedDomainInfoEx.TrustType ==
                                     queryInformation.TrustedDomainInfoEx.TrustType);
            }
            else if (InfoClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainFullInformation)
            {
                return setInformation.TrustedFullInfo.PosixOffset.Offset == queryInformation.TrustedFullInfo.PosixOffset.Offset
                             && (setInformation.TrustedFullInfo.Information.TrustType ==
                                     queryInformation.TrustedFullInfo.Information.TrustType)
                             && (setInformation.TrustedFullInfo.Information.TrustDirection ==
                                     queryInformation.TrustedFullInfo.Information.TrustDirection)
                             && (setInformation.TrustedFullInfo.Information.TrustAttributes ==
                                     queryInformation.TrustedFullInfo.Information.TrustAttributes)
                             && (queryInformation.TrustedFullInfo.AuthInformation.IncomingAuthInfos == 0)
                             && (queryInformation.TrustedFullInfo.AuthInformation.OutgoingAuthInfos == 0)
                             && (queryInformation.TrustedFullInfo.AuthInformation.IncomingAuthenticationInformation == null)
                             && (queryInformation.TrustedFullInfo.AuthInformation.IncomingPreviousAuthenticationInformation == null)
                             && (queryInformation.TrustedFullInfo.AuthInformation.OutgoingAuthenticationInformation == null)
                             && (queryInformation.TrustedFullInfo.AuthInformation.OutgoingPreviousAuthenticationInformation == null);
            }
            else if (InfoClass == _TRUSTED_INFORMATION_CLASS.TrustedDomainSupportedEncryptionTypes)
            {
                return setInformation.TrustedDomainSETs.SupportedEncryptionTypes ==
                            queryInformation.TrustedDomainSETs.SupportedEncryptionTypes;
            }
            else
            {
                return false;
            }
        }

        #endregion CheckIfDataIsSame

        #region IsValidSid

        /// <summary>
        /// Check SID is valid or not
        /// </summary>
        /// <param name="sid">SID to be checked</param>
        /// <returns>Return true if SID is valid, others return false.</returns>
        public bool IsValidSid(_RPC_SID sid)
        {
            return sid.Revision == 1
                         && (sid.SubAuthorityCount <= 15
                         && sid.SubAuthorityCount > 3)
                         && (sid.IdentifierAuthority.Value[0] == 0x00
                         && sid.IdentifierAuthority.Value[1] == 0x00
                         && sid.IdentifierAuthority.Value[2] == 0x00
                         && sid.IdentifierAuthority.Value[3] == 0x00
                         && sid.IdentifierAuthority.Value[4] == 0x00
                         && sid.IdentifierAuthority.Value[5] == 0x05)
                         && (sid.SubAuthority[0] == 21);
        }

        #endregion IsValidSid

        #region IsRPCStringValid

        /// <summary>
        /// Check a RPC string is valid or not.
        /// </summary>
        /// <param name="name">A RPC string to be checked.</param>
        /// <returns>Return true if the RPC string is valid, others return false.</returns>
        public bool IsRPCStringValid(_RPC_UNICODE_STRING name)
        {
            return (name.Length == (2 * name.Buffer.Length)) && (name.MaximumLength == (2 + name.Length));
        }

        #endregion IsRPCStringValid

        #region CreateTrustedDomainsForEnumerate

        /// <summary>
        /// Create trusted domains for enumerate.
        /// </summary>
        /// <param name="deletecheck">A flag indicating to create a new trusted domain 
        /// or delete an existing trusted domain.</param>
        public void CreateTrustedDomainsForEnumerate(bool deletecheck)
        {
            _LSAPR_TRUST_INFORMATION[] info = new _LSAPR_TRUST_INFORMATION[1];
            string domName = LsadManagedAdapter.NewDomainNameforSetdomainInfo;
            char[] domNameArray = new char[domName.Length];
            int countEnum = 0;
            IntPtr? tempTrustedHandle = IntPtr.Zero;

            for (int i = 0; i < 3; i++)
            {
                domNameArray = domName.ToCharArray();
                info[0].Name.Buffer = new ushort[domNameArray.Length];
                Array.Copy(domNameArray, info[0].Name.Buffer, domNameArray.Length);
                info[0].Name.Length = (ushort)(info[0].Name.Buffer.Length * 2);
                info[0].Name.MaximumLength = (ushort)(info[0].Name.Length + 2);
                info[0].Sid = new _RPC_SID[1];
                info[0].Sid[0].Revision = 0x01;
                info[0].Sid[0].IdentifierAuthority = new _RPC_SID_IDENTIFIER_AUTHORITY();
                info[0].Sid[0].IdentifierAuthority.Value = new byte[6];
                info[0].Sid[0].IdentifierAuthority.Value[0] = (byte)Value_Values.NULL_SID_AUTHORITY;
                info[0].Sid[0].IdentifierAuthority.Value[1] = (byte)Value_Values.NULL_SID_AUTHORITY;
                info[0].Sid[0].IdentifierAuthority.Value[2] = (byte)Value_Values.NULL_SID_AUTHORITY;
                info[0].Sid[0].IdentifierAuthority.Value[3] = (byte)Value_Values.NULL_SID_AUTHORITY;
                info[0].Sid[0].IdentifierAuthority.Value[4] = (byte)Value_Values.NULL_SID_AUTHORITY;
                info[0].Sid[0].IdentifierAuthority.Value[5] = (byte)Value_Values.NT_AUTHORITY;
                info[0].Sid[0].SubAuthorityCount = 4;
                info[0].Sid[0].SubAuthority = new uint[info[0].Sid[0].SubAuthorityCount];
                info[0].Sid[0].SubAuthority[0] = 21;
                info[0].Sid[0].SubAuthority[1] = 2;
                info[0].Sid[0].SubAuthority[2] = 9;
                info[0].Sid[0].SubAuthority[3] = (uint)countEnum++;

                if (!deletecheck)
                {
                    LsadManagedAdapter.lsadClientStack.LsarCreateTrustedDomain(
                        LsadManagedAdapter.validPolicyHandle.Value,
                        info[0],
                        ACCESS_MASK.MAXIMUM_ALLOWED,
                        out tempTrustedHandle);
                }
                else
                {
                    LsadManagedAdapter.lsadClientStack.LsarDeleteTrustedDomain(
                        LsadManagedAdapter.validPolicyHandle.Value,
                        info[0].Sid[0]);
                }

                domName = domName + countEnum;
            }
        }

        #endregion CreateTrustedDomainsForEnumerate

        #endregion TrustObjects

        #endregion Methods
    }
}
