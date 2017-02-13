// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Protocols.TestTools.MessageAnalyzer;
using Microsoft.Protocol.TestSuites.ADOD.Adapter.Util;
using System.Globalization;

namespace Microsoft.Protocol.TestSuites.ADOD.Adapter
{
    /// <summary>
    /// Expected message List
    /// </summary>
    [XmlRoot]
    public class ExpectedMessageList
    {
        /// <summary>
        /// Message analyzer Filter
        /// </summary>
        [XmlElementAttribute]
        public string Filter
        {
            get;
            set;
        }

        /// <summary>
        /// List of Expected Messages
        /// </summary>
        [XmlElementAttribute("ExpectedMessage")]
        public List<ExpectedMessage> ExpectedMessages
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Expected Message
    /// </summary>
    public class ExpectedMessage
    {
        /// <summary>
        /// Message Description
        /// </summary>
        [XmlElementAttribute(IsNullable = true)]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Message Name
        /// </summary>
        [XmlElementAttribute]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Destination of the message
        /// </summary>
        [XmlElementAttribute]
        public string Destination
        {
            get;
            set;
        }

        /// <summary>
        /// Source of the message
        /// </summary>
        [XmlElementAttribute]
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Message must apply these verifications
        /// </summary>
        [XmlElementAttribute(IsNullable = true)]
        public VerifyItemList VerifyItemList
        {
            get;
            set;
        }

        /// <summary>
        /// Message must contain these structures
        /// </summary>
        [XmlElementAttribute("StructureField", IsNullable = true)]
        public List<StructureField> Structures
        {
            get;
            set;
        }

        /// <summary>
        /// Message must contain these arrays.
        /// </summary>
        [XmlElementAttribute("ArrayField", IsNullable = true)]
        public List<ArrayField> Arrays
        {
            get;
            set;
        }

