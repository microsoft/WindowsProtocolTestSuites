// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.ResilientHandle
{
    /// <summary>
    /// The configuration of server for negotiate
    /// </summary>
    public struct ResilientHandleServerConfig
    {
        /// <summary>
        /// SMB 2.002 or SMB 2.1 or SMB 3.0
        /// </summary>
        public ModelDialectRevision MaxSmbVersionSupported;

        /// <summary>
        /// Indicate whether server support IoCtl Code FSCTL_LMR_REQUEST_RESILIENCY 
        /// </summary>
        public bool IsIoCtlCodeResiliencySupported;

        /// <summary>
        /// Indicate the platform of SUT
        /// </summary>
        public Platform Platform;

        /// <summary>
        /// Override ToString method to output the state info
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder outputInfo = new StringBuilder();
            outputInfo.AppendFormat("{0}: \r\n", "ResilientHandleServerConfig State");
            outputInfo.AppendFormat("{0}: {1} \r\n", "MaxSmbVersionSupported", this.MaxSmbVersionSupported.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "IsIoCtlCodeResiliencySupported", this.IsIoCtlCodeResiliencySupported.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "Platform", this.Platform.ToString());

            return outputInfo.ToString();
        }
    }

    public enum IoCtlInputCount
    {
        /// <summary>
        /// Smaller than the size of NETWORK_RESILIENCY_REQUEST request
        /// </summary>
        InputCountSmallerThanRequestSize,

        /// <summary>
        /// Equal to the size of NETWORK_RESILIENCY_REQUEST request
        /// </summary>
        InputCountEqualToRequestSize,

        /// <summary>
        /// Greater than the size of NETWORK_RESILIENCY_REQUEST request
        /// </summary>
        InputCountGreaterThanRequestSize
    }

    public enum ResilientTimeout
    {
        /// <summary>
        /// Zero
        /// </summary>
        ZeroTimeout,

        /// <summary>
        /// Timeout is not greater than MaxResiliencyTimeout
        /// </summary>
        ValidTimeout,

        /// <summary>
        /// Timeout is greater than MaxResiliencyTimeout
        /// </summary>
        InvalidTimeout
    }

    /// <summary>
    /// Indicate whether create with Durable Handle
    /// </summary>
    public enum DurableHandle
    {
        DurableHandle,
        NoDurableHandle
    }
}
