// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace RDPSUTControlAgent
{
    class Program
    {
        private static Dictionary<string, string> _param = new Dictionary<string, string>();
        private static ushort listenPort = 4488;

        static void Main(string[] args)
        {
            foreach (var s in args)
            {
                if (s.StartsWith("/"))
                {
                    int index = s.IndexOf(':');
                    if (index > 0)
                    {
                        _param[s.Substring(1, index - 1).Trim().ToLower()] = s.Substring(index + 1);
                    }
                }
            }
            if (_param.Count > 0)
            {
                string[] portOption = { "port", "p" };
                string portValue = GetValue(portOption);
                try
                {
                    listenPort = Convert.ToUInt16(portValue);
                }
                catch
                {
                    Console.WriteLine("Invalid number format for port:{0}", portValue);
                    return;
                }
            }            

            SUTControlListener listener = new SUTControlListener(listenPort);

            listener.Start();
            Console.WriteLine("Start listening on port {0}", listenPort);

            Console.Read();
            
            listener.Stop();
            Console.WriteLine("Stop listening");
        }

        private static string GetValue(string[] sKey)
        {
            foreach(var cKey in sKey)
            {
                if (_param.ContainsKey(cKey))
                {
                    return _param[cKey];
                }
            }
            return null;
        }
    }
}
