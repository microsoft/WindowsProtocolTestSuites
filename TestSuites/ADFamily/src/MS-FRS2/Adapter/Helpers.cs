// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Text;
using System.Reflection;
using System.DirectoryServices.Protocols;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.Messages.Marshaling;
using Microsoft.Protocols.TestSuites.Frs2DataTypes;
using FRS2Model;
using objectGUID = System.Guid;

namespace Microsoft.Protocols.TestSuites.MS_FRS2
{

    public class FRS2Helpers
    {
        static ITestSite TestSite;

        public static string GetStringFromShortArray(short[] raw)
        {
            byte[] conv = new byte[raw.Length * 2];
            Buffer.BlockCopy(raw, 0, conv, 0, raw.Length * 2);
            int end = raw.Length * 2;
            for (int i = 0; i < raw.Length; i++)
            {
                if (raw[i] == 0)
                {
                    end = i * 2;
                    break;
                }
            }
            return Encoding.Unicode.GetString(conv, 0, end);
        }

        public static void Initialize(ITestSite site)
        {
            TestSite = site;
        }

        /// <summary>
        /// Asyncpoll Response is filled in the Response structure and returned.
        /// </summary>
        /// <param name="asyncPtr"></param>
        /// <returns></returns>
        public static _FRS_ASYNC_RESPONSE_CONTEXT GetAsyncResponse(_FRS_ASYNC_RESPONSE_CONTEXT_POINTER asyncPtr)
        {

            _FRS_ASYNC_RESPONSE_CONTEXT asyncResponse = new _FRS_ASYNC_RESPONSE_CONTEXT();
            asyncResponse.status = asyncPtr.status;
            asyncResponse.sequenceNumber = asyncPtr.sequenceNumber;
            asyncResponse.result.versionVectorCount = asyncPtr.result.versionVectorCount;
            asyncResponse.result.epoqueVectorCount = asyncPtr.result.epoqueVectorCount;
            asyncResponse.result.vvGeneration = asyncPtr.result.vvGeneration;
            long asyncaddress = 0;
            long asyncaddress1 = 0;
            asyncaddress = (long)asyncPtr.result.versionVector;
            asyncResponse.result.versionVector = new _FRS_VERSION_VECTOR[asyncResponse.result.versionVectorCount];
            byte[] b = new byte[16];


            for (int i = 0; i < asyncResponse.result.versionVectorCount; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    b[j] = Marshal.ReadByte((IntPtr)asyncaddress);
                    asyncaddress = asyncaddress + 1;
                }
                asyncResponse.result.versionVector[i].dbGuid = new Guid(b);
                asyncResponse.result.versionVector[i].low = (ulong)(Marshal.ReadInt64((IntPtr)asyncaddress));
                asyncaddress = asyncaddress + sizeof(ulong);
                asyncResponse.result.versionVector[i].high = (ulong)(Marshal.ReadInt64((IntPtr)asyncaddress));
                asyncaddress = asyncaddress + sizeof(ulong);
            }
            _FRS_EPOQUE_VECTOR epoqueVector = new _FRS_EPOQUE_VECTOR();
            asyncaddress1 = (long)asyncPtr.result.epoqueVector;
            for (int i = 0; i < asyncResponse.result.epoqueVectorCount; i++)
            {
                asyncResponse.result.epoqueVector[i] = (_FRS_EPOQUE_VECTOR)Marshal.PtrToStructure(asyncPtr.result.epoqueVector, typeof(_FRS_EPOQUE_VECTOR));
                asyncaddress1 += Marshal.SizeOf(epoqueVector);
            }
            return asyncResponse;
        }

