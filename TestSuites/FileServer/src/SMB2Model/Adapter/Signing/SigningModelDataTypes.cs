// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Signing
{
    /// <summary>
    /// The configuration of server for negotiate
    /// </summary>
    public struct SigningConfig
    {
        /// <summary>
        /// SMB 2.002 or SMB 2.1 or SMB 3.0
        /// </summary>
        public ModelDialectRevision MaxSmbVersionSupported;

        /// <summary>
        /// The platform
        /// </summary>
        public Platform Platform;

        /// <summary>
        /// Indicates server configuration of whether require signing
        /// </summary>
        public bool IsServerSigningRequired;

        /// <summary>
        /// Override ToString method to output the state info
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder outputInfo = new StringBuilder();
            outputInfo.AppendFormat("{0}: \r\n", "SigningConfig State");
            outputInfo.AppendFormat("{0}: {1} \r\n", "MaxSmbVersionSupported", this.MaxSmbVersionSupported.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "Platform", this.Platform.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "IsServerSigningRequired", this.IsServerSigningRequired.ToString());

            return outputInfo.ToString();
        }
    }

    /// <summary>
    /// Indicate SMB2_FLAGS_SIGNED in Flags field of SMB2 Packet Header
    /// </summary>
    public enum SigningFlagType
    {
        /// <summary>
        /// When set, indicate packet has been signed
        /// </summary>
        SignedFlagSet,

        /// <summary>
        /// When set, indicate packet has not been signed
        /// </summary>
        SignedFlagNotSet
    }

    /// <summary>
    /// Indicate SMB2_NEGOTIATE_SIGNING_ENABLED field of SecurityMode
    /// </summary>
    public enum SigningEnabledType
    {
        /// <summary>
        /// Indicate SMB2_NEGOTIATE_SIGNING_ENABLED is set
        /// </summary>
        SigningEnabledSet,

        /// <summary>
        /// Indicate SMB2_NEGOTIATE_SIGNING_ENABLED is not set
        /// </summary>
        SigningEnabledNotSet
    }

    /// <summary>
    /// Indicate SMB2_NEGOTIATE_SIGNING_REQUIRED field of SecurityMode
    /// </summary>
    public enum SigningRequiredType
    {
        /// <summary>
        /// Indicate SMB2_NEGOTIATE_SIGNING_REQUIRED is set
        /// </summary>
        SigningRequiredSet,

        /// <summary>
        /// Indicate SMB2_NEGOTIATE_SIGNING_REQUIRED is not set
        /// </summary>
        SigningRequiredNotSet
    }

    /// <summary>
    /// Indicate account type
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// Administrator
        /// </summary>
        Administrator,

        /// <summary>
        /// Guest
        /// </summary>
        Guest
    }

    /// <summary>
    /// SessionId in Signing Model
    /// </summary>
    public enum SigningModelSessionId
    {
        /// <summary>
        /// Indicate the SessionId is Zero
        /// </summary>
        ZeroSessionId,

        /// <summary>
        /// Indicate the SessionId is NonZero
        /// </summary>
        NonZeroSessionId
    }

    /// <summary>
    /// TreeId in Signing Model
    /// </summary>
    public enum SigningModelTreeId
    {
        /// <summary>
        /// Indicate the TreeId is Zero
        /// </summary>
        ZeroTreeId,

        /// <summary>
        /// Indicate the TreeId is NonZero
        /// </summary>
        NonZeroTreeId
    }
}
