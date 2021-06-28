// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestManager.FileServerPlugin
{
    public enum NetError
    {
        NERR_Success = 0,
        NERR_BASE = 2100,
        NERR_UnknownDevDir = (NERR_BASE + 16),
        NERR_DuplicateShare = (NERR_BASE + 18),
        NERR_BufTooSmall = (NERR_BASE + 23),
    }

    public static class ServerHelper
    {
        #region Native API

        const uint MAX_PREFERRED_LENGTH = 0xFFFFFFFF;
        const int NERR_Success = 0;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHARE_INFO_1
        {
            public string shi1_netname;
            public uint shi1_type;
            public string shi1_remark;
            public SHARE_INFO_1(string sharename, uint sharetype, string remark)
            {
                this.shi1_netname = sharename;
                this.shi1_type = sharetype;
                this.shi1_remark = remark;
            }
            public override string ToString()
            {
                return shi1_netname;
            }
        }

        [DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int NetShareEnum(
             StringBuilder ServerName,
             int level,
             ref IntPtr bufPtr,
             uint prefmaxlen,
             ref int entriesread,
             ref int totalentries,
             ref int resume_handle
             );

        [DllImport("Netapi32", CharSet = CharSet.Auto)]
        static extern int NetApiBufferFree(IntPtr Buffer);

        #endregion

        public static string[] EnumShares(string serverName, string userName, string domainName, string password)
        {
            string[] shareList = null;

            using (new ImpersonationHelper(userName, domainName, password))
            {
                List<string> ShareInfos = new List<string>();
                int entriesread = 0;
                int totalentries = 0;
                int resume_handle = 0;
                IntPtr bufPtr = IntPtr.Zero;
                int nStructSize = Marshal.SizeOf(typeof(SHARE_INFO_1));
                StringBuilder server = new StringBuilder(serverName);

                if (NetShareEnum(server, 1, ref bufPtr, MAX_PREFERRED_LENGTH, ref entriesread, ref totalentries, ref resume_handle)
                    == (int)NetError.NERR_Success)
                {
                    IntPtr currentPtr = bufPtr;

                    for (int i = 0; i < entriesread; i++)
                    {
                        SHARE_INFO_1 shi1 = (SHARE_INFO_1)Marshal.PtrToStructure(currentPtr, typeof(SHARE_INFO_1));
                        ShareInfos.Add(shi1.shi1_netname);
                        currentPtr = new IntPtr(currentPtr.ToInt64() + nStructSize);
                    }

                    NetApiBufferFree(bufPtr); 
                    shareList = ShareInfos.ToArray();
                }
            }

            return shareList;
        }
    }
}