        /// <summary>
        /// This method is to verify whether a message is expected.
        /// </summary>
        /// <param name="message">An MA message</param>
        /// <param name="endpointRoles">Specifies the Endpoint Roles.</param>
        /// <returns>true if the message is expected, otherwise return false.</returns>
        public bool Verify(Message message, Dictionary<string, EndpointRole> endpointRoles)
        {
            // Verify type name
            if (!message.TypeName.ToLower(CultureInfo.InvariantCulture).Equals(this.Name.ToLower(CultureInfo.InvariantCulture)))
                return false;
            
            // Verify address
            if (!VerifyAddress(message, endpointRoles))
                return false;
            
            // Verify the verifyList
            if (VerifyItemList != null)
            {
                if (!VerifyItemList.Verify(message))
                    return false;
            }
            
            // Verify structures
            if (Structures != null)
            {
                foreach (StructureField structure in Structures)
                {
                    if (!structure.Verify(message))
                        return false;
                }
            }
            
            // Verify Arrays
            if (Arrays != null)
            {
                foreach (ArrayField array in Arrays)
                {
                    if (!array.Verify(message))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Verify the source and destination of message are expected
        /// </summary>
        /// <param name="message">An MA message</param>
        /// <param name="endpointRoles">Specifies the Endpoint Roles.</param>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        public bool VerifyAddress(Message message, Dictionary<string, EndpointRole> endpointRoles)
        {
            // If source or destination is not in endpointRoles, return false;
            if (!(endpointRoles.ContainsKey(this.Source) && endpointRoles.ContainsKey(this.Destination)))
                return false;

            EndpointRole sourceEndpoint = endpointRoles[this.Source];
            EndpointRole destinationEndpoint = endpointRoles[this.Destination];

            if (message.Source.Equals(sourceEndpoint.IPv4) && message.Destination.Equals(destinationEndpoint.IPv4))
            {
                // For IPv4
                return true;
            }

            if (message.Source.Equals(sourceEndpoint.IPv6) && message.Destination.Equals(destinationEndpoint.IPv6))
            {
                // For IPv6
                return true;
            }

            if (message.Source.Equals(sourceEndpoint.MAC) && message.Destination.Equals(destinationEndpoint.MAC))
            {
                // For Mac
                return true;
            }
                  
            return false;
        }
    }

    /// <summary>
    /// Expected structure
    /// </summary>
    public class StructureField
    {
        /// <summary>
        /// Field name of this structure
        /// </summary>
        [XmlAttributeAttribute]
        public string FieldName
        {
            get;
            set;
        }

        /// <summary>
        /// Verification needed for this structure
        /// </summary>
        [XmlElementAttribute(IsNullable = true)]
        public VerifyItemList VerifyItemList
        {
            get;
            set;
        }

        /// <summary>
        /// Sub structures
        /// </summary>
        [XmlElementAttribute("StructureField", IsNullable = true)]
        public List<StructureField> Structures
        {
            get;
            set;
        }

        /// <summary>
        /// Arrays in this structures
        /// </summary>
        [XmlElementAttribute("ArrayField", IsNullable = true)]
        public List<ArrayField> Arrays
        {
            get;
            set;
        }

        /// <summary>
        /// Verify whether this structure is expected
        /// </summary>
        /// <param name="complexValue">Specifies an MA complex type.</param>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        public bool Verify(ComplexType complexValue)
        {
            // Get the structure by using field name
            ComplexType value;
            if (!MAVerifyUtil.GetFieldValue<ComplexType>(FieldName, complexValue, out value))
                return false;
            
            // Verify the verifyList
            if (VerifyItemList != null)
            {
                if (!VerifyItemList.Verify(value))
                    return false;
            }
            
            // Verify sub-structures
            if (Structures != null)
            {
                foreach (StructureField structure in Structures)
                {
                    if (!structure.Verify(value))
                        return false;
                }
            }
            
            // Verify arrays in this structure
            if (Arrays != null)
            {
                foreach (ArrayField array in Arrays)
                {
                    if (!array.Verify(value))
                        return false;
                }
            }
            return true;
        }

    }

    /// <summary>
    /// Expected Array
    /// </summary>
    public class ArrayField
    {
        /// <summary>
        /// Field name of this array
        /// </summary>
        [XmlAttributeAttribute]
        public string FieldName
        {
            get;
            set;
        }

        /// <summary>
        /// Array items
        /// </summary>
        [XmlElementAttribute("ArrayItem")]
        public List<ArrayItem> ArrayItems
        {
            get;
            set;
        }

        /// <summary>
        /// Verify whether this array is expected
        /// </summary>
        /// <param name="complexValue">MA complex value</param>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        public bool Verify(ComplexType complexValue)
        {
            // Get the array by using field name
            Array elements;
            if (!MAVerifyUtil.GetFieldValue<Array>(FieldName, complexValue, out elements))
            {
                return false;
            }
            if (elements == null)
                return false;
            // Verify array items
            foreach (ArrayItem item in ArrayItems)
            {
                if (!ContainValidItem(elements, item))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Verify array items.
        /// </summary>
        /// <param name="elements">Array of complexType</param>
        /// <param name="item">expected item</param>
        /// <returns>If contains valid item, return TRUE, else return FALSE.</returns>
        private bool ContainValidItem(Array elements, ArrayItem item)
        {
            
            foreach (Object element in elements)
            {
                if (item.Verify(element))
                    return true;
            }
            return false;
        }

    }

    /// <summary>
    /// Expected Array Item of expected array
    /// </summary>
    public class ArrayItem
    {
        /// <summary>
        /// Verify list
        /// </summary>
        [XmlElementAttribute(IsNullable = true)]
        public VerifyItemList VerifyItemList
        {
            get;
            set;
        }

        /// <summary>
        /// A list of StructureField
        /// </summary>
        [XmlElementAttribute("StructureField", IsNullable = true)]
        public List<StructureField> Structures
        {
            get;
            set;
        }

        /// <summary>
        /// A list of ArrayField
        /// </summary>
        [XmlElementAttribute("ArrayField", IsNullable = true)]
        public List<ArrayField> Arrays
        {
            get;
            set;
        }

        /// <summary>
        /// Verify whether this array item is expected
        /// </summary>
        /// <param name="complexValue">complexValue from array</param>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        public bool Verify(Object value)
        {
            // Verify verify list
            if (VerifyItemList != null)
            {
                if (!VerifyItemList.Verify(value))
                    return false;
            }

            if (value is ComplexType)
            {
                ComplexType complexValue = (ComplexType)value;
                // Veiry structures
                if (Structures != null)
                {
                    foreach (StructureField structure in Structures)
                    {
                        if (!structure.Verify(complexValue))
                            return false;
                    }
                }

                // Verify Arrays
                if (Arrays != null)
                {
                    foreach (ArrayField array in Arrays)
                    {
                        if (!array.Verify(complexValue))
                            return false;
                    }
                }
            }
            return true;
        }
    }


    /// <summary>
    /// Verify Item list
    /// </summary>
    public class VerifyItemList
    {
        /// <summary>
        /// Verify operation, and/or
        /// </summary>
        [XmlAttributeAttribute]
        public string Operation
        {
            get;
            set;
        }

        /// <summary>
        /// Verify Items
        /// </summary>
        [XmlElementAttribute("VerifyItem", IsNullable = true)]
        public List<VerifyItem> VerifyItems
        {
            get;
            set;
        }

        /// <summary>
        /// sub Verify List
        /// </summary>
        [XmlElementAttribute("VerifyItemList", IsNullable = true)]
        public List<VerifyItemList> VerifyItemLists
        {
            get;
            set;
        }

        /// <summary>
        /// Structures need to verify 
        /// </summary>
        [XmlElementAttribute("StructureField", IsNullable = true)]
        public List<StructureField> Structures
        {
            get;
            set;
        }

        /// <summary>
        /// Arrays need to verify
        /// </summary>
        [XmlElementAttribute("ArrayField", IsNullable = true)]
        public List<ArrayField> Arrays
        {
            get;
            set;
        }

        /// <summary>
        /// Verify whether a complexValue applies this verifyList
        /// </summary>
        /// <param name="complexValue">Specifies a complexValue to be verified.</param>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        public bool Verify(Object value)
        {
            if (Operation.ToLower(CultureInfo.InvariantCulture).Equals("and"))
            {
                // Process AND Verifycation
                return VerifyAnd(value);
            }
            else if (Operation.ToLower(CultureInfo.InvariantCulture).Equals("or"))
            {
                // Process OR verification
                return VerifyOr(value);
            }
            else
                return false;
        }

        /// <summary>
        /// Process AND verification
        /// </summary>
        /// <param name="complexValue">Specifies a complexValue to be verified.</param>
        /// <returns></returns>
        private bool VerifyAnd(Object value)
        {
            // Verify the verify items in this list
            foreach (VerifyItem item in VerifyItems)
            {
                if (!item.Verify(value))
                    return false;
            }
            
            // Verify the sub verifylist
            if (VerifyItemLists != null)
            {
                foreach (VerifyItemList itemList in VerifyItemLists)
                {
                    if (!itemList.Verify(value))
                        return false;
                }
            }
                        
            if (value is ComplexType)
            {
                // structures and arrays can existed only when value is a complextype
                ComplexType complexValue = (ComplexType)value;
                // Verify structures
                if (Structures != null)
                {
                    foreach (StructureField structure in Structures)
                    {
                        if (!structure.Verify(complexValue))
                            return false;
                    }
                }
                
                // Verify arrays
                if (Arrays != null)
                {
                    foreach (ArrayField array in Arrays)
                    {
                        if (!array.Verify(complexValue))
                            return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Process OR verification
        /// </summary>
        /// <param name="complexValue">Specifies a complexValue to be verified.</param>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        private bool VerifyOr(Object value)
        {
            // Verify the verify items in this list
            foreach (VerifyItem item in VerifyItems)
            {
                if (item.Verify(value))
                    return true;
            }

            // Verify the sub verifylist
            if (VerifyItemLists != null)
            {
                foreach (VerifyItemList itemList in VerifyItemLists)
                {
                    if (itemList.Verify(value))
                        return true;
                }
            }

            if (value is ComplexType)
            {
                // structures and arrays can existed only when value is a complextype
                ComplexType complexValue = (ComplexType)value;
                // Verify structures
                if (Structures != null)
                {
                    foreach (StructureField structure in Structures)
                    {
                        if (structure.Verify(complexValue))
                            return true;
                    }
                }

                // Verify arrays
                if (Arrays != null)
                {
                    foreach (ArrayField array in Arrays)
                    {
                        if (array.Verify(complexValue))
                            return true;
                    }
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Verify Item
    /// </summary>
    public class VerifyItem
    {
        /// <summary>
        /// Operation: equal, notequal, flagset
        /// </summary>
        [XmlAttributeAttribute]
        public string Operation
        {
            get;
            set;
        }

        /// <summary>
        /// Field name
        /// </summary>
        [XmlAttributeAttribute]
        public string FieldName
        {
            get;
            set;
        }

        /// <summary>
        /// Expected value
        /// </summary>
        [XmlAttributeAttribute]
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// value type
        /// </summary>
        [XmlAttributeAttribute]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Verify whether the value applies this verify item
        /// </summary>
        /// <param name="complexValue">MA complexValue</param>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        public bool Verify(Object value)
        {
            return MAVerifyUtil.VerifyValue(value, this);
        }
    }
}
