// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    #region 2.2.1 Common Data Types

    /// <summary>
    /// This is a 16-bit value in little-endian byte order used to encode a date. 
    /// An SMB_DATE value SHOULD be interpreted as follows. The date is represented 
    /// in the local time zone of the server. This field names below are provided 
    /// for reference only.
    /// </summary>
    public enum SmbDateBitMask : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// The year. Add 1980 to the resulting value to return the actual year
        /// </summary>
        YEAR = 0xFE00,

        /// <summary>
        /// The month. Values range from 1 to 12.
        /// </summary>
        MONTH = 0x01E0,

        /// <summary>
        /// The date. Values range from 1 to 31.
        /// </summary>
        DAY = 0x001F,
    }


    /// <summary>
    /// This is a 16-bit value in little-endian byte order used to encode a date.
    /// An SMB_DATE value SHOULD be interpreted as follows. The date is represented 
    /// in the local time zone of the server. This field names below are provided 
    /// for reference only
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SmbDate
    {
        private ushort date;

        /// <summary>
        /// The year. Add 1980 to the resulting value to return the actual year
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The Year should be in range of [1980, 2099].</exception>
        [SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow")]
        public ushort Year
        {
            get
            {
                return (ushort)((((this.date) & (ushort)SmbDateBitMask.YEAR) >> 9) + 1980);
            }
            set
            {
                this.date = (ushort)(((value - 1980) << 9) + (this.Month << 5) + this.Day);
            }
        }


        /// <summary>
        ///  The month. Values range from 1 to 12.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The Month should be in range of [1, 12].</exception>
        public ushort Month
        {
            get
            {
                return (ushort)((this.date & (ushort)SmbDateBitMask.MONTH) >> 5);
            }
            set
            {
                this.date = (ushort)(((this.Year - 1980) << 9) + (value << 5) + this.Day);
            }
        }


        /// <summary>
        /// The date. Values range from 1 to 31.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The day should be in range of [1, 31].</exception>
        public ushort Day
        {
            get
            {
                return (ushort)(this.date & (ushort)SmbDateBitMask.DAY);
            }
            set
            {
                this.date = (ushort)(((this.Year - 1980) << 9) + (this.Month << 5) + value);
            }
        }


        /// <summary>
        /// Current smb date.
        /// </summary>
        public static SmbDate Now
        {
            get
            {
                SmbDate smbDate = DateTime.Now;
                return smbDate;
            }
        }


        /// <summary>
        /// Implicit convert an DateTime to SmbDate.
        /// </summary>
        /// <param name="dateTime">The date time</param>
        /// <returns>An SmbDate</returns>
        public static implicit operator SmbDate(DateTime dateTime)
        {
            SmbDate smbDate = new SmbDate();
            smbDate.Year = (ushort)dateTime.Year;
            smbDate.Month = (ushort)dateTime.Month;
            smbDate.Day = (ushort)dateTime.Day;
            return smbDate;
        }
    }


    /// <summary>
    /// The SMB_FEA data structure is used in Transaction2 subcommands
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_FEA
    {
        /// <summary>
        /// This field MUST be 0x80
        /// </summary>
        public byte ExtendedAttributeFlag;

        /// <summary>
        /// This field MUST contain the length in bytes of the AttributeName field.
        /// </summary>
        public byte AttributeNameLengthInBytes;

        /// <summary>
        /// This field MUST contain the length in bytes of the ValueName field.
        /// </summary>
        public ushort ValueNameLengthInBytes;

        /// <summary>
        /// This field contains the name, in extended ASCII (OEM) characters,
        /// of an extended file attribute. The length of the name MUST NOT exceed 255 bytes. 
        /// An additional byte is added to store a null byte. This field MAY be interpreted as an OEM_STRING.
        /// </summary>
        [Size("AttributeNameLengthInBytes + 1")]
        public byte[] AttributeName;

        /// <summary>
        /// UCHAR  This field contains the value of an extended file attribute.  
        /// The value is expressed as an array of extended ASCII (OEM) characters. 
        /// This array MUST NOT be null-terminated, and its length MUST NOT exceed 65,535 bytes.
        /// </summary>
        [Size("ValueNameLengthInBytes")]
        public byte[] ValueName;
    }


    /// <summary>
    /// The SMB_GEA data structure is used in Transaction2 subcommand requests to request specific extended attribute
    /// (EA) name/value pairs by name.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_GEA
    {
        /// <summary>
        /// This field MUST contain the length in bytes of the AttributeName field.
        /// </summary>
        public byte AttributeNameLengthInBytes;

        /// <summary>
        /// This field contains the name, in extended ASCII (OEM) characters,
        /// of an extended file attribute. The length of the name MUST NOT exceed 255 bytes. 
        /// An additional byte is added to store a null byte. This field MAY be interpreted as an OEM_STRING.
        /// </summary>
        [Size("AttributeNameLengthInBytes + 1")]
        public byte[] AttributeName;
    }


    /// <summary>
    /// An SMB_ERROR MUST be interpreted in one of two ways, depending on the capabilities negotiated between 
    /// client and server: either as an NTSTATUS value (a 32-bit value in little-endian byte order used to encode 
    /// an error message, as defined in [MS-ERRREF] Section 2.3), or as an SMBSTATUS value (as defined below).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SmbStatus
    {
        /// <summary>
        /// An SMB Error Class Code
        /// </summary>
        public SmbErrorClass ErrorClass;

        /// <summary>
        /// This field is reserved and MUST be ignored by both server and client.
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// An SMB Error Code.
        /// </summary>
        public ushort ErrorCode;


        /// <summary>
        /// Implicit convert an SmbStatus to uint.
        /// </summary>
        /// <param name="status">The smb status</param>
        /// <returns>An uint</returns>
        public static implicit operator uint(SmbStatus status)
        {
            return (uint)((byte)status.ErrorClass | status.Reserved << 8 | status.ErrorCode << 16);
        }
    }


    /// <summary>
    /// An unsigned 16-bit field that defines the basic file attributes supported by the SMB protocol
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum SmbFileAttributes : ushort
    {
        /// <summary>
        /// Normal file
        /// </summary>
        SMB_FILE_ATTRIBUTE_NORMAL = 0x0000,

        /// <summary>
        /// Read-only file
        /// </summary>
        SMB_FILE_ATTRIBUTE_READONLY = 0x0001,

        /// <summary>
        /// Hidden file
        /// </summary>
        SMB_FILE_ATTRIBUTE_HIDDEN = 0x0002,

        /// <summary>
        /// System file 
        /// </summary>
        SMB_FILE_ATTRIBUTE_SYSTEM = 0x0004,

        /// <summary>
        /// Volume Label
        /// </summary>
        SMB_FILE_ATTRIBUTE_VOLUME = 0x0008,

        /// <summary>
        /// Directory file
        /// </summary>
        SMB_FILE_ATTRIBUTE_DIRECTORY = 0x0010,

        /// <summary>
        /// File changed since last archive
        /// </summary>
        SMB_FILE_ATTRIBUTE_ARCHIVE = 0x0020,

        /// <summary>
        /// Search for Read-only files.
        /// </summary>
        SMB_SEARCH_ATTRIBUTE_READONLY = 0x0100,

        /// <summary>
        /// Search for Hidden files.
        /// </summary>
        SMB_SEARCH_ATTRIBUTE_HIDDEN = 0x0200,

        /// <summary>
        /// Search for System files.
        /// </summary>
        SMB_SEARCH_ATTRIBUTE_SYSTEM = 0x0400,

        /// <summary>
        /// Search for Directory files
        /// </summary>
        SMB_SEARCH_ATTRIBUTE_DIRECTORY = 0x1000,

        /// <summary>
        /// Search for files that have changed since they were last archived.
        /// </summary>
        SMB_SEARCH_ATTRIBUTE_ARCHIVE = 0x2000,
    }


    /// <summary>
    /// An unsigned 32-bit field that defines the basic file attributes supported by the SMB protocol
    /// </summary>
    [Flags()]
    public enum SmbFileAttributes32 : uint
    {
        /// <summary>
        /// The file is read only.
        /// </summary>
        FILE_ATTRIBUTE_READONLY = 0x00000001,

        /// <summary>
        /// The file is hidden during normal directory searches.
        /// </summary>
        FILE_ATTRIBUTE_HIDDEN = 0x00000002,

        /// <summary>
        /// The file is a system file, a component of the operating system.
        /// </summary>
        FILE_ATTRIBTE_SYSTEM = 0x00000004,

        /// <summary>
        /// The file is a directory.
        /// </summary>
        FILE_ATTRIBUTE_DIRECTORY = 0x00000010,

        /// <summary>
        ///The file has been archived at some point in the past.
        /// </summary>
        FILE_ATTRIBUTE_ARCHIVE = 0x00000020,

        /// <summary>
        /// The file represents a hardware device.
        /// </summary>
        FILE_ATTRIBUTE_DEVICE = 0x00000040,

        /// <summary>
        /// The file is a normal file.
        /// </summary>
        FILE_ATTRIBTE_NORMAL = 0x00000080,

        /// <summary>
        /// The file is a temporary file and the file system will delete it when it is closed.
        /// </summary>
        FILE_ATTRIBUTE_TEMPORARY = 0x00000100,

        /// <summary>
        /// The file is in a compressed state.
        /// </summary>
        FILE_ATTRIBUTE_COMPRESSED = 0x00000800,
    }


    /// <summary>
    /// This is a 16-bit value in little-endian byte order use to encode a time of day.
    /// The SMB_TIME value is usually accompanied by an SMB_DATE value that indicates 
    /// what date corresponds with the given time. An SMB_TIME value SHOULD be interpreted 
    /// as follows. This field names below are provided for reference only. The time is 
    /// represented in the local time zone of the server.
    /// </summary>
    public enum SmbTimeBitMask : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// The hours. Values range from 0 to 23.
        /// </summary>
        HOUR = 0xF800,

        /// <summary>
        /// The minutes. Values range from 0 to 59
        /// </summary>
        MINUTES = 0x07E0,

        /// <summary>
        /// The seconds. Values MUST represent two-second increments
        /// </summary>
        SECONDS = 0x001F,
    }


    /// <summary>
    /// This is a 16-bit value in little-endian byte order use to encode a time of day. 
    /// The SMB_TIME value is usually accompanied by an SMB_DATE value that indicates what
    /// date corresponds with the given time. An SMB_TIME value SHOULD be interpreted as follows. 
    /// This field names below are provided for reference only. The time is represented in the local 
    /// time zone of the server
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SmbTime
    {
        private ushort time;

        /// <summary>
        /// The hours. Values range from 0 to 23.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The hour should be in range of [0, 23].</exception>
        public ushort Hour
        {
            get
            {
                return (ushort)(((this.time) & (ushort)SmbTimeBitMask.HOUR) >> 11);
            }
            set
            {
                this.time = (ushort)((value << 11) + (this.Minutes << 5) + (this.Seconds / 2));
            }
        }


        /// <summary>
        /// The minutes. Values range from 0 to 59
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The Minutes should be in range of [0, 59].</exception>
        public ushort Minutes
        {
            get
            {
                return (ushort)((this.time & (ushort)SmbTimeBitMask.MINUTES) >> 5);
            }
            set
            {
                this.time = (ushort)((this.Hour << 11) + (value << 5) + (this.Seconds / 2));
            }
        }


        /// <summary>
        /// The seconds. Values MUST represent two-second increments
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The Seconds should be in range of [0, 59].</exception>
        public ushort Seconds
        {
            get
            {
                return (ushort)((this.time & (ushort)SmbTimeBitMask.SECONDS) * 2);
            }
            set
            {
                this.time = (ushort)((this.Hour << 11) + (this.Minutes << 5) + (value / 2));
            }
        }


        /// <summary>
        /// Current smb time.
        /// </summary>
        public static SmbTime Now
        {
            get
            {
                SmbTime smbTime = DateTime.Now;
                return smbTime;
            }
        }


        /// <summary>
        /// Implicit convert an DateTime to SmbTime.
        /// </summary>
        /// <param name="dateTime">The date time</param>
        /// <returns>An SmbTime</returns>
        public static implicit operator SmbTime(DateTime dateTime)
        {
            SmbTime smbTime = new SmbTime();
            smbTime.Hour = (ushort)dateTime.Hour;
            smbTime.Minutes = (ushort)dateTime.Minute;
            smbTime.Seconds = (ushort)dateTime.Second;
            return smbTime;
        }
    }


    /// <summary>
    /// This is a 32-bit unsigned integer in little-endian byte order indicating the number of seconds since 
    /// Jan 1, 1970, 00:00:00.0.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct UTime
    {
        /// <summary>
        /// This is a 32-bit unsigned integer in little-endian byte order indicating the number of seconds since
        /// Jan 1, 1970, 00:00:00.0.
        /// </summary>
        public uint Time;

        #region Implicit convertion in case of breaking change.


        /// <summary>
        /// Implicit convert an uint to UTime.
        /// </summary>
        /// <param name="time">The uint time to convert</param>
        /// <returns>An UTime value</returns>
        public static implicit operator UTime(uint time)
        {
            UTime ret = new UTime();
            ret.Time = time;
            return ret;
        }

        #endregion
    }


    /// <summary>
    /// The number of 100-nanosecond intervals that have elapsed since January 1, 1601, in Coordinated 
    /// Universal Time (UTC) format.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FileTime
    {
        /// <summary>
        /// The number of 100-nanosecond intervals that have elapsed since January 1, 1601, in Coordinated 
        /// Universal Time (UTC) format.
        /// </summary>
        public ulong Time;
    }

    /// <summary>
    /// The SMB_FEA_LIST data structure is used to send a concatenated list of SMB_FEA (section 2.2.1.2.2) structures.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_FEA_LIST
    {
        /// <summary>
        /// This field MUST contain the total size of the FEAList field, plus the size of the SizeOfListInBytes field
        /// (4 bytes).
        /// </summary>
        public uint SizeOfListInBytes;

        /// <summary>
        ///A concatenated list of SMB_FEA structures.
        /// </summary>
        public SMB_FEA[] FEAList;
    }

    /// <summary>
    /// The SMB_GEA_LIST data structure is used to send a concatenated list of SMB_GEA (section 2.2.1.2.1) structures.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_GEA_LIST
    {
        /// <summary>
        /// This field MUST contain the total size of the GEAList field, plus the size of the SizeOfListInBytes field
        /// (4 bytes).
        /// </summary>
        public uint SizeOfListInBytes;

        /// <summary>
        /// A concatenated list of SMB_GEA (section 2.2.1.2.1) structures.
        /// </summary>
        public SMB_GEA[] GEAList;
    }

    #endregion


    #region 2.2.2 Defined Constants

    /// <summary>
    /// Current name of command and alternate name used in older documentation if available. 
    /// If a code or code range is marked Unused, they are undefined and reserved for future use.
    /// It they are marked Reserved they are or were reserved for a specific purpose. Both of 
    /// these indicate that client implementations SHOULD NOT send messages using any of those command codes.
    /// </summary>
    public enum SmbCommand : byte
    {
        /// <summary>
        /// Create a new directory.
        /// </summary>
        SMB_COM_CREATE_DIRECTORY = 0x00,

        /// <summary>
        /// Delete an empty directory
        /// </summary>
        SMB_COM_DELETE_DIRECTORY = 0x01,

        /// <summary>
        /// Open a file.
        /// </summary>
        SMB_COM_OPEN = 0x02,

        /// <summary>
        /// Create or open a file.
        /// </summary>
        SMB_COM_CREATE = 0x03,

        /// <summary>
        /// Close a file.
        /// </summary>
        SMB_COM_CLOSE = 0x04,

        /// <summary>
        /// Flush data for a file, or all files associated with a client, PID pair.
        /// </summary>
        SMB_COM_FLUSH = 0x05,

        /// <summary>
        /// Delete a file.
        /// </summary>
        SMB_COM_DELETE = 0x06,

        /// <summary>
        /// Rename a file or set of files.
        /// </summary>
        SMB_COM_RENAME = 0x07,

        /// <summary>
        /// Get file attributes.
        /// </summary>
        SMB_COM_QUERY_INFORMATION = 0x08,

        /// <summary>
        /// Set file attributes.
        /// </summary>
        SMB_COM_SET_INFORMATION = 0x09,

        /// <summary>
        /// Read from a file.
        /// </summary>
        SMB_COM_READ = 0x0A,

        /// <summary>
        /// Write to a file.
        /// </summary>
        SMB_COM_WRITE = 0x0B,

        /// <summary>
        /// Request a byte-range lock on a file.
        /// </summary>
        SMB_COM_LOCK_byte_RANGE = 0x0C,

        /// <summary>
        /// Release a byte-range lock on a file
        /// </summary>
        SMB_COM_UNLOCK_byte_RANGE = 0x0D,

        /// <summary>
        /// Create a temporary file.
        /// </summary>
        SMB_COM_CREATE_TEMPORARY = 0x0E,

        /// <summary>
        /// Create and open a new file.
        /// </summary>
        SMB_COM_CREATE_NEW = 0x0F,

        /// <summary>
        /// Verify that the given pathname resolves to a directory.
        /// Listed as SMBchkpath in some documentation.
        /// </summary>
        SMB_COM_CHECK_DIRECTORY = 0x10,

        /// <summary>
        /// Indicate process exit.
        /// </summary>
        SMB_COM_PROCESS_EXIT = 0x11,

        /// <summary>
        /// Set the current file pointer within a file.
        /// </summary>
        SMB_COM_SEEK = 0x12,

        /// <summary>
        /// Lock and read a byte-range within a file.
        /// </summary>
        SMB_COM_LOCK_AND_READ = 0x13,

        /// <summary>
        /// Write and unlock a byte-range within a file.
        /// </summary>
        SMB_COM_WRITE_AND_UNLOCK = 0x14,

        // Unused	0x15...0x19

        /// <summary>
        /// Read a block in raw mode.
        /// </summary>
        SMB_COM_READ_RAW = 0x1A,

        /// <summary>
        /// Multiplexed block read.
        /// Listed as SMBreadmpx in some documentation.
        /// </summary>
        SMB_COM_READ_MPX = 0x1B,

        /// <summary>
        /// Multiplexed block read, secondary request.
        /// </summary>
        SMB_COM_READ_MPX_SECONDARY = 0x1C,

        /// <summary>
        /// Write a block in raw mode.
        /// </summary>
        SMB_COM_WRITE_RAW = 0x1D,

        /// <summary>
        /// Multiplexed block write.
        /// </summary>
        SMB_COM_WRITE_MPX = 0x1E,

        /// <summary>
        /// Multiplexed block write, secondary request.
        /// </summary>
        SMB_COM_WRITE_MPX_SECONDARY = 0x1F,

        /// <summary>
        /// Raw block write, final response
        /// </summary>
        SMB_COM_WRITE_COMPLETE = 0x20,

        /// <summary>
        /// Reserved, but not implemented.
        /// Also known as SMB_COM_QUERY_INFORMATION_SRV.
        /// </summary>
        SMB_COM_QUERY_SERVER = 0x21,

        /// <summary>
        /// Set extended file attributes.
        /// </summary>
        SMB_COM_SET_INFORMATION2 = 0x22,

        /// <summary>
        /// Get extended file attributes.
        /// </summary>
        SMB_COM_QUERY_INFORMATION2 = 0x23,

        /// <summary>
        /// Lock multiple byte ranges; AndX chaining.
        /// </summary>
        SMB_COM_LOCKING_ANDX = 0x24,

        /// <summary>
        /// Transaction.
        /// </summary>
        SMB_COM_TRANSACTION = 0x25,

        /// <summary>
        /// Transaction secondary request.
        /// </summary>
        SMB_COM_TRANSACTION_SECONDARY = 0x26,

        /// <summary>
        /// Pass an I/O Control function request to the server.
        /// </summary>
        SMB_COM_IOCTL = 0x27,

        /// <summary>
        /// IOCTL secondary request.
        /// </summary>
        SMB_COM_IOCTL_SECONDARY = 0x28,

        /// <summary>
        /// Copy a file or directory.
        /// </summary>
        SMB_COM_COPY = 0x29,

        /// <summary>
        /// Move a file or directory.
        /// </summary>
        SMB_COM_MOVE = 0x2A,

        /// <summary>
        /// Echo request (ping).
        /// </summary>
        SMB_COM_ECHO = 0x2B,

        /// <summary>
        /// Write to and close a file.
        /// </summary>
        SMB_COM_WRITE_AND_CLOSE = 0x2C,

        /// <summary>
        /// Extended file open with AndX chaining.
        /// </summary>
        SMB_COM_OPEN_ANDX = 0x2D,

        /// <summary>
        /// Extended file read with AndX chaining.
        /// </summary>
        SMB_COM_READ_ANDX = 0x2E,

        /// <summary>
        /// Extended file write with AndX chaining.
        /// </summary>
        SMB_COM_WRITE_ANDX = 0x2F,

        /// <summary>
        /// Reserved, but not implemented.
        /// Also known as SMB_COM_SET_NEW_SIZE.
        /// </summary>
        SMB_COM_NEW_FILE_SIZE = 0x30,

        /// <summary>
        /// Close an open file and tree disconnect.
        /// </summary>
        SMB_COM_CLOSE_AND_TREE_DISC = 0x31,

        /// <summary>
        /// Transaction 2 format request/response.
        /// </summary>
        SMB_COM_TRANSACTION2 = 0x32,

        /// <summary>
        /// Transaction 2 secondary request.
        /// </summary>
        SMB_COM_TRANSACTION2_SECONDARY = 0x33,

        /// <summary>
        /// Close an active search.
        /// </summary>
        SMB_COM_FIND_CLOSE2 = 0x34,

        /// <summary>
        /// Notification of the closure of an active search.
        /// </summary>
        SMB_COM_FIND_NOTIFY_CLOSE = 0x35,

        // Unused	0x36...0x5F

        // Reserved	0x60...0x6F

        /// <summary>
        /// Tree connect.
        /// </summary>
        SMB_COM_TREE_CONNECT = 0x70,

        /// <summary>
        /// Tree disconnect
        /// </summary>
        SMB_COM_TREE_DISCONNECT = 0x71,

        /// <summary>
        /// Negotiate protocol dialect.
        /// </summary>
        SMB_COM_NEGOTIATE = 0x72,

        /// <summary>
        /// Session Setup with AndX chaining.
        /// </summary>
        SMB_COM_SESSION_SETUP_ANDX = 0x73,

        /// <summary>
        /// User logoff with AndX chaining.
        /// </summary>
        SMB_COM_LOGOFF_ANDX = 0x74,

        /// <summary>
        /// Tree connect with AndX chaining.
        /// </summary>
        SMB_COM_TREE_CONNECT_ANDX = 0x75,

        // Unused	0x76...0x7D

        /// <summary>
        /// Negotiate security packages with AndX chaining.
        /// </summary>
        SMB_COM_SECURITY_PACKAGE_ANDX = 0x7E,

        // Unused	0x7F

        /// <summary>
        /// Retrieve file system information from the server.
        /// </summary>
        SMB_COM_QUERY_INFORMATION_DISK = 0x80,

        /// <summary>
        /// Directory wildcard search.
        /// </summary>
        SMB_COM_SEARCH = 0x81,

        /// <summary>
        /// Start or continue an extended wildcard directory search.
        /// </summary>
        SMB_COM_FIND = 0x82,

        /// <summary>
        /// Perform a one-time extended wildcard directory search.
        /// </summary>
        SMB_COM_FIND_UNIQUE = 0x83,

        /// <summary>
        /// End an extended wildcard directory search.
        /// </summary>
        SMB_COM_FIND_CLOSE = 0x84,

        // Unused	0x85...0x9F

        /// <summary>
        /// NT format transaction request/response.
        /// </summary>
        SMB_COM_NT_TRANSACT = 0xA0,

        /// <summary>
        /// NT format transaction secondary request.
        /// </summary>
        SMB_COM_NT_TRANSACT_SECONDARY = 0xA1,

        /// <summary>
        /// Create or open a file or a directory.
        /// </summary>
        SMB_COM_NT_CREATE_ANDX = 0xA2,

        // Unused	0xA3

        /// <summary>
        /// Cancel a request currently pending at the server.
        /// </summary>
        SMB_COM_NT_CANCEL = 0xA4,

        /// <summary>
        /// File rename with extended semantics. 
        /// </summary>
        SMB_COM_NT_RENAME = 0xA5,

        // Unused	0xA6...0xBF

        /// <summary>
        /// Create a print queue spool file.
        /// </summary>
        SMB_COM_OPEN_PRINT_FILE = 0xC0,

        /// <summary>
        /// Write to a print queue spool file.
        /// </summary>
        SMB_COM_WRITE_PRINT_FILE = 0xC1,

        /// <summary>
        /// Close a print queue spool file.
        /// </summary>
        SMB_COM_CLOSE_PRINT_FILE = 0xC2,

        /// <summary>
        /// Request print queue information.
        /// </summary>
        SMB_COM_GET_PRINT_QUEUE = 0xC3,

        // Unused	0xC4...0xCF

        // Reserved	0xD0...0xD7

        /// <summary>
        /// Reserved, but not 
        /// </summary>
        SMB_COM_READ_BULK = 0xD8,

        /// <summary>
        /// Reserved, but not implemented.
        /// </summary>
        SMB_COM_WRITE_BULK = 0xD9,

        /// <summary>
        /// Reserved, but not implemented.
        /// </summary>
        SMB_COM_WRITE_BULK_DATA = 0xDA,

        // Unused	0xDB...0xFD

        /// <summary>
        /// As the name suggests, this command code is a designated 
        /// invalid command and SHOULD NOT be used.
        /// </summary>
        SMB_COM_INVALID = 0xFE,

        /// <summary>
        /// Also known as the "NIL" command. It identifies the end of an AndX Chain,
        /// and is only valid in that context. See Section 2.2.3.4.
        /// </summary>
        SMB_COM_NO_ANDX_COMMAND = 0xFF,
    }


    /// <summary>
    /// 2.2.2.2   	Transaction Subcommand Codes
    /// </summary>
    public enum TransSubCommand : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Allows a client to write data to a specific mailslot on the server
        /// </summary>
        TRANS_MAILSLOT_WRITE = 0x0001,

        /// <summary>
        /// Used to set the read mode and blocking mode of a specified named pipe.
        /// </summary>
        TRANS_SET_NMPIPE_STATE = TRANS_MAILSLOT_WRITE,

        /// <summary>
        /// Allows for a raw read of data from a named pipe.
        /// This method of reading data from a named pipe ignores message 
        /// boundaries even if the pipe was set up as a message mode pipe.
        /// </summary>
        TRANS_RAW_READ_NMPIPE = 0x0011,

        /// <summary>
        /// Allows for a client to retrieve information about a specified named pipe.
        /// </summary>
        TRANS_QUERY_NMPIPE_STATE = 0x0021,

        /// <summary>
        /// Used to retrieve pipe information about a named pipe.
        /// </summary>
        TRANS_QUERY_NMPIPE_INFO = 0x0022,

        /// <summary>
        /// Used to copy data out of a named pipe without removing it from the named pipe
        /// </summary>
        TRANS_PEEK_NMPIPE = 0x0023,

        /// <summary>
        /// Used to execute a transacted exchange against a named pipe. 
        /// This transaction has a constraint that it can be used only on a duplex,
        /// message-type pipe.
        /// </summary>
        TRANS_TRANSACT_NMPIPE = 0x0026,

        /// <summary>
        /// Allows for a raw write of data to a named pipe. 
        /// Raw writes to named pipes put bytes directly into a pipe, regardless 
        /// of whether it is a message mode pipe or stream mode pipe
        /// </summary>
        TRANS_RAW_WRITE_NMPIPE = 0x0031,

        /// <summary>
        /// Allows a client to read data from a named pipe
        /// </summary>
        TRANS_READ_NMPIPE = 0x0036,

        /// <summary>
        /// Allows a client to write data to a named pipe.
        /// </summary>
        TRANS_WRITE_NMPIPE = 0x0037,

        /// <summary>
        /// Allows a client to be notified when the specified 
        /// named pipe is available to be connected to.
        /// </summary>
        TRANS_WAIT_NMPIPE = 0x0053,

        /// <summary>
        /// Connect to a named pipe, issue a write to the named pipe,
        /// issue a read from the named pipe, and close the named pipe.
        /// </summary>
        TRANS_CALL_NMPIPE = 0x0054,
    }


    /// <summary>
    /// Transaction Codes used with SMB_COM_TRANSACTION2
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    public enum Trans2SubCommand : ushort
    {
        /// <summary>
        /// Open or create a file and set extended attributes on the file
        /// </summary>
        TRANS2_OPEN2 = 0x00,

        /// <summary>
        /// Begin a search for files within a directory or for a directory
        /// </summary>
        TRANS2_FIND_FIRST2 = 0x0001,

        /// <summary>
        /// Continue a search for files within a directory or for a directory
        /// </summary>
        TRANS2_FIND_NEXT2 = 0x0002,

        /// <summary>
        /// Request information about a file system on the server
        /// </summary>
        TRANS2_QUERY_FS_INFORMATION = 0x0003,

        /// <summary>
        /// Not implemented - The command code was reserved and, in some cases, the command was documented
        /// but never implemented
        /// </summary>
        TRANS2_SET_FS_INFORMATION = 0x0004,

        /// <summary>
        /// Get information about a specific file or directory using a path.
        /// </summary>
        TRANS2_QUERY_PATH_INFORMATION = 0x0005,

        /// <summary>
        /// Set the standard and extended attribute information of a specific file or director using a path
        /// </summary>
        TRANS2_SET_PATH_INFORMATION = 0x0006,

        /// <summary>
        /// Get information about a specific file or directory using a FID.
        /// </summary>
        TRANS2_QUERY_FILE_INFORMATION = 0x0007,

        /// <summary>
        /// Set the standard and extended attribute information of a specific file or director using a FID.
        /// </summary>
        TRANS2_SET_FILE_INFORMATION = 0x0008,

        /// <summary>
        /// Not implemented - The command code was reserved and, in some cases, the command was documented 
        /// but never implemented
        /// </summary>
        TRANS2_FSCTL = 0x0009,

        /// <summary>
        /// Not implemented - The command code was reserved and, in some cases, the command was documented
        /// but never implemented
        /// </summary>
        TRANS2_IOCTL2 = 0x000a,

        /// <summary>
        /// Obsolete - No longer supported. 
        /// </summary>
        TRANS2_FIND_NOTIFY_FIRST = 0x000b,

        /// <summary>
        /// Obsolete - No longer supported. 
        /// </summary>
        TRANS2_FIND_NOTIFY_NEXT = 0x000c,

        /// <summary>
        /// Create a new directory and optionally set the extended attribute information.
        /// </summary>
        TRANS2_CREATE_DIRECTORY = 0x000d,

        /// <summary>
        /// Not implemented - The command code was reserved and, in some cases, the command was documented 
        /// but never implemented
        /// </summary>
        TRANS2_SESSION_SETUP = 0x000e,

        /// <summary>
        /// Request a DFS referral for a file or directory, see [MS-DFSC] for details.
        /// </summary>
        TRANS2_GET_DFS_REFERRAL = 0x0010,

        /// <summary>
        /// Not implemented - The command code was reserved and, in some cases, the command was documented 
        /// but never implemented
        /// </summary>
        TRANS2_REPORT_DFS_INCONSISTENCY = 0x0011,
    }


    /// <summary>
    /// Transaction codes used with SMB_COM_NT_TRANSACT
    /// </summary>
    public enum NtTransSubCommand : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Used to create or open a file or directory when extended attributes 
        /// (EAs) or a security descriptor (SD) are to be applied
        /// </summary>
        NT_TRANSACT_CREATE = 0x0001,

        /// <summary>
        /// Allows device and file system control functions 
        /// to be transferred transparently from client to server.
        /// </summary>
        NT_TRANSACT_IOCTL = 0x0002,

        /// <summary>
        /// Allows a client to change the security descriptor for a file
        /// </summary>
        NT_TRANSACT_SET_SECURITY_DESC = 0x0003,

        /// <summary>
        /// Notifies the client when the directory specified by Fid is modified.
        /// It also returns the names of any files that changed
        /// </summary>
        NT_TRANSACT_NOTIFY_CHANGE = 0x0004,

        /// <summary>
        /// Not implemented - The command code was reserved and, in some cases, the command was documented
        /// but never implemented
        /// </summary>
        NT_TRANSACT_RENAME = 0x0005,

        /// <summary>
        /// Allows a client to retrieve the security descriptor for a file
        /// </summary>
        NT_TRANSACT_QUERY_SECURITY_DESC = 0x0006,
    }


    /// <summary>
    /// This field determines the information contained in the response.
    /// </summary>
    public enum FindInformationLevel : ushort
    {
        /// <summary>
        /// None
        /// </summary>
        NONE = 0x0000,

        /// <summary>
        /// Return standard information about the file(s). See the details below for the list of fields.
        /// </summary>
        SMB_INFO_STANDARD = 0x0001,

        /// <summary>
        /// Return the standard information about the file(s) including the size of the extended attribute list.
        /// </summary>
        SMB_INFO_QUERY_EA_SIZE = 0x0002,

        /// <summary>
        /// Return the standard information about the file(s) including specific extended attributes provided in the request. The requested extended attributes are provided in the Trans_Data. See below for details.
        /// </summary>
        SMB_INFO_QUERY_EAS_FROM_LIST = 0x0003,

        /// <summary>
        /// Return standard information about the file(s) in the directory. Provides support for 64 bit values for important fields.
        /// </summary>
        SMB_FIND_FILE_DIRECTORY_INFO = 0x0101,

        /// <summary>
        /// Return the same information as the above along with the size of the extended attribute list of the file(s).
        /// </summary>
        SMB_FIND_FILE_FULL_DIRECTORY_INFO = 0x0102,

        /// <summary>
        /// Return the names of the file(s).
        /// </summary>
        SMB_FIND_FILE_NAMES_INFO = 0x0103,

        /// <summary>
        /// Return a combination of the data from SMB_FIND_FILE_FULL_DIRECTORY_INFO and SMB_FIND_FILE_NAMES_INFO.
        /// </summary>
        SMB_FIND_FILE_BOTH_DIRECTORY_INFO = 0x0104
    }


    /// <summary>
    /// This field determines the information contained in the response.
    /// </summary>
    public enum QueryFSInformationLevel : ushort
    {
        /// <summary>
        /// None
        /// </summary>
        NONE = 0x0000,

        /// <summary>
        /// Query info allocation
        /// </summary>
        SMB_INFO_ALLOCATION = 0x0001,

        /// <summary>
        /// Query info volume
        /// </summary>
        SMB_INFO_VOLUME = 0x0002,

        /// <summary>
        /// Query file system volume_info
        /// </summary>
        SMB_QUERY_FS_VOLUME_INFO = 0x0102,

        /// <summary>
        /// Query file system vsize_info
        /// </summary>
        SMB_QUERY_FS_SIZE_INFO = 0x0103,

        /// <summary>
        /// Query file system vdevice_info
        /// </summary>
        SMB_QUERY_FS_DEVICE_INFO = 0x0104,

        /// <summary>
        /// Query file system vattribute_info
        /// </summary>
        SMB_QUERY_FS_ATTRIBUTE_INFO = 0x0105
    }


    /// <summary>
    /// The SMB_COM_TRANSACTION2 subcommands TRANS2_QUERY_PATH_INFORMATION 
    /// and TRANS2_QUERY_FILE_INFORMATION return different data based on the client's request.
    /// The client specifies the information it is requesting through the use of the InformationLevel 
    /// field in the subcommand request data. The available information levels are as follows. 
    /// </summary>
    public enum QueryInformationLevel : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Query creation, access, and last write timestamps, size and file attributes
        /// </summary>
        SMB_INFO_STANDARD = 0x0001,

        /// <summary>
        /// Query the SMB_INFO_STANDARD data along with the size of the file's extended attributes
        /// </summary>
        SMB_INFO_QUERY_EA_SIZE = 0x0002,

        /// <summary>
        /// Query specific extended attributes by attribute name.
        /// </summary>
        SMB_INFO_QUERY_EAS_FROM_LIST = 0x0003,

        /// <summary>
        /// Query all of the file's extended attributes.
        /// </summary>
        SMB_INFO_QUERY_ALL_EAS = 0x0004,

        /// <summary>
        /// Validate the syntax of the path provided in the request.
        /// </summary>
        SMB_INFO_IS_NAME_VALID = 0x0006,

        /// <summary>
        /// Query create, access, write, and change timestamps along with file attributes
        /// </summary>
        SMB_QUERY_FILE_BASIC_INFO = 0x0101,

        /// <summary>
        /// Query size, number of links, if a delete is pending, and if the path is a directory.
        /// </summary>
        SMB_QUERY_FILE_STANDARD_INFO = 0x0102,

        /// <summary>
        /// Query the size of the file's extended attributes.
        /// </summary>
        SMB_QUERY_FILE_EA_INFO = 0x0103,

        /// <summary>
        /// Query the long file name in Unicode format.
        /// </summary>
        SMB_QUERY_FILE_NAME_INFO = 0x0104,

        /// <summary>
        /// Query returns SMB_QUERY_FILE_BASIC_INFO, 
        /// SMB_FILE_QUERY_STANDARD_INFO, SMB_FILE_EA_INFO,
        /// and SMB_QUERY_FILE_NAME_INFO in a single request.
        /// </summary>
        SMB_QUERY_FILE_ALL_INFO = 0x0107,

        /// <summary>
        /// Query the 8.3 file name.
        /// </summary>
        SMB_QUERY_FILE_ALT_NAME_INFO = 0x0108,

        /// <summary>
        /// Query file stream information
        /// </summary>
        SMB_QUERY_FILE_STREAM_INFO = 0x0109,

        /// <summary>
        /// Query file compression information
        /// </summary>
        SMB_QUERY_FILE_COMPRESSION_INFO = 0x010B,
    }


    /// <summary>
    /// The SMB_COM_TRANSACTION2 subcommands TRANS2_QUERY_PATH_INFORMATION 
    /// and TRANS2_QUERY_FILE_INFORMATION return different data based on the client's request.
    /// The client specifies the information it is requesting through the use of the InformationLevel 
    /// field in the subcommand request data. The available information levels are as follows. 
    /// </summary>
    public enum SetInformationLevel : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Set file information.
        /// </summary>
        SMB_INFO_STANDARD = 0x0001,

        /// <summary>
        /// Set extended attribute file information
        /// </summary>
        SMB_INFO_SET_EAS = 0x0002,

        /// <summary>
        /// set file information.
        /// </summary>
        SMB_SET_FILE_BASIC_INFO = 0x0101,

        /// <summary>
        /// Mark a file for deletion.
        /// </summary>
        SMB_SET_FILE_DISPOSITION_INFO = 0x0102,

        /// <summary>
        /// Set the allocation size for a file.
        /// </summary>
        SMB_SET_FILE_ALLOCATION_INFO = 0x0103,

        /// <summary>
        /// Set end-of-file information for a file
        /// </summary>
        SMB_SET_FILE_END_OF_FILE_INFO = 0x0104

    }


    /// <summary>
    /// InformationLevel used in SMB_NT_RENAME.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum NtRenameInformationLevel : ushort
    {
        /// <summary>
        /// Create a hard link to the original file.
        /// </summary>
        SMB_NT_RENAME_SET_LINK_INFO = 0x0103,

        /// <summary>
        /// An in-place rename of the file.
        /// </summary>
        SMB_NT_RENAME_RENAME_FILE = 0x0104,

        /// <summary>
        /// Move the file within the path hierarchy. This information level is obsolete. 
        /// Clients MUST NOT use this value in a request.
        /// </summary>
        SMB_NT_RENAME_MOVE_FILE = 0x0105,
    }


    /// <summary>
    /// This section provides an overview of status codes that MAY be returned by the SMB commands
    /// listed in this document, including mappings between the NTSTATUS codes used in the NT LAN Manager
    /// dialect, the SMBSTATUS class/code pairs used in earlier SMB dialects, and common POSIX equivalents. 
    /// The POSIX error code mappings are based upon those used in the Xenix server implementation. 
    /// This is not an exhaustive listing, and MUST NOT be considered normative.
    /// Each command and subcommand description also includes a list of status codes that SHOULD be 
    /// returned by CIFS-compliant servers. Individual implementations MAY return status codes from their underlying 
    /// operating systems; it is up to the implementer to decide how to interpret those status codes. 
    /// The listing below is organized by SMBSTATUS Error Class. It shows SMBSTATUS Error Code values and a 
    /// general description, as well as mappings from NTSTATUS values (as defined in [MS-ERRREF] Section 2.3.1) and
    /// POSIX-style error codes where possible. Note that multiple NTSTATUS values can map to a single SMBSTATUS value.
    /// </summary>
    public enum SmbErrorClass : byte
    {
        /// <summary>
        /// SUCCESS Class 
        /// </summary>
        SUCCESS = 0,

        /// <summary>
        /// ERRDOS class
        /// </summary>
        ERRDOS = 0x01,

        /// <summary>
        /// ERRSRV class
        /// </summary>
        ERRSRV = 0x02,

        /// <summary>
        /// ERRHRD Class
        /// </summary>
        ERRHRD = 0x03,

        /// <summary>
        /// ERRCMD Class
        /// </summary>
        ERRCMD = 0xFF,
    }


    /// <summary>
    /// SUCCESS Class 
    /// </summary>
    public enum SmbErrorCodeOfSUCCESS : ushort
    {
        /// <summary>
        /// Everything worked, no problems
        /// </summary>
        SUCCESS = 0,
    }


    /// <summary>
    /// ERRDOS Class 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum SmbErrorCodeOfERRDOS : ushort
    {
        /// <summary>
        /// Invalid Function.
        /// </summary>
        ERRbadfunc = 0x0001,

        /// <summary>
        /// File not found.
        /// </summary>
        ERRbadfile = 0x0002,

        /// <summary>
        /// A component in the path prefix is not a directory.
        /// </summary>
        ERRbadpath = 0x0003,

        /// <summary>
        /// Too many open files. No FIDs are available.
        /// </summary>
        ERRnofids = 0x0004,

        /// <summary>
        /// Access denied.
        /// </summary>
        ERRnoaccess = 0x0005,

        /// <summary>
        /// Invalid FID.
        /// </summary>
        ERRbadfid = 0x0006,

        /// <summary>
        /// Memory Control Blocks were destroyed.
        /// </summary>
        ERRbadmcb = 0x0007,

        /// <summary>
        /// Insufficient server memory to perform the requested operation.
        /// </summary>
        ERRnomem = 0x0008,

        /// <summary>
        /// The server performed an invalid memory access (invalid address).
        /// </summary>
        ERRbadmem = 0x0009,

        /// <summary>
        /// Invalid environment.
        /// </summary>
        ERRbadenv = 0x000A,

        /// <summary>
        /// Invalid format.
        /// </summary>
        ERRbadformat = 0x000B,

        /// <summary>
        /// Invalid open mode.
        /// </summary>
        ERRbadaccess = 0x000C,

        /// <summary>
        /// Bad data. (May be generated by IOCTL calls on the server.)
        /// </summary>
        ERRbaddata = 0x000D,

        /// <summary>
        /// Invalid drive specified.
        /// </summary>
        ERRbaddrive = 0x000F,

        /// <summary>
        /// Remove of directory failed because it was not empty.
        /// </summary>
        ERRremcd = 0x0010,

        /// <summary>
        /// A file system operation (such as a rename) across two devices was attempted.
        /// </summary>
        ERRdiffdevice = 0x0011,

        /// <summary>
        /// No (more) files found following a file search command.
        /// </summary>
        ERRnofiles = 0x0012,

        /// <summary>
        /// General error.
        /// </summary>
        ERRgeneral = 0x001F,

        /// <summary>
        /// Sharing violation. A requested open mode conflicts with the sharing mode of an existing file handle.
        /// </summary>
        ERRbadshare = 0x0020,

        /// <summary>
        /// A lock request specified an invalid locking mode, or conflicted with an existing file lock.
        /// </summary>
        ERRlock = 0x0021,

        /// <summary>
        /// Attempted to read beyond the end of the file.
        /// </summary>
        ERReof = 0x0026,

        /// <summary>
        /// This command is not supported by the server.
        /// </summary>
        ERRunsup = 0x0032,

        /// <summary>
        /// An attempt to create a file or directory failed because an object with the same pathname already exists.
        /// </summary>
        ERRfilexists = 0x0050,

        /// <summary>
        /// A parameter supplied with the message is invalid.
        /// </summary>
        ERRinvalidparam = 0x0057,

        /// <summary>
        /// Invalid information level.
        /// </summary>
        ERRunknownlevel = 0x007C,

        /// <summary>
        /// An attempt was made to seek to a negative absolute offset within a file.
        /// </summary>
        ERRinvalidseek = 0x0083,

        /// <summary>
        /// The byte range specified in an unlock request was not locked.
        /// </summary>
        ERROR_NOT_LOCKED = 0x009E,

        /// <summary>
        /// No lock request was outstanding for the supplied cancel region.
        /// </summary>
        ERROR_CANCEL_VIOLATION = 0x00AD,

        /// <summary>
        /// Invalid named pipe.
        /// </summary>
        ERRbadpipe = 0x00E6,

        /// <summary>
        /// All instances of the designated named pipe are busy.
        /// </summary>
        ERRpipebusy = 0x00E7,

        /// <summary>
        /// The designated named pipe is in the process of being closed.
        /// </summary>
        ERRpipeclosing = 0x00E8,

        /// <summary>
        /// The designated named pipe exists, but there is no server process listening on the server side.
        /// </summary>
        ERRnotconnected = 0x00E9,

        /// <summary>
        /// There is more data available to read on the designated named pipe.
        /// </summary>
        ERRmoredata = 0x00EA,

        /// <summary>
        /// Inconsistent extended attribute list.
        /// </summary>
        ERRbadealist = 0x00FF,

        /// <summary>
        /// Either there are no extended attributes, or the available extended attributes did not fit into the response.
        /// </summary>
        ERROR_EAS_DIDNT_FIT = 0x0113,

        /// <summary>
        /// The server file system does not support Extended Attributes.
        /// </summary>
        ERROR_EAS_NOT_SUPPORTED = 0x011A,

        /// <summary>
        /// More changes have occurred within the directory than will fit within the specified Change Notify response buffer.
        /// </summary>
        ERR_NOTIFY_ENUM_DIR = 0x03FE,
    }


    /// <summary>
    /// ERRSRV Class 
    /// </summary>
    public enum SmbErrorCodeOfERRSRV : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Unspecified server error. 
        /// </summary>
        ERRerror = 0x0001,

        /// <summary>
        /// Invalid password.
        /// </summary>
        ERRbadpw = 0x0002,

        /// <summary>
        /// DFS pathname not on local server.
        /// </summary>
        ERRbadpath = 0x0003,

        /// <summary>
        /// Access denied. The given UID does not have permission to 
        /// execute the requested command within the current context (TID).
        /// </summary>
        ERRaccess = 0x0004,

        /// <summary>
        /// The TID specified in the command was invalid.
        /// Earlier documentation, with the exception of [SNIA-CIFS], refers to this error code as 
        /// ERRinvnid (Invalid Network Path Identifier).  [SNIA-CIFS] uses both names. 
        /// </summary>
        ERRinvtid = 0x0005,

        /// <summary>
        /// Invalid server name in Tree Connect
        /// </summary>
        ERRinvnetname = 0x0006,

        /// <summary>
        /// A printer request was made to a non-printer device or, conversely, a non-printer request was made to
        /// a printer device.
        /// </summary>
        ERRinvdevice = 0x0007,

        /// <summary>
        /// Invalid Session ID (SID). This error code is only defined when the Direct IPX 
        /// Invalid Session ID (SID). This error code is only defined when the Direct IPX connectionless transport
        /// is in use. 
        /// </summary>
        ERRinvsess = 0x0010,

        /// <summary>
        /// A command with matching MID or SequenceNumber is currently being processed. 
        /// This error code is only defined when the Direct IPX connectionless transport is in use.
        /// </summary>
        ERRworking = 0x0011,

        /// <summary>
        /// Incorrect NetBIOS Called Name when starting an SMB session over Direct IPX. 
        /// This error code is only defined when the Direct IPX connectionless transport is in use.
        /// </summary>
        ERRnotme = 0x0012,

        /// <summary>
        /// An unknown SMB command code was received by the server.
        /// </summary>
        ERRbadcmd = 0x0016,

        /// <summary>
        /// Print queue is full: too many queued items.
        /// </summary>
        ERRqfull = 0x0031,

        /// <summary>
        /// Print queue is full: no space for queued item, or queued item too big.
        /// </summary>
        ERRqtoobig = 0x0032,

        /// <summary>
        /// End Of File on print queue dump.
        /// </summary>
        ERRqeof = 0x0033,

        /// <summary>
        /// Invalid FID for print file.
        /// </summary>
        ERRinvpfid = 0x0034,

        /// <summary>
        /// Unrecognized SMB command code.
        /// </summary>
        ERRsmbcmd = 0x0040,

        /// <summary>
        /// Internal server error
        /// </summary>
        ERRsrverror = 0x0041,

        /// <summary>
        /// The FID and pathname contain incompatible values.
        /// </summary>
        ERRfilespecs = 0x0043,

        /// <summary>
        /// An invalid combination of access permissions 
        /// for a file or directory was presented.
        /// The server cannot set the requested attributes.
        /// </summary>
        ERRbadpermits = 0x0045,

        /// <summary>
        /// The attribute mode presented in a set mode request was invalid.
        /// </summary>
        ERRsetattrmode = 0x0047,

        /// <summary>
        /// Operation timed out.
        /// </summary>
        ERRtimeout = 0x0058,

        /// <summary>
        /// No resources currently available for this SMB request.
        /// </summary>
        ERRnoresource = 0x0059,

        /// <summary>
        /// Too many UIDs active for this SMB session.
        /// </summary>
        ERRtoomanyuids = 0x005A,

        /// <summary>
        /// The UID given is not known as a valid ID on this server session.
        /// </summary>
        ERRbaduid = 0x005B,

        /// <summary>
        /// Write to a named pipe with no reader.
        /// </summary>
        ERRnotconnected = 0x00E9,

        /// <summary>
        /// Temporarily unable to support RAW mode transfers. Use MPX mode.
        /// </summary>
        ERRusempx = 0x00FA,

        /// <summary>
        /// Temporarily unable to support RAW or MPX mode transfers.
        /// Use standard read/write.
        /// </summary>
        ERRusestd = 0x00FB,

        /// <summary>
        /// Continue in MPX mode.
        /// This error code is reserved for future use.
        /// </summary>
        ERRcontmpx = 0x00FC,

        /// <summary>
        /// User account on the target machine is disabled or has expired.
        /// </summary>
        ERRaccountExpired = 0x08BF,

        /// <summary>
        /// The client does not have permission to access this server.
        /// </summary>
        ERRbadClient = 0x08C0,

        /// <summary>
        /// May not access the server at this time.
        /// </summary>
        ERRbadLogonTime = 0x08C1,

        /// <summary>
        /// The user's password has expired
        /// </summary>
        ERRpasswordExpired = 0x08C2,

        /// <summary>
        /// Function not supported by the 
        /// </summary>
        ERRnosupport = 0xFFFF,
    }


    /// <summary>
    /// ERRHRD Class 
    /// </summary>
    public enum SmbErrorCodeOfERRHRD : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Attempt to modify a read-only file system.
        /// </summary>
        ERRnowrite = 0x0013,

        /// <summary>
        /// Unknown unit.
        /// </summary>
        ERRbadunit = 0x0014,

        /// <summary>
        /// Drive not ready.
        /// </summary>
        ERRnotready = 0x0015,

        /// <summary>
        /// Unknown command
        /// </summary>
        ERRbadcmd = 0x0016,

        /// <summary>
        /// Data error (incorrect CRC).
        /// </summary>
        ERRdata = 0x0017,

        /// <summary>
        /// Bad request structure length.
        /// </summary>
        ERRbadreq = 0x0018,

        /// <summary>
        /// Seek error.
        /// </summary>
        ERRseek = 0x0019,

        /// <summary>
        /// Unknown media type.
        /// </summary>
        ERRbadmedia = 0x001A,

        /// <summary>
        /// Sector not found.
        /// </summary>
        ERRbadsector = 0x001B,

        /// <summary>
        /// Printer out of paper.
        /// </summary>
        ERRnopaper = 0x001C,

        /// <summary>
        /// Write fault.
        /// </summary>
        ERRwrite = 0x001D,

        /// <summary>
        /// Read fault.
        /// </summary>
        ERRread = 0x001E,

        /// <summary>
        /// General hardware failure.
        /// </summary>
        ERRgeneral = 0x001F,

        /// <summary>
        /// An attempted open operation conflicts with an existing open.
        /// </summary>
        ERRbadshare = 0x0020,

        /// <summary>
        /// A lock request specified an invalid locking mode, or conflicted with an existing file lock.
        /// </summary>
        ERRlock = 0x0021,

        /// <summary>
        /// The wrong disk was found in a drive.
        /// </summary>
        ERRwrongdisk = 0x0022,

        /// <summary>
        /// No server-side File Control Blocks are available to process the request.
        /// </summary>
        ERRFCBUnavail = 0x0023,

        /// <summary>
        /// A sharing buffer has been exceeded.
        /// </summary>
        ERRsharebufexc = 0x0024,

        /// <summary>
        /// No space on file system.
        /// </summary>
        ERRdiskfull = 0x0027,
    }


    /// <summary>
    /// NTSTATUS codes used in the NT LAN Manager dialect.
    /// </summary>
    public enum NTSTATUS : uint
    {
        /// <summary>
        /// Everything worked, no problems.
        /// </summary>
        STATUS_OK = 0,

        /// <summary>
        /// Invalid Function: the function is not implemented.
        /// </summary>
        STATUS_NOT_IMPLEMENTED = 0xC0000002,

        /// <summary>
        /// Invalid Function: the request to device is invalid.
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010,

        /// <summary>
        /// Invalid Function: the function is illegal.
        /// </summary>
        STATUS_ILLEGAL_FUNCTION = 0xC00000AF,

        /// <summary>
        /// File not found: the file is not found.
        /// </summary>
        STATUS_NO_SUCH_FILE = 0xC000000F,

        /// <summary>
        /// File not found: the device is not found.
        /// </summary>
        STATUS_NO_SUCH_DEVICE = 0xC000000E,

        /// <summary>
        /// File not found: the object specified by the name is not found.
        /// </summary>
        STATUS_OBJECT_NAME_NOT_FOUND = 0xC0000034,

        /// <summary>
        /// A component in the path prefix is not a directory: the path is invalid.
        /// </summary>
        STATUS_OBJECT_PATH_INVALID = 0xC0000039,

        /// <summary>
        /// A component in the path prefix is not a directory: the path is not found.
        /// </summary>
        STATUS_OBJECT_PATH_NOT_FOUND = 0xC000003A,

        /// <summary>
        /// A component in the path prefix is not a directory: the path syntax is bad.
        /// </summary>
        STATUS_OBJECT_PATH_SYNTAX_BAD = 0xC000003B,

        /// <summary>
        /// A component in the path prefix is not a directory: the extended path is not found.
        /// </summary>
        STATUS_DFS_EXIT_PATH_FOUND = 0xC000009B,

        /// <summary>
        /// A component in the path prefix is not a directory: the redirector is not started.
        /// </summary>
        STATUS_REDIRECTOR_NOT_STARTED = 0xC00000FB,

        /// <summary>
        /// Too many open files. No FIDs are available.
        /// </summary>
        STATUS_TOO_MANY_OPENED_FILES = 0xC000011F,

        /// <summary>
        /// Access denied.
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022,

        /// <summary>
        /// Access denied: the lock sequence is invalid.
        /// </summary>
        STATUS_INVALID_LOCK_SEQUENCE = 0xC000001E,

        /// <summary>
        /// Access denied: the view size is invalid.
        /// </summary>
        STATUS_INVALID_VIEW_SIZE = 0xC000001F,

        /// <summary>
        /// Access denied: already committed.
        /// </summary>
        STATUS_ALREADY_COMMITTED = 0xC0000021,

        /// <summary>
        /// Access denied: the connection on the port is refused.
        /// </summary>
        STATUS_PORT_CONNECTION_REFUSED = 0xC0000041,

        /// <summary>
        /// Access denied: the thread is terminating.
        /// </summary>
        STATUS_THREAD_IS_TERMINATING = 0xC000004B,

        /// <summary>
        /// Access denied: the file is pending to delete.
        /// </summary>
        STATUS_DELETE_PENDING = 0xC0000056,

        /// <summary>
        /// Access denied: the privilege is not held.
        /// </summary>
        STATUS_PRIVILEGE_NOT_HELD = 0xC0000061,

        /// <summary>
        /// Access denied: logon failture.
        /// </summary>
        STATUS_LOGON_FAILURE = 0xC000006D,

        /// <summary>
        /// Access denied: the file is a directory.
        /// </summary>
        STATUS_FILE_IS_A_DIRECTORY = 0xC00000BA,

        /// <summary>
        /// Access denied: the file renamed, not found.
        /// </summary>
        STATUS_FILE_RENAMED = 0xC00000D5,

        /// <summary>
        /// Access denied: the process is terminating.
        /// </summary>
        STATUS_PROCESS_IS_TERMINATING = 0xC000010A,

        /// <summary>
        /// Access denied: the target directory is not empty.
        /// </summary>
        STATUS_DIRECTORY_NOT_EMPTY = 0xC0000101,

        /// <summary>
        /// Access denied: the file can not be delete.
        /// </summary>
        STATUS_CANNOT_DELETE = 0xC0000121,

        /// <summary>
        /// Access denied: the file is deleted.
        /// </summary>
        STATUS_FILE_DELETED = 0xC0000123,

        /// <summary>
        /// Invalid FID.
        /// </summary>
        STATUS_SMB_BAD_FID = 0x00060001,

        /// <summary>
        /// Invalid FID: the handle is invalid.
        /// </summary>
        STATUS_INVALID_HANDLE = 0xC0000008,

        /// <summary>
        /// Invalid FID: the type of object is mismatch.
        /// </summary>
        STATUS_OBJECT_TYPE_MISMATCH = 0xC0000024,

        /// <summary>
        /// Invalid FID: the connection on the port is disconnected.
        /// </summary>
        STATUS_PORT_DISCONNECTED = 0xC0000037,

        /// <summary>
        /// Invalid FID: the handle on the port is invalid.
        /// </summary>
        STATUS_INVALID_PORT_HANDLE = 0xC0000042,

        /// <summary>
        /// Invalid FID: the file is closed.
        /// </summary>
        STATUS_FILE_CLOSED = 0xC0000128,

        /// <summary>
        /// Invalid FID: the handle is not closable.
        /// </summary>
        STATUS_HANDLE_NOT_CLOSABLE = 0xC0000235,

        /// <summary>
        /// Insufficient server memory to perform the requested operation: the section is too big.
        /// </summary>
        STATUS_SECTION_TOO_BIG = 0xC0000040,

        /// <summary>
        /// Insufficient server memory to perform the requested operation: the paging files are too many.
        /// </summary>
        STATUS_TOO_MANY_PAGING_FILES = 0xC0000097,

        /// <summary>
        /// Insufficient server memory to perform the requested operation: the server resources are insuffice.
        /// </summary>
        STATUS_INSUFF_SERVER_RESOURCES = 0xC0000205,

        /// <summary>
        /// Invalid open mode: the access is invalid.
        /// </summary>
        STATUS_OS2_INVALID_ACCESS = 0x000C0001,

        /// <summary>
        /// Invalid open mode: the access is denied.
        /// </summary>
        STATUS_ACCESS_DENIED_OfERRbadaccess = 0xC00000CA,

        /// <summary>
        /// Bad data. (May be generated by IOCTL calls on the server.)
        /// </summary>
        STATUS_DATA_ERROR = 0xC000009C,

        /// <summary>
        /// A file system operation (such as a rename) across two devices was attempted.
        /// </summary>
        STATUS_NOT_SAME_DEVICE = 0xC00000D4,

        /// <summary>
        /// No (more) files found following a file search command.
        /// </summary>
        STATUS_NO_MORE_FILES = 0x80000006,

        /// <summary>
        /// General error.
        /// </summary>
        STATUS_UNSUCCESSFUL = 0xC0000001,

        /// <summary>
        /// Sharing violation. The violation of sharing is invalid.
        /// </summary>
        STATUS_SHARING_VIOLATION = 0xC0000043,

        /// <summary>
        /// Sharing violation. the lock conflict with sharing mode.
        /// </summary>
        STATUS_FILE_LOCK_CONFLICT = 0xC0000054,

        /// <summary>
        /// A lock request specified an invalid locking mode, or conflicted with an existing file lock.
        /// </summary>
        STATUS_LOCK_NOT_GRANTED = 0xC0000055,

        /// <summary>
        /// Attempted to read beyond the end of the file.
        /// </summary>
        STATUS_END_OF_FILE = 0xC0000011,

        /// <summary>
        /// This command is not supported by the server.
        /// </summary>
        STATUS_NOT_SUPPORTED = 0XC00000BB,

        /// <summary>
        /// An attempt to create a file or directory failed because an object with the same pathname already exists.
        /// </summary>
        STATUS_OBJECT_NAME_COLLISION = 0xC0000035,

        /// <summary>
        /// A parameter supplied with the message is invalid.
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// Invalid information level.
        /// </summary>
        STATUS_OS2_INVALID_LEVEL = 0x007C0001,

        /// <summary>
        /// An attempt was made to seek to a negative absolute offset within a file.
        /// </summary>
        STATUS_OS2_NEGATIVE_SEEK = 0x00830001,

        /// <summary>
        /// The byte range specified in an unlock request was not locked.
        /// </summary>
        STATUS_RANGE_NOT_LOCKED = 0xC000007E,

        /// <summary>
        /// No lock request was outstanding for the supplied cancel region.
        /// </summary>
        STATUS_OS2_CANCEL_VIOLATION = 0x00AD0001,

        /// <summary>
        /// Invalid named pipe: the information class is invalid.
        /// </summary>
        STATUS_INVALID_INFO_CLASS = 0xC0000003,

        /// <summary>
        /// Invalid named pipe: the pipe state is invalid.
        /// </summary>
        STATUS_INVALID_PIPE_STATE = 0xC00000AD,

        /// <summary>
        /// Invalid named pipe: the read mode is invalid.
        /// </summary>
        STATUS_INVALID_READ_MODE = 0xC00000B4,

        /// <summary>
        /// All instances of the designated named pipe are busy: instance not available.
        /// </summary>
        STATUS_INSTANCE_NOT_AVAILABLE = 0xC00000AB,

        /// <summary>
        /// All instances of the designated named pipe are busy: named pipe not available.
        /// </summary>
        STATUS_PIPE_NOT_AVAILABLE = 0xC00000AC,

        /// <summary>
        /// All instances of the designated named pipe are busy.
        /// </summary>
        STATUS_PIPE_BUSY = 0xC00000AE,

        /// <summary>
        /// The designated named pipe is in the process of being closed: pipe is closing.
        /// </summary>
        STATUS_PIPE_CLOSING = 0xC00000B1,

        /// <summary>
        /// The designated named pipe is in the process of being closed: pipe is empty.
        /// </summary>
        STATUS_PIPE_EMPTY = 0xC00000D9,

        /// <summary>
        /// The designated named pipe exists, but there is no server process listening on the server side.
        /// </summary>
        STATUS_PIPE_DISCONNECTED = 0xC00000B0,

        /// <summary>
        /// There is more data available to read on the designated named pipe: buffer is overflow.
        /// </summary>
        STATUS_BUFFER_OVERFLOW = 0x80000005,

        /// <summary>
        /// There is more data available to read on the designated named pipe: need more processing.
        /// </summary>
        STATUS_MORE_PROCESSING_REQUIRED = 0xC0000016,

        /// <summary>
        /// Either there are no extended attributes, or the available extended attributes did not fit into the response.
        /// </summary>
        STATUS_EA_TOO_LARGE = 0xC0000050,

        /// <summary>
        /// The server file system does not support Extended Attributes.
        /// </summary>
        STATUS_EAS_NOT_SUPPORTED = 0xC000004F,

        /// <summary>
        /// More changes have occurred within the directory than will fit within the specified Change Notify response buffer.
        /// </summary>
        STATUS_NOTIFY_ENUM_DIR = 0x0000010C,

        /// <summary>
        /// Unspecified server error.
        /// </summary>
        STATUS_INVALID_SMB = 0x00010002,

        /// <summary>
        /// Invalid password.
        /// </summary>
        STATUS_WRONG_PASSWORD = 0xC000006A,

        /// <summary>
        /// DFS pathname not on local server.
        /// </summary>
        STATUS_PATH_NOT_COVERED = 0xC0000257,

        /// <summary>
        /// Access denied. The specified UID does not have permission to execute the requested command within the current context (TID).
        /// </summary>
        STATUS_NETWORK_ACCESS_DENIED = 0xC00000CA,

        /// <summary>
        /// The TID specified in the command was invalid: the network name is deleted.
        /// </summary>
        STATUS_NETWORK_NAME_DELETED = 0xC00000C9,

        /// <summary>
        /// The TID specified in the command was invalid: the tid is bad.
        /// </summary>
        STATUS_SMB_BAD_TID = 0x00050002,

        /// <summary>
        /// Invalid server name in Tree Connect.
        /// </summary>
        STATUS_BAD_NETWORK_NAME = 0xC00000CC,

        /// <summary>
        /// A printer request was made to a non-printer device or, conversely, a non-printer request was made to a printer device.
        /// </summary>
        STATUS_BAD_DEVICE_TYPE = 0xC00000CB,

        /// <summary>
        /// An unknown SMB command code was received by the server.
        /// </summary>
        STATUS_SMB_BAD_COMMAND = 0x00160002,

        /// <summary>
        /// Print queue is full - too many queued items.
        /// </summary>
        STATUS_PRINT_QUEUE_FULL = 0xC00000C6,

        /// <summary>
        /// Print queue is full - no space for queued item, or queued item too big.
        /// </summary>
        STATUS_NO_SPOOL_SPACE = 0xC00000C7,

        /// <summary>
        /// Invalid FID for print file.
        /// </summary>
        STATUS_PRINT_CANCELLED = 0xC00000C8,

        /// <summary>
        /// Internal server error.
        /// </summary>
        STATUS_UNEXPECTED_NETWORK_ERROR = 0xC00000C4,

        /// <summary>
        /// Operation timed out: io is timeout.
        /// </summary>
        STATUS_IO_TIMEOUT = 0xC00000B5,

        /// <summary>
        /// No resources currently available for this SMB request.
        /// </summary>
        STATUS_REQUEST_NOT_ACCEPTED = 0xC00000D0,

        /// <summary>
        /// Too many UIDs active for this SMB session.
        /// </summary>
        STATUS_TOO_MANY_SESSIONS = 0xC00000CE,

        /// <summary>
        /// The UID specified is not known as a valid ID on this server session.
        /// </summary>
        STATUS_SMB_BAD_UID = 0x005B0002,

        /// <summary>
        /// Temporarily unable to support RAW mode transfers. Use MPX mode.
        /// </summary>
        STATUS_SMB_USE_MPX = 0x00FA0002,

        /// <summary>
        /// Temporarily unable to support RAW or MPX mode transfers. Use standard read/write.
        /// </summary>
        STATUS_SMB_USE_STANDARD = 0x00FB0002,

        /// <summary>
        /// Continue in MPX mode. This error code is reserved for future use.
        /// </summary>
        STATUS_SMB_CONTINUE_MPX = 0x00FC0002,

        /// <summary>
        /// User account on the target machine is disabled.
        /// </summary>
        STATUS_ACCOUNT_DISABLED = 0xC0000072,

        /// <summary>
        /// User account on the target machine has expired.
        /// </summary>
        STATUS_ACCOUNT_EXPIRED = 0xC0000193,

        /// <summary>
        /// The client does not have permission to access this server.
        /// </summary>
        STATUS_INVALID_WORKSTATION = 0xC0000070,

        /// <summary>
        /// Access to the server is not permitted at this time.
        /// </summary>
        STATUS_INVALID_LOGON_HOURS = 0xC000006F,

        /// <summary>
        /// The user's password has expired.
        /// </summary>
        STATUS_PASSWORD_EXPIRED = 0xC0000071,

        /// <summary>
        /// The user's password has expired. the password must change.
        /// </summary>
        STATUS_PASSWORD_MUST_CHANGE = 0xC0000224,

        /// <summary>
        /// Function not supported by the server.
        /// </summary>
        STATUS_SMB_NO_SUPPORT = 0XFFFF0002,

        /// <summary>
        /// Attempt to modify a read-only file system.
        /// </summary>
        STATUS_MEDIA_WRITE_PROTECTED = 0xC00000A2,

        /// <summary>
        /// Drive not ready.
        /// </summary>
        STATUS_NO_MEDIA_IN_DEVICE = 0xC0000013,

        /// <summary>
        /// Unknown command.
        /// </summary>
        STATUS_INVALID_DEVICE_STATE = 0xC0000184,

        /// <summary>
        /// Data error.
        /// </summary>
        STATUS_DATA_ERROR_OfERRdata = 0xC000003E,

        /// <summary>
        /// Data error (incorrect CRC).
        /// </summary>
        STATUS_CRC_ERROR = 0xC000003F,

        /// <summary>
        /// Unknown media type.
        /// </summary>
        STATUS_DISK_CORRUPT_ERROR = 0xC0000032,

        /// <summary>
        /// Sector not found.
        /// </summary>
        STATUS_NONEXISTENT_SECTOR = 0xC0000015,

        /// <summary>
        /// Printer out of paper.
        /// </summary>
        STATUS_DEVICE_PAPER_EMPTY = 0x8000000E,

        /// <summary>
        /// The wrong disk was found in a drive.
        /// </summary>
        STATUS_WRONG_VOLUME = 0xC0000012,

        /// <summary>
        /// No space on file system.
        /// </summary>
        STATUS_DISK_FULL = 0xC000007F,
    }


    /// <summary>
    /// Data buffer format codes are used to identify the type and format of the fields 
    /// that immediately follow them in the data block of SMB messages. See section 2.2.3.3 
    /// for a description of the data block.
    /// In Core Protocol commands, every field in the data block (following the ByteCount field) is preceded by a
    /// one-byte buffer format field. Commands introduced in dialects subsequent to the Core Protocol typically 
    /// do not include buffer format fields unless they are intended as an extension to an existing command. 
    /// For example, SMB_COM_FIND was introduced in the LAN Manager 1.0 dialect in order to improve the semantics 
    /// of the SMB_COM_SEARCH Core Protocol command. Both commands share the same request and response message 
    /// structures, including the buffer format fields.
    /// </summary>
    public enum DataBufferFormat : byte
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// A two-byte USHORT value indicating the length of the data buffer. 
        /// The data buffer follows immediately after the length field.
        /// </summary>
        DataBuffer = 0x01,

        /// <summary>
        /// A NUL-terminated OEM_STRING.
        /// This format code is used only in the SMB_COM_NEGOTIATE command to identify SMB dialect strings.
        /// </summary>
        DialectString = 0x02,

        /// <summary>
        /// A NUL-terminated string representing a file system path.
        /// In the NT LAN Manager dialect, the string is of type SMB_STRING unless otherwise specified.
        /// </summary>
        PathName = 0x03,

        /// <summary>
        /// A NUL-terminated string.
        /// In the NT LAN Manager dialect, the string is of type SMB_STRING unless otherwise specified.
        /// </summary>
        SmbString = 0x04,

        /// <summary>
        /// A two-byte USHORT value indicating the length of the variable block.
        /// The variable block follows immediately after the length field.
        /// </summary>
        VariableBlock = 0x05,
    }

    #endregion


    #region 2.2.3 SMB Message Structure

    /// <summary>
    /// The header identifies the message as an SMB message, 
    /// specifies the command to be executed, and provides context.
    /// In a response message, the header also includes status information
    /// that indicates whether (and how) the command succeeded or failed.
    /// The SMB_Header structure is a fixed 32-bytes in length.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SmbHeader
    {
        /// <summary>
        /// This field MUST contain the 4-byte literal string '\xFF', 'S', 'M', 'B', 
        /// with the letters represented by their respective ASCII values in the order shown. 
        /// In the earliest available SMB documentation,
        /// this field is defined as a one byte message type (0xFF) followed by a three byte server type identifier.
        /// </summary>
        public uint Protocol;

        /// <summary>
        /// A one-byte command code. 
        /// </summary>
        public SmbCommand Command;

        /// <summary>
        /// 32-bit field used to communicate error messages from the server to the client.
        /// </summary>
        public uint Status;

        /// <summary>
        /// An 8-bit field of 1-bit flags describing various features in effect for the message
        /// </summary>
        public SmbFlags Flags;

        /// <summary>
        /// A 16-bit field of 1-bit flags that represent various features in effect for the message.
        /// Unspecified bits are reserved and MUST be zero
        /// </summary>
        public SmbFlags2 Flags2;

        /// <summary>
        /// If set to a non-zero value, this field represents the high-order
        /// bytes of a process identifier (PID). It is combined with the PIDLow field below to form a full PID.
        /// </summary>
        public ushort PidHigh;

        /// <summary>
        /// UCHAR This 8-byte field has three possible interpretations. 
        /// </summary>
        public ulong SecurityFeatures;

        /// <summary>
        /// This field is reserved and MUST be zero
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// A tree identifier (TID).
        /// </summary>
        public ushort Tid;

        /// <summary>
        /// The lower 16-bits of the PID.
        /// </summary>
        public ushort PidLow;

        /// <summary>
        /// A user identifier (UID).
        /// </summary>
        public ushort Uid;

        /// <summary>
        /// A multiplex identifier (MID).
        /// </summary>
        public ushort Mid;

        #region properties
        /// <summary>
        /// Access pid as uint.
        /// </summary>
        public uint Pid
        {
            get
            {
                return (uint)(PidHigh << 16 | PidLow);
            }
            set
            {
                PidLow = (ushort)value;
                PidHigh = (ushort)(value >> 16);
            }
        }
        #endregion
    }


    /// <summary>
    /// SMB was originally designed as a rudimentary remote procedure call protocol, 
    /// and the parameter block was defined as an array of "one word (two byte) fields containing 
    /// SMB command dependent parameters". In the CIFS dialect, however, the SMB_Parameters.Words 
    /// array MAY contain any arbitrary structure. The format of the SMB_Parameters.Words structure 
    /// is defined individually for each command message. The size of the Words array is still measured
    /// as a count of byte pairs.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SmbParameters
    {
        /// <summary>
        /// The size, in two-byte words, of the Words field. This field MAY be zero,
        /// indicating that the Words field is empty. Note that within the fixed 32 bytes 
        /// of an SMB header, the WordCount field is one byte, causing the Words field to not be word aligned.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The message-specific parameters structure.
        /// The size of this field MUST be (2 x WordCount) bytes. If WordCount is 0, this field is not included
        /// </summary>
        [Size("WordCount")]
        public ushort[] Words;
    }


    /// <summary>
    /// The general structure of the data block is similar to that of the Parameter block,
    /// except that the length of the buffer portion is measured in bytes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SmbData
    {
        /// <summary>
        /// The size, in bytes, of the Bytes field. This field MAY be zero, 
        /// indicating that the Bytes field is empty. Because the SMB_Parameters.
        /// Words field is unaligned and the SMB_Data.ByteCount field is two bytes in size, 
        /// the first byte of SMB_Data.Bytes is also unaligned.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// The message-specific data structure. The size of this field MUST be ByteCount bytes. 
        /// If ByteCount is 0, this field is not included.
        /// </summary>
        [Size("ByteCount")]
        public byte[] Bytes;
    }


    /// <summary>
    /// An 8-bit field of 1-bit flags describing various features in effect for the message
    /// </summary>
    [Flags()]
    public enum SmbFlags : byte
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// This bit is set (1) in the SMB_COM_NEGOTIATE response if the 
        /// server supports SMB_COM_LOCK_AND_READ and SMB_COM_WRITE_AND_UNLOCK commands.
        /// </summary>
        SMB_FLAGS_LOCK_AND_READ_OK = 0x01,

        /// <summary>
        /// Obsolete
        /// When on (on an SMB request being sent to the server), the client guarantees that there is a 
        /// receive buffer posted such that a send without acknowledgment can be used by the server to respond 
        /// to the client's request.
        /// This behavior is specific to an obsolete transport. This bit MUST be set to zero by the client and
        /// MUST be ignored by the server.
        /// </summary>
        SMB_FLAGS_BUF_AVAIL = 0x02,

        /// <summary>
        /// Obsolete
        /// If this bit is set then all pathnames in the SMB SHOULD be treated as case-insensitive. 
        /// </summary>
        SMB_FLAGS_CASE_INSENSITIVE = 0x08,

        /// <summary>
        /// Obsolescent
        /// When set in session setup this bit indicates that all paths sent to the server are already 
        /// in canonicalized format. That is, all file and directory names are composed of valid file name 
        /// characters in all upper-case, and that the path segments are separated by backslash characters ('\').
        /// </summary>
        SMB_FLAGS_CANONICALIZED_PATHS = 0x10,

        /// <summary>
        /// Obsolescent
        /// This bit has meaning only in the deprecated SMB_COM_OPEN, SMB_COM_CREATE,
        /// and SMB_COM_CREATE_NEW SMB request messages where it is used to indicate that the client 
        /// is requesting an Exclusive OpLock. It SHOULD be set to zero by the client, and ignored by the server, 
        /// in all other SMB requests. If the server grants this OpLock request, then this bit SHOULD remain set in
        /// the corresponding response SMB to indicate to the client that the OpLock request was granted.
        /// </summary>
        SMB_FLAGS_OPLOCK = 0x20,

        /// <summary>
        /// Obsolescent
        /// This bit has meaning only in the deprecated SMB_COM_OPEN, SMB_COM_CREATE,
        /// and SMB_COM_CREATE_NEW SMB request messages, where it is used to indicate that the client is requesting
        /// a Batch OpLock. It SHOULD be set to zero by the client, and ignored by the server, in all other SMB 
        /// requests.If the server grants this OpLock request, then this bit SHOULD remain set in the corresponding 
        /// response SMB to indicate to the client that the OpLock request was granted.
        /// If the SMB_FLAGS_OPLOCK bit is clear (0), then the SMB_FLAGS_OPBATCH bit is ignored.
        /// </summary>
        SMB_FLAGS_OPBATCH = 0x40,

        /// <summary>
        /// When on, this message is being sent from the server in response to a client request. 
        /// The Command field usually contains the same value in a protocol request from the client 
        /// to the server as in the matching response from the server to the client. This bit unambiguously 
        /// distinguishes the message as a server response.
        /// </summary>
        SMB_FLAGS_REPLY = 0x80,
    }


    /// <summary>
    /// A 16-bit field of 1-bit flags that represent various features in effect for the message. Unspecified bits
    /// are reserved and MUST be zero
    /// </summary>
    [Flags()]
    public enum SmbFlags2 : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// If the bit is set then the message MAY contain long file names.
        /// If the bit is clear then file names in the message MUST adhere to the 8.3 naming convention.
        /// If set in a client request for directory enumeration, the server MAY return long names (that is, names 
        /// that are not 8.3 names) in the response to this request. If not set in a client request for directory 
        /// enumeration, the server MUST return only the 8.3 Name in the response to this request. This flag indicates
        /// that in a direct enumeration request, paths returned by the server are not restricted to 8.3 names format.
        /// This bit field SHOULD be set to 1 when the negotiated dialect is LANMAN2.0 or later. 
        /// </summary>
        SMB_FLAGS2_LONG_NAMES = 0x0001,

        /// <summary>
        /// If the bit is set the client is aware of extended file attributes. 
        /// The client MUST set this bit if the client is aware of extended attributes.
        /// In response to a client request with this flag set, a server MAY include extended attributes in the 
        /// response. This bit field SHOULD be set to 1 when the negotiated dialect is LANMAN2.0 or later.
        /// </summary>
        SMB_FLAGS2_EAS = 0x0002,

        /// <summary>
        /// If set by the client, the client is requesting signing (if signing is
        /// not yet active) or the message being sent is signed, as specified in section 3.???.
        /// This bit is used on the SMB header of an SMB_COM_SESSION_SETUP_ANDX client request 
        /// (section 2.2.4.53) to indicate that the client supports signing and the server can 
        /// choose to enforce signing on the connection based on its configuration.
        /// If the server wants to turn on signing for this connection, it MUST set this flag 
        /// and also sign the SMB_COM_SESSION_SETUP_ANDX response (section 2.2.4.53), after which all of the traffic
        /// on the connection (except for OpLock Break requests) MUST be signed. In the SMB header of other CIFS client
        /// requests, the setting of this bit indicates that the packet has been signed. This bit field SHOULD be set
        /// to 1 when the negotiated dialect is NT LANMAN or later.
        /// </summary>
        SMB_FLAGS2_SMB_SECURITY_SIGNATURE = 0x0004,

        /// <summary>
        /// Reserved but not implemented.
        /// </summary>
        SMB_FLAGS2_IS_LONG_NAME = 0x0040,

        /// <summary>
        /// If the bit is set, any pathnames in this SMB SHOULD be resolved in the Distributed File System (DFS).
        /// </summary>
        SMB_FLAGS2_DFS = 0x1000,

        /// <summary>
        /// This flag is only useful on a read request.
        /// If the bit is set, then the client MAY read the file 
        /// if the client does not have read permission but does have execute permission.
        /// This bit field SHOULD be set to 1 when the negotiated dialect is LANMAN2.0 or later.
        /// This flag is also known as SMB_FLAGS2_READ_IF_EXECUTE.
        /// </summary>
        SMB_FLAGS2_PAGING_IO = 0x2000,

        /// <summary>
        /// If this bit is set in a client request, the server MUST return 
        /// errors as a 32-bit NTSTATUS codes in the response. If it is clear, the server MUST return errors in 
        /// SMBSTATUS format. If this bit is set in the server response, the Status field in the header 
        /// is formatted as an NTSTATUS code, else it is in SMBSTATUS format.
        /// </summary>
        SMB_FLAGS2_NT_STATUS = 0x4000,

        /// <summary>
        /// If set in a client request or server response, any fields that contain strings in this SMB message MUST 
        /// be encoded as an array of 16-bit Unicode characters, unless otherwise specified.
        /// If this bit is clear, these fields MUST be encoded as an array of OEM characters. This bit field SHOULD
        /// be set to 1 when the negotiated dialect is NT LANMAN.
        /// </summary>
        SMB_FLAGS2_UNICODE = 0x8000,
    }


    /// <summary>
    /// This 8-byte field has three possible interpretations. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SecurityFeatures
    {
        /// <summary>
        /// An encryption key used for validating a messages over connectionless transports.
        /// </summary>
        public uint Key;

        /// <summary>
        /// A session identifier (SID).
        /// </summary>
        public ushort Sid;

        /// <summary>
        /// A number used to identify the sequence of a message over connectionless transports.
        /// </summary>
        public ushort SequenceNumber;
    }


    /// <summary>
    /// The command code associated with the next block pair in the AndX Chain.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AndX
    {
        /// <summary>
        /// This field is reserved and MUST be zero
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// This field is reserved and MUST be zero
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// The offset in bytes, relative to the start of the SMB header, 
        /// of the next Parameter block in the AndX Message. This offset is 
        /// independent of any other size parameters or offsets within the command. This offset MAY point to a
        /// location past the end of the current block pair.
        /// </summary>
        public ushort AndXOffset;
    }

    #endregion


    #region 2.2.4 SMB Commands

    #region 2.2.4.1   	SMB_COM_CREATE_DIRECTORY (0x00)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CREATE_DIRECTORY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_DIRECTORY_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0.
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CREATE_DIRECTORY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_DIRECTORY_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04.
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// A null-terminated string giving the full pathname, relative to the supplied TID, of the directory to be
        /// created.
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] DirectoryName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CREATE_DIRECTORY Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_DIRECTORY_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0.
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CREATE_DIRECTORY Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_DIRECTORY_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0.
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.2   	SMB_COM_DELETE_DIRECTORY (0x01)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_DELETE_DIRECTORY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_DELETE_DIRECTORY_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message.
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_DELETE_DIRECTORY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_DELETE_DIRECTORY_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST contain the value 0x04
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// A null-terminated string that contains the full pathname, relative to the supplied TID, 
        /// of the directory to be deleted. 
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] DirectoryName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_DELETE_DIRECTORY Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_DELETE_DIRECTORY_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message.
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_DELETE_DIRECTORY Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_DELETE_DIRECTORY_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message.
        /// </summary>
        public ushort ByteCount;
    }


    #endregion

    #region 2.2.4.3   	SMB_COM_OPEN (0x02)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_OPEN Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 2
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// A 16-bit field for encoding the requested access mode
        /// </summary>
        public ushort AccessMode;

        /// <summary>
        /// Specifies the type of file desired. This field is used as a search mask.
        /// Both the FileName and the SearchAttributes of a file MUST match for the file to be opened. 
        /// </summary>
        public SmbFileAttributes SearchAttributes;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_OPEN Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// The value of this field MUST be 0x04.
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// A null-terminated string containing the file name of the file to be opened. 
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] FileName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_OPEN Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 7. The length in bytes of the remaining SMB_Parameters.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The FID returned for the open file
        /// </summary>
        public ushort FID;

        /// <summary>
        /// The set of attributes currently assigned to the file. 
        /// This field is formatted in the same way as the SearchAttributes field in the request.
        /// </summary>
        public SmbFileAttributes FileAttrs;

        /// <summary>
        /// Time of the last modification to the opened file
        /// </summary>
        public UTime LastModified;

        /// <summary>
        /// The current size of the opened file in bytes
        /// </summary>
        public uint FileSize;

        /// <summary>
        /// A 16-bit field for encoding the granted access mode.
        /// This field is formatted in the same way as the Request equivalent
        /// </summary>
        public ushort AccessMode;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_OPEN Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.4   	SMB_COM_CREATE (0x03)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CREATE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 3
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// A 16-bit field of 1-bit flags that represent the file attributes to assign to the file if it is created
        /// successfully.
        /// </summary>
        public SmbFileAttributes FileAttributes;

        /// <summary>
        /// The time the file was created on the client represented as the number of seconds since 
        /// Jan 1, 1970, 00:00:00.0. Server support of this field is OPTIONAL.
        /// </summary>
        public UTime CreationTime;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CREATE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_Request_SMB_Data
    {
        /// <summary>
        /// USHORT This field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// A null-terminated string that represents the fully qualified name of the file relative 
        /// to the supplied TID to create or truncate on the server
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] FileName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CREATE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The FID representing the file on the server. 
        /// This value MUST be supplied in the FID field of the SMB Header in subsequent requests that manipulate 
        /// the file
        /// </summary>
        public ushort FID;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CREATE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.5   	SMB_COM_CLOSE (0x04)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CLOSE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CLOSE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 3
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The FID of the object to be closed
        /// </summary>
        public ushort FID;

        /// <summary>
        /// A time value encoded as the number of seconds since January 1, 1970 00:00:00.0.
        /// The client MAY request that the last modification time for the file be updated to this time value. 
        /// A value of 0 or 0xFFFFFF results in the server using the default value. The server is NOT REQUIRED to 
        /// support this request
        /// </summary>
        public UTime LastTimeModified;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CLOSE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CLOSE_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CLOSE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CLOSE_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CLOSE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CLOSE_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.6   	SMB_COM_FLUSH (0x05)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_FLUSH Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FLUSH_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The FID of the file to be flushed. If this field is set to 0xFFFF (-1) all files opened by
        /// the same PID within the SMB connection are to be flushed
        /// </summary>
        public ushort FID;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_FLUSH Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FLUSH_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_FLUSH Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FLUSH_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_FLUSH Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FLUSH_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.7   	SMB_COM_DELETE (0x06)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_DELETE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_DELETE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The file attributes of the file(s) to be deleted. 
        /// If the value of this field is zero, then only normal files MUST be matched for deletion.
        /// If the System or Hidden attributes MUST be specified, then entries with those attributes 
        /// are matched in addition to the normal files.  Read-only files MAY NOT be deleted.  
        /// The read-only attribute of the file MUST be cleared before the file MAY be deleted.
        /// </summary>
        public SmbFileAttributes SearchAttributes;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_DELETE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_DELETE_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// the BufferFormat and pathname of the file(s) to be deleted
        /// Wildcards MAY be used in the filename component of the path
        /// </summary>
        [Size("ByteCount")]
        public byte[] BufferFormatAndFileName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_DELETE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_DELETE_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_DELETE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_DELETE_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.8   	SMB_COM_RENAME (0x07)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_RENAME Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_RENAME_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// Indicates the file attributes that the file(s) to be renamed MUST have. 
        /// If the value of this field is zero, then only normal files MUST be matched to be renamed.
        /// If the System or Hidden attributes are specified, then entries with those attributes MAY be matched in 
        /// addition to the normal files. Read-only files MAY NOT be renamed. The read-only attribute of the file MUST
        /// be cleared before it can be renamed.
        /// </summary>
        public SmbFileAttributes SearchAttributes;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_RENAME Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_RENAME_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04
        /// </summary>
        public byte BufferFormat1;

        /// <summary>
        /// A null-terminated string containing the name of the file or files to be renamed.
        /// Wildcards MAY be used in the filename component of the path
        /// </summary>
        public byte[] OldFileName;

        /// <summary>
        /// This field MUST be 0x04
        /// </summary>
        public byte BufferFormat2;

        /// <summary>
        /// A null-terminated string containing the new name(s) to be given to the file(s) that matches OldFileName 
        /// or the name of the destination directory into which the files matching OldFileName MUST be moved.
        /// </summary>
        public byte[] NewFileName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_RENAME Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_RENAME_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_RENAME Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_RENAME_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.9   	SMB_COM_QUERY_INFORMATION (0x08)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_QUERY_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_QUERY_INFORMATION_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_QUERY_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_QUERY_INFORMATION_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04.
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// A null-terminated string that represents the fully qualified name of the file relative to the supplied TID.
        /// This is the file for which attributes are queried and returned.
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] FileName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_QUERY_INFORMATION Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_QUERY_INFORMATION_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 10
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field is a 16 bit unsigned bit field encoded as SMB_FILE_ATTRIBUTES
        /// </summary>
        public SmbFileAttributes FileAttributes;

        /// <summary>
        /// The time of the last write to the file.
        /// </summary>
        public UTime LastWriteTime;

        /// <summary>
        /// This field contains the size of the file in bytes. 
        /// Since this size is limited to 32 bits this command is inappropriate for files whose size is too large
        /// </summary>
        public uint FileSize;

        /// <summary>
        /// Array of USHORT This field is reserved and MUST be 0.
        /// </summary>
        [StaticSize(5, StaticSizeMode.Elements)]
        public ushort[] Reserved;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_QUERY_INFORMATION Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_QUERY_INFORMATION_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.10   	SMB_COM_SET_INFORMATION (0x09)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_SET_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SET_INFORMATION_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 8
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field is a 16 bit unsigned bit field encoded as SMB_FILE_ATTRIBUTES 
        /// </summary>
        public SmbFileAttributes FileAttributes;

        /// <summary>
        /// The time of the last write to the file
        /// </summary>
        public UTime LastWriteTime;

        /// <summary>
        /// This field is reserved and MUST be set to 0
        /// </summary>
        [StaticSize(5, StaticSizeMode.Elements)]
        public ushort[] Reserved;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_SET_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SET_INFORMATION_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// A null-terminated string that represents the fully qualified name of the file relative 
        /// to the supplied TID. This is the file for which attributes are set.
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] FileName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_SET_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SET_INFORMATION_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_SET_INFORMATION Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SET_INFORMATION_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.11   	SMB_COM_READ (0x0A)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_READ Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 5
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid 16-bit signed integer indicating the file from which the data MUST be read
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This field is a 16-bit unsigned integer indicating the number of bytes to be read from the file.
        /// The client MUST ensure that the amount of data requested will fit in the negotiated maximum buffer size.
        /// </summary>
        public ushort CountOfBytesToRead;

        /// <summary>
        /// This field is a 32-bit unsigned integer indicating the offset in number of bytes from which to begin
        /// reading from the file.
        /// The client MUST ensure that the amount of data requested fits in the negotiated maximum buffer size.
        /// Because this field is limited to 32-bits this command is inappropriate for files having 64-bit offsets.
        /// </summary>
        public uint ReadOffsetInBytes;

        /// <summary>
        /// This field is a 16-bit unsigned integer indicating the remaining number of bytes that the client intends 
        /// to read from the file. 
        /// This is an advisory field and MAY be zero.
        /// </summary>
        public ushort EstimateOfRemainingBytesToBeRead;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_READ Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_READ Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 5
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The actual number of bytes returned to the client. 
        /// This MUST be equal to CountOfBytesToRead unless the end of file was reached before reading
        /// CoutOfBytesToRead bytes or the ReadOffsetInBytes pointed at or beyond the end of file.
        /// </summary>
        public ushort CountOfBytesReturned;

        /// <summary>
        /// Reserved and MUST be 0.
        /// </summary>
        [StaticSize(4, StaticSizeMode.Elements)]
        public ushort[] Reserved;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_READ Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 3 + CountOfBytesRead
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x01
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// The number of bytes read that are contained in the following array of bytes.
        /// </summary>
        public ushort CountOfBytesRead;

        /// <summary>
        /// The actual bytes read from the file
        /// </summary>
        [Size("CountOfBytesRead")]
        public byte[] Bytes;
    }

    #endregion

    #region 2.2.4.12   	SMB_COM_WRITE (0x0B)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 5
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid 16-bit signed integer indicating the file to which the data MUST be written
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This field is a 16-bit unsigned integer indicating the number of bytes to be written to the file. 
        /// The client MUST ensure that the amount of data sent can fit in the negotiated maximum buffer size
        /// </summary>
        public ushort CountOfBytesToWrite;

        /// <summary>
        /// This field is a 32-bit unsigned integer indicating the offset in number of bytes from the beginning of 
        /// the file at which to begin writing to the file. The client MUST ensure that the amount of data sent fits
        /// in the negotiated maximum buffer size. Because this field is limited to 32-bits this command is
        /// inappropriate for files having 64-bit offsets.
        /// </summary>
        public uint WriteOffsetInBytes;

        /// <summary>
        /// This field is a 16-bit unsigned integer indicating the remaining number of bytes that the client
        /// anticipates to write to the file. This is an advisory field and MAY be zero. This information MAY
        /// be used by the server to optimize cache behavior.
        /// </summary>
        public ushort EstimateOfRemainingBytesToBeWritten;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_Request_SMB_Data
    {
        /// <summary>
        /// CountOfBytesToWrite + 3
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x01
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// This field MUST match 
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// The raw bytes to be written to the file
        /// </summary>
        [Size("ByteCount - 3")]
        public byte[] Data;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// Indicates the actual number of bytes written to the file. For successful writes, 
        /// this MUST equal the CountOfBytesToWrite in the client Request. If the number of 
        /// bytes written differs from the number requested and no error is indicated, then the server has 
        /// no resources available to satisfy the complete write
        /// </summary>
        public ushort CountOfBytesWritten;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.13   	SMB_COM_LOCK_BYTE_RANGE (0x0C)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_LOCK_BYTE_RANGE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOCK_BYTE_RANGE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 5
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid 16-bit signed integer indicating the file from which the data MUST be read
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This field is a 32-bit unsigned integer indicating the number of contiguous bytes to be locked.
        /// </summary>
        public uint CountOfBytesToLock;

        /// <summary>
        /// This field is a 32-bit unsigned integer indicating the offset in number of bytes from which to begin 
        /// the lock. Because this field is limited to 32-bits this command is inappropriate for files having 
        /// 64-bit offsets
        /// </summary>
        public uint LockOffsetInBytes;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_LOCK_BYTE_RANGE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOCK_BYTE_RANGE_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_LOCK_BYTE_RANGE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOCK_BYTE_RANGE_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_LOCK_BYTE_RANGE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOCK_BYTE_RANGE_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message.
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.14   	SMB_COM_UNLOCK_BYTE_RANGE (0x0D)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_UNLOCK_BYTE_RANGE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_UNLOCK_BYTE_RANGE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 5
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid 16-bit signed integer indicating the file from which the data MUST be read.
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This field is a 32-bit unsigned integer indicating the number of contiguous bytes to be unlocked.
        /// </summary>
        public uint CountOfBytesToUnlock;

        /// <summary>
        /// This field is a 32-bit unsigned integer indicating the offset in number of bytes 
        /// from which to begin the unlock. Because this field is limited to 32-bits this command is inappropriate
        /// for files having 64-bit offsets
        /// </summary>
        public uint UnlockOffsetInBytes;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_UNLOCK_BYTE_RANGE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_UNLOCK_BYTE_RANGE_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_UNLOCK_BYTE_RANGE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_UNLOCK_BYTE_RANGE_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_UNLOCK_BYTE_RANGE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_UNLOCK_BYTE_RANGE_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.15   	SMB_COM_CREATE_TEMPORARY (0x0E)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CREATE_TEMPORARY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_TEMPORARY_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 6
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field SHOULD be ignored by the server. 
        /// </summary>
        public SmbFileAttributes FileAttributes;

        /// <summary>
        /// The time the file was created on the client represented as the number of seconds 
        /// since Jan 1, 1970, 00:00:00.0. Server support of this field is OPTIONAL
        /// </summary>
        public UTime CreationTime;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CREATE_TEMPORARY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_TEMPORARY_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// A null-terminated string that represents the fully qualified name
        /// of the directory relative to the supplied TID in which to create the temporary file.
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] DirectoryName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CREATE_TEMPORARY Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_TEMPORARY_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 2
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The FID representing the file on the server.
        /// This value MUST be supplied in the FID field of the SMB Header in subsequent requests which manipulate
        /// the file
        /// </summary>
        public ushort FID;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CREATE_TEMPORARY Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_TEMPORARY_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// A null-terminated string that contains the temporary file name generated by the server.
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] TemporaryFileName;
    }

    #endregion

    #region 2.2.4.16   	SMB_COM_CREATE_NEW (0x0F)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CREATE_NEW Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_NEW_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 3
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// A 16-bit field of 1-bit flags that represent the
        /// file attributes to assign to the file if it is created successfully
        /// </summary>
        public SmbFileAttributes FileAttributes;

        /// <summary>
        /// The time the file was created on the client 
        /// represented as the number of seconds since Jan 1, 1970, 00:00:00.0
        /// </summary>
        public UTime CreationTime;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CREATE_NEW Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_NEW_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// MUST be 0x04 the format code for an SMB_STRING
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// A null-terminated string that contains the fully qualified name of the file,
        /// relative to the supplied TID, to create on the server
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] FileName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CREATE_NEW Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_NEW_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The FID representing the file on the server. 
        /// This value MUST be supplied in subsequent requests that manipulate the file
        /// </summary>
        public ushort FID;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CREATE_NEW Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CREATE_NEW_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.17   	SMB_COM_CHECK_DIRECTORY (0x10)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CHECK_DIRECTORY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CHECK_DIRECTORY_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be zero. No parameters are sent by this message. 
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CHECK_DIRECTORY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CHECK_DIRECTORY_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04. This is a buffer type indicator that identifies the next field as an SMB_STRING
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// A null-terminated character string giving the pathname to be tested.
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] DirectoryName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CHECK_DIRECTORY Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CHECK_DIRECTORY_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be zero. No parameters are sent by this message.
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CHECK_DIRECTORY Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CHECK_DIRECTORY_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be zero. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.18   	SMB_COM_PROCESS_EXIT (0x11)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_PROCESS_EXIT Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_PROCESS_EXIT_Request_SMB_Parameters
    {
        /// <summary>
        /// MUST be zero. No parameters are sent
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_PROCESS_EXIT Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_PROCESS_EXIT_Request_SMB_Data
    {
        /// <summary>
        /// MUST be zero. No data bytes are sent
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_PROCESS_EXIT Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_PROCESS_EXIT_Response_SMB_Parameters
    {
        /// <summary>
        /// MUST be zero. No parameters are returned
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_PROCESS_EXIT Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_PROCESS_EXIT_Response_SMB_Data
    {
        /// <summary>
        /// MUST be zero. No data bytes are returned
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.19   	SMB_COM_SEEK (0x12)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_SEEK Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SEEK_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 4.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The File ID of the open file within which to seek
        /// </summary>
        public ushort FID;

        /// <summary>
        /// The seek mode.
        /// </summary>
        public ushort Mode;

        /// <summary>
        /// A 32-bit signed long value indicating the file position,
        /// relative to the position indicated in Mode, to which to set the updated file pointer. 
        /// The value of Offset ranges from -2TB to +2TB (-231 to (231 -1) bytes). 
        /// </summary>
        public int Offset;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_SEEK Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SEEK_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be zero. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_SEEK Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SEEK_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 2
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// A 32-bit unsigned value indicating the absolute file position, relative to the start of the file, 
        /// at which the file pointer is currently set. The value of Offset ranges from 0 to 4TB (0 to 232 - 1 bytes).
        /// </summary>
        public uint Offset;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_SEEK Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SEEK_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// The seek mode. Possible values are
    /// </summary>
    public enum SeekModeValues
    {
        /// <summary>
        /// Seek from the start of the file.
        /// </summary>
        Start = 0,

        /// <summary>
        /// Seek from the current position.
        /// </summary>
        Current = 1,

        /// <summary>
        /// Seek from the end of the file.
        /// </summary>
        End = 2
    }

    #endregion

    #region 2.2.4.20   	SMB_COM_LOCK_AND_READ (0x13)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_LOCK_AND_READ Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOCK_AND_READ_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 5
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid 16-bit signed integer indicating the file from which the data MUST be read
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This field is a 16-bit unsigned integer indicating the number of bytes to be read from the file. 
        /// The client MUST ensure that the amount of data requested will fit in the negotiated maximum buffer size
        /// </summary>
        public ushort CountOfBytesToRead;

        /// <summary>
        /// This field is a 32-bit unsigned integer indicating the offset in number of bytes from which to begin
        /// reading from the file. 
        /// The client MUST ensure that the amount of data requested fits in the negotiated maximum buffer size.
        /// Because this field is limited to 32-bits this command is inappropriate for files having 64-bit offsets.
        /// </summary>
        public uint ReadOffsetInBytes;

        /// <summary>
        /// This field is a 16-bit unsigned integer indicating the remaining number 
        /// of bytes that the client expects to read from the file. This is an advisory field and MAY be zero.
        /// </summary>
        public ushort EstimateOfRemainingBytesToBeRead;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_LOCK_AND_READ Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOCK_AND_READ_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_LOCK_AND_READ Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOCK_AND_READ_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 5
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The actual number of bytes returned to the client. 
        /// This MUST be equal to CountOfBytesToRead unless the end of file was reached before reading 
        /// CoutOfBytesToRead bytes or the ReadOffsetInBytes pointed at or beyond the end of file.
        /// </summary>
        public ushort CountOfBytesReturned;

        /// <summary>
        /// Reserved and MUST be zero
        /// </summary>
        [StaticSize(4, StaticSizeMode.Elements)]
        public ushort[] Reserved;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_LOCK_AND_READ Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOCK_AND_READ_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 3 + CountOfBytesRead
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x01
        /// </summary>
        public byte BufferType;

        /// <summary>
        /// The number of bytes read which are contained in the following array of bytes.
        /// </summary>
        public ushort CountOfBytesRead;

        /// <summary>
        /// The array of bytes read from the file. The array is not null-terminated.
        /// </summary>
        [Size("CountOfBytesRead")]
        public byte[] Bytes;
    }

    #endregion

    #region 2.2.4.21   	SMB_COM_WRITE_AND_UNLOCK (0x14)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_AND_UNLOCK Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_AND_UNLOCK_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 5
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid 16-bit signed integer 
        /// indicating the file to which the data MUST be written
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This field is a 16-bit unsigned integer indicating the number of bytes to be written to the file. 
        /// The client MUST ensure that the amount of data sent can fit in the negotiated maximum buffer size
        /// </summary>
        public ushort CountOfBytesToWrite;

        /// <summary>
        /// This field is a 32-bit unsigned integer indicating the offset in number of bytes from the beginning 
        /// of the file at which to begin writing to the file. The client MUST ensure that the amount of data sent 
        /// can fit in the negotiated maximum buffer size. 
        /// Because this field is limited to 32-bits this command is inappropriate for files having 64-bit offsets.
        /// </summary>
        public uint WriteOffsetInBytes;

        /// <summary>
        /// This field is a 16-bit unsigned integer indicating the remaining number of bytes the client anticipates 
        /// to write to the file. 
        /// This is an advisory field and MAY be zero. This information can be used by the server to optimize 
        /// cache behavior.
        /// </summary>
        public ushort EstimateOfRemainingBytesToBeWritten;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_AND_UNLOCK Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_AND_UNLOCK_Request_SMB_Data
    {
        /// <summary>
        /// CountOfBytesToWrite + 3
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x01
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// This field MUST be CountOfBytesToWrite
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// The raw bytes to be written to the file
        /// </summary>
        [Size("DataLength")]
        public byte[] Data;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_AND_UNLOCK Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_AND_UNLOCK_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// Indicates the actual number of bytes written to the file.
        /// For successful writes, this MUST equal the CountOfBytesToWrite in the client Request. 
        /// If the number of bytes written differs from the number requested and no error is indicated, 
        /// then the server has no resources available to satisfy the complete write
        /// </summary>
        public ushort CountOfBytesWritten;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_AND_UNLOCK Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_AND_UNLOCK_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.22   	SMB_COM_READ_RAW (0x1A)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_READ_RAW Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_RAW_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be either 8 or 10
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid 16-bit signed integer indicating the file from which the data MUST be read
        /// </summary>
        public ushort FID;

        /// <summary>
        /// The offset in bytes from the start of the file at which the read MUST begin. 
        /// This is the lower 32 bits of a 64 bit value if the WordCount is 10
        /// </summary>
        public uint Offset;

        /// <summary>
        /// The requested maximum number of bytes to read from the file and return to the client.
        /// The value MAY exceed the negotiated buffer size
        /// </summary>
        public ushort MaxCountOfBytesToReturn;

        /// <summary>
        /// The requested minimum number of bytes to read from the file and return to the client. 
        /// This field is used only when reading from a named pipe or a device. It is ignored when reading from
        /// a standard file.
        /// </summary>
        public ushort MinCountOfBytesToReturn;

        /// <summary>
        /// Support for this field is optional and this field is used only when reading from a named pipe or
        /// i/o device. It does not apply when reading from a regular file. If Timeout is 0 or the server does
        /// not support Timeout and no data is currently available, the server MUST send a zero byte response. 
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// This field SHOULD be set to 0
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// This field is optional, and is included only when WordCount is 10. This field is the upper 32 bits 
        /// of the offset in bytes from the start of the file at which the read MUST start. This field allows 
        /// the client request to specify 64 bit file offsets
        /// </summary>
        public uint OffsetHigh;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_READ_RAW Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_RAW_Request_SMB_Data
    {
        /// <summary>
        /// The length in bytes of the remaining SMB_Data. This field MUST be 0.
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// The server MUST not return the typical response data when responding to this request. 
    /// The server MUST respond with one message containing the raw data being read from the file or named pipe. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_RAW_Response
    {
        /// <summary>
        /// The server MUST not return the typical response data when responding to this request. 
        /// The server MUST respond with one message containing the raw data being read from the file or named pipe. 
        /// </summary>
        public byte[] ResponseData;
    }

    #endregion

    #region 2.2.4.23   	SMB_COM_READ_MPX (0x1B)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_READ_MPX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_MPX_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 8. The length in two-byte words of the remaining SMB_Parameters.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid 16-bit signed integer indicating the file from which the data MUST be read
        /// </summary>
        public ushort FID;

        /// <summary>
        /// The offset in bytes from the start of the file at which the read begins
        /// </summary>
        public uint Offset;

        /// <summary>
        /// The requested maximum number of bytes to read from the file and return to the client. 
        /// The value MAY exceed the negotiated buffer size
        /// </summary>
        public ushort MaxCountOfBytesToReturn;

        /// <summary>
        /// The requested minimum number of bytes to read from the file and return to the client. 
        /// This field is used only when reading from a named pipe or a device. 
        /// It MUST be ignored when reading from a standard file
        /// </summary>
        public ushort MinCountOfBytesToReturn;

        /// <summary>
        /// Support for this field is optional and this field is used only when reading from a named pipe or
        /// I/O device. It does not apply when reading from a regular file.If Timeout is zero or the server does
        /// not support Timeout and no data is currently available, the server MUST send a successful response 
        /// with the DataLength field set to zero. If the Timeout value is -1 (0xFFFFFFFF, "wait forever"), then 
        /// the server MUST wait until there are at least MinCountOfBytesToReturn bytes of data read from the 
        /// device before returning a response to the client. If the Timeout value is -2 (0xFFFFFFFE, "default") 
        /// the server MUST wait for the default timeout associated with the named pipe or I/O device. If the 
        /// server supports Timeout and Timeout is greater than zero, the server MUST wait to send the response 
        /// until the either the MinCountOfBytesToReturn or more data become available or the Timeout in 
        /// milliseconds elapses. If Timeout is greater than zero and the milliseconds elapse before the 
        /// MinCountOfBytesToReturn are read, the server MUST send a response with the Error field set to 
        /// indicate that the Timeout occurred along with any bytes already read.  
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// This field SHOULD be set to zero
        /// </summary>
        public ushort Reserved;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_READ_MPX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_MPX_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be zero. No data is sent by this message.
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_READ_MPX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_MPX_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 8. The length in two-byte words of the remaining SMB_Parameters.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The offset in bytes from the start of the file at which the read occurred.
        /// </summary>
        public uint Offset;

        /// <summary>
        /// The total number of bytes expected to be returned in all responses to this request. 
        /// This value usually starts at MaxCountOfBytesToReturn, but MAY be an over estimate. 
        /// The overestimate MAY be reduced while the read is in progress. The last response generated by 
        /// the server MUST contain the actual total number of read bytes sent to the client in all of the responses. 
        /// If this value in the last response is less than MaxCountOfBytesToReturn, then the end of file was
        /// encountered during the read. If this value is exactly zero (0), the original Offset into the file began
        /// at or after the end of file; in this case only one response MUST be generated. The value of the field
        /// MAY (and usually does) exceed the negotiated buffer size.
        /// </summary>
        public ushort Count;

        /// <summary>
        /// This integer MUST be -1 for regular files. For I/O devices or named pipes this indicates the number
        /// of bytes remaining to be  read from the file after the bytes returned in the response were de-queued.
        /// Servers MAY support this function and SHOULD return -1 if they do not support it
        /// </summary>
        public ushort Remaining;

        /// <summary>
        /// Not used and MUST be zero
        /// </summary>
        public ushort DataCompactionMode;

        /// <summary>
        /// This field MUST be set to zero
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// This field is the number of read bytes included in this response.
        /// The value of this field MUST NOT cause the message to exceed the client's maximum buffer size as
        /// specified in MaxBufferSize of the COM_SESSION_SETUP_AND_X client request SMB.
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// The offset in bytes from the beginning of the SMB header to the start of the Buffer field in the
        /// SMB Data
        /// </summary>
        public ushort DataOffset;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_READ_MPX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_MPX_Response_SMB_Data
    {
        /// <summary>
        /// The length in bytes of the remaining SMB_Data. The length MUST be between DataLength and 
        /// DataLength + 3
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// Padding bytes to align data on proper address boundary. The DataOffset field points to the first
        /// byte after this field
        /// </summary>
        public byte[] Pad;

        /// <summary>
        /// The bytes read from the file
        /// </summary>
        public byte[] Data;
    }

    #endregion

    // 2.2.4.24   	SMB_COM_READ_MPX_SECONDARY (0x1C)
    // This command is no longer used in conjunction with the SMB_COM_READ_MPX command. Clients SHOULD NOT
    // send requests using this command code. Servers receiving requests with this command code MUST return
    // STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd).

    #region 2.2.4.25   	SMB_COM_WRITE_RAW (0x1D)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_RAW Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_RAW_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 12 or 14
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid 16-bit unsigned integer indicating the file, named pipe, 
        /// or device to which the data MUST be written.
        /// </summary>
        public ushort FID;

        /// <summary>
        /// The total number of bytes to be written to file during the entire dialog. 
        /// The value MAY exceed the maximum buffer size (MaxBufferSize) established for the session.
        /// </summary>
        public ushort CountOfBytes;

        /// <summary>
        /// This field is reserved and MUST be ignored by the server.
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// The offset in bytes from the start of the file at which the write SHOULD begin. If WordCount is
        /// 14 this is the lower 32-bits of a 64-bit value.
        /// </summary>
        public uint Offset;

        /// <summary>
        /// This field is the timeout in milliseconds to wait for the write to complete. 
        /// Support for this field is optional and this field is used only when writing to a named pipe or I/O device. 
        /// It does not apply, and MUST be zero, when writing to regular file. If Timeout is zero or the server does
        /// not support Timeout, the server MUST NOT block. If the Timeout value is -1 (0xFFFFFFFF, "wait forever"),
        /// then the server MUST block until all bytes have been written to the device before returning a response 
        /// to the client. If the Timeout value is -2 (0xFFFFFFFE, "default"), the server MUST wait for the default 
        /// timeout associated with the named pipe or I/O device. If the server supports Timeout and Timeout
        /// is greater than zero, then the server MUST wait to send the response until all the bytes are written to
        /// the device or the Timeout in milliseconds elapses. If Timeout is greater than zero and the milliseconds
        /// elapse before all of the bytes are written, the server MUST send a response with the Error field set to
        /// indicate that the Timeout occurred along with the count of any bytes already written. 
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// A 16-bit field containing flags defined as follows. The flag names below are provided for reference
        /// only If WritethroughMode is not set, this SMB is assumed to be a form of write behind (cached write). 
        /// The SMB transport layer guarantees delivery of raw data from the client. If an error occurs at the 
        /// server end, all bytes MUST be received and discarded. If an error occurs while writing data to disk 
        /// (such as disk full) the next access to the file handle (another write, close, read, etc.) MUST result
        /// in an error, reporting this situation. If WritethroughMode is set, the server MUST receive the data,
        /// write it to disk and then send a Final Server Response (see below) indicating the result of the write.
        /// The total number of bytes successfully written MUST also be returned in  the SMB_Parameters.Count field
        /// of the response.
        /// </summary>
        public WriteMode WriteMode;

        /// <summary>
        /// This field MUST be 0
        /// </summary>
        public uint Reserved2;

        /// <summary>
        /// This field is the number of bytes included in the SMB_Data block that are to be written to the file.
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// This field is the offset in bytes from the start of the SMB header to the start of the data which
        /// is to be written to the file, the Data[] field. Specifying this offset allows the client to efficiently
        /// align the data buffer
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// ULONG If WordCount is 14, this is the upper 32 bits of the 64-bit offset in bytes from the start of
        /// the file at which the write MUST start. Support of this field is OPTIONAL.
        /// </summary>
        public uint OffsetHigh;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_RAW Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_RAW_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 0.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// Padding bytes for the client to 
        /// align the data on an appropriate boundary for transfer of the SMB transport. The server MUST ignore 
        /// these bytes
        /// </summary>
        public byte[] Pad;

        /// <summary>
        /// The bytes to be written to the file
        /// </summary>
        public byte[] Data;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_RAW_InterimResponse_SMB_Parameters
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_RAW_InterimResponse_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field is valid when writing to named pipes or I/O devices.
        /// This field indicates the number of bytes remaining to be read after the requested write was completed. 
        /// If the client wrote to a disk file, this field MUST be set to -1 (0xFFFF).
        /// </summary>
        public ushort Available;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_RAW_InterimResponse_SMB_Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_RAW_InterimResponse_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_RAW_FinalResponse_SMB_Parameters
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_RAW_FinalResponse_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field contains the total number of bytes written to the file by the server.
        /// </summary>
        public ushort Count;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_RAW_FinalResponse_SMB_Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_RAW_FinalResponse_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message.
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// A 16-bit field containing flags defined as follows. The flag names below are provided for reference only
    /// </summary>
    [Flags()]
    public enum WriteMode : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// If set, the server MUST NOT respond to the client before the data is written to disk (write-through).
        /// </summary>
        WritethroughMode = 0x0001,

        /// <summary>
        /// If set the server SHOULD set the Interim Response Response.SMB_Parameters.Available field correctly 
        /// for writes to named pipes or I/O devices.
        /// </summary>
        ReadBytesAvailable = 0x0002,

        /// <summary>
        /// Applicable to named pipes only. If set the named pipe MUST be written to in raw mode
        /// (no translation; opposite of message mode).
        /// </summary>
        NamedPipeRaw = 0x0004,

        /// <summary>
        /// Applicable to named pipes only. If set, this data is the start of a message.
        /// </summary>
        NamedPipeStart = 0x0008
    }

    #endregion

    #region 2.2.4.26   	SMB_COM_WRITE_MPX (0x1E)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_MPX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_MPX_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 12. The length in two-byte words of the remaining SMB_Parameters.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid 16-bit signed integer indicating the file to which the data should be written
        /// </summary>
        public ushort FID;

        /// <summary>
        /// The requested total number of bytes to write to the file. The value MAY exceed the negotiated buffer size
        /// </summary>
        public ushort TotalByteCount;

        /// <summary>
        /// The server MUST ignore this value
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The offset in bytes from the start of the file at which the write should begin. This value 
        /// indicates the offset at which to write the data contained in the SMB_Data.Bytes.Buffer field of
        /// the same message
        /// </summary>
        public uint ByteOffsetToBeginWrite;

        /// <summary>
        /// This field MUST be ignored by the server. 
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// A 16-bit field containing flags ,If WritethroughMode is not set the server is assumed to be performing
        /// a form of write behind (cached writing). The SMB transport layer guarantees delivery of all secondary
        /// requests from the client. If an error occurs at the server end, all bytes received MUST be and thrown
        /// away. If an error occurs while writing data to disk such as disk full, the next access of the file
        /// handle (another write, close, read, etc.) MUST return the fact that the error occurred. The value of
        /// this but MUST be the same for all requests that are part of the same write operation.
        /// If WritethroughMode is set the server MUST receive the data, write it to disk and then send a final
        /// response indicating the result of the write
        /// </summary>
        public WriteMpxWriteMode WriteMode;

        /// <summary>
        /// This field is a bit mask indicating this SMB request's identity to the server. The server's response 
        /// MUST contain the logical OR of all of the RequestMask values received. This response MUST be generated
        /// </summary>
        public uint RequestMask;

        /// <summary>
        /// This field value is the number of data bytes included in this request.
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// This field value is the offset in bytes from the start of the SMB header to the start of the data buffer. 
        /// </summary>
        public ushort DataOffset;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_MPX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_MPX_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 1.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// Null padding bytes to align Buffer to a 16- or 32-bit boundary.
        /// </summary>
        public byte[] Pad;

        /// <summary>
        /// The raw data in bytes which are to be written to the file.
        /// </summary>
        public byte[] Buffer;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_MPX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_MPX_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 2.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field is the logical OR-ing of the RequestMask value contained in each SMB_COM_WRITE_MPX received
        /// since the last sequenced SMB_COM_WRITE_MPX. The server only responds to the final (sequenced) command.
        /// This response contains the accumulated ResponseMask from all successfully received requests. The client
        /// uses the ResponseMask received to determine which packets, if any, MUST be retransmitted.
        /// </summary>
        public uint ResponseMask;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_MPX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_MPX_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message.
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// A 16-bit field containing flags defined as follows
    /// </summary>
    [Flags()]
    public enum WriteMpxWriteMode : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0x0000,

        /// <summary>
        /// If set, the server MUST NOT respond to the client before the data is written to disk.
        /// </summary>
        WritethroughMode = 0x0001,

        /// <summary>
        /// If set, this flag indicates that messages are being sent over a connectionless transport. This flag
        /// MUST be set.
        /// </summary>
        ConnectionlessMode = 0x0080
    }

    #endregion

    // 2.2.4.27   	SMB_COM_WRITE_MPX_SECONDARY (0x1F)
    // This command was introduced in the LAN Manager 1.0 dialect. It was rendered obsolete in the NT LAN Manager
    // dialect. 
    // This command is no longer used in conjunction with the SMB_COM_WRITE_MPX command. Clients SHOULD NOT send
    // requests using this command code, and servers receiving requests with this command code MUST return 
    // STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd).

    // 2.2.4.28   	SMB_COM_WRITE_COMPLETE (0x20)
    // This command was introduced in LAN Manager 1.0 dialect. This command is deprecated. This command is sent
    // by the server in the final response of an SMB_COM_WRITE_RAW command. See SMB_COM_WRITE_RAW for details.

    // 2.2.4.29   	SMB_COM_QUERY_SERVER (0x21)
    // This command was introduced in the NT LAN Manager dialect, and was reserved but not implemented.
    // Clients SHOULD NOT send requests using this command code, and servers receiving requests with this command
    // code MUST return STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd).

    #region 2.2.4.30   	SMB_COM_SET_INFORMATION2 (0x22)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_SET_INFORMATION2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SET_INFORMATION2_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 6
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This is the FID representing the file for which attributes are to be set
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This is the date when the file was created
        /// </summary>
        public SmbDate CreateDate;

        /// <summary>
        /// This is the time on CreateDate when 	the file was created.
        /// </summary>
        public SmbTime CreationTime;

        /// <summary>
        /// This is the date when the file was last accessed.
        /// </summary>
        public SmbDate LastAccessDate;

        /// <summary>
        /// This is the time on LastAccessDate when the file was last accessed.
        /// </summary>
        public SmbTime LastAccessTime;

        /// <summary>
        /// This is the date when data were last written to the file
        /// </summary>
        public SmbDate LastWriteDate;

        /// <summary>
        /// This is the time on LastWriteDate when data were last written to the file.
        /// </summary>
        public SmbTime LastWriteTime;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_SET_INFORMATION2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SET_INFORMATION2_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_SET_INFORMATION2 Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SET_INFORMATION2_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_SET_INFORMATION2 Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SET_INFORMATION2_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.31   	SMB_COM_QUERY_INFORMATION2 (0x23)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_QUERY_INFORMATION2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_QUERY_INFORMATION2_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid FID that the client has obtained through
        /// a previous SMB command which successfully opened the file. 
        /// </summary>
        public ushort FID;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_QUERY_INFORMATION2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_QUERY_INFORMATION2_Request_SMB_Data
    {
        /// <summary>
        /// field MUST be 0. No data is sent by this message.
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_QUERY_INFORMATION2 Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_QUERY_INFORMATION2_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 11
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field is the date when the file was created
        /// </summary>
        public SmbDate CreateDate;

        /// <summary>
        /// This field is the time on CreateDate when the file was created.
        /// </summary>
        public SmbTime CreationTime;

        /// <summary>
        /// This field is the date when the file was last accessed
        /// </summary>
        public SmbDate LastAccessDate;

        /// <summary>
        /// This field is the time on LastAccessDate when the file was last accessed.
        /// </summary>
        public SmbTime LastAccessTime;

        /// <summary>
        /// This field is the date when data were last written to the file.
        /// </summary>
        public SmbDate LastWriteDate;

        /// <summary>
        /// This field is the time on LastWriteDate when data were last written to the file.
        /// </summary>
        public SmbTime LastWriteTime;

        /// <summary>
        /// This field contains the number of bytes in the file in bytes. Since this size 
        /// is limited to 32, bits this command is inappropriate for files whose size is too large. 
        /// </summary>
        public uint FileDataSize;

        /// <summary>
        /// This field contains the allocation size of the file in bytes. Since this size 
        /// is limited to 32, bits this command is inappropriate for files whose size is too large.
        /// </summary>
        public uint FileAllocationSize;

        /// <summary>
        /// This field is a 16 bit unsigned bit field encoding the attributes of the file.
        /// </summary>
        public SmbFileAttributes FileAttributes;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_QUERY_INFORMATION2 Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_QUERY_INFORMATION2_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.32   	SMB_COM_LOCKING_ANDX (0x24)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_LOCKING_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOCKING_ANDX_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 8
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. 
        /// This value MUST be set to 0xFF if there are no additional SMB commands in the client request packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this request is sent,
        /// and the server MUST ignore this value when the message is received
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the SMB header to the start 
        /// of the WordCount field in the next SMB command in this packet. This field is valid only if the
        /// AndXCommand field is not set to 0xFF. If AndXCommand is 0xFF, this field MUST be ignored by the server.
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// SHORT This field MUST be a valid 16-bit signed integer indicating the file from which the data SHOULD
        /// be read.
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This field is a 8-bit unsigned integer bit mask indicating the nature of the lock request and the 
        /// format of the LOCKING_ANDX_RANGE data. If the negotiated protocol is NT LAN Manager or later and 
        /// CAP_LARGE_FILES was negotiated, then the Locks and Unlocks arrays are in the large file 64-bit offset
        /// LOCKING_ANDX_RANGE format. This allows specification of 64 bit offsets for very large files.
        /// If TypeOfLock has the SHARED_LOCK bit set, the lock is specified as a shared read-only lock.
        /// If shared read-only locks cannot be supported by a server, the server SHOULD map the lock to an
        /// exclusive lock for both read and write. Locks for both read and write messages in which TypeOfLock bit
        /// READ_WRITE_LOCK is set SHOULD be prohibited by the server and the server SHOULD return an appropriate
        /// error to the client. If TypeOfLock has the CHANGE_LOCKTYPE bit set, the client is requesting that the
        /// server atomically change the lock type from a shared lock to an exclusive lock or vice versa. If the
        /// server cannot do this in an atomic fashion, the server MUST reject this request and return an
        /// appropriate error to the client. Closing a file with locks still in force causes the locks to be
        /// released in a nondeterministic order. 
        /// If the Locks vector contains one and only one entry (NumberOfRequestedLocks == 1) and TypeOfLock has
        /// the CANCEL_LOCK bit set, the client is requesting that the server cancel a previously requested,
        /// but not yet responded to, lock. This allows the client to cancel lock requests that can be waiting
        /// forever to complete (see Timeout below).
        /// </summary>
        public LockingAndxTypeOfLock TypeOfLock;

        /// <summary>
        /// This field is valid only in SMB_COM_LOCKING_ANDX SMB requests sent from the server to the client
        /// in response to a change in the existing  Oplock's state. This field is an 8-bit unsigned integer
        /// indicating the OpLock level now in effect for the Locks held for the FID in the request. If
        /// NewOplockLevel is 0, the client possesses no oplocks on the file at all. If NewOplockLevel is 1,
        /// then the client possesses a Level II oplock. The client MUST take appropriate action to ensure
        /// data integrity through flushing any dirty buffers to the server and confirming the receipt of the
        /// server's SMB_COM_LOCKING_ANDX through sending an SMB_LOCKING_ANDX request having the OPLOCK_RELEASE
        /// flag set in the TypeOfLock field or with a file close SMB if the file is no longer in use by the
        /// client. The client is then free to attempt to re-establish any REQUIRED locks for the FID by
        /// sending an SMB_COM_LOCKING_ANDX request to the server. If the client sends an SMB_LOCKING_ANDX
        /// SMB with the OPLOCK_RELEASE flag set in the TypeOfLock field and NumberofRequestedUnlocks and
        /// NumberOfRequestedLocks is zero, then the client is acknowledging an OpLock break without requesting
        /// any new locks and the server MUST NOT send a response. A close being sent to the server and break 
        /// oplock notification from the server could cross on the wire. The client SHOULD ignore a break
        /// oplock notification associated to close it sent for a file that it does not have open.
        /// </summary>
        public NewOplockLevelValue NewOplockLevel;

        /// <summary>
        /// This field is a 32-bit unsigned integer value. Timeout is the maximum amount of time to wait in
        /// milliseconds for the byte range(s) specified in Locks to become locked. A Timeout value of 0
        /// indicates that the server SHOULD fail immediately if any lock range specified is already locked
        /// and cannot be locked by this request. A Timeout value of -1 indicates that the server SHOULD wait
        /// as long as it takes (wait forever) for each byte range specified to become unlocked so that it
        /// can be locked by this request. Any other value of Timeout specifies the maximum number of 
        /// milliseconds to wait for all Locks lock ranges specified to become available and be locked by
        /// this request. Server support of this field is OPTIONAL. 
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// This field is a 16-bit unsigned integer value containing the number of entries in the Unlocks array
        /// </summary>
        public ushort NumberOfRequestedUnlocks;

        /// <summary>
        /// This field is a 16-bit unsigned integer value containing the number of entries in the Locks array.
        /// </summary>
        public ushort NumberOfRequestedLocks;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_LOCKING_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOCKING_ANDX_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 0
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An array of byte ranges to be unlocked. If 32-bit offsets are being used, 
        /// this field uses LOCKING_ANDX_RANGE32 (see below) and is (10 * NumberOfRequestedUnlocks) bytes in length. 
        /// If 64-bit offsets are being used, this field uses LOCKING_ANDX_RANGE64 (see below) and is 
        /// (20 * NumberOfRequestedUnlocks) bytes in length.
        /// </summary>
        public Object[] Unlocks;

        /// <summary>
        /// An array of byte ranges to be locked. If 32-bit offsets are being used, this field uses
        /// LOCKING_ANDX_RANGE32 (see below) and is (10 * NumberOfRequestedUnlocks) bytes in length.
        /// If 64-bit offsets are being used, this field uses LOCKING_ANDX_RANGE64 (see below) and is 
        /// (20 * NumberOfRequestedUnlocks) bytes in length
        /// </summary>
        public Object[] Locks;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_LOCKING_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOCKING_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 2
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet.
        /// This value MUST be set to 0xFF if there are no additional SMB command responses in the server
        /// response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent, and the client MUST ignore
        /// this field
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the SMB header to the start
        /// of the WordCount field in the next SMB command response in this packet. This field is valid
        /// only if the AndXCommand field is not set to 0xFF. 
        /// If AndXCommand is 0xFF, this field MUST be ignored by the client.
        /// </summary>
        public ushort AndXOffset;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_LOCKING_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOCKING_ANDX_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message.
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// The LOCKING_ANDX_RANGE32 data type 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LOCKING_ANDX_RANGE32
    {
        /// <summary>
        /// The PID of the process requesting the locking change
        /// </summary>
        public ushort PID;

        /// <summary>
        /// The 32-bit unsigned integer value that is the offset into the file at which the locking change
        /// MUST begin
        /// </summary>
        public uint ByteOffset;

        /// <summary>
        /// The 32-bit unsigned integer value that is the number of bytes beginning at OffsetInBytes that
        /// MUST be locked or unlocked
        /// </summary>
        public uint LengthInBytes;
    }


    /// <summary>
    /// The LOCKING_ANDX_RANGE64 data type 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LOCKING_ANDX_RANGE64
    {
        /// <summary>
        /// The PID of the process requesting the locking change
        /// </summary>
        public ushort PID;

        /// <summary>
        /// This field pads the structure to DWORD alignment and MUST be zero (0).
        /// </summary>
        public ushort Pad;

        /// <summary>
        /// The 32-bit unsigned integer value that is the high 32-bits of a 64-bit offset into the file at
        /// which the locking change MUST begin
        /// </summary>
        public uint ByteOffsetHigh;

        /// <summary>
        /// The 32-bit unsigned integer value that is the low 32-bits of a 64-bit offset into the file at
        /// which the locking change MUST begin
        /// </summary>
        public uint ByteOffsetLow;

        /// <summary>
        /// The 32-bit unsigned integer value that is the high 32-bits o
        /// f a 64-bit value specifying the number of bytes that MUST be locked or unlocked
        /// </summary>
        public uint LengthInBytesHigh;

        /// <summary>
        /// The 32-bit unsigned integer value that is the low 32-bits of a 64-bit value specifying 
        /// the number of bytes that MUST be locked or unlocked
        /// </summary>
        public uint LengthInBytesLow;
    }


    /// <summary>
    /// This field is a 8-bit unsigned integer bit mask indicating the nature of the lock request and the
    /// format of the LOCKING_ANDX_RANGE data. 
    /// If the negotiated protocol is NT LAN Manager or later and CAP_LARGE_FILES was negotiated, then the
    /// Locks and Unlocks arrays are in the large file 64-bit offset LOCKING_ANDX_RANGE format. This allows
    /// specification of 64 bit offsets for very large files. If TypeOfLock has the SHARED_LOCK bit set, 
    /// the lock is specified as a shared read-only lock. If shared read-only locks cannot be supported by
    /// a server, the server SHOULD map the lock to an exclusive lock for both read and write. Locks for 
    /// both read and write messages in which TypeOfLock bit READ_WRITE_LOCK is set SHOULD be prohibited
    /// by the server and the server SHOULD return an appropriate error to the client. If TypeOfLock has 
    /// the CHANGE_LOCKTYPE bit set, the client is requesting that the server atomically change the lock
    /// type from a shared lock to an exclusive lock or vice versa. If the server cannot do this in an 
    /// atomic fashion, the server MUST reject this request and return an appropriate error to the client.
    /// Closing a file with locks still in force causes the locks to be released in a nondeterministic order. 
    /// If the Locks vector contains one and only one entry (NumberOfRequestedLocks == 1) and TypeOfLock has
    /// the CANCEL_LOCK bit set, the client is requesting that the server cancel a previously requested,
    /// but not yet responded to, lock. This allows the client to cancel lock requests that can be waiting
    /// forever to complete (see Timeout below).
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    public enum LockingAndxTypeOfLock : byte
    {
        /// <summary>
        /// Request for an exclusive read and write lock.
        /// </summary>
        READ_WRITE_LOCK = 0x00,

        /// <summary>
        /// Request for a shared read-only lock.
        /// </summary>
        SHARED_LOCK = 0x01,

        /// <summary>
        /// Notification to the client that an OpLock change has occurred on the FID supplied in the request.
        /// </summary>
        OPLOCK_RELEASE = 0x02,

        /// <summary>
        /// Request to atomically change the lock type from a shared lock to an exclusive lock or 
        /// vice versa for the specified Locks. 
        /// </summary>
        CHANGE_LOCKTYPE = 0x04,

        /// <summary>
        /// Request to cancel ALL outstanding lock requests for the given FID and PID.
        /// </summary>
        CANCEL_LOCK = 0x08,

        /// <summary>
        /// Indicates that the LOCKING_ANDX_RANGE format is the 64-bit file offset version. 
        /// If this flag is not set then the LOCKING_ANDX_RANGE format is the 32-bit file offset version.
        /// </summary>
        LARGE_FILES = 0x10
    }

    #endregion

    #region 2.2.4.33   	SMB_COM_TRANSACTION (0x25)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TRANSACTION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be Words.SetupCount (see below) plus 14.
        /// This value represents the total number of parameter words and MUST be greater than or equal to 14
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The total number of transaction parameter bytes the client expects to send to the server for
        /// this request. Parameter bytes for a transaction are carried within the SMB_Data.Trans_Parameters
        /// field of the SMB_COM_TRANSACTION request. If the size of all of the REQUIRED SMB_Data.
        /// Trans_Parameters for a given transaction causes the request to exceed the MaxBufferSize
        /// established during session setup, then the client MUST NOT send all of the parameters in 
        /// one request. The client MUST break up the parameters and send additional requests using
        /// the SMB_COM_TRANSACTION_SECODARY command to send the additional parameters. Any single 
        /// request MUST not exceed the MaxBufferSize established during session setup. The client
        /// indicates to the server to expect additional parameters, and thus at least one 
        /// SMB_COM_TRANSACTION_SECONDARY, by setting ParameterCount (see below) to be less than
        /// TotalParameterCount.See SMB_COM_TRANSACTION_SECONDARY for more information
        /// </summary>
        public ushort TotalParameterCount;

        /// <summary>
        /// The total number of transaction data bytes the client expects to send to the server for this
        /// request. Data bytes of a transaction are carried within the SMB_Data.Trans_Data field of the 
        /// SMB_COM_TRANSACTION request. If the size of all of the REQUIRED SMB_Data.Trans_Data 
        /// for a given transaction causes the request to exceed the MaxBufferSize established during 
        /// session setup, then the client MUST not send all of the data in one request. The client MUST
        /// break up the data and send additional requests using the SMB_COM_TRANSACTION_SECODARY 
        /// command to send the additional data. Any single request MUST NOT exceed the MaxBufferSize
        /// established during session setup. The client indicates to the server to expect additional
        /// data, and thus at least one SMB_COM_TRANSACTION_SECONDARY, by setting DataCount (see below)
        /// to be less than TotalDataCount. See SMB_COM_TRANSACTION_SECONDARY for more information
        /// </summary>
        public ushort TotalDataCount;

        /// <summary>
        /// The maximum number of SMB_Data.Trans_Parameters bytes that the client accepts in the
        /// transaction response. The server MUST NOT return more than this number of bytes in the
        /// SMB_Data.Trans_Parameters field of the response.
        /// </summary>
        public ushort MaxParameterCount;

        /// <summary>
        /// The maximum number of SMB_Data.Trans_Data bytes that the client accepts  in the transaction
        /// response. The server MUST NOT return more than this number of bytes in the SMB_Data.Trans_Data field
        /// </summary>
        public ushort MaxDataCount;

        /// <summary>
        /// The maximum number of bytes that the client accepts in the Setup field of the transaction response.
        /// The server MUST NOT return more than this number of bytes in the Setup field.
        /// </summary>
        public byte MaxSetupCount;

        /// <summary>
        /// A padding byte. This field MUST be zero. Existing CIFS implementations MAY combine this field with
        /// MaxSetupCount to form a USHORT. If MaxSetupCount is defined as a USHORT, the high order byte MUST
        /// be zero
        /// </summary>
        public byte Reserved1;

        /// <summary>
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be
        /// set to zero by the client sending the request, and MUST be ignored by the server receiving the 
        /// request. The client MAY set either or both of the following bit flags
        /// </summary>
        public TransSmbParametersFlags Flags;

        /// <summary>
        /// The value of this field MUST be the maximum number of milliseconds the server SHOULD wait for
        /// completion of the transaction before generating a timeout and returning a response to the client.
        /// The client SHOULD set this to 0 to indicate that no time-out is expected. A value of zero indicates
        /// that the server SHOULD return an error if the resource is not immediately available. If the
        /// operation does not complete within the specified time, the server MAY abort the request and send
        /// a failure response. 
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// Reserved. This field MUST be zero in the client request. The server MUST ignore the contents 
        /// of this field.
        /// </summary>
        public ushort Reserved2;

        /// <summary>
        /// The number of transaction parameter bytes the client is sending to the server in this request.
        /// Parameter bytes for a transaction are carried within the SMB_Data.Trans_Parameters field of the
        /// SMB_COM_TRANSACTION request. If the transaction request fits within a single SMB_COM_TRANSACTION
        /// request (the request size does not exceed MaxBufferSize), then this value SHOULD be equal to
        /// TotalParameterCount. Otherwise, the sum of the ParameterCount values in the primary and secondary
        /// transaction request messages MUST be equal to the smallest TotalParameterCount value reported to
        /// the server. If the value of this field is less than the value of TotalParameterCount, then at least
        /// one SMB_COM_TRANSACTION_SECONDARY message MUST be used to transfer the remaining transaction
        /// SMB_Data.Trans_Parameters bytes. The ParameterCount field MUST be used to determine the number
        /// of transaction SMB_Data.Trans_Parameters bytes contained within the SMB_COM_TRANSACTION message.
        /// </summary>
        public ushort ParameterCount;

        /// <summary>
        /// This field MUST contain the number of bytes from the start of the SMB Header to the start of the
        /// SMB_Data.Trans_Parameters field. Server implementations MUST use this value to locate the
        /// transaction parameter block within the request.
        /// </summary>
        public ushort ParameterOffset;

        /// <summary>
        /// The number of transaction data bytes the client is sending to the server in this request.
        /// Data bytes for a transaction are carried within the SMB_Data.Trans_Data field of the 
        /// SMB_COM_TRANSACTION request. If the transaction request fits within a single SMB_COM_TRANSACTION 
        /// request (the request size does not exceed MaxBufferSize), then this value SHOULD be equal to
        /// TotalDataCount. Otherwise, the sum of the DataCount values in the primary and secondary 
        /// transaction request messages MUST be equal to the smallest TotalDataCount value reported 
        /// to the server. If the value of this field is less than the value of TotalDataCount, then
        /// at least one SMB_COM_TRANSACTION_SECONDARY message MUST be used to transfer the remaining 
        /// transaction SMB_Data.Trans_Data bytes. The DataCount field MUST be used to determine 
        /// the number of transaction SMB_Data.Trans_Data bytes contained within the SMB_COM_TRANSACTION message.
        /// </summary>
        public ushort DataCount;

        /// <summary>
        /// This field MUST be the number of bytes from the start of the SMB Header of the request to the 
        /// start of the SMB_Data.Trans_Data field. 
        /// Server implementations MUST use this value to locate the transaction data block within the request
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// This field MUST be the number of setup words that are included in the transaction request.
        /// </summary>
        public byte SetupCount;

        /// <summary>
        /// A padding byte. This field MUST be zero. Existing CIFS implementations MAY combine this field 
        /// with SetupCount to form a USHORT.
        /// If SetupCount is defined as a USHORT, the high order byte MUST be zero
        /// </summary>
        public byte Reserved3;

        /// <summary>
        /// An array of two-byte words that provide transaction context to the server. 
        /// The size and content of the array are specific to individual subcommands
        /// </summary>
        [Size("SetupCount")]
        public ushort[] Setup;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TRANSACTION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION_Request_SMB_Data
    {
        /// <summary>
        /// The number of bytes in the Bytes array that follows
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// The pathname of the mailslot or named pipe to which the transaction
        /// subcommand applies or a client supplied identifier that provides a name for the transaction.
        /// </summary>
        public byte[] Name;

        /// <summary>
        /// An array of padding bytes used to align the next field to a 2 byte or 4 byte boundary.
        /// </summary>
        public byte[] Pad1;

        /// <summary>
        /// Transaction parameter bytes. See the individual SMB_COM_TRANSACTION subprotocol 
        /// subcommands descriptions for information on the parameters sent for each subcommand.
        /// </summary>
        public byte[] Trans_Parameters;

        /// <summary>
        /// An array of padding bytes used to align the next field to a 2 byte or 4 byte boundary.
        /// </summary>
        public byte[] Pad2;

        /// <summary>
        /// Transaction data bytes. See the individual SMB_COM_TRANSACTION subprotocol 
        /// subcommands descriptions for information on the data sent for each subcommand.
        /// </summary>
        public byte[] Trans_Data;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TRANSACTION_ErrorResponse_SMB_Parameters
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION_InterimResponse_SMB_Parameters
    {
        /// <summary>
        /// this files MUST be zero
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TRANSACTION_ErrorResponse_SMB_Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION_InterimResponse_SMB_Data
    {
        /// <summary>
        /// this files MUST be zero
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters
    {
        /// <summary>
        /// The value of Words.SetupCount plus 10. This value represents
        /// the total number of SMB parameter words and MUST be greater than or equal to 10
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The total number of transaction parameter bytes the server expects to send to the 
        /// client for this response. Parameter bytes for a transaction are carried within the
        /// SMB_Data.Trans_Parameters field of the SMB_COM_TRANSACTION response. If the size of all
        /// of the REQUIRED SMB_Data.Trans_Parameters for a given transaction causes the response to
        /// exceed the MaxBufferSize established during session setup, then the server MUST NOT send
        /// all of the parameters in one response. The server MUST break up the parameters and send
        /// additional responses using the SMB_COM_TRANSACTION command to send the additional parameters.
        /// Any single response MUST NOT exceed the MaxBufferSize established during session setup.
        /// The server indicates to the client to expect additional parameters in at least one more
        /// SMB_COM_TRANSACTION response by setting ParameterCount (see below) to be less than TotalParameterCount
        /// </summary>
        public ushort TotalParameterCount;

        /// <summary>
        /// The total number of transaction data bytes the server expects to send to the client for this
        /// response. Data bytes of a transaction are carried within the SMB_Data.Trans_Data field of the
        /// SMB_COM_TRANSACTION response. If the size of all of the REQUIRED SMB_Data.Trans_Data 
        /// for a given transaction causes the response to exceed the MaxBufferSize established during
        /// session setup, then the server MUST NOT send all of the data in one response. The server MUST
        /// break up the data and send additional responses using the SMB_COM_TRANSACTION command to send 
        /// the additional data. Any single request MUST NOT exceed the MaxBufferSize established during 
        /// session setup. The server indicates to the client to expect additional data in at least one
        /// more SMB_COM_TRANSACTION response, by setting DataCount (see below) to be less than TotalDataCount.
        /// </summary>
        public ushort TotalDataCount;

        /// <summary>
        /// Reserved. This field MUST be zero in the client request. The server MUST ignore the contents
        /// of this field.
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// The number of transaction parameter bytes being sent in this response. If the transaction fits
        /// within a single SMB_COM_TRANSACTION response, then this value MUST be equal to TotalParameterCount.
        /// Otherwise, the sum of the ParameterCount values in the transaction response messages  MUST
        /// be equal to the smallest TotalParameterCount value reported by the server. The ParameterCount
        /// field MUST be used to determine the number of transaction parameter bytes contained within the response
        /// </summary>
        public ushort ParameterCount;

        /// <summary>
        /// This field MUST contain the number of bytes from the start of the SMB Header to the start of the
        /// SMB_Data.Trans_Parameters field.
        /// Client implementations MUST use this value to locate the transaction parameter block within the response.
        /// </summary>
        public ushort ParameterOffset;

        /// <summary>
        /// The offset in bytes relative to all of the transaction parameter bytes in this transaction response
        /// at which this block of parameter bytes SHOULD be placed. This value MUST be used by the client to
        /// correctly reassemble the transaction response parameters when the response messages are received
        /// out of order
        /// </summary>
        public ushort ParameterDisplacement;

        /// <summary>
        /// The number of transaction data bytes being sent in this response. If the transaction response fits
        /// within a single SMB_COM_TRANSACTION, then this value MUST be equal to TotalDataCount. Otherwise,
        /// the sum of the DataCount values in the primary and secondary transaction responses MUST be equal
        /// to the smallest TotalDataCount value reported to the client. If the value of this field is less
        /// than the value of TotalDataCount, then at least one additional SMB_COM_TRANSACTION response MUST
        /// be used to transfer the remaining data bytes.
        /// </summary>
        public ushort DataCount;

        /// <summary>
        /// This field MUST be the number of bytes from the start of the SMB Header of the response to the start
        /// of the SMB_Data.Trans_Data field.
        /// Client implementations MUST use this value to locate the transaction data block within the response
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// The offset in bytes relative to all of the transaction data bytes in this transaction response
        /// at which this block of data bytes SHOULD be placed. This value MUST be used by the client to
        /// correctly reassemble the transaction data when the response messages are received out of order.
        /// </summary>
        public ushort DataDisplacement;

        /// <summary>
        /// The number of setup words that are included in the transaction response.
        /// </summary>
        public byte SetupCount;

        /// <summary>
        /// A padding byte. This field MUST be zero. Existing CIFS implementations can combine this field
        /// with SetupCount to form a USHORT. 
        /// If SetupCount is defined as a USHORT, the high order byte MUST be zero.
        /// </summary>
        public byte Reserved2;

        /// <summary>
        /// An array of two-byte words that provide transaction results from the server. 
        /// The size and content of the array are specific to individual subprotocol subcommands.
        /// </summary>
        [Size("SetupCount")]
        public ushort[] Setup;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TRANSACTION_SuccessResponse_SMB_Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION_SuccessResponse_SMB_Data
    {
        /// <summary>
        /// The number of bytes in the SMB_Data.Bytes array that follows
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An array of padding bytes used to align the next field to a 16 or 32 bit boundary.
        /// </summary>
        public byte[] Pad1;

        /// <summary>
        /// Transaction parameter bytes. See the individual SMB_COM_TRANSACTION subcommand 
        /// descriptions for information on parameters returned by the server for each subcommand.
        /// </summary>
        public byte[] Trans_Parameters;

        /// <summary>
        /// An array of padding bytes used to align the next field to a 16 or 32 bit boundary.
        /// </summary>
        public byte[] Pad2;

        /// <summary>
        /// Transaction data bytes. See the individual SMB_COM_TRANSACTION subcommand 
        /// descriptions for information on data returned by the server for each subcommand.
        /// </summary>
        public byte[] Trans_Data;
    }


    /// <summary>
    /// A set of bit flags that alter the behavior of the requested operation. 
    /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by
    /// the server receiving the request. The client MAY set either or both of the following bit flags
    /// </summary>
    [Flags()]
    public enum TransSmbParametersFlags : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// If set, following the completion of the operation the server MUST disconnect the tree connection
        /// associated with the tree identifier (TID) field received in the SMB header of this request.
        /// The client SHOULD NOT send a subsequent SMB_COM_TREE_DISCONNECT for this tree connect. 
        /// </summary>
        DISCONNECT_TID = 0x0001,

        /// <summary>
        /// This is a one-way transaction. The server MUST attempt to complete the transaction, but MUST
        /// NOT send a response to the client.
        /// </summary>
        NO_RESPONSE = 0x0002
    }

    #endregion

    #region 2.2.4.34   	SMB_COM_TRANSACTION_SECONDARY (0x26)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TRANSACTION_SECONDARY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Parameters
    {
        /// <summary>
        /// This value represents the total number of SMB parameter words and MUST be 8.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The total number of transaction parameter bytes to be sent to the server over the course of
        /// this transaction.
        /// This value MAY be less than or equal to the TotalParameterCount in preceding request messages
        /// that are part of the same transaction. This value represents transaction parameter bytes, 
        /// not SMB parameter words
        /// </summary>
        public ushort TotalParameterCount;

        /// <summary>
        /// The total number of transaction data bytes to be sent to the server over the course of this transaction. 
        /// This value MAY be less than or equal to the TotalDataCount in preceding request messages that are part 
        /// of the same transaction. This value represents transaction data bytes, not SMB data bytes.
        /// </summary>
        public ushort TotalDataCount;

        /// <summary>
        /// The number of transaction parameter bytes being sent in the SMB message. This value MUST be less
        /// than TotalParameterCount. The sum of the ParameterCount values across all of the request messages 
        /// in a transaction MUST be equal to the TotalParameterCount
        /// reported in the last request message of the transaction.
        /// </summary>
        public ushort ParameterCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the transaction parameter bytes contained
        /// in this SMB message. 
        /// This MUST be the number of bytes from the start of the SMB message to the start of the SMB_Data.
        /// Bytes.Trans_Parameters field. Server implementations MUST use this value to locate the transaction
        /// parameter block within the SMB message.
        /// </summary>
        public ushort ParameterOffset;

        /// <summary>
        /// The offset, relative to all of the transaction parameter bytes sent to the server in this transaction,
        /// at which this block of parameter bytes MUST be placed. This value can be used by the server to
        /// correctly reassemble the transaction parameters even if the SMB request messages are received out of order
        /// </summary>
        public ushort ParameterDisplacement;

        /// <summary>
        /// The number of transaction data bytes being sent in this SMB message. This value MUST be less than 
        /// the value of TotalDataCount. 
        /// The sum of the DataCount values across all of the request messages in a transaction MUST be equal
        /// to the smallest TotalDataCount value reported to the server
        /// </summary>
        public ushort DataCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the transaction data bytes contained in
        /// this SMB message. 
        /// This MUST be the number of bytes from the start of the SMB message to the start of the SMB_Data.
        /// Bytes.Trans_Data field. 
        /// Server implementations MUST use this value to locate the transaction data block within the SMB message.
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// The offset, relative to all of the transaction data bytes sent to the server in this transaction, at 
        /// which this block of parameter bytes MUST be placed. This value can be used by the server to correctly
        /// reassemble the transaction data block even if the SMB request messages are received out of order.
        /// </summary>
        public ushort DataDisplacement;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TRANSACTION_SECONDARY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Data
    {
        /// <summary>
        /// The number of bytes in the SMB_Data.Bytes array, which follows.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 16 or 32 bit boundary.
        /// </summary>
        public byte[] Pad1;

        /// <summary>
        /// Transaction parameter bytes. 
        /// </summary>
        public byte[] Trans_Parameters;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 16 or 32 bit boundary.
        /// </summary>
        public byte[] Pad2;

        /// <summary>
        /// Transaction data bytes
        /// </summary>
        public byte[] Trans_Data;
    }


    // struct SMB_COM_TRANSACTION_SECONDARY_Response
    // There is no response message defined for the SMB_COM_TRANSACTION_SECONDARY request. 

    #endregion

    #region 2.2.4.35   	SMB_COM_IOCTL (0x27)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_IOCTL Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_IOCTL_Request_SMB_Parameters
    {
        /// <summary>
        /// This value of this field MUST be set to 14
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The Fid of the device or file to which the IOCTL is to be sent
        /// </summary>
        public ushort FID;

        /// <summary>
        /// The implementation dependent device category for the request.
        /// </summary>
        public IoctlCategory Category;

        /// <summary>
        /// The implementation dependent device function for the request
        /// </summary>
        public IoctlFunction Function;

        /// <summary>
        /// The total number of IOCTL parameter bytes the client is sending to the server in this request.
        /// Parameter bytes for an IOCTL are carried within the SMB_Data.Parameters field of the SMB_COM_IOCTL
        /// request. This value MUST be the same as ParameterCount. 
        /// </summary>
        public ushort TotalParameterCount;

        /// <summary>
        /// The total number of IOCTL data bytes the client is sending to the server in this request. Data bytes
        /// for an IOCTL are carried within the SMB_Data.Data field of the SMB_COM_IOCTL request. This value MUST
        /// be the same as DataCount. 
        /// </summary>
        public ushort TotalDataCount;

        /// <summary>
        /// The maximum number of SMB_Data.Parameters bytes that the client accepts in the IOCTL response.
        /// The server MUST NOT return more than this number of bytes in the SMB_Data.Parameter field of the response.
        /// </summary>
        public ushort MaxParameterCount;

        /// <summary>
        /// The maximum number of SMB_Data.Data bytes that the client accepts in the IOCTL response. The server
        /// MUST NOT return more than this number of bytes in the SMB_Data.Data field.
        /// </summary>
        public ushort MaxDataCount;

        /// <summary>
        /// The value of this field MUST be the maximum number of milliseconds the server SHOULD wait for
        /// completion of the transaction before generating a timeout and returning a response to the client.
        /// The client SHOULD set this to 0 to indicate that no time-out is expected. A value of zero indicates
        /// that the server SHOULD return an error if the resource is not immediately available.
        /// If the operation does not complete within the specified time, the server MAY abort the request
        /// and send a failure response
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// Reserved. This field MUST be zero in the client request. The server MUST ignore the contents 
        /// of this field. 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The number of IOCTL parameter bytes the client is sending to the server in this request.
        /// Parameter bytes for an IOCTL are carried within the SMB_Data.Parameters field of the SMB_COM_IOCTL
        /// request. This value MUST be the same as TotalParameterCount.
        /// </summary>
        public ushort ParameterCount;

        /// <summary>
        /// The client SHOULD set the value of this field to 0. The server MUST ignore the value of this field.
        /// </summary>
        public ushort ParameterOffset;

        /// <summary>
        /// The total number of IOCTL data bytes the client is sending to the server in this request. Data bytes
        /// for an IOCTL are carried within the SMB_Data.Data field of the SMB_COM_IOCTL request. This value MUST
        /// be the same as TotalDataCount
        /// </summary>
        public ushort DataCount;

        /// <summary>
        /// The client SHOULD set the value of this field to 0. The server MUST ignore the value of this field.
        /// </summary>
        public ushort DataOffset;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_IOCTL Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_IOCTL_Request_SMB_Data
    {
        /// <summary>
        /// The number of bytes in the Bytes array that follows
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An array of padding bytes used to align the next field to a 2 byte or 4 byte boundary.
        /// </summary>
        public byte[] Pad1;

        /// <summary>
        /// IOCTL parameter bytes. The contents are implementation dependent.
        /// </summary>
        public byte[] Parameters;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 2 byte or 4 byte boundary.
        /// </summary>
        public byte[] Pad2;

        /// <summary>
        /// Transaction data bytes. The contents are implementation dependent.
        /// </summary>
        public byte[] Data;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_IOCTL Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_IOCTL_Response_SMB_Parameters
    {
        /// <summary>
        /// The value of this field MUST be set to 8
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The total number of IOCTL parameter bytes the server is sending to the client in this response.
        /// Parameter bytes for an IOCTL are carried within the SMB_Data.Parameters field of the SMB_COM_IOCTL
        /// request. This value MUST be the same as ParameterCount and this value MUST be less than or equal
        /// to the MaxParameterCount field value in the client's request
        /// </summary>
        public ushort TotalParameterCount;

        /// <summary>
        /// The total number of IOCTL data bytes the server is sending to the client in this response. 
        /// Data bytes for an IOCTL are carried within the SMB_Data.Data field of the SMB_COM_IOCTL request. 
        /// This value MUST be the same as DataCount and this value MUST be less than or equal to the
        /// MaxDataCount field value in the client's request.
        /// </summary>
        public ushort TotalDataCount;

        /// <summary>
        /// The total number of IOCTL parameter bytes the server is sending to the client in this response. 
        /// Parameter bytes for an IOCTL are carried within the SMB_Data.Parameters field of the SMB_COM_IOCTL
        /// request. This value MUST be the same as TotalParameterCount and this value MUST be less than or
        /// equal to the MaxParameterCount field value in the client's request.
        /// </summary>
        public ushort ParameterCount;

        /// <summary>
        /// This field MUST contain the number of bytes from the start of the SMB Header to the start of the
        /// SMB_Data.Parameters field. 
        /// Client implementations MUST use this value to locate the IOCTL parameter block within the response
        /// </summary>
        public ushort ParameterOffset;

        /// <summary>
        /// The server SHOULD set the value of this field to 0. The client MUST ignore the value of this field.
        /// </summary>
        public ushort ParameterDisplacement;

        /// <summary>
        /// The total number of IOCTL data bytes the server is sending to the client in this response. 
        /// Data bytes for an IOCTL are carried within the SMB_Data.Data field of the SMB_COM_IOCTL request.
        /// This value MUST be the same as TotalDataCount and this value MUST be less than or equal to the
        /// MaxDataCount field value of the client's request
        /// </summary>
        public ushort DataCount;

        /// <summary>
        /// This field MUST be the number of bytes from the start of the SMB Header of the response to the
        /// start of the SMB_Data.Data field.
        /// Client implementations MUST use this value to locate the IOCTL data block within the response
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// The server SHOULD set the value of this field to 0. The client MUST ignore the value of this field
        /// </summary>
        public ushort DataDisplacement;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_IOCTL Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_IOCTL_Response_SMB_Data
    {
        /// <summary>
        /// The number of bytes in the SMB_Data.Bytes array, which follows.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 16 or 32 bit boundary.
        /// </summary>
        public byte[] Pad1;

        /// <summary>
        /// IOCTL parameter bytes. The contents are implementation dependent.
        /// </summary>
        public byte[] Parameters;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 16 or 32 bit boundary.
        /// </summary>
        public byte[] Pad2;

        /// <summary>
        /// IOCTL data bytes. The contents are implementation dependent.
        /// </summary>
        public byte[] Data;
    }


    /// <summary>
    /// the device category.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum IoctlCategory : ushort
    {
        /// <summary>
        /// the IOCTL category of SERIAL_DEVICE
        /// </summary>
        SERIAL_DEVICE = 0x0001,

        /// <summary>
        /// the IOCTL category of PRINTER_DEVICE
        /// </summary>
        PRINTER_DEVICE = 0x0005,

        /// <summary>
        /// the IOCTL category of GENERAL_DEVICE
        /// </summary>
        GENERAL_DEVICE = 0x000B,

        /// <summary>
        /// the IOCTL category of SPOOLER_DEVICE
        /// </summary>
        SPOOLER_DEVICE = 0x0053,
    }


    /// <summary>
    /// the device function.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum IoctlFunction : ushort
    {
        /// <summary>
        /// Set the baud rate on the serial device
        /// </summary>
        SET_BAUD_RATE = 0x41,

        /// <summary>
        /// Set serial device line control information.
        /// </summary>
        SET_LINE_CONTROL = 0x42,

        /// <summary>
        /// Not implemented.
        /// </summary>
        SET_TRANSMIT_TIMEOUT = 0x44,

        /// <summary>
        /// Not implemented.
        /// </summary>
        SET_BREAK_OFF = 0x45,

        /// <summary>
        /// Not implemented.
        /// </summary>
        SET_MODEM_CONTROL = 0x46,

        /// <summary>
        /// Not implemented.
        /// </summary>
        SET_BREAK_ON = 0x4B,

        /// <summary>
        /// Not implemented.
        /// </summary>
        STOP_TRANSMIT = 0x47,

        /// <summary>
        /// Not implemented.
        /// </summary>
        START_TRANSMIT = 0x48,

        /// <summary>
        /// Set serial device device control information.
        /// </summary>
        SET_DCB_INFORMATION = 0x53,

        /// <summary>
        /// Get the baud rate on the serial device.
        /// </summary>
        GET_BAUD_RATE = 0x61,

        /// <summary>
        /// Get serial device line control information.
        /// </summary>
        GET_LINE_CONTROL = 0x62,

        /// <summary>
        /// Not implemented.
        /// </summary>
        GET_COMM_STATUS = 0x64,

        /// <summary>
        /// Not implemented.
        /// </summary>
        GET_LINE_STATUS = 0x65,

        /// <summary>
        /// Not implemented.
        /// </summary>
        GET_MODEM_OUTPUT = 0x66,

        /// <summary>
        /// Not implemented.
        /// </summary>
        GET_MODEM_INPUT = 0x67,

        /// <summary>
        /// Not implemented.
        /// </summary>
        GET_INQUEUE_COUNT = 0x68,

        /// <summary>
        /// Not implemented.
        /// </summary>
        GET_OUTQUEUE_COUNTE = 0x69,

        /// <summary>
        /// Get serial device device error information.
        /// </summary>
        GET_COMM_ERROR = 0x6D,

        /// <summary>
        /// Not implemented.
        /// </summary>
        GET_COMM_EVENT = 0x72,

        /// <summary>
        /// Get serial device device control information.
        /// </summary>
        GET_DCB_INFORMATION = 0x73,

        /// <summary>
        /// Print job ID and printer share name.
        /// </summary>
        GET_PRINTER_ID = 0x60,

        /// <summary>
        /// Always returns OS2_STATUS_PRINTER_HAPPY (0x90).
        /// </summary>
        GET_PRINTER_STATUS = 0x66,
    }


    /// <summary>
    /// the Data of GET_BAUD_RATE and SET_BAUD_RATE
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IoctlBAudRateData
    {
        /// <summary>
        /// the baud rate
        /// </summary>
        public ushort BaudRate;
    }


    /// <summary>
    /// the Parameters GET_LINE_CONTROL and SET_LINE_CONTROL
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IoctlLineControlParameters
    {
        /// <summary>
        /// the data bits
        /// </summary>
        public byte DataBits;

        /// <summary>
        /// the parity
        /// </summary>
        public byte Parity;

        /// <summary>
        /// the stop bits
        /// </summary>
        public byte StopBits;

        /// <summary>
        /// the trans break
        /// </summary>
        public byte TransBreak;
    }


    /// <summary>
    /// the Data of GET_DCB_INFORMATION and SET_DCB_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IoctlDcbInformation
    {
        /// <summary>
        /// the write timeout
        /// </summary>
        public ushort WriteTimeout;

        /// <summary>
        /// the read timeout
        /// </summary>
        public ushort ReadTimeout;

        /// <summary>
        /// the control hand shake
        /// </summary>
        public byte ControlHandShake;

        /// <summary>
        /// the flow replace
        /// </summary>
        public byte FlowReplace;

        /// <summary>
        /// the tiomeout
        /// </summary>
        public byte Timeout;

        /// <summary>
        /// the error replacement char
        /// </summary>
        public byte ErrorReplacementChar;

        /// <summary>
        /// the break replacement char
        /// </summary>
        public byte BreakReplacementChar;

        /// <summary>
        /// the xon char
        /// </summary>
        public byte XonChar;

        /// <summary>
        /// the xoff char
        /// </summary>
        public byte XoffChar;
    }


    /// <summary>
    /// the Data of GET_COMM_ERROR
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IoctlCommError
    {
        /// <summary>
        /// the error value
        /// </summary>
        public ushort Error;
    }


    /// <summary>
    /// the Data of GET_PRINTER_STATUS
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IoctlPrinterStatus
    {
        /// <summary>
        /// the status value
        /// </summary>
        public byte Status;
    }


    /// <summary>
    /// the Data of GET_PRINTER_ID
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IoctlPrinterId
    {
        /// <summary>
        /// the print job id
        /// </summary>
        public ushort JobId;

        /// <summary>
        /// the print job name
        /// </summary>
        public byte[] Buffer;
    }

    #endregion

    // 2.2.4.36   	SMB_COM_IOCTL_SECONDARY (0x28)
    // This command is a companion to SMB_COM_IOCTL, which has been deprecated. Please see SMB_COM_IOCTL
    // for more information. Clients SHOULD NOT send requests using this command code, and servers
    // receiving requests with this command code MUST return STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd).

    // 2.2.4.37   	SMB_COM_COPY (0x29)
    // This command was used to perform server-side file copies, but is no longer used. Clients SHOULD
    // NOT send requests using this command code. Servers receiving requests with this command code MUST
    // return STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd).

    // 2.2.4.38   	SMB_COM_MOVE (0x2A)
    // This command was used to move files on the server, but is no longer in use. Clients SHOULD NOT 
    // send requests using this command code. Servers receiving requests with this command code MUST
    // return STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd).

    #region 2.2.4.39   	SMB_COM_ECHO (0x2B)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_ECHO Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_ECHO_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The number of times the server SHOULD echo the contents of the SMB_Data.Data field.
        /// </summary>
        public ushort EchoCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_ECHO Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_ECHO_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to zero, indicating the number of bytes of data.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// Data to echo. The value does not matter
        /// </summary>
        [Size("ByteCount")]
        public byte[] Data;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_ECHO Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_ECHO_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The sequence number of this echo response message
        /// </summary>
        public ushort SequenceNumber;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_ECHO Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_ECHO_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be the same as it was in the request
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be the same as it was in the request
        /// </summary>
        [Size("ByteCount")]
        public byte[] Data;
    }

    #endregion

    #region 2.2.4.40   	SMB_COM_WRITE_AND_CLOSE (0x2C)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_AND_CLOSE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_AND_CLOSE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be either 6 or 12
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid 16-bit signed integer indicating the file to which the data SHOULD be written
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This field is a 16-bit unsigned integer indicating the number of bytes to be written to the file.
        /// The client MUST ensure that the amount of data sent can fit in the negotiated maximum buffer size.
        /// If the value of this field is zero (0), the server MUST truncate or extend the file to match the
        /// WriteOffsetInBytes
        /// </summary>
        public ushort CountOfBytesToWrite;

        /// <summary>
        /// This field is a 32-bit unsigned integer indicating the offset in number of bytes from the beginning
        /// of the file at which to begin writing to the file. 
        /// The client MUST ensure that the amount of data sent can fit in the negotiated maximum buffer size. 
        /// Because this field is limited to 32-bits, this command is inappropriate for files having 64-bit offsets.
        /// </summary>
        public uint WriteOffsetInBytes;

        /// <summary>
        /// This field is a 32-bit unsigned integer indicating  the number of seconds since Jan 1, 1970, 00:00:00.0.
        /// The server SHOULD set the last write time of the file represented by the FID to this value. If the
        /// value is zero (0), the server SHOULD use the current local time of the server to set the value.
        /// Failure to set the time MUST not result in an error response from the server.
        /// </summary>
        public UTime LastWriteTime;

        /// <summary>
        /// This field is OPTIONAL. This field is reserved and MUST be zero (0). This field is ONLY used in the 
        /// 12 word version of the request
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public uint[] Reserved;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_AND_CLOSE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_AND_CLOSE_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 1 + CountOfBytesToWrite
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// The value of this field SHOULD be ignored. This is padding to force the byte alignment to a
        /// double word boundary
        /// </summary>
        public byte Pad;

        /// <summary>
        /// The raw bytes to be written to the file
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] Data;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_AND_CLOSE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_AND_CLOSE_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// Indicates the actual number of bytes written to the file. For successful writes, this MUST 
        /// equal the CountOfBytesToWrite in the client's request. If the number of bytes written differs
        /// from the number requested and no error is indicated, then the server has no resources available
        /// with which to satisfy the complete write.
        /// </summary>
        public ushort CountOfBytesWritten;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_AND_CLOSE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_AND_CLOSE_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.41   	SMB_COM_OPEN_ANDX (0x2D)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_OPEN_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_ANDX_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 15
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. 
        /// This value MUST be set to 0xFF if there are no additional SMB commands in the client request packet. 
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This field MUST be 0 when the message is sent and the server MUST 
        /// ignore this value when the message is received
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the SMB header to the start of the
        /// WordCount field in the next SMB command in this packet. This field is valid only if the AndXCommand 
        /// field is not set to 0xFF. If AndXCommand is 0xFF, this field MUST be ignored by the server
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// A 16-bit field of flags for requesting attribute data and locking
        /// </summary>
        public Flags Flags;

        /// <summary>
        /// A 16-bit field for encoding the requested access mode. See section 3.2.4.5.1 for a discussion on
        /// sharing modes.
        /// </summary>
        public AccessMode AccessMode;

        /// <summary>
        /// The set of attributes that the file MUST have in order to be found while searching to see if it exists.
        /// If none of the attribute bytes are set, the file attributes MUST refer to a regular file. 
        /// <WB> The Windows NT server ALWAYS ignores SearchAttrs in open requests. </WB>
        /// </summary>
        public SmbFileAttributes SearchAttrs;

        /// <summary>
        /// The set of attributes that the file is to have if the file needs to be created. If none of the
        /// attribute bytes are set, the file attributes MUST refer to a regular file.
        /// </summary>
        public SmbFileAttributes FileAttrs;

        /// <summary>
        /// 32-bit integer time value to be assigned to the file as a time of creation if the file is to be created. 
        /// </summary>
        public UTime CreationTime;

        /// <summary>
        /// A 16-bit field that controls the way a file SHOULD be treated when it is opened for use by certain
        /// extended SMB requests. 
        /// </summary>
        public OpenMode OpenMode;

        /// <summary>
        /// The number of bytes to reserve on file creation or truncation. This field MAY be ignored by the server
        /// </summary>
        public uint AllocationSize;

        /// <summary>
        /// This field is a 32-bit unsigned integer value containing the number of milliseconds to wait on a blocked 
        /// open request before returning without successfully opening the file.
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// This field is reserved and MUST be zero
        /// </summary>
        [StaticSize(2, StaticSizeMode.Elements)]
        public ushort[] Reserved;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_OPEN_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_ANDX_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// A buffer containing the name of the file to be opened
        /// </summary>
        [Size("ByteCount")]
        public byte[] FileName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_OPEN_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 15
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. 
        /// This value MUST be set to 0xFF if there are no additional SMB command responses in the server response
        /// packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent, and the client MUST ignore this field
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the SMB header to the start of the
        /// WordCount field in the next SMB command response in this packet. This field is valid only if the
        /// AndXCommand field is not set to 0xFF. If AndXCommand is 0xFF, this field MUST be ignored by the client
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// A valid FID representing the open instance of the file.
        /// </summary>
        public ushort FID;

        /// <summary>
        /// The actual file system attributes of the file. If none of the attribute bytes are set,
        /// the file attributes MUST refer to a regular file
        /// </summary>
        public SmbFileAttributes FileAttrs;

        /// <summary>
        /// 32-bit integer time value of last modification to the file
        /// </summary>
        public UTime LastWriteTime;

        /// <summary>
        /// The number of bytes in the file. This field is advisory and MAY be used.
        /// </summary>
        public uint FileDataSize;

        /// <summary>
        /// A 16-bit field that shows granted access rights to the file
        /// </summary>
        public AccessRightsValue AccessRights;

        /// <summary>
        /// A 16-bit field that shows the resource type opened.
        /// </summary>
        public ResourceTypeValue ResourceType;

        /// <summary>
        /// A 16-bit field that shows the status of the named pipe if the resource type opened is a named pipe
        /// </summary>
        public SMB_NMPIPE_STATUS NMPipeStatus;

        /// <summary>
        /// A 16-bit field that shows the results of the open operation
        /// </summary>
        public OpenResultsValues OpenResults;

        /// <summary>
        /// This field MUST be zero
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public ushort[] Reserved;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_OPEN_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_ANDX_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// A 16-bit field of flags for requesting attribute data and locking
    /// </summary>
    [Flags()]
    public enum Flags : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0x0000,

        /// <summary>
        /// If this bit is set the client requests that the file attribute data in the response be populated. 
        /// All fields after the FID in the response will be populate.  If this bit is not set all fields after 
        /// the FID in the response will be zero.
        /// </summary>
        REQ_ATTRIB = 0x0001,

        /// <summary>
        /// Client requests an exclusive OpLock on the file
        /// </summary>
        REQ_OPLOCK = 0x0002,

        /// <summary>
        /// Client requests a Batch OpLock on the file.
        /// </summary>
        REQ_OPLOCK_BATCH = 0x0004
    }


    /// <summary>
    /// A 16-bit field for encoding the requested access mode. See section 3.2.4.5.1 for a discussion on
    /// sharing modes.
    /// </summary>
    public enum AccessMode : ushort
    {
        /// <summary>
        /// Open for reading
        /// </summary>
        Reading = 0x0000,

        /// <summary>
        /// Open for writing
        /// </summary>
        Writing = 0x0001,

        /// <summary>
        /// Open for reading and writing
        /// </summary>
        Reading_and_writing = 0x0002,

        /// <summary>
        /// Open for execution
        /// </summary>
        Execution = 0x0003
    }


    /// <summary>
    /// A 16-bit field that controls the way a file SHOULD be treated when it is opened for use by certain
    /// extended SMB requests
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    public enum OpenMode : ushort
    {
        /// <summary>
        /// The request SHOULD fail and an error returned indicating the prior existence of the file.
        /// </summary>
        FileExistsOpts_Fail_Error = 0x0000,

        /// <summary>
        /// The file is to be appended.
        /// </summary>
        FileExistsOpts_Appened = 0x0001,

        /// <summary>
        /// The file is to be truncated to zero (0) length.
        /// </summary>
        FileExistsOpts_Truncat_Zero = 0x0002,

        /// <summary>
        /// Reserved of FileExistsOpts.
        /// </summary>
        FileExistsOpts_Reserved = 0x0003,

        /// <summary>
        /// If the file does not exist, return error.
        /// </summary>
        CreateFile_Open = 0x0000,

        /// <summary>
        /// If the file does not exist, create it.
        /// </summary>
        CreateFile_Create = 0x0010
    }


    /// <summary>
    /// A 16-bit field that shows granted access rights to the file.
    /// </summary>
    public enum AccessRightsValue : ushort
    {
        /// <summary>
        /// Read-only Access
        /// </summary>
        SMB_DA_ACCESS_READ = 0x0000,

        /// <summary>
        /// Write-only Access
        /// </summary>
        SMB_DA_ACCESS_WRITE = 0x0001,

        /// <summary>
        ///Read/Write Access
        /// </summary>
        SMB_DA_ACCESS_READ_WRITE = 0x0002,
    }


    /// <summary>
    /// A 16-bit field that shows the resource type opened.
    /// </summary>
    public enum ResourceTypeValue : ushort
    {
        /// <summary>
        /// Disk file or directory.
        /// </summary>
        FileTypeDisk = 0x0000,

        /// <summary>
        /// Byte mode named pipe.
        /// </summary>
        FileTypeByteModePipe = 0x0001,

        /// <summary>
        /// Message-mode named pipe.
        /// </summary>
        FileTypeMessageModePipe = 0x0002,

        /// <summary>
        /// Printer device.
        /// </summary>
        FileTypePrinter = 0x0003,

        /// <summary>
        /// Character-mode device. When an extended protocol has been negotiated, this value allows a device to be
        /// opened for driver-level I/O. This provides direct access to real-time and interactive devices such as
        /// modems, scanners, and so on.
        /// </summary>
        FileTypeCommDevice = 0x0004,

        /// <summary>
        /// Unknown file type.
        /// </summary>
        FileTypeUnknown = 0xFFFF,
    }


    /// <summary>
    /// A 16-bit field that shows the status of the named pipe if the resource type opened is a named pipe.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum SMB_NMPIPE_STATUS : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        None = 0,

        /// <summary>
        /// An 8-bit unsigned integer that gives the maximum number of instances the named pipe can have.
        /// </summary>
        ICount = 0x00FF,

        /// <summary> 
        /// If set, indicates that the client opened or set the named pipe to message mode.
        /// Otherwise, indicates that the named pipe was opened in or set to byte mode by the client.
        /// </summary>
        ReadMode = 0x0300,

        /// <summary>
        /// If set, indicates the named pipe was created by the server as a message mode pipe.
        /// Otherwise, indicates that the named pipe was created as a byte mode pipe.
        /// </summary>
        NamedPipeType = 0x0C00,

        /// <summary>
        /// Reserved
        /// </summary>
        Reserved = 0x3000,

        /// <summary>
        /// If set, indicates server-side end of a named pipe.
        /// Otherwise, indicates client-side end of the named pipe. 
        /// </summary>
        Endpoint = 0x4000,

        /// <summary>
        /// If set, indicates that reads and writes return immediately if no data is available.
        /// Otherwise, indicates that reads and writes block if no data is available.
        /// </summary>
        Blocking = 0x8000,
    }


    /// <summary>
    /// A 16-bit field that shows the results of the open operation.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum OpenResultsValues : ushort
    {
        /// <summary>
        /// The file existed and was opened. 
        /// </summary>
        OpenResult1 = 0x0001,

        /// <summary>
        /// The file did not exist and was therefore created.
        /// </summary>
        OpenResult2 = 0x0002,

        /// <summary>
        /// The file existed and was truncated.
        /// </summary>
        OpenResult3 = 0x0003,

        /// <summary>
        /// If not set, No OpLock was requested, the OpLock could not be granted, or the server does not support OpLocks.
        /// Otherwise, An OpLock was requested by the client and was granted by the server.
        /// </summary>
        LockStatus = 0x8000,
    }

    #endregion

    #region 2.2.4.42   	SMB_COM_READ_ANDX (0x2E)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_READ_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_ANDX_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be either 10 or 12
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet.
        /// This value MUST be set to 0xFF if there are no additional SMB commands in the client request packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this request is sent, and the server MUST
        /// ignore this value when the message is received
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the SMB header to the start of
        /// the WordCount field in the next SMB command in this packet. This field is valid only if the
        /// AndXCommand field is not set to 0xFF. 
        /// If AndXCommand is 0xFF, this field MUST be ignored by the server
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// This field MUST be a valid FID indicating the file from which the data MUST be read.
        /// </summary>
        public ushort FID;

        /// <summary>
        /// If WordCount is 10 this field represents a 32-bit offset, measured in bytes, 
        /// of where the read MUST start relative to the beginning of the file. If WordCount is 12 this 
        /// field represents the lower 32 bits of a 64-bit offset.
        /// </summary>
        public uint Offset;

        /// <summary>
        /// The maximum number of bytes to read. A single request MAY NOT return more data than the maximum
        /// negotiated buffer size for the session. If MaxCountOfBytesToReturn exceeds the maximum negotiated
        /// buffer size the server MUST return the number of bytes that fit within the maximum negotiated
        /// buffer size
        /// </summary>
        public ushort MaxNumberOfBytesToReturn;

        /// <summary>
        /// The requested minimum number of bytes to return. This field is used only when reading from a named
        /// pipe or a device. It is ignored when reading from a standard file.
        /// </summary>
        public ushort MinNumberOfBytesToReturn;

        /// <summary>
        /// This field represents the amount of time, in milliseconds, that a server MUST wait before sending
        /// a response. It is used only when reading from a named pipe or I/O device and does not apply when 
        /// reading from a regular file. Support for this field is optional.Two values have special meaning in 
        /// this field: 
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// Count of bytes remaining to satisfy client's read request. This field is not used in the NT LAN 
        /// Manager dialect. Clients MUST set this field to zero and servers MUST ignore it.
        /// </summary>
        public ushort Remaining;

        /// <summary>
        /// This field is OPTIONAL. If WordCount is 10 this field is not included in the request.
        /// If WordCount is 12 this field represents the upper 32 bits of a 64-bit offset, measured
        /// in bytes, of where the read SHOULD start relative to the beginning of the file.
        /// </summary>
        [MarshalingCondition("IsOffsetHighPresent")]
        public uint OffsetHigh;


        /// <summary>
        /// A method to determine whether OffsetHigh should be marshalled.
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsOffsetHighPresent(MarshalingType marshalingType, object value)
        {
            return ((SMB_COM_READ_ANDX_Request_SMB_Parameters)value).WordCount == 12;
        }
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_READ_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_ANDX_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_READ_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 12
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. This value MUST be set to 0xFF 
        /// if there are no additional SMB command responses in the server response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent, and the client MUST
        /// ignore this field
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the SMB header to
        /// the start of the WordCount field in the next SMB command response in this packet.
        /// This field is valid only if the AndXCommand field is  not set to 0xFF. If AndXCommand
        /// is 0xFF, this field MUST be ignored by the client
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// This field is valid when reading from named pipes or I/O devices. This field indicates 
        /// the number of bytes remaining to be read after the requested read was completed. If the
        /// client read from a disk file, this field MUST be set to -1 (0xFFFF). 
        /// </summary>
        public ushort Available;

        /// <summary>
        /// Reserved and SHOULD be 0
        /// </summary>
        public ushort DataCompactionMode;

        /// <summary>
        /// This field MUST be 0
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// The number of data bytes included in the response. This field MAY be 0. If this value
        /// is less than the value in the Request.SMB_Parameters.MaxCountOfBytesToReturn field it 
        /// indicates that the read operation has reached the end of the file (EOF).
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// The offset in bytes from the header of the read data
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// Reserved and MUST be 0. The last 5 words are reserved in order
        /// to make the SMB_COM_READ_ANDX Response the same size as the SMB_COM_WRITE_ANDX Response.
        /// </summary>
        [StaticSize(5, StaticSizeMode.Elements)]
        public ushort[] Reserved2;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_READ_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_ANDX_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 0
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field is OPTIONAL. When using the NT LAN Manager dialect, this field MAY be used to align
        /// the Data field to a 16-bit boundary relative to the start of the SMB Header. If Unicode strings
        /// are being used, this field MUST be present. When used, this field MUST be one padding byte long
        /// </summary>
        public byte[] Pad;

        /// <summary>
        /// The actual bytes read in response to the request
        /// </summary>
        public byte[] Data;
    }

    #endregion

    #region 2.2.4.43   	SMB_COM_WRITE_ANDX (0x2F)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_ANDX_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be either 12 or 14.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet.
        /// This value MUST be set to 0xFF if there are no additional SMB commands in the client request packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this request is sent, and the server MUST ignore this
        /// value when the message is received
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the SMB header to the start of
        /// the WordCount field in the next SMB command in this packet. This field is valid only if the
        /// AndXCommand field is not set to 0xFF. If AndXCommand is 0xFF, this field MUST be ignored by the server.
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// This field MUST be a valid FID indicating the file to which the data SHOULD be written.
        /// </summary>
        public ushort FID;

        /// <summary>
        /// If WordCount is 12 this field represents a 32-bit offset, measured in bytes, of where the write
        /// SHOULD start relative to the beginning of the file. 
        /// If WordCount is 14 this field represents the lower 32 bits of a 64-bit offset.
        /// </summary>
        public uint Offset;

        /// <summary>
        /// This field represents the amount of time, in milliseconds, that a server MUST wait before sending
        /// a response. It is used only when writing to a named pipe or I/O device and does not apply when
        /// writing to a disk file. Support for this field is optional.
        /// Two values have special meaning in this field: 
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// A 16-bit field containing flags
        /// </summary>
        public WriteAndxWriteMode WriteMode;

        /// <summary>
        /// This field is an advisory field telling the server approximately how many bytes are to be
        /// written to this file before the next non-write operation. It SHOULD include the number of bytes
        /// to be written by this request. The server MAY either ignore this field or use it to perform 
        /// optimizations.
        /// </summary>
        public ushort Remaining;

        /// <summary>
        /// This field MUST be 0
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// This field is the number of bytes included in the SMB_Data that are to be written to the file.
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// The offset in bytes from the start of the SMB header to the start of the data that is to be written to 
        /// the file. Specifying this offset allows a client to efficiently align the data buffer.
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// This field is optional. If WordCount is 12 this field is not included in the request.
        /// If WordCount is 14 this field represents the upper 32 bits of a 64-bit offset, measured in
        /// bytes, of where the write SHOULD start relative to the beginning of the file
        /// </summary>
        [MarshalingCondition("IsOffsetHighPresent")]
        public uint OffsetHigh;


        /// <summary>
        /// A method to determine whether OffsetHigh should be marshalled.
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsOffsetHighPresent(MarshalingType marshalingType, object value)
        {
            return ((SMB_COM_WRITE_ANDX_Request_SMB_Parameters)value).WordCount == 14;
        }
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_ANDX_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 1
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// Padding byte which MUST be ignored
        /// </summary>
        public byte Pad;

        /// <summary>
        /// The bytes to be written to the file
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] Data;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 6. The length in two-byte words of the remaining SMB_Parameters.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. This value MUST be set to
        /// 0xFF if there are no additional SMB command responses in the server response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent, and the client MUST
        /// ignore this field.
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the SMB header
        /// to the start of the WordCountfield in the next SMB command response in this packet.
        /// This field is valid only if the AndXCommand field is not 
        /// set to 0xFF. If AndXCommand is 0xFF, this field MUST be ignored by the client. 
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// The number of bytes written to the file
        /// </summary>
        public ushort Count;

        /// <summary>
        /// This field is valid when writing to named pipes or I/O devices. This field indicates 
        /// the number of bytes remaining to be read after the requested write was completed. 
        /// If the client wrote to a disk file, this field MUST be set to -1 (0xFFFF). 
        /// </summary>
        public ushort Available;

        /// <summary>
        /// This field MUST be 0.
        /// </summary>
        public uint Reserved;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_ANDX_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// A 16-bit field containing flags defined as follows
    /// </summary>
    [Flags()]
    public enum WriteAndxWriteMode : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// If set the server MUST NOT respond to the client before the data is written to disk (write-through).
        /// </summary>
        WritethroughMode = 0x0001,

        /// <summary>
        /// If set the server SHOULD set the Response.SMB_Parameters.Available field correctly for
        /// writes to named pipes or I/O devices.
        /// </summary>
        ReadBytesAvailable = 0x0002,

        /// <summary>
        /// Applicable to named pipes only. If set, the named pipe MUST be written to in raw mode (no translation).
        /// </summary>
        RAW_MODE = 0x0004,

        /// <summary>
        /// Applicable to named pipes only. If set, this data is the start of a message
        /// </summary>
        MSG_START = 0x0008
    }

    #endregion

    // 2.2.4.44   	SMB_COM_NEW_FILE_SIZE (0x30)
    // This command was reserved but not implemented. It was also never defined. It is listed in [SNIA-CIFS].
    // However, it is not defined in that document and does not appear in any other references. 
    // Clients SHOULD NOT send requests using this command code, and servers receiving requests with this command
    // code MUST return STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd).

    #region 2.2.4.45   	SMB_COM_CLOSE_AND_TREE_DISC (0x31)

    // SMB_COM_CLOSE_AND_TREE_DISC should be formatted exactly like SMB_COM_CLOSE.

    #endregion

    #region 2.2.4.46   	SMB_COM_TRANSACTION2 (0x32)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TRANSACTION2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION2_Request_SMB_Parameters
    {
        /// <summary>
        /// The value of Words.SetupCount plus 14. This value represents the total number of SMB parameter words 
        /// and MUST be greater than or equal to 14
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The total number of SMB_COM_TRANSACTION2 parameter bytes to be sent in this transaction request.
        /// This value MAY be reduced in any or all subsequent SMB_COM_TRANSACTION2_SECONDARY requests that
        /// are part of the same transaction. This value represents transaction parameter bytes, not SMB
        /// parameter words. Transaction parameter bytes are carried within in the SMB_Data block of the
        /// SMB_COM_TRANSACTION2 request.
        /// </summary>
        public ushort TotalParameterCount;

        /// <summary>
        /// The total number of SMB_COM_TRANSACTION2 data bytes to be sent in this transaction request.
        /// This value MAY be reduced in any or all subsequent SMB_COM_TRANSACTION2_SECONDARY requests
        /// that are part of the same transaction. This value represents transaction  data bytes,
        /// not SMB data bytes.
        /// </summary>
        public ushort TotalDataCount;

        /// <summary>
        /// The maximum number of parameter bytes that the client will accept in the transaction reply.
        /// The server MUST NOT return more than this number of parameter bytes.
        /// </summary>
        public ushort MaxParameterCount;

        /// <summary>
        /// USHORT The maximum number of data bytes that the client will accept in the transaction reply.
        /// The server MUST NOT return more than this number of data bytes.
        /// </summary>
        public ushort MaxDataCount;

        /// <summary>
        /// Maximum number of setup bytes that the client will accept in the transaction reply.
        /// The server MUST NOT return more than this number of setup bytes.
        /// </summary>
        public byte MaxSetupCount;

        /// <summary>
        /// A padding byte. This field MUST be zero. Existing CIFS implementations MAY combine this field
        /// with MaxSetupCount to form a USHORT.
        /// If MaxSetupCount is defined as a USHORT, the high order byte MUST be zero.
        /// </summary>
        public byte Reserved1;

        /// <summary>
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields
        /// MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. 
        /// </summary>
        public ushort Flags;

        /// <summary>
        /// The number of milliseconds the server SHOULD wait for completion of the transaction before
        /// generating a timeout. A value of zero indicates that the operation MUST NOT block.
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// Reserved. This field MUST be zero in the client request. The server MUST ignore the contents
        /// of this field
        /// </summary>
        public ushort Reserved2;

        /// <summary>
        /// The number of transaction parameter bytes being sent in this SMB message. 
        /// If the transaction fits within a single SMB_COM_TRANSACTION2 request,
        /// then this value MUST be equal to TotalParameterCount. Otherwise, the sum
        /// of the ParameterCount values in the primary and secondary transaction 
        /// request messages MUST be equal to the smallest TotalParameterCount value
        /// reported to the server. If the value of this field is less than the 
        /// value of TotalParameterCount, then at least one SMB_COM_TRANSACTION2_SECONDARY
        /// message MUST be used to transfer the remaining parameter bytes. 
        /// The ParameterCount field MUST be used to determine the number of transaction
        /// parameter bytes contained within the SMB_COM_TRANSACTION2 message
        /// </summary>
        public ushort ParameterCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the 
        /// transaction parameter bytes. This MUST be the number of bytes from the start
        /// of the SMB message to the start of the SMB_Data.Bytes.Parameters field. 
        /// Server implementations MUST use this value to locate the 
        /// transaction parameter block within the SMB message
        /// </summary>
        public ushort ParameterOffset;

        /// <summary>
        /// The number of transaction data bytes being sent in this SMB message. 
        /// If the transaction fits within a single SMB_COM_TRANSACTION2 request,
        /// then this value MUST be equal to TotalDataCount. Otherwise, the sum 
        /// of the DataCount values in the primary and secondary transaction 
        /// request messages MUST be equal to the smallest TotalDataCount 
        /// value reported to the server. If the value of this field is less than the
        /// value of TotalDataCount, then at least one SMB_COM_TRANSACTION2_SECONDARY
        /// message MUST be used to transfer the remaining data bytes.
        /// </summary>
        public ushort DataCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the transaction
        /// data bytes. This MUST be the number of bytes from the start
        /// of the SMB message to the start of the SMB_Data.Bytes.Data field.
        /// Server implementations MUST use this value to locate the transaction
        /// data block within the SMB message.
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// The number of setup words that are included in the transaction request.
        /// </summary>
        public byte SetupCount;

        /// <summary>
        /// A padding byte. This field MUST be zero. Existing CIFS implementations
        /// MAY combine this field with SetupCount to form a USHORT. 
        /// If SetupCount is defined as a USHORT, the high order byte MUST be zero
        /// </summary>
        public byte Reserved3;

        /// <summary>
        /// An array of two-byte words that provide transaction context to the server.
        /// The size and content of the array are specific to individual subcommands.
        /// SMB_COM_TRANSACTION2 messages MAY exceed the maximum size of a single SMB 
        /// message (as determined by the value of the MaxBufferSize session 
        /// parameter). If this is the case, then the client MUST use one or more
        /// SMB_COM_TRANSACTION2_SECONDARY messages to transfer transaction Data
        /// and Parameter bytes that did not fit in the initial message.
        /// </summary>
        [Size("SetupCount")]
        public ushort[] Setup;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TRANSACTION2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION2_Request_SMB_Data
    {
        /// <summary>
        /// The number of bytes in the SMB_Data.Bytes array
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field is present but not used in SMB_COM_TRANSACTION2 requests. 
        /// If Unicode support has been negotiated, then this field MUST be 
        /// aligned to a 16-bit boundary and MUST consist of two null bytes 
        /// (a null-terminator). If Unicode support has not been negotiated this 
        /// field will contain only one null byte.
        /// </summary>
        public byte[] Name;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 2 or 4 byte boundary.
        /// </summary>
        public byte[] Pad1;

        /// <summary>
        /// Transaction parameter bytes. See the individual SMB_COM_TRANSACTION2 
        /// subcommand descriptions for information on parameters sent
        /// for each subcommand.
        /// </summary>
        public byte[] Trans2_Parameters;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 2 or 4 byte boundary. 
        /// </summary>
        public byte[] Pad2;

        /// <summary>
        /// Transaction data bytes. See the individual SMB_COM_TRANSACTION2 subcommand 
        /// descriptions for information on data sent for each subcommand.
        /// </summary>
        public byte[] Trans2_Data;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TRANSACTION2_InterimResponse_SMB_Parameters
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION2_InterimResponse_SMB_Parameters
    {
        /// <summary>
        /// WordCount fields MUST be zero.
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TRANSACTION2_InterimResponse_SMB_Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION2_InterimResponse_SMB_Data
    {
        /// <summary>
        ///  ByteCount fields MUST be zero.
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters
    {
        /// <summary>
        /// The value of Words.SetupCount plus 10. This value represents the total 
        /// number of SMB parameter words and MUST be greater than or equal
        /// to 10
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The total number of SMB_COM_TRANSACTION2 parameter bytes to be sent
        /// in this transaction response. This value MAY be reduced in any 
        /// or all subsequent SMB_COM_TRANSACTION2 responses that are part of
        /// the same transaction. This value represents transaction parameter
        /// bytes, not SMB parameter words. Transaction parameter bytes are 
        /// carried within in the SMB_data block
        /// </summary>
        public ushort TotalParameterCount;

        /// <summary>
        /// The total number of SMB_COM_TRANSACTION2 data bytes to be sent in
        /// this transaction response. This value MAY be reduced in any or all 
        /// subsequent SMB_COM_TRANSACTION2 responses that are part of the same
        /// transaction. This value represents transaction data bytes, 
        /// not SMB data bytes
        /// </summary>
        public ushort TotalDataCount;

        /// <summary>
        /// Reserved. This field MUST be zero in the client request. 
        /// The server MUST ignore the contents of this field
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// The number of transaction parameter bytes being sent in this SMB message. 
        /// If the transaction fits within a single SMB_COM_TRANSACTION2
        /// response, then this value MUST be equal to TotalParameterCount.
        /// Otherwise, the sum of the ParameterCount values in the transaction 
        /// response messages MUST be equal to the smallest TotalParameterCount
        /// value reported by the server. The ParameterCount field MUST be 
        /// used to determine the number of transaction parameter bytes 
        /// contained within the SMB message
        /// </summary>
        public ushort ParameterCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the 
        /// transaction parameter bytes. This MUST be the number of bytes from
        /// the start of the SMB message to the start of the SMB_Data.Bytes.
        /// Parameters field. Server implementations MUST use this value to locate
        /// the transaction parameter block within the SMB message
        /// </summary>
        public ushort ParameterOffset;

        /// <summary>
        /// The offset, relative to all of the transaction parameter bytes in 
        /// this transaction response, at which this block of parameter 
        /// bytes MUST be placed. This value MAY be used by the client to
        /// correctly reassemble the transaction parameters even if the SMB 
        /// response messages are received out of order
        /// </summary>
        public ushort ParameterDisplacement;

        /// <summary>
        /// The number of transaction data bytes being sent in this SMB message.
        /// If the transaction fits within a single SMB_COM_TRANSACTION2 
        /// response, then this value MUST be equal to TotalDataCount. Otherwise,
        /// the sum of the DataCount values in the transaction response
        /// messages MUST be equal to the smallest TotalDataCount value reported by the server.
        /// </summary>
        public ushort DataCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the
        /// transaction data bytes. This MUST be the number of bytes from 
        /// the start of the SMB message to the start of the SMB_Data.Bytes.
        /// Data field. Server implementations MUST use this value to
        /// locate the transaction data block within the SMB message
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// The offset, relative to all of the transaction data bytes in this
        /// transaction response, at which this block of data bytes
        /// MUST be placed. This value MAY be used by the client to correctly
        /// reassemble the transaction data even if the SMB response
        /// messages are received out of order.
        /// </summary>
        public ushort DataDisplacement;

        /// <summary>
        /// The number of setup words that are included in the transaction response.
        /// </summary>
        public byte SetupCount;

        /// <summary>
        /// A padding byte. This field MUST be zero. Existing CIFS implementations
        /// MAY combine this field with SetupCount to form a USHORT.
        /// If SetupCount is defined as a USHORT, the high order byte MUST be zero
        /// </summary>
        public byte Reserved2;

        /// <summary>
        /// An array of two-byte words that provide transaction results from the server.
        /// The size and content of the array are specific to individual subcommands.
        /// </summary>
        [Size("SetupCount")]
        public ushort[] Setup;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TRANSACTION2_FinalResponse_SMB_Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION2_FinalResponse_SMB_Data
    {
        /// <summary>
        /// The number of bytes in the SMB_Data.Bytes array, which follows.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 2 byte or 4 byte boundary.
        /// </summary>
        public byte[] Pad1;

        /// <summary>
        /// Transaction parameter bytes. See the individual SMB_COM_TRANSACTION2
        /// subcommand descriptions for information on parameters
        /// returned by the server for each subcommand.
        /// </summary>
        public byte[] Trans2_Parameters;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 2 or 4 byte boundary.
        /// </summary>
        public byte[] Pad2;

        /// <summary>
        /// Transaction data bytes. See the individual SMB_COM_TRANSACTION2 subcommand descriptions 
        /// for information on data returned by the server for each subcommand.
        /// </summary>
        public byte[] Trans2_Data;
    }


    /// <summary>
    /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields 
    /// MUST be set to zero by the client sending the request, and MUST be ignored by the server
    /// receiving the request. The client MAY set either or both of the following bit flags
    /// </summary>
    [Flags()]
    public enum Trans2SmbParametersFlags : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// If set, following the completion of the operation the server MUST
        /// disconnect the tree connection associated with the tree identifier (TID)
        /// field received in the SMB header of this request. The client SHOULD NOT 
        /// send a subsequent SMB_COM_TREE_DISCONNECT for this tree connect. 
        /// </summary>
        DISCONNECT_TID = 0x0001,

        /// <summary>
        /// This is a one-way transaction. The server MUST attempt to complete the 
        /// transaction, but MUST NOT send a response to the client.
        /// </summary>
        NO_RESPONSE = 0x0002
    }

    #endregion

    #region 2.2.4.47   	SMB_COM_TRANSACTION2_SECONDARY (0x33)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TRANSACTION2_SECONDARY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION2_SECONDARY_Request_SMB_Parameters
    {
        /// <summary>
        /// This value represents the total number of SMB parameter words and MUST be 9.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The total number of transaction parameter bytes to be sent to the
        /// server over the course of this transaction. This value MAY be less
        /// than or equal to the TotalParameterCount in preceding request
        /// messages that are part of the same transaction. This value represents 
        /// transaction parameter bytes, not SMB parameter words. 
        /// </summary>
        public ushort TotalParameterCount;

        /// <summary>
        /// The total number of transaction data bytes to be sent to the 
        /// server over the course of this transaction. This value MAY be less than
        /// or equal to the TotalDataCount in preceding request messages that
        /// are part of the same transaction. This value represents transaction 
        /// data bytes, not SMB data bytes
        /// </summary>
        public ushort TotalDataCount;

        /// <summary>
        /// The number of transaction parameter bytes being sent in the SMB message.
        /// This value MUST be less than TotalParameterCount.
        /// The sum of the ParameterCount values across all of the request
        /// messages in a transaction MUST be equal to the TotalParameterCount 
        /// reported in the last request message of the transaction.
        /// </summary>
        public ushort ParameterCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the 
        /// transaction parameter bytes contained in this SMB message.
        /// This MUST be the number of bytes from the start of the SMB message
        /// to the start of the SMB_Data.Bytes.Trans2_Parameters field.
        /// Server implementations MUST use this value to locate the 
        /// transaction parameter block within the SMB message
        /// </summary>
        public ushort ParameterOffset;

        /// <summary>
        /// The offset, relative to all of the transaction parameter bytes
        /// sent to the server in this transaction, at which this block of parameter 
        /// bytes SHOULD be placed. This value MAY be used by the server to
        /// correctly reassemble the transaction parameters even if the SMB request 
        /// messages are received out of order.
        /// </summary>
        public ushort ParameterDisplacement;

        /// <summary>
        /// The number of transaction data bytes being sent in this SMB message. 
        /// This value MUST be less than the value of TotalDataCount.
        /// The sum of the DataCount values across all of the request messages
        /// in a transaction MUST be equal to the smallest TotalDataCount
        /// value reported to the server
        /// </summary>
        public ushort DataCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the
        /// transaction data bytes contained in this SMB message. 
        /// This MUST be the number of bytes from the start of the SMB 
        /// message to the start of the SMB_Data.Bytes.Trans2_Data field. 
        /// Server implementations MUST use this value to locate the
        /// transaction data block within the SMB message.
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// The offset, relative to all of the transaction data bytes
        /// sent to the server in this transaction, 
        /// at which this block of parameter bytes SHOULD be placed. 
        /// This value MAY be used by the server to correctly reassemble the transaction 
        /// data block even if the SMB request messages are received out of order.
        /// </summary>
        public ushort DataDisplacement;

        /// <summary>
        /// Either a valid File ID returned by a previous Open or Create operation,
        /// or 0xFFFF. A FID value of 0xFFFF is, by definition, 
        /// an invalid FID and indicates that no FID is being sent in this request. 
        /// See the individual descriptions of the Trans2 subcommands
        /// for specific information on the use of this field.
        /// </summary>
        public ushort FID;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TRANSACTION2_SECONDARY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION2_SECONDARY_Request_SMB_Data
    {
        /// <summary>
        /// The number of bytes in the SMB_Data.Bytes array, which follows.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 16 or 32 bit boundary.
        /// </summary>
        public byte[] Pad1;

        /// <summary>
        /// Transaction parameter bytes
        /// </summary>
        public byte[] Trans2_Parameters;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 16 or 32 bit boundary.
        /// </summary>
        public byte[] Pad2;

        /// <summary>
        /// Transaction data bytes
        /// </summary>
        public byte[] Trans2_Data;
    }

    // struct SMB_COM_TRANSACTION2_SECONDARY_Response
    // There is no response message defined for the SMB_COM_TRANSACTION2_SECONDARY command. 

    #endregion

    #region 2.2.4.48   	SMB_COM_FIND_CLOSE2 (0x34)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_FIND_CLOSE2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_CLOSE2_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// A search handle, also known as a Search ID (SID). This MUST be the SID
        /// value returned in the initial TRANS2_FIND_FIRST2 
        /// subcommand request
        /// </summary>
        public ushort SearchHandle;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_FIND_CLOSE2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_CLOSE2_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message.
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_FIND_CLOSE2 Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_CLOSE2_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_FIND_CLOSE2 Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_CLOSE2_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    // 2.2.4.49   	SMB_COM_FIND_NOTIFY_CLOSE (0x35)
    // This command was introduced in the LAN Manager 1.2 dialect, and was reserved but not implemented. 

    #region 2.2.4.50   	SMB_COM_TREE_CONNECT (0x70)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TREE_CONNECT Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_CONNECT_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be zero. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TREE_CONNECT Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_CONNECT_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 6
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// A buffer format identifier. The value of this field MUST be 0x04.
        /// </summary>
        public byte BufferFormat1;

        /// <summary>
        /// A null-terminated string that represents the server and share
        /// name of the resource to which the client is attempting to connect.
        /// This field MUST be encoded using Universal Naming Convention (UNC)
        /// syntax. The string MUST be a null-terminated array of OEM characters, 
        /// even if the client and server have negotiated to use Unicode strings.
        /// A share path in UNC syntax would be represented by a string in the following form:
        /// \\server\share
        /// </summary>
        public byte[] Path;

        /// <summary>
        /// A buffer format identifier. The value of this field MUST be 0x04.
        /// </summary>
        public byte BufferFormat2;

        /// <summary>
        /// A null-terminated string that represents a share password in plaintext form.
        /// The string MUST be a null-terminated array of OEM characters,
        /// even if the client and server have negotiated to use Unicode strings.
        /// </summary>
        public byte[] Password;

        /// <summary>
        /// A buffer format identifier. The value of this field MUST be 0x04.
        /// </summary>
        public byte BufferFormat3;

        /// <summary>
        /// A null-terminated string representing the type of resource the client
        /// intends to access. This field MUST be a null-terminated array
        /// of OEM characters, even if the client and server have negotiated to use Unicode strings. 
        /// </summary>
        public byte[] Service;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TREE_CONNECT Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_CONNECT_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be set to 2
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The maximum size, in bytes, of the largest SMB message that the
        /// server MAY receive. This is the size of the largest SMB message 
        /// that the client MAY send to the server. SMB message size includes
        /// the size of the SMB header, parameter, and data blocks. This 
        /// size MUST not include any transport-layer framing or other transport-layer data.
        /// </summary>
        public ushort MaxBufferSize;

        /// <summary>
        /// The newly generated Tree ID, used in subsequent CIFS client
        /// requests to refer to a resource relative to the SMB_Data.Bytes.
        /// Path given in the request. Most access to the server requires
        /// a valid TID, whether the resource is password protected or not.
        /// The value 0xFFFF is reserved; the server MUST NOT return a TID value of 0xFFFF.
        /// </summary>
        public ushort TID;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TREE_CONNECT Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_CONNECT_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.51   	SMB_COM_TREE_DISCONNECT (0x71)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TREE_DISCONNECT Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_DISCONNECT_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message.
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TREE_DISCONNECT Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_DISCONNECT_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TREE_DISCONNECT Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_DISCONNECT_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TREE_DISCONNECT Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_DISCONNECT_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.52   	SMB_COM_NEGOTIATE (0x72)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NEGOTIATE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NEGOTIATE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_NEGOTIATE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NEGOTIATE_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This is a variable length list of dialect identifiers in order 
        /// of preference from least to most preferred. The client MUST
        /// only list dialects that it supports.
        /// </summary>
        [Size("ByteCount")]
        public byte[] Dialects;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NEGOTIATE_MinimumResponse_SMB_Parameters
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NEGOTIATE_MinimumResponse_SMB_Parameters
    {
        /// <summary>
        /// The value of this field MUST be greater than or equal to 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The index of the dialect selected by the server from the list
        /// presented in the request. Dialect entries are numbered starting 
        /// with zero, so a DialectIndex value of zero indicates that the
        /// first entry in the list has been selected. If the server does not
        /// support any of the listed dialects, it MUST return a DialectIndex of 0XFFFF. 
        /// </summary>
        public ushort DialectIndex;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_NEGOTIATE_MinimumResponse_SMB_Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NEGOTIATE_MinimumResponse_SMB_Data
    {
        /// <summary>
        /// The value of this field is variable, depending upon the dialect selected.
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters
    {
        /// <summary>
        /// The value of this field MUST be 17
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The index of the dialect selected by the server from the list
        /// presented in the request. Dialect entries are numbered starting 
        /// with zero, so a DialectIndex value of zero indicates the first
        /// entry in the list. If the server does not support any of the 
        /// listed dialects, it MUST return a DialectIndex of 0XFFFF.
        /// </summary>
        public ushort DialectIndex;

        /// <summary>
        /// An 8-bit field, indicating the security modes supported or REQUIRED by the server
        /// </summary>
        public SecurityModes SecurityMode;

        /// <summary>
        /// The maximum number of outstanding SMB operations the server
        /// supports. This value includes existing OpLocks, 
        /// the NT_TRANSACT_NOTIFY_CHANGE subcommand, and any other command 
        /// that are pending on the server. If the negotiated MaxMpxCount is one, 
        /// then OpLock support MUST be disabled for this session. The MaxMpxCount
        /// MUST be greater than zero. This parameter has no specific 
        /// relationship to the SMB_COM_READ_MPX and SMB_COM_WRITE_MPX commands. 
        /// </summary>
        public ushort MaxMpxCount;

        /// <summary>
        /// The maximum number of virtual circuits that MAY be established between
        /// the client and the server as part of the same SMB session. 
        /// </summary>
        public ushort MaxNumberVcs;

        /// <summary>
        /// The maximum size, in bytes, of the largest SMB message the server
        /// can receive. This is the size of the largest SMB message that the client
        /// MAY send to the server. SMB message size includes the size of the 
        /// SMB header, parameter, and data blocks. This size does not include any 
        /// transport-layer framing or other transport-layer data. The server MUST
        /// provide a MaxBufferSize of 1024 bytes (1Kbyte) or larger.
        /// If CAP_RAW_MODE is negotiated, then the SMB_COM_WRITE_RAW command
        /// can bypass the MaxBufferSize limit. Otherwise, SMB messages sent to the server 
        /// MUST have a total size less than or equal to the MaxBufferSize value.
        /// This includes AndX chained messages.<WBN>
        /// The default MaxBufferSize on Windows NT server is 4356 bytes
        /// (4KB + 260Bytes) if the server has 512MB of memory or less. If the server has more 
        /// than 512MB of memory, then the default MaxBufferSize is 16644 bytes 
        /// (16KB + 260Bytes). Windows NT servers always use a MaxBufferSize value that 
        /// is a multiple of four (4). The MaxBufferSize can be configured through the following registry setting:  
        /// HKLM\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters\SizeReqBuf</WBN>
        /// </summary>
        public uint MaxBufferSize;

        /// <summary>
        /// The maximum raw buffer size, in bytes, available on the server. 
        /// This value specifies the maximum message size which the client MUST not
        /// exceed when sending an SMB_COM_WRITE_RAW client request, and the
        /// maximum message size that the server MUST not exceed when sending an 
        /// SMB_COM_READ_RAW response. This value is only significant if CAP_RAW_MODE is negotiated.
        /// </summary>
        public uint MaxRawSize;

        /// <summary>
        /// A unique token identifying the SMB connection. This value is
        /// generated by the server for each SMB connection. If the client wishes to
        /// create an additional virtual circuit and attach it to the same
        /// SMB connection, the client MUST provide the SessionKey in the 
        /// SMB_COM_SESSION_SETUP_ANDX. This allows multiple transport-level
        /// connections to be bound together to form a single logical SMB
        /// connection.
        /// </summary>
        public uint SessionKey;

        /// <summary>
        /// A 32-bit field providing a set of server capability indicators.
        /// This bit field is used to indicate to the client which 
        /// features are supported by the server. Any value not listed in 
        /// the following table is unused. The server MUST set the unused
        /// bits to 0 in a response, and the client MUST ignore these bits
        /// </summary>
        public Capabilities Capabilities;

        /// <summary>
        /// The number of 100-nanosecond intervals that have elapsed since 
        /// January 1, 1601, in Coordinated Universal Time (UTC) format
        /// </summary>
        public FileTime SystemTime;

        /// <summary>
        /// A signed 16-bit signed integer that represents the server's time
        /// zone, in minutes, from UTC. The time zone of the server 
        /// MUST be expressed in minutes, plus or minus, from UTC.
        /// </summary>
        public short ServerTimeZone;

        /// <summary>
        /// This field MUST be 0 or 8. The length of the random challenged
        /// used in challenge/response authentication. If the server 
        /// does not support challenge/response authentication, this field
        /// MUST be zero. This field is often referred to as EncryptionKeyLength.
        /// </summary>
        public byte ChallengeLength;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data
    {
        /// <summary>
        /// field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An array of unsigned bytes that MUST be ChallengeLength bytes 
        /// long and MUST represent the server challenge. This array MUST NOT
        /// be nul-terminated.
        /// </summary>
        public byte[] Challenge;

        /// <summary>
        /// The name of NT domain or workgroup to which the server belongs.
        /// </summary>
        public byte[] DomainName;
    }


    /// <summary>
    /// This is a variable length list of dialect identifiers in order of preference from least to most preferred. 
    /// The client MUST only list dialects that it supports. The structure of the list entries is as follows:
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_Dialect
    {
        /// <summary>
        /// This field MUST be 0x02. This is a buffer format indicator 
        /// that identifies the next field as a NUL-terminated array of characters
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// A null-terminated string identifying an SMB dialect. A list of common dialects is presented in Section 1.7.
        /// </summary>
        [String(StringEncoding.ASCII)]
        public string DialectString;
    }


    /// <summary>
    /// An 8-bit field, indicating the security modes supported or REQUIRED by the server, as follows:
    /// </summary>
    [Flags()]
    public enum SecurityModes : byte
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// If clear (0), the server supports only Share Level access control.
        /// If set (1), the server supports only User Level access control.
        /// </summary>
        NEGOTIATE_USER_SECURITY = 0x01,

        /// <summary>
        /// If clear, the server supports only plaintext password authentication.
        /// If set, the server supports challenge/response authentication. 
        /// </summary>
        NEGOTIATE_ENCRYPT_PASSWORDS = 0x02,

        /// <summary>
        /// If clear, the server does not support SMB security signatures.
        /// If set, the server supports SMB security signatures for this connection. 
        /// </summary>
        NEGOTIATE_SECURITY_SIGNATURES_ENABLED = 0x04,

        /// <summary>
        /// If clear, the security signatures are OPTIONAL for this connection.
        /// if set, the server requires security signatures.
        /// This bit MUST be clear if the previous bit is clear.
        /// </summary>
        NEGOTIATE_SECURITY_SIGNATURES_REQUIRED = 0x08,
    }

    #endregion

    #region 2.2.4.53   	SMB_COM_SESSION_SETUP_ANDX (0x73)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_SESSION_SETUP_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters
    {
        /// <summary>
        /// The value of this field MUST be 13
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be either the command code for the next SMB command in the packet or 0xFF.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent, and the client MUST ignore this field.
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start
        /// of the SMB header to the start of the WordCount field in the next
        /// SMB command in this packet. This field is valid only if the 
        /// AndXCommand field is not set to 0xFF. If AndXCommand is 0xFF, this 
        /// field MUST be ignored by the server.
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// The maximum size, in bytes, of the largest SMB message that
        /// the client can receive. This is the size of the largest SMB message
        /// that the server MAY send to the client. SMB message size 
        /// includes the size of the SMB header, parameter, and data blocks. This
        /// size MUST not include any transport-layer framing or other transport-layer data.
        /// </summary>
        public ushort MaxBufferSize;

        /// <summary>
        /// The maximum number of pending multiplexed requests supported 
        /// by the client. This value MUST be less than or equal to the MaxMpxCount
        /// value provided by the server in the SMB_COM_NEGOTIATE response
        /// </summary>
        public ushort MaxMpxCount;

        /// <summary>
        /// The number of this VC (virtual circuit) between the client 
        /// and the server. This field SHOULD be set to a value of 0 for the first
        /// virtual circuit between the client and the server and it 
        /// SHOULD be set to a unique nonzero value for additional virtual circuit.
        /// </summary>
        public ushort VcNumber;

        /// <summary>
        /// The client MUST set this to be equal to the SessionKey field
        /// in the SMB_COM_NEGOTIATE response for this SMB connection
        /// </summary>
        public uint SessionKey;

        /// <summary>
        /// The length, in bytes, of the contents of the SMB_Data.OEMPassword field.
        /// </summary>
        public ushort OEMPasswordLen;

        /// <summary>
        /// The length, in bytes, of the contents of the SMB_Data.UnicodePassword field.
        /// </summary>
        public ushort UnicodePasswordLen;

        /// <summary>
        /// Reserved. This field MUST be zero. The server MUST ignore the contents of this field.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A 32-bit field providing a set of client capability indicators.
        /// The client uses this field to report its own set of capabilities to
        /// the server. The client capabilities are a subset of the server capabilities
        /// </summary>
        public Capabilities Capabilities;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_SESSION_SETUP_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Data
    {
        /// <summary>
        /// USHORT
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// The contents of this field depend upon the authentication methods in use:
        /// If Unicode has not been negotiated and the client is sending a 
        /// plaintext password, this field MUST contain the password represented in the OEM character set.
        /// If the client is using challenge/response authentication, this field 
        /// MAY contain a cryptographic response. This field MAY be empty.
        /// The OEMPassword value is an array of bytes, not a null-terminated string.
        /// </summary>
        public byte[] OEMPassword;

        /// <summary>
        /// The contents of this field depend upon the authentication methods in use:
        /// If Unicode has been negotiated and the client is sending a plaintext 
        /// password, this field MUST contain the password represented in UTF-16LE Unicode.
        /// If the client is using challenge/response authentication, this field MAY 
        /// contain a cryptographic response. This field MAY be empty. 
        /// </summary>
        public byte[] UnicodePassword;

        /// <summary>
        /// Padding bytes. If Unicode support has been enabled and SMB_FLAGS2_UNICODE
        /// is set in SMB_Header.Flags2, this field MUST contain zero
        /// or one NULL bytes as needed to ensure that the AccountName
        /// string is aligned on a 16-bit boundary. This also forces alignment of subsequent
        /// strings without additional padding
        /// </summary>
        public byte[] Pad;

        /// <summary>
        /// The name of the account (username) with which the user authenticates.
        /// </summary>
        public byte[] AccountName;

        /// <summary>
        /// A string representing the desired authentication domain. This MAY
        /// be the empty string. If SMB_FLAGS2_UNICODE is set in the Flags2 
        /// field of the SMB header of the request, this string MUST be a 
        /// null-terminated array of 16-bit Unicode characters. Otherwise, this
        /// string MUST be a null-terminated array of OEM characters. If this
        /// string consists of Unicode characters, this field MUST be aligned 
        /// to start on a 2-byte boundary from the start of the SMB header.
        /// </summary>
        public byte[] PrimaryDomain;

        /// <summary>
        /// A string representing the native operating system of the CIFS client.
        /// If SMB_FLAGS2_UNICODE is set in the Flags2 field of the SMB
        /// header of the request, this string MUST be a null-terminated array of
        /// 16-bit Unicode characters. Otherwise, this string MUST be 
        /// a null-terminated array of OEM characters. If this string consists
        /// of Unicode characters, this field MUST be aligned to start on 
        /// a 2-byte boundary from the start of the SMB header.
        /// </summary>
        public byte[] NativeOS;

        /// <summary>
        /// A string that represents the native LAN manager type of the client.
        /// If SMB_FLAGS2_UNICODE is set in the Flags2 field of the SMB header of the request,
        /// this string MUST be a null-terminated array of 16-bit Unicode characters.
        /// Otherwise, this string MUST be a null-terminated array of OEM characters.
        /// If this string consists of Unicode characters, this field MUST be aligned
        /// to start on a 2-byte boundary from the start of the SMB header.
        /// </summary>
        public byte[] NativeLanMan;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_SESSION_SETUP_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// The value of this field MUST be 3.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. This value 
        /// MUST be set to 0xFF if there are no additional SMB
        /// command responses in the server response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent, 
        /// and the client MUST ignore this field.
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of 
        /// the SMB header to the start of the WordCount field in 
        /// the next SMB command response in this packet. This field is valid 
        /// only if the AndXCommand field is not set to 0xFF. 
        /// If AndXCommand is 0xFF, this field MUST be ignored by the client. 
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// A 16-bit field. The two lowest order bits have been defined
        /// </summary>
        public ActionValues Action;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_SESSION_SETUP_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Data
    {
        /// <summary>
        /// USHORT
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// Padding bytes. If Unicode support has been enabled, this field MUST contain zero or 
        /// one null bytes as needed to ensure that the NativeOS field, which follows, is aligned 
        /// on a 16-bit boundary
        /// </summary>
        public byte[] Pad;

        /// <summary>
        /// A string that represents the native operating system of the server.
        /// If SMB_FLAGS2_UNICODE is set in the Flags2 field 
        /// of the SMB header of the response, the string MUST be a null-terminated
        /// array of 16-bit Unicode characters. Otherwise, 
        /// the string MUST be a null-terminated array of OEM characters. 
        /// If the string consists of Unicode characters, this field 
        /// MUST be aligned to start on a 2-byte boundary from the start of the SMB header.
        /// </summary>
        public byte[] NativeOS;

        /// <summary>
        /// A string that represents the native LAN Manager type of the server.
        /// If SMB_FLAGS2_UNICODE is set in the Flags2 field of the
        /// SMB header of the response, the string MUST be a null-terminated 
        /// array of 16-bit Unicode characters. Otherwise, the string
        /// MUST be a null-terminated array of OEM characters. If the string 
        /// consists of Unicode characters, this field MUST be aligned 
        /// to start on a 2-byte boundary from the start of the SMB header.
        /// </summary>
        public byte[] NativeLanMan;

        /// <summary>
        /// A string representing the primary domain or workgroup name of the server. 
        /// If SMB_FLAGS2_UNICODE is set in the Flags2 field
        /// of the SMB header of the response, the string MUST be a null-terminated
        /// array of 16-bit Unicode characters. Otherwise, the
        /// string MUST be a null-terminated array of OEM characters.
        /// If the string consists of Unicode characters, this field MUST be aligned
        /// to start on a 2-byte boundary from the start of the SMB header.
        /// </summary>
        public byte[] PrimaryDomain;
    }


    /// <summary>
    /// A 32-bit field providing a set of client capability indicators. 
    /// The client uses this field to report its own set of capabilities to
    /// the server. The client capabilities are a subset of the server capabilities
    /// </summary>
    [Flags()]
    public enum Capabilities : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// The client supports SMB_COM_READ_RAW and SMB_COM_WRITE_RAW requests.
        /// Raw mode is not supported over connectionless transports.
        /// </summary>
        CAP_RAW_MODE = 0x00000001,

        /// <summary>
        /// The client supports SMB_COM_READ_MPX and SMB_COM_WRITE_MPX requests.
        /// MPX mode is only supported over connectionless transports. 
        /// </summary>
        CAP_MPX_MODE = 0x00000002,

        /// <summary>
        /// The client supports UTF-16LE Unicode strings
        /// </summary>
        CAP_UNICODE = 0x00000004,

        /// <summary>
        /// The client supports 64-bit file offsets. 
        /// </summary>
        CAP_LARGE_FILES = 0x00000008,

        /// <summary>
        /// The client supports SMB commands particular to the NT LAN Manager dialect. 
        /// </summary>
        CAP_NT_SMBS = 0x00000010,

        /// <summary>
        /// The client supports the use of Microsoft remote procedure call (MS-RPC) for remote API calls. 
        /// </summary>
        CAP_RPC_REMOTE_APIS = 0x00000020,

        /// <summary>
        /// The client supports 32-bit status codes, received in the Status field of the SMB header.
        /// CAP_STATUS32 is also sometimes referred to as CAP_NT_STATUS. 
        /// </summary>
        CAP_STATUS32 = 0x00000040,

        /// <summary>
        /// The client supports level II opportunistic locks (OpLocks). 
        /// </summary>
        CAP_LEVEL_II_OPLOCKS = 0x00000080,

        /// <summary>
        /// The client supports the SMB_COM_LOCK_AND_READ command. 
        /// </summary>
        CAP_LOCK_AND_READ = 0x00000100,

        /// <summary>
        /// The client supports the TRANS2_FIND_FIRST2, TRANS2_FIND_NEXT2, and FIND_CLOSE2 command requests. 
        /// </summary>
        CAP_NT_FIND = 0x00000200,

        /// <summary>
        /// This value was reserved but not implemented and MUST be zero.
        /// </summary>
        CAP_BULK_TRANSFER = 0x00000400,

        /// <summary>
        /// This value was reserved but not implemented and MUST be zero.
        /// </summary>
        CAP_COMPRESSED_DATA = 0x00000800,

        /// <summary>
        /// The client supports the DFS Referral Protocol, as specified in [MS-DFSC]. 
        /// </summary>
        CAP_DFS = 0x00001000,

        /// <summary>
        /// This value was reserved but not implemented and MUST be zero.
        /// </summary>
        CAP_QUADWORD_ALIGNED = 0x00002000,

        /// <summary>
        /// The client supports large read operations.
        /// This capability affects the maximum size, in bytes, of the client buffer
        /// for receiving an SMB_COM_READ_ANDX response from the server.
        /// When this capability is set by the client, the maximum client buffer size
        /// for receiving anSMB_COM_READ_ANDX can be up to 65,535 bytes
        /// rather than the MaxBufferSize field. 
        /// </summary>
        CAP_LARGE_READX = 0x00004000,

        /// <summary>
        /// include all of the Capabilities
        /// </summary>
        ALL = CAP_RAW_MODE | CAP_MPX_MODE | CAP_UNICODE | CAP_LARGE_FILES
            | CAP_NT_SMBS | CAP_RPC_REMOTE_APIS | CAP_LEVEL_II_OPLOCKS
            | CAP_STATUS32 | CAP_LOCK_AND_READ | CAP_NT_FIND | CAP_DFS | CAP_LARGE_READX,
    }


    /// <summary>
    /// A 16-bit field which indicates the authentication actions. The two lowest-order bits have been defined.
    /// </summary>
    [Flags()]
    public enum ActionValues : ushort
    {
        /// <summary>
        /// None
        /// </summary>
        NONE = 0,

        /// <summary>
        /// If clear (0), the user successfully authenticated and is logged in.
        /// if set (1), authentication failed but the server has granted guest access. The user is logged in as Guest.
        /// </summary>
        GuestAccess = 0x0001,

        /// <summary>
        /// If clear, the NTLM user session key will be used for message signing (if enabled).
        /// If set, the LM session key will be used for message signing.
        /// </summary>
        LmSigning = 0x0002,
    }

    #endregion

    #region 2.2.4.54   	SMB_COM_LOGOFF_ANDX (0x74)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_LOGOFF_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOGOFF_ANDX_Request_SMB_Parameters
    {
        /// <summary>
        /// The value of this field MUST be 2
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. This value
        /// MUST be set to 0xFF if there are no additional 
        /// SMB command responses in the server response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent,
        /// and the client MUST ignore this field.
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of
        /// the SMB header to the start of the WordCount field in the next SMB
        /// command in this packet. This field is valid only if the AndXCommand
        /// field is not set to 0xFF
        /// </summary>
        public ushort AndXOffset;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_LOGOFF_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOGOFF_ANDX_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_LOGOFF_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOGOFF_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// The value of this field MUST be 2
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The secondary SMB command response in the packet. This value MUST be
        /// set to 0xFF if there are no additional SMB command 
        /// responses in the server response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent
        /// and the client MUST ignore this value when the message is received
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the
        /// SMB header to the start of the WordCount field in the next SMB 
        /// command in this packet. This field is valid only if the AndXCommand 
        /// field is not set to 0xFF
        /// </summary>
        public ushort AndXOffset;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_LOGOFF_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_LOGOFF_ANDX_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.55   	SMB_COM_TREE_CONNECT_ANDX (0x75)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TREE_CONNECT_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Parameters
    {
        /// <summary>
        /// The value of this field MUST be 4
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. This value
        /// MUST be set to 0xFF if there are no additional SMB command
        /// requests in the request packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this request is sent,
        /// and the server MUST ignore this value.
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of
        /// the SMB header to the start of the WordCount field of the next SMB command
        /// request in this packet. This field is valid only if the AndXCommand 
        /// field is not set to 0xFF. If AndXCommand is 0xFF, this field MUST be
        /// ignored by the server.
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// A 16-bit field used to modify the SMB_COM_TREE_CONNECT_ANDX request. 
        /// The client MUST set reserved values to 0, and the server MUST
        /// ignore them.
        /// </summary>
        public ushort Flags;

        /// <summary>
        /// This field MUST be the length, in bytes, of the SMB_Data.Bytes.Password field.
        /// </summary>
        public ushort PasswordLength;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TREE_CONNECT_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Data
    {
        /// <summary>
        /// The value of this field MUST be 3 or greater
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// The value of this field MUST be 3 or greater The SMB_Parameters.
        /// Bytes.PasswordLength MUST be the full length of the Password field. 
        /// If the Password is the null byte, the password length is 1.
        /// </summary>
        public byte[] Password;

        /// <summary>
        /// Padding bytes. If Unicode support has been enabled and
        /// SMB_FLAGS2_UNICODE is set in SMB_Header.Flags2, this field MUST contain 
        /// zero or one null bytes as needed to ensure that the Path 
        /// string is aligned on a 16-bit boundary
        /// </summary>
        public byte[] Pad;

        /// <summary>
        /// A null-terminated string that represents the server and share 
        /// name of the resource to which the client is attempting to connect. This field
        /// MUST be encoded using Universal Naming Convention (UNC) syntax.
        /// If SMB_FLAGS2_UNICODE is set in the Flags2 field of the SMB header of the 
        /// request, the string MUST be a null-terminated array of 16-bit
        /// Unicode characters. Otherwise, the string MUST be a null-terminated array of 
        /// OEM characters. If the string consists of Unicode characters, 
        /// this field MUST be aligned to start on a 2-byte boundary from the start of 
        /// the SMB header. A path in UNC syntax would be represented by a string in the following form:
        /// \\server\share
        /// </summary>
        public byte[] Path;

        /// <summary>
        /// The type of resource that the client intends to access. 
        /// This field MUST be a null-terminated array of OEM characters even if the client
        /// and server have negotiated to use Unicode strings.
        /// </summary>
        public byte[] Service;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TREE_CONNECT_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// The value of this field MUST be 3.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. 
        /// This value MUST be set to 0xFF if there are no additional SMB command 
        /// responses in the server response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent,
        /// and the client MUST ignore this field
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of 
        /// the SMB header to the start of the WordCount field in the 
        /// next SMB command response in this packet. This field is valid 
        /// only if the AndXCommand field is not set to 0xFF. If AndXCommand 
        /// is 0xFF, this field MUST be ignored by the client
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// A 16-bit field. The following OptionalSupport field flags are defined. 
        /// Any combination of the following flags MUST be supported.
        /// All undefined values are considered reserved. The server SHOULD set
        /// them to 0, and the client MUST ignore them
        /// </summary>
        public ushort OptionalSupport;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_TREE_CONNECT_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Data
    {
        /// <summary>
        /// The value of this field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// The type of the shared resource to which the TID is connected.
        /// The Service field MUST be encoded as a null-terminated array of OEM 
        /// characters, even if the client and server have negotiated to
        /// use Unicode strings. The valid values for this field are as follows:
        /// Service String	Description
        /// "A:"	Disk Share
        /// "LPT1:"	Printer Share
        /// "IPC"	Named Pipe
        /// "COMM"	Serial Communications device
        /// </summary>
        public byte[] Service;

        /// <summary>
        /// Padding bytes. If Unicode support has been enabled and SMB_FLAGS2_UNICODE is set in SMB_Header.Flags2,
        /// this field MUST contain zero or one null padding byte as needed to ensure that the NativeFileSystem string
        /// is aligned on a 16-bit boundary.
        /// </summary>
        public byte[] Pad;

        /// <summary>
        /// The name of the file system on the local resource to which 
        /// the returned TID is now connected. If SMB_FLAGS2_UNICODE is set in the Flags2 
        /// field of the SMB header of the response, this value MUST 
        /// be a null-terminated string of Unicode characters. Otherwise, this field MUST 
        /// be a null-terminated string of OEM characters. For resources 
        /// that are not backed by a file system, such as the IPC$ share used for named
        /// pipes, this field MUST be set to the empty string.
        /// </summary>
        public byte[] NativeFileSystem;
    }


    /// <summary>
    /// A 16-bit field used to modify the SMB_COM_TREE_CONNECT_ANDX request. 
    /// The client MUST set reserved values to 0, and the server MUST 
    /// ignore them.
    /// </summary>
    [Flags()]
    public enum TreeConnectAndxFlags : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// If set, the tree connection specified by the SMB_Header.
        /// TID of the request SHOULD be disconnected when the server sends the response.
        /// If this tree disconnect fails, the error SHOULD be ignored
        /// </summary>
        TREE_CONNECT_ANDX_DISCONNECT_TID = 0x0001,

        /// <summary>
        /// Reserved. Must be zero. 
        /// </summary>
        Reserved = 0x0002,
    }

    #endregion

    // 2.2.4.56   	SMB_COM_SECURITY_PACKAGE_ANDX (0x7E)
    // This command was used to negotiate security packages and related information,
    // but is no longer used. Documentation describing the implementation of this 
    // command can be found in [XOPEN-SMB]. Clients SHOULD NOT send requests using 
    // this command code. Servers receiving requests with this command code MUST 
    // return STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd).

    #region 2.2.4.57   	SMB_COM_QUERY_INFORMATION_DISK (0x80)


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_QUERY_INFORMATION_DISK Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_QUERY_INFORMATION_DISK_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this command.
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_QUERY_INFORMATION_DISK Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_QUERY_INFORMATION_DISK_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this command.
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_QUERY_INFORMATION_DISK Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_QUERY_INFORMATION_DISK_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 5.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field is a 16 bit unsigned value that represents the total 
        /// count of logical allocation units available on the volume
        /// </summary>
        public ushort TotalUnits;

        /// <summary>
        /// This field is a 16 bit unsigned value that represents the number
        /// of blocks per allocation unit for the volume
        /// </summary>
        public ushort BlocksPerUnit;

        /// <summary>
        /// This field is a 16 bit unsigned value that represents the size
        /// in bytes of each allocation unit for the volume
        /// </summary>
        public ushort BlockSize;

        /// <summary>
        /// This field is a 16 bit unsigned value that represents the total
        /// number of free allocation units available on the volume
        /// </summary>
        public ushort FreeUnits;

        /// <summary>
        /// This field is a 16 bit unsigned field and is reserved. The client SHOULD ignore this field.
        /// </summary>
        public ushort Reserved;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_QUERY_INFORMATION_DISK Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_QUERY_INFORMATION_DISK_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.58   	SMB_COM_SEARCH (0x81)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_SEARCH Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SEARCH_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 2
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The maximum number of directory entries to return. This value represents
        /// the maximum number of entries across the entirety 
        /// of the search, not just the initial response. 
        /// </summary>
        public ushort MaxCount;

        /// <summary>
        /// An attribute mask used to specify the standard attributes a file 
        /// MUST have in order to match the search. If the value of this field 
        /// is zero, then only normal files are returned. If the Volume Label 
        /// attribute is set, then only the volume label MUST be returned 
        /// (the Volume Label attribute is exclusive). If the Directory, System,
        /// or Hidden attributes are specified, then those entries are 
        /// returned in addition to the normal files. 
        /// </summary>
        public SmbFileAttributes SearchAttributes;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_SEARCH Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SEARCH_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 5 or greater
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04, which indicates that a null-terminated SMB_STRING is to follow.
        /// </summary>
        public byte BufferFormat1;

        /// <summary>
        /// A null-terminated SMB_STRING. This is the full directory path 
        /// (relative to the TID) of the file(s) being sought.
        /// Only the final component of the path MAY contain wildcards. 
        /// This string MAY be the empty string
        /// </summary>
        public byte[] FileName;

        /// <summary>
        /// This field MUST be 0x05, which indicates a variable block is to follow.
        /// </summary>
        public byte BufferFormat2;

        /// <summary>
        /// This field MUST be either 0 or 21. If the value of this field
        /// is zero, then this is an initial search request. 
        /// The server MUST allocate resources to maintain search state
        /// so that subsequent requests MAY be processed.
        /// If the value of this field is 21 then this request MUST be
        /// the continuation of a previous search, and the next 
        /// field MUST contain a ResumeKey previously returned by the server.
        /// </summary>
        public ushort ResumeKeyLength;

        /// <summary>
        /// If the value of the previous field, ResumeKeyLength, is 21
        /// then this field MUST contain a ResumeKey returned by the server in response 
        /// to a previous SMB_COM_SEARCH request. The ResumeKey contains
        /// data used by both the client and the server to maintain the state of the 
        /// search. The structure of the ResumeKey is described below.
        /// </summary>
        [Size("ResumeKeyLength / 21")]
        public SMB_Resume_Key[] ResumeKey;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_SEARCH Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SEARCH_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The number of directory entries returned in this response message.
        /// This value MUST be less than or equal to the value 
        /// of MaxCount in the initial request.
        /// </summary>
        public ushort Count;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_SEARCH Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SEARCH_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 3
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x05, which indicates that a variable-size block is to follow. 
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// The size, in bytes, of the DirectoryInformationData array, which follows. This field MUST be equal
        /// to 43 times the value of SMB_Parameters. Count.
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// An array of zero or more SMB_Directory_Information records. 
        /// The structure and contents of these records is given below. 
        /// Note that the SMB_Directory_Information record structure is
        /// a fixed 43 bytes in length.
        /// </summary>
        [Size("DataLength/43")]
        public SMB_Directory_Information[] DirectoryInformationData;
    }


    /// <summary>
    /// The structure of the ResumeKey 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_Resume_Key
    {
        /// <summary>
        /// This field is reserved and MUST NOT be modified by the client.
        /// Older documentation is contradictory as to whether this field is 
        /// reserved for client side or server side use. New server
        /// implementations SHOULD avoid using or modifying the content of this field.
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// This field is maintained by the server and MUST NOT be modified 
        /// by the client. The contents of this field are server specific. 
        /// </summary>
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] ServerState;

        /// <summary>
        /// This field MAY be used by the client to maintain state across
        /// a series of SMB_COM_SEARCH calls. The value provided by the client
        /// MUST be returned in each ResumeKey provided in the response. 
        /// The contents of this field are client specific.
        /// </summary>
        [StaticSize(4, StaticSizeMode.Elements)]
        public byte[] ClientState;
    }


    /// <summary>
    /// Windows NT server makes use of the ServerState field as follows
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ServerState
    {
        /// <summary>
        /// This is the name portion of the 8.3 format file name. The name left justified and space padded. 
        /// </summary>
        [StaticSize(8, StaticSizeMode.Elements)]
        public byte[] FileName;

        /// <summary>
        /// This is the file extension of the 8.3 format file name. It is left justified and space padded.
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public byte[] FileExt;

        /// <summary>
        /// This is a one byte search identifier, used by the server to uniquely
        /// identify the search operation. The use of a one-byte field implies
        /// that the NT server can manage a maximum of 256 concurrent searches per SMB session
        /// </summary>
        public byte SearchID;

        /// <summary>
        /// A server-specific index used to continue the search at the correct
        /// place in the remote directory
        /// </summary>
        public uint FileIndex;
    }


    /// <summary>
    /// the SMB_Directory_Information record structure 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_Directory_Information
    {
        /// <summary>
        /// While each DirectoryInformationData entry will have
        /// a ResumeKey field, the client MUST use only the ResumeKey value from the
        /// last DirectoryInformationData entry when continuing the search 
        /// with a subsequent SMB_COM_SEARCH command.
        /// </summary>
        public SMB_Resume_Key ResumeKey;

        /// <summary>
        /// These are the file system attributes of the file.
        /// </summary>
        public byte FileAttributes;

        /// <summary>
        /// The time when the file was last modified. The SMB_TIME structure 
        /// contains a set of bit fields indicating hours, minutes, and seconds 
        /// (with a 2 second resolution).
        /// </summary>
        public SmbTime LastWriteTime;

        /// <summary>
        /// The date when the file was last modified. The SMB_DATE structure 
        /// contains a set of bit fields indicating the year, month, and date
        /// </summary>
        public SmbDate LastWriteDate;

        /// <summary>
        /// The size of the file, in bytes. If the file is larger than (232 - 1)
        /// bytes in size then the server SHOULD return the least 
        /// significant 32 bits of the file size
        /// </summary>
        public uint FileSize;

        /// <summary>
        /// The null-terminated 8.3 name format file name. The file name and 
        /// extension, including the '.' delimiter are left justified in the field.
        /// The character string MUST be padded with " " (space) characters,
        /// as necessary, to reach 12 bytes in length. The final byte of the field
        /// MUST contain the NUL
        /// </summary>
        [StaticSize(13, StaticSizeMode.Elements)]
        public byte[] FileName;
    }

    #endregion

    #region 2.2.4.59   	SMB_COM_FIND (0x82)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_FIND Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 2
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The maximum number of directory entries to return. This value
        /// represents the maximum number of entries across
        /// the entirety of the search, not just the initial response. 
        /// </summary>
        public ushort MaxCount;

        /// <summary>
        /// An attribute mask used to specify the standard attributes a
        /// file MUST have in order to match the search. If the value of this field 
        /// is zero, then only normal files MUST be returned. If the Volume 
        /// Label attribute is set, then only the volume label MUST be returned 
        /// (the Volume Label attribute is exclusive). If the Directory, System,
        /// or Hidden attributes are specified, then those entries MUST be
        /// returned in addition to the normal files. 
        /// </summary>
        public SmbFileAttributes SearchAttributes;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_FIND Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 5 or greater
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04, which indicates that a NUL-terminated ASCII string is to follow.
        /// </summary>
        public byte BufferFormat1;

        /// <summary>
        /// A null-terminated character string. This is the full directory
        /// path (relative to the TID) of the file(s) being sought. 
        /// Only the final component of the path MAY contain wildcards.
        /// This string MAY be the empty string
        /// </summary>
        public byte[] FileName;

        /// <summary>
        /// This field MUST be 0x05, which indicates a variable block is to follow.
        /// </summary>
        public byte BufferFormat2;

        /// <summary>
        /// This field MUST be either 0 or 21. If the value of this field 
        /// is zero, then this is an initial search request.
        /// The server MUST allocate resources to maintain search state so
        /// that subsequent requests can be processed. 
        /// If the value of this field is 21 then this request MUST be the
        /// continuation of a previous search, and the next field MUST contain
        /// a ResumeKey previously returned by the server
        /// </summary>
        public ushort ResumeKeyLength;

        /// <summary>
        /// If the value of the ResumeKeyLength field is 21 then this
        /// field MUST contain a ResumeKey returned by the server in response to a
        /// previous SMB_COM_SEARCH request. The ResumeKey contains
        /// data used by both the client and the server to maintain the state of the search.
        /// The structure of the ResumeKey is described below.
        /// </summary>
        [Size("ResumeKeyLength / 21")]
        public SMB_Resume_Key[] ResumeKey;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_FIND Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The number of directory entries returned in this
        /// response message. This value MUST be less than or
        /// equal to the value of MaxCount in the initial request.
        /// </summary>
        public ushort Count;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_FIND Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 3.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x05, which indicates that a variable-size block is to follow.
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// The size, in bytes, of the DirectoryInformationData array,
        /// which follows. This field MUST be equal to 43 times the value of
        /// SMB_Parameters.Words.Count.
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// An array of zero or more SMB_Directory_Information records.
        /// The structure and contents of these records is given below.
        /// Note that the SMB_Directory_Information record structure is
        /// a fixed 43 bytes in length.
        /// </summary>
        [Size("DataLength/43")]
        public SMB_Directory_Information[] DirectoryInformationData;
    }

    #endregion

    #region 2.2.4.60   	SMB_COM_FIND_UNIQUE (0x83)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_FIND_UNIQUE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_UNIQUE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 2.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The maximum number of directory entries to return
        /// </summary>
        public ushort MaxCount;

        /// <summary>
        /// An attribute mask used to specify the standard attributes a 
        /// file MUST have in order to match the search. If the value of this field 
        /// is 0, then only normal files MUST be returned. If the Volume
        /// Label attribute is set, then the server MUST only return the volume
        /// label MUST. If the Directory, System, or Hidden attributes
        /// are specified, then those entries MUST be returned in addition to the normal
        /// files
        /// </summary>
        public SmbFileAttributes SearchAttributes;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_FIND_UNIQUE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_UNIQUE_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 5 or greater
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04, which indicates that a NUL-terminated ASCII string is to follow.
        /// </summary>
        public byte BufferFormat1;

        /// <summary>
        /// A NUL-terminated SMB_STRING. This is the full directory
        /// path (relative to the TID) of the file(s) being sought.
        /// Only the final component of the path MAY contain wildcards.
        /// This string MAY be the empty string
        /// </summary>
        public byte[] FileName;

        /// <summary>
        /// This field MUST be 0x05, which indicates a variable block is to follow.
        /// </summary>
        public byte BufferFormat2;

        /// <summary>
        /// USHORT  This field MUST be 0. No Resume Key is permitted in
        /// the SMB_COM_FIND_UNIQUE request. If the server receives an SMB_COM_FIND_UNIQUE
        /// request with a non-zero ResumeKeyLength, it MUST ignore this field
        /// </summary>
        public ushort ResumeKeyLength;

        /// <summary>
        /// No Resume Key is permitted in the SMB_COM_FIND_UNIQUE request.
        /// </summary>
        [Size("ResumeKeyLength / 21")]
        public SMB_Resume_Key[] ResumeKey;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_FIND_UNIQUE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_UNIQUE_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The number of directory entries returned in this response
        /// message. This value MUST be less than or equal to the value 
        /// of MaxCount in the initial request.
        /// </summary>
        public ushort Count;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_FIND_UNIQUE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_UNIQUE_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 3
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x05, which indicates that a variable-size block is to follow.
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// The size in bytes of the DirectoryInformationData array that follows.
        /// This field MUST be equal to 43 times the value of 
        /// SMB_Parameters.Words.Count.
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// An array of zero or more SMB_Directory_Information records. 
        /// The structure and contents of these records is given below. 
        /// Note that the SMB_Directory_Information record structure is
        /// a fixed 43 bytes in length.
        /// </summary>
        [Size("DataLength/43")]
        public SMB_Directory_Information[] DirectoryInformationData;
    }

    #endregion

    #region 2.2.4.61   	SMB_COM_FIND_CLOSE (0x84)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_FIND_CLOSE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_CLOSE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 2
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field has no meaning in this context. It SHOULD be set 
        /// to zero by the client and MUST be ignored by the server. 
        /// </summary>
        public ushort MaxCount;

        /// <summary>
        /// This field has no meaning in this context. It SHOULD be set 
        /// to zero by the client and MUST be ignored by the server
        /// </summary>
        public ushort SearchAttributes;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_FIND_CLOSE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_CLOSE_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 26
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04, which indicates that a NUL-terminated ASCII string is to follow.
        /// </summary>
        public byte BufferFormat1;

        /// <summary>
        /// A NUL-terminated SMB_STRING. This MUST be the empty string. 
        /// </summary>
        [Size("ByteCount - 25")]
        public byte[] FileName;

        /// <summary>
        /// This field MUST be 0x05, which indicates a variable block is to follow.
        /// </summary>
        public byte BufferFormat2;

        /// <summary>
        /// This field MUST be 21
        /// </summary>
        public ushort ResumeKeyLength;

        /// <summary>
        /// This MUST be the last ResumeKey returned by the server 
        /// in the search being closed. See SMB_COM_FIND for a description of the
        /// SMB_Resume_Key data structure.
        /// </summary>
        public SMB_Resume_Key ResumeKey;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_FIND_CLOSE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_CLOSE_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The server SHOULD set this field to zero, and the client MUST 
        /// ignore the value of this field. No entries are returned in the response.
        /// </summary>
        public ushort Count;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_FIND_CLOSE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_FIND_CLOSE_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 3.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x05, which indicates that a variable-size block is to follow.
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// This field MUST be zero. No DirectoryInformationData records are returned.
        /// </summary>
        public ushort DataLength;
    }

    #endregion

    #region 2.2.4.62   	SMB_COM_NT_TRANSACT (0xA0)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NT_TRANSACT Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_TRANSACT_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be greater than or equal to 19
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// Maximum number of setup bytes that the client will accept in the 
        /// transaction reply. The server MUST NOT return more than this number 
        /// of setup bytes.
        /// </summary>
        public byte MaxSetupCount;

        /// <summary>
        /// Two padding bytes. This field MUST be zero. This field is used to
        /// align the next field to a 32-bit boundary.
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// The total number of SMB_COM_NT_TRANSACT parameter bytes to be 
        /// sent in this transaction request. This value MAY be reduced in any or all
        /// subsequent SMB_COM_NT_TRANSACT_SECONDARY requests that are part 
        /// of the same transaction. This value represents transaction parameter 
        /// bytes, not SMB parameter words. Transaction parameter bytes are 
        /// carried within in the SMB_Data block of the SMB_COM_NT_TRANSACT request
        /// or subsequent SMB_COM_NT_TRANSACT_SECONDARY requests.
        /// </summary>
        public uint TotalParameterCount;

        /// <summary>
        /// The total number of SMB_COM_NT_TRANSACT data bytes to be sent
        /// in this transaction request. This value MAY be reduced in any or all 
        /// subsequent SMB_COM_NT_TRANSACT_SECONDARY requests that are part
        /// of the same transaction. This value represents transaction data bytes,
        /// not SMB data bytes
        /// </summary>
        public uint TotalDataCount;

        /// <summary>
        /// The maximum number of parameter bytes that the client will accept
        /// in the transaction reply. The server MUST NOT return more than 
        /// this number of parameter bytes.
        /// </summary>
        public uint MaxParameterCount;

        /// <summary>
        /// ULONG The maximum number of data bytes that the client will accept
        /// in the transaction reply. The server MUST NOT return more than
        /// this number of data bytes.
        /// </summary>
        public uint MaxDataCount;

        /// <summary>
        /// The number of transaction parameter bytes being sent in this 
        /// SMB message. If the transaction fits within a single SMB_COM_NT_TRANSACT
        /// request, then this value MUST be equal to TotalParameterCount.
        /// Otherwise, the sum of the ParameterCount values in the primary and 
        /// secondary transaction request messages MUST be equal to the smallest
        /// TotalParameterCount value reported to the server. If the value
        /// of this field is less than the value of TotalParameterCount, then 
        /// at least one SMB_COM_NT_TRANSACT_SECONDARY message MUST be used to 
        /// transfer the remaining parameter bytes.
        /// The ParameterCount field MUST be used to determine the number of
        /// transaction parameter bytes contained within the SMB_COM_NT_TRANSACT
        /// message
        /// </summary>
        public uint ParameterCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the
        /// transaction parameter bytes. This MUST be the number of bytes from 
        /// the start of the SMB message to the start of the SMB_Data.Bytes.
        /// Parameters field. Server implementations MUST use this value to locate
        /// the transaction parameter block within the SMB message
        /// </summary>
        public uint ParameterOffset;

        /// <summary>
        /// The number of transaction data bytes being sent in this SMB message. 
        /// If the transaction fits within a single SMB_COM_NT_TRANSACT request, 
        /// then this value MUST be equal to TotalDataCount. Otherwise, the sum
        /// of the DataCount values in the primary and secondary transaction
        /// request messages MUST be equal to the smallest TotalDataCount value
        /// reported to the server. If the value of this field is less than the
        /// value of TotalDataCount, then at least one SMB_COM_NT_TRANSACT_SECONDARY
        /// message MUST be used to transfer the remaining data bytes.
        /// </summary>
        public uint DataCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the
        /// transaction data bytes. This MUST be the number of bytes from the start
        /// of the SMB message to the start of the SMB_Data.Bytes.Data field.
        /// Server implementations MUST use this value to locate the transaction
        /// data block within the SMB message
        /// </summary>
        public uint DataOffset;

        /// <summary>
        /// The number of setup words that are included in the transaction request.
        /// </summary>
        public byte SetupCount;

        /// <summary>
        /// The transaction subcommand code. The subcommand code is used to 
        /// identify the operation to be performed by the server.
        /// </summary>
        public NtTransSubCommand Function;

        /// <summary>
        /// An array of two-byte words that provide transaction context to
        /// the server. The size and content of the array are specific to the 
        /// individual subcommands.
        /// </summary>
        [Size("SetupCount")]
        public ushort[] Setup;
    }

    /// <summary>
    /// the Setup struct in NT_TRANSACT_NOTIFY_CHANGE request
    /// </summary>
    public struct NT_TRANSACT_NOTIFY_SETUP
    {
        /// <summary>
        /// specify the types of operations to monitor.
        /// </summary>
        public CompletionFilter filter;

        /// <summary>
        /// The FID of the directory to monitor.
        /// </summary>
        public ushort Fid;

        /// <summary>
        /// If all subdirectories are to be watched, then set this to TRUE; otherwise, FALSE.
        /// </summary>
        public byte WatchTree;

        /// <summary>
        /// Reserved. This value MUST be 0.
        /// </summary>
        public byte Reserved;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_NT_TRANSACT Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_TRANSACT_Request_SMB_Data
    {
        /// <summary>
        /// The number of bytes in the SMB_Data.Bytes array, which follows.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 32 bit boundary.
        /// </summary>
        public byte[] Pad1;

        /// <summary>
        /// Transaction parameter bytes. See the individual SMB_COM_NT_TRANSACT subcommand descriptions for
        /// information on parameters sent for each subcommand.
        /// </summary>
        public byte[] NT_Trans_Parameters;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 32 bit boundary.
        /// </summary>
        public byte[] Pad2;

        /// <summary>
        /// Transaction data bytes. See the individual SMB_COM_NT_TRANSACT 
        /// subcommand descriptions for information on data sent for each subcommand.
        /// </summary>
        public byte[] NT_Trans_Data;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NT_TRANSACT_ErrorResponse_SMB_Parameters
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_TRANSACT_InterimResponse_SMB_Parameters
    {
        /// <summary>
        /// WordCount Fields must be zero
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_NT_TRANSACT_ErrorResponse_SMB_Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_TRANSACT_InterimResponse_SMB_Data
    {
        /// <summary>
        /// ByteCount Fields must be zero
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters
    {
        /// <summary>
        /// The value of Words.SetupCount plus 18. This value represents the total
        /// number of SMB parameter words and MUST be greater 
        /// than or equal to 18
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// Reserved. This field MUST be zero in the server response. The client
        /// MUST ignore the contents of this field.
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public byte[] Reserved1;

        /// <summary>
        /// The total number of SMB_COM_NT_TRANSACT parameter bytes to be sent
        /// in this transaction response. This value MAY be reduced in any
        /// or all subsequent SMB_COM_NT_TRANSACT responses that are part of
        /// the same transaction. This value represents transaction parameter
        /// bytes, not SMB parameter words. Transaction parameter bytes are 
        /// carried within in the SMB_data block
        /// </summary>
        public uint TotalParameterCount;

        /// <summary>
        /// The total number of SMB_COM_NT_TRANSACT data bytes to be sent in
        /// this transaction response. This value MAY be reduced in any or all
        /// subsequent SMB_COM_NT_TRANSACT responses that are part of the same 
        /// transaction. This value represents transaction data bytes, 
        /// not SMB data bytes
        /// </summary>
        public uint TotalDataCount;

        /// <summary>
        /// The number of transaction parameter bytes being sent in this SMB
        /// message. If the transaction fits within a single SMB_COM_NT_TRANSACT
        /// response, then this value MUST be equal to TotalParameterCount. 
        /// Otherwise, the sum of the ParameterCount values in the transaction 
        /// response messages MUST be equal to the smallest TotalParameterCount
        /// value reported by the server.
        /// The ParameterCount field MUST be used to determine the number of
        /// transaction parameter bytes contained within the SMB message
        /// </summary>
        public uint ParameterCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the
        /// transaction parameter bytes. This MUST be the number of bytes from
        /// the start of the SMB message to the start of the SMB_Data.Bytes.
        /// Parameters field. Server implementations MUST use this value to 
        /// locate the transaction parameter block within the SMB message
        /// </summary>
        public uint ParameterOffset;

        /// <summary>
        /// The offset, relative to all of the transaction parameter bytes
        /// in this transaction response, at which this block of parameter bytes MUST 
        /// be placed. This value can be used by the client to correctly
        /// reassemble the transaction parameters even if the SMB response messages are
        /// received out of order.
        /// </summary>
        public uint ParameterDisplacement;

        /// <summary>
        /// The number of transaction data bytes being sent in this SMB message.
        /// If the transaction fits within a single SMB_COM_NT_TRANSACT response,
        /// then this value MUST be equal to TotalDataCount. Otherwise, the sum 
        /// of the DataCount values in the transaction response messages MUST be
        /// equal to the smallest TotalDataCount value reported by the server.
        /// </summary>
        public uint DataCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the 
        /// transaction data bytes. This MUST be the number of bytes from the start 
        /// of the SMB message to the start of the SMB_Data.Bytes.Data field. 
        /// Server implementations MUST use this value to locate the transaction 
        /// data block within the SMB message.
        /// </summary>
        public uint DataOffset;

        /// <summary>
        /// The offset, relative to all of the transaction data bytes in 
        /// this transaction response, at which 
        /// this block of data bytes MUST be placed. This value can be
        /// used by the client to correctly reassemble the transaction
        /// data even if the SMB response messages are received out of order. 
        /// </summary>
        public uint DataDisplacement;

        /// <summary>
        /// The number of Setup words that are included in the transaction response.
        /// </summary>
        public byte SetupCount;

        /// <summary>
        /// An array of two-byte words that provide transaction results from the server.
        /// The size and content of the array are specific to individual
        /// subcommand.
        /// </summary>
        [Size("SetupCount")]
        public ushort[] Setup;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 0
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 32 bit boundary.
        /// </summary>
        public byte[] Pad1;

        /// <summary>
        /// Transaction parameter bytes. See the individual SMB_COM_NT_TRANSACT 
        /// subcommand descriptions for information on 
        /// parameters returned by the server for each subcommand.
        /// </summary>
        public byte[] Parameters;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 32 bit boundary.
        /// </summary>
        public byte[] Pad2;

        /// <summary>
        /// Transaction data bytes. See the individual SMB_COM_NT_TRANSACT
        /// subcommand descriptions for information on data returned by the server
        /// for each subcommand.
        /// </summary>
        public byte[] Data;
    }

    #endregion

    #region 2.2.4.63   	SMB_COM_NT_TRANSACT_SECONDARY (0xA1)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NT_TRANSACT_SECONDARY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Parameters
    {
        /// <summary>
        /// This value represents the total number of SMB parameter words and MUST be 18.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// Reserved. Used to align the following fields to a 32-bit boundary.
        /// This field MUST contain null bytes in the server response. 
        /// The client MUST ignore the contents of this field.
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public byte[] Reserved1;

        /// <summary>
        /// The total number of transaction parameter bytes to be sent to 
        /// the server over the course of this transaction.
        /// This value MAY be less than or equal to the TotalParameterCount 
        /// in preceding request messages that are part of the
        /// same transaction. This value represents transaction parameter 
        /// bytes, not SMB parameter words
        /// </summary>
        public uint TotalParameterCount;

        /// <summary>
        /// The total number of transaction data bytes to be sent to 
        /// the server over the course of this transaction. This value MAY be less
        /// than or equal to the TotalDataCount in preceding request
        /// messages that are part of the same transaction. This value represents 
        /// transaction data bytes, not SMB data bytes
        /// </summary>
        public uint TotalDataCount;

        /// <summary>
        /// The number of transaction parameter bytes being sent in the SMB 
        /// message. This value MUST be less than TotalParameterCount. The sum of 
        /// the ParameterCount values across all of the request messages in
        /// a transaction MUST be equal to the TotalParameterCount reported in the
        /// last request message of the transaction
        /// </summary>
        public uint ParameterCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the 
        /// transaction parameter bytes contained in this SMB message. 
        /// This MUST be the number of bytes from the start of the SMB message
        /// to the start of the SMB_Data.Bytes.Parameters field. Server 
        /// implementations MUST use this value to locate the transaction
        /// parameter block within the SMB message.
        /// </summary>
        public uint ParameterOffset;

        /// <summary>
        /// The offset, relative to all of the transaction parameter bytes
        /// sent to the server in this transaction, at which this block of parameter
        /// bytes MUST be placed. This value can be used by the server to 
        /// correctly reassemble the transaction parameters even if the SMB request 
        /// messages are received out of order
        /// </summary>
        public uint ParameterDisplacement;

        /// <summary>
        /// The number of transaction data bytes being sent in this SMB message.
        /// This value MUST be less than the value of TotalDataCount. The sum of
        /// the DataCount values across all of the request messages in a transaction
        /// MUST be equal to the smallest TotalDataCount value reported to 
        /// the server
        /// </summary>
        public uint DataCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the 
        /// transaction data bytes contained in this SMB message. This MUST be the
        /// number of bytes from the start of the SMB message to the start of
        /// the SMB_Data.Bytes.Data field. Server implementations MUST use this 
        /// value to locate the transaction data block within the SMB message.
        /// </summary>
        public uint DataOffset;

        /// <summary>
        /// The offset, relative to all of the transaction data bytes sent to 
        /// the server in this transaction, at which this block of parameter bytes
        /// MUST be placed. This value can be used by the server to correctly 
        /// reassemble the transaction data block even if the SMB request messages
        /// are received out of order.
        /// </summary>
        public uint DataDisplacement;

        /// <summary>
        /// Reserved. MUST be zero. The server MUST ignore the contents of this field.
        /// </summary>
        public byte Reserved2;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_NT_TRANSACT_SECONDARY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Data
    {
        /// <summary>
        /// The number of bytes in the SMB_Data.Bytes array, which follows.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 32 bit boundary.
        /// </summary>
        public byte[] Pad1;

        /// <summary>
        /// Transaction parameter bytes. 
        /// </summary>
        public byte[] NT_Trans_Parameters;

        /// <summary>
        /// An array of padding bytes, used to align the next field to a 32 bit boundary.
        /// </summary>
        public byte[] Pad2;

        /// <summary>
        /// Array of UCHAR Transaction data bytes
        /// </summary>
        public byte[] NT_Trans_Data;
    }

    // struct  SMB_COM_NT_TRANSACT_SECONDARY_Response
    // There is no response message defined for the SMB_COM_NT_TRANSACT_SECONDARY command. 

    #endregion

    #region 2.2.4.64   	SMB_COM_NT_CREATE_ANDX (0xA2)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NT_CREATE_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_CREATE_ANDX_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 24
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. This value
        /// MUST be set to 0xFF if there are no additional SMB command 
        /// responses in the server response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent,
        /// and the client MUST ignore this field.
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the
        /// SMB header to the start of the WordCount field in the next SMB command
        /// in this packet. This field is valid only if the AndXCommand field 
        /// is not set to 0xFF. If AndXCommand is 0xFF, this field MUST be ignored
        /// by the server
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// An unused value that SHOULD be set to 0 when sending this message. 
        /// The server MUST ignore this field when receiving this message
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// This field MUST be the length of the FileName field (see below) in bytes. 
        /// </summary>
        public ushort NameLength;

        /// <summary>
        /// A 32-bit field containing a set of flags that modify the client request.
        /// Unused bit fields SHOULD be set to 0 by the client when sending
        /// a message and MUST be ignored when received by the server
        /// </summary>
        public uint Flags;

        /// <summary>
        /// If nonzero, this value is the File ID of an opened root directory, 
        /// and the FileName field MUST be handled as relative to the directory 
        /// specified by this RootDirectoryFID. If this value is zero the FileName 
        /// field MUST be handled as relative to the root of the share (the TID). 
        /// The RootDirectoryFID MUST have been acquired in a previous message exchange.
        /// </summary>
        public uint RootDirectoryFID;

        /// <summary>
        /// A 32-bit field of flags that indicate standard, specific, and generic 
        /// access rights. These rights are used in access-control entries (ACEs)
        /// and are the primary means of specifying the requested or granted access 
        /// to an object. If this value is 0, it represents a request to query 
        /// the attributes without access the file
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        /// The client MUST set this value to the initial allocation size of the
        /// file in bytes. The server MUST ignore this field if this request is
        /// to open an existing file. This field MUST be used only if the file is
        /// created or overwritten. The value MUST be set to 0 in all other 
        /// cases. This does not apply to directory related requests. This is the
        /// number of bytes to be allocated represented as a 64 bit integer 
        /// value
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// A 32-bit field containing encoded file attribute values and file 
        /// access behavior flag values. The attribute and flag value names are 
        /// for reference purposes only. If ATTR_NORMAL (see below) is set 
        /// as the requested attribute value, it MUST be the only attribute value 
        /// set in order to have the desired effect. Including any other 
        /// attribute value causes the ATTR_NORMAL value to be ignored. Any combination 
        /// of the flag values (see below) is acceptable. 
        /// </summary>
        public uint ExtFileAttributes;

        /// <summary>
        /// A 32-bit field that specifies how the file SHOULD be shared
        /// with other processes. The names in the table below are provided for reference
        /// use only. If ShareAccess values of FILE_SHARE_READ, FILE_SHARE_WRITE, 
        /// or FILE_SHARE_DELETE are set for a printer file or a named pipe, 
        /// the server SHOULD ignore these values. The value MUST be 
        /// FILE_SHARE_NONE or some combination of the other values
        /// </summary>
        public uint ShareAccess;

        /// <summary>
        /// A 32-bit value that represents the action to take if the file 
        /// already exists or if the file is a new file and does not already exist
        /// </summary>
        public uint CreateDisposition;

        /// <summary>
        /// A 32-bit field containing flag options to use if creating the file or
        /// directory. This field MUST be set to 0 or a combination of the 
        /// following possible values. Unused bit fields SHOULD be set to 0 by
        /// the client when sending a request and SHOULD be ignored when received
        /// by the server. Below is a list of the valid values and their associated
        /// behavior. Windows server implementations SHOULD reserve all bits
        /// not specified in the following definitions. 
        /// </summary>
        public uint CreateOptions;

        /// <summary>
        /// A value that indicates what security context the server SHOULD use
        /// when executing the command on behalf of the client. Value names 
        /// are provided for convenience only
        /// </summary>
        public uint ImpersonationLevel;

        /// <summary>
        /// A 32-bit field containing a set of options that specify the
        /// security tracking mode. These options specify whether the server is to 
        /// be given a snapshot of the client's security context (called
        /// static tracking) or is to be continually updated to track changes to 
        /// the client's security context (called dynamic tracking). When 
        /// bit 0 of the SecurityFlags field is set to FALSE, static tracking is 
        /// requested. When bit 0 the SecurityFlags field is set to TRUE,
        /// dynamic tracking is requested. Unused bit fields SHOULD be set to 0
        /// by the client when sending a request and MUST be ignored when
        /// received by the server. This field MUST be set to 0 or a combination
        /// of the following possible values. Value names are provided for
        /// convenience only. Supported values are
        /// </summary>
        public byte SecurityFlags;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_NT_CREATE_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_CREATE_ANDX_Request_SMB_Data
    {
        /// <summary>
        /// The length in bytes of the remaining SMB_Data. If SMB_FLAGS2_UNICODE
        /// is set in the Flags2 field of the SMB header of the request,
        /// this field has a minimum value of 3. If SMB_FLAGS2_UNICODE is not set,
        /// this field has a minimum value of 2. This field MUST be the 
        /// total length of the Name field plus any padding added for alignment
        /// </summary>
        public ushort ByteCount;


        /// <summary>
        /// If the FileName string consists of Unicode characters, this field MUST be 
        /// aligned to start on a 2-byte boundary from the start of the SMB Header.
        /// </summary>
        public byte[] Pad;


        /// <summary>
        /// A string that represents the fully qualified name of the file 
        /// relative to the supplied TID to create or truncate on the server. 
        /// If SMB_FLAGS2_UNICODE is set in the Flags2 field of the SMB header
        /// of the request, the FileName string MUST be a null-terminated 
        /// array of 16-bit Unicode characters. Otherwise, the FileName string
        /// MUST be a null-terminated array of extended ASCII (OEM) characters. 
        /// If the FileName string consists of Unicode characters, this field
        /// MUST be aligned to start on a 2-byte boundary from the start of the
        /// SMB header. 
        /// </summary>
        [Size("ByteCount")]
        public byte[] FileName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NT_CREATE_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 26
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet.
        /// This value MUST be set to 0xFF if there are no additional SMB command responses
        /// in the server response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent,
        /// and the client MUST ignore this field
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the 
        /// SMB header to the start of the WordCount field in the next SMB
        /// command response in this packet. This field is valid only if the
        /// AndXCommand field is not set to 0xFF. If AndXCommand is 0xFF, 
        /// this field MUST be ignored by the client
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// The oplock level granted to the client process
        /// </summary>
        public OplockLevelValue OplockLevel;

        /// <summary>
        /// A FID representing the file or directory that was created or opened
        /// </summary>
        public ushort FID;

        /// <summary>
        /// A 32-bit value that represents the action to take if the file already exists or if the file is a new
        /// file and does not already exist.
        /// </summary>
        public NtTransactCreateDisposition CreateDisposition;

        /// <summary>
        /// A 64 bit integer value representing the time the file was created. 
        /// The time value is a signed 64 bit integer representing either 
        /// an absolute time or a time interval. Times are specified in units 
        /// of 100ns. A positive value expresses an absolute time, where the
        /// base time (the 64- bit integer with value 0) is the beginning of
        /// the year 1601 AD in the Gregorian calendar. A negative value expresses
        /// a time interval relative to some base time, usually the current time.
        /// </summary>
        public FileTime CreateTime;

        /// <summary>
        /// The time the file was last accessed encoded in the same format as CreateTime (see above).
        /// </summary>
        public FileTime LastAccessTime;

        /// <summary>
        /// The time the file was last written, encoded in the same format as CreateTime (see above).
        /// </summary>
        public FileTime LastWriteTime;

        /// <summary>
        /// The time the file was last changed, encoded in the same format as CreateTime (see above).
        /// </summary>
        public FileTime LastChangeTime;

        /// <summary>
        /// A 32 bit value composed of encoded file attribute values and file access behavior flag values. 
        /// See Request.SMB_Parameters.ExtFileAttributes above for the encoding.
        /// This value provides the attributes the server assigned
        /// to the file or directory as a result of the command.
        /// </summary>
        public SMB_EXT_FILE_ATTR ExtFileAttributes;

        /// <summary>
        /// The number of bytes allocated to the file by the server. 
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// The end of file offset value. 
        /// </summary>
        public ulong EndOfFile;

        /// <summary>
        /// The file type.
        /// </summary>
        public FileTypeValue ResourceType;

        /// <summary>
        /// A 16-bit field that contains the state of the named pipe
        /// if the FID represents a named pipe instance. This value MUST be 
        /// any combination of the following bit values. Unused bit fields
        /// SHOULD be set to 0 by the server when sending a response and MUST 
        /// be ignored when received by the client
        /// </summary>
        public SMB_NMPIPE_STATUS NMPipeStatus;

        /// <summary>
        /// If the returned FID represents a directory then the server MUST
        /// set this value to a non-zero value. If the FID is not a directory 
        /// then the server MUST set this value to 0 (FALSE).
        /// </summary>
        public byte Directory;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_NT_CREATE_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_CREATE_ANDX_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// The new oplock level.
    /// </summary>
    public enum NewOplockLevelValue : byte
    {
        /// <summary>
        /// No oplock granted.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// the client possesses a Level II OpLock.
        /// </summary>
        Level_II = 0x01,
    }


    /// <summary>
    /// The oplock level.
    /// </summary>
    public enum OplockLevelValue : byte
    {
        /// <summary>
        /// No oplock granted.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Exclusive oplock granted.
        /// </summary>
        Exclusive = 0x01,

        /// <summary>
        /// Batch oplock granted.
        /// </summary>
        Batch = 0x02,

        /// <summary>
        /// Level II oplock granted.
        /// </summary>
        LevelII = 0x03,
    }


    /// <summary>
    /// The file type. This field MUST be interpreted as follows.
    /// </summary>
    public enum FileTypeValue : ushort
    {
        /// <summary>
        /// File or Directory.
        /// </summary>
        FileTypeDisk = 0x0000,

        /// <summary>
        /// Byte mode named pipe
        /// </summary>
        FileTypeByteModePipe = 0x0001,

        /// <summary>
        /// Message mode named pipe
        /// </summary>
        FileTypeMessageModePipe = 0x0002,

        /// <summary>
        /// Printer device
        /// </summary>
        FileTypePrinter = 0x0003,

        /// <summary>
        /// Unknown file type.
        /// </summary>
        FileTypeUnknown = 0xFFFF,
    }

    #endregion

    #region 2.2.4.65   	SMB_COM_NT_CANCEL (0xA4)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NT_CANCEL Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_CANCEL_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this request
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_NT_CANCEL Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_CANCEL_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data are sent by this request
        /// </summary>
        public ushort ByteCount;
    }

    // struct SMB_COM_NT_CANCEL_Response
    // There is no response message defined for the SMB_COM_NT_CANCEL command. 

    #endregion

    #region 2.2.4.66   	SMB_COM_NT_RENAME (0xA5)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NT_RENAME Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_RENAME_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0x04.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field indicates the attributes that the target file(s) MUST have. If the attribute is 0x0000,
        /// then only normal files are renamed or linked. If the system file or hidden attributes are specified,
        /// then the rename is inclusive of both special types.
        /// </summary>
        public SmbFileAttributes SearchAttributes;

        /// <summary>
        /// This field MUST be one of the three values shown in the following table.
        /// </summary>
        public NtRenameInformationLevel InformationLevel;

        /// <summary>
        /// This field SHOULD be set to 0x00000000 by the client and MUST be ignored by the server.
        /// </summary>
        public uint Reserved;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_NT_RENAME Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_RENAME_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 0x0004.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04.
        /// </summary>
        public byte BufferFormat1;

        /// <summary>
        /// A null-terminated string containing the full path name of the file to be manipulated.
        /// Wildcards are not supported.
        /// </summary>
        public byte[] OldFileName;

        /// <summary>
        /// This field MUST be 0x04.
        /// </summary>
        public byte BufferFormat2;

        /// <summary>
        /// A null-terminated string containing the new full path name to be assigned to the file provided in
        /// OldFileName or the full path into which the file is to be moved.
        /// </summary>
        public byte[] NewFileName;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NT_RENAME Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_RENAME_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this Response
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_NT_RENAME Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_RENAME_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data are sent by this Response
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.67   	SMB_COM_OPEN_PRINT_FILE (0xC0)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_OPEN_PRINT_FILE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_PRINT_FILE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 2
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// Length, in bytes, of the printer-specific control data that is to 
        /// be included as the first part of the spool file. The server MUST pass 
        /// this initial portion of the spool file to the printer unmodified. 
        /// </summary>
        public ushort SetupLength;

        /// <summary>
        /// A 16-bit field that contains a flag which specifies the print file mode.
        /// </summary>
        public ushort Mode;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_OPEN_PRINT_FILE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_PRINT_FILE_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 2
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x04, representing an ASCII string
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// A null-terminated string containing a suggested name for the spool file.
        /// The server MAY ignore, modify, or use this information in 
        /// some other way to identify the print job
        /// </summary>
        [Size("ByteCount - 1")]
        public byte[] Identifier;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_OPEN_PRINT_FILE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_PRINT_FILE_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The returned file handle that MUST be used by subsequent write and 
        /// close operations on the spool file. When the spool file is closed, 
        /// the file is queued and printed.
        /// </summary>
        public ushort FID;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_OPEN_PRINT_FILE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_PRINT_FILE_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message.
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// A 16-bit field that contains a flag which specifies the print file mode.
    /// </summary>
    public enum OpenPrintFileMode : ushort
    {
        /// <summary>
        /// . Starting SetupLength bytes into the spool file, the server MAY
        /// modify character sequences to normalize them for printer output. 
        /// For example, the printer can convert tab characters in the spool
        /// file to sequences of spaces, or normalize end-of-line sequences.
        /// </summary>
        Textmode = 0,

        /// <summary>
        /// The server MUST NOT modify the contents of the spool file before sending it to the printer.
        /// </summary>
        Binarymode = 1
    }
    #endregion

    #region 2.2.4.68   	SMB_COM_WRITE_PRINT_FILE (0xC1)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_PRINT_FILE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_PRINT_FILE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid FID, creating using the SMB_COM_OPEN_PRINT_FILE command
        /// </summary>
        public ushort FID;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_PRINT_FILE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_PRINT_FILE_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be greater than or equal to 3.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be 0x01
        /// </summary>
        public byte BufferFormat;

        /// <summary>
        /// Length, in bytes, of the following data block
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// Bytes to be written to the spool file indicated by FID
        /// </summary>
        [Size("DataLength")]
        public byte[] Data;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_PRINT_FILE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_PRINT_FILE_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_WRITE_PRINT_FILE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_PRINT_FILE_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    #region 2.2.4.69   	SMB_COM_CLOSE_PRINT_FILE (0xC2)

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CLOSE_PRINT_FILE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CLOSE_PRINT_FILE_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 1.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// This field MUST be a valid FID, created using the SMB_COM_OPEN_PRINT_FILE command. 
        /// Following successful execution of this command,
        /// this FID MUST be invalidated.
        /// </summary>
        public ushort FID;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CLOSE_PRINT_FILE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CLOSE_PRINT_FILE_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }


    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_CLOSE_PRINT_FILE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CLOSE_PRINT_FILE_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 0. No parameters are sent by this message
        /// </summary>
        public byte WordCount;
    }


    /// <summary>
    /// the SMB_Data struct of SMB_COM_CLOSE_PRINT_FILE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_CLOSE_PRINT_FILE_Response_SMB_Data
    {
        /// <summary>
        /// This field MUST be 0. No data is sent by this message
        /// </summary>
        public ushort ByteCount;
    }

    #endregion

    // 2.2.4.70   	SMB_COM_GET_PRINT_QUEUE (0xC3)
    // This command was used to generate a list of items currently in a print
    // queue associated with the given TID. Clients SHOULD NOT send requests using
    // this command code. Servers receiving requests with this command code MUST 
    // return STATUS_NOT_IMPLEMENTED (ERRDOS/ERRbadfunc).

    // 2.2.4.71   	SMB_COM_READ_BULK (0xD8)
    // Clients SHOULD NOT send requests using this command code. Servers
    // receiving requests with this command code MUST return STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd). 

    // 2.2.4.72   	SMB_COM_WRITE_BULK (0xD9)
    // Clients SHOULD NOT send requests using this command code. Servers 
    // receiving requests with this command code MUST return STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd). 

    // 2.2.4.73   	SMB_COM_WRITE_BULK_DATA (0xDA)
    // Clients SHOULD NOT send requests using this command code. Servers
    // receiving requests with this command code MUST return STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd). 

    // 2.2.4.74   	SMB_COM_INVALID (0xFE)
    // Clients SHOULD NOT send requests using this command code. Servers
    // receiving requests with this command code MUST return STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd).

    // 2.2.4.75   	SMB_COM_NO_ANDX_COMMAND (0xFF)
    // In the earliest SMB protocol specifications (see [IBM-SMB]), this 
    // command code was reserved for proprietary protocol extensions. That usage 
    // is obsolete. Core Protocol documentation from Microsoft, including [SMB-CORE] 
    // and [XEXTNP], does not include any reference to the use this command code for
    // protocol extensions (or any other purpose).

    #endregion


    #region 2.2.5 Transaction Subcommands

    #region 2.2.5.1   	TRANS_SET_NMPIPE_STATE (0x0001)

    /// <summary>
    /// the Trans_Parameters struct of TRANS_SET_NMPIPE_STATE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_SET_NMPIPE_STATE_RequestTransParameters
    {
        /// <summary>
        /// This field contains the value that defines the state being set on the pipe.
        /// Any combination of the following flags MUST be valid for
        /// the set operation. All other flags are considered unused and SHOULD be set
        /// to 0 when this message is sent. The server MUST ignore the 
        /// unused bits when the message is received
        /// </summary>
        public PipeState PipeState;
    }


    /// <summary>
    /// This field contains the value that defines the state being set on the pipe.
    /// Any combination of the following flags MUST be valid for
    /// the set operation. All other flags are considered unused and SHOULD be set 
    /// to 0 when this message is sent. The server MUST ignore the 
    /// unused bits when the message is received
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum PipeState : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// If set, the pipe returns immediately on a read request if no
        /// data is available. If not set, a read request blocks until the read.
        /// </summary>
        Blocking = 0x8000,

        /// <summary>
        /// If set, the named pipe endpoint is a server endpoint; otherwise, it is a consumer endpoint. Because the
        /// CIFS client is a consumer requesting service from the named pipe, this bit MUST always be clear, indicating
        /// that the client is accessing the consumer endpoint.
        /// </summary>
        Endpoint = 0x4000,

        /// <summary>
        /// If set, the native mode of the pipe is message mode. If clear, the native mode of the pipe is byte mode.
        /// </summary>
        PipeType = 0x0400,

        /// <summary>
        /// If set, the named pipe is operating in message mode. If not set,
        /// the named pipe is operating in stream mode. In message mode, the 
        /// system treats the bytes read or written in each I/O operation to
        /// the pipe as a message unit. The system MUST perform write
        /// operations on message-type pipes as if write-through mode were enabled. 
        /// </summary>
        ReadMode = 0x0100,

        /// <summary>
        /// An 8-bit count value used to manage instances of the named pipe. This value MUST be ignored by the client.
        /// </summary>
        Icount = 0x00FF,
    }

    #endregion

    #region 2.2.5.2   	TRANS_RAW_READ_NMPIPE (0x0011)

    /// <summary>
    /// the Trans_Data struct of TRANS_RAW_READ_NMPIPE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_RAW_READ_NMPIPE_Response_Trans_Data
    {
        /// <summary>
        /// The data buffer that MUST contain the bytes read from the named
        /// pipe in raw mode. The size of the buffer MUST be equal to the value 
        /// in TotalDataCount.
        /// </summary>
        public byte[] BytesRead;
    }

    #endregion

    #region 2.2.5.3   	TRANS_QUERY_NMPIPE_STATE (0x0021)

    /// <summary>
    /// the Trans_Parameters struct of TRANS_QUERY_NMPIPE_STATE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_QUERY_NMPIPE_STATE_Response_Trans_Parameters
    {
        /// <summary>
        /// This field contains the value that defines the current state 
        /// of the named pipe. Any combination of the following flags MUST be valid 
        /// for the query operation. All other flags are considered unused 
        /// and SHOULD be set to 0 when this message is sent. The client MUST ignore 
        /// the unused bits when the message is received
        /// </summary>
        public SMB_NMPIPE_STATUS NMPipeStatus;
    }

    #endregion

    #region 2.2.5.4   	TRANS_QUERY_NMPIPE_INFO (0x0022)

    /// <summary>
    /// the Trans_Parameters struct of TRANS_QUERY_NMPIPE_INFO Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_QUERY_NMPIPE_INFO_Request_Trans_Parameters
    {
        /// <summary>
        /// This field MUST be set to 1. This value (as specified in [MS-DTYP]
        /// section 2.2.54) describes the information level being queried for 
        /// the pipe. The only supported value is 0x0001. If the server 
        /// receives any other value, it MUST fail the request with a status of 
        /// STATUS_INVALID_PARAMETER (ERRDOS/ERRinvalidparam).
        /// </summary>
        public ushort Level;
    }


    /// <summary>
    /// the Trans_Data struct of TRANS_QUERY_NMPIPE_INFO Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_QUERY_NMPIPE_INFO_Response_Trans_Data
    {
        /// <summary>
        /// This field MUST be the actual size of the buffer for outgoing (server) I/O.
        /// </summary>
        public ushort OutputBufferSize;

        /// <summary>
        /// This field MUST be the actual size of the buffer for incoming (client) I/O.
        /// </summary>
        public ushort InputBufferSize;

        /// <summary>
        /// This field MUST be the maximum number of allowed instances of the named pipe.
        /// </summary>
        public byte MaximumInstances;

        /// <summary>
        /// This field MUST be the current number of named pipe instances. 
        /// The count increments when the server creates a named pipe, and 
        /// decrements when the server closes the named pipe for an unconnected pipe,
        /// or when both server and client close the named pipe for
        /// a connected pipe. 
        /// </summary>
        public byte CurrentInstances;

        /// <summary>
        /// This field MUST be the length in bytes of the pipe name including
        /// the terminating null character
        /// </summary>
        public byte PipeNameLength;

        /// <summary>
        /// If SMB_FLAGS_UNICODE is set in the Flags2 field.
        /// Pad one null byte to ensure PipeName start on a 2-byte boundary from the start of the SMB header.
        /// Else pad zero byte.
        /// </summary>
        public byte[] Pad;

        /// <summary>
        /// This field MUST be a null-terminated string containing the name
        /// of the named pipe, not including the initial \\NodeName string
        /// (that is, of the form \PIPE\pipename). If SMB_FLAGS2_UNICODE is
        /// set in the Flags2 field of the SMB header of the response, 
        /// the name string MUST be in a null-terminated array of 16-bit
        /// Unicode characters. Otherwise, the name string MUST be a null-terminated 
        /// array of OEM characters. If the PipeName field consists of
        /// Unicode characters, this field MUST be aligned to start on a 2-byte boundary 
        /// from the start of the SMB header.
        /// </summary>
        [Size("PipeNameLength")]
        public byte[] PipeName;
    }

    #endregion

    #region 2.2.5.5   	TRANS_PEEK_NMPIPE (0x0023)

    /// <summary>
    /// the Trans_Parameters struct of TRANS_PEEK_NMPIPE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_PEEK_NMPIPE_Response_Trans_Parameters
    {
        /// <summary>
        /// This field contains the total number of bytes available to be read from the pipe.
        /// </summary>
        public ushort ReadDataAvailable;

        /// <summary>
        /// If the named pipe is a message mode pipe, this MUST be set
        /// to the number of bytes remaining in the message that was peeked 
        /// (the number of bytes in the message minus the number of bytes read).
        /// If the entire message was read, this value is 0. 
        /// If the named pipe is a stream mode pipe, this value MUST be set to 0.
        /// </summary>
        public ushort MessageBytesLength;

        /// <summary>
        /// The status of the named pipe.
        /// </summary>
        public ushort NamedPipeState;
    }


    /// <summary>
    /// the Trans_Data struct of TRANS_PEEK_NMPIPE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_PEEK_NMPIPE_Response_Trans_Data
    {
        /// <summary>
        /// This field contains the data read from the named pipe
        /// </summary>
        public byte[] ReadData;
    }

    #endregion

    #region 2.2.5.6   	TRANS_TRANSACT_NMPIPE (0x0026)

    /// <summary>
    /// the Trans_Data struct of TRANS_TRANSACT_NMPIPE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_TRANSACT_NMPIPE_Request_Trans_Data
    {
        /// <summary>
        /// This field MUST contain the bytes to be written to the named 
        /// pipe as part of the transacted operation
        /// </summary>
        public byte[] WriteData;
    }


    /// <summary>
    /// the Trans_Data struct of TRANS_TRANSACT_NMPIPE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_TRANSACT_NMPIPE_Response_Trans_Data
    {
        /// <summary>
        /// This field MUST contain data read from the named pipe
        /// </summary>
        public byte[] ReadData;
    }

    #endregion

    #region 2.2.5.7   	TRANS_RAW_WRITE_NMPIPE (0x0031)

    /// <summary>
    /// the Trans_Data struct of TRANS_RAW_WRITE_NMPIPE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_RAW_WRITE_NMPIPE_Request_Trans_Data
    {
        /// <summary>
        /// This field MUST contain the bytes to write to the named pipe
        /// in raw format. The size of the buffer MUST be equal to the value in 
        /// TotalDataCount.
        /// </summary>
        public byte[] WriteData;
    }


    /// <summary>
    /// the Trans_Parameters struct of TRANS_RAW_WRITE_NMPIPE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_RAW_WRITE_NMPIPE_Response_Trans_Parameters
    {
        /// <summary>
        /// This field MUST be set to the number of bytes written to the pipe.
        /// </summary>
        public ushort BytesWritten;
    }

    #endregion

    #region 2.2.5.8   	TRANS_READ_NMPIPE (0x0036)

    /// <summary>
    /// the Trans_Data struct of TRANS_READ_NMPIPE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_READ_NMPIPE_Response_Trans_Data
    {
        /// <summary>
        /// This field MUST contain the bytes read from the named pipe.
        /// The size of the buffer MUST be equal to the value in TotalDataCount. 
        /// If the named pipe is a message mode pipe, and the entire message 
        /// was not read, the Status field in the SMB header MUST be set to 
        /// STATUS_BUFFER_OVERFLOW.
        /// </summary>
        public byte[] ReadData;
    }

    #endregion

    #region 2.2.5.9   	TRANS_WRITE_NMPIPE (0x0037)

    /// <summary>
    /// the Trans_Data struct of TRANS_WRITE_NMPIPE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_WRITE_NMPIPE_Request_Trans_Data
    {
        /// <summary>
        /// This field MUST contain the bytes to write to the named pipe. 
        /// The size of the buffer MUST be equal to the value in TotalDataCount
        /// </summary>
        public byte[] WriteData;
    }


    /// <summary>
    /// the Trans_Parameters struct of TRANS_WRITE_NMPIPE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_WRITE_NMPIPE_Response_Trans_Parameters
    {
        /// <summary>
        /// This field MUST be set to the number of bytes written to the pipe. 
        /// </summary>
        public ushort BytesWritten;
    }

    #endregion

    // 2.2.5.10   	TRANS_WAIT_NMPIPE (0x0053)
    // none

    #region 2.2.5.11   	TRANS_CALL_NMPIPE (0x0054)

    /// <summary>
    /// the Trans_Data struct of TRANS_CALL_NMPIPE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_CALL_NMPIPE_Request_Trans_Data
    {
        /// <summary>
        /// This field MUST contain the bytes to write to the named pipe.
        /// The size of the buffer MUST be equal to the value in TotalDataCount
        /// </summary>
        public byte[] WriteData;
    }


    /// <summary>
    /// the Trans_Data struct of TRANS_CALL_NMPIPE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_CALL_NMPIPE_Response_Trans_Data
    {
        /// <summary>
        /// This field MUST contain the bytes read from the named pipe. 
        /// The size of the buffer MUST be equal to the value in TotalDataCount field of the response
        /// If the named pipe is a message mode pipe, and the entire message
        /// was not read, the Status field in the SMB header MUST be set to STATUS_BUFFER_OVERFLOW
        /// </summary>
        public byte[] ReadData;
    }

    // 2.2.5.12   	TRANS_MAILSLOT_WRITE (0x0001)
    // Windows clients do not send TRANS_MAILSLOT_WRITE commands via CIFS sessions. 
    // Related protocols, such as [MS-BRWS], send Class 2 mailslot messages as NetBIOS datagrams. 
    // TRANS_MAILSLOT_WRITE commands carrying Class 2 messages do not require a response. See [MS-MAIL].

    #endregion

    // 2.2.5.12   	TRANS_MAILSLOT_WRITE (0x0001)
    // none

    #endregion


    #region 2.2.6 Transaction2 Subcommands

    #region 2.2.6.1   	TRANS2_OPEN2 (0x0000)

    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_OPEN2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_OPEN2_Request_Trans2_Parameters
    {
        /// <summary>
        /// This 16-bit field of flags is used to request that the server take certain actions.
        /// </summary>
        public ushort Flags;

        /// <summary>
        /// A 16-bit field for encoding the requested access mode. See section 3.2.4.5.1
        /// for a discussion on sharing modes.
        /// </summary>
        public ushort AccessMode;

        /// <summary>
        /// This field MUST be set to zero (0) and be ignored by the server
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// ATTRIBUTES Attributes to apply to the file if it needs to be created.
        /// </summary>
        public SmbFileAttributes FileAttributes;

        /// <summary>
        /// A time value expressed in seconds past Jan 1, 1970 00:00:00:00 to apply 
        /// to the file's attributes if the file is created
        /// </summary>
        public uint CreationTime;

        /// <summary>
        /// A 16-bit field that controls the way a file SHOULD be treated when it is
        /// opened for use by certain extended SMB requests
        /// </summary>
        public ushort OpenMode;

        /// <summary>
        /// The number of bytes to reserve for the file if the file is being created or truncated. 
        /// </summary>
        public uint AllocationSize;

        /// <summary>
        /// This field MUST be set to zero (0).
        /// </summary>
        [StaticSize(5, StaticSizeMode.Elements)]
        public ushort[] Reserved;

        /// <summary>
        /// A buffer containing the name of the file to be opened, created, or truncated.
        /// The string MUST be null terminated
        /// </summary>
        public byte[] FileName;
    }


    /// <summary>
    /// the Trans2_Data struct of TRANS2_OPEN2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_OPEN2_Request_Trans2_Data
    {
        /// <summary>
        /// A list of extended file attribute name / value pairs that are to be assigned to the file.
        /// </summary>
        public SMB_FEA_LIST ExtendedAttributeList;
    }


    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_OPEN2 Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_OPEN2_Response_Trans2_Parameters
    {
        /// <summary>
        /// This field contains the FID of the opened file.
        /// </summary>
        public ushort Fid;

        /// <summary>
        /// The file attributes assigned to the file after the open or create has occurred.
        /// </summary>
        public SmbFileAttributes FileAttributes;

        /// <summary>
        /// The creation time assigned to the file by the server expressed as the number 
        /// of seconds since Jan 1, 1970 00:00:00:00.
        /// </summary>
        public uint CreationTime;

        /// <summary>
        /// The current size of the file in bytes.
        /// </summary>
        public uint FileDataSize;

        /// <summary>
        /// The access that the server granted as a result of the request. See DesiredAccess
        /// above for the encoding of the access rights.
        /// </summary>
        public ushort AccessMode;

        /// <summary>
        /// The type of file that was opened or created as a result of the request.
        /// </summary>
        public FileTypeValue ResourceType;

        /// <summary>
        /// The state of the IPC device and is valid only if FileType is a named pipe.
        /// </summary>
        public SMB_NMPIPE_STATUS NMPipeStatus;

        /// <summary>
        /// The state of the IPC device and is valid only if FileType is a named pipe.
        /// </summary>
        public OpenResultsValues OpenResults;

        /// <summary>
        /// This field SHOULD be set to zero (0) and MUST be ignored by the server.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// USHORT  If an error was detected in the FileExtededAttributeList,
        /// then this field contains the offset in bytes to the specific List[] 
        /// entry that caused the error.
        /// </summary>
        public ushort ExtendedAttributeErrorOffset;

        /// <summary>
        /// The total size of the extended attributes for the opened file.
        /// </summary>
        public uint ExtendedAttributeLength;
    }


    /// <summary>
    /// This 16-bit field of flags is used to request that the server take certain actions.
    /// </summary>
    [Flags()]
    public enum Trans2Open2Flags : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Return additional information in the response; populate the CreationTimeInSeconds, 
        /// DataSize, GrantedAccess, FileType, and DeviceState
        /// fields in the response.
        /// </summary>
        Additionalinformation = 0x0001,

        /// <summary>
        /// Exclusive oplock requested.
        /// </summary>
        Exclusiveoplock = 0x0002,

        /// <summary>
        /// Batch oplock requested.
        /// </summary>
        Batchoplock = 0x0004,

        /// <summary>
        /// Return total length of Extended Attributes.
        /// </summary>
        ExtendedAttributes = 0x0008
    }


    /// <summary>
    /// USHORT A 16-bit field for encoding the requested access mode. See section 3.2.4.5.1 
    /// for a discussion on sharing modes
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum Trans2Open2DesiredAccess : ushort
    {
        /// <summary>
        /// AccessMode mask
        /// </summary>
        AccessMode = 0x0007,

        /// <summary>
        /// Open for reading
        /// </summary>
        AccessMode_OpenForReading = 0x0000,

        /// <summary>
        /// Open for writing
        /// </summary>
        AccessMode_OpenForWriting = 0x0001,

        /// <summary>
        /// Open for reading and writing
        /// </summary>
        AccessMode_OpenForReadingAndWriting = 0x0002,

        /// <summary>
        /// Open for execution
        /// </summary>
        AccessMode_OpenForExecution = 0x0003,

        /// <summary>
        /// SharingMode mask
        /// </summary>
        SharingMode = 0x0070,

        /// <summary>
        /// Compatibility mode
        /// </summary>
        SharingMode_CompatibilityMode = 0x0000,

        /// <summary>
        /// Deny read/write/execute to others (exclusive use requested)
        /// </summary>
        SharingMode_DenyReadWriteExecute = 0x0010,

        /// <summary>
        /// Deny write to others
        /// </summary>
        SharingMode_DenyWrite = 0x0020,

        /// <summary>
        /// Deny read/execute to others
        /// </summary>
        SharingMode_DenyReadExecute = 0x0030,

        /// <summary>
        /// Deny nothing to others
        /// </summary>
        SharingMode_DenyNothing = 0x0040,

        /// <summary>
        /// ReferenceLocality mask
        /// </summary>
        ReferenceLocality = 0x0700,

        /// <summary>
        /// Unknown locality of reference
        /// </summary>
        ReferenceLocality_UnknownLocality = 0x0000,

        /// <summary>
        /// Mainly sequential access
        /// </summary>
        ReferenceLocality_MainlySequentialAccess = 0x0100,

        /// <summary>
        /// Mainly random access
        /// </summary>
        ReferenceLocality_MainlyRandomAccess = 0x0200,

        /// <summary>
        /// Random access with some locality
        /// </summary>
        ReferenceLocality_RandomAccessWithSomeLocality = 0x0300,

        /// <summary>
        /// CacheMode mask
        /// </summary>
        CacheMode = 0x1000,

        /// <summary>
        /// Perform caching on file
        /// </summary>
        CacheMode_PerformCache = 0x0000,

        /// <summary>
        /// Do not cache the file
        /// </summary>
        CacheMode_DoNotCache = 0x1000,

        /// <summary>
        /// WritethroughMode mask
        /// </summary>
        WritethroughMode = 0x4000,

        /// <summary>
        /// WritethroughMode is not set
        /// </summary>
        WritethroughMode_NotSet = 0x0000,

        /// <summary>
        /// no read ahead or write behind is allowed on this file or device.
        /// When the response is returned, data is expected to be on the disk or device.
        /// </summary>
        WritethroughMode_Set = 0x4000,
    }

    #endregion

    #region 2.2.6.2   	TRANS2_FIND_FIRST2 (0x0001)

    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_FIND_FIRST2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_FIND_FIRST2_Request_Trans2_Parameters
    {
        /// <summary>
        /// File attributes to apply as a constraint to the file search.
        /// </summary>
        public SmbFileAttributes SearchAttributes;

        /// <summary>
        /// The server MUST NOT return more entries than indicated by the value of this field.
        /// </summary>
        public ushort SearchCount;

        /// <summary>
        /// This bit field contains flags used to request that the server manage
        /// the state of the transaction based on how the client wishes to 
        /// traverse the results
        /// </summary>
        public Trans2FindFlags Flags;

        /// <summary>
        /// This field contains an information level code, which determines t
        /// he information contained in the response. The response formats are found 
        /// later in this section. A client that has not negotiated long names
        /// support MUST only request SMB_INFO_STANDARD. If a client that has not 
        /// negotiated long names support requests an InformationLevel other 
        /// than SMB_INFO_STANDARD, the server MUST return an error of NT_STATUS of 
        /// STATUS_INVALID_PARAMETER or SMB error of ERRDOS/ERRinvalidparam.
        /// </summary>
        public FindInformationLevel InformationLevel;

        /// <summary>
        /// This field specifies if the find is searching for directories or for files.
        /// </summary>
        public Trans2FindFirst2SearchStorageType SearchStorageType;

        /// <summary>
        /// The file pattern to search for. This field MAY contain wildcard characters.
        /// </summary>
        public byte[] FileName;
    }


    /// <summary>
    /// the Trans2_Data struct of TRANS2_FIND_FIRST2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_FIND_FIRST2_Request_Trans2_Data
    {
        /// <summary>
        /// A list of extended attribute (EA) names. The value of the AttributeName fields MUST be used by the server
        /// to query the set of extended attributes that match the set of AttributeName values provided in this list.
        /// </summary>
        public SMB_GEA_LIST GetExtendedAttributeList;
    }


    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_FIND_FIRST2 Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_FIND_FIRST2_Response_Trans2_Parameters
    {
        /// <summary>
        /// The server generated search identifier for this transaction.
        /// It MUST be provided in TRANS2_FIND_NEXT2 transactions
        /// </summary>
        public ushort SID;

        /// <summary>
        /// The number of entries returned by the search
        /// </summary>
        public ushort SearchCount;

        /// <summary>
        /// This field MUST be one (1) if the search MAY be continued 
        /// using the TRANS2_FIND_NEXT2 transaction. This field MUST be zero (0)
        /// if this response is the last and the find has reached the end of the search results
        /// </summary>
        public ushort EndOfSearch;

        /// <summary>
        /// If Request.Trans2_Parameters.InformationLevel is not 
        /// SMB_INFO_QUERY_EAS_FROM_LIST, this field MUST be zero (0). 
        /// If InformationLevel is SMB_INFO_QUERY_EAS_FROM_LIST this field 
        /// marks the offset to an extended attribute who's retrieval
        /// caused an error. This field MUST contain the offset in bytes
        /// to the SMB_EA entry in Trans2_Data.ExtendedAttributesList 
        /// which identifies the extended attribute that caused the error
        /// or zero (0) if no error was encountered.
        /// </summary>
        public ushort EaErrorOffset;

        /// <summary>
        /// If the server cannot resume the search, this field MUST be
        /// zero (0). If the server MAY resume the search, then this field contains 
        /// the offset in bytes into the Trans2_Data at which the file name 
        /// of the last entry returned by the server is located. This value 
        /// MAY be used in the Trans2_Parameters of the request to continue
        /// a search. See TRANS2_FINDNEXT2 for more information
        /// </summary>
        public ushort LastNameOffset;
    }


    /// <summary>
    /// the Trans2_Data struct of TRANS2_FIND_FIRST2 Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_FIND_FIRST2_Response_Trans2_Data
    {
        /// <summary>
        /// it is different according to the query information level.
        /// </summary>
        public Object Data;
    }


    /// <summary>
    /// the query information level of SMB_INFO_STANDARD_OF_TRANS2_FIND_FIRST2
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_INFO_STANDARD_OF_TRANS2_FIND_FIRST2
    {
        /// <summary>
        /// This field is optional. If the SMB_FIND_RETURN_RESUME_KEYS bit 
        /// is set in the Flags field of the TRANS2_FIND_FIRST2 
        /// request parameters, then this field MUST contain the server
        /// generate resume key. The resume key MUST be supplied in 
        /// subsequent TRANS2_FIND_NEXT2 requests to continue the search.
        /// If the SMB_FIND_RETURN_RESUME_KEYS bit is not set, 
        /// then the server MUST NOT include this field
        /// </summary>
        public uint ResumeKey;

        /// <summary>
        /// This field contains the date when the file was created.
        /// </summary>
        public SmbDate CreationDate;

        /// <summary>
        /// This field contains the time when the file was created
        /// </summary>
        public SmbTime CreationTime;

        /// <summary>
        /// This field contains the date when the file was last accessed.
        /// </summary>
        public SmbDate LastAccessDate;

        /// <summary>
        /// This field contains the time when the file was last accessed.
        /// </summary>
        public SmbTime LastAccessTime;

        /// <summary>
        /// This field contains the date when data was last written to the file .
        /// </summary>
        public SmbDate LastWriteDate;

        /// <summary>
        /// This field contains the time when data was last written to the file.
        /// </summary>
        public SmbTime LastWriteTime;

        /// <summary>
        /// This field contains the file size in filesystem allocation units
        /// </summary>
        public uint DataSize;

        /// <summary>
        /// This field contains the size of the filesystem allocation unit in bytes.
        /// </summary>
        public uint AllocationSize;

        /// <summary>
        /// This field contains the file attributes.
        /// </summary>
        public SmbFileAttributes Attributes;

        /// <summary>
        /// This field contains the length of the FileName field in bytes.
        /// </summary>
        public byte FileNameLength;

        /// <summary>
        /// This field contains the name of the file
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }


    /// <summary>
    /// the query information level of SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_FIND_FIRST2
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_FIND_FIRST2
    {
        /// <summary>
        /// This field is OPTIONAL. If the SMB_FIND_RETURN_RESUME_KEYS bit
        /// is set in the Flags field of the TRANS2_FIND_FIRST2 request parameters, 
        /// then this field MUST contain the server generate resume key.
        /// The resume key MUST be supplied in subsequent TRANS2_FIND_NEXT2 requests 
        /// to continue the search. If the SMB_FIND_RETURN_RESUME_KEYS 
        /// bit is not set, then the server MUST NOT include this field
        /// </summary>
        public uint ResumeKey;

        /// <summary>
        /// This field contains the date when the file was created
        /// </summary>
        public SmbDate CreationDate;

        /// <summary>
        /// This field contains the time when the file was created
        /// </summary>
        public SmbTime CreationTime;

        /// <summary>
        /// This field contains the date when the file was last accessed.
        /// </summary>
        public SmbDate LastAccessDate;

        /// <summary>
        /// This field contains the time when the file was last accessed.
        /// </summary>
        public SmbTime LastAccessTime;

        /// <summary>
        /// This field contains the date when data was last written to the file .
        /// </summary>
        public SmbDate LastWriteDate;

        /// <summary>
        /// This field contains the size of the filesystem allocation unit in bytes.
        /// </summary>
        public SmbTime LastWriteTime;

        /// <summary>
        /// This field contains the file attributes.
        /// </summary>
        public uint DataSize;

        /// <summary>
        /// This field contains the size of the file's extended attribute information in bytes.
        /// </summary>
        public uint AllocationSize;

        /// <summary>
        /// This field contains the file attributes.
        /// </summary>
        public SmbFileAttributes Attributes;

        /// <summary>
        /// This field contains the size of the file's extended attribute information in bytes.
        /// </summary>
        public uint EaSize;

        /// <summary>
        /// This field contains the length of the FileName field in bytes.
        /// </summary>
        public byte FileNameLength;

        /// <summary>
        /// This field contains the name of the file
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }


    /// <summary>
    /// the query information level of SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_FIND_FIRST2
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_FIND_FIRST2
    {
        /// <summary>
        /// This field is OPTIONAL. If the SMB_FIND_RETURN_RESUME_KEYS bit
        /// is set in the Flags field of the TRANS2_FIND_FIRST2 request parameters,
        /// then this field MUST contain the server generate resume key.
        /// The resume key MUST be supplied in subsequent TRANS2_FIND_NEXT2 requests 
        /// to continue the search. If the SMB_FIND_RETURN_RESUME_KEYS bit 
        /// is not set, then the server MUST NOT include this field
        /// </summary>
        public uint ResumeKey;

        /// <summary>
        /// This field contains the date when the file was created
        /// </summary>
        public SmbDate CreationDate;

        /// <summary>
        /// This field contains the time when the file was created
        /// </summary>
        public SmbTime CreationTime;

        /// <summary>
        /// This field contains the date when the file was last accessed.
        /// </summary>
        public SmbDate LastAccessDate;

        /// <summary>
        /// This field contains the time when the file was last accessed.
        /// </summary>
        public SmbTime LastAccessTime;

        /// <summary>
        /// This field contains the date when data was last written to the file .
        /// </summary>
        public SmbDate LastWriteDate;

        /// <summary>
        /// This field contains the time when data was last written to the file.
        /// </summary>
        public SmbTime LastWriteTime;

        /// <summary>
        /// This field contains the file size in filesystem allocation units
        /// </summary>
        public uint DataSize;

        /// <summary>
        /// This field contains the size of the filesystem allocation unit in bytes.
        /// </summary>
        public uint AllocationSize;

        /// <summary>
        /// This field contains the file attributes
        /// </summary>
        public SmbFileAttributes Attributes;

        /// <summary>
        /// This field contains the size of the file's extended attribute information in bytes.
        /// </summary>
        public uint EaSize;

        /// <summary>
        /// A list of extended file attribute name value pairs where the AttributeName
        /// field values match those that were provided in the request
        /// </summary>
        public SMB_FEA[] ExtendedAttributeList;

        /// <summary>
        /// This field contains the length of the FileName field in bytes.
        /// </summary>
        public byte FileNameLength;

        /// <summary>
        /// This field contains the name of the file
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }


    /// <summary>
    /// the query information level of SMB_FIND_FILE_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_FIND_FILE_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2
    {
        /// <summary>
        /// This field contains the offset in bytes from this entry in the list 
        /// to the next entry in the list. If there are no additional
        /// entries the value MUST be zero (0).
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// This field MUST contain the byte offset of the file within the 
        /// parent directory. This member is undefined for file systems, 
        /// such as NTFS, in which the position of a file within the parent 
        /// directory is not fixed and can be changed at any time to maintain
        /// sort order
        /// </summary>
        public uint FileIndex;

        /// <summary>
        /// This field contains the date and time when the file was created.
        /// </summary>
        public FileTime CreationTime;

        /// <summary>
        /// This field contains the date and time when the file was last accessed.
        /// </summary>
        public FileTime LastAccessTime;

        /// <summary>
        /// This field contains the date and time when data was last written to the file.
        /// </summary>
        public FileTime LastWriteTime;

        /// <summary>
        /// This field contains the date and time when the file attributes where last changed. 
        /// </summary>
        public FileTime LastAttrChangeTime;

        /// <summary>
        /// This field contains the offset in bytes to the start of the
        /// file to the first byte after the end of the file.
        /// </summary>
        public ulong EndOfFile;

        /// <summary>
        /// This field contains the file allocation size, in bytes. Usually,
        /// this value is a multiple of the sector or cluster size of the underlying
        /// physical device.
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// This field contains file attribute information flags encoded as follows.
        /// </summary>
        public SmbFileAttributes32 FileAttributes;

        /// <summary>
        /// This field contains the length of the FileName field in bytes.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// This field contains the name of the file
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }


    /// <summary>
    /// the query information level of SMB_FIND_FILE_FULL_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_FIND_FILE_FULL_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2
    {
        /// <summary>
        /// This field contains the offset in bytes from this entry in the 
        /// list to the next entry in the list. If there are no additional entries 
        /// the value MUST be zero (0).
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// This field MUST contain the byte offset of the file within the 
        /// parent directory. This member is undefined for file systems, such as NTFS,
        /// in which the position of a file within the parent directory is
        /// not fixed and can be changed at any time to maintain sort order
        /// </summary>
        public uint FileIndex;

        /// <summary>
        /// This field contains the date and time when the file was created.
        /// </summary>
        public FileTime CreationTime;

        /// <summary>
        /// This field contains the date and time when the file was last accessed.
        /// </summary>
        public FileTime LastAccessTime;

        /// <summary>
        /// This field contains the date and time when data was last written to the file.
        /// </summary>
        public FileTime LastWriteTime;

        /// <summary>
        /// This field contains the date and time when the file attributes where last changed. 
        /// </summary>
        public FileTime LastAttrChangeTime;

        /// <summary>
        /// This field contains the offset in bytes to the start of the file 
        /// to the first byte after the end of the file.
        /// </summary>
        public ulong EndOfFile;

        /// <summary>
        /// This field contains the file allocation size, in bytes. Usually, 
        /// this value is a multiple of the sector or cluster size
        /// of the underlying physical device.
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// This field contains file attribute information flags encoded as follows.
        /// </summary>
        public SmbFileAttributes32 ExtFileAttributes;

        /// <summary>
        /// This field contains the length of the FileName field in bytes.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// This field contains the length of the ExtendedAttributeList in bytes
        /// </summary>
        public uint EaSize;

        /// <summary>
        /// This field contains the name of the file
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }


    /// <summary>
    /// the query information level of SMB_FIND_FILE_BOTH_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_FIND_FILE_BOTH_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2
    {
        /// <summary>
        /// This field contains the offset in bytes from this entry in the list to
        /// the next entry in the list. If there are no additional entries the
        /// value MUST be zero (0).
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// This field MUST contain the byte offset of the file within the
        /// parent directory. This member is undefined for file systems, such as NTFS,
        /// in which the position of a file within the parent directory is not
        /// fixed and can be changed at any time to maintain sort order
        /// </summary>
        public uint FileIndex;

        /// <summary>
        /// This field contains the date and time when the file was created.
        /// </summary>
        public FileTime CreationTime;

        /// <summary>
        /// This field contains the date and time when the file was last accessed.
        /// </summary>
        public FileTime LastAccessTime;

        /// <summary>
        /// This field contains the date and time when data was last written to the file.
        /// </summary>
        public FileTime LastWriteTime;

        /// <summary>
        /// This field contains the date and time when the file was last changed. 
        /// </summary>
        public FileTime LastChangeTime;

        /// <summary>
        /// Absolute new end-of-file position as a byte offset from the start 
        /// of the file. EndOfFile specifies the byte offset to the end of the file.
        /// Because this value is zero-based, it actually refers to the first free
        /// byte in the file. In other words, EndOfFile is the offset to the
        /// byte immediately following the last valid byte in the file. 
        /// </summary>
        public ulong EndOfFile;

        /// <summary>
        /// This field contains the file allocation size, in bytes. Usually,
        /// this value is a multiple of the sector or cluster size of the underlying physical device
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// ULONG This field contains file attribute information flags encoded as follows.
        /// </summary>
        public SmbFileAttributes32 FileAttributes;

        /// <summary>
        /// This field MUST contain the length of the FileName field in bytes.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// This field MUST contain the length of the ExtendedAttributeList in bytes.
        /// </summary>
        public uint EaSize;

        /// <summary>
        /// This field MUST contain the length of the ShortName in bytes.
        /// </summary>
        public byte ShortNameLength;

        /// <summary>
        /// field is reserved and MUST be zero (0).
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// This field MUST contain the 8.3 name of the file in Unicode format.
        /// </summary>
        [StaticSize(24, StaticSizeMode.Elements)]
        public byte[] ShortName;

        /// <summary>
        /// This field contains the long name of the file.
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }


    /// <summary>
    /// SMB_FIND_FILE_NAMES_INFO 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_FIND_FILE_NAMES_INFO_OF_TRANS2_FIND_FIRST2
    {
        /// <summary>
        /// This field contains the offset in bytes from this entry in the
        /// list to the next entry in the list. If there are no additional 
        /// entries the value MUST be zero (0).
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// Byte offset of the file within the parent directory. This member
        /// is undefined for file systems, such as NTFS, in which the position
        /// of a file within the parent directory is not fixed and can be
        /// changed at any time to maintain sort order.
        /// </summary>
        public uint FileIndex;

        /// <summary>
        /// This field MUST contain the length of the FileName field in bytes.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// This field contains the name of the file.
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }


    /// <summary>
    /// the flags in TRANS2_FIND_FIRST2 
    /// This bit field contains flags used to request that the server manage
    /// the state of the transaction based on how the client
    /// wishes to traverse the results
    /// </summary>
    [Flags()]
    public enum Trans2FindFlags : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Close the search after this request.
        /// </summary>
        SMB_FIND_CLOSE_AFTER_REQUEST = 0x0001,

        /// <summary>
        /// Close search when end of search is reached
        /// </summary>
        SMB_FIND_CLOSE_AT_EOS = 0x0002,

        /// <summary>
        /// Return resume keys for each entry found
        /// </summary>
        SMB_FIND_RETURN_RESUME_KEYS = 0x0004,

        /// <summary>
        /// Continue search from previous ending place
        /// </summary>
        SMB_FIND_CONTINUE_FROM_LAST = 0x0008,

        /// <summary>
        /// Find with backup intent.
        /// </summary>
        SMB_FIND_WITH_BACKUP_INTENT = 0x0010
    }


    /// <summary>
    /// This field specifies if the find is searching for directories or for
    /// files. This field MUST be one of two values: 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    public enum Trans2FindFirst2SearchStorageType : uint
    {
        /// <summary>
        /// Search for only directories.
        /// </summary>
        FILE_DIRECTORY_FILE = 0x00000001,

        /// <summary>
        /// Search only for files.
        /// </summary>
        FILE_NON_DIRECTORY_FILE = 0x00000040
    }

    #endregion

    #region 2.2.6.3   	TRANS2_FIND_NEXT2 (0x0002)

    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_FIND_NEXT2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_FIND_NEXT2_Request_Trans2_Parameters
    {
        /// <summary>
        /// This field MUST be the search identifier (SID) returned in TRANS2_FIND_FIRST2 response
        /// </summary>
        public ushort SID;

        /// <summary>
        /// This field MUST be the maximum number of entries to return in the response.
        /// </summary>
        public ushort SearchCount;

        /// <summary>
        /// This field determines the information contained in the response.
        /// The response formats are found in TRANS2_FIND_FIRST2.
        /// See the individual response formats for detailed information.
        /// A client that has not negotiated long names support MUST 
        /// only request SMB_INFO_STANDARD. If a client that has not
        /// negotiated long names support requests an information level other
        /// than SMB_INFO_STANDARD the server MUST return an error of
        /// STATUS_INVALID_PARAMETER or ERRDOS/ERRinvalidparam
        /// </summary>
        public FindInformationLevel InformationLevel;
        /// <summary>
        /// This field MUST be the value of a ResumeKey field returned
        /// in the response from a TRANS2_FIND_FIRST2 or TRANS2_FIND_NEXT2 that is
        /// part of the same search (same SID).
        /// </summary>
        public uint ResumeKey;

        /// <summary>
        /// This bit mask field is used to request that the server manage
        /// the state of the transaction based on how the client wishes to traverse
        /// the results
        /// </summary>
        public Trans2FindFlags Flags;

        /// <summary>
        /// 
        /// </summary>
        public byte[] FileName;
    }


    /// <summary>
    /// the Trans2_Data struct of TRANS2_FIND_NEXT2 Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_FIND_NEXT2_Request_Trans2_Data
    {
        /// <summary>
        /// A list of extended file attribute name/value pairs. The value
        /// of the ValueName field MUST be used by the server to further constrain 
        /// the find query to files having the set of extended attributes 
        /// that match the set of AttributeName values provided in this list. 
        /// A client MAY NOT query for specific AttributeName == ValueName 
        /// conditions through supplying ValueName values. The ValueName field
        /// SHOULD be ignored by the server.
        /// </summary>
        public SMB_GEA_LIST GetExtendedAttributeList;
    }


    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_FIND_NEXT2 Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_FIND_NEXT2_Response_Trans2_Parameters
    {
        /// <summary>
        /// The number of entries returned by the search
        /// </summary>
        public ushort SearchCount;

        /// <summary>
        /// This field MUST be one (1) if the search MAY be continued 
        /// using the TRANS2_FIND_NEXT2 transaction. This field MUST be zero (0)
        /// if this response is the last and the find has reached the end of the search results
        /// </summary>
        public ushort EndOfSearch;

        /// <summary>
        /// If Request.Trans2_Parameters.InformationLevel is not 
        /// SMB_INFO_QUERY_EAS_FROM_LIST, this field MUST be zero (0). 
        /// If InformationLevel is SMB_INFO_QUERY_EAS_FROM_LIST this field 
        /// marks the offset to an extended attribute who's retrieval
        /// caused an error. This field MUST contain the offset in bytes
        /// to the SMB_EA entry in Trans2_Data.ExtendedAttributesList 
        /// which identifies the extended attribute that caused the error
        /// or zero (0) if no error was encountered.
        /// </summary>
        public ushort EaErrorOffset;

        /// <summary>
        /// If the server cannot resume the search, this field MUST be
        /// zero (0). If the server MAY resume the search, then this field contains 
        /// the offset in bytes into the Trans2_Data at which the file name 
        /// of the last entry returned by the server is located. This value 
        /// MAY be used in the Trans2_Parameters of the request to continue
        /// a search. See TRANS2_FINDNEXT2 for more information
        /// </summary>
        public ushort LastNameOffset;
    }


    /// <summary>
    /// the Trans2_Data struct of TRANS2_FIND_NEXT2 Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_FIND_NEXT2_Response_Trans2_Data
    {
        /// <summary>
        /// it is different according to the query information level.
        /// </summary>
        public Object Data;
    }

    // response structure is same as TRANS2_FIND_FIRST2

    #endregion

    #region 2.2.6.4   	TRANS2_QUERY_FS_INFORMATION (0x0003)

    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_QUERY_FS_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_QUERY_FS_INFORMATION_Request_Trans2_Parameters
    {
        /// <summary>
        /// This field determines the information contained in the response. 
        /// The response formats are found later in this section. See the individual 
        /// response formats below for detailed information.
        /// </summary>
        public QueryFSInformationLevel InformationLevel;
    }


    /// <summary>
    /// the Trans2_Data struct of TRANS2_QUERY_FS_INFORMATION Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_QUERY_FS_INFORMATION_Response_Trans2_Data
    {
        /// <summary>
        /// it is different according to the query information level.
        /// </summary>
        public Object Data;
    }


    /// <summary>
    /// the query information level of SMB_INFO_ALLOCATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_INFO_ALLOCATION
    {
        /// <summary>
        /// ULONG  This field contains a file system identifier.
        /// Windows servers always return zero (0).
        /// </summary>
        public uint idFileSystem;

        /// <summary>
        /// This field contains the number of sectors per allocation unit.
        /// </summary>
        public uint cSectorUnit;

        /// <summary>
        /// This field contains the total number of allocation units.
        /// </summary>
        public uint cUnit;

        /// <summary>
        /// This field contains the total number of available allocation units.
        /// </summary>
        public uint cUnitAvailable;

        /// <summary>
        /// This field contains the number of bytes per sector
        /// </summary>
        public ushort cbSector;
    }


    /// <summary>
    /// the query information level of SMB_INFO_VOLUME
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_INFO_VOLUME
    {
        /// <summary>
        /// This field contains the serial number of the volume
        /// </summary>
        public uint ulVolSerialNbr;

        /// <summary>
        /// This field contains the number of characters in the VolumeLabel field.
        /// </summary>
        public byte cCharCount;

        /// <summary>
        /// This field contains the volume label
        /// </summary>
        [Size("cCharCount")]
        public byte[] VolumeLabel;
    }


    /// <summary>
    /// the query information level of SMB_QUERY_FS_VOLUME_INFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_QUERY_FS_VOLUME_INFO
    {
        /// <summary>
        /// This field contains the date and time when the volume was created.
        /// </summary>
        public FileTime VolumeCreationTime;

        /// <summary>
        /// This field contains the serial number of the volume
        /// </summary>
        public uint SerialNumber;

        /// <summary>
        /// This field contains the size of the VolumeLabel field in bytes.
        /// </summary>
        public uint VolumeLabelSize;

        /// <summary>
        /// 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// This field contains the Unicode encoded volume label.
        /// </summary>
        [Size("VolumeLabelSize/2")]
        public ushort[] VolumeLabel;
    }


    /// <summary>
    /// the query information level of SMB_QUERY_FS_SIZE_INFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_QUERY_FS_SIZE_INFO
    {
        /// <summary>
        /// This field contains the total number of allocation assigned to the volume.
        /// </summary>
        public ulong TotalAllocationUnits;

        /// <summary>
        /// This field contains the total number of unallocated or free allocation units.
        /// </summary>
        public ulong TotalFreeAllocationUnits;

        /// <summary>
        /// This field contains the number of sectors per allocation unit.
        /// </summary>
        public uint SectorsPerAllocationUnit;

        /// <summary>
        /// This field contains the bytes per sector.
        /// </summary>
        public uint BytesPerSector;
    }


    /// <summary>
    /// the query information level of SMB_QUERY_FS_DEVICE_INFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_QUERY_FS_DEVICE_INFO
    {
        /// <summary>
        /// This field contains the device type on which the volume resides.
        /// Some of these device types are not currently accessible 
        /// over the network and MAY never be accessible over the network. 
        /// Some MAY change to be accessible over the network. The values
        /// for device types that are inaccessible over the network MAY be
        /// redefined to be just reserved at some date in the future.
        /// </summary>
        public DeviceType DeviceType;

        /// <summary>
        /// This 16-bit field of flags contains the device's characteristics.
        /// </summary>
        public DeviceCharacteristics DeviceCharacteristics;
    }


    /// <summary>
    /// This field contains the device type on which the volume resides. 
    /// Some of these device types are not currently accessible over the network
    /// and MAY never be accessible over the network. Some MAY change to 
    /// be accessible over the network. The values for device types that are 
    /// inaccessible over the network MAY be redefined to be just reserved 
    /// at some date in the future.
    /// </summary>
    public enum DeviceType : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_BEEP = 0x0001,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_CD_ROM = 0x0002,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_CD_ROM_FILE_SYSTEM = 0x0003,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_CONTROLLER = 0x0004,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_DATALINK = 0x0005,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_DFS = 0x0006,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_DISK = 0x0007,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_DISK_FILE_SYSTEM = 0x0008,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_FILE_SYSTEM = 0x0009,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_INPORT_PORT = 0x000a,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_KEYBOARD = 0x000b,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_MAILSLOT = 0x000c,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_MIDI_IN = 0x000d,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_MIDI_OUT = 0x000e,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_MOUSE = 0x000f,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_MULTI_UNC_PROVIDER = 0x0010,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_NAMED_PIPE = 0x0011,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_NETWORK = 0x0012,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_NETWORK_BROWSER = 0x0013,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_NETWORK_FILE_SYSTEM = 0x0014,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_NULL = 0x0015,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_PARALLEL_PORT = 0x0016,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_PHYSICAL_NETCARD = 0x0017,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_PRINTER = 0x0018,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_SCANNER = 0x0019,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_SERIAL_MOUSE_PORT = 0x001a,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_SERIAL_PORT = 0x001b,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_SCREEN = 0x001c,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_SOUND = 0x001d,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_STREAMS = 0x001e,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_TAPE = 0x001f,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_TAPE_FILE_SYSTEM = 0x0020,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_TRANSPORT = 0x0021,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_UNKNOWN = 0x0022,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_VIDEO = 0x0023,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_VIRTUAL_DISK = 0x0024,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_WAVE_IN = 0x0025,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_WAVE_OUT = 0x0026,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_8042_PORT = 0x0027,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_NETWORK_REDIRECTOR = 0x0028,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_BATTERY = 0x0029,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_BUS_EXTENDER = 0x002a,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_MODEM = 0x002b,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_VDM = 0x002c,
    }


    /// <summary>
    /// This 16-bit field of flags contains the device's characteristics. The individual flags are as follows
    /// </summary>
    [Flags()]
    public enum DeviceCharacteristics : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_REMOVABLE_MEDIA = 0x0001,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_READ_ONLY_DEVICE = 0x0002,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_FLOPPY_DISKETTE = 0x0004,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_WRITE_ONCE_MEDIA = 0x0008,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_REMOTE_DEVICE = 0x0010,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_DEVICE_IS_MOUNTED = 0x0020,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_VIRTUAL_VOLUME = 0x0040,
    }


    /// <summary>
    /// the query information level of SMB_QUERY_FS_ATTRIBUTE_INFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_QUERY_FS_ATTRIBUTE_INFO
    {
        /// <summary>
        /// This 16-bit field of flags contains the file system's attributes.
        /// </summary>
        public FileSystemAttributes FileSystemAttributes;

        /// <summary>
        /// This field contains the maximum size in bytes of a file name on the file system.
        /// </summary>
        public uint MaxFileNameLengthInBytes;

        /// <summary>
        /// This field contains the size in bytes of the FileSystemName field.
        /// </summary>
        public uint LengthOfFileSystemName;

        /// <summary>
        /// This field contains the Unicode encoded name of the file system.
        /// </summary>
        [Size("LengthOfFileSystemName/2")]
        public ushort[] FileSystemName;
    }


    /// <summary>
    /// This 16-bit field of flags contains the file system's attributes. The individual flags are as follows:
    /// </summary>
    [Flags()]
    public enum FileSystemAttributes : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_CASE_SENSITIVE_SEARCH = 0x00000001,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_CASE_PRESERVED_NAMES = 0x00000002,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_UNICODE_ON_DISK = 0x00000004,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_PERSISTENT_ACLS = 0x00000008,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_FILE_COMPRESSION = 0x00000010,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_VOLUME_IS_COMPRESSED = 0x00008000,
    }

    #endregion

    // 2.2.6.5   	TRANS2_SET_FS_INFORMATION (0x0004)
    // Clients SHOULD NOT send requests using this command code.
    // Servers receiving requests with this command code MUST return 
    // STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd).

    #region 2.2.6.6   	TRANS2_QUERY_PATH_INFORMATION (0x0005)

    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_QUERY_PATH_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_QUERY_PATH_INFORMATION_Request_Trans2_Parameters
    {
        /// <summary>
        /// This field determines the information contained in the response. 
        /// The response formats are found later in this section. 
        /// See the individual response formats below for detailed information.
        /// A client that has not negotiated long names support 
        /// MUST only request SMB_INFO_STANDARD. If a client that has not 
        /// negotiated long names support requests an InformationLevel other 
        /// than SMB_INFO_STANDARD, the server MUST return an error of 
        /// NT_STATUS of STATUS_INVALID_PARAMETER or SMB error of ERRDOS/ERRinvalidparam
        /// </summary>
        public QueryInformationLevel InformationLevel;

        /// <summary>
        /// This field is reserved and MUST be zero (0).
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// The file name or directory name for which to retrieve the information.
        /// </summary>
        public byte[] FileName;
    }


    /// <summary>
    /// the Trans2_Data struct of TRANS2_QUERY_PATH_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_QUERY_PATH_INFORMATION_Request_Trans2_Data
    {
        /// <summary>
        /// This field MUST contain an array of extended file attribute
        /// name value pairs. The server MUST return only the file's extended
        /// attributes where the file's extended attribute's name is in this
        /// array of extended attribute name value pairs. The values of
        /// the name value pairs in this array MUST be ignored by the server.
        /// The client SHOULD provide nullvalues for the values in the array.
        /// </summary>
        public SMB_GEA_LIST GetExtendedAttributeList;
    }


    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_QUERY_PATH_INFORMATION Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Parameters
    {
        /// <summary>
        /// If Request.Trans2_Parameters.InformationLevel is not 
        /// SMB_INFO_QUERY_EAS_FROM_LIST, this field MUST be zero (0). If InformationLevel
        /// is SMB_INFO_QUERY_EAS_FROM_LIST this field marks the offset to 
        /// an extended attribute who's retrieval caused an error. This field MUST
        /// contain the offset in bytes to the SMB_EA entry in Trans2_Data.
        /// ExtendedAttributesList that caused the error or zero (0) if no error was
        /// encountered.
        /// </summary>
        public ushort EaErrorOffset;
    }


    /// <summary>
    /// the Trans2_Data struct of TRANS2_QUERY_PATH_INFORMATION Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Data
    {
        /// <summary>
        /// it is different according to the query information level.
        /// </summary>
        public Object Data;
    }


    /// <summary>
    /// the query information level of SMB_INFO_STANDARD_OF_TRANS2_QUERY_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_INFO_STANDARD_OF_TRANS2_QUERY_PATH_INFORMATION
    {
        /// <summary>
        /// This field contains the date when the file was created.
        /// </summary>
        public SmbDate CreationDate;

        /// <summary>
        /// This field contains the time when the file was created.
        /// </summary>
        public SmbTime CreationTime;

        /// <summary>
        /// This field contains the date when the file was last accessed.
        /// </summary>
        public SmbDate LastAccessDate;

        /// <summary>
        /// This field contains the time when the file was last accessed.
        /// </summary>
        public SmbTime LastAccessTime;

        /// <summary>
        /// This field contains the date when data was last written to the file .
        /// </summary>
        public SmbDate LastWriteDate;

        /// <summary>
        /// This field contains the time when data was last written to the file.
        /// </summary>
        public SmbTime LastWriteTime;

        /// <summary>
        /// This field contains the file size in filesystem allocation units
        /// </summary>
        public uint DataSize;

        /// <summary>
        /// This field contains the size of the filesystem allocation unit in bytes.
        /// </summary>
        public uint AllocationSize;

        /// <summary>
        /// This field contains the file attributes.
        /// </summary>
        public SmbFileAttributes Attributes;
    }


    /// <summary>
    /// the query information level of SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_QUERY_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_QUERY_PATH_INFORMATION
    {
        /// <summary>
        /// This field contains the date when the file was created.
        /// </summary>
        public SmbDate CreationDate;

        /// <summary>
        /// This field contains the time when the file was created.
        /// </summary>
        public SmbTime CreationTime;

        /// <summary>
        /// This field contains the date when the file was last accessed.
        /// </summary>
        public SmbDate LastAccessDate;

        /// <summary>
        /// This field contains the time when the file was last accessed.
        /// </summary>
        public SmbTime LastAccessTime;

        /// <summary>
        /// DATE  This field contains the date when data was last written to the file .
        /// </summary>
        public SmbDate LastWriteDate;

        /// <summary>
        /// This field contains the time when data was last written to the file.
        /// </summary>
        public SmbTime LastWriteTime;

        /// <summary>
        /// field contains the file size in filesystem allocation units
        /// </summary>
        public uint DataSize;

        /// <summary>
        /// This field contains the size of the filesystem allocation unit in bytes.
        /// </summary>
        public uint AllocationSize;

        /// <summary>
        /// This field contains the file attributes
        /// </summary>
        public ushort Attributes;

        /// <summary>
        /// This field contains the size of the file's extended attribute information in bytes.
        /// </summary>
        public uint EaSize;
    }


    /// <summary>
    /// the query information level of SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_QUERY_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_QUERY_PATH_INFORMATION
    {
        /// <summary>
        /// This field MUST contain the total size of the FileExtendedAttributeList.
        /// </summary>
        public uint SizeOfListInBytes;

        /// <summary>
        /// A list of extended file attribute name value pairs where the AttributeName 
        /// field values match those that were provided in the request
        /// </summary>
        public SMB_FEA[] ExtendedAttributeList;
    }


    /// <summary>
    /// the query information level of SMB_INFO_QUERY_ALL_EAS_OF_TRANS2_QUERY_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_INFO_QUERY_ALL_EAS_OF_TRANS2_QUERY_PATH_INFORMATION
    {
        /// <summary>
        /// This field MUST contain the total size of the FileExtendedAttributeList.
        /// </summary>
        public uint SizeOfListInBytes;

        /// <summary>
        /// A list of all of the extended file attribute name value pairs assigned to the file.
        /// </summary>
        public SMB_FEA[] ExtendedAttributeList;
    }


    /// <summary>
    /// the query information level of SMB_QUERY_FILE_BASIC_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_QUERY_FILE_BASIC_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    {
        /// <summary>
        /// This field contains the date and time when the file was created.
        /// </summary>
        public FileTime CreationTime;

        /// <summary>
        /// This field contains the date and time when the file was last accessed.
        /// </summary>
        public FileTime LastAccessTime;

        /// <summary>
        /// This field contains the date and time when data was last written to the file.
        /// </summary>
        public FileTime LastWriteTime;

        /// <summary>
        /// FILETIME This field contains the date and time when the file was last changed. 
        /// </summary>
        public FileTime LastChangeTime;

        /// <summary>
        /// This field contains the file attributes.
        /// </summary>
        public SmbFileAttributes FileAttributes;
    }


    /// <summary>
    /// the query information level of SMB_QUERY_FILE_STANDARD_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_QUERY_FILE_STANDARD_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    {
        /// <summary>
        /// This field contains the number of bytes that are allocated to the file.
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// This field contains the offset in bytes to the start of the file to the
        /// first byte after the end of the file.
        /// </summary>
        public ulong EndOfFile;

        /// <summary>
        /// field contains the number of hard links to the file.
        /// </summary>
        public uint NumberOfLinks;

        /// <summary>
        /// This field indicates if the there is a delete action pending for the file.
        /// </summary>
        public byte DeletePending;

        /// <summary>
        /// This field indicates if the file is a directory
        /// </summary>
        public byte Directory;
    }


    /// <summary>
    /// the query information level of SMB_QUERY_FILE_EA_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_QUERY_FILE_EA_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    {
        /// <summary>
        /// This field MUST contain the length of a file's list of extended attributes in bytes.
        /// </summary>
        public uint EaSize;
    }


    /// <summary>
    /// the query information level of SMB_QUERY_FILE_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_QUERY_FILE_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    {
        /// <summary>
        /// This field MUST contain the length of the FileName field in bytes.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// This field contains the name of the file.
        /// </summary>
        [Size("FileNameLength/2")]
        public ushort[] FileName;
    }


    /// <summary>
    /// the query information level of SMB_QUERY_FILE_ALL_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_QUERY_FILE_ALL_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    {
        /// <summary>
        /// This field contains the date and time when the file was created.
        /// </summary>
        public FileTime CreationTime;

        /// <summary>
        /// This field contains the date and time when the file was last accessed.
        /// </summary>
        public FileTime LastAccessTime;

        /// <summary>
        /// This field contains the date and time when data was last written to the file.
        /// </summary>
        public FileTime LastWriteTime;

        /// <summary>
        /// This field contains the date and time when the file was last changed. 
        /// </summary>
        public FileTime LastChangeTime;

        /// <summary>
        /// This field contains the file attributes
        /// </summary>
        public SmbFileAttributes32 FileAttributes;

        /// <summary>
        /// Reserved field
        /// </summary> 
        public uint Reserved1;

        /// <summary>
        /// This field contains the number of bytes that are allocated to the file.
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// This field contains the offset in bytes to the start of the file to the 
        /// first byte after the end of the file.
        /// </summary>
        public ulong EndOfFile;

        /// <summary>
        /// This field contains the number of hard links to the file.
        /// </summary>
        public uint NumberOfLinks;

        /// <summary>
        /// This field indicates if the there is a delete action pending for the file.
        /// </summary>
        public byte DeletePending;

        /// <summary>
        /// This field indicates if the file is a directory
        /// </summary>
        public byte Directory;

        /// <summary>
        /// Reserved field
        /// </summary>
        public ushort Reserved2;

        /// <summary>
        /// This field MUST contain the length of a file's list of extended attributes in bytes. 
        /// </summary>
        public uint EaSize;

        /// <summary>
        /// This field MUST contain the length in bytes of the FileName field.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// This field contains the name of the file.
        /// </summary>
        [Size("FileNameLength/2")]
        public ushort[] FileName;
    }


    /// <summary>
    /// This field MUST be one of the following values
    /// </summary>
    public enum AlignmentRequirement : uint
    {
        /// <summary>
        /// possible value
        /// </summary>
        FILE_BYTE_ALIGNMENT = 0x00000000,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_WORD_ALIGNMENT = 0x00000001,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_LONG_ALIGNMENT = 0x00000003,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_QUAD_ALIGNMENT = 0x00000007,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_OCTA_ALIGNMENT = 0x0000000F,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_32_BYTE_ALIGNMENT = 0x0000001F,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_64_BYTE_ALIGNMENT = 0x0000003F,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_128_BYTE_ALIGNMENT = 0x0000007F,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_256_BYTE_ALIGNMENT = 0x000000FF,

        /// <summary>
        /// possible value
        /// </summary>
        FILE_512_BYTE_ALIGNMENT = 0x000001FF,
    }


    /// <summary>
    /// the query information level of SMB_QUERY_FILE_ALT_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_QUERY_FILE_ALT_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    {
        /// <summary>
        /// This field contains the length in bytes of the FileName field.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// This field contains the 8.3 name of the file in Unicode. The string is NOT null terminated.
        /// </summary>
        [Size("FileNameLength / 2")]
        public ushort[] FileName;
    }


    /// <summary>
    /// the query information level of SMB_QUERY_FILE_STREAM_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_QUERY_FILE_STREAM_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    {
        /// <summary>
        /// A 32-bit unsigned integer that contains the byte offset from
        /// the beginning of this entry, at which the next FILE_ STREAM _INFORMATION 
        /// entry is located, if multiple entries are present in a buffer.
        /// This member is zero if no other entries follow this one. An implementation
        /// MUST use this value to determine the location of the next entry 
        /// (if multiple entries are present in a buffer), and MUST NOT assume that 
        /// the value of NextEntryOffset is the same as the size of the current entry.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// A 32-bit unsigned integer that contains the length, in bytes, of the stream name string.
        /// </summary>
        public uint StreamNameLength;

        /// <summary>
        /// A 64-bit signed integer that contains the size, in bytes, of the stream.
        /// The value of this field MUST be greater than or equal to 0.
        /// </summary>
        public ulong StreamSize;

        /// <summary>
        /// A 64-bit signed integer that contains the file stream allocation
        /// size in bytes. Usually, this value is a multiple of the sector
        /// or cluster size of the underlying physical device. The value of 
        /// this field MUST be greater than or equal to 0.
        /// </summary>
        public ulong StreamAllocationSize;

        /// <summary>
        /// A sequence of Unicode characters containing the name of the stream
        /// using the form ":streamname:$DATA", or "::$DATA" for the default stream.
        /// The :$DATA string that follows streamname is an internal data type
        /// tag that is unintentionally exposed. The leading ??and trailing 
        /// ?$DATA?characters are not part of the stream name and MUST be 
        /// stripped from this field to derive the actual stream name. A resulting
        /// empty string for the stream name denotes the default stream. Since
        /// this field might not be null-terminated, it MUST be handled as a 
        /// sequence of StreamNameLength bytes
        /// </summary>
        [Size("StreamNameLength / 2")]
        public ushort[] StreamName;
    }


    /// <summary>
    /// the query information level of SMB_QUERY_FILE_COMPRESSION_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_QUERY_FILE_COMPRESSION_INFO_OF_TRANS2_QUERY_PATH_INFORMATION
    {
        /// <summary>
        /// A 64-bit signed integer that contains the size, in bytes, of 
        /// the compressed file. This value MUST be greater than or equal to 0.
        /// </summary>
        public ulong CompressedFileSize;

        /// <summary>
        /// A 16-bit unsigned integer that contains the compression format.
        /// The actual compression operation associated with each of 
        /// these compression format values is implementation-dependent. 
        /// An implementation can associate any local compression algorithm
        /// with the values described in the following table, because the 
        /// compressed data does not travel across the wire in the context
        /// of this transaction. The following compression formats are valid only for NTFS.
        /// </summary>
        public ushort CompressionFormat;

        /// <summary>
        /// An 8-bit unsigned integer that contains the compression unit 
        /// shift that is the number of bits by which to left-shift a 1 bit to 
        /// arrive at the compression unit size. The compression unit size 
        /// is the number of bytes in a compression unit, that is, the number 
        /// of bytes to be compressed. This value is implementation-defined.
        /// </summary>
        public byte CompressionUnitShift;

        /// <summary>
        /// An 8-bit unsigned integer that contains the compression chunk 
        /// size in bytes in log 2 format. The chunk size is the number of bytes 
        /// that the operating system's implementation of the Lempel-Ziv 
        /// compression algorithm tries to compress at one time. This value is 
        /// implementation-defined.
        /// </summary>
        public byte ChunkShift;

        /// <summary>
        /// An 8-bit unsigned integer that specifies, in log 2 format,
        /// the amount of space that MUST be saved by compression to successfully
        /// compress a compression unit. If that amount of space is not 
        /// saved by compression, the data in that compression unit MUST be stored 
        /// uncompressed. Each successfully compressed compression unit 
        /// MUST occupy at least one cluster that is less in bytes than an uncompressed
        /// compression unit. Therefore, the cluster shift is the number 
        /// of bits by which to left shift a 1 bit to arrive at the size of a cluster.
        /// This value is implementation defined.
        /// </summary>
        public byte ClusterShift;

        /// <summary>
        /// A 24-bit reserved value. This field SHOULD be set to 0, and MUST be ignored.
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public byte[] Reserved;
    }

    #endregion

    #region 2.2.6.7   	TRANS2_SET_PATH_INFORMATION (0x0006)

    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_SET_PATH_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_SET_PATH_INFORMATION_Request_Trans2_Parameters
    {
        /// <summary>
        /// This field determines the information contained in the request 
        /// Trans2_Data. The request formats are found later in this section. 
        /// See the individual request formats below for detailed information.
        /// A client that has not negotiated long names support MUST use only 
        /// SMB_INFO_STANDARD. If a client that has not negotiated long names 
        /// support uses an InformationLevel other than SMB_INFO_STANDARD, 
        /// the server MUST return an error of NT_STATUS of STATUS_INVALID_PARAMETER 
        /// or SMB error of ERRDOS/ERRinvalidparam.
        /// Name Value
        /// </summary>
        public SetInformationLevel InformationLevel;

        /// <summary>
        /// This field is reserved and MUST be zero (0).
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// The file name or directory name for which to retrieve the information.
        /// </summary>
        public byte[] FileName;
    }


    /// <summary>
    /// the Trans2_Data struct of TRANS2_SET_PATH_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_SET_PATH_INFORMATION_Request_Trans2_Data
    {
        /// <summary>
        /// it is different according to the query information level.
        /// </summary>
        public Object Data;
    }


    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_SET_PATH_INFORMATION Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_SET_PATH_INFORMATION_Response_Trans2_Parameters
    {
        /// <summary>
        /// This field contains the offset in bytes into the ExtendedAttributeList 
        /// that identifies the attribute which caused an error. 
        /// This field is only meaningful when the InformationLevel is set to SMB_INFO_QUERY_EA_FROM_LIST
        /// </summary>
        public ushort EaErrorOffset;
    }


    /// <summary>
    /// the query information level of SMB_INFO_STANDARD_OF_TRANS2_SET_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_INFO_STANDARD_OF_TRANS2_SET_PATH_INFORMATION
    {
        /// <summary>
        /// This field contains the date when the file was created
        /// </summary>
        public SmbDate CreationDate;

        /// <summary>
        /// This field contains the time when the file was created
        /// </summary>
        public SmbTime CreationTime;

        /// <summary>
        /// This field contains the date when the file was last accessed.
        /// </summary>
        public SmbDate LastAccessDate;

        /// <summary>
        /// This field contains the time when the file was last accessed.
        /// </summary>
        public SmbTime LastAccessTime;

        /// <summary>
        /// This field contains the date when data was last written to the file .
        /// </summary>
        public SmbDate LastWriteDate;

        /// <summary>
        /// This field contains the time when data was last written to the file.
        /// </summary>
        public SmbTime LastWriteTime;

        /// <summary>
        /// MUST be set to zero when sent and MUST be ignored on receipt.
        /// </summary>
        [StaticSize(10, StaticSizeMode.Elements)]
        public byte[] Reserved;
    }


    /// <summary>
    /// the query information level of SMB_INFO_SET_EAS_OF_TRANS2_SET_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_INFO_SET_EAS
    {
        /// <summary>
        /// This field MUST contain the total size of the ExtendedAttributeList.
        /// </summary>
        public uint SizeOfListInBytes;

        /// <summary>
        /// A list of extended file attribute name value pairs where the AttributeName 
        /// field values match those that were provided in the request
        /// </summary>
        public SMB_FEA[] ExtendedAttributeList;
    }


    /// <summary>
    /// the query information level of SMB_SET_FILE_BASIC_INFO_OF_TRANS2_SET_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_SET_FILE_BASIC_INFO
    {
        /// <summary>
        /// A 64-bit signed integer that contains the time when the file was created.
        /// A valid time for this field is an integer greater than 0. 
        /// When setting file attributes, a value of 0 indicates to the server
        /// that it MUST NOT change this attribute. When setting file attributes,
        /// a value of -1 indicates to the server that it MUST NOT change this 
        /// attribute for all subsequent operations on the same file handle. 
        /// This field MUST NOT be set to a value less than -1.
        /// </summary>
        public FileTime CreationTime;

        /// <summary>
        /// A 64-bit signed integer that contains the last time the file 
        /// was accessed in the format of a FILETIME structure. A valid time 
        /// for this field is an integer greater than 0. When setting file 
        /// attributes, a value of 0 indicates to the server that it MUST NOT
        /// change this attribute. When setting file attributes, a value of -1
        /// indicates to the server that it MUST NOT change this attribute
        /// for all subsequent operations on the same file handle. This field 
        /// MUST NOT be set to a value less than -1.
        /// </summary>
        public FileTime LastAccessTime;

        /// <summary>
        /// A 64-bit signed integer that contains the last time information
        /// was written to the file in the format of a FILETIME structure. 
        /// A valid time for this field is an integer greater than 0. 
        /// When setting file attributes, a value of 0 indicates to the server 
        /// that it MUST NOT change this attribute. When setting file attributes,
        /// a value of -1 indicates to the server that it MUST NOT 
        /// change this attribute for all subsequent operations on the same 
        /// file handle. This field MUST NOT be set to a value less than -1
        /// </summary>
        public FileTime LastWriteTime;

        /// <summary>
        /// A 64-bit signed integer that contains the last time the 
        /// file was changed in the format of a FILETIME structure.
        /// A valid time for this field is an integer greater than 0.
        /// When setting file attributes, a value of 0 indicates to the server 
        /// that it MUST NOT change this attribute. When setting file 
        /// attributes, a value of -1 indicates to the server that it MUST NOT 
        /// change this attribute for all subsequent operations on the
        /// same file handle. This field MUST NOT be set to a value less than -1.
        /// </summary>
        public FileTime ChangeTime;

        /// <summary>
        /// A 32-bit unsigned integer that contains the file attributes to be set.
        /// </summary>
        public SmbFileAttributes32 FileAttributes;

        /// <summary>
        /// A 32-bit field. This field is reserved. This field can be set to any value, and MUST be ignored.
        /// </summary>
        public uint Reserved;
    }


    /// <summary>
    /// the query information level of SMB_SET_FILE_DISPOSITION_INFO_OF_TRANS2_SET_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_SET_FILE_DISPOSITION_INFO
    {
        /// <summary>
        /// An 8-bit field that is set to 1 to indicate that a file
        /// SHOULD be deleted when it is closed; otherwise, 0
        /// </summary>
        public byte DeletePending;
    }


    /// <summary>
    /// the query information level of SMB_SET_FILE_ALLOCATION_INFO_OF_TRANS2_SET_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_SET_FILE_ALLOCATION_INFO
    {
        /// <summary>
        /// A 64-bit signed integer containing the file allocation size
        /// in bytes. Usually, this value is a multiple of the 
        /// sector or cluster size of the underlying physical device.
        /// This value MUST be greater than or equal to 0.
        /// All unused allocation (beyond EOF) is freed.
        /// </summary>
        public ulong AllocationSize;
    }


    /// <summary>
    /// the query information level of SMB_SET_FILE_END_OF_FILE_INFO_OF_TRANS2_SET_PATH_INFORMATION
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_SET_FILE_END_OF_FILE_INFO
    {
        /// <summary>
        /// A 64-bit signed integer that contains the absolute new end
        /// of file position as a byte offset from the start of the file.
        /// EndOfFile specifies the offset from the beginning of the file
        /// to the byte following the last byte in the file. 
        /// It is the offset from the beginning of the file at which new
        /// bytes appended to the file are to be written. 
        /// The value of this field MUST be greater than or equal to 0.
        /// </summary>
        public ulong EndOfFile;
    }

    #endregion

    #region 2.2.6.8   	TRANS2_QUERY_FILE_INFORMATION (0x0007)

    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_QUERY_FILE_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_QUERY_FILE_INFORMATION_Request_Trans2_Parameters
    {
        /// <summary>
        /// This field MUST contain a valid FID returned from a previously successful SMB open command
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This field determines the information contained in the response.
        /// See TRANS2_QUERY_PATH_INFORMATION for complete details
        /// </summary>
        public QueryInformationLevel InformationLevel;
    }

    #endregion

    #region 2.2.6.9   	TRANS2_SET_FILE_INFORMATION (0x0008)

    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_SET_FILE_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_SET_FILE_INFORMATION_Request_Trans2_Parameters
    {
        /// <summary>
        /// This field MUST contain a valid FID returned from a previously successful SMB open command.
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This field determines the information contained in the response.
        /// See TRANS2_SET_PATH_INFORMATION for complete details
        /// </summary>
        public SetInformationLevel InformationLevel;

        /// <summary>
        /// Reserved, but not implemented.
        /// </summary>
        public ushort Reserved;
    }


    /// <summary>
    /// the Trans2_Data struct of TRANS2_SET_FILE_INFORMATION Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_SET_FILE_INFORMATION_Request_Trans2_Data
    {
        /// <summary>
        /// it is different according to the set path information level.
        /// </summary>
        public Object Data;
    }

    #endregion

    // 2.2.6.10   	TRANS2_FSCTL (0x0009)
    // This Transaction2 subcommand was introduced in the LAN Manager 2.0 dialect.
    // This subcommand is reserved but not implemented. 
    // Clients SHOULD NOT send requests using this command code. Servers receiving 
    // requests with this command code MUST return STATUS_NOT_IMPLEMENTED (ERRDOS/ERRbadfunc).

    // 2.2.6.11   	TRANS2_IOCTL2 (0x000A)
    // This Transaction2 subcommand was introduced in the NT LAN Manager dialect.
    // This subcommand is reserved but not implemented. 
    // Clients SHOULD NOT send requests using this command code. Servers receiving
    // requests with this command code MUST return STATUS_NOT_IMPLEMENTED (ERRDOS/ERRbadfunc).

    // 2.2.6.12   	TRANS2_FIND_NOTIFY_FIRST (0x000B)
    // This Transaction2 subcommand was introduced in the LAN Manager 2.0 dialect. 
    // It was rendered obsolete in the NT LAN Manager dialect. 
    // Clients SHOULD NOT send requests using this command code. Servers receiving 
    // requests with this command code MUST return STATUS_NOT_IMPLEMENTED (ERRDOS/ERRbadfunc).

    // 2.2.6.13   	TRANS2_FIND_NOTIFY_NEXT (0x000C)
    // This Transaction2 subcommand was introduced in the LAN Manager 2.0 dialect. 
    // It was rendered obsolete in the NT LAN Manager dialect.
    // Clients SHOULD NOT send requests using this command code. Servers receiving
    // requests with this command code MUST return STATUS_NOT_IMPLEMENTED (ERRDOS/ERRbadfunc).

    #region 2.2.6.14   	TRANS2_CREATE_DIRECTORY (0x000D)

    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_CREATE_DIRECTORY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_CREATE_DIRECTORY_Request_Trans2_Parameters
    {
        /// <summary>
        /// This field is reserved and MUST be zero (0).
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// The directory name to assign to the new directory
        /// </summary>
        public byte[] DirectoryName;
    }


    /// <summary>
    /// the Trans2_Data struct of TRANS2_CREATE_DIRECTORY Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_CREATE_DIRECTORY_Request_Trans2_Data
    {
        /// <summary>
        /// A list of extended file attribute name value pairs where the AttributeName
        /// field values match those that were provided in the request
        /// </summary>
        public SMB_FEA_LIST ExtendedAttributeList;
    }


    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_CREATE_DIRECTORY Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_CREATE_DIRECTORY_Response_Trans2_Parameters
    {
        /// <summary>
        /// This field contains the offset in bytes into the ExtendedAttributeList 
        /// that identifies the attribute that caused an error. 
        /// This field is only meaningful when the request included Trans2_Data
        /// </summary>
        public ushort EaErrorOffset;
    }

    #endregion

    // 2.2.6.15   	TRANS2_SESSION_SETUP (0x000E)
    // This Transaction2 subcommand was introduced in the NT LAN Manager
    // dialect. This subcommand is reserved but not implemented. 
    // Clients SHOULD NOT send requests using this command code. Servers 
    // receiving requests with this command code SHOULD return STATUS_NOT_IMPLEMENTED 
    // or ERRDOS (0x01) and ERRbadfunc (0x0001). 

    #region 2.2.6.16   	TRANS2_GET_DFS_REFERRAL (0x0010)

    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_GET_DFS_REFERRAL Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_GET_DFS_REFERRAL_Request_Trans2_Parameters
    {
        /// <summary>
        /// REFERRAL This field MUST be a properly formatted DFS referral request, 
        /// as specified in [MS-DFSC] section 2.2.2
        /// </summary>
        public REQ_GET_DFS_REFERRAL ReferralRequest;
    }


    /// <summary>
    /// the Trans2_Data struct of TRANS2_GET_DFS_REFERRAL Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_GET_DFS_REFERRAL_Response_Trans2_Data
    {
        /// <summary>
        /// field MUST be a properly formatted DFS referral response, as specified in [MS-DFSC] section 2.2.3
        /// </summary>
        public RESP_GET_DFS_REFERRAL ReferralResponse;
    }


    /// <summary>
    /// A DFS client sends a DFS referral request using the REQ_GET_DFS_REFERRAL message.
    /// The format of this message is as follows.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct REQ_GET_DFS_REFERRAL
    {
        /// <summary>
        /// A 16-bit integer that indicates the highest DFS referral version understood by the client.
        /// </summary>
        public ushort MaxReferralLevel;

        /// <summary>
        /// A null-terminated Unicode string specifying the path to be resolved. 
        /// The specified path MUST NOT be case-sensitive.
        /// Its format depends on the type of referral request: Domain referral:
        /// The path MUST be an empty string (containing 
        /// just the null terminator). A client MUST use DFS referral
        /// version 3 or later for a domain referral request.
        /// </summary>
        public byte[] RequestFileName;
    }


    /// <summary>
    /// A DFS server responds to a DFS client referral request with the
    /// RESP_GET_DFS_REFERRAL message. The fixed-length portion of this message
    /// is referred to as the "referral header" in this document. 
    /// The format of this message is as follows.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RESP_GET_DFS_REFERRAL
    {
        /// <summary>
        /// A 16-bit integer indicating the number of bytes?not characters?
        /// in the prefix of the referral request path that is matched 
        /// in the referral response.
        /// </summary>
        public ushort PathConsumed;

        /// <summary>
        /// A 16-bit integer indicating the number of referral entries
        /// immediately following the referral header
        /// </summary>
        public ushort NumberOfReferrals;

        /// <summary>
        /// A 32-bit field representing a series of flags that 
        /// are combined by using the bitwise OR operation. 
        /// Only the R, S, and T bits are defined and used. 
        /// The other bits MUST be set to 0 by the server and
        /// ignored upon receipt by the client
        /// </summary>
        public ReferralHeaderFlags ReferralHeaderFlags;

        /// <summary>
        /// As many DFS_REFERRAL_V1, DFS_REFERRAL_V2 structures as indicated by the NumberOfReferrals field.
        /// </summary>
        public Object ReferralEntries;
    }


    /// <summary>
    /// A 32-bit field representing a series of flags that are
    /// combined by using the bitwise OR operation. Only the R, S, and T bits are defined and 
    /// used. The other bits MUST be set to 0 by the server and ignored upon receipt by the client
    /// </summary>
    [Flags()]
    public enum ReferralHeaderFlags : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// The R bit MUST be set to 1 if all of the targets in the referral 
        /// entries returned are DFS root targets capable of handling DFS 
        /// referral requests and set to 0 otherwise.
        /// </summary>
        R = 0x00000001,

        /// <summary>
        /// The S bit MUST be set to 1 if all of the targets in the referral
        /// response can be accessed without requiring further referral
        /// requests and set to 0 otherwise.
        /// </summary>
        S = 0x00000002,

        /// <summary>
        /// The T bit MUST be set to 1 if DFS client target failback is
        /// enabled for all targets in this referral response. 
        /// This value MUST be set to 0 by the server and ignored by
        /// the client for all DFS referral versions except DFS referral version 4.
        /// </summary>
        T = 0x00000004,
    }


    /// <summary>
    /// The format of the version 1 referral entry is as follows.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DFS_REFERRAL_V1
    {
        /// <summary>
        /// A 16-bit integer indicating the version number of the
        /// referral entry. MUST always be 0x0001 for DFS_REFERRAL_V1.
        /// </summary>
        public ushort VersionNumber;

        /// <summary>
        /// A 16-bit integer indicating the total size of the referral entry in bytes.
        /// </summary>
        public ushort Size;

        /// <summary>
        /// A 16-bit integer indicating the type of server hosting the target.
        /// This field MUST be set to 0x0001 if DFS root targets are returned,
        /// and to 0x0000 otherwise. Note that sysvol targets are not DFS root 
        /// targets; the field MUST be set to 0x0000 for a sysvol referral response.
        /// </summary>
        public ushort ServerType;

        /// <summary>
        /// A series of bit flags. MUST be set to 0x0000 and ignored on receipt.
        /// </summary>
        public ushort ReferralEntryFlags;

        /// <summary>
        /// A null-terminated Unicode character string that specifies a DFS target.
        /// </summary>
        [Size("Size - 8")]
        public byte[] ShareName;
    }


    /// <summary>
    /// The format of the version 2 referral entry is as follows.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DFS_REFERRAL_V2
    {
        /// <summary>
        /// A 16-bit integer indicating the version number of the referral entry.
        /// MUST always be 0x0002 for DFS_REFERRAL_V2
        /// </summary>
        public ushort VersionNumber;

        /// <summary>
        /// A 16-bit integer indicating the total size of the referral entry in bytes
        /// </summary>
        public ushort Size;

        /// <summary>
        /// A 16-bit integer indicating the type of server hosting the target. 
        /// This field MUST be set to 0x0001 if DFS root targets are returned, 
        /// and to 0x0000 otherwise. Note that sysvol targets are not DFS root 
        /// targets; the field MUST be set to 0x0000 for a sysvol referral 
        /// response.
        /// </summary>
        public ushort ServerType;

        /// <summary>
        /// MUST be set to 0x0000 by the server and ignored on receipt by the client.
        /// </summary>
        public ushort ReferralEntryFlags;

        /// <summary>
        /// MUST be set to 0x00000000 by the server and ignored by the client
        /// </summary>
        public uint Proximity;

        /// <summary>
        /// A 32-bit integer indicating the time-out value, in seconds, of the 
        /// DFS root or DFS link. MUST be set to the time-out value of the
        /// DFS root or the DFS link in the DFS metadata for which 
        /// the referral response is being sent.
        /// When there is more than one referral entry, the TimeToLive
        /// of each referral entry MUST be the same
        /// </summary>
        public uint TimeToLive;

        /// <summary>
        /// A 16-bit integer indicating the offset, in bytes, from 
        /// the beginning of this referral entry to the DFS path that corresponds to 
        /// the DFS root or the DFS link for which target information is returned.
        /// </summary>
        public ushort DFSPathOffset;

        /// <summary>
        /// A 16-bit integer indicating the offset, in bytes, from the beginning of this referral 
        /// entry to the DFS path that corresponds to the DFS root or the DFS
        /// link for which target information is returned. This path MAY either be the same as the 
        /// path as pointed to by the DFSPathOffset field or be an 8.3 name. In the former case, 
        /// the string referenced MAY be the same as that in the DFSPathOffset field or a duplicate copy.
        /// </summary>
        public ushort DFSAlternatePathOffset;

        /// <summary>
        /// A 16-bit integer indicating the offset, in bytes, from beginning of this
        /// referral entry to the DFS target path that correspond 
        /// to this entry.
        /// </summary>
        public ushort NetworkAddressOffset;

        /// <summary>
        /// DFS root or the DFS link
        /// </summary>
        public string DFSPath;

        /// <summary>
        /// DFS root or the DFS link
        /// </summary>
        public string DFSAlternatePath;

        /// <summary>
        /// DFS target path
        /// </summary>
        public string DFSTargetPath;
    }

    #endregion

    // 2.2.6.17   	TRANS2_REPORT_DFS_INCONSISTENCY (0x0011)
    // This Transaction2 subcommand was introduced in the NT LAN Manager
    // dialect. This subcommand is reserved but not implemented. 
    // Clients SHOULD NOT send requests using this command code. Servers
    // receiving requests with this command code SHOULD return STATUS_NOT_IMPLEMENTED (ERRDOS/ERRbadfunc). 

    #endregion


    #region 2.2.7 NT Transact Subcommands

    #region 2.2.7.1   	NT_TRANSACT_CREATE (0x0001)

    /// <summary>
    /// the NT_Trans_Parameters struct of NT_TRANSACT_CREATE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_CREATE_Request_NT_Trans_Parameters
    {
        /// <summary>
        /// A 32-bit field containing a set of flags that modify the client
        /// request. Unused bits SHOULD be set to 0 by the client when sending
        /// a message and MUST be ignored when received by the server. 
        /// </summary>
        public NtTransactFlags Flags;

        /// <summary>
        /// If nonzero, this value is the FID of an opened root directory, 
        /// and the Name field MUST be handled as relative to the directory
        /// specified by this FID. If this value is zero the Name field MUST 
        /// be handled as relative to the root of the share (the TID). 
        /// The FID MUST have been acquired in a previous message exchange
        /// </summary>
        public uint RootDirectoryFID;

        /// <summary>
        /// A 32-bit field containing standard, specific, 
        /// and generic access rights. These rights are used in 
        /// access-control entries (ACEs) and are the primary
        /// means of specifying the requested or granted access to an object.
        /// If this value is 0, it represents a request to query 
        /// the attributes without access the file. If the value is not 0, the bits 
        /// represent requests for the following types of access:
        /// </summary>
        public NtTransactDesiredAccess DesiredAccess;

        /// <summary>
        /// The client MUST set this value to the initial allocation
        /// size of the file in bytes. The server MUST ignore this field if this request
        /// is to open an existing file. This field MUST be used only
        /// if the file is created or overwritten. The value MUST be set to 0 in all 
        /// other cases. This does not apply to directory related requests.
        /// This is the number of bytes to be allocated represented as a 64 bit
        /// integer value
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// A 32-bit field containing encoded file attribute values and
        /// file access behavior flag values. The attribute and flag value names
        /// are for reference purposes only. If ATTR_NORMAL (see below) is 
        /// set as the requested attribute value, it MUST be the only attribute
        /// value set in order to have the desired effect. Including any other
        /// attribute value causes the ATTR_NORMAL value to be ignored.
        /// Any combination of the flag values (see below) is acceptable
        /// </summary>
        public SMB_EXT_FILE_ATTR ExtFileAttributes;

        /// <summary>
        /// A 32-bit field that specifies how the file SHOULD be shared
        /// with other processes. The names in the table below are provided for 
        /// reference use only. The value MUST be FILE_SHARE_NONE or some
        /// combination of the other values
        /// </summary>
        public NtTransactShareAccess ShareAccess;

        /// <summary>
        /// A 32-bit value that represents the action to take if the file
        /// already exists or if the file is a new file and does not already exist
        /// </summary>
        public NtTransactCreateDisposition CreateDisposition;

        /// <summary>
        /// A 32-bit field containing flag options to use if creating the 
        /// file or directory. This field MUST be set to 0 or a combination of the
        /// following possible values. Unused bit fields SHOULD be set to 0
        /// by the client when sending a request and SHOULD be ignored when
        /// received by the server. Below is a list of the valid values and 
        /// their associated behavior
        /// </summary>
        public NtTransactCreateOptions CreateOptions;

        /// <summary>
        /// Length of Security Descriptor in bytes
        /// </summary>
        public uint SecurityDescriptorLength;

        /// <summary>
        /// Length of Extended Attributes in bytes
        /// </summary>
        public uint EALength;

        /// <summary>
        /// Length of the file name in characters
        /// </summary>
        public uint NameLength;

        /// <summary>
        /// A value that indicates what security context the server SHOULD use
        /// when executing the command on behalf of the client. 
        /// Value names are provided for convenience only. Supported values are: 
        /// </summary>
        public NtTransactImpersonationLevel ImpersonationLevel;

        /// <summary>
        /// A 32-bit field containing a set of options that specify the
        /// security tracking mode. These options specify whether the server is
        /// to be given a snapshot of the client's security context (called 
        /// static tracking) or is to be continually updated to track changes
        /// to the client's security context (called dynamic tracking). 
        /// When bit 0 of the SecurityFlags field is set to FALSE, static tracking
        /// is requested. When bit 0 the SecurityFlags field is set to TRUE, 
        /// dynamic tracking is requested. Unused bit fields SHOULD be set to
        /// 0 by the client when sending a request and MUST be ignored when 
        /// received by the server. This field MUST be set to 0 or a combination 
        /// of the following possible values. Value names are provided for 
        /// convenience only. Supported values are: 
        /// </summary>
        public NtTransactSecurityFlags SecurityFlags;

        /// <summary>
        /// The name of the file; not null-terminated.  If SMB_FLAGS2_UNICODE
        /// is set in the Flags2 field of the SMB header of the request, 
        /// this field MUST be an array of 16-bit Unicode characters. Otherwise,
        /// it MUST be an array of extended ASCII (OEM) characters. 
        /// If the Name consists of Unicode characters, this field MUST
        /// be aligned to start on a 2-byte boundary from the start of the SMB header
        /// </summary>
        [Size("NameLength")]
        public byte[] Name;
    }


    /// <summary>
    /// the NT_Trans_Data struct of NT_TRANSACT_CREATE Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_CREATE_Request_NT_Trans_Data
    {
        /// <summary>
        /// The security descriptor to use when requesting access to the file. 
        /// The self-relative form of a SECURITY_DESCRIPTOR MUST be used.
        /// See section 2.4.6 SECURITY_DESCRIPTOR of [MS-DTYPE] for the details
        /// </summary>
        public RawSecurityDescriptor SecurityDescriptor;

        /// <summary>
        /// The list of extended attributes that SHOULD be applied to the new file.
        /// </summary>
        public FILE_FULL_EA_INFORMATION[] ExtendedAttributes;
    }


    /// <summary>
    /// This information class is used to query or set extended attribute (EA) information for a file.
    /// </summary>
    public struct FILE_FULL_EA_INFORMATION
    {
        /// <summary>
        /// 4-byte aligned integer that contains the byte offset from the beginning of this entry, at which the next
        /// FILE_FULL_EA_INFORMATION entry is located
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// An 8-bit unsigned integer that contains one of the following flag values
        /// </summary>
        public FULL_EA_FLAGS Flags;

        /// <summary>
        /// An 8-bit unsigned integer that contains the length, in bytes, of the extended attribute name in the
        /// EaName field. This value MUST NOT include the terminating null character to EaName.
        /// </summary>
        public byte EaNameLength;

        /// <summary>
        /// A 16-bit unsigned integer that contains the length, in bytes, of the extended attribute value in the
        /// EaValue field.
        /// </summary>
        public ushort EaValueLength;

        /// <summary>
        /// An array of 8-bit ASCII characters that contains the extended attribute name followed by a single
        /// null-termination character byte.
        /// </summary>
        [Size("EaNameLength+1")]
        public byte[] EaName;

        /// <summary>
        /// An array of bytes that contains the extended attribute value.
        /// </summary>
        [Size("EaValueLength")]
        public byte[] EaValue;
    }


    /// <summary>
    /// the FULL_EA_FLAGS.
    /// </summary>
    [Flags()]
    public enum FULL_EA_FLAGS : byte
    {
        /// <summary>
        /// The file does not use EAs.
        /// </summary>
        NONE = 0x00,

        /// <summary>
        /// If this flag is set, the file to which the EA belongs cannot be interpreted without understanding 
        /// the associated extended attributes.
        /// </summary>
        FILE_NEED_EA = 0x80,
    }


    /// <summary>
    /// the NT_Trans_Parameters struct of NT_TRANSACT_CREATE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_CREATE_Response_NT_Trans_Parameters
    {
        /// <summary>
        /// The oplock level granted to the client process
        /// </summary>
        public OplockLevelValue OpLockLevel;

        /// <summary>
        /// Reserved and MUST be zero (0).
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// The file ID value representing the file or directory that was created or opened.
        /// </summary>
        public ushort FID;

        /// <summary>
        /// A 32-bit value that represents the action to take if the file already exists or if the file is a new file
        /// and does not already exist.
        /// </summary>
        public NtTransactCreateActionValues CreateAction;

        /// <summary>
        /// Offset of the extended attribute that caused an error if an error occurred with an extended attribute.
        /// </summary>
        public uint EAErrorOffset;

        /// <summary>
        /// A 64 bit integer value representing the time the file was created.
        /// The time value is a signed 64 bit integer representing either
        /// an absolute time or a time interval. Times are specified in units
        /// of 100ns. A positive value expresses an absolute time, 
        /// where the base time (the 64- bit integer with value 0) is the
        /// beginning of the year 1601 AD in the Gregorian calendar. 
        /// A negative value expresses a time interval relative to some
        /// base time, usually the current time.
        /// </summary>
        public FileTime CreationTime;

        /// <summary>
        /// The time the file was last accessed encoded in the same format as CreateTime (see above).
        /// </summary>
        public FileTime LastAccessTime;

        /// <summary>
        /// The time the file was last written, encoded in the same format as CreateTime (see above).
        /// </summary>
        public FileTime LastWriteTime;

        /// <summary>
        /// The time the file was last changed, encoded in the same format as CreateTime (see above).
        /// </summary>
        public FileTime LastChangeTime;

        /// <summary>
        /// A 32 bit value composed of encoded file attribute values 
        /// and file access behavior flag values.  See NT_Trans_Parameters.ExtFileAttributes 
        /// in the request description above for the encoding. This 
        /// value provides the attributes the server assigned to the file or directory as a
        /// result of the command
        /// </summary>
        public SMB_EXT_FILE_ATTR ExtFileAttributes;

        /// <summary>
        /// The number of bytes allocated to the file by the server.
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// The end of file offset value
        /// </summary>
        public ulong EndOfFile;

        /// <summary>
        /// The file type
        /// </summary>
        public FileTypeValue ResourceType;

        /// <summary>
        /// A 16-bit field that contains the state of the named pipe if the
        /// FID represents a named pipe instance. This value MUST be
        /// any combination of the following bit values. Unused bit fields
        /// SHOULD be set to 0 by the server when sending a response and
        /// MUST be ignored when received by the client. 
        /// </summary>
        public SMB_NMPIPE_STATUS NMPipeStatus;

        /// <summary>
        /// If the returned FID represents a directory then the server
        /// MUST set this value to a non-zero value. If the FID is 
        /// not a directory then the server MUST set this value to 0 (FALSE).
        /// </summary>
        public byte Directory;
    }

    #endregion

    #region 2.2.7.2   	NT_TRANSACT_IOCTL (0x0002)

    /// <summary>
    /// the NT_Trans_Parameters struct of NT_TRANSACT_IOCTL Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_IOCTL_SETUP
    {
        /// <summary>
        /// Windows NT device or file system control code
        /// </summary>
        public uint FunctionCode;

        /// <summary>
        /// MUST contain a valid FID obtained from a previously successful
        /// SMB open command. The Fid MUST be for either an I/O device
        /// or a file system control device. The type of FID being supplied 
        /// is specified by IsFctl
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This field is TRUE if the command is a file system control
        /// command and the FID is a file system control device. 
        /// Otherwise, the command is a device control command and FID is an I/O device.
        /// </summary>
        public byte IsFctl;

        /// <summary>
        /// If bit 0 is set, the command is to be applied to a share root
        /// handle. The share MUST be a Distributed File System (DFS) type
        /// </summary>
        public byte IsFlags;
    }


    /// <summary>
    /// the NT_Trans_Data struct of NT_TRANSACT_IOCTL Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_IOCTL_Request_NT_Trans_Data
    {
        /// <summary>
        /// The raw bytes that are passed to the fsctl or ioctl function as the input buffer.
        /// </summary>
        public byte[] Data;
    }


    /// <summary>
    /// FILESYSTEM_STATISTICS data element
    /// </summary>
    public struct FsCtlCode_FILESYSTEM_STATISTICS
    {
        /// <summary>
        /// A 16-bit unsigned integer value containing the type of file system.
        /// This field MUST contain one of the following values.
        /// </summary>
        public ushort FileSystemType;

        /// <summary>
        /// A 16-bit unsigned integer value containing the version. This field MUST contain 0x00000001.
        /// </summary>
        public ushort Version;

        /// <summary>
        /// A 32-bit unsigned integer value that indicates the size, in bytes, of this structure plus the size of the
        /// file system-specific structure that follows this structure, rounded up to a multiple of 64, and then
        /// multiplied by the number of processors.
        /// </summary>
        public uint SizeOfCompleteStructure;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number of read operations on user files.
        /// </summary>
        public uint UserFileReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number of bytes read from user files.
        /// </summary>
        public uint UserFileReadBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number of read operations on user files that went to the
        /// disk rather than the cache. This value includes sub-read operations.
        /// </summary>
        public uint UserDiskReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number of write operations on user files.
        /// </summary>
        public uint UserFileWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number of bytes written to user files.
        /// </summary>
        public uint UserFileWriteBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number of write operations on user files that went to disk
        /// rather than the cache.
        /// </summary>
        public uint UserDiskWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number of read operations on metadata files.
        /// </summary>
        public uint MetaDataReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number of bytes read from metadata files.
        /// </summary>
        public uint MetaDataReadBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number of read operations on metadata files.
        /// </summary>
        public uint MetaDataDiskReads;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number of write operations on metadata files.
        /// </summary>
        public uint MetaDataWrites;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number of bytes written to metadata files.
        /// </summary>
        public uint MetaDataWriteBytes;

        /// <summary>
        /// A 32-bit unsigned integer value containing the number of write operations on metadata files
        /// </summary>
        public uint MetaDataDiskWrites;
    }


    /// <summary>
    /// the NT_Trans_Data struct of NT_TRANSACT_IOCTL Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_IOCTL_Response_NT_Trans_Data
    {
        /// <summary>
        /// Results returned by either an I/O device or a file system control command. 
        /// The results are the raw bytes returned from the command
        /// if the command was successful.
        /// </summary>
        public byte[] Data;
    }

    #endregion

    #region 2.2.7.3   	NT_TRANSACT_SET_SECURITY_DESC (0x0003)

    /// <summary>
    /// the NT_Trans_Parameters struct of NT_TRANSACT_SET_SECURITY_DESC Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_SET_SECURITY_DESC_Request_NT_Trans_Parameters
    {
        /// <summary>
        /// File identifier or handle of the target file
        /// </summary>
        public ushort FID;

        /// <summary>
        /// Reserved. This value MUST be 0
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// Fields of security descriptor to be set. This is a bit field.
        /// These values MAY be logically OR-ed together to set
        /// several descriptors in one request. The server MUST set only 
        /// the descriptors requested by SecurityInformation
        /// </summary>
        public NtTransactSecurityInformation SecurityInformation;
    }


    /// <summary>
    /// the NT_Trans_Data struct of NT_TRANSACT_SET_SECURITY_DESC Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_SET_SECURITY_DESC_Request_NT_Trans_Data
    {
        /// <summary>
        /// The requested security descriptor structure. The self-relative 
        /// form of a SECURITY_DESCRIPTOR is required. 
        /// See section 2.4.6 SECURITY_DESCRIPTOR of [MS-DTYPE] for the details. 
        /// </summary>
        public RawSecurityDescriptor SecurityInformation;
    }

    #endregion

    #region 2.2.7.4   	NT_TRANSACT_NOTIFY_CHANGE (0x0004)

    /// <summary>
    /// the struct of NT_TRANSACT_NOTIFY_CHANGE_Setup
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_NOTIFY_CHANGE_Setup
    {
        /// <summary>
        /// A 64-bit field of flags that specify the types of operations to monitor.
        /// </summary>
        public uint CompletionFilter;

        /// <summary>
        /// The FID of the directory to monitor
        /// </summary>
        public ushort FID;

        /// <summary>
        /// If all subdirectories are to be watched, then set this to TRUE; otherwise, FALSE.
        /// </summary>
        public bool WatchTree;

        /// <summary>
        /// Reserved. This value MUST be 0.
        /// </summary>
        public byte Reserved;
    }


    /// <summary>
    /// the NT_Trans_Parameters struct of NT_TRANSACT_NOTIFY_CHANGE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_NOTIFY_CHANGE_Response_NT_Trans_Parameters
    {
        /// <summary>
        /// The response contains FILE_NOTIFY_INFORMATION structures, as defined below. 
        /// The NextEntryOffset field of the structure 
        /// specifies the offset, in bytes, from the start of the current entry
        /// to the next entry in the list. If this is the last
        /// entry in the list, this field is zero. Each entry in the list 
        /// MUST be longword aligned, so NextEntryOffset MUST be a multiple of 4.
        /// </summary>
        public FILE_NOTIFY_INFORMATION[] FileNotifyInformation;
    }


    /// <summary>
    /// The response contains FILE_NOTIFY_INFORMATION structures, as 
    /// defined below. The NextEntryOffset field of the structure 
    /// specifies the offset, in bytes, from the start of the current 
    /// entry to the next entry in the list. If this is the last entry
    /// in the list, this field is zero. Each entry in the list MUST
    /// be longword aligned, so NextEntryOffset MUST be a multiple of 4.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FILE_NOTIFY_INFORMATION
    {
        /// <summary>
        /// Offset, in bytes, from the start of the current entry to
        /// the next entry in the list. Each entry in the list MUST
        /// be 4-byte aligned, so NextEntryOffset MUST be a multiple
        /// of 4. If this is the last entry in the list, the value is 0.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// Action taken for the specified file name.
        /// </summary>
        public FILE_ACTION Action;

        /// <summary>
        /// Length, in bytes, of the name of the changed file
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// The file name of the file that has been changed. 
        /// The name MUST NOT be null-terminated. The name is either in the OEM character
        /// set or in Unicode characters, depending upon the state of the
        /// SMB_FLAGS2_UNICODE bit in the Flags2 field of the SMB header. 
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName1;
    }


    /// <summary>
    /// Action taken for the specified file name.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum FILE_ACTION : uint
    {
        /// <summary>
        /// File is added
        /// </summary>
        FILE_ACTION_ADDED = 0x00000001,

        /// <summary>
        /// File is removed
        /// </summary>
        FILE_ACTION_REMOVED = 0x00000002,

        /// <summary>
        /// File is modified
        /// </summary>
        FILE_ACTION_MODIFIED = 0x00000003,

        /// <summary>
        /// File is renamed
        /// </summary>
        FILE_ACTION_RENAMED_OLD_NAME = 0x00000004,

        /// <summary>
        /// File is renamed
        /// </summary>
        FILE_ACTION_RENAMED_NEW_NAME = 0x00000005,

        /// <summary>
        /// File is added
        /// </summary>
        FILE_ACTION_ADDED_STREAM = 0x00000006,

        /// <summary>
        /// File is removed
        /// </summary>
        FILE_ACTION_REMOVED_STREAM = 0x00000007,

        /// <summary>
        /// File is modified
        /// </summary>
        FILE_ACTION_MODIFIED_STREAM = 0x00000008,
    }


    #endregion

    // 2.2.7.5   	NT TRANSACT RENAME (0x0005)
    // This is NT Transaction subcommand was introduced in the
    // NT LAN Manager dialect. This subcommand was reserved but not implemented. 
    // Clients SHOULD NOT send requests using this subcommand code.
    // Servers receiving requests with this subcommand code MUST return STATUS_SMB_BAD_COMMAND (ERRSRV/ERRbadcmd).

    #region 2.2.7.6   	NT_TRANSACT_QUERY_SECURITY_DESC (0x0006)

    /// <summary>
    /// the NT_Trans_Parameters struct of NT_TRANSACT_QUERY_SECURITY_DESC Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_QUERY_SECURITY_DESC_Request_NT_Trans_Parameters
    {
        /// <summary>
        /// Fid of the target file. The Fid MUST have been obtained through 
        /// a previously successful SMB open request
        /// </summary>
        public ushort FID;

        /// <summary>
        /// Reserved. This value MUST be 0.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// A 32-bit field representing the requested fields of the
        /// security descriptor to be retrieved. These values MAY be
        /// logically OR-ed together to request several descriptors 
        /// in one request. The descriptor response format contains storage
        /// for all of the descriptors. The client MUST ignore the
        /// values returned for descriptors who's bit was not included in this
        /// field as part of the request.
        /// </summary>
        public NtTransactSecurityInformation SecurityInfoFields;
    }


    /// <summary>
    /// the NT_Trans_Parameters struct of NT_TRANSACT_QUERY_SECURITY_DESC Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_QUERY_SECURITY_DESC_Response_NT_Trans_Parameters
    {
        /// <summary>
        /// The length of the returned SecurityDescriptor field.
        /// </summary>
        public uint LengthNeeded;
    }


    /// <summary>
    /// the NT_Trans_Data struct of NT_TRANSACT_QUERY_SECURITY_DESC Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_QUERY_SECURITY_DESC_Response_NT_Trans_Data
    {
        /// <summary>
        /// The requested security descriptor structure. The self-relative 
        /// form of a SECURITY_DESCRIPTOR is returned.
        /// See section 2.4.6 SECURITY_DESCRIPTOR of [MS-DTYPE] for complete details.
        /// </summary>
        public RawSecurityDescriptor SecurityInformation;
    }

    #endregion

    #endregion


    #region Other message structs undifined in TD

    /// <summary>
    /// Array of USHORT in subcommand of Transaction
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TransSetup
    {
        /// <summary>
        /// This field MUST be set to the transaction subcommand value 
        /// </summary>
        public ushort Subcommand;

        /// <summary>
        /// This field MUST be set to the FID for the named pipe to read.
        /// This field MUST be set to a valid FID from a server response
        /// for a previous SMB command to open or create a named pipe. 
        /// These commands include SMB_COM_OPEN, SMB_COM_CREATE, SMB_COM_CREATE_TEMPORARY,
        /// SMB_COM_CREATE_NEW, SMB_COM_OPEN_ANDX, SMB_COM_NT_CREATE_ANDX, 
        /// and SMB_COM_NT_TRANSACT with subcommand NT_TRANSACT_CREATE. 
        /// </summary>
        public ushort FID;
    }


    /// <summary>
    /// Array of USHORT in subcommand of Transaction
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Trans_Wait_Call_Setup
    {
        /// <summary>
        /// This field MUST be set to the transaction subcommand value 
        /// </summary>
        public ushort Subcommand;

        /// <summary>
        /// This field MUST be in the range of 0 to 9. The larger value being the higher priority. 
        /// </summary>
        public ushort Priority;
    }


    /// <summary>
    /// A 32-bit field containing a set of flags that modify the client request.
    /// Unused bits SHOULD be set to 0 by the client when sending
    /// a message and MUST be ignored when received by the server. 
    /// </summary>
    [Flags()]
    public enum NtTransactFlags : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0x00000000,
        /// <summary>
        /// Level I oplock requested
        /// </summary>
        NT_CREATE_REQUEST_OPLOCK = 0x00000002,

        /// <summary>
        /// Batch oplock requested
        /// </summary>
        NT_CREATE_REQUEST_OPBATCH = 0x00000004,

        /// <summary>
        /// Target for open is a directory
        /// </summary>
        NT_CREATE_OPEN_TARGET_DIR = 0x00000008
    }


    /// <summary>
    /// the DesiredAccess in subcommand of NtTransact
    /// </summary>
    [Flags()]
    public enum NtTransactDesiredAccess : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0x00000000,

        /// <summary>
        /// Indicates the right to read data from the file
        /// </summary>
        FILE_READ_DATA = 0x00000001,

        /// <summary>
        /// Indicates the right to write data into the file beyond the end of the file.
        /// </summary>
        FILE_WRITE_DATA = 0x00000002,

        /// <summary>
        /// Indicates the right to append data to the file beyond the end of the file only.
        /// </summary>
        FILE_APPEND_DATA = 0x00000004,

        /// <summary>
        /// Indicates the right to read the extended attributes of the file.
        /// </summary>
        FILE_READ_EA = 0x00000008,

        /// <summary>
        /// Indicates the right to write or change the extended attributes of the file.
        /// </summary>
        FILE_WRITE_EA = 0x00000010,

        /// <summary>
        /// Indicates the right to execute the file.
        /// </summary>
        FILE_EXECUTE = 0x00000020,

        /// <summary>
        /// Indicates the right to read the attributes of the file.
        /// </summary>
        FILE_READ_ATTRIBUTES = 0x00000080,

        /// <summary>
        /// Indicates the right to change the attributes of the file.
        /// </summary>
        FILE_WRITE_ATTRIBUTES = 0x00000100,

        /// <summary>
        /// Indicates the right to delete or to rename the file.
        /// </summary>
        DELETE = 0x00010000,

        /// <summary>
        /// Indicates the right to read the security descriptor of the file.
        /// </summary>
        READ_CONTROL = 0x00020000,

        /// <summary>
        /// Indicates the right to change the discretionary access control list
        /// (DACL) in the security descriptor of the file.
        /// </summary>
        WRITE_DAC = 0x00040000,

        /// <summary>
        /// Indicates the right to change the owner in the security descriptor of the file.
        /// </summary>
        WRITE_OWNER = 0x00080000,

        /// <summary>
        /// SHOULD NOT be used by the sender and MUST be ignored by the receiver.
        /// </summary>
        SYNCHRONIZE = 0x00100000,

        /// <summary>
        /// Indicates the right to read or change the system access control
        /// list (SACL) in the security descriptor for the file. If the 
        /// SE_SECURITY_NAME privilege is not set in the access token, 
        /// the server MUST fail the open request and return STATUS_PRIVILEGE_NOT_HELD
        /// </summary>
        ACCESS_SYSTEM_SECURITY = 0x01000000,

        /// <summary>
        /// Indicates that the client is requesting an open to the file
        /// with the highest level of access that the client has on this file.
        /// If no access is granted for the client on this file, 
        /// the server MUST fail the open and return a STATUS_ACCESS_DENIED
        /// </summary>
        MAXIMUM_ALLOWED = 0x02000000,

        /// <summary>
        /// Indicates a request for all of the access flags that are
        /// previously listed except MAXIMAL_ACCESS and ACCESS_SYSTEM_SECURITY
        /// </summary>
        GENERIC_ALL = 0x10000000,

        /// <summary>
        /// Indicates a request for the following combination of
        /// access flags listed previously in this table: 
        /// FILE_READ_ATTRIBUTES, FILE_EXECUTE, SYNCHRONIZE, and READ_CONTROL
        /// </summary>
        GENERIC_EXECUTE = 0x20000000,

        /// <summary>
        /// Indicates a request for the following combination
        /// of access flags listed previously in this table: 
        /// FILE_WRITE_DATA, FILE_APPEND_DATA, SYNCHRONIZE, 
        /// FILE_WRITE_ATTRIBUTES, FILE_WRITE_EA, and READ_CONTROL
        /// </summary>
        GENERIC_WRITE = 0x40000000,

        /// <summary>
        /// Indicates a request for the following combination 
        /// of access flags listed previously in this table: 
        /// FILE_WRITE_DATA, FILE_APPEND_DATA, SYNCHRONIZE, 
        /// FILE_WRITE_ATTRIBUTES, FILE_WRITE_EA, and READ_CONTROL
        /// </summary>
        GENERIC_READ = 0x80000000
    }


    /// <summary>
    /// The action type taken in establishing the open.
    /// </summary>
    public enum NtTransactCreateActionValues : uint
    {
        /// <summary>
        /// An existing file was deleted and a new file was created in its place.
        /// </summary>
        FILE_SUPERSEDED = 0x00000000,

        /// <summary>
        /// An existing file was opened.
        /// </summary>
        FILE_OPENED = 0x00000001,

        /// <summary>
        /// A new file was created.
        /// </summary>
        FILE_CREATED = 0x00000002,

        /// <summary>
        /// An existing file was overwritten.
        /// </summary>
        FILE_OVERWRITTEN = 0x00000003
    }


    /// <summary>
    /// A 32-bit field containing encoded file attribute values and file access behavior flag values.
    /// </summary>
    [Flags()]
    public enum SMB_EXT_FILE_ATTR : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0x00000000,

        /// <summary>
        /// The file is read only. Applications can read the file but cannot write to it or delete it.
        /// </summary>
        ATTR_READONLY = 0x00000001,

        /// <summary>
        /// The file is hidden. It is not to be included in an ordinary directory listing.
        /// </summary>
        ATTR_HIDDEN = 0x00000002,

        /// <summary>
        /// The file is part of or is used exclusively by the operating system.
        /// </summary>
        ATTR_SYSTEM = 0x00000004,

        /// <summary>
        /// The file is a directory.
        /// </summary>
        ATTR_DIRECTORY = 0x00000010,

        /// <summary>
        /// The file has not been archived since it was last modified.
        /// Applications use this attribute to mark files for backup or removal.
        /// </summary>
        ATTR_ARCHIVE = 0x00000020,

        /// <summary>
        /// The file has no other attributes set. This attribute is valid only if used alone.
        /// </summary>
        ATTR_NORMAL = 0x00000080,

        /// <summary>
        /// The file is temporary. This is a hint to the cache manager 
        /// that it does not need to flush the file to backing storage.
        /// </summary>
        ATTR_TEMPORARY = 0x00000100,

        /// <summary>
        /// The file or directory is compressed. For a file, this means 
        /// that all of the data in the file is compressed. For a directory,
        /// this means that compression is the default for newly created files and subdirectories.
        /// </summary>
        ATTR_COMPRESSED = 0x00000800,

        /// <summary>
        /// Indicates that the file is to be accessed according to POSIX 
        /// rules. This includes allowing multiple files with names differing
        /// only in case, for file systems that support such naming
        /// </summary>
        POSIX_SEMANTICS = 0x01000000,

        /// <summary>
        /// Indicates that the file is being opened or created for a 
        /// backup or restore operation. The server SHOULD allow the client 
        /// to override normal file security checks, provided it has 
        /// the necessary permission to do so.
        /// </summary>
        BACKUP_SEMANTICS = 0x02000000,

        /// <summary>
        /// Requests that the server is delete the file immediately 
        /// after all of its handles have been closed.
        /// </summary>
        DELETE_ON_CLOSE = 0x04000000,

        /// <summary>
        /// Indicates that the file is to be accessed sequentially from beginning to end. 
        /// </summary>
        SEQUENTIAL_SCAN = 0x08000000,

        /// <summary>
        /// Indicates that the application intends to access the file
        /// randomly. The server MAY use this flag to optimize file caching.
        /// </summary>
        RANDOM_ACCESS = 0x10000000,

        /// <summary>
        /// Requests that the server open the file with no intermediate 
        /// buffering or caching; the server MAY NOT honor the request. 
        /// The application MUST meet certain requirements when working 
        /// with files opened with FILE_FLAG_NO_BUFFERING. File access 
        /// MUST begin at offsets within the file that are integer 
        /// multiples of the volume's sector size; and MUST be for numbers of
        /// bytes that are integer multiples of the volume's sector size. 
        /// For example, if the sector size is 512 bytes, an application
        /// can request reads and writes of 512, 1024, or 2048 bytes, 
        /// but not of 335, 981, or 7171 bytes.
        /// </summary>
        NO_BUFFERING = 0x20000000,

        /// <summary>
        /// Instructs the operating system to write through any intermediate cache and go directly to the file.
        /// The operating system can still cache write operations, but cannot lazily flush them.
        /// </summary>
        WRITE_THROUGH = 0x80000000
    }


    /// <summary>
    /// the ShareAccess in subcommand of NtTransact
    /// </summary>
    [Flags()]
    public enum NtTransactShareAccess : uint
    {
        /// <summary>
        /// (No bits set.)
        /// Prevents the file from being shared.
        /// </summary>
        NONE = 0x00000000,

        /// <summary>
        /// Other open operations can be performed on the file for read access.
        /// </summary>
        FILE_SHARE_READ = 0x00000001,

        /// <summary>
        /// Other open operations can be performed on the file for write access.
        /// </summary>
        FILE_SHARE_WRITE = 0x00000002,

        /// <summary>
        /// Other open operations can be performed on the file for delete access. 
        /// </summary>
        FILE_SHARE_DELETE = 0x00000004
    }


    /// <summary>
    /// A 32-bit value that represents the action to take if the file already exists or if the file is a new file
    /// and does not already exist.
    /// </summary>
    public enum NtTransactCreateDisposition : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0x00000006,

        /// <summary>
        /// (No bits set.)
        /// If the file already exists then it SHOULD be superseded (overwritten).
        /// If it does not already exist then it SHOULD be created.
        /// </summary>
        FILE_SUPERSEDE = 0x00000000,

        /// <summary>
        /// If the file already exists it SHOULD be opened rather than creating a new file.
        /// If the file does not already exist, then the operation MUST fail.
        /// </summary>
        FILE_OPEN = 0x00000001,

        /// <summary>
        /// If the file already exists then the operation MUST fail. 
        /// If the file does not already exist, then it SHOULD be created.
        /// </summary>
        FILE_CREATE = 0x00000002,

        /// <summary>
        /// If the file already exists it SHOULD be opened. 
        /// If the file does not already exist, then it SHOULD be created.
        /// </summary>
        FILE_OPEN_IF = 0x00000003,

        /// <summary>
        /// If the file already exists it SHOULD be opened and truncated.
        /// If the file does not already exist, the operation MUST fail.
        /// </summary>
        FILE_OVERWRITE = 0x00000004,

        /// <summary>
        /// If the file already exists it SHOULD be opened and truncated.
        /// If the file does not already exist, then it SHOULD be created.
        /// </summary>
        FILE_OVERWRITE_IF = 0x00000005
    }


    /// <summary>
    /// the CreateOptions in subcommand of NtTransact
    /// </summary>
    [Flags()]
    public enum NtTransactCreateOptions : uint
    {
        /// <summary>
        /// The file being created or opened is a directory file. With this option, 
        /// the CreateDisposition field MUST be set to FILE_CREATE, FILE_OPEN, or FILE_OPEN_IF.
        /// When this bit field is set, other compatible CreateOptions include only the following: 
        /// FILE_WRITE_THROUGH, FILE_OPEN_FOR_BACKUP_INTENT, and FILE_OPEN_BY_FILE_ID.
        /// </summary>
        FILE_DIRECTORY_FILE = 0x00000001,

        /// <summary>
        /// Applications that write data to the file MUST actually transfer the data into the
        /// file before any write request is considered complete. If FILE_NO_INTERMEDIATE_BUFFERING is set,
        /// the server MUST assume that FILE_WRITE_THROUGH is set in the create request.
        /// </summary>
        FILE_WRITE_THROUGH = 0x00000002,

        /// <summary>
        /// This option indicates that access to the file MAY be sequential.
        /// The server MAY use this information to influence its caching and read-ahead strategy for this file. 
        /// The file MAY in fact be accessed randomly,
        /// but the server MAY optimize its caching and read-ahead policy for sequential access.
        /// </summary>
        FILE_SEQUENTIAL_ONLY = 0x00000004,

        /// <summary>
        /// The file SHOULD NOT be cached or buffered in an internal buffer by the server. 
        /// This option is incompatible when the FILE_APPEND_DATA bit field is set in the DesiredAccess field.
        /// </summary>
        FILE_NO_INTERMEDIATE_BUFFERING = 0x00000008,

        /// <summary>
        /// This flag MUST be ignored by the server, and clients SHOULD set this to 0.
        /// </summary>
        FILE_SYNCHRONOUS_IO_ALERT = 0x00000010,

        /// <summary>
        /// This flag MUST be ignored by the server, and clients SHOULD set this to 0.
        /// </summary>
        FILE_SYNCHRONOUS_IO_NONALERT = 0x00000020,

        /// <summary>
        /// If the file being opened is a directory, 
        /// the server MUST fail the request with STATUS_FILE_IS_A_DIRECTORY in
        /// the Status field of the SMB header in the server response. 
        /// </summary>
        FILE_NON_DIRECTORY_FILE = 0x00000040,

        /// <summary>
        /// This option SHOULD NOT be sent by the clients, and this option MUST be ignored by the server. 
        /// </summary>
        FILE_CREATE_TREE_CONNECTION = 0x00000080,

        /// <summary>
        /// This option SHOULD NOT be sent by the clients, and this option MUST be ignored by the server. 
        /// </summary>
        FILE_COMPLETE_IF_OPLOCKED = 0x00000100,

        /// <summary>
        /// The application that initiated the client's request does not understand extended attributes (EAs).
        /// If the EAs on an existing file being opened indicate that the caller SHOULD understand EAs to 
        /// correctly interpret the file, the server SHOULD fail this request with STATUS_ACCESS_DENIED in
        /// the Status field of the SMB header in the server response. 
        /// </summary>
        FILE_NO_EA_KNOWLEDGE = 0x00000200,

        /// <summary>
        /// This option SHOULD NOT be sent by the clients, and this option MUST be ignored if received by the server.
        /// </summary>
        FILE_OPEN_FOR_RECOVERY = 0x00000400,

        /// <summary>
        /// Indicates that access to the file MAY be random. 
        /// The server MAY use this information to influence its caching and read-ahead strategy for this file.
        /// This is a hint to the server that sequential read-ahead operations MAY NOT be appropriate on the file. 
        /// </summary>
        FILE_RANDOM_ACCESS = 0x00000800,

        /// <summary>
        /// The file SHOULD be automatically deleted when the last 
        /// open request on this file is closed. When this option is set, the DesiredAccess
        /// field MUST include the DELETE flag. This option is often used for temporary files.
        /// </summary>
        FILE_DELETE_ON_CLOSE = 0x00001000,

        /// <summary>
        /// Opens a file based on the FileId. If this option is set,
        /// the server MUST fail the request with STATUS_NOT_SUPPORTED in the
        /// Status field of the SMB header in the server response. 
        /// </summary>
        FILE_OPEN_BY_FILE_ID = 0x00002000,

        /// <summary>
        /// The file is being opened or created for the purposes of either a backup or
        /// a restore operation. Thus, the server MAY make appropriate checks to ensure that the caller
        /// is capable of overriding whatever security checks have been placed on the file to allow a
        /// backup or restore operation to occur. The server MAY choose to check for certain access rights 
        /// to the file before checking the DesiredAccess field. 
        /// </summary>
        FILE_OPEN_FOR_BACKUP_INTENT = 0x00004000,

        /// <summary>
        /// When a new file is created, the file MUST not be compressed even it is on a compressed volume. 
        /// The flag MUST be ignored when opening an existing file.
        /// </summary>
        FILE_NO_COMPRESSION = 0x00008000,

        /// <summary>
        /// This option SHOULD NOT be sent by the clients, and this option MUST be ignored if received 
        /// by the server. 
        /// </summary>
        FILE_RESERVE_OPFILTER = 0x00100000,

        /// <summary>
        /// In a hierarchical storage management environment, this option 
        /// requests that the file SHOULD NOT be recalled from
        /// tertiary storage such as tape. A file recall can take up 
        /// to several minutes in a hierarchical storage management environment. 
        /// The clients can specify this option to avoid such delays.
        /// </summary>
        FILE_OPEN_NO_RECALL = 0x00400000,

        /// <summary>
        /// This option SHOULD NOT be sent by the clients, and this option
        /// MUST be ignored if received by the server.
        /// </summary>
        FILE_OPEN_FOR_FREE_SPACE_QUERY = 0x00800000
    }


    /// <summary>
    /// the ImpersonationLevel  in subcommand of NtTransact
    /// </summary>
    public enum NtTransactImpersonationLevel : uint
    {
        /// <summary>
        /// The server cannot impersonate or identify the client.
        /// </summary>
        SEC_ANONYMOUS = 0x00000000,

        /// <summary>
        /// The server can get the identity and privileges of the client, but cannot impersonate the client.
        /// </summary>
        SEC_IDENTIFY = 0x00000001,

        /// <summary>
        /// The server can impersonate the client's security context on the local system.
        /// </summary>
        SEC_IMPERSONATE = 0x00000002,

        /// <summary>
        /// The server can impersonate the client's security context on remote systems.
        /// </summary>
        SEC_DELEGATION = 0x00000003
    }


    /// <summary>
    /// the SecurityFlags  in subcommand of NtTransact
    /// </summary>
    [Flags()]
    public enum NtTransactSecurityFlags : byte
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// When set, dynamic tracking is requested. When this bit field is not set, static tracking is requested.
        /// </summary>
        SMB_SECURITY_CONTEXT_TRACKING = 0x01,

        /// <summary>
        /// Specifies that only the enabled aspects of the client's security context are available to the server. 
        /// If this flag is not specified all aspects of the client's security context are available.
        /// This flag allows the client to limit the groups and privileges that a server MAY use while 
        /// impersonating the client.
        /// </summary>
        SMB_SECURITY_EFFECTIVE_ONLY = 0x02,
    }


    /// <summary>
    /// the SecurityInformation  in subcommand of NtTransact
    /// </summary>
    [Flags()]
    public enum NtTransactSecurityInformation : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Owner of the object or resource
        /// </summary>
        OWNER_SECURITY_INFORMATION = 0x00000001,

        /// <summary>
        /// Group associated with the object or resource
        /// </summary>
        GROUP_SECURITY_INFORMATION = 0x00000002,

        /// <summary>
        /// Discretionary Access Control List associated with the object or resource.
        /// </summary>
        DACL_SECURITY_INFORMATION = 0x00000004,

        /// <summary>
        /// System Access Control List associated with the object or resource.
        /// </summary>
        SACL_SECURITY_INFORMATION = 0x00000008
    }


    /// <summary>
    /// the NotifyChangeSetup  in subcommand of NtTransact
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NtTransactNotifyChangeSetup
    {
        /// <summary>
        /// A 64-bit field of flags that specify the types of operations to monitor.
        /// </summary>
        public CompletionFilter CompletionFilterValue;

        /// <summary>
        /// The FID of the directory to monitor.
        /// </summary>
        public ushort FID;

        /// <summary>
        /// If all subdirectories are to be watched, then set this to TRUE; otherwise, FALSE.
        /// </summary>
        public byte WatchTree;

        /// <summary>
        /// Reserved. This value MUST be 0.
        /// </summary>
        public byte Reserved;
    }


    /// <summary>
    /// A 64-bit field of flags that specify the types of operations to monitor.
    /// </summary>
    [Flags()]
    public enum CompletionFilter : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0x00000000,
        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_FILE_NAME = 0x00000001,

        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_DIR_NAME = 0x00000002,

        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_NAME = 0x00000003,

        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_ATTRIBUTES = 0x00000004,

        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_SIZE = 0x00000008,

        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_LAST_WRITE = 0x00000010,

        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_LAST_ACCESS = 0x00000020,

        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_CREATION = 0x00000040,

        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_EA = 0x00000080,

        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_SECURITY = 0x00000100,

        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_STREAM_NAME = 0x00000200,

        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_STREAM_SIZE = 0x00000400,

        /// <summary>
        /// possible value 
        /// </summary>
        FILE_NOTIFY_CHANGE_STREAM_WRITE = 0x00000800
    }


    /// <summary>
    /// A state that determines whether this node signs messages. 
    /// This parameter has four possible values.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum MessageSigningPolicyValues
    {
        /// <summary>
        /// Message Signing is REQUIRED. Any connection to a node that does not use signing MUST be 
        /// disconnected.
        /// </summary>
        MessageSigningRequired = 1,

        /// <summary>
        /// Message Signing is enabled. If the other node enables or requires signing, it MUST be used.
        /// </summary>
        MessageSigningEnabled = 2,

        /// <summary>
        /// Message Signing is disabled unless the other party requires it. If the other party requires
        /// message signing, it MUST be used. Otherwise, message signing MUST NOT be used.
        /// </summary>
        MessageSigningEnabledIfRequire = 3,

        /// <summary>
        /// Message Signing is disabled. Message signing MUST NOT be used.
        /// </summary>
        MessageSigningDisabled = 4,
    }


    /// <summary>
    /// A state that determines whether plaintext authentication is permitted.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum PlaintextAuthenticationPolicyValues
    {
        /// <summary>
        /// If the server does not support challenge/response authentication, 
        /// the client MUST authenticate using plaintext passwords. The server 
        /// indicates support for challenge/response authentication using the 
        /// 0x02 flag bit of the SecurityMode field returned in the SMB_COM_NEGOTIATE
        /// response.
        /// </summary>
        Enabled = 0,

        /// <summary>
        /// If the server does not support challenge/response authentication,
        /// the client MUST disconnect from the server.
        /// </summary>
        Disabled = 1,
    }


    /// <summary>
    /// A state that determines the LAN Manager challenge/response authentication
    /// mechanism to be used. 
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum LMAuthenticationPolicyValues
    {
        /// <summary>
        /// LAN Manager challenge/response authentication (LM) is disabled.
        /// The client MUST NOT calculate and return either an LM or LMv2 response.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// LAN Manager challenge/response authentication (LM) is enabled.
        /// If the server supports challenge/response authentication, the client MUST 
        /// calculate and send the LM response.
        /// </summary>
        LmEnabled = 1,

        /// <summary>
        /// LAN Manager v2 challenge/response authentication (LMv2) is enabled.
        /// If the server supports challenge/response authentication, the client MUST 
        /// calculate and send the LMv2 response.
        /// </summary>
        LmV2Enabled = 2,
    }


    /// <summary>
    /// A state that determines the NT LAN Manager challenge/response authentication
    /// mechanism to be used.
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum NTLMAuthenticationPolicyValues
    {
        /// <summary>
        /// NT LAN Manager challenge/response authentication (NTLM) is disabled.
        /// The client MUST NOT calculate and return either an NTLM or NTLMv2 response.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// NT LAN Manager challenge/response authentication (NTLM) is enabled.
        /// If the server supports challenge/response authentication, the client MUST 
        /// calculate and send the NTLM response.
        /// </summary>
        NtlmEnabled = 1,

        /// <summary>
        /// NT LAN Manager v2 challenge/response authentication (NTLMv2) is enabled.
        /// If the server supports challenge/response authentication, the client MUST 
        /// calculate and send the NTLMv2 response
        /// </summary>
        NtlmV2Enabled = 2,
    }


    /// <summary>
    /// the const values in the request of Transaction.
    /// </summary>
    public static class SmbComTransactionPacket
    {
        /// <summary>
        /// the size in bytes of WordCount in SmbParameters.
        /// </summary>
        public const ushort SmbParametersWordCountLength = 1;

        /// <summary>
        /// the size in bytes of ByteCount in SmbData.
        /// </summary>
        public const ushort SmbDataByteCountLength = 2;
    }


    /// <summary>
    /// Fixed size of all three kinds of EA structures
    /// </summary>
    public static class EA
    {
        /// <summary>
        /// the correct fixed size in bytes of the SMB_EA is always 4.
        /// </summary>
        internal const ushort SMB_EA_FIXED_SIZE = 4;

        /// <summary>
        /// // the correct fixed size in bytes of the GET_SMB_EA is always 1.
        /// </summary>
        internal const ushort SMB_QUERY_EA_FIXED_SIZE = 1;

        /// <summary>
        /// the correct fixed size in bytes of the FULL_EA is always 8.
        /// </summary>
        internal const ushort FULL_EA_FIXED_SIZE = 8;
    }

    #endregion


    #region Structures defined by StackSdk

    /// <summary>
    /// the type of SMB Packet: Request/Response, Single/Batched.
    /// </summary>
    public enum SmbPacketType : uint
    {
        /// <summary>
        /// possible value
        /// </summary>
        NONE = 0,

        /// <summary>
        /// possible value
        /// </summary>
        SingleRequest = 1,

        /// <summary>
        /// possible value
        /// </summary>
        SingleResponse = 2,

        /// <summary>
        /// possible value
        /// </summary>
        BatchedRequest = 3,

        /// <summary>
        /// possible value
        /// </summary>
        BatchedResponse = 4,
    }


    /// <summary>
    /// Message signing policy.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum SignStateValue : uint
    {
        /// <summary>
        /// possible value
        /// </summary>
        DISABLED = 1,

        /// <summary>
        /// possible value
        /// </summary>
        ENABLED = 2,

        /// <summary>
        /// possible value
        /// </summary>
        REQUIRED = 3,
    }


    /// <summary>
    /// The session states.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum SessionStateValue : uint
    {
        /// <summary>
        /// A session setup is in progress for this session.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// The session is valid and a session key and UID are available for this session.
        /// </summary>
        Valid = 2,
    }


    /// <summary>
    /// 2.1.1 Direct TCP Transport
    /// The extended version of the SMB Protocol can use direct TCP 
    /// over either IPv4 or IPv6 as a reliable stream-oriented transport 
    /// for SMB messages. No NetBIOS layer is provided or used. TCP 
    /// provides a full, duplex, sequenced, and reliable transport for 
    /// the connection. When using TCP as the reliable connection-oriented 
    /// transport, the extended version of the SMB Protocol makes no higher-level 
    /// attempts to ensure sequenced delivery of messages between a client 
    /// and server. The TCP transport has mechanisms to detect failures of 
    /// either the client node or server node and to deliver such an indication 
    /// to the client or server software so that it can clean up the state.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TransportHeader
    {
        /// <summary>
        /// First byte of the Direct TCP Transport packet header MUST be zero (0x00).
        /// </summary>
        public byte Zero;

        /// <summary>
        /// The length, in bytes, of the SMB message. 
        /// This length is formatted as a 3-byte integer in network byte order. 
        /// The length field does not include the 4-byte Direct TCP Transport header; 
        /// rather, it is only the length of the enclosed SMB message.
        /// </summary>
        [Size("3")]
        public byte[] StreamProtocolLength;
    }


    /// <summary>
    /// the user account information
    /// </summary>
    public class CifsUserAccount
    {
        #region fields

        private string domainName;
        private string userName;
        private string password;

        #endregion


        #region properties

        /// <summary>
        /// the domain name
        /// </summary>
        public string DomainName
        {
            get
            {
                return this.domainName;
            }
        }


        /// <summary>
        /// the user name
        /// </summary>
        public string UserName
        {
            get
            {
                return this.userName;
            }
        }


        /// <summary>
        /// the password
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// to disable default constructor
        /// </summary>
        private CifsUserAccount()
        {
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="domain">the domain name</param>
        /// <param name="user">the user name</param>
        /// <param name="pswd">the password</param>
        public CifsUserAccount(string domain, string user, string pswd)
        {
            this.domainName = domain;
            this.userName = user;
            this.password = pswd;
        }


        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="userAccount">the source CifsUserAccount</param>
        public CifsUserAccount(CifsUserAccount userAccount)
        {
            if (userAccount != null)
            {
                this.domainName = userAccount.DomainName;
                this.userName = userAccount.UserName;
                this.password = userAccount.Password;
            }
        }

        #endregion
    }


    /// <summary>
    /// Contains the default parameters used by packet api in client CIFS role.
    /// </summary>
    public struct ClientDefaultParameters
    {
        /// <summary>
        /// the default value of Flag used to create request packets.
        /// </summary>
        public SmbFlags Flag;

        /// <summary>
        /// the default value of Flag2 used to create request packets.
        /// </summary>
        public SmbFlags2 Flag2;

        /// <summary>
        /// the default value of TransSmbParametersFlags used to create request packets.
        /// </summary>
        public TransSmbParametersFlags TransSmbParametersFlags;

        /// <summary>
        /// the default value of Trans2SmbParametersFlags used to create request packets.
        /// </summary>
        public Trans2SmbParametersFlags Trans2SmbParametersFlags;

        /// <summary>
        /// the default value of Timeout used to create request packets.
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// the default value of MaxParameterCount used to create Trans2/NTTrans request packets.
        /// </summary>
        public ushort MaxParameterCount;

        /// <summary>
        /// the default value of MaxDataCount used to create Trans2/NTTrans request packets.
        /// </summary>
        public ushort MaxDataCount;

        /// <summary>
        /// the default value of MaxSetupCount used to create Trans2/NTTrans request packets.
        /// </summary>
        public byte MaxSetupCount;
    }


    /// <summary>
    /// A 16-bit field. The following OptionalSupport field flags are defined. Any combination of the following flags
    /// MUST be supported. All undefined values are considered reserved. The server SHOULD set them to 0, and the client
    /// MUST ignore them.
    /// </summary>
    [Flags()]
    public enum OptionalSupport : ushort
    {
        /// <summary>
        /// None
        /// </summary>
        NONE = 0x0000,

        /// <summary>
        /// If set, the server supports the use of SMB_FILE_ATTRIBUTES (section 2.2.1.2.4) exclusive search
        /// attributes in client requests.
        /// </summary>
        SMB_SUPPORT_SEARCH_BITS=0x0001,

        /// <summary>
        /// If set, this share is managed by DFS, as specified in [MS-DFSC].
        /// </summary>
        SMB_SHARE_IS_IN_DFS=0x0002,
    }

    #endregion
}
