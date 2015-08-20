// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// NCB Return codes
    /// </summary>
    internal enum NcbReturnCode : byte
    {
        /// <summary>
        /// good return
        /// </summary>
        NRC_GOODRET = 0x00,

        /// <summary>
        /// illegal buffer length 
        /// </summary>
        NRC_BUFLEN = 0x01,

        /// <summary>
        /// illegal command    
        /// </summary>
        NRC_ILLCMD = 0x03,

        /// <summary>
        /// command timed out
        /// </summary>
        NRC_CMDTMO = 0x05,

        /// <summary>
        /// message incomplete, issue another command
        /// </summary>
        NRC_INCOMP = 0x06,

        /// <summary>
        /// illegal buffer address   
        /// </summary>
        NRC_BADDR = 0x07,

        /// <summary>
        /// session number out of range
        /// </summary>
        NRC_SNUMOUT = 0x08,

        /// <summary>
        /// no resource available
        /// </summary>
        NRC_NORES = 0x09,

        /// <summary>
        /// session closed
        /// </summary>
        NRC_SCLOSED = 0x0a,

        /// <summary>
        /// command cancelled
        /// </summary>
        NRC_CMDCAN = 0x0b,

        /// <summary>
        /// duplicate name    
        /// </summary>
        NRC_DUPNAME = 0x0d,

        /// <summary>
        /// name table full  
        /// </summary>
        NRC_NAMTFUL = 0x0e,

        /// <summary>
        /// no deletions, name has active sessions
        /// </summary>
        NRC_ACTSES = 0x0f,

        /// <summary>
        /// local session table full  
        /// </summary>
        NRC_LOCTFUL = 0x11,

        /// <summary>
        /// remote session table full    
        /// </summary>
        NRC_REMTFUL = 0x12,

        /// <summary>
        /// illegal name number  
        /// </summary>
        NRC_ILLNN = 0x13,

        /// <summary>
        /// no callname  
        /// </summary>
        NRC_NOCALL = 0x14,

        /// <summary>
        /// cannot put * in NCB_NAME       
        /// </summary>
        NRC_NOWILD = 0x15,

        /// <summary>
        /// name in use on remote adapter      
        /// </summary>
        NRC_INUSE = 0x16,

        /// <summary>
        /// name deleted  
        /// </summary>
        NRC_NAMERR = 0x17,
        /// <summary>
        /// session ended abnormally  
        /// </summary>
        NRC_SABORT = 0x18,

        /// <summary>
        /// name conflict detected   
        /// </summary>
        NRC_NAMCONF = 0x19,

        /// <summary>
        /// interface busy, IRET before retrying 
        /// </summary>
        NRC_IFBUSY = 0x21,

        /// <summary>
        /// too many commands outstanding, retry later
        /// </summary>
        NRC_TOOMANY = 0x22,

        /// <summary>
        /// ncb_lana_num field invalid   
        /// </summary>
        NRC_BRIDGE = 0x23,

        /// <summary>
        /// command completed while cancel occurring
        /// </summary>
        NRC_CANOCCR = 0x24,

        /// <summary>
        /// command not valid to cancel 
        /// </summary>
        NRC_CANCEL = 0x26,

        /// <summary>
        /// name defined by anther local process   
        /// </summary>
        NRC_DUPENV = 0x30,

        /// <summary>
        /// environment undefined. RESET required
        /// </summary>
        NRC_ENVNOTDEF = 0x34,

        /// <summary>
        /// required OS resources exhausted 
        /// </summary>
        NRC_OSRESNOTAV = 0x35,

        /// <summary>
        /// max number of applications exceeded
        /// </summary>
        NRC_MAXAPPS = 0x36,

        /// <summary>
        /// no saps available for netbios
        /// </summary>
        NRC_NOSAPS = 0x37,

        /// <summary>
        /// requested resources are not available 
        /// </summary>
        NRC_NORESOURCES = 0x38,

        /// <summary>
        /// invalid ncb address or length > segment
        /// </summary>
        NRC_INVADDRESS = 0x39,

        /// <summary>
        /// invalid NCB DDID
        /// </summary>
        NRC_INVDDID = 0x3B,

        /// <summary>
        /// lock of user area failed 
        /// </summary>
        NRC_LOCKFAIL = 0x3C,

        /// <summary>
        /// NETBIOS not loaded 
        /// </summary>
        NRC_OPENERR = 0x3f,

        /// <summary>
        /// system error
        /// </summary>
        NRC_SYSTEM = 0x40,

        /// <summary>
        /// asynchronous command is not yet finished
        /// </summary>
        NRC_PENDING = 0xff,
    }

    /// <summary>
    /// NCB Command codes from
    /// </summary>
    internal enum NcbCommand : byte
    {
        /// <summary>
        /// NCB CALL
        /// </summary>
        NCBCALL = 0x10,

        /// <summary>
        /// NCB LISTEN 
        /// </summary>
        NCBLISTEN = 0x11,

        /// <summary>
        /// NCB HANG UP     
        /// </summary>
        NCBHANGUP = 0x12,

        /// <summary>
        /// NCB SEND  
        /// </summary>
        NCBSEND = 0x14,

        /// <summary>
        /// NCB RECEIVE
        /// </summary>
        NCBRECV = 0x15,

        /// <summary>
        /// NCB RECEIVE ANY 
        /// </summary>
        NCBRECVANY = 0x16,

        /// <summary>
        /// NCB CHAIN SEND 
        /// </summary>
        NCBCHAINSEND = 0x17,

        /// <summary>
        /// NCB SEND DATAGRAM 
        /// </summary>
        NCBDGSEND = 0x20,

        /// <summary>
        /// NCB RECEIVE DATAGRAM
        /// </summary>
        NCBDGRECV = 0x21,

        /// <summary>
        /// NCB SEND BROADCAST DATAGRAM 
        /// </summary>
        NCBDGSENDBC = 0x22,

        /// <summary>
        /// NCB RECEIVE BROADCAST DATAGRAM
        /// </summary>
        NCBDGRECVBC = 0x23,

        /// <summary>
        /// NCB ADD NAME
        /// </summary>
        NCBADDNAME = 0x30,

        /// <summary>
        /// NCB DELETE NAME 
        /// </summary>
        NCBDELNAME = 0x31,

        /// <summary>
        /// NCB RESET
        /// </summary>
        NCBRESET = 0x32,

        /// <summary>
        /// NCB ADAPTER STATUS 
        /// </summary>
        NCBASTAT = 0x33,

        /// <summary>
        /// NCB SESSION STATUS
        /// </summary>
        NCBSSTAT = 0x34,

        /// <summary>
        /// NCB CANCEL 
        /// </summary>
        NCBCANCEL = 0x35,

        /// <summary>
        /// NCB ADD GROUP NAME 
        /// </summary>
        NCBADDGRNAME = 0x36,

        /// <summary>
        /// NCB ENUMERATE LANA NUMBERS
        /// </summary>
        NCBENUM = 0x37,

        /// <summary>
        /// NCB UNLINK  
        /// </summary>
        NCBUNLINK = 0x70,

        /// <summary>
        /// NCB SEND NO ACK
        /// </summary>
        NCBSENDNA = 0x71,

        /// <summary>
        /// NCB CHAIN SEND NO ACK       
        /// </summary>
        NCBCHAINSENDNA = 0x72,

        /// <summary>
        /// NCB LAN STATUS ALERT 
        /// </summary>
        NCBLANSTALERT = 0x73,

        /// <summary>
        /// NCB ACTION  
        /// </summary>
        NCBACTION = 0x77,

        /// <summary>
        /// NCB FIND NAME    
        /// </summary>
        NCBFINDNAME = 0x78,

        /// <summary>
        /// NCB TRACE
        /// </summary>
        NCBTRACE = 0x79,

        /// <summary>
        /// high bit set == asynchronous
        /// </summary>
        ASYNCH = 0x80
    }

    /// <summary>
    /// the structure returned from the NCB command NCBENUM.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct LANA_ENUM
    {
        /// <summary>
        /// Number of valid entries in lanaNum[]
        /// </summary>
        public byte length;

        /// <summary>
        /// size is MAX_LANA + 1
        /// </summary>
        public byte[] lanaNum;
    }

    /// <summary>
    /// Network Control Block
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
    internal struct NCB
    {
        /// <summary>
        /// command code  
        /// </summary>
        public byte ncb_command;

        /// <summary>
        /// return code 
        /// </summary>
        public byte ncb_retcode;

        /// <summary>
        /// local session number
        /// </summary>
        public byte ncb_lsn;

        /// <summary>
        /// number of our network name
        /// </summary>
        public byte ncb_num;

        /// <summary>
        /// the 4bytes padding for win64 using 8bytes alignment.<para/>
        /// if win32, it's null.
        /// </summary>
        public byte[] ncb_padding0;

        /// <summary>
        /// address of message buffer
        /// </summary>
        [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        public IntPtr ncb_buffer;

        /// <summary>
        /// size of message buffer
        /// </summary>
        public ushort ncb_length;

        /// <summary>
        /// blank-padded name of remote. the size is 16.
        /// </summary>
        public byte[] ncb_callname;

        /// <summary>
        /// our blank-padded netname. the size is 16.
        /// </summary>
        public byte[] ncb_name;

        /// <summary>
        /// rcv timeout/retry count 
        /// </summary>
        public byte ncb_rto;

        /// <summary>
        /// send timeout/sys timeout
        /// </summary>
        public byte ncb_sto;

        /// <summary>
        /// the 4bytes padding for win64 using 8bytes alignment.<para/>
        /// if win32, it's null.
        /// </summary>
        public byte[] ncb_padding1;

        /// <summary>
        /// POST routine address
        /// </summary>
        public IntPtr ncb_post;

        /// <summary>
        /// lana (adapter) number 
        /// </summary>
        public byte ncb_lana_num;

        /// <summary>
        /// 0xff => commmand pending
        /// </summary>
        public byte ncb_cmd_cplt;

        /// <summary>
        /// 18 #ifdef _WIN64; 10 #else.
        /// </summary>
        public byte[] ncb_reserve;

        /// <summary>
        /// the 4bytes padding for win64 using 8bytes alignment.<para/>
        /// if win32, it's null.
        /// </summary>
        public byte[] ncb_padding2;

        /// <summary>
        /// HANDLE to Win32 event which will be set to the signaled  
        /// state when an ASYNCH command completes
        /// </summary>
        public IntPtr ncb_event;
    }

    /// <summary>
    /// Pinvoke the windows API of netbios.
    /// </summary>
    internal static class NetbiosNativeMethods
    {
        /// <summary>
        /// the Windows API of Netbios.
        /// </summary>
        /// <param name="pNcb">the net control block.</param>
        [DllImport("Netapi32.dll")]
        internal static extern void Netbios(IntPtr pNcb);
    }
}
