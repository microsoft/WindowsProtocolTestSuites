// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
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

    public interface IWspStructure : IWspObject
    {

    }

    public interface IWspMessage : IWspObject
    {
        WspMessageHeader Header { get; set; }
    }

    public interface IWspInMessage : IWspMessage
    {

    }

    public interface IWspOutMessage : IWspMessage
    {
        IWspInMessage Request { get; set; }
    }
}
