// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// PTF properties to show on the UI.
    /// </summary>
    public class PtfPropertyView : List<PtfPropertyView>, INotifyPropertyChanged
    {
        public delegate void ModifiedCallback();
        public static ModifiedCallback Modified;

        public event PropertyChangedEventHandler PropertyChanged;

        string propertyName;

        /// <summary>
        /// The property name.
        /// </summary>
        public string Name
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        /// <summary>
        /// The value of propertyNode.
        /// </summary>
        public string Value
        {
            get
            {
                return propertyNode.Value;
            }
            set
            {
                propertyNode.Value = value;
            }
        }

        public List<string> ChoiceItems
        {
            get { return propertyNode.ChoiceItems; }
        }

        /// <summary>
        /// The default value of propertyNode.
        /// </summary>
        public string DefaultValue { get { return propertyNode.DefaultValue; } }

        /// <summary>
        /// The description of propertyNode.
        /// </summary>
        public string Description { get { return propertyNode.Description; } }

        /// <summary>
        /// The tip information of propertyNode.
        /// </summary>
        public string ToolTip
        {
            get
            {
                return
                    string.Format(
                        "{0}\r\nDefault: {1}\r\n",
                        propertyNode.Name, DefaultValue, Description);
            }
        }

        public ControlType ControlType
        {
            get
            {
                if (propertyNode.ValueType == PtfPropertyType.Password) return ControlType.Password;
                if (propertyNode.ValueType == PtfPropertyType.Group) return ControlType.Group;
                if (ChoiceItems == null || ChoiceItems.Count() <= 1) return ControlType.Text;
                return ControlType.Choice;
            }
        }

        private PtfProperty propertyNode;

        /// <summary>
        /// Constructor of PtfPropertyView
        /// </summary>
        /// <param name="propertyNode">The property node which bind to the View</param>
        /// <param name="parent">Parent path</param>
        public PtfPropertyView(PtfProperty propertyNode, string parent = "")
        {
            this.propertyNode = propertyNode;
            if (string.IsNullOrEmpty(parent))
                propertyName = propertyNode.Name;
            else
                propertyName = string.Format("{0}.{1}", parent, propertyNode.Name);
            propertyNode.PropertyChanged += propertyNode_PropertyChanged;
        }

        void propertyNode_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Modified != null && e.PropertyName == "Value")
            {
                Modified();
            }
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(e.PropertyName));
            }
        }

        public override string ToString()
        {
            if (ControlType == Kernel.ControlType.Group)
            {
                return string.Format("Group {0}", Name);
            }
            return string.Format("Property {0}={1}", Name, Value);
        }

        private int Order = 0;

        /// <summary>
        /// Sorts the items using the defined order.
        /// </summary>
        /// <param name="order">Order definition</param>
        public void SortItems(Dictionary<string, int> order)
        {
            foreach (var propertyView in this)
            {
                if (order.ContainsKey(propertyView.Name))
                {
                    propertyView.Order = order[propertyView.Name];
                }
            }
            if (Count < 2) return;
            for (int i = 1; i < Count; i++)
                for (int j = 0; j < i; j++)
                {
                    if (this[j].Order > this[j + 1].Order)
                    {
                        var item = this[j];
                        this[j] = this[j + 1];
                        this[j + 1] = item;
                    }
                }
        }

        /// <summary>
        /// Specifies whether the node is an empty group.
        /// </summary>
        public bool IsEmptyGroup
        {
            get
            {
                if (propertyNode.ValueType != PtfPropertyType.Group) return false;
                if (Count == 0) return true;
                foreach (var p in this) if (!p.IsEmptyGroup) return false;
                return true;
            }
        }
    }

    public enum ControlType { Group, Text, Choice, Password }
}
