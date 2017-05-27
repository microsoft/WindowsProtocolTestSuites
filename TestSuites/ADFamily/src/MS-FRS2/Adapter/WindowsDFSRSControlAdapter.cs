// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace Microsoft.Protocols.TestSuites.MS_FRS2
{
    public static class WindowsDFSRSControlAdapter
    {
        public static Microsoft.Protocols.TestTools.ITestSite Site { set; get; }
        public static void StopDFSRS(string computer)
        {
            try
            {
                Site.Log.Add(TestTools.LogEntryKind.Checkpoint, "Will stop DFSR service on {0}", computer);
                using (ServiceController sc = new ServiceController("DFS Replication", computer))
                {
                    if (sc.Status != ServiceControllerStatus.Stopped)
                    {
                        sc.Stop();
                        Site.Log.Add(TestTools.LogEntryKind.Checkpoint, "Action invoked");
                        System.Threading.Thread.Sleep(2000);
                        do
                        {
                            sc.Refresh();
                            if (sc.Status != ServiceControllerStatus.Stopped)
                            {
                                Site.Log.Add(TestTools.LogEntryKind.Checkpoint, "not stopped, sleeping");
                                System.Threading.Thread.Sleep(10000);
                            }
                        }
                        while (sc.Status != ServiceControllerStatus.Stopped);
                    }
                }
            }
            catch
            { }
        }

        public static void StartDFSRS(string computer)
        {
            try
            {
                Site.Log.Add(TestTools.LogEntryKind.Checkpoint, "Will start DFSR service on {0}", computer);
                using (ServiceController sc = new ServiceController("DFS Replication", computer))
                {
                    if (sc.Status != ServiceControllerStatus.Running)
                        sc.Start();
                    Site.Log.Add(TestTools.LogEntryKind.Checkpoint, "Action invoked");
                    System.Threading.Thread.Sleep(2000);
                    do
                    {
                        sc.Refresh();
                        if (sc.Status != ServiceControllerStatus.Running)
                        {
                            Site.Log.Add(TestTools.LogEntryKind.Checkpoint, "not running, sleeping");
                            System.Threading.Thread.Sleep(10000);
                        }
                    }
                    while (sc.Status != ServiceControllerStatus.Running);
                }
            }
            catch
            {
            }
        }
    }
}
