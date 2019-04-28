// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This class defines the interactive adapter node.
    /// </summary>
    public class InteractiveAdapterNode : IAdapterConfig
    {
        private string displayName;

        /// <summary>
        /// The display name of this node.
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
        }

        private string name;

        /// <summary>
        /// The name of this node.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Creates XML node.
        /// </summary>
        /// <param name="doc">xml document</param>
        /// <returns></returns>
        public XmlNode CreateXmlNode(XmlDocument doc)
        {
            XmlNode node = doc.CreateNode(XmlNodeType.Element, "Adapter", StringResource.DefaultNamespace);
            XmlAttribute typeAttr = doc.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance");
            typeAttr.Value = "interactive";
            XmlAttribute nameAttr = doc.CreateAttribute(null, "name", null);
            nameAttr.Value = name;
            node.Attributes.Append(typeAttr);
            node.Attributes.Append(nameAttr);
            return node;
        }

        /// <summary>
        /// Constructor of InteractiveAdapterNode.
        /// </summary>
        /// <param name="name">The name of node</param>
        /// <param name="displayName">the display name of node</param>
        public InteractiveAdapterNode(string name, string displayName)
        {
            this.name = name;
            this.displayName = displayName;
        }

#pragma warning disable 0067
        public event ContentModifiedEventHandler ContentModified;
    }

    /// <summary>
    /// This class defines the powershell adapter node.
    /// </summary>
    public class PowerShellAdapterNode : IAdapterConfig
    {
        private string displayName;

        /// <summary>
        /// The display name of this node.
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
        }

        private string name;

        /// <summary>
        /// The name of this node.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        private string scriptDir;

        /// <summary>
        /// The directory of powershell script.
        /// </summary>
        public string ScriptDir
        {
            get { return scriptDir; }
            set
            {
                scriptDir = value;
                if (ContentModified != null) ContentModified();
            }
        }

        /// <summary>
        /// Creates xml node.
        /// </summary>
        /// <param name="doc">xml document</param>
        /// <returns></returns>
        public XmlNode CreateXmlNode(XmlDocument doc)
        {
            XmlNode node = doc.CreateNode(XmlNodeType.Element, "Adapter", StringResource.DefaultNamespace);
            XmlAttribute typeAttr = doc.CreateAttribute("xsi", "type", StringResource.XsiNamespace);
            typeAttr.Value = "powershell";
            XmlAttribute nameAttr = doc.CreateAttribute(null, "name", null);
            nameAttr.Value = name;
            XmlAttribute scriptDirAttr = doc.CreateAttribute(null, "scriptdir", null);
            scriptDirAttr.Value = ScriptDir;
            node.Attributes.Append(typeAttr);
            node.Attributes.Append(nameAttr);
            node.Attributes.Append(scriptDirAttr);
            return node;
        }

        /// <summary>
        /// Constructor of PowerShellAdapterNode.
        /// </summary>
        /// <param name="name">The name of node</param>
        /// <param name="displayName">The display name of node</param>
        /// <param name="scriptdir">The script directory.</param>
        public PowerShellAdapterNode(string name, string displayName, string scriptdir)
        {
            this.name = name;
            this.displayName = displayName;
            ScriptDir = scriptdir;
        }


        public event ContentModifiedEventHandler ContentModified;
    }

    /// <summary>
    /// This class defines the managed adapter node.
    /// </summary>
    public class ManagedAdapterNode : IAdapterConfig
    {
        /// <summary>
        /// Constructor of ManagedAdapterNode.
        /// </summary>
        /// <param name="name">The name of node</param>
        /// <param name="displayName">The display name of node</param>
        /// <param name="adapterType">The type of node</param>
        public ManagedAdapterNode(string name, string displayName, string adapterType)
        {
            this.name = name;
            this.displayName = displayName;
            this.AdapterType = adapterType;
        }
        private string displayName;

        /// <summary>
        /// The display name of this node.
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
        }

        private string name;

        /// <summary>
        /// The name of this node.
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        private string adapterType;

        /// <summary>
        /// The adapter type of this node.
        /// </summary>
        public string AdapterType
        {
            get
            {
                return adapterType; 
            }
            set
            {
                adapterType = value;
                if (ContentModified != null) ContentModified();
            }
        }

        /// <summary>
        /// Creates xml node.
        /// </summary>
        /// <param name="doc">The xml document</param>
        /// <returns></returns>
        public XmlNode CreateXmlNode(XmlDocument doc)
        {
            XmlNode node = doc.CreateNode(XmlNodeType.Element, "Adapter", StringResource.DefaultNamespace);
            XmlAttribute typeAttr = doc.CreateAttribute("xsi", "type", StringResource.XsiNamespace);
            typeAttr.Value = "managed";
            XmlAttribute nameAttr = doc.CreateAttribute(null, "name", null);
            nameAttr.Value = name;
            XmlAttribute adapterTypeAttr = doc.CreateAttribute(null, "adaptertype", null);
            adapterTypeAttr.Value = AdapterType;
            node.Attributes.Append(typeAttr);
            node.Attributes.Append(nameAttr);
            node.Attributes.Append(adapterTypeAttr);
            return node;
        }


        public event ContentModifiedEventHandler ContentModified;
    }

    /// <summary>
    /// This class defines shell adapter node.
    /// </summary>
    public class ShellAdapterNode : IAdapterConfig
    {
        private string displayName;

        /// <summary>
        /// The display name of this node.
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
        }

        private string name;

        /// <summary>
        /// The name of this node.
        /// </summary>
        public string Name
        {
            get { return name; }
        }


        private string scriptDir;

        /// <summary>
        /// The directory of script.
        /// </summary>
        public string ScriptDir
        {
            get { return scriptDir; }
            set
            {
                scriptDir = value;
                if (ContentModified != null) ContentModified();
            }
        }

        /// <summary>
        /// Creates xml Node.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlNode CreateXmlNode(XmlDocument doc)
        {
            XmlNode node = doc.CreateNode(XmlNodeType.Element, "Adapter", StringResource.DefaultNamespace);
            XmlAttribute typeAttr = doc.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance");
            typeAttr.Value = "shell";
            XmlAttribute nameAttr = doc.CreateAttribute(null, "name", null);
            nameAttr.Value = name;
            XmlAttribute scriptDirAttr = doc.CreateAttribute(null, "scriptdir", null);
            scriptDirAttr.Value = ScriptDir;
            node.Attributes.Append(typeAttr);
            node.Attributes.Append(nameAttr);
            node.Attributes.Append(scriptDirAttr);
            return node;
        }

        /// <summary>
        /// Constructor of ShellAdapterNode.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="scriptdir"></param>
        public ShellAdapterNode(string name, string displayName, string scriptdir)
        {
            this.name = name;
            this.displayName = displayName;
            ScriptDir = scriptdir;
        }


        public event ContentModifiedEventHandler ContentModified;
    }

}
