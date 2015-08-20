// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// thread manager is a manager for block/unblock thread, especially for block thread.<para/>
    /// 1. thread is constructed and put into thread manager,<para/>
    /// 2. start the thread. the thread maybe blocked, such as Socket.Receive(),<para/>
    /// 3. when the user want to exit, invoke the Stop/Dispose of thread manager, it will:<para/>
    ///     a. set the ThreadExitEvent<para/>
    ///     b. invoke the Unblock() to unblock the blocked thread,<para/>
    ///     c. wait for the thread to exit.
    /// </summary>
    internal class ThreadManager : IDisposable
    {
        /// <summary>
        /// a delegate that is used to unblock the blocked thread.<para/>
        /// this function maybe invoked when thread is start, maybe the thread is not blocked.<para/>
        /// for example, when the received thread of TcpClient is blocked at Socket.Receive()<para/>
        /// the ThreadManager will first set the ExitEvent,
        /// then invoke this delegate to close the socket,
        /// and wait for thread to exit<para/>
        /// the thread will unblock from the Socket.Receive() then check the ExitEvent
        /// </summary>
        public delegate void UnblockFunction();

        #region Fields

        /// <summary>
        /// a Thread object that indicates the managed thread.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// a ManualResetEvent object that represent the exit event for thread<para/>
        /// if the event is set, the thread need to cleanup itself and exit the thread function.
        /// </summary>
        private ManualResetEvent threadExitEvent;

        /// <summary>
        /// a delegate that is used to unblock the blocked thread.<para/>
        /// for example, when the received thread of TcpClient is blocked at Socket.Receive()<para/>
        /// the ThreadManager will first set the ExitEvent,
        /// then invoke this delegate to close the socket,
        /// and wait for thread to exit<para/>
        /// the thread will unblock from the Socket.Receive() then check the ExitEvent
        /// </summary>
        private UnblockFunction unblockFunction;

        #endregion

        #region Properties

        /// <summary>
        /// get a bool value that indicates that whether thread should exit.<para/>
        /// if true, thread must cleanup and exit; otherwise, thread should not exit.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public bool ShouldExit
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("ThreadManager");
                }

                return this.threadExitEvent.WaitOne(0, false);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        protected ThreadManager()
        {
            this.threadExitEvent = new ManualResetEvent(false);
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="threadStart">
        /// a ThreadStart delegate that represent the unblock thread<para/>
        /// unblock means that when exit the thread, need not to unblock,
        /// just need to wait for the ThreadExitEvent.
        /// </param>
        public ThreadManager(ThreadStart threadStart)
            : this()
        {
            this.thread = new Thread(threadStart);

            if (threadStart != null && threadStart.Method != null)
            {
                this.thread.Name = threadStart.Method.Name;
            }
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="threadStart">
        /// a ThreadStart delegate that represent the block thread<para/>
        /// when stop thread, the unblockFunction will be invoked, then trigger the exit event.
        /// thread unblock and cleanup itself.
        /// </param>
        /// <param name="unblockFunction">
        /// a delegate that is used to unblock the blocked thread.<para/>
        /// for example, when the received thread of TcpClient is blocked at Socket.Receive()<para/>
        /// the ThreadManager will first set the ExitEvent,
        /// then invoke this delegate to close the socket,
        /// and wait for thread to exit<para/>
        /// the thread will unblock from the Socket.Receive() then check the ExitEvent
        /// </param>
        public ThreadManager(ThreadStart threadStart, UnblockFunction unblockFunction)
            : this(threadStart)
        {
            this.unblockFunction = unblockFunction;
        }

        #endregion

        #region Methods

        /// <summary>
        /// start the managed thread.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void Start()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("ThreadManager");
            }

            if (this.thread == null)
            {
                return;
            }

            this.thread.Start();
        }


        /// <summary>
        /// stop the thread. it will:<para/>
        /// a. set the ThreadExitEvent
        /// b. invoke the Unblock() to unblock the blocked thread,
        /// c. wait for the thread to exit.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void Stop()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("ThreadManager");
            }

            if (this.thread == null)
            {
                return;
            }

            // notify the thread, it should cleanup and exit.
            this.threadExitEvent.Set();

            // if the thread is blocked, unblock it.
            // otherwise, donot block it and just wait for it to exit.
            if (this.unblockFunction != null)
            {
                this.unblockFunction();
            }

            // wait for thread to cleanup.
            if (this.thread.ThreadState != ThreadState.Unstarted)
            {
                this.thread.Join();
            }

            // set the thread to null, it cannot stop twice.
            this.thread = null;
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
                    if (this.thread != null)
                    {
                        this.Stop();
                        this.thread = null;
                    }
                    if (this.threadExitEvent != null)
                    {
                        this.threadExitEvent.Close();
                        this.threadExitEvent = null;
                    }
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed:

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer 
        /// </summary>
        ~ThreadManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
