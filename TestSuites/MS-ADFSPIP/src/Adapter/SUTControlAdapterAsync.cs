// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Remotely controls SUT asynchronously, so that the main
    /// thread won't be blocked and waiting for client result.
    /// </summary>
    public class SUTControlAdapterAsync : ManagedAdapterBase
    {
        private readonly ISUTControlAdapter _adapter;
     
        /// <summary>
        /// Initializes the async adapter with an ISUTControlAdapter
        /// instance.
        /// </summary>
        public SUTControlAdapterAsync(ISUTControlAdapter adapter)
        {
            _adapter = adapter;
        }

        /// <summary>
        /// Trigger the proxy to publish an applicatioan asynchronously.
        /// </summary>
        public Task<SutResult> TriggerPublishApplicationAsync()
        {
            return Task.Factory.StartNew(() => {
                string error; var result = _adapter.TriggerPublishApplication(out error);
                return new SutResult(result, error);
            });
        }

        /// <summary>
        /// Trigger the proxy to publish an applicatioan asynchronously.
        /// </summary>
        public Task<SutResult> TriggerPublishNonClaimsAppAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                string error; var result = _adapter.TriggerPublishNonClaimsApp(out error);
                return new SutResult(result, error);
            });
        }

        /// <summary>
        /// Trigger the proxy to remove an application asynchronously.
        /// </summary>
        public Task<SutResult> TriggerRemoveApplicationAsync()
        {
            return Task.Factory.StartNew(() => {
                string error; var result = _adapter.TriggerRemoveApplication(out error);
                return new SutResult(result, error);
            });
        }

        /// <summary>
        /// Trigger the proxy to install application proxy Windows feature
        /// asynchronously.
        /// </summary>
        public Task<SutResult> TriggerInstallApplicationProxyAsync()
        {
            return Task.Factory.StartNew(() => {
                string error; 
                var result = _adapter.TriggerInstallApplicationProxy(out error);
                return new SutResult(result, error);
            });
        }

        /// <summary>
        /// The result returned from the client side.
        /// This struct will be returned by the async methods.
        /// </summary>
        public struct SutResult
        {
            /// <summary>
            /// The return value.
            /// </summary>
            public bool Return;

            /// <summary>
            /// The output message.
            /// </summary>
            public string Message;

            /// <summary>
            /// The error message.
            /// </summary>
            public string Error;

            public SutResult(bool result)
            {
                Return  = result;
                Error   = string.Empty;
                Message = string.Empty;
            }

            public SutResult(bool result, string error)
            {
                Return  = result;
                Error   = error;
                Message = string.Empty;
            }
        }
   
    }
}
