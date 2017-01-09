// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.Swn;

namespace SwnSample
{
    class Program
    {
        static void PrintNotification(RESP_ASYNC_NOTIFY respNotify)
        {
            if ((SwnMessageType)respNotify.MessageType == SwnMessageType.RESOURCE_CHANGE_NOTIFICATION)
            {
                Console.WriteLine("MessageType: SWN_SERVER_MESSAGE_RESOURCE_CHANGE_NOTIFICATION");

                RESOURCE_CHANGE[] resourceChangeList;
                SwnUtility.ParseResourceChange(respNotify, out resourceChangeList);

                foreach (var res in resourceChangeList)
                {
                    Console.WriteLine("Resource name: {0}", res.ResourceName);
                    switch ((SwnResourceChangeType)res.ChangeType)
                    {
                        case SwnResourceChangeType.RESOURCE_STATE_UNKNOWN:
                            Console.WriteLine("Change type: RESOURCE_STATE_UNKNOWN");
                            break;
                        case SwnResourceChangeType.RESOURCE_STATE_AVAILABLE:
                            Console.WriteLine("Change type: RESOURCE_STATE_AVAILABLE");
                            break;
                        case SwnResourceChangeType.RESOURCE_STATE_UNAVAILABLE:
                            Console.WriteLine("Change type: SWN_RESOURCE_STATE_UNAVAILABLE");
                            break;
                        default:
                            Console.WriteLine("Change type: Unknown type {0}", res.ChangeType);
                            break;
                    }
                }
            }
            else if ((SwnMessageType)respNotify.MessageType == SwnMessageType.RESOURCE_MOVE_NOTIFICATION)
            {
                Console.WriteLine("MessageType: SWN_SERVER_MESSAGE_RESOURCE_MOVE_NOTIFICATION");

                MOVE_REQUEST moveRequest;
                SwnUtility.ParseMoveRequest(respNotify, out moveRequest);

                foreach (var ip in moveRequest.IPAddrList)
                {
                    Console.WriteLine("---------------------------------");
                    if (((uint)SwnIPAddrType.MOVE_DST_IPADDR_V4 & ip.Flags) != 0)
                    {
                        Console.WriteLine("IPAddr V4: {0}", ip.IPV4);
                    }
                    if (((uint)SwnIPAddrType.MOVE_DST_IPADDR_V6 & ip.Flags) != 0)
                    {
                        Console.WriteLine("IPAddr V6: {0}", ip.IPV6);
                    }
                }
            }
            else
            {
                Console.WriteLine("MessageType: Unknown type: {0}", respNotify.MessageType);
            }
        }

        static void Main(string[] args)
        {
            SwnClient client = new SwnClient();
            string serverName = "GeneralFS";
            string serverAddr = "192.168.1.200";
            string resourceName = "GeneralFS";
            string clientName = Guid.NewGuid().ToString();

            int retVar = 0;
            TimeSpan timeOut = new TimeSpan(0, 0, 10);
            AccountCredential accountCredential = new AccountCredential("contoso.com", "Administrator", "Password01!");
            NlmpClientCredential nlmpCredential = new NlmpClientCredential(serverName, "contoso.com", "Administrator", "Password01!");
            ClientSecurityContext securityContext = new NlmpClientSecurityContext(nlmpCredential);

            //Bind to server
            client.SwnBind(serverName, accountCredential, securityContext, 
                RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY, timeOut);

            //Get interface list
            WITNESS_INTERFACE_LIST interfaceList;
            try
            {
                retVar = client.WitnessGetInterfaceList(out interfaceList);
                Console.WriteLine("Call WitnessGetInterfaceList: " + retVar);
            }
            catch (TimeoutException)
            {
                client.SwnUnbind(timeOut);
                return;
            }

            string swnServerName = "";

            foreach (var info in interfaceList.InterfaceInfo)
            {
                if ((info.Flags & (uint)SwnNodeFlagsValue.INTERFACE_WITNESS) != 0)
                {
                    if ((info.Flags & (uint)SwnNodeFlagsValue.IPv4) != 0)
                    {
                        swnServerName = (new IPAddress(info.IPV4)).ToString();
                    }
                    else if ((info.Flags & (uint)SwnNodeFlagsValue.IPv6) != 0)
                    {
                        byte[] ipv6 = new byte[16];
                        Buffer.BlockCopy(info.IPV6, 0, ipv6, 0, 16);
                        swnServerName = (new IPAddress(ipv6)).ToString();
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                    break;
                }
            }

            SwnClient client2 = new SwnClient();
            client2.SwnBind(swnServerName, accountCredential, securityContext, 
                RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY, timeOut);

            //Registration
            IntPtr pContext;
            retVar = client2.WitnessRegister(SwnVersion.SWN_VERSION_1, resourceName, serverAddr, clientName, out pContext);
            Console.WriteLine("Call WitnessRegister: " + retVar);
            
            uint callId = 0;
            try
            {
                RESP_ASYNC_NOTIFY respNotify;
                callId = client2.WitnessAsyncNotify(pContext);
                Console.WriteLine("Call WitnessAsyncNotify: " + callId);

                retVar = client2.WitnessAsyncNotifyExpect(callId, out respNotify);
                Console.WriteLine("Call WitnessAsyncNotify: " + retVar);
                Console.WriteLine("NumberOfMessages: " + respNotify.NumberOfMessages);
                Console.WriteLine("Length: " + respNotify.Length);
                PrintNotification(respNotify);
             
                callId = client2.WitnessAsyncNotify(pContext);
                Console.WriteLine("Call WitnessAsyncNotify: " + callId);
             
                retVar = client2.WitnessAsyncNotifyExpect(callId, out respNotify);
                Console.WriteLine("Call WitnessAsyncNotify: " + retVar);
                Console.WriteLine("NumberOfMessages: " + respNotify.NumberOfMessages);
                Console.WriteLine("Length: " + respNotify.Length);
                PrintNotification(respNotify);
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Throw a TimeoutException.");
            }

            //UnRegistration
            retVar = client2.WitnessUnRegister(pContext);
            Console.WriteLine("Call WitnessUnRegister: " + retVar);
            
            client2.SwnUnbind(timeOut);
            client.SwnUnbind(timeOut);
            
            Console.ReadKey();
        }
    }
}