        #region Update
        /// <summary>
        /// From updateptr(frsUpdate) returned by the server. FRSUpdate structures are
        /// filled and returned.
        /// </summary>
        /// <param name="updateptr"></param>
        /// <param name="updateCount"></param>
        /// <param name="frsUpdate"></param>
        public static void getUpdates(IntPtr updateptr, uint updateCount, out _FRS_UPDATE[] frsUpdate)
        {
            frsUpdate = new _FRS_UPDATE[updateCount];
            long updatesAddress = (long)updateptr;
            byte[] contentguid = new byte[16];

            //From updateptr(frsUpdate) returned by the server. FRSUpdate structures are
            //filled and returned.
            for (int i = 0; i < updateCount; i++)
            {
                frsUpdate[i].present = (present_Values)Marshal.ReadInt32((IntPtr)updatesAddress);
                updatesAddress = updatesAddress + 4;
                frsUpdate[i].nameConflict = Marshal.ReadInt32((IntPtr)updatesAddress);
                updatesAddress = updatesAddress + 4;
                frsUpdate[i].attributes = (uint)Marshal.ReadInt32((IntPtr)updatesAddress);
                updatesAddress = updatesAddress + 4;
                frsUpdate[i].fence = (_FILETIME)Marshal.PtrToStructure((IntPtr)updatesAddress, typeof(_FILETIME));
                updatesAddress = updatesAddress + 8;
                frsUpdate[i].clock = (_FILETIME)Marshal.PtrToStructure((IntPtr)updatesAddress, typeof(_FILETIME));
                updatesAddress = updatesAddress + 8;
                frsUpdate[i].createTime = (_FILETIME)Marshal.PtrToStructure((IntPtr)updatesAddress, typeof(_FILETIME));
                updatesAddress = updatesAddress + 8;
                for (int j = 0; j < 16; j++)
                {
                    contentguid[j] = Marshal.ReadByte((IntPtr)updatesAddress);
                    updatesAddress = updatesAddress + 1;
                }
                frsUpdate[i].contentSetId = new Guid(contentguid);
                frsUpdate[i].hash = new byte[20];
                for (int j = 0; j < 20; j++)
                {
                    frsUpdate[i].hash[j] = Marshal.ReadByte((IntPtr)updatesAddress);
                    updatesAddress = updatesAddress + 1;
                }
                frsUpdate[i].rdcSimilarity = new byte[16];
                for (int j = 0; j < 16; j++)
                {
                    frsUpdate[i].rdcSimilarity[j] = Marshal.ReadByte((IntPtr)updatesAddress);
                    updatesAddress = updatesAddress + 1;
                }
                byte[] uiddbguid = new byte[16];
                for (int j = 0; j < 16; j++)
                {
                    uiddbguid[j] = Marshal.ReadByte((IntPtr)updatesAddress);
                    updatesAddress = updatesAddress + 1;
                }
                frsUpdate[i].uidDbGuid = new Guid(uiddbguid);

                frsUpdate[i].uidVersion = (ulong)Marshal.ReadInt64((IntPtr)updatesAddress);
                updatesAddress = updatesAddress + 8;

                byte[] gvsndbguid = new byte[16];
                for (int j = 0; j < 16; j++)
                {
                    gvsndbguid[j] = Marshal.ReadByte((IntPtr)updatesAddress);
                    updatesAddress = updatesAddress + 1;
                }
                frsUpdate[i].gvsnDbGuid = new Guid(gvsndbguid);

                frsUpdate[i].gvsnVersion = (ulong)Marshal.ReadInt64((IntPtr)updatesAddress);
                updatesAddress = updatesAddress + 8;

                byte[] parentdbguid = new byte[16];
                for (int j = 0; j < 16; j++)
                {
                    parentdbguid[j] = Marshal.ReadByte((IntPtr)updatesAddress);
                    updatesAddress = updatesAddress + 1;
                }
                frsUpdate[i].parentDbGuid = new Guid(parentdbguid);

                frsUpdate[i].parentVersion = (ulong)Marshal.ReadInt64((IntPtr)updatesAddress);
                updatesAddress = updatesAddress + 8;
                frsUpdate[i].name = new short[262];
                for (int j = 0; j < 262; j++)
                {
                    frsUpdate[i].name[j] = (short)Marshal.ReadInt16((IntPtr)updatesAddress);
                    updatesAddress += sizeof(short);

                }

                var nameRaw = new byte[frsUpdate[i].name.Length * 2];
                Buffer.BlockCopy(frsUpdate[i].name, 0, nameRaw, 0, frsUpdate[i].name.Length * 2);
                string fname = Encoding.Unicode.GetString(nameRaw);
                if (-1 != fname.IndexOf('\0'))
                    fname = fname.Substring(0, fname.IndexOf('\0'));
                TestSite.Log.Add(LogEntryKind.Checkpoint, "Received FRS_UPDATE with name: {0}", fname);
                frsUpdate[i].flags = (flags_Values)Marshal.ReadInt32((IntPtr)updatesAddress);
                updatesAddress += sizeof(Int32);

            }

        }

