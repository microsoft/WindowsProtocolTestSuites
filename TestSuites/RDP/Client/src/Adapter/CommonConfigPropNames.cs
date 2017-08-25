// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Microsoft.Protocols.TestSuites
{
    /// <summary>
    /// The defined property names in PTFConfig
    /// </summary>
    public partial class ConfigPropNames
    {
        /// <summary>
        /// The namespace of Environment properties.
        /// </summary>
        public const string EnvironmentNamespace    = "Environment.{0}";

        /// <summary>
        /// The namespace of Testing properties.
        /// </summary>
        public const string TestingNamespace        = "Testing.{0}";

        #region Environment Names
        
        /// <summary>
        /// The name of SUT computer name property.
        /// </summary>
        public const string SutComputerName = "Environment.SUT.ComputerName";
        /// <summary>
        /// The name of SUT computer name property.
        /// </summary>
        public const string SutPort = "Environment.SUT.Port";
        /// <summary>
        /// The name of SUT computer name property.
        /// </summary>
        public const string SutOs = "Environment.SUT.OS";
        /// <summary>
        /// The name of SUT computer name property.
        /// </summary>
        public const string SutIPv4 = "Environment.SUT.IPv4";
        /// <summary>
        /// The name of SUT computer name property.
        /// </summary>
        public const string SutIPv6 = "Environment.SUT.IPv6";
        /// <summary>
        /// The name of SUT computer name property.
        /// </summary>
        public const string DriverPort = "Environment.DriverComputer.Port";
        /// <summary>
        /// The name of SUT computer name property.
        /// </summary>
        public const string DomainName = "Environment.Domain.Name";
        /// <summary>
        /// The name of SUT computer name property.
        /// </summary>
        public const string IsDomain = "Environment.Domain.InDomain";

        #endregion

        #region Testing Names

        /// <summary>
        /// The name of disabled requirement verification switch property.
        /// </summary>
        public const string ReqSwitch = "Testing.DisabledRequirements";
            
        #endregion

        /// <summary>
        /// This method is used to combine the property namespace and name.
        /// </summary>
        /// <param name="space">The namespace of the property name.</param>
        /// <param name="name">The property name.</param>
        /// <returns>The full property name.</returns>
        public static string GetCombinedPropName(string space, string name)
        {
            return String.Format(space, name);
        }

        /// <summary>
        /// This method is used to retrieve environment related names.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>The full property name.</returns>
        public static string GetEnvironmentPropName(string name)
        {
            return GetCombinedPropName(EnvironmentNamespace, name);
        }

        /// <summary>
        /// This method is used to retrieve testing related names.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>The full property name.</returns>
        public static string GetTestingPropName(string name)
        {
            return GetCombinedPropName(TestingNamespace, name);
        }
    }
}
