// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This class defines the filter rule.
    /// </summary>
    public class Rule :List<Rule>, INotifyPropertyChanged
    {
        /// <summary>
        /// The name of this rule.
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// The description of this rule.
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// A list of categories.
        /// </summary>
        public List<string> CategoryList{get;set;}

        private RuleSupportStatus _status;

        /// <summary>
        /// The status that this rule supported.
        /// </summary>
        public RuleSupportStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        }

        private RuleSelectStatus _selectStatus;

        /// <summary>
        /// The select status of this rule.
        /// </summary>
        public RuleSelectStatus SelectStatus
        {
            set
            {
                if (_selectStatus != value)
                {
                    ChangeSelectStatus(value);
                    if (ContentModified != null) ContentModified();
                }
            }
            get
            {
                return _selectStatus;
            }
        }

        /// <summary>
        /// Indicates this rule was selected or not.
        /// </summary>
        public bool? IsSelected
        {
            get {
                switch (SelectStatus)
                {
                    case RuleSelectStatus.UnSelected: return false;
                    case RuleSelectStatus.Selected: return true;
                    case RuleSelectStatus.Partial: return null;
                }
                return null;
            }
            set
            {
                if (value == null) SelectStatus = RuleSelectStatus.UnSelected;
                else
                    SelectStatus = (bool)value ? RuleSelectStatus.Selected : RuleSelectStatus.UnSelected;
            }
        }

        //Default value of IsSelected is false
        public Rule()
        {
            CategoryList = new List<string>();
            SelectStatus = RuleSelectStatus.UnSelected;
            Status = RuleSupportStatus.Default;
        }

        /// <summary>
        /// To change the select status of this rule.
        /// </summary>
        /// <param name="status">The status to be changed to.</param>
        public void ChangeSelectStatus(RuleSelectStatus status)
        {
            SelectThisRule(status);
            if (status == RuleSelectStatus.Partial) return;
            foreach (Rule rule in this)
            {
                rule.ChangeSelectStatus(status);
            }
        }

        private void SelectThisRule(RuleSelectStatus status)
        {
            if (_selectStatus == status) return;
            _selectStatus = status;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("SelectStatus"));
                PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }
 
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method to add rule.
        /// </summary>
        /// <param name="rule"></param>
        public new void Add(Rule rule)
        {
            rule.ContentModified += () =>
            {
                if (ContentModified != null) ContentModified();
            };
            rule.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "SelectStatus") UpdateSelectStatus();
            };
            base.Add(rule);
        }
        private void UpdateSelectStatus()
        {
            if (Count == 0) return;
            RuleSelectStatus status = this[0].SelectStatus;
            for (int i = 1; i < Count; i++)
            {
                if (this[i].SelectStatus != status)
                {
                    SelectThisRule(RuleSelectStatus.Partial);
                    return;
                }
            }
            SelectThisRule(status);
        }

        internal static Rule FromXmlNode(XmlNode ruleNode)
        {
            Rule rule = CreateFromXmlNode(ruleNode);
            Queue<Rule> rules = new Queue<Rule>();
            Queue<XmlNode> nodes = new Queue<XmlNode>();

            foreach (XmlNode r in ruleNode.SelectNodes("Rule"))
            {
                rules.Enqueue(rule);
                nodes.Enqueue(r);
            }
            while (nodes.Count > 0)
            {
                XmlNode node = nodes.Dequeue();
                Rule parent = rules.Dequeue();

                Rule rule1 = CreateFromXmlNode(node);
                parent.Add(rule1);
                foreach (XmlNode r in node.SelectNodes("Rule"))
                {
                    rules.Enqueue(rule1);
                    nodes.Enqueue(r);
                }
            }
            return rule;
        }

        private static Rule CreateFromXmlNode(XmlNode node)
        {
            Rule rule = new Rule()
            {
                // Normalize rule name by removing dot since FindRuleByName will use dot as split delimiter
                Name = node.Attributes["name"].Value.Replace(".", string.Empty)
            };
            foreach (XmlNode n in node.SelectNodes("Category"))
            {
                rule.CategoryList.Add(n.Attributes["name"].Value);
            }
            return rule;
        }

        public event ContentModifiedEventHandler ContentModified;
    }

    /// <summary>
    /// Enumerates the supportStatus of a rule.
    /// </summary>
    public enum RuleSupportStatus { Selected = 0, NotSupported = 1, Unknown = 2, Default = 3 }

    /// <summary>
    /// Enumerates the selectStatus of a rule. 
    /// </summary>
    public enum RuleSelectStatus { Selected, Partial, UnSelected }
}
