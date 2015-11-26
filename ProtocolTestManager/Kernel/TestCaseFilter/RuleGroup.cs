// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This class defines rule group.
    /// </summary>
    public class RuleGroup : List<Rule>, INotifyPropertyChanged
    {
        /// <summary>
        /// RuleGroup Name.
        /// </summary>
        public string Name { set; get; }

        public string Description { set; get; }

        public RuleType RuleGroupType { set; get; }

        private RuleSelectStatus selectStatus;
        public RuleSelectStatus SelectStatus
        {
            set
            {
                if (selectStatus != value)
                {
                    ChangeSelectStatus(value);
                    if (ContentModified != null) ContentModified();
                }
            }
            get
            {
                return selectStatus;
            }
        }

        public bool? IsSelected
        {
            get
            {
                switch (SelectStatus)
                {
                    case RuleSelectStatus.NotSelected: return false;
                    case RuleSelectStatus.Selected: return true;
                    case RuleSelectStatus.Partial: return null;
                }
                return null;
            }
            set
            {
                if (value == null) SelectStatus = RuleSelectStatus.NotSelected;
                else
                    SelectStatus = (bool)value ? RuleSelectStatus.Selected : RuleSelectStatus.NotSelected;
            }
        }

        public RuleGroup()
        {
            SelectStatus =  RuleSelectStatus.NotSelected;
        }

        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "By Design")]
        public static RuleGroup FromXmlNode(XmlNode node)
        {
            RuleType ruletype = RuleType.Selector;
            var ruleTypeAttribute = node.Attributes["type"];
            if (ruleTypeAttribute != null)
                ruletype = (RuleType)Enum.Parse(typeof(RuleType), ruleTypeAttribute.Value);
            RuleGroup gp = new RuleGroup()
            {
                Name = node.Attributes["name"].Value,
                RuleGroupType = ruletype
            };
            var rules = node.SelectNodes("Rule");
            foreach (XmlNode rule in rules)
            {
                gp.Add(Rule.FromXmlNode(rule));
            }
            return gp;
        }

        public new void Add(Rule rule)
        {
            rule.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "SelectStatus") UpdateSelectStatus();
            };
            rule.ContentModified += () =>
            {
                if (ContentModified != null) ContentModified();
            };
            base.Add(rule);
            UpdateSelectStatus();
        }

        public void ChangeSelectStatus(RuleSelectStatus status)
        {
            SelectThisGroup(status);
            if (status == RuleSelectStatus.Partial) return;
            foreach (Rule rule in this)
            {
                rule.ChangeSelectStatus(status);
            }
        }

        private void SelectThisGroup(RuleSelectStatus status)
        {
            selectStatus = status;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("SelectStatus"));
                PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }

        private void UpdateSelectStatus()
        {
            if (Count == 0) return;
            RuleSelectStatus status = this[0].SelectStatus;
            for (int i = 1; i < Count; i++)
            {
                if (this[i].SelectStatus != status)
                {
                    SelectThisGroup(RuleSelectStatus.Partial);
                    return;
                }
            }
            SelectThisGroup(status);
        }

        private void SelectChildren(RuleSelectStatus status)
        {
            if (status == RuleSelectStatus.Partial) return;
            foreach (Rule rule in this)
            {
                rule.SelectStatus = status;
            }
        }

        public List<string> GetCategories(bool IsSelected)
        {
            List<string> categories = new List<string>();
            Stack<Rule> rulestack = new Stack<Rule>();
            foreach (Rule r in this) rulestack.Push(r);
            while (rulestack.Count > 0)
            {
                Rule r = rulestack.Pop();
                if ((r.SelectStatus != RuleSelectStatus.NotSelected && IsSelected) ||
                    (r.SelectStatus == RuleSelectStatus.NotSelected && !IsSelected))
                {
                    foreach (string cat in r.CategoryList)
                    {
                        if (!categories.Contains(cat)) categories.Add(cat);
                    }
                    foreach (Rule childrule in r) rulestack.Push(childrule);
                }
            }
            return categories;
        }

        public List<string> GetSelectedRules(bool IsSelected)
        {
            List<string> ruleList = new List<string>();

            Stack<Rule> ruleStack = new Stack<Rule>();
            Stack<string> nameStack = new Stack<string>();
            foreach (Rule r in this)
            {
                ruleStack.Push(r);
                nameStack.Push(Name + "." + r.Name);
            }
            while (ruleStack.Count > 0)
            {
                Rule rule = ruleStack.Pop();
                string name = nameStack.Pop();
                if (rule.Count > 0)
                {
                    foreach (Rule r in rule)
                    {
                        ruleStack.Push(r);
                        nameStack.Push(name + "." + r.Name);
                    }
                    continue;
                }
                if ((rule.SelectStatus == RuleSelectStatus.Selected && IsSelected) ||
                    (rule.SelectStatus != RuleSelectStatus.Selected && !IsSelected)
                    ) ruleList.Add(name);
            }
            return ruleList;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event ContentModifiedEventHandler ContentModified;
       
    }

    public enum RuleType { Selector, Remover };
}
