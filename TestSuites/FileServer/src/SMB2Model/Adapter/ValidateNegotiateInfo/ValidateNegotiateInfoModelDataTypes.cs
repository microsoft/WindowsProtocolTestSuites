// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.ValidateNegotiateInfo
{
    /// <summary>
    /// Wrap Capabilities_Values for Model to use
    /// </summary>
    public enum ModelCapabilities: uint
    {
        /// <summary>
        /// No flags are set
        /// </summary>
        None = 0,   

        /// <summary>
        /// The first three flags of Capabilities_Values are set 
        /// </summary>
        FullCapabilitiesForNonSmb30 = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU,  

        /// <summary>
        /// All the valid flags of Capabilities_Values are set
        /// </summary>
        FullCapabilitiesForSmb30 = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU 
            | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_ENCRYPTION | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,

        /// <summary>
        /// All bits are set, for invalid case
        /// </summary>
        AllBitsSet = 0xFFFFFFFF,
    }

    /// <summary>
    /// Indicate the type of dialect field in ValidateNegotiateInfo request
    /// </summary>
    public enum DialectType
    {
        /// <summary>
        /// Set nothing in dialect field
        /// </summary>
        None = 0,

        /// <summary>
        /// Use the same dialect with the one in Negotiate request
        /// </summary>
        DialectSameWithNegotiate,

        /// <summary>
        /// Use a different dialect from the one in Negotiate request
        /// </summary>
        DialectDifferentFromNegotiate,
    }

    /// <summary>
    /// Indicates if Capabilities in ValidateNeogitateInfo request is the same with Negotiate or not.
    /// </summary>
    public enum CapabilitiesType
    {
        CapabilitiesSameWithNegotiate,
        CapabilitiesDifferentFromNegotiate
    }

    /// <summary>
    /// Indicates if SecurityMode in ValidateNeogitateInfo request is the same with Negotiate or not.
    /// </summary>  
    public enum SecurityModeType
    {
        SecurityModeSameWithNegotiate,
        SecurityModeDifferentFromNegotiate
    }

    /// <summary>
    /// Indicates if ClientGuid in ValidateNeogitateInfo request is the same with Negotiate or not.
    /// </summary>  
    public enum ClientGuidType
    {
        ClientGuidSameWithNegotiate,
        ClientGuidDifferentFromNegotiate
    }

    /// <summary>
    /// Indicates if Ioctl code ValidateNegotiateInfo is supported in server or not
    /// </summary>
    
    public enum ValidateNegotiateInfoInServer
    {
        NotSupportValidateNegotiateInfo,

        SupportValidateNegotiateInfo
    }

    public struct ValidateNegotiateInfoConfig
    {
        public Platform Platform;

        /// <summary>
        /// Indicate whether server support IoCtl Code FSCTL_VALIDATE_NEGOTIATE_INFO 
        /// </summary>
        public ValidateNegotiateInfoInServer ValidateNegotiateInfoSupported;

        public override string ToString()
        {
            StringBuilder outputInfo = new StringBuilder();
            outputInfo.AppendFormat("{0}: \r\n", "ValidateNegotiateInfoConfig State");
            outputInfo.AppendFormat("{0}: {1} \r\n", "Platform", this.Platform.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "ValidateNegotiateInfoSupported", this.ValidateNegotiateInfoSupported.ToString());

            return outputInfo.ToString();
        }
    }
}
