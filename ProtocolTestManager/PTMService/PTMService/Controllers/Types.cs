// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using Microsoft.Protocols.TestManager.PTMService.PTMKernelService;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers
{
    public class TestSuite
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public bool Removed { get; set; }

        public TestCaseInfo[] TestCases { get; set; }
    }

    public class Configuration
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int TestSuiteId { get; set; }

        public string Description { get; set; }

        public bool IsConfigured { get; set; }
    }

    public class Rule
    {
        public bool? IsSelected { get; set; }

        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string[] Categories { get; set; }

        public Rule[]? Rules { get; set; }

        public string[]? MappingRules { get; set; }
    }

    public class RuleGroup
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }

        public Rule[] Rules { get; set; }

        public static RuleGroup[] FromKernalRuleGroups(IEnumerable<Common.Types.RuleGroup> ruleGroups)
        {
            List<RuleGroup> groups = new List<RuleGroup>();
            if (ruleGroups != null)
            {
                foreach (var g in ruleGroups)
                {
                    var rg = new RuleGroup()
                    {
                        DisplayName = g.DisplayName,
                        Name = g.Name,
                    };

                    if (g.Rules.Count > 0)
                    {
                        var drList = new List<Rule>();
                        AddRules(g.Rules, drList);
                        rg.Rules = drList.ToArray();
                    }
                    groups.Add(rg);
                }
            }

            return groups.ToArray();
        }

        public static RuleGroup[] UpdateByMappingTable(RuleGroup[] ruleGroups, int mappingIndex, Dictionary<string, List<Common.Types.Rule>> mappingTable, IEnumerable<Common.Types.RuleGroup> selectedRules = null)
        {
            if (mappingIndex == -1)
            {
                return ruleGroups;
            }

            List<RuleGroup> groups = new List<RuleGroup>();
            if (ruleGroups != null)
            {
                for (int i = 0; i < ruleGroups.Count(); i++)
                {
                    var g = ruleGroups.ElementAt(i);
                    if (i == mappingIndex)
                    {
                        UpdateRulesByMappingTable(g.Name, g.Name, g.Rules, mappingTable, selectedRules);
                    }
                    groups.Add(g);
                }
            }

            return groups.ToArray();
        }

        public static IEnumerable<Common.Types.RuleGroup> ToKernalRuleGroups(RuleGroup[] ruleGroups)
        {
            List<Common.Types.RuleGroup> groups = new List<Common.Types.RuleGroup>();
            foreach (var g in ruleGroups)
            {
                if (g.Rules.Length == 0)
                    continue;

                var rg = new Common.Types.RuleGroup()
                {
                    DisplayName = g.DisplayName,
                    Name = g.Name,
                };

                var drList = new List<Common.Types.Rule>();
                AddKernalRules(g.Rules, drList);
                rg.Rules = drList.ToArray();

                groups.Add(rg);
            }

            return groups.ToArray();
        }

        private static void AddRules(IList<Common.Types.Rule> rules, List<Rule> displayRule)
        {
            foreach (var r in rules)
            {
                Rule dr = new Rule()
                {
                    DisplayName = r.DisplayName,
                    Name = r.Name,
                    Categories = r.Categories,
                    IsSelected = (r.SelectStatus == RuleSelectStatus.Selected) ? true : (r.SelectStatus == RuleSelectStatus.UnSelected) ? false : null,
                };
                if (r.Count > 0)
                {
                    var drList = new List<Rule>();
                    AddRules(r, drList);
                    dr.Rules = drList.ToArray();
                }

                displayRule.Add(dr);
            }
        }

        private static void UpdateRulesByMappingTable(string groupName, string parentRuleName, IList<Rule> rules, Dictionary<string, List<Common.Types.Rule>> mappingTable, IEnumerable<Common.Types.RuleGroup> selectedRules = null)
        {
            if (rules == null)
            {
                return;
            }

            foreach (var r in rules)
            {
                if (r.Categories == null)
                {
                    continue;
                }
                if (r.Categories.Length > 0 && mappingTable.ContainsKey(r.Categories.FirstOrDefault()))
                {
                    var mappingValues = mappingTable.GetValueOrDefault(r.Categories.FirstOrDefault());
                    var hiddenNodes = mappingValues.Select(x => x.Name).Distinct();
                    r.MappingRules = hiddenNodes.ToArray();
                    // Make it be compatible to PTMGUI exported profile.xml
                    var newSelectedHiddenNodes = mappingValues.Where(i => i.SelectStatus != RuleSelectStatus.UnSelected).Select(x => x.Name).Distinct();
                    if (newSelectedHiddenNodes.Count() > 0 && !r.Name.Contains('%') && selectedRules != null)
                    {
                        var selectedRule = selectedRules.Where(i => i.Name == groupName).FirstOrDefault();
                        if (selectedRule != null)
                        {
                            var originalRules = selectedRule.Rules;
                            var rule = selectedRule.Rules.Where(i => i.Name.Contains(parentRuleName) && (i.Name.Contains('.') ? i.Name.Remove(0, i.Name.LastIndexOf('.') + 1) : i.Name) == r.Name && i.Categories.OrderBy(s => s).SequenceEqual(r.Categories.OrderBy(s => s))).FirstOrDefault();
                            if (rule != null)
                            {
                                originalRules.Remove(rule);
                                foreach (var mapRule in newSelectedHiddenNodes)
                                {
                                    Common.Types.Rule newRule = new Common.Types.Rule()
                                    {
                                        Name = rule.Name,
                                        DisplayName = rule.DisplayName,
                                        Categories = rule.Categories,
                                        SelectStatus = rule.SelectStatus
                                    };
                                    newRule.Name = rule.Name + '%' + mapRule;
                                    originalRules.Add(newRule);
                                }
                            }
                        }
                    }
                }
                if (r.Rules != null)
                {
                    UpdateRulesByMappingTable(groupName, r.Name, r.Rules, mappingTable, selectedRules);
                }
            }
        }

        private static void AddKernalRules(IList<Rule> rules, List<Common.Types.Rule> kernalRules)
        {
            foreach (var r in rules)
            {
                Common.Types.Rule dr = new Common.Types.Rule()
                {
                    DisplayName = r.DisplayName,
                    Name = r.Name,
                    Categories = r.Categories,
                    SelectStatus = r.IsSelected == null ? RuleSelectStatus.Partial : r.IsSelected == true ? RuleSelectStatus.Selected : RuleSelectStatus.UnSelected,
                };
                if (r.Rules != null && r.Rules.Length > 0)
                {
                    var drList = new List<Common.Types.Rule>();
                    AddKernalRules(r.Rules, drList);
                    dr.AddRange(drList);
                }

                kernalRules.Add(dr);
            }
        }
    }

    public class TestSuiteRules
    {
        public RuleGroup[] AllRules { get; set; }

        public RuleGroup[] SelectedRules { get; set; }

        public int TargetFilterIndex { get; set; }
        public int MappingFilterIndex { get; set; }
    }

    public class PropertyGetItem
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string[] Choices { get; set; }

        public string Description { get; set; }
    }

    public class PropertyGetItemGroup
    {
        public string Name { get; set; }

        public PropertyGetItem[] Items { get; set; }
    }

    public class PropertySetItem
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }

    public class PropertySetItemGroup
    {
        public string Name { get; set; }

        public PropertySetItem[] Items { get; set; }
    }

    public class TestResultItem
    {
        public TestResultOverview Overview { get; set; }

        public TestCaseOverview[] Results { get; set; }
    }
}
