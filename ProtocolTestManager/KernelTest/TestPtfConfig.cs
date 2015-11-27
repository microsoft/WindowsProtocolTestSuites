// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestManager.Kernel;
using System.Collections.Generic;
using System.Xml;
namespace CodeCoverage
{
    [TestClass]
    public class TestPtfConfig
    {
        [TestMethod]
        public void LoadPtfConfig()
        {
            PtfConfig ptfConfig = new PtfConfig(
                new List<string>() { @"Resources\PtfConfig_Config1.ptfconfig", @"Resources\PtfConfig_Config2.ptfconfig" },
                new List<string>() { @"Resources\PtfConfig_Config1_default.ptfconfig", @"Resources\PtfConfig_Config2_default.ptfconfig" }
                );
            PtfProperty defaultGroup = ptfConfig.PtfPropertyRoot.FindChildByName("Default Group");
            Assert.IsNotNull(defaultGroup, "Default Group exists.");
            

            PtfProperty property01 = defaultGroup.FindChildByName("Property01");
            Assert.IsNotNull(property01, "Property01 exists.");
            Assert.AreEqual("value01-2", property01.Value);

            PtfProperty group01 = ptfConfig.PtfPropertyRoot.FindChildByName("Group01");
            Assert.IsNotNull(group01, "Group01 exists.");

            Assert.IsNotNull(ptfConfig.PtfPropertyRoot.FindChildByName("Group02"), "Default Group exists.");
        }

        [TestMethod]
        public void IsGroupProperty()
        {
            PtfConfig ptfConfig = new PtfConfig(
                new List<string>() { @"Resources\PtfConfig_Config1.ptfconfig", @"Resources\PtfConfig_Config2.ptfconfig" },
                new List<string>() { @"Resources\PtfConfig_Config1_default.ptfconfig", @"Resources\PtfConfig_Config2_default.ptfconfig" }
                );
            PtfProperty defaultGroup = ptfConfig.PtfPropertyRoot.FindChildByName("Default Group");
            Assert.IsTrue(defaultGroup.ValueType == PtfPropertyType.Group, "defaultGroup.IsGroup is true.");

            PtfProperty property01 = defaultGroup.FindChildByName("Property01");
            Assert.IsFalse(property01.ValueType == PtfPropertyType.Group, "property01.IsGroup is false.");

            PtfProperty group01 = ptfConfig.PtfPropertyRoot.FindChildByName("Group01");
            Assert.IsTrue(group01.ValueType == PtfPropertyType.Group, "group01.IsGroup is true.");
        }

        [TestMethod]
        public void GetPropertyByName()
        {
            PtfConfig ptfConfig = new PtfConfig(
                new List<string>() { @"Resources\PtfConfig_Config1.ptfconfig", @"Resources\PtfConfig_Config2.ptfconfig" },
                new List<string>() { @"Resources\PtfConfig_Config1_default.ptfconfig", @"Resources\PtfConfig_Config2_default.ptfconfig" }
                );
            PtfProperty p0 = ptfConfig.GetPropertyNodeByName("Group02.Property03");
            Assert.AreEqual("value03", p0.Value, "Verify property value of Group02.Property03.");

            PtfProperty p1 = ptfConfig.GetPropertyNodeByName("Property01");
            Assert.AreEqual("value01-2", p1.Value, "Verify property value of Property01.");
        }

        [TestMethod]
        public void HideProperties()
        {
            PtfConfig ptfConfig = new PtfConfig(
                new List<string>() { @"Resources\PtfConfig_Config1.ptfconfig", @"Resources\PtfConfig_Config2.ptfconfig" },
                new List<string>() { @"Resources\PtfConfig_Config1_default.ptfconfig", @"Resources\PtfConfig_Config2_default.ptfconfig" }
                );
            List<string> HiddenProperties = new List<string>() 
            { 
                "Property01",
                "Group02.Property03" 
            };

            var view = ptfConfig.CreatePtfPropertyView(HiddenProperties);
            
        }

        [TestMethod]
        public void LoadAdapter()
        {
            PtfConfig ptfConfig = new PtfConfig(
                new List<string>() { @"Resources\PtfConfig_Config1.ptfconfig", @"Resources\PtfConfig_Config2.ptfconfig" },
                new List<string>() { @"Resources\PtfConfig_Config1_default.ptfconfig", @"Resources\PtfConfig_Config2_default.ptfconfig" }
                );

            Assert.IsTrue(ptfConfig.adapterTable.ContainsKey("ISutControlAdapter"),"Adapter is loaded");

        }

        [TestMethod]
        public void ReadOldStylePtfConfig()
        {
            PtfConfig ptfConfig = new PtfConfig(
                new List<string>() { @"Resources\MS-DRSR.ptfconfig" },
                new List<string>() { @"Resources\MS-DRSR1.ptfconfig" }
                );

            var propertyNode = ptfConfig.GetPropertyNodeByName("Common.DomainFunctionLevel");
            Assert.IsNotNull(propertyNode, "The property node should exist.");
            Assert.AreEqual("DS_BEHAVIOR_WIN2012R2", propertyNode.Value, "Verify the property value.");
        }

        [TestMethod]
        public void GetPropertyByName_NameHasDot()
        {
            PtfConfig ptfConfig = new PtfConfig(
                new List<string>() { @"Resources\MS-DRSR.ptfconfig" },
                new List<string>() { @"Resources\MS-DRSR1.ptfconfig" }
                );


            var propertyNode1 = ptfConfig.GetPropertyNodeByName("GroupName1.SubGroup1.Property1");
            Assert.IsNotNull(propertyNode1, "The property node should exist.");
            Assert.AreEqual("value1", propertyNode1.Value, "Verify the property value.");


            var propertyNode2 = ptfConfig.GetPropertyNodeByName("GroupName1.SubGroup2.Property2");
            Assert.IsNotNull(propertyNode2, "The property node should exist.");
            Assert.AreEqual("value2", propertyNode2.Value, "Verify the property value.");


            var propertyNode3 = ptfConfig.GetPropertyNodeByName("GroupName1.Property3");
            Assert.IsNotNull(propertyNode3, "The property node should exist.");
            Assert.AreEqual("value3", propertyNode3.Value, "Verify the property value.");

        }

        [TestMethod]
        public void TestFileProperties()
        {
            PtfConfig ptfConfig = new PtfConfig(
                new List<string>() { @"Resources\PtfConfig_Config1.ptfconfig", @"Resources\PtfConfig_Config2.ptfconfig" },
                new List<string>() { @"Resources\PtfConfig_Config1_default.ptfconfig", @"Resources\PtfConfig_Config2_default.ptfconfig" }
                );
            var properties = ptfConfig.FileProperties["PtfConfig_Config1.ptfconfig"];
            Assert.IsTrue(
                properties.Contains("Group03.Property3"), "Verify the existence of Group03.Property3");
            Assert.IsTrue(
                properties.Contains("Group03.Group031.Property31"), "Verify the existence of Group03.Group031.Property31");

        }
    }
}
