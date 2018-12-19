// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestSuites;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    /// <summary>
    /// A singleton Handler shared by all protocol-based SUT control adapter
    /// </summary>
    public class SUTControlProtocolHandler
    {
        #region variables

        ITestSite Site;
        // Agent list: server list for SUT control protocols
        List<IPEndPoint> AgentList;
        ISUTControltransport transport;
        TimeSpan timeout;
        ushort requestId;
        // whether always need response
        bool alwaysNeedResponse;

        private static SUTControlProtocolHandler context = null;

        #endregion variables

        #region Property

        /// <summary>
        /// Whether using TCP transport
        /// </summary>
        public bool IsUsingTCP
        {
            get
            {
                return (transport is TCPSUTControlTransport);
            }
        }
        #endregion Property

        #region Constructor
        /// <summary>
        /// private Constructor
        /// </summary>
        /// <param name="testSite"></param>
        private SUTControlProtocolHandler(ITestSite testSite)
        {
            this.Site = testSite;

            // Initiate request id
            requestId = 1;

            // Initiate transport
            string transportType = testSite.Properties["SUTControl.TransportType"];
            if (transportType == null)
            {
                transportType = "TCP";
            }
            if (transportType.Equals("TCP"))
            {
                transport = new TCPSUTControlTransport();
            }
            else
            {
                transport = new UDPSUTControlTransport();
            }

            // Initiate timeout time span
            timeout = new TimeSpan(0, 0, 20);
            
            // Initiate Connect payload type
            bool clientSupportRDPFile = false;
            try
            {
                clientSupportRDPFile = bool.Parse(testSite.Properties["SUTControl.ClientSupportRDPFile"]);
            }
            catch
            {
            }
            
            // Initiate alwaysNeedResponse
            alwaysNeedResponse = true;
            try
            {
                alwaysNeedResponse = bool.Parse(testSite.Properties["SUTControl.AlwaysNeedResponse"]);
            }
            catch
            {
            }

            // Get Agent addresses
            string addresses = testSite.Properties["SUTControl.AgentAddress"];
            string[] addressList = addresses.Split(';');
            AgentList = new List<IPEndPoint>();
            foreach (string address in addressList)
            {
                try
                {
                    if (address != null && address.Trim().Length > 0)
                    {
                        int separator = address.IndexOf(':');
                        //Consider SUTControl.AgentAddress may be SUT hostname, or IP addresses
                        List<IPAddress> addList = CommonUtility.GetHostIPs(this.Site, address.Substring(0, separator)); 
                        string port = address.Substring(separator + 1).Trim();
                        foreach(IPAddress add in addList)
                        {
                            IPEndPoint endpoint = new IPEndPoint(add, int.Parse(port));
                            AgentList.Add(endpoint);
                        }                        
                    }
                }
                catch (Exception e)
                {
                    this.Site.Log.Add(LogEntryKind.Comment, "RDP SUT Control Protocol Adapter: Invalid Agent IP address/host name string or port number:" + e.Message);
                }
            }
        }
        #endregion Constructor

        #region public methods

        /// <summary>
        /// Static method to get a SUTControlProtocolHandler instance
        /// </summary>
        /// <param name="testSite"></param>
        /// <returns></returns>
        public static SUTControlProtocolHandler GetInstance(ITestSite testSite)
        {
            if (context == null)
            {
                context = new SUTControlProtocolHandler(testSite);
            }
            return context;
        }
        
        /// <summary>
        /// Get Next requestId
        /// </summary>
        /// <returns>RequestId</returns>
        public ushort GetNextRequestId()
        {
            return requestId++;
        }

        /// <summary>
        /// Send SUT control request message to SUT agent and get the response is necessary
        /// </summary>
        /// <param name="requestMessage">SUT Control Request Message</param>
        /// <param name="ResponseNeeded">Whether response is needed, if true, must get a response, if false, apply the value of alwaysNeedResponse</param>
        /// <param name="payload">out parameter, is the payload of response</param>
        /// <returns></returns>
        public int OperateSUTControl(SUT_Control_Request_Message requestMessage, bool ResponseNeeded, out byte[] payload)
        {

            payload = null;

            foreach (IPEndPoint agentEndpoint in AgentList)
            {
                if (transport.Connect(timeout, agentEndpoint))
                {
                    transport.SendSUTControlRequestMessage(requestMessage);

                    if (alwaysNeedResponse || ResponseNeeded)
                    {
                        //expect response only when alwaysNeedResponse is true
                        SUT_Control_Response_Message responseMessage = transport.ExpectSUTControlResponseMessage(timeout, requestMessage.requestId);

                        if (responseMessage != null)
                        {
                            if (responseMessage.resultCode == (uint)SUTControl_ResultCode.SUCCESS)
                            {
                                transport.Disconnect();
                                payload = responseMessage.payload;
                                this.Site.Log.Add(LogEntryKind.Comment, "RDP SUT Control Protocol Adapter: CommandId is {0}: Success, agent: {1}.", requestMessage.commandId, agentEndpoint.ToString());
                                return 1;
                            }
                            else
                            {
                                string errorMessage = (responseMessage.errorMessage != null) ? responseMessage.errorMessage : "";
                                this.Site.Log.Add(LogEntryKind.Comment, "RDP SUT Control Protocol Adapter: CommandId is {0}: error in response: {1}, error message: {2}", requestMessage.commandId, agentEndpoint.ToString(), errorMessage);
                            }
                        }
                        else
                        {
                            this.Site.Log.Add(LogEntryKind.Comment, "RDP SUT Control Protocol Adapter: CommandId is {0}: Not get response from agent: {1}.", requestMessage.commandId, agentEndpoint.ToString());
                        }
                    }
                    transport.Disconnect();
                }
                else
                {
                    this.Site.Log.Add(LogEntryKind.Comment, "RDP SUT Control Protocol Adapter: CommandId is {0}: Cannot connect to the agent: {1}.", requestMessage.commandId, agentEndpoint.ToString());
                }
            }
            if (alwaysNeedResponse || ResponseNeeded)
            {
                // if alwaysNeedResponse is true, all response return failure
                return -1;
            }
            else
            {
                // if alwaysNeedReponse is false, have send request to all Agents, return 1 for success
                return 1;
            }
        }

        #endregion Public methods
    }
}
