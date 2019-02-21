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

        /// <summary>
        /// RuleGroup Description.
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// RuleGroup Type.
        /// </summary>
        public RuleType RuleGroupType { set; get; }

        /// <summary>
        /// Set to true when loading profile.
        /// </summary>
        public bool isLoadingProfile { set; get; }

        /// <summary>
        /// A table which helps lookup rules in mapping filter by category name in target filter
        /// Key: category name of a rule in target filter
        /// Value: list of rules in mapping filter
        /// </summary>
        public Dictionary<string, List<Rule>> featureMappingTable = null;

        /// <summary>
        /// A RuleGroup defined as a mapping filter
        /// </summary>
        public RuleGroup mappingRuleGroup = null;

        /// <summary>
        /// A table which helps lookup rules in target filter by category name in mapping filter
        /// Key: category name of a rule in mapping filter
        /// Value: list of rules in target filter
        /// </summary>
        public Dictionary<string, List<Rule>> reverseFeatureMappingTable = null;

        /// <summary>
        /// RuleGroup with index specified in config.xml
        /// </summary>
        public RuleGroup targetRuleGroup = null;

        /// <summary>
        /// Status of RefreshFeatureMapping
        /// </summary>
        public enum RefreshStatus : Int16
        {
            /// <summary>
            /// Current task is done
            /// </summary>
            Done = 0x0000,

            /// <summary>
            /// Current task is Refreshing Feature Mapping
            /// </summary>
            FeatureMapping = 0x0001,

            /// <summary>
            /// Current task is Refreshing Reverse Feature Mapping
            /// </summary>
            ReverseFeatureMapping = 0x0002,
        }

        /// <summary>
        /// Status of refreshing feature mapping
        /// </summary>
        public RefreshStatus refreshStatus;

        /// <summary>
        /// Refresh feature mapping from target filter ruleGroup to mapping filter ruleGroup
        /// </summary>
        private void RefreshFeatureMapping()
        {
            // Not to refresh feature mapping when loading profile or the result of filtering cases will be changed
            if (!isLoadingProfile)
            {
                // If feature mapping table exists and we are updating from target filter to mapping filter
                if (featureMappingTable != null && !refreshStatus.HasFlag(RefreshStatus.ReverseFeatureMapping))
                {
                    refreshStatus |= RefreshStatus.FeatureMapping;

                    // Get selected categories only to avoid refreshing for rule with partial selection
                    List<string> categories = GetSelectedCategories(RuleGroupType == RuleType.Selector);

                    // Unselect all features in mappingFilter
                    mappingRuleGroup.SelectStatus = RuleSelectStatus.UnSelected;

                    // Select mapping features in mappingFilter based on featureMappingTable
                    foreach (string category in categories)
                    {
                        if (featureMappingTable.ContainsKey(category))
                        {
                            List<Rule> ruleList = featureMappingTable[category];
                            foreach (Rule rule in ruleList)
                            {
                                // Check selection status from reverse feature mapping table
                                string key = rule.CategoryList[0];
                                RuleSelectStatus currentSelectStatus = RuleSelectStatus.Selected;
                                foreach (var r in mappingRuleGroup.reverseFeatureMappingTable[key])
                                {
                                    if (r.SelectStatus == RuleSelectStatus.UnSelected)
                                    {
                                        currentSelectStatus = RuleSelectStatus.Partial;
                                        break;
                                    }
                                }
                                rule.SelectStatus = currentSelectStatus;
                            }
                        }
                    }
                    refreshStatus &= ~RefreshStatus.FeatureMapping;
                }
                // If reverse feature mapping table exists and we are updating from mapping filter to target filter
                else if (reverseFeatureMappingTable != null && !targetRuleGroup.refreshStatus.HasFlag(RefreshStatus.FeatureMapping))
                {
                    List<string> categories = GetCategories(RuleGroupType == RuleType.Selector);

                    targetRuleGroup.refreshStatus |= RefreshStatus.ReverseFeatureMapping;

                    // Unselect all features in targetFilter
                    targetRuleGroup.SelectStatus = RuleSelectStatus.UnSelected;

                    // Set select status to partial for rule in target filter
                    foreach (string category in categories)
                    {
                        if (reverseFeatureMappingTable.ContainsKey(category))
                        {
                            List<Rule> ruleList = reverseFeatureMappingTable[category];

                            foreach (Rule rule in ruleList)
                            {
                                // Check selection status from feature mapping table
                                bool noCaseSelected = true;
                                RuleSelectStatus currentSelectStatus = RuleSelectStatus.Selected;
                                string key = rule.CategoryList[0];
                                foreach (var r in targetRuleGroup.featureMappingTable[key])
                                {
                                    if (r.SelectStatus == RuleSelectStatus.UnSelected)
                                    {
                                        currentSelectStatus = RuleSelectStatus.Partial;
                                    }
                                    else
                                    {
                                        // RuleSelectStatus.Selected or RuleSelectStatus.Partial
                                        noCaseSelected = false;
                                    }
                                }
                                if (noCaseSelected)
                                {
                                    rule.SelectStatus = RuleSelectStatus.UnSelected;
                                }
                                else
                                {
                                    rule.SelectStatus = currentSelectStatus;
                                }
                            }
                        }
                    }
                    targetRuleGroup.refreshStatus &= ~RefreshStatus.ReverseFeatureMapping;
                }
            }
            isLoadingProfile = false;
        }

        private RuleSelectStatus selectStatus;
        public RuleSelectStatus SelectStatus
        {
            set
            {
                if (selectStatus != value)
                {
                    ChangeSelectStatus(value);
                    if (ContentModified != null)
                    {
                        RefreshFeatureMapping();
                        ContentModified();
                    }
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

        public RuleGroup()
        {
            SelectStatus = RuleSelectStatus.UnSelected;
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
                if (ContentModified != null)
                {
                    RefreshFeatureMapping();
                    ContentModified();
                }
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
                if ((r.SelectStatus != RuleSelectStatus.UnSelected && IsSelected) ||
                    (r.SelectStatus == RuleSelectStatus.UnSelected && !IsSelected))
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

        /// <summary>
        /// Only get categories from selected rule
        /// </summary>
        /// <param name="IsSelected"></param>
        /// <returns></returns>
        public List<string> GetSelectedCategories(bool IsSelected)
        {
            List<string> categories = new List<string>();
            Stack<Rule> rulestack = new Stack<Rule>();
            foreach (Rule r in this) rulestack.Push(r);
            while (rulestack.Count > 0)
            {
                Rule r = rulestack.Pop();
                if ((r.SelectStatus != RuleSelectStatus.UnSelected && IsSelected) ||
                    (r.SelectStatus == RuleSelectStatus.UnSelected && !IsSelected))
                {
                    foreach (string cat in r.CategoryList)
                    {
                        if (r.SelectStatus == RuleSelectStatus.Selected)
                        {
                            if (!categories.Contains(cat)) categories.Add(cat);
                        }
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
