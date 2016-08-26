// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ShareUtil
{
    #region NetAPI

    public enum NetError
    {
        NERR_Success = 0,
        NERR_BASE = 2100,
        NERR_UnknownDevDir = (NERR_BASE + 16),
        NERR_DuplicateShare = (NERR_BASE + 18),
        NERR_BufTooSmall = (NERR_BASE + 23),
    }

    [Flags]
    public enum SHARE_TYPE : uint
    {
        STYPE_DISKTREE = 0,
        STYPE_PRINTQ = 1,
        STYPE_DEVICE = 2,
        STYPE_IPC = 3,
        STYPE_CLUSTER_FS = 0x02000000,
        STYPE_CLUSTER_SOFS = 0x04000000,
        STYPE_CLUSTER_DFS = 0x08000000,
        STYPE_SPECIAL = 0x80000000,
        STYPE_TEMPORARY = 0x40000000,
    }

    [Flags]
    public enum SHARE_INFO_1005_Flags : uint
    {
        SHI1005_FLAGS_DFS = 0x1,
        SHI1005_FLAGS_DFS_ROOT = 0x2,
        SHI1005_FLAGS_RESTRICT_EXCLUSIVE_OPENS = 0x100,
        SHI1005_FLAGS_FORCE_SHARED_DELETE = 0x200,
        SHI1005_FLAGS_ALLOW_NAMESPACE_CACHING = 0x400,
        SHI1005_FLAGS_ACCESS_BASED_DIRECTORY_ENUM = 0x800,
        SHI1005_FLAGS_FORCE_LEVELII_OPLOCK = 0x1000,
        SHI1005_FLAGS_ENABLE_HASH = 0x2000,
        CSC_CACHE_MANUAL_REINT = 0x0,
        CSC_CACHE_AUTO_REINT = 0x10,
        CSC_CACHE_VDO = 0x20,
        CSC_CACHE_NONE = 0x30
    }

    public class ShareCache
    {
        #region Win32
        [StructLayout(LayoutKind.Sequential)]
        public struct SHARE_INFO_502
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi502_netname;
            public uint shi502_type;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi502_remark;
            public Int32 shi502_permissions;
            public Int32 shi502_max_uses;
            public Int32 shi502_current_uses;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi502_path;
            internal IntPtr shi502_passwd;
            public Int32 shi502_reserved;
            internal IntPtr shi502_security_descriptor;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SHARE_INFO_503
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi503_netname;
            public uint shi503_type;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi503_remark;
            public Int32 shi503_permissions;    // used w/ share level security only
            public Int32 shi503_max_uses;
            public Int32 shi503_current_uses;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi503_path;
            internal IntPtr shi503_passwd;    // used w/ share level security only
            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi503_servername;
            public Int32 shi503_reserved;
            internal IntPtr shi503_security_descriptor;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SHARE_INFO_1005
        {
            public Int32 shi1005_flags;
        }
        
        [DllImport("Netapi32.dll", SetLastError = true)]
        static extern int NetShareGetInfo(
            [MarshalAs(UnmanagedType.LPWStr)] string serverName,
            [MarshalAs(UnmanagedType.LPWStr)] string netName,
            Int32 level,
            out IntPtr bufPtr);

        [DllImport("Netapi32.dll", SetLastError = true)]
        static extern int NetShareSetInfo(
            [MarshalAs(UnmanagedType.LPWStr)] string serverName,
            [MarshalAs(UnmanagedType.LPWStr)] string netName,
            Int32 level,
            IntPtr bufPtr,
            out Int32 param_err);

        [DllImport("Netapi32", CharSet = CharSet.Auto)]
        static extern int NetApiBufferFree(IntPtr Buffer);

        #endregion

        public static int GetShareCachingInfo(string ComputerName, string ShareName)
        {
            IntPtr ptr = IntPtr.Zero;
            int errCode = 0;
            errCode = NetShareGetInfo(ComputerName, ShareName, 1005, out ptr);
            if (errCode == (int)NetError.NERR_Success)
            {
                SHARE_INFO_1005 shareInfo =
                    (SHARE_INFO_1005)Marshal.PtrToStructure(ptr, typeof(SHARE_INFO_1005));
                NetApiBufferFree(ptr);
                return shareInfo.shi1005_flags;
            }
            else
            {
                throw new Win32Exception(errCode);
            }
        }

        public static void SetShareCachingInfo(string ComputerName, string ShareName, int ShareFlags)
        {
            IntPtr ptr = IntPtr.Zero;
            int param_Err = 0;
            int errCode = 0;
            SHARE_INFO_1005 shareInfo = new SHARE_INFO_1005();
            shareInfo.shi1005_flags = ShareFlags;
            ptr = Marshal.AllocHGlobal(Marshal.SizeOf(shareInfo));
            Marshal.StructureToPtr(shareInfo, ptr, true);
            errCode = NetShareSetInfo(ComputerName, ShareName, 1005, ptr, out param_Err);
            if (errCode == (int)NetError.NERR_Success)
            {
                SHARE_INFO_1005 shareInfo2 =
                    (SHARE_INFO_1005)Marshal.PtrToStructure(ptr, typeof(SHARE_INFO_1005));
                NetApiBufferFree(ptr);
            }
            else
            {
                throw new Win32Exception(errCode);
            }
        }
    }
    #endregion
   
    class ShareUtil
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                Console.WriteLine("Retrieve share caching info for share {0} on server {1}:", args[1], args[0]);
                uint uFlag = (uint)ShareCache.GetShareCachingInfo(args[0], args[1]);
                Console.WriteLine((SHARE_INFO_1005_Flags)uFlag);
            }
            else if(args.Length == 4)
            {
                Console.WriteLine("Retrieve share caching info for share {0} on server {1}", args[1], args[0]);

                uint uFlag = (uint)ShareCache.GetShareCachingInfo(args[0], args[1]);
                Console.WriteLine((SHARE_INFO_1005_Flags)uFlag);

                SHARE_INFO_1005_Flags flags = (SHARE_INFO_1005_Flags)Enum.Parse(typeof(SHARE_INFO_1005_Flags), args[2]);

                bool setFlag = (bool)Boolean.Parse(args[3]);
                if (setFlag)
                {
                    flags = (SHARE_INFO_1005_Flags)uFlag | flags;
                }
                else
                {
                    flags = (SHARE_INFO_1005_Flags)uFlag & ~(flags);
                }

                Console.WriteLine("Share caching info for share {0} on server {1} to flags: {2}", args[1], args[0], flags);

                ShareCache.SetShareCachingInfo(args[0], args[1], (int)flags);
                uFlag = (uint)ShareCache.GetShareCachingInfo(args[0], args[1]);
                Console.WriteLine("Share caching info after update: {0}", (SHARE_INFO_1005_Flags)uFlag);
            }
            else
            {
                Console.WriteLine("Usage: ShareUtil <computer name> <share name> <SHARE_INFO_1005_Flags> <true to set flag, false to clear flag>");
            }
        }
    }
}
