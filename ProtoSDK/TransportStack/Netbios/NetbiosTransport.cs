// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// Wrapper native netbios function, the class can represent one client and multiply server
    /// and one server multiply client.
    /// </summary>
    internal class NetbiosTransport : IDisposable
    {
        #region Fields

        /// <summary>
        /// because listen call must be terminated using cancel. so keep the 
        /// previous listen call ncb point. It does not need to be freed because it just 
        /// keep a copy point of the original one. the original one will be freed in
        /// Netbios(ref NCB) function, so when disposing, it does not need to be freed.
        /// </summary>
        private IntPtr listeningNcbPtr;

        /// <summary>
        /// the locker.
        /// </summary>
        private readonly object listeningNcbLocker = new object();

        /// <summary>
        /// an int value that specifies the listening ncb size.
        /// </summary>
        private int listeningNcbSize;

        /// <summary>
        /// a byte value that specifies the number for the local network name.<para/>
        /// This number is returned by Netbios  after a successful NCBADDNAME or NCBADDGRNAME command.<para/>
        /// This number, not the name, must be used with all datagram commands and for NCBRECVANY commands.
        /// </summary>
        private byte ncbNum;

        /// <summary>
        /// a byte value that specifies the network adapter id.<para/>
        /// its value is initialized by transport pool.
        /// </summary>
        private byte networkAdapterId;

        /// <summary>
        /// a string that specifies the local netbios name.<para/>
        /// its value is initialized by config.<para/>
        /// it's used to invoke the netbios command.
        /// </summary>
        private string localNetbiosName;

        /// <summary>
        /// a byte value that specifies the adapter index.<para/>
        /// when initialize the transport pool, it's used to set the networkAdapterId.
        /// </summary>
        private byte adapterIndex;

        /// <summary>
        /// a ushort value that specifies the max buffer size.<para/>
        /// its value is initialized by config.
        /// </summary>
        private ushort maxBufferSize;

        /// <summary>
        /// a byte value that specifies the max session number.<para/>
        /// when initialize the transport pool, it's used to set the adapter.
        /// </summary>
        private byte maxSessionNum;

        /// <summary>
        /// a byte value that specifies the max names number.<para/>
        /// when initialize the transport pool, it's used to set the adapter.
        /// </summary>
        private byte maxNames;

        #endregion

        #region Propterties

        /// <summary>
        /// get a byte value that specifies the network adapter id.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public byte NetworkAdapterId
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("NetbiosTransport");
                }

                return this.networkAdapterId;
            }
        }


        /// <summary>
        /// get a byte value that specifies the number for the local network name.<para/>
        /// This number is returned by Netbios  after a successful NCBADDNAME or NCBADDGRNAME command.<para/>
        /// This number, not the name, must be used with all datagram commands and for NCBRECVANY commands.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public byte NcbNum
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("NetbiosTransport");
                }

                return this.ncbNum;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="localName">
        /// a string that specifies the local netbios name.<para/>
        /// its value is initialized by config.<para/>
        /// it's used to invoke the netbios command.
        /// </param>
        /// <param name="adapterIndex">
        /// a byte value that specifies the adapter index.<para/>
        /// when initialize the transport pool, it's used to set the networkAdapterId.
        /// </param>
        /// <param name="maxBufferSize">
        /// a ushort value that specifies the max buffer size.<para/>
        /// its value is initialized by config.
        /// </param>
        /// <param name="maxSessionNum">
        /// a byte value that specifies the max session number.<para/>
        /// when initialize the transport pool, it's used to set the adapter.
        /// </param>
        /// <param name="maxNames">
        /// a byte value that specifies the max names number.<para/>
        /// when initialize the transport pool, it's used to set the adapter.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// throw when localName is null.
        /// </exception>
        public NetbiosTransport(
            string localName, byte adapterIndex, ushort maxBufferSize, byte maxSessionNum, byte maxNames)
        {
            if (localName == null)
            {
                throw new ArgumentNullException("localName");
            }

            this.localNetbiosName = localName;
            this.adapterIndex = adapterIndex;
            this.maxBufferSize = maxBufferSize;
            this.maxSessionNum = maxSessionNum;
            this.maxNames = maxNames;

            this.networkAdapterId = this.GetAdapterId();

            NetbiosTransportPool.Initialize(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Connect to remote machine
        /// </summary>
        /// <param name="remoteName">The remote machine bios name</param>
        /// <returns>The session id</returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// throw when connect fails.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when remoteName is null.
        /// </exception>
        public int Connect(string remoteName)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosTransport");
            }

            if (remoteName == null)
            {
                throw new ArgumentNullException("remoteName");
            }

            NCB ncb = new NCB();

            try
            {
                NetbiosUtility.InitNcb(ref ncb);
                ncb.ncb_command = (byte)NcbCommand.NCBCALL;
                ncb.ncb_lana_num = networkAdapterId;
                ncb.ncb_name = NetbiosUtility.ToNetbiosName(localNetbiosName);
                ncb.ncb_callname = NetbiosUtility.ToNetbiosName(remoteName);

                InvokeNetBios(ref ncb);

                if (ncb.ncb_retcode != (byte)NcbReturnCode.NRC_GOODRET)
                {
                    throw new InvalidOperationException("Failed in NCBCALL command, error is "
                        + ((NcbReturnCode)ncb.ncb_retcode).ToString());
                }
            }
            finally
            {
                NetbiosUtility.FreeNcbNativeFields(ref ncb);
            }

            return ncb.ncb_lsn;
        }


        /// <summary>
        /// Receive data
        /// </summary>
        /// <param name="sessionId">Specified the session identify</param>
        /// <returns>The received data</returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// throw when receive encounters error.
        /// </exception>
        public byte[] Receive(int sessionId)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosTransport");
            }

            NCB ncb = new NCB();

            try
            {
                NetbiosUtility.InitNcb(ref ncb);
                ncb.ncb_command = (byte)NcbCommand.NCBRECV;
                ncb.ncb_lana_num = networkAdapterId;
                ncb.ncb_lsn = (byte)sessionId;
                ncb.ncb_buffer = Marshal.AllocHGlobal(maxBufferSize);
                ncb.ncb_length = maxBufferSize;

                InvokeNetBios(ref ncb);

                if (ncb.ncb_retcode == (byte)NcbReturnCode.NRC_SCLOSED)
                {
                    return null;
                }

                if (ncb.ncb_retcode != (byte)NcbReturnCode.NRC_GOODRET)
                {
                    throw new InvalidOperationException("Failed in NCBRECV command, error is "
                        + ((NcbReturnCode)ncb.ncb_retcode).ToString());
                }

                byte[] receivedData = new byte[ncb.ncb_length];
                Marshal.Copy(ncb.ncb_buffer, receivedData, 0, receivedData.Length);

                return receivedData;
            }
            finally
            {
                NetbiosUtility.FreeNcbNativeFields(ref ncb);
            }
        }


        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="sessionId">The session id</param>
        /// <param name="buffer">The data buffer</param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// throw when the buffer is larger than the max buffer size
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// throw when call native netbios api fails
        /// </exception>
        public void Send(int sessionId, byte[] buffer)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosTransport");
            }

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            NCB ncb = new NCB();

            try
            {
                NetbiosUtility.InitNcb(ref ncb);
                ncb.ncb_command = (byte)NcbCommand.NCBSEND;
                ncb.ncb_buffer = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, ncb.ncb_buffer, buffer.Length);
                ncb.ncb_length = (ushort)buffer.Length;
                ncb.ncb_lana_num = networkAdapterId;
                ncb.ncb_lsn = (byte)sessionId;

                InvokeNetBios(ref ncb);

                if (ncb.ncb_retcode != (byte)NcbReturnCode.NRC_GOODRET)
                {
                    throw new InvalidOperationException("Failed in NCBSEND command, error is "
                        + ((NcbReturnCode)ncb.ncb_retcode).ToString());
                }
            }
            finally
            {
                NetbiosUtility.FreeNcbNativeFields(ref ncb);
            }
        }


        /// <summary>
        /// Disconnect the session
        /// </summary>
        /// <param name="sessionId">The session id</param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// throw when disconnect fails
        /// </exception>
        public void Disconnect(int sessionId)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosTransport");
            }

            NCB ncb = new NCB();

            try
            {
                NetbiosUtility.InitNcb(ref ncb);
                ncb.ncb_command = (byte)NcbCommand.NCBHANGUP;
                ncb.ncb_lana_num = networkAdapterId;
                ncb.ncb_lsn = (byte)sessionId;

                InvokeNetBios(ref ncb);

                //if remote machine disconnect the session first, local call disconnect will return NRC_SNUMOUT,
                //and this is not an error which we need to notify user.
                if ((ncb.ncb_retcode != (byte)NcbReturnCode.NRC_GOODRET)
                    && (ncb.ncb_retcode != (byte)NcbReturnCode.NRC_SNUMOUT)
                    && (ncb.ncb_retcode != (byte)NcbReturnCode.NRC_SCLOSED))
                {
                    throw new InvalidOperationException("Failed in NCBHANGUP command, error is "
                        + ((NcbReturnCode)ncb.ncb_retcode).ToString());
                }
            }
            finally
            {
                NetbiosUtility.FreeNcbNativeFields(ref ncb);
            }
        }


        /// <summary>
        /// Listen to local netbios endpoint
        /// </summary>
        /// <returns>The connected session id</returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// throw when listen fails
        /// </exception>
        public byte Listen()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosTransport");
            }

            NCB ncb = new NCB();

            try
            {
                NetbiosUtility.InitNcb(ref ncb);
                ncb.ncb_command = (byte)NcbCommand.NCBLISTEN;
                ncb.ncb_lana_num = networkAdapterId;
                //* means it can accept any connection.
                ncb.ncb_callname = NetbiosUtility.ToNetbiosName("*");
                ncb.ncb_name = NetbiosUtility.ToNetbiosName(localNetbiosName);

                InvokeNetBios(ref ncb);

                if (ncb.ncb_retcode != (byte)NcbReturnCode.NRC_GOODRET)
                {
                    throw new InvalidOperationException("Failed in NCBLISTEN command, error is "
                        + ((NcbReturnCode)ncb.ncb_retcode).ToString());
                }
            }
            finally
            {
                NetbiosUtility.FreeNcbNativeFields(ref ncb);
            }

            return ncb.ncb_lsn;
        }


        /// <summary>
        /// Cancel the previous pending listen call
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void CancelListen()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosTransport");
            }

            lock (listeningNcbLocker)
            {
                if (listeningNcbPtr == IntPtr.Zero)
                {
                    return;
                }

                NCB ncb = new NCB();

                NetbiosUtility.InitNcb(ref ncb);
                ncb.ncb_command = (byte)NcbCommand.NCBCANCEL;
                ncb.ncb_buffer = listeningNcbPtr;
                ncb.ncb_length = (ushort)listeningNcbSize;
                ncb.ncb_lana_num = networkAdapterId;

                InvokeNetBios(ref ncb);
            }
        }


        /// <summary>
        /// Reset the adapter, it will clear all registered names, and reset the maxsession,
        /// maxName
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// throw when reset adapter fails
        /// </exception>
        public void ResetAdapter()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosTransport");
            }

            NCB ncb = new NCB();

            try
            {
                NetbiosUtility.InitNcb(ref ncb);
                ncb.ncb_command = (byte)NcbCommand.NCBRESET;
                ncb.ncb_lana_num = networkAdapterId;
                Marshal.WriteByte(ncb.ncb_callname, 0, this.maxSessionNum);
                Marshal.WriteByte(ncb.ncb_callname, 2, this.maxNames);

                InvokeNetBios(ref ncb);

                if (ncb.ncb_retcode != (byte)NcbReturnCode.NRC_GOODRET)
                {
                    throw new InvalidOperationException("Failed in NCBRESET command, error is "
                        + ((NcbReturnCode)ncb.ncb_retcode).ToString());
                }
            }
            finally
            {
                NetbiosUtility.FreeNcbNativeFields(ref ncb);
            }
        }


        /// <summary>
        /// Get the adapter id
        /// </summary>
        /// <returns>The adapter id</returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// throw when Get adapterId fails
        /// </exception>
        public byte GetAdapterId()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosTransport");
            }

            NCB ncb = new NCB();

            try
            {
                NetbiosUtility.InitNcb(ref ncb);
                ncb.ncb_command = (byte)NcbCommand.NCBENUM;
                ncb.ncb_buffer = Marshal.AllocHGlobal(maxBufferSize);
                ncb.ncb_length = maxBufferSize;

                InvokeNetBios(ref ncb);

                if (ncb.ncb_retcode != (byte)NcbReturnCode.NRC_GOODRET)
                {
                    throw new InvalidOperationException("Failed in NCBENUM command, error is "
                        + ((NcbReturnCode)ncb.ncb_retcode).ToString());
                }

                LANA_ENUM lenum = new LANA_ENUM();
                lenum.length = Marshal.ReadByte(ncb.ncb_buffer, 0);
                lenum.lanaNum = new byte[lenum.length];

                for (int i = 0; i < lenum.length; i++)
                {
                    lenum.lanaNum[i] = Marshal.ReadByte(ncb.ncb_buffer, i + 1);
                }

                return lenum.lanaNum[adapterIndex];
            }
            finally
            {
                NetbiosUtility.FreeNcbNativeFields(ref ncb);
            }
        }


        /// <summary>
        /// Register bios name for further call
        /// </summary>
        /// <returns>The name index</returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// throw when register name fails
        /// </exception>
        public byte RegisterName()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosTransport");
            }

            NCB ncb = new NCB();

            try
            {
                NetbiosUtility.InitNcb(ref ncb);
                ncb.ncb_command = (byte)NcbCommand.NCBADDNAME;
                ncb.ncb_lana_num = networkAdapterId;
                ncb.ncb_name = NetbiosUtility.ToNetbiosName(localNetbiosName);

                InvokeNetBios(ref ncb);

                if (ncb.ncb_retcode != (byte)NcbReturnCode.NRC_GOODRET)
                {
                    throw new InvalidOperationException("Failed in NCBADDNAME command, error is "
                        + ((NcbReturnCode)ncb.ncb_retcode).ToString());
                }

                this.ncbNum = ncb.ncb_num;
                return ncb.ncb_num;
            }
            finally
            {
                NetbiosUtility.FreeNcbNativeFields(ref ncb);
            }
        }


        /// <summary>
        /// Unregister the bios name
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void UnRegisterName()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosTransport");
            }

            // if the local name in .ctr is null, exception is thrown,
            // the gc will dispose the object and this method will be invoked.
            if (this.localNetbiosName == null)
            {
                return;
            }

            NCB ncb = new NCB();

            try
            {
                NetbiosUtility.InitNcb(ref ncb);
                ncb.ncb_command = (byte)NcbCommand.NCBDELNAME;
                ncb.ncb_lana_num = networkAdapterId;
                ncb.ncb_num = ncbNum;
                ncb.ncb_name = NetbiosUtility.ToNetbiosName(localNetbiosName);

                InvokeNetBios(ref ncb);
            }
            finally
            {
                NetbiosUtility.FreeNcbNativeFields(ref ncb);
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// the dispose flags 
        /// </summary>
        private bool disposed;

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
        /// <param name = "disposing">
        /// If disposing equals true, Managed and unmanaged resources are disposed. if false, Only unmanaged resources 
        /// can be disposed. 
        /// </param>
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
                // If disposing is false, only the following code is executed:
                // remote the instance from pool.
                NetbiosTransportPool.Remove(this);

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer 
        /// </summary>
        ~NetbiosTransport()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Call native Netbios interface
        /// </summary>
        /// <param name="ncb">The NCB structure</param>
        private void InvokeNetBios(ref NCB ncb)
        {
            IntPtr ncbPtr = IntPtr.Zero;
            int ncbSize = 0;

            try
            {
                lock (listeningNcbLocker)
                {
                    ncbPtr = NetbiosUtility.MarshalNcb(ref ncb, out ncbSize);

                    if (ncb.ncb_command == (byte)NcbCommand.NCBLISTEN)
                    {
                        listeningNcbPtr = ncbPtr;
                        listeningNcbSize = ncbSize;
                    }
                }
              
                NetbiosNativeMethods.Netbios(ncbPtr);

                ncb = NetbiosUtility.UnMarshalNcb(ncbPtr, ncbSize);
            }
            finally
            {
                lock (listeningNcbLocker)
                {
                    Marshal.FreeHGlobal(ncbPtr);

                    if (ncb.ncb_command == (byte)NcbCommand.NCBLISTEN)
                    {
                        listeningNcbPtr = IntPtr.Zero;
                        listeningNcbSize = 0;
                    }
                }
            }
        }

        #endregion
    }
}
