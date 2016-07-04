// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc
{
    [Serializable]
    public class DynamicVCException : Exception
    {
        private DynamicVCException() { }
        private DynamicVCException(string message) : base(message) { }
        private DynamicVCException(string message, Exception inner) : base(message, inner) { }
        protected DynamicVCException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }


        private static int mainThreadId = -1;
        private static bool cleanupThread = false;
        private static DynamicVCException unprocessedException = null;
        public static void SetMainThread()
        {
            if (mainThreadId == -1)
            {
                mainThreadId = Thread.CurrentThread.ManagedThreadId;
            }
        }

        /// <summary>
        /// Whether the current main thread is Test Clean Up thread
        /// </summary>
        /// <param name="cleanup"></param>
        public static void SetCleanUp(bool cleanup)
        {
            cleanupThread = cleanup;
        }

        public static void ProcessException()
        {
            if (!IsMainThread())
            {
                return;
            }

            if (unprocessedException != null)
            {
                throw unprocessedException;
            }
        }

        private static bool IsMainThread()
        {
            return (Thread.CurrentThread.ManagedThreadId == mainThreadId);
        }

        private static void HandleException(DynamicVCException e)
        {
            if (IsMainThread() && (!cleanupThread))
            {
                // Not work for protocol based adapter Need more investigation
                // throw e;
            }
            else if (unprocessedException == null)
            {
                unprocessedException = e;
            }
        }

        public static void Throw(Exception inner)
        {
            DynamicVCException e = new DynamicVCException("Check inner exception for more details.", inner);

            HandleException(e);
        }

        public static void Throw(string message)
        {
            DynamicVCException e = new DynamicVCException(message);

            HandleException(e);
        }

        public static void Throw(string message, Exception inner)
        {
            DynamicVCException e = new DynamicVCException(message, inner);

            HandleException(e);
        }
    }

    /// <summary>
    /// Exception type for BasePDU decoding failure.
    /// </summary>
    public class PDUDecodeException : Exception
    {
        byte[] decodingData;
        Type decodingType;

        public PDUDecodeException(Type targetType, byte[] data)
        {
            decodingType = targetType;
            decodingData = data;
        }

        /// <summary>
        /// The data to be decoded.
        /// </summary>
        public byte[] DecodingData
        {
            get
            {
                return decodingData;
            }
            set
            {
                decodingData = value;
            }
        }

        /// <summary>
        /// The type to be decoded to.
        /// </summary>
        public Type DecodingType
        {
            get
            {
                return decodingType;
            }
            set
            {
                decodingType = value;
            }
        }

        public override string ToString()
        {
            string msg = String.Format("Failed to decode to {0}", decodingType.Name);
            return msg;
        }
    }
}
