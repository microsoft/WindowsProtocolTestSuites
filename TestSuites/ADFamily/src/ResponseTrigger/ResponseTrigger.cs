// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Microsoft.Protocols.TestSuites.AdtsPublishDcScenario
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("command format: NonEXResponseTrigger.exe DCName/IP DomainName ex/nonex");
                Console.WriteLine("i.e: NonEXResponseTrigger.exe 192.168.0.1 contoso.com nonex");
            }
            else
            {
                byte[] domain = Encoding.ASCII.GetBytes(args[1]);
                // LDAP uses port 389
                int ldapPort = 389;
                int offset = 0;
                TcpClient client = new TcpClient(args[0], ldapPort);
                NetworkStream stream = client.GetStream();

                // Here common1-6 stand for the default part in this request like Attribute, NtVer Filter,Scope, baseObject
                // They are static in the dynamic network environment
                // Length1-4 stand for length of their contents; only affected by the length of the current DomainName.
                // 86,77,38,13 stand for the length of the default part in this request.
                byte[] common1 = new byte[] { 0x30, 0x84, 0x00, 0x00, 0x00 };
                byte length1 = (byte)(86 + domain.Length);
                byte[] common2 = new byte[] { 0x02, 0x01, 0x04, 0x63, 0x84, 0x00, 0x00, 0x00 };
                byte length2 = (byte)(77 + domain.Length);
                byte[] common3 = new byte[] { 0x04, 0x00, 0x0A, 0x01, 0x00, 0x0A, 0x01, 0x00, 0x02, 0x01, 0x00, 0x02, 0x01, 
                    0x00, 0x01, 0x01, 0x00, 0xA0, 0x84, 0x00, 0x00, 0x00 };
                byte length3 = (byte)(38 + domain.Length);
                byte[] common4 = new byte[] { 0xA3, 0x84, 0x00, 0x00, 0x00 };
                byte length4 = (byte)(13 + domain.Length);
                byte[] common5 = new byte[] { 0x04, 0x09, 0x44, 0x6E, 0x73, 0x64, 0x6F, 0x6D, 0x61, 0x69, 0x6E, 0x04 };
                byte domainNameLength = (byte)domain.Length;
                byte[] common6;

                if ("nonex" == args[2])
                {
                    common6 = new byte[] { 0xA3, 0x84, 0x00, 0x00, 0x00, 0x0D, 0x04, 0x05, 0x4E, 0x74, 0x56, 0x65, 0x72,
                        0x04, 0x04, 0x02, 0x00, 0x00, 0x10, 0x30, 0x84, 0x00, 0x00, 0x00, 0x0A, 0x04, 0x08, 0x6E, 0x65, 
                        0x74, 0x6C, 0x6F, 0x67, 0x6F, 0x6E };
                }
                else
                {
                    common6 = new byte[] { 0xA3, 0x84, 0x00, 0x00, 0x00, 0x0D, 0x04, 0x05, 0x4E, 0x74, 0x56, 0x65, 0x72,
                        0x04, 0x04, 0x0c, 0x00, 0x00, 0x00, 0x30, 0x84, 0x00, 0x00, 0x00, 0x0A, 0x04, 0x08, 0x6E, 0x65, 
                        0x74, 0x6C, 0x6F, 0x67, 0x6F, 0x6E };
                }

                byte[] request = new byte[common1.Length + 1 + common2.Length + 1 + common3.Length + 1 + common4.Length 
                    + 1 + common5.Length + 1 + domain.Length + common6.Length];

                for (int i = 0; i < common1.Length; i++)
                {
                    request[offset] = common1[i];
                    offset++;
                }

                request[offset] = length1;
                offset++;

                for (int i = 0; i < common2.Length; i++)
                {
                    request[offset] = common2[i];
                    offset++;
                }

                request[offset] = length2;
                offset++;

                for (int i = 0; i < common3.Length; i++)
                {
                    request[offset] = common3[i];
                    offset++;
                }

                request[offset] = length3;
                offset++;

                for (int i = 0; i < common4.Length; i++)
                {
                    request[offset] = common4[i];
                    offset++;
                }

                request[offset] = length4;
                offset++;

                for (int i = 0; i < common5.Length; i++)
                {
                    request[offset] = common5[i];
                    offset++;
                }

                // Add DomainName into packet
                request[offset] = domainNameLength;
                offset++;

                for (int i = 0; i < domain.Length; i++)
                {
                    request[offset] = domain[i];
                    offset++;
                }

                for (int i = 0; i < common6.Length; i++)
                {
                    request[offset] = common6[i];
                    offset++;
                }

                stream.Write(request, 0, request.Length);
                request = new byte[4096];
                stream.Read(request, 0, request.Length);
            }
        }
    }
}
