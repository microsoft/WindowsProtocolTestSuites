// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// capabilities variables for smb 
    /// </summary>
    public abstract class SmbCapability
    {
        #region Fields

        /// <summary>
        /// the under layer transport of smb.
        /// </summary>
        private TransportType transportType;

        /// <summary>
        /// A new constant SMB_INFO_PASSTHROUGH is defined as 0x03e8 (decimal 1,000). New values  for the 
        /// InformationLevel field can be set in a client request for a number of  subcommands of the 
        /// SMB_COM_TRANSACTION2. An InformationLevel value of SMB_INFO_PASSTHROUGH  and higher indicates that the 
        /// client is requesting a native Microsoft Windows® NT operating  system information level, as specified in 
        /// section 3.2.4.7 and in [MS-FSCC] section 2.4. The client requests a native Windows NT information level 
        /// by adding SMB_INFO_PASSTHROUGH to the value of the native Windows NT information level, and then passes 
        /// this in the  InformationLevel field in the client request. If the server supports the 
        /// CAP_INFOLEVEL_PASSTHRU  (the CAP_INFOLEVEL_PASSTHRU bit is set in the Capabilities field in the 
        /// SMB_COM_NEGOTIATE response),  the Data buffer in the server response is formatted with information based 
        /// on the native Windows  NT information level requested. The specific format of the data block in the server 
        /// response is  based on the native Windows NT information level, as specified in [MS-FSCC] section 2.4. 
        /// </summary>
        private bool isUsePassThrough;

        /// <summary>
        /// the flags of smb header. 
        /// </summary>
        protected SmbHeader_Flags_Values flag;

        /// <summary>
        /// the basic default flags2 default values 
        /// </summary>
        private SmbHeader_Flags2_Values flags2;

        /// <summary>
        /// default max parameter count 
        /// </summary>
        private ushort maxParameterCount = (ushort)256;

        /// <summary>
        /// default max data count 
        /// </summary>
        private ushort maxDataCount = (ushort)4096;

        /// <summary>
        /// default max setup count 
        /// </summary>
        private byte maxSetupCount = (byte)255;

        /// <summary>
        /// default timeout 
        /// </summary>
        private uint timeout;

        #endregion

        #region Capabilities Variables

        /// <summary>
        /// When true (in  SMB_COM_SESSION_SETUP_ANDX  defined later in this document), all paths sent to the server by
        /// the client are already canonicalized. This means that file/directory names are in upper case, are valid 
        /// characters, . and .. have been removed, and single backslashes are used as separators. 
        /// </summary>
        public bool IsCanonicalized
        {
            get
            {
                return SmbHeader_Flags_Values.CANONICALIZED
                    == (this.flag & SmbHeader_Flags_Values.CANONICALIZED);
            }
            set
            {
                if (value)
                {
                    this.flag |= SmbHeader_Flags_Values.CANONICALIZED;
                }
                else
                {
                    this.flag &= ~SmbHeader_Flags_Values.CANONICALIZED;
                }
            }
        }


        /// <summary>
        /// When true, all pathnames in this SMB must be treated as caseless.  When off, the pathnames are case 
        /// sensitive. 
        /// </summary>
        public bool IsCaseInsensitive
        {
            get
            {
                return SmbHeader_Flags_Values.CASE_INSENSITIVE
                    == (this.flag & SmbHeader_Flags_Values.CASE_INSENSITIVE);
            }
            set
            {
                if (value)
                {
                    this.flag |= SmbHeader_Flags_Values.CASE_INSENSITIVE;
                }
                else
                {
                    this.flag &= ~SmbHeader_Flags_Values.CASE_INSENSITIVE;
                }
            }
        }


        /// <summary>
        /// A flag that indicates whether this node supports authentication,  as specified in [RFC4178], for selecting 
        /// the authentication protocol. 
        /// </summary>
        public bool IsSupportsExtendedSecurity
        {
            get
            {
                return SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY 
                    == (this.flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY);
            }
            set
            {
                if (value)
                {
                    this.flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY;
                }
                else
                {
                    this.flags2 &= ~SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY;
                }
            }
        }


        /// <summary>
        /// If set in a server response, the returned error code MUST be a 32-bit error code in  Status field. 
        /// Otherwise, the Status.DosError.ErrorClass and Status.DosError.Error  fields MUST contain the MS-DOS-style 
        /// error information. When passing 32-bit error  codes is negotiated (see SMB_COM_NEGOTIATE Server Response 
        /// Extension and  SMB_COM_SESSION_SETUP_ANDX Client Request Extension), this flag MUST be set for every SMB.  
        /// This bit field SHOULD be set to 1 when NTLM 0.12 or later is negotiated for the SMB dialect. This bit 
        /// field is called bit 14, as specified in [CIFS] section 3.1.2. 
        /// </summary>
        public bool IsNtStatus
        {
            get
            {
                return SmbHeader_Flags2_Values.SMB_FLAGS2_NT_STATUS
                    == (this.flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_NT_STATUS);
            }
            set
            {
                if (value)
                {
                    this.flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_NT_STATUS;
                }
                else
                {
                    this.flags2 &= ~SmbHeader_Flags2_Values.SMB_FLAGS2_NT_STATUS;
                }
            }
        }


        /// <summary>
        /// all of the following conditions are met:     1. The server supports Distributed File System (DFS) (as 
        /// indicated by the CAP_DFS      flag specified in section 2.2.3). 2. NTLM 0.12 or later is negotiated for 
        /// the SMB      dialect.     3. The share is a DFS share. (as indicated by the SMB_SHARE_IS_IN_DFS flag 
        /// specified      in section 2.2.7).     4. The operation is targeted at a DFS namespace as indicated by the 
        /// application via      the higher-level action specified in section 3.2.4.2). The client MUST set the flag 
        /// for a SMB_COM_TREE_CONNECT_ANDX request (section 2.2.6) when  conditions 1, 2, and 4 in the preceding list 
        /// are met. For other commands, if a valid TID  is used the flag MUST be set if it was set in the 
        /// SMB_COM_TREE_CONNECT_ANDX request based  on the preceding statement, and if the SMB_COM_TREE_CONNECT_ANDX 
        /// response had the  SMB_SHARE_IS_IN_DFS bit set (condition 3 in the preceding list). 
        /// </summary>
        public bool IsSupportsDfs
        {
            get
            {
                return SmbHeader_Flags2_Values.SMB_FLAGS2_DFS
                    == (this.flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_DFS);
            }
            set
            {
                if (value)
                {
                    this.flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_DFS;
                }
                else
                {
                    this.flags2 &= ~SmbHeader_Flags2_Values.SMB_FLAGS2_DFS;
                }
            }
        }


        /// <summary>
        /// If set, the path in the request MUST contain an @GMT token (that is, a Previous Version  token), as 
        /// specified in section 3.2.4.3.1. This bit field SHOULD be set to 1 only when  NTLM 0.12 or later is 
        /// negotiated for the SMB dialect. 
        /// </summary>
        public bool IsReparsePath
        {
            get
            {
                return SmbHeader_Flags2_Values.SMB_FLAGS2_REPARSE_PATH
                    == (this.flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_REPARSE_PATH);
            }
            set
            {
                if (value)
                {
                    this.flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_REPARSE_PATH;
                }
                else
                {
                    this.flags2 &= ~SmbHeader_Flags2_Values.SMB_FLAGS2_REPARSE_PATH;
                }
            }
        }


        /// <summary>
        /// If set in a client request for directory enumeration, the server may return long names  (that is, names 
        /// that are not 8.3 names) in the response to this request. If not set in  a client request for directory 
        /// enumeration, the server MUST return only 8.3 Name in the  response to this request. This flag indicates 
        /// that in a direct enumeration request, paths  are not restricted to 8.3 names by the server. This bit field 
        /// SHOULD be set to 1 when  NTLM1.2X002 or later is negotiated for the SMB dialect. This bit field is called 
        /// bit 0,  as specified in [CIFS] section 3.1.2. 
        /// </summary>
        public bool IsKnowLongNames
        {
            get
            {
                return SmbHeader_Flags2_Values.SMB_FLAGS2_KNOWS_LONG_NAMES
                    == (this.flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_KNOWS_LONG_NAMES);
            }
            set
            {
                if (value)
                {
                    this.flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_KNOWS_LONG_NAMES;
                }
                else
                {
                    this.flags2 &= ~SmbHeader_Flags2_Values.SMB_FLAGS2_KNOWS_LONG_NAMES;
                }
            }
        }


        /// <summary>
        /// If set in a client request or server response, any fields that contain strings in this  SMB message MUST 
        /// be encoded as an array of 16-bit Unicode characters. Otherwise,  these fields MUST be encoded as an array 
        /// of OEM characters. This bit field SHOULD be  set to 1 when NTLM 0.12 or later is negotiated for the SMB 
        /// dialect. This bit field is  called bit 15, as specified in [CIFS] section 3.1.2. 
        /// </summary>
        public bool IsUnicode
        {
            get
            {
                return SmbHeader_Flags2_Values.SMB_FLAGS2_UNICODE
                    == (this.flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_UNICODE);
            }
            set
            {
                if (value)
                {
                    this.flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_UNICODE;
                }
                else
                {
                    this.flags2 &= ~SmbHeader_Flags2_Values.SMB_FLAGS2_UNICODE;
                }
            }
        }


        /// <summary>
        /// If set in a client request, the client is aware of extended attributes. The client MUST  set this bit if 
        /// the client is aware of extended attributes. In response to a client request  with this flag set, a server 
        /// MAY include extended attributes in the response. This bit  field SHOULD be set to 1 when NTLM1.2X002 or 
        /// later is negotiated for the SMB dialect. This bit field is called bit 1, as specified in [CIFS] section 
        /// 3.1.2. 
        /// </summary>
        public bool IsKnowEAS
        {
            get
            {
                return SmbHeader_Flags2_Values.SMB_FLAGS2_KNOWS_EAS
                    == (this.flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_KNOWS_EAS);
            }
            set
            {
                if (value)
                {
                    this.flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_KNOWS_EAS;
                }
                else
                {
                    this.flags2 &= ~SmbHeader_Flags2_Values.SMB_FLAGS2_KNOWS_EAS;
                }
            }
        }


        /// <summary>
        /// A new constant SMB_INFO_PASSTHROUGH is defined as 0x03e8 (decimal 1,000). New values  for the 
        /// InformationLevel field can be set in a client request for a number of  subcommands of the 
        /// SMB_COM_TRANSACTION2. An InformationLevel value of SMB_INFO_PASSTHROUGH  and higher indicates that the 
        /// client is requesting a native Microsoft Windows® NT operating  system information level, as specified in 
        /// section 3.2.4.7 and in [MS-FSCC] section 2.4. The client requests a native Windows NT information level 
        /// by adding SMB_INFO_PASSTHROUGH to the value of the native Windows NT information level, and then passes 
        /// this in the  InformationLevel field in the client request. If the server supports the 
        /// CAP_INFOLEVEL_PASSTHRU  (the CAP_INFOLEVEL_PASSTHRU bit is set in the Capabilities field in the 
        /// SMB_COM_NEGOTIATE response),  the Data buffer in the server response is formatted with information based 
        /// on the native Windows  NT information level requested. The specific format of the data block in the server 
        /// response is  based on the native Windows NT information level, as specified in [MS-FSCC] section 2.4. 
        /// </summary>
        public bool IsUsePassThrough
        {
            get
            {
                return this.isUsePassThrough;
            }
            set
            {
                this.isUsePassThrough = value;
            }
        }


        #endregion

        #region Default Values

        /// <summary>
        /// the under layer transport of smb.
        /// </summary>
        internal TransportType TransportType
        {
            get
            {
                return this.transportType;
            }
            set
            {
                this.transportType = value;
            }
        }


        /// <summary>
        /// return the smb default flag2, merge other capabilities.  such as IsUnicode, IsKnowEAS, 
        /// IsSupportsExtendedSecurity, and so on. 
        /// </summary>
        public SmbHeader_Flags2_Values Flags2
        {
            get
            {
                return this.flags2;
            }
            set
            {
                this.flags2 = value;
            }
        }


        /// <summary>
        /// the flags of smb header. 
        /// </summary>
        public SmbHeader_Flags_Values Flag
        {
            get
            {
                return this.flag;
            }
            set
            {
                this.flag = value;
            }
        }


        /// <summary>
        /// default max parameter count 
        /// </summary>
        public ushort MaxParameterCount
        {
            get
            {
                return this.maxParameterCount;
            }
            set
            {
                this.maxParameterCount = value;
            }
        }


        /// <summary>
        /// default max data count 
        /// </summary>
        public ushort MaxDataCount
        {
            get
            {
                return this.maxDataCount;
            }
            set
            {
                this.maxDataCount = value;
            }
        }


        /// <summary>
        /// default max setup count 
        /// </summary>
        public byte MaxSetupCount
        {
            get
            {
                return this.maxSetupCount;
            }
            set
            {
                this.maxSetupCount = value;
            }
        }


        /// <summary>
        /// default timeout 
        /// </summary>
        public uint Timeout
        {
            get
            {
                return this.timeout;
            }
            set
            {
                this.timeout = value;
            }
        }


        #endregion

        #region Const Values

        /// <summary>
        /// A new constant SMB_INFO_PASSTHROUGH is defined as 0x03e8 (decimal 1,000). New values  for the 
        /// InformationLevel field can be set in a client request for a number of  subcommands of the 
        /// SMB_COM_TRANSACTION2. An InformationLevel value of SMB_INFO_PASSTHROUGH  and higher indicates that the 
        /// client is requesting a native Microsoft Windows® NT operating  system information level, as specified in 
        /// section 3.2.4.7 and in [MS-FSCC] section 2.4. The client requests a native Windows NT information level 
        /// by adding SMB_INFO_PASSTHROUGH to the value of the native Windows NT information level, and then passes 
        /// this in the  InformationLevel field in the client request. If the server supports the 
        /// CAP_INFOLEVEL_PASSTHRU  (the CAP_INFOLEVEL_PASSTHRU bit is set in the Capabilities field in the 
        /// SMB_COM_NEGOTIATE response),  the Data buffer in the server response is formatted with information based 
        /// on the native Windows  NT information level requested. The specific format of the data block in the server 
        /// response is  based on the native Windows NT information level, as specified in [MS-FSCC] section 2.4. 
        /// </summary>
        public const ushort CONST_SMB_INFO_PASSTHROUGH = 0x03e8;

        /// <summary>
        /// the count in bytes of byte
        /// </summary>
        public const int NUM_BYTES_OF_BYTE = 1;

        /// <summary>
        /// the count in bytes of WORD
        /// </summary>
        public const int NUM_BYTES_OF_WORD = 2;

        /// <summary>
        /// the count of two bytes alignment
        /// </summary>
        public const int TWO_BYTES_ALIGN = 2;

        /// <summary>
        /// the count of four bytes alignment
        /// </summary>
        public const int FOUR_BYTES_ALIGN = 4;

        #endregion

        #region Constructor

        /// <summary>
        /// constructor 
        /// </summary>
        protected SmbCapability()
        {
            this.IsCanonicalized = true;
            this.IsCaseInsensitive = true;
            this.IsKnowEAS = true;
            this.IsUnicode = true;
            this.IsNtStatus = true;
            this.IsReparsePath = true;
            this.IsSupportsDfs = true;
            this.IsSupportsExtendedSecurity = true;
        }


        #endregion
    }
}
