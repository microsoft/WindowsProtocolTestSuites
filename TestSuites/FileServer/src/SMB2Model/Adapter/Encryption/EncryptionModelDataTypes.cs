// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Encryption
{
    /// <summary>
    /// The configuration of server for negotiate
    /// </summary>
    public struct EncryptionConfig
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
        /// Indicates whether the server enables global encryption.
        /// </summary>
        public bool IsGlobalEncryptDataEnabled;

        /// <summary>
        /// Indicates whether the server enables global reject the unencrypted messages
        /// </summary>
        public bool IsGlobalRejectUnencryptedAccessEnabled;

        /// <summary>
        /// Override ToString method to output the state info
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder outputInfo = new StringBuilder();
            outputInfo.AppendFormat("{0}: \r\n", "EncryptionConfig State");
            outputInfo.AppendFormat("{0}: {1} \r\n", "MaxSmbVersionSupported", this.MaxSmbVersionSupported.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "Platform", this.Platform.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "IsGlobalEncryptDataEnabled", this.IsGlobalEncryptDataEnabled.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "IsGlobalRejectUnencryptedAccessEnabled", this.IsGlobalRejectUnencryptedAccessEnabled.ToString());

            return outputInfo.ToString();
        }
    }

    /// <summary>
    /// The abstract TreeId for encryption model
    /// </summary>
    public enum EncryptionTreeId
    {
        /// <summary>
        /// The tree id has not been initialized
        /// </summary>
        NoTreeId,

        /// <summary>
        /// The tree id indicates treeconnect to encrypted share
        /// </summary>
        TreeIdToEncryptShare,

        /// <summary>
        /// The tree id indicates treeconnect to unencrypted share
        /// </summary>
        TreeIdToUnEncryptShare
    }

    /// <summary>
    /// Indicates whether client supports encryption
    /// </summary>
    public enum ClientSupportsEncryptionType
    {
        /// <summary>
        /// Indicate client supports encryption
        /// </summary>
        ClientSupportsEncryption,

        /// <summary>
        /// Indicates client not supports encryption
        /// </summary>
        ClientNotSupportsEncryption
    }

    /// <summary>
    /// Indicates whether session encrypt data set
    /// </summary>
    public enum SessionEncryptDataType
    {
        /// <summary>
        /// Indicates session encrypt data not set
        /// </summary>
        SessionEncryptDataNotSet,

        /// <summary>
        /// Indicate session encrypt data set
        /// </summary>
        SessionEncryptDataSet,
    }

    /// <summary>
    /// Indicates whether share encrypt data set
    /// </summary>
    public enum ShareEncryptDataType
    {
        /// <summary>
        /// Indicate share encrypt data set
        /// </summary>
        ShareEncryptDataSet,

        /// <summary>
        /// Indicates share encrypt data not set
        /// </summary>
        ShareEncryptDataNotSet
    }

    /// <summary>
    /// Indicates whether connect to encrypted share
    /// </summary>
    public enum ConnectToShareType
    {
        /// <summary>
        /// Indicate connect to encrypted share
        /// </summary>
        ConnectToEncryptedShare,

        /// <summary>
        /// Indicates connect to unencrypted share
        /// </summary>
        ConnectToUnEncryptedShare
    }

    /// <summary>
    /// Indicates whether the request has been encrypted
    /// </summary>
    public enum ModelRequestType
    {
        /// <summary>
        /// Indicate the request has been encrypted
        /// </summary>
        EncryptedRequest,

        /// <summary>
        /// Indicates the request has not been encrypted
        /// </summary>
        UnEncryptedRequest
    }

    /// <summary>
    /// Indicates whether the response has been encrypted
    /// </summary>
    public enum ModelResponseType
    {
        /// <summary>
        /// Indicate the response has been encrypted
        /// </summary>
        EncryptedResponse,

        /// <summary>
        /// Indicates the response has not been encrypted
        /// </summary>
        UnEncryptedResponse
    }
}
