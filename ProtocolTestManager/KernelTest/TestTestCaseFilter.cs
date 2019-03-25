// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestManager.Kernel;
using System.Collections.Generic;
using System.IO;
namespace CodeCoverage
{
    [TestClass]
    public class TestTestCaseFilter
    {
        private TestCaseFilter CreateSampleTestFilter()
        {
            TestCaseFilter filter = new TestCaseFilter();
            RuleGroup group1 = new RuleGroup()
            {
                Name = "Group1",
                Description = "This is the first group.",
                RuleGroupType = RuleType.Selector
            };
            Rule rule1 = new Rule()
            {
                Name = "Rule1",
                SelectStatus = RuleSelectStatus.Selected
            };
            Rule rule11 = new Rule()
            {
                Name = "Rule11",
                SelectStatus = RuleSelectStatus.Selected,
                CategoryList = new System.Collections.Generic.List<string>() { "Cat1", "Cat2", "Cat3" }
            };
            Rule rule12 = new Rule()
            {
                Name = "Rule12",
                SelectStatus = RuleSelectStatus.UnSelected,
                CategoryList = new System.Collections.Generic.List<string>() { "Cat4", "Cat5", "Cat6" }
            };
            rule1.Add(rule11);
            rule1.Add(rule12);
            group1.Add(rule1);

            RuleGroup group2 = new RuleGroup()
            {
                Name = "Group2",
                Description = "This is the second group.",
                RuleGroupType = RuleType.Selector
            };
            Rule rule2 = new Rule()
            {
                Name = "Rule2",
                SelectStatus = RuleSelectStatus.UnSelected,
                CategoryList = new System.Collections.Generic.List<string>() { "Cat1", "Cat2", "Cat3" }
            };
            group2.Add(rule2);

            RuleGroup group3 = new RuleGroup()
            {
                Name = "Group3",
                Description = "This is the third group.",
                SelectStatus = RuleSelectStatus.Selected,
                RuleGroupType = RuleType.Remover
            };
            Rule rule3 = new Rule()
            {
                Name = "Rule3",
                SelectStatus = RuleSelectStatus.Selected,
                CategoryList = new System.Collections.Generic.List<string>() { "Cat7", "Cat8" }
            };
            group3.Add(rule3);

            filter.Add(group1);
            filter.Add(group2);
            filter.Add(group3);

            rule11.SelectStatus = RuleSelectStatus.Selected;
            rule3.SelectStatus = RuleSelectStatus.UnSelected;
            return filter;
        }

        [TestMethod]
        public void SaveTestFilterProfile()
        {
            TestCaseFilter filter = CreateSampleTestFilter();
            using (var stream = new FileStream("test.profile", FileMode.Create))
            {
                filter.SaveProfile(stream);
            }

            //Manual verify the profile
        }

        [TestMethod]
        public void TestFindARuleByName()
        {
            TestCaseFilter filter = CreateSampleTestFilter();

            Rule rule = filter.FindRuleByName("Group1.Rule1.Rule11");

            Assert.IsNotNull(rule, "Find rule: Group1.Rule1.");
            Assert.AreEqual("Rule11", rule.Name, "Verify the name of the rule.");
        }

        [TestMethod]
        public void TestFindARuleByName_NotFound()
        {
            TestCaseFilter filter = CreateSampleTestFilter();

            Rule rule = filter.FindRuleByName("Group1.Rule1.NonExist");

            Assert.IsNull(rule, "Non-exist rule: Group1.Rule1.NonExis.");
        }

        [TestMethod]
        public void LoadRulesFromConfigFile()
        {
            AppConfig appConfig = AppConfig.LoadConfig("TestAppConfig", "0.0.0.0", @".\Resources", @".\Resources");
            var filter = TestCaseFilter.LoadFromXml(appConfig.RuleDefinitions);
            Rule Rule1 = filter.FindRuleByName("ACategory.Rule1");
            Assert.IsNotNull(Rule1, "Rule1 is found.");
            Rule Rule2 = filter.FindRuleByName("ACategory.Rule2");
            Assert.IsNotNull(Rule2, "Rule2 is found.");
            Assert.AreEqual(2, Rule2.CategoryList.Count, "Verify the categories in Rule2");
            Assert.IsTrue(Rule2.CategoryList.Contains("Cat21"), "Find Cat21.");
            Assert.IsTrue(Rule2.CategoryList.Contains("Cat22"), "Find Cat22.");
            Rule Rule32 = filter.FindRuleByName("ACategory.Rule3.Rule31");
            Assert.IsNotNull(Rule1, "Rule31 is found.");
        }

