// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using CIFS = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;
using FSCC = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// Capture code for SMB Adapter.
    /// </summary>
    public partial class SmbAdapter : ISmbAdapter
    {
        #region Verify FileId Generation section 2.2.1.3.1.

        /// <summary>
        /// Verify FileId Generation. 
        /// </summary>
        /// <param name="fileId">The FileId to verify.</param>
        private void VerifyMessageSyntaxFileIdGeneration(byte[] fileId)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9059");

            //
            // Verify MS-SMB requirement: MS-SMB_R9059.
            //
            // 64-bit is 8 bytes.
            Site.CaptureRequirementIfAreEqual<int>(
                8,
                fileId.Length,
                9059,
                @"[In FileId Generation]The generation of FileIds MUST satisfy the following constraints:The FileId 
                MUST be a 64-bit opaque value.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R109064");

            // Possible values are valid, which means if this field exists, the value is valid.
            // Here capture this requirement directly.
            Site.CaptureRequirement(
                109064,
                @"[In FileId Generation]The generation of FileIds MUST satisfy the following constraints:possible 
                values for FileId are valid.");
        }

        #endregion

        #region Verify VolumeGUID Generation section 2.2.1.3.2.

        /// <summary>
        /// Verify VolumeGUID Generation.
        /// </summary>
        /// <param name="volumeGuid">A VolumeGUID to be verified.</param>
        /// Disable warning CA1801 because according to Test Case design, 
        /// volumeGuid are used for extension.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        private void VerifyMessageSyntaxVolumeGUIDGeneration(byte[] volumeGuid)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9066");

            // 1 byte is 8 bits
            Site.CaptureRequirementIfAreEqual<int>(
                128, volumeGuid.Length * 8, 9066,
                @"[In VolumeGUID Generation]The generation of VolumeGUIDs MUST satisfy the following 
                constraints:The VolumeGUID MUST be a 128-bit opaque value.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9067");

            // If the volumeGUID isn't unique, it will throw exception and the case will stop and won't come here.
            // So this requirement has been verified by the protocol SDK.
            Site.CaptureRequirement(
                9067,
                @"[In VolumeGUID Generation]The generation of VolumeGUIDs MUST satisfy the following constraints:The 
                VolumeGUID MUST be unique for a logical file system volume on a given server.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9060");

            // If the volumeGUID isn't unique, it will throw exception and the case will stop and won't come here.
            // So this requirement has been verified by the protocol SDK.
            Site.CaptureRequirement(
                9060,
                @"[In FileId Generation]The generation of FileIds MUST satisfy the following constraints:The FileId MUST be unique 
                for a file on a given object store.");


        }

        #endregion

        #region Verify Copychunk Resume Key Generation section 2.2.1.3.3.

        /// <summary>
        /// Verify Copychunk Resume Key Generation.
        /// </summary>
        /// <param name="copyChunkResumeKey">The Copychunk Resume Key to be verified.</param>
        private void VerifyMessageSyntaxCopychunkResumeKey(byte[] copyChunkResumeKey)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9072");

            //
            // Verify MS-SMB requirement: MS-SMB_R9072.
            //
            // The requirement is described as "The Copychunk Resume Key MUST be a 24-byte opaque value".
            // The requirement is verified if the byte length of copyChunkResumeKey is 24.
            // copyChunkResumeKey is defined as an array of byte, so the length of the array is the byte-length of 
            // copyChunkResumeKey.
            Site.CaptureRequirementIfAreEqual<int>(
                24,
                copyChunkResumeKey.Length,
                9072,
                @"[In Copychunk Resume Key Generation] The generation of Copychunk Resume Keys MUST satisfy the 
                following constraints:The Copychunk Resume Key MUST be a 24-byte opaque value generated by an SMB 
                server in response to a request by the client (an SMB_COM_NT_TRANSACTION request with an 
                NT_TRANSACT_IOCTL subcommand for the FSCTL_SRV_REQUEST_RESUME_KEY).");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9075");

            // All possible values for the Copychunk Resume Key are valid. It means if this field exists,
            // the value is valid. Here capture it directly.
            Site.CaptureRequirement(
                9075,
                @"[In Copychunk Resume Key Generation] The generation of Copychunk Resume Keys MUST satisfy the 
                following constraints:All possible values for the Copychunk Resume Key are valid.");
        }

        #endregion

        #region Verify Access Masks section 2.2.1.4.

        /// <summary>
        /// Verify Access Masks. 
        /// </summary>
        /// <param name="smbNtCreateAndxPacket">Packets for SmbNtCreateAndx Response.</param>
        /// <param name="isDirectoryFile">To judge whether it's the Directory File, if yes set to true, else set to 
        /// false.</param> 
        private void VerifyMessageSyntaxAccessMasks(
            SmbNtCreateAndxResponsePacket smbNtCreateAndxPacket,
            bool isDirectoryFile)
        {
            if (isDirectoryFile)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9102");

                //
                // Verify MS-SMB requirement: MS-SMB_R9102.
                //
                // DIRECTORY_ACCESS_MASK is the bit-or result of all the flags listed in this requirement.
                // If an asscess mask is used on a directory, then it is the combination of the flags listed in this 
                // requirement.
                // The bit-and result with DIRECTORY_ACCESS_MASK must be zero, otherwise, it means the bit not listed 
                // in the requirement was set, which doesn't fit the rule of Directory_Access_Mask defined in the TD
                // (Technical Document).
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    ~DirectoryAccessMask & BytesToUInt32(smbNtCreateAndxPacket.SmbParameters.MaximalAccessRights),
                    9102,
                    "[In Directory_Access_Mask]Directory_Access_Mask (4 bytes):For a directory, the value MUST be " +
                    "constructed using the following values:[" +
                    "FILE_LIST_DIRECTORY 0x00000001," +
                    "FILE_ADD_FILE 0x00000002," +
                    "FILE_ADD_SUBDIRECTORY 0x00000004," +
                    "FILE_READ_E 0x00000008," +
                    "FILE_WRITE_E 0x00000010," +
                    "FILE_TRAVER 0x00000020," +
                    "FILE_DELETE_CH 0x00000040," +
                    "FILE_READ_ATTRIBUTE 0x00000080," +
                    "FILE_WRITE_ATTRIBUTE 0x00000100," +
                    "DELET 0x00010000," +
                    "READ_CONTROL 0x00020000," +
                    "WRITE_DAC 0x00040000," +
                    "WRITE_OWNE 0x00080000," +
                    "SYNCHRONIZ 0x00100000," +
                    "ACCESS_SYSTEM_SECURIT 0x01000000," +
                    "MAXIMUM_ALLOWED 0x02000000," +
                    "GENERIC_ALL 0x10000000," +
                    "GENERIC_EXECUTE 0x20000000," +
                    "GENERIC_WRITE 0x40000000," +
                    "GENERIC_READ 0x80000000].");
            }
            else
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9080");
                //
                // Verify MS-SMB requirement: MS-SMB_R9080.
                //
                // The value MUST have at least one of FILE_WRITE_DATA, FILE_APPEND_DATA or GENERIC_WRITE being set.
                // According to TD , the value of the reserved bit is as following.
                const uint reservedBit = 64;

                // FilePipePrinterAccessMask is the bit-or result of all the flags listed in this RS's.
                // If a assess mask is used on a file, named pipe or printer, then it is the combination of the
                // flags listed in this RS, the bit-or result with FilePipePrinterAccessMask must be the same as 
                // FilePipePrinterAccessMask, otherwise, it means the bit not listed in the RS was set, 
                // it doesn't fit the rule of File_Pipe_Printer_Access_Mask defined in the TD.
                // and according to TD , the the reserved bit should be ignored.
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    ~FilePipePrinterAcess
                    & ~reservedBit
                    & BytesToUInt32(smbNtCreateAndxPacket.SmbParameters.MaximalAccessRights),
                    9080,
                    "[In File_Pipe_Printer_Access_Mask]File_Pipe_Printer_Access_Mask (4 bytes):  For a file, named " +
                    "pipe, or printer, the value MUST be constructed using the following values [" +
                    "FILE_READ_DATA 0x00000001," +
                    "FILE_WRITE_DATA 0x00000002," +
                    "FILE_APPEND_DATA 0x00000004," +
                    "FILE_READ_EA 0x00000008," +
                    "FILE_WRITE_EA 0x00000010," +
                    "FILE_EXECU 0x00000020," +
                    "FILE_READ_ATTRIBUTES 0x00000080," +
                    "FILE_WRITE_ATTRIBUTES 0x00000100," +
                    "DELET 0x00010000," +
                    "READ_CONTROL 0x00020000," +
                    "WRITE_DAC 0x00040000," +
                    "WRITE_OWNER 0x00080000," +
                    "SYNCHRONIZE 0x00100000," +
                    "ACCESS_SYSTEM_SECURITY 0x01000000," +
                    "MAXIMUM_ALLOWED 0x02000000," +
                    "GENERIC_AL 0x10000000," +
                    "GENERIC_EXECUTE 0x20000000," +
                    "GENERIC_WRITE 0x40000000," +
                    "GENERIC_READ 0x80000000]");
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R078");

            // For the Access Masks, it is the combination of the values listed in R9102.
            // If this RS failed, R9102 will not pass. This requirement has been verified in the R9102 logic also.
            Site.CaptureRequirement(
                9078,
                @"[In Access Masks]Each access mask MUST be a combination of zero or more of the bit positions.");
        }

        /// <summary>
        /// Verify Access Masks.
        /// </summary>
        /// <param name="smbTreeConnectAndxPacket">Packets for SmbTreeConnectAndxPacket Response.</param>
        /// <param name="shareType">To judge if the share type is the Printer</param>
        private void VerifyMessageSystaxAccessMasksForPrinter(
            SmbTreeConnectAndxResponsePacket smbTreeConnectAndxPacket,
            ShareType shareType)
        {
            if (shareType == ShareType.Printer)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R109080");
                //
                // Verify MS-SMB requirement: MS-SMB_R109080.
                //
                bool isContainFILE_WRITE_DATA = (smbTreeConnectAndxPacket.SmbParameters.MaximalShareAccessRights &
                    (uint)CIFS.NtTransactDesiredAccess.FILE_WRITE_DATA) == (uint)CIFS.NtTransactDesiredAccess.FILE_WRITE_DATA;
                bool isContainFile_APPEND_DATA = (smbTreeConnectAndxPacket.SmbParameters.MaximalShareAccessRights &
                    (uint)CIFS.NtTransactDesiredAccess.FILE_APPEND_DATA) == (uint)CIFS.NtTransactDesiredAccess.FILE_APPEND_DATA;
                bool isContainGENERIC_WRITE = (smbTreeConnectAndxPacket.SmbParameters.MaximalShareAccessRights &
                    (uint)CIFS.NtTransactDesiredAccess.GENERIC_WRITE) == (uint)CIFS.NtTransactDesiredAccess.GENERIC_WRITE;
                bool isContainValue = isContainFILE_WRITE_DATA || isContainFile_APPEND_DATA || isContainGENERIC_WRITE;

                // If the value has at least one of the following: FILE_WRITE_DATA, FILE_APPEND_DATA, or GENERIC_WRITE,
                // then the isContainValue equals true, this requirement is captured.
                Site.CaptureRequirementIfIsTrue(
                    isContainValue,
                    109080,
                    "[In File_Pipe_Printer_Access_Mask]File_Pipe_Printer_Access_Mask (4 bytes): For a printer, the value MUST have at least one of the following: FILE_WRITE_DATA, FILE_APPEND_DATA, or GENERIC_WRITE.");
            }
        }

        #endregion

        #region Verify SMB Header Extensions section 2.2.3.1.

        /// <summary>
        /// Verify SMB Header Extensions.
        /// </summary>
        /// <param name="smbHeader">The SMB Header.</param>
        /// <param name="isNtManagerNegotiated">If NT LAN Manager or later is negotiated for the SMB dialect.</param>
        /// <param name="isExtendedSecuritySupported">
        /// If the client or the SUT supports extended security.
        /// </param>
        /// <param name="isPathContainsLongNames">If the path contained in the message contains long names.</param>
        private void VerifyMessageSyntaxSMBHeaderExtension(
            CIFS.SmbHeader smbHeader,
            bool isNtManagerNegotiated,
            bool isExtendedSecuritySupported,
            bool isPathContainsLongNames)
        {
            // isNTManagerNegotiated means if NT LAN Manager or later is negotiated for the SMB dialect.
            if (isNtManagerNegotiated)
            {
                //
                // Verify requirement MS-SMB_R134 and MS-SMB_R135
                //
                string isR134Implementated = Site.Properties.Get("SHOULDMAYR134Implementation");
                bool isR135Satisfied = ((uint)SmbHeader_Flags2_Values.SMB_FLAGS2_REPARSE_PATH
                    == ((uint)smbHeader.Flags2 & (uint)SmbHeader_Flags2_Values.SMB_FLAGS2_REPARSE_PATH));

                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R135,SmbHeader.Flags2:{0}",
                        (uint)smbHeader.Flags2);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R135.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR135Satisfied,
                        135,
                        @"[In SMB Header Extensions]This bit field[Flags2: SMB_FLAGS2_REPARSE_PATH 0x0400]is set to one 
                        only when NT LAN Manager or later is negotiated for the SMB dialect.");

                    if (null == isR134Implementated)
                    {
                        Site.Properties.Add("SHOULDMAYR134Implementation", Boolean.TrueString);
                        isR134Implementated = Boolean.TrueString;
                    }
                }

                if (null != isR134Implementated)
                {
                    bool implemented = Boolean.Parse(isR134Implementated);
                    bool isSatisfied = isR135Satisfied;

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R134,SmbHeader.Flags2:{0}",
                        (uint)smbHeader.Flags2);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R134.
                    //
                    Site.CaptureRequirementIfAreEqual<Boolean>(
                        implemented,
                        isSatisfied,
                        134,
                        String.Format("[In SMB Header Extensions]This bit field[Flags2:SMB_FLAGS2_REPARSE_PATH 0x0400] " +
                        "SHOULD be set to one only when NT LAN Manager or later is negotiated for the SMB dialect. This " +
                        "requirement is {0}implemented", implemented ? "" : "not "));
                }

                // isExtendedSecuritySupported means if the client or the SUT supports extended security.
                if (isExtendedSecuritySupported)
                {
                    //
                    // Verify requirement MS-SMB_R138 and MS-SMB_R139
                    //
                    string isR138Implementated = Site.Properties.Get("SHOULDMAYR138Implementation");
                    bool isR139Satisfied = ((uint)SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY
                        == ((uint)smbHeader.Flags2 & (uint)SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY));

                    if (isWindows)
                    {
                        //
                        // The following statement code will be run only when debugging.
                        //
                        Site.Log.Add(LogEntryKind.Debug,
                            @"Verify MS-SMB_R138,SmbHeader.Flags2:{0}",
                            (uint)smbHeader.Flags2);

                        //
                        // Verify MS-SMB requirement: MS-SMB_R139.
                        //
                        Site.CaptureRequirementIfIsTrue(
                            isR139Satisfied,
                            139,
                            @"[In SMB Header Extensions]This bit field[Flags2: SMB_FLAGS2_EXTENDED_SECURITY 0x0800] is 
                            set to onewhen NTLM 0.12 or later is negotiated for the SMB dialect dialect and the client 
                            or server supports extended security  in Windows .");

                        if (null == isR138Implementated)
                        {
                            Site.Properties.Add("SHOULDMAYR138Implementation", Boolean.TrueString);
                            isR138Implementated = Boolean.TrueString;
                        }
                    }

                    if (null != isR138Implementated)
                    {
                        bool implemented = Boolean.Parse(isR138Implementated);
                        bool isSatisfied = isR139Satisfied;

                        //
                        // The following statement code will be run only when debugging.
                        //
                        Site.Log.Add(LogEntryKind.Debug,
                            @"Verify MS-SMB_R138,SmbHeader.Flags2:{0}",
                            (uint)smbHeader.Flags2);

                        //
                        // Verify MS-SMB requirement: MS-SMB_R138.
                        //
                        Site.CaptureRequirementIfAreEqual<Boolean>(
                            implemented,
                            isSatisfied,
                            138,
                            String.Format("[In SMB Header Extensions]This bit field" +
                            "[Flags2:SMB_FLAGS2_EXTENDED_SECURITY 0x0800] SHOULD be set to one only when NTLM 0.12 or " +
                            "later is negotiated for the SMB dialect dialect and the client or server supports extended " +
                            "security. This requirement is {0}implemented", implemented ? "" : "not "));
                    }
                }
            }

            if (((uint)smbHeader.Flags2 & (uint)SmbHeader_Flags2_Values.SMB_FLAGS2_IS_LONG_NAME) ==
                 (uint)SmbHeader_Flags2_Values.SMB_FLAGS2_IS_LONG_NAME)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R5300,Flags2:{0}",
                    (uint)smbHeader.Flags2);

                //
                // Verify MS-SMB requirement: MS-SMB_R5300.
                //
                // isPathContainsLongNames means if the path contained in the message contains long names
                // [larger than  8.3 names].
                Site.CaptureRequirementIfIsTrue(
                    isPathContainsLongNames,
                    5300,
                    @"[In SMB Header Extensions]If [Flags2: SMB_FLAGS2_IS_LONG_NAME 0x0040] set, the path contained in 
                    the message[negotiated SMB dialect] contains long names[larger than 8.3 names].");
            }
            else
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R5301,Flags2:{0}",
                    (uint)smbHeader.Flags2);

                //
                // Verify MS-SMB requirement: MS-SMB_R5301.
                //
                // isPathContainsLongNames means if the path contained in the message contains long names
                // [larger than 8.3 names].
                Site.CaptureRequirementIfIsFalse(
                    isPathContainsLongNames,
                    5301,
                    @"[In SMB Header Extensions]otherwise[if Flags2: SMB_FLAGS2_IS_LONG_NAME 0x0040 is not set], the 
                    paths are restricted to 8.3 names.");
            }
        }

        #endregion

        #region Verify SMB_COM_WRITE_ANDX Server Response section 2.2.4.3.2.

        /// <summary>
        /// Verify SMB_COM_WRITE_ANDX Server Response.
        /// </summary>
        /// <param name="response">The SMB_COM_WRITE_ANDX Server Response.</param>
        private void VerifyMessageSyntaxSmbComWriteAndXResponse(SmbWriteAndxResponsePacket response)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9944,Reserved:{0}",
                response.SmbParameters.Reserved);

            //
            // Verify MS-SMB requirement: MS-SMB_R9944.
            //
            bool isVerifyR9944 = (2 == Marshal.SizeOf(response.SmbParameters.Reserved))
                && (0 == response.SmbParameters.Reserved);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR9944,
                9944,
                @"[In Server Response Extensions]Reserved (2 bytes):  Servers MUST set this field to zero.");
        }

        #endregion

        #region Verify SMB_COM_NEGOTIATE Extended Security Server Response section 2.2.4.5.2.1.

        /// <summary>
        /// Verify SMB_COM_NEGOTIATE Extended Security Server Response.
        /// </summary>
        /// <param name="response">The SMB_COM_NEGOTIATE Extended Security Server Response.</param>
        /// <param name="isTokenConfiguredToUsed">
        /// If the first GSS token (or fragment thereof) produced by the GSS authentication protocol is 
        /// configured to be used by the SUT.
        /// </param>
        /// <param name="isReAuthentSupported">If dynamic re-authentication is supported.</param>
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        /// Disable warning CA1801 because according to Test Case design, 
        /// the parameter isTokenConfiguredToUsed is used for extension.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        private void VerifyMessageSyntaxSmbComNegotiateExtendedSecurityServerResponse(
            SmbNegotiateResponsePacket response,
            bool isTokenConfiguredToUsed,
            bool isReAuthentSupported)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9962");

            //
            // Verify MS-SMB requirement: MS-SMB_R9962.
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                0,
                (uint)(~CapabilitiesAllSet & response.SmbParameters.Capabilities),
                9962,
                @"[In Extended Security Response]Capabilities (4 bytes): The server MUST set the unused bits to zero.");

            // CAP_COMPRESSED_DATA, CAP_DYNAMIC_REAUTH, CAP_EXTENDED_SECURITY, CAP_INFOLEVEL_PASSTHRU, CAP_LARGE_WRITE, 
            // CAP_LWIO, CAP_UNIX are the new capabilities.
            if (((response.SmbParameters.Capabilities
                & Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_COMPRESSED_DATA)
                == Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_COMPRESSED_DATA)
                || ((response.SmbParameters.Capabilities
                & Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_DYNAMIC_REAUTH)
                == Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_DYNAMIC_REAUTH)
                || ((response.SmbParameters.Capabilities
                & Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_EXTENDED_SECURITY)
                == Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_EXTENDED_SECURITY)
                || ((response.SmbParameters.Capabilities
                & Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_INFOLEVEL_PASSTHRU)
                == Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_INFOLEVEL_PASSTHRU)
                || ((response.SmbParameters.Capabilities
                & Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_LARGE_WRITE)
                == Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_LARGE_WRITE)
                || ((response.SmbParameters.Capabilities
                & Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_LWIO)
                == Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_LWIO)
                || ((response.SmbParameters.Capabilities
                & Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_UNIX)
                == Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_UNIX))
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R8558");

                // The condition is verified if the new capabilities is used, If at least one of them was used, 
                // it means new capabilities are considered. and the requirement can be captured directly. 
                // And setting the SMB_Parameters.Words.Capabilities field of the response based on the server under 
                // test. Capabilities attribute is server internal behavior.
                Site.CaptureRequirement(
                    8558,
                    @"[In Receiving an SMB_COM_NEGOTIATE Request]New Capabilities: The new capabilities flags specified 
                    in section 2.2.4.5.1 MUST also be considered when setting the  SMB_Parameters.Words.Capabilities 
                    field of the response based on the Server.Capabilities attribute.");
            }

            if ((Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_EXTENDED_SECURITY
                & response.SmbParameters.Capabilities)
                == Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_EXTENDED_SECURITY)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R239");

                //
                // Verify MS-SMB requirement: MS-SMB_R239.
                //
                Site.CaptureRequirementIfAreEqual<byte>(
                    0,
                    response.SmbParameters.EncryptionKeyLength,
                    239,
                    @"ChallengeLength (1 byte):  When the CAP_EXTENDED_SECURITY bit is set, the server MUST set this 
                    value to zero.");
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R5426:ServerGuid:{0}",
                response.SmbData.ServerGuid);

            //
            // Verify MS-SMB requirement: MS-SMB_R5426.
            //
            // Whether a field is unique or not couldn't be verified. Here verify this requirement partially, only 
            // verify the type of the field.
            bool isVerifyR5426 = (typeof(Guid) == response.SmbData.ServerGuid.GetType());

            Site.CaptureRequirementIfIsTrue(
                isVerifyR5426,
                5426,
                @"[In Extended Security Response]ServerGUID (16 bytes):This field MUST be a GUID generated by the 
                server to uniquely identify this server. ");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R241,ByteCount:{0}",
                response.SmbData.ByteCount);

            //
            // Verify MS-SMB requirement: MS-SMB_R241.
            //
            bool isVerifyR241 = ((2 == Marshal.SizeOf(response.SmbData.ByteCount))
                && (response.SmbData.ByteCount >= 0x0010));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR241,
                241,
                @"[In Extended Security Response]ByteCount (2 bytes):This field MUST be greater than or equal to 
                0x0010.");

            //
            // Verify requirement MS-SMB_R5430 and MS-SMB_R105430
            //
            string isR5430Implementated = Site.Properties.Get("SHOULDMAYR5430Implementation");

            // The SecurityBlob is an array of bytes, if the length of the array is greater than 0, it means the 
            // SecurityBlob is contained in the response.
            bool isR105430Satisfied = response.SmbData.SecurityBlob.Length > 0;

            if (isWindows && (sutOsVersion != Platform.Win2K))
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R105430,SecurityBlob.Length:{0}",
                    response.SmbData.SecurityBlob.Length);

                //
                // Verify MS-SMB requirement: MS-SMB_R105430.
                //
                Site.CaptureRequirementIfIsTrue(
                    isR105430Satisfied,
                    105430,
                    @"[In Extended Security Response]SecurityBlob (variable):A security binary large object (BLOB) that 
                    contains an authentication token as produced by the GSS protocol (as specified in section 3.2.4.2.4 
                    and [RFC2743]) in Windows(except windows 2000).<40>");

                if (null == isR5430Implementated)
                {
                    Site.Properties.Add("SHOULDMAYR5430Implementation", Boolean.TrueString);
                    isR5430Implementated = Boolean.TrueString;
                }
            }

            if (null != isR5430Implementated)
            {
                bool implemented = Boolean.Parse(isR5430Implementated);
                bool isSatisfied = isR105430Satisfied;

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R5430,SecurityBlob.Length:{0}",
                    response.SmbData.SecurityBlob.Length);

                //
                // Verify MS-SMB requirement: MS-SMB_R5430.
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implemented,
                    isSatisfied,
                    5430,
                    String.Format("[In Extended Security Response]SecurityBlob (variable):A security binary large " +
                    "object (BLOB) that SHOULD contain an authentication token as produced by the GSS protocol (as " +
                    "specified in section 3.2.4.2.4 and [RFC2743]).<41> This requirement is {0}" +
                    "implemented", implemented ? "" : "not "));
            }

            if ((sutOsVersion == Platform.Win2K3) ||
                (sutOsVersion == Platform.Win2K3R2) ||
                (sutOsVersion == Platform.WinVista) ||
                (sutOsVersion == Platform.Win2K8) ||
                (sutOsVersion == Platform.Win7) ||
                (sutOsVersion == Platform.Win2K8R2))
            {
                if ((response.SmbParameters.Capabilities
                    & Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_LWIO)
                    == Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_LWIO)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R406932");

                    // If the OS is one of Win2K3, Win2K3R2, WinVista, Win2K8, Win7 or Win2K8R2, and the CAP_LWIO flag 
                    // of Capabilities field of SmbParameter in the response is set, this requirement is covered. So 
                    // here capture the requirement directly.
                    Site.CaptureRequirement(
                        406932,
                        @"<34> Section 2.2.4.5.2.1: Windows Server 2003, Windows Server 2003 R2, Windows Vista, Windows 
                        Server 2008, Windows 7, and Windows Server 2008 R2 clients and servers support CAP_LWIO.");
                }
            }
            if (isWindows)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R6932");

                //
                // Verify MS-SMB requirement: MS-SMB_R6932.
                //
                Site.CaptureRequirementIfAreEqual<
                    Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities>(
                    0,
                    Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_COMPRESSED_DATA
                    & response.SmbParameters.Capabilities,
                    6932,
                    @"<36> Section 2.2.4.5.2.1: Windows-based clients and servers do not support 
                    CAP_COMPRESSED_DATA and this capability is never set.");

                // If isReAuthentSupported is true, it means dynamic re-authentication is supported.
                if (isReAuthentSupported)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R6934");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R6934.
                    //
                    Site.CaptureRequirementIfAreEqual<
                        Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities>(
                        0x00000000,
                        Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_DYNAMIC_REAUTH
                        & response.SmbParameters.Capabilities,
                        6934,
                        @"<37> Section 2.2.4.5.2.1: Windows servers do not set the CAP_DYNAMIC_REAUTH flag even
                        if dynamic re-authentication is supported.");
                }
            }

            if ((response.SmbParameters.Capabilities
                & Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_EXTENDED_SECURITY)
                == Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_EXTENDED_SECURITY)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R2172");

                //
                // Verify MS-SMB requirement: MS-SMB_R2172.
                //
                // SmbNegotiateResponsePacket is the packet specified in section 2.2.4.5.2.
                Site.CaptureRequirementIfAreEqual<Type>(
                    typeof(SmbNegotiateResponsePacket),
                    response.GetType(),
                    2172,
                    @"[In Receiving an SMB_COM_NEGOTIATE Response]Storing extended security token: If the capabilities
                    returned in the SMB_COM_NEGOTIATE response  include CAP_EXTENDED_SECURITY, then the response MUST 
                    take the form defined in section 2.2.4.5.2).");
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9952");

            // The response's form have been verified in R239, R241, R5430, R2172. Here capture it directly.
            Site.CaptureRequirement(
                9952,
                "[In Extended Security Response] If the selected dialect is NT LAN Manager and the client has " +
                "indicated extended security is being used, a successful response MUST take the following form " +
                "[SMB_Parameters" +
                  "{" +
                  "UCHAR  WordCount;" +
                  "Words" +
                    "{" +
                    "USHORT   DialectIndex;" +
                    "UCHAR    SecurityMode;" +
                    "USHORT   MaxMpxCount;" +
                    "USHORT   MaxNumberVcs;" +
                    "ULONG    MaxBufferSize;" +
                    "ULONG    MaxRawSize;" +
                    "ULONG    SessionKey;" +
                    "ULONG    Capabilities;" +
                    "FILETIME SystemTime;" +
                    "SHORT    ServerTimeZone;" +
                    "UCHAR    ChallengeLength;" +
                    "}" +
                  "}" +
                "SMB_Data" +
                  "{" +
                  "USHORT ByteCount;" +
                  "Bytes" +
                    "{" +
                    "USHORT ServerGUID;" +
                    "UCHAR  SecurityBlob[];" +
                    "}" +
                  "}" +
                "].");

            // Win7, WinVista, WinXP is client versions of Windows.
            if ((sutOsVersion == Platform.Win7) ||
               (sutOsVersion == Platform.WinVista) ||
               (sutOsVersion == Platform.WinXP))
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R6895");

                //
                // Verify MS-SMB requirement: MS-SMB_R6895.
                //
                Site.CaptureRequirementIfAreEqual<uint>(
                    4356,
                    response.SmbParameters.MaxBufferSize,
                    6895,
                    @"<27> Section 2.2.4.5.2.1: Windows defaults to a MaxBufferSize value of 4,356 bytes on client 
                    versions of Windows.");
            }

            // Win2K, Win2K3,Win2k3R2,Win2K8,Win2K8R2 is server version of Windows.
            if (isWindows &&
                ((sutOsVersion == Platform.Win2K) ||
                (sutOsVersion == Platform.Win2K3) ||
                (sutOsVersion == Platform.Win2K3R2) ||
                (sutOsVersion == Platform.Win2K8) ||
                (sutOsVersion == Platform.Win2K8R2)))
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R6894");

                //
                // Verify MS-SMB requirement: MS-SMB_R6894.
                //
                Site.CaptureRequirementIfAreEqual<uint>(
                    16644,
                    response.SmbParameters.MaxBufferSize,
                    6894,
                    @"<27> Section 2.2.4.5.2.1: Windows defaults to a MaxBufferSize value of 16,644 bytes on server 
                    versions of Windows.");
            }
            if (isWindows)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R8573");

                // The parameter of this method is an extended security response from the SUT, if the os 
                // is windows-based, it means that Windows-based SMB servers support Extended Security. Here capture 
                // this requirement directly.
                Site.CaptureRequirement(
                    8573,
                    @"[In Receiving an SMB_COM_NEGOTIATE Response] <104> Section 3.2.5.2: Windows-based SMB servers 
                    support Extended Security. ");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R11004");

                //
                // Verify MS-SMB requirement: MS-SMB_R11004.
                //
                Site.CaptureRequirementIfAreEqual<
                    Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities>(
                    0,
                    response.SmbParameters.Capabilities & Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.
                    Capabilities.CAP_UNIX,
                    11004,
                    @"<35> Section 2.2.4.5.2.1: Windows-based clients and servers do not support CAP_UNIX; 
                    therefore, this capability is never set.");
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R2309");

            // If R9952 has been verified, this requirement is verified. Since the form specified in this 
            // requirement is of SmbNegotiateResponsePacket type.
            Site.CaptureRequirement(
                2309,
                @"[In Receiving an SMB_COM_NEGOTIATE Request] Generating Extended Security Token: The response
                [SMB_COM_NEGOTIATE response] MUST take the form specified in section  2.2.4.1.2.  ");

            if (((Capabilities)response.SmbParameters.Capabilities & Capabilities.CapNtSmbs)
                == Capabilities.CapNtSmbs)
            {
                //
                // Verify requirement MS-SMB_R9975 and MS-SMB_R106919
                //
                string isR9975Implementated = Site.Properties.Get("SHOULDMAYR9975Implementation");
                bool isR106919Satisfied = (((Capabilities)response.SmbParameters.Capabilities
                    & Capabilities.CapNtFind)
                    == Capabilities.CapNtFind);

                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R106919,Capabilities:{0}",
                        (uint)response.SmbParameters.Capabilities);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R106919.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR106919Satisfied,
                        106919,
                        @"<30> Section 2.2.4.5.2.1: Windows-based clients assume that CAP_NT_FIND is set if CAP_NT_SMBS 
                        is set.");
                    if (null == isR9975Implementated)
                    {
                        Site.Properties.Add("SHOULDMAYR9975Implementation", Boolean.TrueString);
                        isR9975Implementated = Boolean.TrueString;
                    }
                }

                if (null != isR9975Implementated)
                {
                    bool implemented = Boolean.Parse(isR9975Implementated);
                    bool isSatisfied = isR106919Satisfied;

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R9975,Capabilities:{0}",
                        (uint)response.SmbParameters.Capabilities);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R9975.
                    //
                    Site.CaptureRequirementIfAreEqual<Boolean>(
                        implemented,
                        isSatisfied,
                        9975,
                        String.Format("[In Extended Security Response]CAP_NT_FIND 0x00000200:This bit SHOULD<30> be " +
                        "set if CAP_NT_SMBS is set. This requirement is {0}implemented", implemented ? "" : "not "));
                }
            }

            if ((sutOsVersion == Platform.Win2K) ||
               (sutOsVersion == Platform.Win2K3) ||
               (sutOsVersion == Platform.Win2K3R2) ||
               (sutOsVersion == Platform.Win2K8) ||
               (sutOsVersion == Platform.Win2K8R2))
            {
                if (response.SmbData.SecurityBlob.Length >= 0)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R4748");

                    // Protocol SDK have ensured to encode SecurityBlob as GSS token. Here capture this requirement 
                    // directly.
                    Site.CaptureRequirement(
                        4748,
                        @"[In Receiving an SMB_COM_NEGOTIATE Request]  <112>Section 3.3.5.2: Windows-based SMB 
                        serverssupport Extended Security, and  are configured to use SPNEGO (as specified in [RFC4178]) 
                        as their GSS authentication protocol.");

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R4749");

                    // The protocol SDK has ensured to encode SecurityBlob as GSS token. Here capture this requirement 
                    // directly.
                    Site.CaptureRequirement(
                        4749,
                        @"[In Receiving an SMB_COM_NEGOTIATE Request] <112>Section 3.3.5.2: Windows operating systems 
                        that use extended security send a GSS token (or fragment) if their SPNEGO implementation 
                        supports it.");
                }
            }
        }

        #endregion

        #region Verify SMB_COM_NEGOTIATE  Non-Extended Security Server Response section 2.2.4.5.2.2.

        /// <summary>
        /// Verify SMB_COM_NEGOTIATE Non-Extended Security Server Response.
        /// </summary>
        /// <param name="response">The SMB_COM_NEGOTIATE Non-Extended Security Server Response.</param>
        /// <param name="isAuthenticationSupported">
        /// Whether the SUT supports challenge/response authentication or not.
        /// </param>
        /// Disable warning CA1801 because according to Test Case design, 
        /// the parameter isAuthenticationSupported is used for extension.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        private void VerifyMessageSyntaxSmbComNegotiateNonExtendedSecurityServerResponse(
            SmbNegotiateImplicitNtlmResponsePacket response,
            bool isAuthenticationSupported)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9212");

            //
            // Verify MS-SMB requirement: MS-SMB_R9212.
            //
            // "The server MUST set the unused bits to zero in a response." will be verified in R109215 and R209215.
            // Here only verify the capabilities's byte-length.
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf((uint)response.SmbParameters.Capabilities),
                9212,
                @"[In Non-Extended Security Response]Capabilities (4 bytes):  The server MUST set the unused bits to 
                zero in a response.");

            //
            // Verify requirement MS-SMB_R109215 and MS-SMB_R209215
            //
            // The 4bytes have been verified in R9212, here only verify whether the unused bits were set to zero or not.
            string isR109215Implementated = Site.Properties.Get("SHOULDMAYR109215Implementation");

            // All the flags of Capabilities of SMB are listed in the following equation.
            Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities capabilitiesAllSet
                = (Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_RAW_MODE
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_MPX_MODE
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_UNICODE
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_LARGE_FILES
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_NT_SMBS
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_RPC_REMOTE_APIS
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_STATUS32
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_LEVEL_II_OPLOCKS
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_LOCK_AND_READ
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_NT_FIND
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_DFS
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_INFOLEVEL_PASSTHRU
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_LARGE_READX
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_LARGE_WRITE
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_LWIO
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_UNIX
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_COMPRESSED_DATA
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_DYNAMIC_REAUTH
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_PERSISTENT_HANDLES
                | Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb.Capabilities.CAP_EXTENDED_SECURITY);
            bool isR209215Satisfied = (0 == ((uint)~capabilitiesAllSet & (uint)response.SmbParameters.Capabilities));
            if (isWindows)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                     @"Verify MS-SMB_R209215,Capabilities:{0}",
                     (uint)response.SmbParameters.Capabilities);

                //
                // Verify MS-SMB requirement: MS-SMB_R209215.
                //
                Site.CaptureRequirementIfIsTrue(
                    isR209215Satisfied,
                    209215,
                    @"[In Non-Extended Security Response]Capabilities (4 bytes): value not listed in the following 
                    table is unused in Windows.");

                if (null == isR109215Implementated)
                {
                    Site.Properties.Add("SHOULDMAYR109215Implementation", Boolean.TrueString);
                    isR109215Implementated = Boolean.TrueString;
                }
            }

            if (null != isR109215Implementated)
            {
                bool implemented = Boolean.Parse(isR109215Implementated);
                bool isSatisfied = isR209215Satisfied;

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R109215,Capabilities:{0}",
                    (uint)response.SmbParameters.Capabilities);

                //
                // Verify MS-SMB requirement: MS-SMB_R109215.
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implemented,
                    isSatisfied,
                    109215,
                    String.Format("[In Non-Extended Security Response]Capabilities (4 bytes): value not listed in the " +
                    "following table SHOULD be unused. This requirement is {0}implemented", implemented ? "" : "not "));
            }

            //
            // Verify requirement MS-SMB_R105227 and MS-SMB_R205227
            //
            // Here verify R105227 and R205227 partially, only verify whether the unused bits are set to zero or not.
            string isR105227Implementated = Site.Properties.Get("SHOULDMAYR105227Implementation");
            bool isR205227Satisfied = (0 == ((uint)~capabilitiesAllSet & (uint)response.SmbParameters.Capabilities));

            if (isWindows)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R205227,Capabilities:{0}",
                    (uint)response.SmbParameters.Capabilities);

                //
                // Verify MS-SMB requirement: MS-SMB_R205227.
                //
                Site.CaptureRequirementIfIsTrue(
                    isR205227Satisfied,
                    205227,
                    @"[In Message Syntax]Unless otherwise noted, unused or reserved bits in bit fields is set to zero 
                    when being sent in Windows.");

                if (null == isR105227Implementated)
                {
                    Site.Properties.Add("SHOULDMAYR105227Implementation", Boolean.TrueString);
                    isR105227Implementated = Boolean.TrueString;
                }
            }

            if (null != isR105227Implementated)
            {
                bool implemented = Boolean.Parse(isR105227Implementated);
                bool isSatisfied = isR205227Satisfied;

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R105227,Capabilities:{0}",
                    (uint)response.SmbParameters.Capabilities);

                //
                // Verify MS-SMB requirement: MS-SMB_R105227.
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implemented,
                    isSatisfied,
                    105227,
                    String.Format("[In Message Syntax]Unless otherwise noted, unused or reserved bits in bit fields " +
                    "SHOULD be set to zero when being sent. This requirement is {0}implemented", implemented ? "" : "not "));
            }

            //
            // Verify requirement MS-SMB_R109216 and MS-SMB_R9216
            //
            // The Capabilities's byte length has been verified in R9212.
            string isR9216Implementated = Site.Properties.Get("SHOULDMAYR9216Implementation");
            bool isR109216Satisfied = (0 == ((uint)~capabilitiesAllSet & (uint)response.SmbParameters.Capabilities));
            if (isWindows)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R109216,Capabilities:{0}",
                    (uint)response.SmbParameters.Capabilities);

                //
                // Verify MS-SMB requirement: MS-SMB_R109216.
                //
                Site.CaptureRequirementIfIsTrue(
                    isR109216Satisfied,
                    109216,
                    @"[In Non-Extended Security Response]Capabilities (4 bytes):  A server is set the unused bits to 
                    zero in a response in Windows.");

                if (null == isR9216Implementated)
                {
                    Site.Properties.Add("SHOULDMAYR9216Implementation", Boolean.TrueString);
                    isR9216Implementated = Boolean.TrueString;
                }
            }

            if (null != isR9216Implementated)
            {
                bool implemented = Boolean.Parse(isR9216Implementated);
                bool isSatisfied = isR109216Satisfied;

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9216,Capabilities:{0}",
                    (uint)response.SmbParameters.Capabilities);

                //
                // Verify MS-SMB requirement: MS-SMB_R9216.
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implemented,
                    isSatisfied,
                    9216,
                    String.Format("[In Non-Extended Security Response]Capabilities (4 bytes):  A server SHOULD set the " +
                    "unused bits to zero in a response. This requirement is {0}implemented", implemented ? "" : "not "));
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9219,ChallengeLength:{0}",
                response.SmbParameters.ChallengeLength);

            //
            // Verify MS-SMB requirement: MS-SMB_R9219.
            bool isVerifyR9219 = ((1 == Marshal.SizeOf(response.SmbParameters.ChallengeLength))
                && (0x08 == response.SmbParameters.ChallengeLength));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR9219,
                9219,
                @"[In Non-Extended Security Response] ChallengeLength (1 byte):  The value of this field MUST be 0x08 and is the length of the random challenge used in challenge/response authentication.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9222, ByteCound:{0}",
                response.SmbData.ByteCount);

            //
            // Verify MS-SMB requirement: MS-SMB_R9222.
            //
            bool isVerifyR9222 = ((2 == Marshal.SizeOf(response.SmbData.ByteCount))
                && (response.SmbData.ByteCount >= 0x0003));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR9222,
                9222,
                @"[In Non-Extended Security Response] ByteCount (2 bytes):  This field MUST be greater than or equal to 
                0x0003.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9223");

            //
            // Verify MS-SMB requirement: MS-SMB_R9223.
            //
            Site.CaptureRequirementIfAreEqual<int>(
                response.SmbParameters.ChallengeLength,
                response.SmbData.Challenge.Length,
                9223,
                @"[In Non-Extended Security Response] Challenge (variable):  An array of unsigned bytes that MUST be 
                the length of the number of bytes specified in the ChallengeLength field.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9224");

            //
            // Verify MS-SMB requirement: MS-SMB_R9224.
            //
            // Whether the byte is unsigned or not is decided by the protocol SDK's decoding method.
            // The protocol SDK has ensured that they are unsigned bytes.
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(byte[]),
                response.SmbData.Challenge.GetType(),
                9224,
                @"[In Non-Extended Security Response] Challenge (variable):  An array of unsigned bytes MUST represent 
                the server challenge.");

            if (isWindows &&
                ((sutOsVersion == Platform.Win2K) ||
                (sutOsVersion == Platform.Win2K3) ||
                (sutOsVersion == Platform.Win2K3R2) ||
                (sutOsVersion == Platform.Win2K8) ||
                (sutOsVersion == Platform.Win2K8R2)))
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9538");

                //
                // Verify MS-SMB requirement: MS-SMB_R9538.
                //
                Site.CaptureRequirementIfAreEqual<uint>(
                    16644,
                    response.SmbParameters.MaxBufferSize,
                    9538,
                    @"<41> Section 2.2.4.5.2.2: Windows-based servers default to a MaxBufferSize value of 
                    16,644 bytes.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9541");

                //
                // Verify MS-SMB requirement: MS-SMB_R9541.
                //
                Site.CaptureRequirementIfAreEqual<int>(
                    8,
                    response.SmbData.Challenge.Length,
                    9541,
                    @"<42> Section 2.2.4.5.2.2: Windows-based servers provide 8-bit cryptographic challenges.");
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9200");

            // The response's form is verified in the R9212, R9219, R9222, R9223, R9224. Here capture this requirement 
            // directly.
            Site.CaptureRequirement(
                9200,
                @"[In Non-Extended Security Response]If extended security is not being used and the NT LAN Manager 
                dialect has been selected, then a successful response MUST take the following form
                [SMB_Parameters
                  {
                  UCHAR  WordCount;
                  Words
                    {
                    USHORT   DialectIndex;
                    UCHAR    SecurityMode;
                    USHORT   MaxMpxCount;
                    USHORT   MaxNumberVcs;
                    ULONG    MaxBufferSize;
                    ULONG    MaxRawSize;
                    ULONG    SessionKey;
                    ULONG    Capabilities;
                    FILETIME SystemTime;
                    SHORT    ServerTimeZone;
                    UCHAR    ChallengeLength;
                    }
                  }
                SMB_Data
                  {
                  USHORT ByteCount;
                  Bytes
                    {
                    UCHAR      Challenge[];
                    SMB_STRING DomainName[];
                    SMB_STRING ServerName[];
                    }
                  }
                ].");
        }

        #endregion

        #region Verify SMBHeader section 2.2.4.5.2.2.

        /// <summary>
        /// Verify SMB_COM_NEGOTIATE Extended Security Server Response.
        /// </summary>
        /// <param name="smbHeader">The data type of CIFS.SmbHeader.</param>
        /// <param name="response">The SMB_COM_NEGOTIATE Extended Security Server Response</param>
        /// Disable warning CA1811 because according to Test Case design, this method is used for extension.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void VerifyMessageSyntaxSmbComNegotiateNonExtendedSecurityServerResponse(
            CIFS.SmbHeader smbHeader,
            SmbNegotiateResponsePacket response)
        {
            // If DialectIndex is 5, it means NT LAN Manager or later is negotiated for the SMB dialect.
            int ntlanManagerNegotiated = int.Parse(Site.Properties["NtLanManagerNegotiated"]);

            if (response.SmbParameters.DialectIndex == ntlanManagerNegotiated)
            {
                //
                // Verify requirement MS-SMB_R11132 and MS-SMB_R132
                //
                string isR11132Implementated = Site.Properties.Get("SHOULDMAYR11132Implementation");
                bool isR132Satisfied = ((((uint)smbHeader.Flags2) & 0x0040) == 0x0040);

                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R132 , Flags2 is : {0}",
                        smbHeader.Flags2);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R132.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR132Satisfied,
                        132,
                        @"[In SMB Header Extensions]This bit field[Flags2: SMB_FLAGS2_IS_LONG_NAME 0x0040] is set 
                        to one when NT LAN Manager or later is negotiated for the SMB dialect in WIndows.");

                    if (null == isR11132Implementated)
                    {
                        Site.Properties.Add("isR11132Implementated", Boolean.TrueString);
                        isR11132Implementated = Boolean.TrueString;
                    }
                }

                if (null != isR11132Implementated)
                {
                    bool implemented = Boolean.Parse(isR11132Implementated);
                    bool isSatisfied = isR132Satisfied;

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R11132");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R11132.
                    //
                    Site.CaptureRequirementIfAreEqual<Boolean>(
                        implemented,
                        isSatisfied,
                        11132,
                        String.Format(@"[In SMB Header Extensions]This bit field
                        [Flags2:SMB_FLAGS2_IS_LONG_NAME 0x0040] SHOULD be set to one when NT LAN Manager or later is 
                        negotiated for the SMB dialect. This requirement is {0}implemented", implemented ? "" : "not "));
                }
            }
        }

        #endregion Verify SMBHeader section 2.2.4.5.2.2

        #region Verify SMB_COM_SESSION_SETUP_ANDX Server Response section 2.2.4.6.2.

        /// <summary>
        /// Verify SMB_COM_SESSION_SETUP_ANDX Server Response. 
        /// </summary>
        /// <param name="response">The SMB_COM_SESSION_SETUP_ANDX Server Response.</param>
        private void VerifyMessageSyntaxSmbComSessionSetupAndXResponse(SmbSessionSetupAndxResponsePacket response)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R2329");

            //
            // Verify MS-SMB requirement: MS-SMB_R2329.
            //
            // If the response isn't null, it means the SUT has created and returned a 
            // SMB_COM_SESSION_SETUP_ANDX response.
            Site.CaptureRequirementIfIsNotNull(
                response,
                2329,
                @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Extended Security: If the GSS mechanism indicates 
                success, then  the server MUST create an SMB_COM_SESSION_SETUP_ANDX response (section 2.2.4.6.2).");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R114181");

            //
            // Verify MS-SMB requirement: MS-SMB_R114181.
            //
            // In SessionSetupRequest, when the status is MoreProcessingRequired, means the session setup fails, 
            // and additional roundtrips starts, so this requirement is captured directly.
            Site.CaptureRequirement(
                114181,
                @"[In Sequence Diagram] Session Setup Roundtrip: Otherwise[If authentication does not succeed after a single roundtrip], additional roundtrips will be required.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R323");

            //
            // Verify MS-SMB requirement: MS-SMB_R323.
            //
            // Here consider the WordCount as byte type, it is 1 byte. So only verify its value.
            Site.CaptureRequirementIfAreEqual<byte>(
                0x04,
                response.SmbParameters.WordCount,
                323,
                @"[In Server Response Extensions]WordCount (1 byte):  The value of this field MUST be 0x04. ");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R8391");

            // The Security BLOB has been built by protocol SDK as the rules specified in [RFC2743].
            // So this requirement has been covered in the protocol SDK.
            Site.CaptureRequirement(
                8391,
                @"[In Sequence Diagram] Session Setup Roundtrip: The security BLOB in the session setup response is 
                built as specified in [RFC2743].");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R305555");

            //
            // Verify MS-SMB requirement: MS-SMB_R305555.
            //
            // The SmbSessionSetupAndxResponsePacket is using extended security, so this requirement's condition is met.
            // ActionValues.LmSigning means the SMB_SETUP_USE_LANMAN_KEY 0x0002 bit.
            Site.CaptureRequirementIfAreEqual<Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs.
                ActionValues>(
                0,
                response.SmbParameters.Action
                & Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs.ActionValues.LmSigning,
                305555,
                @"[In Server Response Extensions]This bit[SMB_SETUP_USE_LANMAN_KEY0x0002] is not used with extended 
                security and MUST be clear (0x0000).");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R329");

            //
            // Verify MS-SMB requirement: MS-SMB_R329.
            //
            // Considering the SecurityBlobLength as ushort.
            // Since ushort is 2 bytes, the SecurityBlobLength's byte length will be checked.
            Site.CaptureRequirementIfAreEqual<ushort>(
                (ushort)response.SmbData.SecurityBlob.Length,
                response.SmbParameters.SecurityBlobLength,
                329,
                @"[In Server Response Extensions]SecurityBlobLength (2 bytes):  This value MUST specify the length in 
                bytes of the variable-length SecurityBlob that is contained within the response.  ");

            if ((response.SmbHeader.Flags2 & CIFS.SmbFlags2.SMB_FLAGS2_UNICODE) == CIFS.SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R5567,ByteCount:{0}",
                    response.SmbData.ByteCount);

                //
                // Verify MS-SMB requirement: MS-SMB_R5567.
                //
                bool isVerifyR5567 = ((2 == Marshal.SizeOf(response.SmbData.ByteCount))
                    && (response.SmbData.ByteCount >= 0x0006));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR5567,
                    5567,
                    @"[In Server Response Extensions]ByteCount (2 bytes):  If SMB_FLAGS2_UNICODE is set in the 
                    SMB_Header.Flags2 field,  then this field MUST be greater than or equal to 0x0006.");
            }
            else
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R5568,ByteCount:{0}",
                     response.SmbData.ByteCount);

                //
                // Verify MS-SMB requirement: MS-SMB_R5568.
                //
                bool isVerifyR5568 = ((2 == Marshal.SizeOf(response.SmbData.ByteCount))
                    && (response.SmbData.ByteCount >= 0x0003));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR5568,
                    5568,
                    @"[In Server Response Extensions]ByteCount (2 bytes): If SMB_FLAGS2_UNICODE is not set, 
                    then this field MUST be greater than or equal to 0x0003.");
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R5569,SmbData.SecurityBlob.Length:{0}",
                response.SmbData.SecurityBlob.Length);

            //
            // Verify MS-SMB requirement: MS-SMB_R5569.
            //
            // The latter half of the requirement is explaining the context of SecurityBlob field, which is informative.
            // If the SecurityBlob.length > 0, this requirement will be covered.
            bool isVerifyR5569 = response.SmbData.SecurityBlob.Length > 0;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR5569,
                5569,
                @"[In Server Response Extensions]SecurityBlob (variable): This value MUST contain the authentication 
                token being returned to the client, as specified in section 3.3.5.3 and [RFC2743].");

            if ((response.SmbHeader.Flags2 & CIFS.SmbFlags2.SMB_FLAGS2_UNICODE) == CIFS.SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R332, NativeOS:{0}",
                    BitConverter.ToString(response.SmbData.NativeOS));

                //
                // Verify MS-SMB requirement: MS-SMB_R332.
                //
                // If a string is an array of null-terminated Unicode characters, 
                // the last two bytes of the string are '\0'.
                int nativeOsLen = response.SmbData.NativeOS.Length;
                bool isVerifyR332 = false;
                if (0 == nativeOsLen)
                {
                    isVerifyR332 = true;
                }
                else
                {
                    isVerifyR332 = ('\0' == response.SmbData.NativeOS[nativeOsLen - 1])
                        && ('\0' == response.SmbData.NativeOS[nativeOsLen - 2]);
                }

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR332,
                    332,
                    @"[In Server Response Extensions,NativeOS (variable)]If SMB_FLAGS2_UNICODE is set in the Flags2 
                    field of the SMB header of the response, the string MUST be a null-terminated array of 16-bit 
                    Unicode characters. ");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R334");

                int lengthFromStart = int.Parse(Site.Properties["LengthFromStart"]);

                //
                // Verify MS-SMB requirement: MS-SMB_R334.
                //
                // 192 represents the total length of the header and the fields before securityblob.
                // The bytes before NativeOS include 192 and SecurityBlob, and the bytes length should be even.
                Site.CaptureRequirementIfAreEqual<int>(
                    0,
                    ((lengthFromStart + response.SmbParameters.SecurityBlobLength) % 2),
                    334,
                    @"[In Server Response Extensions]NativeOS (variable): If the name string consists of Unicode 
                    characters, this field MUST be aligned to start on a 2-byte boundary from the start of the SMB 
                    header.<47> ");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R338");

                //
                // Verify MS-SMB requirement: MS-SMB_R338.
                //
                // 192 represents the total length of the header and the fields before security blob
                // The bytes before NativeLANMan include 192 ,SecurityBlob and NativeOS and the bytes length should be 
                // even.
                Site.CaptureRequirementIfAreEqual<int>(
                    0,
                    ((lengthFromStart + response.SmbParameters.SecurityBlobLength + response.SmbData.NativeOS.Length)
                    % 2),
                    338,
                    @"[In Server Response Extensions]NativeLANMan (variable): If the name string consists of Unicode 
                    characters, this field MUST be aligned to start on a 2-byte boundary from the start of the SMB 
                    header.<48> ");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R5572,NativeLanMan:{0}",
                     BitConverter.ToString(response.SmbData.NativeLanMan));

                int lanManLength = response.SmbData.NativeLanMan.Length;
                bool isVerifyR5572 = false;
                if (0 == lanManLength)
                {
                    isVerifyR5572 = true;
                }
                else
                {
                    isVerifyR5572 = ('\0' == response.SmbData.NativeLanMan[lanManLength - 1])
                    && ('\0' == response.SmbData.NativeLanMan[lanManLength - 2]);
                }

                //
                // Verify MS-SMB requirement: MS-SMB_R5572.
                //
                Site.CaptureRequirementIfIsTrue(
                    isVerifyR5572,
                    5572,
                    @"[In Server Response Extensions]NativeLANMan (variable): If SMB_FLAGS2_UNICODE is set in the 
                    Flags2 field of the SMB header of the response, the string MUST be a null-terminated array of 
                    16-bit Unicode characters.");
            }
            else
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R333,NativeOS:{0}",
                    BitConverter.ToString(response.SmbData.NativeOS));

                //
                // Verify MS-SMB requirement: MS-SMB_R333.
                //
                // If the string is null-terminated array of OEM characters, only the last byte of the string is '\0'.
                int nativeOSLen = response.SmbData.NativeOS.Length;
                bool isVerifyR333 = (('\0' == response.SmbData.NativeOS[nativeOSLen - 1])
                    && ('\0' != response.SmbData.NativeOS[nativeOSLen - 2]));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR333,
                    333,
                    "[In Server Response Extensions]NativeOS (variable):  Otherwise[If SMB_FLAGS2_UNICODE is not set" +
                    "in the Flags2 field of the SMB header of the response], the string MUST be a null-terminated array" +
                    "of OEM characterst.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R337,NativeLANMan:{0}",
                    BitConverter.ToString(response.SmbData.NativeLanMan));

                //
                // Verify MS-SMB requirement: MS-SMB_R337.
                //
                // If the string is null-terminated array of OEM characters, only the last byte of the string is '\0'.
                int nativeLanManLen = response.SmbData.NativeLanMan.Length;
                bool isVerifyR337 = (('\0' == response.SmbData.NativeLanMan[nativeLanManLen - 1])
                    && ('\0' != response.SmbData.NativeLanMan[nativeLanManLen - 2]));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR337,
                    337,
                    @"[In Server Response Extensions]NativeLANMan (variable):  Otherwise[If SMB_FLAGS2_UNICODE is not 
                    set in the Flags2 field of the SMB header of the response], the string MUST be a null-terminated 
                    array of OEM characters. ");
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R5553");

            // The response's form is verified in the R323, R329, R5567, R5568, R5569, R332, R334, R338, R5572, 
            // R333, R337. Here capture this requirement directly.
            Site.CaptureRequirement(
                5553,
                @"[In Server Response Extensions]When extended security is being used (see section 3.2.4.2.4), a 
                successful response MUST take the following form
                [SMB_Parameters
                  {
                  UCHAR  WordCount;
                  Words
                    {
                    UCHAR  AndXCommand;
                    UCHAR  AndXReserved;
                    USHORT AndXOffset;
                    USHORT Action;
                    USHORT SecurityBlobLength;
                    }
                  }
                SMB_Data
                  {
                  USHORT ByteCount;
                  Bytes
                    {
                    UCHAR      SecurityBlob[SecurityBlobLength];
                    SMB_STRING NativeOS;
                    SMB_STRING NativeLanMan;
                    SMB_STRING PrimaryDomain;
                    }
                  }
                ].");
        }

        #endregion

        #region Verify SMB_COM_TREE_CONNECT_ANDX Server Response section 2.2.4.7.2.

        /// <summary>
        /// Verify SMB_COM_TREE_CONNECT_ANDX Server Response. 
        /// </summary>
        /// <param name="response">The SMB_COM_TREE_CONNECT_ANDX Server Response.</param>
        /// <param name="isNotionSupported">Whether Implementations support the notion of a guest account.</param>
        /// <param name="isShi1005Set">
        /// Whether the SHI1005_FLAGS_ALLOW_NAMESPACE_CACHING bit is set for the share.
        /// </param>
        /// <param name="isNoAliasingSet">
        /// Whether NoAliasingOnFilesystem registry key is set to 1.</param>
        /// <param name="isShortFileNameDisabled">
        /// Whether short file name generation is disabled in the file system.
        /// </param>
        /// <param name="allowedGuestAccess">The access allowed for the guest account.</param>
        /// <param name="isRequestExtResponse">The extended requestor's Response.</param>
        /// Disable warning CA1801 because according to Test Case design, 
        /// the parameters isShi1005Set, isNoAliasingSet, isShortFileNameDisabled are used for extension.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        private void VerifyMessageSyntaxSmbComTreeConnectResponse(
            SmbTreeConnectAndxResponsePacket response,
            bool isNotionSupported,
            bool isShi1005Set,
            bool isNoAliasingSet,
            bool isShortFileNameDisabled,
            int allowedGuestAccess,
            bool isRequestExtResponse)
        {
            if (isRequestExtResponse)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R376");

                //
                // Verify MS-SMB requirement: MS-SMB_R376.
                //
                Site.CaptureRequirementIfAreEqual<byte>(
                    0x07,
                    response.SmbParameters.WordCount,
                    376,
                    @"[In Server Response Extensions]WordCount (1 byte):  The value of this field MUST be 0x07.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R35644,OptionalSupport:{0}",
                    response.SmbParameters.OptionalSupport);

                //
                // Verify MS-SMB requirement: MS-SMB_R35644.
                //
                bool isVerifyR35644 = (((response.SmbParameters.OptionalSupport & 0x0001) == 0x0001)
                    || ((response.SmbParameters.OptionalSupport & 0x0002) == 0x0002)
                    || ((response.SmbParameters.OptionalSupport & 0x000C) == 0x000C)
                    || ((response.SmbParameters.OptionalSupport & 0x0010) == 0x0010)
                    || ((response.SmbParameters.OptionalSupport & 0x0020) == 0x0020));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR35644,
                    35644,
                    @"[In Server Response Extensions]OptionalSupport (2 bytes):Combination of the following flags [
                    SMB_SUPPORT_SEARCH_BITS 0x0001,
                    SMB_SHARE_IS_IN_DFS 0x0002,
                    SMB_CSC_MASK 0x000C,
                    SMB_UNIQUE_FILE_NAME 0x0010,
                    SMB_EXTENDED_SIGNATURES 0x0020] 
                    MUST be supported. ");

                //
                // Verify requirement MS-SMB_R505644 and MS-SMB_R515644
                //
                string isR505644Implementated = Site.Properties.Get("SHOULDMAYR505644Implementation");
                ushort optionalSupportFlags = 0x0001 | 0x0002 | 0x000c | 0x0010 | 0x0020;
                bool isR515644Satisfied = (0 == (response.SmbParameters.OptionalSupport & (ushort)~optionalSupportFlags));

                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R515644,OptionalSupport:{0}",
                        response.SmbParameters.OptionalSupport);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R515644.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR515644Satisfied,
                        515644,
                        @"[In Server Response Extensions]OptionalSupport (2 bytes): The server is set them to zero in 
                        Windows.");
                    if (null == isR505644Implementated)
                    {
                        Site.Properties.Add("SHOULDMAYR505644Implementation", Boolean.TrueString);
                        isR505644Implementated = Boolean.TrueString;
                    }
                }

                if (null != isR505644Implementated)
                {
                    bool implemented = Boolean.Parse(isR505644Implementated);
                    bool isSatisfied = isR515644Satisfied;

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R505644,OptionalSupport:{0}",
                        response.SmbParameters.OptionalSupport);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R505644.
                    //
                    Site.CaptureRequirementIfAreEqual<Boolean>(
                        implemented,
                        isSatisfied,
                        505644,
                        String.Format("[In Server Response Extensions]OptionalSupport (2 bytes): The server SHOULD set " +
                        "them to zero. This requirement is {0}implemented", implemented ? "" : "not "));
                }

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R5651");

                //
                // Verify MS-SMB requirement: MS-SMB_R5651.
                //
                // The latter half of the requirement is explaining the content of MaximalShareAccessRights field which 
                // is informative.So here verify this requirement partially, only verify the byte size of 
                // MaximalShareAccessRights field.
                Site.CaptureRequirementIfAreEqual<int>(
                    4,
                    Marshal.SizeOf(response.SmbParameters.MaximalShareAccessRights),
                    5651,
                    @"[In Server Response Extensions]MaximalShareAccessRights (4 bytes): This field MUST specify the 
                    maximum rights that the user has to this share based on the security enforced by the share.");

                //
                // Verify MS-SMB requirement: MS-SMB_R5652.
                //
                // If the field is encoded in an AccessMask format, then only the flags listed in section 2.2.1.4
                // could be set, and the other unused flags is set to 0.
                // AccessMask is a uint variable which represent that all the flags listed 
                // in section2.2.1.4 were set to 1. 
                // So the bit-or result of the field with AccessMask should be equal to AccessMask, 
                // otherwise it means the flags not listed in section 2.2.1.4 was set, which is invalid.
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    ~AccessMask & response.SmbParameters.MaximalShareAccessRights,
                    5652,
                    "[In Server Response ExtensionsMaximalShareAccessRights (4 bytes): This field MUST be encoded in " +
                    "an AccessMask format, as specified in section 2.2.1.4.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R5653");

                //
                // Verify MS-SMB requirement: MS-SMB_R5653.
                //
                // The latter half of requirement is explaining the content of GuestMaximalShareAccessRights field 
                // which is informative. So verify this requirement partially. Only verify the byte size of
                // GuestMaximalShareAccessRights field.
                Site.CaptureRequirementIfAreEqual<int>(
                    4,
                    Marshal.SizeOf(response.SmbParameters.GuestMaximalShareAccessRights),
                    5653,
                    @"[In Server Response Extensions]GuestMaximalShareAccessRights (4 bytes): This field MUST specify 
                    the maximum rights that the guest account has on this share based on the security enforced by the 
                    share.");

                // isNotionSupported means if implementations support the notion of a guest account
                if (!isNotionSupported)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R5656");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R5656.
                    //
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0,
                        response.SmbParameters.GuestMaximalShareAccessRights,
                        5656,
                        @"[In Server Response Extensions]GuestMaximalShareAccessRights (4 bytes): Implementations that 
                        do not support the notion of a guest account MUST set this field to zero, which implies no 
                        access.");
                }

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R105656");

                //
                // Verify MS-SMB requirement: MS-SMB_R105656.
                //
                // If the field is encoded in an AccessMask format, then only the flags listed 
                // in section 2.2.1.4 could be set, and the other unused flags is set to 0. 
                // AccessMask is a uint variable which represent that all the flags listed 
                // in section2.2.1.4 were set to 1. 
                // So the bit-or result of the field with AccessMask should be equal to AccessMask, 
                // otherwise it means the flags not listed in section 2.2.1.4 was set, which is invalid.
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    ~AccessMask & response.SmbParameters.GuestMaximalShareAccessRights,
                    105656,
                    "[In Server Response Extensions]GuestMaximalShareAccessRights (4 bytes):" +
                    "This field MUST be encoded in an AccessMask format, as specified in section 2.2.1.4.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9623,GuestMaximalShareAccessRights:{0}",
                    response.SmbParameters.GuestMaximalShareAccessRights);

                //
                // Verify MS-SMB requirement: MS-SMB_R9623.
                //
                // isNotionSupported means whether implementation supports the notion of a guest account.
                // allowedGuestAccess means the access allowed for the guest account.
                bool isVerifyR9623 = isNotionSupported
                    || ((uint)allowedGuestAccess == response.SmbParameters.GuestMaximalShareAccessRights);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR9623,
                    9623,
                    @"<50> Section 2.2.4.7.2: Windows-based clients and servers support the notion of a guest account 
                    and set this field to the access allowed for the guest account.");
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R5617");

            // If the requirement above have been verified, it means the SUT has sent the response as the 
            // format.
            Site.CaptureRequirement(
                5617,
                @"[In Server Response Extensions]When a server returns extended information, the response takes 
                the following format.");
        }

        #endregion

        #region Verify SMB_COM_NT_CREATE_ANDX Server Response section 2.2.4.9.2.

        /// <summary>
        /// Verify SMB_COM_NT_CREATE_ANDX Server Response.
        /// </summary>
        /// <param name="response">The SMB_COM_NT_CREATE_ANDX Server Response.</param>
        /// <param name="isNotionSupported">Whether Implementations support the notion of a guest account.</param>
        private void VerifyMessageSyntaxSmbComNtCreateAndXResponseForNotNotionSupported(
                     SmbNtCreateAndxResponsePacket response,
                     bool isNotionSupported)
        {
            // isNotionSupported means if implementations support the notion of a guest account.
            if (!isNotionSupported)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R5917");

                //
                // Verify MS-SMB requirement: MS-SMB_R5917.
                //
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    BytesToUInt32(response.SmbParameters.GuestMaximalAccessRights),
                    5917,
                    @"[In Server Response Extensions]GuestMaximalAccessRights (4 bytes): Implementations that do not 
                    support the notion of a guest account MUST set this field to zero.<57>");
            }

        }

        #endregion Verify SMB_COM_NT_CREATE_ANDX Server Response section 2.2.4.9.2

        #region Verify SMB_COM_NT_CREATE_ANDX Server Response section 2.2.4.9.2.

        /// <summary>
        /// Verify SMB_COM_NT_CREATE_ANDX Server Response.
        /// </summary>
        /// <param name="response">The SMB_COM_NT_CREATE_ANDX Server Response.</param>
        /// <param name="isSutWantClientLeverageNewCapabilities">
        /// If the SUT wants the client to leverage the new capabilities.
        /// </param> 
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        /// Disable warning CA1505 because according to Test Case design, 
        /// excessive maintainability index is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        private void VerifyMessageSyntaxSmbComNtCreateAndXResponse(
                     SmbNtCreateAndxResponsePacket response,
                     bool isSutWantClientLeverageNewCapabilities)
        {
            if ((sutOsVersion == Platform.Win2K3) ||
                (sutOsVersion == Platform.Win2K3R2) ||
                (sutOsVersion == Platform.Win2K8R2) ||
                (sutOsVersion == Platform.Win2K))
            {
                //
                // Verify requirement MS-SMB_R14840 and MS-SMB_R2372
                //
                string isR14840Implementated = Site.Properties.Get("SHOULDMAYR14840Implementation");
                bool isR2372Satisfied = (response != null);

                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R2372");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R2372.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR2372Satisfied,
                        2372,
                        @"[In Receiving an SMB_COM_NT_CREATE_ANDX Request] <113> Section 3.3.5.5: SMB servers on 
                        Windows 2000 Server, Windows Server 2003, Windows Server 2008, and Windows Server 2008 R2 use 
                        extended responses if the client requests them[new capabilities].");

                    if (null == isR14840Implementated)
                    {
                        Site.Properties.Add("SHOULDMAYR14840Implementation", Boolean.TrueString);
                        isR14840Implementated = Boolean.TrueString;
                    }
                }

                if (null != isR14840Implementated)
                {
                    bool implemented = Boolean.Parse(isR14840Implementated);
                    bool isSatisfied = isR2372Satisfied;

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R14840");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R14840.
                    //
                    Site.CaptureRequirementIfAreEqual<Boolean>(
                        implemented,
                        isSatisfied,
                        14840,
                        String.Format("[In Receiving an SMB_COM_NT_CREATE_ANDX Request] The processing of an " +
                        "SMB_COM_NT_CREATE_ANDX request is handled as specified in [MS-CIFS] section 3.3.5.50 with the " +
                        "following additions: On a successful create or open, if the NT_CREATE_REQUEST_EXTENDED_RESPONSE " +
                        "flag was set in the Flags field of the request, then the server SHOULD<113> send an extended " +
                        "response (section 2.2.4.9.2). This requirement is {0}implemented", implemented ? "" : "not "));
                }
            }

            //
            // Verify requirement MS-SMB_R19248 and MS-SMB_R109248
            //
            string isR19248Implementated = Site.Properties.Get("SHOULDMAYR19248Implementation");
            bool isR109248Satisfied = ((CIFS.FileTypeValue.FileTypeDisk == response.SmbParameters.ResourceType)
                || (CIFS.FileTypeValue.FileTypeByteModePipe == response.SmbParameters.ResourceType)
                || (CIFS.FileTypeValue.FileTypeMessageModePipe == response.SmbParameters.ResourceType)
                || (CIFS.FileTypeValue.FileTypePrinter == response.SmbParameters.ResourceType)
                || (CIFS.FileTypeValue.FileTypeUnknown == response.SmbParameters.ResourceType));
            if (isWindows)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R109248,ResourceType:{0}",
                    response.SmbParameters.ResourceType);

                //
                // Verify MS-SMB requirement: MS-SMB_R109248.
                //
                Site.CaptureRequirementIfIsTrue(
                    isR109248Satisfied,
                    109248,
                    "[In Server Response Extensions]ResourceType (2 bytes):  The file type. This field <54> is " +
                    "interpreted as follows:[" +
                    "FileTypeDisk0x0000 means File or Directory;" +
                    "FileTypeByteModePipe0x0001 means Byte mode named pipe;" +
                    "FileTypeMessageModePipe 0x0002 means Message mode named pipe;" +
                    "FileTypePrinter 0x0003 means Printer Device;" +
                    "FileTypeUnknown 0xFFFF means Unknown file type.]" +
                    "in Windows");

                if (null == isR19248Implementated)
                {
                    Site.Properties.Add("SHOULDMAYR19248Implementation", Boolean.TrueString);
                    isR19248Implementated = Boolean.TrueString;
                }
            }

            if (null != isR19248Implementated)
            {
                bool implemented = Boolean.Parse(isR19248Implementated);
                bool isSatisfied = isR109248Satisfied;

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R19248,ResourceType:{0}",
                    response.SmbParameters.ResourceType);

                //
                // Verify MS-SMB requirement: MS-SMB_R19248.
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implemented,
                    isSatisfied,
                    19248,
                    String.Format("[In Server Response Extensions]ResourceType (2 bytes):  The file type. This field " +
                    "SHOULD<55> be interpreted as follows: [" +
                    "FileTypeDisk 0x0000 means File or Directory;" +
                    "FileTypeByteModePipe 0x0001 means Byte mode named pipe;" +
                    "FileTypeMessageModePipe 0x0002 means Message mode named pipe;" +
                    "FileTypePrinter 0x0003 means Printer Device;" +
                    "FileTypeUnknown 0xFFFF means Unknown file type.]" +
                    "This requirement is {0}implemented", implemented ? "" : "not "));
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9611");

            //
            // Verify MS-SMB requirement: MS-SMB_R9611.
            //
            Site.CaptureRequirementIfAreNotEqual<CIFS.FileTypeValue>(
                (CIFS.FileTypeValue)0x04,
                response.SmbParameters.ResourceType,
                9611,
                @"<54> Section 2.2.4.9.2: Windows-based servers no longer return the FileTypeCommDevice resource 
                type that is listed in [MS-CIFS] section 2.2.4.64.2.");

            if ((response.SmbParameters.ResourceType == CIFS.FileTypeValue.FileTypeByteModePipe)
               || (response.SmbParameters.ResourceType == CIFS.FileTypeValue.FileTypeMessageModePipe))
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9250");

                // When the filetype is named pipe, the protocol SDK will encode the devicestate as NMPipeStatus.
                // Here capture it directly.
                Site.CaptureRequirement(
                    9250,
                    @"[In Server Response Extensions]NMPipeStatus_or_FileStatusFlags (2 bytes): If the ResourceType 
                    field is a named pipe (FileTypeByteModePipe or FileTypeMessageModePipe),then this field MUST be the 
                    NMPipeStatus field.");
            }
            else if (response.SmbParameters.ResourceType == CIFS.FileTypeValue.FileTypeDisk)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9252");

                // Need debug and modify, DeviceState is none.
                //
                // Verify MS-SMB requirement: MS-SMB_R9252.
                //
                // All the flags of FileStatusFlags set.
                // 0x0001 is the value of NO_EAS in the FileStatusFlags field
                // 0x0002 is the value of NO_SUBSTREAMS in the FileStatusFlags field
                // 0x0004 is the value of NO_REPARSETAG in the FileStatusFlags field
                ushort fileStatus = 0x0001 | 0x0002 | 0x0004;

                // According to TD, the FileStatusFlags field could be any combination of 0x0001, 0x0002 and 0x0004, 
                // so if the bit-or result of the field is equal to fileStatus(all file status flags set), this 
                // field is FileStatusFlags field.
                Site.CaptureRequirementIfAreEqual<ushort>(
                    fileStatus,
                    (ushort)(fileStatus | (ushort)response.SmbParameters.NMPipeStatus_or_FileStatusFlags),
                    9252,
                    @"[In Server Response Extensions]NMPipeStatus (2 bytes): If the ResourceType field is FileTypeDisk, 
                    then this field MUST be the FileStatusFlags field.");

                //
                // Verify requirement MS-SMB_R9255 and MS-SMB_R109255
                //
                string isR9255Implementated = Site.Properties.Get("SHOULDMAYR9255Implementation");
                bool isR109255Satisfied = (0
                    == ((ushort)~fileStatus & (ushort)response.SmbParameters.NMPipeStatus_or_FileStatusFlags));

                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R109255,FileStatusFlags:{0}",
                        (ushort)response.SmbParameters.NMPipeStatus_or_FileStatusFlags);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R109255.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR109255Satisfied,
                        109255,
                        @"[In Server Response Extensions]FileStatusFlags (2 bytes): Unused bit fields are set to zero 
                        by the server in Windows.");

                    if (null == isR9255Implementated)
                    {
                        Site.Properties.Add("isR9255Implementated", Boolean.TrueString);
                        isR9255Implementated = Boolean.TrueString;
                    }
                }

                if (null != isR9255Implementated)
                {
                    bool implemented = Boolean.Parse(isR9255Implementated);
                    bool isSatisfied = isR109255Satisfied;

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R9255,FileStatusFlags:{0}",
                        (ushort)response.SmbParameters.NMPipeStatus_or_FileStatusFlags);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R9255.
                    //
                    Site.CaptureRequirementIfAreEqual<Boolean>(
                        implemented,
                        isSatisfied,
                        9255,
                        String.Format("[In Server Response Extensions]FileStatusFlags (2 bytes): Unused bit fields " +
                        "SHOULD be set to zero by the server." +
                        "This requirement is {0}implemented", implemented ? "" : "not "));
                }
            }

            if ((response.SmbParameters.ResourceType == (CIFS.FileTypeValue)0x04)
                 || (response.SmbParameters.ResourceType == CIFS.FileTypeValue.FileTypePrinter)
                 || (response.SmbParameters.ResourceType == CIFS.FileTypeValue.FileTypeUnknown))
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9259");

                //
                // Verify MS-SMB requirement: MS-SMB_R9259.
                //
                Site.CaptureRequirementIfAreEqual<int>(
                    0,
                    (int)response.SmbParameters.NMPipeStatus_or_FileStatusFlags,
                    9259,
                    @"[In Server Response Extensions]FileStatusFlags (2 bytes): For all other values
                    [except NO_EAS 0x0001,NO_SUBSTREAMS 0x0002,NO_REPARSETAG 0x0004]of ResourceType, this field SHOULD 
                    be set to zero by the server when sending a response.");

                //
                // Verify requirement MS-SMB_R119259 and MS-SMB_R109259.
                //
                string isR119259Implementated = Site.Properties.Get("isR119259Implementated");

                // In the protocol SDK the FilestatusFlags is represented by DeviceState.
                bool isR109259Satisfied = (0 == (int)response.SmbParameters.NMPipeStatus_or_FileStatusFlags);

                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                       @"Verify MS-SMB_R109259,DeviceState:{0}",
                       response.SmbParameters.NMPipeStatus_or_FileStatusFlags);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R109259.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR109259Satisfied,
                        109259,
                        @"[In Server Response Extensions]FileStatusFlags (2 bytes): For  other 
                        values[except FileTypeDisk, FileTypeByteModePipe  and FileTypeMessageModePipe]of ResourceType, 
                        this field is set to zero by the server when sending a response in Windows.");

                    if (null == isR119259Implementated)
                    {
                        Site.Properties.Add("isR119259Implementated", Boolean.TrueString);
                        isR119259Implementated = Boolean.TrueString;
                    }
                }

                if (null != isR119259Implementated)
                {
                    bool implemented = Boolean.Parse(isR119259Implementated);
                    bool isSatisfied = isR109259Satisfied;

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                       @"Verify MS-SMB_R119259,DeviceState:{0}",
                       response.SmbParameters.NMPipeStatus_or_FileStatusFlags);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R119259.
                    //
                    Site.CaptureRequirementIfAreEqual<Boolean>(
                        implemented,
                        isSatisfied,
                        119259,
                        String.Format("[In Server Response Extensions]FileStatusFlags (2 bytes): For  other " +
                        "values[except FileTypeDisk, FileTypeByteModePipe  and FileTypeMessageModePipe]of ResourceType, " +
                        "this field SHOULD be set to zero by the server when sending a response. " +
                        "This requirement is {0}implemented", implemented ? "" : "not "));
                }
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R5909");

            //
            // Verify MS-SMB requirement: MS-SMB_R5909.
            //
            // All the guids are defined as byte[], only this type can be verified and it has been verified in the SDK.
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(byte[]),
                response.SmbParameters.VolumeGUID.GetType(),
                5909,
                @"[In Server Response Extensions]VolumeGUID (16 bytes):  This field MUST be a GUID value that uniquely 
                identifies the volume on which the file resides.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R5910");

            //
            // Verify MS-SMB requirement: MS-SMB_R5910.
            //
            Site.CaptureRequirementIfAreEqual<int>(
                8,
                response.SmbParameters.FileId.Length,
                5910,
                @"[In Server Response Extensions]FileId (8 bytes):  This field MUST be a 64-bit opaque value that 
                uniquely identifies this file on a volume.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R5913");

            //
            // Verify MS-SMB requirement: MS-SMB_R5913.
            //
            // If the field is encoded in an AccessMask format, then only the flags listed 
            // in section 2.2.1.4 could be set, and the other unused flags is set to 0. 
            // AccessMask is a uint variable which represent that all the flags listed 
            // in section2.2.1.4 were set to 1. 
            // So the bit-or result of the field with AccessMask should be equal to AccessMask,
            // otherwise it means the flags not listed in section 2.2.1.4 was set, which is invalid.
            Site.CaptureRequirementIfAreEqual<uint>(
                0,
                ~AccessMask & BytesToUInt32(response.SmbParameters.MaximalAccessRights),
                5913,
                "[In Server Response Extensions]MaximalAccessRights (4 bytes): " +
                "This field MUST be encoded in an AccessMask format, as specified in section 2.2.1.4.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R5915");

            //
            // Verify MS-SMB requirement: MS-SMB_R5915.
            //
            // If the field is encoded in an AccessMask format, then only the flags listed 
            // in section 2.2.1.4 could be set, and the other unused flags is set to 0. 
            // AccessMask is a uint variable which represent that all the flags listed 
            // in section2.2.1.4 were set to 1. 
            // So the bit-or result of the field with AccessMask should be equal to AccessMask,
            // otherwise it means the flags not listed in section 2.2.1.4 was set, which is invalid.
            Site.CaptureRequirementIfAreEqual<uint>(
                0,
                ~AccessMask & BytesToUInt32(response.SmbParameters.GuestMaximalAccessRights),
                5915,
                "[In Server Response Extensions]GuestMaximalAccessRights (4 bytes): " +
                "This field MUST be encoded in an AccessMask format, as specified in section 2.2.1.4.");

            //
            // Verify requirement MS-SMB_R539 and MS-SMB_R9617 MS-SMB_R9618
            //
            string isR539Implementated = Site.Properties.Get("SHOULDMAYR539Implementation");
            bool isR9617Satisfied = (0x0000 == response.SmbData.ByteCount);

            if (isWindows)
            {
                if ((sutOsVersion == Platform.Win2K3) ||
                   (sutOsVersion == Platform.Win2K3R2) ||
                   (sutOsVersion == Platform.Win2K8R2))
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R9617,ByteCount:{0}",
                        response.SmbData.ByteCount);

                    //
                    // Verify MS-SMB requirement:  MS-SMB_R9617.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR9617Satisfied,
                        9617,
                        "<59> Section 2.2.4.9.2: Windows Server 2003, Windows Server 2003 R2, and Windows Server 2008" +
                        "set this field to zero.");
                }
                if (null == isR539Implementated)
                {
                    Site.Properties.Add("SHOULDMAYR539Implementation", Boolean.TrueString);
                    isR539Implementated = Boolean.TrueString;
                }
            }

            if (null != isR539Implementated)
            {
                bool implemented = Boolean.Parse(isR539Implementated);
                bool isSatisfied = isR9617Satisfied;

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R539,ByteCount:{0}",
                    response.SmbData.ByteCount);

                //
                // Verify MS-SMB requirement: MS-SMB_R539.
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implemented,
                    isSatisfied,
                    539,
                    String.Format("[In Server Response Extensions]SMB_Data: ByteCount (2 bytes): This field SHOULD<59> " +
                    "be zero. This requirement is {0}implemented", implemented ? "" : "not "));
            }

            //
            // Verify requirement MS-SMB_R502 and MS-SMB_R9610
            //
            string isR502Implementated = Site.Properties.Get("SHOULDMAYR502Implementation");
            bool isR9610Satisfied = (1 == Marshal.SizeOf(response.SmbParameters.WordCount))
                 && (0x2A == response.SmbParameters.WordCount);

            if (isWindows)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9610,WordCount:{0}",
                    response.SmbParameters.WordCount);

                //
                // Verify MS-SMB requirement: MS-SMB_R9610.
                //
                Site.CaptureRequirementIfIsTrue(
                    isR9610Satisfied,
                    9610,
                    @"<53> Section 2.2.4.9.2: Windows-based servers set this field to 0x2A, even though the 
                    prescribed 100 bytes (50 WORDs) are present in the SMB_Parameters.Words portion of the message.");

                if (null == isR502Implementated)
                {
                    Site.Properties.Add("SHOULDMAYR502Implementation", Boolean.TrueString);
                    isR502Implementated = Boolean.TrueString;
                }
            }

            if (null != isR502Implementated)
            {
                bool implemented = Boolean.Parse(isR502Implementated);
                bool isSatisfied = (1 == Marshal.SizeOf(response.SmbParameters.WordCount))
                    && (0x2A == response.SmbParameters.WordCount);

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R502,WordCount:{0}",
                    response.SmbParameters.WordCount);

                //
                // Verify MS-SMB requirement: MS-SMB_R502.
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implemented,
                    isSatisfied,
                    502,
                    String.Format("[In Server Response Extensions]WordCount (1 byte):WordCount (1 bytes): This field " +
                    "SHOULD<54> be 0x32. This requirement is {0}implemented", implemented ? "" : "not "));
            }

            if ((sutOsVersion == Platform.Win2K3) ||
                (sutOsVersion == Platform.Win2K3R2) ||
                (sutOsVersion == Platform.Win2K8) ||
                (sutOsVersion == Platform.Win2K8R2))
            {
                // 0x0400 is the SMB_FLAGS2_REPARSE_PATH flag
                if (((ushort)response.SmbHeader.Flags2
                    & (ushort)SmbHeader_Flags2_Values.SMB_FLAGS2_REPARSE_PATH)
                    == (ushort)SmbHeader_Flags2_Values.SMB_FLAGS2_REPARSE_PATH)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R8590");

                    // If the OS is Win2k3, or Win2K3R2 or Win2K8 or Win2K8R2, and SMB_FLAGS2_REPARSE_PATH flag is set, 
                    // it means that these OSs support that flag. So capture this requirement directly.
                    Site.CaptureRequirement(
                        8590,
                        @"[In Scanning a Path for a Previous Version Token] <111> Section 3.3.5.1.1: SMB servers on 
                        Windows Server 2003, Windows Server 2003 R2, Windows Server 2008, and Windows Server 2008 R2 
                        support the SMB_FLAGS2_REPARSE_PATH flag and previous version access. ");
                }
            }

            // isSerWantCliLeverageNewCap means if the SUT wants the client to leverage the new 
            // capabilities.
            if (isSutWantClientLeverageNewCapabilities)
            {
                //
                // Verify requirement MS-SMB_R30066 and MS-SMB_R30067
                //
                string isR30066Implementated = Site.Properties.Get("SHOULDMAYR30066Implementation");

                // If the SUT does not fill in the VolumeGUID and FileId fields, these fields are zero.
                bool isR30067Satisfied = (IsAllBytesZero(response.SmbParameters.VolumeGUID)) &&
                    (IsAllBytesZero(response.SmbParameters.FileId));

                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R30067,VolumeGUID:{0},FileId:{1}",
                        BytesToString(response.SmbParameters.VolumeGUID),
                        BytesToUInt64(response.SmbParameters.FileId));

                    //
                    // Verify MS-SMB requirement: MS-SMB_R30067.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR30067Satisfied,
                        30067,
                        "[In Receiving an SMB_COM_NT_CREATE_ANDX Request, The processing of an SMB_COM_NT_CREATE_ANDX" +
                        "request is handled as specified in [MS-CIFS] section 3.3.5.50 with the following additions: ]" +
                        "If the server sends the new response, then it MUST construct a response as specified in section" +
                        "2.2.4.9.2. with the addition of the following rules: The server does not fill in the VolumeGUID" +
                        "and FileId fields if it wants the client to leverage the new apabilities in Windows. ");

                    if (null == isR30066Implementated)
                    {
                        Site.Properties.Add("SHOULDMAYR30066Implementation", Boolean.FalseString);
                        isR30066Implementated = Boolean.FalseString;
                    }
                }
                if (null != isR30066Implementated)
                {
                    bool implemented = Boolean.Parse(isR30066Implementated);
                    bool isSatisfied = !isR30067Satisfied;

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R30066:VolumeGUID:{0},FileId:{1}",
                        BytesToString(response.SmbParameters.VolumeGUID),
                        BytesToUInt64(response.SmbParameters.FileId));

                    //
                    // Verify MS-SMB requirement: MS-SMB_R30066.
                    //
                    Site.CaptureRequirementIfAreEqual<Boolean>(
                        implemented,
                        isSatisfied,
                        30066,
                        String.Format("[In Receiving an SMB_COM_NT_CREATE_ANDX Request, The processing of an" +
                        "SMB_COM_NT_CREATE_ANDX request is handled as specified in [MS-CIFS] section 3.3.5.50 with the" +
                        "following additions: ] If the server sends the new response, then it MUST construct a response" +
                        "as specified in section 2.2.4.9.2. with the addition of the following rules: The server" +
                        "MAY<117> fill in the VolumeGUID and FileId fields if it wants the client to leverage the new" +
                        "capabilities. This requirement is {0}implemented", implemented ? "" : "not "));
                }
            }

            if ((sutOsVersion == Platform.Win2K) ||
                (sutOsVersion == Platform.Win2K3) ||
                (sutOsVersion == Platform.Win2K8) ||
                (sutOsVersion == Platform.Win2K8R2))
            {
                //
                // The following statement code will be run only when debugging.
                //
                // For VolumeGUID and FileId are both bytes array, they can't be added into log.
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R30090");

                //
                // Verify MS-SMB requirement: MS-SMB_R30090.
                //
                bool isVerifyR30090 = (IsAllBytesZero(response.SmbParameters.VolumeGUID))
                     && (IsAllBytesZero(response.SmbParameters.FileId));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR30090,
                    30090,
                    @"[In Receiving an SMB_COM_NT_CREATE_ANDX Request] <116> Section 3.3.5.5: SMB servers on Windows 
                    2000 Server,Windows Server 2003, Windows Server 2008, and Windows Server 2008 R2  return zero for 
                    the VolumeGUID and FileId. ");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                  @"Verify MS-SMB_R11005");

                //
                // Verify MS-SMB requirement: MS-SMB_R11005.
                //
                bool isVerifyR11005 = IsAllBytesZero(response.SmbParameters.VolumeGUID);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR11005,
                    11005,
                    @"Section 2.2.4.9.2: Windows 2000 Server, Windows Server 2003, 
                    Windows Server 2003 R2, and Windows Server 2008 set the VolumeGUID 
                    field to zero.");

                //
                // The following statement code will be run only when debugging.
                //
                // For VolumeGUID and FileId are both bytes array, they can't be added into log.
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9612");

                //
                // Verify MS-SMB requirement: MS-SMB_R9612.
                //
                bool isVerifyR9612 = IsAllBytesZero(response.SmbParameters.FileId);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR9612,
                    9612,
                    @"Section 2.2.4.9.2: Windows 2000 Server, Windows Server 2003,
                    Windows Server 2003 R2, and Windows Server 2008 set the FileId 
                    field to zero.");
            }

            if ((sutOsVersion == Platform.Win2K3R2) ||
                (sutOsVersion == Platform.Win2K8R2) ||
                (sutOsVersion == Platform.Win7) ||
                (sutOsVersion == Platform.WinVista) ||
                (sutOsVersion == Platform.WinXP))
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                             @"Verify MS-SMB_R9616,GuestMaximalAccessRights:{0}",
                             BytesToString(response.SmbParameters.FileId));

                //
                // Verify MS-SMB requirement: MS-SMB_R9616.
                //
                // If the FileId isn't null, it means the SUT has set this field.
                Site.CaptureRequirementIfIsNotNull(
                    response.SmbParameters.FileId,
                    9616,
                    @"<57> Section 2.2.4.9.2: Windows-based servers and clients support the notion of a guest 
                    accountand set this field appropriately for the defined guest account rights.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R118266");

                //
                // Verify MS-SMB requirement: MS-SMB_R118266.
                //
                // SmbNtCreateAndxResponsePacket is the extended response accroding to TD, So the response is an 
                // extended response.
                Site.CaptureRequirementIfAreEqual<byte>(
                    0x2A,
                    response.SmbParameters.WordCount,
                    118266,
                    @"[In Receiving an SMB_COM_NT_CREATE_ANDX Response]<106> Section 3.2.5.5: Windows-based servers 
                    will always set this value to 0x2A (42) when using an extended response.");
            }
        }

        #endregion

        #region Verify TRANS2_FIND_FIRST2 Client Request section 2.2.6.1.1.

        /// <summary>
        /// Verify TRAN2_FIND_FIRST2 Client Request.
        /// </summary>
        /// <param name="informationLevel">The InformationLevel in the Trans2Parameters of the 
        /// TRAN2_FIND_FIRST2 Client Request.</param>
        /// <param name="response">The TRAN2_FIND_FIRST2 Server Response.</param>
        /// <param name="isFindFullDirectoryInfoNull">To verify whether the struct of 
        /// SMB_FIND_FILE_FULL_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2 is null or not.</param>
        /// <param name="isFindBothDirectoryInfoNull">To verify whether the struct of 
        /// SMB_FIND_FILE_BOTH_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2 is null or not.</param>
        private void VerifyMessageSyntaxTrans2FindFirst2Request(
                      InformationLevel informationLevel,
                      SmbTrans2FindFirst2ResponsePacket response,
                      bool isFindFullDirectoryInfoNull,
                      bool isFindBothDirectoryInfoNull)
        {
            // SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO and SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO are the new 
            // informationLevel specified in section 2.2.2.3.1. 
            if ((informationLevel == InformationLevel.SmbFindFileIdBothDirectoryInfo)
               || (informationLevel == InformationLevel.SmbFindFileIdFullDirectoryInfo))
            {
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R2404 ,isfindIdFullDirectoryInfoNull is : {0}, 
                    isfinIdBothDirectoryInfoNull is {1}",
                    isFindFullDirectoryInfoNull,
                    isFindBothDirectoryInfoNull);

                //
                // Verify MS-SMB requirement: MS-SMB_R2404.
                //
                // If one of the new Information Levels structure isn't null, it means the structure has been used for 
                // the return of subsequent entries in the enumeration continuation.
                bool isVerifyR2404 = (isFindFullDirectoryInfoNull
                    || isFindBothDirectoryInfoNull);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR2404,
                    2404,
                    @"[In Receiving a TRANS2_FIND_NEXT2 Request New Information Levels] If the query is started using 
                    one of the new Information Levels, then as specified in section 2.2.2.3.1, the same Information 
                    Level structure MUST be used for the return of subsequent entries in the enumeration 
                    continuation.");

                if (isWindows)
                {
                    // 0x0105 is the number of SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO,
                    // 0x0106 is the number of SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO,
                    // These two numbers are the new Infromation Level.

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R30038");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R30038.
                    //
                    // 0xC00000BB is the errorcode STATUS_NOT_SUPPORTED's number. 
                    // According to TD, if the SUT doesn't support the new Information Levels, it will 
                    // return STATUS_NOT_SUPPORTED. Otherwise it wouldn't return STATUS_NOT_SUPPORTED.
                    Site.CaptureRequirementIfAreNotEqual<uint>(
                        0xC00000BB,
                        response.SmbHeader.Status,
                        30038,
                        @"[InReceiving a TRANS2_FIND_FIRST2 Request New Information Levels] <123> Section 
                        3.3.5.9.2:Windows servers support these new Information Levels for directory queries.");
                }
                if (informationLevel == InformationLevel.SmbFindFileIdBothDirectoryInfo)
                {
                    SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO[] payload =
                    (SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO[])response.Trans2Data.Data;
                    //
                    // Verify MS-SMB requirement: MS-SMB_R9487.
                    //
                    Site.CaptureRequirementIfAreEqual<CIFS.SmbFileAttributes32>(
                        CIFS.SmbFileAttributes32.FILE_ATTRIBUTE_DIRECTORY,
                        (CIFS.SmbFileAttributes32)payload[0].FileAttributes,
                        9487,
                        @"[In SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO Extensions]ExtFileAttributes (4 bytes):  Extended attributes 
                for this file that MUST be marked as a DIRECTORY.");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R9488.
                    //
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0,
                        payload[0].EaSize,
                        9488,
                        @"[In SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO Extensions]EaSize (4 bytes):  This field MUST be set to zero 
                when sending a response .");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R9491.
                    //
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0x00,
                        payload[0].Reserved,
                        9491,
                        @"[In SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO] Reserved (1 byte):  This member MUST be 0x00.");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R9493.
                    //
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0x0000,
                        payload[0].Reserved2,
                        9493,
                        @"[In SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO]Reserved2 (2 bytes):This member MUST be 0x0000.");


                    //
                    // Verify MS-SMB requirement: MS-SMB_R3478.
                    //
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0x0000,
                        payload[0].Reserved2,
                        3478,
                        @"[In SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO]Reserved2 (2 bytes):This member MUST be 0x0000.");


                    //
                    // Verify MS-SMB requirement: MS-SMB_R9483.
                    //
                    Site.CaptureRequirementIfAreEqual<ulong>(
                        0x00000000,
                        payload[0].FileIndex,
                        9483,
                        @"[In SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO]FileIndex (4 bytes):  This field MUST be set to zero when sending a response.");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R9485.
                    //
                    Site.CaptureRequirementIfAreEqual<ulong>(
                        0x00000000,
                        payload[0].AllocationSize,
                        9485,
                        @"[In SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO]AllocationSize (8 bytes):  This LARGE_INTEGER field MUST be set to zero when sending a response.");

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R9495");

                    // If Field isn't unique, it will throw exception and the case will stop and won't come here.
                    // So this requirement has been verified by the protocol SDK.
                    Site.CaptureRequirement(
                        9495,
                        @"[In SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO]FileId (8 bytes):This number MUST be unique 
                        for each file on a given volume.");

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R3480");

                    // If Field isn't unique, it will throw exception and the case will stop and won't come here.
                    // So this requirement has been verified by the protocol SDK.
                    Site.CaptureRequirement(
                        3480,
                        @"[In SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO]FileId (8 bytes):This number MUST be unique 
                        for each file on a given volume.");
                }
            }
        }

        #endregion

        #region Verify NT_TRANSACT_CREATE section 2.2.7.1.

        /// <summary>
        /// Verify impersonationLevel of SmbNtCreateAndxRequestPacket request.
        /// </summary>
        /// <param name="impersonationLevel">The impersonationLevel in the SMBParameter of the 
        /// SmbNtCreateAndxRequestPacket request from client.</param>
        private void VerifyMessageSyntaxNtTransactCreateRequest(int impersonationLevel)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9329,impersonationLevel:{0}",
                impersonationLevel);

            //
            // Verify MS-SMB requirement: MS-SMB_R9329.
            //
            bool isVerifyR9329 = ((0x00000000 == impersonationLevel)
                || (0x00000001 == (uint)impersonationLevel)
                || (0x00000002 == (uint)impersonationLevel)
                || (0x00000003 == (uint)impersonationLevel));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR9329,
                9329,
                "[In NT_TRANSACT_CREATE (0x0001) Extensions]This field MUST be set to one of the following values [" +
                "SECURITY_ANONYMOUS 0x00000000," +
                "SECURITY_IDENTIFICATIO 0x00000001," +
                "SECURITY_IMPERSONATION 0x00000002," +
                "SECURITY_DELEGATION 0x00000003.]");
        }

        #endregion

        #region Verify FSCTL_SRV_ENUMERATE_SNAPSHOTS Server Response section 2.2.7.2.2.1.

        /// <summary>
        /// Verify FSCTL_SRV_ENUMERATE_SNAPSHOTS Server Response.
        /// </summary>
        /// <param name="response">The FSCTL_SRV_ENUMERATE_SNAPSHOTS Server Response.</param>
        private void VerifyMessageSyntaxFsctlSrvEnumerateSnapshotsRequest(
            SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket response)
        {
            if (response.NtTransData.snapShotMultiSZ != null)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9403,the length of snapShotMultiSZ is {0}",
                    response.NtTransData.snapShotMultiSZ.Length);

                //
                // Verify MS-SMB requirement: MS-SMB_R9403.
                //
                // The snapshots take the form "@GMT-YYYY.MM.DD-HH.MM.SS." and are terminated by two additional 16-bit 
                // unicode NULL characters, so one snapshot is 50 bytes.The byte length divide 50 is the number 
                // of returned snapshots.
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)response.NtTransData.snapShotMultiSZ.Length / 50,
                    response.NtTransData.NumberOfSnapShotsReturned,
                    9403,
                    @"[In FSCTL_SRV_ENUMERATE_SNAPSHOTS Response]NumberOfSnapShotsReturned (4 bytes):  This value MUST 
                    be the number of snapshots that are returned in this response.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9407,SnapShotMultiSZ:{0}",
                    BitConverter.ToString(response.NtTransData.snapShotMultiSZ));

                //
                // Verify MS-SMB requirement: MS-SMB_R9407.
                //
                bool isVerifyR9407 = true;

                // Check if every snapshot is null-terminated.
                // One snapshot is 50 bytes, and the last two bytes should be '\0'.
                for (int i = 0; i < response.NtTransData.NumberOfSnapShotsReturned; ++i)
                {
                    if (!((response.NtTransData.snapShotMultiSZ[i * 50 + 48] == '\0')
                        && (response.NtTransData.snapShotMultiSZ[i * 50 + 49] == '\0')))
                    {
                        isVerifyR9407 = false;
                        break;
                    }
                }

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR9407,
                    9407,
                    @"[In FSCTL_SRV_ENUMERATE_SNAPSHOTS Response]SnapShotMultiSZ (variable):  Each snapshot MUST be 
                    encoded as a NULL-terminated sequence of 16-bit Unicode characters.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9408,snapShotMultisz:{0}",
                    BitConverter.ToString(response.NtTransData.snapShotMultiSZ));

                //
                // Verify MS-SMB requirement: MS-SMB_R9408.
                //
                bool isVerifyR9408 = true;

                // Check if every snapshot takes the form: @GMT-YYYY.MM.DD-HH.MM.SS.
                for (int i = 0; i < response.NtTransData.NumberOfSnapShotsReturned; ++i)
                {
                    if (!IsSnapShotValid(response.SnapShots[i]))
                    {
                        isVerifyR9408 = false;
                        break;
                    }
                }

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR9408,
                    9408,
                    @"[In FSCTL_SRV_ENUMERATE_SNAPSHOTS Response]SnapShotMultiSZ (variable):  Each snapshot MUST take 
                    on the following form: @GMT-YYYY.MM.DD-HH.MM.SS.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9409,SnapShotMultiSZ:{0}",
                    BitConverter.ToString(response.NtTransData.snapShotMultiSZ));

                //
                // Verify MS-SMB requirement: MS-SMB_R9409.
                //
                bool isVerifyR9409 = true;

                // For every 50 snapShotMultiSZ combian composing a snapShot, if snapShot is unicode null 
                // characters, the last 2 snapShotMultiSZ must be '0'.
                for (int i = 0; i < response.NtTransData.NumberOfSnapShotsReturned; ++i)
                {
                    if (!((response.NtTransData.snapShotMultiSZ[i * 50 + 48] == '\0')
                        && (response.NtTransData.snapShotMultiSZ[i * 50 + 49] == '\0')))
                    {
                        isVerifyR9409 = false;
                        break;
                    }
                }

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR9409,
                    9409,
                    @"[In FSCTL_SRV_ENUMERATE_SNAPSHOTS Response]SnapShotMultiSZ (variable):  The concatenated list 
                    MUST be terminated by two additional 16-bit Unicode NULL characters.");

                if (response.NtTransData.NumberOfSnapShotsReturned == 0)
                {
                    //
                    // Verify requirement MS-SMB_R9410 and MS-SMB_R109410
                    //
                    string isR9410Implementated = Site.Properties.Get("SHOULDMAYR9410Implementation");
                    bool isR109410Satisfied = (('\0' == response.NtTransData.snapShotMultiSZ[0])
                        && ('\0' == response.NtTransData.snapShotMultiSZ[1]));

                    if (isWindows)
                    {
                        //
                        // The following statement code will be run only when debugging.
                        //
                        Site.Log.Add(LogEntryKind.Debug,
                            @"Verify MS-SMB_R109410,SnapShotMultiSZ:{0}",
                            BitConverter.ToString(response.NtTransData.snapShotMultiSZ));

                        //
                        // Verify MS-SMB requirement: MS-SMB_R109410.
                        //
                        Site.CaptureRequirementIfIsTrue(
                            isR109410Satisfied,
                            109410,
                            @"[In FSCTL_SRV_ENUMERATE_SNAPSHOTS Response]SnapShotMultiSZ (variable):  If the response 
                            contains no snapshots,then the server  sets this field to two 16-bit Unicode NULL 
                            characters in Windows.");

                        if (null == isR9410Implementated)
                        {
                            Site.Properties.Add("SHOULDMAYR9410Implementation", Boolean.TrueString);
                            isR9410Implementated = Boolean.TrueString;
                        }
                    }

                    if (null != isR9410Implementated)
                    {
                        bool implemented = Boolean.Parse(isR9410Implementated);
                        bool isSatisfied = isR109410Satisfied;

                        //
                        // The following statement code will be run only when debugging.
                        //
                        Site.Log.Add(LogEntryKind.Debug,
                            @"Verify MS-SMB_R9410,SnapShotMultiSZ:{0}",
                            BitConverter.ToString(response.NtTransData.snapShotMultiSZ));

                        //
                        // Verify MS-SMB requirement: MS-SMB_R9410.
                        //
                        Site.CaptureRequirementIfAreEqual<Boolean>(
                            implemented,
                            isSatisfied,
                            9410,
                            String.Format("[In FSCTL_SRV_ENUMERATE_SNAPSHOTS Response]SnapShotMultiSZ (variable): If " +
                            "the response contains no snapshots, then the server SHOULD set this field to two 16-bit " +
                            "Unicode NULL characters. This requirement is {0}implemented", implemented ? "" : "not "));
                    }
                }
            }
        }

        #endregion

        #region Verify FSCTL_SRV_REQUEST_RESUME_KEY Response section 2.2.7.2.2.2.

        /// <summary>
        /// Verify FSCTL_SRV_REQUEST_RESUME_KEY Response.
        /// </summary>
        /// <param name="response">The FSCTL_SRV_REQUEST_RESUME_KEY Response.</param>
        private void VerifyMessageSyntaxFsctlSrvRequestResumeKeyResponse(
            SmbNtTransFsctlSrvRequestResumeKeyResponsePacket response)
        {
            //
            // Verify requirement MS-SMB_R9418 and MS-SMB_R109418
            //
            string isR9418Implementated = Site.Properties.Get("SHOULDMAYR9418Implementation");
            bool isR109418Satisfied = (4 == Marshal.SizeOf(response.NtTransData.ContextLength))
                && (0 == response.NtTransData.ContextLength);

            if (isWindows)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R109418, ContextLength:{0}",
                    response.NtTransData.ContextLength);

                //
                // Verify MS-SMB requirement: MS-SMB_R109418.
                //
                Site.CaptureRequirementIfIsTrue(
                    isR109418Satisfied,
                    109418,
                    @"[In FSCTL_SRV_REQUEST_RESUME_KEY Response]ContextLength (4 bytes):  Since this feature is not 
                    used, this field is set to zero by the server in Windows.");

                if (null == isR9418Implementated)
                {
                    Site.Properties.Add("SHOULDMAYR9418Implementation", Boolean.TrueString);
                    isR9418Implementated = Boolean.TrueString;
                }
            }

            if (null != isR9418Implementated)
            {
                bool implemented = Boolean.Parse(isR9418Implementated);
                bool isSatisfied = isR109418Satisfied;

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9418, ContextLength:{0}",
                    response.NtTransData.ContextLength);

                //
                // Verify MS-SMB requirement: MS-SMB_R9418.
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implemented,
                    isSatisfied,
                    9418,
                    String.Format("[In FSCTL_SRV_REQUEST_RESUME_KEY Response]ContextLength (4 bytes):  Since this " +
                    "feature is not used,this field SHOULD be set to zero by the server. " +
                    "This requirement is {0}implemented", implemented ? "" : "not "));
            }

            //
            // Verify requirement MS-SMB_R9421 and MS-SMB_R9578
            //
            string isR9421Implementated = Site.Properties.Get("SHOULDMAYR9421Implementation");

            // If Context.length=0, it means that Context is contained in the packet and is initialized.
            bool isR9578Satisfied = (response.NtTransData.Context.Length == 0);

            if (isWindows)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9578,Context.Length:{0}",
                    response.NtTransData.Context.Length);

                //
                // Verify MS-SMB requirement: MS-SMB_R9578.
                //
                Site.CaptureRequirementIfIsTrue(
                    isR9578Satisfied,
                    9578,
                    @"<73> Section 2.2.7.2.2.2: Windows based servers include the Context field but do not initialize 
                    it.");

                if (null == isR9421Implementated)
                {
                    Site.Properties.Add("SHOULDMAYR9421Implementation", Boolean.TrueString);
                    isR9421Implementated = Boolean.TrueString;
                }
            }

            if (null != isR9421Implementated)
            {
                bool implemented = Boolean.Parse(isR9421Implementated);
                bool isSatisfied = (response.NtTransData.Context.Length == 0);

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9421,Context.Length:{0}",
                    response.NtTransData.Context.Length);

                //
                // Verify MS-SMB requirement: MS-SMB_R9421.
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implemented,
                    isSatisfied,
                    9421,
                    String.Format("[In FSCTL_SRV_REQUEST_RESUME_KEY Response]Context (variable): Since this feature is " +
                    "not used, this field SHOULD NOT<74> be included in the response. " +
                    "This requirement is {0}implemented", implemented ? "" : "not "));
            }
        }

        #endregion

        #region Verify FSCTL_SRV_COPYCHUNK Server Response section 2.2.7.2.2.3.

        /// <summary>
        /// Verify FSCTL_SRV_COPYCHUNK Server Response.
        /// </summary>
        /// <param name="response">The FSCTL_SRV_COPYCHUNK Server Response.</param>
        private void VerifyMessageSyntaxFsctlSrvCopychunkResponse(
            SmbNtTransFsctlSrvCopyChunkResponsePacket response)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9424");

            //
            // Verify MS-SMB requirement: MS-SMB_R9424.
            //
            // The latter half of the requirement is explaining the content of ChunksWritten field and is informative.
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(response.NtTransData.ChunksWritten),
                9424,
                @"[In FSCTL_SRV_COPYCHUNK Response]ChunksWritten (4 bytes): This field MUST represent the number of 
                copychunk operations successfully processed by the server.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9426");

            //
            // Verify MS-SMB requirement: MS-SMB_R9426.
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                0,
                response.NtTransData.ChunkBytesWritten,
                9426,
                @"[In FSCTL_SRV_COPYCHUNK Response]ChunkBytesWritten (4 bytes):  This field MUST be set to zero by the 
                server.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R3415");

            //
            // Verify MS-SMB requirement: MS-SMB_R3415.
            //
            // The latter half of the requirement is explaining the content of TotalBytesWritten field and is 
            // informative.
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(response.NtTransData.ChunkBytesWritten),
                3415,
                @"[In FSCTL_SRV_COPYCHUNK Response]TotalBytesWritten (4 bytes): This field MUST represent the total 
                number of bytes written to the destination file across all copychunk operations.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R5008");

            // The requirement about FSCTL_SRV_COPYCHUNK request have been verified in R9424, R9426, R3415, this 
            // requirement can be verified directly.
            Site.CaptureRequirement(
                5008,
                @"[In Receiving an FSCTL_SRV_COPYCHUNK Request]If the timeout restriction is not enforced, then the 
                server MUST process  all of the chunks that are described by the request, as specified in section 
                2.2.7.2.2.<127>");
        }

        #endregion

        #region Verify NT_TRANSACT_QUERY_QUOTA Server Response section 2.2.7.3.2.

        /// <summary>
        /// Verify NT_TRANSACT_QUERY_QUOTA Server Response.
        /// </summary>
        /// <param name="response">The NT_TRANSACT_QUERY_QUOTA Server Response.</param>
        /// <param name="sidListLength">The SidListLength int of the NTTransParameter of the NT_TRANSACT_QUERY_QUOTA 
        /// Client Request.</param>
        /// <param name="startSidLength">The startSidLength int of the NTTransParameter of the NT_TRANSACT_QUERY_QUOTA 
        /// Client Request.</param>
        /// <param name="quotaInfoCount">The number of the quota information for all SIDs.</param>
        /// <param name="ntTranMaxDataCount">The max number of the quota information for all SIDs.</param>
        /// Disable warning CA1801 because according to Test Case design, 
        /// the parameter quotaInfoCount is used for extension.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        private void VerifyMessageSyntaxNtTransactQueryQuotaResponse(
            SmbNtTransQueryQuotaResponsePacket response,
            int sidListLength,
            int startSidLength,
            int quotaInfoCount,
            uint ntTranMaxDataCount)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9454");

            //
            // Verify MS-SMB requirement: MS-SMB_R9454.
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                response.SmbParameters.TotalDataCount,
                response.NtTransParameters.QuotaDataSize,
                9454,
                @"[In Server Response]DataLength (4 bytes):  This field MUST be equal to the SMB_Parameters.Words.
                TotalDataCount field.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R30016");

            //
            // Verify MS-SMB requirement: MS-SMB_R30016.
            //
            // Copying is the server behavior. If "Status" isn't null, it means the SUT has copied 
            // "Status" into reponse.
            Site.CaptureRequirementIfIsNotNull(
                response.SmbHeader.Status,
                30016,
                @"[In Receiving an NT_TRANS_QUERY_QUOTA Request] <128> Section 3.3.5.10.2: The returned Status is 
                copied into the SMB_Header.Status field of the response. ");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R2464, the Count is: {0}",
                response.NtTransDataList.Count);

            //
            // Verify MS-SMB requirement: MS-SMB_R2464.
            //
            Site.CaptureRequirementIfIsTrue(
                response.NtTransDataList.Count <= ntTranMaxDataCount,
                2464,
                @"The server MUST return as much of the available quota information that is able to fit in the maximum 
                response buffer size denoted by MaxDataCount.");

            // This is R9433's condition: Whether both[NT_Trans_Parameters.SidListLength and 
            // NT_Trans_Parameters.StartSidLength] are zero.
            if ((sidListLength == 0) && (startSidLength == 0))
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9433, response.NtTransDataList: {0}",
                    response.NtTransDataList);

                // If Count isn't null, it means the SUT has enumerated the quota information.
                //
                // Verify MS-SMB requirement: MS-SMB_R9433.
                //
                Site.CaptureRequirementIfIsNotNull(
                    response.NtTransDataList,
                    9433,
                    @"[In Client Request]If both[NT_Trans_Parameters.SidListLength and  
                    NT_Trans_Parameters.StartSidLength] are zero, then the quota information for all SIDs on the 
                    underlying object store of a share MUST be enumerated by the server.");
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R2465,NtTransDataList.Count{0}",
                response.NtTransDataList.Count);

            //
            // Verify MS-SMB requirement: MS-SMB_R2465.
            //
            // The SUT place the quota information in the NtTransDataList.
            bool isVerifyR2465 = response.NtTransDataList.Count >= 0;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR2465,
                2465,
                @"The server MUST place the quota information in the response, as specified in section 2.2.7.3.2, and 
                send the response back to the client.");
        }

        #endregion

        #region Verify NT_TRANSACT_SET_QUOTA Server Response section 2.2.7.4.2.

        /// <summary>
        /// Verify NT_TRANSACT_SET_QUOTA Server Response.
        /// </summary>
        /// <param name="response">The NT_TRANSACT_SET_QUOTA Server Response.</param>
        /// <param name="returnedStatus">The returned status.</param>
        private void VerifyMessageSyntaxNtTransactSetQuotaRequest(
            SmbNtTransSetQuotaResponsePacket response,
            uint returnedStatus)
        {
            if (isWindows)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R30006");

                //
                // Verify MS-SMB requirement: MS-SMB_R30006.
                //
                Site.CaptureRequirementIfAreEqual<uint>(
                    returnedStatus,
                    response.SmbHeader.Status,
                    30006,
                    @"[In Receiving an NT_TRANS_SET_QUOTA Request] <129> Section 3.3.5.10.3: The returned Status is 
                    copied into the SMB_Header.Status field of the response.");
            }
        }

        #endregion

        #region Verify SMB_FIND_FILE_BOTH_DIRECTORY_INFO section 2.2.8.1.1.

        /// <summary>
        /// Verify SMB_FIND_FILE_BOTH_DIRECTORY_INFO structure.
        /// </summary>
        /// <param name="info">A SMB_FIND_FILE_BOTH_DIRECTORY_INFO structure.</param>
        private void VerifyMessageSyntaxSmbFindFileBothDirectoryInfo(FSCC.FileBothDirectoryInformation info)
        {
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R1755");

            //
            // Verify MS-SMB requirement: MS-SMB_R1755.
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                0,
                info.FileCommonDirectoryInformation.FileIndex,
                1755,
                @"[In SMB_FIND_FILE_BOTH_DIRECTORY_INFO Extensions]FileIndex (4 bytes):  This field MUST be set to zero 
                when sending a response.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R1757");

            //
            // Verify MS-SMB requirement: MS-SMB_R1757.
            //
            Site.CaptureRequirementIfAreEqual<long>(
                0,
                info.FileCommonDirectoryInformation.EndOfFile,
                1757,
                @"[In SMB_FIND_FILE_BOTH_DIRECTORY_INFO Extensions]EndOfFile (8 bytes):  This LARGE_INTEGER field MUST 
                be set to zero when sending a response.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R1759");

            //
            // Verify MS-SMB requirement: MS-SMB_R1759.
            //
            Site.CaptureRequirementIfAreEqual<long>(
                0,
                info.FileCommonDirectoryInformation.AllocationSize,
                1759,
                @"[In SMB_FIND_FILE_BOTH_DIRECTORY_INFO Extensions]AllocationSize (8 bytes):  This LARGE_INTEGER field 
                MUST be set to zero when sending a response.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R1776");

            //
            // Verify MS-SMB requirement: MS-SMB_R1776.
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                0,
                info.EaSize,
                1776,
                @"[In SMB_FIND_FILE_BOTH_DIRECTORY_INFO Extensions]EaSize (4 bytes):  This field MUST be set to zero 
                when sending a response .");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R1775");

            //
            // Verify MS-SMB requirement: MS-SMB_R1775.
            //
            Site.CaptureRequirementIfAreEqual<CIFS.SmbFileAttributes32>(
                CIFS.SmbFileAttributes32.FILE_ATTRIBUTE_DIRECTORY,
                (CIFS.SmbFileAttributes32)info.FileCommonDirectoryInformation.FileAttributes,
                1775,
                @"[In SMB_FIND_FILE_BOTH_DIRECTORY_INFO Extensions]ExtFileAttributes (4 bytes):  Extended attributes 
                for this file that MUST be marked as a DIRECTORY.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R3474");

            //
            // Verify MS-SMB requirement: MS-SMB_R3474.
            //
            Site.CaptureRequirementIfAreEqual<byte>(
                0x00,
                info.Reserved,
                3474,
                @"[In SMB_FIND_FILE_BOTH_DIRECTORY_INFO Extensions]Reserved (1 byte):  This field MUST be 0x00.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R3476");

            //
            // Verify MS-SMB requirement: MS-SMB_R3476.
            //
            // ShortName is defined as short[], so here need to use info.ShortName.Length * 
            // Marshal.SizeOf (info.ShortName[0]) to represents the byte-length of ShortName.
            Site.CaptureRequirementIfAreEqual<int>(
                24,
                info.ShortName.Length * Marshal.SizeOf(info.ShortName[0]),
                3476,
                @"[In SMB_FIND_FILE_BOTH_DIRECTORY_INFO Extensions]ShortName (24 bytes):The ShortName field MUST be 
                formatted as an array of 16-bit Unicode characters");

            bool nullTerminated = (info.ShortNameLength > 0) && (info.ShortName[info.ShortNameLength - 1] == 0) && (info.ShortName[info.ShortNameLength - 2] != 0);

            //
            // Verify MS-SMB requirement: MS-SMB_R103476.
            //

            Site.CaptureRequirementIfIsFalse(
                nullTerminated,
                103476,
                @"[In SMB_FIND_FILE_BOTH_DIRECTORY_INFO Extensions]ShortName 
                (24 bytes):The ShortName field MUST NOT be NULL terminated.<2>");
        }

        #endregion

        #region Verify SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO section 2.2.8.1.2.

        /// <summary>
        /// Verify SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO Structure.
        /// </summary>
        /// <param name="info">A SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO Structure.</param>
        private void VerifyMessageSyntaxSmbFindFileIdFullDirectoryInfo(FSCC.FileFullDirectoryInformation info)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9469");

            //
            // Verify MS-SMB requirement: MS-SMB_R9469.
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                0,
                info.FileCommonDirectoryInformation.FileIndex,
                9469,
                @"[In SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO]FileIndex (4 bytes):  This field MUST be set to zero when 
                sending a response.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9471");

            //
            // Verify MS-SMB requirement: MS-SMB_R9471.
            //
            Site.CaptureRequirementIfAreEqual<long>(
                0,
                info.FileCommonDirectoryInformation.EndOfFile,
                9471,
                @"[In SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO]EndOfFile (8 bytes):  This LARGE_INTEGER field MUST be set 
                to zero when sending a response.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9473");

            //
            // Verify MS-SMB requirement: MS-SMB_R9473
            //
            Site.CaptureRequirementIfAreEqual<long>(
                0,
                info.FileCommonDirectoryInformation.AllocationSize,
                9473,
                @"[In SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO]AllocationSize (8 bytes):  This LARGE_INTEGER field MUST be 
                set to zero when sending a response.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9477");

            //
            // Verify MS-SMB requirement: MS-SMB_R9477.
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                0,
                info.EaSize,
                9477,
                @"[In SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO]EaSize (4 bytes):  This field MUST be set to zero when 
                sending a response.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9475");

            //
            // Verify MS-SMB requirement: MS-SMB_R9475.
            //
            Site.CaptureRequirementIfAreEqual<CIFS.SmbFileAttributes32>(
                CIFS.SmbFileAttributes32.FILE_ATTRIBUTE_DIRECTORY,
                (CIFS.SmbFileAttributes32)info.FileCommonDirectoryInformation.FileAttributes,
                9475,
                @"[In SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO]ExtFileAttributes (4 bytes):  Extended attributes for this 
                file that MUST be marked as a DIRECTORY.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9480");

            // If the FileID isn't unique, it will throw exception and the case will stop and won't come here.
            // So this requirement has been verified by the protocol SDK.
            Site.CaptureRequirement(
                9480,
                @"[In SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO]This number MUST be unique for each file on a given volume.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9481");

            //
            // Verify MS-SMB requirement: MS-SMB_R9481
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                0,
                info.FileCommonDirectoryInformation.FileIndex,
                9481,
                @"[In SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO]Fileindex (4 bytes):  This  field MUST be 
                set to zero when sending a response.");
        }

        #endregion

        #region Verify Transport section 2.1.

        /// <summary>
        /// Verify the Transport type of the protocol.
        /// </summary>
        /// <param name="packet">A packet to be verified.</param>
        /// Disable warning CA1801 because according to Test Case design, this parameter is used for extension.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        private void VerifyTransport(CIFS.SmbPacket packet)
        {
            if (this.transport == TransportType.DirectTcp)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R1");

                // If the transport type is DirectTCP, a connection between the SUT and the client on a 
                // TCP port is ensured by the protocol SDK. Here capture it directly.
                Site.CaptureRequirement(
                    1,
                    @"[In Direct TCP Transport]When using Direct TCP as the SMB transport, the implementer MUST 
                    establish a TCP connection from an SMB client to a TCP port on the server.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R5076");

                // If the transportType is DirectTCP, it means the protocol supports TCP, so capture the requirement 
                // directly.
                Site.CaptureRequirement(
                    5076,
                    @"[In Transport]In addition to the transport protocols listed in section 2.1 of [MS-CIFS], the
                    extended version of the protocol supports the use of TCP as a transport layer.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R205076");

                // If the transportType is DirectTCP, it means the protocol supports TCP, so capture the requirement 
                // directly.
                Site.CaptureRequirement(
                    205076,
                    @"[In Transport]Hereafter, the special TCP-related characteristics that are employed in the
                    application of SMB over TCP are known as the Direct TCP transport.<2>");

                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R5");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R5.
                    //
                    Site.CaptureRequirementIfAreEqual<string>(
                        "445",
                        Site.Properties.Get("SutPort"),
                        5,
                        @"<3> Section 2.1: Windows-based clients and servers use TCP port 445 as the destination TCP 
                        port on the SMB server, the well-known port number assigned by IANA to Microsoft-DS.");
                }

                //
                // Verify requirement MS-SMB_R4 and MS-SMB_R10004
                //
                string isR4Implementated = Site.Properties.Get("SHOULDMAYR4Implementation");
                bool isR10004Satisfied = ("445" == Site.Properties.Get("SutPort"));

                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R10004,port:{0}",
                        Site.Properties.Get("SutPort"));

                    //
                    // Verify MS-SMB requirement: MS-SMB_R10004.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR10004Satisfied,
                        10004,
                        @"[In Direct TCP Transport]The SMB server listens for connections on port 445 in Windows. ");

                    if (null == isR4Implementated)
                    {
                        Site.Properties.Add("SHOULDMAYR4Implementation", Boolean.TrueString);
                        isR4Implementated = Boolean.TrueString;
                    }
                }

                if (null != isR4Implementated)
                {
                    bool implemented = Boolean.Parse(isR4Implementated);
                    bool isSatisfied = isR10004Satisfied;

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R4,port:{0}",
                        Site.Properties.Get("SutPort"));

                    //
                    // Verify MS-SMB requirement: MS-SMB_R4.
                    //
                    Site.CaptureRequirementIfAreEqual<Boolean>(
                        implemented,
                        isSatisfied,
                        4,
                        String.Format("[In Direct TCP Transport]The SMB server SHOULD listen for connections on port" +
                        "445. This requirement is {0}implemented", implemented ? "" : "not "));
                }
                if (null != packet.TransportHeader.StreamProtocolLength)
                {
                    //
                    // Verify MS-SMB requirement: MS-SMB_R5095.
                    //

                    Site.CaptureRequirementIfAreEqual<int>(
                       3,
                       packet.TransportHeader.StreamProtocolLength.Length,
                       5095,
                       @"[In Message Syntax]Stream Protocol Length (3 bytes): This length 
                        is formatted as a 3-byte integer in network byte order). ");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R6.
                    //
                    // TransportHeader length is sum of length of StreamProtocolLength field and Zero field(always 1) 
                    int transportHeaderLength = packet.TransportHeader.StreamProtocolLength.Length + 1;
                    Site.CaptureRequirementIfAreEqual<int>(
                        4, transportHeaderLength,
                        6,
                        @"[In Direct TCP Transport]When using Direct 
                        TCP as the SMB transport, the implementer MUST 
                        prepend a 4-byte Direct TCP transport packet 
                        header to each SMB message.");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R7.
                    //
                    // TransportHeader.StreamProtocolLength indicates the length of the SMB message 
                    // It's parsed by protocol SDK.
                    Site.CaptureRequirementIfAreEqual<int>(
                        3, packet.TransportHeader.StreamProtocolLength.Length,
                        7,
                        @"[In Direct TCP Transport]This transport header 
                        [the 4-byte Direct TCP Transport packet header] 
                        MUST be formatted as a byte of zero (8 zero bits) 
                        followed by 3 bytes that indicate the length of 
                        the SMB message that is encapsulated.");

                    //
                    // Verify MS-SMB requirement: MS-SMB_R8.
                    //
                    // TransportHeader.Zero is the first byte of TransportHeader
                    // It's parsed by protocol SDK.
                    Site.CaptureRequirementIfAreEqual<int>(
                        0, packet.TransportHeader.Zero,
                        8,
                        @"[In Direct TCP Transport]Zero (1 byte):
                        The first byte of the Direct TCP transport 
                        packet header MUST be zero (0x00).");
                }
            }
        }

        #endregion

        #region Verify Common Message Syntax section 2.2.

        /// <summary>
        /// Verify The Common Message Syntax.
        /// </summary>
        /// <param name="packet">A smbPacket.</param>
        private void VerifyCommonMessageSyntax(CIFS.SmbPacket packet)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R105076");

            // The implement of CIFS is assured by the protocol SDK. Here capture it directly.
            Site.CaptureRequirement(
                105076,
                @"[In Messages]An SMB version 1.0 Protocol implementation MUST implement CIFS, as specified by 
                section 2 of the [MS-CIFS] specification. ");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R5219");

            // The organization of data is decided by protocol SDK,whether little-endian byte order or big-endian 
            // byte order, protocol SDK has ensured that data is organized in little-endian byte order. So here capture 
            // the requirement directly.
            Site.CaptureRequirement(
                5219,
                "[In Message Syntax]Unless otherwise specified, multi-byte fields (that is, 16-bit, 32-bit, and 64-bit" +
                "fields) in an SMB message MUST be transmitted in little-endian byte order" +
                "(least significant byte first).");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R75");

            // Once all the length and items of smbHeader are checked, this requirement is covered.
            Site.CaptureRequirement(
                 75,
                 @"All SMB messages MUST begin with a fixed-length SMB header 
                (as specified in [MS-CIFS], section 2.2.1). ");

            //
            // Verify requirement MS-SMB_R77 and MS-SMB_R100077
            //
            string isR77Implementated = Site.Properties.Get("SHOULDMAYR77Implementation");
            bool isR100077Satisfied = (packet.SmbHeader.Reserved == 0);

            if (isWindows)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R100077,Reserved:{0}",
                    packet.SmbHeader.Reserved);

                //
                // Verify MS-SMB requirement: MS-SMB_R100077.
                //
                Site.CaptureRequirementIfIsTrue(
                    isR100077Satisfied,
                    100077,
                    @"[In Message Syntax]Unless otherwise noted, fields marked as Reserved is set to 0 when being sent 
                    in Windows.");

                if (null == isR77Implementated)
                {
                    Site.Properties.Add("SHOULDMAYR77Implementation", Boolean.TrueString);
                    isR77Implementated = Boolean.TrueString;
                }
            }

            if (null != isR77Implementated)
            {
                bool implemented = Boolean.Parse(isR77Implementated);
                bool isSatisfied = isR100077Satisfied;

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R77,Reserved:{0}",
                    packet.SmbHeader.Reserved);

                //
                // Verify MS-SMB requirement: MS-SMB_R77.
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implemented,
                    isSatisfied,
                    77,
                    String.Format("[In Message Syntax]Unless otherwise noted, fields marked as Reserved SHOULD be set" +
                    "to 0 when being sent. This requirement is {0}implemented", implemented ? "" : "not "));
            }
        }

        #endregion

        #region Verify Extended File Attribute section 2.2.1.2.1.

        /// <summary>
        /// Verify Extended File Attribute.
        /// </summary>
        /// <param name="fileAttribute">File Attribute need to be verified.</param>
        /// <param name="isNotContentIndexed">If file or directory is not indexed by a content indexing service.</param>
        /// Disable warning CA1811 because according to Test Case design, this method is used for extension.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void VerifyExtendedFileAttribute(
            uint fileAttribute,
            bool isNotContentIndexed)
        {
            if ((fileAttribute & 0x00002000) == 0x00002000)
            {
                //
                // Verify requirement MS-SMB_R9024 and MS-SMB_R109024
                //
                string isR9024Implementated = Site.Properties.Get("SHOULDMAYR9024Implementation");
                bool isR109024Satisfied = isNotContentIndexed;
                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R109024,fileAttribute:{0}",
                        fileAttribute);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R109024.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR109024Satisfied,
                        109024,
                        @"[In Extended File Attribute (SMB_EXT_FILE_ATTR) Extensions]
                        ATTR_NOT_CONTENT_INDEXED 0x00002000 means File or directory is not indexed by a content 
                        indexing service in Windows.");

                    if (null == isR9024Implementated)
                    {
                        Site.Properties.Add("SHOULDMAYR9024Implementation", Boolean.TrueString);
                        isR9024Implementated = Boolean.TrueString;
                    }
                }

                if (null != isR9024Implementated)
                {
                    bool implemented = Boolean.Parse(isR9024Implementated);
                    bool isSatisfied = isR109024Satisfied;

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R9024,fileAttribute:{0}",
                        fileAttribute);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R9024.
                    //
                    Site.CaptureRequirementIfAreEqual<Boolean>(
                        implemented,
                        isSatisfied,
                        9024,
                        String.Format("[In Extended File Attribute (SMB_EXT_FILE_ATTR) Extensions] " +
                        "ATTR_NOT_CONTENT_INDEXED 0x00002000 means File or directory SHOULD NOT be indexed by a content " +
                        "indexing service. This requirement is {0}implemented", implemented ? "" : "not "));
                }
            }
        }

        #endregion

        #region Verify Receiving an SMB_COM_TREE_CONNECT_ANDX Request section 3.3.5.4.

        /// <summary>
        /// Verify Receiving an SMB_COM_TREE_CONNECT_ANDX Request.
        /// </summary>
        /// <param name="response">The SMB_COM_TREE_CONNECT_ANDX Response.</param>
        /// <param name="isRequestExtResponse">The extended requestor's Response.</param>
        /// <param name="computedMaxRights">The computed MaximalShareAccessRights.</param>
        /// <param name="computedOptionalSupport">The computed server share OptionalSupport.</param>
        /// <param name="isGuestAccountSupported"> If the system support the guest account.</param>
        /// <param name="isSupportExtSignature">If it's the extended Information.</param>
        /// Disable warning CA1801 because according to Test Case design, 
        /// computedMaxRights and computedOptionalSupport are used for extension.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        private void VerifyReceiveSmbComTreeConnectAndXRequest(
            SmbTreeConnectAndxResponsePacket response,
            bool isRequestExtResponse,
            uint computedMaxRights,
            ushort computedOptionalSupport,
            bool isGuestAccountSupported,
            bool isSupportExtSignature)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R8300");

            // The implementation of CIFS is ensured by protocol SDK, and it couldn't be expressed in code.
            // Here capture it direcly.
            Site.CaptureRequirement(
                8300,
                @"[In Protocol Details] An SMB implementation MUST implement CIFS, as specified by section 3 of the 
                [MS-CIFS] specification.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R30109");

            // Since the algorithm is coded by protocol SDK, it will make sure that the SUT calculate the 
            // maximal share access rights for the user, which requests the tree connect using the algorithm described 
            // in the TD. Here capture this requirement directly.
            Site.CaptureRequirement(
                30109,
                @"[In Receiving an SMB_COM_TREE_CONNECT_ANDX Request] Requesting Extended Information: The server MUST 
                calculate the maximal share access rights for the user that requests the tree connect using the 
                following algorithm 
                [MaxRights = 0x00000000
                IF Server.Share.SecurityDescriptor == NULL
                  MaxRights = 0xFFFFFFFF
                ELSE
                  FOR EACH AccessBit value defined in section 2.2.1.4
                    Compute access for the user, using Server.Share.SecurityDescriptor and 
                     Server.Session.SecurityContext, as described in [MS-DTYP] section 2.5.2.1.
                    IF access was granted
                      MaxRights = MaxRights | AccessBit;
                    END IF
                  END FOR
                END IF].");

            // If isSupportExtSignature is TRUE, it means it's the Extended Information.
            if (isSupportExtSignature)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R30110");

                //
                // Verify MS-SMB requirement: MS-SMB_R30110.
                //
                // This is the server internal behavior, if MaximalShareAccessRights is not null, it means the server 
                // under test has set ACCESS_MASK in MaximalShareAccessRights.
                Site.CaptureRequirementIfIsNotNull(
                    response.SmbParameters.MaximalShareAccessRights,
                    30110,
                    @"[In Receiving an SMB_COM_TREE_CONNECT_ANDX Request] Requesting Extended Information: The 
                    computed MaxRights ACCESS_MASK MUST be placed in the SMB_Parameters.Words.MaximalShareAccessRights 
                    of the response.");
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R4828");

            //
            // Verify MS-SMB requirement: MS-SMB_R4828.
            //
            // Using the same algorithm is the server internal behavior. If GuestMaximalShareAccessRights is not null, 
            // it means the SUT has set it.
            Site.CaptureRequirementIfIsNotNull(
                response.SmbParameters.GuestMaximalShareAccessRights,
                4828,
                @"[In Receiving an SMB_COM_TREE_CONNECT_ANDX Request]Requesting Extended Information: Using the same 
                algorithm, the SMB_Parameters.Words.GuestMaximalAccessRights field of the response MUST be set to the 
                calculated highest access rights the guest account has on this share.");

            if (isRequestExtResponse)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R30107");

                // Since response is of type SmbTreeConnectAndxResponsePacket, so if 
                // TREE_CONNECT_ANDX_EXTENDED_RESPONSE is set, this requirement is verified. Here capture it directly.
                Site.CaptureRequirement(
                    30107,
                    @"[In Receiving an SMB_COM_TREE_CONNECT_ANDX Request] Requesting Extended Information: If the 
                    TREE_CONNECT_ANDX_EXTENDED_RESPONSE is set in the Flags field of the SMB_COM_TREE_CONNECT_ANDX 
                    request, then the server MUST respond with the structure specified in section 2.2.4.7.2.");
            }

            // if isSupportExtSignature is TRUE, it means it's the Extended Information.
            if (isSupportExtSignature)
            {
                //
                // The following statement code will be run only when debugging.
                //

                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R30108 the OptionalSupport is {0}",
                    response.SmbParameters.OptionalSupport);

                //
                // Verify MS-SMB requirement: MS-SMB_R30108.
                //
                // 0x0001 is the value of SMB_SUPPORT_SEARCH_BITS 
                // 0x0002 is the value of SMB_SHARE_IS_IN_DFS
                // 0x000c is the value of SMB_CSC_MASK
                // 0x0010 is the value of SMB_UNIQUE_FILE_NAME
                // 0x0020 is the value of SMB_EXTENDED_SIGNATURES
                // if the sever only set these bits of OptionalSupport, it means the SUT populate 
                // OptionalSupport.
                ushort optionalSupportFlags = 0x0001 | 0x0002 | 0x000c | 0x0010 | 0x0020;
                bool isR30108Satisfied = (0x0000
                    == (response.SmbParameters.OptionalSupport & (ushort)~optionalSupportFlags));

                Site.CaptureRequirementIfIsTrue(
                    isR30108Satisfied,
                    30108,
                    @"[In Receiving an SMB_COM_TREE_CONNECT_ANDX Request] Requesting Extended Information: The server 
                    MUST populate the SMB_Parameters.Words.OptionalSupport field of the response with a value of 
                    Server.Share. OptionalSupport.");
            }

            // isGuestAccountSupported means if the system support the guest account. 
            if (!isGuestAccountSupported)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R2356");

                //
                // Verify MS-SMB requirement: MS-SMB_R2356.
                //
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    response.SmbParameters.GuestMaximalShareAccessRights,
                    2356,
                    @"[In Receiving an SMB_COM_TREE_CONNECT_ANDX Request]Requesting Extended Information: If the system 
                    does not support the guest account, then it MUST set GuestMaximalAccessRights to zero.");
            }
        }

        #endregion

        #region Verify Receiving an SMB_COM_NT_CREATE_ANDX Request section 3.3.5.5.

        /// <summary>
        /// Verify Receiving an SMB_COM_NT_CREATE_ANDX Request.
        /// </summary>
        /// <param name="createFlag2">The Flag2 in the smbheader of the SMB_COM_NT_CREATE_ANDX Request</param>
        /// <param name="response">The SMB_COM_NT_CREATE_ANDX Response.</param>
        /// <param name="computedMaximalAccessRights">The computed MaximalAccessRights.</param>
        /// <param name="computedGuestMaximalAccessRights">The computed GuestMaximalAccessRights.</param>
        /// <param name="isNoSecurityApplied">If file is applied security.</param>
        /// Disable warning CA1801 because according to Test Case design, computedMaximalAccessRights,
        /// computedGuestMaximalAccessRights and isNoSecurityApplied are used for extension.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        private void VerifyReceiveSmbComNtCreateAndXRequest(
            uint createFlag2,
            SmbNtCreateAndxResponsePacket response,
            uint computedMaximalAccessRights,
            uint computedGuestMaximalAccessRights,
            bool isNoSecurityApplied)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R30070, the MaximalAccessRights is {0}",
                BytesToUInt32(response.SmbParameters.MaximalAccessRights));

            //
            // Verify MS-SMB requirement: MS-SMB_R30070.
            //
            // As the response isn't null and all the items have been verified, it means the SUT has 
            // constructed a response as specified. If the MaximalAccessRights isn't null, it means the server under 
            // test has stored them.
            bool isVerifyR30070 = (response != null)
                && (response.SmbParameters.MaximalAccessRights != null);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR30070,
                30070,
                @"[In Receiving an SMB_COM_NT_CREATE_ANDX Request, The processing of an SMB_COM_NT_CREATE_ANDX request 
                is handled as specified in [MS-CIFS] section 3.3.5.50 with the following additions: ] If the server 
                sends the new response, then it MUST construct a response as specified in section 2.2.4.9.2. with the 
                addition of the following rules: The server MUST store them in the MaximalAccessRights field of the 
                response. ");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R30073");

            //
            // Verify MS-SMB requirement: MS-SMB_R30073.
            //
            // If GuestMaximalAccessRights isn't null, it means the SUT has placed it into 
            // GuestMaximalAccessRights.
            Site.CaptureRequirementIfIsNotNull(
                response.SmbParameters.GuestMaximalAccessRights,
                30073,
                @"[In Receiving an SMB_COM_NT_CREATE_ANDX Request] Likewise, it [the server] MUST place it in the
                GuestMaximalAccessRights field of the response. ");

            // 0x00000010 is NT_CREATE_REQUEST_EXTENDED_RESPONSE flag
            if ((createFlag2 & 0x00000010) == 0x00000010)
            {
                //
                // Verify requirement MS-SMB_R14840 and MS-SMB_R30063
                //
                string isR14840Implementated = Site.Properties.Get("SHOULDMAYR14840Implementation");

                // SmbNtCreateAndxResponsePacket is the extended response type specified in section 2.2.4.9.2 
                bool isR30063Satisfied = ((typeof(SmbNtCreateAndxResponsePacket)) == response.GetType());

                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R30063,response's type:{0}",
                        response.GetType());

                    //
                    // Verify MS-SMB requirement: MS-SMB_R30063.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR30063Satisfied,
                        30063,
                        @"[In Receiving an SMB_COM_NT_CREATE_ANDX Request]On a successful create or open, if the 
                        NT_CREATE_REQUEST_EXTENDED_RESPONSE flag was set in the Flags field of the request,then the 
                        server sends an extended response (section 2.2.4.9.2) in Windows.");

                    if (null == isR14840Implementated)
                    {
                        Site.Properties.Add("SHOULDMAYR14840Implementation", Boolean.TrueString);
                        isR14840Implementated = Boolean.TrueString;
                    }
                }

                if (null != isR14840Implementated)
                {
                    bool implemented = Boolean.Parse(isR14840Implementated);
                    bool isSatisfied = isR30063Satisfied;

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R14840,response's type:{0}",
                        response.GetType());

                    //
                    // Verify MS-SMB requirement: MS-SMB_R14840.
                    //
                    Site.CaptureRequirementIfAreEqual<Boolean>(
                        implemented,
                        isSatisfied,
                        14840,
                        String.Format("[In Receiving an SMB_COM_NT_CREATE_ANDX Request] The processing of an " +
                        "SMB_COM_NT_CREATE_ANDX request is handled as specified in [MS-CIFS] section 3.3.5.50 with the " +
                        "following additions: On a successful create or open, if the NT_CREATE_REQUEST_EXTENDED_RESPONSE " +
                        "flag was set in the Flags field of the request, then the server SHOULD<113> send an extended " +
                        "response (section 2.2.4.9.2). This requirement is {0}implemented", implemented ? "" : "not "));
                }

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R30064");

                //The server MUST query the file attributes and use them to set the FileStatusFlags in the response
                bool isR30064Verified = (uint)response.SmbParameters.ExtFileAttributes != 0;

                //
                // Verify MS-SMB requirement: MS-SMB_R30064.
                //
                Site.CaptureRequirementIfIsTrue(
                    isR30064Verified,
                    30064,
                    @"[In Receiving an SMB_COM_NT_CREATE_ANDX Request, The processing of an SMB_COM_NT_CREATE_ANDX 
                    request is handled as specified in [MS-CIFS] section 3.3.5.50 with the following additions: ] If 
                    the server sends the new response, then it MUST construct a response as specified in section 
                    2.2.4.9.2. with the addition of the following rules: The server MUST query the file attributes and 
                    use them to set the FileStatusFlags in the response.<114>");
            }
        }

        #endregion

        #region Verify Receiving an SMB_COM_READ_ANDX Request section 3.3.5.7 section 2.2.4.2.2.

        /// <summary>
        /// Verify Receiving an SMB_COM_READ_ANDX Request.
        /// </summary>
        /// <param name="response">The SMB_COM_READ_ANDX Response.</param>
        /// <param name="minNumberOfBytesToReturn">The minimum number of returned Bytes.</param>
        /// <param name="isReadOnPipe">If the read operation is on a named pipe.</param>
        /// <param name="isReadOnFile">If the read operation is on a file.</param>
        /// Disable warning CA1801 because according to Test Case design, 
        /// minNumberOfBytesToReturn is used for extension.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        private void VerifyReceiveSmbComReadAndXRequest(
            SmbReadAndxResponsePacket response,
            int minNumberOfBytesToReturn,
            bool isReadOnPipe,
            bool isReadOnFile)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9183");

            //
            // Verify MS-SMB requirement: MS-SMB_R9183.
            //
            // According to the description about smbHeader in section 2.2.3.1, the fixed-size is 32bytes.
            // This requirement is partially derived from R9182. Here verify if one type of the server response 
            // (SMB_COM_READ_ ANDX)'s smb header is 32bytes.
            Site.CaptureRequirementIfAreEqual<int>(
                32,
                Marshal.SizeOf(response.SmbHeader),
                9183,
                @"[In SMB Header Extensions] The server responses[SMB_COM_READ_ANDX ], with the exception of the
                SMB_COM_READ_RAW response message, as specified in [MS-CIFS] section 2.2.4.22.2, MUST begin with the 
                same fixed-size SMB header.");

            // Reserved2 is defined as short array.
            // Here use response.SmbParameters.Reserved2.Length * Marshal.SizeOf(response.SmbParameters.Reserved2[0]) 
            // to represent the byte-length of Reserved2.
            int logInfoForFalse = 0;

            bool isVerifyR9930 = true;

            foreach (ushort r2 in response.SmbParameters.Reserved2)
            {
                if (r2 != 0)
                {
                    isVerifyR9930 = false;
                    logInfoForFalse = r2;
                    break;
                }
            }

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9930, {0}",
                isVerifyR9930 ? "All member of the Reserved2 are zero" : logInfoForFalse.ToString());

            //
            // Verify MS-SMB requirement: MS-SMB_R9930.
            //
            Site.CaptureRequirementIfIsTrue(
                isVerifyR9930,
                9930,
                @"[In Server Response Extensions]Reserved2 (8 bytes):  This field MUST be set to zero by the server.");

            // isReadOnPipe means if the read operation is on a named pipe
            if (isReadOnPipe)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R30058");

                // How a field is explained is decided by protocol SDK and couldn't be verified. 
                // The protocol SDK has ensured that the field is interpreted as the 32-bit Timeout field. Here capture 
                // this requirement directly.
                Site.CaptureRequirement(
                    30058,
                    @"[In Receiving an SMB_COM_READ_ANDX Request] The processing of an SMB_COM_READ_ANDX request is 
                    handled as specified in [MS-CIFS] section 3.3.5.35 with the following additions: If the read 
                    operation is on a named pipe, then the Timeout_or_MaxCountHigh field MUST be interpreted as the 
                    32-bit Timeout field, as specified in section 2.2.4.1.2.");
            }

            // isReadOnFile means if the read operation is on a file.
            else if (isReadOnFile)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R30059");

                // How a field is explained is decided by protocol SDK and couldn't be verified.
                // The protocol SDK has ensured that the field is interpreted as the 16-bit MaxCountHigh field followed 
                // by a 16-bit Reserved field. Here capture this requirement directly.
                Site.CaptureRequirement(
                    30059,
                    @"[In Receiving an SMB_COM_READ_ANDX Request] The processing of an SMB_COM_READ_ANDX request is 
                    handled as specified in [MS-CIFS] section 3.3.5.35 with the following additions: If the read 
                    operation is on a file, then the Timeout_or_MaxCountHigh field MUST be interpreted as the 16-bit 
                    MaxCountHigh field followed by a 16-bit Reserved field, as specified in section 2.2.4.1.1. ");
            }
            if (response.SmbParameters.DataLengthHigh == 0)
            {
                //
                // Verify MS-SMB requirement: MS-SMB_R9929.
                //
                Site.CaptureRequirement(
                    9929,
                    @"[In Server Response Extensions]If the data read is smaller than  0x00010000 bytes (64KB) in length this field MUST be set to zero.");

            }
        }

        #endregion

        #region Verify Receiving an FSCTL_SRV_ENUMERATE_SNAPSHOTS Function Code section 3.3.5.10.1.1.

        /// <summary>
        /// Verify Receiving an FSCTL_SRV_ENUMERATE_SNAPSHOTS Function Code.
        /// </summary>
        /// <param name="response">The FSCTL_SRV_ENUMERATE_SNAPSHOTS response from the SUT.</param>
        /// <param name="expectedSnapShotsCount">The expected SnapShots number.</param>
        /// <param name="expectedSnapshotsReturnedCount">The expected SnapShots returned number.</param>
        /// <param name="isMaxDataCountLargeEnough">Is MaxDataCount large enough.</param>
        /// Disable warning CA1801 because according to Test Case design, 
        /// expectedSnapShotsCount and expectedSnapshotsReturnedCount are used for extension.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        private void VerifyReceiveFsctlSrvEnumerateSnapshotsFunctionCode(
            SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket response,
            int expectedSnapShotsCount,
            int expectedSnapshotsReturnedCount,
            bool isMaxDataCountLargeEnough)
        {

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R2437");

            //
            // Verify MS-SMB requirement: MS-SMB_R2437.
            //
            // The total number of previous versions can't be got.
            Site.CaptureRequirementIfAreEqual<uint>(
                (uint)response.SnapShots.Count,
                response.NtTransData.NumberOfSnapShotsReturned,
                2437,
                @"The NumberOfSnapshots MUST contain the total number of previous versions that are available for 
                    the volume, and NumberOfSnapshotsReturned contains the number of entries that are returned in this 
                    enumeration.");

            // isMaxDataCountLargeEnough means if MaxDataCount is large enough.
            if (!isMaxDataCountLargeEnough)
            {
                //
                // Verify requirement MS-SMB_R4980 and MS-SMB_R4981
                //
                string isR4980Implementated = Site.Properties.Get("SHOULDMAYR4980Implementation");

                // NumberOfSnapShotsReturned is the number of returned entries.
                bool isR4981Satisfied = (0 == response.NtTransData.NumberOfSnapShotsReturned);

                if (isWindows)
                {
                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R4981,NumberOfSnapShotsReturned:{0}",
                        response.NtTransData.NumberOfSnapShotsReturned);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R4981.
                    //
                    Site.CaptureRequirementIfIsTrue(
                        isR4981Satisfied,
                        4981,
                        @"[In Receiving an FSCTL_SRV_ENUMERATE_SNAPSHOTS Function Code]If MaxDataCount is not large 
                            enough to hold all of the entries,then the server does return zero entries in windows.");

                    if (null == isR4980Implementated)
                    {
                        Site.Properties.Add("SHOULDMAYR4980Implementation", Boolean.TrueString);
                        isR4980Implementated = Boolean.TrueString;
                    }
                }

                if (null != isR4980Implementated)
                {
                    bool implemented = Boolean.Parse(isR4980Implementated);
                    bool isSatisfied = isR4981Satisfied;

                    //
                    // The following statement code will be run only when debugging.
                    //
                    Site.Log.Add(LogEntryKind.Debug,
                        @"Verify MS-SMB_R4980,NumberOfSnapShotsReturned:{0}",
                        response.NtTransData.NumberOfSnapShotsReturned);

                    //
                    // Verify MS-SMB requirement: MS-SMB_R4980.
                    //
                    Site.CaptureRequirementIfAreEqual<Boolean>(
                        implemented,
                        isSatisfied,
                        4980,
                        String.Format("[In Receiving an FSCTL_SRV_ENUMERATE_SNAPSHOTS Function Code]If " +
                        "MaxDataCount is not large enough to hold all of the entries,then the server SHOULD return " +
                        "zero entry. This requirement is {0}implemented", implemented ? "" : "not "));
                }
            }
            if (response.NtTransData.snapShotMultiSZ != null)
            {

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R4982");

                //
                // Verify MS-SMB requirement: MS-SMB_R4982.
                //
                // The available previous versions are stored in the SnapShotMultiSZ. Once the snapShotArraySize is 
                // equal to the length of SnapShotMultiSZ, this requirement is verified.
                Site.CaptureRequirementIfAreEqual<int>(
                    response.NtTransData.snapShotMultiSZ.Length,
                    (int)response.NtTransData.SnapShotArraySize,
                    4982,
                    @"[In Receiving an FSCTL_SRV_ENUMERATE_SNAPSHOTS Function Code]The value returned in
                    SnapShotArraySize MUST be the size required to receive all of the available previous versions.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R2436,NumberOfSnapShotsReturned:{0}",
                    response.NtTransData.NumberOfSnapShotsReturned);

                //
                // Verify MS-SMB requirement: MS-SMB_R2436.
                //
                // NumberOfSnapShotsReturned contains the number of entries that are returned in the enumeration.
                bool isVerifyR2436 = response.NtTransData.NumberOfSnapShotsReturned >= 0;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR2436,
                    2436,
                    @"[In Receiving an FSCTL_SRV_ENUMERATE_SNAPSHOTS Function Code] The server MUST return an
                    enumeration of available previous versions, as specified in section 2.2.7.2.2.");
            }
        }

        #endregion

        #region Verify Receiving an FSCTL_SRV_REQUEST_RESUME_KEY Function Code section 3.3.5.10.1.2.

        /// <summary>
        /// Verify Receiving an FSCTL_SRV_REQUEST_RESUME_KEY Function Code.
        /// </summary>
        /// <param name="response">The FSCTL_SRV_REQUEST_RESUME_KEY response from the SUT.</param>
        /// Disable warning CA1811 because according to Test Case design, this method is used for extension.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void verifyFsctlSrvRequestResumeKeyFunctionCode(
            SmbNtTransFsctlSrvRequestResumeKeyResponsePacket response)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R4988");

            //
            // Verify MS-SMB requirement: MS-SMB_R4988.
            //
            // Accordint to TD, the 24-bytes data generated by the SUT is saved in the field of ResumeKey.
            // ResumeKey is defined as an array of bytes, so the length of the array is the byte-length of ResumeKey.
            // And the latter half of the requirement is explaing the content of ResumeKey content and is informative.
            Site.CaptureRequirementIfAreEqual<int>(
                24,
                response.NtTransData.ResumeKey.Length,
                4988,
                @"[In Receiving an FSCTL_SRV_REQUEST_RESUME_KEY Function Code]The server MUST generate a 24-byte value 
                that is used to uniquely identify the open of the file against which this operation is executed.");
        }

        #endregion

        #region Verify SMB_QUERY_FS_ATTRIBUTE_INFO section 2.2.8.2.1.

        /// <summary>
        /// Verify SMB_QUERY_FS_ATTRIBUTE_INFO.
        /// </summary> 
        /// <param name="info">An SMB_QUERY_FS_ATTRIBUTE_INFO to be verified.</param>
        /// Disable warning CA1811 because according to Test Case design, this method is used for extension.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void VerifySmbQueryFsAttributeInfo(
            Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs.SMB_QUERY_FS_ATTRIBUTE_INFO info)
        {
            //
            // Verify requirement MS-SMB_R9497 and MS-SMB_R9583
            //
            string isR9497Implementated = Site.Properties.Get("SHOULDMAYR9497Implementation");

            // 0x00200000 is FILE_SUPPORTS_TRANSACTIONS flag.
            // 0x01000000 is FILE_SUPPORTS_OPEN_BY_FILE_ID flag.
            bool isR9583Satisfied = (((uint)info.FileSystemAttributes & 0x00200000) == 0x00000000)
                && (((uint)info.FileSystemAttributes & 0x01000000) == 0x00000000);

            if (isWindows)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9583: FileSystemAttributes:{0}",
                    (uint)(info.FileSystemAttributes));

                //
                // Verify MS-SMB requirement: MS-SMB_R9583.
                //
                Site.CaptureRequirementIfIsTrue(
                    isR9583Satisfied,
                    9583,
                    "<76> Section 2.2.8.2.1: The following attribute flags are removed by the Windows Server 2008 R2" +
                    "SMB server before sending the attribute data block to the client:FILE_SUPPORTS_TRANSACTIONS," +
                    "FILE_SUPPORTS_OPEN_BY_FILE_ID.");

                if (null == isR9497Implementated)
                {
                    Site.Properties.Add("SHOULDMAYR9497Implementation", Boolean.TrueString);
                    isR9497Implementated = Boolean.TrueString;
                }
            }

            if (null != isR9497Implementated)
            {
                bool implemented = Boolean.Parse(isR9497Implementated);
                bool isSatisfied = isR9583Satisfied;

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9497: FileSystemAttributes:{0}",
                    (uint)(info.FileSystemAttributes));

                //
                // Verify MS-SMB requirement: MS-SMB_R9497.
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implemented,
                    isSatisfied,
                    9497,
                    String.Format("[In SMB_QUERY_FS_ATTRIBUTE_INFO]For this Information Level, the server SHOULD check " +
                    "the FileSystemAttributes field and remove the attribute flags that are not supported by the " +
                    "underlying object store before sending the response to the client.<76>" +
                    "This requirement is {0}implemented", implemented ? "" : "not "));
            }
            if (sutOsVersion == Platform.Win2K8R2)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R9583, fileAttributes:{0}",
                    (uint)info.FileSystemAttributes);

                //
                // Verify MS-SMB requirement: MS-SMB_R9583.
                //
                // 0x00200000 is the FILE_SUPPORTS_TRANSACTIONS flag,
                // 0x01000000 is the FILE_SUPPORTS_OPEN_BY_FILE_ID flag.
                bool isVerifyR9583 = (((uint)info.FileSystemAttributes & 0x00200000) == 0x00000000)
                    && (((uint)info.FileSystemAttributes & 0x01000000) == 0x00000000);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR9583,
                    9583,
                    @"<77> Section 2.2.8.2.1: The following attribute flags are removed by the Windows Server 2008 R2 
                    SMB server before sending the attribute data block to the client:FILE_SUPPORTS_TRANSACTIONS,
                    FILE_SUPPORTS_OPEN_BY_FILE_ID.");
            }
        }

        #endregion

        #region Verify Application Requests Querying User Quota Information section 3.2.4.13.

        /// <summary>
        /// Verify Application Requests Querying User Quota Information.
        /// </summary>
        /// <param name="isReturnSingleEntry">Whether Entry is single or not.</param>
        /// <param name="response">The NT_TRANSACT_QUERY_QUOTA response.</param>
        private void VerifyNtTransQueryQuotaRequestAndResponse(
            bool isReturnSingleEntry,
            SmbNtTransQueryQuotaResponsePacket response)
        {
            // In the stackSDK ReturnSinEntry is defined as a byte type, here judge if it is equal to 1.
            if (isReturnSingleEntry)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R8457");

                //
                // Verify MS-SMB requirement: MS-SMB_R8457.
                //
                // The user quota information entry is returned in NtTransDataList. If the NtTransDataList's count is 
                // 1, it means the SUT only returns one user quota information entry.
                Site.CaptureRequirementIfAreEqual<int>(
                    1,
                    response.NtTransDataList.Count,
                    8457,
                    @"[In Application Requests Querying Quota Information,The application MUST provide:]
                    ReturnSingleEntry: If TRUE,then the server MUST return a single user quota information entry.");
            }
        }

        #endregion

        #region Verify Receiving an SMB_COM_SESSION_SETUP_ANDX Request section 3.3.5.3.

        /// <summary>
        /// Verify Receiving an SMB_COM_SESSION_SETUP_ANDX Request.
        /// </summary>
        /// <param name="firsUid">The UID of the first SMB_COM_SESSION_SETUP_ANDX Request from client.</param>
        /// <param name="response">The SMB_COM_SESSION_SETUP_ANDX Response from the SUT.</param>
        private void VerifyReceiveSmbComSessionSetupAndXRequest(
            int firsUid,
            SmbSessionSetupAndxResponsePacket response)
        {
            if (firsUid == 0)
            {
                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R30115");

                //
                // Verify MS-SMB requirement: MS-SMB_R30115.
                //
                // If the the SUT generates a UID, it must not be zero, and the generated UID is stored 
                // in UID field of SmbHeader.
                Site.CaptureRequirementIfAreNotEqual<ushort>(
                    0,
                    response.SmbHeader.Uid,
                    30115,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Extended Security: If the request did not
                    specify a UID in the SMB header of the request, then a UID MUST be generated to represent this 
                    user's authentication.");

                //
                // The following statement code will be run only when debugging.
                //
                Site.Log.Add(LogEntryKind.Debug,
                    @"Verify MS-SMB_R30116");

                //
                // Verify MS-SMB requirement: MS-SMB_R30116.
                //
                // It's server internal behavior.
                // Only verify whether the UID of the response is zero after it was generated by the SUT.
                Site.CaptureRequirementIfAreNotEqual<ushort>(
                    0,
                    response.SmbHeader.Uid,
                    30116,
                    @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Extended Security: If the request did not 
                    specify a UID in the SMB header of the request, then its [UID's] value MUST be placed in the UID 
                    field of the SMB header of the response.");
            }
        }

        #endregion

        #region Verify Scanning a Path for a Previous Version Token section 3.3.5.1.1.

        /// <summary>
        /// Verify scanning a path for a previous version token when the directory not exist.
        /// </summary>
        /// <param name="trans2FindFirst2ErrorResponse">The TRANS2_FIND_FIRST2 error response.</param>
        /// Disable warning CA1811 because according to Test Case design, this method is used for extension.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void VerifyScanPathForPreviousVersionTokenWithoutDirectory(
            SmbErrorResponsePacket trans2FindFirst2ErrorResponse)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R6004");

            //
            // Verify MS-SMB requirement: MS-SMB_R6004
            //
            Site.CaptureRequirementIfAreEqual<SmbStatus>(
                SmbStatus.STATUS_OBJECT_NAME_NOT_FOUND,
                (SmbStatus)trans2FindFirst2ErrorResponse.SmbHeader.Status,
                6004,
                @"[In Scanning a Path for a Previous Version Token] If a previous version token is found in the 
                pathname, but the directory does not exist for the given snapshot, then the server MUST fail the 
                operation with STATUS_OBJECT_NAME_NOT_FOUND.");
        }

        /// <summary>
        /// Verify scanning a path for a previous version token when the file not exist.
        /// </summary>
        /// <param name="trans2FindFirst2ErrorResponse">The TRANS2_FIND_FIRST2 error response.</param>
        /// Disable warning CA1811 because according to Test Case design, this method is used for extension.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void VerifyScanPathForPreviousVersionTokenWithoutFile(
            SmbErrorResponsePacket trans2FindFirst2ErrorResponse)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R8556");

            //
            // Verify MS-SMB requirement: MS-SMB_R8556.
            //
            Site.CaptureRequirementIfAreEqual<SmbStatus>(
                SmbStatus.STATUS_OBJECT_NAME_NOT_FOUND,
                (SmbStatus)trans2FindFirst2ErrorResponse.SmbHeader.Status,
                8556,
                @"[In Scanning a Path for a Previous Version Token] If a previous version token is found in the 
                pathname, but the file does not exist for the given snapshot, then the server MUST fail the operation 
                with STATUS_OBJECT_NAME_NOT_FOUND.");
        }

        #endregion

        #region Verify Directory Access Mask section 2.2.1.4.2.

        /// <summary>
        /// Verify Directory_Access_Mask.
        /// </summary>
        /// <param name="createResponse">The create response.</param>
        /// Disable warning CA1811 because according to Test Case design, this method is used for extension.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void VerifyMessageSyntaxDirectoryAccessMask(SmbNtCreateAndxResponsePacket createResponse)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R9102");

            //
            // Verify MS-SMB requirement: MS-SMB_R9102.
            //
            // DIRECTORY_ACCESS_MASK is the bit-or result of all the flags listed in this RS.
            // If an access mask is used on a directory, it should be the combination of the flags listed in this RS, 
            // and the bit-and result with DIRECTORY_ACCESS_MASK must be zero.
            // Otherwise, it means the bit not listed in the RS was set,
            // then it doesn't fit the rule of Directory_Access_Mask defined in the TD.
            Site.CaptureRequirementIfAreEqual<uint>(
                0,
                ~DirectoryAccessMask & (uint)Convert.ToInt32(createResponse.SmbParameters.MaximalAccessRights),
                9102,
                "[In Directory_Access_Mask]Directory_Access_Mask (4 bytes):For a directory, the value MUST be " +
                "constructed using the following values:[" +
                    "FILE_LIST_DIRECTORY" +
                    "0x00000001,FILE_ADD_FILE" +
                    "0x00000002,FILE_ADD_SUBDIRECTORY" +
                    "0x00000004,FILE_READ_EA" +
                    "0x00000008,FILE_WRITE_EA" +
                    "0x00000010,FILE_TRAVERSE" +
                    "0x00000020,FILE_DELETE_CHILD" +
                    "0x00000040,FILE_READ_ATTRIBUTES" +
                    "0x00000080,FILE_WRITE_ATTRIBUTES" +
                    "0x00000100,DELETE" +
                    "0x00010000,READ_CONTROL" +
                    "0x00020000,WRITE_DAC" +
                    "0x00040000,WRITE_OWNER" +
                    "0x00080000,SYNCHRONIZE" +
                    "0x00100000,ACCESS_SYSTEM_SECURITY" +
                    "0x01000000,MAXIMUM_ALLOWED" +
                    "0x02000000,GENERIC_ALL" +
                    "0x10000000,GENERIC_EXECUTE" +
                    "0x20000000,GENERIC_WRITE" +
                    "0x40000000,GENERIC_READ" +
                    "0x80000000].");
        }

        #endregion

        #region Verify SMB_COM_SESSION_SETUP_ANDX request section 2.2.4.6.2.

        /// <summary>
        /// Verify SMB_COM_SESSION_SETUP_ANDX request.
        /// </summary>
        /// <param name="sessionSetupResponse">The session set up response.</param>
        /// Disable warning CA1811 because according to Test Case design, this method is used for extension.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void VerifySmbComSessionSetupAndxRequest(SmbSessionSetupAndxResponsePacket sessionSetupResponse)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R5568,ByteCount:{0}",
                sessionSetupResponse.SmbData.ByteCount);

            //
            // Verify MS-SMB requirement: MS-SMB_R5568
            //
            bool isVerifyR5568 = ((2 == Marshal.SizeOf(sessionSetupResponse.SmbData.ByteCount))
                && (sessionSetupResponse.SmbData.ByteCount >= 0x0003));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR5568,
                5568,
                @"[In Server Response Extensions]ByteCount (2 bytes): If SMB_FLAGS2_UNICODE is not set, then this field 
                MUST be greater than or equal to 0x0003.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R333,NativeOS:{0}",
                BitConverter.ToString(sessionSetupResponse.SmbData.NativeOS));

            //
            // Verify MS-SMB requirement: MS-SMB_R333
            //
            // If the string is null-terminated array of OEM characters, only the last byte of the string is '\0'.
            int nativeOsLen = sessionSetupResponse.SmbData.NativeOS.Length;
            bool isVerifyR333 = (((byte)0 == sessionSetupResponse.SmbData.NativeOS[nativeOsLen - 1])
                && ((byte)0 != sessionSetupResponse.SmbData.NativeOS[nativeOsLen - 2]));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR333,
                333,
                @"[In Server Response Extensions]NativeOS (variable):  Otherwise[If SMB_FLAGS2_UNICODE is not set in 
                the Flags2 field of the SMB header of the response], the string MUST be a null-terminated array of OEM 
                characters.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R337,NativeLANMan:{0}",
                BitConverter.ToString(sessionSetupResponse.SmbData.NativeLanMan));

            //
            // Verify MS-SMB requirement: MS-SMB_R337
            //
            // If the string is null-terminated array of OEM characters, only the last byte of the string is '\0'.
            int nativeLanManLen = sessionSetupResponse.SmbData.NativeLanMan.Length;
            bool isVerifyR337 = (('\0' == sessionSetupResponse.SmbData.NativeLanMan[nativeLanManLen - 1]) &&
                ('\0' != sessionSetupResponse.SmbData.NativeLanMan[nativeLanManLen - 2]));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR337,
                337,
                @"[In Server Response Extensions]NativeLANMan (variable):  Otherwise[If SMB_FLAGS2_UNICODE is not set 
                in the Flags2 field of the SMB header of the response], the string MUST be a null-terminated array of 
                OEM characters. ");
        }

        #endregion

        #region Verify SMB_COM_SESSION_SETUP_ANDX Request section 3.3.5.3.

        /// <summary>
        /// Verify SMB_COM_SESSION_SETUP_ANDX Request.
        /// </summary>
        /// <param name="sessionSetupResponse">The session set up response.</param>
        /// Disable warning CA1811 because according to Test Case design, this method is used for extension.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void VerifySmbComSessionSetUpAndxRequest(SmbSessionSetupAndxResponsePacket sessionSetupResponse)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R2333");

            //
            // Verify MS-SMB requirement: MS-SMB_R2333
            //
            Site.CaptureRequirementIfAreEqual<SmbStatus>(
                SmbStatus.STATUS_MORE_PROCESSING_REQUIRED,
                (SmbStatus)sessionSetupResponse.SmbHeader.Status,
                2333,
                @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Request]Extended Security: Otherwise[If the GSS mechanism 
                does not indicate that the current output token is the last output token of the authentication exchange 
                based on the return code], the Status field in the SMB header of the response MUST be set to 
                STATUS_MORE_PROCESSING_REQUIRED.");
        }

        #endregion

        #region Verify Sequence Diagram section 3.2.4.2.4.1.

        /// <summary>
        /// Verify Sequence Diagram.
        /// </summary>
        /// <param name="errorResponsePacket">The error response packet.</param>
        /// Disable warning CA1811 because according to Test Case design, this method is used for extension.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void VerifySequenceDiagram(SmbErrorResponsePacket errorResponsePacket)
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R4179");

            // The adapter logic has realized the behavior of this RS, it can be captured directly.
            Site.CaptureRequirement(
                4179,
                @"[In Sequence Diagram] Session Setup Roundtrip: If the session setup has to be continued, the security 
                package on the client and/or server requires an additional roundtrip before the session setup can be 
                established.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R4180");

            //
            // Verify MS-SMB requirement: MS-SMB_R4180
            //
            // If the packet is not null, it means the new security packages were returned.
            Site.CaptureRequirementIfIsNotNull(
                errorResponsePacket,
                4180,
                "[In Sequence Diagram] Session Setup Roundtrip:This[If the session setup has to be continued, the " +
                "security package on the client and or server needs an additional roundtrip before the session setup can " +
                "be established.] is especially true of new security packages that support mutual authentication between " +
                "the client and server.");
        }

        #endregion

        #region Verify SMB_COM_SESSION_SETUP_ANDX Response section 3.2.5.3.

        /// <summary>
        /// Verify SMB_COM_SESSION_SETUP_ANDX Response.
        /// </summary>
        /// Disable warning CA1811 because according to Test Case design, this method is used for extension.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private void VerifySmbComSessionSetupAndxResponse()
        {
            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R4184");

            //
            // Verify MS-SMB requirement: MS-SMB_R4184.
            //
            // The adapter logic has realized the behavior of this RS, it can be captured directly.
            Site.CaptureRequirement(
                4184,
                @"[In Sequence Diagram] Session Setup Roundtrip: Each additional roundtrips MUST consist  
                of one SMB_COM_SESSION_SETUP_ANDX client request and one SMB_COM_SESSION_SETUP_ANDX server 
                response.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R4186");

            //
            // Verify MS-SMB requirement: MS-SMB_R4186.
            //
            // The adapter logic has realized the behavior of this RS, it can be captured directly.
            Site.CaptureRequirement(
                4186,
                @"[In Sequence Diagram]Session Setup Roundtrip: All additional SMB session setup round trips follow 
                the same sequence details as Session Setup Round Trip, as described earlier in this topic.<87>");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R2193");

            //
            // Verify MS-SMB requirement: MS-SMB_R2193.
            //
            // The adapter logic has realized the behavior of this RS, it can be captured directly.
            Site.CaptureRequirement(
                2193,
                @"[In Receiving an SMB_COM_SESSION_SETUP_ANDX Response]NTLM Authentication: The connection MUST remain 
                open for the client to attempt another authentication.");

            //
            // The following statement code will be run only when debugging.
            //
            Site.Log.Add(LogEntryKind.Debug,
                @"Verify MS-SMB_R104181");

            //
            // Verify MS-SMB requirement: MS-SMB_R104181.
            //
            // In SessionSetupRequest, when server response on the session setup request, 
            // means the authentication successed after only one roundtrip, so this requirement is
            // captured directly
            Site.CaptureRequirement(
                104181,
                @"[In Sequence Diagram] Session Setup Roundtrip: If authentication succeeds after a single roundtrip, then only one session setup exchange is required.");
        }

        #endregion
    }
}