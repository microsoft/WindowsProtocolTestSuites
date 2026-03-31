// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk;
#if WINDOWS
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;
#endif
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestManager.SMBDPlugin
{
    /// <summary>
    /// Cross-platform RDMA adapter interface
    /// </summary>
    public interface IRdmaPlatformAdapter
    {
        bool IsConnected { get; }
        NtStatus Connect(string localIpAddress, string remoteIpAddress, int port, uint maxReceiveSize);
        NtStatus Disconnect();
        NtStatus SendMessage(byte[] data);
        NtStatus ReceiveMessage(TimeSpan timeout, out byte[] data);
        NtStatus RegisterBuffer(uint length, SmbdBufferReadWrite permission, bool isBigEndian, out SmbdBufferDescriptorV1 descriptor);
        NtStatus WriteRegisteredBuffer(byte[] source, SmbdBufferDescriptorV1 descriptor);
        NtStatus ReadRegisteredBuffer(byte[] destination, SmbdBufferDescriptorV1 descriptor);
        NtStatus Query(out RdmaAdapterData adapterInfo);
        NtStatus Negotiate(
            SmbdVersion minVersion,
            SmbdVersion maxVersion,
            ushort creditsRequested,
            ushort receiveCreditMax,
            uint preferredSendSize,
            uint maxReceiveSize,
            uint maxFragmentedSize,
            out SmbdNegotiateResponse response
        );
    }
    public class RdmaAdapterData
    {
        public int VendorId;

        public int DeviceId;

        public uint MaxInboundSge;

        public uint MaxInboundRequests;

        public uint MaxInboundLength;

        public uint MaxOutboundSge;

        public uint MaxOutboundRequests;

        public uint MaxOutboundLength;

        public uint MaxInlineData;

        public uint MaxInboundReadLimit;

        public uint MaxOutboundReadLimit;

        public uint MaxCqEntries;

        public uint MaxRegistrationSize;

        public uint MaxWindowSize;

        public uint LargeRequestThreshold;

        public uint MaxCallerData;

        public uint MaxCalleeData;
    }
#if WINDOWS
    /// <summary>
    /// Windows RDMA platform adapter
    /// </summary>
    public class WindowsRdmaPlatformAdapter : IRdmaPlatformAdapter
    {
        private RdmaAdapter adapter;
        private SmbdClient smbdClient;
        private RdmaProviderInfo[] providers;
        private bool initialized = false;

        public bool IsConnected { get; private set; }

        public WindowsRdmaPlatformAdapter()
        {
            smbdClient = new SmbdClient();
        }

        public NtStatus Connect(string localIpAddress, string remoteIpAddress, int port, uint maxReceiveSize)
        {
            // Load RDMA providers
            NtStatus status = (NtStatus)RdmaProvider.LoadRdmaProviders(out providers);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            // Find suitable adapter
            foreach (var provider in providers)
            {
                if (provider.Provider == null)
                {
                    continue;
                }

                RdmaAdapter outputAdapter;
                status = (NtStatus)provider.Provider.OpenAdapter(localIpAddress, (short)System.Net.Sockets.AddressFamily.InterNetwork, out outputAdapter);
                if (status == NtStatus.STATUS_SUCCESS)
                {
                    adapter = outputAdapter;
                    break;
                }
            }

            if (adapter == null)
            {
                return NtStatus.STATUS_UNSUCCESSFUL;
            }

            // Connect to server
            status = smbdClient.ConnectToServerOverRdma(
                localIpAddress,
                remoteIpAddress,
                port,
                System.Net.Sockets.AddressFamily.InterNetwork,
                128, // max inbound
                128, // max outbound
                4,   // max inbound read limit
                maxReceiveSize
            );

            if (status == NtStatus.STATUS_SUCCESS)
            {
                IsConnected = true;
                initialized = true;
            }

            return status;
        }

        public NtStatus Disconnect()
        {
            if (smbdClient != null)
            {
                smbdClient.Disconnect();
                IsConnected = false;
            }
            return NtStatus.STATUS_SUCCESS;
        }

        public NtStatus SendMessage(byte[] data)
        {
            return smbdClient.SendMessage(data);
        }

        public NtStatus ReceiveMessage(TimeSpan timeout, out byte[] data)
        {
            return smbdClient.ReceiveMessage(timeout, out data);
        }

        public NtStatus RegisterBuffer(uint length, SmbdBufferReadWrite permission, bool isBigEndian, out SmbdBufferDescriptorV1 descriptor)
        {
            return smbdClient.RegisterBuffer(length, permission, isBigEndian, out descriptor);
        }

        public NtStatus WriteRegisteredBuffer(byte[] source, SmbdBufferDescriptorV1 descriptor)
        {
            return smbdClient.WriteRegisteredBuffer(source, descriptor);
        }

        public NtStatus ReadRegisteredBuffer(byte[] destination, SmbdBufferDescriptorV1 descriptor)
        {
            return smbdClient.ReadRegisteredBuffer(destination, descriptor);
        }

        public NtStatus Query(out RdmaAdapterData adapterInfo)
        {
            if (adapter != null)
            {
                RdmaAdapterInfo info = null;
                int status = adapter.Query(out info);
                adapterInfo = new RdmaAdapterData()
                {
                    VendorId = info.VendorId,
                    DeviceId = info.DeviceId,
                    MaxInboundSge = info.MaxInboundSge,
                    MaxInboundRequests = info.MaxInboundRequests,
                    MaxInboundLength = info.MaxInboundLength,
                    MaxOutboundSge = info.MaxOutboundSge,
                    MaxOutboundRequests = info.MaxOutboundRequests,
                    MaxOutboundLength = info.MaxOutboundLength,
                    MaxInlineData = info.MaxInlineData,
                    MaxInboundReadLimit = info.MaxInboundReadLimit,
                    MaxOutboundReadLimit = info.MaxOutboundReadLimit,
                    MaxCqEntries = info.MaxCqEntries,
                    MaxRegistrationSize = info.MaxRegistrationSize,
                    MaxWindowSize = info.MaxWindowSize,
                    LargeRequestThreshold = info.LargeRequestThreshold,
                    MaxCallerData = info.MaxCallerData,
                    MaxCalleeData = info.MaxCalleeData,
                };
                return (NtStatus)status;

            }
            else
            {
                adapterInfo = new RdmaAdapterData
                {
                    MaxInboundRequests = 128,
                    MaxOutboundRequests = 128,
                    MaxInboundReadLimit = 4
                };
                return NtStatus.STATUS_SUCCESS;
            }
        }

        public NtStatus Negotiate(
            SmbdVersion minVersion,
            SmbdVersion maxVersion,
            ushort creditsRequested,
            ushort receiveCreditMax,
            uint preferredSendSize,
            uint maxReceiveSize,
            uint maxFragmentedSize,
            out SmbdNegotiateResponse response)
        {
            return smbdClient.Negotiate(minVersion, maxVersion, creditsRequested, receiveCreditMax, preferredSendSize, maxReceiveSize, maxFragmentedSize, out response);
        }
    }
#endif
    /// <summary>
    /// Linux RDMA platform adapter
    /// </summary>
    public class LinuxRdmaPlatformAdapter : IRdmaPlatformAdapter
    {
        private Microsoft.Protocols.TestTools.StackSdk.FileAccessService.RdmaLinux.RdmaLinuxAdapter linuxAdapter;
        private Dictionary<IntPtr, (IntPtr handle, uint length)> registeredBuffers; // Track registered buffers on Linux
        private bool initialized = false;

        public bool IsConnected => linuxAdapter?.IsConnected ?? false;

        public LinuxRdmaPlatformAdapter()
        {
            linuxAdapter = new Microsoft.Protocols.TestTools.StackSdk.FileAccessService.RdmaLinux.RdmaLinuxAdapter();
            registeredBuffers = new Dictionary<IntPtr, (IntPtr, uint)>(); // Initialize buffer tracking dictionary
        }

        public NtStatus Connect(string localIpAddress, string remoteIpAddress, int port, uint maxReceiveSize)
        {
            var status = linuxAdapter.Connect(remoteIpAddress, (ushort)port);
            return ConvertLinuxStatusToNtStatus(status);
        }

        public NtStatus Disconnect()
        {
            if (linuxAdapter != null)
            {
                linuxAdapter.Disconnect();
                return NtStatus.STATUS_SUCCESS;
            }
            return NtStatus.STATUS_SUCCESS;
        }

        public NtStatus SendMessage(byte[] data)
        {
            if (linuxAdapter != null)
            {
                var status = linuxAdapter.Send(data);
                return ConvertLinuxStatusToNtStatus(status);
            }
            return NtStatus.STATUS_UNSUCCESSFUL;
        }

        public NtStatus ReceiveMessage(TimeSpan timeout, out byte[] data)
        {
            data = null;
            if (linuxAdapter != null)
            {
                var status = linuxAdapter.Receive(out data);
                return ConvertLinuxStatusToNtStatus(status);
            }
            return NtStatus.STATUS_UNSUCCESSFUL;
        }

        public NtStatus RegisterBuffer(uint length, SmbdBufferReadWrite permission, bool isBigEndian, out SmbdBufferDescriptorV1 descriptor)
        {
            descriptor = new SmbdBufferDescriptorV1();
            if (linuxAdapter != null)
            {
                // Allocate buffer on Linux
                var handle = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)length);

                // Record allocated buffer information
                registeredBuffers[handle] = (handle, length);

                // For Linux, set values for SmbdBufferDescriptorV1
                // Offset 0 means starting from the beginning of the buffer
                // Token as identifier (can use lower 32 bits of handle)
                // Length is the buffer length
                descriptor.Offset = 0;
                descriptor.Token = (uint)(handle.ToInt64() & 0xFFFFFFFF);
                descriptor.Length = length;

                return NtStatus.STATUS_SUCCESS;
            }
            return NtStatus.STATUS_UNSUCCESSFUL;
        }

        public NtStatus WriteRegisteredBuffer(byte[] source, SmbdBufferDescriptorV1 descriptor)
        {
            // Find the corresponding buffer
            var targetAddr = new IntPtr((long)descriptor.Token); // Using address information stored in Token
            System.Runtime.InteropServices.Marshal.Copy(source, 0, targetAddr, Math.Min(source.Length, (int)descriptor.Length));
            return NtStatus.STATUS_SUCCESS;
        }

        public NtStatus ReadRegisteredBuffer(byte[] destination, SmbdBufferDescriptorV1 descriptor)
        {
            // Find the corresponding buffer
            var sourceAddr = new IntPtr((long)descriptor.Token); // Using address information stored in Token
            var length = Math.Min(destination.Length, (int)descriptor.Length);
            System.Runtime.InteropServices.Marshal.Copy(sourceAddr, destination, 0, length);
            return NtStatus.STATUS_SUCCESS;
        }

        public NtStatus Query(out RdmaAdapterData adapterInfo)
        {
            // Return default values for Linux adapter
            adapterInfo = new RdmaAdapterData
            {
                MaxInboundRequests = 128,
                MaxOutboundRequests = 128,
                MaxInboundReadLimit = 4
            };
            return NtStatus.STATUS_SUCCESS;
        }

        public NtStatus Negotiate(
            SmbdVersion minVersion,
            SmbdVersion maxVersion,
            ushort creditsRequested,
            ushort receiveCreditMax,
            uint preferredSendSize,
            uint maxReceiveSize,
            uint maxFragmentedSize,
            out SmbdNegotiateResponse response)
        {
            response = new SmbdNegotiateResponse
            {
                MinVersion = minVersion,
                MaxVersion = maxVersion,
                CreditsRequested = creditsRequested,
                CreditsGranted = receiveCreditMax,
                PreferredSendSize = preferredSendSize,
                MaxReceiveSize = maxReceiveSize,
                MaxFragmentedSize = maxFragmentedSize
            };
            return NtStatus.STATUS_SUCCESS;
        }

        private NtStatus ConvertLinuxStatusToNtStatus(Microsoft.Protocols.TestTools.StackSdk.FileAccessService.RdmaLinux.RdmaLinuxStatus linuxStatus)
        {
            switch (linuxStatus)
            {
                case Microsoft.Protocols.TestTools.StackSdk.FileAccessService.RdmaLinux.RdmaLinuxStatus.SUCCESS:
                    return NtStatus.STATUS_SUCCESS;
                case Microsoft.Protocols.TestTools.StackSdk.FileAccessService.RdmaLinux.RdmaLinuxStatus.ERROR_TIMEOUT:
                    return NtStatus.STATUS_IO_TIMEOUT;
                case Microsoft.Protocols.TestTools.StackSdk.FileAccessService.RdmaLinux.RdmaLinuxStatus.ERROR_CONNECTION_CLOSED:
                    return NtStatus.STATUS_CONNECTION_DISCONNECTED;
                case Microsoft.Protocols.TestTools.StackSdk.FileAccessService.RdmaLinux.RdmaLinuxStatus.ERROR_INVALID_ARGUMENT:
                    return NtStatus.STATUS_INVALID_PARAMETER;
                default:
                    return NtStatus.STATUS_UNSUCCESSFUL;
            }
        }
    }

    /// <summary>
    /// RDMA Platform Adapter Factory
    /// </summary>
    public static class RdmaPlatformAdapterFactory
    {
        public static IRdmaPlatformAdapter CreateAdapter()
        {
#if WINDOWS
            return new WindowsRdmaPlatformAdapter();
#else
            return new LinuxRdmaPlatformAdapter();
#endif
        }
    }
}