        [TestMethod]
        public void LoadProfile()
        {
            TestCaseFilter filter = CreateSampleTestFilter();
            using (var stream = new FileStream(@"Resources\TestCaseFilter_Load.profile", FileMode.Open))
            {
                filter.LoadProfile(stream);
            }

            Assert.AreEqual(
                RuleSelectStatus.Partial,
                filter[0].SelectStatus,
                "The group is partial selected");
            Assert.AreEqual(
                RuleSelectStatus.Selected,
                filter[2].SelectStatus,
                "The group is selected");
            Rule selectedRule = filter.FindRuleByName("Group1.Rule1.Rule12");
            Assert.AreEqual(
                RuleSelectStatus.Selected,
                selectedRule.SelectStatus,
                "The rule is selected");
            Rule notSelectedRule = filter.FindRuleByName("Group1.Rule1.Rule11");
            Assert.AreEqual(
                RuleSelectStatus.UnSelected,
                notSelectedRule.SelectStatus,
                "The rule is not selected");
        }

        [TestMethod]
        public void TestGetSelectedRules()
        {
            TestCaseFilter filter = CreateSampleTestFilter();
            List<string> rules = filter.GetRuleList(true);
            Assert.IsTrue(
                rules.Contains("Group1.Rule1.Rule11"),
                "Group1.Rule1.Rule11 is selected.");
        }

        [TestMethod]
        public void TestFilterExpression()
        {
            TestCaseFilter filter = CreateSampleTestFilter();
            string filterExp = filter.GetFilterExpression();
            Assert.AreEqual(
                "(TestCategory=Cat1|TestCategory=Cat2|TestCategory=Cat3)&(TestCategory!=Cat7&TestCategory!=Cat8)",
                filterExp,
                "Verify filter expression");
        }

        [TestMethod]
        public void TestFilterNotCondition()
        {
            TestCaseFilter filter = new TestCaseFilter();
            RuleGroup group1 = new RuleGroup()
            {
                Name = "Group1",
                Description = "This is the first group.",
                RuleGroupType = RuleType.Selector
            };
            Rule rule11 = new Rule()
            {
                Name = "Rule11",
                SelectStatus = RuleSelectStatus.Selected,
                CategoryList = new System.Collections.Generic.List<string>() { "Cat1", "Cat2", "Cat3" }
            };
            Rule rule12 = new Rule()
            {
                Name = "Rule12",
                SelectStatus = RuleSelectStatus.Selected,
                CategoryList = new System.Collections.Generic.List<string>() { "Cat4", "Cat5", "Cat6" }
            };
            group1.Add(rule11);
            group1.Add(rule12);

            RuleGroup group2 = new RuleGroup()
            {
                Name = "Group2",
                Description = "This is the second group.",
                RuleGroupType = RuleType.Selector
            };
            Rule rule2 = new Rule()
            {
                Name = "Rule2",
                SelectStatus = RuleSelectStatus.Selected,
                CategoryList = new System.Collections.Generic.List<string>() { "!Cat1", "!Cat2", "Cat3" }
            };
            group2.Add(rule2);
            filter.Add(group1);
            filter.Add(group2);

            string filterExp = filter.GetFilterExpression();
            Assert.AreEqual(
                "(TestCategory=Cat4|TestCategory=Cat5|TestCategory=Cat6|TestCategory=Cat1|TestCategory=Cat2|TestCategory=Cat3)&(TestCategory!=Cat1|TestCategory!=Cat2|TestCategory=Cat3)",
                filterExp,
                "Verify filter expression with NOT condition.");

        }
    }
}
