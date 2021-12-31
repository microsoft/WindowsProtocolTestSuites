// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// Common interface for WSP structures and messages.
    /// </summary>
    public interface IWspObject
    {
        /// <summary>
        /// Marshall WSP object to bytes.
        /// </summary>
        /// <param name="buffer">The WSP buffer used to contain marshalled bytes.</param>
        void ToBytes(WspBuffer buffer);

        /// <summary>
        /// Unmarshall bytes and initialize WSP object.
        /// </summary>
        /// <param name="buffer">The WSP buffer containing bytes to be unmarshalled.</param>
        void FromBytes(WspBuffer buffer);
    }

    /// <summary>
    /// An empty interface to identify WSP structure types.
    /// </summary>
    public interface IWspStructure : IWspObject
    {

    }

    /// <summary>
    /// An empty interface to identify SeekDescription types.
    /// </summary>
    public interface IWspSeekDescription : IWspStructure
    {

    }

    /// <summary>
    /// An empty interface to identify restriction types.
    /// </summary>
    public interface IWspRestriction : IWspStructure
    {

    }

    /// <summary>
    /// Interface for WSP messages.
    /// </summary>
    public interface IWspMessage : IWspObject
    {
        WspMessageHeader Header { get; set; }
    }

    /// <summary>
    /// Interface for WSP messages sent from client.
    /// </summary>
    public interface IWspInMessage : IWspMessage
    {

    }

    /// <summary>
    /// Interface for WSP messages sent from server.
    /// </summary>
    public interface IWspOutMessage : IWspMessage
    {
        IWspInMessage Request { get; set; }
    }
}
