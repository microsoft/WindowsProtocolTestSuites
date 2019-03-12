// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics.CodeAnalysis;
namespace Microsoft.Protocols.TestManager.Kernel
{
    [XmlRoot]
    public class TestCaseFilter : List<RuleGroup>
    {
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "By Design")]
        public static TestCaseFilter LoadFromXml(XmlNode RuleDefinitions)
        {
            TestCaseFilter filter = new TestCaseFilter();
            var groups = RuleDefinitions.SelectNodes("Group");
            foreach (XmlNode group in groups)
            {
                RuleGroup gp = RuleGroup.FromXmlNode(group);
                filter.Add(gp);
            }
            return filter;
        }

        /// <summary>
        /// Save the profile.
        /// </summary>
        /// <param name="stream">Profile stream</param>
        /// <returns>Indicates this operations success or fail</returns>
        public bool SaveProfile(Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream);
            writer.WriteStartDocument();
            writer.WriteStartElement("RuleGroups");

            foreach (RuleGroup ruleGroup in this)
            {
                writer.WriteStartElement("RuleGroup");
                writer.WriteAttributeString("name", ruleGroup.Name);
                Stack<Rule> ruleStack = new Stack<Rule>();
                Stack<string> path = new Stack<string>();
                foreach (Rule rule in ruleGroup)
                {
                    ruleStack.Push(rule);
                    path.Push(ruleGroup.Name);
                }
                while (ruleStack.Count > 0)
                {
                    Rule topRule = ruleStack.Pop();
                    string parent = path.Pop();
                    if (topRule.SelectStatus != RuleSelectStatus.UnSelected && topRule.Count == 0)
                    {
                        // Normalize rule name by removing dot since FindRuleByName will use dot as split delimiter
                        writer.WriteStartElement("Rule");
                        writer.WriteAttributeString("name", parent + "." + topRule.Name.Replace(".", string.Empty));
                        writer.WriteEndElement();
                    }
                    foreach (Rule childRule in topRule)
                    {
                        ruleStack.Push(childRule);
                        path.Push(parent + "." + topRule.Name);
                    }
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            return true;
        }

        public bool LoadProfile(Stream stream)
        {
            XmlDocument ruleGroupFile = new XmlDocument();
            ruleGroupFile.XmlResolver = null;
            UnselectAllRules();
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.XmlResolver = null;
                settings.DtdProcessing = DtdProcessing.Prohibit;
                XmlReader xmlReader = XmlReader.Create(stream, settings);
                ruleGroupFile.Load(xmlReader);
                foreach (XmlNode ruleGroupNode in ruleGroupFile.DocumentElement.SelectNodes("RuleGroup"))
                {
                    foreach (XmlNode ruleNode in ruleGroupNode.SelectNodes("Rule"))
                    {
                        string ruleName = ruleNode.Attributes["name"].Value;
                        Rule rule = FindRuleByName(ruleName);
                        if (rule != null)
                        {
                            // When loading profile, do not refresh feature mapping table
                            UpdateLoadingProfileFlag(rule.Name);
                            rule.SelectStatus = RuleSelectStatus.Selected;
                        }
                    }
                }
            }

            catch (XmlException e)
            {
                throw new XmlException(string.Format(StringResource.LoadProfileError, e.Message));
            }
            //Apply rules
            return true;
        }

        private void UpdateLoadingProfileFlag(string ruleName)
        {
            foreach (RuleGroup ruleGroup in this)
            {
                Stack<Rule> ruleStack = new Stack<Rule>();
                foreach (Rule r in ruleGroup) ruleStack.Push(r);
                while (ruleStack.Count > 0)
                {
                    Rule r = ruleStack.Pop();
                    if (r.Name.Contains(ruleName))
                    {
                        ruleGroup.isLoadingProfile = true;
                        return;
                    }
                    foreach (Rule childRule in r) ruleStack.Push(childRule);
                }
            }
        }

        public void UnselectAllRules()
        {
            foreach (RuleGroup ruleGroup in this)
            {
                ruleGroup.SelectStatus = RuleSelectStatus.UnSelected;
            }
        }
        public new void Add(RuleGroup group)
        {
            group.ContentModified += group_SelectionChanged;
            base.Add(group);
        }

        void group_SelectionChanged()
        {
            if (ContentModified != null) ContentModified();
        }
        public Rule FindRuleByName(string RuleName)
        {
            string[] ruleParts = RuleName.Split('.');
            int level = 0;
            foreach (RuleGroup ruleGroup in this)
            {
                if (ruleGroup.Name == ruleParts[0])
                {
                    level = 1;
                    Stack<Rule> ruleStack = new Stack<Rule>();
                    foreach (Rule childRule in ruleGroup)
                    {
                        ruleStack.Push(childRule);
                    }
                    while (ruleStack.Count != 0)
                    {
                        Rule topRule = ruleStack.Pop();
                        if (topRule.Name == ruleParts[level])
                        {
                            level++;
                            if (level == ruleParts.Length)
                            {
                                return topRule;
                            }
                            foreach (Rule childRule in topRule)
                            {
                                ruleStack.Push(childRule);
                            }
                        }
                    }
                }
            }
            return null;
        }

        public ContentModifiedEventHandler ContentModified;

        public List<TestCase> FilterTestCaseList(List<TestCase> inputList)
        {
            List<TestCase> result = new List<TestCase>();
            List<Filter> filters = new List<Filter>();
            foreach (RuleGroup g in this)
            {
                filters.Add(new Filter(g.GetCategories(g.RuleGroupType == RuleType.Selector), g.RuleGroupType));
            }
            foreach (var item in inputList)
            {
                foreach (Filter f in filters)
                {
                    if (!f.FilterTestCase(item.Category)) goto NotSelected;
                }
                result.Add(item);
            NotSelected:
                continue;
            }
            return result;
        }

        public List<string> GetRuleList(bool IsSelected)
        {
            List<string> ruleList = new List<string>();

            foreach (RuleGroup g in this)
            {
                ruleList = ruleList.Concat(g.GetSelectedRules(IsSelected)).ToList();
            }
            return ruleList;
        }

        public string GetFilterExpression()
        {
            StringBuilder sb = new StringBuilder();

            bool andMark = false;
            foreach (RuleGroup g in this)
            {
                if (g.RuleGroupType == RuleType.Selector)
                {
                    bool orMark = false;
                    var categories = g.GetCategories(true);
                    if (categories.Count == 0) continue;
                    if (andMark) sb.Append("&");
                    sb.Append("(");
                    foreach(string category in categories)
                    {
                        if (orMark) sb.Append("|");
                        if(category[0] != '!')
                            sb.AppendFormat("TestCategory={0}", category);
                        else
                            sb.AppendFormat("TestCategory!={0}", category.Substring(1));
                        orMark = true;
                    }
                    sb.Append(")");
                }
                else if (g.RuleGroupType == RuleType.Remover)
                {

                    bool smallAndMark = false;
                    var categories = g.GetCategories(false);
                    if (categories.Count == 0) continue;
                    if (andMark) sb.Append("&");
                    sb.Append("(");
                    foreach (string category in categories)
                    {
                        if (smallAndMark) sb.Append("&");
                        if(category[0] != '!')
                            sb.AppendFormat("TestCategory!={0}", category);
                        else
                            sb.AppendFormat("TestCategory={0}", category.Substring(1));
                        smallAndMark = true;
                    }
                    sb.Append(")");
                }
                andMark = true;
            }

            return sb.ToString();

        }
    }

}
