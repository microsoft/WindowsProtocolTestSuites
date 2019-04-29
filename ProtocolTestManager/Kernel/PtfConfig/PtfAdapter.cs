// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Defines the adapter types.
    /// </summary>
    public enum AdapterType
    {
        Interactive = 0, Managed = 1, PowerShell = 2, Shell = 3
    }

    /// <summary>
    /// Represents the configurations of an adapter.
    /// </summary>
    public class PtfAdapterView : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor of PtfAdapterView.
        /// </summary>
        public PtfAdapterView()
        {
            Methods = new List<AdapterMethod>();
        }

        /// <summary>
        /// The name of this ptf adapter veiw.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The friendly name of ptf adapter view.
        /// </summary>
        public string FriendlyName { get; set; }

        private AdapterType type;

        /// <summary>
        /// The adapter type.
        /// </summary>
        public AdapterType Type
        {
            get { return type; }
            set
            {
                type = value;
                NotifyPropertyChanged("Type");
                NotifyPropertyChanged("TypeIndex");
            }
        }

        /// <summary>
        /// The type of adapter configuration.
        /// </summary>
        public IAdapterConfig AdapterConfig
        {
            get
            {
                switch (type)
                {
                    case AdapterType.Interactive:
                        return InteractiveAdapter;
                    case AdapterType.PowerShell:
                        return PowerShellAdapter;
                    case AdapterType.Managed:
                        return ManagedAdapter;
                    case AdapterType.Shell:
                        return ShellAdapter;
                }
                return null;
            }
        }

        public int TypeIndex
        {
            get { return (int)type; }
            set
            {
                type = (AdapterType)value;
                NotifyPropertyChanged("TypeIndex");
                NotifyPropertyChanged("Type");
                Modified();
            }
        }

        private InteractiveAdapterNode interactiveAdapter;
        public InteractiveAdapterNode InteractiveAdapter {
            get
            {
                if (interactiveAdapter == null)
                {
                    interactiveAdapter = new InteractiveAdapterNode(Name, FriendlyName);
                    interactiveAdapter.ContentModified += Modified;
                }
                return interactiveAdapter;
            }
            set
            {
                interactiveAdapter = value;
                interactiveAdapter.ContentModified += Modified;
            }
        }

        private PowerShellAdapterNode powershellAdapter;
        public PowerShellAdapterNode PowerShellAdapter
        {
            get
            {
                if (powershellAdapter == null)
                {
                    powershellAdapter = new PowerShellAdapterNode(Name, FriendlyName, ".\\");
                    powershellAdapter.ContentModified += Modified;
                }
                return powershellAdapter;
            }
            set
            {
                powershellAdapter = value;
                powershellAdapter.ContentModified += Modified;
            }

        }

        private ManagedAdapterNode managedAdapter;
        public ManagedAdapterNode ManagedAdapter
        {
            get
            {
                if (managedAdapter == null)
                {
                    managedAdapter = new ManagedAdapterNode(Name, FriendlyName, "DefaultType");
                    managedAdapter.ContentModified += Modified;
                }
                return managedAdapter;
            }
            set
            {
                managedAdapter = value;
                managedAdapter.ContentModified += Modified;
            }

        }

        private ShellAdapterNode shellAdapter;
        public ShellAdapterNode ShellAdapter
        {
            get
            {
                if (shellAdapter == null)
                {
                    shellAdapter = new ShellAdapterNode(Name, FriendlyName, ".\\");
                    shellAdapter.ContentModified += Modified;
                }
                return shellAdapter;
            }
            set
            {
                shellAdapter = value;
                shellAdapter.ContentModified += Modified;
            }

        }

        public string Description { get; set; }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private void Modified()
        {
            if (ContentModified != null) ContentModified();
        }

        public List<AdapterMethod> Methods { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event ContentModifiedEventHandler ContentModified;
    }

    /// <summary>
    /// Represents a method in an adapter.
    /// </summary>
    public class AdapterMethod
    {
        /// <summary>
        /// Constructor of AdapterMethod.
        /// </summary>
        /// <param name="method">The MethodInfo</param>
        public AdapterMethod(MethodInfo method)
        {
            Name = method.Name;
            var methodHelp = method.GetCustomAttributes(false)
                                .FirstOrDefault(o => o.GetType().FullName == "Microsoft.Protocols.TestTools.MethodHelpAttribute");
            if (methodHelp != null)
            {
                PropertyInfo property = methodHelp.GetType().GetProperty("HelpMessage");
                HelpMessage = property.GetValue(methodHelp, null) as string;
            }
            else
            {
                HelpMessage = "";
            }
        }

        /// <summary>
        /// The name of this method.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The help message of this method.
        /// </summary>
        public string HelpMessage { get; set; }
    }
}