        /// <summary>
        /// frsUpdate byte array is filled into the FRS_UPDATE_PTR structure to 
        /// give it as input parameter to Initializefiletransferasync. 
        /// </summary>
        /// <param name="frsUpdate"></param>
        /// <returns></returns>
        public static FRS_UPDATE_PTR updateFrs(_FRS_UPDATE frsUpdate)
        {
            FRS_UPDATE_PTR frsUpdatePtr = new FRS_UPDATE_PTR(frsUpdate);
            return frsUpdatePtr;
        }

        /// <summary>
        /// FrsUpdateStructToByteArray is used to convert frsUpdate structure to byte array.
        /// </summary>
        /// <param name="frsUpdate"></param>
        /// <returns></returns>
        public static byte[] FrsUpdateStructToByteArray(_FRS_UPDATE frsUpdate)
        {
            byte[] buff = new byte[688];
            int count = 0;
            BitConverter.GetBytes((uint)frsUpdate.present).CopyTo(buff, count);
            count = count + BitConverter.GetBytes((uint)frsUpdate.present).Length;

            BitConverter.GetBytes(frsUpdate.nameConflict).CopyTo(buff, count);
            count = count + BitConverter.GetBytes(frsUpdate.nameConflict).Length;

            BitConverter.GetBytes(frsUpdate.attributes).CopyTo(buff, count);
            count = count + BitConverter.GetBytes(frsUpdate.attributes).Length;

            BitConverter.GetBytes(frsUpdate.fence.dwLowDateTime).CopyTo(buff, count);
            count = count + BitConverter.GetBytes(frsUpdate.fence.dwLowDateTime).Length;

            BitConverter.GetBytes(frsUpdate.fence.dwHighDateTime).CopyTo(buff, count);
            count = count + BitConverter.GetBytes(frsUpdate.fence.dwHighDateTime).Length;

            BitConverter.GetBytes(frsUpdate.clock.dwLowDateTime).CopyTo(buff, count);
            count = count + BitConverter.GetBytes(frsUpdate.clock.dwLowDateTime).Length;

            BitConverter.GetBytes(frsUpdate.clock.dwHighDateTime).CopyTo(buff, count);
            count = count + BitConverter.GetBytes(frsUpdate.clock.dwHighDateTime).Length;

            BitConverter.GetBytes(frsUpdate.createTime.dwLowDateTime).CopyTo(buff, count);
            count = count + BitConverter.GetBytes(frsUpdate.createTime.dwLowDateTime).Length;

            BitConverter.GetBytes(frsUpdate.createTime.dwHighDateTime).CopyTo(buff, count);
            count = count + BitConverter.GetBytes(frsUpdate.createTime.dwHighDateTime).Length;

            //ContentsetId
            frsUpdate.contentSetId.ToByteArray().CopyTo(buff, count);
            count = count + frsUpdate.contentSetId.ToByteArray().Length;

            frsUpdate.hash.CopyTo(buff, count);
            count = count + frsUpdate.hash.Length;

            frsUpdate.rdcSimilarity.CopyTo(buff, count);
            count = count + frsUpdate.rdcSimilarity.Length;

            //uidDBGuid
            frsUpdate.uidDbGuid.ToByteArray().CopyTo(buff, count);
            count = count + frsUpdate.uidDbGuid.ToByteArray().Length;

            BitConverter.GetBytes(frsUpdate.uidVersion).CopyTo(buff, count);
            count = count + BitConverter.GetBytes(frsUpdate.uidVersion).Length;

            //gvsnDBGuid
            frsUpdate.gvsnDbGuid.ToByteArray().CopyTo(buff, count);
            count = count + frsUpdate.gvsnDbGuid.ToByteArray().Length;

            BitConverter.GetBytes(frsUpdate.gvsnVersion).CopyTo(buff, count);
            count = count + BitConverter.GetBytes(frsUpdate.gvsnVersion).Length;

            //parentDbGuid
            frsUpdate.parentDbGuid.ToByteArray().CopyTo(buff, count);
            count = count + frsUpdate.parentDbGuid.ToByteArray().Length;

            BitConverter.GetBytes(frsUpdate.parentVersion).CopyTo(buff, count);
            count = count + BitConverter.GetBytes(frsUpdate.parentVersion).Length;

            for (int i = 0; i < frsUpdate.name.Length; i++)
            {
                BitConverter.GetBytes(frsUpdate.name[0]);
                count = count + 2;
            }

            BitConverter.GetBytes((uint)frsUpdate.flags).CopyTo(buff, count);
            count = count + BitConverter.GetBytes((uint)frsUpdate.flags).Length;

            return buff;
        }
        #endregion

