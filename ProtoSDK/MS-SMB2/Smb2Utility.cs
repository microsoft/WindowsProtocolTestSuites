// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// Utility function of smb2
    /// </summary>
    public static class Smb2Utility
    {
        #region CONST

        /// <summary>
        /// Compounded requests MUST be aligned on 8-byte boundaries, so the alignment factor is 8.
        /// </summary>
        private const int alignmentFactor8 = 8;

        /// <summary>
        /// aligned on 4 bytes boundaries.
        /// </summary>
        private const int alignmentFactor4 = 4;

        // The max credits client can have.
        private const int maxCredits = 400;

        // If current credits < maxCredits - 50, the credits client will request.
        private const ushort creditsA = 10;

        // If maxCredits - 50 <= current credits < maxCredits, the credits the client will request.
        private const ushort creditsB = 1;

        //If current credits >= maxCredits, the credits the client will request.
        private const ushort creditsC = 0;

        //The max credits server will grand for one request.
        private const ushort maxCreditsServerGrandedInOneResponse = 100;

        private const int sizeOfWChar = 2;
        #endregion

        private static List<DialectRevision> allDialects;

        /// <summary>
        /// Use static constructor to initialize allDialects to make sure the value of allDialects can only be set once.
        /// </summary>
        static Smb2Utility()
        {
            allDialects = new List<DialectRevision>();
            foreach (DialectRevision dialect in Enum.GetValues(typeof(DialectRevision)))
            {
                //Exclude Smb2Unknown and Smb2Wildcard
                if ((DialectRevision)dialect != DialectRevision.Smb2Unknown
                    && (DialectRevision)dialect != DialectRevision.Smb2Wildcard)
                {
                    allDialects.Add(dialect);
                }
            }
            allDialects.Sort();
        }

        public static IEnumerable<byte> ConcatStruct<T>(this IEnumerable<byte> first, T second) where T : struct
        {
            return first.Concat(TypeMarshal.ToBytes(second));
        }

        /// <summary>
        /// Marshals struct array to bytes. Based on TypeMarshal.
        /// </summary>
        /// <typeparam name="T">The marshal type</typeparam>
        /// <param name="array">The array to marshal</param>
        /// <returns>The bytes marshalled from array.</returns>
        public static byte[] MarshalStructArray<T>(T[] array) where T : struct
        {
            int structSize = TypeMarshal.GetNativeMemorySize<T>();
            byte[] buffer = new byte[structSize * array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                Array.Copy(TypeMarshal.ToBytes(array[i]), 0, buffer, structSize * i, structSize);
            }

            return buffer;
        }

        /// <summary>
        /// Unmarshals bytes to specified type array. Based on TypeMarshal.
        /// </summary>
        /// <typeparam name="T">The target marshal type</typeparam>
        /// <param name="buffer">The bytes</param>
        /// <param name="count">The array length</param>
        /// <returns>The T array.</returns>
        public static T[] UnmarshalStructArray<T>(byte[] buffer, uint count) where T : struct
        {
            T[] array = new T[count];

            int offset = 0;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = TypeMarshal.ToStruct<T>(buffer, ref offset);
            }

            return array;
        }

        /// <summary>
        /// Marshals a structure object to bytes. 
        /// Based on System.Runtime.InteropServices.Marshal.
        /// </summary>
        /// <param name="obj">The object to marshal</param>
        /// <returns>Byte array.</returns>
        public static byte[] MarshalStructure(object obj)
        {
            int len = Marshal.SizeOf(obj);
            byte[] arr = new byte[len];
            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        /// <summary>
        /// Unmarshals a byte array to the specified structure.
        /// Based on System.Runtime.InteropServices.Marshal.
        /// </summary>
        /// <param name="data">The bytes from which to unmarshal.</param>
        /// <param name="obj">The unmarshalled object.</param>
        public static T UnmarshalStructure<T>(byte[] data)
        {
            T obj;
            int len = Marshal.SizeOf(typeof(T));
            IntPtr i = Marshal.AllocHGlobal(len);
            Marshal.Copy(data, 0, i, len);
            obj = (T)Marshal.PtrToStructure(i, typeof(T));
            Marshal.FreeHGlobal(i);
            return obj;
        }

        public static void ZeroOutBuffer(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }
        }

        public static T[] Append<T>(this T[] source, T element)
        {
            if (source == null)
                return new T[] { element };

            return source.Concat(new T[] { element }).ToArray();
        }

        /// <summary>
        /// [Deprecated method. Please use TypeMarshal.ToBytes instead]
        /// Convert a structure to byte array, this method mainly is used for marshal the structure which is
        /// not defined in Smb2
        /// </summary>
        /// <typeparam name="T">The structure type</typeparam>
        /// <param name="dataStructure">The structure</param>
        /// <returns>The byte array</returns>
        public static byte[] ToBytes<T>(T dataStructure) where T : struct
        {
            return TypeMarshal.ToBytes(dataStructure);
        }


        /// <summary>
        /// [Deprecated method. Please use TypeMarshal.ToStruct instead]
        /// Convert byte array to structure,  this method mainly is used for un-marshal the structure which is
        /// not defined in Smb2
        /// </summary>
        /// <typeparam name="T">The structure type</typeparam>
        /// <param name="data">The data buffer to be converted</param>
        /// <returns>The converted structure</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T ToStructure<T>(byte[] data) where T : struct
        {
            return ToStructure<T>(data, 0, data.Length);
        }


        /// <summary>
        /// [Deprecated method. Please use TypeMarshal.ToStruct instead]
        /// Convert byte array to structure,  this method mainly is used for un-marshal the structure which is
        /// not defined in Smb2
        /// </summary>
        /// <typeparam name="T">The structure type</typeparam>
        /// <param name="data">The data buffer to be converted</param>
        /// <param name="offset">The offset of data from where the data will be used</param>
        /// <param name="count">The used dataLen</param>
        /// <returns>The converted structure</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T ToStructure<T>(byte[] data, int offset, int count) where T : struct
        {
            return TypeMarshal.ToStruct<T>(ArrayUtility.SubArray(data, offset, count));
        }


        /// <summary>
        /// Convert time format
        /// </summary>
        /// <param name="time">The DateTime format time</param>
        /// <returns>The _FILETIME format</returns>
        public static _FILETIME DateTimeToFileTime(DateTime time)
        {
            long dateTime = time.ToFileTimeUtc();

            _FILETIME fileTime = new _FILETIME();

            fileTime.dwLowDateTime = (uint)dateTime;
            fileTime.dwHighDateTime = (uint)(dateTime >> 32);

            return fileTime;
        }

        public static byte[] CreateBufferFromString(string str)
        {
            return Encoding.Unicode.GetBytes(str);
        }

        /// <summary>
        /// Create SymbolicLinkReparseBuffer
        /// </summary>
        /// <param name="flags">The flag</param>
        /// <param name="unparsedPathLength">The length, in bytes, of the unparsed portion of the path.</param>
        /// <param name="substituteName">The substitute name of the symbol link</param>
        /// <param name="printName">A friendly name</param>
        /// <returns>A SymbolicLinkReparseBuffer object</returns>
        public static SymbolicLinkReparseBuffer CreateSymbolicLinkReparseBuffer(
            SymbolicLinkReparseBuffer_Flags_Values flags,
            ushort unparsedPathLength,
            string substituteName,
            string printName
            )
        {
            if (substituteName == null)
            {
                throw new ArgumentNullException("substituteName");
            }

            if (printName == null)
            {
                throw new ArgumentNullException("printName");
            }

            SymbolicLinkReparseBuffer sybolicLinkReparse = new SymbolicLinkReparseBuffer();

            sybolicLinkReparse.Flags = flags;
            sybolicLinkReparse.PathBuffer = new byte[substituteName.Length * sizeOfWChar + printName.Length * sizeOfWChar];
            sybolicLinkReparse.PrintNameLength = (ushort)(printName.Length * sizeOfWChar);
            sybolicLinkReparse.ReparseDataLength = (ushort)(Smb2Consts.StaticPortionSizeInSymbolicLinkErrorResponse
                + sybolicLinkReparse.PathBuffer.Length);
            sybolicLinkReparse.ReparseTag = SymbolicLinkReparseBuffer_ReparseTag_Values.IO_REPARSE_TAG_SYMLINK;
            sybolicLinkReparse.Reserved = 0;
            sybolicLinkReparse.SubstituteNameLength = (ushort)(substituteName.Length * sizeOfWChar);
            sybolicLinkReparse.SubstituteNameOffset = 0;
            sybolicLinkReparse.PrintNameOffset = (ushort)sybolicLinkReparse.SubstituteNameLength;

            Array.Copy(Encoding.Unicode.GetBytes(substituteName), sybolicLinkReparse.PathBuffer, sybolicLinkReparse.SubstituteNameLength);
            Array.Copy(Encoding.Unicode.GetBytes(printName), 0, sybolicLinkReparse.PathBuffer, sybolicLinkReparse.PrintNameOffset,
                sybolicLinkReparse.PrintNameLength);

            return sybolicLinkReparse;
        }


        /// <summary>
        /// Create CREATE_CONTEXT_Request_Values or Create CREATE_CONTEXT_Response_Values
        /// </summary>
        /// <param name="contextType">The context type</param>
        /// <param name="contextData">The context data, corresponding to the buffer TD 2.2.13.2 
        /// dataOffset, dataLen refered</param>
        /// <returns>The CREATE_CONTEXT_Values</returns>
        public static CREATE_CONTEXT CreateCreateContextValues(
            CreateContextTypeValue contextType,
            byte[] contextData
            )
        {
            string contextName = null;

            switch (contextType)
            {
                case CreateContextTypeValue.SMB2_CREATE_EA_BUFFER:
                    contextName = CreateContextNames.SMB2_CREATE_EA_BUFFER;
                    break;
                case CreateContextTypeValue.SMB2_CREATE_SD_BUFFER:
                    contextName = CreateContextNames.SMB2_CREATE_SD_BUFFER;
                    break;
                case CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST:
                    contextName = CreateContextNames.SMB2_CREATE_DURABLE_HANDLE_REQUEST;
                    break;
                case CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT:
                    contextName = CreateContextNames.SMB2_CREATE_DURABLE_HANDLE_RECONNECT;
                    break;
                case CreateContextTypeValue.SMB2_CREATE_ALLOCATION_SIZE:
                    contextName = CreateContextNames.SMB2_CREATE_ALLOCATION_SIZE;
                    break;
                case CreateContextTypeValue.SMB2_CREATE_QUERY_MAXIMAL_ACCESS_REQUEST:
                    contextName = CreateContextNames.SMB2_CREATE_QUERY_MAXIMAL_ACCESS_REQUEST;
                    break;
                case CreateContextTypeValue.SMB2_CREATE_TIMEWARP_TOKEN:
                    contextName = CreateContextNames.SMB2_CREATE_TIMEWARP_TOKEN;
                    break;
                case CreateContextTypeValue.SMB2_CREATE_QUERY_ON_DISK_ID:
                    contextName = CreateContextNames.SMB2_CREATE_QUERY_ON_DISK_ID;
                    break;
                case CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE:
                    contextName = CreateContextNames.SMB2_CREATE_REQUEST_LEASE;
                    break;
                case CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2:
                    contextName = CreateContextNames.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2;
                    break;
                case CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2:
                    contextName = CreateContextNames.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2;
                    break;

            }

            if (contextName == null)
            {
                throw new ArgumentException("The contextType is not supported", "contextType");
            }

            CREATE_CONTEXT createContext = new CREATE_CONTEXT();

            createContext.NameOffset = Smb2Consts.NameOffsetInCreateContextValues;

            createContext.NameLength = (ushort)contextName.Length;

            if (contextData != null)
            {
                createContext.DataOffset = (ushort)(createContext.NameOffset + Smb2Utility.AlignBy8Bytes(
                    createContext.NameLength));

                createContext.DataLength = (uint)contextData.Length;
            }

            byte[] nameArray = Encoding.ASCII.GetBytes(contextName);

            if (createContext.DataOffset != 0)
            {
                createContext.Buffer = new byte[createContext.DataOffset - createContext.NameOffset
                    + createContext.DataLength];

                Array.Copy(nameArray, createContext.Buffer, nameArray.Length);
                Array.Copy(contextData, 0, createContext.Buffer, createContext.DataOffset - createContext.NameOffset,
                    contextData.Length);
            }
            else
            {
                createContext.Buffer = new byte[createContext.NameLength];

                Array.Copy(nameArray, createContext.Buffer, nameArray.Length);
            }

            return createContext;
        }


        /// <summary>
        /// Create FILE_NOTIFY_INFORMATION
        /// </summary>
        /// <param name="action">The changes that occurred on the file. This field MUST
        ///  contain one of the following values.</param>
        /// <param name="fileName">The fileName</param>
        /// <returns>A FILE_NOTIFY_INFORMATION object</returns>
        public static FILE_NOTIFY_INFORMATION CreateFileNotifyInformation(
             Action_Values action,
             string fileName
            )
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            FILE_NOTIFY_INFORMATION fileNotifyInfo = new FILE_NOTIFY_INFORMATION();

            fileNotifyInfo.Action = action;
            fileNotifyInfo.FileName = Encoding.Unicode.GetBytes(fileName);
            fileNotifyInfo.FileNameLength = (uint)fileNotifyInfo.FileName.Length;

            return fileNotifyInfo;
        }


        /// <summary>
        /// Set offset field of FILE_NOTIFY_INFORMATION
        /// </summary>
        /// <param name="fileNotifyInformations">A list of FILE_NOTIFY_INFORMATION data</param>
        public static void SetOffset(FILE_NOTIFY_INFORMATION[] fileNotifyInformations)
        {
            if (fileNotifyInformations == null)
            {
                throw new ArgumentNullException("fileNotifyInformations");
            }

            for (int i = 0; i < fileNotifyInformations.Length - 1; i++)
            {
                fileNotifyInformations[i].NextEntryOffset = (uint)AlignBy4Bytes(TypeMarshal.ToBytes(fileNotifyInformations[i]).Length);
            }
        }


        /// <summary>
        /// Generate lease key for client to user in Smb2CreateCreateRequest with
        /// SMB2_CREATE_REQUEST_LEASE createContext
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateLeaseKey()
        {
            Guid leaseKey = Guid.NewGuid();

            return leaseKey.ToByteArray();
        }

        public static void Align8(ref byte[] buffer)
        {
            Align8(0, ref buffer);
        }

        public static void Align8(uint baseOffset, ref byte[] buffer)
        {
            if ((buffer.Length + baseOffset) % 8 != 0)
            {
                buffer = buffer.Concat(new byte[8 - (buffer.Length + baseOffset) % 8]).ToArray();
            }
        }

        public static void Align8(ref uint length)
        {
            if (length % 8 != 0)
            {
                length += 8 - length % 8;
            }
        }


        /// <summary>
        /// Padding bytes to the end of the originalBuffer to make sure the length is multiple of 8
        /// </summary>
        /// <param name="originalBuffer">The orginal buffer</param>
        /// <returns>The padded buffer</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Throw when originalBuffer is null.
        /// </exception>
        internal static byte[] PaddingTo8BytesAlignedBuffer(byte[] originalBuffer)
        {
            if (originalBuffer == null)
            {
                throw new ArgumentNullException("originalBuffer");
            }

            if (originalBuffer.Length % alignmentFactor8 != 0)
            {
                int paddingLen = alignmentFactor8 - originalBuffer.Length % alignmentFactor8;

                byte[] alignedBuffer = new byte[originalBuffer.Length + paddingLen];

                originalBuffer.CopyTo(alignedBuffer, 0);

                return alignedBuffer;
            }
            else
            {
                return (byte[])originalBuffer.Clone();
            }
        }


        /// <summary>
        /// Calculate the 8 aligned length for the given length
        /// </summary>
        /// <param name="originalLength">The orginal length</param>
        /// <returns>The caculated aligned length</returns>
        internal static int AlignBy8Bytes(int originalLength)
        {
            return AlignByNBytes(originalLength, alignmentFactor8);
        }


        /// <summary>
        /// Calculate the 4 aligned length for the given length
        /// </summary>
        /// <param name="originalLength">The orginal length</param>
        /// <returns>The caculated aligned length</returns>
        internal static int AlignBy4Bytes(int originalLength)
        {
            return AlignByNBytes(originalLength, alignmentFactor4);
        }


        /// <summary>
        /// Calculate aligned length for the original length
        /// </summary>
        /// <param name="originalLength">The original</param>
        /// <param name="alignmentFactor">The alignment factor</param>
        /// <returns>The caculated aligned length</returns>
        internal static int AlignByNBytes(int originalLength, int alignmentFactor)
        {
            if (originalLength < 0)
            {
                throw new ArgumentException("originalLength must larger than zero", "originalLength");
            }

            if (alignmentFactor <= 0)
            {
                throw new ArgumentException("originalLength must larger than zero", "alignmentFactor");
            }

            int remainder = originalLength % alignmentFactor;

            if (remainder > 0)
            {
                return originalLength + alignmentFactor - remainder;
            }
            else
            {
                return originalLength;
            }
        }


        /// <summary>
        /// Assemble processId and treeId to asyncId
        /// </summary>
        /// <param name="processId">The processId in smb2 packet header</param>
        /// <param name="treeId">The treeId in smb2 packet header</param>
        /// <returns>asyncId</returns>
        internal static ulong AssembleToAsyncId(uint processId, uint treeId)
        {
            //aysncID is 8 bytes in ASYNC Header, which can be convert from 
            //processeId,treeId in little-endian order.
            ulong asyncId = ((ulong)treeId << 32) + processId;

            return asyncId;
        }


        /// <summary>
        /// Generate Tcp transport payload, this function is used to prefix length header to smb2 packet
        /// </summary>
        /// <param name="packet">The smb2 packet data</param>
        /// <returns>The generated tcp transport payload</returns>
        internal static byte[] GenerateTcpTransportPayLoad(byte[] packet)
        {
            byte[] temp = BitConverter.GetBytes(packet.Length);

            byte[] streamProtoclLen = new byte[temp.Length];

            //The length, in bytes, of the SMB message. This length is formatted as a 3-byte integer in network byte order.
            streamProtoclLen[1] = temp[2];
            streamProtoclLen[2] = temp[1];
            streamProtoclLen[3] = temp[0];

            byte[] tcpPayLoad = new byte[packet.Length + streamProtoclLen.Length];

            Array.Copy(streamProtoclLen, tcpPayLoad, streamProtoclLen.Length);
            Array.Copy(packet, 0, tcpPayLoad, streamProtoclLen.Length, packet.Length);

            return tcpPayLoad;
        }


        /// <summary>
        /// Test if the contents of the two array are matched.
        /// </summary>
        /// <param name="array1">The array to be tested</param>
        /// <param name="array2">The array to be tested</param>
        /// <returns>True if they contain same content, else false</returns>
        internal static bool AreEqual(byte[] array1, byte[] array2)
        {
            if (array1 == null && array2 == null)
            {
                return true;
            }

            if (array1 == null || array2 == null)
            {
                return false;
            }

            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Calculate the credits client will request or server will grand
        /// </summary>
        /// <param name="requestCredits">The credits client request</param>
        /// <param name="currentCredits">The current credits client has</param>
        /// <returns>The credits client will request or server will grand</returns>
        internal static ushort CaculateResponseCredits(ushort requestCredits, int currentCredits)
        {
            if (currentCredits < (maxCredits - 5 * creditsA))
            {
                if (requestCredits == 0)
                {
                    if (currentCredits == 0)
                    {
                        //at lease make sure client will have 1 credits
                        return creditsB;
                    }
                    else
                    {
                        return requestCredits;
                    }
                }
                else if (requestCredits < maxCreditsServerGrandedInOneResponse)
                {
                    return requestCredits;
                }
                else
                {
                    return creditsA;
                }
            }
            else if (currentCredits < maxCredits)
            {
                return creditsB;
            }
            else
            {
                return creditsC;
            }
        }


        /// <summary>
        /// Calculate the credits client will ask server for based on current credits it has
        /// </summary>
        /// <param name="currentCredits">The current credits it has</param>
        /// <returns>The credits it will request</returns>
        internal static ushort CaculateRequestCredits(int currentCredits)
        {
            return CaculateResponseCredits(maxCreditsServerGrandedInOneResponse, currentCredits);
        }

        internal static byte[] MarshalCreateContextRequests(Smb2CreateContextRequest[] requests)
        {
            byte[] buffer = new byte[0];
            List<CREATE_CONTEXT> createContextStructs = new List<CREATE_CONTEXT>();

            foreach (var request in requests)
            {
                string name = null;
                byte[] dataBuffer = null;

                var createEaBuffer = request as Smb2CreateEaBuffer;
                if (createEaBuffer != null)
                {
                    name = CreateContextNames.SMB2_CREATE_EA_BUFFER;

                    List<FileFullEaInformation> fileFullEaInfoStructs = new List<FileFullEaInformation>();
                    foreach (var fileFullEaInfo in createEaBuffer.FileFullEaInformations)
                    {
                        var fileFullEaInfoStruct = new FileFullEaInformation
                        {
                            EaName = Encoding.ASCII.GetBytes(fileFullEaInfo.EaName + "\0"),
                            EaNameLength = (byte)Encoding.ASCII.GetByteCount(fileFullEaInfo.EaName),
                            EaValue = fileFullEaInfo.EaValue.ToArray(),
                            EaValueLength = (ushort)fileFullEaInfo.EaValue.Length,
                            Flags = fileFullEaInfo.Flags
                        };

                        fileFullEaInfoStruct.NextEntryOffset = (uint)TypeMarshal.GetBlockMemorySize(fileFullEaInfoStruct);

                        fileFullEaInfoStructs.Add(fileFullEaInfoStruct);
                    }

                    if (fileFullEaInfoStructs.Count > 0)
                    {
                        var last = fileFullEaInfoStructs[fileFullEaInfoStructs.Count - 1];
                        last.NextEntryOffset = 0;
                        fileFullEaInfoStructs[fileFullEaInfoStructs.Count - 1] = last;
                    }

                    var createEaBufferStruct = new CREATE_EA_BUFFER
                    {
                        FileFullInformations = fileFullEaInfoStructs.ToArray()
                    };

                    dataBuffer = TypeMarshal.ToBytes(createEaBufferStruct);

                    goto encapsulateCreateContextStruct;
                }

                var createSdBuffer = request as Smb2CreateSdBuffer;
                if (createSdBuffer != null)
                {
                    name = CreateContextNames.SMB2_CREATE_SD_BUFFER;

                    var sid = new SecurityIdentifier(createSdBuffer.SID);
                    dataBuffer = new byte[sid.BinaryLength];
                    sid.GetBinaryForm(dataBuffer, 0);

                    goto encapsulateCreateContextStruct;
                }

                var createDurableHandleRequest = request as Smb2CreateDurableHandleRequest;
                if (createDurableHandleRequest != null)
                {
                    name = CreateContextNames.SMB2_CREATE_DURABLE_HANDLE_REQUEST;
                    var createDurableHandleRequestStruct = new CREATE_DURABLE_HANDLE_REQUEST
                    {
                        DurableRequest = createDurableHandleRequest.DurableRequest
                    };
                    dataBuffer = TypeMarshal.ToBytes(createDurableHandleRequestStruct);

                    goto encapsulateCreateContextStruct;
                }

                var createDurableHandleReconnect = request as Smb2CreateDurableHandleReconnect;
                if (createDurableHandleReconnect != null)
                {
                    name = CreateContextNames.SMB2_CREATE_DURABLE_HANDLE_RECONNECT;
                    var createDurableHandleReconnectStruct = new CREATE_DURABLE_HANDLE_RECONNECT
                    {
                        Data = createDurableHandleReconnect.Data
                    };
                    dataBuffer = TypeMarshal.ToBytes(createDurableHandleReconnectStruct);

                    goto encapsulateCreateContextStruct;
                }

                var createAllocationSize = request as Smb2CreateAllocationSize;
                if (createAllocationSize != null)
                {
                    name = CreateContextNames.SMB2_CREATE_ALLOCATION_SIZE;
                    var createAllocationSizeStruct = new CREATE_ALLOCATION_SIZE
                    {
                        AllocationSize = createAllocationSize.AllocationSize
                    };
                    dataBuffer = TypeMarshal.ToBytes(createAllocationSizeStruct);

                    goto encapsulateCreateContextStruct;
                }

                var createQueryMaximalAccessRequest = request as Smb2CreateQueryMaximalAccessRequest;
                if (createQueryMaximalAccessRequest != null)
                {
                    name = CreateContextNames.SMB2_CREATE_QUERY_MAXIMAL_ACCESS_REQUEST;
                    var createQueryMaximalAccessRequestStruct = new CREATE_QUERY_MAXIMAL_ACCESS_REQUEST
                    {
                        Timestamp = createQueryMaximalAccessRequest.Timestamp
                    };
                    dataBuffer = TypeMarshal.ToBytes(createQueryMaximalAccessRequestStruct);

                    goto encapsulateCreateContextStruct;
                }

                var createQueryMaximalAccessRequestEmpty = request as Smb2CreateQueryMaximalAccessRequestEmpty;
                if (createQueryMaximalAccessRequestEmpty != null)
                {
                    name = CreateContextNames.SMB2_CREATE_QUERY_MAXIMAL_ACCESS_REQUEST;
                    dataBuffer = new byte[0];

                    goto encapsulateCreateContextStruct;
                }

                var createTimewrapToken = request as Smb2CreateTimewrapToken;
                if (createTimewrapToken != null)
                {
                    name = CreateContextNames.SMB2_CREATE_TIMEWARP_TOKEN;
                    var createTimewrapTokenStruct = new CREATE_TIMEWARP_TOKEN
                    {
                        Timestamp = createTimewrapToken.Timestamp
                    };
                    dataBuffer = TypeMarshal.ToBytes(createTimewrapTokenStruct);

                    goto encapsulateCreateContextStruct;
                }

                var createQueryOnDiskId = request as Smb2CreateQueryOnDiskId;
                if (createQueryOnDiskId != null)
                {
                    name = CreateContextNames.SMB2_CREATE_QUERY_ON_DISK_ID;
                    dataBuffer = new byte[0];

                    goto encapsulateCreateContextStruct;
                }

                var createRequestLease = request as Smb2CreateRequestLease;
                if (createRequestLease != null)
                {
                    name = CreateContextNames.SMB2_CREATE_REQUEST_LEASE;
                    var createRequestLeaseStruct = new CREATE_REQUEST_LEASE
                    {
                        LeaseDuration = createRequestLease.LeaseDuration,
                        LeaseFlags = createRequestLease.LeaseFlags,
                        LeaseKey = createRequestLease.LeaseKey,
                        LeaseState = createRequestLease.LeaseState
                    };
                    dataBuffer = TypeMarshal.ToBytes(createRequestLeaseStruct);

                    goto encapsulateCreateContextStruct;
                }

                var createRequestLeaseV2 = request as Smb2CreateRequestLeaseV2;
                if (createRequestLeaseV2 != null)
                {
                    name = CreateContextNames.SMB2_CREATE_REQUEST_LEASE_V2;
                    var createRequestLeaseV2Struct = new CREATE_REQUEST_LEASE_V2
                    {
                        LeaseDuration = createRequestLeaseV2.LeaseDuration,
                        LeaseFlags = createRequestLeaseV2.LeaseFlags,
                        LeaseKey = createRequestLeaseV2.LeaseKey,
                        LeaseState = createRequestLeaseV2.LeaseState,
                        Epoch = createRequestLeaseV2.Epoch,
                        ParentLeaseKey = createRequestLeaseV2.ParentLeaseKey
                    };
                    dataBuffer = TypeMarshal.ToBytes(createRequestLeaseV2Struct);

                    goto encapsulateCreateContextStruct;
                }

                var createDurableHandleRequestV2 = request as Smb2CreateDurableHandleRequestV2;
                if (createDurableHandleRequestV2 != null)
                {
                    name = CreateContextNames.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2;
                    var createDurableHandleRequestV2Struct = new CREATE_DURABLE_HANDLE_REQUEST_V2
                    {
                        CreateGuid = createDurableHandleRequestV2.CreateGuid,
                        Timeout = createDurableHandleRequestV2.Timeout,
                        Flags = createDurableHandleRequestV2.Flags,
                    };
                    dataBuffer = TypeMarshal.ToBytes(createDurableHandleRequestV2Struct);

                    goto encapsulateCreateContextStruct;
                }

                var createDurableHandleReconnectV2 = request as Smb2CreateDurableHandleReconnectV2;
                if (createDurableHandleReconnectV2 != null)
                {
                    name = CreateContextNames.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2;
                    var createDurableHandleReconnectV2Struct = new CREATE_DURABLE_HANDLE_RECONNECT_V2
                    {
                        FileId = createDurableHandleReconnectV2.FileId,
                        CreateGuid = createDurableHandleReconnectV2.CreateGuid,
                        Flags = createDurableHandleReconnectV2.Flags,
                    };
                    dataBuffer = TypeMarshal.ToBytes(createDurableHandleReconnectV2Struct);

                    goto encapsulateCreateContextStruct;
                }

                var createAppInstanceId = request as Smb2CreateAppInstanceId;
                if (createAppInstanceId != null)
                {
                    name = CreateContextNames.SMB2_CREATE_APP_INSTANCE_ID;
                    var createAppInstanceIdStruct = new CREATE_APP_INSTANCE_ID
                    {
                        StructureSize = 20,
                        AppInstanceId = createAppInstanceId.AppInstanceId
                    };
                    dataBuffer = TypeMarshal.ToBytes(createAppInstanceIdStruct);

                    goto encapsulateCreateContextStruct;
                }

                var createAppInstanceVersion = request as Smb2CreateAppInstanceVersion;
                if (createAppInstanceVersion != null)
                {
                    name = CreateContextNames.SMB2_CREATE_APP_INSTANCE_VERSION;
                    var createAppInstanceVersionStruct = new CREATE_APP_INSTANCE_VERSION
                    {
                        StructureSize = 24,
                        Reserved = 0,
                        AppInstanceVersionHigh = createAppInstanceVersion.AppInstanceVersionHigh,
                        AppInstanceVersionLow = createAppInstanceVersion.AppInstanceVersionLow
                    };
                    dataBuffer = TypeMarshal.ToBytes(createAppInstanceVersionStruct);

                    goto encapsulateCreateContextStruct;
                }

                var svhdxOpenDeviceContext = request as Smb2CreateSvhdxOpenDeviceContext;
                if (svhdxOpenDeviceContext != null)
                {
                    name = CreateContextNames.SVHDX_OPEN_DEVICE_CONTEXT;
                    var svhdxOpenDeviceContextStruct = new SVHDX_OPEN_DEVICE_CONTEXT
                    {
                        Version = svhdxOpenDeviceContext.Version,
                        HasInitiatorId = svhdxOpenDeviceContext.HasInitiatorId,
                        Reserved = new byte[3],
                        InitiatorId = svhdxOpenDeviceContext.InitiatorId,
                        Flags = svhdxOpenDeviceContext.Flags,
                        OriginatorFlags = svhdxOpenDeviceContext.OriginatorFlags,
                        OpenRequestId = svhdxOpenDeviceContext.OpenRequestId,
                        InitiatorHostNameLength = svhdxOpenDeviceContext.InitiatorHostNameLength,
                    };

                    byte[] initiatorHostNameBinary = Encoding.Unicode.GetBytes(svhdxOpenDeviceContext.InitiatorHostName);
                    if (svhdxOpenDeviceContext.InitiatorHostName.Length < 126)
                    {
                        svhdxOpenDeviceContextStruct.InitiatorHostName = new byte[126];
                        Buffer.BlockCopy(
                            initiatorHostNameBinary,
                            0,
                            svhdxOpenDeviceContextStruct.InitiatorHostName,
                            0,
                            svhdxOpenDeviceContext.InitiatorHostNameLength);
                    }
                    else
                    {
                        svhdxOpenDeviceContextStruct.InitiatorHostName = initiatorHostNameBinary;
                    }


                    dataBuffer = TypeMarshal.ToBytes(svhdxOpenDeviceContextStruct);
                    goto encapsulateCreateContextStruct;
                }

                var svhdxOpenDeviceContextV2 = request as Smb2CreateSvhdxOpenDeviceContextV2;
                if (svhdxOpenDeviceContextV2 != null)
                {
                    name = CreateContextNames.SVHDX_OPEN_DEVICE_CONTEXT;
                    var svhdxOpenDeviceContextStructV2 = new SVHDX_OPEN_DEVICE_CONTEXT_V2
                    {
                        Version = svhdxOpenDeviceContextV2.Version,
                        HasInitiatorId = svhdxOpenDeviceContextV2.HasInitiatorId,
                        Reserved = new byte[3],
                        InitiatorId = svhdxOpenDeviceContextV2.InitiatorId,
                        Flags = svhdxOpenDeviceContextV2.Flags,
                        OriginatorFlags = svhdxOpenDeviceContextV2.OriginatorFlags,
                        OpenRequestId = svhdxOpenDeviceContextV2.OpenRequestId,
                        InitiatorHostNameLength = svhdxOpenDeviceContextV2.InitiatorHostNameLength,
                        VirtualDiskPropertiesInitialized = svhdxOpenDeviceContextV2.VirtualDiskPropertiesInitialized,
                        ServerServiceVersion = svhdxOpenDeviceContextV2.ServerServiceVersion,
                        VirtualSectorSize = svhdxOpenDeviceContextV2.VirtualSectorSize,
                        PhysicalSectorSize = svhdxOpenDeviceContextV2.PhysicalSectorSize,
                        VirtualSize = svhdxOpenDeviceContextV2.VirtualSize
                    };

                    byte[] initiatorHostNameBinary = Encoding.Unicode.GetBytes(svhdxOpenDeviceContextV2.InitiatorHostName);
                    if (svhdxOpenDeviceContextV2.InitiatorHostName.Length < 126)
                    {
                        svhdxOpenDeviceContextStructV2.InitiatorHostName = new byte[126];
                        Buffer.BlockCopy(
                            initiatorHostNameBinary,
                            0,
                            svhdxOpenDeviceContextStructV2.InitiatorHostName,
                            0,
                            svhdxOpenDeviceContextV2.InitiatorHostNameLength);
                    }
                    else
                    {
                        svhdxOpenDeviceContextStructV2.InitiatorHostName = initiatorHostNameBinary;
                    }


                    dataBuffer = TypeMarshal.ToBytes(svhdxOpenDeviceContextStructV2);
                    goto encapsulateCreateContextStruct;
                }

            encapsulateCreateContextStruct:
                // Guid is transfered in binary format
                Guid guidName;
                var nameBuffer = Guid.TryParse(name, out guidName) ? guidName.ToByteArray() : Encoding.ASCII.GetBytes(name);

                var createContextStruct = new CREATE_CONTEXT();
                createContextStruct.Buffer = new byte[0];

                createContextStruct.NameOffset = 16;
                createContextStruct.NameLength = (ushort)nameBuffer.Length;
                createContextStruct.Buffer = createContextStruct.Buffer.Concat(nameBuffer).ToArray();

                if (dataBuffer != null && dataBuffer.Length > 0)
                {
                    Smb2Utility.Align8(ref createContextStruct.Buffer);
                    createContextStruct.DataOffset = (ushort)(16 + createContextStruct.Buffer.Length);
                    createContextStruct.DataLength = (uint)dataBuffer.Length;
                    createContextStruct.Buffer = createContextStruct.Buffer.Concat(dataBuffer).ToArray();
                }

                createContextStructs.Add(createContextStruct);
            }

            if (createContextStructs.Count > 0)
            {
                Smb2Utility.Align8(ref buffer);

                for (int i = 0; i < createContextStructs.Count - 1; i++)
                {
                    var createContextStruct = createContextStructs[i];
                    Align8(ref createContextStruct.Buffer);
                    createContextStruct.Next = (uint)(16 + createContextStruct.Buffer.Length);

                    buffer = buffer.Concat(TypeMarshal.ToBytes(createContextStruct)).ToArray();
                }

                buffer = buffer.Concat(TypeMarshal.ToBytes(createContextStructs[createContextStructs.Count - 1])).ToArray();
            }

            return buffer;
        }

        internal static Smb2CreateContextResponse[] UnmarshalCreateContextResponses(byte[] buffer)
        {
            List<Smb2CreateContextResponse> createContexts = new List<Smb2CreateContextResponse>();

            uint offset = 0;
            while (offset < buffer.Length)
            {
                CREATE_CONTEXT contextStruct = TypeMarshal.ToStruct<CREATE_CONTEXT>(buffer.Skip((int)offset).ToArray());
                string createContextName = Encoding.ASCII.GetString(contextStruct.Name);

                switch (createContextName)
                {
                    case CreateContextNames.SMB2_CREATE_DURABLE_HANDLE_REQUEST:
                        var createDurableHandleResponse = TypeMarshal.ToStruct<CREATE_DURABLE_HANDLE_RESPONSE>(contextStruct.Data);
                        createContexts.Add(new Smb2CreateDurableHandleResponse());
                        break;

                    case CreateContextNames.SMB2_CREATE_QUERY_MAXIMAL_ACCESS_REQUEST:
                        var createQueryMaximalAccessResponse = TypeMarshal.ToStruct<CREATE_QUERY_MAXIMAL_ACCESS_RESPONSE>(contextStruct.Data);
                        createContexts.Add(new Smb2CreateQueryMaximalAccessResponse()
                        {
                            MaximalAccess = createQueryMaximalAccessResponse.MaximalAccess,
                            QueryStatus = createQueryMaximalAccessResponse.QueryStatus
                        });
                        break;

                    case CreateContextNames.SMB2_CREATE_QUERY_ON_DISK_ID:
                        var createQueryOnDiskIdResponse = TypeMarshal.ToStruct<CREATE_QUERY_ON_DISK_ID_RESPONSE>(contextStruct.Data);
                        createContexts.Add(new Smb2CreateQueryOnDiskIdResponse()
                        {
                            DiskIdBuffer = new List<byte>(createQueryOnDiskIdResponse.DiskIDBuffer)
                        });
                        break;

                    case CreateContextNames.SMB2_CREATE_REQUEST_LEASE: // Same value with CreateContextNames.SMB2_CREATE_REQUEST_LEASE_V2
                        switch (contextStruct.DataLength)
                        {
                            case 32:
                                var createResponseLease = TypeMarshal.ToStruct<CREATE_RESPONSE_LEASE>(contextStruct.Data);
                                createContexts.Add(new Smb2CreateResponseLease()
                                {
                                    LeaseDuration = createResponseLease.LeaseDuration,
                                    LeaseFlags = createResponseLease.LeaseFlags,
                                    LeaseKey = createResponseLease.LeaseKey,
                                    LeaseState = createResponseLease.LeaseState
                                });
                                break;
                            case 52:
                                var createResponseLeaseV2 = TypeMarshal.ToStruct<CREATE_RESPONSE_LEASE_V2>(contextStruct.Data);
                                createContexts.Add(new Smb2CreateResponseLeaseV2()
                                {
                                    LeaseDuration = createResponseLeaseV2.LeaseDuration,
                                    LeaseKey = createResponseLeaseV2.LeaseKey,
                                    LeaseState = createResponseLeaseV2.LeaseState,
                                    Epoch = createResponseLeaseV2.Epoch,
                                    Reserved = createResponseLeaseV2.Reserved,
                                    Flags = createResponseLeaseV2.Flags,
                                    ParentLeaseKey = createResponseLeaseV2.ParentLeaseKey
                                });
                                break;
                            default:
                                throw new InvalidOperationException("Unexpected data size for SMB2_CREATE_REQUEST_LEASE " + contextStruct.DataLength);
                        }
                        break;

                    case CreateContextNames.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2:
                        var createDurableHandleResponseV2 = TypeMarshal.ToStruct<CREATE_DURABLE_HANDLE_RESPONSE_V2>(contextStruct.Data);
                        createContexts.Add(new Smb2CreateDurableHandleResponseV2()
                        {
                            Timeout = createDurableHandleResponseV2.Timeout,
                            Flags = createDurableHandleResponseV2.Flags
                        });
                        break;

                    default:
                        {
                            Guid createContextGuid = Guid.Empty;
                            if (contextStruct.NameLength == 16)
                            {
                                createContextGuid = new Guid(contextStruct.Name);
                            }

                            if (createContextGuid != Guid.Empty && createContextGuid.ToString() == CreateContextNames.SVHDX_OPEN_DEVICE_CONTEXT)
                            {
                                if (contextStruct.DataLength == 168) //The size of SVHDX_OPEN_DEVICE_CONTEXT structure is 168
                                {
                                    var createOpenDeviceContextResponse = TypeMarshal.ToStruct<SVHDX_OPEN_DEVICE_CONTEXT>(contextStruct.Data);
                                    createContexts.Add(new Smb2CreateSvhdxOpenDeviceContextResponse()
                                    {
                                        Version = createOpenDeviceContextResponse.Version,
                                        HasInitiatorId = createOpenDeviceContextResponse.HasInitiatorId,
                                        InitiatorId = createOpenDeviceContextResponse.InitiatorId,
                                        Flags = createOpenDeviceContextResponse.Flags,
                                        OriginatorFlags = createOpenDeviceContextResponse.OriginatorFlags,
                                        OpenRequestId = createOpenDeviceContextResponse.OpenRequestId,
                                        InitiatorHostNameLength = createOpenDeviceContextResponse.InitiatorHostNameLength,
                                        InitiatorHostName = Encoding.Unicode.GetString(createOpenDeviceContextResponse.InitiatorHostName)
                                    });
                                }
                                else
                                {
                                    var createOpenDeviceContextResponseV2 = TypeMarshal.ToStruct<SVHDX_OPEN_DEVICE_CONTEXT_V2>(contextStruct.Data);
                                    createContexts.Add(new Smb2CreateSvhdxOpenDeviceContextResponseV2()
                                    {
                                        Version = createOpenDeviceContextResponseV2.Version,
                                        HasInitiatorId = createOpenDeviceContextResponseV2.HasInitiatorId,
                                        InitiatorId = createOpenDeviceContextResponseV2.InitiatorId,
                                        Flags = createOpenDeviceContextResponseV2.Flags,
                                        OriginatorFlags = createOpenDeviceContextResponseV2.OriginatorFlags,
                                        OpenRequestId = createOpenDeviceContextResponseV2.OpenRequestId,
                                        InitiatorHostNameLength = createOpenDeviceContextResponseV2.InitiatorHostNameLength,
                                        InitiatorHostName = Encoding.Unicode.GetString(createOpenDeviceContextResponseV2.InitiatorHostName),
                                        VirtualDiskPropertiesInitialized = createOpenDeviceContextResponseV2.VirtualDiskPropertiesInitialized,
                                        ServerServiceVersion = createOpenDeviceContextResponseV2.ServerServiceVersion,
                                        VirtualSectorSize = createOpenDeviceContextResponseV2.VirtualSectorSize,
                                        PhysicalSectorSize = createOpenDeviceContextResponseV2.PhysicalSectorSize,
                                        VirtualSize = createOpenDeviceContextResponseV2.VirtualSize
                                    });
                                }
                                break;
                            }
                            else
                            { 
                                throw new InvalidOperationException("Unexpected create context: " + createContextName + ", the GUID format is: " + createContextGuid.ToString()); 
                            }
                        }
                }

                offset += contextStruct.Next;

                if (contextStruct.Next == 0)
                    break;
            }

            return createContexts.ToArray();

        }

        internal static FILE_NOTIFY_INFORMATION[] UnmarshalFileNotifyInformation(byte[] buffer)
        {
            List<FILE_NOTIFY_INFORMATION> listFileNotifyInformation = new List<FILE_NOTIFY_INFORMATION>();
            uint offset = 0;
            while (offset < buffer.Length)
            {
                FILE_NOTIFY_INFORMATION fileNotifyInformationStruct = TypeMarshal.ToStruct<FILE_NOTIFY_INFORMATION>(buffer.Skip((int)offset).ToArray());
                listFileNotifyInformation.Add(fileNotifyInformationStruct);

                if (fileNotifyInformationStruct.NextEntryOffset == 0)
                {
                    break; //If there are no subsequent structures, the NextEntryOffset field MUST be 0.
                }
                else
                {
                    offset += fileNotifyInformationStruct.NextEntryOffset;
                }
            }
            return listFileNotifyInformation.ToArray();
        }

        /// <summary>
        /// Test if the response packet is an interim response packet
        /// </summary>
        /// <param name="smb2Packet">The smb2 response packet</param>
        /// <returns>True if it is an interim packet, otherwise false.</returns>
        public static bool IsInterimResponsePacket(Smb2Packet smb2Packet)
        {
            Smb2SinglePacket singlePacket = smb2Packet as Smb2SinglePacket;

            if (singlePacket == null)
            {
                return false;
            }

            //if (!(singlePacket is Smb2ErrorResponsePacket))
            //{
            //    return false;
            //}

            if ((singlePacket.Header.Status == Smb2Status.STATUS_PENDING)
                    && ((singlePacket.Header.Flags & Packet_Header_Flags_Values.FLAGS_ASYNC_COMMAND)
                    == Packet_Header_Flags_Values.FLAGS_ASYNC_COMMAND))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// check the status whether is error status.<para/>
        /// if true, throw exception; otherwise, return its value.
        /// </summary>
        /// <param name="status">
        /// a uint value that specifies the value to check.
        /// </param>
        /// <param name="packet">
        /// a Smb2Packet object that specifies the packet.
        /// </param>
        /// <returns>
        /// if status indicates that the packet is not error, return status.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when server response error.
        /// </exception>
        internal static uint CheckStatus(uint status, Smb2Packet packet)
        {
            NtStatus ntStatus = (NtStatus)status;

            return status;
        }

        public static NETWORK_INTERFACE_INFO_Response[] UnmarshalNetworkInterfaceInfoResponse(byte[] buffer)
        {
            List<NETWORK_INTERFACE_INFO_Response> listNetworkInterfaceInfo = new List<NETWORK_INTERFACE_INFO_Response>();
            uint offset = 0;
            while (offset < buffer.Length)
            {
                NETWORK_INTERFACE_INFO_Response networkInterfaceInfoStruct = TypeMarshal.ToStruct<NETWORK_INTERFACE_INFO_Response>(buffer.Skip((int)offset).ToArray());

                listNetworkInterfaceInfo.Add(networkInterfaceInfoStruct);

                if (networkInterfaceInfoStruct.Next == 0)
                {
                    break; //This field MUST be set to zero if there are no subsequent network interfaces.
                }
                else
                {
                    offset += networkInterfaceInfoStruct.Next;
                }
            }

            return listNetworkInterfaceInfo.ToArray();
        }

        public static string GetUncPath(string server, string share, string file = null)
        {
            return string.Format(@"\\{0}\{1}{2}", server, share, !string.IsNullOrEmpty(file) ? "\\" + file : string.Empty);
        }

        public static string GetServerName(string uncPath)
        {
            string[] segments = uncPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            return segments[0];
        }

        public static string GetShareName(string uncPath)
        {
            string[] segments = uncPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Format(@"\\{0}\{1}", segments[0], segments[1]);
        }

        public static string GetFileName(string uncPath)
        {
            string[] segments = uncPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            for (int i = 2; i < segments.Length; i++)
            {
                sb.Append(segments[i]);
                sb.Append("\\");
            }
            sb.Length--;
            return sb.ToString();
        }

        public static string GetIPCPath(string server)
        {
            return string.Format(@"\\{0}\IPC$", server);
        }

        public static string CreateRandomString(int lenOfKb)
        {
            if (lenOfKb <= 0)
            {
                throw new InvalidOperationException("Specified length of random string is too small");
            }

            return CreateRandomStringInByte(lenOfKb * 1024);
        }

        public static string CreateRandomStringInByte(int lenOfByte)
        {
            if (lenOfByte <= 0)
            {
                throw new InvalidOperationException("Specified length of random string is too small");
            }

            if (lenOfByte > int.MaxValue)
            {
                throw new InvalidOperationException("Specified length of random string is too large");
            }

            StringBuilder sb = new StringBuilder(lenOfByte);
            char ch;
            Random random = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < lenOfByte; i++)
            {
                //Randomly pick up char from range 32~126 (space to ~)
                ch = Convert.ToChar(random.Next(32, 126));
                sb.Append(ch);
            }
            return sb.ToString();
        }

        public static byte[] CreateRandomByteArray(int lenOfByte)
        {
            return Encoding.ASCII.GetBytes(CreateRandomStringInByte(lenOfByte));
        }

        public static string GetCifsServicePrincipalName(string serverName)
        {
            try
            {
                // If the server name is an IP address. No need to query DNS.
                // The server may not support kerberos. Use NTLM instead.
                IPAddress address;
                if (IPAddress.TryParse(serverName, out address))
                {
                    return null;
                }

                // If both client and server are in domain, it will use DNS to get the FQDN.
                // Otherwise, if client is not in domain, but server is, LLMNR is used to get the hostname.
                IPHostEntry hostEntry = Dns.GetHostEntry(serverName);
                return "cifs/" + hostEntry.HostName;
            }
            catch
            {
                // For workgroup environment, it will use NTLM authentication method
                // and it does not need CifsServicePrincipalName.
                return null;
            }
        }

        //Carried from Infrastructure\src\ProtoSDK\Common\DtypUtility.cs
        public static DateTime ConvertToDateTimeUtc(_FILETIME fileTime)
        {
            long fileTimeLong = (((long)fileTime.dwHighDateTime) << 32) | fileTime.dwLowDateTime;
            if (fileTimeLong > DateTime.MaxValue.ToFileTimeUtc())
            {
                return DateTime.MaxValue.ToUniversalTime();
            }

            return DateTime.FromFileTimeUtc(fileTimeLong);
        }

        //Carried from Infrastructure\src\ProtoSDK\Common\DtypUtility.cs
        public static _FILETIME ConvertToFileTime(DateTime dateTime)
        {
            _FILETIME retVal;

            if (dateTime == DateTime.MaxValue.ToUniversalTime())
            {
                retVal.dwLowDateTime = 0xFFFFFFFFu;
                // According to TD section 2.5 KERB_VALIDATION_INFO,
                // "(If the session should not expire,)
                // (If the client should not be logged off,)
                // (If the password will not expire,)
                // this structure SHOULD have the
                // dwHighDateTime member set to 0x7FFFFFFF and the dwLowDateTime member
                // set to 0xFFFFFFFF."
                retVal.dwHighDateTime = 0x7FFFFFFFu;
            }
            else
            {
                long fileTime = dateTime.ToFileTimeUtc();
                retVal.dwLowDateTime = (uint)(fileTime & 0xFFFFFFFF);
                retVal.dwHighDateTime = (uint)((fileTime >> 32) & 0xFFFFFFFF);
            }

            return retVal;
        }

        /// <summary>
        /// Get principle name based on hostname
        /// </summary>
        /// <param name="hostName">The host name of server</param>
        /// <returns>The principle name of the server.</returns>
        public static string GetPrincipleName(string hostName)
        {
            //if host name has domain, remove it. 
            int pos = hostName.IndexOf('.');
            if (pos == -1)
            {
                return hostName;
            }
            else
            {
                return hostName.Substring(0, pos);
            }
        }

        /// <summary>
        /// Calculate credit charge based on given data length
        /// </summary>
        /// <param name="dataLengthInByte">Length of data</param>
        /// <param name="dialect">SMB2 dialect negotiated on the connection</param>
        /// <param name="supportMultiCredit">Set true if underlying connection support multicredit, otherwise set false</param>
        /// <returns>Number of credit charge</returns>
        public static ushort CalculateCreditCharge(uint dataLengthInByte, DialectRevision dialect, bool supportMultiCredit = true)
        {
            //If a client requests writing to a file, Connection.Dialect is not "2.002", and if Connection.SupportsMultiCredit is TRUE,
            //the CreditCharge field in the SMB2 header MUST be set to ( 1 + (Length ?1) / 65536 ).
            if (dialect != DialectRevision.Smb2002
                && supportMultiCredit)
            {
                return (ushort)(1 + ((dataLengthInByte - 1) / 65536));
            }
            else
            {
                //If the client implements the SMB 2.1 dialect or SMB 3.x dialect family and Connection.SupportsMultiCredit is FALSE,
                //CreditCharge SHOULD<93> be set to 0
                //Otherwise, the CreditCharge field MUST be set to 0
                return 0;
            }
        }

        /// <summary>
        /// Determine if a given dialect belongs to the SMB 3.x dialect family
        /// </summary>
        /// <param name="dialect">Dialect to be determined</param>
        /// <returns>Return true if given dialect belongs to the SMB 3.x dialect family, otherwise return false</returns>
        public static bool IsSmb3xFamily(DialectRevision dialect)
        {
            return dialect == DialectRevision.Smb30 || dialect == DialectRevision.Smb302 || dialect == DialectRevision.Smb311;
        }

        /// <summary>
        /// Determine if a given dialect belongs to the SMB 2 dialect family
        /// </summary>
        /// <param name="dialect">Dialect to be determined</param>
        /// <returns>Return true if given dialect belongs to the SMB 2 dialect family, otherwise return false</returns>
        public static bool IsSmb2Family(DialectRevision dialect)
        {
            return dialect == DialectRevision.Smb2002 || dialect == DialectRevision.Smb21;
        }

        /// <summary>
        /// Get string representation of a given array
        /// </summary>
        /// <typeparam name="T">Type of array items</typeparam>
        /// <param name="array">Array object</param>
        /// <returns>String representation of the given array</returns>
        public static string GetArrayString<T>(T[] array)
        {
            if (array == null || array.Length == 0)
            {
                return "{}";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            foreach (var item in array)
            {
                sb.Append(item.ToString() + ",");
            }

            sb.Length--;

            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// Get requested dialects according to max dialect client supports
        /// </summary>
        /// <param name="clientMaxDialectSupported">The max dialect client supports</param>
        /// <returns>The requested dialects</returns>
        public static DialectRevision[] GetDialects(DialectRevision clientMaxDialectSupported)
        {
            int index = allDialects.IndexOf(clientMaxDialectSupported);
            DialectRevision[] dialects = allDialects.GetRange(0, index + 1).ToArray();

            return dialects;
        }
    }
}
