﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This class defines the logger.
    /// </summary>
    public class Logger : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Logger()
        {
            GroupByOutcome = new GroupByOutcome();
            GroupByCategory = new GroupByCategory();
        }
        /// <summary>
        /// Initialize the logger.
        /// </summary>
        /// <param name="allTestCases">all test cases</param>
        public void Initialize(List<TestCase> allTestCases)
        {
            AllTestCases = allTestCases;
            GroupByOutcome.SetTestCaseList(allTestCases);
            GroupByCategory.SetTestCaseList(allTestCases);
        }

        /// <summary>
        /// Filter test cases.
        /// </summary>
        /// <param name="testcases">test cases</param>
        public void ApplyFilteredList(List<TestCase> testcases)
        {
            GroupByOutcome.SetTestCaseList(testcases);
            GroupByCategory.SetTestCaseList(testcases);
        }

        /// <summary>
        /// Group test cases by outcome.
        /// </summary>
        public GroupByOutcome GroupByOutcome { get; set; }

        /// <summary>
        /// Group test cases by category.
        /// </summary>
        public GroupByCategory GroupByCategory { get; set; }

        /// <summary>
        /// List of all test cases.
        /// </summary>
        public List<TestCase> AllTestCases { get; set; }


        private void NotifyPropertyChange(object sender, string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// To refresh the test case lists.
        /// </summary>
        public void RefreshTestCaseLists()
        {
            GroupByOutcome.SetTestCaseList(AllTestCases);
            GroupByCategory.SetTestCaseList(AllTestCases);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private TestCase runningTestCase = null;

        /// <summary>
        /// The running test case.
        /// </summary>
        public TestCase RunningTestCase
        {
            get
            {
                return GroupByOutcome.RunningTestCase;
            }
        }

        /// <summary>
        /// Path of index.html
        /// </summary>
        public string IndexHtmlFilePath
        {
            get;
            set;
        }
        /// <summary>
        /// Update the case status and case log
        /// Find the related case name, change its status and show log to user
        /// </summary>
        public void UpdateCaseFromHtmlLog(TestCaseStatus status, string testCaseName, string testCaseLogPath)
        {
            runningTestCase = AllTestCases.FirstOrDefault(c => c.Name == testCaseName);
            if (runningTestCase == null) return;
            GroupByOutcome.ChangeStatus(testCaseName, status);
            runningTestCase.LogUri = new Uri(testCaseLogPath);
            runningTestCase = null;
        }

        /// <summary>
        /// If existing running test cases, change the status to Other.
        /// </summary>
        public void FinishTest()
        {
            if (runningTestCase != null)
            {
                GroupByOutcome.ChangeStatus(runningTestCase.Name, TestCaseStatus.Other);
            }
        }
    }

    /// <summary>
    /// A test case group in the run page.
    /// </summary>
    public class TestCaseGroup : INotifyPropertyChanged
    {
        private int checkedNumber = 0;

        /// <summary>
        /// The constuctor of TestCaseGroup.
        /// </summary>
        public TestCaseGroup(string name, int totalCasesCnt)
        {
            Name = name;
            totalCasesCount = totalCasesCnt;
            TestCaseList = new ObservableCollection<TestCase>();
            autohide = true;
        }
        private string name;

        /// <summary>
        /// Count of all cases, used to show progress to user.
        /// </summary>
        private int totalCasesCount;

        /// <summary>
        /// Name of the group
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("HeaderText");
            }
        }

        /// <summary>
        /// The header text of the group.
        /// </summary>
        public string HeaderText
        {
            get
            {
                if (checkedNumber > 0)
                    return string.Format("{0}: {1}/{2} Checked: {3}", Name, TestCaseList.Count, totalCasesCount, checkedNumber);
                return string.Format("{0}: {1}/{2}", Name, TestCaseList.Count, totalCasesCount);
            }
        }

        private bool autohide;

        /// <summary>
        /// If true, hide the group if it is empty.
        /// </summary>
        public bool Autohide
        {
            get { return autohide; }
            set
            {
                autohide = value;
                OnPropertyChanged("Autohide");
                OnPropertyChanged("Visibility");
            }
        }

        /// <summary>
        /// Add a test case to the group.
        /// </summary>
        /// <param name="testcase">The test case.</param>
        public void AddTestCase(TestCase testcase)
        {
            testcase.PropertyChanged += TestcasePropertyChanged;
            if (testcase.IsChecked) checkedNumber++;
            TestCaseList.Insert(0, testcase);
            OnPropertyChanged("Visibility");
            OnPropertyChanged("HeaderText");
            OnPropertyChanged("IsChecked");
        }

        /// <summary>
        /// Remove the test case from the group.
        /// </summary>
        /// <param name="testcase">The test case.</param>
        public void RemoveTestCase(TestCase testcase)
        {
            testcase.PropertyChanged -= TestcasePropertyChanged;
            if (testcase.IsChecked) checkedNumber--;
            TestCaseList.Remove(testcase);
            OnPropertyChanged("Visibility");
            OnPropertyChanged("HeaderText");
            OnPropertyChanged("IsChecked");
        }

        /// <summary>
        /// The Visibility of the group. Used for databinding.
        /// </summary>
        public string Visibility
        {
            get
            {
                if (!autohide) return "Visible";
                if (TestCaseList.Count > 0) return "Visible";
                return "Collapsed";
            }
        }

        static bool holdUpdatingHeader = false;

        /// <summary>
        /// Holds updating the header in the test case list.
        /// Updating the header is a high-cost operation. If changing the status of a large number of test cases, hold it to improve the performance.
        /// </summary>
        public static void HoldUpdatingHeader()
        {
            holdUpdatingHeader = true;
        }
        /// <summary>
        /// Resumes updating the header int the test case list.
        /// </summary>
        public static void ResumeUpdatingHeader()
        {
            holdUpdatingHeader = false;
        }
        private void TestcasePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "IsChecked" && !holdUpdatingHeader)
            {
                UpdateHeader();
            }
        }

        /// <summary>
        /// Updates the header.
        /// </summary>
        public void UpdateHeader()
        {
            checkedNumber = 0;
            foreach (TestCase testcase in TestCaseList)
            {
                if (testcase.IsChecked) checkedNumber++;
            }
            OnPropertyChanged("HeaderText");
            OnPropertyChanged("IsChecked");

        }

        public bool? IsChecked
        {
            get
            {
                if (checkedNumber == 0) return false;
                else if (checkedNumber == TestCaseList.Count) return true;
                return null;
            }
        }

        /// <summary>
        /// Checks all the sub items.
        /// </summary>
        /// <param name="isChecked">Flags to indicate a case should be checked or not</param>
        public void CheckAllSubItems(bool isChecked)
        {
            HoldUpdatingHeader();
            foreach (TestCase testcase in TestCaseList) testcase.IsChecked = isChecked;
            ResumeUpdatingHeader();
        }

        /// <summary>
        /// Test cases in the group.
        /// </summary>
        public ObservableCollection<TestCase> TestCaseList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    public delegate void UpdateTestCaseStatusCallback(TestCaseGroup from, TestCaseGroup to, TestCase testcase);

}