        #region GetRdcFileInfo

        public static _FRS_RDC_FILEINFO GetRdcFileInfo(IntPtr frsFileInfo)
        {
            _FRS_RDC_FILEINFO rdcFile = new _FRS_RDC_FILEINFO();

            long ptr = (long)frsFileInfo;

            rdcFile.onDiskFileSize = (ulong)Marshal.ReadInt64((IntPtr)ptr);
            ptr += sizeof(ulong);
            rdcFile.fileSizeEstimate = (ulong)Marshal.ReadInt64((IntPtr)ptr);
            ptr += sizeof(ulong);
            rdcFile.rdcVersion = (ushort)Marshal.ReadInt16((IntPtr)ptr);
            ptr += sizeof(ushort);
            rdcFile.rdcMinimumCompatibleVersion = (ushort)Marshal.ReadInt16((IntPtr)ptr);
            ptr += sizeof(ushort);
            rdcFile.rdcSignatureLevels = Marshal.ReadByte((IntPtr)ptr);
            ptr += sizeof(byte);
            rdcFile.compressionAlgorithm = (_RDC_FILE_COMPRESSION_TYPES)Marshal.ReadByte((IntPtr)ptr);
            ptr += sizeof(byte);
            ptr += 5;

            _FRS_RDC_PARAMETERS[] frsRdcParam = new _FRS_RDC_PARAMETERS[rdcFile.rdcSignatureLevels];

            for (int i = 0; i < rdcFile.rdcSignatureLevels; i++)
            {
                frsRdcParam[i].rdcChunkerAlgorithm = (ushort)Marshal.ReadInt16((IntPtr)ptr);
                frsRdcParam[i].rdcChunkerAlgorithm = (ushort)(frsRdcParam[i].rdcChunkerAlgorithm << 8 | frsRdcParam[i].rdcChunkerAlgorithm >> 8);
                rdcFile.rdcFilterParameters = frsRdcParam[i];

                ptr += 8;
            }

            return rdcFile;
        }

        #endregion

        #region GetUpdateCancelData
        public static IntPtr GetUpdateCancelData(_FRS_UPDATE_CANCEL_DATA cancelData)
        {
            IntPtr cancelDataPtr = Marshal.AllocHGlobal(792);
            int ptrPos = 0;
            Marshal.WriteInt32(cancelDataPtr, ptrPos, (int)cancelData.blockingUpdate.present);
            ptrPos = ptrPos + Marshal.SizeOf((int)cancelData.blockingUpdate.present);

            Marshal.WriteInt32(cancelDataPtr, ptrPos, cancelData.blockingUpdate.nameConflict);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.blockingUpdate.nameConflict);

