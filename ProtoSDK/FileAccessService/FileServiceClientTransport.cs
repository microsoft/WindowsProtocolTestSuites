// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// FileServiceClientTransport is the base class for file access family transport
    /// </summary>
    public abstract class FileServiceClientTransport : IDisposable
    {
        #region fields

        /// <summary>
        /// Indicate if this object has been disposed
        /// </summary>
        private bool disposed;

        #endregion


        #region Property

        /// <summary>
        /// To detect whether there are packets cached in the queue of Transport.
        /// Usually, it should be called after Disconnect to assure all events occurred in transport
        /// have been handled.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The transport is not connected.</exception>
        public abstract bool IsDataAvailable
        {
            get;
        }


        /// <summary>
        /// the context of client transport, that contains the runtime states and variables.<para/>
        /// this interface provides no method, user need to down-casting to the specified class.<para/>
        /// for example, down-casting to CifsClientContext, SmbClientContext or Smb2ClientGlobalContext.
        /// </summary>
        public abstract FileServiceClientContext Context
        {
            get;
        }

        #endregion


        #region Dispose

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
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                }

                // Call the appropriate methods to clean up unmanaged resources.

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~FileServiceClientTransport()
        {
            Dispose(false);
        }

        #endregion


        #region Methods For Dfsc

        /// <summary>
        /// Set up connection with server.
        /// Including 4 steps: 1. Tcp connection 2. Negotiation 3. SessionSetup 4. TreeConnect in order
        /// </summary>
        /// <param name="server">server name of ip address</param>
        /// <param name="client">client name of ip address</param>
        /// <param name="domain">user's domain</param>
        /// <param name="userName">user's name</param>
        /// <param name="password">user's password</param> 
        /// <param name="timeout">The pending time to get server's response in step 2, 3 or 4</param>
        /// <exception cref="System.Net.ProtocolViolationException">Fail to set up connection with server</exception>
        public abstract void Connect(
            string server,
            string client,
            string domain,
            string userName,
            string password,
            TimeSpan timeout,
            SecurityPackageType securityPackage, 
            bool useServerToken);


        /// <summary>
        /// Disconnect from server.
        /// Including 3 steps: 1. TreeDisconnect 2. Logoff 3. Tcp disconnection in order.
        /// </summary>
        /// <param name="timeout">The pending time to get server's response in step 1 or 2</param>
        /// <exception cref="System.Net.ProtocolViolationException">Fail to disconnect from server</exception>
        /// <exception cref="System.InvalidOperationException">The transport is not connected</exception>
        public abstract void Disconnect(TimeSpan timeout);


        /// <summary>
        /// Send a Dfs request to server
        /// </summary>
        /// <param name="payload">REQ_GET_DFS_REFERRAL or REQ_GET_DFS_REFERRAL_EX structure in byte array</param>
        /// <param name="isEX">Indicates if payload contains REQ_GET_DFS_REFERRAL_EX structure</param>
        /// <exception cref="System.ArgumentNullException">the payload to be sent is null.</exception>
        /// <exception cref="ArgumentException">Transport does not support REQ_GET_DFS_REFERRAL_EX packets</exception>
        /// <exception cref="System.InvalidOperationException">The transport is not connected</exception>
        public abstract void SendDfscPayload(byte[] payload, bool isEX);


        /// <summary>
        /// Wait for the Dfs response packet from server.
        /// Including 3 steps: 1. TreeDisconnect 2. Logoff 3. Tcp disconnection in order.
        /// </summary>
        /// <param name="status">The status of response</param>
        /// <param name="payload">RESP_GET_DFS_REFERRAL structure in byte array</param>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <exception cref="System.InvalidOperationException">The transport is not connected</exception>
        public abstract void ExpectDfscPayload(TimeSpan timeout, out uint status, out byte[] payload);


        #endregion


        #region Methods For Rpce

        /// <summary>
        /// Connect to a share indicated by shareName in server
        /// This will use smb over tcp as transport. Only one server
        /// can be connected at one time
        /// </summary>
        /// <param name="serverName">The server Name</param>
        /// <param name="port">The server port</param>
        /// <param name="ipVersion">The ip version</param>
        /// <param name="domain">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password</param>
        /// <param name="shareName">The share name</param>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract void ConnectShare(string serverName, int port, IpVersion ipVersion, string domain,
            string userName, string password, string shareName);

        /// <summary>
        /// Connect to a share indicated by shareName in server
        /// This will use smb over tcp as transport. Only one server
        /// can be connected at one time
        /// </summary>
        /// <param name="serverName">The server Name</param>
        /// <param name="port">The server port</param>
        /// <param name="ipVersion">The ip version</param>
        /// <param name="domain">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password</param>
        /// <param name="shareName">The share name</param>
        /// <param name="securityPackage">The security package</param>
        /// <param name="useServerToken">Whether to use token from server</param>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract void ConnectShare(string serverName, int port, IpVersion ipVersion, string domain,
            string userName, string password, string shareName, SecurityPackageType securityPackage, bool useServerToken);


        /// <summary>
        /// Connect to a share indicated by shareName in server.
        /// This will use smb over netbios as transport. Only one server
        /// can be connected at one time.
        /// </summary>
        /// <param name="serverNetBiosName">The server netbios name</param>
        /// <param name="clientNetBiosName">The client netbios name</param>
        /// <param name="domain">The domain name</param>
        /// <param name="userName">The user name</param>
        /// <param name="password">The password</param>
        /// <param name="shareName">The share name</param>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract void ConnectShare(string serverNetBiosName, string clientNetBiosName, string domain,
            string userName, string password, string shareName);


        /// <summary>
        /// Create File, named pipe, directory. One transport can only create one file.
        /// </summary>
        /// <param name="fileName">The file, namedpipe, directory name</param>
        /// <param name="desiredAccess">The desired access</param>
        /// <param name="impersonationLevel">The impersonation level</param>
        /// <param name="fileAttribute">The file attribute, this field is only valid when create file.
        /// </param>
        /// <param name="createDisposition">Defines the action the server MUST take if the file that is
        /// specified in the name field already exists</param>
        /// <param name="createOption">Specifies the options to be applied when creating or opening the file</param>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract void Create(string fileName, FsFileDesiredAccess desiredAccess, FsImpersonationLevel impersonationLevel,
            FsFileAttribute fileAttribute, FsCreateDisposition createDisposition, FsCreateOption createOption);


        /// <summary>
        /// Create directory. One transport can only create one directory
        /// </summary>
        /// <param name="directoryName">The directory name</param>
        /// <param name="desiredAccess">The desired access</param>
        /// <param name="impersonationLevel">The impersonation level</param>
        /// <param name="fileAttribute">The file attribute, this field is only valid when create file.
        /// </param>
        /// <param name="createDisposition">Defines the action the server MUST take if the file that is
        /// specified in the name field already exists</param>
        /// <param name="createOption">Specifies the options to be applied when creating or opening the file</param>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract void Create(string directoryName, FsDirectoryDesiredAccess desiredAccess, FsImpersonationLevel impersonationLevel,
            FsFileAttribute fileAttribute, FsCreateDisposition createDisposition, FsCreateOption createOption);


        /// <summary>
        /// Write data to server. cifs/smb implementation of this interface should pay attention to offset.
        /// They may not accept ulong as offset
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="offset">The offset of the file from where client wants to start writing</param>
        /// <param name="data">The data which will be written to server</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract uint Write(TimeSpan timeout, ulong offset, byte[] data);


        /// <summary>
        /// Read data from server, start at the positon indicated by offset
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="offset">From where it will read</param>
        /// <param name="length">The length of the data client wants to read</param>
        /// <param name="data">The read data</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract uint Read(TimeSpan timeout, ulong offset, uint length, out byte[] data);


        /// <summary>
        /// Do IO control on server, this function does not accept file system control code as control code.
        /// for that use, use FileSystemControl() function instead
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="controlCode">The IO control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="output">The output data of this control operation</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract uint IoControl(TimeSpan timeout, uint controlCode, byte[] input, out byte[] output);


        /// <summary>
        /// Do IO control on server, this function does not accept file system control code as control code.
        /// for that use, use FileSystemControl() function instead
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="controlCode">The IO control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="inputResponse">The input data in the response of this control operation</param>
        /// <param name="outputResponse">The output data in the response of this control operation</param>
        /// <param name="maxInputResponse">The maximum number of bytes that the server can return for the input data</param>
        /// <param name="maxOutputResponse">The maximum number of bytes that the server can return for the output data</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract uint IoControl(TimeSpan timeout, uint controlCode, byte[] input, out byte[] inputResponse,
            out byte[] outputResponse, uint maxInputResponse, uint maxOutputResponse);


        /// <summary>
        /// Do File system control on server
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="controlCode">The file system control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="output">The output data of this control operation</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract uint IoControl(TimeSpan timeout, FsCtlCode controlCode, byte[] input, out byte[] output);


        /// <summary>
        /// Do File system control on server
        /// </summary>
        /// <param name="timeout">The pending time to get server's response</param>
        /// <param name="controlCode">The file system control code</param>
        /// <param name="input">The input data of this control operation</param>
        /// <param name="inputResponse">The input data in the response of this control operation</param>
        /// <param name="outputResponse">The output data in the response of this control operation</param>
        /// <param name="maxInputResponse">The maximum number of bytes that the server can return for the input data</param>
        /// <param name="maxOutputResponse">The maximum number of bytes that the server can return for the output data</param>
        /// <returns>
        /// a uint value that specifies the status of response packet.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract uint IoControl(TimeSpan timeout, FsCtlCode controlCode, byte[] input, out byte[] inputResponse,
            out byte[] outputResponse, uint maxInputResponse, uint maxOutputResponse);


        /// <summary>
        /// Close file, named pipe, directory
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract void Close();


        /// <summary>
        /// Disconnect share
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if there is any error occurred</exception>
        public abstract void DisconnetShare();

        #endregion
    }
}
