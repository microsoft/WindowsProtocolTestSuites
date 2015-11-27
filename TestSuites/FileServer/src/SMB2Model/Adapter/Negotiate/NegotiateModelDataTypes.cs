// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Negotiate
{
    /// <summary>
    /// The configuration of server for negotiate
    /// </summary>
    public struct NegotiateServerConfig
    {
        /// <summary>
        /// SMB 2.002 or SMB 2.1 or SMB 3.0
        /// </summary>
        public DialectRevision MaxSmbVersionSupported;

        /// <summary>
        /// Override ToString method to output the state info
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder outputInfo = new StringBuilder();
            outputInfo.AppendFormat("{0}: {1}", "NegotiateServerConfig State", System.Environment.NewLine);
            outputInfo.AppendFormat("{0}: {1} {2}", "MaxSmbVersionSupported", this.MaxSmbVersionSupported.ToString(), System.Environment.NewLine);

            return outputInfo.ToString();
        }
    }
}
