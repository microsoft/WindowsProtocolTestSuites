// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestSuites.BranchCache.TestSuite;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;

namespace Microsoft.Protocols.TestSuites.BranchCache
{
    public static class TestUtility
    {
        public static byte[] DownloadHTTPFile(string server, string file)
        {
            WebClient client = new WebClient();
            return client.DownloadData(string.Format("http://{0}/{1}", server, file));
        }

        public static byte[] DownloadSMB2File(string server, string share, string file)
        {
            return File.ReadAllBytes(string.Format(@"\\{0}\{1}\{2}", server, share, file));
        }

        public static bool DoUntilSucceed(Func<bool> func, TimeSpan timeout, TimeSpan retryInterval)
        {
            DateTime startTime = DateTime.Now;

            bool result = true;
            do
            {
                result = func();

                if (!result && retryInterval != TimeSpan.Zero)
                    Thread.Sleep(retryInterval);
            } while (!result && DateTime.Now - startTime < timeout);

            if (!result)
                throw new TimeoutException();

            return result;
        }

        public static byte[] GenerateRandomArray(int length)
        {
            Random random = new Random();
            byte[] data = new byte[length];
            random.NextBytes(data);
            return data;
        }
    }
}
