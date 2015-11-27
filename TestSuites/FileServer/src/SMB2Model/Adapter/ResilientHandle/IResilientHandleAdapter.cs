// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.ResilientHandle
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="status"></param>
    /// <param name="c">Config is for the workaround of SE bug.</param>
    public delegate void IoCtlResiliencyResponseEventHandler(ModelSmb2Status status, ResilientHandleServerConfig c);
    public delegate void ReEstablishResilientOpenResponseEventHandler(ModelSmb2Status status);

    public interface IResilientHandleAdapter : IAdapter
    {
        event IoCtlResiliencyResponseEventHandler IoCtlResiliencyResponse;
        event ReEstablishResilientOpenResponseEventHandler ReEstablishResilientOpenResponse;

        void ReadConfig(out ResilientHandleServerConfig config);

        /// <summary>
        /// Connect, Negotiate, Session Set up, Tree Connect and Create
        /// </summary>
        /// <param name="clientMaxDialect"></param>
        /// <param name="durableHandle">Indicate whether the Open is created with Durable Handle.</param>
        void PrepareOpen(ModelDialectRevision clientMaxDialect, DurableHandle durableHandle);

        void IoCtlResiliencyRequest(
            IoCtlInputCount inputCount,
            ResilientTimeout timeout);

        void LogOff();

        void Disconnect();

        void ReEstablishResilientOpenRequest(ModelUser user);
    }
}
