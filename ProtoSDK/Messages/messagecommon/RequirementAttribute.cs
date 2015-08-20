// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// An attribute which indicates a requirement exists for field, structure or RPC operation.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class RequirementAttribute : System.Attribute
    {
        int requirementID;
        String protocolDocName;     
        String description;
        String sectionNumber;

        /// <summary>
        /// Constructs requirement attribute.
        /// </summary>
        /// <param name="protocolDocName">Name of the protocol document.</param>
        /// <param name="sectionNumber">Section number of requirement in document.</param>
        /// <param name="requirementID">Requirement ID.</param>
        /// <param name="description">Description of the requirement.</param>
        public RequirementAttribute(String protocolDocName, String sectionNumber,
                            int requirementID, String description)
        {
            this.requirementID = requirementID;        
            this.description =  description;
            this.protocolDocName =  protocolDocName;
            this.sectionNumber = sectionNumber;
        }
        /// <summary>
        /// Indicates requirement ID.
        /// </summary>
        public int RequirementID { get { return requirementID; } }
        /// <summary>
        /// Indicates description of the requirement.
        /// </summary>
        public String Description { get { return description; } }
        /// <summary>
        /// Indicates name of the protocol document.
        /// </summary>
        public String ProtocolDocName { get { return protocolDocName; } }
        /// <summary>
        /// Indicates section number of requirement in document.
        /// </summary>
        public String SectionNumber { get { return sectionNumber; } }
    }


    /// <summary>
    /// An attribute which indicates the range of the variable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class PossibleValueRangeAttribute : System.Attribute
    {
        private string minValue;
        private string maxValue;

        /// <summary>
        /// Constructs value range attribute.
        /// </summary>
        /// <param name="minValue">Indicates the minimum value.</param>
        /// <param name="maxValue">Indicates the maximum value.</param>
        public PossibleValueRangeAttribute(string minValue, string maxValue)
        {
            if (String.IsNullOrEmpty("minValue"))
            {
                throw new ArgumentNullException("minValue");
            }
            if (String.IsNullOrEmpty("maxValue"))
            {
                throw new ArgumentNullException("maxValue");
            }

            this.minValue = minValue;
            this.maxValue = maxValue;
        }
        /// <summary>
        /// Indicates the minimum value.
        /// </summary>
        public string MinValue { get { return minValue; } }
        /// <summary>
        /// Indicates the maximun value.
        /// </summary>
        public string MaxValue { get { return maxValue; } }
    }
    
    /// <summary>
    /// An attribute which indicates the possible values of the variable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class PossibleValueAttribute : System.Attribute
    {
        private readonly Type enumType;

        /// <summary>
        /// Constructs possible value attribute.
        /// </summary>
        /// <param name="enumType">An enum type to indicate possible values.</param>
        public PossibleValueAttribute(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException("an enum type is needed in constructor of PossibleValue attribute");
            }

            this.enumType = enumType;
        }
        /// <summary>
        /// Indicates the type of enum.
        /// </summary>
        public Type EnumType { get { return enumType; } }
    }
 }
