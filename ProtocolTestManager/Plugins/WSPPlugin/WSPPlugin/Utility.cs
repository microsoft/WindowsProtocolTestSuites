// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP;

namespace Microsoft.Protocols.TestManager.WSPServerPlugin
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
        public static MessageBuilderParameter BuildParameter()
        {
            char[] delimiter = new char[] { ',' };

            Configs config = new Configs();
            config.LoadDefaultValues();

            var parameter = new MessageBuilderParameter();

            parameter.PropertySet_One_DBProperties = config.PropertySet_One_DBProperties.Split(delimiter);

            parameter.PropertySet_Two_DBProperties = config.PropertySet_Two_DBProperties.Split(delimiter);

            parameter.Array_PropertySet_One_Guid = new Guid(config.Array_PropertySet_One_Guid);

            parameter.Array_PropertySet_One_DBProperties = config.Array_PropertySet_One_DBProperties.Split(delimiter);

            parameter.Array_PropertySet_Two_Guid = new Guid(config.Array_PropertySet_Two_Guid);

            parameter.Array_PropertySet_Two_DBProperties = config.Array_PropertySet_Two_DBProperties.Split(delimiter);

            parameter.Array_PropertySet_Three_Guid = new Guid(config.Array_PropertySet_Three_Guid);

            parameter.Array_PropertySet_Three_DBProperties = config.Array_PropertySet_Three_DBProperties.Split(delimiter);

            parameter.Array_PropertySet_Four_Guid = new Guid(config.Array_PropertySet_Four_Guid);

            parameter.Array_PropertySet_Four_DBProperties = config.Array_PropertySet_Four_DBProperties.Split(delimiter);

            parameter.EachRowSize = MessageBuilder.rowWidth;

            parameter.EType = UInt32.Parse(config.EType);

            parameter.BufferSize = UInt32.Parse(config.BufferSize);

            parameter.LCID_VALUE = UInt32.Parse(config.LCID_VALUE);

            parameter.ClientBase = UInt32.Parse(config.ClientBase);

            parameter.RowsToTransfer = UInt32.Parse(config.RowsToTransfer);

            parameter.NumberOfSetBindingsColumns = Int32.Parse(config.NumberOfSetBindingsColumns);

            parameter.ColumnParameters = new MessageBuilderColumnParameter[parameter.NumberOfSetBindingsColumns];

            for (int i = 0; i < parameter.NumberOfSetBindingsColumns; i++)
            {
                parameter.ColumnParameters[i] = new MessageBuilderColumnParameter();

                parameter.ColumnParameters[i].Guid = new Guid((string)config.GetType().GetProperty($"columnGuid_{i}").GetValue(config, null));

                parameter.ColumnParameters[i].PropertyId = UInt32.Parse((string)config.GetType().GetProperty($"columnPropertyId_{i}").GetValue(config, null));

                parameter.ColumnParameters[i].ValueOffset = UInt16.Parse((string)config.GetType().GetProperty($"columnValueOffset_{i}").GetValue(config, null));

                parameter.ColumnParameters[i].StatusOffset = UInt16.Parse((string)config.GetType().GetProperty($"columnStatusOffset_{i}").GetValue(config, null));

                parameter.ColumnParameters[i].LengthOffset = UInt16.Parse((string)config.GetType().GetProperty($"columnLengthOffset_{i}").GetValue(config, null));

                parameter.ColumnParameters[i].StorageType = (StorageType)Enum.Parse(typeof(StorageType), (string)config.GetType().GetProperty($"columnStorageType_{i}").GetValue(config, null));
            }
            return parameter;
        }
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
