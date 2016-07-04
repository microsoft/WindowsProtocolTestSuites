// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Sqos
{
    /// <summary>
    /// SqosClient is the exposed interface for testing Sqos server.
    /// </summary>
    public sealed class SqosClient : IDisposable
    {
        #region Fields

        private Smb2ClientTransport transport;
        private bool disposed;
        private TimeSpan timeout;

        #endregion

        #region Constructor & Dispose

        /// <summary>
        /// Constructor
        /// </summary>
        public SqosClient(TimeSpan timeout)
        {
            this.timeout = timeout;
        }

        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                    if (this.transport != null)
                    {
                        this.transport.Dispose();
                        this.transport = null;
                    }
                }

                this.disposed = true;
            }
        }

        /// <summary>
        /// finalizer
        /// </summary>
        ~SqosClient()
        {
            Dispose(false);
        }

        #endregion

        /// <summary>
        /// Set up connection with server.
        /// Including 5 steps: 1. Tcp connection 2. Negotiation 3. SessionSetup 4. TreeConnect 5. Create an open on the vhd file
        /// </summary>
        /// <param name="serverName">server name</param>
        /// <param name="serverIP">server IP address</param> 
        /// <param name="domain">user's domain</param>
        /// <param name="userName">user's name</param>
        /// <param name="password">user's password</param>
        /// <param name="securityPackage">The security package</param>
        /// <param name="useServerToken">Whether to use token from server</param>
        /// <param name="shareName">Name of the share when TreeConnect</param>
        /// <param name="vhdName">Name of the vhd file</param>
        /// <returns>Status of the create operation</returns>
        public uint ConnectToVHD(
            string serverName,
            IPAddress serverIP,
            string domain,
            string userName,
            string password,
            SecurityPackageType securityPackage,
            bool useServerToken,
            string shareName,
            string vhdName)
        {
            transport = new Smb2ClientTransport();
            transport.ConnectShare(serverName, serverIP, domain, userName, password, shareName, securityPackage, useServerToken);
            uint status;
            Smb2CreateContextResponse[] serverContexts;
            CREATE_Response response;
            transport.Create(
                vhdName,
                // The desired access is set the same as Windows client's behavior
                FsFileDesiredAccess.GENERIC_READ | FsFileDesiredAccess.GENERIC_WRITE | FsFileDesiredAccess.DELETE,
                ShareAccess_Values.FILE_SHARE_DELETE | ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE,
                FsImpersonationLevel.Impersonation,
                FsFileAttribute.NONE,
                FsCreateDisposition.FILE_OPEN_IF,
                FsCreateOption.NONE,
                null,
                out status,
                out serverContexts,
                out response);
            return status;
        }

        /// <summary>
        /// Disconnect from server.
        /// </summary>
        public void Disconnect()
        {
            if (transport != null)
            {
                this.transport.Disconnect(timeout);
            }
        }

        /// <summary>
        /// Close the shared virtual disk file
        /// </summary>
        public void Close()
        {
            transport.Close();
        }

        /// <summary>
        /// Read content from the shared virtual disk file
        /// </summary>
        /// <param name="offset">In bytes, from the beginning of the virtual disk where data should be read</param>
        /// <param name="length">The number of bytes to read</param>
        /// <param name="data">The buffer to receive the data.</param>
        /// <returns>Status of response packet</returns>
        public uint Read(ulong offset, uint length, out byte[] data)
        {
            return transport.Read(timeout, offset, length, out data);
        }

        /// <summary>
        /// Write content to the shared virtual disk file
        /// </summary>
        /// <param name="offset">In bytes, from the beginning of the virtual disk where data should be written</param>
        /// <param name="data">A buffer containing the bytes to be written.</param>
        /// <returns>Status of response packet</returns>
        public uint Write(ulong offset, byte[] data)
        {
            return transport.Write(timeout, offset, data);
        }

        /// <summary>
        /// Send sqos request and receive sqos response
        /// </summary>
        /// <param name="request">The sqos request packet</param>
        /// <param name="response">The sqos response packet</param>
        /// <returns>Status of the sqos response</returns>
        public uint SendAndReceiveSqosPacket(SqosRequestPacket request, out SqosResponsePacket response)
        {
            transport.SendIoctlPayload(CtlCode_Values.FSCTL_STORAGE_QOS_CONTROL, request.ToBytes());
            uint smb2Status;
            byte[] payload;
            transport.ExpectIoctlPayload(out smb2Status, out payload);
            if (smb2Status != Smb2Status.STATUS_SUCCESS || payload == null)
            {
                response = null;
                return smb2Status;
            }

            response = new SqosResponsePacket();
            STORAGE_QOS_CONTROL_Header responseHeader = TypeMarshal.ToStruct<STORAGE_QOS_CONTROL_Header>(payload);

            // Parse the header first to decode the ProtocolVersion field.
            if (responseHeader.ProtocolVersion == (ushort)SQOS_PROTOCOL_VERSION.Sqos10)
            {
                response.FromBytes(SqosResponseType.V10, payload);
            }
            else if (responseHeader.ProtocolVersion == (ushort)SQOS_PROTOCOL_VERSION.Sqos11)
            {
                response.FromBytes(SqosResponseType.V11, payload);
            }

            return smb2Status;
        }
    }
}
