// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    public class FakeServerMain
    {
        public static void Main()
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("ADFS_FakeServer");
            if (processes != null && processes.Length > 1)
            {
                Console.WriteLine("One instance already running");
            }


            #region try to start
            try
            {
                Processor p = new Processor();
                p.Initialize();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to start. Reason: " + (e.Message == null ? "" : e.Message));
                return;
            }

            //Console.WriteLine("Started successfully");
            #endregion


            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
