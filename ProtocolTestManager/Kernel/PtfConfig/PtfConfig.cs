// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Microsoft.Protocols.TestManager.Kernel
{


    public delegate void ContentModifiedEventHandler();

    /// <summary>
    /// PTFConfig file.
    /// </summary>
    public class PtfConfig
    {
        // ConfigNode Tree root according to ptfconfig files
        public PtfProperty PtfPropertyRoot { get; set; }

        /// <summary>
        /// The name of Configfile.
        /// </summary>
        public List<string> ConfigFileNames { get; set; }

        public Dictionary<string, List<string>> FileProperties;

        //xml document name list
        public List<XmlDocument> XmlDocList { get; set; }

        /// <summary>
        /// Constructor of PtfConfig
        /// </summary>
        /// <param name="activeConfigFile">A list of active configfile</param>
        /// <param name="defaultConfigFile">A list of default configfile</param>
        public PtfConfig(List<string> activeConfigFile, List<string> defaultConfigFile)
        {
            ConfigFileNames = activeConfigFile;
            List<string> DefaultConfigFileNames = defaultConfigFile;
            XmlDocList = new List<XmlDocument>();

            //Deal with config file in Env file
            PtfProperty DefaultPtfPropertyRoot = new PtfProperty();
            Merge(DefaultConfigFileNames, DefaultPtfPropertyRoot);
            AdjustDefaultGroup(DefaultPtfPropertyRoot);

            XmlDocList = new List<XmlDocument>();
            //Deal with config file in Bin file
            PtfPropertyRoot = new PtfProperty();
            FileProperties = Merge(ConfigFileNames, PtfPropertyRoot);
            AdjustDefaultGroup(PtfPropertyRoot);

            //Set Env value as default value
            SetDefaultValues(DefaultPtfPropertyRoot);
        }

        /// <summary>
        /// Save changes back to ptfconfig file in bin folder.
        /// </summary>
        /// <returns></returns>
        public void Save(string path)
        {
            for (int k = 0; k < ConfigFileNames.Count; k++)
            {
                string fileName = Path.GetFileName(ConfigFileNames[k]);
                fileName = Path.Combine(path, fileName);
                Utility.RemoveReadonly(fileName);
                XmlDocList[k].Save(fileName);
            }
        }

        /// <summary>
        /// Merges PTFConfig files 
        /// </summary>
        /// <param name="configFileNames">PTFConfig file names</param>
        /// <param name="RootNode">PTF property tree</param>
        /// <returns>Filename-property list dictionary</returns>
        public Dictionary<string, List<string>> Merge(List<string> configFileNames, PtfProperty RootNode)
        {
            Dictionary<string, List<string>> propertyMap = new Dictionary<string, List<string>>();
            try
            {
                if (configFileNames == null)
                {
                    throw new ArgumentException(StringResource.ConfigFileNameNotSpecified);
                }
                //Create an XmlNamespaceManager for resolving namespaces.
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
                nsmgr.AddNamespace("tc", StringResource.DefaultNamespace);
                foreach (string configFileName in configFileNames)
                {
                    if (configFileName == null)
                        continue;
                    XmlDocument doc = new XmlDocument();
                    doc.XmlResolver = null;
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.XmlResolver = null;
                    settings.DtdProcessing = DtdProcessing.Prohibit;
                    using (var xmlReader = XmlReader.Create(configFileName, settings))
                    {
                        doc.Load(xmlReader);
                        //record each xmldoc for Config node to refer to
                        XmlDocList.Add(doc);
                        //Properties
                        XmlNode node = doc.DocumentElement.SelectSingleNode("tc:Properties", nsmgr);
                        string filename = System.IO.Path.GetFileName(configFileName);
                        propertyMap[filename] = new List<string>();
                        Stack<string> parent = new Stack<string>();
                        Stack<XmlNode> nodes = new Stack<XmlNode>();
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            string propertyName = "";
                            if (child.NodeType == XmlNodeType.Element)
                            {
                                propertyName = child.Attributes["name"].Value;
                                if (child.Name == "Property")
                                {
                                    propertyMap[filename].Add(propertyName);
                                }
                                else if (child.Name == "Group")
                                {
                                    foreach (XmlNode subChild in child.ChildNodes)
                                    {
                                        if (subChild.NodeType == XmlNodeType.Element)
                                        {
                                            parent.Push(child.Attributes["name"].Value);
                                            nodes.Push(subChild);
                                        }
                                    }
                                }
                            }
                        }
                        while (nodes.Count > 0)
                        {
                            var n = nodes.Pop();
                            var p = parent.Pop();
                            if (n.Name == "Property")
                            {
                                propertyMap[filename].Add(string.Format("{0}.{1}", p, n.Attributes["name"].Value));
                            }
                            else if (n.Name == "Group")
                            {
                                foreach (XmlNode child in n.ChildNodes)
                                {
                                    if (child.NodeType == XmlNodeType.Element)
                                    {
                                        parent.Push(string.Format("{0}.{1}", p, n.Attributes["name"].Value));
                                        nodes.Push(child);
                                    }
                                }
                            }
                        }
                        MergePropertyAndGroup(RootNode, node);
                    }
                }
                LoadAdapters();
            }
            catch (XmlException e)
            {
                throw new XmlException("Merge Exception" + e);
            }
            return propertyMap;
        }

        public Dictionary<string, XmlNode> adapterTable;
        public Dictionary<string, XmlDocument> adapterDocTable;

        private void LoadAdapters()
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("tc", StringResource.DefaultNamespace);
            adapterTable = new Dictionary<string, XmlNode>();
            adapterDocTable = new Dictionary<string, XmlDocument>();
            foreach (XmlDocument doc in XmlDocList)
            {
                XmlNode adapterNode = doc.SelectSingleNode("tc:TestSite/tc:Adapters", nsmgr);
                if (adapterNode == null) continue;
                foreach (XmlNode adapter in adapterNode.SelectNodes("tc:Adapter", nsmgr))
                {
                    adapterTable[adapter.Attributes["name"].Value] = adapter;
                    adapterDocTable[adapter.Attributes["name"].Value] = doc;
                }
            }
        }

        /// <summary>
        /// recurrsively create Config Node tree from ptfconfig file
        /// </summary>
        /// <param name="baseConfigNode"></param>
        /// <param name="root"></param>
        private void MergePropertyAndGroup(PtfProperty baseConfigNode, XmlNode root)
        {
            Dictionary<string, XmlNode> propertyDict = new Dictionary<string, XmlNode>();
            Dictionary<string, XmlNode> groupDict = new Dictionary<string, XmlNode>();
            string value = "";
            //to sort Group node before Property node
            //record the pos of first Property node. If no Propert, default value is -1
            int propertyPos = -1;
            foreach (XmlNode child in root.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    value = child.Attributes["name"].Value;

                    if (child.Name == "Property")
                    {
                        if (propertyDict.ContainsKey(value))
                            throw new InvalidOperationException(
                                string.Format(StringResource.DuplicatePTFConfigNode, child.Name, value));

                        propertyDict.Add(value, child);
                    }
                    else
                    {
                        if (groupDict.ContainsKey(value))
                            throw new InvalidOperationException(
                                string.Format(StringResource.DuplicatePTFConfigNode, child.Name, value));
                        groupDict.Add(value, child);
                    }
                }
            }

            //Merge Group First
            foreach (XmlNode child in groupDict.Values)
            {
                bool duplicate = false;
                PtfProperty config = null;
                for (int i = 0; i < baseConfigNode.Count; i++)
                {
                    if (baseConfigNode[i].ValueType != PtfPropertyType.Group && propertyPos == -1)
                    {
                        propertyPos = i;
                    }
                    if (baseConfigNode[i].ValueType == PtfPropertyType.Group && baseConfigNode[i].Name == child.Attributes["name"].Value)
                    {
                        duplicate = true;
                        config = baseConfigNode[i];
                        break;
                    }
                }
                if (duplicate)
                {
                    //duplicate, first merge Group Node's Attribute
                    //Recurrsively Merge Property and Group to create data structure
                    if (child.Attributes["description"] != null)
                    {
                        config.Description = child.Attributes["description"].Value;
                        config.RefXmlNode = child;
                    }
                    MergePropertyAndGroup(config, child);
                }
                else
                {
                    //create new Group Node
                    //Insert Group before Property
                    //Recurrsively Merge Property and Group to create data structure
                    PtfProperty newGroup = new PtfProperty(child, true);
                    if (propertyPos >= 0)
                    {
                        baseConfigNode.Insert(propertyPos, newGroup);
                        propertyPos++;
                    }
                    else
                    {
                        baseConfigNode.Add(newGroup);
                    }
                    MergePropertyAndGroup(newGroup, child);
                }
            }
            //Merge Property
            foreach (XmlNode child in propertyDict.Values)
            {
                bool duplicate = false;
                PtfProperty config = null;
                foreach (PtfProperty childConfig in baseConfigNode)
                {
                    if (childConfig.ValueType != PtfPropertyType.Group && childConfig.Name == child.Attributes["name"].Value)
                    {
                        duplicate = true;
                        config = childConfig;
                        break;
                    }
                }
                //First remove old node
                if (duplicate)
                {
                    baseConfigNode.Remove(config);
                }
                //Insert new node
                PtfProperty newProperty = new PtfProperty(child, false);
                baseConfigNode.Add(newProperty);
            }
            groupDict.Clear();
            propertyDict.Clear();
        }

        /// <summary>
        /// Set Propert under Properties node into DefaultGroup
        /// </summary>
        public void AdjustDefaultGroup(PtfProperty RootNode)
        {
            PtfProperty defaultGroup = new PtfProperty()
            {
                Name = StringResource.DefaultGroupName,
                Description = "",
                ValueType = PtfPropertyType.Group
                
            };
            for (int i = 0; i < RootNode.Count; i++)
            {
                if (RootNode[i].ValueType != PtfPropertyType.Group)
                {
                    defaultGroup.Add(RootNode[i]);
                    RootNode.RemoveAt(i);
                    i--;
                }
            }
            //Set DefaultGroup node to be the first one
            RootNode.Insert(0, defaultGroup);
        }

        /// <summary>
        /// Set Environment config as default value
        /// </summary>
        private void SetDefaultValues(PtfProperty DefaultPropertyRoot)
        {
            Stack<PtfProperty> ConfigStack = new Stack<PtfProperty>();
            ConfigStack.Push(PtfPropertyRoot);
            while (ConfigStack.Count > 0)
            {
                PtfProperty topNode = ConfigStack.Pop();
                string defaultValue = GetDefaultValueByName(DefaultPropertyRoot, topNode.Name);
                topNode.DefaultValue = defaultValue;
                foreach (PtfProperty cn in topNode)
                {
                    ConfigStack.Push(cn);
                }
            }
        }

        private string GetDefaultValueByName(PtfProperty root, string NodeName)
        {
            Stack<PtfProperty> ConfigStack = new Stack<PtfProperty>();
            ConfigStack.Push(root);
            while (ConfigStack.Count > 0)
            {
                PtfProperty topNode = ConfigStack.Pop();
                if (topNode.Name == NodeName)
                {
                    return topNode.Value;
                }
                foreach (PtfProperty cn in topNode)
                {
                    ConfigStack.Push(cn);
                }
            }
            return "";
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <param name="propertyValue">A list of property values</param>
        /// <returns></returns>
        public bool SetPropertyValue(string propertyName, List<string> propertyValue)
        {
            if (propertyValue.Count == 0) return false;
            PtfProperty propertyNode = GetPropertyNodeByName(propertyName);
            if (propertyNode == null) return false;

            return true;
        }

        /// <summary>
        /// Gets the PtfProperty instance from a property name.
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <returns>The PtfProperty</returns>
        public PtfProperty GetPropertyNodeByName(string propertyName)
        {
            string searchName = "." + propertyName;
            string[] groups = propertyName.Split('.');

            //Property
            Stack<PtfProperty> s = new Stack<PtfProperty>();

            //Base
            Stack<string> b = new Stack<string>();


            foreach (PtfProperty p in PtfPropertyRoot)
            {
                // Move all the properties under Default Group to the root.
                if (p.Name == StringResource.DefaultGroupName)
                {
                    foreach (PtfProperty child in p)
                    {
                        s.Push(child);
                        b.Push("");
                    }
                }
                else
                {
                    s.Push(p);
                    b.Push("");
                }
            }

            while(s.Count> 0)
            {
                PtfProperty p = s.Pop();
                string n = b.Pop();
                if (p.ValueType == PtfPropertyType.Group)
                {
                    foreach (PtfProperty child in p)
                    {
                        s.Push(child);
                        b.Push(n + "." + p.Name);
                    }
                }
                else
                {
                    if (n + "." + p.Name == searchName) return p;
                }
            }
            return null;
        }


        /// <summary>
        /// Creates PtfPropertyView
        /// </summary>
        /// <param name="hideProperties">Hide properties.</param>
        /// <returns>An instance of PtfPropertyView</returns>
        public PtfPropertyView CreatePtfPropertyView(List<string> hideProperties)
        {
            List<PtfProperty> hide = new List<PtfProperty>();
            foreach (string name in hideProperties)
            {
                PtfProperty p = GetPropertyNodeByName(name);
                if (p != null) hide.Add(p);
            }

            // Root Group
            PtfPropertyRoot.ValueType = PtfPropertyType.Group;
            PtfPropertyView propertyView = new PtfPropertyView(PtfPropertyRoot);
            Stack<TreeStackItem> propertyStack = new Stack<TreeStackItem>();
            propertyStack.Push(new TreeStackItem(PtfPropertyRoot, propertyView));

            while (propertyStack.Count > 0)
            {
                TreeStackItem p = propertyStack.Pop();

                if (p.PropertyNode.ValueType != PtfPropertyType.Group)
                {
                    if (hide.Contains(p.PropertyNode)) continue;
                    var view = new PtfPropertyView(p.PropertyNode, p.PathFrom(3));
                    p.PropertyView.Add(view);
                }
                else
                {
                    PtfPropertyView view;
                    if (p.Path.Count <= 2)
                    {
                        view = new PtfPropertyView(p.PropertyNode);
                        p.PropertyView.Add(view);
                    }
                    else view = p.PropertyView;
                    for (int i = p.PropertyNode.Count - 1; i >= 0; i--)
                    {
                        var child = p.PropertyNode[i];
                        TreeStackItem c = new TreeStackItem(
                            child,
                            view);
                        c.Path.InsertRange(0, p.Path);
                        c.Path.Add(p.PropertyNode.Name);
                        propertyStack.Push(c);
                    }
                }
            }

            var v = propertyView[0];
            // Remove empty view
            for (int i = 0; i < v.Count; i++)
            {
                if (v[i].IsEmptyGroup)
                {
                    v.RemoveAt(i);
                    i--;
                    continue;
                }
                var childv = v[i];
                for (int ii = 0; ii < childv.Count; ii++)
                {
                    if (childv[ii].IsEmptyGroup)
                    {
                        childv.RemoveAt(ii);
                        ii--;
                    }
                }
            }
            return v;
        }

        /// <summary>
        /// Applies the adapter config.
        /// </summary>
        /// <param name="adapter">the adapter config</param>
        public void ApplyAdapterConfig(IAdapterConfig adapter)
        {
            XmlDocument doc = adapterDocTable[adapter.Name];
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("tc", StringResource.DefaultNamespace);
            XmlNode adapterNode = doc.SelectSingleNode("tc:TestSite/tc:Adapters", nsmgr);
            XmlNode node = adapter.CreateXmlNode(doc);
            adapterNode.RemoveChild(adapterTable[adapter.Name]);
            adapterNode.AppendChild(node);
            adapterTable[adapter.Name] = node;
        }
    }

    class TreeStackItem
    {
        public PtfProperty PropertyNode;
        public PtfPropertyView PropertyView;
        public List<string> Path;
        public TreeStackItem(PtfProperty node, PtfPropertyView view)
        {
            PropertyNode = node;
            PropertyView = view;
            Path = new List<string>();
        }

        public string PathFrom(int index)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = index; i < Path.Count; i++)
            {
                sb.AppendFormat(".{0}", Path[i]);
            }
            return sb.ToString().Trim(new char[] { '.' });
        }
    }
}
