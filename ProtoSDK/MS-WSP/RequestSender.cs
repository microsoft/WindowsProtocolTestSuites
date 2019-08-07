// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// RequeseSender class sends  MS-WSP messages to the protocol server
    /// and receives subsequent response for the same.
    /// </summary>
    public class RequestSender
    {
        /// <summary>
        /// Name of the Pipe
        /// </summary>
        string pipeName = string.Empty;
        /// <summary>
        /// Handle of the pipe
        /// </summary>
        public SafeFileHandle handle;

        #region Constants

        /// <summary>
        /// 4 byte uint value indicating Read Mode for Creating NamedPipe
        /// </summary>
        public const uint GENERIC_READ = 0x80000000;
        /// <summary>
        /// 4 byte uint value indicating Write Mode for Creating NamedPipe
        /// </summary>
        public const uint GENERIC_WRITE = 0x40000000;
        /// <summary>
        /// Constant Parameter used for 
        /// creating a Named Pipe which indicates DWCreationDisposition
        /// </summary>
        public const uint OPEN_EXISTING = 3;
        /// <summary>
        /// Constant Parameter used for creating 
        /// a Named Pipe which indicates Flag Parameter
        /// </summary>
        public const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        /// <summary>
        /// Constant Parameter indicating the Buffer Size of NamedPipe.
        /// </summary>
        public const int BUFFER_SIZE = 0x4000;

        #endregion

        #region Win32 API declaration

        /// <summary>
        /// Win32 API call to CreateFile
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeFileHandle CreateFile(
           String pipeName,              // pipe name 
           uint dwDesiredAccess,         // read and write access 
           uint dwShareMode,             // no sharing 
           IntPtr lpSecurityAttributes,  // default security attributes
           uint dwCreationDisposition,   // opens existing pipe
           uint dwFlagsAndAttributes,    // default attributes
           IntPtr hTemplate);            // no template file

        /// <summary>
        /// Win32 API call to Read/Write in NamedPipe
        /// </summary>
        [DllImport
            ("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool TransactNamedPipe(
           SafeFileHandle handle,  //IntPtr hNamedPipe,       // pipe handle 
            byte[] lpInBuffer,       //message to server
            uint nInBufferSize,      // size of read buffer 
            [Out] byte[] lpOutBuffer,// buffer to receive reply
          uint nOutBufferSize,      // size of read buffer 
            out int lpBytesRead,//ref int lpBytesRead,      // bytes read
            IntPtr lpOverlapped);


        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">path to open a PIPE 
        /// communication channel</param>
        public RequestSender(string path)
        {
            this.pipeName = path;
            // Open the PIPE
            handle = CreateFile
                (pipeName, GENERIC_READ | GENERIC_WRITE,
                0, IntPtr.Zero, OPEN_EXISTING,
                     FILE_FLAG_OVERLAPPED,
                     IntPtr.Zero);
            if (handle.IsInvalid)
                throw new
                    InvalidProgramException("Could not create the Handle");

        }

        /// <summary>
        /// Sends the Message to the named PIPE 
        /// and obtains the response on a buffer
        /// </summary>
        /// <param name="messageBLOB">Message sent across the pipe</param>
        /// <param name="readBuffer">Buffer Read</param>
        /// <returns>Number of bytes read</returns>
        public int SendMessage(byte[] messageBLOB, out byte[] readBuffer)
        {
            int bufferRead = -1;
            readBuffer = null;
            // If handle is valid Send the message through the Pipe
            if (!handle.IsInvalid)
            {
                readBuffer = new byte[BUFFER_SIZE];
                TransactNamedPipe
                    (handle,
                    messageBLOB,
                    (uint)messageBLOB.Length,
                    readBuffer, BUFFER_SIZE, out bufferRead, IntPtr.Zero);
            }
            return bufferRead;
        }
    }
}

