// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using System.Collections.Generic;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    ///  partial CommonAdapter for MS-FSCC capture code
    /// </summary>
    public partial class SmbAdapter : ISmbAdapter
    {
        #region Verify the datatype of REPARSE_DATA_BUFFER in section 2.1.2.2

        /// <summary>
        /// Verify the datatype of REPARSE_DATA_BUFFER
        /// </summary>
        /// <param name="reparseDataBuffer">the instance of  REPARSE_DATA_BUFFER</param>
        public void VerifyDataTypeReparseDataBuffer(REPARSE_DATA_BUFFER reparseDataBuffer)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R46");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R46
            //
            //just to verify the type is byte array
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(byte[]),
                reparseDataBuffer.DataBuffer.GetType(),
                46,
                @"[In REPARSE_DATA_BUFFER]DataBuffer (variable):  A variable-length array of 8-bit unsigned integer 
                values containing reparse-specific data for the reparse point.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R47");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R47
            //
            //The format cannot be actually check, just to verify the type is byte array
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(byte[]),
                reparseDataBuffer.DataBuffer.GetType(),
                47,
                @"[In REPARSE_DATA_BUFFER]DataBuffer (variable): The format of this data is defined by the owner 
                (that is, the implementer of the filter driver associated with the specified ReparseTag) of the 
                reparse point.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R41");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R41
            //
            Site.CaptureRequirementIfAreEqual<ushort>(
                (ushort)Marshal.SizeOf(reparseDataBuffer.DataBuffer),
                reparseDataBuffer.ReparseDataLength,
                41,
                @"[In REPARSE_DATA_BUFFER]ReparseDataLength (2 bytes):  A 16-bit unsigned integer value containing the 
                size, in bytes, of the reparse data in the DataBuffer member.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R40");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R40
            //
            //  unique identifier cannot be verified, so here just to verify its type of 32-bit unsigned integer
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(uint),
                reparseDataBuffer.ReparseTag.GetType(),
                40,
                @"[In REPARSE_DATA_BUFFER]ReparseTag (4 bytes):  A 32-bit unsigned integer value containing the reparse 
                point tag that uniquely identifies the owner of the reparse point.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R42");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R42
            //
            // the size is 2 bytes
            Site.CaptureRequirementIfAreEqual<int>(
                2,
                Marshal.SizeOf(reparseDataBuffer.Reserved),
                42,
                @"[In REPARSE_DATA_BUFFER]Reserved (2 bytes):  A 16-bit field.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R38");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R38
            //
            // high bit must be set 1 by uesed
            Site.CaptureRequirementIfAreEqual<uint>(
                0x80000000,
                reparseDataBuffer.ReparseTag & 0x80000000,
                38,
                @"This reparse data buffer[REPARSE_DATA_BUFFER] MUST be used only with reparse tag values whose high 
                bit is set to 1.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Veify the datatype Reparse Tags in section 2.1.2.1

        /// <summary>
        /// Verify the Reparse Tags values
        /// </summary>
        /// <param name="ReparseTags">the value of Reparse Tags</param>
        public void VerifyDataTypeReparseTags(uint ReparseTags)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (bool.Parse(Site.Properties["IsWindows"]))
            {

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R18. ReparseTags : {0}", ReparseTags);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R18
                //
                // high bit set to 1 is 0x80000000
                Site.CaptureRequirementIfAreEqual<uint>(
                    0x80000000,
                    ReparseTags & 0x80000000,
                    18,
                    @"All reparse tags defined by Microsoft components MUST have the high bit set to 1.");
            }
            else
            {

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R19. ReparseTags: {0}", ReparseTags);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R19
                //
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    ReparseTags & 0x80000000,
                    19,
                    @"Non-Microsoft reparse tags MUST have the high bit set to 0.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify the return of FSCTL_WRITE_USN_CLOSE_RECORD Reply in section 2.3.70
        /// <summary>
        /// Verify the return of FSCTL_WRITE_USN_CLOSE_RECORD Reply
        /// </summary>
        /// <param name="returnedStatusCode">the returned status code from FSCTL_WRITE_USN_CLOSE_RECORD Reply</param>
        /// <param name="isUSNChangeSupport">check if the file system support the use of a USN change journal.</param>
        public void VerifyDataTypeFsctlWriteUsnCloseRecordReply(long returnedStatusCode,
                                                                bool isUSNChangeSupport)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R967. returnedStatusCode: {0}", returnedStatusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R967
            //
            bool isVerifyR967 = (returnedStatusCode == (long)FsctlWriteUsnCloseRecordReplyStatus.STATUS_SUCCESS ||
                                 returnedStatusCode == (long)FsctlWriteUsnCloseRecordReplyStatus.STATUS_INVALID_PARAMETER ||
                                 returnedStatusCode == (long)FsctlWriteUsnCloseRecordReplyStatus.STATUS_INVALID_DEVICE_REQUEST);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR967,
                967,
                @"[In FSCTL_WRITE_USN_CLOSE_RECORD Reply]The status code returned directly by the function that processes 
                this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_INVALID_PARAMETER 0xC000000D, 
                STATUS_INVALID_DEVICE_REQUEST 0xC0000010].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R964");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R964
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(long),
                returnedStatusCode.GetType(),
                964,
                @"This message[FSCTL_WRITE_USN_CLOSE_RECORD Reply] returns the results of the FSCTL_WRITE_USN_CLOSE_RECORD 
                request as a single field, Usn, which is a 64-bit signed integer that contains the server file system's 
                USN (update sequence number) for the file or directory.");

            // the file system does not support the use of a USN change journal.
            if (!isUSNChangeSupport)
            {

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R969");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R969
                //
                Site.CaptureRequirementIfAreEqual<long>(
                    (long)FsctlWriteUsnCloseRecordReplyStatus.STATUS_INVALID_DEVICE_REQUEST,
                     returnedStatusCode,
                    969,
                    @"[In FSCTL_WRITE_USN_CLOSE_RECORD Reply]Value of status code STATUS_INVALID_DEVICE_REQUEST 0xC0000010 
                    means the file system does not support the use of a USN change journal.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R966. returnedStatusCode: {0}", returnedStatusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R966
            //
            // the returned status code must be in the valid range,as specified in [MS-ERREF] section 2.3
            bool isVerifyR966 = ((uint)returnedStatusCode >= 0x00000000 && (uint)returnedStatusCode <= 0xC03A0019);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR966,
                966,
                @"This message[FSCTL_WRITE_USN_CLOSE_RECORD Reply] returns a status code, as specified in [MS-ERREF] 
                section 2.3.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region  Verify the Pathname in section 2.1.5
        /// <summary>
        /// Verify the Pathname
        /// </summary>
        /// <param name="Pathname">the array of Pathname</param>
        /// <param name="isSentOverWire">check if the Pathname is sent over the wire</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")] 
        public void VerifyDataTypePathname(string Pathname, bool isSentOverWire)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R147. the length of Pathname: {0}", Pathname.Length);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R147
            //
            bool isVerifyR147 = (Pathname.Length <= 32760);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR147,
                147,
                @"A pathname MUST be no more than 32,760 characters in length.");

            // Get the name components from Pathname
            string[] componentnames = Pathname.Split('\\');
            // compname and streamname
            string compname = string.Empty;
            string streamname = string.Empty;

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R153.");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R153
            //
            // Dirname and compname has been parsed by spliting '\', so check the number of component
            bool isVerifyR153 = (componentnames.Length > 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR153,
                153,
                "[In Pathname]Dirname and compname components MUST be separated by the \"\\\" backslash character.");

            // check if the compname and streamname is contained
            bool hasCompAndStreamName = componentnames[componentnames.Length - 1].Contains(":");

            if (hasCompAndStreamName)
            {
                string[] CompAndStreamName = componentnames[componentnames.Length - 1].Split(':');
                //In TD, the valid is compname:streamname
                if (CompAndStreamName.Length == 2)
                {
                    compname = CompAndStreamName[0];
                    streamname = CompAndStreamName[1];
                }

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R155. compname: {0}, streamname: {1}", compname, streamname);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R155
                //
                bool isVerifyR155 = (!string.IsNullOrEmpty(compname) && !string.IsNullOrEmpty(streamname));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR155,
                    155,
                    @"[In Pathname]If a streamname component exists, a compname component MUST also exist.");


                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R154. compname: {0}, streamname: {1}", compname, streamname);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R154
                //
                // After parsing pathname separated by the "":"" colon character, if compname and streamname exist, 
                // this requirment can be verified
                bool isVerifyR154 = (!string.IsNullOrEmpty(compname) && !string.IsNullOrEmpty(streamname));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR154,
                    154,
                    @"[In Pathname]Compname and streamname components MUST be separated by the "":"" colon character.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R150");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R150
            //
            // no '\' for zero dirname, composed of name components more than one indicate more dirnames
            bool isVerifyR150 = (!Pathname.Contains("\\") || componentnames.Length > 1);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR150,
                150,
                @"A valid pathname has zero or more dirnames.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R151. ");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R151
            //
            // check if compname not exist or one
            bool isVerifyR151 = (!hasCompAndStreamName || (hasCompAndStreamName && string.IsNullOrEmpty(compname)));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR151,
                151,
                @"A valid pathname has zero or one compname.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R152");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R152
            //
            // check if streamname not exist or one
            bool isVerifyR152 = (!hasCompAndStreamName || (hasCompAndStreamName && string.IsNullOrEmpty(streamname)));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR152,
                152,
                @"A valid pathname has zero or one streamname.");


            //
            // Verify MS-FSCC requirement: MS-FSCC_R156
            //
            bool isVerifyR156 = true;

            for (int i = 0; i < componentnames.Length; i++)
            {
                // check the compname and streamname
                if (i == (componentnames.Length - 1) && hasCompAndStreamName)
                {
                    if (compname.Length > 255)
                    {
                        //
                        // Add the debug information
                        //
                        Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R156. The Compname length: {0}", compname.Length);

                        isVerifyR156 = false;
                        break;
                    }
                    if (streamname.Length > 255)
                    {
                        //
                        // Add the debug information
                        //
                        Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R156. The Streamname length: {0}", streamname.Length);

                        isVerifyR156 = false;
                        break;
                    }
                }
                else
                {
                    if (componentnames[i].Length > 255)
                    {
                        //
                        // Add the debug information
                        //
                        Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R156. The Dirname length: {0}", componentnames[i].Length);

                        isVerifyR156 = false;
                        break;
                    }
                }

            }

            Site.CaptureRequirementIfIsTrue(
                isVerifyR156,
                156,
                @"[In Pathname]Each pathname component MUST be no more than 255 characters in length.");



            //
            // Verify MS-FSCC requirement: MS-FSCC_R157
            //
            bool isVerifyR157 = true;

            string DirOrFileName = string.Empty;
            for (int j = 0; j < componentnames.Length; j++)
            {

                // Get the compname
                if (j == componentnames.Length - 1 && hasCompAndStreamName && string.IsNullOrEmpty(compname))
                {
                    DirOrFileName = compname;
                }
                else
                {
                    DirOrFileName = componentnames[j];
                }

                //check  dirname or filename
                for (int i = 0; i < DirOrFileName.Length; i++)
                {
                    if ((DirOrFileName[i] == '\\' || DirOrFileName[i] == '/' || DirOrFileName[i] == '[' || DirOrFileName[i] == ']' ||
                         DirOrFileName[i] == '"' || DirOrFileName[i] == ':' || DirOrFileName[i] == '|' || DirOrFileName[i] == '<' ||
                         DirOrFileName[i] == '>' || DirOrFileName[i] == '+' || DirOrFileName[i] == '=' || DirOrFileName[i] == ';' ||
                         DirOrFileName[i] == ',') || (DirOrFileName[i] >= 0x00 && DirOrFileName[i] <= 0x1F))
                    {
                        isVerifyR157 = false;
                        //
                        // Add the debug information
                        //
                        Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R157. DirOrFileName[{0}]: {1}", i, DirOrFileName[i]);
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsTrue(
                isVerifyR157,
                157,
                @"[In Pathname]All Unicode characters are legal in a dirname or filename component except the following:
                    The characters "" \ / [ ] : 
                     < > + = ; , ?
                    Control characters, ranging from 0x00 through 0x1F.");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R158
            //
            bool isVerifyR158 = true;

            for (int i = 0; i < streamname.Length; i++)
            {
                if (streamname[i] == '\\' || streamname[i] == '/' || streamname[i] == 0x00)
                {
                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R158. streamname[{0}]: {1}", i, streamname[i]);

                    isVerifyR158 = false;
                    break;

                }
            }
            Site.CaptureRequirementIfIsTrue(
                isVerifyR158,
                158,
                @"[In Pathname]All Unicode characters are legal in a streamname component except the following:
                The characters \ / :
                Control character 0x00.");

            if (isSentOverWire)
            {
                //
                // Verify MS-FSCC requirement: MS-FSCC_R159
                //
                bool isVerifyR159 = true;
                for (int i = 1; i < Pathname.Length; i++)
                {
                    if (Pathname[i] == '.' || Pathname[i - 1] == '.')
                    {
                        //
                        // Add the debug information
                        //
                        Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R159. Pathname[{0}]: {1}, Pathname[{2}]: {3}", 
                            i - 1, Pathname[i - 1], i, Pathname[i]);
                        isVerifyR159 = false;
                    }
                }

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR159,
                    159,
                    @"[In Pathname]Pathnames sent over the wire MUST NOT contain dirname or compname components equal 
                    to ""."" (single period) or "".."" (two periods).");


                // If the R159 is verified, this requirement also can be covered, 
                // so this requirement can be captured directly
                Site.CaptureRequirement(
                    164,
                    @"[In Relative Directory Names]Except where explicitly permitted, relative directory names MUST 
                    NOT be sent over the wire.");
            }
            else
            {
                // get the directory or file name
                foreach (string name in componentnames)
                {
                    // the '.' or '..' is contained
                    if (name.Contains(".") || name.Contains(".."))
                    {
                        // '.' is valid for reserved
                        Site.CaptureRequirement(
                            161,
                            @"[In Relative Directory Names]The directory name component ""."" is reserved and refers 
                            to the current directory.");

                        // '..' is valid for reserved
                        Site.CaptureRequirement(
                            162,
                            @"[In Relative Directory Names]The directory name component "".."" is reserved and refers 
                            to the parent directory.");

                        // '.' and '..' is valid for reserved
                        Site.CaptureRequirement(
                            163,
                            @"[In Relative Directory Names]The file names ""."" and "".."" are reserved.");
                    }
                }

            }

            // After the parsing of pathname and above verification, this requirement can be captured directly
            Site.CaptureRequirement(
                148,
                @"A pathname is composed of name components denoting directories, files, and steams.");

            // After the parsing of pathname and above verification, this requirement can be captured directly
            Site.CaptureRequirement(
                149,
                @"The general form of a valid pathname is ""dirname\...\compname:streamname"" where dirname is a 
                directory name, compname is a file name or a directory name, and streamname is a stream name.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify the message of FSCTL_READ_FILE_USN_DATA Reply in section 2.3.44

        /// <summary>
        /// Verify the message of FSCTL_READ_FILE_USN_DATA Reply
        /// </summary>
        /// <param name="reply">the Reply of FSCTL_READ_FILE_USN_DATA</param>
        /// <param name="returnedStatusCode">the status code returned in response, as specified in [MS-ERREF] 
        /// section 2.3</param>
        /// <param name="expectedUSNLength">the expected total length of the update sequence number (USN) record 
        /// in bytes</param>
        /// <param name="isReturnedByFSCTL">check if the a USN RECORD element returned by this FSCTL</param>
        /// <param name="isUsnChangeLogged">check if USN change journal records have been logged for the directory of 
        /// the file associated with this record</param>
        /// <param name="expectedNumOfChanges">the expected number (assigned by the file system when the file is created) 
        /// of the file or directory for which this record notes changes</param>
        public void VerifyMessageSyntaxFsctlReadFileUsnDataReply(FSCTL_READ_FILE_USN_DATA_Reply reply,
                                                                  FsctlReadFileUsnDataReplyStatus returnedStatusCode,
                                                                  bool isReturnedByFSCTL,
                                                                  bool isUsnChangeLogged)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R776. returnedStatusCode: {0}", returnedStatusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R776
            //
            // the returned status code must be in the valid range,as specified in [MS-ERREF] section 2.3
            bool isVerifyR776 = ((uint)returnedStatusCode >= 0x00000000 && (uint)returnedStatusCode <= 0xC03A0019);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR776,
                776,
                @"This message[FSCTL_READ_FILE_USN_DATA Reply] returns a status code, as specified in [MS-ERREF] section 2.3.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R777. The Returned Status Code : {0}", returnedStatusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R777
            //
            // STATUS_SUCCESS 0x00000000
            // STATUS_INVALID_PARAMETER 0xC000000D, 
            // STATUS_INVALID_USER_BUFFER 0xC00000E8, 
            // STATUS_BUFFER_TOO_SMALL 0xC0000023, 
            // STATUS_INVALID_DEVICE_REQUEST 0xC0000010
            bool isVerifyR777 = (returnedStatusCode == FsctlReadFileUsnDataReplyStatus.STATUS_SUCCESS ||
                                 returnedStatusCode == FsctlReadFileUsnDataReplyStatus.STATUS_BUFFER_TOO_SMALL ||
                                 returnedStatusCode == FsctlReadFileUsnDataReplyStatus.STATUS_INVALID_DEVICE_REQUEST ||
                                 returnedStatusCode == FsctlReadFileUsnDataReplyStatus.STATUS_INVALID_PARAMETER ||
                                 returnedStatusCode == FsctlReadFileUsnDataReplyStatus.STATUS_INVALID_USER_BUFFER);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR777,
                777,
                @"[In FSCTL_READ_FILE_USN_DATA Reply]The status code returned directly by the function that processes 
                this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_INVALID_PARAMETER 0xC000000D, 
                STATUS_INVALID_USER_BUFFER 0xC00000E8, STATUS_BUFFER_TOO_SMALL 0xC0000023, STATUS_INVALID_DEVICE_REQUEST 
                0xC0000010].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R766. FileAttributes: {0}", reply.FileAttributes);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R766
            //
            // the combination of FileAttributes, as specified in section 2.6
            uint bitsCombination = (0x00000020 | 0x00000800 | 0x00000010 | 0x00004000 | 0x00000002 |
                                     0x00000080 | 0x00002000 | 0x00001000 | 0x00000001 | 0x00000400 | 0x00000200 |
                                     0x00000004 | 0x00000100);
            // 0x00000000 also is value in FileAttributes field
            bool isVerifyR766 = (reply.FileAttributes == 0x00000000 || (reply.FileAttributes & ~bitsCombination) == 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR766,
                766,
                @"[In USN_RECORD]FileAttributes (4 bytes):  A 32-bit unsigned integer that contains attributes for the 
                file or directory associated with this record.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R773");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R773
            //
            // verify the type of this field
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(byte[]),
                reply.FileName.GetType(),
                773,
                @"[In USN_RECORD]FileName (variable):  A variable-length field of UNICODE characters containing the name 
                of the file or directory associated with this record in Unicode format.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R769");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R769
            //
            Site.CaptureRequirementIfAreEqual<ushort>(
                (ushort)reply.FileName.Length,
                reply.FileNameLength,
                769,
                @"[In USN_RECORD]FileNameLength (2 bytes):  A 16-bit unsigned integer that contains the length of the 
                file or directory name associated with this record in bytes.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R772. FileNameOffset: {0}", reply.FileNameOffset);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R772
            //
            // FileName is parse by FileNameOffset and FileNameLength, which is got in correct size, 
            // indicate FileNameOffset is from the beginning of the structure
            bool isVerifyR772 = (reply.FileNameOffset.GetType() == typeof(UInt16) &&
                                 (ushort)reply.FileName.Length == reply.FileNameLength);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR772,
                772,
                @"[In USN_RECORD]FileNameOffset (2 bytes):  A 16-bit unsigned integer that contains the offset in bytes 
                of the FileName member from the beginning of the structure.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R720. FileReferenceNumber: {0}", reply.FileReferenceNumber);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R720
            //
            bool isVerifyR720 = (reply.FileReferenceNumber.GetType() == typeof(UInt64) &&
                                Marshal.SizeOf(reply.FileReferenceNumber) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR720,
                720,
                @"[In USN_RECORD]FileReferenceNumber (8 bytes):  A 64-bit unsigned integer, opaque to the client, 
                containing the number (assigned by the file system when the file is created) of the file or directory 
                for which this record notes changes. T");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R721");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R721
            //
            //be unique cannot be verified, 
            // so here just to verify its type
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(UInt64),
                reply.FileReferenceNumber.GetType(),
                721,
                @"[In USN_RECORD]FileReferenceNumber (8 bytes):  The FileReferenceNumber is an arbitrarily assigned 
                value (unique within the volume on which the file is stored) that associates a journal record with a file.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R718");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R718
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(UInt16),
                reply.MajorVersion.GetType(),
                718,
                @"[In USN_RECORD]MajorVersion (2 bytes):  A 16-bit unsigned integer that contains the major version of 
                    the change journal software for this record.<25>");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R719");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R719
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(UInt16),
                reply.MinorVersion.GetType(),
                719,
                @"[In USN_RECORD]MinorVersion (2 bytes):  A 16-bit unsigned integer that contains the minor version of 
                    the change journal software for this record.<26>");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R723");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R723
            //
            // ordinal number of the directory cannot be got on wire,
            // so just to verify its type
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(UInt64),
                reply.ParentFileReferenceNumber.GetType(),
                723,
                @"[In USN_RECORD]ParentFileReferenceNumber (8 bytes):  A 64-bit unsigned integer, opaque to the client, 
                containing the ordinal number of the directory on which the file or directory that is associated with 
                this record is located.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R724. ParentFileReferenceNumber: {0}", 
                reply.ParentFileReferenceNumber);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R724
            //
            //arbitrarily assigned value cannot be verified, so here verify value is in valid ranged
            bool isVerifyR724 = (reply.ParentFileReferenceNumber >= 0 && reply.ParentFileReferenceNumber <= UInt64.MaxValue);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR724,
                724,
                @"[In USN_RECORD]ParentFileReferenceNumber (8 bytes): This is an arbitrarily assigned value (unique 
                within the volume on which the file is stored) that associates a journal record with a parent directory.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R730");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R730
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Reason_Values),
                reply.Reason.GetType(),
                730,
                @"[In USN_RECORD]Reason (4 bytes):  A 32-bit unsigned integer that contains flags that indicate reasons 
                for changes that have accumulated in this file or directory journal record since the file or directory 
                was opened.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R734. Reason: {0}", reply.Reason);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R734
            //
            // used bits combination
            uint bitsUsed = ((uint)Reason_Values.USN_REASON_BASIC_INFO_CHANGE | (uint)Reason_Values.USN_REASON_CLOSE |
                             (uint)Reason_Values.USN_REASON_COMPRESSION_CHANGE | (uint)Reason_Values.USN_REASON_DATA_EXTEND |
                             (uint)Reason_Values.USN_REASON_DATA_OVERWRITE | (uint)Reason_Values.USN_REASON_DATA_TRUNCATION |
                             (uint)Reason_Values.USN_REASON_EA_CHANGE | (uint)Reason_Values.USN_REASON_ENCRYPTION_CHANGE |
                             (uint)Reason_Values.USN_REASON_FILE_CREATE | (uint)Reason_Values.USN_REASON_FILE_DELETE |
                             (uint)Reason_Values.USN_REASON_HARD_LINK_CHANGE | (uint)Reason_Values.USN_REASON_INDEXABLE_CHANGE |
                             (uint)Reason_Values.USN_REASON_NAMED_DATA_EXTEND | (uint)Reason_Values.USN_REASON_NAMED_DATA_OVERWRITE |
                             (uint)Reason_Values.USN_REASON_NAMED_DATA_TRUNCATION | (uint)Reason_Values.USN_REASON_OBJECT_ID_CHANGE |
                             (uint)Reason_Values.USN_REASON_RENAME_NEW_NAME | (uint)Reason_Values.USN_REASON_RENAME_OLD_NAME |
                             (uint)Reason_Values.USN_REASON_REPARSE_POINT_CHANGE | (uint)Reason_Values.USN_REASON_SECURITY_CHANGE |
                             (uint)Reason_Values.USN_REASON_STREAM_CHANGE);

            // Reason must use above bits
            bool isVerifyR734 = (((uint)reply.Reason & ~bitsUsed) == 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR734,
                734,
                @"[In USN_RECORD]Reason (4 bytes):  Possible values for the reason code are as follows (all unused bits 
                are reserved for future use and MUST NOT be used):[USN_REASON_BASIC_INFO_CHANGE
                    0x00008000, 
                    USN_REASON_CLOSE
                    0x80000000, 
                    USN_REASON_COMPRESSION_CHANGE
                    0x00020000, 
                    USN_REASON_DATA_EXTEND
                    0x00000002, 
                    USN_REASON_DATA_OVERWRITE
                    0x00000001, 
                    USN_REASON_DATA_TRUNCATION
                    0x00000004, 
                    USN_REASON_EA_CHANGE
                    0x00000400, 
                    USN_REASON_ENCRYPTION_CHANGE
                    0x00040000, 
                    USN_REASON_FILE_CREATE
                    0x00000100, 
                    USN_REASON_FILE_DELETE
                    0x00000200, 
                    USN_REASON_HARD_LINK_CHANGE
                    0x00010000, 
                    USN_REASON_INDEXABLE_CHANGE
                    0x00004000, 
                    USN_REASON_NAMED_DATA_EXTEND
                    0x00000020, 
                    USN_REASON_NAMED_DATA_OVERWRITE
                    0x00000010, 
                    USN_REASON_NAMED_DATA_TRUNCATION
                    0x00000040, 
                    USN_REASON_OBJECT_ID_CHANGE
                    0x00080000, 
                    USN_REASON_RENAME_NEW_NAME
                    0x00002000, 
                    USN_REASON_RENAME_OLD_NAME
                    0x00001000, 
                    USN_REASON_REPARSE_POINT_CHANGE
                    0x00100000, 
                    USN_REASON_SECURITY_CHANGE
                    0x00000800, USN_REASON_STREAM_CHANGE 0x00200000].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R717");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R717
            //
            //
            // The total length of the update sequence number
            bool isVerifyR717 = (reply.RecordLength.GetType() == typeof(uint)) &&
                (Marshal.SizeOf(reply.RecordLength) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR717,
                717,
                @"[In USN_RECORD]RecordLength (4 bytes):  A 32-bit unsigned integer that contains the total length of 
                the update sequence number (USN) record in bytes.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R725");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R725
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Int64),
                reply.Usn.GetType(),
                725,
                @"[In USN_RECORD]Usn (8 bytes):  A 64-bit signed integer, opaque to the client, containing the USN of 
                the record.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R726");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R726
            //
            // be unique cannot be verified, so here just to verify the type
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Int64),
                reply.Usn.GetType(),
                726,
                @"[In USN_RECORD]Usn (8 bytes):  This value is unique within the volume on which the file is stored.");

            // if no USN change journal records have been logged for the directory of the file associated with this record.
            if (!isUsnChangeLogged)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R728");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R728
                //
                Site.CaptureRequirementIfAreEqual<long>(
                    0,
                    reply.Usn,
                    728,
                    @"[In USN_RECORD]Usn (8 bytes):  This value MUST be 0 if no USN change journal records have been 
                    logged for the directory of the file associated with this record. ");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R727. Usn: {0}", reply.Usn);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R727
            //
            bool isVerifyR727 = (reply.Usn >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR727,
                727,
                @"[In USN_RECORD]Usn (8 bytes):  This value MUST be greater than or equal to 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R729. TimeStamp: {0}{1}", reply.TimeStamp.dwHighDateTime, 
                reply.TimeStamp.dwLowDateTime);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R729
            //
            bool isVerifyR729 = (reply.TimeStamp.GetType() 
                == typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME) &&
                reply.TimeStamp.dwHighDateTime >= 0 && reply.TimeStamp.dwLowDateTime >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR729,
                729,
                @"[In USN_RECORD]TimeStamp (8 bytes):  A structure containing the absolute system time this change 
                journal event was logged, expressed as the number of 100-nanosecond intervals since January 1, 1601 
                (UTC), in the format of a FILETIME (section 2.1.1) structure.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R763");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R763
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(UInt32),
                reply.SecurityId.GetType(),
                763,
                @"[In USN_RECORD]SecurityId (4 bytes):  A 32-bit unsigned integer that contains an index of a unique 
                security identifier assigned to the file or directory associated with this record.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R756");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R756
            //
            // SourceInfo has been parse into an enum by stack, 
            // so here just to verify the type is the one defined in stack
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(SourceInfo_Values),
                reply.SourceInfo.GetType(),
                756,
                @"[In USN_RECORD]SourceInfo (4 bytes):  A 32-bit unsigned integer that provides additional information 
                about the source of the change.");

            //a USN RECORD element returned by this FSCTL
            if (isReturnedByFSCTL)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, 
                    "Verify MS-FSCC_R775. Reason: {0}, TimeStamp: {1}{2}, SourceInfo: {3}, SecurityId: {4}", 
                    reply.Reason, reply.TimeStamp.dwHighDateTime, reply.TimeStamp.dwLowDateTime, 
                    reply.SourceInfo, reply.SecurityId);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R775
                //
                bool isVerifyR775 = (reply.Reason == 0 && reply.TimeStamp.dwHighDateTime == 0 
                    && reply.TimeStamp.dwLowDateTime == 0 && reply.SourceInfo == 0 && reply.SecurityId == 0);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR775,
                    775,
                    @"[In USN_RECORD]The fields Reason, TimeStamp, SourceInfo, and SecurityId for a USN RECORD element 
                    returned by this FSCTL MUST all be set 0.<28>");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Capture requirement in section 5

        /// <summary>
        /// Verify the response of FSCTL_FILESYSTEM_GET_STATISTICS Request
        /// </summary>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT or others</param>
        /// <param name="ReturnedStatusCode">the status code returned in response, as specified in [MS-ERREF] 
        /// section 2.3</param>
        public void VerifyMessageSyntaxResonseStatusForFsctlFileSystemGetStatisticsRequest(
            string typeOfFileSystem,
            FsctlFilesystemGetStatisticsReplyStatus ReturnedStatusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (isWindows)
            {
                if (typeOfFileSystem != "NTFS" && typeOfFileSystem != "FAT" && typeOfFileSystem != "exFAT")
                {
                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1856");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1856
                    //
                    // STATUS_INVALID_DEVICE_REQUEST 0xC0000010, as specified in [MS-ERREF] section 2.3

                    Site.CaptureRequirementIfAreEqual<uint>(
                        0xC0000010,
                        (uint)ReturnedStatusCode,
                        1856,
                        @"<10> Section 2.3.7: Other file systems[except NTFS, FAT, and exFAT file systems] return 
                        STATUS_INVALID_DEVICE_REQUEST.");

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1858");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1858
                    //
                    // STATUS_INVALID_DEVICE_REQUEST 0xC0000010, as specified in [MS-ERREF] section 2.3
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0xC0000010,
                        (uint)ReturnedStatusCode,
                        1858,
                        @"<11> Section 2.3.8: Other file systems[except NTFS, FAT, and exFAT file systems] return 
                        STATUS_INVALID_DEVICE_REQUEST.");
                }
                else
                {
                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1855");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1855
                    //
                    // STATUS_INVALID_DEVICE_REQUEST code is not returned
                    Site.CaptureRequirementIfAreNotEqual<uint>(
                        0xC0000010,
                        (uint)ReturnedStatusCode,
                        1855,
                        @"<10> Section 2.3.7: This FSCTL[FSCTL_FILESYSTEM_GET_STATISTICS Request] is implemented on NTFS, 
                        FAT, and exFAT file systems.");

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1857");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1857
                    //
                    // STATUS_INVALID_DEVICE_REQUEST code is not returned
                    Site.CaptureRequirementIfAreNotEqual<uint>(
                        0xC0000010,
                        (uint)ReturnedStatusCode,
                        1857,
                        @"<11> Section 2.3.8: This FSCTL[FSCTL_FILESYSTEM_GET_STATISTICS Reply]  is implemented on NTFS, 
                        FAT, and exFAT file systems.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FSCTL_GET_REPARSE_POINT Reply message
        /// </summary>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT , CDFS or others</param>
        /// <param name="ReturnedStatusCode">the status code returned in response, as specified in [MS-ERREF] 
        /// section 2.3</param>
        public void VerifyMessageSyntaxFsctlGetReparsePointReply(string typeOfFileSystem,
                                                                  FsctlGetReparsePointReplyStatus ReturnedStatusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            string ServerOS = Site.Properties["ServerOSPlatform"];
            if (ServerOS == "Win2k3" && ((typeOfFileSystem == "FAT") || (typeOfFileSystem == "CDFS")))
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1865");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1865
                //
                // STATUS_INVALID_DEVICE_REQUEST 0xC0000010, as specified in [MS-ERREF] section 2.3
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)FsctlGetReparsePointReplyStatus.STATUS_INVALID_DEVICE_REQUEST,
                    (uint)ReturnedStatusCode,
                    1865,
                    @"<15> Section 2.3.18: Windows Server?2003 returns STATUS_INVALID_DEVICE_REQUEST for a file on a 
                    FAT or CDFS file system. ");
            }

            if (ServerOS == "Win2k8" && ((typeOfFileSystem == "FAT") || (typeOfFileSystem == "CDFS")))
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1867");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1867
                //
                // STATUS_INVALID_DEVICE_REQUEST 0xC0000010, as specified in [MS-ERREF] section 2.3
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)FsctlGetReparsePointReplyStatus.STATUS_INVALID_DEVICE_REQUEST,
                    (uint)ReturnedStatusCode,
                    1867,
                    @"<15> Section 2.3.18: Windows Server?2008 returns STATUS_INVALID_DEVICE_REQUEST for a file on a 
                    FAT or CDFS file system.");
            }
            if (ServerOS == "Win2k" && ((typeOfFileSystem == "FAT") || (typeOfFileSystem == "CDFS")))
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1863");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1863
                //
                // STATUS_INVALID_DEVICE_REQUEST 0xC0000010, as specified in [MS-ERREF] section 2.3
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)FsctlGetReparsePointReplyStatus.STATUS_INVALID_DEVICE_REQUEST,
                    (uint)ReturnedStatusCode,
                    1863,
                    @"<15> Section 2.3.18: Windows?2000 returns STATUS_INVALID_DEVICE_REQUEST for a file on a 
                    FAT or CDFS file system. ");
            }
            if (ServerOS == "WinNT4.0" && ((typeOfFileSystem == "FAT") || (typeOfFileSystem == "CDFS")))
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1862");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1862
                //
                // STATUS_INVALID_DEVICE_REQUEST 0xC0000010, as specified in [MS-ERREF] section 2.3
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)FsctlGetReparsePointReplyStatus.STATUS_INVALID_DEVICE_REQUEST,
                    (uint)ReturnedStatusCode,
                    1862,
                    @"<15> Section 2.3.18: Windows?NT?4.0 returns STATUS_INVALID_DEVICE_REQUEST for a file on an NTFS, 
                    FAT, or CDFS file system. ");
            }
            if (ServerOS == "WinVista" && ((typeOfFileSystem == "FAT") || (typeOfFileSystem == "CDFS")))
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1866");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1866
                //
                // STATUS_INVALID_DEVICE_REQUEST 0xC0000010, as specified in [MS-ERREF] section 2.3
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)FsctlGetReparsePointReplyStatus.STATUS_INVALID_DEVICE_REQUEST,
                    (uint)ReturnedStatusCode,
                    1866,
                    @"<15> Section 2.3.18: Windows?Vista returns STATUS_INVALID_DEVICE_REQUEST for a file on a FAT 
                    or CDFS file system. ");
            }
            if (ServerOS == "WinXP" && ((typeOfFileSystem == "FAT") || (typeOfFileSystem == "CDFS")))
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1864");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1864
                //
                // STATUS_INVALID_DEVICE_REQUEST 0xC0000010, as specified in [MS-ERREF] section 2.3
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)FsctlGetReparsePointReplyStatus.STATUS_INVALID_DEVICE_REQUEST,
                    (uint)ReturnedStatusCode,
                    1864,
                    @"<15> Section 2.3.18: Windows?XP returns STATUS_INVALID_DEVICE_REQUEST for a file on a FAT or 
                    CDFS file system. ");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FSCTL_IS_PATHNAME_VALID Reply message
        /// </summary>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT , CDFS or others</param>
        /// <param name="ReturnedStatusCode">the status code returned in response, as specified in [MS-ERREF] 
        /// section 2.3</param>
        public void VerifyMessageSyntaxFsctlIsPathNameValidReply(string typeOfFileSystem,
                                                                  uint ReturnedStatusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            string ServerOS = Site.Properties["ServerOSPlatform"];
            if ((ServerOS == "Win2k3" || ServerOS == "Win2k8" || ServerOS == "Win2k" || ServerOS == "WinVista" 
                || ServerOS == "WinXP") && ((typeOfFileSystem == "FAT") || (typeOfFileSystem == "NTFS")))
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1870");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1870
                //
                // STATUS_SUCCESS 0x00000000 as specified in [MS-ERREF] section 2.3
                Site.CaptureRequirementIfAreEqual<uint>(
                    0x00000000,
                    ReturnedStatusCode,
                    1870,
                    @"<17> Section 2.3.22:  NTFS and FAT on Windows?2000, Windows?XP, Windows Server?2003, Windows?Vista, 
                    and Windows Server?2008 ignore the input parameter and return STATUS_SUCCESS whenever this FSCTL 
                    is invoked.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FSCTL_IS_VOLUME_DIRTY Reply message
        /// </summary>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT , CDFS or others</param>
        /// <param name="Flags">the field in FSCTL_IS_VOLUME_DIRTY Reply message</param>
        public void VerifyMessageSyntaxFsctlIsVolumeDirtyReply(string typeOfFileSystem,
                                                           uint Flags)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (typeOfFileSystem == "NTFS")
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1872");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1872
                //
                Site.CaptureRequirementIfAreNotEqual<uint>(
                    0x00000004,
                    Flags,
                    1872,
                    @"<19> Section 2.3.24: This[Flags (4 bytes): Value VOLUME_SESSION_OPEN 0x00000004] is not 
                    applicable for NTFS.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply about FormattingImplementationInfo and 
        /// LastModifyingImplementationInfo field
        /// </summary>
        /// <param name="reply">the FSCTL_QUERY_ON_DISK_VOLUME_INFO reply message</param>
        public void VerifyMessageSyntaxFsctlQueryOnDiskVolumeInfoReply(FSCTL_QUERY_ON_DISK_VOLUME_INFO_Reply reply)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (bool.Parse(Site.Properties["IsWindows"]))
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1883. FormattingImplementationInfo: {0}", 
                    reply.FormattingImplementationInfo);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1883
                //
                bool isVerifyR1883 = (reply.FormattingImplementationInfo.Equals("*Microsoft Windows"));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1883,
                    1883,
                    "<23> Section 2.3.40: This value[FormattingImplementationInfo] is set to \"*Microsoft Windows\" when the media is formatted on Windows.");

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1884. LastModifyingImplementationInfo: {0}", reply.LastModifyingImplementationInfo);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1884
                //
                bool isVerifyR1884 = (reply.LastModifyingImplementationInfo.Equals("*Microsoft Windows"));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1884,
                    1884,
                    "<24> Section 2.3.40: This value[LastModifyingImplementationInfo] is set to \"*Microsoft Windows\" when the media is written to on a Windows system.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FSCTL_READ_FILE_USN_DATA Reply 
        /// </summary>
        /// <param name="reply">the FSCTL_READ_FILE_USN_DATA reply message</param>
        public void VerifyMessageSyntaxFsctlReadFileUsnDataReply(FSCTL_READ_FILE_USN_DATA_Reply reply)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            string ServerOS = Site.Properties["ServerOSPlatform"];
            if (ServerOS == "Win2k" || ServerOS == "WinXP" || ServerOS == "Win2k3" || ServerOS == "WinVista" || ServerOS == "Win2k8")
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1885");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1885
                //
                Site.CaptureRequirementIfAreEqual<ushort>(
                    2,
                    reply.MajorVersion,
                    1885,
                    @"<25> Section 2.3.44:  The major version number[In USN_RECORD] is 2 for file systems created on 
                    Windows?2000, Windows?XP, Windows Server?2003, Windows?Vista, and Windows Server?2008.");

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1886");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1886
                //
                Site.CaptureRequirementIfAreEqual<ushort>(
                    0,
                    reply.MinorVersion,
                    1886,
                    @"<26> Section 2.3.44: The minor version number[In USN_RECORD] is 0 for file systems created on 
                    Windows?2000, Windows?XP, Windows Server?2003, Windows?Vista, and Windows Server?2008.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FSCTL_SET_ENCRYPTION Request
        /// </summary>
        /// <param name="typeOfFileSystem">the type of file system, NTFS, FAT, exFAT , CDFS or others</param>
        public void VerifyMessageSyntaxFsctlSetEecryptionRequest(string typeOfFileSystem)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1891");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1891
            //
            Site.CaptureRequirementIfAreEqual<string>(
                "NTFS",
                typeOfFileSystem,
                1891,
                @"<31> Section 2.3.51: This message[FSCTL_SET_ENCRYPTION Request] is only implemented on NTFS.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FSCTL_SET_ENCRYPTION Reply
        /// </summary>
        /// <param name="expectedNTFSMajorVersion">the NTFS Major Version</param>
        /// <param name="ReturnedStatusCode">the status code returned in response, as specified in [MS-ERREF] 
        /// section 2.3</param>
        /// <param name="isEncyptDrvLoadedorClearEncryptRequested">check if the NTFS encryption driver is not 
        /// loaded or the FILE_CLEAR_ENCRYPTION operation was requested</param>
        /// <param name="isINDEX_ROOTattributeFound">check if the $INDEX_ROOT attribute of the directory that was 
        /// trying to be encrypted, is found</param>
        public void VerifyMessageSyntaxFsctlSetEncryptionReply(ushort expectedNTFSMajorVersion,
                                                                FsctlSetEncryptionReplyStatus ReturnedStatusCode,
                                                                bool isEncyptDrvLoadedorClearEncryptRequested,
                                                                bool isINDEX_ROOTattributeFound)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (bool.Parse(Site.Properties["IsWindows"]))
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1898.");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1898
                //
                bool isVerifyR1898 = (expectedNTFSMajorVersion >= 2);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1898,
                    1898,
                    @"<36> Section 2.3.52:  On Windows, encryption requires NTFS major version 2 or greater.");

                if (isEncyptDrvLoadedorClearEncryptRequested)
                {
                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1899");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1899
                    //
                    // STATUS_INVALID_DEVICE_REQUEST 0xC0000010
                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)FsctlSetEncryptionReplyStatus.STATUS_INVALID_DEVICE_REQUEST,
                        (uint)ReturnedStatusCode,
                        1899,
                        @"<37> Section 2.3.52: Windows returns this error code[STATUS_INVALID_DEVICE_REQUEST 0xC0000010] 
                        if the NTFS encryption driver is not loaded or the FILE_CLEAR_ENCRYPTION operation was requested 
                        on a file containing a stream that is still marked as encrypted.");
                }

                if (!isINDEX_ROOTattributeFound)
                {
                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1900");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1900
                    //
                    //STATUS_FILE_CORRUPT_ERROR 0xC0000102
                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)FsctlSetEncryptionReplyStatus.STATUS_FILE_CORRUPT_ERROR,
                        (uint)ReturnedStatusCode,
                        1900,
                        @"<38> Section 2.3.52:  Windows returns this error code[STATUS_FILE_CORRUPT_ERROR 0xC0000102] 
                        if the $INDEX_ROOT attribute of the directory that was trying to be encrypted, could not be found.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FSCTL_SET_SHORT_NAME_BEHAVIOR Request
        /// </summary>
        /// <param name="typeOfFileSystem">the type of file system, NTFS, FAT, exFAT , CDFS or others</param>
        public void VerifyMessageSyntaxFsctlSetShortNameBehaviorRequest(string typeOfFileSystem)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            string ServerOS = Site.Properties["ServerOSPlatform"];
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1094. The OS platform is {0}", ServerOS);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1094
            //
            Site.CaptureRequirementIfAreEqual<string>(
                "WinPE",
                ServerOS,
                1094,
                @"[In FILE_COMPRESSION_INFORMATION]CompressedFileSize (8 bytes):  
                A 64-bit signed integer that contains the size, in bytes, of the compressed file.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1096");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1096
            //
            Site.CaptureRequirementIfAreEqual<string>(
                "NTFS",
                typeOfFileSystem,
                1096,
                @"<42> Section 2.3.59: This operation is[FSCTL_SET_SHORT_NAME_BEHAVIOR Request] only implemented on NTFS.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FSCTL_SET_SHORT_NAME_BEHAVIOR Reply
        /// </summary>
        public void VerifyMessageSyntaxFsctlSetShortNameBehaviorReply()
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            string ServerOS = Site.Properties["ServerOSPlatform"];
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R
            //
            Site.CaptureRequirementIfAreEqual<string>(
                "WinPE",
                ServerOS,
                1907,
                @"<43> Section 2.3.60: Windows supports this FSCTL[FSCTL_SET_SHORT_NAME_BEHAVIOR Reply] only on WinPE.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        

        /// <summary>
        /// Verify the structure of FileCompressionInformation
        /// </summary>
        /// <param name="info">the instance of this FileCompressionInformation</param>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT or others</param>
        /// <param name="expectedInitializedClusterShift">cluster size set for the file system at initialization. 
        /// NTFS is 12 by defaults</param>
        public void VerifyDataTypeFileCompressionInformation(FileCompressionInformation info,
                                                              string typeOfFileSystem,
                                                              byte expectedInitializedClusterShift)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (typeOfFileSystem == "NTFS")
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1940");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1940
                //
                // expectedInitializedClusterShift is 12 by defaults or initialized with a different cluster size
                Site.CaptureRequirementIfAreEqual<byte>(
                    expectedInitializedClusterShift,
                    info.ClusterShift,
                    1940,
                    @"<59> Section 2.4.9:NTFS defaults to a 4-kilobyte cluster size, resulting in a ClusterShift value 
                    of 12, but NTFS file systems can be initialized with a different cluster size, so the value may vary.");

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1945");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1945
                //
                // set to be an initialized value
                Site.CaptureRequirementIfAreEqual<byte>(
                    expectedInitializedClusterShift,
                    info.ClusterShift,
                    1945,
                    @"<61> Section 2.4.9: If an NTFS file system is initialized with a different cluster size, the value 
                    of ClusterShift would be log 2 of the cluster size for that file system.");

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1943");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1943
                // depended on an initialized value
                Site.CaptureRequirementIfAreEqual<byte>(
                    expectedInitializedClusterShift,
                    info.ClusterShift,
                    1943,
                    @"<61> Section 2.4.9: The value of this field[ClusterShift] depends on the cluster size set for the 
                    file system at initialization.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FileDirectoryInformation fields
        /// </summary>
        /// <param name="FileIndex">the FileIndex field in FileDirectoryInformation structure</param>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT or others</param>
        public void VerifyDataTypeFileDirectoryInformation(uint FileIndex, string typeOfFileSystem)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (typeOfFileSystem == "NTFS")
            {
                string ServerOS = Site.Properties["ServerOSPlatform"];
                if (ServerOS == "Win2k" || ServerOS == "WinXP" || ServerOS == "Win2k3" || ServerOS == "WinVista" || ServerOS == "Win2k8")
                {

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1947");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1947
                    //
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0,
                        FileIndex,
                        1947,
                        @"<62> Section 2.4.10: Windows?2000, Windows?XP, Windows Server?2003, Windows?Vista, and Windows 
                        Server?2008 set this value[FileIndex] to 0 for files on NTFS file systems.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FileFullDirectoryInformation fields
        /// </summary>
        /// <param name="FileIndex">the FileIndex field in FileFullDirectoryInformation structure</param>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT or others</param>
        public void VerifyDataTypeFileFullDirectoryInformation(uint FileIndex, string typeOfFileSystem)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (typeOfFileSystem == "NTFS")
            {
                string ServerOS = Site.Properties["ServerOSPlatform"];
                if (ServerOS == "Win2k" || ServerOS == "WinXP" || ServerOS == "Win2k3" || ServerOS == "WinVista" || ServerOS == "Win2k8")
                {

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1950");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1950
                    //
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0,
                        FileIndex,
                        1950,
                        @"<64> Section 2.4.14: Windows?2000, Windows?XP, Windows Server?2003, Windows?Vista, and Windows 
                        Server?2008 set this value[FileIndex] to 0 for files on NTFS file systems.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FileIdBothDirectoryInformation fields
        /// </summary>
        /// <param name="FileIndex">the FileIndex field in FileIdBothDirectoryInformation structure</param>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT or others</param>
        public void VerifyDataTypeFileIdBothDirectoryInformation(uint FileIndex, string typeOfFileSystem)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (typeOfFileSystem == "NTFS")
            {
                string ServerOS = Site.Properties["ServerOSPlatform"];
                if (ServerOS == "Win2k" || ServerOS == "WinXP" || ServerOS == "Win2k3" || ServerOS == "WinVista" || ServerOS == "Win2k8")
                {

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1952");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1952
                    //
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0,
                        FileIndex,
                        1952,
                        @"<65> Section 2.4.17: Windows?2000, Windows?XP, Windows Server?2003, Windows?Vista, and Windows 
                        Server?2008 set this value[FileIndex] to 0 for files on NTFS file systems.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FileIdFullDirectoryInformation fields
        /// </summary>
        /// <param name="FileIndex">the FileIndex field in FileIdFullDirectoryInformation structure</param>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT or others</param>
        public void VerifyDataTypeFileIdFullDirectoryInformation(uint FileIndex, string typeOfFileSystem)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (typeOfFileSystem == "NTFS")
            {
                string ServerOS = Site.Properties["ServerOSPlatform"];
                if (ServerOS == "Win2k" || ServerOS == "WinXP" || ServerOS == "Win2k3" || ServerOS == "WinVista" || ServerOS == "Win2k8")
                {

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1954");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1954
                    //
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0,
                        FileIndex,
                        1954,
                        @"<66> Section 2.4.18: Windows?2000, Windows?XP, Windows Server?2003, Windows?Vista, and Windows 
                        Server?2008 set this value[FileIndex] to 0 for files on NTFS file systems.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FileNamesInformation fields
        /// </summary>
        /// <param name="FileIndex">the FileIndex field in FileNamesInformation structure</param>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT or others</param>
        public void VerifyDataTypeFileNamesInformation(uint FileIndex, string typeOfFileSystem)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (typeOfFileSystem == "NTFS")
            {
                string ServerOS = Site.Properties["ServerOSPlatform"];
                if (ServerOS == "Win2k" || ServerOS == "WinXP" || ServerOS == "Win2k3" || ServerOS == "WinVista" || ServerOS == "Win2k8")
                {

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1956");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1956
                    //
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0,
                        FileIndex,
                        1956,
                        @"<67> Section 2.4.26: Windows?000, Windows XP, Windows Server 2003, Windows Vista, and Windows 
                    Server 2008 set this value[FileIndex] to 0 for files on NTFS file systems.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FileFsAttributeInformation fields
        /// </summary>
        /// <param name="info">the instance of FileFsAttributeInformation</param>
         [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public void VerifyDataTypeFileFsAttributeInformation(FileFsAttributeInformation info)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            string ServerOS = Site.Properties["ServerOSPlatform"];

            // check if OS is not the specified one, FILE_READ_ONLY_VOLUME is not set
            if (ServerOS != "WinXP" && ServerOS != "Win2k3" && ServerOS != "WinVista" && ServerOS != "Win2k8")
            {

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1967. FileSystemAttributes: {0}", info.FileSystemAttributes);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1967
                //
                // check FILE_READ_ONLY_VOLUME bit is not set
                bool isVerifyR1967 = ((info.FileSystemAttributes & FileSystemAttributes_Values.FILE_READ_ONLY_VOLUME) == 0);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1967,
                    1967,
                    @"<76> Section 2.5.1: This attribute[FILE_READ_ONLY_VOLUME 0x00080000] is only available on Windows?XP, 
                    Windows Server?2003, Windows?Vista, and Windows Server?2008.");
            }

            if (ServerOS == "WinVistaSP1" || ServerOS == "Win2k8" || ServerOS == "Win7")
            {

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1972. FileSystemName: {0}", info.FileSystemName);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1972
                //
                bool isVerifyR1972 = (info.FileSystemName.Equals("FAT") ||
                                      info.FileSystemName.Equals("FAT16") ||
                                      info.FileSystemName.Equals("FAT32") ||
                                      info.FileSystemName.Equals("exFAT") ||
                                      info.FileSystemName.Equals("NTFS") ||
                                      info.FileSystemName.Equals("CDFS") ||
                                      info.FileSystemName.Equals("UDF"));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1972,
                    1972,
                    @"<79> Section 2.5.1: For Windows?Vista SP1 and Windows Server?2008, valid values [for FileSystemName] 
                    are: ""FAT"",""FAT16"",""FAT32"",""exFAT"",""NTFS"",""CDFS"",and ""UDF"".");


                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1971. FileSystemName: {0}", info.FileSystemName);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1971
                //
                bool isVerifyR1971 = (info.FileSystemName.Equals("FAT") ||
                                      info.FileSystemName.Equals("FAT16") ||
                                      info.FileSystemName.Equals("FAT32") ||
                                      info.FileSystemName.Equals("exFAT") ||
                                      info.FileSystemName.Equals("NTFS") ||
                                      info.FileSystemName.Equals("CDFS") ||
                                      info.FileSystemName.Equals("UDF"));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1971,
                    1971,
                    @"<79> Section 2.5.1:For Windows 7, valid values [for FileSystemName] are: ""FAT"",""FAT16"",""FAT32"",""exFAT"",""NTFS"",""CDFS"",""UDF"".");
            }

            if (ServerOS == "WinXP")
            {

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1974. FileSystemName: {0}", info.FileSystemName);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1974
                //
                bool isVerifyR1974 = (info.FileSystemName.Equals("FAT") ||
                                      info.FileSystemName.Equals("FAT16") ||
                                      info.FileSystemName.Equals("FAT32") ||
                                      info.FileSystemName.Equals("NTFS") ||
                                      info.FileSystemName.Equals("CDFS"));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1974,
                    1974,
                    @"<79> Section 2.5.1: For Windows?XP, valid values [for FileSystemName] are: ""FAT"",""FAT16"",""FAT32"",""NTFS"",and ""CDFS"".");
            }

            if (ServerOS == "WinVista")
            {

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1973. FileSystemName: {0}", info.FileSystemName);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1973
                //
                bool isVerifyR1973 = (info.FileSystemName.Equals("FAT") ||
                                      info.FileSystemName.Equals("FAT16") ||
                                      info.FileSystemName.Equals("FAT32") ||
                                      info.FileSystemName.Equals("NTFS") ||
                                      info.FileSystemName.Equals("CDFS") ||
                                      info.FileSystemName.Equals("UDF"));


                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1973,
                    1973,
                    @"<79> Section 2.5.1:For Windows?Vista RTM, valid values [for FileSystemName] are: ""FAT"",""FAT16"",
                    ""FAT32"",""NTFS"",""CDFS"",and ""UDF"".");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FileFsControlInformation fields
        /// </summary>
        /// <param name="info">the instance of FileFsControlInformation</param>
        public void VerifyDataTypeFileFsControlInformation(FileFsControlInformation info)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (bool.Parse(Site.Properties["SUT.Platform.OS.isWindows"]))
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1976");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1976
                //
                Site.CaptureRequirementIfAreEqual<long>(
                    0,
                    info.FreeSpaceThreshold,
                    1976,
                    @"<81> Section 2.5.2: Windows sets this value[FreeSpaceThreshold] to 0.");

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1977");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1977
                //
                Site.CaptureRequirementIfAreEqual<long>(
                    0,
                    info.FreeSpaceStopFiltering,
                    1977,
                    @"<82> Section 2.5.2: Windows sets this value[FreeSpaceStopFiltering] to 0.");

                // the bit flags combination
                uint bitsCombination = ((uint)FileSystemControlFlags_Values.FILE_VC_CONTENT_INDEX_DISABLED |
                                        (uint)FileSystemControlFlags_Values.FILE_VC_LOG_QUOTA_LIMIT |
                                        (uint)FileSystemControlFlags_Values.FILE_VC_LOG_QUOTA_THRESHOLD |
                                        (uint)FileSystemControlFlags_Values.FILE_VC_LOG_VOLUME_LIMIT |
                                        (uint)FileSystemControlFlags_Values.FILE_VC_LOG_VOLUME_THRESHOLD |
                                        (uint)FileSystemControlFlags_Values.FILE_VC_QUOTA_ENFORCE |
                                        (uint)FileSystemControlFlags_Values.FILE_VC_QUOTA_TRACK |
                                        (uint)FileSystemControlFlags_Values.FILE_VC_QUOTAS_INCOMPLETE |
                                        (uint)FileSystemControlFlags_Values.FILE_VC_QUOTAS_REBUILDING);
                // FileSystemControlFlags is set to any other bit except all above
                if (((uint)info.FileSystemControlFlags & ~bitsCombination) == 0)
                {

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1978");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1978
                    //
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0,
                        (uint)info.FileSystemControlFlags,
                        1978,
                        @"<83> Section 2.5.2: Windows sets flags not defined below[[FILE_VC_CONTENT_INDEX_DISABLED 
                        0x00000008, FILE_VC_LOG_QUOTA_LIMIT 0x00000020, FILE_VC_LOG_QUOTA_THRESHOLD 0x00000010, 
                        FILE_VC_LOG_VOLUME_LIMIT 0x00000080, FILE_VC_LOG_VOLUME_THRESHOLD 0x00000040, 
                        FILE_VC_QUOTA_ENFORCE 0x00000002, FILE_VC_QUOTA_TRACK 0x00000001, FILE_VC_QUOTAS_INCOMPLETE 
                        0x00000100, FILE_VC_QUOTAS_REBUILDING 0x00000200] to zero.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FileFsVolumeInformation fields
        /// </summary>
        /// <param name="info">the instance of FileFsVolumeInformation</param>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT or others</param>
        public void VerifyDataTypeFileFsVolumeInformation(FileFsVolumeInformation info,
                                                           string typeOfFileSystem)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (isWindows)
            {
                if (typeOfFileSystem == "NTFS")
                {
                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1986");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1986
                    //
                    // 0x01 for TRUE
                    Site.CaptureRequirementIfAreEqual<byte>(
                        0x01,
                        (byte)info.SupportsObjects,
                        1986,
                        @"<91> Section 2.5.8: This value[SupportsObjects] is TRUE for NTFS.");
                }
                else
                {

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1987");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1987
                    //
                    // 0x00 for FALSE
                    Site.CaptureRequirementIfAreEqual<byte>(
                        0x00,
                        (byte)info.SupportsObjects,
                        1987,
                        @"<91> Section 2.5.8: This value[SupportsObjects] is FALSE for other file systems[except NTFS] 
                        implemented by Windows.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the return of the FileObjectIdInformation query
        /// </summary>
        /// <param name="returnedStatusCode">the returned status code</param>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT or others</param>
        public void VerifyResponseForFileObjectIdInformationQuery(uint returnedStatusCode,
                                                                   string typeOfFileSystem)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (bool.Parse(Site.Properties["IsWindows"]) && typeOfFileSystem == "FAT")
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1957");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1957
                //
                // STATUS_INVALID_DEVICE_REQUEST 0xC0000010, as specified in [MS-ERREF] section 2.3
                Site.CaptureRequirementIfAreEqual<uint>(
                    0xC0000010,
                    returnedStatusCode,
                    1957,
                    @"<68> Section 2.4.28: The Microsoft FAT file system does not support the use of ObjectIds, 
                    and returns a status code of STATUS_INVALID_DEVICE_REQUEST.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the returned Status Codes for FSCTL
        /// </summary>
        /// <param name="returnedStatusCode">the returned status code</param>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT or others</param>
        /// <param name="isRequiredFileSystemInstalled">check if the file system is installed, which support a required 
        /// file system filter</param>
        public void VerifyReturnStatusCodeForFSCTL(uint returnedStatusCode,
                                                    string typeOfFileSystem,
                                                    bool isRequiredFileSystemInstalled)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (bool.Parse(Site.Properties["IsWindows"]) && typeOfFileSystem == "FAT")
            {
                if (typeOfFileSystem == "FAT")
                {
                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1849");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1849
                    //
                    // STATUS_INVALID_DEVICE_REQUEST 0xC0000010, as specified in [MS-ERREF] section 2.3
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0xC0000010,
                        returnedStatusCode,
                        1849,
                        @"<7> Section 2.2: FSCTLs involving these technologies[Reparse points, object IDs, and the 
                        update sequence number (USN) change journal] will return STATUS_INVALID_DEVICE_REQUEST when 
                        the specified file, directory, or volume is located on a volume formatted with the FAT file system.");
                }

                // not install
                if (!isRequiredFileSystemInstalled)
                {

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1850");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1850
                    //
                    // STATUS_INVALID_DEVICE_REQUEST 0xC0000010, as specified in [MS-ERREF] section 2.3
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0xC0000010,
                        returnedStatusCode,
                        1850,
                        @"<7> Section 2.2: Windows also returns STATUS_INVALID_DEVICE_REQUEST when a required file 
                        system filter is supported by the file system but is not installed (see section 2.3.68).");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the fields of structure FILE_FS_FULL_SIZE_INFORMATION
        /// </summary>
        /// <param name="info">the instance of FILE_FS_FULL_SIZE_INFORMATION</param>
        /// <param name="isPerUserQuotasUsed">FILE_VOLUME_QUOTAS in FileSystemAttributes indicate that the file system 
        /// supports per-user quotas.</param>
        /// <param name="expectedTotalFreeAllocationUnits">the the total number of free allocation units on the disk</param>
        public void VerifyDataTypeFileFsFullSizeInformationField(FileFsFullSizeInformation info,
                                                                  bool isPerUserQuotasUsed,
                                                                  uint expectedTotalFreeAllocationUnits)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            string ServerOS = Site.Properties["ServerOSPlatform"];
            if (ServerOS == "Win2k" || ServerOS == "WinXP" || ServerOS == "Win2k3" || ServerOS == "WinVista" ||
                ServerOS == "Win2k8")
            {
                if (isPerUserQuotasUsed)
                {

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1981. CallerAvailableAllocationUnits: {0}", 
                        info.CallerAvailableAllocationUnits);

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1981
                    //
                    bool isVerifyR1981 = (info.CallerAvailableAllocationUnits < expectedTotalFreeAllocationUnits);

                    Site.CaptureRequirementIfIsTrue(
                        isVerifyR1981,
                        1981,
                        @"<86> Section 2.5.4: In Windows 2000, Windows XP, Windows Server 2003, Windows Vista, and 
                        Windows Server 2008, if per-user quotas are in use, this value[CallerAvailableAllocationUnits] 
                        may be less than the total number of free allocation units on the disk.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the return of the FileFsObjectIdInformation query
        /// </summary>
        /// <param name="info"> the instance of FileFsObjectIdInformation</param>
        /// <param name="returnedStatusCode">the returned status code</param>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT or others</param>
        public void VerifyResponseForFileFsObjectIdInformationQuery(FileFsObjectIdInformation info,
                                                                     uint returnedStatusCode,
                                                                     string typeOfFileSystem)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (bool.Parse(Site.Properties["SUT.Platform.OS.isWindows"]))
            {
                if (typeOfFileSystem == "FAT")
                {
                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1982");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1982
                    //
                    // STATUS_INVALID_DEVICE_REQUEST 0xC0000010, as specified in [MS-ERREF] section 2.3
                    Site.CaptureRequirementIfAreEqual<uint>(
                        0xC0000010,
                        returnedStatusCode,
                        1982,
                        @"<87> Section 2.5.6: The Microsoft FAT file system does not support the use of object IDs, 
                        and returns a status code of STATUS_INVALID_DEVICE_REQUEST.");
                }


                //
                // Verify MS-FSCC requirement: MS-FSCC_R1983
                //
                bool isVerifyR1983 = true;

                // the length of ExtendedInfo is fixed 48 bytes
                if (info.ExtendedInfo.Length != 48)
                {
                    isVerifyR1983 = false;
                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1983. The length of ExtendedInfo: {0}", 
                        info.ExtendedInfo.Length);

                }
                else
                {
                    // not write information means 48 bytes of 0x00 in this field
                    for (int i = 0; i < 48; i++)
                    {
                        if (info.ExtendedInfo[i] != 0x00)
                        {
                            //
                            // Add the debug information
                            //
                            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1983. ExtendedInfo[{0}] : {1}", i, 
                                info.ExtendedInfo[i]);
                            isVerifyR1983 = false;
                            break;
                        }
                    }
                }

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1983,
                    1983,
                    @"<88> Section 2.5.6: Windows does not write information into the ExtendedInfo field for file systems.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the fields of structure FileFsSizeInformation
        /// </summary>
        /// <param name="info">the instance of FILE_FS_SIZE_INFORMATION</param>
        /// <param name="isPerUserQuotasUsed">FILE_VOLUME_QUOTAS in FileSystemAttributes indicate that the file system 
        /// supports per-user quotas.</param>
        /// <param name="expectedTotalFreeAllocationUnits">the the total number of free allocation units on the disk</param>
        public void VerifyDataTypeFileFsSizeInformationField(FileFsSizeInformation info,
                                                                  bool isPerUserQuotasUsed,
                                                                  uint expectedTotalFreeAllocationUnits)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            string ServerOS = Site.Properties["ServerOSPlatform"];
            if (ServerOS == "Win2k" || ServerOS == "WinXP" || ServerOS == "Win2k3" || ServerOS == "WinVista" ||
                ServerOS == "Win2k8")
            {
                if (isPerUserQuotasUsed)
                {

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1985. ActualAvailableAllocationUnits: {0}", 
                        info.ActualAvailableAllocationUnits);

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1985
                    //
                    bool isVerifyR1985 = (info.ActualAvailableAllocationUnits < expectedTotalFreeAllocationUnits);

                    Site.CaptureRequirementIfIsTrue(
                        isVerifyR1985,
                        1985,
                        @"<90> Section 2.5.7: In Windows 2000, Windows XP, Windows Server 2003, Windows Vista, and 
                        Windows Server 2008, if per-user quotas are in use, this value[ActualAvailableAllocationUnits] 
                        may be less than the total number of free allocation units on the disk.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the File Attributes 
        /// </summary>
        /// <param name="FileAttributes">the FileAttributes field</param>
        public void VerifyDataTypeFileAttributes(uint FileAttributes)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (bool.Parse(Site.Properties["IsWindows"]))
            {

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1988");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1988
                //
                // FILE_ATTRIBUTE_NORMAL 0x00000080 is not set
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    FileAttributes & 0x00000080,
                    1988,
                    @"<92> Section 2.6: The Windows file system does not persist the FILE_ATTRIBUTE_NORMAL flag.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        /// <summary>
        /// Verify the FSCTL_GET_COMPRESSION Reply from server
        /// </summary>
        /// <param name="typeOfFileSystem">the type of file system: NTFS, FAT, exFAT or others</param>
        /// <param name="returnedStatusCode">the status code returned in response, as specified in [MS-ERREF] section 2.3</param>
        public void VerifyMessageSyntaxFsctlGetCompressionReply(string typeOfFileSystem, uint returnedStatusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            string ServerOS = Site.Properties["ServerOSPlatform"];
            if (ServerOS == "Win2k" || ServerOS == "WinXP" || ServerOS == "Win2k3" || ServerOS == "WinVista" ||
                ServerOS == "Win2k8")
            {
                Site.DefaultProtocolDocShortName = "MS-FSCC";
                if (typeOfFileSystem == "NTFS")
                {

                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1861");

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R1861
                    //
                    // to support indicate the status code STATUS_INVALID_DEVICE_REQUEST 0xC0000010 not be returned
                    // a cluster size is internal context, here no need to verify
                    Site.CaptureRequirementIfAreNotEqual<uint>(
                        0xC0000010,
                        returnedStatusCode,
                        1861,
                        @"<14> Section 2.3.12: Windows 2000, Windows XP, Windows Server 2003, Windows Vista, and 
                        Windows Server 2008 support file compression on volumes that are formatted with the NTFS file 
                        system and have a cluster size less than or equal to 4 kilobytes.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify data type TARGET_LINK_TRACKING_INFORMATION_BUFFER_1 section 2.3.27.1
        /// <summary>
        /// Verify data type TARGET_LINK_TRACKING_INFORMATION_BUFFER_1 
        /// </summary>
        /// <param name="request">The FSCTL_LMR_SET_LINK_TRACKING_INFORMATION Request</param>
        /// <param name="isNetBiosNameKnown">If the NetBIOS name of the destination computer is known</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void VerifyDataTypeTargetLinkTrackingInformationBuffer1(
            FsccFsctlLmrSetLinkTrackingInformationRequestPacket request, bool isNetBiosNameKnown)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (request.Payload.TargetLinkTrackingInformationLength < 36)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R555,TargetLinkTrackingInformationBuffer:{0}", 
                    BitConverter.ToString(request.Payload.TargetLinkTrackingInformationBuffer));

                //
                // Verify MS-FSCC requirement: MS-FSCC_R555
                //
                bool isVerifyR555 = false;
                // The NetBiosName is a NULL-terminated ASCII string
                int netBiosLen = request.Payload.TargetLinkTrackingInformationBuffer.Length;
                if (isNetBiosNameKnown)
                {
                    isVerifyR555 = ('\0' == request.Payload.TargetLinkTrackingInformationBuffer[netBiosLen - 1]);
                }
                else
                    isVerifyR555 = (request.Payload.TargetLinkTrackingInformationBuffer.Length == 0);
                
                Site.CaptureRequirementIfIsTrue(
                    isVerifyR555,
                    555,
                    @"[In TARGET_LINK_TRACKING_INFORMATION_Buffer_1]If the TargetLinkTrackingInformationLength value is less than 36, the TARGET_LINK_TRACKING_INFORMATION_Buffer data element MUST be as follows:[NetBIOSName (variable)].");

                if (isNetBiosNameKnown)
                {
                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R556:{0}", 
                        BitConverter.ToString(request.Payload.TargetLinkTrackingInformationBuffer));

                    //
                    // Verify MS-FSCC requirement: MS-FSCC_R556
                    //
                    // The latter half of the RS is explaining the context of NetBIOSName is informative
                    bool isVerifyR556 = ('\0' == request.Payload.TargetLinkTrackingInformationBuffer[netBiosLen - 1]);

                    Site.CaptureRequirementIfIsTrue(
                        isVerifyR556,
                        556,
                        @"[In TARGET_LINK_TRACKING_INFORMATION_Buffer_1]NetBIOSName (variable):  A NULL-terminated ASCII string containing the NetBIOS name of the destination computer, if known.");
                }
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply section 2.3.40
        /// <summary>
        /// Verify data type FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply 
        /// </summary>
        /// <param name="response">The FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply </param>
        /// <param name="statusCode">The status code of the FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply </param>
        /// <param name="isDirectoryNumberKnown">If the directory number known</param>
        /// <param name="isMajorVersionKnow">If the major version number of the file system is known and applicable.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void VerifyDataTypeFsctlQueryOnDiskVolumeInfoReply(
            FsccFsctlQueryOnDiskVolumeInfoResponsePacket response,
            uint statusCode,
            bool isDirectoryNumberKnown,
            bool isMajorVersionKnow)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the comment information for debugging
            //
            Site.Log.Add(LogEntryKind.Comment,
                "Verify MS-FSCC_R672 DirectoryCount: {0};",
                response.Payload.DirectoryCount);
            //
            // Verify MS-FSCC requirement 672
            //
            bool isVerifyR672 = (response.Payload.DirectoryCount.GetType() == typeof(Int64))
                                   && (Marshal.SizeOf(response.Payload.DirectoryCount) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR672,
                672,
                @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply]DirectoryCount (8 bytes):  A 64-bit signed integer.");
            //
            // Add the comment information for debugging
            //
            Site.Log.Add(LogEntryKind.Comment,
                "Verify MS-FSCC_R676 FileCount: {0};",
                response.Payload.FileCount);

            //
            // Verify MS-FSCC requirement 676
            //
            bool isVerified_r676 = (response.Payload.FileCount.GetType() == typeof(Int64))&&
                (Marshal.SizeOf(response.Payload.FileCount) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerified_r676,
                676,
                 @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply]FileCount (8 bytes):  A 64-bit signed integer.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R673");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R673
            //
            Site.CaptureRequirementIfAreEqual<int>(
                8,
                Marshal.SizeOf(response.Payload.DirectoryCount),
                673,
                @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply]DirectoryCount (8 bytes): The number of directories on the specified disk.");

            // isDirectoryNumberKnown means the directory number is known.
            if (!isDirectoryNumberKnown)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R674");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R674
                //
                Site.CaptureRequirementIfAreEqual<long>(
                    -1,
                    response.Payload.DirectoryCount,
                    674,
                    @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply]DirectoryCount (8 bytes): This member is -1 if the number is unknown.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R679");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R679
            //
            Site.CaptureRequirementIfAreEqual<int>(
                8,
                Marshal.SizeOf(response.Payload.FileCount),
                679,
                @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply]FileCount (8 bytes): For UDF file systems with a virtual allocation table, this information is available only if the UDF revision is greater than 1.50.");

            // isMajorVersionKnow means Major Version number is known.
            if (!isMajorVersionKnow)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R681");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R681
                //
                Site.CaptureRequirementIfAreEqual<short>(
                    -1,
                    response.Payload.FsFormatMajVersion,
                    681,
                    @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply]FsFormatMajVersion (2 bytes):  Returns -1 if the number is unknown or not applicable.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R684,FsFormatName:{0}", BitConverter.ToString(response.Payload.FsFormatName));

            //
            // Verify MS-FSCC requirement: MS-FSCC_R684
            //
            bool isVerifyR684 = ((24 == response.Payload.FsFormatName.Length) &&
                                 ("UDF" == BitConverter.ToString(response.Payload.FsFormatName)));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR684,
                684,
                @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply]FsFormatName (24 bytes):  Always returns ""UDF"" in Unicode characters.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R686");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R686
            //
            bool isVerifyR686 = ((8 == Marshal.SizeOf(response.Payload.FormatTime)) &&
                                 (typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME) == response.Payload.FormatTime.GetType()));
            Site.CaptureRequirementIfIsTrue(
                isVerifyR686,
                686,
                @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply]FormatTime (8 bytes):  Expressed as a FILETIME structure, specified in section 2.1.1.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R688");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R688
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME),
                response.Payload.LastUpdateTime.GetType(),
                688,
                @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply]LastUpdateTime (8 bytes): Expressed as a FILETIME structure, specified in section 2.1.1.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R692");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R692
            //
            Site.CaptureRequirementIfAreEqual<int>(
                68,
                response.Payload.LastModifyingImplementationInfo.Length,
                692,
                @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply]LastModifyingImplementationInfo (68 bytes):  The last implementation that modified the disk.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R695,statusCode:{0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R695
            //
            bool isVerifyR695 = (((uint)FsctlQueryOnDiskVolumeInfoReplyStatus.STATUS_SUCCESS == statusCode) ||
                                 ((uint)FsctlQueryOnDiskVolumeInfoReplyStatus.STATUS_BUFFER_TOO_SMALL == statusCode) ||
                                 ((uint)FsctlQueryOnDiskVolumeInfoReplyStatus.STATUS_INVALID_PARAMETER == statusCode) ||
                                 ((uint)FsctlQueryOnDiskVolumeInfoReplyStatus.STATUS_INVALID_USER_BUFFER == statusCode));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR695,
                695,
                @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply]The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_INVALID_USER_BUFFER 0xC00000E8, STATUS_BUFFER_TOO_SMALL 0xC0000023, STATUS_INVALID_PARAMETER 0xC000000D].");

            // The R695 have verified the status Code returned. So we capture this requirement directly.
            Site.CaptureRequirement(
                694,
                @"This message[FSCTL_QUERY_ON_DISK_VOLUME_INFO Reply] returns a status code, as specified in [MS-ERREF] section 2.3.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FSCTL_QUERY_ALLOCATED_RANGES Request section 2.3.35
        /// <summary>
        /// Verify data type FSCTL_QUERY_ALLOCATED_RANGES Request 
        /// </summary>
        /// <param name="request">The FSCTL_QUERY_ALLOCATED_RANGES Request</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void VerifyDataTypeFsctlQueryAllocatedRangeRequest(FsccFsctlQueryAllocatedRangesRequestPacket request)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R633");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R633
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(long),
                request.Payload.FileOffset.GetType(),
                633,
                @"[In FILE_ALLOCATED_RANGE_BUFFER]FileOffset (8 bytes):  A 64-bit signed integer that contains the file offset, in bytes, of the start of a range of bytes in a file.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R634,FileOffset:{0}", request.Payload.FileOffset);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R634
            //
            bool isVerifyR634 = request.Payload.FileOffset >= 0;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR634,
                634,
                @"[In FILE_ALLOCATED_RANGE_BUFFER]FileOffset (8 bytes):  The value of this field MUST be greater than or equal to 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R635");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R635
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(long),
                request.Payload.Length.GetType(),
                635,
                @"[In FILE_ALLOCATED_RANGE_BUFFER]Length (8 bytes):  A 64-bit signed integer that contains the size, in bytes, of the range.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R636,Length:{0}", request.Payload.Length);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R636
            //
            bool isVerifyR636 = request.Payload.Length >= 0;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR636,
                636,
                @"[In FILE_ALLOCATED_RANGE_BUFFER]Length (8 bytes): The value of this field MUST be greater than or equal to 0.");

            // The R633 634 635 636 have verified the member of FILE_ALLOCATED_RANGE_BUFFER, so we capture this requirement directly.
            Site.CaptureRequirement(
                631,
                @"The message[FSCTL_QUERY_ALLOCATED_RANGES request] contains a FILE_ALLOCATED_RANGE_BUFFER data element.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FSCTL_QUERY_ALLOCATED_RANGES Reply section 2.3.36
        /// <summary>
        /// Verify data type FSCTL_QUERY_ALLOCATED_RANGES Reply 
        /// </summary>
        /// <param name="response">The FSCTL_QUERY_ALLOCATED_RANGES Reply </param>
        /// <param name="statusCode">The status code returned</param>
        /// <param name="isParameterInvalid">If the handle is not to a file, or the size of the input buffer is 
        /// less than the size of a FILE_ALLOCATED_RANGE_BUFFER structure, or the given FileOffset is less than 0, 
        /// or the given Length is less than 0, or the given FileOffset plus the given Length is larger 
        /// than 0x7FFFFFFFFFFFFFFF.</param>
        /// <param name="isUserBufferInvalid">If the input buffer or output buffer is not aligned to a 
        /// 4-byte boundary.</param>
        /// <param name="isBufferTooSmall">If the output buffer is too small to contain a FILE_ALLOCATED_RANGE_BUFFER 
        /// structure.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void VerifyDataTypeFsctlQueryAllocatedRangesReply(
            FsccFsctlQueryAllocatedRangesResponsePacket response,
            uint statusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R638");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R638
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(FSCTL_QUERY_ALLOCATED_RANGES_Reply),
                response.Payload.GetType(),
                638,
                @"This message[FSCTL_QUERY_ALLOCATED_RANGES Reply] MUST return an array of zero or more FILE_ALLOCATED_RANGE_BUFFER data elements.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R643");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R643
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(long),
                response.Payload.FileOffset.GetType(),
                643,
                @"[In FILE_ALLOCATED_RANGE_BUFFER]FileOffset (8 bytes):  A 64-bit signed integer that contains the file offset in bytes from the start of the file; the start of a range of bytes to which storage is allocated.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R646,FileOffset:{0}", response.Payload.FileOffset);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R646
            //
            bool isVerifyR646 = response.Payload.FileOffset >= 0;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR646,
                646,
                @"[In FILE_ALLOCATED_RANGE_BUFFER]FileOffset (8 bytes): This value MUST be greater than or equal to 0.<22>");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R647");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R647
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(long),
                response.Payload.Length.GetType(),
                647,
                @"[In FILE_ALLOCATED_RANGE_BUFFER]Length (8 bytes):  A 64-bit signed integer that contains the size, in bytes, of the range.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R648,Length:{0}", response.Payload.Length);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R648
            //
            bool isVerifyR648 = response.Payload.Length >= 0;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR648,
                648,
                @"[In FILE_ALLOCATED_RANGE_BUFFER]Length (8 bytes):  This value MUST be greater than or equal to 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R650,statusCode:{0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R650
            //
            bool isVerifyR650 = ((statusCode == (uint)FsctlQueryAllocatedRangesReplyStatus.STATUS_SUCCESS) ||
                                 (statusCode == (uint)FsctlQueryAllocatedRangesReplyStatus.STATUS_INVALID_PARAMETER) ||
                                 (statusCode == (uint)FsctlQueryAllocatedRangesReplyStatus.STATUS_INVALID_USER_BUFFER) ||
                                 (statusCode == (uint)FsctlQueryAllocatedRangesReplyStatus.STATUS_BUFFER_TOO_SMALL) ||
                                 (statusCode == (uint)FsctlQueryAllocatedRangesReplyStatus.STATUS_BUFFER_OVERFLOW));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR650,
                650,
                @"[In FSCTL_QUERY_ALLOCATED_RANGES Reply]The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_INVALID_PARAMETER 0xC000000D, STATUS_INVALID_USER_BUFFER 0xC00000E8, STATUS_BUFFER_TOO_SMALL 0xC0000023, STATUS_BUFFER_OVERFLOW 0x80000005].");

            // R650 have verified all the status code can be returned. We capture this RS directly.
            Site.CaptureRequirement(
                649,
                @"This message[FSCTL_QUERY_ALLOCATED_RANGES Reply] returns a status code, as specified in [MS-ERREF] section 2.3.");

            if (statusCode == (uint)FsctlQueryAllocatedRangesReplyStatus.STATUS_INVALID_PARAMETER)
            {
                // isParameterInvalid means the handle is not to a file, or the size of the input buffer is less than the size of a FILE_ALLOCATED_RANGE_BUFFER structure, or the given FileOffset is less than 0, or the given Length is less than 0, or the given FileOffset plus the given Length is larger than 0x7FFFFFFFFFFFFFFF.
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)FsctlQueryAllocatedRangesReplyStatus.STATUS_INVALID_PARAMETER,
                    statusCode,
                    651,
                    @"[In FSCTL_QUERY_ALLOCATED_RANGES Reply]Value of status code STATUS_INVALID_PARAMETER 0xC000000D means the handle is not to a file, or the size of the input buffer is less than the size of a FILE_ALLOCATED_RANGE_BUFFER structure, or the given FileOffset is less than 0, or the given Length is less than 0, or the given FileOffset plus the given Length is larger than 0x7FFFFFFFFFFFFFFF.");
            }
            else if (statusCode == (uint)FsctlQueryAllocatedRangesReplyStatus.STATUS_INVALID_USER_BUFFER)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R652");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R652
                //
                // isUserBufferInvalid means the input buffer or output buffer is not aligned to a 4-byte boundary.
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)FsctlQueryAllocatedRangesReplyStatus.STATUS_INVALID_USER_BUFFER,
                    statusCode,
                    652,
                    @"[In FSCTL_QUERY_ALLOCATED_RANGES Reply]Value of status code STATUS_INVALID_USER_BUFFER 0xC00000E8 means the input buffer or output buffer is not aligned to a 4-byte boundary.");
            }
            else if (statusCode == (uint)FsctlQueryAllocatedRangesReplyStatus.STATUS_BUFFER_TOO_SMALL)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R653");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R653
                //
                // isBufferTooSmall means the output buffer is too small to contain a FILE_ALLOCATED_RANGE_BUFFER structure.
 
                Site.CaptureRequirementIfAreEqual<uint>(
                    (uint)FsctlQueryAllocatedRangesReplyStatus.STATUS_BUFFER_TOO_SMALL,
                    statusCode,
                    653,
                    @"[In FSCTL_QUERY_ALLOCATED_RANGES Reply]Value of status code STATUS_BUFFER_TOO_SMALL 0xC0000023 means the output buffer is too small to contain a FILE_ALLOCATED_RANGE_BUFFER structure.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data typ FSCTL_SET_OBJECT_ID_EXTENDED_Request esection 2.3.55
        /// <summary>
        /// Veryfy data typ FSCTL_SET_OBJECT_ID_EXTENDED_Request
        /// </summary>
        /// <param name="request">The FSCTL_SET_OBJECT_ID_EXTENDED_Request instance</param>
        public void VerifyDataTypeFsctSetObjectIdExtendedRequest(FSCTL_SET_OBJECT_ID_EXTENDED_Request request)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R863");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R863
            //
            Site.CaptureRequirementIfAreEqual<int>(
                48,
                request.ExtendedInfo.Length * Marshal.SizeOf(request.ExtendedInfo[0]),
                863,
                @"[In EXTENDED_INFO]ExtendedInfo (48 bytes):  A 48-byte binary large object(BLOB) containing user-defined extended data that was passed to this FSCTL by an application.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify the datatype of NTFS_VOLUME_DATA_BUFFER_REPLY in section 2.3.14
        /// <summary>
        /// Verify the datatype of NTFS_VOLUME_DATA_BUFFER_REPLY in section 2.3.14
        /// </summary>
        /// <param name="reply">the NTFS_VOLUME_DATA_BUFFER reply</param>
        /// <param name="returnedStatusCode">the returned status code from reply</param>
        public void VerifyMessageSyntaxFsctlGetNtfsVolumeDataReply(NTFS_VOLUME_DATA_BUFFER_Reply reply,
                                                             FsctlGetNtfsVolumeDataReplyStatus returnedStatusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R422. VolumeSerialNumber: {0}", reply.VolumeSerialNumber);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R422
            //
            bool isVerifyR422 = (reply.VolumeSerialNumber.QuadPart.GetType() == typeof(Int64) &&
                                Marshal.SizeOf(reply.VolumeSerialNumber.QuadPart) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR422,
                422,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]VolumeSerialNumber (8 bytes):  A 64-bit signed integer that contains the serial number of the volume.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R431. BytesPerCluster: {0}", reply.BytesPerCluster);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R431
            //
            bool isVerifyR431 = (reply.BytesPerCluster.GetType() == typeof(UInt32) &&
                                 Marshal.SizeOf(reply.BytesPerCluster) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR431,
                431,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]BytesPerCluster (4 bytes):  A 32-bit unsigned integer that contains the number of bytes in a cluster on the specified volume.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R433. BytesPerFileRecordSegment: {0}", reply.BytesPerFileRecordSegment);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R433
            //
            bool isVerifyR433 = (reply.BytesPerFileRecordSegment.GetType() == typeof(UInt32) &&
                                 Marshal.SizeOf(reply.BytesPerFileRecordSegment) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR433,
                433,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]BytesPerFileRecordSegment (4 bytes):  A 32-bit unsigned integer that contains the number of bytes in a file record segment.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R434. ClustersPerFileRecordSegment: {0}", reply.ClustersPerFileRecordSegment);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R434
            //
            bool isVerifyR434 = (reply.ClustersPerFileRecordSegment.GetType() == typeof(UInt32) &&
                                 Marshal.SizeOf(reply.ClustersPerFileRecordSegment) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR434,
                434,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]ClustersPerFileRecordSegment (4 bytes):  A 32-bit unsigned integer that contains the number of clusters in a file record segment.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R426. FreeClusters: {0}", reply.FreeClusters);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R426
            //
            bool isVerifyR426 = (reply.FreeClusters.QuadPart.GetType() == typeof(Int64) &&
                                 Marshal.SizeOf(reply.FreeClusters.QuadPart) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR426,
                426,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]FreeClusters (8 bytes):  A 64-bit signed integer that contains the number of free clusters in the specified volume.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R436. MftStartLcn: {0}", reply.MftStartLcn);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R436
            //
            bool isVerifyR436 = (reply.MftStartLcn.QuadPart.GetType() == typeof(Int64) &&
                                 Marshal.SizeOf(reply.MftStartLcn.QuadPart) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR436,
                436,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]MftStartLcn (8 bytes):  A 64-bit signed integer that contains the starting logical cluster number of the master file table.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R437 QuadPart is :{0}",
                reply.Mft2StartLcn.QuadPart);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R437
            //
            bool isVerifyR437 = (reply.Mft2StartLcn.QuadPart.GetType() == typeof(Int64) &&
                                 Marshal.SizeOf(reply.Mft2StartLcn.QuadPart) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR437,
                437,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]Mft2StartLcn (8 bytes):  A 64-bit signed integer that contains the starting logical cluster number of the master file table mirror.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R435. MftValidDataLength: {0}", reply.MftValidDataLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R435
            //
            bool isVerifyR435 = (reply.MftValidDataLength.QuadPart.GetType() == typeof(Int64) &&
                                Marshal.SizeOf(reply.MftValidDataLength.QuadPart) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR435,
                435,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]MftValidDataLength (8 bytes):  A 64-bit signed integer that contains the size of the master file table in bytes.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R439. MftZoneEnd: {0}", reply.MftZoneEnd);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R439
            //
            bool isVerifyR439 = (reply.MftZoneEnd.QuadPart.GetType() == typeof(Int64) &&
                                 Marshal.SizeOf(reply.MftZoneEnd.QuadPart) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR439,
                439,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]MftZoneEnd (8 bytes):  A 64-bit signed integer that contains the ending logical cluster number of the master file table zone.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R438. MftZoneStart: {0}", reply.MftZoneStart);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R438
            //
            bool isVerifyR438 = (reply.MftZoneStart.QuadPart.GetType() == typeof(Int64) &&
                                 Marshal.SizeOf(reply.MftZoneStart.QuadPart) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR438,
                438,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]MftZoneStart (8 bytes):  A 64-bit signed integer that contains the starting logical cluster number of the master file table zone.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R424. NumberSectors: {0}", reply.NumberSectors);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R424
            //
            bool isVerifyR424 = (reply.NumberSectors.QuadPart.GetType() == typeof(Int64) &&
                                 Marshal.SizeOf(reply.NumberSectors.QuadPart) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR424,
                424,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]NumberSectors (8 bytes):  A 64-bit signed integer that contains the number of sectors in the specified volume.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R425. TotalClusters: {0}", reply.TotalClusters);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R425
            //
            bool isVerifyR425 = (reply.TotalClusters.QuadPart.GetType() == typeof(Int64) &&
                                 Marshal.SizeOf(reply.TotalClusters.QuadPart) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR425,
                425,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]TotalClusters (8 bytes):  A 64-bit signed integer that contains the total number of clusters in the specified volume.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R427. TotalReserved: {0}", reply.TotalReserved);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R427
            //
            bool isVerifyR427 = (reply.TotalReserved.QuadPart.GetType() == typeof(Int64) &&
                                Marshal.SizeOf(reply.TotalReserved.QuadPart) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR427,
                427,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]TotalReserved (8 bytes):  A 64-bit signed integer that contains the number of reserved clusters in the specified volume.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R440. Returned Status Code: {0}", returnedStatusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R440
            //
            // the returned status code must be in the valid range,as specified in [MS-ERREF] section 2.3
            bool isVerifyR440 = ((uint)returnedStatusCode >= 0x00000000 && (uint)returnedStatusCode <= 0xC03A0019);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR440,
                440,
                @"This message[NTFS_VOLUME_DATA_BUFFER_REPLY] also returns a status code, as specified in [MS-ERREF] section 2.3.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R441. The status code returned: {0}", returnedStatusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R441
            //
            bool isVerifyR441 = (returnedStatusCode == FsctlGetNtfsVolumeDataReplyStatus.STATUS_SUCCESS ||
                                 returnedStatusCode == FsctlGetNtfsVolumeDataReplyStatus.STATUS_INVALID_PARAMETER ||
                                 returnedStatusCode == FsctlGetNtfsVolumeDataReplyStatus.STATUS_VOLUME_DISMOUNTED);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR441,
                441,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_INVALID_PARAMETER 0xC000000D, STATUS_VOLUME_DISMOUNTED 0xC000026E].");


            // After the verification of all elements of NTFS_VOLUME_DATA_BUFFER_REPLY,
            // this requirement can be captured directly
            Site.CaptureRequirement(
                420,
                @"The FSCTL_GET_NTFS_VOLUME_DATA reply message returns the results of the FSCTL_GET_NTFS_VOLUME_DATA request as an NTFS_VOLUME_DATA_BUFFER_REPLY element.");

            //
            //Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R423 Actual VolumeSerialNumber: {0}",
                reply.VolumeSerialNumber.QuadPart);
            //
            // Verify MS-FSCC requirement 423
            //
            // As the it's unalbe for us to verify if the sn is "unique"
            // We only verify the size of the sn heree
            Site.CaptureRequirementIfAreEqual<int>(
                8,
                Marshal.SizeOf(reply.VolumeSerialNumber),
                423,
                 @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]VolumeSerialNumber (8 bytes): This is a unique number 
                 assigned to the volume media by the operating system when the volume is formatted.");

            //
            //Add the debug information
            //
            Site.Log.Add(LogEntryKind.Comment,
                "Verify MS-FSCC_R439 Actual MftZoneEnd: {0}",
                reply.MftZoneEnd.QuadPart);

            //
            //Add the debug information
            //
            Site.Log.Add(LogEntryKind.Comment,
                "Verify MS-FSCC_R430 in Actual BytesPerSector: {0}",
                 reply.BytesPerSector);
            //
            // Verify MS-FSCC requirement 430
            //
            bool isVerifyR430 =
                (reply.BytesPerSector.GetType() == typeof(UInt32) &&
                 Marshal.SizeOf(reply.BytesPerSector) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR430,
                430,
                @"[In NTFS_VOLUME_DATA_BUFFER_REPLY]BytesPerSector (4 bytes):  A 32-bit unsigned 
                integer that contains the number of bytes in a sector on the specified volume.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify data type FSCTL_GET_OBJECT_ID Reply section 2.3.16
        /// <summary>
        /// Verify data type FSCTL_GET_OBJECT_ID Reply  
        /// </summary>
        /// <param name="statusCode">The status code returned from the FSCTL_GET_OBJECT_ID Reply</param>
        /// <param name="isUseofObjectIDSupported">If the file system of the volume containing the specified file or 
        /// directory support the use of object IDs</param>
        public void VerifyDataTypeFsctlGetObjectIDReply(uint statusCode, bool isUseofObjectIDSupported)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            // isUseofObjectIDSupported means the file system of the volume containing the specified file or directory 
            // support the use of object IDs
            if (!isUseofObjectIDSupported)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R451");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R451
                //
                // According to TD, the status code means the return value. so here we check the status code.
                Site.CaptureRequirementIfAreNotEqual<uint>(
                    0x00000000,
                    statusCode,
                    451,
                    @"[In FSCTL_GET_OBJECT_ID Reply]If the file system of the volume containing the specified file or directory does not support the use of object IDs, the request will not succeed.");
            }
            if (statusCode == (uint)FsctlGetObjectIdReplyStatus.STATUS_INVALID_DEVICE_REQUEST)
            {
                // Since the ErrorCode specified in section 2.2 is  STATUS_INVALID_DEVICE_REQUEST, so if the returned error code is STATUS_INVALID_DEVICE_REQUEST, the RS was verified. Here capture it directly.
                Site.CaptureRequirement(
                    452,
                    @"[In FSCTL_GET_OBJECT_ID Reply]The error code returned in this situation[the file system of the volume does not support the use of object IDs] is specified in section 2.2.");
            }

            // As we'll verify if the return status code is valid, and the returned code always exsits,
            // we capture this rs directly
            Site.CaptureRequirement(
                453,
                @"This message[FSCTL_GET_OBJECT_ID Reply]  also returns a status code, 
                as specified in [MS-ERREF] section 2.3.");


            //
           //Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R454 return Status Code: {0}.",
                statusCode);

            // Verify if the return status code is valid
            // According to the TD of [MS-ERREF] decribes: STATUS_SUCCESS = 0x00000000
            bool isVerifyR454 = (
                statusCode == 0x00000000 || statusCode == 0xC000000D || statusCode == 0xC00002F0 ||
                statusCode == 0xC0000010);
            //
            // Verify MS-FSCC requirement 454
            //
            Site.CaptureRequirementIfIsTrue(
                isVerifyR454,
                454,
                @"[In FSCTL_GET_OBJECT_ID Reply] The status code returned directly by the function that processes 
                this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_INVALID_PARAMETER 0xC000000D, 
                STATUS_OBJECTID_NOT_FOUND 0xC00002F0, STATUS_INVALID_DEVICE_REQUEST 0xC0000010].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FSCTL_GET_OBJECT_ID Request section 2.3.15
        /// <summary>
        /// Verify data type FSCTL_GET_OBJECT_ID Request
        /// </summary>
        /// <param name="objectID">The object ID to be verified</param>
        public void VerifyDataTypeFsctlGetObjectIDRequest(byte[] objectID)  
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R445");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R445
            //
            Site.CaptureRequirementIfAreEqual<int>(
                16,
                objectID.Length,
                445,
                @"[In FSCTL_GET_OBJECT_ID Request]Object identifiers are 16-byte opaque values that are used to track files and directories.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FSCTL_PIPE_TRANSCEIVE Reply section 2.3.34
        /// <summary>
        /// Verify data type FSCTL_PIPE_TRANSCEIVE Reply 
        /// </summary>
        /// <param name="statusCode">The FSCTL_PIPE_TRANSCEIVE Reply </param>
        public void VerifyDataTypeFsctlPipeTransceiveReply(uint statusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R622,statusCode:{0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R622
            //
            bool isVerifyR622 = ((statusCode == (uint)FsctlPipeTransceiveReplyStatus.STATUS_SUCCESS) ||
                                 (statusCode == (uint)FsctlPipeTransceiveReplyStatus.STATUS_PIPE_DISCONNECTED) ||
                                 (statusCode == (uint)FsctlPipeTransceiveReplyStatus.STATUS_INVALID_PIPE_STATE) ||
                                 (statusCode == (uint)FsctlPipeTransceiveReplyStatus.STATUS_PIPE_BUSY) ||
                                 (statusCode == (uint)FsctlPipeTransceiveReplyStatus.STATUS_INVALID_USER_BUFFER) ||
                                 (statusCode == (uint)FsctlPipeTransceiveReplyStatus.STATUS_INSUFFICIENT_RESOURCES));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR622,
                622,
                @"[In FSCTL_PIPE_TRANSCEIVE Reply]The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_PIPE_DISCONNECTED 0xC00000B0, STATUS_INVALID_PIPE_STATE 0xC00000AD, STATUS_PIPE_BUSY 0xC00000AE, STATUS_INVALID_USER_BUFFER 0xC00000E8, STATUS_INSUFFICIENT_RESOURCES 0xC000009A].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region verify data type FILE_ID_GLOBAL_TX_DIR_INFORMATION  section 2.4.19
        /// <summary>
        /// verify data type FILE_ID_GLOBAL_TX_DIR_INFORMATION
        /// </summary>
        /// <param name="info">A FILE_ID_GLOBAL_TX_DIR_INFORMATION instance</param>
        /// <param name="isWriteLocked">Is the file is locked for modification by a transaction.</param>
        /// <param name="isVisibleToTx">is the file visible to transacted enumerators of the directory whose 
        /// transaction ID is in the LockingTransactionId field.</param>
        /// <param name="isVisibleOutsideTx">Is the file visible to transacted enumerators of the directory other 
        /// than the one whose transaction ID is in the LockingTransactionId field, and it is visible to 
        /// non-transacted enumerators of the directory.</param>
        public void VerifyDataTypeFileIdGlobalTxDirectoryInformation(
            FileIdGlobalTxDirectoryInformation info,
            bool isWriteLocked,
            bool isVisibleToTx,
            bool isVisibleOutsideTx)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1345");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1345
            //
            // If a field is ignored or not cann't be verified, here verify the RS partially, only verify the byte length of FileId field.
            Site.CaptureRequirementIfAreEqual<int>(
                8,
                info.FileId.Length,
                1345,
                @"[In <FILE_ID_GLOBAL_TX_DIR_INFORMATION>]FileId (8 bytes):  For file systems that do not support FileId, this field MUST be ignored.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1348");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1348
            //
            // If a field is ignored or not cann't be verified, so verify this RS partically, only verify the LockingTransactionId fields' bytes.
            Site.CaptureRequirementIfAreEqual<int>(
                16,
                Marshal.SizeOf(info.LockingTransactionId),
                1348,
                @"[In <FILE_ID_GLOBAL_TX_DIR_INFORMATION>]LockingTransactionId (16 bytes):  If the FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED flag is not set in the TxInfoFlags field, this field MUST be ignored.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1320");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1320
            //
            // Here only verify the RS partially, since if an implementation assume something or not couldn't be verified in Capture code. Only verify the byte length of NextEntryOffset field.
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(info.FileCommonDirectoryInformation.NextEntryOffset),
                1320,
                @"[In <FILE_ID_GLOBAL_TX_DIR_INFORMATION>]NextEntryOffset (4 bytes): An implementation MUST NOT assume that the value of NextEntryOffset is the same as the size of the current entry.");

            if ((info.TxInfoFlags & TxInfoFlags_Values.FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED) == 0x00000000)
            {

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1351");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1351
                //
                // The other flags means FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_VISIBLE_OUTSIDE_TX and FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_VISIBLE_TO_TX, here verify if the bit-And result is zero. if it is zero, it means that the other flags are not set.
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    (uint)((TxInfoFlags_Values.FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_VISIBLE_OUTSIDE_TX | TxInfoFlags_Values.FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_VISIBLE_TO_TX) & info.TxInfoFlags),
                    1351,
                    @"[In <FILE_ID_GLOBAL_TX_DIR_INFORMATION>]TxInfoFlags (4 bytes):  If the FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED flag is not set, the other flags MUST NOT be set.");
            }
            if ((info.TxInfoFlags & TxInfoFlags_Values.FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED) == TxInfoFlags_Values.FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1353, TxInfoFlags:{0}", info.TxInfoFlags);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1353
                //
                // isWriteLocked means the file is locked for modification by a transaction. 
                bool isVerifyR1353 = isWriteLocked;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1353,
                    1353,
                    @"[In <FILE_ID_GLOBAL_TX_DIR_INFORMATION>]TxInfoFlags (4 bytes): Value FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_WRITELOCKED 0x00000001 means the file is locked for modification by a transaction.");
            }
            if ((info.TxInfoFlags & TxInfoFlags_Values.FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_VISIBLE_TO_TX) == TxInfoFlags_Values.FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_VISIBLE_TO_TX)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1355, TxInfoFlags:{0}", info.TxInfoFlags);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1355
                //
                // isVisibleToTx means the file is visible to transacted enumerators of the directory whose transaction ID is in the LockingTransactionId field.
                bool isVerifyR1355 = isVisibleToTx;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1355,
                    1355,
                    @"[In <FILE_ID_GLOBAL_TX_DIR_INFORMATION>]TxInfoFlags (4 bytes): Value FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_VISIBLE_TO_TX 0x00000002 means the file is visible to transacted enumerators of the directory whose transaction ID is in the LockingTransactionId field.");
            }
            if ((info.TxInfoFlags & TxInfoFlags_Values.FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_VISIBLE_OUTSIDE_TX) == TxInfoFlags_Values.FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_VISIBLE_OUTSIDE_TX)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1356, TxInfoFlags:{0}", info.TxInfoFlags);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1356
                //
                // isVisibleOutsideTx means  the file is visible to transacted enumerators of the directory other than the one whose transaction ID is in the LockingTransactionId field, and it is visible to non-transacted enumerators of the directory.
                bool isVerifyR1356 = isVisibleOutsideTx;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1356,
                    1356,
                    @"[In <FILE_ID_GLOBAL_TX_DIR_INFORMATION>]TxInfoFlags (4 bytes): Value FILE_ID_GLOBAL_TX_DIR_INFO_FLAG_VISIBLE_OUTSIDE_TX 0x00000004 means the file is visible to transacted enumerators of the directory other than the one whose transaction ID is in the LockingTransactionId field, and it is visible to non-transacted enumerators of the directory.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify File Attribute section 2.6
        /// <summary>
        /// Verify the File Attribute Flags
        /// </summary>
        /// <param name="fileAttribute">The File Attribute to be verified</param>
        public void VerifyFileAttribute(uint fileAttribute)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1820");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1820
            //
            Site.CaptureRequirementIfAreNotEqual<uint>(
                0x00000000,
                fileAttribute,
                1820,
                @"[In File Attributes]There is no file attribute with the value 0x00000000 because a value of 0x00000000 in the FileAttributes field means that the file attributes for this file MUST NOT be changed when setting basic information for the file.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1819");
            // Couldn't find out any enum to represent the fileAttribute flags. so here have to use the numbers.
            bool isVerifyR1819 = (((fileAttribute & 0x00000020) == 0x00000020) ||
                                  ((fileAttribute & 0x00000800) == 0x00000800) ||
                                  ((fileAttribute & 0x00000010) == 0x00000010) ||
                                  ((fileAttribute & 0x00004000) == 0x00004000) ||
                                  ((fileAttribute & 0x00000002) == 0x00000002) ||
                                  ((fileAttribute & 0x00000080) == 0x00000080) ||
                                  ((fileAttribute & 0x00002000) == 0x00002000) ||
                                  ((fileAttribute & 0x00001000) == 0x00001000) ||
                                  ((fileAttribute & 0x00000400) == 0x00000400) ||
                                  ((fileAttribute & 0x00000200) == 0x00000200) ||
                                  ((fileAttribute & 0x00000004) == 0x00000004) ||
                                  ((fileAttribute & 0x00000100) == 0x00000100));

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1819
            //
            Site.CaptureRequirementIfIsTrue(
                isVerifyR1819,
                1819,
                @"[In File Attributes]The following attributes[FILE_ATTRIBUTE_ARCHIVE 0x00000020, FILE_ATTRIBUTE_COMPRESSED 0x00000800, FILE_ATTRIBUTE_DIRECTORY 0x00000010, FILE_ATTRIBUTE_ENCRYPTED 0x00004000, FILE_ATTRIBUTE_HIDDEN 0x00000002, FILE_ATTRIBUTE_NORMAL 0x00000080, FILE_ATTRIBUTE_NOT_CONTENT_INDEXED 0x00002000, FILE_ATTRIBUTE_OFFLINE 0x00001000, FILE_ATTRIBUTE_READONLY 0x00000001, FILE_ATTRIBUTE_REPARSE_POINT 0x00000400, FILE_ATTRIBUTE_SPARSE_FILE 0x00000200, FILE_ATTRIBUTE_SYSTEM 0x00000004, FILE_ATTRIBUTE_TEMPORARY 0x00000100] are defined for files and directories. They can be used in any combination unless noted in the description of the attribute's meaning.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify FSCTL_CREATE_OR_OBJECT_ID Relpy section 2.3.2
        /// <summary>
        /// Verify The FSCTL_CREATE_OR_OBJECT_ID Relpy 
        /// </summary>
        /// <param name="returnValue">The FSCTL_CREATE_OR_OBJECT_ID Relpy </param>
        public void VerifyDataTypeFsctlCreateOrObjectIDReply(
            FsctlCreateOrGetObjectIdReplyStatus returnValue)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R191,returnValue:{0}", returnValue);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R191
            //
            bool isVerifyR191 = ((returnValue == FsctlCreateOrGetObjectIdReplyStatus.STATUS_SUCCESS) ||
                                 (returnValue == FsctlCreateOrGetObjectIdReplyStatus.STATUS_DUPLICATE_NAME) ||
                                 (returnValue == FsctlCreateOrGetObjectIdReplyStatus.STATUS_INVALID_PARAMETER));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR191,
                191,
                @"[In FSCTL_CREATE_OR_GET_OBJECT_ID Reply] The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following[STATUS_DUPLICATE_NAME 0xC00000BD, STATUS_INVALID_PARAMETER 0xC000000D].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify FSCTL_DELETE_OBJECT_ID Reply section 2.3.4
        /// <summary>
        /// Verify The reply of FSCTL_DELETE_OBJECT_ID
        /// </summary>
        /// <param name="response">The response of FSCTL_DELETE_OBJECT_ID</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void VerifyFsctlDeleteObjectIDReply(FsccFsctlDeleteObjectIdResponsePacket response)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R200: status code:{0}", response.Payload);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R200
            //
            // Since the FsctlDeleteObjectIdReplyStatus don't have STATUS_INVALID_PARAMETER statue, so here using the actural number to represent it.
            bool isVerifyR200 = ((response.Payload == FsctlDeleteObjectIdReplyStatus.STATUS_SUCCESS) ||
                                 (response.Payload == FsctlDeleteObjectIdReplyStatus.STATUS_ACCESS_DENIED) ||
                                 (response.Payload == FsctlDeleteObjectIdReplyStatus.STATUS_MEDIA_WRITE_PROTECTED) ||
                                 (response.Payload == FsctlDeleteObjectIdReplyStatus.STATUS_OBJECT_NAME_NOT_FOUND) ||
                                 ((uint)response.Payload == 0xC000000D));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR200,
                200,
                @"[In FSCTL_DELETE_OBJECT_ID Reply]The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_INVALID_PARAMETER 0xC000000D, STATUS_ACCESS_DENIED 0xC0000022, STATUS_OBJECT_NAME_NOT_FOUND 0xC0000034, STATUS_MEDIA_WRITE_PROTECTED 0xC00000A2]");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify FSCTL_GET_REPARSE_POINT Reply section 2.3.18
        /// <summary>
        /// Verify FSCTL_GET_REPARSE_POINT Reply 
        /// </summary>
        /// <param name="statusCode">The status code return from server</param>
        public void VerifyDataTypeFsctlGetReparsePointReply(uint statusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R470,statusCode:{0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R470
            //
            bool isVerifyR470 = ((statusCode == (uint)FsctlGetReparsePointReplyStatus.STATUS_SUCCESS) ||
                                 (statusCode == (uint)FsctlGetReparsePointReplyStatus.STATUS_BUFFER_OVERFLOW) ||
                                 (statusCode == (uint)FsctlGetReparsePointReplyStatus.STATUS_BUFFER_TOO_SMALL) ||
                                 (statusCode == (uint)FsctlGetReparsePointReplyStatus.STATUS_INVALID_DEVICE_REQUEST) ||
                                 (statusCode == (uint)FsctlGetReparsePointReplyStatus.STATUS_INVALID_PARAMETER) ||
                                 (statusCode == (uint)FsctlGetReparsePointReplyStatus.STATUS_NOT_A_REPARSE_POINT));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR470,
                470,
                @"[In FSCTL_GET_REPARSE_POINT Reply]The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_BUFFER_TOO_SMALL 0xC0000023, STATUS_INVALID_PARAMETER 0xC000000D, STATUS_BUFFER_OVERFLOW 0x80000005, STATUS_NOT_A_REPARSE_POINT 0xC0000275, STATUS_INVALID_DEVICE_REQUEST 0xC0000010].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify related RS in 2.3.35
        /// <summary>
        /// Verify message syntax In FSCTL_QUERY_ALLOCATED_RANGES
        /// </summary>
        /// <param name="fsctlQueryAllocatedRangesReply">The FSCTL_QUERY_ALLOCATED_RANGES_Reply type structure.</param>
        public void VerifyMessageSyntaxFsctlQueryAllocatedRanges(
          FSCTL_QUERY_ALLOCATED_RANGES_Reply fsctlQueryAllocatedRangesReply)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R633,the value of FileOffset is {0}", fsctlQueryAllocatedRangesReply.FileOffset);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R633
            //
            // 64 bits equal 8 bytes
            bool isVerifyR633 =
                (Marshal.SizeOf(fsctlQueryAllocatedRangesReply.FileOffset) == 8) &&
                (fsctlQueryAllocatedRangesReply.FileOffset.GetType() == typeof(long));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR633,
                633,
                @"[In FILE_ALLOCATED_RANGE_BUFFER]FileOffset (8 bytes):  A 64-bit signed integer that contains the file offset, in bytes, of the start of a range of bytes in a file.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R634,the value of FileOffset is {0}", fsctlQueryAllocatedRangesReply.FileOffset);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R634
            //
            bool isVerifyR634 =
                (fsctlQueryAllocatedRangesReply.FileOffset >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR634,
                634,
                @"[In FILE_ALLOCATED_RANGE_BUFFER]FileOffset (8 bytes):  The value of this field MUST be greater than or equal to 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R635,the value of the length is {0}", fsctlQueryAllocatedRangesReply.Length);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R635
            //
            bool isVerifyR635 =
                (Marshal.SizeOf(fsctlQueryAllocatedRangesReply.Length) == 8) &&
                (fsctlQueryAllocatedRangesReply.Length.GetType() == typeof(long));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR635,
                635,
                @"[In FILE_ALLOCATED_RANGE_BUFFER]Length (8 bytes):  A 64-bit signed integer that contains the size, in bytes, of the range.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R636,the value of Length is {0}", fsctlQueryAllocatedRangesReply.Length);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R636
            //
            bool isVerifyR636 = (fsctlQueryAllocatedRangesReply.Length >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR636,
                636,
                @"[In FILE_ALLOCATED_RANGE_BUFFER]Length (8 bytes): The value of this field MUST be greater than or equal to 0.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion Verify related RS in 2.3.35

        #region Verify related RS in 2.5.1 section
        /// <summary>
        /// Verify the message syntax In FILE_FS_ATTRIBUTE_INFORMATION
        /// </summary>
        /// <param name="fileFsAttributeInformation">A FileFsAttributeInformation type structure.</param>
        /// <param name="maximumFileNameComponentLength">The maximum file name component length, in bytes, supported by the specified file system.</param>
        public void VerifyMessageSyntaxFileFsAttributeInformation(
            FileFsAttributeInformation fileFsAttributeInformation,
            int maximumFileNameComponentLength)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1684");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1684
            //
            // If FILE_FS_ATTRIBUTE_INFORMATION is not null in this message, then mean the message contains it, so can capture this requirement just by verifying this field is not null
            Site.CaptureRequirementIfIsNotNull(
                fileFsAttributeInformation,
                1684,
                @"The message[FileFsAttributeInformation] contains a FILE_FS_ATTRIBUTE_INFORMATION data element.");
           
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1703,the value of FileSystemAttributes is {0}", fileFsAttributeInformation.FileSystemAttributes);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1703
            //
            // If the two flags don't equal 1 at the same time, then means they are incompatible
            FileSystemAttributes_Values flagsOr = (FileSystemAttributes_Values.FILE_FILE_COMPRESSION | FileSystemAttributes_Values.FILE_VOLUME_IS_COMPRESSED);

            bool isVerifyR1703 =
                (fileFsAttributeInformation.FileSystemAttributes & flagsOr) != flagsOr;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1703,
                1703,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]FileSystemAttributes (4 bytes):  This flag[FILE_FILE_COMPRESSION 0x00000010] is incompatible with the FILE_VOLUME_IS_COMPRESSED flag.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1697,the value of FileSystemAttributes is {0}", fileFsAttributeInformation.FileSystemAttributes);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1697
            //
            // The same logic with R1703
            Site.CaptureRequirementIfIsTrue(
                isVerifyR1703,
                1697,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]FileSystemAttributes (4 bytes): This flag[FILE_VOLUME_IS_COMPRESSED 0x00008000] is incompatible with the FILE_FILE_COMPRESSION flag.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1711,the value of FileSystemNameLength is {0}", fileFsAttributeInformation.FileSystemNameLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1711
            //
            bool isVerifyR1711 =
                (Marshal.SizeOf(fileFsAttributeInformation.FileSystemNameLength) == 4) &&
                (fileFsAttributeInformation.FileSystemNameLength == fileFsAttributeInformation.FileSystemName.Length);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1711,
                1711,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]FileSystemNameLength (4 bytes):  A 32-bit unsigned integer that contains the length, in bytes, of the file system name in the FileSystemName field.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1712,the value of FileSystemNameLength is {0}", fileFsAttributeInformation.FileSystemNameLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1712
            //

            Site.CaptureRequirementIfIsTrue(
                fileFsAttributeInformation.FileSystemNameLength > 0,
                1712,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]FileSystemNameLength (4 bytes):  The value of this field MUST be greater than 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1709,the value of MaximumComponentNameLength is {0}", fileFsAttributeInformation.MaximumComponentNameLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1709
            //
            bool isVerifyR1709 =
                (Marshal.SizeOf(fileFsAttributeInformation.MaximumComponentNameLength) == 4) &&
                (fileFsAttributeInformation.MaximumComponentNameLength == maximumFileNameComponentLength);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1709,
                1709,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]MaximumComponentNameLength (4 bytes):  A 32-bit signed integer that contains the maximum file name component length, in bytes, supported by the specified file system.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1710,the value of MaximumComponentNameLength is {0}", fileFsAttributeInformation.MaximumComponentNameLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1710
            //

            Site.CaptureRequirementIfIsTrue(
                fileFsAttributeInformation.MaximumComponentNameLength > 0,
                1710,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]MaximumComponentNameLength (4 bytes): The value of this field MUST be greater than 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1716");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1716
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                fileFsAttributeInformation.FileSystemNameLength,
                (uint)fileFsAttributeInformation.FileSystemName.Length,
                1716,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]FileSystemName (variable): This field MUST be handled as a sequence of FileSystemNameLength bytes.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.5.1 section

        #region Verify related RS in 2.5.2 section
        /// <summary>
        /// Verify the message syntax in  FILE_FS_CONTROL_INFORMATION
        /// </summary>
        /// <param name="fileFsControlInformation">A FileFsControlInformation type structure.</param>
        /// <param name="defaultPerUserDiskQuatoLimit">The default per-user disk quota limit in bytes for the volume.</param>
        /// <param name="deaultPerUserDiskQuotaWarningThreshold">the default per-user disk quota warning threshold 
        /// in bytes for the volume</param>
        /// <param name="minAmountOfFreeDiskSpaceForStartFiltering">minimum amount of free disk space in bytes that is 
        /// required for the operating system's content indexing service to begin document filtering.</param>
        /// <param name="minAmountOfFreeDiskSpaceForStopFiltering">the minimum amount of free disk space in bytes that 
        /// is required for the content indexing service to continue filtering</param>
        /// <param name="minAmountOfFreeDiskSpaceForThreshold">the minimum amount of free disk space in bytes that is 
        /// required for the indexing service to continue to filter documents and merge word lists.</param>
        public void VerifyMessageSyntaxFileFsControlInformation(
            FileFsControlInformation fileFsControlInformation,
            long deaultPerUserDiskQuotaWarningThreshold,
            long minAmountOfFreeDiskSpaceForStartFiltering,
            long minAmountOfFreeDiskSpaceForStopFiltering,
            long minAmountOfFreeDiskSpaceForThreshold)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1720");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1720
            //
            // If FILE_FS_CONTROL_INFORMATION is not null in the message then means the message contains it, 
            // so can capture this requirement just by verifying this field is not null
            Site.CaptureRequirementIfIsNotNull(
                fileFsControlInformation,
                1720,
                @"The message[FileFsControlInformation] contains a FILE_FS_CONTROL_INFORMATION data element.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1735,the value of DefaultQuotaLimit is {0}", 
                fileFsControlInformation.DefaultQuotaLimit);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1735
            //
            bool isVerifyR1735 = (Marshal.SizeOf(fileFsControlInformation.DefaultQuotaLimit) == 8) &&
                ((fileFsControlInformation.DefaultQuotaLimit >= 0) ||
                (fileFsControlInformation.DefaultQuotaLimit == 0xFFFFFFFFFFFFFFFF));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1735,
                1735,
                @"[In FILE_FS_CONTROL_INFORMATION]DefaultQuotaLimit (8 bytes):  This field MUST be set to a 64-bit integer value greater than or equal to 0 to set a default disk quota limit per user for this volume, or to (-1) to specify that no default quota limit per user is set.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1732,the value of DefaultQuotaThreshold is {0}", 
                fileFsControlInformation.DefaultQuotaThreshold);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1732
            //
            bool isVerifyR1732 =
                (Marshal.SizeOf(fileFsControlInformation.DefaultQuotaThreshold) == 8) &&
                (fileFsControlInformation.DefaultQuotaThreshold == (ulong)deaultPerUserDiskQuotaWarningThreshold);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1732,
                1732,
                @"[In FILE_FS_CONTROL_INFORMATION]DefaultQuotaThreshold (8 bytes):  A 64-bit signed integer that contains the default per-user disk quota warning threshold in bytes for the volume.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1733,the value of DefaultQuotaThreshold is {0}", 
                fileFsControlInformation.DefaultQuotaThreshold);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1733
            //
            bool isVerifyR1733 = (Marshal.SizeOf(fileFsControlInformation.DefaultQuotaThreshold) == 8) &&
                (fileFsControlInformation.DefaultQuotaThreshold >= 0 ||
                fileFsControlInformation.DefaultQuotaThreshold == 0xFFFFFFFFFFFFFFFF);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1733,
                1733,
                @"[In FILE_FS_CONTROL_INFORMATION]DefaultQuotaThreshold (8 bytes): This field MUST be set to a 64-bit integer value greater than or equal to 0 to set a default quota warning threshold per user for this volume, or to (-1) to specify that no default quota warning threshold per user is set.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1723,the value of FreeSpaceStartFiltering is {0}", 
                fileFsControlInformation.FreeSpaceStartFiltering);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1723
            //
            bool isVerifyR1723 =
                (Marshal.SizeOf(fileFsControlInformation.FreeSpaceStartFiltering) == 8) &&
                (fileFsControlInformation.FreeSpaceStartFiltering == minAmountOfFreeDiskSpaceForStartFiltering);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1723,
                1723,
                @"[In FILE_FS_CONTROL_INFORMATION]FreeSpaceStartFiltering (8 bytes):  A 64-bit signed integer that contains the minimum amount of free disk space in bytes that is required for the operating system's content indexing service to begin document filtering.");

            //
            // Verify requirement MS-FSCC_R1724 and MS-FSCC_R1975
            //
            string isR1724Implementated = Site.Properties.Get("SHOULDMAY.IsR1724Implementated");
            bool isR1975Satisfied = (fileFsControlInformation.FreeSpaceStartFiltering == 0);

            if (isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1975,the value of FreeSpaceStartFiltering is {0}", fileFsControlInformation.FreeSpaceStartFiltering);
                //
                // Verify MS-FSCC requirement: MS-FSCC_R1975
                //
                Site.CaptureRequirementIfIsTrue(
                    isR1975Satisfied,
                    1975,
                    @"<80> Section 2.5.2: Windows sets this value[FreeSpaceStartFiltering] to 0.");

                if (null == isR1724Implementated)
                {
                    Site.Properties.Add("SHOULDMAY.IsR1724Implementated", Boolean.TrueString);
                    isR1724Implementated = Boolean.TrueString;
                }
            }

            if (null != isR1724Implementated)
            {
                bool implement = Boolean.Parse(isR1724Implementated);
                bool isSatisfied = isR1975Satisfied;

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1724,the value of FreeSpaceStartFiltering is {0}", fileFsControlInformation.FreeSpaceStartFiltering);
                //
                // Verify MS-FSCC requirement: MS-FSCC_R1724
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implement,
                    isSatisfied,
                    1724,
                    String.Format(@"[In FILE_FS_CONTROL_INFORMATION]FreeSpaceStartFiltering (8 bytes):  This value SHOULD be set to 0.<80> This requirement is {0} implement", implement ? "" : "not"));
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1729,the value of FreeSpaceStopFiltering is {0}", fileFsControlInformation.FreeSpaceStopFiltering);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1729
            //
            bool isVerifyR1729 =
                (Marshal.SizeOf(fileFsControlInformation.FreeSpaceStopFiltering) == 8) &&
                (fileFsControlInformation.FreeSpaceStopFiltering == minAmountOfFreeDiskSpaceForStopFiltering);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1729,
                1729,
                @"[In FILE_FS_CONTROL_INFORMATION]FreeSpaceStopFiltering (8 bytes):  A 64-bit signed integer that contains the minimum amount of free disk space in bytes that is required for the content indexing service to continue filtering.");

            //
            // Verify requirement MS-FSCC_R1730 and MS-FSCC_R1977
            //
            string isR1730Implementated = Site.Properties.Get("SHOULDMAY.IsR1730Implementated");
            bool isR1977Satisfied = (fileFsControlInformation.FreeSpaceStopFiltering == 0);

            if (isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1977,the value of FreeSpaceStopFiltering is{0}", fileFsControlInformation.FreeSpaceStopFiltering);
                //
                // Verify MS-FSCC requirement: MS-FSCC_R1977
                //
                Site.CaptureRequirementIfIsTrue(
                    isR1977Satisfied,
                    1977,
                    @"<82> Section 2.5.2: Windows sets this value[FreeSpaceStopFiltering] to 0.");

                if (null == isR1730Implementated)
                {
                    Site.Properties.Add("SHOULDMAY.IsR1730Implementated", Boolean.TrueString);
                    isR1730Implementated = Boolean.TrueString;
                }
            }

            if (null != isR1730Implementated)
            {
                bool implement = Boolean.Parse(isR1730Implementated);
                bool isSatisfied = isR1977Satisfied;

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1730,the value of FreeSpaceStopFiltering is{0}", fileFsControlInformation.FreeSpaceStopFiltering);
                //
                // Verify MS-FSCC requirement: MS-FSCC_R1730
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implement,
                    isSatisfied,
                    1730,
                    String.Format(@"[In FILE_FS_CONTROL_INFORMATION]FreeSpaceStopFiltering (8 bytes): This value SHOULD be set to 0.<82> This requirement is {0} implement", implement ? "" : "not"));
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1726,the value of FreeSpaceThreshold is {0}", fileFsControlInformation.FreeSpaceThreshold);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1726
            //
            bool isVerifyR1726 =
                (Marshal.SizeOf(fileFsControlInformation.FreeSpaceThreshold) == 8) &&
                (fileFsControlInformation.FreeSpaceThreshold == minAmountOfFreeDiskSpaceForThreshold);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1726,
                1726,
                @"[In FILE_FS_CONTROL_INFORMATION]FreeSpaceThreshold (8 bytes):  A 64-bit signed integer that contains the minimum amount of free disk space in bytes that is required for the indexing service to continue to filter documents and merge word lists.");

            //
            // Verify requirement MS-FSCC_R1727 and MS-FSCC_R1976
            //
            string isR1727Implementated = Site.Properties.Get("SHOULDMAY.IsR1727Implementated");
            bool isR1976Satisfied = (fileFsControlInformation.FreeSpaceThreshold == 0);

            if (isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1976,the value of FreeSpaceThreshold is {0}", fileFsControlInformation.FreeSpaceThreshold);
                //
                // Verify MS-FSCC requirement: MS-FSCC_R1976
                //
                Site.CaptureRequirementIfIsTrue(
                    isR1976Satisfied,
                    1976,
                    @"<81> Section 2.5.2: Windows sets this value[FreeSpaceThreshold] to 0.");

                if (null == isR1727Implementated)
                {
                    Site.Properties.Add("SHOULDMAY.IsR1727Implementated", Boolean.TrueString);
                    isR1727Implementated = Boolean.TrueString;
                }
            }

            if (null != isR1727Implementated)
            {
                bool implement = Boolean.Parse(isR1727Implementated);
                bool isSatisfied = isR1976Satisfied;

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1727,the value of FreeSpaceThreshold is {0}", fileFsControlInformation.FreeSpaceThreshold);
                //
                // Verify MS-FSCC requirement: MS-FSCC_R1727
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implement,
                    isSatisfied,
                    1727,
                    String.Format(@"[In FILE_FS_CONTROL_INFORMATION]FreeSpaceThreshold (8 bytes):  This value SHOULD be set to 0.<81> This requirement is {0} implement", implement ? "" : "not"));
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.5.2 section

        #region Verify related RS in 2.4.15 section
        /// <summary>
        /// Verify the message syntax in FILE_FULL_EA_INFORMATION
        /// </summary>
        /// <param name="fileFullEaInformation">A FileFullEaInformation type structure.</param>
        /// <param name="sizeOfCurrEntry">The size of the current entry.</param>
        /// <param name="extendedAttributeName"> the extended attribute name followed by a single null-termination character byte.</param>
        /// <param name="extendedAttributeValue">The extended attrubute value.</param>
        /// <param name="sizeOfByteOffset">the byte offset from the beginning of this entry, at which the next FILE_ FULL_EA _INFORMATION entry is located, if multiple entries are present in the buffer.</param>
        /// <param name="areMultipleEntriesPresent">A bool variable that means multiple entries are present in the buffer.</param>
        /// <param name="isNextEntryLocated">A bool variable that means the next FILE_ FULL_EA _INFORMATION entry is located</param>
        public void VerifyMessageSyntaxFileFullEaInformation(
            FileFullEaInformation fileFullEaInformation,
            uint sizeOfCurrEntry,
            string extendedAttributeName,
            string extendedAttributeValue,
            uint sizeOfByteOffset,
            bool areMultipleEntriesPresent,
            bool isNextEntryLocated)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1206,the value of EaName is {0}", fileFullEaInformation.EaName);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1206
            //
            // Not verify ASCII characters
            bool isVerifyR1206 = (fileFullEaInformation.EaName.ToString().Contains(extendedAttributeName));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1206,
                1206,
                @"[In FILE_FULL_EA_INFORMATION]EaName (variable):  An array of 8-bit ASCII characters that contains the extended attribute name followed by a single null-termination character byte.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1207,the value of EaValue is {0}", fileFullEaInformation.EaValue);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1207
            //
            bool isVerifyR1207 = (fileFullEaInformation.EaValue.ToString().Contains(extendedAttributeValue));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1207,
                1207,
                @"[In FILE_FULL_EA_INFORMATION]EaValue (variable):  An array of bytes that contains the extended attribute value.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1203,the value of EaNameLength is {0}", fileFullEaInformation.EaNameLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1203
            //
            bool isVerifyR1203 =
                (Marshal.SizeOf(fileFullEaInformation.EaNameLength) == 1) &&
                (fileFullEaInformation.EaNameLength == fileFullEaInformation.EaName.Length);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1203,
                1203,
                @"[In FILE_FULL_EA_INFORMATION]EaNameLength (1 byte):  An 8-bit unsigned integer that contains the length, in bytes, of the extended attribute name in the EaName field.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1205,the value of EaValueLength is {0}", fileFullEaInformation.EaValueLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1205
            //
            bool isVerifyR1205 =
                (Marshal.SizeOf(fileFullEaInformation.EaValueLength) == 2) &&
                (fileFullEaInformation.EaValueLength == fileFullEaInformation.EaValue.Length);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1205,
                1205,
                @"[In FILE_FULL_EA_INFORMATION]EaValueLength (2 bytes):  A 16-bit unsigned integer that contains the length, in bytes, of the extended attribute value in the EaValue field.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1200,the value of Flags is {0}", fileFullEaInformation.Flags);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1200
            //
            // From definition of Flags_Values, V1 equals 0.
            bool isVerifyR1200 =
                (Marshal.SizeOf(fileFullEaInformation.Flags) == 1) &&
                (fileFullEaInformation.Flags == FILE_FULL_EA_INFORMATION_FLAGS.FILE_NEED_EA ||
                fileFullEaInformation.Flags == FILE_FULL_EA_INFORMATION_FLAGS.NONE);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1200,
                1200,
                @"[In FILE_FULL_EA_INFORMATION]Flags (1 byte):  An 8-bit unsigned integer that contains one of the following flag values:[0x00000000, FILE_NEED_EA 0x00000080].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1199");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1199
            //
            Site.CaptureRequirementIfAreNotSame(
                sizeOfCurrEntry,
                fileFullEaInformation.NextEntryOffset,
                1199,
                @"[In FILE_FULL_EA_INFORMATION]NextEntryOffset (4 bytes):  An implementation MUST NOT assume that the value of NextEntryOffset field is the same as the size of the current entry.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1196,the value of NextEntryOffset is {0}", fileFullEaInformation.NextEntryOffset);

            // Check if multiple entries are present in the buffer
            if (areMultipleEntriesPresent)
            {
                //
                // Verify MS-FSCC requirement: MS-FSCC_R1196
                //
                bool isVerifyR1196 =
                    (Marshal.SizeOf(fileFullEaInformation.NextEntryOffset) == 4) &&
                    (fileFullEaInformation.NextEntryOffset == sizeOfByteOffset) &&
                    (isNextEntryLocated);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1196,
                    1196,
                    @"[In FILE_FULL_EA_INFORMATION]NextEntryOffset (4 bytes):  A 32-bit unsigned 4-byte aligned integer that contains the byte offset from the beginning of this entry, at which the next FILE_ FULL_EA _INFORMATION entry is located, if multiple entries are present in the buffer.");
            }
            // if just one entry is present in the buffer
            else if (!areMultipleEntriesPresent)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1196,the value of NextEntryOffset is {0}", fileFullEaInformation.NextEntryOffset);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1196
                //
                bool isVerifyR1196 =
                    (Marshal.SizeOf(fileFullEaInformation.NextEntryOffset) == 4) &&
                    (fileFullEaInformation.NextEntryOffset == sizeOfByteOffset);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1196,
                    1196,
                    @"[In FILE_FULL_EA_INFORMATION]NextEntryOffset (4 bytes):  A 32-bit unsigned 4-byte aligned integer that contains the byte offset from the beginning of this entry, at which the next FILE_ FULL_EA _INFORMATION entry is located, if multiple entries are present in the buffer.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.4.15 section

        #region Verify related RS in 2.4.15.1 section
        /// <summary>
        /// Verify the message syntax in FILE_GET_EA_INFORMATION
        /// </summary>
        /// <param name="fileGetEaInformation">A FILE_GET_EA_INFORMATION type structure.</param>
        /// <param name="sizeOfCurrEntry">The size of the current entry.</param>
        /// <param name="isNoOtherEntriesFollow">A bool variable that means no other entries follow this one[FILE_GET_EA_INFORMATION entry].</param>
        /// <param name="extendedAttrubuteName">The extended attribute name followed by a single null-termination character byte.</param>
        /// <param name="sizeOfByteOffset"> the byte offset from the beginning of this entry, at which the next FILE_GET_EA_INFORMATION entry is located, if multiple entries are present in a buffer.</param>
        /// <param name="areMultipleEntriesPresent">A bool variable that means multiple entries are present in the buffer.</param>
        /// <param name="isNextEntryLocated">A bool variable that means the next FILE_GET_EA_INFORMATION entry is located</param>
        public void VerifyMessageSyntaxFILE_GET_EA_INFORMATION(
            FILE_GET_EA_INFORMATION fileGetEaInformation,
            uint sizeOfCurrEntry,
            bool isNoOtherEntriesFollow,
            string extendedAttrubuteName,
            uint sizeOfByteOffset,
            bool areMultipleEntriesPresent,
            bool isNextEntryLocated)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1216,the value of EaName is {0}", fileGetEaInformation.EaName);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1216
            //
            // Not verify ASCII charaters
            bool isVerifyR1216 = (fileGetEaInformation.EaName.ToString().Contains(extendedAttrubuteName));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1216,
                1216,
                @"[In FILE_GET_EA_INFORMATION]EaName (variable):  An array of 8-bit ASCII characters that contains the extended attribute name followed by a single null-termination character byte.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1213");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1213
            //
            Site.CaptureRequirementIfAreNotSame(
                sizeOfCurrEntry,
                fileGetEaInformation.NextEntryOffset,
                1213,
                @"[In FILE_GET_EA_INFORMATION]NextEntryOffset (4 bytes): An implementation MUST NOT assume that the value of NextEntryOffset is the same as the size of the current entry.");

            if (isNoOtherEntriesFollow)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1211");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1211
                //
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    fileGetEaInformation.NextEntryOffset,
                    1211,
                    @"[In FILE_GET_EA_INFORMATION]NextEntryOffset (4 bytes):  This member MUST be zero if no other entries follow this one[FILE_GET_EA_INFORMATION entry].");
            }

            // Check if multiple entries are present in a buffer
            if (areMultipleEntriesPresent)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1210,the value of NextEntryOffset is {0}", fileGetEaInformation.NextEntryOffset);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1210
                //
                bool isVerifyR1210 =
                    (Marshal.SizeOf(fileGetEaInformation.NextEntryOffset) == 4) &&
                    (fileGetEaInformation.NextEntryOffset == sizeOfByteOffset) &&
                    (isNextEntryLocated);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1210,
                    1210,
                    @"[In FILE_GET_EA_INFORMATION]NextEntryOffset (4 bytes):  A 32-bit unsigned integer that contains the byte offset from the beginning of this entry, at which the next FILE_GET_EA_INFORMATION entry is located, if multiple entries are present in a buffer.");
            }
            else if (!areMultipleEntriesPresent)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1210,the value of NextEntryOffset is {0}", fileGetEaInformation.NextEntryOffset);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1210
                //
                bool isVerifyR1210 =
                    (Marshal.SizeOf(fileGetEaInformation.NextEntryOffset) == 4) &&
                    (fileGetEaInformation.NextEntryOffset == sizeOfByteOffset);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1210,
                    1210,
                    @"[In FILE_GET_EA_INFORMATION]NextEntryOffset (4 bytes):  A 32-bit unsigned integer that contains the byte offset from the beginning of this entry, at which the next FILE_GET_EA_INFORMATION entry is located, if multiple entries are present in a buffer.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.4.15.1 section

        #region Verify related RS in 2.4.21 section
        /// <summary>
        /// Verify the message syntax in FILE_LINK_INFORMATION
        /// </summary>
        /// <param name="fileLinkInformation">A FileLinkInformation type structure.</param>
        /// <param name="isLinkCreationOperationFail">A bool variable that means the link creation operation fail.</param>
        /// <param name="isLinkExists">A variable to indicate the link already exists.</param>
        /// <param name="isNetworkOperations">A variable to indicate that is a network operations</param>
        /// <param name="isLinkCreatedIndiffDirectory">A variable that means the link is to be created in a different 
        /// directory from the file that is being linked to.</param>
        /// <param name="isSpecifyFullPathName">A bool variable that means this member(FileName) specifies the full 
        /// path name for the link to be created.</param>
        /// <param name="isSpecifyOnlyFileName">A bool variable that means this member(FileName) specifies only the 
        /// file name for the link to be created.</param>
        public void VerifyMessageSyntaxFileLinkInformation(
            FileLinkInformation fileLinkInformation,
            bool isLinkCreationOperationFail,
            bool isLinkExists,
            bool isNetworkOperations,
            bool isLinkCreatedIndiffDirectory,
            bool isSpecifyFullPathName,
            bool isSpecifyOnlyFileName)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if (fileLinkInformation.RootDirectory.Equals(null) && isLinkCreatedIndiffDirectory)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1380,the value of FileName is {0}", fileLinkInformation.FileName);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1380
                //

                Site.CaptureRequirementIfIsTrue(
                    isSpecifyFullPathName,
                    1380,
                    @"[In FILE_LINK_INFORMATION]FileName (variable):  If the RootDirectory member is NULL, and the link is to be created in a different directory from the file that is being linked to, this member specifies the full path name for the link to be created.");
            }
            else
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1381,the value of FileName is {0}", fileLinkInformation.FileName);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1381
                //

                Site.CaptureRequirementIfIsTrue(
                    isSpecifyOnlyFileName,
                    1381,
                    @"[In FILE_LINK_INFORMATION]FileName (variable): Otherwise[If opposite to the condition:  the RootDirectory member is NULL, and the link is to be created in a different directory from the file that is being linked to], it specifies only the file name.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1377,the value of FileNameLength is {0}", fileLinkInformation.FileNameLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1377
            //
            bool isVerifyR1377 =
                (Marshal.SizeOf(fileLinkInformation.FileNameLength) == 4) &&
                (fileLinkInformation.FileNameLength == fileLinkInformation.FileName.Length);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1377,
                1377,
                @"[In FILE_LINK_INFORMATION]FileNameLength (4 bytes):  A 32-bit unsigned integer that contains the length, in bytes, of the FileName field.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1370,the value of ReplaceIfExists is {0}", fileLinkInformation.ReplaceIfExists);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1370
            //
            // 8 bits equal 1 byte.
            bool isVerifyR1370 =
                (Marshal.SizeOf(fileLinkInformation.ReplaceIfExists) == 1) &&
                (fileLinkInformation.ReplaceIfExists == 1);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1370,
                1370,
                @"[In FILE_LINK_INFORMATION]ReplaceIfExists (1 byte):  An 8-bit field that is set to 1 to indicate that if the link already exists, it should be replaced with the new link.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1375");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1375
            //
            // 64 bits equal 8 bytes.
            // "contains ..." is informative, not verify
            Site.CaptureRequirementIfAreEqual<int>(
                8,
                Marshal.SizeOf((ulong)fileLinkInformation.RootDirectory),
                1375,
                @"[In FILE_LINK_INFORMATION]RootDirectory (8 bytes):  A 64-bit unsigned integer that contains the file handle for the directory where the link is to be created.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1378,the length of FileName is {0}", fileLinkInformation.FileName.Length);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1378
            //
            // Every unicode character occupy two bit
            // So the length must be a even number.
            bool isVerifyR1378 = (fileLinkInformation.FileName.Length % 2 == 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1378,
                1378,
                @"[In FILE_LINK_INFORMATION]FileName (variable):  A sequence of Unicode characters containing the name to be assigned to the newly created link.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1415");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1415
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                fileLinkInformation.FileNameLength,
                (uint)fileLinkInformation.FileName.Length,
                1415,
                @"[In FILE_LINK_INFORMATION]FileName (variable):  This field MUST be handled as a sequence of FileNameLength bytes.");

            // Check if the caller wants the link creation operation to fail 
            // and if the link already exists.
            if (isLinkCreationOperationFail && isLinkExists)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1371");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1371
                //
                Site.CaptureRequirementIfAreEqual<Byte>(
                    0,
                    fileLinkInformation.ReplaceIfExists,
                    1371,
                    @"[In FILE_LINK_INFORMATION]ReplaceIfExists (1 byte): MUST be set to 0 if the caller wants the link creation operation to fail if the link already exists.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1373");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1373
            //
            // When the field is not null then means it can contain any value, so can capture this requirement just by verifying this field is not null
            Site.CaptureRequirementIfIsNotNull(
                fileLinkInformation.Reserved,
                1373,
                @"[In FILE_LINK_INFORMATION]Reserved (7 bytes):  This field can contain any value.");

            // Check if is for network operations
            if (isNetworkOperations)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1376");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1376
                //
                // From the definition of FileLinkInformation_RootDirectory_Values,
                // V1 equals 0.
                Site.CaptureRequirementIfAreEqual<FileLinkInformation_RootDirectory_Values>(
                    FileLinkInformation_RootDirectory_Values.V1,
                    fileLinkInformation.RootDirectory,
                    1376,
                    @"[In FILE_LINK_INFORMATION]RootDirectory (8 bytes): For network operations, this value MUST be zero.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.4.21 section

        #region Verify related RS in 2.4.22 section
        /// <summary>
        /// Verify the message syntax in FILE_MAILSLOT_QUERY_INFORMATION
        /// </summary>
        /// <param name="fileMailslotQueryInformation">A FileMailslotQueryInformation type structure.</param>
        /// <param name="sizeOfMailslotQuota">The quota in bytes for the mailslot.</param>
        /// <param name="maxSizeOfSingleMessage">The maximum size of a single message that can be written to the mailslot in bytes.</param>
        /// <param name="totalNumOFMsgToBeRead">The total number of messages waiting to be read from the mailslot.</param>
        /// <param name="sizeOfNextMsg">The next message size in bytes.</param>
        /// <param name="readWaitTime">The time a read operation can wait for a message to be written to the mailslot before a time-out occurs in milliseconds.</param>
        public void VerifyMessageSyntaxFileMailslotQueryInformation(
            FileMailslotQueryInformation fileMailslotQueryInformation,
            uint sizeOfMailslotQuota,
            uint maxSizeOfSingleMessage,
            uint totalNumOFMsgToBeRead,
            uint sizeOfNextMsg,
            long readWaitTime)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1386,the value of MailslotQuota is {0}", fileMailslotQueryInformation.MailslotQuota);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1386
            //
            bool isVerifyR1386 =
                (Marshal.SizeOf(fileMailslotQueryInformation.MailslotQuota) == 4) &&
                (fileMailslotQueryInformation.MailslotQuota == sizeOfMailslotQuota);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1386,
                1386,
                @"[In FILE_MAILSLOT_QUERY_INFORMATION]MailslotQuota (4 bytes):  A 32-bit unsigned integer that contains the quota in bytes for the mailslot.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1384,the value of MaximumMessageSize is {0}", fileMailslotQueryInformation.MaximumMessageSize);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1384
            //
            bool isVerifyR1384 =
                (Marshal.SizeOf(fileMailslotQueryInformation.MaximumMessageSize) == 4) &&
                (fileMailslotQueryInformation.MaximumMessageSize == maxSizeOfSingleMessage);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1384,
                1384,
                @"[In FILE_MAILSLOT_QUERY_INFORMATION]MaximumMessageSize (4 bytes):  A 32-bit unsigned integer that contains the maximum size of a single message that can be written to the mailslot in bytes.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1385");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1385
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                0,
                fileMailslotQueryInformation.MaximumMessageSize,
                1385,
                @"[In FILE_MAILSLOT_QUERY_INFORMATION]MaximumMessageSize (4 bytes): To specify that the message can be of any size, set this value to zero.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1389,the value of MessagesAvailable is {0}", fileMailslotQueryInformation.MessagesAvailable);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1389
            //
            bool isVerifyR1389 =
                (Marshal.SizeOf(fileMailslotQueryInformation.MessagesAvailable) == 4) &&
                (fileMailslotQueryInformation.MessagesAvailable == totalNumOFMsgToBeRead);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1389,
                1389,
                @"[In FILE_MAILSLOT_QUERY_INFORMATION]MessagesAvailable (4 bytes):  A 32-bit unsigned integer that contains the total number of messages waiting to be read from the mailslot.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1388,the value of NextMessageSize is {0}", fileMailslotQueryInformation.NextMessageSize);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1388
            //
            bool isVerifyR1388 =
                (Marshal.SizeOf(fileMailslotQueryInformation.NextMessageSize) == 4) &&
                (fileMailslotQueryInformation.NextMessageSize == sizeOfNextMsg);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1388,
                1388,
                @"[In FILE_MAILSLOT_QUERY_INFORMATION]NextMessageSize (4 bytes):  A 32-bit unsigned integer that contains the next message size in bytes.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1390,the value of ReadTimeout is {0}", fileMailslotQueryInformation.ReadTimeout);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1390
            //
            bool isVerifyR1390 =
                (Marshal.SizeOf(fileMailslotQueryInformation.ReadTimeout) == 8) &&
                (fileMailslotQueryInformation.ReadTimeout == readWaitTime);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1390,
                1390,
                @"[In FILE_MAILSLOT_QUERY_INFORMATION]ReadTimeout (8 bytes):  A 64-bit signed integer that contains the time a read operation can wait for a message to be written to the mailslot before a time-out occurs in milliseconds.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1391,the value of ReadTimeout is {0}", fileMailslotQueryInformation.ReadTimeout);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1391
            //
            bool isVerifyR1391 =
                (fileMailslotQueryInformation.ReadTimeout == -1) ||
                (fileMailslotQueryInformation.ReadTimeout >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1391,
                1391,
                @"[In FILE_MAILSLOT_QUERY_INFORMATION]ReadTimeout (8 bytes):  The value of this field MUST be (-1) or greater than or equal to 0.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.4.22 section

        #region Verify related RS in 2.4.24 section
        /// <summary>
        /// Verify the message syntax in FILE_MODE_INFORMATION
        /// </summary>
        /// <param name="fileModeInformation">A FILE_MODE_INFORMATION type structure.</param>
        /// <param name="isDeleteFile">A bool variable that means delete the file when the last handle to the file is closed.</param>
        protected void VerifyMessageSyntaxFileModeinformation(
            FILE_MODE_INFORMATION fileModeInformation,
            bool isDeleteFile)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            if ((fileModeInformation.Mode & Mode_Values.FILE_DELETE_ON_CLOSE) == Mode_Values.FILE_DELETE_ON_CLOSE)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1410,the value of Mode is {0}", fileModeInformation.Mode);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1410
                //

                Site.CaptureRequirementIfIsTrue(
                    isDeleteFile,
                    1410,
                    @"[In FILE_MODE_INFORMATION]Mode (4 bytes): When [value FILE_DELETE_ON_CLOSE 0x00001000 is] set, delete the file when the last handle to the file is closed.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.4.24 section

        #region Verify related RS in 2.4.37 section
        /// <summary>
        /// Verify the message syntax in FileNameInformation
        /// </summary>
        /// <param name="fileNameInformation">A FileNameInformation type structure.</param>
        public void VerifyMessageSyntaxFileNameInformation(
            FileNameInformation fileNameInformation)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1618,the length of FileName is {0}", fileNameInformation.FileName.Length);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1618
            //
            // Eack unicode character occupy 2 bit, so the length must be an even number
            bool isVerifyR1618 = true;

            for (int index = 0; index < fileNameInformation.FileName.Length - 1; index++)
            {
                if (index % 2 != 0)
                {
                    if (fileNameInformation.FileName[index] != 0)
                    {
                        isVerifyR1618 = false;
                        break;
                    }
                }
            }
            Site.CaptureRequirementIfIsTrue(
                isVerifyR1618,
                1618,
                @"[In FILE_NAME_INFORMATION]FileName (variable):  A sequence of Unicode characters containing the file name.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1674");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1674
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                fileNameInformation.FileNameLength,
                (uint)fileNameInformation.FileName.Length,
                1674,
                @"[In FILE_NAME_INFORMATION]FileName (variable):  This field MUST be handled as a sequence of FileNameLength bytes.");
           
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1617,the value of FileNameLength is {0}", fileNameInformation.FileNameLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1617
            //
            // 32 bits equal 4 bytes
            bool isVerifyR1617 =
                (Marshal.SizeOf(fileNameInformation.FileNameLength) == 4) &&
                (fileNameInformation.FileNameLength ==
                (uint)fileNameInformation.FileName.Length);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1617,
                1617,
                @"[In FILE_NAME_INFORMATION]FileNameLength (4 bytes):  A 32-bit unsigned integer that contains the length, in bytes, of the FileName field.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.4.37 section

        #region Verify related RS in 2.4.26
        /// <summary>
        /// Verify the message syntax in FileNamesInformation
        /// </summary>
        /// <param name="fileNamesInformation">A FileNamesInformation type structure.</param>
        /// <param name="isNTFS">A variable to indicate that is a NTFS.</param>
        /// <param name="sizeOfCurrentEntry">The size of the current entry.</param>
        /// <param name="noOtherEntriesFollow">A variable to indicate that no other entries follow this one[FILE_ NAMES _INFORMATION entry]</param>
        /// <param name="sizeOfByteOffset">The byte offset of the file within the parent directory</param>
        /// <param name="nextEntryOffset">The byte offset from the beginning of this entry, at which the next FILE_ NAMES _INFORMATION entry is located, if multiple entries are present in a buffer.</param>
        /// <param name="areMultipleEntriesPresent">A bool variable that means multiple entries are present in the buffer</param>
        /// <param name="isNextEntryLocated">A bool variable that means the next next FILE_ NAMES _INFORMATION entry is located.</param>
        public void VerifyMessageSyntaxFileNamesInformation(
            FileNamesInformation fileNamesInformation,
            uint sizeOfByteOffset,
            bool isNTFS,
            uint sizeOfCurrentEntry,
            bool noOtherEntriesFollow,
            uint nextEntryOffset,
            bool areMultipleEntriesPresent,
            bool isNextEntryLocated)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1423,the value of FileIndex is {0}", fileNamesInformation.FileIndex);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1423
            //
            // 32 bits equal 4 bytes
            bool isVerifyR1423 =
                (Marshal.SizeOf(fileNamesInformation.FileIndex) == 4) &&
                (fileNamesInformation.FileIndex == sizeOfByteOffset);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1423,
                1423,
                @"[In FILE_NAMES_INFORMATION]FileIndex (4 bytes):  A 32-bit unsigned integer that contains the byte offset of the file within the parent directory.");

            //
            // Verify requirement MS-FSCC_R1424 and MS-FSCC_R1956
            //
            string isR1424Implementated = Site.Properties.Get("isR1424Implementated");
            bool isR1956Satisfied = isNTFS && (fileNamesInformation.FileIndex == 0);

            if (isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1956,the value of FileIndex is {0}", fileNamesInformation.FileIndex);
                //
                // Verify MS-FSCC requirement: MS-FSCC_R1956
                //
                Site.CaptureRequirementIfIsTrue(
                    isR1956Satisfied,
                    1956,
                    @"<67> Section 2.4.26: Windows?000, Windows XP, Windows Server 2003, Windows Vista, and Windows 
                    Server 2008 set this value[FileIndex] to 0 for files on NTFS file systems.");

                if (null == isR1424Implementated)
                {
                    Site.Properties.Add("isR1424Implementated", Boolean.TrueString);
                    isR1424Implementated = Boolean.TrueString;
                }
            }

            if (null != isR1424Implementated)
            {
                bool implement = Boolean.Parse(isR1424Implementated);
                bool isSatisfied = isR1956Satisfied;

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1424,the value of FileIndex is {0}", fileNamesInformation.FileIndex);
                //
                // Verify MS-FSCC requirement: MS-FSCC_R1424
                //
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    implement,
                    isSatisfied,
                    1424,
                    String.Format(@"[In FILE_NAMES_INFORMATION]FileIndex (4 bytes): For file systems in which the position of a file within the parent directory is not fixed and can be changed at any time to maintain sort order, this field SHOULD be set to 0. This requirement is {0} implement", implement ? "" : "not"));
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1427,the length of FileName is {0}", fileNamesInformation.FileName.Length);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1427
            //
            // Each unicode character occupy 2 bits, so the length must be an even number.
            bool isVerifyR1427 = (fileNamesInformation.FileName.Length % 2 == 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1427,
                1427,
                @"[In FILE_NAMES_INFORMATION]FileName (variable):  A sequence of Unicode 
characters containing the file name.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1620");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1620
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                fileNamesInformation.FileNameLength,
                (uint)fileNamesInformation.FileName.Length,
                1620,
                @"[In FILE_NAMES_INFORMATION]FileName (variable): This field MUST be handled as a sequence of FileNameLength bytes.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1426,the value of FileNameLength is {0}", fileNamesInformation.FileNameLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1426
            //
            bool isVerifyR1426 = (Marshal.SizeOf(fileNamesInformation.FileNameLength) == 4) && (fileNamesInformation.FileNameLength == (uint)fileNamesInformation.FileName.Length);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1426,
                1426,
                @"[In FILE_NAMES_INFORMATION]FileNameLength (4 bytes):  A 32-bit unsigned integer that contains the length, in bytes, of the FileName field.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1419,the value of NextEntryOffset is {0}", fileNamesInformation.NextEntryOffset);

            // Check if multiple entries are present in a buffer.
            if (areMultipleEntriesPresent)
            {
                //
                // Verify MS-FSCC requirement: MS-FSCC_R1419
                //
                bool isVerifyR1419 =
                    (Marshal.SizeOf(fileNamesInformation.NextEntryOffset) == 4) &&
                    (fileNamesInformation.NextEntryOffset == nextEntryOffset) &&
                    (isNextEntryLocated);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1419,
                    1419,
                    @"[In FILE_NAMES_INFORMATION]NextEntryOffset (4 bytes):  A 32-bit unsigned integer that contains the byte offset from the beginning of this entry, at which the next FILE_ NAMES _INFORMATION entry is located, if multiple entries are present in a buffer.");
            }
            // if just one entry is present in a buffer
            else if (!areMultipleEntriesPresent)
            {
                //
                // Verify MS-FSCC requirement: MS-FSCC_R1419
                //
                bool isVerifyR1419 =
                    (Marshal.SizeOf(fileNamesInformation.NextEntryOffset) == 4) &&
                    (fileNamesInformation.NextEntryOffset == nextEntryOffset);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1419,
                    1419,
                    @"[In FILE_NAMES_INFORMATION]NextEntryOffset (4 bytes):  A 32-bit unsigned integer that contains the byte offset from the beginning of this entry, at which the next FILE_ NAMES _INFORMATION entry is located, if multiple entries are present in a buffer.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1422");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1422
            //
            Site.CaptureRequirementIfAreNotSame(
                sizeOfCurrentEntry,
                fileNamesInformation.NextEntryOffset,
                1422,
                @"[In FILE_NAMES_INFORMATION]NextEntryOffset (4 bytes): An implementation MUST NOT assume that the value of NextEntryOffset is the same as the size of the current entry.");

            if (noOtherEntriesFollow)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1420");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1420
                //
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    fileNamesInformation.NextEntryOffset,
                    1420,
                    @"[In FILE_NAMES_INFORMATION]NextEntryOffset (4 bytes): This member MUST be zero if no other entries follow this one[FILE_ NAMES _INFORMATION entry].");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.4.26

        #region Verify related RS in 2.3.44 section
        /// <summary>
        /// Verify the message syntax in FSCTL_READ_FILE_USN_DATA_Reply
        /// </summary>
        /// <param name="fsctlReadFileUsnDataReply">The status code returned directly by the function that processes 
        /// this FSCTL.</param>
        /// <param name="statusCode">A trailing Unicode NULL character.</param>
        /// <param name="numberOfFileOrDirectory">the number (assigned by the file system when the file is created) of 
        /// the file or directory for which this record notes changes</param>
        /// <param name="totalLengthOfUSN"> the total length of the update sequence number (USN) record in bytes</param>
        /// <param name="indexOfUniqueSecurityId">Index of a unique security identifier assigned to the file or directory 
        /// associated with this record.</param>
        /// <param name="fsctlCommand">FSCTL command which server doesnot understand</param>
        /// <param name="trailingUnicodeNullChar">The usn of the record.</param>
        /// <param name="isUsnChangeJournalRecordLogged">A variable to indicate that USN change journal records have been 
        /// logged for the directory of the file associated with this record.</param>
        /// <param name="usnOfRecord">The USN of the record</param>
        /// <param name="fileAttributes">attributes for the file or directory associated with this record.</param>
        /// <param name="diretoryOrdinalNum">The ordinal number of the directory on which the file or directory that is 
        /// associated with this record is located.</param>
        public void VerifyMessageSyntaxFsctlReadFileUsnDataReplyForOld(
            FSCTL_READ_FILE_USN_DATA_Reply fsctlReadFileUsnDataReply,
            bool isUsnChangeJournalRecordLogged)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            // Status list of FsctlReadFileUsnDataReply
            List<Enum> statusCodeList = new List<Enum>();
            statusCodeList.Add(FsctlReadFileUsnDataReplyStatus.STATUS_BUFFER_TOO_SMALL);
            statusCodeList.Add(FsctlReadFileUsnDataReplyStatus.STATUS_INVALID_DEVICE_REQUEST);
            statusCodeList.Add(FsctlReadFileUsnDataReplyStatus.STATUS_INVALID_PARAMETER);
            statusCodeList.Add(FsctlReadFileUsnDataReplyStatus.STATUS_INVALID_USER_BUFFER);
            statusCodeList.Add(FsctlReadFileUsnDataReplyStatus.STATUS_SUCCESS);

         
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R766,the value of FileAttributes is {0}", fsctlReadFileUsnDataReply.FileAttributes);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R766
            //
            bool isVerifyR766 =
                (Marshal.SizeOf(fsctlReadFileUsnDataReply.FileAttributes) == 4) &&
                (fsctlReadFileUsnDataReply.FileAttributes.GetType() == typeof(uint));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR766,
                766,
                @"[In USN_RECORD]FileAttributes (4 bytes):  A 32-bit unsigned integer that contains attributes for the 
                file or directory associated with this record.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R773,the length of the FileName is {0}", fsctlReadFileUsnDataReply.FileName.Length);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R773
            //
            // Each unicode character occupy 2 bits, so the length must be an even number
            bool isVerifyR773 = (fsctlReadFileUsnDataReply.FileName.Length % 2 == 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR773,
                773,
                @"[In USN_RECORD]FileName (variable):  A variable-length field of UNICODE characters containing the name 
                of the file or directory associated with this record in Unicode format."); 

           
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R769,the value of FileNameLength is {0}", fsctlReadFileUsnDataReply.FileNameLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R769
            //
            bool isVerifyR769 =
                (Marshal.SizeOf(fsctlReadFileUsnDataReply.FileNameLength) == 2) &&
                (fsctlReadFileUsnDataReply.FileNameLength == (ushort)fsctlReadFileUsnDataReply.FileName.Length);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR769,
                769,
                @"[In USN_RECORD]FileNameLength (2 bytes):  A 16-bit unsigned integer that contains the length of the 
                file or directory name associated with this record in bytes.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R771,the value of FileNameLength is {0}", fsctlReadFileUsnDataReply.FileNameLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R771
            //
            // If R769 was verified ,then means FileNameLength determine file name length
            Site.CaptureRequirementIfIsTrue(
                isVerifyR769,
                771,
                @"[In USN_RECORD]FileNameLength (2 bytes):Use this member to determine file name length rather than depending on a trailing NULL to delimit the file name in FileName.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R772,the value of FileNameOffset is {0}", fsctlReadFileUsnDataReply.FileNameOffset);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R772
            //
            // 16 bits equal 2 bytes
            // From the beginning of the structure to the FileName member 
            // there are 60 bytes,so the FileNameOffset should be 60
            bool isVerifyR772 =
                (Marshal.SizeOf(fsctlReadFileUsnDataReply.FileNameOffset) == 2) &&
                (fsctlReadFileUsnDataReply.FileNameOffset == 60);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR772,
                772,
                @"[In USN_RECORD]FileNameOffset (2 bytes):  A 16-bit unsigned integer that contains the offset in bytes 
                of the FileName member from the beginning of the structure.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R720,the value of FileReferenceNumber is {0}", fsctlReadFileUsnDataReply.FileReferenceNumber);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R720
            //
            // 64 bits equal 8 bytes
            bool isVerifyR720 =
                (Marshal.SizeOf(fsctlReadFileUsnDataReply.FileReferenceNumber) == 8) &&
                (fsctlReadFileUsnDataReply.FileReferenceNumber.GetType() == typeof(ulong));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR720,
                720,
                @"[In USN_RECORD]FileReferenceNumber (8 bytes):  A 64-bit unsigned integer, opaque to the client, 
                containing the number (assigned by the file system when the file is created) of the file or directory 
                for which this record notes changes. T");

            if (isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R718,the value of MajorVersion is {0}", fsctlReadFileUsnDataReply.MajorVersion);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R718
                //
                // 16 bits equal 2 bytes
                // From TD <25>,The major version number is 2 for file systems 
                // created on Windows 2000, Windows XP, Windows Server 2003, 
                // Windows Vista, and Windows Server 2008.
                bool isVerifyR718 =
                    (Marshal.SizeOf(fsctlReadFileUsnDataReply.MajorVersion) == 2) &&
                    (fsctlReadFileUsnDataReply.MajorVersion == 2);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR718,
                    718,
                    @"[In USN_RECORD]MajorVersion (2 bytes):  A 16-bit unsigned integer that contains the major version of 
                    the change journal software for this record.<25>");

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R719,the vlaue of MinorVersion is {0}", fsctlReadFileUsnDataReply.MinorVersion);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R719
                //
                // 16 bits equal 2 bytes
                // From TD <26>,The minor version number is 0 for file systems 
                // created on Windows 2000, Windows XP, Windows Server 2003, 
                // Windows Vista, and Windows Server 2008.
                bool isVerifyR719 =
                    (Marshal.SizeOf(fsctlReadFileUsnDataReply.MinorVersion) == 2) &&
                    (fsctlReadFileUsnDataReply.MinorVersion == 0);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR719,
                    719,
                    @"[In USN_RECORD]MinorVersion (2 bytes):  A 16-bit unsigned integer that contains the minor version of 
                    the change journal software for this record.<26>");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R723,the value of ParentFileReferenceNumber is {0}", fsctlReadFileUsnDataReply.ParentFileReferenceNumber);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R723
            //
            bool isVerifyR723 =
                (Marshal.SizeOf(fsctlReadFileUsnDataReply.ParentFileReferenceNumber) == 8) &&
                (fsctlReadFileUsnDataReply.ParentFileReferenceNumber.GetType() == typeof(ulong));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR723,
                723,
                @"[In USN_RECORD]ParentFileReferenceNumber (8 bytes):  A 64-bit unsigned integer, opaque to the client, 
                containing the ordinal number of the directory on which the file or directory that is associated with 
                this record is located.");

            List<Enum> reasonCodeList = new List<Enum>();
            reasonCodeList.Add(Reason_Values.USN_REASON_BASIC_INFO_CHANGE);
            reasonCodeList.Add(Reason_Values.USN_REASON_CLOSE);
            reasonCodeList.Add(Reason_Values.USN_REASON_COMPRESSION_CHANGE);
            reasonCodeList.Add(Reason_Values.USN_REASON_DATA_EXTEND);
            reasonCodeList.Add(Reason_Values.USN_REASON_DATA_OVERWRITE);
            reasonCodeList.Add(Reason_Values.USN_REASON_DATA_TRUNCATION);
            reasonCodeList.Add(Reason_Values.USN_REASON_EA_CHANGE);
            reasonCodeList.Add(Reason_Values.USN_REASON_ENCRYPTION_CHANGE);
            reasonCodeList.Add(Reason_Values.USN_REASON_FILE_CREATE);
            reasonCodeList.Add(Reason_Values.USN_REASON_FILE_DELETE);
            reasonCodeList.Add(Reason_Values.USN_REASON_HARD_LINK_CHANGE);
            reasonCodeList.Add(Reason_Values.USN_REASON_INDEXABLE_CHANGE);
            reasonCodeList.Add(Reason_Values.USN_REASON_NAMED_DATA_EXTEND);
            reasonCodeList.Add(Reason_Values.USN_REASON_NAMED_DATA_OVERWRITE);
            reasonCodeList.Add(Reason_Values.USN_REASON_NAMED_DATA_TRUNCATION);
            reasonCodeList.Add(Reason_Values.USN_REASON_OBJECT_ID_CHANGE);
            reasonCodeList.Add(Reason_Values.USN_REASON_RENAME_NEW_NAME);
            reasonCodeList.Add(Reason_Values.USN_REASON_RENAME_OLD_NAME);
            reasonCodeList.Add(Reason_Values.USN_REASON_REPARSE_POINT_CHANGE);
            reasonCodeList.Add(Reason_Values.USN_REASON_SECURITY_CHANGE);
            reasonCodeList.Add(Reason_Values.USN_REASON_STREAM_CHANGE);
            
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R717,the value of RecordLength is {0}", fsctlReadFileUsnDataReply.RecordLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R717
            //
            bool isVerifyR717 =
                (Marshal.SizeOf(fsctlReadFileUsnDataReply.RecordLength) == 4) &&
                (fsctlReadFileUsnDataReply.RecordLength.GetType() == typeof(uint));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR717,
                717,
                @"[In USN_RECORD]RecordLength (4 bytes):  A 32-bit unsigned integer that contains the total length of 
                the update sequence number (USN) record in bytes.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R763,the value of SecurityId is {0}", fsctlReadFileUsnDataReply.SecurityId);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R763
            //
            // 32 bits equal 4 bytes.
            bool isVerifyR763 =
                (Marshal.SizeOf(fsctlReadFileUsnDataReply.SecurityId) == 4) &&
                (fsctlReadFileUsnDataReply.SecurityId.GetType() == typeof(uint));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR763,
                763,
                @"[In USN_RECORD]SecurityId (4 bytes):  A 32-bit unsigned integer that contains an index of a unique 
                security identifier assigned to the file or directory associated with this record.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R775,the value of Reason is {0},the value of TimeStamp is {1},the value of SourceInfo is {2},the value of SecurityId is {3}", fsctlReadFileUsnDataReply.Reason, fsctlReadFileUsnDataReply.TimeStamp, fsctlReadFileUsnDataReply.SourceInfo, fsctlReadFileUsnDataReply.SecurityId);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R775
            //
            bool isVerifyR775 =
                (fsctlReadFileUsnDataReply.Reason == 0) &&
                (fsctlReadFileUsnDataReply.TimeStamp.dwHighDateTime == 0) &&
                (fsctlReadFileUsnDataReply.TimeStamp.dwLowDateTime == 0) &&
                (fsctlReadFileUsnDataReply.SourceInfo == 0) &&
                (fsctlReadFileUsnDataReply.SecurityId == 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR775,
                775,
                @"[In USN_RECORD]The fields Reason, TimeStamp, SourceInfo, and SecurityId for a USN RECORD element 
                    returned by this FSCTL MUST all be set 0.<28>");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R729");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R729
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME),
                fsctlReadFileUsnDataReply.TimeStamp.GetType(),
                729,
                @"[In USN_RECORD]TimeStamp (8 bytes):  A structure containing the absolute system time this change 
                journal event was logged, expressed as the number of 100-nanosecond intervals since January 1, 1601 
                (UTC), in the format of a FILETIME (section 2.1.1) structure.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R725,the value of Usn is {0}", fsctlReadFileUsnDataReply.Usn);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R725
            //
            // 64 bits equal 8 bytes.
            bool isVerifyR725 =
                (Marshal.SizeOf(fsctlReadFileUsnDataReply.Usn) == 8) &&
                (fsctlReadFileUsnDataReply.Usn.GetType() == typeof(long));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR725,
                725,
                @"[In USN_RECORD]Usn (8 bytes):  A 64-bit signed integer, opaque to the client, containing the USN of 
                the record.");

            // Check if no USN change journal records have been logged for the 
            // directory of the file associated with this record
            if (!isUsnChangeJournalRecordLogged)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R728");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R728
                //
                Site.CaptureRequirementIfAreEqual<long>(
                    0,
                    fsctlReadFileUsnDataReply.Usn,
                    728,
                    @"[In USN_RECORD]Usn (8 bytes):  This value MUST be 0 if no USN change journal records have been 
                    logged for the directory of the file associated with this record. ");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R727,the value of Usn is {0}", fsctlReadFileUsnDataReply.Usn);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R727
            //

            Site.CaptureRequirementIfIsTrue(
                fsctlReadFileUsnDataReply.Usn >= 0,
                727,
                @"[In USN_RECORD]Usn (8 bytes):  This value MUST be greater than or equal to 0.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.3.44 section

        #region Verify related RS in 2.3.46 section
        /// <summary>
        /// Verify the messge syntax in FSCTL_RECALL_FILE Reply
        /// </summary>
        /// <param name="statusCode">The status code returned directly by the function that processes this FSCTL.</param>
        public void VerifyMessageSyntaxFsctlRecallFileReply(
            FsctlRecallFileReplyStatus statusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            // status code list, as specified in [MS-ERREF]
            List<Enum> statusCodeList = new List<Enum>();
            statusCodeList.Add(FsctlRecallFileReplyStatus.ERROR_INVALID_FUNCTION);
            statusCodeList.Add(FsctlRecallFileReplyStatus.STATUS_ACCESS_DENIED);
            statusCodeList.Add(FsctlRecallFileReplyStatus.STATUS_INVALID_DEVICE_REQUEST);
            statusCodeList.Add(FsctlRecallFileReplyStatus.STATUS_NOT_SUPPORTED);
            statusCodeList.Add(FsctlRecallFileReplyStatus.STATUS_SUCCESS);

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R790,the value of statusCode is {0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R790
            //

            Site.CaptureRequirementIfIsTrue(
                statusCodeList.Contains(statusCode),
                790,
                @"[In FSCTL_RECALL_FILE Reply]The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_ACCESS_DENIED 0xC0000022, ERROR_INVALID_FUNCTION 0x00000001, STATUS_NOT_SUPPORTED 0xC00000BB].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R789,the value of statusCode is {0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R789
            //

            Site.CaptureRequirementIfIsTrue(
                statusCodeList.Contains(statusCode),
                789,
                @"The only data item this message[FSCTL_RECALL_FILE Reply] returns is a status code, as specified in [MS-ERREF] section 2.3.");
            
            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.3.46 section

        #region Verify related RS in 2.3.48 section
        /// <summary>
        /// Verify the message syntax in FSCTL_SET_COMPRESSION Reply
        /// </summary>
        /// <param name="statusCode">The status code returned directly by the function that processes this FSCTL.</param>
        public void VerifyMessageSyntaxFsctlSetCompressionReply(
            FsctlSetCompressionReplyStatus statusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            List<Enum> statusList = new List<Enum>();
            statusList.Add(FsctlSetCompressionReplyStatus.STATUS_DISK_FULL);
            statusList.Add(FsctlSetCompressionReplyStatus.STATUS_INVALID_DEVICE_REQUEST);
            statusList.Add(FsctlSetCompressionReplyStatus.STATUS_INVALID_PARAMETER);
            statusList.Add(FsctlSetCompressionReplyStatus.STATUS_SUCCESS);

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R807,the value of statusCode is {0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R807
            //

            Site.CaptureRequirementIfIsTrue(
                statusList.Contains(statusCode),
                807,
                @"[In FSCTL_SET_COMPRESSION Reply]The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_INVALID_PARAMETER 0xC000000D, STATUS_INVALID_DEVICE_REQUEST 0xC0000010, STATUS_DISK_FULL 0xC000007].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R806,the value of statusCode is {0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R806
            //

            Site.CaptureRequirementIfIsTrue(
                statusList.Contains(statusCode),
                806,
                @"The only data item this message[FSCTL_SET_COMPRESSION Reply] returns is a status code, as specified in [MS-ERREF] section 2.3.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.3.48 section

        #region Verify related RS in 2.3.50 section
        /// <summary>
        /// Verify the message syntax in FSCTL_SET_DEFECT_MANAGEMENT Reply
        /// </summary>
        /// <param name="statusCode">The status code returned directly by the function that processes this FSCTL.</param>
        public void VerifyMessageSyntaxFsctlSetDefectManagementReply(
            FsctlSetDefectManagementReplyStatus statusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            List<Enum> statusList = new List<Enum>();
            statusList.Add(FsctlSetDefectManagementReplyStatus.STATUS_FILE_INVALID);
            statusList.Add(FsctlSetDefectManagementReplyStatus.STATUS_INVALID_DEVICE_REQUEST);
            statusList.Add(FsctlSetDefectManagementReplyStatus.STATUS_INVALID_PARAMETER);
            statusList.Add(FsctlSetDefectManagementReplyStatus.STATUS_SHARING_VIOLATION);
            statusList.Add(FsctlSetDefectManagementReplyStatus.STATUS_SUCCESS);
            statusList.Add(FsctlSetDefectManagementReplyStatus.STATUS_VERIFY_REQUIRED);
            statusList.Add(FsctlSetDefectManagementReplyStatus.STATUS_VOLUME_DISMOUNTED);
            statusList.Add(FsctlSetDefectManagementReplyStatus.STATUS_WRONG_VOLUME);

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R816,the value of statusCode is {0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R816
            //

            Site.CaptureRequirementIfIsTrue(
                statusList.Contains(statusCode),
                816,
                @"The only data item this message[FSCTL_SET_DEFECT_MANAGEMENT Reply] returns is a status code, as specified in [MS-ERREF] section 2.3.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R817,the value of statusCode is {0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R817
            //

            Site.CaptureRequirementIfIsTrue(
                statusList.Contains(statusCode),
                817,
                @"The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_INVALID_PARAMETER 0xC000000D, STATUS_INVALID_DEVICE_REQUEST 0xC0000010, STATUS_SHARING_VIOLATION 0xC0000043, STATUS_VOLUME_DISMOUNTED 0xC000026e, STATUS_FILE_INVALID 0xC0000098, STATUS_WRONG_VOLUME 0xC0000012, STATUS_VERIFY_REQUIRED 0x80000016].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.3.50 section

        #region Verify related RS in 2.3.52 section
        /// <summary>
        /// Verify the message syntax in FSCTL_SET_ENCRYPTION Reply
        /// </summary>
        /// <param name="statusCode">The status code returned directly by the function that processes this FSCTL.</param>
        /// <param name="messageStatus">Message status code returned from the server.</param>
        /// <param name="isDirectoryNotSupportEncryption">A variable that means directory does not support encryption.</param>
        /// <param name="isFileSysContainSpecFile">A variable that means the file system of the volume containing the specified file.</param>
        public void VerifyMessageSyntaxFsctlSetEncryptionReply(
            FsctlSetEncryptionReplyStatus statusCode,
            MessageStatus messageStatus,
            bool isFileSysContainSpecFile,
            bool isDirectoryNotSupportEncryption)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //Check If the file system of the volume containing the specified file or directory does not support encryption
            if (isFileSysContainSpecFile || isDirectoryNotSupportEncryption)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R837");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R837
                //
                // If the message status is not equal with SUCCESS,then means the request will not succeed
                Site.CaptureRequirementIfAreNotEqual<MessageStatus>(
                    MessageStatus.Success,
                    messageStatus,
                    837,
                    @"[In FSCTL_SET_ENCRYPTION Reply]If the file system of the volume containing the specified file or directory does not support encryption, the request MUST NOT succeed.");
            }

            List<Enum> statusList = new List<Enum>();
            statusList.Add(FsctlSetEncryptionReplyStatus.STATUS_BUFFER_TOO_SMALL);
            statusList.Add(FsctlSetEncryptionReplyStatus.STATUS_FILE_CORRUPT_ERROR);
            statusList.Add(FsctlSetEncryptionReplyStatus.STATUS_INVALID_DEVICE_REQUEST);
            statusList.Add(FsctlSetEncryptionReplyStatus.STATUS_INVALID_PARAMETER);
            statusList.Add(FsctlSetEncryptionReplyStatus.STATUS_INVALID_USER_BUFFER);
            statusList.Add(FsctlSetEncryptionReplyStatus.STATUS_MEDIA_WRITE_PROTECTED);
            statusList.Add(FsctlSetEncryptionReplyStatus.STATUS_SUCCESS);
            statusList.Add(FsctlSetEncryptionReplyStatus.STATUS_VOLUME_DISMOUNTED);
            statusList.Add(FsctlSetEncryptionReplyStatus.STATUS_VOLUME_NOT_UPGRADED);

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R840,the value of statusCode is {0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R840
            //

            Site.CaptureRequirementIfIsTrue(
                statusList.Contains(statusCode),
                840,
                @"[In FSCTL_SET_ENCRYPTION Reply]The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or the following:[STATUS_MEDIA_WRITE_PROTECTED 0xC00000A2, STATUS_INVALID_PARAMETER 0xC000000D, STATUS_BUFFER_TOO_SMALL 0xC0000023, STATUS_VOLUME_NOT_UPGRADED 0xC000029C, STATUS_INVALID_DEVICE_REQUEST 0xC0000010, STATUS_FILE_CORRUPT_ERROR 0xC0000102, STATUS_VOLUME_DISMOUNTED 0xC000026E, STATUS_INVALID_USER_BUFFER 0xC00000E8].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R839,the value of statusCode is {0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R839
            //

            Site.CaptureRequirementIfIsTrue(
                statusList.Contains(statusCode),
                839,
                @"The only data item this message[FSCTL_SET_ENCRYPTION Reply] returns is a status code, as specified in [MS-ERREF] section 2.3.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.3.52 section

        #region Verify related RS in 2.3.54 section
        /// <summary>
        /// Verify the messge syntax in FSCTL_SET_OBJECT_ID Reply
        /// </summary>
        /// <param name="statusCode">The status code returned directly by the function that processes this FSCTL.</param>
        /// <param name="isFileSysContainSpecFile">A variable that means the file system of the volume containing the specified file.</param>
        /// <param name="isDirectoryNotSupportObjIdUse">A variable that means directory does not support the use of object IDs</param>
        public void VerifyMessageSyntaxFsctlSetObjectIdReply(
            FsctlSetObjectIdReplyStatus statusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            List<Enum> statusList = new List<Enum>();
            statusList.Add(FsctlSetObjectIdReplyStatus.STATUS_ACCESS_DENIED);
            statusList.Add(FsctlSetObjectIdReplyStatus.STATUS_INVALID_DEVICE_REQUEST);
            statusList.Add(FsctlSetObjectIdReplyStatus.STATUS_INVALID_PARAMETER);
            statusList.Add(FsctlSetObjectIdReplyStatus.STATUS_OBJECT_NAME_COLLISION);
            statusList.Add(FsctlSetObjectIdReplyStatus.STATUS_SUCCESS);

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R855,the value of statusCode is {0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R855
            //

            Site.CaptureRequirementIfIsTrue(
                statusList.Contains(statusCode),
                855,
                @"[In FSCTL_SET_OBJECT_ID Reply]The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following:[STATUS_INVALID_PARAMETER 0xC000000D, STATUS_ACCESS_DENIED 0xC0000022, STATUS_OBJECT_NAME_COLLISION 0xC0000035, STATUS_INVALID_DEVICE_REQUEST 0xC0000010].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R854,the value of statusCode is {0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R854
            //

            Site.CaptureRequirementIfIsTrue(
                statusList.Contains(statusCode),
                854,
                @"The only data item this message[FSCTL_SET_OBJECT_ID Reply] returns is a status code, as specified in [MS-ERREF] section 2.3.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion Verify related RS in 2.3.54 section

        #region Verify related RS in 2.3.56 section
        /// <summary>
        /// Verify the message syntax in FSCTL_SET_OBJECT_ID_EXTENDED Reply
        /// </summary>
        /// <param name="statusCode"> The status code returned directly by the function that processes this FSCTL.</param>
        /// <param name="isDirectoryNotSupportObjectIdUse">A variable that means directory does not support the use of ObjectIds</param>
        /// <param name="isFileSysContainSpecFile">A variable that means the file system of the volume containing the specified file.</param>
        /// <param name="messageStatus">Message status code returned from the server.</param>
        public void VerifyMessageSyntaxFsctlSetObjectIdExtendedReply(
            MessageStatus messageStatus,
            bool isFileSysContainSpecFile,
            bool isDirectoryNotSupportObjectIdUse)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            // Check If the file system of the volume containing the specified file or directory does not support the use of ObjectIds
            if (isFileSysContainSpecFile || isDirectoryNotSupportObjectIdUse)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R868");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R868
                //
                // If the message status is not equal with SUCCESS,then means the request will not succeed
                Site.CaptureRequirementIfAreNotEqual<MessageStatus>(
                    MessageStatus.Success,
                    messageStatus,
                    868,
                    @"[In FSCTL_SET_OBJECT_ID_EXTENDED Reply] If the file system of the volume containing the specified file or directory does not support the use of ObjectIds, the request will not succeed.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion Verify related RS in 2.3.56 section

        #region Verify related RS in 2.3.58 section
        /// <summary>
        /// Verify the message syntax in FSCTL_SET_REPARSE_POINT Reply
        /// </summary>
        /// <param name="statusCode">The status code returned directly by the function that processes this FSCTL.</param>
        /// <param name="isDirectoryNotSupportReparsePoint">A bool variable that means directory does not support reparse points.</param>
        /// <param name="isFileSysContainSpecFile">A bool variable that means the file system of the volume containing the specified file.</param>
        /// <param name="messageStatus">Message status code returned from the server.</param>
        public void VerifyMessageSyntaxFsctlSetReparsePointReply(
            FsctlSetReparsePointReplyStatus statusCode,
            MessageStatus messageStatus,
            bool isFileSysContainSpecFile,
            bool isDirectoryNotSupportReparsePoint)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            // Check If the file system of the volume containing the specified file or directory does not support reparse points.
            if (isFileSysContainSpecFile || isDirectoryNotSupportReparsePoint)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R882");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R882
                //
                // If the message status is not equal with SUCCESS,then means the request will not succeed
                Site.CaptureRequirementIfAreNotEqual<MessageStatus>(
                    MessageStatus.Success,
                    messageStatus,
                    882,
                    @"[In FSCTL_SET_REPARSE_POINT Reply] If the file system of the volume containing the specified file or directory does not support reparse points, the request will not succeed.");
            }

            List<Enum> statusList = new List<Enum>();
            statusList.Add(FsctlSetReparsePointReplyStatus.STATUS_INVALID_DEVICE_REQUEST);
            statusList.Add(FsctlSetReparsePointReplyStatus.STATUS_INVALID_PARAMETER);
            statusList.Add(FsctlSetReparsePointReplyStatus.STATUS_IO_REPARSE_DATA_INVALID);
            statusList.Add(FsctlSetReparsePointReplyStatus.STATUS_SUCCESS);

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R885,the value of statusCode is {0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R885
            //

            Site.CaptureRequirementIfIsTrue(
                statusList.Contains(statusCode),
                885,
                @"[In FSCTL_SET_REPARSE_POINT Reply]The status code returned directly by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following: [STATUS_INVALID_PARAMETER 0xC000000D, 
    STATUS_INVALID_BUFFER_SIZE 0xC0000206, 
    STATUS_IO_REPARSE_DATA_INVALID 0xC0000278, 
    STATUS_INVALID_DEVICE_REQUEST 0xC0000010].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R884,the value of statusCode is {0}", statusCode);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R884
            //

            Site.CaptureRequirementIfIsTrue(
                statusList.Contains(statusCode),
                884,
                @"The only data item this message[FSCTL_SET_REPARSE_POINT Reply] returns is a status code, as specified in [MS-ERREF] section 2.3.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion Verify related RS in 2.3.58 section

        #region Verify the datatype of FileObjectIdInformation Type 1 in section 2.4.28.1

        /// <summary>
        /// Verify the datatype of FileObjectIdInformation Type 1
        /// </summary>
        /// <param name="fileObjectIdInformationType1">the instance of FileObjectIdInformation Type 1 structure</param>
        public void VerifyDataTypeFileObjectIdInformationType1(FileObjectIdInformation_Type_1 fileObjectIdInformationType1)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1469");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1469
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Guid),
                fileObjectIdInformationType1.BirthObjectId.GetType(),
                1469,
                @"[In FileObjectIdInformation Type 1] BirthObjectId (16 bytes):  A 16-byte GUID value containing the object identifier of the object at the time it was created.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1467. BirthVolumeId: {0}", fileObjectIdInformationType1.BirthVolumeId);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1467
            //
            bool isVerifyR1467 = (fileObjectIdInformationType1.BirthVolumeId.GetType() == typeof(Guid));

            if (fileObjectIdInformationType1.ObjectId == Guid.Empty)
            {
                if (fileObjectIdInformationType1.BirthVolumeId != Guid.Empty)
                {
                    isVerifyR1467 = false;
                }
            }

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1467,
                1467,
                @"[In FileObjectIdInformation Type 1] BirthVolumeId (16 bytes):  A 16-byte GUID that uniquely identifies the volume on which the object resided when the object identifier was created, or zero if the volume had no object identifier at that time.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1471");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1471
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Guid),
                fileObjectIdInformationType1.DomainId.GetType(),
                1471,
                @"[In FileObjectIdInformation Type 1] DomainId (16 bytes):  A 16-byte GUID value containing the domain identifier.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1461");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1461
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(ulong),
                fileObjectIdInformationType1.FileReferenceNumber.GetType(),
                1461,
                @"[In FileObjectIdInformation Type 1] FileReferenceNumber (8 bytes):  A 64-bit unsigned integer that contains the file reference number for the file.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1463");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1463
            //
            // be unique cannot be verified, so here just to the field size which is 8 bytes
            Site.CaptureRequirementIfAreEqual<int>(
                8,
                Marshal.SizeOf(fileObjectIdInformationType1.FileReferenceNumber),
                1463,
                @"[In FileObjectIdInformation Type 1] FileReferenceNumber (8 bytes):  The file reference number is unique within the volume on which the file exists.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1464");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1464
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Guid),
                fileObjectIdInformationType1.ObjectId.GetType(),
                1464,
                @"[In FileObjectIdInformation Type 1] ObjectId (16 bytes):  A 16-byte GUID that uniquely identifies the file or directory within the volume on which it resides.");
        }

        #endregion

        #region Verify the datatype of FIND_BY_SID_DATA structure for FSCTL_FIND_FILES_BY_SID Request in section 2.3.9

        /// <summary>
        /// Verify the datatype of FIND_BY_SID_DATA structure for FSCTL_FIND_FILES_BY_SID Request
        /// </summary>
        /// <param name="findBySidData">the instance of FIND_BY_SID_DATA </param>
        /// <param name="isFirstCall">check if this call is the first one</param>
        public void VerifyDataTypeFindBySidData(FSCTL_FIND_FILES_BY_SID_Request findBySidData,
                                                 bool isFirstCall)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R388");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R388
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(uint),
                findBySidData.Restart.GetType(),
                388,
                @"[In FIND_BY_SID_DATA]Restart (4 bytes):  A 32-bit unsigned integer value that indicates to restart the search.");

            if (isFirstCall)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R389");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R389
                //
                Site.CaptureRequirementIfAreEqual<uint>(
                    1,
                    findBySidData.Restart,
                    389,
                    @"[In FIND_BY_SID_DATA]Restart (4 bytes):  This value MUST be 1 on first call so that the search starts from the root.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R391");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R391
            //
            // The defination of SID is in format from MS-SECO
            // so here the type is verified
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(SID),
                findBySidData.SID.GetType(),
                391,
                @"[In FIND_BY_SID_DATA]SID (variable):  A SID (see [MS-SECO]) data element that specifies the owner.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify the datatype of FileGetQuotaInformation in section 2.4.33.1

        /// <summary>
        /// Verify the datatype of FileGetQuotaInformation
        /// </summary>
        /// <param name="fileGetQuotaInformation">the instance of Verify the datatype of FileGetQuotaInformation</param>
        /// <param name="isMutipleEntries">check if multiple entries are present in a buffer</param>
        public void VerifyDataTypeFileGetQuotaInformation(FILE_QUOTA_INFORMATION fileGetQuotaInformation,
                                                           bool isMutipleEntries)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1549");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1549
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(uint),
                fileGetQuotaInformation.NextEntryOffset.GetType(),
                1549,
                @"[In FileGetQuotaInformation]NextEntryOffset (4 bytes):  A 32-bit unsigned integer that contains the byte offset from the beginning of this entry, at which the next FILE_GET_QUOTA_INFORMATION entry is located, if multiple entries are present in a buffer.");


            if (isMutipleEntries)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1551");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1551
                //
                // NextEntryOffset is equal to the sid length and the size (4 bytes) of NextEntryOffset field
                Site.CaptureRequirementIfAreEqual<uint>(
                    fileGetQuotaInformation.SidLength + 4,
                    fileGetQuotaInformation.NextEntryOffset,
                    1551,
                    @"[In FileGetQuotaInformation]NextEntryOffset (4 bytes): An implementation MUST use this value to determine the location of the next entry (if multiple entries are present in a buffer).");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1555");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1555
            //
            // The structure is got by the stack parsing according to the format
            // so here the type is to be verified
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(SID),
                fileGetQuotaInformation.Sid.GetType(),
                1555,
                @"[In FileGetQuotaInformation]Sid (variable): SIDs are sent in little-endian format and require no packing.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1556");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1556
            //
            // from stack parese, if the type is SID, the format can be verified
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(SID),
                fileGetQuotaInformation.Sid.GetType(),
                1556,
                @"[In FileGetQuotaInformation]Sid (variable): The format of a SID is as specified in [MS-DTYP] section 2.4.2.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1553");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1553
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                (uint)Marshal.SizeOf(fileGetQuotaInformation.Sid),
                fileGetQuotaInformation.SidLength,
                1553,
                @"[In FileGetQuotaInformation]SidLength (4 bytes):  A 32-bit unsigned integer that contains the length, in bytes, of the <Sid> data element.");

            // NextEntryOffset is set 0
            if (0 == fileGetQuotaInformation.NextEntryOffset)
            {
                // here cannot be sure whether the parsing will continue when NextEntryOffset is 0,
                // so no other entries following is thought by default, just to capture this requirment directly
                Site.CaptureRequirement(
                    1550,
                    @"[In FileGetQuotaInformation]NextEntryOffset (4 bytes): This member MUST be zero if no other entries follow this one[FILE_GET_QUOTA_INFORMATION entry].");
            }

            // Above verification has been done for FileGetQuotaInformation structure, 
            // so capture this requirment directly
            Site.CaptureRequirement(
                1548,
                @"The information class[FileGetQuotaInformation] is used to provide the list of SIDs for which query quota information is requested.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify the datatype of FILE_OBJECTID_BUFFER Type 1 in section 2.1.3.1
        /// <summary>
        /// Verify the datatype of FILE_OBJECTID_BUFFER Type 1 in section 2.1.3.1
        /// </summary>        
        /// <param name="fileObjectidBufferType1">the instance of structure FILE_OBJECTID_BUFFER_Type_1</param>
        /// <param name="isNTFSFileSystem">check if file system is NTFS</param>
        public void VerifyDataTypeFileObjectidBufferType1(FILE_OBJECTID_BUFFER_Type_1 fileObjectidBufferType1,
                                                           bool isNTFSFileSystem)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R121");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R121
            //
            // here just to verify the field type
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Guid),
                fileObjectidBufferType1.ObjectId.GetType(),
                121,
                @"[In FILE_OBJECTID_BUFFER Type 1]ObjectId (16 bytes):  A 16-byte GUID that uniquely identifies the file or directory within the volume on which it resides.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R127");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R127
            //
            // Here just to verify the type of this field
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Guid),
                fileObjectidBufferType1.BirthObjectId.GetType(),
                127,
                @"[In FILE_OBJECTID_BUFFER Type 1]BirthObjectId (16 bytes):  A 16-byte GUID value containing the object identifier of the object at the time it was created.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R131");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R131
            //
            //
            //the action of assigned cannot be captured, so here verify the assigned BirthObjectId size
            // 16 bytes for actual size
            Site.CaptureRequirementIfAreNotEqual<int>(
                16,
                Marshal.SizeOf(fileObjectidBufferType1.BirthObjectId),
                131,
                @"[In FILE_OBJECTID_BUFFER Type 1]BirthObjectId (16 bytes): The object ID is assigned at file creation time.<3>");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R124. BirthVolumeId: {0}", fileObjectidBufferType1.BirthVolumeId);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R124
            //
            bool isVerifyR124 = (fileObjectidBufferType1.BirthVolumeId.GetType() == typeof(Guid));

            if (fileObjectidBufferType1.ObjectId == Guid.Empty)
            {
                if (fileObjectidBufferType1.BirthVolumeId != Guid.Empty)
                {
                    isVerifyR124 = false;
                }
            }

            Site.CaptureRequirementIfIsTrue(
                isVerifyR124,
                124,
                @"[In FILE_OBJECTID_BUFFER Type 1]BirthVolumeId (16 bytes):  A 16-byte GUID that uniquely identifies the volume on which the object resided when the object identifier was created, or zero if the volume had no object identifier at that time.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R132");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R132
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Guid),
                fileObjectidBufferType1.DomainId.GetType(),
                132,
                @"[In FILE_OBJECTID_BUFFER Type 1]DomainId (16 bytes):  A 16-byte GUID value containing the domain identifier.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R133");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R133
            //
            // unused or not can not be verified on traffic, so here just to verify the type
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Guid),
                fileObjectidBufferType1.DomainId.GetType(),
                133,
                @"[In FILE_OBJECTID_BUFFER Type 1]DomainId (16 bytes):  This value is unused.");

            if (isNTFSFileSystem)
            {
                // After verification above, the struct have been verified, so here
                // just to capture this requirement directly
                Site.CaptureRequirement(
                    1844,
                    @"<4> Section 2.1.3.1: The NTFS file system places no constraints on the format of the 48 bytes of information following the ObjectId in this structure.");
            }

            if (bool.Parse(Site.Properties["IsWindows"]))
            {
                // After verification above, the struct have been got, so here
                // just to capture this requirement directly
                Site.CaptureRequirement(
                    1845,
                    @"<4> Section 2.1.3.1: This format[FILE_OBJECTID_BUFFER Type 1] of the FILE_OBJECTID_BUFFER is used on Windows by the Microsoft Distributed Link Tracking Service (see [MS-DLTW] section 3.1.6).");
            }

            string ServerOS = Site.Properties["ServerOSPlatform"];
            if (ServerOS == "Win2k" || ServerOS == "WinXP" || ServerOS == "Win2k3" || ServerOS == "WinVista" ||
                ServerOS == "Win2k8" || ServerOS == "Win2k8R2" || ServerOS == "Win7")
            {
                // Above all has make the ObjectId verified, and a random unique value cannot be actually validated
                // so just to capture this requirement directly
                Site.CaptureRequirement(
                    1843,
                    @"<3> Section 2.1.3.1: When a file is moved or copied from one volume to another, the ObjectId member value changes to a random unique value to avoid the potential for ObjectId collisions because the object ID is not guaranteed to be unique across volumes.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify the datatype of FILE_PIPE_LOCAL_INFORMATION in section 2.4.30
        /// <summary>
        /// Verify the datatype of FILE_PIPE_LOCAL_INFORMATION in section 2.4.30
        /// </summary>
        /// <param name="filePipeLocalInformation">the instance of structure FILE_PIPE_LOCAL_INFORMATION</param>
        /// <param name="isFirstInstance">check if this instance is the first one</param>
        /// <param name="expectedNumOfNamedPipeInstance">the expected number of current named pipe instances</param>
        public void VerifyDataTypeFilePipeLocalInformation(FilePipeLocalInformation filePipeLocalInformation,
                                                            bool isFirstInstance,
                                                            uint expectedNumOfNamedPipeInstance)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1509. CurrentInstances: {0}", filePipeLocalInformation.CurrentInstances);

            // 
            // Verify MS-FSCC requirement: MS-FSCC_R1509
            //
            bool isVerifyR1509 = (filePipeLocalInformation.CurrentInstances.GetType() == typeof(UInt32) &&
                                  filePipeLocalInformation.CurrentInstances == expectedNumOfNamedPipeInstance);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1509,
                1509,
                @"[In FILE_PIPE_LOCAL_INFORMATION]CurrentInstances (4 bytes):  A 32-bit unsigned integer that contains the number of current named pipe instances.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1510");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1510
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(uint),
                filePipeLocalInformation.InboundQuota.GetType(),
                1510,
                @"[In FILE_PIPE_LOCAL_INFORMATION]InboundQuota (4 bytes):  A 32-bit unsigned integer that contains the inbound quota in bytes for the named pipe.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1507");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1507
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(uint),
                filePipeLocalInformation.MaximumInstances.GetType(),
                1507,
                @"[In FILE_PIPE_LOCAL_INFORMATION]MaximumInstances (4 bytes):  A 32-bit unsigned integer that contains the maximum number of instances that can be created for this pipe.");

            if (isFirstInstance)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1508. MaximumInstances: {0}", filePipeLocalInformation.MaximumInstances);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1508
                //
                // specify this MaximumInstances value means its must be filled with any non-negative numbers
                bool isVerifyR1508 = (filePipeLocalInformation.MaximumInstances >= 0);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1508,
                    1508,
                    @"[In FILE_PIPE_LOCAL_INFORMATION]MaximumInstances (4 bytes): The first instance of the pipe MUST specify this value.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1502");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1502
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(NamedPipeConfiguration_Values),
                filePipeLocalInformation.NamedPipeConfiguration.GetType(),
                1502,
                @"[In FILE_PIPE_LOCAL_INFORMATION]NamedPipeConfiguration (4 bytes):  A 32-bit unsigned integer that contains the named pipe configuration.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1503. NamedPipeConfiguration: {0}", filePipeLocalInformation.NamedPipeConfiguration);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1503
            //
            bool isVerifyR1503 = (filePipeLocalInformation.NamedPipeConfiguration == NamedPipeConfiguration_Values.FILE_PIPE_FULL_DUPLEX ||
                                  filePipeLocalInformation.NamedPipeConfiguration == NamedPipeConfiguration_Values.FILE_PIPE_INBOUND ||
                                  filePipeLocalInformation.NamedPipeConfiguration == NamedPipeConfiguration_Values.FILE_PIPE_OUTBOUND);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1503,
                1503,
                @"[In FILE_PIPE_LOCAL_INFORMATION]NamedPipeConfiguration (4 bytes): MUST be one of the following:[FILE_PIPE_INBOUND 0x00000000, FILE_PIPE_OUTBOUND 0x00000001, FILE_PIPE_FULL_DUPLEX 0x00000002].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1520");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1520
            //
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(NamedPipeEnd_Values),
                filePipeLocalInformation.NamedPipeEnd.GetType(),
                1520,
                @"[In FILE_PIPE_LOCAL_INFORMATION]NamedPipeEnd (4 bytes):  A 32-bit unsigned integer that contains the type of the named pipe end, which specifies whether this is the client or the server side of a named pipe.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1521. NamedPipeEnd: {0}", filePipeLocalInformation.NamedPipeEnd);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1521
            //
            bool isVerifyR1521 = (filePipeLocalInformation.NamedPipeEnd == NamedPipeEnd_Values.FILE_PIPE_CLIENT_END ||
                                  filePipeLocalInformation.NamedPipeEnd == NamedPipeEnd_Values.FILE_PIPE_SERVER_END);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1521,
                1521,
                @"[In FILE_PIPE_LOCAL_INFORMATION]NamedPipeEnd (4 bytes): MUST be one of the following:[FILE_PIPE_CLIENT_END 0x00000000, FILE_PIPE_SERVER_END 0x00000001].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1514");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1514
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(NamedPipeState_Values),
                filePipeLocalInformation.NamedPipeState.GetType(),
                1514,
                @"[In FILE_PIPE_LOCAL_INFORMATION]NamedPipeState (4 bytes):  A 32-bit unsigned integer that contains the named pipe state that specifies the connection status for the named pipe.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1515.");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1515
            //
            bool isVerifyR1515 = (filePipeLocalInformation.NamedPipeState == FilePipeLocalInformation_NamedPipeState_Values.FILE_PIPE_CLOSING_STATE ||
                                  filePipeLocalInformation.NamedPipeState == FilePipeLocalInformation_NamedPipeState_Values.FILE_PIPE_CONNECTED_STATE ||
                                  filePipeLocalInformation.NamedPipeState == FilePipeLocalInformation_NamedPipeState_Values.FILE_PIPE_DISCONNECTED_STATE ||
                                  filePipeLocalInformation.NamedPipeState == FilePipeLocalInformation_NamedPipeState_Values.FILE_PIPE_LISTENING_STATE);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1515,
                1515,
                @"[In FILE_PIPE_LOCAL_INFORMATION]NamedPipeState (4 bytes): MUST be one of the following:[FILE_PIPE_DISCONNECTED_STATE 0x00000001, FILE_PIPE_LISTENING_STATE 0x00000002, FILE_PIPE_CONNECTED_STATE 0x00000003, FILE_PIPE_CLOSING_STATE 0x00000004].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1498");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1498
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(NamedPipeType_Values),
                filePipeLocalInformation.NamedPipeType.GetType(),
                1498,
                @"[In FILE_PIPE_LOCAL_INFORMATION]NamedPipeType (4 bytes): NamedPipeType (4 bytes):  A 32-bit unsigned integer that contains the named pipe type.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1499. NamedPipeType: {0}", filePipeLocalInformation.NamedPipeType);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1499
            //
            bool isVerifyR1499 = (filePipeLocalInformation.NamedPipeType == NamedPipeType_Values.FILE_PIPE_BYTE_STREAM_TYPE ||
                                  filePipeLocalInformation.NamedPipeType == NamedPipeType_Values.FILE_PIPE_MESSAGE_TYPE);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1499,
                1499,
                @"[In FILE_PIPE_LOCAL_INFORMATION]NamedPipeType (4 bytes): MUST be one of the following:[FILE_PIPE_BYTE_STREAM_TYPE 0x00000000, FILE_PIPE_MESSAGE_TYPE 0x00000001].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1512");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1512
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(uint),
                filePipeLocalInformation.OutboundQuota.GetType(),
                1512,
                @"[In FILE_PIPE_LOCAL_INFORMATION]OutboundQuota (4 bytes):  A 32-bit unsigned integer that contains outbound quota in bytes for the named pipe.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1511");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1511
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(uint),
                filePipeLocalInformation.ReadDataAvailable.GetType(),
                1511,
                @"[In FILE_PIPE_LOCAL_INFORMATION]ReadDataAvailable (4 bytes):  A 32-bit unsigned integer that contains the bytes of data available to be read from the named pipe.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1513");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1513
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(uint),
                filePipeLocalInformation.WriteQuotaAvailable.GetType(),
                1513,
                @"[In FILE_PIPE_LOCAL_INFORMATION]WriteQuotaAvailable (4 bytes):  A 32-bit unsigned integer that contains the write quota in bytes for the named pipe.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify the datatype of FILE_RENAME_INFORMATION for SMB2 in section 2.4.34.2
        /// <summary>
        /// Verify the datatype of FILE_RENAME_INFORMATION for SMB2 in section 2.4.34.2
        /// </summary>
        /// <param name="fileRenameInformationForSMB2">the instance of structure FILE_RENAME_INFORMATION for SMB2</param>
        /// <param name="isRenameFail">check if the rename operation fail</param>
        /// <param name="isForNetworkOperation">check if FILE_RENAME_INFORMATION is for network operations</param>
        public void VerifyDataTypeFileRenameInformationForSMB2(FileRenameInformation_SMB2 fileRenameInformationForSMB2,
                                                                bool isRenameFail,
                                                                bool isForNetworkOperation)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1587");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1587
            //
            //Under C#, the charater array is Unicode, so here just to verify its type
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(byte[]),
                fileRenameInformationForSMB2.FileName.GetType(),
                1587,
                @"[In FILE_RENAME_INFORMATION for SMB2]FileName (variable):  A sequence of Unicode characters containing the file name.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1588. The last byte of FileName: {0}, FileName: {1}", fileRenameInformationForSMB2.FileName[fileRenameInformationForSMB2.FileNameLength - 1], fileRenameInformationForSMB2.FileName);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1588
            //
            bool isVerifyR1588 = (fileRenameInformationForSMB2.FileName[fileRenameInformationForSMB2.FileNameLength - 1] != '\0');

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1588,
                1588,
                @"[In FILE_RENAME_INFORMATION for SMB2]FileName (variable): It is not a NULL-terminated string.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1588. The last byte of FileName: {0}, FileName: {1}", fileRenameInformationForSMB2.FileName[fileRenameInformationForSMB2.FileNameLength - 1], fileRenameInformationForSMB2.FileName);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1589
            //
            bool isVerifyR1589 = (fileRenameInformationForSMB2.FileName[fileRenameInformationForSMB2.FileNameLength - 1] != '\0');

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1589,
                1589,
                @"[In FILE_RENAME_INFORMATION for SMB2]FileName (variable): This field MUST be handled as a sequence of FileNameLength bytes, not as a NULL-terminated string.");

            if (0 == (ulong)fileRenameInformationForSMB2.RootDirectory)
            {

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1590. FileName: {0}", fileRenameInformationForSMB2.FileName);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1590
                //
                bool isVerifyR1590 = string.IsNullOrEmpty(fileRenameInformationForSMB2.FileName.ToString());

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1590,
                    1590,
                    @"[In FILE_RENAME_INFORMATION for SMB2]FileName (variable): If the RootDirectory member is 0, this member MUST specify an absolute path name to be assigned to the file.");
            }
            else
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1591. FileName: {0}", fileRenameInformationForSMB2.FileName);

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1591
                //
                // check if the path has relative information
                bool isVerifyR1591 = (System.IO.Path.IsPathRooted(fileRenameInformationForSMB2.FileName.ToString()));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1591,
                    1591,
                    @"[In FILE_RENAME_INFORMATION for SMB2]FileName (variable): If the RootDirectory member is not 0, this member MUST specify a relative path name, relative to RootDirectory, for the new name of the file.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1586. FileNameLength: {0}", fileRenameInformationForSMB2.FileNameLength);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1586
            //
            bool isVerifyR1586 = (fileRenameInformationForSMB2.FileNameLength.GetType() == typeof(uint) &&
                                  fileRenameInformationForSMB2.FileNameLength == Marshal.SizeOf(fileRenameInformationForSMB2.FileName));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1586,
                1586,
                @"[In FILE_RENAME_INFORMATION for SMB2]FileNameLength (4 bytes):  A 32-bit unsigned integer that contains the length in bytes of the new name for the file, including the trailing NULL if present.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1578");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1578
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(byte),
                fileRenameInformationForSMB2.ReplaceIfExists.GetType(),
                1578,
                @"[In FILE_RENAME_INFORMATION for SMB2]ReplaceIfExists (1 byte):  MUST be an 8-bit field.");

            // ReplaceIfExists is set to 0
            if (0 == fileRenameInformationForSMB2.ReplaceIfExists)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1580.");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1580
                //
                bool isVerifyR1580 = isRenameFail;
                // just to check the bool resault
                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1580,
                    1580,
                    @"[In FILE_RENAME_INFORMATION for SMB2]ReplaceIfExists (1 byte): If set to 0, the rename operation MUST fail if a file with the given name already exists.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1581");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1581
            //
            // 7 bytes for size
            Site.CaptureRequirementIfAreEqual<uint>(
                7,
                (uint)Marshal.SizeOf(fileRenameInformationForSMB2.Reserved),
                1581,
                @"[In FILE_RENAME_INFORMATION for SMB2]Reserved (7 bytes):  Reserved area for alignment.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1582");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1582
            //
            //any value cannnot be actually verified, so just to check its type
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(byte[]),
                fileRenameInformationForSMB2.Reserved.GetType(),
                1582,
                @"[In FILE_RENAME_INFORMATION for SMB2]Reserved (7 bytes): This field can contain any value.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1584");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1584
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(FileRenameInformation_SMB2_RootDirectory_Values),
                fileRenameInformationForSMB2.RootDirectory.GetType(),
                1584,
                @"[In FILE_RENAME_INFORMATION for SMB2]RootDirectory (8 bytes):  A 64-bit unsigned integer that contains the file handle for the root directory.");

            if (isForNetworkOperation)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1585");

                //
                // Verify MS-FSCC requirement: MS-FSCC_R1585
                //
                Site.CaptureRequirementIfAreEqual<ulong>(
                    0,
                    (ulong)fileRenameInformationForSMB2.RootDirectory,
                    1585,
                    @"[In FILE_RENAME_INFORMATION for SMB2]RootDirectory (8 bytes):  For network operations, this value MUST always be zero.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion


        #region Verify data type FileBasicInformation

        /// <summary>
        /// Verify the data type FileBasicInformation, which is defined in TD 2.4.7
        /// </summary>
        /// <param name="fileBasicInformation">FileBasicInformation type data</param>
        public void VerifyDataTypeFileBasicInformation(
            FileBasicInformation fileBasicInformation)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1024, actual  CreationTime: {0}",
                fileBasicInformation.CreationTime.ToString());

            //
            // Verify MS-FSCC requirement 1024
            //
            // Verify if the actual creationTime equals to the expcted creationTime
            Site.CaptureRequirementIfAreEqual<int>(
                8,
                Marshal.SizeOf(fileBasicInformation.CreationTime),
                1024,
                @"[In FILE_BASIC_INFORMATION]CreationTime (8 bytes):  A 64-bit signed integer that contains the 
                time when the file was created.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1025, actual CreationTime: {0}",
                fileBasicInformation.CreationTime.ToString());

            //
            // Verify MS-FSCC requirement 1025
            //
            bool isVerifiedR1025 =
                fileBasicInformation.CreationTime.GetType() == typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME);
                // Marshal.SizeOf(fileBasicInformation.CreationTime) == sizeofCreationTime);

            // And the size of the CreationTime is verified above, we didn't verify here again
            Site.CaptureRequirementIfIsTrue(
                isVerifiedR1025,
                1025,
                @"[In FILE_BASIC_INFORMATION]CreationTime (8 bytes): All dates and times in this message are 
                in absolute system-time format, which is represented as a FILETIME (section 2.1.1) structure.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1026, actual CreationTime: {0}",
                fileBasicInformation.CreationTime.ToString());
            
            //
            // Verify MS-FSCC requirement 1026
            //
            // As the size of the CreationTime is verified above, this rs will not be verified again
            bool isVerifyR1026 = (fileBasicInformation.CreationTime.dwHighDateTime > 0 ||
                 fileBasicInformation.CreationTime.dwLowDateTime > 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1026,
                1026,
                @"[In FILE_BASIC_INFORMATION]CreationTime (8 bytes): A valid time for this field is an integer 
                greater than 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1029, actual CreationTime: {0}",
                 fileBasicInformation.CreationTime.ToString());
            
            //
            // Verify MS-FSCC requirement 1029
            //
            // As the type of the CreationTime has been verified above, this rs will not be verified again
            // As the type of the CreationTime is uint, it can not be less 0, so only verifiy if greater than or equal to 0
            bool isVerifyR1029 = (fileBasicInformation.CreationTime.dwHighDateTime >= 0 &&
                 fileBasicInformation.CreationTime.dwLowDateTime >= 0);
            
            Site.CaptureRequirementIfIsTrue(
                isVerifyR1029,
                1029,
                @"[In FILE_BASIC_INFORMATION]CreationTime (8 bytes): This field MUST NOT be set to a value less 
                than -1.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1030, actual LastAccessTime: {0}",
                 fileBasicInformation.LastAccessTime.ToString());

            //
            // Verify MS-FSCC requirement 1030
            //
            // Verify if the actual lastaccesstime equals to the expected lastaccesstime.
            int sizeofLastAccessTime = 8;
            bool isVerifyR1030 =
                ((fileBasicInformation.LastAccessTime.GetType() == typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME) &&
                 Marshal.SizeOf(fileBasicInformation.LastAccessTime) == sizeofLastAccessTime));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1030,
                1030,
                @"[In FILE_BASIC_INFORMATION]LastAccessTime (8 bytes):  A 64-bit signed integer that contains the 
                last time the file was accessed in the format of a FILETIME structure.");

           
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1031, actual LastAccessTime: {0}",
                fileBasicInformation.LastAccessTime.ToString());
            //
            // Verify MS-FSCC requirement 1031
            //
            bool isVerifyR1031 = (fileBasicInformation.LastAccessTime.dwHighDateTime > 0 ||
                 fileBasicInformation.LastAccessTime.dwLowDateTime > 0) &&
                 (Marshal.SizeOf(fileBasicInformation.LastAccessTime) == sizeofLastAccessTime);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1031,
                1031,
                @"[In FILE_BASIC_INFORMATION]LastAccessTime (8 bytes): A valid time for this field is an integer 
                greater than 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1034, actual LastAccessTime: {0}",
                 fileBasicInformation.LastAccessTime.ToString());
            //
            // Verify MS-FSCC requirement 1034
            //
            // As the type of the LastAccessTime is uint, it can not be less 0, so only verifiy if >= 0
            bool isVerifyR1034 = (fileBasicInformation.LastAccessTime.dwHighDateTime >= 0 &&
                 fileBasicInformation.LastAccessTime.dwLowDateTime >= 0) &&
                 Marshal.SizeOf(fileBasicInformation.LastAccessTime) == sizeofLastAccessTime;

            Site.CaptureRequirementIfIsTrue(
                 isVerifyR1034,
                 1034,
                 @"[In FILE_BASIC_INFORMATION]LastAccessTime (8 bytes): This field MUST NOT be set to a value less
                 than -1. <54>");

          
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1035, actual LastWriteTime: {0}",
                fileBasicInformation.LastWriteTime.ToString());
                        
            int sizeofLastWriteTime = 8;
            //
            // Verify MS-FSCC requirement 1035
            //
            bool isVerifyR1035 =
                (fileBasicInformation.LastWriteTime.GetType() == typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME) &&
                 Marshal.SizeOf(fileBasicInformation.LastWriteTime) == sizeofLastWriteTime);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1035,
                1035,
                @"[In FILE_BASIC_INFORMATION]LastWriteTime (8 bytes):  A 64-bit signed integer that contains the 
                last time information was written to the file in the format of a FILETIME structure.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1036, actual LastWriteTime: {0}",
                 fileBasicInformation.LastWriteTime.ToString());

            
            //
            // Verify MS-FSCC requirement 1036
            //
            // And the size of the LastWriteTime is verified above, we didn't verify here again
            bool isVerifyR1036 = (fileBasicInformation.LastWriteTime.dwHighDateTime > 0 ||
                 fileBasicInformation.LastWriteTime.dwLowDateTime > 0) &&
                 Marshal.SizeOf(fileBasicInformation.LastWriteTime) == sizeofLastWriteTime;

            Site.CaptureRequirementIfIsTrue(
                 isVerifyR1036,
                 1036,
                 @"[In FILE_BASIC_INFORMATION]LastWriteTime (8 bytes): A valid time for this field is an 
                 integer greater than 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1039, actual LastWriteTime: {0}",
                fileBasicInformation.LastWriteTime.ToString());
            
            //
            // Verify MS-FSCC requirement 1039
            //
            // As the type of the LastWriteTime is uint, it can not be less 0, so only verifiy if >= 0
            bool isVerifyR1039 = (fileBasicInformation.LastWriteTime.dwHighDateTime >= 0 &&
                                  fileBasicInformation.LastWriteTime.dwLowDateTime >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1039,
                1039,
                @"[In FILE_BASIC_INFORMATION]LastWriteTime (8 bytes): This field MUST NOT be set to a value less 
                than -1. <55>");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1040, actual ChangeTime: {0}",
                fileBasicInformation.ChangeTime.ToString());
            
            //
            // Verify MS-FSCC requirement 1040
            //
            bool isVerifyR1040 =
                (fileBasicInformation.ChangeTime.GetType() == typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME) &&
                 Marshal.SizeOf(fileBasicInformation.ChangeTime) == 8 );

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1040,
                1040,
                @"[In FILE_BASIC_INFORMATION]ChangeTime (8 bytes):  A 64-bit signed integer that contains the last
                time the file was changed in the format of a FILETIME structure.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1041, actual ChangeTime: {0}",
                 fileBasicInformation.ChangeTime.ToString());
            //
            // Verify MS-FSCC requirement 1041
            //
            bool isVerifyR1041 = (fileBasicInformation.ChangeTime.dwHighDateTime > 0 ||
                 fileBasicInformation.ChangeTime.dwLowDateTime > 0) &&
                 Marshal.SizeOf(fileBasicInformation.ChangeTime) == 8;

            Site.CaptureRequirementIfIsTrue(
                 isVerifyR1041,
                 1041,
                 @"[In FILE_BASIC_INFORMATION]ChangeTime (8 bytes):  A valid time for this field is an integer 
                 greater than 0.");
        
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1044, actual ChangeTime: {0}",
                 fileBasicInformation.ChangeTime.ToString());
            //
            // Verify MS-FSCC requirement 1044
            //
            // As the type of the ChangeTime is uint, it can not be less 0, so only verifiy if >= 0
            bool isVerifyR1044 = (fileBasicInformation.ChangeTime.dwHighDateTime >= 0 &&
                                  fileBasicInformation.ChangeTime.dwLowDateTime >= 0) &&
                                  Marshal.SizeOf(fileBasicInformation.ChangeTime) == 8; 

            Site.CaptureRequirementIfIsTrue(
                 isVerifyR1044,
                 1044,
                 @"[In FILE_BASIC_INFORMATION]ChangeTime (8 bytes):  This field MUST NOT be set to a value less 
                 than -1. <56>");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1045, actual FileAttributes: {0}",
                fileBasicInformation.FileAttributes);
            //
            // Verify MS-FSCC requirement 1045
            //
            // right now, the file attribute is Fuzzy and untestable, so we didn't verify if it's legal.

            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(fileBasicInformation.FileAttributes),
                1045,
                @"[In FILE_BASIC_INFORMATION]FileAttributes (4 bytes):  A 32-bit unsigned integer that contains 
                the file attributes.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1046, actual FileAttributes: {0}", fileBasicInformation.FileAttributes);
            
            //
            // Verify MS-FSCC requirement 1046
            //
            // Verify the size and the value of FileAttributes, and the values are:
            // FILE_ATTRIBUTE_ARCHIVE 0x00000020, FILE_ATTRIBUTE_COMPRESSED 0x00000800, 
            // FILE_ATTRIBUTE_DIRECTORY 0x00000010, FILE_ATTRIBUTE_ENCRYPTED 0x00004000, 
            // FILE_ATTRIBUTE_HIDDEN 0x00000002, FILE_ATTRIBUTE_NORMAL 0x00000080, 
            // FILE_ATTRIBUTE_NOT_CONTENT_INDEXED 0x00002000, FILE_ATTRIBUTE_OFFLINE 0x00001000, 
            // FILE_ATTRIBUTE_READONLY 0x00000001, FILE_ATTRIBUTE_REPARSE_POINT 0x00000400, 
            // FILE_ATTRIBUTE_SPARSE_FILE 0x00000200, FILE_ATTRIBUTE_SYSTEM 0x00000004, 
            // FILE_ATTRIBUTE_TEMPORARY 0x00000100] 
            uint bitUsed = (
                0x00000020 | 0x00000800 | 0x00000010 | 0x00004000 |
                0x00000002 | 0x00000080 | 0x00002000 | 0x00001000 |
                0x00000001 | 0x00000400 | 0x00000200 | 0x00000004 | 0x00000100);
            
            // Get the bits which the file attributes not used
            uint bitUnused = ~bitUsed;
            const int sizeOfFileAttributes = 4;

            bool isVerifyR11046 = ((sizeOfFileAttributes == Marshal.SizeOf(fileBasicInformation.FileAttributes)) &&
                                  ((bitUnused & fileBasicInformation.FileAttributes) == 0x00000000));
            // Verify the size and the actual value of FileAttributes
            Site.CaptureRequirementIfIsTrue(
                isVerifyR11046,
                1046,
                @"[In FILE_BASIC_INFORMATION]FileAttributes (4 bytes): Valid file attributes are specified in 
                section 2.6.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1047, actual type of Reserved: {0};", fileBasicInformation.Reserved.GetType());
            
            //
            // Verify MS-FSCC requirement 1047
            //
            const int sizeOfReserved = 4;

            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfReserved,
                Marshal.SizeOf(fileBasicInformation.Reserved),
                1047,
                @"[In FILE_BASIC_INFORMATION]Reserved (4 bytes):  A 32-bit field.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1048");
            //
            // Verify MS-FSCC requirement 1048
            //
            // As the value of the Reserved can be any value, only verify the size of it
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfReserved,
                Marshal.SizeOf(fileBasicInformation.Reserved),
                1048,
                @"[In FILE_BASIC_INFORMATION]Reserved (4 bytes): This field is reserved.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1050");
            
            //
            // Verify MS-FSCC requirement 1050
            //
            // As the value of the Reserved can be any value, this rs will be only verified the size.
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfReserved,
                Marshal.SizeOf(fileBasicInformation.Reserved),
                1050,
                @"[In FILE_BASIC_INFORMATION]Reserved (4 bytes): This field can be set to any value.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1023");
                       
            //
            // Verify MS-FSCC requirement 1023
            //
            // As all the elements in the FILE_BASIC_INFORMATION are verified above,
            // This rs will be captured directly
            Site.CaptureRequirement(
                1023,
                @"The FILE_BASIC_INFORMATION data element is as follows:[CreationTime, ..., LastAccessTime, ..., 
                LastWriteTime, ..., ChangeTime, ..., FileAttributes, Reserved].");

            // for FILE_BASIC_INFORMATION is defined as section 2.4.7 in the sdk,
            // after all the member of FILE_BASIC_INFORMATION is verified , this rs can be covered.
            Site.CaptureRequirement(
                978,
                @"[In FileAllInformation]BasicInformation (40 bytes):  
                A FILE_BASIC_INFORMATION structure specified in section 2.4.7.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify data type FileStandardInformation

        /// <summary>
        ///  Verify the data type FileStandardInformation, which is defined in TD 2.4.38
        /// </summary>
        /// <param name="fileStandardInformation"> FileStandardInformation type data</param>
        /// <param name="isFileDeletionRequested"> Check if FileDeletion Requested</param>
        /// <param name="isDirector"> Check if is a Director</param>
        public void VerifyDataTypeFileStandardInformation(
            FileStandardInformation fileStandardInformation,
            bool isFileDeletionRequested,
            bool isDirector)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1624, AllocationSize: {0};",
                fileStandardInformation.AllocationSize);
            //
            // Verify MS-FSCC requirement 1624
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Int64),
                fileStandardInformation.AllocationSize.GetType(),
                1624,
                @"[In FILE_STANDARD_INFORMATION]AllocationSize (8 bytes):  A 64-bit signed integer that contains 
                the file allocation size in bytes.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1626, AllocationSize: {0};",
                fileStandardInformation.AllocationSize);
            //
            // Verify MS-FSCC requirement 1626
            //
            // And the size of the AllocationSize is verified above, we didn't verify here again
            Site.CaptureRequirementIfIsTrue(
                (fileStandardInformation.AllocationSize >= 0),
                 1626,
                @"[In FILE_STANDARD_INFORMATION]AllocationSize (8 bytes):  The value of this field MUST be greater than or equal to 0.");
           
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1628, EndOfFile: {0}", fileStandardInformation.EndOfFile);

            const int sizeOfEndOfFile = 8;
            //
            // Verify MS-FSCC requirement 1628
            //
            // As we parse the file by using the EndofFile, the sucessfully parsing have verified the actual value, 
            // only verify the size of endoffile
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfEndOfFile,
                Marshal.SizeOf(fileStandardInformation.EndOfFile),
                1628,
                @"[In FILE_STANDARD_INFORMATION]EndOfFile (8 bytes):  EndOfFile specifies the offset to the byte 
                immediately following the last valid byte in the file.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1629, EndOfFile: {0}", fileStandardInformation.EndOfFile);

            //
            // Verify MS-FSCC requirement 1629
            //
            // As we parse the file by using the EndofFile, it's useless to verify the actual value if is valid
            // after parsing successfully, only verify the size of endoffile
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfEndOfFile,
                Marshal.SizeOf(fileStandardInformation.EndOfFile),
                1629,
                @"[In FILE_STANDARD_INFORMATION]EndOfFile (8 bytes):  Because this value is zero-based, it actually 
                refers to the first free byte in the file. That is, it is the offset from the beginning of the file 
                at which new bytes appended to the file will be written.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1637, Reserved: {0}", fileStandardInformation.Reserved);
            //
            // Verify MS-FSCC requirement 1637
            //
            // As the reserved field is reserved, it can be any value, this rs will be only verified with size
            const int sizeOfReserved = 2;
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfReserved,
                fileStandardInformation.Reserved.Length,
                1637,
                @"[In FILE_STANDARD_INFORMATION]Reserved (2 bytes): This field is reserved.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1630 EndOfFile: {0};",
                fileStandardInformation.EndOfFile);
            //
            // Verify MS-FSCC requirement 1630
            //
            // As the size of the EndOfFile is verified above, we didn't verify again here.
            Site.CaptureRequirementIfIsTrue(
                (fileStandardInformation.EndOfFile >= 0),
                1630,
                @"[In FILE_STANDARD_INFORMATION]EndOfFile (8 bytes):  The value of this field MUST be greater than 
                or equal to 0.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1631 ");
            //
            // Verify MS-FSCC requirement 1631
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(UInt32),
                fileStandardInformation.NumberOfLinks.GetType(),
                1631,
                @"[In FILE_STANDARD_INFORMATION]NumberOfLinks (4 bytes):  A 32-bit unsigned integer that contains the 
                number of non-deleted links to this file.");

            if (isFileDeletionRequested)
            {
                //
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1632 DeletePending: {0};",
                    fileStandardInformation.DeletePending);
                //
                // Verify MS-FSCC requirement 1632
                //
                bool isVerifyR1632 = (Marshal.SizeOf(fileStandardInformation.DeletePending) == 1 &&
                                         fileStandardInformation.DeletePending == 1);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1632,
                    1632,
                    @"[In FILE_STANDARD_INFORMATION]DeletePending (1 byte):  An 8-bit field that MUST be set to 1 to 
                     indicate that a file deletion has been requested.");
            }
            else
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1633 DeletePending: {0};",
                    fileStandardInformation.DeletePending);
                //
                // Verify MS-FSCC requirement 1633
                //
                bool isVerifyR1633 = (Marshal.SizeOf(fileStandardInformation.DeletePending) == 1 &&
                                         fileStandardInformation.DeletePending == 0);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1633,
                    1633,
                    @"[In FILE_STANDARD_INFORMATION]DeletePending (1 byte):   otherwise[if a file deletion has not been 
                    requested],[it is set to] 0");
            }

            if (isDirector)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1634, Directory: {0};",
                    fileStandardInformation.Directory);
                //
                // Verify MS-FSCC requirement 1634
                //
                bool isVerifyR1634 = (Marshal.SizeOf(fileStandardInformation.Directory) == 1 &&
                                         fileStandardInformation.Directory == 1);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1634,
                    1634,
                    @"[In FILE_STANDARD_INFORMATION]Directory (1 byte):  An 8-bit field that MUST be set to 1 to indicate 
                    that the file is a director.");
            }
            else
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1635 Directory: {0};",
                    fileStandardInformation.Directory);

                bool isVerifyR1635 = (Marshal.SizeOf(fileStandardInformation.Directory) == 1 &&
                                        fileStandardInformation.Directory == 0);
                //
                // Verify MS-FSCC requirement 1635
                //
                // And the size of the Directory is verified above, we didn't verify here again
                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1635,
                    1635,
                    @"[In FILE_STANDARD_INFORMATION]Directory (1 byte):  otherwise[if the file is not a director], 
                    [it is set to]0.");
            }
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1636 , Reserved: {0};",
                BytesToString(fileStandardInformation.Reserved));
            //
            // Verify MS-FSCC requirement 1636
            //
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfReserved,
                fileStandardInformation.Reserved.Length,
                1636,
                @"[In FILE_STANDARD_INFORMATION]Reserved (2 bytes):  A 16-bit field.");

            // As the Reserved can be set to any value. we can capture this req1638 directly and the size here won't be verified again.
            Site.CaptureRequirement(
                1638,
                @"[In FILE_STANDARD_INFORMATION]Reserved (2 bytes): This field can be set to any value.");
            // FILE_STANDARD_INFORMATION has been defined as section 2.4.38
            // As all the elements in the FILE_STANDARD_INFORMATION have been verified above
            // This rs wii be captured directly
            Site.CaptureRequirement(
                979,
                @"[In FileAllInformation]StandardInformation (24 bytes):  
                A FILE_STANDARD_INFORMATION structure specified in section 2.4.38.");
            // As all the elements in the FILE_STANDARD_INFORMATION have been verified above
            // This rs wii be captured directly
            Site.CaptureRequirement(
                1623,
                @"[In FileStandardInformation]The FILE_STANDARD_INFORMATION data element is as follows:
                [AllocationSize, ..., EndOfFile, ..., NumberOfLinks, DeletePending, Directory, Reserved].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify data  type FileInternalInformation

        /// <summary>
        /// Verify the data type FileInternalInformation, which is defined in TD 2.4.20
        /// </summary>
        /// <param name="fileInternalInformationm"> FileInternalInformation type data</param>
        /// <param name="isSupportFileReferenceNum"> verify whether the server support FileReferenceNum or not</param>
        public void VerifyDataTypeFileInternalInformation(
            FileInternalInformation fileInternalInformationm,
            bool isSupportFileReferenceNum)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1362 ");
            //
            // Verify MS-FSCC requirement 1362
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Int64),
                fileInternalInformationm.IndexNumber.GetType(),
                1362,
                @"[In FILE_INTERNAL_INFORMATION]IndexNumber (8 bytes):  A 64-bit signed integer that contains the 8-byte 
                file reference number for the file.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1363 IndexNumber is : {0}",
                fileInternalInformationm.IndexNumber);
            //
            // Verify MS-FSCC requirement 1363
            //
            // As "unique" here is unable for us to verify
            // We only verify the type of IndexNumber here
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Int64),
                fileInternalInformationm.IndexNumber.GetType(),
                1363,
                @"[In FILE_INTERNAL_INFORMATION]IndexNumber (8 bytes): 
                This number MUST be assigned by the file system and is unique to the volume on which 
                the file or directory is located.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1365 IndexNumber: {0};",
                fileInternalInformationm.IndexNumber);
            //
            // Verify MS-FSCC requirement 1365
            //
            // as the size of the IndexNumber is verified above, we didn't verify the size again.
            Site.CaptureRequirementIfIsTrue(
                (fileInternalInformationm.IndexNumber >= 0),
                1365,
                @"[In FILE_INTERNAL_INFORMATION]IndexNumber (8 bytes): The value of this field MUST be greater than or equal to 0.");

            if (!isSupportFileReferenceNum)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1366 , IndexNumber: {0};",
                    fileInternalInformationm.IndexNumber);
                //
                // Verify MS-FSCC requirement 1366
                //
                // as the size of IndexNumber is verified above, we didn't verify the size again.
                Site.CaptureRequirementIfIsTrue(
                    (fileInternalInformationm.IndexNumber == 0),
                    1366,
                    @"[In FILE_INTERNAL_INFORMATION]IndexNumber (8 bytes):For file systems which do not support a file 
                    reference number, this field MUST be set to 0.");
            }
            // FILE_INTERNAL_INFORMATION is definned as section 2.4.20 in the sdk,
            // As all the elements in the FILE_INTERNAL_INFORMATION have been verified above
            // This re will be captured directly
            Site.CaptureRequirement(
                980,
                @"[In FileAllInformation]InternalInformation (8 bytes):  
                A FILE_INTERNAL_INFORMATION structure specified in section 2.4.20.");

            // As all the elements in the FILE_INTERNAL_INFORMATION have been verified above
            // This re will be captured directly
            Site.CaptureRequirement(
                1361,
                @"[In FileInternalInformation]The FILE_INTERNAL_INFORMATION data element is as follows:[IndexNumber, ...].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileAccessInformation

        /// <summary>
        /// Verify the data type FILE_ACCESS_INFORMATION 
        /// </summary>
        /// <param name="fileAccessInformation"> FILE_ACCESS_INFORMATION type data </param>
        public void VerifyDataTypeFileAccessInformation(
            FILE_ACCESS_INFORMATION fileAccessInformation)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "MS-FSCC_R976,AccessFlags is {0}", fileAccessInformation.AccessFlags);

            //
            // Verify requirement 976
            //
            Site.CaptureRequirementIfAreEqual<int>(
                4 ,
                Marshal.SizeOf(fileAccessInformation.AccessFlags),
                976,
                @"[In FILE_ACCESS_INFORMATION]AccessFlags (4 bytes):  A DWORD that MUST contain values specified in 
                ACCESS_MASK of [MS-DTYP].");

            // As the FILE_ACCESS_INFORMATION only contains the AccessFlags elements
            // And the AccessFlags is verified above, this rs can be captured directly
            Site.CaptureRequirement(
                975,
                @"The FILE_ACCESS_INFORMATION data element is as follows:[AccessFlags].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R982");

            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(FILE_ACCESS_INFORMATION),
                fileAccessInformation.GetType(),
                982,
                @"[In FileAllInformation]AccessInformation (4 bytes):  
                A FILE_ACCESS_INFORMATION structure specified in section 2.4.1.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileNameInformation

        /// <summary>
        /// Verify the data type FileNameInformation
        /// </summary>
        /// <param name="fileNameInformation"> FileNameInformation type data </param>
        public void VerifyDataTypeFileNameInformation(
            FileNameInformation fileNameInformation)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1413 Actual FileNameLength: {0}, Expected file name length: {1};",
                fileNameInformation.FileNameLength, fileNameInformation.FileName.Length);
            //
            // Verify MS-FSCC requirement 1413
            //
            // Verify the type of the filenamelength and its content
            bool isVerifyR1413 = ((uint)fileNameInformation.FileName.Length == fileNameInformation.FileNameLength &&
                                     fileNameInformation.FileNameLength.GetType() == typeof(UInt32));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1413,
                1413,
                @"[In FILE_NAME_INFORMATION]FileNameLength (4 bytes):  A 32-bit unsigned integer that contains 
                the length, in bytes, of the FileName field.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1414 Actual FileName: {0}",
                fileNameInformation.FileName);
            //
            // Verify MS-FSCC requirement 1414
            //
            bool isVerifyR1414 = true;

            for (int index = 0; index < fileNameInformation.FileName.Length - 1; index++)
            {
                if (index % 2 != 0)
                {
                    if (fileNameInformation.FileName[index] != 0)
                    {
                        isVerifyR1414 = false;
                        break;
                    }
                }
            }

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1414,
                1414,
                @"[In FILE_NAME_INFORMATION]FileName (variable):  A sequence of Unicode characters containing the file name.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1428 ");
            //
            // Verify MS-FSCC requirement 1428
            //
            // We only verify the FileName contains FileNameLength bytes

            Site.CaptureRequirementIfAreEqual<int>(
                fileNameInformation.FileName.Length,
                (int)fileNameInformation.FileNameLength,
                1428,
                @"[In FILE_NAME_INFORMATION]FileName (variable):  This field MUST be handled as a sequence of FileNameLength bytes.");

            // As all the elements in the FILE_NAME_INFORMATION are verified above,
            // This rs will be captured directly
            Site.CaptureRequirement(
                1412,
                @"[In FileNameInformation]The FILE_NAME_INFORMATION data element is as follows:[FileNameLength, 
                FileName (variable)].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileModeInformation

        /// <summary>
        /// Verify data type FILE_MODE_INFORMATION
        /// </summary>
        /// <param name="fileModeInformation"> FILE_MODE_INFORMATION type data </param>
        public void VerifyDataTypeFileModeInformation(
            FILE_MODE_INFORMATION fileModeInformation)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1402 the Mode is : {0};",
                fileModeInformation.Mode);

            //
            // Verify MS-FSCC requirement 1402
            //
            // as we cannot know how the mode specify on file creation or file open how the file will subsequently 
            // be accessed from this rs, we only verify the type here

            Site.CaptureRequirementIfAreEqual<Type>(
                 typeof(Mode_Values),
                 fileModeInformation.Mode.GetType(),
                 1402,
                 @"[In FILE_MODE_INFORMATION]Mode (4 bytes):  A ULONG that MUST specify on file creation or 
                 file open how the file will subsequently be accessed");

         
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,"Verify MS-FSCC_R984");
            //
            // Verify requirement 984
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(FILE_MODE_INFORMATION),
                fileModeInformation.GetType(),
                984,
                @"[In FileAllInformation]ModeInformation (4 bytes):  
                A FILE_MODE_INFORMATION structure specified in section 2.4.24.");

            // As all the elements in the FILE_MODE_INFORMATION have been verified above
            // This rs will be captured directly
            Site.CaptureRequirement(
                1401,
                @"[In FileModeInformation]The FILE_MODE_INFORMATION data element is as follows:[Mode].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify date type FILE_ALIGNMENT_INFORMATIO

        /// <summary>
        /// Verify date type FILE_ALIGNMENT_INFORMATIO
        /// </summary>
        /// <param name="FILE_ALIGNMENT_INFORMATIONData"> FILE_ALIGNMENT_INFORMATIO type data </param>
        /// <param name="dataLength"> Actual data length, used to verify the alignment</param>
        public void VerifyDataTypeFileAlignmentInformation(
            FILE_ALIGNMENT_INFORMATION fileAlignmentInformationData,
            int dataLength)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R989, Actual AlignmentRequirement: {0}",
                 fileAlignmentInformationData.AlignmentRequirement);
            // 
            // Verify MS-FSCC requirement 989
            //
            // Verfiy the type and the value of the AlignmentRequirement
            bool isVerifyR989 = (
                fileAlignmentInformationData.AlignmentRequirement.GetType() == typeof(AlignmentRequirement_Values) &&
               (fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_128_BYTE_ALIGNMENT ||
                fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_256_BYTE_ALIGNMENT ||
                fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_32_BYTE_ALIGNMENT ||
                fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_512_BYTE_ALIGNMENT ||
                fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_64_BYTE_ALIGNMENT ||
                fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_BYTE_ALIGNMENT ||
                fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_LONG_ALIGNMENT ||
                fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_OCTA_ALIGNMENT ||
                fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_QUAD_ALIGNMENT ||
                fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_WORD_ALIGNMENT));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR989,
                989,
                @"[In FILE_ALIGNMENT_INFORMATIO]AlignmentRequirement (4 bytes):  
                A 32-bit unsigned integer that MUST contain one of the following values
                [FILE_BYTE_ALIGNMENT 0x00000000, FILE_WORD_ALIGNMENT 0x00000001,
                 FILE_LONG_ALIGNMENT 0x00000003, FILE_QUAD_ALIGNMENT 0x00000007, 
                 FILE_OCTA_ALIGNMENT 0x0000000f, FILE_32_BYTE_ALIGNMENT 0x0000001f, 
                 FILE_64_BYTE_ALIGNMENT 0x0000003f, FILE_128_BYTE_ALIGNMENT 0x0000007f, 
                 FILE_256_BYTE_ALIGNMENT 0x000000ff, FILE_512_BYTE_ALIGNMENT 0x000001ff].");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R985, Actual AlignmentInformation type: {0}, Expected AlignmentInformation type: {1}.",
                fileAlignmentInformationData.GetType().Name, typeof(FILE_ALIGNMENT_INFORMATION));
            //
            // Verify requirement 985
            //
            // Verify type of the AlignmentInformation specified in section 2.4.3
            int sizeOfAlignmentInformation = 4;

            bool isVerifyR985 = (
                Marshal.SizeOf(fileAlignmentInformationData) == sizeOfAlignmentInformation &&
                fileAlignmentInformationData.GetType() == typeof(FILE_ALIGNMENT_INFORMATION));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR985,
                985,
                @"[In FileAllInformation]AlignmentInformation (4 bytes):  
                A FILE_ALIGNMENT_INFORMATION structure specified in section 2.4.3.");

            const int sizeOfAlignmentRequirement = 4;
            if (fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_WORD_ALIGNMENT)
            {
                //
                // Add the debug information.
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R991, DataLength: {0}", dataLength);
                
                //
                // Verify requirement 991
                //
                // Verify the size and if the actual data length aligned on a 2-byte boundary
                Site.CaptureRequirementIfIsTrue(
                     dataLength % 2 == 0,
                     991,
                     @"[In FILE_ALIGNMENT_INFORMATIO]AlignmentRequirement (4 bytes):  If this value
                     [FILE_WORD_ALIGNMENT 0x00000001] is specified, data MUST be aligned on a 2-byte boundary.");
            }

            if (fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_LONG_ALIGNMENT)
            {
               
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R992,  DataLength: {0}", dataLength);
                //
                // Verify requirement 992
                //
                Site.CaptureRequirementIfIsTrue(
                     dataLength % 4 == 0,
                     992,
                     @"[In FILE_ALIGNMENT_INFORMATIO]AlignmentRequirement (4 bytes):  If this value
                     [FILE_LONG_ALIGNMENT 0x00000003] is specified, data MUST be aligned on a 4-byte boundary.");
            }

            if (fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_QUAD_ALIGNMENT)
            {
               
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R993, DataLength: {0}", dataLength);
                //
                // Verify requirement 993
                //
                // Verify the size and if the actual data length aligned on a 8-byte boundary
                Site.CaptureRequirementIfIsTrue(
                    dataLength % 8 == 0,
                    993,
                    @"[In FILE_ALIGNMENT_INFORMATIO]AlignmentRequirement (4 bytes):  If this value
                    [FILE_QUAD_ALIGNMENT 0x00000007] is specified, data MUST be aligned on an 8-byte boundary.");
            }

            if (fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_OCTA_ALIGNMENT)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R994, DataLength: {0}", dataLength);
                //
                // Verify requirement 994
                //
                // Verify the size and if the actual data length aligned on a 16-byte boundary
                Site.CaptureRequirementIfIsTrue(
                     dataLength % 16 == 0,
                     994,
                     @"[In FILE_ALIGNMENT_INFORMATIO]AlignmentRequirement (4 bytes):  If this value
                     [FILE_OCTA_ALIGNMENT 0x0000000f] is specified, data MUST be aligned on a 16-byte boundary.");
            }

            if (fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_32_BYTE_ALIGNMENT)
            {
               
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R995, DataLength: {0}", dataLength);
                //
                // Verify requirement 995
                //
                // Verify the size and if the actual data length aligned on a 32-byte boundary
                Site.CaptureRequirementIfIsTrue(
                    dataLength % 32 == 0,
                    995,
                    @"[In FILE_ALIGNMENT_INFORMATIO]AlignmentRequirement (4 bytes):  If this value
                    [FILE_32_BYTE_ALIGNMENT 0x0000001f] is specified, data MUST be aligned on a 32-byte boundary.");
            }

            if (fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_64_BYTE_ALIGNMENT)
            {
                
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R996, DataLength: {0}", dataLength);
                //
                // Verify requirement 996
                //
                // Verify the size and if the actual data length aligned on a 64-byte boundary
                Site.CaptureRequirementIfIsTrue(
                    dataLength % 64 == 0,
                    996,
                    @"[In FILE_ALIGNMENT_INFORMATIO]AlignmentRequirement (4 bytes):  If this value
                    [FILE_64_BYTE_ALIGNMENT 0x0000003f] is specified, data MUST be aligned on a 64-byte boundary.");
            }

            if (fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_128_BYTE_ALIGNMENT)
            {
                
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R997, DataLength: {0}", dataLength);
                //
                // Verify requirement 997
                //
                // Verify the size and if the actual data length aligned on a 128-byte boundary
               Site.CaptureRequirementIfIsTrue(
                     dataLength % 128 == 0,
                     997,
                     @"[In FILE_ALIGNMENT_INFORMATIO]AlignmentRequirement (4 bytes):  If this value
                     [FILE_128_BYTE_ALIGNMENT 0x0000007f] is specified, data MUST be aligned on a 128-byte boundary.");
            }

            if (fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_256_BYTE_ALIGNMENT)
            {
               
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R998, DataLength: {0}", dataLength);
                //
                // Verify requirement 998
                //
                // Verify the size and if the actual data length aligned on a 256-byte boundary
                Site.CaptureRequirementIfIsTrue(
                    dataLength % 256 == 0,
                    998,
                    @"[In FILE_ALIGNMENT_INFORMATIO]AlignmentRequirement (4 bytes):  If this value
                    [FILE_256_BYTE_ALIGNMENT 0x000000ff] is specified, data MUST be aligned on a 256-byte boundary.");
            }

            if (fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_512_BYTE_ALIGNMENT)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R999, DataLength: {0}", dataLength);
                //
                // Verify requirement 999
                //
                // Verify the size and if the actual data length aligned on a 512-byte boundary
                Site.CaptureRequirementIfIsTrue(
                    dataLength % 512 == 0,
                    999,
                    @"[In FILE_ALIGNMENT_INFORMATIO]AlignmentRequirement (4 bytes):  If this value
                    [FILE_512_BYTE_ALIGNMENT 0x000001ff] is specified, data MUST be aligned on a 512-byte boundary.");
            }

            //As the elements in the structure are verified above, this rs will be captured directly
            Site.CaptureRequirement(
                988,
                @"[In FileAlignmentInformation] The FILE_ALIGNMENT_INFORMATION data element is as follows:
                [AlignmentRequirement].");

            if (fileAlignmentInformationData.AlignmentRequirement == AlignmentRequirement_Values.FILE_BYTE_ALIGNMENT)
            {
                //
                // Verify requirement 990
                //

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R990");

                // If there are no alignment requirements for the device, the dateLenth of the device can be
                // any valid value without any alignment requirements.So this rs will be only covered the sizeof the AlignmentRequirement.

                Site.CaptureRequirementIfAreEqual<int>(
                    sizeOfAlignmentRequirement,
                    Marshal.SizeOf(fileAlignmentInformationData.AlignmentRequirement),
                    990,
                    @"[In FILE_ALIGNMENT_INFORMATIO]AlignmentRequirement (4 bytes):  If this value
                    [FILE_BYTE_ALIGNMENT 0x00000000] is specified, there are no alignment requirements for the 
                    device.");
            }

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileAlternateNameInformation

        /// <summary>
        /// Verify data type FileAlternateNameInformation
        /// </summary>
        /// <param name="fileAlternateNameInformation"> FileAlternateNameInformation type data </param>
        public void VerifyDataTypeFileAlternateNameInformation(
            FileAlternateNameInformation fileAlternateNameInformation)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1007, Actual FileNameLength: {0}, Expected FileNameLength: {1}.",
                 fileAlternateNameInformation.FileNameLength, fileAlternateNameInformation.FileName.Length);

            // Verify if the alternate name is in the format of 8.3
            string fileNameStr = BytesToString(fileAlternateNameInformation.FileName);

            // Get the index of the '.'
            int index = fileNameStr.IndexOf('.');
            if (index >= 0)
            {
                // begin2Dot: the length of the characters before '.'
                int begin2Dot = index;
                // Dot2End: the length of the characters after '.'
                int dot2End = fileNameStr.Length - index;
                //
                // Verify requirement 1007
                //
                bool isVerifyR1007 = (begin2Dot <= 8 && dot2End <= 3);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1007,
                    1007,
                    @"[In FileAlternateNameInformation]The alternate name for a file is its 8.3 format name 
                    (eight characters that appear before the '.' and three characters that appear after).");
            }

            
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1013, Actual FileName is : {0}",
                BytesToString(fileAlternateNameInformation.FileName));
            //
            // Verify requirement 1013
            //
            // We verify the FileName contains FileNameLength bytes .
            Site.CaptureRequirementIfAreEqual<int>(
                (int)fileAlternateNameInformation.FileNameLength,
                fileAlternateNameInformation.FileName.Length,
                1013,
                @"[In FILE_NAME_INFORMATION]FileName (variable): This field MUST be handled as a sequence of 
                FileNameLength bytes.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1010, Actual FileNameLength: {0}, Expected FileNameLength: {1}.",
                fileAlternateNameInformation.FileNameLength, fileAlternateNameInformation.FileName.Length);

            //
            // Verify requirement 1010
            //
            // Verify type and value of the FileNameLength
            bool isVerifyR1010 = (
                fileAlternateNameInformation.FileNameLength.GetType() == typeof(UInt32) &&
                fileAlternateNameInformation.FileNameLength == fileAlternateNameInformation.FileName.Length);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1010,
                1010,
                @"[In FILE_NAME_INFORMATION]FileNameLength (4 bytes):  
                A 32-bit unsigned integer that contains the length in bytes of the FileName member.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1011");
            //
            // Verify requirement 1011
            //
            Site.CaptureRequirementIfIsNotNull(
                fileAlternateNameInformation.FileName,
                1011,
                @"[In FILE_NAME_INFORMATION]FileName (variable):  
                A sequence of Unicode characters containing the file name.");

            // Verify req 1008, 1922
            string isR1008Implemented = Site.Properties["isR1008Implemented"];

            if (isWindows)
            {
                    //
                    // Add the debug information
                    //
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1922");
                    //
                    // Verify requirement 1922
                    //
                    // This rs will be only verified if the alternate name is not null when the file system 
                    // is NTFS system.If it is not null, that indicates the NTFS assigns an alternate name to a file
                    Site.CaptureRequirementIfIsNotNull(
                        fileAlternateNameInformation.FileName,
                        1922,
                        @"<53> Section 2.4.5: NTFS assigns an alternate name to a file whose full name is not 
                        compliant with restrictions for file names under MS-DOS and 16-bit Windows unless the system
                        has been configured through a registry entry to not generate these names to improve 
                        performance.");

                if (isR1008Implemented == null)
                {
                    Site.Properties.Add("isR1008Implemented", Boolean.TrueString);
                    isR1008Implemented = Boolean.TrueString;
                }
            }
            if (isR1008Implemented != null)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1922");

                bool isImplemented = Boolean.Parse(isR1008Implemented);
                bool isVerified_r1008 = (fileAlternateNameInformation.FileName != null);

                // And as the format of the filename is verified above, this rs will not be verified if the 
                // filename is in the format of 8.3 format.
                Site.CaptureRequirementIfAreEqual<Boolean>(
                    isImplemented,
                    isVerified_r1008,
                    1008,
                    string.Format(@"[In FileAlternateNameInformation]A file MAY have an alternate name to 
                    achieve compatibility with the 8.3 naming requirements of legacy applications. <53> This req
                    is {0} implemented", isImplemented ? "" : "not"));
            }
            // As all the elements in the FILE_NAME_INFORMATION are verified above
            // This rs will be captured directly
            Site.CaptureRequirement(
                1009,
                @"[In FileAlternateNameInformation]The FILE_NAME_INFORMATION data element is as follows:
                [FileNameLength, FileName (variable)].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileStreamInformation

        /// <summary>
        /// Verify data type FileStreamInformation
        /// </summary>
        /// <param name="fileStreamInformation"> FileStreamInformation type data </param>
        /// <param name="isMultipleEntriesPresent"> Check if multiple entries are present </param>
        /// <param name="isNoOtherEntriesFollow"> Check if no other entries follow this one[FILE_ STREAM _INFORMATION entry].</param>
        [SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength")] 
        public void VerifyDataTypeFileStreamInformation(
            FileStreamInformation fileStreamInformation,
            bool isMultipleEntriesPresent,
            bool isNoOtherEntriesFollow)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";

            // if multiple entries are present
            if (isMultipleEntriesPresent)
            {
                
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1659 ,the NextEntryOffset is : {0}.",
                    fileStreamInformation.NextEntryOffset);
                //
                // Verify requirement 1659
                //
                // Verify the type and the value of the NextEntryOffset
                // As it's unable for us to verify the NextEntryOffset, we only check if the value is in the valid range.
                bool isVerifyR1659 = (
                    fileStreamInformation.NextEntryOffset.GetType() == typeof(UInt32) &&
                    fileStreamInformation.NextEntryOffset <= fileStreamInformation.StreamAllocationSize);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1659,
                    1659,
                    @"[In FILE_STREAM_INFORMATION]NextEntryOffset (4 bytes):  
                    A 32-bit unsigned integer that contains the byte offset from the beginning of this entry, 
                    at which the next FILE_ STREAM _INFORMATION entry is located, if multiple entries are present 
                    in a buffer.");
            }

            //if no other entries follow this one[FILE_ STREAM _INFORMATION entry].
            if (isNoOtherEntriesFollow)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1660");
                //
                // Verify requirement 1660
                //
                // Verify the value of NextEntryOffset
                Site.CaptureRequirementIfAreEqual<uint>(
                    0,
                    fileStreamInformation.NextEntryOffset,
                    1660,
                    @"[In FILE_STREAM_INFORMATION]NextEntryOffset (4 bytes): This member is zero if no other entries 
                    follow this one[FILE_ STREAM _INFORMATION entry].");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1663 Actual StreamNameLength: {0}, Expected StreamNameLength: {1}.",
                fileStreamInformation.StreamNameLength, fileStreamInformation.StreamName.Length);

            //
            // Verify requirement 1663
            //
            // Verify the type and the value of the StreamNameLength
            bool isVerified_r1663 = (
                fileStreamInformation.StreamNameLength.GetType() == typeof(UInt32) &&
                fileStreamInformation.StreamNameLength == fileStreamInformation.StreamName.Length);

            Site.CaptureRequirementIfIsTrue(
                isVerified_r1663,
                1663,
                @"[In FILE_STREAM_INFORMATION]StreamNameLength (4 bytes):  
                A 32-bit unsigned integer that contains the length, in bytes, of the stream name string.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1664 ");
            //
            // Verify requirement 1664
            //
            // Verify the type and the value of the StreamSize
            // As it's unable for us to verify the StreamSize, we didn't verify here.

            Site.CaptureRequirementIfAreEqual<Type>(
                 typeof(UInt64),
                 fileStreamInformation.StreamSize.GetType(),
                 1664,
                 @"[In FILE_STREAM_INFORMATION]StreamSize (8 bytes):  A 64-bit signed integer that contains the size, 
                 in bytes, of the stream.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1665 StreamSize is : {0}",
                fileStreamInformation.StreamSize);
            //
            // Verify requirement 1665
            //
            // As the type is verified above, we didn't verify here again
            Site.CaptureRequirementIfIsTrue(
                fileStreamInformation.StreamSize >= 0,
                1665,
                @"[In FILE_STREAM_INFORMATION]StreamSize (8 bytes):  The value of this field MUST be greater than 
                or equal to 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1666");

            //
            // Verify requirement 1666
            //
            // Verify the type and the value of the StreamAllocationSize
            // As it's unable for us to verify if the StreamAllocationSize is valid, we only verify the type here
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Int64),
                fileStreamInformation.StreamAllocationSize.GetType(),
                1666,
                @"[In FILE_STREAM_INFORMATION]StreamAllocationSize (8 bytes):  A 64-bit signed integer that 
                contains the file stream allocation size in bytes.");

         
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1668 Actual StreamAllocationSize: {0}",
                fileStreamInformation.StreamAllocationSize);
            //
            // Verify requirement 1668
            //
            // Verify the value of the StreamAllocationSize
            Site.CaptureRequirementIfIsTrue(
                (fileStreamInformation.StreamAllocationSize >= 0),
                1668,
                @"[In FILE_STREAM_INFORMATION]StreamAllocationSize (8 bytes):  The value of this field MUST be 
                greater than or equal to 0.");

            // Since this rs is informative, we capture it directly
            Site.CaptureRequirement(
                1670,
                @"[In FILE_STREAM_INFORMATION]StreamName (variable):  
                The :$DATA string that follows streamname is an internal data type tag that is 
                unintentionally exposed via this info class.");

            // Since the structure of the StreamName is verified when capturing the 1671
            // We capture this ts directly
            Site.CaptureRequirement(
                1672,
                @"[In FILE_STREAM_INFORMATION]StreamName (variable):  
                The leading ¡®:¡¯ and trailing ¡®:$DATA¡¯ characters MUST be stripped from this field to 
                derive the actual stream name.");

            string defaultStream = BytesToString(fileStreamInformation.StreamName);
            // if the stream name is empty
            if (defaultStream == String.Empty)
            {
                // Since the default StreamName is defined by system
                // We capture this ts directly if the streamname is empty.
                Site.CaptureRequirement(
                    1673,
                    @"[In FILE_STREAM_INFORMATION]StreamName (variable):  
                    A resulting empty string for the stream name denotes the default stream.");
            }

           
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1714");
            //
            // Verify requirement 1714
            //
            // According to the rs, this field MUST be handled as a sequence of StreamNameLength bytes.
            // We verify the length of the StreamName
            Site.CaptureRequirementIfAreEqual<int>(
                 fileStreamInformation.StreamName.Length ,
                 (int)fileStreamInformation.StreamNameLength,
                 1714,
                 @"[In FILE_STREAM_INFORMATION]StreamName (variable):  
                 This field MUST be handled as a sequence of StreamNameLength bytes.");
            // As the portions of a buffer that are within a FILE_STREAM_INFORMATION is verified above
            // We capture this rs directly as the data element not in a FILE_STREAM_INFORMATION can be set to any value
            Site.CaptureRequirement(
                1656,
                @"[In FileStreamInformation]Any portions of a buffer that are not within a FILE_STREAM_INFORMATION 
                data element can contain any value.");
            // As all the elements in the FILE_STREAM_INFORMATION have been verified above 
            // This rs will be captured directly
            Site.CaptureRequirement(
                1658,
                @"[In FileStreamInformation] The FILE_STREAM_INFORMATION data element is as follows:[NextEntryOffset,
                StreamNameLength, StreamSize, ..., StreamAllocationSize, ..., StreamName (variable)].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify data type FileCompressionInformation

        /// <summary>
        /// Verify data type FileCompressionInformation
        /// </summary>
        /// <param name="fileCompressionInfomation"> FileCompressionInformation type data </param>
        /// <param name="compressedFileSize"> A 64-bit signed integer that contains the size, in bytes, of the compressed file. </param>
        /// <param name="compressionFormat"> A 16-bit unsigned integer that contains the compression format. </param>
        /// <param name="compressionUnitShift"> An 8-bit unsigned integer that contains the compression unit shift, which is the number of bits by which to left-shift a 1 bit to arrive at the compression unit size. </param>
        /// <param name="chunkShift"> An 8-bit unsigned integer that contains the compression chunk size in bytes in log 2 format. </param>
        /// <param name="clusterShift"> An 8-bit unsigned integer that specifies, in log 2 format, the amount of space that must be saved by compression to successfully compress a compression unit. </param>
        /// <param name="clusterSize">  size of the cluster </param>
        public void VerifyDataTypeFileCompressionInformationForOld(
            FileCompressionInformation fileCompressionInfomation,
            int  compressionFormat,
            byte chunkShift,
            byte clusterShift,
            byte clusterSize)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1094 Actual CompressedFileSize is : {0}",
                fileCompressionInfomation.CompressedFileSize);
            //
            // Verify requirement 1094
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Int64),
                fileCompressionInfomation.CompressedFileSize.GetType(),
                1094,
                @"[In FILE_COMPRESSION_INFORMATION]CompressedFileSize (8 bytes):  
                A 64-bit signed integer that contains the size, in bytes, of the compressed file.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1095 Actual CompressedFileSize: {0}",
                fileCompressionInfomation.CompressedFileSize);

            //
            // Verify requirement 1095
            //
            Site.CaptureRequirementIfIsTrue(
                fileCompressionInfomation.CompressedFileSize >= 0,
                1095,
                @"[In FILE_COMPRESSION_INFORMATION]CompressedFileSize (8 bytes): 
                This value MUST be greater than or equal to 0.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1096 Actual CompressionFormat: {0}, Expected CompressionFormat: {1}.",
                fileCompressionInfomation.CompressionFormat, compressionFormat);
            //
            // Verify requirement 1096
            //
            // Verify type and value of the CompressionFormat
            bool isVerifyR1096 = (
                fileCompressionInfomation.CompressionFormat.GetType() == typeof(CompressionFormat_Values) &&
                (uint)fileCompressionInfomation.CompressionFormat == compressionFormat);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1096,
                1096,
                @"[In FILE_COMPRESSION_INFORMATION]CompressionFormat (2 bytes):  
                A 16-bit unsigned integer that contains the compression format.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "MS-FSCC_R1098, CompressionFormat: {0}",
                 fileCompressionInfomation.CompressionFormat);

            //
            // Verify requirement 1098
            //
            bool isVerifyR1098 = (
                (fileCompressionInfomation.CompressionFormat.GetType() == typeof(CompressionFormat_Values)) &&
                ((fileCompressionInfomation.CompressionFormat == CompressionFormat_Values.COMPRESSION_FORMAT_DEFAULT) ||
                (fileCompressionInfomation.CompressionFormat == CompressionFormat_Values.COMPRESSION_FORMAT_LZNT1) ||
                (fileCompressionInfomation.CompressionFormat == CompressionFormat_Values.COMPRESSION_FORMAT_NONE)));


            Site.CaptureRequirementIfIsTrue(
                isVerifyR1098,
                1098,
                @"[In FILE_COMPRESSION_INFORMATION]CompressionFormat (2 bytes): An implementation can associate any 
                local compression algorithm with the values described in the following table[COMPRESSION_FORMAT_NONE 
                0x0000, COMPRESSION_FORMAT_DEFAULT 0x0001, COMPRESSION_FORMAT_LZNT1 0x0002, All other values] because
                the compressed data does not travel across the wire in the context of FSCTL, FileInformation class, 
                or FileSystemInformation class requests or replies.<58>");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1103 Actual CompressionUnitShift: {0}",
                fileCompressionInfomation.CompressionUnitShift);
            //
            // Verify requirement 1103
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(byte),
                fileCompressionInfomation.CompressionUnitShift.GetType(),
                1103,
                @"[In FILE_COMPRESSION_INFORMATION]CompressionUnitShift (1 byte):  
                An 8-bit unsigned integer that contains the compression unit shift, 
                which is the number of bits by which to left-shift a 1 bit to arrive at the compression unit size.");

            // As the value of the CompressionUnitShift is verified above rs, we didn't verify here again
            // We capture this rs directly.
            Site.CaptureRequirement(
                1104,
                @"[In FILE_COMPRESSION_INFORMATION]CompressionUnitShift (1 byte):  
                The compression unit size is the number of bytes in a compression unit, that is, 
                the number of bytes to be compressed.");

            //
            // Verify requirement 1105
            //

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "MS-FSCC_R1105, CompressionUnitShift: {0}",
                fileCompressionInfomation.CompressedFileSize);

            // As the value is implementation-defined, and the <59> will be verified separately
            // Verify the size of CompressionUnitShift here.
            const int sizeOfCompressionUnitShift = 1;
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfCompressionUnitShift,
                Marshal.SizeOf(fileCompressionInfomation.CompressionUnitShift),
                1105,
                @"[In FILE_COMPRESSION_INFORMATION]CompressionUnitShift (1 byte):  This value is implementation-
                defined.<59>");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "MS-FSCC ReqId: {0}, ChunkShift: {1}", 1108, fileCompressionInfomation.ChunkShift);
            //
            // Verify requirement 1108
            //
            // As the value is implementation-defined, and the <60> will be verified separately
            // Verify the size of ChunkShift here.
            const int sizeOfChunkShift = 1;
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfChunkShift,
                Marshal.SizeOf(fileCompressionInfomation.ChunkShift),
                1108,
                @"[In FILE_COMPRESSION_INFORMATION]ChunkShift (1 byte): This value is implementation-defined.<60>");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "MS-FSCC ReqId: {0}, ClusterShift: {1}", 1113, fileCompressionInfomation.ClusterShift);
            //
            // Verify requirement 1113
            //
            // As the value is implementation-defined, and the <61> will be verified separately
            // Verify the size of ClusterShift here.
            const int sizeOfClusterShift = 1;
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfClusterShift,
                Marshal.SizeOf(fileCompressionInfomation.ClusterShift),
                1113,
                @"[In FILE_COMPRESSION_INFORMATION]ClusterShift (1 byte):  This value is implementation defined.<61>");

            //
            // Verify requirement 1106
            //

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1106 Actual ChunkShift: {0}, Expected ChunkShift: {1}.",
                fileCompressionInfomation.ChunkShift, chunkShift);

            // Verify type and value of the ChunkShift
            bool isVerifyR1106 = (
                fileCompressionInfomation.ChunkShift.GetType() == typeof(byte) &&
                fileCompressionInfomation.ChunkShift == chunkShift);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1106,
                1106,
                @"[In FILE_COMPRESSION_INFORMATION]ChunkShift (1 byte):  An 8-bit unsigned integer that contains 
                the compression chunk size in bytes in log 2 format.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1109 Actual ClusterShift: {0}, Expected ClusterShift: {1}.",
                fileCompressionInfomation.ClusterShift, clusterShift);
            //
            // Verify requirement 1109
            //
            // Verify type and value of the ClusterShift
            bool isVerifyR1109 = (
                fileCompressionInfomation.ClusterShift.GetType() == typeof(Byte) &&
                fileCompressionInfomation.ClusterShift == clusterShift);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1109,
                1109,
                @"[In FILE_COMPRESSION_INFORMATION]ClusterShift (1 byte):  
                An 8-bit unsigned integer that specifies, in log 2 format, the amount of space that must 
                be saved by compression to successfully compress a compression unit.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1112 Actual clusterSize: {0}, Expected clusterSize: {1}.",
                fileCompressionInfomation.ClusterShift, clusterSize);
            //
            // Verify requirement 1112
            //
            // Verify the value of the ClusterShift
            // As the type of the ClusterShift is verified above, we didn't verify here again
            bool isVerifyR1112 = ((fileCompressionInfomation.ClusterShift << 1) == clusterSize);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1112,
                1112,
                @"[In FILE_COMPRESSION_INFORMATION]ClusterShift (1 byte): 
                the cluster shift is the number of bits by which to left shift a 1 bit to arrive 
                at the size of a cluster.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1114");
            //
            // Verify requirement 1114
            //
            Site.CaptureRequirementIfAreEqual<int>(
                3,
                fileCompressionInfomation.Reserved.Length,
                1114,
                @"[In FILE_COMPRESSION_INFORMATION]Reserved (3 bytes):  A 24-bit reserved value.");

            // As all the elements in the FILE_COMPRESSION_INFORMATION have been verified above
            // This rs will be captured directly
            Site.CaptureRequirement(
                1093,
                @"[In FileCompressionInformation]The FILE_COMPRESSION_INFORMATION data element is as follows.:
                [CompressedFileSize, ..., CompressionFormat, CompressionUnitShift, ChunkShift, ClusterShift, 
                Reserved]");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify data type FileNetworkOpenInformation

        /// <summary>
        /// Verify data type FileNetworkOpenInformation
        /// </summary>
        /// <param name="fileNetworkOpenInformation"> FileNetworkOpenInformation data type </param>
        /// <param name="allocationSize"> A 64-bit signed integer that contains the file allocation size in bytes. </param>
        /// <param name="fileAttributes"> A 32-bit unsigned integer that contains the file attributes. </param>
        public void VerifyDataTypeFileNetworkOpenInformation(
            FileNetworkOpenInformation fileNetworkOpenInformation,
            int allocationSize,
            uint fileAttributes)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1434 Actual CreationTime: {0}",
                fileNetworkOpenInformation.CreationTime.ToString());
            //
            // Verify requirement 1434
            //
            // Verify the type and the value of the CreationTime
            int sizeOfCreationTime = 8;

            bool isVerifyR1434 = (
                Marshal.SizeOf(fileNetworkOpenInformation.CreationTime) == sizeOfCreationTime &&
                fileNetworkOpenInformation.CreationTime.GetType() == typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1434,
                1434,
                @"[In FILE_NETWORK_OPEN_INFORMATION]CreationTime (8 bytes):  
                A 64-bit signed integer that contains the time when the file was created in the format of 
                a FILETIME structure.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1435 Actual CreationTime: {0}",
                fileNetworkOpenInformation.CreationTime.ToString());
            //
            // Verify requirement 1435
            //
            // Verify CreationTime >= 0
            // As the type and the value is verified above, we didn't verify here again
            bool isVerifyR1435 = (
                fileNetworkOpenInformation.CreationTime.dwHighDateTime >= 0 &&
                fileNetworkOpenInformation.CreationTime.dwLowDateTime >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1435,
                1435,
                @"[In FILE_NETWORK_OPEN_INFORMATION]CreationTime (8 bytes): 
                The value of this field MUST be greater than or equal to 0.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1436 Actual LastAccessTime: {0}",
                fileNetworkOpenInformation.LastAccessTime.ToString());
            //
            // Verify requirement 1436
            //
            // Verify the type and the value of the LastAccessTime
            int sizeOfLastAccessTime = 8;
            bool isVerifyR1436 = (
                Marshal.SizeOf(fileNetworkOpenInformation.LastAccessTime) == sizeOfLastAccessTime &&
                fileNetworkOpenInformation.LastAccessTime.GetType() == typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1436,
                1436,
                @"[In FILE_NETWORK_OPEN_INFORMATION]LastAccessTime (8 bytes):  A 64-bit signed integer that contains 
                the last time the file was accessed in the format of a FILETIME structure.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1437 Actual LastAccessTime: {0}",
                fileNetworkOpenInformation.LastAccessTime.ToString());

            //
            // Verify requirement 1437
            //
            // Verify LastAccessTime >= 0
            // As the type and the value of the LastAccessTime is Verified above, we didn't verify here again
            bool isVerifyR1437 = (
                fileNetworkOpenInformation.LastAccessTime.dwHighDateTime >= 0 &&
                fileNetworkOpenInformation.LastAccessTime.dwLowDateTime >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1437,
                1437,
                @"[In FILE_NETWORK_OPEN_INFORMATION]LastAccessTime (8 bytes): 
                The value of this field MUST be greater than or equal to 0.");

            //
            // Verify requirement 1438
            //

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1438 Actual LastWriteTime: {0}",
                fileNetworkOpenInformation.LastWriteTime.ToString());

            // Verify the type and the value of the LastWriteTime
            int sizeOfLastWriteTime = 8;

            bool isVerifyR1438 = (
                Marshal.SizeOf(fileNetworkOpenInformation.LastWriteTime) == sizeOfLastWriteTime &&
                fileNetworkOpenInformation.LastWriteTime.GetType() == typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1438,
                1438,
                @"[In FILE_NETWORK_OPEN_INFORMATION]LastWriteTime (8 bytes):  
                A 64-bit signed integer that contains the last time information was written to the file in the 
                format of a FILETIME structure.");
            
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1439 Actual LastWriteTime: {0}",
                fileNetworkOpenInformation.LastWriteTime.ToString());
            //
            // Verify requirement 1439
            //
            // Verify LastWriteTime >= 0
            // As the type and the value of the LastWriteTime is verified above, we didn't verify here again

            bool isVerifyR1439 = (
                fileNetworkOpenInformation.LastWriteTime.dwHighDateTime >= 0 &&
                fileNetworkOpenInformation.LastWriteTime.dwLowDateTime >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1439,
                1439,
                @"[In FILE_NETWORK_OPEN_INFORMATION]LastWriteTime (8 bytes): 
                The value of this field MUST be greater than or equal to 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1440 Actual ChangeTime: {0}",
                fileNetworkOpenInformation.ChangeTime.ToString());

            //
            // Verify requirement 1440
            //
            // Verify the type and the value of the ChangeTime
            int sizeOfChangeTime = 8;

            bool isVerifyR1440 = (
                Marshal.SizeOf(fileNetworkOpenInformation.ChangeTime) == sizeOfChangeTime &&
                fileNetworkOpenInformation.ChangeTime.GetType() == typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1440,
                1440,
                @"[In FILE_NETWORK_OPEN_INFORMATION]ChangeTime (8 bytes):  
                A 64-bit signed integer that contains the last time the file was changed in the format of 
                a FILETIME structure.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1441 Actual ChangeTime: {0}",
                fileNetworkOpenInformation.ChangeTime.ToString());
            //
            // Verify requirement 1441
            //
            // Verify the ChangeTime >= 0
            // As the type and the value of the changeTime is verified above, we didn't verify here again
            bool isVerified_r1441 = (
                fileNetworkOpenInformation.ChangeTime.dwHighDateTime >= 0 &&
                fileNetworkOpenInformation.ChangeTime.dwLowDateTime >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerified_r1441,
                1441,
                @"[In FILE_NETWORK_OPEN_INFORMATION]ChangeTime (8 bytes):  
                The value of this field MUST be greater than or equal to 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1444 Actual AllocationSize: {0}, Expected AllocationSize: {1}.",
                fileNetworkOpenInformation.AllocationSize, allocationSize);

            //
            // Verify requirement 1444
            //
            // As the type and the value of the AllocationSize is verified above, we didn't verify here again
            Site.CaptureRequirementIfIsTrue(
                fileNetworkOpenInformation.AllocationSize >= 0,
                1444,
                @"[In FILE_NETWORK_OPEN_INFORMATION]AllocationSize (8 bytes):  
                The value of this field MUST be greater than or equal to 0.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1445 EndOfFile: {0}.",
                fileNetworkOpenInformation.EndOfFile);
            //
            // Verify requirement 1445
            //
            // As it's unenable for us to verify the EndOfFile, what we only can do is just verifying the 
            // offset is in the range of the AllocationSize
            bool isVerifyR1445 = (
                fileNetworkOpenInformation.EndOfFile.GetType() == typeof(Int64) &&
                fileNetworkOpenInformation.EndOfFile <= fileNetworkOpenInformation.AllocationSize);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1445,
                1445,
                @"[In FILE_NETWORK_OPEN_INFORMATION]EndOfFile (8 bytes):  
                 A 64-bit signed integer that contains the absolute new end-of-file position as a byte offset 
                 from the start of the file.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, 
                "MS-FSCC_R1446, EndOfFile is : {0}",
                fileNetworkOpenInformation.EndOfFile);
            //
            // Verify requirement 1446
            //
            // As we parse the file by using the endoFile, it's useless for us to verify if this value valid again
            // Verify the size for this rs
            const int sizeOfEndOfFile = 8;
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfEndOfFile,
                Marshal.SizeOf(fileNetworkOpenInformation.EndOfFile),
                1446,
                @"[In FILE_NETWORK_OPEN_INFORMATION]EndOfFile (8 bytes): EndOfFile specifies the offset to the byte 
                immediately following the last valid byte in the file.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, 
                "MS-FSCC_R1447, EndOfFile: {0}",
                fileNetworkOpenInformation.EndOfFile);
            //
            // Verify requirement 1447
            //
            // As we parse the file by using the endoFile, it's useless for us to verify if this value valid again
            // Verify the size for this rs
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfEndOfFile,
                Marshal.SizeOf(fileNetworkOpenInformation.EndOfFile),
                1447,
                @"[In FILE_NETWORK_OPEN_INFORMATION]EndOfFile (8 bytes): Because this value is zero-based, it 
                actually refers to the first free byte in the file. That is, it is the offset from the beginning of 
                the file at which new bytes appended to the file will be written.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "MS-FSCC ReqId: {0}", 1450);

            const int sizeOfFileAttributes = 4;
            //
            // Verify requirement 1450
            //
            // Verify the size and the value of FileAttributes, and the values are:
            // FILE_ATTRIBUTE_ARCHIVE 0x00000020, FILE_ATTRIBUTE_COMPRESSED 0x00000800, 
            // FILE_ATTRIBUTE_DIRECTORY 0x00000010, FILE_ATTRIBUTE_ENCRYPTED 0x00004000, 
            // FILE_ATTRIBUTE_HIDDEN 0x00000002, FILE_ATTRIBUTE_NORMAL 0x00000080, 
            // FILE_ATTRIBUTE_NOT_CONTENT_INDEXED 0x00002000, FILE_ATTRIBUTE_OFFLINE 0x00001000, 
            // FILE_ATTRIBUTE_READONLY 0x00000001, FILE_ATTRIBUTE_REPARSE_POINT 0x00000400, 
            // FILE_ATTRIBUTE_SPARSE_FILE 0x00000200, FILE_ATTRIBUTE_SYSTEM 0x00000004, 
            // FILE_ATTRIBUTE_TEMPORARY 0x00000100] 
            uint bitUsed = (
                0x00000020 | 0x00000800 | 0x00000010 | 0x00004000 |
                0x00000002 | 0x00000080 | 0x00002000 | 0x00001000 |
                0x00000001 | 0x00000400 | 0x00000200 | 0x00000004 | 0x00000100);
            
            // Get the bits which the file attributes not used
            uint bitUnused = ~bitUsed;
            bool isVerufyR1450 =  (((sizeOfFileAttributes == Marshal.SizeOf(fileNetworkOpenInformation.FileAttributes)) &&
                (bitUnused & fileNetworkOpenInformation.FileAttributes) == 0x00000000));

            Site.CaptureRequirementIfIsTrue(
                isVerufyR1450,
                1450,
                @"[In FILE_NETWORK_OPEN_INFORMATION]FileAttributes (4 bytes): Valid attributes are as specified in 
                section 2.6.");

            //
            // Verify requirement 1452
            //

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "MS-FSCC_R1452 ,Reserved is : {0} ",
                fileNetworkOpenInformation.Reserved);

            // As the reserved field is reserved, and the value can be any value
            // This rs will be captured directly after verify the size of reserved
            const int sizeOfReserved = 4;
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfReserved,
                Marshal.SizeOf(fileNetworkOpenInformation.Reserved),
                1452,
                @"[In FILE_NETWORK_OPEN_INFORMATION]Reserved (4 bytes): This field is reserved.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1448 EndOfFile: {0}.",
                fileNetworkOpenInformation.EndOfFile);
            //
            // Verify requirement 1448
            //
            Site.CaptureRequirementIfIsTrue(
                fileNetworkOpenInformation.EndOfFile >= 0,
                1448,
                @"[In FILE_NETWORK_OPEN_INFORMATION]EndOfFile (8 bytes): The value of this field 
                MUST be greater than or equal to 0.");          
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1449 Actual FileAttributes: {0}, Expected FileAttributes: {1}.",
                fileNetworkOpenInformation.FileAttributes, fileAttributes);
            //
            // Verify requirement 1449
            //
            // Verify the type and the value of the fileAttributes
            bool isVerifyR1449 = (fileNetworkOpenInformation.FileAttributes.GetType() == typeof(UInt32) &&
                                    fileNetworkOpenInformation.FileAttributes == fileAttributes);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1449,
                1449,
                @"[In FILE_NETWORK_OPEN_INFORMATION]FileAttributes (4 bytes):  
                A 32-bit unsigned integer that contains the file attributes.");

           

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1451 Actual Reserved is: {0}",
                fileNetworkOpenInformation.Reserved);
            //
            // Verify requirement 1451
            //
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfReserved,
                Marshal.SizeOf(fileNetworkOpenInformation.Reserved),
                1451,
                @"[In FILE_NETWORK_OPEN_INFORMATION]Reserved (4 bytes):  A 32-bit field.");
            //
            // Add the debug information
            //
           Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1453 Actual Reserved: {0}.",
                fileNetworkOpenInformation.Reserved);
            //
            // Verify requirement 1453
            //
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfReserved,
                Marshal.SizeOf(fileNetworkOpenInformation.Reserved),
                1453,
                @"[In FILE_NETWORK_OPEN_INFORMATION]Reserved (4 bytes): This field can be set to any value.");

            // As all the elements in the FILE_NETWORK_OPEN_INFORMATION have been verified above
            // This rs will be captured directly
            Site.CaptureRequirement(
                1433,
                @"[In FileNetworkOpenInformation]The FILE_NETWORK_OPEN_INFORMATION data element is as follows:
                [CreationTime, ..., LastAccessTime, ..., LastWriteTime, ..., ChangeTime, ..., AllocationSize, ..., 
                EndOfFile, ..., FileAttributes, Reserved].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileAttributeTagInformation

        /// <summary>
        /// Verify data type FileAttributeTagInformation
        /// </summary>
        /// <param name="fileAttributeTagInformation"> FileAttributeTagInformation type data </param>
        /// <param name="fileAttributes"> A 32-bit unsigned integer that contains the file attributes. </param>
        public void VerifyDataTypeFileAttributeTagInformation(
            FileAttributeTagInformation fileAttributeTagInformation,
            uint fileAttributes)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1016 Actual size of FileAttributes: {0}, Expected size of FileAttributes: {1}.",
                fileAttributeTagInformation.FileAttributes, fileAttributes);
            //
            // Verify requirement 1016
            //
            // Verify type and value of the fileAttributeTagInformation
            bool isVerifyR1016 = (
                fileAttributeTagInformation.FileAttributes.GetType() == typeof(UInt32) &&
                fileAttributeTagInformation.FileAttributes == fileAttributes);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1016,
                1016,
                @"[In FILE_ATTRIBUTE_TAG_INFORMATION]FileAttributes (4 bytes):  
                A 32-bit unsigned integer that contains the file attributes.");

          
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1017, actual FileAttributes:{0}",
                fileAttributeTagInformation.FileAttributes);
            //
            // Verify requirement 1017
            //
            const int sizeOfFileAttributes = 4;
            // Verify the size and the value of FileAttributes, and the values are:
            // FILE_ATTRIBUTE_ARCHIVE 0x00000020, FILE_ATTRIBUTE_COMPRESSED 0x00000800, 
            // FILE_ATTRIBUTE_DIRECTORY 0x00000010, FILE_ATTRIBUTE_ENCRYPTED 0x00004000, 
            // FILE_ATTRIBUTE_HIDDEN 0x00000002, FILE_ATTRIBUTE_NORMAL 0x00000080, 
            // FILE_ATTRIBUTE_NOT_CONTENT_INDEXED 0x00002000, FILE_ATTRIBUTE_OFFLINE 0x00001000, 
            // FILE_ATTRIBUTE_READONLY 0x00000001, FILE_ATTRIBUTE_REPARSE_POINT 0x00000400, 
            // FILE_ATTRIBUTE_SPARSE_FILE 0x00000200, FILE_ATTRIBUTE_SYSTEM 0x00000004, 
            // FILE_ATTRIBUTE_TEMPORARY 0x00000100] 
            uint bitUsed = (
                0x00000020 | 0x00000800 | 0x00000010 | 0x00004000 |
                0x00000002 | 0x00000080 | 0x00002000 | 0x00001000 |
                0x00000001 | 0x00000400 | 0x00000200 | 0x00000004 | 0x00000100);
            // Get the bits which the file attributes not used
            uint bitUnused = ~bitUsed;
            bool isVerifyR1017 = ((sizeOfFileAttributes == Marshal.SizeOf(fileAttributeTagInformation.FileAttributes)) &&
                                 ((bitUnused & fileAttributeTagInformation.FileAttributes) == 0x00000000));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1017,
                1017,
                @"[In FILE_ATTRIBUTE_TAG_INFORMATION]FileAttributes (4 bytes): Valid file attributes are as specified
                in section 2.6.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1018, Actual ReparseTag: {0}.",
                fileAttributeTagInformation.ReparseTag);

            //
            // Verify requirement 1018
            //
            // Verify type of the ReparseTag
            // As the ReparseTag is not defined clearly in this rs, we didn't verify the value here
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(UInt32),
                fileAttributeTagInformation.ReparseTag.GetType(),
                1018,
                @"[In FILE_ATTRIBUTE_TAG_INFORMATION]ReparseTag (4 bytes):  A 32-bit unsigned integer that specifies the 
                reparse point tag.");

            // If the FileAttributes member includes the FILE_ATTRIBUTE_REPARSE_POINT attribute flag
            // FILE_ATTRIBUTE_REPARSE_POINT value is 0x00000400
            if ((fileAttributeTagInformation.FileAttributes & 0x00000400) == 0x00000400)
            {
                //
                // Verify requirement 1019
                //

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1019, Actual ReparseTag: {0}",
                    fileAttributeTagInformation.ReparseTag);

                // When the FileAttributes member includes the FILE_ATTRIBUTE_REPARSE_POINT attribute flag
                // We only verify the size of ReparseTag here
                int sizeOfReparseTag = 4;

                Site.CaptureRequirementIfAreEqual<int>(
                    sizeOfReparseTag,
                    Marshal.SizeOf(fileAttributeTagInformation.ReparseTag),
                    1019,
                    @"[In FILE_ATTRIBUTE_TAG_INFORMATION]ReparseTag (4 bytes): 
                    If the FileAttributes member includes the FILE_ATTRIBUTE_REPARSE_POINT attribute flag, 
                    this member specifies the reparse tag.");
            }
            // As all the elements in the FILE_ATTRIBUTE_TAG_INFORMATION have been verified above
            // This rs will be captured directly
            Site.CaptureRequirement(
                1015,
                @"[In FileAttributeTagInformation]The FILE_ATTRIBUTE_TAG_INFORMATION data element is as follows:
                [FileAttributes, ReparseTag].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FilePositionInformation

        /// <summary>
        /// Verify data type FILE_POSITION_INFORMATION
        /// </summary>
        /// <param name="filePositionInformation"> FILE_POSITION_INFORMATION type data </param>
        public void VerifyDataTypeFilePositionInformation(
            FILE_POSITION_INFORMATION filePositionInformation)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1531 CurrentByteOffset: {0};",
                filePositionInformation.CurrentByteOffset);
            //
            // Verify MS-FSCC requirement 1531
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                 typeof(_LARGE_INTEGER),
                 filePositionInformation.CurrentByteOffset.GetType(),
                 1531,
                 @"[In FILE_POSITION_INFORMATION]CurrentByteOffset (8 bytes):  A LARGE_INTEGER that MUST contain 
                 the offset, in bytes, of the file pointer from the beginning of the file.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R983, Actual PositionInformation type: {0}, Expected PositionInformation type: {1}.",
                filePositionInformation.GetType().Name, typeof(FILE_POSITION_INFORMATION));
            //
            // Verify MS-FSCC requirement 983
            //
            // Verify type of the PositionInformation specified in section 2.4.32
            int sizeOfPositionInformation = 8;

            bool isVerifyR983 = (
                Marshal.SizeOf(filePositionInformation) == sizeOfPositionInformation &&
                filePositionInformation.GetType() == typeof(FILE_POSITION_INFORMATION));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR983,
                983,
                @"[In FileAllInformation]PositionInformation (8 bytes):  
                A FILE_POSITION_INFORMATION structure specified in section 2.4.32.");

            // As all the elements in the FILE_POSITION_INFORMATION have been verified above
            // This rs will be captured directly
            Site.CaptureRequirement(
                1530,
                @"[In FilePositionInformation]The FILE_POSITION_INFORMATION data element is as follows:
                [CurrentByteOffset, ...].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileFsVolumeInformation

        /// <summary>
        ///  Verify data type FileFsVolumeInformation
        /// </summary>
        /// <param name="fileFsVolumeInformation"> FileFsVolumeInformation type data </param>
        /// <param name="volumeSerialNumber"> A 32-bit unsigned integer that contains the serial number of the volume. </param>
        /// <param name="isSupportsOOFileSysObj"> If the file system supports object-oriented file system objects. </param>
        /// <param name="volumeLabel"> A variable-length Unicode field containing the name of the volume. </param>
        public void VerifyDataTypeFileFsVolumeInformationForOld(
            FileFsVolumeInformation fileFsVolumeInformation,
            uint volumeSerialNumber,
            bool isSupportsOOFileSysObj)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1796");
            //
            // Verify requirement 1796
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(FileFsVolumeInformation),
                fileFsVolumeInformation.GetType(),
                1796,
                @"The message[FileFsVolumeInformation] contains a FILE_FS_VOLUME_INFORMATION data element.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1798 Actual VolumeCreationTime: {0}",
                fileFsVolumeInformation.VolumeCreationTime.ToString());
            //
            // Verify requirement 1798
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(long),
                fileFsVolumeInformation.VolumeCreationTime.GetType(),
                1798,
                @"[In FILE_FS_VOLUME_INFORMATION]VolumeCreationTime (8 bytes):  
                A 64-bit signed integer that contains the time when the volume was created in the format of a FILETIME structure.");

            
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1799 Actual VolumeCreationTime: {0}",
                fileFsVolumeInformation.VolumeCreationTime);
            //
            // Verify requirement 1799
            //
            Site.CaptureRequirementIfIsTrue(
                fileFsVolumeInformation.VolumeCreationTime >= 0,
                1799,
                @"[In FILE_FS_VOLUME_INFORMATION]VolumeCreationTime (8 bytes): 
                The value of this field MUST be greater than or equal to 0.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1800 Actual VolumeSerialNumber: {0}, Expected VolumeSerialNumber: {1}.",
                fileFsVolumeInformation.VolumeSerialNumber, volumeSerialNumber);
            //
            // Verify requirement 1800
            //
            bool isVerifyR1800 = (
                fileFsVolumeInformation.VolumeSerialNumber.GetType() == typeof(UInt32) &&
                Marshal.SizeOf(fileFsVolumeInformation.VolumeSerialNumber) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1800,
                1800,
                @"[In FILE_FS_VOLUME_INFORMATION]VolumeSerialNumber (4 bytes):  
                A 32-bit unsigned integer that contains the serial number of the volume.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1801");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1802 Actual VolumeSerialNumber: {0}",
                fileFsVolumeInformation.VolumeSerialNumber);
            //
            // Verify requirement 1802
            //
            // As no specific format or content of this field is required for protocol interoperation.
            // We only verify the size of the VolumeSerialNumber
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(fileFsVolumeInformation.VolumeSerialNumber),
                1802,
                @"[In FILE_FS_VOLUME_INFORMATION]VolumeSerialNumber (4 bytes):  
                No specific format or content of this field is required for protocol interoperation.");

          
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1803 Actual VolumeSerialNumber: {0}",
                fileFsVolumeInformation.VolumeSerialNumber);
            //
            // Verify requirement 1803
            //
            // As this value is not required to be unique, alsp no specific format or content.
            // That means the value of volumeSerialNumber can be any value, we only the verify the size of it
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(fileFsVolumeInformation.VolumeSerialNumber),
                1803,
                @"[In FILE_FS_VOLUME_INFORMATION]VolumeSerialNumber (4 bytes):  
                This value is not required to be unique.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1804 Actual VolumeLabelLength: {0}, Expected VolumeLabelLength: {1}.",
                fileFsVolumeInformation.VolumeLabelLength, fileFsVolumeInformation.VolumeLabel.Length);
            //
            // Verify requirement 1804
            //
            // Verify type and the value of the VolumeLabelLength
            bool isVerifyR1804 = (
                fileFsVolumeInformation.VolumeLabelLength.GetType() == typeof(UInt32) &&
                fileFsVolumeInformation.VolumeLabelLength == fileFsVolumeInformation.VolumeLabel.Length);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1804,
                1804,
                @"[In FILE_FS_VOLUME_INFORMATION]VolumeLabelLength (4 bytes):  
                A 32-bit unsigned integer that contains the length, in bytes, including the trailing NULL, 
                if present, of the name of the volume.");

            // If the file system supports object-oriented file system objects.
            if (isSupportsOOFileSysObj)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1805 Actual SupportsObjects: {0}, Expected SupportsObjects: {1}.",
                    fileFsVolumeInformation.SupportsObjects, SupportsObjects_Values.V1);
                //
                // Verify requirement 1805
                //
                // Verify type and value of the SupportsObjects
                bool isVerifyR1805 = (
                    fileFsVolumeInformation.SupportsObjects.GetType() == typeof(SupportsObjects_Values) &&
                    fileFsVolumeInformation.SupportsObjects == SupportsObjects_Values.V1);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1805,
                    1805,
                    @"[In FILE_FS_VOLUME_INFORMATION]SupportsObjects (1 byte):  A 1-byte Boolean (unsigned char) that is TRUE (0x01) 
                    if the file system supports object-oriented file system objects.");
            }

            // if the file system does not support object-oriented file system objects
            else
            {
                
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1806 Actual SupportsObjects: {0}, Expected SupportsObjects: {1}.",
                    fileFsVolumeInformation.SupportsObjects, SupportsObjects_Values.V2);
                //
                // Verify requirement 1806
                //
                // Verify type and value of the SupportsObjects
                bool isVerifyR1806 = (
                    fileFsVolumeInformation.SupportsObjects.GetType() == typeof(Byte) &&
                    fileFsVolumeInformation.SupportsObjects == SupportsObjects_Values.V2);

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1806,
                    1806,
                    @"[In FILE_FS_VOLUME_INFORMATION]SupportsObjects (1 byte): 
                    otherwise[if the file system does not support object-oriented file system objects], [it is] FALSE (0x00).<91>");
            }
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1808 Actual VolumeLabel : {0}",
                fileFsVolumeInformation.VolumeLabel.ToString());
            //
            // Verify requirement 1808
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Byte[]),
                fileFsVolumeInformation.VolumeLabel.GetType(),
                1808,
                @"[In FILE_FS_VOLUME_INFORMATION]VolumeLabel (variable):  
                A variable-length Unicode field containing the name of the volume.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1809");

            //
            // Verify requirement 1809
            //
            Site.CaptureRequirementIfAreEqual<int>(
                fileFsVolumeInformation.VolumeLabel.Length,
                (int)fileFsVolumeInformation.VolumeLabelLength,
                1809,
                @"[In FILE_FS_VOLUME_INFORMATION]VolumeLabel (variable): 
                The content of this field can be a NULL-terminated string or can be a string padded with the 
                space character to be VolumeLabelLength bytes long.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify data type FileFsSizeInformation

        /// <summary>
        /// Verify data type FileFsSizeInformation
        /// </summary>
        /// <param name="fileFsSizeInformation"> FileFsSizeInformation data type </param>
        /// <param name="sectorsPerAllocationUnit"> A 32-bit unsigned integer that contains the number of sectors in each allocation unit. </param>
        /// <param name="bytesPerSector"> A 32-bit unsigned integer that contains the number of bytes in each sector. </param>
        public void VerifyDataTypeFileFsSizeInformation(
            FileFsSizeInformation fileFsSizeInformation,
            uint sectorsPerAllocationUnit,
            uint bytesPerSector)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1787");
            //
            // Verify requirement 1787
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(FileFsSizeInformation),
                fileFsSizeInformation.GetType(),
                1787,
                @"The message[FileFsSizeInformation] contains a FILE_FS_SIZE_INFORMATION data element.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1789 ");
            //
            // Verify requirement 1789
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Int64),
                fileFsSizeInformation.TotalAllocationUnits.GetType(),
                1789,
                @"[In FILE_FS_SIZE_INFORMATION]TotalAllocationUnits (8 bytes):  
                A 64-bit signed integer that contains the total number of allocation units on the volume 
                that are available to the user associated with the calling thread.");


            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1790 Actual TotalAllocationUnits: {0}",
                fileFsSizeInformation.TotalAllocationUnits);
            //
            // Verify requirement 1790
            //
            Site.CaptureRequirementIfIsTrue(
                fileFsSizeInformation.TotalAllocationUnits >= 0,
                1790,
                @"[In FILE_FS_SIZE_INFORMATION]TotalAllocationUnits (8 bytes):  
                This value MUST be greater than or equal to 0.<89>");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1791");
            //
            // Verify requirement 1791
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Int64),
                fileFsSizeInformation.ActualAvailableAllocationUnits.GetType(),
                1791,
                @"[In FILE_FS_SIZE_INFORMATION]ActualAvailableAllocationUnits (8 bytes):  
                A 64-bit signed integer that contains the total number of free allocation units 
                on the volume that are available to the user associated with the calling thread.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1792 Actual ActualAvailableAllocationUnits: {0}",
                fileFsSizeInformation.ActualAvailableAllocationUnits);
            //
            // Verify requirement 1792
            //
            Site.CaptureRequirementIfIsTrue(
                fileFsSizeInformation.ActualAvailableAllocationUnits >= 0,
                1792,
                @"[In FILE_FS_SIZE_INFORMATION]ActualAvailableAllocationUnits (8 bytes):  
                This value MUST be greater than or equal to 0.<90>");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1793 Actual SectorsPerAllocationUnit: {0}, Expected SectorsPerAllocationUnit: {1}.",
                fileFsSizeInformation.SectorsPerAllocationUnit, sectorsPerAllocationUnit);
            //
            // Verify requirement 1793
            //
            // Verify type and value of SectorsPerAllocationUnit
            bool isVerifyR1793 = (
                fileFsSizeInformation.SectorsPerAllocationUnit.GetType() == typeof(UInt32) &&
                fileFsSizeInformation.SectorsPerAllocationUnit == sectorsPerAllocationUnit);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1793,
                1793,
                @"[In FILE_FS_SIZE_INFORMATION]SectorsPerAllocationUnit (4 bytes):  
                A 32-bit unsigned integer that contains the number of sectors in each allocation unit.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1794 Actual BytesPerSector: {0}, Expected BytesPerSector: {1}.",
                fileFsSizeInformation.BytesPerSector, bytesPerSector);
            //
            // Verify requirement 1794
            //
            // Verify  type and value of BytesPerSector
            bool isVerifyR1794 = (
                fileFsSizeInformation.BytesPerSector.GetType() == typeof(UInt32) &&
                fileFsSizeInformation.BytesPerSector == bytesPerSector);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1794,
                1794,
                @"[In FILE_FS_SIZE_INFORMATION]BytesPerSector (4 bytes):  A 32-bit unsigned integer that contains 
                the number of bytes in each sector.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify data type FileFsDeviceInformation

        /// <summary>
        /// Verify data type FileFsDeviceInformation
        /// </summary>
        /// <param name="fileFsDeviceInformation"> FileFsDeviceInformation type data </param>
        public void VerifyDataTypeFileFsDeviceInformation(
            FileFsDeviceInformation fileFsDeviceInformation)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1811 ");
            //
            // Verify requirement 1811
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(FileFsDeviceInformation),
                fileFsDeviceInformation.GetType(),
                1811,
                @"The message[FileFsDeviceInformation] contains a FILE_FS_DEVICE_INFORMATION data element.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1813 ");
            //
            // Verify requirement 1813
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(DeviceType_Values),
                fileFsDeviceInformation.DeviceType.GetType(),
                1813,
                @"[In FILE_FS_DEVICE_INFORMATION]DeviceType (4 bytes):  This identifies the type of given volume.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1814 Actual DeviceType: {0}",
                fileFsDeviceInformation.DeviceType);
            //
            // Verify requirement 1814
            //
            // Verify the value of the DeviceType
            bool isVerifyR1814 = (
                fileFsDeviceInformation.DeviceType == DeviceType_Values.FILE_DEVICE_CD_ROM ||
                fileFsDeviceInformation.DeviceType == DeviceType_Values.FILE_DEVICE_DISK);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1814,
                1814,
                @"[In FILE_FS_DEVICE_INFORMATION]DeviceType (4 bytes): 
                It MUST be one of the following:[FILE_DEVICE_CD_ROM 0x00000002, 
                FILE_DEVICE_DISK 0x00000007].");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1817 Actual Characteristics is: {0}",
                fileFsDeviceInformation.Characteristics);

            // Verify the size of the Characteristics, as the type of it here isn't mentioned clearly
            //
            // Verify requirement 1817
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Characteristics_Values),
                fileFsDeviceInformation.Characteristics.GetType(),
                1817,
                @"[In FILE_FS_DEVICE_INFORMATION]Characteristics (4 bytes):  
                A bit field which identifies various characteristics about a given volume.");
          
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1818 Actual size of Characteristics: {0}",
                fileFsDeviceInformation.Characteristics);
            //
            // Verify requirement 1818
            //
            // Verify the value of Characteristics
            const int characteristicsBitUsed = (
                0x00000001 | 0x00000002 | 0x00000004 | 0x00000008 | 0x00000010 |
                0x00000020 | 0x00000040 | 0x00000080 | 0x00000100 | 0x00000800 |
                0x00001000 | 0x00002000 | 0x00020000);

            // characteristicsBitUnused contains the bit that not used
            const int characteristicsBitUnused = ~characteristicsBitUsed;

            bool isVerifyR1818 = (
                ((uint)fileFsDeviceInformation.Characteristics & characteristicsBitUnused) == 0x00000000);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1818,
                1818,
                @"[In FILE_FS_DEVICE_INFORMATION]Characteristics (4 bytes): The following are valid bit values.
                [FILE_REMOVABLE_MEDIA 0x00000001, FILE_READ_ONLY_DEVICE 0x00000002, FILE_FLOPPY_DISKETTE 0x00000004, 
                FILE_WRITE_ONCE_MEDIA 0x00000008, FILE_REMOTE_DEVICE 0x00000010, FILE_DEVICE_IS_MOUNTED 0x00000020, 
                FILE_VIRTUAL_VOLUME 0x00000040, FILE_AUTOGENERATED_DEVICE_NAME 0x00000080, FILE_DEVICE_SECURE_OPEN 0x00000100, 
                FILE_CHARACTERISTIC_PNP_DEVICE 0x00000800, FILE_CHARACTERISTIC_TS_DEVICE 0x00001000, 
                FILE_CHARACTERISTIC_WEBDAV_DEVICE 0x00002000]");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileFsAttributeInformation

        /// <summary>
        /// Verify data type FileFsAttributeInformation
        /// </summary>
        /// <param name="fileFsAttributeInformation"> FileFsAttributeInformation type data </param>
        public void VerifyDataTypeFileFsAttributeInformationForOld(
            FileFsAttributeInformation fileFsAttributeInformation)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1684");
            //
            // Verify requirement 1684
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(FileFsAttributeInformation),
                fileFsAttributeInformation.GetType(),
                1684,
                @"The message[FileFsAttributeInformation] contains a FILE_FS_ATTRIBUTE_INFORMATION data element.");

            // if fileFsAttributeInformation.FileSystemAttributes  contains FILE_FILE_COMPRESSION or FILE_VOLUME_IS_COMPRESSED
            if ((fileFsAttributeInformation.FileSystemAttributes & FileSystemAttributes_Values.FILE_FILE_COMPRESSION) == FileSystemAttributes_Values.FILE_FILE_COMPRESSION ||
                (fileFsAttributeInformation.FileSystemAttributes & FileSystemAttributes_Values.FILE_VOLUME_IS_COMPRESSED) == FileSystemAttributes_Values.FILE_VOLUME_IS_COMPRESSED)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1703 Actual FileSystemAttributes: {0}",
                    fileFsAttributeInformation.FileSystemAttributes);
                //
                // Verify requirement 1703
                //
                // Incompatible means FILE_FILE_COMPRESSION and FILE_VOLUME_IS_COMPRESSED cannot both apear in FileSystemAttrubutes
                uint fileSystemAttributesBitUsed = (uint)(FileSystemAttributes_Values.FILE_VOLUME_IS_COMPRESSED | FileSystemAttributes_Values.FILE_FILE_COMPRESSION);

                // Verify the value of FileSystemAttributes
                bool isVerifyR1703 = (
                    (((uint)fileFsAttributeInformation.FileSystemAttributes & fileSystemAttributesBitUsed) != (uint)fileFsAttributeInformation.FileSystemAttributes));

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1703,
                    1703,
                    @"[In FILE_FS_ATTRIBUTE_INFORMATION]FileSystemAttributes (4 bytes):  This flag[FILE_FILE_COMPRESSION 0x00000010] is incompatible with the FILE_VOLUME_IS_COMPRESSED flag.");
            }
           

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1709  MaximumComponentNameLength: {0}",
                fileFsAttributeInformation.MaximumComponentNameLength);
            //
            // Verify requirement 1709
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(Int32),
                fileFsAttributeInformation.MaximumComponentNameLength.GetType(),
                1709,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]MaximumComponentNameLength (4 bytes):  A 32-bit signed integer that contains the maximum file name component length, in bytes, supported by the specified file system.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1710 Actual MaximumComponentNameLength: {0}",
                fileFsAttributeInformation.MaximumComponentNameLength);
            //
            // Verify requirement 1710
            //
            Site.CaptureRequirementIfIsTrue(
                fileFsAttributeInformation.MaximumComponentNameLength >= 0,
                1710,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]MaximumComponentNameLength (4 bytes): The value of this field MUST be greater than 0.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1711 Actual FileSystemNameLength: {0}, Expected FileSystemNameLength: {1}.",
                fileFsAttributeInformation.FileSystemNameLength, fileFsAttributeInformation.FileSystemName.Length);
            //
            // Verify requirement 1711
            //
            // Verify the type and the value of the FileSystemNameLength
            bool isVerifyR1711 = (
                fileFsAttributeInformation.FileSystemNameLength.GetType() == typeof(UInt32) &
                fileFsAttributeInformation.FileSystemNameLength == fileFsAttributeInformation.FileSystemName.Length);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1711,
                1711,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]FileSystemNameLength (4 bytes):  A 32-bit unsigned integer that contains the length, in bytes, of the file system name in the FileSystemName field.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1712");
            //
            // Verify requirement 1712
            //
            Site.CaptureRequirementIfAreEqual<int>(
                fileFsAttributeInformation.FileSystemName.Length,
                (int)fileFsAttributeInformation.FileSystemNameLength,
                1712,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]FileSystemNameLength (4 bytes):  The value of this field MUST be greater than 0.");

            // As the rs is informative, we capture this rs directly
            Site.CaptureRequirement(
                1713,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]FileSystemName (variable):  
                A variable-length Unicode field containing the name of the file system.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1716 ");
            
            //
            // Verify requirement 1716
            //
            Site.CaptureRequirementIfAreEqual<int>(
                (int)fileFsAttributeInformation.FileSystemNameLength,
                fileFsAttributeInformation.FileSystemName.Length,
                1716,
                @"[In FILE_FS_ATTRIBUTE_INFORMATION]FileSystemName (variable): This field MUST be handled as a sequence of FileSystemNameLength bytes.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1697 FileSystemAttributes is : {0}",
                fileFsAttributeInformation.FileSystemAttributes );
            
            //
            // Verify requirement 1697
            //
            // Only get the FILE_VOLUME_IS_COMPRESSED bit in the FileSystemAttributes.
            uint bitBothSet = (uint)(FileSystemAttributes_Values.FILE_FILE_COMPRESSION | FileSystemAttributes_Values.FILE_VOLUME_IS_COMPRESSED);

            bool isVerifyR1697 = (((uint)fileFsAttributeInformation.FileSystemAttributes & bitBothSet) != bitBothSet);
            // Check FILE_VOLUME_IS_COMPRESSED bit and FILE_FILE_COMPRESSION bit are not set at the same time.
            Site.CaptureRequirementIfIsTrue(
                isVerifyR1697,
                1697,
                 @"[In FILE_FS_ATTRIBUTE_INFORMATION]FileSystemAttributes (4 bytes): This flag[FILE_VOLUME_IS_COMPRESSED 0x00008000] is incompatible with the FILE_FILE_COMPRESSION flag.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileFsControlInformation

        /// <summary>
        /// Verify data type FileFsControlInformation
        /// </summary>
        /// <param name="fileFsControlInformation"> FileFsControlInformation type data </param>
        /// <param name="freeSpaceThreshold"> A 64-bit signed integer that contains the minimum amount of free disk space in bytes that is required for the indexing service to continue to filter documents and merge word lists. </param>
        /// <param name="freeSpaceStopFiltering"> A 64-bit signed integer that contains the minimum amount of free disk space in bytes that is required for the content indexing service to continue filtering. </param>
        /// <param name="defaultQuotaLimit"> A 64-bit signed integer that contains the default per-user disk quota limit in bytes for the volume. </param>
        public void VerifyDataTypeFileFsControlInformationForOld(
            FileFsControlInformation fileFsControlInformation,
           // long freeSpaceThreshold,
            long defaultQuotaLimit)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1720 ");
            //
            // Verify requirement 1720
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(FileFsControlInformation),
                fileFsControlInformation.GetType(),
                1720,
                @"The message[FileFsControlInformation] contains a FILE_FS_CONTROL_INFORMATION data element.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1734 Actual DefaultQuotaLimit: {0}, Expected DefaultQuotaLimit: {1}.",
                fileFsControlInformation.DefaultQuotaLimit, defaultQuotaLimit);
            //
            // Verify requirement 1734
            //
            // Verify the type and the value of the DefaultQuotaLimit
            bool isVerifyR1734 = (
                fileFsControlInformation.DefaultQuotaLimit.GetType() == typeof(Int64) &&
                fileFsControlInformation.DefaultQuotaLimit == (ulong)defaultQuotaLimit);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1734,
                1734,
                @"[In FILE_FS_CONTROL_INFORMATION]DefaultQuotaLimit (8 bytes):  A 64-bit signed integer that contains 
                the default per-user disk quota limit in bytes for the volume.");
           
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1736 actual FileSystemControlFlags is : {0}",
                fileFsControlInformation.FileSystemControlFlags);
            //
            // Verify requirement 1736
            //
            // Verify the type of FileSystemControlFlags
            // And for the value of it, we'll verify in the next rs
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(FileSystemControlFlags_Values),
                fileFsControlInformation.FileSystemControlFlags.GetType(),
                1736,
                @"[In FILE_FS_CONTROL_INFORMATION]FileSystemControlFlags (4 bytes):  
                A 32-bit unsigned integer that contains a bitmask of flags that control quota enforcement and 
                logging of user-related quota events on the volume.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1737 Actual FileSystemControlFlags: {0}.",
                fileFsControlInformation.FileSystemControlFlags);
            //
            // Verify requirement 1737
            //
            // As the type of the FileSystemControlFlags is verified above, we didn't here again
            // Verify the valid value of FileSystemControlFlags
            const uint FileSystemControlFlagsBitUsed = (
                0x00000008 | 0x00000020 | 0x00000010 | 0x00000080 | 0x00000040 | 0x00000002 |
                0x00000001 | 0x00000100 | 0x00000200);

            // FileSystemControlFlagsBitUnUsed contains the bit that FileSystemControlFlags unused 
            const uint fileSystemControlFlagsBitUnUsed = ~FileSystemControlFlagsBitUsed;

            bool isVerifyR1737 = (
                ((uint)fileFsControlInformation.FileSystemControlFlags & fileSystemControlFlagsBitUnUsed) == 0x00000000);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1737,
                1737,
                @"[In FILE_FS_CONTROL_INFORMATION]FileSystemControlFlags (4 bytes):  The following bit flags
                [FILE_VC_CONTENT_INDEX_DISABLED 0x00000008, FILE_VC_LOG_QUOTA_LIMIT 0x00000020, 
                FILE_VC_LOG_QUOTA_THRESHOLD 0x00000010, FILE_VC_LOG_VOLUME_LIMIT 0x00000080, 
                FILE_VC_LOG_VOLUME_THRESHOLD 0x00000040, FILE_VC_QUOTA_ENFORCE 0x00000002, 
                FILE_VC_QUOTA_TRACK 0x00000001, FILE_VC_QUOTAS_INCOMPLETE 0x00000100, 
                FILE_VC_QUOTAS_REBUILDING 0x00000200] are valid in any combination.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1739 the FileSystemControlFlags is {0}",
                fileFsControlInformation.FileSystemControlFlags);
            //
            // Verify requirement 1739
            //
            const uint bitUsed_r1739 =
                (uint)FileSystemControlFlags_Values.FILE_VC_CONTENT_INDEX_DISABLED |
                (uint)FileSystemControlFlags_Values.FILE_VC_LOG_QUOTA_LIMIT |
                (uint)FileSystemControlFlags_Values.FILE_VC_LOG_QUOTA_THRESHOLD |
                (uint)FileSystemControlFlags_Values.FILE_VC_LOG_VOLUME_LIMIT |
                (uint)FileSystemControlFlags_Values.FILE_VC_LOG_VOLUME_THRESHOLD |
                (uint)FileSystemControlFlags_Values.FILE_VC_QUOTA_ENFORCE |
                (uint)FileSystemControlFlags_Values.FILE_VC_QUOTA_TRACK |
                (uint)FileSystemControlFlags_Values.FILE_VC_QUOTAS_INCOMPLETE |
                (uint)FileSystemControlFlags_Values.FILE_VC_QUOTAS_REBUILDING;

            const uint bitUnUsed_r1739 = ~bitUsed_r1739;

            Site.CaptureRequirementIfAreEqual<uint>(
                0x00000000,
                bitUnUsed_r1739 & (uint)fileFsControlInformation.FileSystemControlFlags,
                1739,
                @"[In FILE_FS_CONTROL_INFORMATION]FileSystemControlFlags (4 bytes):  Bits not defined in the 
                following table[FILE_VC_CONTENT_INDEX_DISABLED 0x00000008, FILE_VC_LOG_QUOTA_LIMIT 0x00000020, 
                FILE_VC_LOG_QUOTA_THRESHOLD 0x00000010, FILE_VC_LOG_VOLUME_LIMIT 0x00000080, 
                FILE_VC_LOG_VOLUME_THRESHOLD 0x00000040, FILE_VC_QUOTA_ENFORCE 0x00000002, FILE_VC_QUOTA_TRACK 
                0x00000001, FILE_VC_QUOTAS_INCOMPLETE 0x00000100, FILE_VC_QUOTAS_REBUILDING 0x00000200] MUST be 
                ignored.<83><84>");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileFsFullSizeInformation

        /// <summary>
        /// Verify data type FileFsFullSizeInformation
        /// </summary>
        /// <param name="fileFsFullSizeInformation"> FileFsFullSizeInformation type data </param>
        /// <param name="bytesPerSector"> A 32-bit unsigned integer that contains the number of bytes in each sector. </param>
        public void VerifyDataTypeFileFsFullSizeInformation(
            FileFsFullSizeInformation fileFsFullSizeInformation,
            uint bytesPerSector)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1762 ");
            //
            // Verify requirement 1762
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(FileFsFullSizeInformation),
                fileFsFullSizeInformation.GetType(),
                1762,
                @"The message[FileFsFullSizeInformation] contains a FILE_FS_FULL_SIZE_INFORMATION data element.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1764 Actual TotalAllocationUnits: {0}",
                fileFsFullSizeInformation.TotalAllocationUnits);
            //
            // Verify requirement 1764
            //
            // Verify the type and the value of the TotalAllocationUnits
            bool isVerifyR1764 = (
                fileFsFullSizeInformation.TotalAllocationUnits.GetType() == typeof(Int64) &&
                Marshal.SizeOf(fileFsFullSizeInformation.TotalAllocationUnits) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1764,
                1764,
                @"[In FILE_FS_FULL_SIZE_INFORMATION]TotalAllocationUnits (8 bytes):  
                A 64-bit signed integer that contains the total number of allocation units on the volume 
                that are available to the user associated with the calling thread.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1765 Actual TotalAllocationUnits: {0}",
                fileFsFullSizeInformation.TotalAllocationUnits);
            //
            // Verify requirement 1765
            //
            Site.CaptureRequirementIfIsTrue(
                fileFsFullSizeInformation.TotalAllocationUnits >= 0,
                1765,
                @"[In FILE_FS_FULL_SIZE_INFORMATION]TotalAllocationUnits (8 bytes): The value of this field MUST be 
                greater than or equal to 0.<85>");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1766 Actual CallerAvailableAllocationUnits: {0}",
                fileFsFullSizeInformation.CallerAvailableAllocationUnits);
            //
            // Verify requirement 1766
            //
            // Verify the type and the value of the CallerAvailableAllocationUnits 
            bool isVerifyR1766 = (
                fileFsFullSizeInformation.CallerAvailableAllocationUnits.GetType() == typeof(Int64) &&
                Marshal.SizeOf(fileFsFullSizeInformation.CallerAvailableAllocationUnits) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1766,
                1766,
                @"[In FILE_FS_FULL_SIZE_INFORMATION]CallerAvailableAllocationUnits (8 bytes):  
                A 64-bit signed integer that contains the total number of free allocation units on the volume that 
                are available to the user associated with the calling thread.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1767 Actual CallerAvailableAllocationUnits: {0}",
                fileFsFullSizeInformation.CallerAvailableAllocationUnits);
            //
            // Verify requirement 1767
            //
            Site.CaptureRequirementIfIsTrue(
                fileFsFullSizeInformation.CallerAvailableAllocationUnits >= 0,
                1767,
                @"[In FILE_FS_FULL_SIZE_INFORMATION]CallerAvailableAllocationUnits (8 bytes): 
                The value of this field MUST be greater than or equal to 0.<86>");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1768 Actual ActualAvailableAllocationUnits: {0}",
                fileFsFullSizeInformation.ActualAvailableAllocationUnits);
            //
            // Verify requirement 1768
            //
            // Verify the type and the value of the actualAvailableAllocationUnits
            bool isVerifyR1768 = (
                fileFsFullSizeInformation.ActualAvailableAllocationUnits.GetType() == typeof(Int64) &&
                Marshal.SizeOf(fileFsFullSizeInformation.ActualAvailableAllocationUnits) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1768,
                1768,
                @"[In FILE_FS_FULL_SIZE_INFORMATION]ActualAvailableAllocationUnits (8 bytes):  
                A 64-bit signed integer that contains the total number of free allocation units on the volume.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1769 Actual ActualAvailableAllocationUnits: {0}.",
                fileFsFullSizeInformation.ActualAvailableAllocationUnits);
            //
            // Verify requirement 1769
            //
            Site.CaptureRequirementIfIsTrue(
                fileFsFullSizeInformation.ActualAvailableAllocationUnits >= 0,
                1769,
                @"[In FILE_FS_FULL_SIZE_INFORMATION]ActualAvailableAllocationUnits (8 bytes): 
                The value of this field MUST be greater than or equal to 0.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1770 Actual SectorsPerAllocationUnit: {0}",
                fileFsFullSizeInformation.SectorsPerAllocationUnit);
            //
            // Verify requirement 1770
            //
            // Verify the type and the value of the SectorsPerAllocationUnit
            bool isVerifyR1770 = (
                fileFsFullSizeInformation.SectorsPerAllocationUnit.GetType() == typeof(UInt32) &&
                Marshal.SizeOf(fileFsFullSizeInformation.SectorsPerAllocationUnit) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1770,
                1770,
                @"[In FILE_FS_FULL_SIZE_INFORMATION]SectorsPerAllocationUnit (4 bytes):  
                A 32-bit unsigned integer that contains the number of sectors in each allocation unit.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1771 Actual BytesPerSector: {0}, Expected BytesPerSector: {1}.",
                fileFsFullSizeInformation.BytesPerSector, bytesPerSector);
            //
            // Verify requirement 1771
            //
            // Verify the type and the value of the BytesPerSector
            bool isVerifyR1771 = (
                fileFsFullSizeInformation.BytesPerSector.GetType() == typeof(UInt32) &&
                fileFsFullSizeInformation.BytesPerSector == bytesPerSector);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1771,
                1771,
                @"[In FILE_FS_FULL_SIZE_INFORMATION]BytesPerSector (4 bytes):  
                A 32-bit unsigned integer that contains the number of bytes in each sector.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileFsObjectIdInformation

        /// <summary>
        /// Verify data type FileFsObjectIdInformation
        /// </summary>
        /// <param name="fileFsObjectIdInformation"> FileFsObjectIdInformation type data </param>
        /// <param name="isNoExInfoWritten"> If no extended information has been written for this file system volume </param>
        public void VerifyDataTypeFileFsObjectIdInformation(
            FileFsObjectIdInformation fileFsObjectIdInformation,
            bool isNoExInfoWritten)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1780");
            //
            // Verify requirement 1780
            //
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(FileFsObjectIdInformation),
                fileFsObjectIdInformation.GetType(),
                1780,
                @"The message[FileFsObjectIdInformation] contains a FILE_FS_OBJECTID_INFORMATION data element.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1782 Actual ObjectId is : {0}",
                fileFsObjectIdInformation.ObjectId);
            //
            // Verify requirement 1782
            //
            // Verify the type and the size of the ObjectId
            // As the ObjectId is defined by users, so it's unable for us to check if the id 
            // really identifies the file system volume on the disk
            Site.CaptureRequirementIfAreEqual<Type>(
                 typeof(Guid),
                 fileFsObjectIdInformation.ObjectId.GetType(),
                 1782,
                 @"[In FILE_FS_OBJECTID_INFORMATION]ObjectId (16 bytes):  
                 A 16-byte GUID that identifies the file system volume on the disk.");
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1784 Actual ExtendedInfo is : {0}",
                fileFsObjectIdInformation.ExtendedInfo);
            //
            // Verify requirement 1784
            //
            // As the extended information on the file system volume is not clear in this rs
            // We only verify the size of the ExtendedInfo here
            Site.CaptureRequirementIfAreEqual<int>(
                48,
                fileFsObjectIdInformation.ExtendedInfo.Length,
                1784,
                @"[In FILE_FS_OBJECTID_INFORMATION]ExtendedInfo (48 bytes):  A 48-byte value containing extended 
                information on the file system volume.");

            // If no extended information has been written for this file system volume
            if (isNoExInfoWritten)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1785 Actual size of ExtendedInfo: {0}, Expected size of ExtendedInfo: {1}.",
                    fileFsObjectIdInformation.ExtendedInfo.Length, 48);
                //
                // Verify requirement 1785
                //
                // Verify the value of the ExtendedInfo, when no extended information has been written for this file 
                // system volume
                bool isVerifyR1785 = true;
                for (int i = 0; i < fileFsObjectIdInformation.ExtendedInfo.Length; ++i)
                {
                    if (fileFsObjectIdInformation.ExtendedInfo[i] != 0x00)
                    {
                        isVerifyR1785 = false;
                        break;
                    }
                }

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR1785,
                    1785,
                    @"[In FILE_FS_OBJECTID_INFORMATION]ExtendedInfo (48 bytes):  
                    If no extended information has been written for this file system volume, 
                    the server MUST return 48 bytes of 0x00 in this field.<88>");
            }
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1783 Actual type of ObjectId: {0}",
                BytesToString(fileFsObjectIdInformation.ObjectId.ToByteArray()));
            //
            // Verify requirement 1783
            //
            // Since the ObjectId is not required to be unique on the system.
            // We only verify the size of the obejctId
            Site.CaptureRequirementIfAreEqual<int>(
                16,
                Marshal.SizeOf(fileFsObjectIdInformation.ObjectId),
                1783,
                @"[In FILE_FS_OBJECTID_INFORMATION]ObjectId (16 bytes):  
                This value is not required to be unique on the system.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileEaInformation

        /// <summary>
        /// Verify the data type FileEaInformation in TD 2.4.12
        /// </summary>
        /// <param name="fileEaInformation"> FileEaInformation type data </param>
        /// <param name="EAlength"> Length of extended attributes (EA) for the file (in bytes) </param>
        public void VerifyDataTypeFileEaInformation(
            FileEaInformation fileEaInformation,
            uint EAlength)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debugging information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1157, Actual EaSize: {0}, Expected EaLength: {1};",
                fileEaInformation.EaSize, EAlength);
            //
            // Verify MS-FSCC requirement 1157
            //
            // verfiy the type of the EaSize and the content.
            bool isVerifyR1157 = (fileEaInformation.EaSize.GetType() == typeof(UInt32) &&
                                     fileEaInformation.EaSize == EAlength);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1157,
                1157,
                @"[In FILE_EA_INFORMATION]EaSize (4 bytes):  A 32-bit unsigned integer that contains the 
                combined length, in bytes, of the extended attributes (EA) for the file.");
            // FILE_EA_INFORMATION has been definned as section 2.4.12. 
            // As the rs in section 2.4.12 are verified
            // Verify type of the EaInformation specified in section 2.4.12
            Site.CaptureRequirement(
                981,
                @"[In FileAllInformation]EaInformation (4 bytes):  
                A FILE_EA_INFORMATION structure specified in section 2.4.12.");

            // As all the elements in the FILE_EA_INFORMATION have been verified above
            // This rs will be captured directly
            Site.CaptureRequirement(
                1156,
                @"[In FileEaInformation]The FILE_EA_INFORMATION data element is as follows:[EaSize].");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion


        #region Verify data type FileBothDirectoryInformation

        public void VerifyDataTypeFileBothDirectoryInformationOfFSCC(
            FileBothDirectoryInformation FileBothDirectoryInformationData,
            bool isNoOtherEntriesFollow)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";


            //
            // Add the debug information 
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1054 ");

            //
            // Verify MS-FSCC requirement 1054
            //
            // As the infra parses the packet by using the offset info, it's useless for us to verify if it's 
            // valid again, this rs will be only verifed the size here.
            const int sizeOfNextEntryOffset = 4;
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfNextEntryOffset,
                Marshal.SizeOf(FileBothDirectoryInformationData.FileCommonDirectoryInformation.NextEntryOffset),
                1054,
                @"[In FILE_BOTH_DIR_INFORMATION]NextEntryOffset (4 bytes):  A 32-bit unsigned integer that 
                contains the byte offset from the beginning of this entry, at which the next 
                FILE_BOTH_DIR_INFORMATION entry is located, if multiple entries are present in a buffer.");

            // If no other entries follow this element.
            if (isNoOtherEntriesFollow)
            {
               
                //
                // Add the debug information 
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1055, actual NextEntryOffset:{0}.",
                     FileBothDirectoryInformationData.FileCommonDirectoryInformation.NextEntryOffset);
                //
                // Verify MS-FSCC requirement 1055
                //
                Site.CaptureRequirementIfIsTrue(
                    FileBothDirectoryInformationData.FileCommonDirectoryInformation.NextEntryOffset == 0 &&
                    Marshal.SizeOf(FileBothDirectoryInformationData.FileCommonDirectoryInformation.NextEntryOffset) == sizeOfNextEntryOffset,
                    1055,
                    @"[In FILE_BOTH_DIR_INFORMATION]NextEntryOffset (4 bytes):  This member is zero if no other 
                    entries follow this one.");
            }

            // if multiple entries are present in a buffer
            else
            {
               

                //
                // Add the debug information 
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSCC_R1056, actual NextEntryOffset:{0}.",
                     FileBothDirectoryInformationData.FileCommonDirectoryInformation.NextEntryOffset);
                //
                // Verify MS-FSCC requirement 1056
                //
                // This rs is informative in fact, and will be only verified the size, as the FILE_BOTH_DIR_INFORMATION structure we used
                // is parsed according to this offset information.
                Site.CaptureRequirementIfAreEqual<int>(
                    sizeOfNextEntryOffset,
                    Marshal.SizeOf(FileBothDirectoryInformationData.FileCommonDirectoryInformation.NextEntryOffset),
                    1056,
                    @"[In FILE_BOTH_DIR_INFORMATION]NextEntryOffset (4 bytes):  An implementation MUST use this 
                    value to determine the location of the next entry (if multiple entries are present in a 
                    buffer).");
            }

            #region this rs will be coverd when all the elements have been verified
            //
            // Verify MS-FSCC requirement 1053
            //

            //
            // Add the debug information 
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R1053 ");

            // As all the elements in the structure have been verified above.
            // This rs will be captured directly.
            Site.CaptureRequirement(
                    1053,
                    @"[In FileBothDirectoryInformation]The FILE_BOTH_DIR_INFORMATION data element is as follows
                    [NextEntryOffset, FileIndex, CreationTime, ..., LastAccessTime, ..., LastWriteTime, ..., 
                    ChangeTime, ..., EndOfFile, ..., AllocationSize, ..., FileAttributes, FileNameLength, EaSize, 
                    ShortNameLength, Reserved, ShortName, ..., ..., ..., ..., ..., ..., FileName (variable), ...].
                    ");
            #endregion

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify data type FileBothDirectoryInformation
        /// <summary>
        /// Verify the FILE_BOTH_DIR_INFORMATION, which is specified in TD 2.4.8
        /// </summary>
        /// <param name="fileBothDirectoryInformation"> FileBothDirectoryInformation type data </param>
        /// <param name="allocationSize">A 64-bit signed integer that contains the file allocation size, in bytes</param>
        /// <param name="endOfFilePositon">A 64-bit signed integer that contains the absolute new end-of-file position as a byte offset from the start of the file</param>
        /// <param name="isNoEntries">no other entries follow this one[FILE_ID_BOTH_DIR_INFORMATION entry</param>
        public void VerifyFileBothDirectoryInformationOfFSCC(
            FileBothDirectoryInformation fileBothDirectoryInformation,
            long allocationSize
            //bool isNoEntries
            )
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1061 actual  CreationTime: {0}",
                fileBothDirectoryInformation.FileCommonDirectoryInformation.CreationTime.ToString());

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1061
            //
            int sizeOfCreationTime = 8;
            bool isVerifyR1061 = (
                typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME) == fileBothDirectoryInformation.FileCommonDirectoryInformation.CreationTime.GetType() &&
                (Marshal.SizeOf(fileBothDirectoryInformation.FileCommonDirectoryInformation.CreationTime) == sizeOfCreationTime));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1061,
                1061,
                @"[In FILE_BOTH_DIR_INFORMATION]CreationTime (8 bytes):  A 64-bit signed integer that contains the time when the file was created.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1062 ");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1062
            //

            Site.CaptureRequirementIfAreEqual<Type>(
                 typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME),
                fileBothDirectoryInformation.FileCommonDirectoryInformation.CreationTime.GetType(),
                1062,
                @"[In FILE_BOTH_DIR_INFORMATION]CreationTime (8 bytes): All dates and times are in absolute system-time format, 
                which is represented as a FILETIME (section 2.1.1) structure.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1063 actual  CreationTime: {0}",
                fileBothDirectoryInformation.FileCommonDirectoryInformation.CreationTime.ToString());

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1063
            //
            bool isVerifyR1063 = (
                fileBothDirectoryInformation.FileCommonDirectoryInformation.LastWriteTime.dwHighDateTime >= 0 &&
                fileBothDirectoryInformation.FileCommonDirectoryInformation.LastWriteTime.dwLowDateTime >= 0 &&
                Marshal.SizeOf(fileBothDirectoryInformation.FileCommonDirectoryInformation.LastWriteTime) == sizeOfCreationTime);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1063,
                1063,
                @"[In FILE_BOTH_DIR_INFORMATION]CreationTime (8 bytes): This value MUST be greater than or equal to 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1070 Actual EndOfFile: {0}",
                fileBothDirectoryInformation.FileCommonDirectoryInformation.EndOfFile);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1070
            //
            bool isVerifyR1070 = (
                fileBothDirectoryInformation.FileCommonDirectoryInformation.EndOfFile.GetType() == typeof(Int64) &&
                Marshal.SizeOf(fileBothDirectoryInformation.FileCommonDirectoryInformation.EndOfFile) == 8);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1070,
                1070,
                @"[In FILE_BOTH_DIR_INFORMATION]EndOfFile (8 bytes):  A 64-bit signed integer that contains the absolute 
                new end-of-file position as a byte offset from the start of the file.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1071 EndOfFile: {0}",
                fileBothDirectoryInformation.FileCommonDirectoryInformation.EndOfFile);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1071
            //
            const int sizeOfEndOfFile = 8;

            // As we parse the file by using the EndofFile, the sucessfully parsing have verified the actual value, 
            // only verify the size of endoffile
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfEndOfFile,
                Marshal.SizeOf(fileBothDirectoryInformation.FileCommonDirectoryInformation.EndOfFile),
                1071,
                @"[In FILE_BOTH_DIR_INFORMATION]EndOfFile (8 bytes):  EndOfFile specifies the offset to the byte 
                immediately following the last valid byte in the file.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1073 EndOfFile: {0}",
                fileBothDirectoryInformation.FileCommonDirectoryInformation.EndOfFile);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1073
            //

            Site.CaptureRequirementIfIsTrue(
                fileBothDirectoryInformation.FileCommonDirectoryInformation.EndOfFile >= 0,
                1073,
                @"[In FILE_BOTH_DIR_INFORMATION]EndOfFile (8 bytes):  The value of this field MUST be greater than or equal to 0.");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1149
            //
            Site.CaptureRequirementIfAreEqual<uint>(
                fileBothDirectoryInformation.FileNameLength,
                (uint)fileBothDirectoryInformation.FileName.Length,
                1149,
                @"[In FILE_BOTH_DIR_INFORMATION]FileName (variable):  This field MUST be handled as a sequence of FileNameLength bytes.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1079 FileNameLength: {0}, type: {1}",
                fileBothDirectoryInformation.FileNameLength, typeof(UInt32));

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1079
            //
            bool isVerifyR1079 = (
                fileBothDirectoryInformation.FileNameLength.GetType() == typeof(UInt32) &&
                fileBothDirectoryInformation.FileNameLength == fileBothDirectoryInformation.FileName.Length);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1079,
                1079,
                @"[In FILE_BOTH_DIR_INFORMATION]FileNameLength (4 bytes):  A 32-bit unsigned integer that 
                contains the length, in bytes, of the FileName field.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1064 actual LastAccessTime: {0}",
                fileBothDirectoryInformation.FileCommonDirectoryInformation.LastAccessTime.ToString());

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1064
            //
            // Verify the LastAccessTime's type and value
            const int sizeOfLastAccessTime = 8;
            bool isVerifyR1064 = (
                fileBothDirectoryInformation.FileCommonDirectoryInformation.LastAccessTime.GetType() == typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME) &&
                (Marshal.SizeOf(fileBothDirectoryInformation.FileCommonDirectoryInformation.LastAccessTime) == sizeOfLastAccessTime));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1064,
                1064,
                @"[In FILE_BOTH_DIR_INFORMATION]LastAccessTime (8 bytes):  A 64-bit signed integer that contains the last time 
                the file was accessed in the format of a FILETIME structure.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1065 actual LastAccessTime: {0}",
                fileBothDirectoryInformation.FileCommonDirectoryInformation.LastAccessTime.ToString());

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1065
            //
            bool isVerifyR1065 = (
                fileBothDirectoryInformation.FileCommonDirectoryInformation.LastAccessTime.dwHighDateTime >= 0 &&
                fileBothDirectoryInformation.FileCommonDirectoryInformation.LastAccessTime.dwLowDateTime >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1065,
                1065,
                @"[In FILE_BOTH_DIR_INFORMATION]LastAccessTime (8 bytes): This value MUST be greater than or equal to 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1066 actual LastWriteTime: {0}",
                fileBothDirectoryInformation.FileCommonDirectoryInformation.LastWriteTime.ToString());

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1066
            //
            const int sizeOfLastWriteTime = 8;
            bool isVerifyR1066 = (
                fileBothDirectoryInformation.FileCommonDirectoryInformation.LastWriteTime.GetType() == typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME) &&
                Marshal.SizeOf(fileBothDirectoryInformation.FileCommonDirectoryInformation.LastWriteTime) == sizeOfLastWriteTime);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1066,
                1066,
                @"[In FILE_BOTH_DIR_INFORMATION]LastWriteTime (8 bytes): A 64-bit signed integer that contains the last time 
                information was written to the file in the format of a FILETIME structure.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1067 actual LastWriteTime: {0}",
                fileBothDirectoryInformation.FileCommonDirectoryInformation.LastWriteTime.ToString());

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1067
            //
            bool isVerifyR1067 = (
                fileBothDirectoryInformation.FileCommonDirectoryInformation.LastWriteTime.dwHighDateTime >= 0 &&
                fileBothDirectoryInformation.FileCommonDirectoryInformation.LastWriteTime.dwLowDateTime >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1067,
                1067,
                @"[In FILE_BOTH_DIR_INFORMATION]LastWriteTime (8 bytes):  This value MUST be greater than or equal to 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1068 actual  ChangeTime: {0}",
                fileBothDirectoryInformation.FileCommonDirectoryInformation.ChangeTime.ToString());

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1068
            //
            const int sizeOfChangeTime = 8;
            bool isVerifyR1068 = (
                fileBothDirectoryInformation.FileCommonDirectoryInformation.ChangeTime.GetType() == typeof(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc.FILETIME) &&
                Marshal.SizeOf(fileBothDirectoryInformation.FileCommonDirectoryInformation.ChangeTime) == sizeOfChangeTime);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1068,
                1068,
                @"[In FILE_BOTH_DIR_INFORMATION]ChangeTime (8 bytes):  A 64-bit signed integer that contains the last time 
                the file was changed in the format of a FILETIME structure.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1069 Actual ChangeTime: {0}",
                fileBothDirectoryInformation.FileCommonDirectoryInformation.ChangeTime.ToString());

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1069
            //
            bool isVerifyR1069 = (
                fileBothDirectoryInformation.FileCommonDirectoryInformation.ChangeTime.dwHighDateTime >= 0 &&
                fileBothDirectoryInformation.FileCommonDirectoryInformation.ChangeTime.dwLowDateTime >= 0);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1069,
                1069,
                @"[In FILE_BOTH_DIR_INFORMATION]ChangeTime (8 bytes): This value MUST be greater than or equal to 0.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1074 Actual AllocationSize: {0},expected AllocationSize: {1}",
                fileBothDirectoryInformation.FileCommonDirectoryInformation.AllocationSize, allocationSize);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1074
            //
            bool isVerifyR1074 = (
                typeof(Int64) == fileBothDirectoryInformation.FileCommonDirectoryInformation.AllocationSize.GetType() &&
                fileBothDirectoryInformation.FileCommonDirectoryInformation.AllocationSize == allocationSize);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR1074,
                1074,
                @"[In FILE_BOTH_DIR_INFORMATION]AllocationSize (8 bytes):  A 64-bit signed integer that contains the file allocation size, in bytes.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R1086 Actual Reserved: {0}",
                fileBothDirectoryInformation.Reserved);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R1086
            //
            // This field can contain any valude, so we just verify its size
            const int sizeOfReserved = 1;
            Site.CaptureRequirementIfAreEqual<int>(
                sizeOfReserved,
                Marshal.SizeOf(fileBothDirectoryInformation.Reserved),
                1086,
                @"[In FILE_BOTH_DIR_INFORMATION]Reserved (1 byte): This field can contain any value.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion


        #region Verify data type FSCTL_FILESYSTEM_GET_STATISTICS_Reply in section 2.3.8
        /// <summary>
        /// Verify date type FSCTL_FILESYSTEM_GET_STATISTICS_Reply
        /// </summary>
        /// <param name="fsctlFileSystemGetStatisticsReply"> A FSCTL_FILESYSTEM_GET_STATISTICS_Reply date type </param>
        /// <param name="processors"> The number of processors </param>
        public void VerifyFsctlFileSystemGetStatisticsReply(
            FSCTL_FILESYSTEM_GET_STATISTICS_Reply fsctlFileSystemGetStatisticsReply)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            // If value [FILESYSTEM_STATISTICS_TYPE_NTFS 0x0001] is set
            if (((uint)fsctlFileSystemGetStatisticsReply.FileSystemType & (uint)FileSystemType_Values.FILESYSTEM_STATISTICS_TYPE_NTFS) != 0)
            {
                //
                // Verify MS-FSCC requirement: MS-FSCC_R235
                //
                Site.CaptureRequirementIfAreEqual<Type>(
                    typeof(FileSystemType_Values),
                    fsctlFileSystemGetStatisticsReply.FileSystemType.GetType(),
                    235,
                    @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]FileSystemType (2 bytes): If this value
                    [FILESYSTEM_STATISTICS_TYPE_NTFS 0x0001] is set, this structure is followed by an NTFS_STATISTICS structure.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
            "Verify MS-FSCC_R233 FileSystemType: {0}",
            fsctlFileSystemGetStatisticsReply.FileSystemType);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R233
            //
            bool isVerifyR233 = (
                fsctlFileSystemGetStatisticsReply.FileSystemType == FileSystemType_Values.FILESYSTEM_STATISTICS_TYPE_NTFS ||
                fsctlFileSystemGetStatisticsReply.FileSystemType == FileSystemType_Values.FILESYSTEM_STATISTICS_TYPE_FAT ||
                fsctlFileSystemGetStatisticsReply.FileSystemType == FileSystemType_Values.FILESYSTEM_STATISTICS_TYPE_EXFAT);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR233,
                233,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]FileSystemType (2 bytes): This field MUST contain one of the following values:
                [FILESYSTEM_STATISTICS_TYPE_NTFS 0x0001,  FILESYSTEM_STATISTICS_TYPE_FAT 0x0002, FILESYSTEM_STATISTICS_TYPE_EXFAT 0x0003]");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R227
            //
            // Stack according to TD define the Metadata files, when we get it, it like the RS description, so it can be directly verified
            Site.CaptureRequirement(
                227,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]Metadata files are system files that contain information that the file system uses for its internal organization.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R258 Actual MetaDataDiskWrites: {0}",
                fsctlFileSystemGetStatisticsReply.MetaDataDiskWrites);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R258
            //
            bool isVerifyR258 = (
                fsctlFileSystemGetStatisticsReply.MetaDataDiskWrites.GetType() == typeof(UInt32) &&
                Marshal.SizeOf(fsctlFileSystemGetStatisticsReply.MetaDataDiskWrites) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR258,
                258,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]MetaDataDiskWrites (4 bytes):  A 32-bit unsigned integer value containing 
                the number of write operations on metadata files.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R253 Actual MetaDataReadBytes: {0}",
                fsctlFileSystemGetStatisticsReply.MetaDataReadBytes);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R253
            //
            bool isVerifyR253 = (
                fsctlFileSystemGetStatisticsReply.MetaDataReadBytes.GetType() == typeof(UInt32) &&
                Marshal.SizeOf(fsctlFileSystemGetStatisticsReply.MetaDataReadBytes) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR253,
                253,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]MetaDataReadBytes (4 bytes):  A 32-bit unsigned integer value containing the number of bytes read from metadata files.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R252 Actual MetaDataReads: {0}",
                fsctlFileSystemGetStatisticsReply.MetaDataReads);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R252
            //
            bool isVerifyR252 = (
                fsctlFileSystemGetStatisticsReply.MetaDataReads.GetType() == typeof(UInt32) &&
                Marshal.SizeOf(fsctlFileSystemGetStatisticsReply.MetaDataReads) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR252,
                252,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]MetaDataReads (4 bytes):  A 32-bit unsigned integer value containing the number of read operations on metadata files.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R257 Actual MetaDataWriteBytes: {0}",
                fsctlFileSystemGetStatisticsReply.MetaDataWriteBytes);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R257
            //
            bool isVerifyR257 = (
                fsctlFileSystemGetStatisticsReply.MetaDataWriteBytes.GetType() == typeof(UInt32) &&
                Marshal.SizeOf(fsctlFileSystemGetStatisticsReply.MetaDataWriteBytes) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR257,
                257,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]MetaDataWriteBytes (4 bytes):  A 32-bit unsigned integer value containing the number of bytes written to metadata files.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSCC_R242");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R242
            //
            // For the SizeOfCompleteStructure's algorithm, R243 has verified, so we just verify SizeOfCompleteStructure type
            Site.CaptureRequirementIfAreEqual<Type>(
                typeof(UInt32),
                fsctlFileSystemGetStatisticsReply.MetaDataWriteBytes.GetType(),
                242,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]SizeOfCompleteStructure (4 bytes):  A 32-bit unsigned integer value that 
                indicates the size, in bytes, of this structure plus the size of the file system-specific structure that follows this 
                structure, rounded up to a multiple of 64, and then multiplied by the number of processors.");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R229
            //
            // Stack according to TD define statistics structures, when we get it, it like RS describe, so it can be directly verified
            Site.CaptureRequirement(
                229,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]The statistics structures contain fields that may overflow during the server's lifetime. This is by design.");

            //
            // Verify MS-FSCC requirement: MS-FSCC_R226
            //
            // Usually we set userfiles are available, so it can be directly verified
            Site.CaptureRequirement(
                226,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]User files are available for the user.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R246 Actual UserDiskReads: {0}",
                fsctlFileSystemGetStatisticsReply.UserDiskReads);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R246
            //
            bool isVerifyR246 = (
                fsctlFileSystemGetStatisticsReply.UserDiskReads.GetType() == typeof(UInt32) &&
                Marshal.SizeOf(fsctlFileSystemGetStatisticsReply.UserDiskReads) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR246,
                246,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]UserDiskReads (4 bytes):  A 32-bit unsigned integer value containing
                the number of read operations on user files that went to the disk rather than the cache.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R250 Actual UserDiskWrites: {0}",
                fsctlFileSystemGetStatisticsReply.UserDiskWrites);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R250
            //
            bool isVerifyR250 = (
                fsctlFileSystemGetStatisticsReply.UserDiskWrites.GetType() == typeof(UInt32) &&
                Marshal.SizeOf(fsctlFileSystemGetStatisticsReply.UserDiskWrites) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR250,
                250,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]UserDiskWrites (4 bytes):  A 32-bit unsigned integer value containing 
                the number of write operations on user files that went to disk rather than the cache.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R245 Actual UserFileReadBytes: {0}",
                fsctlFileSystemGetStatisticsReply.UserFileReadBytes);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R245
            //
            bool isVerifyR245 = (
                fsctlFileSystemGetStatisticsReply.UserFileReadBytes.GetType() == typeof(UInt32) &&
                Marshal.SizeOf(fsctlFileSystemGetStatisticsReply.UserFileReadBytes) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR245,
                245,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]UserFileReadBytes (4 bytes):  A 32-bit unsigned integer value containing the number of bytes read from user files.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R244 Actual UserFileReads: {0}",
                fsctlFileSystemGetStatisticsReply.UserFileReads);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R244
            //
            bool isVerifyR244 = (
                fsctlFileSystemGetStatisticsReply.UserFileReads.GetType() == typeof(UInt32) &&
                Marshal.SizeOf(fsctlFileSystemGetStatisticsReply.UserFileReads) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR244,
                244,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]UserFileReads (4 bytes):  A 32-bit unsigned integer value containing the number of read operations on user files.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R248 Actual UserFileWrites: {0}",
                fsctlFileSystemGetStatisticsReply.UserFileWrites);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R248
            //
            bool isVerifyR248 = (
                fsctlFileSystemGetStatisticsReply.UserFileWrites.GetType() == typeof(UInt32) &&
                Marshal.SizeOf(fsctlFileSystemGetStatisticsReply.UserFileWrites) == 4);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR248,
                248,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]UserFileWrites (4 bytes):  A 32-bit unsigned integer value containing the number of write operations on user files.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R240 Actual Version: {0},expected Version: {1}",
                fsctlFileSystemGetStatisticsReply.Version, Version_Values.V1);

            //
            // Verify MS-FSCC requirement: MS-FSCC_R240
            //
            bool isVerifyR240 = (
                fsctlFileSystemGetStatisticsReply.Version.GetType() == typeof(Version_Values) &&
                fsctlFileSystemGetStatisticsReply.Version == Version_Values.V1);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR240,
                240,
                @"[In FSCTL_FILESYSTEM_GET_STATISTICS Reply]Version (2 bytes):  16-bit unsigned integer value containing the version.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }
        #endregion

        #region Verify FSCTL_GET_COMPRESSION Reply

        /// <summary>
        /// Verify data type FSCTL_GET_COMPRESSION_Reply
        /// </summary>
        /// <param name="getCompressionResponse"> FsccFsctlGetCompressionResponsePacket type data </param>
        /// <param name="retStatusCode"> the status code that this message returns</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void VerifyDataTypeFSCTLGETCOMPRESSIONReply(
            FsccFsctlGetCompressionResponsePacket getCompressionResponse,
            uint retStatusCode)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            //Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R405 Actual CompressionState: {0};",
                getCompressionResponse.Payload.CompressionState);
            
            //
            // Verify MS-FSCC requirement 405
            //
            //
            // since the rs didn't give the condition of the given values
            // we verify if the CompressionState is in the following values
            //
            bool isVerifyR405 = (
               (getCompressionResponse.Payload.CompressionState == CompressionState_Values.COMPRESSION_FORMAT_DEFAULT ||
                getCompressionResponse.Payload.CompressionState == CompressionState_Values.COMPRESSION_FORMAT_LZNT1 ||
                getCompressionResponse.Payload.CompressionState == CompressionState_Values.COMPRESSION_FORMAT_NONE));

            Site.CaptureRequirementIfIsTrue(
                isVerifyR405,
                405,
                @"[In FSCTL_GET_COMPRESSION Reply]CompressionState (2 bytes):  One of the following 
                standard values[COMPRESSION_FORMAT_NONE 0x0000, COMPRESSION_FORMAT_DEFAULT 0x0001, COMPRESSION_FORMAT_LZNT1 0x0002, All other values] MUST be returned.");

            // As we'll verify if the return status code is valid, and the returned code always exsits,
            // we capture this rs directly            
            Site.CaptureRequirement(
                414,
                @"This message[FSCTL_GET_COMPRESSION Reply] also returns a status code, 
                as specified in [MS-ERREF] section 2.3.");
            //
            //Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug,
                "Verify MS-FSCC_R415 return Status Code: {0}.",
                retStatusCode);
            //
            // Verify MS-FSCC requirement 415
            //
            // Verify if the return status code is valid
            // According to the TD of [MS-ERREF] decribes: STATUS_SUCCESS = 0x00000000
            bool isVerifyR415 = (
                retStatusCode == 0x00000000 || retStatusCode == 0xC000000D || retStatusCode == 0xC0000010);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR415,
                415,
                @"[In FSCTL_GET_COMPRESSION Reply]The status code that is returned directly 
                by the function that processes this FSCTL MUST be STATUS_SUCCESS or one of the following:
                [STATUS_INVALID_PARAMETER 0xC000000D, STATUS_INVALID_DEVICE_REQUEST 0xC0000010].");
            // As the structure CompressionState has been verified above.
            // This rs will be captured directly
            Site.CaptureRequirement(
                403,
                @"The FSCTL_GET_COMPRESSION reply message returns the results of the FSCTL_GET_COMPRESSION request 
                as a 16-bit unsigned integer value that indicates the current compression state of the file or 
                directory.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion

        #region Verify data type FSCTL_QUERY_SPARING_INFO Reply

        /// <summary>
        /// Verify data type FSCTL_QUERY_SPARING_INFO Reply
        /// </summary>
        /// <param name="FSCTL_QUERY_SPARING_INFOReply"> FSCTL_QUERY_SPARING_INFOReply type data </param>
        public void VerifyDataTypeFSCTLQUERYSPARINGINFOReply(
             FSCTL_QUERY_SPARING_INFO_Reply reply)
        {
            Site.DefaultProtocolDocShortName = "MS-FSCC";
            //
            // Add the comment information for debugging
            //
            Site.Log.Add(LogEntryKind.Debug,"Verify MS-FSCC_R705 ");
            //
            // Verify MS-FSCC requirement 705
            //
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(reply.SparingUnitBytes),
                705,
                @"[In FSCTL_QUERY_SPARING_INFO Reply]SparingUnitBytes (4 bytes):  
                The size of a sparing packet and the underlying error check and correction (ECC) 
                block size of the volume.");

            // As we'll verify if the return status code is valid, we capture this rs directly
            Site.CaptureRequirement(
                710,
                @"This message[FSCTL_QUERY_SPARING_INFO Reply] returns a status code, 
                as specified in [MS-ERREF] section 2.3.");

            Site.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
        }

        #endregion
    }
}