// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// WSP client.
    /// </summary>
    public class WspClient : IDisposable
    {
        #region Constructors
        public WspClient()
        {

        }
        #endregion

        #region Properties
        /// <summary>
        /// Indicating whether the client version is 32bit/64-bit (false/true).
        /// </summary>
        public bool Is64bitClientVersion { get; private set; }

        /// <summary>
        /// Indicating whether the server version is 32bit/64-bit (false/true).
        /// </summary>
        public bool Is64bitServerVersion { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Send CPMConnectIn.
        /// </summary>
        /// <param name="clientVersion">clientVersion field to be used.</param>
        /// <param name="clientIsRemote">clientIsRemote field to be used.</param>
        /// <param name="machineName">machineName field to be used.</param>
        /// <param name="userName">userName field to be used.</param>
        /// <param name="propertySet1">propertySet1 field to be used.</param>
        /// <param name="propertySet2">propertySet2 field to be used.</param>
        /// <param name="aPropertySet">aPropertySet field to be used.</param>
        public void SendCPMConnectIn(
            UInt32 clientVersion,
            UInt32 clientIsRemote,
            string machineName,
            string userName,
            CDbPropSet propertySet1,
            CDbPropSet propertySet2,
            CDbPropSet[] aPropertySet
            )
        {
            if ((clientVersion & WspConsts.Is64bitVersion) != 0)
            {
                Is64bitClientVersion = true;
            }
            else
            {
                Is64bitClientVersion = false;
            }

            var request = new CPMConnectIn()
            {
                Header = new WspMessageHeader
                {
                    _msg = WspMessageHeader_msg_Values.CPMConnectIn,
                },
                _iClientVersion = clientVersion,
                _fClientIsRemote = clientIsRemote,
                MachineName = machineName,
                UserName = userName,
                cPropSets = 2,
                PropertySet1 = propertySet1,
                PropertySet2 = propertySet2,
                cExtPropSet = (UInt32)aPropertySet.Length,
                aPropertySets = aPropertySet,
            };

            Send(request);
        }

        /// <summary>
        /// Send CPMCreateQueryIn.
        /// </summary>
        /// <param name="columnSet">columnSet field to be used.</param>
        /// <param name="restrictionArray">restrictionArray field to be used.</param>
        /// <param name="sortSet">sortSet field to be used.</param>
        /// <param name="categorizationSet">categorizationSet field to be used.</param>
        /// <param name="rowsetProperties">rowsetProperties field to be used.</param>
        /// <param name="pidMapper">pidMapper field to be used.</param>
        /// <param name="columnGroupArray">columnGroupArray field to be used.</param>
        /// <param name="lcid">lcid field to be used.</param>
        public void SendCPMCreateQueryIn(
            CColumnSet? columnSet,
            CRestrictionArray? restrictionArray,
            CInGroupSortAggregSets? sortSet,
            CCategorizationSet? categorizationSet,
            CRowsetProperties rowsetProperties,
            CPidMapper pidMapper,
            CColumnGroupArray columnGroupArray,
            UInt32 lcid
            )
        {
            var request = new CPMCreateQueryIn
            {
                Header = new WspMessageHeader
                {
                    _msg = WspMessageHeader_msg_Values.CPMCreateQueryIn,
                },
                ColumnSet = columnSet,
                RestrictionArray = restrictionArray,
                SortSet = sortSet,
                CCategorizationSet = categorizationSet,
                RowSetProperties = rowsetProperties,
                PidMapper = pidMapper,
                GroupArray = columnGroupArray,
                Lcid = lcid,
            };

            Send(request);
        }

        /// <summary>
        /// Send message to server.
        /// </summary>
        /// <param name="request">Message to be sent.</param>
        public void Send(IWspInMessage request)
        {
            lastRequest = request;

            sender.SendMessage(Helper.ToBytes(request), out lastResponseBytes);
        }

        /// <summary>
        /// Expect reponse from server.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="response">Response from server.</param>
        /// <returns>Status</returns>
        public UInt32 ExpectMessage<T>(out T response) where T : struct, IWspOutMessage
        {
            response = new T();

            response.Request = lastRequest;

            var header = new WspMessageHeader();

            Helper.FromBytes(ref header, lastResponseBytes);

            if (header._msg != lastRequest.Header._msg)
            {
                throw new InvalidOperationException("Unexpected response from server!");
            }

            if (header._status != 0)
            {
                response.Header = header;
                return header._status;
            }

            var buffer = new WspBuffer(lastResponseBytes);

            response.FromBytes(buffer);

            // Update the state of client according to response.
            UpdateContext(response);

            // Check the response size against unmarshalled size.
            if (buffer.ReadOffset != buffer.WriteOffset)
            {
                throw new InvalidOperationException("Unexpected response size from server!");
            }

            return 0;
        }
        #endregion

        private void UpdateContext(IWspOutMessage response)
        {
            switch (response.Header._msg)
            {
                case WspMessageHeader_msg_Values.CPMConnectOut:
                    {
                        var connectOutMessage = (CPMConnectOut)response;
                        if ((connectOutMessage._serverVersion & WspConsts.Is64bitVersion) != 0)
                        {
                            Is64bitServerVersion = true;
                        }
                        else
                        {
                            Is64bitServerVersion = false;
                        }
                    }
                    break;
            }
        }


        public void Dispose()
        {

        }

        public RequestSender sender;

        private IWspInMessage lastRequest;

        private byte[] lastResponseBytes;
    }
}