// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc
{
    public class DataFragmentManager
    {
        public uint Length { get; private set; }

        private MemoryStream buffer = null;

        public DataFragmentManager()
        {
            Length = 0;
            Initialized = false;
            Completed = false;
        }

        public void AddFirstData(uint length, byte[] data)
        {
            Initialized = true;
            Length = length;
            //TODO: need to handle huge buffer larger than int.Max
            buffer = new MemoryStream(Convert.ToInt32(length));
            buffer.Write(data, 0, data.Length);

            SimpleLogger.Log("DataFragmentManager: Add first data, length: {0}, total: {1}", data.Length, Length);

            Completed = (data.LongLength == buffer.Length);
        }

        public void AppendData(byte[] data)
        {
            if (null == buffer)
            {
                DynamicVCException.Throw("Haven't added the first buffer yet.");
                return;
            }

            int len = data.Length;
            SimpleLogger.Log("DataFragmentManager: Append a data fragment, length: {0}, total: {1}", len, Length);

            long expected = (long)Length - buffer.Length;

            if ((long)len > expected)
            {
                DynamicVCException.Throw("The length of the data is larger than expected.");
            }
            buffer.Write(data, 0, len);

            if (0 == expected)
            {
                Completed = true;
            }
        }

        public byte[] Data
        {
            get
            {
                return buffer.ToArray();
            }
        }

        public bool Completed { get; private set; }

        public bool Initialized { get; private set; }
    }
}
