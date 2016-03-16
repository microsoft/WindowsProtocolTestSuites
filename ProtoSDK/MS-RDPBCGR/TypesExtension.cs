// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    public partial struct RDP_SERVER_REDIRECTION_PACKET
    {
        /// <summary>
        /// IsTargetNetAddressVisible
        /// Try to determine whether to hide a field in struct RDP_SERVER_REDIRECTION_PACKET
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsTargetNetAddressVisible(MarshalingType marshalingType, object FieldName)
        {
            if (FieldName == null)
            {
                return false;
            }

            RDP_SERVER_REDIRECTION_PACKET packetData = (RDP_SERVER_REDIRECTION_PACKET)FieldName;

            return packetData.TargetNetAddressesLength != 0;
        }

        /// <summary>
        /// IsLoadBalanceInfoVisible
        /// Try to determine whether to hide a field in struct RDP_SERVER_REDIRECTION_PACKET
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsLoadBalanceInfoVisible(MarshalingType marshalingType, object FieldName)
        {
            if (FieldName == null)
            {
                return false;
            }

            RDP_SERVER_REDIRECTION_PACKET packetData = (RDP_SERVER_REDIRECTION_PACKET)FieldName;

            return packetData.LoadBalanceInfoLength != 0;
        }

        /// <summary>
        /// IsUserNameVisible
        /// Try to determine whether to hide a field in struct RDP_SERVER_REDIRECTION_PACKET
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsUserNameVisible(MarshalingType marshalingType, object FieldName)
        {
            if (FieldName == null)
            {
                return false;
            }

            RDP_SERVER_REDIRECTION_PACKET packetData = (RDP_SERVER_REDIRECTION_PACKET)FieldName;

            return packetData.UserNameLength != 0;
        }

        /// <summary>
        /// IsDomainVisible
        /// Try to determine whether to hide a field in struct RDP_SERVER_REDIRECTION_PACKET
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsDomainVisible(MarshalingType marshalingType, object FieldName)
        {
            if (FieldName == null)
            {
                return false;
            }

            RDP_SERVER_REDIRECTION_PACKET packetData = (RDP_SERVER_REDIRECTION_PACKET)FieldName;

            return packetData.DomainLength != 0;
        }

        /// <summary>
        /// IsPasswordLengthVisible
        /// Try to determine whether to hide a field in struct RDP_SERVER_REDIRECTION_PACKET
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsPasswordVisible(MarshalingType marshalingType, object FieldName)
        {
            if (FieldName == null)
            {
                return false;
            }

            RDP_SERVER_REDIRECTION_PACKET packetData = (RDP_SERVER_REDIRECTION_PACKET)FieldName;

            return packetData.PasswordLength != 0;
        }

        /// <summary>
        /// IsTargetFQDNVisible
        /// Try to determine whether to hide a field in struct RDP_SERVER_REDIRECTION_PACKET
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsTargetFQDNVisible(MarshalingType marshalingType, object FieldName)
        {
            if (FieldName == null)
            {
                return false;
            }

            RDP_SERVER_REDIRECTION_PACKET packetData = (RDP_SERVER_REDIRECTION_PACKET)FieldName;

            return packetData.TargetFQDNLength != 0;
        }

        /// <summary>
        /// IsTargetNetBiosNameVisible
        /// Try to determine whether to hide a field in struct RDP_SERVER_REDIRECTION_PACKET
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsTargetNetBiosNameVisible(MarshalingType marshalingType, object FieldName)
        {
            if (FieldName == null)
            {
                return false;
            }

            RDP_SERVER_REDIRECTION_PACKET packetData = (RDP_SERVER_REDIRECTION_PACKET)FieldName;

            return packetData.TargetNetBiosNameLength != 0;
        }

        /// <summary>
        /// IsTargetNetAddressesVisible
        /// Try to determine whether to hide a field in struct RDP_SERVER_REDIRECTION_PACKET
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsTargetNetAddressesVisible(MarshalingType marshalingType, object FieldName)
        {
            if (FieldName == null)
            {
                return false;
            }

            RDP_SERVER_REDIRECTION_PACKET packetData = (RDP_SERVER_REDIRECTION_PACKET)FieldName;

            return packetData.TargetNetAddressesLength != 0;
        }
    }
}
