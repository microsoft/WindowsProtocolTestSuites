// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    #region NDR Engine Internal Structures

    /// <summary>
    /// Delegation function to allocate memory, used by NDR engine.
    /// Copied from midl.exe generated stub code.
    /// </summary>
    /// <param name="s">Buffer size.</param>
    /// <returns>Pointer to allocated memory.</returns>
    internal delegate IntPtr PfnAllocate(uint s);

    /// <summary>
    /// Delegation function to free allocated memory , used by NDR engine.
    /// Copied from midl.exe generated stub code.
    /// </summary>
    /// <param name="f">Pointer to allocated memory.</param>
    internal delegate void PfnFree(IntPtr f);

    /// <summary>
    /// Copied from midl.exe generated stub code.
    /// </summary>
    internal struct MIDL_TYPE_PICKLING_INFO
    {
        public uint Version;
        public uint Flags;
        [MarshalAs(
            UnmanagedType.ByValArray,
            ArraySubType = UnmanagedType.U4,
            SizeConst = 3)]
        public uint[] Reserved;
    }

    /// <summary>
    /// Copied from midl.exe generated stub code.
    /// </summary>
    internal struct RPC_VERSION
    {
        public ushort MajorVersion;
        public ushort MinorVersion;
    }

    /// <summary>
    /// Copied from midl.exe generated stub code.
    /// </summary>
    internal struct RPC_SYNTAX_IDENTIFIER
    {
        public Guid SyntaxGUID;
        public RPC_VERSION SyntaxVersion;
    }

    /// <summary>
    /// Copied from midl.exe generated stub code.
    /// </summary>
    internal struct RPC_CLIENT_INTERFACE
    {
        public uint Length;
        public RPC_SYNTAX_IDENTIFIER InterfaceId;
        public RPC_SYNTAX_IDENTIFIER TransferSyntax;
        public IntPtr DispatchTable;
        public uint RpcProtseqEndpointCount;
        public IntPtr RpcProtseqEndpoint;
        public uint Reserved;
        public IntPtr InterpreterInfo;
        public uint Flags;
    }

    /// <summary>
    /// Copied from midl.exe generated stub code.
    /// </summary>
    // suppress message "warning CA1049 : Implement IDisposable on 
    // 'Microsoft.Protocols.TestTools.StackSdk.MIDL_STUB_DESC'." because
    // this is a NDR engine defined structure.
    // PickleMarshaller will release unmanaged resources.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
    internal struct MIDL_STUB_DESC
    {
        public IntPtr RpcInterfaceInformation;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public PfnAllocate pfnAllocate;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public PfnFree pfnFree;

        public IntPtr pHandle;

        public IntPtr apfnNdrRundownRoutines;
        public IntPtr aGenericBindingRoutinePairs;
        public IntPtr apfnExprEval;
        public IntPtr aXmitQuintuple;

        public IntPtr pFormatTypes;

        public int fCheckBounds;
        public uint Version;
        public IntPtr pMallocFreeStruct;
        public int MIDLVersion;
        public IntPtr CommFaultOffsets;
        public IntPtr aUserMarshalQuadruple;
        public IntPtr NotifyRoutineTable;
        public uint mFlags;
        public IntPtr CsRoutineTables;
        public IntPtr ProxyServerInfo;
        public IntPtr pExprInfo;
    }
    #endregion

    /// <summary>
    /// Convert unmanaged memory pointed by the IntPtr pointer
    /// to structure.
    /// The structure must be defined as struct, not class.
    /// If the structure contains no IntPtr members,
    /// the implementation can simply call Marshal.PicklePtrToStructure(pointer, type);
    /// </summary>
    /// <param name="pointer">The pointer to unmanaged memory.</param>
    /// <returns>Structure converted from unmanaged memory.</returns>
    public delegate object PicklePtrToStructure(IntPtr pointer);

    /// <summary>
    /// <para>Provides methods for encoding unmanaged object to NDR-encoded data,
    /// and decoding NDR-encoded data to unmanaged object.</para>
    /// 
    /// <para>Basic knowledge for NDR encoding/decoding please refer to:</para>
    /// <para>http://msdn.microsoft.com/en-us/library/ms764233(VS.85).aspx</para>
    /// <para>http://msdn.microsoft.com/en-us/library/aa378670(VS.85).aspx</para>
    /// 
    /// <para>To encode/decode your data type using this class correctly, 
    /// you need do following things:</para>
    /// 
    /// <para>1. Make sure your data type definition conforms to NDR standard:
    /// http://www.opengroup.org/onlinepubs/9629399/chap14.htm</para>
    /// 
    /// <para>2. Write IDL contains your data type, make sure the topmost data type
    /// has definition of pointer, for example:
    /// typedef struct ... MyType, *PMyType;</para>
    /// 
    /// <para>3. Write ACF using the [encode] and [decode] attributes as interface attributes
    /// for the topmost data type pointer, for example:
    /// typedef [encode, decode] PMyType;</para>
    /// 
    /// <para>4. Compile the IDL and ACF using midl.exe to generate stub code.
    /// More details please install Microsoft Platform SDK, 
    /// and refer to files under Samples\NetDS\RPC\Pickle\picklt.</para>
    /// 
    /// <para>5. Open the generated stub code with postfix "_c.c", locate at MIDL_TypeFormatString.
    /// It's a static const byte array. Copy this byte array to your C# code as the type format string.
    /// The type format string is the only parameter to construct this class.
    /// Note: You need to apply NdrFcShort and NdrFcLong macros manually or by a regex tool.
    /// For example, NdrFcShort(0x1234) results {0x34, 0x12} and NdrFcLong(0x1234) results {0x34, 0x12, 0x0, 0x0}.</para>
    /// 
    /// <para>6. Open the generated stub code with postfix "_c.c", locate at function with postfix "_Encode" or "_Decode".</para>
    /// <para>It must looks like:                                                                                         </para>                               
    /// <para>PMyType_Encode(PMyType * _pType)                                                                            </para>  
    /// <para>    {                                                                                                       </para>          
    /// <para>          NdrMesTypeEncode2(                                                                                </para>  
    /// <para>                ...                                                                                         </para>  
    /// <para>                ( PFORMAT_STRING  )&amp;PrimitiveTypes__MIDL_TypeFormatString.Format[2],                    </para>  
    /// <para>                _pType);                                                                                    </para>     
    /// <para>    }                                                                                                       </para>  
    /// <para>The array index number in MIDL_TypeFormatString.Format[] is the format string offset.
    /// In your C# code, the format string offset is one of the parameters to call this class'
    /// Encode/Decode methods.</para>
    /// 
    /// <para>7. Define your data type in C# code. Be careful of the type mapping from C to C#,
    /// for example, C long is C# int (4 bytes), not C# long (8 bytes). Use MarshalAs attribute when appropriate.</para>
    /// 
    /// <para>8. Call this class using defined data type, type format string and offset to accomplish the 
    /// encoding/decoding tasks.</para>
    /// 
    /// <para>You can define multiple data types in IDL/ACF file according to above steps.
    /// They share the same type format string, each has its own format string offset.</para>
    /// </summary>
    public sealed class PickleMarshaller : IDisposable
    {
        #region variable

        /// <summary>
        /// Indicates the instance has been disposed or not.
        /// </summary>
        private bool disposed;

        private MIDL_TYPE_PICKLING_INFO __MIDL_TypePicklingInfo;
        private MIDL_STUB_DESC stubDesc;
        private byte[] typeFormatString;

        #endregion

        /// <summary>
        /// Initializes an instance of PickleMarshaller class from a type format string.
        /// </summary>
        /// <param name="midlTypeFormatString">The type format string used to initialize the object.
        /// More details about format string and how to generate it please refer to current class' summary.</param>
        public PickleMarshaller(byte[] midlTypeFormatString)
        {
            if (midlTypeFormatString == null)
            {
                throw new ArgumentNullException("midlTypeFormatString");
            }

            typeFormatString = new byte[midlTypeFormatString.Length];
            Buffer.BlockCopy(midlTypeFormatString, 0, typeFormatString, 0, midlTypeFormatString.Length);

            #region MIDL_TYPE_PICKLING_INFO __MIDL_TypePicklingInfo
            // Following values are assigned according to midl.exe generated rpc stub code:
            //    static MIDL_TYPE_PICKLING_INFO __MIDL_TypePicklingInfo =
            //    {
            //    0x33205054, /* Signature & version: TP 1 */
            //    0x3, /* Flags: Oicf NewCorrDesc */
            //    0,
            //    0,
            //    0,
            //    };
            // See summary of current class for details on how to generate rpc stub code.
            __MIDL_TypePicklingInfo = new MIDL_TYPE_PICKLING_INFO();
            __MIDL_TypePicklingInfo.Version = 0x33205054; /* Signature & version: TP 1 */
            __MIDL_TypePicklingInfo.Flags = 0x3; /* Flags: Oicf NewCorrDesc */
            __MIDL_TypePicklingInfo.Reserved = new uint[] { 0, 0, 0 };
            #endregion

            #region RPC_CLIENT_INTERFACE rpcClientInterface
            // Following values are assigned according to midl.exe generated rpc stub code:
            //        static const RPC_CLIENT_INTERFACE type_pickle___RpcClientInterface =
            //        {
            //        sizeof(RPC_CLIENT_INTERFACE),
            //        {{0x906B0CE0,0xC70B,0x1067,{0xB3,0x17,0x00,0xDD,0x01,0x06,0x62,0xDA}},{1,0}},
            //        {{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}},
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0x00000000
            //        };
            // See summary of current class for details on how to generate rpc stub code.
            RPC_CLIENT_INTERFACE rpcClientInterface = new RPC_CLIENT_INTERFACE();
            rpcClientInterface.Length = (uint)Marshal.SizeOf(typeof(RPC_CLIENT_INTERFACE));
            rpcClientInterface.InterfaceId = new RPC_SYNTAX_IDENTIFIER();
            rpcClientInterface.InterfaceId.SyntaxGUID = new Guid(0x906B0CE0, 0xC70B, 0x1067, 0xB3, 0x17, 0x00, 0xDD, 0x01, 0x06, 0x62, 0xDA);
            rpcClientInterface.InterfaceId.SyntaxVersion = new RPC_VERSION();
            rpcClientInterface.InterfaceId.SyntaxVersion.MajorVersion = 1;
            rpcClientInterface.InterfaceId.SyntaxVersion.MinorVersion = 0;
            rpcClientInterface.TransferSyntax = new RPC_SYNTAX_IDENTIFIER();
            rpcClientInterface.TransferSyntax.SyntaxGUID = new Guid(0x8A885D04, 0x1CEB, 0x11C9, 0x9F, 0xE8, 0x08, 0x00, 0x2B, 0x10, 0x48, 0x60);
            rpcClientInterface.TransferSyntax.SyntaxVersion = new RPC_VERSION();
            rpcClientInterface.TransferSyntax.SyntaxVersion.MajorVersion = 2;
            rpcClientInterface.TransferSyntax.SyntaxVersion.MinorVersion = 0;
            rpcClientInterface.DispatchTable = IntPtr.Zero;
            rpcClientInterface.RpcProtseqEndpointCount = 0;
            rpcClientInterface.RpcProtseqEndpoint = IntPtr.Zero;
            rpcClientInterface.Reserved = 0;
            rpcClientInterface.InterpreterInfo = IntPtr.Zero;
            rpcClientInterface.Flags = 0x00000000;
            #endregion

            #region MIDL_STUB_DESC stubDesc
            // Following values are assigned according to midl.exe generated rpc stub code:
            //    static const MIDL_STUB_DESC type_pickle_StubDesc = 
            //    {
            //    (void *)& type_pickle___RpcClientInterface,
            //    MIDL_user_allocate,
            //    MIDL_user_free,
            //    &ImplicitPicHandle,
            //    0,
            //    0,
            //    0,
            //    0,
            //    Picklt__MIDL_TypeFormatString.Format,
            //    1, /* -error bounds_check flag */
            //    0x50004, /* Ndr library version */
            //    0,
            //    0x70001f4, /* MIDL Version 7.0.500 */
            //    0,
            //    0,
            //    0,  /* notify & notify_flag routine table */
            //    0x1, /* MIDL flag */
            //    0, /* cs routines */
            //    0,   /* proxy/server info */
            //    0
            //    };
            // See summary of current class for details on how to generate rpc stub code.
            stubDesc = new MIDL_STUB_DESC();
            stubDesc.RpcInterfaceInformation = Marshal.AllocHGlobal(Marshal.SizeOf(rpcClientInterface));
            Marshal.StructureToPtr(rpcClientInterface, stubDesc.RpcInterfaceInformation, false);
            stubDesc.pfnAllocate = new PfnAllocate(MIDL_user_allocate);
            stubDesc.pfnFree = new PfnFree(MIDL_user_free);
            stubDesc.apfnNdrRundownRoutines = IntPtr.Zero;
            stubDesc.aGenericBindingRoutinePairs = IntPtr.Zero;
            stubDesc.apfnExprEval = IntPtr.Zero;
            stubDesc.aXmitQuintuple = IntPtr.Zero;
            stubDesc.pFormatTypes = Marshal.AllocHGlobal(midlTypeFormatString.Length);
            Marshal.Copy(typeFormatString, 0, stubDesc.pFormatTypes, typeFormatString.Length);
            stubDesc.fCheckBounds = 1; /* -error bounds_check flag */
            stubDesc.Version = 0x50004; /* Ndr library version */
            stubDesc.pMallocFreeStruct = IntPtr.Zero;
            stubDesc.MIDLVersion = 0x600016e; /* MIDL Version 6.0.366 */
            stubDesc.CommFaultOffsets = IntPtr.Zero;
            stubDesc.aUserMarshalQuadruple = IntPtr.Zero;
            stubDesc.NotifyRoutineTable = IntPtr.Zero;  /* notify & notify_flag routine table */
            stubDesc.mFlags = 0x1; /* MIDL flag */
            stubDesc.CsRoutineTables = IntPtr.Zero; /* cs routines */
            stubDesc.ProxyServerInfo = IntPtr.Zero; /* proxy/server native */
            stubDesc.pExprInfo = IntPtr.Zero; /* Reserved5 */
            #endregion
        }


        /// <summary>
        /// Decodes a block of NDR-encoded data to a structure.
        /// </summary>
        /// <typeparam name="T">Type of the decoded object.</typeparam>
        /// <param name="buffer">The byte array containing NDR-encoded data.</param>
        /// <param name="formatStringOffset">Format string offset of the unmanaged object data type.</param>
        /// <returns>The decoded structure.</returns>
        /// <exception cref="PickleException">Internal NDR function failed.</exception>
        /// <exception cref="ArgumentNullException">Thrown when buffer is null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public T Decode<T>(byte[] buffer, int formatStringOffset, bool force32Bit, int align) where T : struct
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            return Decode<T>(buffer, 0, buffer.Length, formatStringOffset, force32Bit, align);
        }


        /// <summary>
        /// Decodes a block of NDR-encoded data to a structure.
        /// </summary>
        /// <typeparam name="T">Type of the decoded object.</typeparam>
        /// <param name="buffer">The byte array containing NDR-encoded data.</param>
        /// <param name="index">Beginning of the NDR-encoded data in the buffer.</param>
        /// <param name="count">Length of the NDR-encoded data.</param>
        /// <param name="formatStringOffset">Format string offset of the unmanaged object data type.</param>
        /// <returns>The decoded structure.</returns>
        /// <exception cref="PickleException">Internal NDR function failed.</exception>
        /// <exception cref="ArgumentNullException">Thrown when buffer is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index or count is out of range.</exception>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public T Decode<T>(byte[] buffer, int index, int count, int formatStringOffset, bool force32Bit, int alignment) where T : struct
        {
            PicklePtrToStructure converter = delegate (IntPtr ptr)
            {
                return TypeMarshal.ToStruct<T>(ptr, force32Bit, alignment);
            };

            return (T)Decode(converter, buffer, index, count, formatStringOffset);
        }


        /// <summary>
        /// Decodes a block of NDR-encoded data to a structure. If the <paramref name="converter"/> simply
        /// calls Marshal.PtrToStructure(IntPtr ptr, Type structureType) internally, use another overload method
        /// Decode(Type type, byte[] buffer, int index, int count, int formatStringOffset) instead.
        /// </summary>
        /// <param name="converter">Delegation to convert unmanaged pointer into object.</param>
        /// <param name="buffer">The byte array containing NDR-encoded data.</param>
        /// <param name="index">Beginning of the NDR-encoded data in the buffer.</param>
        /// <param name="count">Length of the NDR-encoded data.</param>
        /// <param name="formatStringOffset">Format string offset of the unmanaged object data type.</param>
        /// <returns>The decoded structure.</returns>
        /// <exception cref="PickleException">Thrown when internal NDR function failed.</exception>
        /// <exception cref="ArgumentNullException">Thrown when converter or buffer is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index or count is out of range.</exception>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults")]
        public object Decode(PicklePtrToStructure converter, byte[] buffer, int index, int count, int formatStringOffset)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (index < 0 || index >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException("index", "index is out of range");
            }
            if (count < 0 || (index + count) > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("count", "count is out of range");
            }


            IntPtr ndrHandle = IntPtr.Zero;
            IntPtr pObj = IntPtr.Zero;
            IntPtr pBuf = IntPtr.Zero;
            IntPtr format = IntPtr.Zero;

            try
            {
                #region init handle and environment

                pBuf = Marshal.AllocHGlobal(count);
                Marshal.Copy(buffer, index, pBuf, count);

                int rt = PickleNativeMethods.MesDecodeBufferHandleCreate(pBuf, (uint)count, out ndrHandle);
                if (rt != PickleError.RPC_S_OK)
                {
                    throw new PickleException(rt, "Failed to create handle on given buffer.");
                }

                if (stubDesc.pHandle != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(stubDesc.pHandle);
                    stubDesc.pHandle = IntPtr.Zero;
                }
                stubDesc.pHandle = Marshal.AllocHGlobal(Marshal.SizeOf(ndrHandle));
                Marshal.WriteIntPtr(stubDesc.pHandle, ndrHandle);

                #endregion

                format = Marshal.AllocHGlobal(typeFormatString.Length);

                Marshal.Copy(typeFormatString, 0, format, typeFormatString.Length);

                PickleNativeMethods.NdrMesTypeDecode2(
                     ndrHandle,
                     ref __MIDL_TypePicklingInfo,
                     ref stubDesc,
                     format + formatStringOffset,
                     ref pObj);

                if (pObj == IntPtr.Zero)
                {
                    throw new PickleException("Failed to decode on given buffer and formatStringOffset.");
                }
                return converter(pObj);
            }
            finally
            {
                if (ndrHandle != IntPtr.Zero)
                {
                    PickleNativeMethods.MesHandleFree(ndrHandle);
                    //Ignore error
                }
                if (stubDesc.pHandle != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(stubDesc.pHandle);
                    stubDesc.pHandle = IntPtr.Zero;
                }
                if (pObj != IntPtr.Zero)
                {
                    MIDL_user_free(pObj);
                    pObj = IntPtr.Zero;
                }
                if (pBuf != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pBuf);
                    pBuf = IntPtr.Zero;
                }

                if (format != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(format);
                }
            }
        }


        /// <summary>
        /// Encodes an unmanaged structure to NDR-encoded data.
        /// </summary>
        /// <param name="pObj">Pointer to an unmanaged structure.</param>
        /// <param name="formatStringOffset">Format string offset of the unmanaged structure data type.</param>
        /// <returns>The byte array containing NDR-encoded data.</returns>
        /// <exception cref="PickleException">Input structure and offset are not NDR-standard.</exception>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults")]
        public byte[] Encode(IntPtr pObj, int formatStringOffset)
        {
            IntPtr buf = IntPtr.Zero;
            uint encodedSize = 0;
            IntPtr ndrHandle = IntPtr.Zero;
            try
            {
                #region init handle and environment

                int rt = PickleNativeMethods.MesEncodeDynBufferHandleCreate(out buf, out encodedSize, out ndrHandle);
                if (rt != PickleError.RPC_S_OK)
                {
                    throw new PickleException(rt, "Failed to encode on given structure and formatStringOffset.");
                }

                if (stubDesc.pHandle != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(stubDesc.pHandle);
                    stubDesc.pHandle = IntPtr.Zero;
                }
                stubDesc.pHandle = Marshal.AllocHGlobal(Marshal.SizeOf(ndrHandle));
                Marshal.WriteIntPtr(stubDesc.pHandle, ndrHandle);

                #endregion

                byte[] format = new byte[typeFormatString.Length - formatStringOffset];
                Buffer.BlockCopy(typeFormatString, formatStringOffset, format, 0, format.Length);

                PickleNativeMethods.NdrMesTypeEncode2(
                     ndrHandle,
                     ref __MIDL_TypePicklingInfo,
                     ref stubDesc,
                     format,
                     ref pObj);

                byte[] managedBuf = new byte[encodedSize];
                Marshal.Copy(buf, managedBuf, 0, (int)encodedSize);

                return managedBuf;
            }
            finally
            {
                if (ndrHandle != IntPtr.Zero)
                {
                    PickleNativeMethods.MesHandleFree(ndrHandle);
                    //Ignore error
                }
                if (stubDesc.pHandle != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(stubDesc.pHandle);
                    stubDesc.pHandle = IntPtr.Zero;
                }
            }
        }

        #region private methods

        /// <summary>
        /// Callback function to allocate memory, used by NDR engine.
        /// Copied from midl.exe generated stub code.
        /// </summary>
        /// <param name="s">Buffer size.</param>
        /// <returns>Pointer to allocated memory.</returns>
        private static IntPtr MIDL_user_allocate(uint s)
        {
            IntPtr pcAllocated = Marshal.AllocHGlobal((int)s + 15);
            IntPtr pcUserPtr;

            // NDR engine will pad buffer according to NDR standard,
            // and leaves the padding as uninitialized.
            // So, to get consistency across test runs,
            // we must manually fill the allocated buffer with 0.
            for (int i = 0; i < (int)s + 15; i++)
            {
                Marshal.WriteByte(IntPtrUtility.Add(pcAllocated, i), 0x0);
            }

            // align to 8
            pcUserPtr = IntPtrUtility.Align(pcAllocated, 8);
            if (pcUserPtr == pcAllocated)
            {
                pcUserPtr = IntPtrUtility.Add(pcAllocated, 8);
            }

            // record the offset
            byte offset = (byte)IntPtrUtility.CalculateOffset(pcUserPtr, pcAllocated);
            Marshal.WriteByte(IntPtrUtility.Add(pcUserPtr, -1), offset);

            return (pcUserPtr);
        }


        /// <summary>
        /// Callback function to free allocated memory , used by NDR engine.
        /// Copied from midl.exe generated stub code.
        /// </summary>
        /// <param name="f">Pointer to allocated memory.</param>
        private static void MIDL_user_free(IntPtr f)
        {
            byte offset = Marshal.ReadByte(IntPtrUtility.Add(f, -1));
            IntPtr pcAllocated = IntPtrUtility.Add(f, -offset);

            Marshal.FreeHGlobal(pcAllocated);
        }

        #endregion


        #region IDisposable Members

        /// <summary>
        /// Implement IDisposable.
        /// Do not make this method virtual.
        /// A derived class should not be able to override this method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                }

                // safely release RpcInterfaceInformation
                if (stubDesc.RpcInterfaceInformation != IntPtr.Zero)
                {
                    // Call the appropriate methods to clean up
                    // unmanaged resources here.
                    // If disposing is false,
                    // only the following code is executed.
                    Marshal.FreeHGlobal(stubDesc.RpcInterfaceInformation);
                    stubDesc.RpcInterfaceInformation = IntPtr.Zero;
                }

                // safely release pFormatTypes
                if (stubDesc.pFormatTypes != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(stubDesc.pFormatTypes);
                    stubDesc.pFormatTypes = IntPtr.Zero;
                }

                // Note disposing has been done.
                disposed = true;
            }
        }


        /// <summary>
        /// Use C# destructor syntax for finalization code.
        /// This destructor will run only if the Dispose method
        /// does not get called.
        /// It gives your base class the opportunity to finalize.
        /// Do not provide destructors in class derived from this class.
        /// </summary>
        ~PickleMarshaller()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        #endregion
    }
}
