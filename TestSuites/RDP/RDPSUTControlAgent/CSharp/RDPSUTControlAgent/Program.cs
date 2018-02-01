// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDPSUTControlAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            SUTControlListener listener = new SUTControlListener();

            listener.Start();
            Console.WriteLine("Start listening");

            Console.Read();

            listener.Stop();
            Console.WriteLine("Stop listening");
        }
    }
}
