// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.RdmaLinux
{
    /// <summary>
    /// RDMA status codes for Linux implementation (matching C++ enum)
    /// </summary>
    public enum RdmaLinuxStatus : int
    {
        SUCCESS = 0,
        ERROR_GENERAL = -1,
        ERROR_TIMEOUT = -2,
        ERROR_INVALID_ARGUMENT = -3,
        ERROR_CONNECTION_CLOSED = -4,
        ERROR_NO_COMPLETION = -5,
        ERROR_RESOURCE = -6,
        ERROR_BUSY = -7,
    }

    /// <summary>
    /// RDMA operation flags for memory registration (matching C++ enum)
    /// </summary>
    [Flags]
    public enum RdmaLinuxAccessFlags : uint
    {
        LOCAL_WRITE = 1,
        REMOTE_WRITE = 2,
        REMOTE_READ = 4
    }

    public enum RdmaLinuxOPCode
    {
        Recv = 0,
        Other = 1
    }

    /// <summary>
    /// QP (Queue Pair) states
    /// </summary>
    public enum QpState
    {
        IBV_QPS_RESET = 0,
        IBV_QPS_INIT = 1,
        IBV_QPS_RTR = 2,  // Ready to Receive
        IBV_QPS_RTS = 3,  // Ready to Send
        IBV_QPS_SQD = 4,  // Send Queue Drained
        IBV_QPS_SQE = 5,  // Send Queue Error
        IBV_QPS_ERR = 6,   // Error
        IBV_QPS_UNKNOWN = -1
    }

    /// <summary>
    /// RDMA buffer descriptor for Linux implementation
    /// </summary>
    public struct RdmaLinuxBufferDescriptor
    {
        public ulong Address;
        public uint Rkey;
        public uint Length;
    }

    /// <summary>
    /// RDMA completion structure (matching C++ struct)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RdmaCompletion
    {
        public ulong wr_id;
        public uint status;
        public uint byte_len;
        public uint qp_num;
        public uint op_code;
        public uint vendor_err;
    }

    /// <summary>
    /// Native P/Invoke declarations for the C++ RDMA wrapper library
    /// </summary>
    internal static class RdmaNative
    {
        private const string NativeLibrary = "libRdmaLinuxAdapter.so";

        [DllImport(NativeLibrary, EntryPoint = "rdma_connect_client", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus rdma_connect_client(string host, string port, out IntPtr out_handle);

        [DllImport(NativeLibrary, EntryPoint = "disconnect", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus disconnect(IntPtr handle);

        [DllImport(NativeLibrary, EntryPoint = "rdma_send", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus rdma_send(IntPtr handle, byte[] data, IntPtr len);

        [DllImport(NativeLibrary, EntryPoint = "rdma_send", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus rdma_send(IntPtr handle, IntPtr data, IntPtr len);

        [DllImport(NativeLibrary, EntryPoint = "post_receive", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus post_receive(IntPtr handle, IntPtr buf, IntPtr len, ulong recv_slot_id);

        [DllImport(NativeLibrary, EntryPoint = "poll_completion", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus poll_completion(IntPtr handle, out RdmaCompletion completion, int timeout_ms, int completion_type);

        [DllImport(NativeLibrary, EntryPoint = "register_memory_window", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus register_memory_window(
            IntPtr handle,
            IntPtr buf,
            IntPtr len,
            uint access_flags,
            out IntPtr out_mw_handle,
            out uint out_rkey);

        [DllImport(NativeLibrary, EntryPoint = "deregister_memory_window", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus deregister_memory_window(IntPtr handle, IntPtr mw_handle);

        [DllImport(NativeLibrary, EntryPoint = "write", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus write(
            IntPtr handle,
            IntPtr local_buf,
            IntPtr len,
            ulong remote_addr,
            uint rkey);

        [DllImport(NativeLibrary, EntryPoint = "read", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus read(
            IntPtr handle,
            IntPtr local_buf,
            IntPtr len,
            ulong remote_addr,
            uint rkey);

        [DllImport(NativeLibrary, EntryPoint = "post_write", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus post_write(
            IntPtr handle,
            IntPtr local_buf,
            IntPtr len,
            ulong remote_addr,
            uint rkey,
            ulong wr_id);

        [DllImport(NativeLibrary, EntryPoint = "post_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus post_read(
            IntPtr handle,
            IntPtr local_buf,
            IntPtr len,
            ulong remote_addr,
            uint rkey,
            ulong wr_id);

        [DllImport(NativeLibrary, EntryPoint = "wait_for_disconnect", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus wait_for_disconnect(IntPtr handle, int timeout_seconds);

        [DllImport(NativeLibrary, EntryPoint = "invalidate_memory_window", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus invalidate_memory_window(IntPtr handle, long mwHandle, out ulong out_result_id);

        [DllImport(NativeLibrary, EntryPoint = "check_invalidate_completion", CallingConvention = CallingConvention.Cdecl)]
        public static extern RdmaLinuxStatus check_invalidate_completion(IntPtr handle, ulong result_id, int timeout_ms);

        [DllImport(NativeLibrary, EntryPoint = "rdma_alloc_aligned", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr rdma_alloc_aligned(IntPtr size);

        [DllImport(NativeLibrary, EntryPoint = "rdma_free_aligned", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rdma_free_aligned(IntPtr ptr);
    }

    class RegisteredMr
    {
        public IntPtr MwHandle;     // Native MW handle
        public IntPtr BufferPtr;    // Page-aligned native buffer address
        public bool IsDeregistered; // Life cycle status
        public int Length;
        public uint Token;
    }

    /// <summary>
    /// Linux RDMA adapter implementation using the native C++ RDMA wrapper
    /// </summary>
    public class RdmaLinuxAdapter : IDisposable
    {
        private IntPtr clientHandle;
        private volatile bool isConnected;
        private readonly object connectionLock = new object();

        // Track registered memory regions to prevent premature deallocation
        private readonly Dictionary<long, RegisteredMr> registeredBuffers = new Dictionary<long, RegisteredMr>();
        private readonly Dictionary<uint, long> tokenToHandleMap = new Dictionary<uint, long>();
        private readonly object mrLock = new object();

        // Map wr_id (Slot ID) to mrHandle.
        private readonly Dictionary<ulong, long> pendingReceiveHandles = new Dictionary<ulong, long>();
        private readonly object recvLock = new object();

        // Tracks active native calls to coordinate safe disconnect
        private int activeNativeCalls = 0;

        /// <summary>
        /// Gets whether the adapter is connected
        /// </summary>
        public bool IsConnected => isConnected;

        /// <summary>
        /// Gets the client handle for direct native calls
        /// </summary>
        /// <returns>Client handle</returns>
        public IntPtr GetClientHandle()
        {
            return clientHandle;
        }

        /// <summary>
        /// Initializes a new instance of the RdmaLinuxAdapter class
        /// </summary>
        public RdmaLinuxAdapter()
        {
            // Native library will be loaded when first operation is performed
        }

        private void EnterNativeCall()
        {
            Interlocked.Increment(ref activeNativeCalls);
        }

        private void ExitNativeCall()
        {
            Interlocked.Decrement(ref activeNativeCalls);
        }

        /// <summary>
        /// Connects to a remote RDMA server
        /// </summary>
        /// <param name="serverIp">Server IP address</param>
        /// <param name="port">Server port</param>
        /// <returns>Connection status</returns>
        public RdmaLinuxStatus Connect(string serverIp, ushort port)
        {
            lock (connectionLock)
            {
                if (isConnected)
                    return RdmaLinuxStatus.ERROR_GENERAL;

                try
                {
                    EnterNativeCall();
                    try
                    {
                        var status = RdmaNative.rdma_connect_client(serverIp, port.ToString(), out clientHandle);
                        if (status == RdmaLinuxStatus.SUCCESS)
                        {
                            isConnected = true;
                        }
                        return status;
                    }
                    finally
                    {
                        ExitNativeCall();
                    }
                }
                catch (Exception ex)
                {
                    LogDebug($"RDMA connection failed: {ex.Message}");
                    return RdmaLinuxStatus.ERROR_GENERAL;
                }
            }
        }

        /// <summary>
        /// Disconnects from the remote server
        /// </summary>
        public void Disconnect()
        {
            IntPtr handleToDisconnect = IntPtr.Zero;
            lock (connectionLock)
            {
                LogDebug($"Attempting Disconnect. isConnected: {isConnected}, Handle: {clientHandle}");

                if (!isConnected || clientHandle == IntPtr.Zero)
                {
                    isConnected = false;
                    clientHandle = IntPtr.Zero;
                    return;
                }

                isConnected = false;
                handleToDisconnect = clientHandle;
                clientHandle = IntPtr.Zero;
            }

            // Wait for in-flight native calls to complete before destroying the native client
            var waitStart = DateTime.UtcNow;
            while (Interlocked.CompareExchange(ref activeNativeCalls, 0, 0) > 0)
            {
                Thread.Sleep(10);
                if ((DateTime.UtcNow - waitStart).TotalSeconds > 10)
                {
                    LogDebug("Disconnect: Timeout waiting for active native calls to complete");
                    break;
                }
            }

            // Clean up pending receives
            List<long> pendingMrHandles;
            lock (recvLock)
            {
                pendingMrHandles = pendingReceiveHandles.Values.ToList();
                pendingReceiveHandles.Clear();
            }

            foreach (var mrHandle in pendingMrHandles)
            {
                try
                {
                    DeregisterMemory(mrHandle);
                }
                catch (Exception ex)
                {
                    LogDebug($"Failed to deregister pending receive: {ex.Message}");
                }
            }

            try
            {
                if (handleToDisconnect != IntPtr.Zero)
                {
                    EnterNativeCall();
                    try
                    {
                        RdmaNative.disconnect(handleToDisconnect);
                    }
                    finally
                    {
                        ExitNativeCall();
                    }
                }
            }
            catch (Exception ex)
            {
                LogDebug($"RDMA disconnect exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends data over RDMA connection
        /// </summary>
        /// <param name="data">Data to send</param>
        /// <returns>Send status</returns>
        public RdmaLinuxStatus Send(byte[] data)
        {
            if (data == null)
                return RdmaLinuxStatus.ERROR_INVALID_ARGUMENT;

            IntPtr handle;
            lock (connectionLock)
            {
                if (!isConnected || clientHandle == IntPtr.Zero)
                {
                    return RdmaLinuxStatus.ERROR_CONNECTION_CLOSED;
                }
                handle = clientHandle;
            }

            long mrHandle = 0;
            try
            {
                uint rkey;
                ulong address;

                var regStatus = RegisterMemory(data,
                    (uint)(RdmaLinuxAccessFlags.LOCAL_WRITE | RdmaLinuxAccessFlags.REMOTE_WRITE | RdmaLinuxAccessFlags.REMOTE_READ),
                    out mrHandle,
                    out rkey,
                    out address);

                if (regStatus != RdmaLinuxStatus.SUCCESS)
                {
                    LogDebug($"[Send] ERROR: Memory registration failed with status {regStatus}");
                    return regStatus;
                }

                EnterNativeCall();
                RdmaLinuxStatus sendStatus;
                try
                {
                    sendStatus = RdmaNative.rdma_send(handle, (IntPtr)address, (IntPtr)data.Length);
                }
                finally
                {
                    ExitNativeCall();
                }

                if (sendStatus != RdmaLinuxStatus.SUCCESS)
                {
                    LogDebug($"[Send] ERROR: rdma_send failed with status {sendStatus}");
                    return sendStatus;
                }

                RdmaCompletion completion;
                var pollStatus = PollCompletion(out completion, timeoutMs: 5000, completion_type: (int)RdmaLinuxOPCode.Other);

                if (pollStatus != RdmaLinuxStatus.SUCCESS)
                {
                    LogDebug($"[Send] ERROR: Poll completion failed with status {pollStatus}");
                    return pollStatus;
                }

                if (completion.status != 0)
                {
                    LogDebug($"[Send] ERROR: Send completion error detected: status={completion.status} (0x{completion.status:X}), vendor_err={completion.vendor_err}, opcode={completion.op_code}");
                    return RdmaLinuxStatus.ERROR_GENERAL;
                }

                return RdmaLinuxStatus.SUCCESS;
            }
            catch (Exception ex)
            {
                LogDebug($"[Send] EXCEPTION: {ex.GetType().Name}: {ex.Message}, StackTrace: {ex.StackTrace}");
                return RdmaLinuxStatus.ERROR_GENERAL;
            }
            finally
            {
                if (mrHandle != 0)
                {
                    var deregStatus = DeregisterMemory(mrHandle);
                    if (deregStatus != RdmaLinuxStatus.SUCCESS)
                    {
                        LogDebug($"[Send] WARNING: Failed to deregister memory {mrHandle}, status={deregStatus}");
                    }
                }
            }
        }

        /// <summary>
        /// Posts a receive buffer for incoming RDMA data
        /// </summary>
        /// <param name="buffer">Buffer to receive data into</param>
        /// <param name="slotId">Slot ID to identify this receive operation</param>
        /// <param name="address">Output: Address of the registered buffer</param>
        /// <param name="mrHandle">Output: Memory region handle</param>
        /// <returns>Post receive status</returns>
        public RdmaLinuxStatus PostReceive(byte[] buffer, ulong slotId, out ulong address, out long mrHandle)
        {
            address = 0;
            mrHandle = 0;

            if (buffer == null)
                return RdmaLinuxStatus.ERROR_INVALID_ARGUMENT;

            IntPtr handle;
            lock (connectionLock)
            {
                if (!isConnected || clientHandle == IntPtr.Zero)
                    return RdmaLinuxStatus.ERROR_CONNECTION_CLOSED;
                handle = clientHandle;
            }

            lock (recvLock)
            {
                if (pendingReceiveHandles.ContainsKey(slotId))
                {
                    LogDebug($"[Critical Block] PostReceive failed: Slot {slotId} is BUSY (Pending Recv). Check completion logic.");
                    return RdmaLinuxStatus.ERROR_BUSY;
                }
            }

            long tempMrHandle = 0;
            try
            {
                uint rkey;
                var regStatus = RegisterMemory(buffer, (uint)(RdmaLinuxAccessFlags.LOCAL_WRITE | RdmaLinuxAccessFlags.REMOTE_WRITE | RdmaLinuxAccessFlags.REMOTE_READ),
                                             out tempMrHandle, out rkey, out address);

                if (regStatus != RdmaLinuxStatus.SUCCESS)
                {
                    LogDebug($"Failed to register receive buffer: {regStatus}");
                    return regStatus;
                }

                EnterNativeCall();
                RdmaLinuxStatus postStatus;
                try
                {
                    postStatus = RdmaNative.post_receive(
                        handle,
                        (IntPtr)address,
                        (IntPtr)buffer.Length,
                        slotId);
                }
                finally
                {
                    ExitNativeCall();
                }

                if (postStatus != RdmaLinuxStatus.SUCCESS)
                {
                    LogDebug($"Failed to post receive: {postStatus}");
                    DeregisterMemory(tempMrHandle);
                    address = 0;
                    mrHandle = 0;
                    return postStatus;
                }

                lock (recvLock)
                {
                    pendingReceiveHandles[slotId] = tempMrHandle;
                }

                mrHandle = tempMrHandle;
                return RdmaLinuxStatus.SUCCESS;
            }
            catch (Exception ex)
            {
                LogDebug($"PostReceive failed with exception: {ex.Message}");
                if (tempMrHandle != 0)
                {
                    DeregisterMemory(tempMrHandle);
                }
                lock (recvLock)
                {
                    pendingReceiveHandles.Remove(slotId);
                }
                return RdmaLinuxStatus.ERROR_GENERAL;
            }
        }

        /// <summary>
        /// Polls for RDMA completion events
        /// </summary>
        /// <param name="completion">Output: Completion information</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <param name="completion_type">Completion type (0=Recv, 1=Other)</param>
        /// <returns>Poll status</returns>
        public RdmaLinuxStatus PollCompletion(out RdmaCompletion completion, int timeoutMs, int completion_type)
        {
            completion = new RdmaCompletion();

            IntPtr handle;
            lock (connectionLock)
            {
                if (!isConnected || clientHandle == IntPtr.Zero)
                    return RdmaLinuxStatus.ERROR_CONNECTION_CLOSED;
                handle = clientHandle;
            }

            try
            {
                EnterNativeCall();
                RdmaLinuxStatus status;
                try
                {
                    status = RdmaNative.poll_completion(handle, out completion, timeoutMs, completion_type);
                }
                finally
                {
                    ExitNativeCall();
                }

                if (status == RdmaLinuxStatus.SUCCESS && completion_type == (int)RdmaLinuxOPCode.Recv)
                {
                    lock (recvLock)
                    {
                        pendingReceiveHandles.Remove(completion.wr_id);
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                LogDebug($"RDMA poll_completion failed: {ex.Message}");
                return RdmaLinuxStatus.ERROR_GENERAL;
            }
        }

        /// <summary>
        /// Receives data from a previously posted receive buffer
        /// </summary>
        /// <param name="data">Output: Received data</param>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>Receive status</returns>
        public RdmaLinuxStatus Receive(out byte[] data, int timeoutMs = 5000)
        {
            data = null;

            IntPtr handle;
            lock (connectionLock)
            {
                if (!isConnected || clientHandle == IntPtr.Zero)
                    return RdmaLinuxStatus.ERROR_CONNECTION_CLOSED;
                handle = clientHandle;
            }

            try
            {
                // Poll for completion of a previously posted receive
                RdmaCompletion completion;

                EnterNativeCall();
                RdmaLinuxStatus status;
                try
                {
                    status = RdmaNative.poll_completion(handle, out completion, timeoutMs, (int)RdmaLinuxOPCode.Recv);
                }
                finally
                {
                    ExitNativeCall();
                }

                if (status != RdmaLinuxStatus.SUCCESS)
                {
                    if (status != RdmaLinuxStatus.ERROR_TIMEOUT)
                    {
                        LogDebug($"RDMA poll_completion failed: {status}");
                    }
                    return status;
                }

                if (completion.status != 0) // IBV_WC_SUCCESS == 0
                {
                    LogDebug($"RDMA receive work completion failed with status {completion.status}, vendor_err {completion.vendor_err}");
                    data = null;
                    return RdmaLinuxStatus.ERROR_GENERAL;
                }

                long mrHandle = 0;
                lock (recvLock)
                {
                    pendingReceiveHandles.TryGetValue(completion.wr_id, out mrHandle);
                }

                if (mrHandle != 0)
                {
                    RegisteredMr mr;
                    lock (mrLock)
                    {
                        registeredBuffers.TryGetValue(mrHandle, out mr);
                    }

                    if (mr.BufferPtr != IntPtr.Zero)
                    {
                        int copyLen = (int)Math.Min(completion.byte_len, (uint)mr.Length);
                        data = new byte[copyLen];
                        Marshal.Copy(mr.BufferPtr, data, 0, copyLen);
                    }
                    else
                    {
                        LogDebug($"Critical: Buffer pointer not found for MR Handle {mrHandle}");
                        data = new byte[0];
                    }

                    DeregisterMemory(mrHandle);

                    lock (recvLock)
                    {
                        pendingReceiveHandles.Remove(completion.wr_id);
                    }
                }
                else
                {
                    // If we receive a completion for an ID we aren't tracking, it might be
                    // an internal protocol message or a logic error.
                    LogDebug($"Warning: Received completion for unknown Slot ID (wr_id): {completion.wr_id}");
                    data = new byte[0];
                }

                return RdmaLinuxStatus.SUCCESS;
            }
            catch (Exception ex)
            {
                LogDebug($"RDMA receive failed: {ex.Message}");
                return RdmaLinuxStatus.ERROR_GENERAL;
            }
        }

        /// <summary>
        /// Registers a memory region for RDMA operations
        /// </summary>
        /// <param name="buffer">Buffer to register</param>
        /// <param name="accessFlags">Access flags</param>
        /// <param name="mrHandle">Memory region handle</param>
        /// <param name="rkey">Remote key</param>
        /// <param name="address">Virtual address</param>
        /// <returns>Registration status</returns>
        public RdmaLinuxStatus RegisterMemory(byte[] buffer, uint accessFlags,
            out long mrHandle, out uint rkey, out ulong address)
        {
            mrHandle = 0;
            rkey = 0;
            address = 0;

            if (buffer == null)
                return RdmaLinuxStatus.ERROR_INVALID_ARGUMENT;

            IntPtr handle;
            lock (connectionLock)
            {
                if (!isConnected || clientHandle == IntPtr.Zero)
                    return RdmaLinuxStatus.ERROR_CONNECTION_CLOSED;
                handle = clientHandle;
            }

            // Allocate a page-aligned (4K) native buffer for RDMA registration
            IntPtr alignedBuf = RdmaNative.rdma_alloc_aligned((IntPtr)buffer.Length);
            if (alignedBuf == IntPtr.Zero)
            {
                LogDebug("RDMA memory registration failed: unable to allocate aligned buffer");
                return RdmaLinuxStatus.ERROR_RESOURCE;
            }

            // Copy managed data into the aligned buffer
            Marshal.Copy(buffer, 0, alignedBuf, buffer.Length);

            EnterNativeCall();
            IntPtr mwHandle;
            RdmaLinuxStatus status;
            try
            {
                status = RdmaNative.register_memory_window(
                    handle,
                    alignedBuf,
                    (IntPtr)buffer.Length,
                    accessFlags,
                    out mwHandle,
                    out rkey);
            }
            finally
            {
                ExitNativeCall();
            }

            if (status == RdmaLinuxStatus.SUCCESS)
            {
                mrHandle = mwHandle.ToInt64();
                address = (ulong)alignedBuf.ToInt64();

                lock (mrLock)
                {
                    // Track this buffer to prevent premature deallocation
                    registeredBuffers[mrHandle] = new RegisteredMr
                    {
                        MwHandle = mwHandle,
                        BufferPtr = alignedBuf,
                        IsDeregistered = false,
                        Length = buffer.Length,
                        Token = rkey
                    };
                    tokenToHandleMap[rkey] = mrHandle;
                }
            }
            else
            {
                // Registration failed, free aligned buffer immediately
                RdmaNative.rdma_free_aligned(alignedBuf);
            }

            return status;
        }

        /// <summary>
        /// Deregisters a memory region
        /// </summary>
        /// <param name="mrHandle">Memory region handle</param>
        /// <returns>Deregistration status</returns>
        public RdmaLinuxStatus DeregisterMemory(long mrHandle)
        {
            RegisteredMr mr;
            lock (mrLock)
            {
                if (!registeredBuffers.TryGetValue(mrHandle, out mr))
                    return RdmaLinuxStatus.ERROR_INVALID_ARGUMENT;

                if (mr.IsDeregistered)
                    return RdmaLinuxStatus.SUCCESS;
            }

            // Capture state outside the lock for native call
            bool shouldCallNative = false;
            IntPtr nativeHandle = IntPtr.Zero;
            IntPtr mwHandle = IntPtr.Zero;

            lock (connectionLock)
            {
                nativeHandle = clientHandle;
                shouldCallNative = nativeHandle != IntPtr.Zero;
            }

            lock (mrLock)
            {
                // Re-check in case another thread deregistered while we were waiting
                if (!registeredBuffers.TryGetValue(mrHandle, out mr) || mr.IsDeregistered)
                    return RdmaLinuxStatus.SUCCESS;

                mwHandle = mr.MwHandle;
                mr.IsDeregistered = true;
            }

            RdmaLinuxStatus status = RdmaLinuxStatus.SUCCESS;
            if (shouldCallNative && mwHandle != IntPtr.Zero)
            {
                try
                {
                    EnterNativeCall();
                    try
                    {
                        status = RdmaNative.deregister_memory_window(nativeHandle, mwHandle);
                    }
                    finally
                    {
                        ExitNativeCall();
                    }
                }
                catch
                {
                    status = RdmaLinuxStatus.ERROR_GENERAL;
                }
            }

            lock (mrLock)
            {
                if (mr.BufferPtr != IntPtr.Zero)
                {
                    try { RdmaNative.rdma_free_aligned(mr.BufferPtr); } catch { }
                }
                registeredBuffers.Remove(mrHandle);
                tokenToHandleMap.Remove(mr.Token);
            }

            return status;
        }

        /// <summary>
        /// Read data from the locally registered memory window.
        /// </summary>
        /// <param name="memoryHandlerId">Memory window handle</param>
        /// <param name="buffer">Target buffer</param>
        /// <returns>NTSTATUS status code</returns>
        public NtStatus ReadFromMemory(long memoryHandlerId, byte[] buffer)
        {
            if (buffer == null)
                return NtStatus.STATUS_INVALID_PARAMETER_2;

            lock (mrLock)
            {
                if (!registeredBuffers.TryGetValue(memoryHandlerId, out var mr))
                {
                    return NtStatus.STATUS_INVALID_PARAMETER_2;
                }
                if (mr.IsDeregistered || mr.BufferPtr == IntPtr.Zero)
                {
                    return NtStatus.STATUS_INVALID_PARAMETER_2;
                }
                if (mr.Length < buffer.Length)
                {
                    return NtStatus.STATUS_BUFFER_OVERFLOW;
                }
                try
                {
                    Marshal.Copy(mr.BufferPtr, buffer, 0, buffer.Length);
                    return NtStatus.STATUS_SUCCESS;
                }
                catch
                {
                    return NtStatus.STATUS_INVALID_PARAMETER_2;
                }
            }
        }

        /// <summary>
        /// Retrieve information on all registered memory windows
        /// </summary>
        public IEnumerable<(uint Token, long MemoryHandlerId, bool IsValid, int Length)> EnumerateMemoryWindows()
        {
            lock (mrLock)
            {
                foreach (var kvp in tokenToHandleMap)
                {
                    uint token = kvp.Key;
                    long handle = kvp.Value;

                    if (registeredBuffers.TryGetValue(handle, out var mr))
                    {
                        yield return (token, handle, !mr.IsDeregistered, mr.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Performs RDMA write operation
        /// </summary>
        /// <param name="localBuffer">Local buffer</param>
        /// <param name="remoteDescriptor">Remote buffer descriptor</param>
        /// <returns>Write status</returns>
        public RdmaLinuxStatus Write(byte[] localBuffer, RdmaLinuxBufferDescriptor remoteDescriptor)
        {
            if (localBuffer == null)
                return RdmaLinuxStatus.ERROR_INVALID_ARGUMENT;

            if ((uint)localBuffer.Length > remoteDescriptor.Length)
                return RdmaLinuxStatus.ERROR_INVALID_ARGUMENT;

            IntPtr handle;
            lock (connectionLock)
            {
                if (!isConnected || clientHandle == IntPtr.Zero)
                    return RdmaLinuxStatus.ERROR_CONNECTION_CLOSED;
                handle = clientHandle;
            }

            long mrHandle = 0;
            try
            {
                uint rkey;
                ulong address;
                var regStatus = RegisterMemory(localBuffer, (uint)(RdmaLinuxAccessFlags.LOCAL_WRITE | RdmaLinuxAccessFlags.REMOTE_WRITE | RdmaLinuxAccessFlags.REMOTE_READ),
                                             out mrHandle, out rkey, out address);
                LogDebug($"RegisterMemory for write: {regStatus}");
                if (regStatus != RdmaLinuxStatus.SUCCESS)
                    return regStatus;

                EnterNativeCall();
                RdmaLinuxStatus writeStatus;
                try
                {
                    writeStatus = RdmaNative.write(handle, (IntPtr)address,
                        (IntPtr)localBuffer.Length, remoteDescriptor.Address, remoteDescriptor.Rkey);
                }
                finally
                {
                    ExitNativeCall();
                }
                LogDebug($"Call Write : {writeStatus}");
                if (writeStatus != RdmaLinuxStatus.SUCCESS)
                    return writeStatus;

                RdmaCompletion completion;
                var pollStatus = PollCompletion(out completion, timeoutMs: 5000, completion_type: (int)RdmaLinuxOPCode.Other);
                LogDebug($"PollCompletion for Write : {pollStatus}");
                return pollStatus;
            }
            catch (Exception ex)
            {
                LogDebug($"RDMA write failed: {ex.Message}");
                return RdmaLinuxStatus.ERROR_GENERAL;
            }
            finally
            {
                if (mrHandle != 0)
                {
                    DeregisterMemory(mrHandle);
                }
            }
        }

        /// <summary>
        /// Performs RDMA read operation
        /// </summary>
        /// <param name="localBuffer">Local buffer to receive data</param>
        /// <param name="remoteDescriptor">Remote buffer descriptor</param>
        /// <returns>Read status</returns>
        public RdmaLinuxStatus Read(byte[] localBuffer, RdmaLinuxBufferDescriptor remoteDescriptor)
        {
            if (localBuffer == null)
                return RdmaLinuxStatus.ERROR_INVALID_ARGUMENT;

            IntPtr handle;
            lock (connectionLock)
            {
                if (!isConnected || clientHandle == IntPtr.Zero)
                    return RdmaLinuxStatus.ERROR_CONNECTION_CLOSED;
                handle = clientHandle;
            }

            long mrHandle = 0;
            try
            {
                uint rkey;
                ulong address;
                var regStatus = RegisterMemory(localBuffer, (uint)(RdmaLinuxAccessFlags.LOCAL_WRITE | RdmaLinuxAccessFlags.REMOTE_WRITE | RdmaLinuxAccessFlags.REMOTE_READ),
                                             out mrHandle, out rkey, out address);
                LogDebug($"RegisterMemory for read: {regStatus}");
                if (regStatus != RdmaLinuxStatus.SUCCESS)
                    return regStatus;

                EnterNativeCall();
                RdmaLinuxStatus readStatus;
                try
                {
                    readStatus = RdmaNative.read(
                        handle,
                        (IntPtr)address,
                        (IntPtr)localBuffer.Length,
                        remoteDescriptor.Address,
                        remoteDescriptor.Rkey);
                }
                finally
                {
                    ExitNativeCall();
                }
                LogDebug($"Call Read : {readStatus}");
                if (readStatus != RdmaLinuxStatus.SUCCESS)
                    return readStatus;

                RdmaCompletion completion;
                var pollStatus = PollCompletion(out completion, timeoutMs: 5000, completion_type: (int)RdmaLinuxOPCode.Other);
                LogDebug($"PollCompletion for Read : {pollStatus}");
                if (pollStatus != RdmaLinuxStatus.SUCCESS)
                    return pollStatus;

                // Copy data from the page-aligned buffer back to the managed array
                if (mrHandle != 0)
                {
                    RegisteredMr mr;
                    lock (mrLock)
                    {
                        registeredBuffers.TryGetValue(mrHandle, out mr);
                    }
                    if (mr != null && mr.BufferPtr != IntPtr.Zero)
                    {
                        Marshal.Copy(mr.BufferPtr, localBuffer, 0, localBuffer.Length);
                    }
                }
                return RdmaLinuxStatus.SUCCESS;
            }
            catch (Exception ex)
            {
                LogDebug($"RDMA read failed: {ex.Message}");
                return RdmaLinuxStatus.ERROR_GENERAL;
            }
            finally
            {
                if (mrHandle != 0)
                {
                    DeregisterMemory(mrHandle);
                }
            }
        }

        /// <summary>
        /// Checks if the connection is still active
        /// </summary>
        /// <returns>True if connected, false otherwise.</returns>
        public bool IsConnectionActive()
        {
            return isConnected;
        }

        /// <summary>
        /// Notifies when the connection is disconnected
        /// </summary>
        /// <param name="timeoutMs">Timeout in milliseconds to wait for disconnection</param>
        /// <returns>True if disconnected within timeout, false otherwise</returns>
        public bool WaitForDisconnect(int timeoutMs)
        {
            if (!isConnected)
            {
                return true;
            }

            IntPtr handle;
            lock (connectionLock)
            {
                if (!isConnected || clientHandle == IntPtr.Zero)
                {
                    return true;
                }
                handle = clientHandle;
            }

            int timeoutSeconds = Math.Max(1, timeoutMs / 1000);

            EnterNativeCall();
            RdmaLinuxStatus status;
            try
            {
                status = RdmaNative.wait_for_disconnect(handle, timeoutSeconds);
            }
            finally
            {
                ExitNativeCall();
            }

            if (status == RdmaLinuxStatus.SUCCESS || status == RdmaLinuxStatus.ERROR_CONNECTION_CLOSED)
            {
                lock (connectionLock)
                {
                    isConnected = false;
                }
                LogDebug($"Disconnected detected by event listener");
                return true;
            }
            else if (status == RdmaLinuxStatus.ERROR_TIMEOUT)
            {
                LogDebug($"Timeout reached while waiting for disconnect event");
                return false;
            }

            return !isConnected;
        }

        /// <summary>
        /// Invalidate the memory window
        /// </summary>
        /// <param name="memoryHandlerId">Memory window handle</param>
        /// <param name="resultId">Output: Asynchronous operation result ID</param>
        /// <returns>Status code</returns>
        public RdmaLinuxStatus InvalidateMemoryWindow(long memoryHandlerId, out ulong resultId)
        {
            resultId = 0;

            IntPtr handle;
            lock (connectionLock)
            {
                if (!isConnected || clientHandle == IntPtr.Zero)
                    return RdmaLinuxStatus.ERROR_CONNECTION_CLOSED;
                handle = clientHandle;
            }

            lock (mrLock)
            {
                if (!registeredBuffers.TryGetValue(memoryHandlerId, out var mr))
                    return RdmaLinuxStatus.ERROR_INVALID_ARGUMENT;

                if (mr.IsDeregistered)
                    return RdmaLinuxStatus.ERROR_INVALID_ARGUMENT;
            }

            try
            {
                EnterNativeCall();
                try
                {
                    var status = RdmaNative.invalidate_memory_window(
                        handle,
                        memoryHandlerId,
                        out resultId);
                    return status;
                }
                finally
                {
                    ExitNativeCall();
                }
            }
            catch (Exception ex)
            {
                LogDebug($"InvalidateMemoryWindow failed: {ex.Message}");
                return RdmaLinuxStatus.ERROR_GENERAL;
            }
        }

        /// <summary>
        /// Waiting for a specific Invalidate operation to complete
        /// </summary>
        public RdmaLinuxStatus WaitForInvalidateCompletion(ulong resultId, int timeoutMs = 5000)
        {
            IntPtr handle;
            lock (connectionLock)
            {
                if (!isConnected || clientHandle == IntPtr.Zero)
                    return RdmaLinuxStatus.ERROR_CONNECTION_CLOSED;
                handle = clientHandle;
            }

            try
            {
                EnterNativeCall();
                try
                {
                    return RdmaNative.check_invalidate_completion(handle, resultId, timeoutMs);
                }
                finally
                {
                    ExitNativeCall();
                }
            }
            catch (Exception ex)
            {
                LogDebug($"WaitForInvalidateCompletion failed: {ex.Message}");
                return RdmaLinuxStatus.ERROR_GENERAL;
            }
        }

        /// <summary>
        /// Logs debug information
        /// </summary>
        /// <param name="message">Debug message</param>
        private void LogDebug(string message)
        {
            try
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [RdmaLinuxAdapter]{message}" + Environment.NewLine;
                File.AppendAllText("rdma_debug.log", logMessage);
            }
            catch
            {
                // Ignore logging failures
            }
        }

        #region IDisposable Implementation
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    Disconnect();
                }
                else
                {
                    // Finalizer: do not block or call native disconnect.
                    // Just release any pinned buffers so the GC isn't hindered.
                    lock (mrLock)
                    {
                        registeredBuffers.Clear();
                    }
                    lock (recvLock)
                    {
                        pendingReceiveHandles.Clear();
                    }
                    isConnected = false;
                    clientHandle = IntPtr.Zero;
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RdmaLinuxAdapter()
        {
            Dispose(false);
        }
        #endregion
    }

    /// <summary>
    /// Helper class to convert between Windows RDMA status and Linux RDMA status
    /// </summary>
    public static class RdmaStatusConverter
    {
        /// <summary>
        /// Converts Linux RDMA status to Windows NtStatus
        /// </summary>
        public static NtStatus ToNtStatus(RdmaLinuxStatus linuxStatus)
        {
            return linuxStatus switch
            {
                RdmaLinuxStatus.SUCCESS => NtStatus.STATUS_SUCCESS,
                RdmaLinuxStatus.ERROR_TIMEOUT => NtStatus.STATUS_IO_TIMEOUT,
                RdmaLinuxStatus.ERROR_CONNECTION_CLOSED => NtStatus.STATUS_CONNECTION_DISCONNECTED,
                RdmaLinuxStatus.ERROR_RESOURCE => NtStatus.STATUS_INSUFFICIENT_RESOURCES,
                _ => NtStatus.STATUS_UNSUCCESSFUL
            };
        }

        /// <summary>
        /// Converts Windows NtStatus to Linux RDMA status
        /// </summary>
        public static RdmaLinuxStatus ToRdmaLinuxStatus(NtStatus ntStatus)
        {
            return ntStatus switch
            {
                NtStatus.STATUS_SUCCESS => RdmaLinuxStatus.SUCCESS,
                NtStatus.STATUS_IO_TIMEOUT => RdmaLinuxStatus.ERROR_TIMEOUT,
                NtStatus.STATUS_CONNECTION_DISCONNECTED => RdmaLinuxStatus.ERROR_CONNECTION_CLOSED,
                NtStatus.STATUS_INSUFFICIENT_RESOURCES => RdmaLinuxStatus.ERROR_RESOURCE,
                _ => RdmaLinuxStatus.ERROR_GENERAL
            };
        }
    }
}
