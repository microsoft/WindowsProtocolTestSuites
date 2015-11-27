// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Xml;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Represents a property in a PTFConfig file.
    /// </summary>
    public class PtfProperty : List<PtfProperty>, INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor of PtfProperty.
        /// </summary>
        public PtfProperty()
        {
        }


        /// <summary>
        /// Create Confignode from Property node in a ptfconfig file.
        /// </summary>
        /// <param name="xmlNode">The node of the property.</param>
        /// <param name="isGroup">Whether the node is a group node.</param>
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "By Design")]
        public PtfProperty(XmlNode xmlNode, bool isGroup)
        {
            RefXmlNode = xmlNode;
            Name = xmlNode.Attributes["name"].Value;
            if (isGroup)
            {
                //Description
                if (xmlNode.Attributes["description"] != null)
                {
                    Description = xmlNode.Attributes["description"].Value;
                }
                ValueType = PtfPropertyType.Group;
            }
            else
            {
                //Value
                Value = xmlNode.Attributes["value"].Value;
                foreach (XmlNode xn in xmlNode.ChildNodes)
                {
                    //Description
                    if (xn.Name == "Description")
                    {
                        if (xn.FirstChild != null) Description = xn.FirstChild.Value;
                        else Description = "";
                    }
                    //Type
                    else if (xn.Name == "Type")
                    {
                        switch (xn.FirstChild.Value.ToUpper())
                        {
                            case "PASSWORD": ValueType = PtfPropertyType.Password; break;
                            case "BOOL": ValueType = PtfPropertyType.Bool; break;
                            case "INTEGER": ValueType = PtfPropertyType.Integer; break;
                            case "STRING": ValueType = PtfPropertyType.String; break;
                            //may through exception
                            default: break;
                        }
                    }
                    //ValueType
                    else if (xn.Name == "Choice")
                    {
                        ChoiceItems = new List<string>(xn.FirstChild.Value.Split(','));
                        for (int k = 0; k < ChoiceItems.Count; k++)
                        {
                            ChoiceItems[k] = ChoiceItems[k].Trim();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Property type.
        /// </summary>
        public PtfPropertyType Type { get; set; }

        /// <summary>
        /// Name of property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of property.
        /// </summary>
        public string Description { get; set; }

        private string _defaultValue;

        /// <summary>
        /// The default value of property.
        /// </summary>
        public string DefaultValue
        {
            get
            {
                return _defaultValue;
            }
            set
            {
                _defaultValue = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("DefaultValue"));
            }
        }

        //Reference to XmlNode of different Xml file and record wich file to save changes
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "By Design")]
        public XmlNode RefXmlNode { get; set; }

        //Property value
        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                //Save changes directly back to refXmlNode
                if (RefXmlNode != null)
                {
                    RefXmlNode.Attributes["value"].Value = value;
                }
                _value = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public List<string> _choiceItems;
        public List<string> ChoiceItems
        {
            get { return _choiceItems; }
            set
            {
                _choiceItems = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ChoiceItems"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ControlType"));
                }
            }
        }

        public PtfPropertyType ValueType { get; set; }

        public PtfProperty FindChildByName(string name)
        {
            if (Count == 0) return null;
            foreach (PtfProperty cn in this)
            {
                if (cn.Name == name)
                    return cn;
            }
            return null;
        }

        public override string ToString()
        {
            if (ValueType == PtfPropertyType.Group)
            {
                return string.Format("Group {0}", Name);
            }
            return string.Format("Property {0}={1}", Name, Value);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// The type of the property value.
    /// </summary>
    public enum PtfPropertyType { Bool, Integer, String, Password, Group }
}