            Marshal.WriteInt32(cancelDataPtr, ptrPos, (int)cancelData.blockingUpdate.attributes);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.blockingUpdate.attributes);

            Marshal.WriteInt32(cancelDataPtr, ptrPos, (int)cancelData.blockingUpdate.fence.dwLowDateTime);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.blockingUpdate.fence.dwLowDateTime);

            Marshal.WriteInt32(cancelDataPtr, ptrPos, (int)cancelData.blockingUpdate.fence.dwHighDateTime);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.blockingUpdate.fence.dwHighDateTime);

            Marshal.WriteInt32(cancelDataPtr, ptrPos, (int)cancelData.blockingUpdate.clock.dwLowDateTime);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.blockingUpdate.clock.dwLowDateTime);

            Marshal.WriteInt32(cancelDataPtr, ptrPos, (int)cancelData.blockingUpdate.clock.dwHighDateTime);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.blockingUpdate.clock.dwHighDateTime);

            Marshal.WriteInt32(cancelDataPtr, ptrPos, (int)cancelData.blockingUpdate.createTime.dwLowDateTime);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.blockingUpdate.createTime.dwLowDateTime);

            Marshal.WriteInt32(cancelDataPtr, ptrPos, (int)cancelData.blockingUpdate.createTime.dwHighDateTime);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.blockingUpdate.createTime.dwHighDateTime);

            byte[] contentIdBuff = new byte[cancelData.blockingUpdate.contentSetId.ToByteArray().Length];
            cancelData.blockingUpdate.contentSetId.ToByteArray().CopyTo(contentIdBuff, 0);
            for (int i = 0; i < contentIdBuff.Length; i++)
            {
                Marshal.WriteByte(cancelDataPtr, ptrPos, contentIdBuff[i]);
                ptrPos = ptrPos + sizeof(byte);
            }

            for (int i = 0; i < cancelData.blockingUpdate.hash.Length; i++)
            {
                Marshal.WriteByte(cancelDataPtr, ptrPos, cancelData.blockingUpdate.hash[i]);
                ptrPos = ptrPos + sizeof(byte);
            }

            for (int i = 0; i < cancelData.blockingUpdate.rdcSimilarity.Length; i++)
            {
                Marshal.WriteByte(cancelDataPtr, ptrPos, cancelData.blockingUpdate.rdcSimilarity[i]);
                ptrPos = ptrPos + sizeof(byte);
            }

            byte[] uIdDbGuidBuff = new byte[cancelData.blockingUpdate.uidDbGuid.ToByteArray().Length];
            cancelData.blockingUpdate.uidDbGuid.ToByteArray().CopyTo(uIdDbGuidBuff, 0);
            for (int i = 0; i < uIdDbGuidBuff.Length; i++)
            {
                Marshal.WriteByte(cancelDataPtr, ptrPos, uIdDbGuidBuff[i]);
                ptrPos = ptrPos + sizeof(byte);
            }

            Marshal.WriteInt64(cancelDataPtr, ptrPos, (long)cancelData.blockingUpdate.uidVersion);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.blockingUpdate.uidVersion);

            byte[] gvsnDbGuidBuff = new byte[cancelData.blockingUpdate.gvsnDbGuid.ToByteArray().Length];
            cancelData.blockingUpdate.gvsnDbGuid.ToByteArray().CopyTo(gvsnDbGuidBuff, 0);
            for (int i = 0; i < gvsnDbGuidBuff.Length; i++)
            {
                Marshal.WriteByte(cancelDataPtr, ptrPos, gvsnDbGuidBuff[i]);
                ptrPos = ptrPos + sizeof(byte);
            }

            Marshal.WriteInt64(cancelDataPtr, ptrPos, (long)cancelData.blockingUpdate.gvsnVersion);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.blockingUpdate.gvsnVersion);

            byte[] parentDbGuidBuff = new byte[cancelData.blockingUpdate.parentDbGuid.ToByteArray().Length];
            cancelData.blockingUpdate.parentDbGuid.ToByteArray().CopyTo(parentDbGuidBuff, 0);
            for (int i = 0; i < parentDbGuidBuff.Length; i++)
            {
                Marshal.WriteByte(cancelDataPtr, ptrPos, parentDbGuidBuff[i]);
                ptrPos = ptrPos + sizeof(byte);
            }

            Marshal.WriteInt64(cancelDataPtr, ptrPos, (long)cancelData.blockingUpdate.parentVersion);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.blockingUpdate.parentVersion);

            for (int i = 0; i < cancelData.blockingUpdate.name.Length; i++)
            {
                Marshal.WriteInt16(cancelDataPtr, ptrPos, cancelData.blockingUpdate.name[i]);
                ptrPos = ptrPos + Marshal.SizeOf(cancelData.blockingUpdate.name[i]);
            }

            Marshal.WriteInt32(cancelDataPtr, ptrPos, (int)cancelData.blockingUpdate.flags);
            ptrPos = ptrPos + Marshal.SizeOf((int)cancelData.blockingUpdate.flags);

            byte[] cancelContentIdBuff = new byte[cancelData.contentSetId.ToByteArray().Length];
            cancelData.contentSetId.ToByteArray().CopyTo(cancelContentIdBuff, 0);
            for (int i = 0; i < cancelContentIdBuff.Length; i++)
            {
                Marshal.WriteByte(cancelDataPtr, ptrPos, cancelContentIdBuff[i]);
                ptrPos = ptrPos + sizeof(byte);
            }

            byte[] cancelGvsnDbIdBuff = new byte[cancelData.gvsnDatabaseId.ToByteArray().Length];
            cancelData.gvsnDatabaseId.ToByteArray().CopyTo(cancelGvsnDbIdBuff, 0);
            for (int i = 0; i < cancelGvsnDbIdBuff.Length; i++)
            {
                Marshal.WriteByte(cancelDataPtr, ptrPos, cancelGvsnDbIdBuff[i]);
                ptrPos = ptrPos + sizeof(byte);
            }

            byte[] cancelUidDbIdBuff = new byte[cancelData.uidDatabaseId.ToByteArray().Length];
            cancelData.uidDatabaseId.ToByteArray().CopyTo(cancelUidDbIdBuff, 0);
            for (int i = 0; i < cancelUidDbIdBuff.Length; i++)
            {
                Marshal.WriteByte(cancelDataPtr, ptrPos, cancelUidDbIdBuff[i]);
                ptrPos = ptrPos + sizeof(byte);
            }

            byte[] cancelParentDbIdBuff = new byte[cancelData.parentDatabaseId.ToByteArray().Length];
            cancelData.parentDatabaseId.ToByteArray().CopyTo(cancelParentDbIdBuff, 0);
            for (int i = 0; i < cancelParentDbIdBuff.Length; i++)
            {
                Marshal.WriteByte(cancelDataPtr, ptrPos, cancelParentDbIdBuff[i]);
                ptrPos = ptrPos + sizeof(byte);
            }

            Marshal.WriteInt64(cancelDataPtr, ptrPos, (long)cancelData.gvsnVersion);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.gvsnVersion);

            Marshal.WriteInt64(cancelDataPtr, ptrPos, (long)cancelData.uidVersion);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.uidVersion);

            Marshal.WriteInt64(cancelDataPtr, ptrPos, (long)cancelData.parentVersion);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.parentVersion);

            Marshal.WriteInt32(cancelDataPtr, ptrPos, (int)cancelData.cancelType);
            ptrPos = ptrPos + Marshal.SizeOf((int)cancelData.cancelType);

            Marshal.WriteInt32(cancelDataPtr, ptrPos, cancelData.isUidValid);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.isUidValid);

            Marshal.WriteInt32(cancelDataPtr, ptrPos, cancelData.isParentUidValid);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.isParentUidValid);

            Marshal.WriteInt32(cancelDataPtr, ptrPos, cancelData.isBlockerValid);
            ptrPos = ptrPos + Marshal.SizeOf(cancelData.isBlockerValid);

            return cancelDataPtr;
        }
        #endregion

        public static _FRS_ID_GVSN[] DecompressRecords(IntPtr compressedRecords)
        {
            _FRS_ID_GVSN[] decompressRecords = new _FRS_ID_GVSN[10];
            return decompressRecords;
        }

        public static byte[] PtrToByteArray(IntPtr bufferPtr, uint len)
        {
            byte[] buffer = new byte[len];

            long pos = (long)bufferPtr;

            for (int i = 0; i < len; i++)
            {
                buffer[i] = Marshal.ReadByte((IntPtr)pos);
                pos = pos + sizeof(byte);
            }

            return buffer;
        }



        public static string ComputeNTDSConnectionPath(string domaindns, string dcname)
        {
            string[] str = domaindns.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if ((str == null || str.Length == 0) && ConfigStore.IsTestSYSVOL)
                TestSite.Assert.Fail("Invalid domain anme");
            string dn = "";
            for (int i = 0; i < str.Length; i++)
            {
                dn += ("dc=" + str[i] + ",");
            }
            dn = dn.Remove(dn.Length - 1);
            return "cn=ntds settings,cn=" + dcname + ",cn=servers,cn=default-first-site-name,cn=sites,cn=configuration," + dn;
        }

    }




}
