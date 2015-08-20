// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the pool to manage all netbios transport.<para/>
    /// when the first instance is constructed, the netbios is reset.<para/>
    /// while the last instance is disposed, need to reset the netbios:<para/>
    /// this is very important for multiple instance in a process, such as test cases.<para/>
    /// if test case A construct netbios transport first, it will reset the netbios.<para/>
    /// if test case B then construct the netbios transport, it will not reset the netbios.<para/>
    /// but in this while, these two test cases used the same local netbios name.<para/>
    /// so, test case B will failed, with error code: 0x15.<para/>
    /// the solution:<para/>
    /// 1. need a static member to store all instance, such as NetbiosTransport.transports.<para/>
    /// 2. when construct NetbiosTransport instance, add it to transports.<para/>
    /// 3. when dispose instance, if the last instance, reset the hasInitialized to false.
    /// </summary>
    internal static class NetbiosTransportPool
    {
        #region Fields

        /// <summary>
        /// a Dictionary&lt;byte, int&gt; that specifies whether the netbios enviroment has been initialized.<para/>
        /// if true, do not need to reset the netbios;
        /// otherwise, reset the netbios, all registered name are invalid.
        /// </summary>
        private static Dictionary<byte, int> hasInitialized = new Dictionary<byte, int>();

        /// <summary>
        /// an object that specifies the global locker.
        /// </summary>
        private static object netbiosEnvLocker = new object();

        #endregion

        #region Methods

        /// <summary>
        /// initialize the netbios transport pool.
        /// </summary>
        /// <param name="transport">
        /// a NetbiosTransport object that is used to initialize the pool.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when transport is null.
        /// </exception>
        public static void Initialize(NetbiosTransport transport)
        {
            if (transport == null)
            {
                throw new ArgumentNullException("transport");
            }

            lock (netbiosEnvLocker)
            {
                byte networkAdapterId = transport.NetworkAdapterId;

                if (!hasInitialized.ContainsKey(networkAdapterId))
                {
                    transport.ResetAdapter();
                    hasInitialized[networkAdapterId] = 0;
                }

                hasInitialized[networkAdapterId]++;

                // try to register name, if failed, do not add this instance to pool.
                transport.RegisterName();
            }
        }


        /// <summary>
        /// remove the transport instance from transports.<para/>
        /// when dispose the last instance, reset the hasInitialized.
        /// </summary>
        /// <param name="transport">
        /// a NetbiosTransport object that is used to remove from pool.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when transport is null.
        /// </exception>
        public static  void Remove(NetbiosTransport transport)
        {
            if (transport == null)
            {
                throw new ArgumentNullException("transport");
            }

            lock (netbiosEnvLocker)
            {
                byte networkAdapterId = transport.NetworkAdapterId;

                if (hasInitialized.ContainsKey(networkAdapterId))
                {
                    hasInitialized[networkAdapterId]--;

                    // the next instance will reset the adapter.
                    if (hasInitialized[networkAdapterId] == 0)
                    {
                        hasInitialized.Remove(networkAdapterId);
                    }
                }

                // unregister the name.
                transport.UnRegisterName();
            }
        }

        #endregion
    }
}
