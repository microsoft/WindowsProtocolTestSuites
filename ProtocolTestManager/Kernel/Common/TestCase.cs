// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Represents a test case.
    /// </summary>
    public class TestCase : INotifyPropertyChanged
    {
        /// <summary>
        /// The categories of the test case
        /// </summary>
        public List<string> Category { get; set; }

        /// <summary>
        /// The tool tip of the test case on the UI
        /// </summary>
        public string ToolTipOnUI { get; set; }

        /// <summary>
        /// The name of the test case.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The fully qualified name of the test case
        /// </summary>
        public string FullName { get; set; }

        private bool isChecked;
        /// <summary>
        /// Indicate the test case is checked for running by case
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }

        private TestCaseStatus status;
        /// <summary>
        /// The status of the test case. NotRun, Running, Passed, Failed or Other
        /// </summary>
        public TestCaseStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        }

        private string log;
        /// <summary>
        /// The log of the test case
        /// </summary>
        public string Log
        {
            get { return log; }
            set
            {
                log = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Log"));
            }
        }

        /// <summary>
        /// The uri of the html log
        /// </summary>
        public Uri LogUri
        {
            get;
            set;
        }

        //Default value of IsSelected is false
        public TestCase()
        {
            Category = new List<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// The status of a test case
    /// </summary>
    public enum TestCaseStatus { NotRun, Running, Passed, Failed, Waiting, Other}
}
