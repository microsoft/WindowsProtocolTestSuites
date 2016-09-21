// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.BranchCache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;

    /// <summary>
    /// Receive probe message event.
    /// </summary>
    /// <param name="sender">The received remote point property</param>
    /// <param name="probeMsg">The received ProbeMsg</param>
    public delegate void ReceiveProbeMsgHandler(IPEndPoint sender, ProbeMsg probeMsg);

    /// <summary>
    /// The interface of content caching and retrieval framework.
    /// </summary>
    public interface IBranchCacheFrameworkAdapter : IAdapter
    {
        /// <summary>
        /// Receive the MS-PCCRR request message handle.
        /// </summary>
        event EventHandler<ReceivedPccrrRequestEventArg> ReceivePccrrRequestHandler;

        /// <summary>
        /// Receive the Probe message.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
            Justification = "Disable for it is neccessary in test code")]
        event ReceiveProbeMsgHandler ReceiveProbeMessage;

        /// <summary>
        /// Get the segment IDs computed using content information that is got from the content server.
        /// </summary>
        /// <param name="serverAddress"> The content server address.</param>
        /// <param name="port"> The port that the content server is listening.</param>
        /// <param name="path"> The path of the file to request.</param>
        /// <returns>Returns segment IDs if success.</returns>
        string[] GetSegmentIds(string serverAddress, int port, string path);

        /// <summary>
        /// Get the byte array list of segment IDs computed using content information that is got from the content server.
        /// </summary>
        /// <param name="serverAddress"> The content server address.</param>
        /// <param name="port"> The port that the content server is listening.</param>
        /// <param name="path"> The path of the file to request.</param>
        /// <returns>Returns the byte array list of segment IDs if success.</returns>
        ICollection<byte[]> GetSegmentIdsByteArray(string serverAddress, int port, string path);

        /// <summary>
        /// Get the content information from the content server.
        /// </summary>
        /// <param name="serverAddress"> The content server address.</param>
        /// <param name="port"> The port that the content server is listening.</param>
        /// <param name="path"> The path of the file to request.</param>
        /// <returns> Returns content information if success.</returns>
        Content_Information_Data_Structure GetContentInfo(string serverAddress, int port, string path);

        /// <summary>
        /// Offer the content information to the hosted cache server.
        /// </summary>
        /// <param name="contentInfo">ContentInfo set to the hosted cache server</param>
        void OfferHostedCacheContentInfo(Content_Information_Data_Structure contentInfo);

        /// <summary>
        /// Start the MS-PCCRR server listening to incoming MS-PCCRR request message.
        /// </summary>
        /// <param name="port">The listening port.</param>
        void StartPccrrServerListening(int port);

        /// <summary>
        /// Send the ProbeMatch message.
        /// </summary>
        /// <param name="relatesTo">The relatesTo.</param>
        /// <param name="instanceId">The instanceId.</param>
        /// <param name="messageNumber">The messageNumber.</param>
        /// <param name="matches">The matches.</param>
        /// <param name="ip">The ip address of content client.</param>
        /// <param name="port">The port of content client.</param>
        void SendProbeMatchMessage(string relatesTo, string instanceId, uint messageNumber, ServiceProperty[] matches, string ip, int port);

        /// <summary>
        /// Start the MS-PCCRD server listening to incoming MS-PCCRD request message.
        /// </summary>
        void StartPccrdServerListening();
    }
}
