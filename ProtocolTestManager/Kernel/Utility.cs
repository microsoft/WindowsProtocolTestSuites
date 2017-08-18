// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Reflection;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Utility for the test suite manager.
    /// </summary>
    public class Utility
    {
        private TestSuiteFamilies testSuiteFamilies = null;
        private string testSuiteDir;
        private string installDir;
        public string LastRuleSelectionFilename;
        private AppConfig appConfig = null;
        private PtfConfig ptfconfig = null;
        private Detector detector;
        private PrerequisitView prerequisits;
        private List<Microsoft.Protocols.TestManager.Detector.DetectingItem> detectSteps;
        private TestCaseFilter filter = null;
        private TestSuite testSuite = null;
        private TestEngine testEngine = null;

        public Utility()
        {
            installDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, ".."));
        }

        /// <summary>
        /// Test suite specific configures.
        /// </summary>
        public AppConfig AppConfig
        {
            get
            {
                return appConfig;
            }
        }

        #region Introduction Page
        /// <summary>
        /// Test suite introduction.
        /// </summary>
        public TestSuiteFamilies TestSuiteIntroduction
        {
            get
            {
                if (testSuiteFamilies == null)
                {
                    string introfile = Path.Combine(installDir, @"etc\TestSuiteIntro.xml");
                    if (File.Exists(introfile))
                        testSuiteFamilies = TestSuiteFamilies.Load(introfile);
                    else
                        testSuiteFamilies = TestSuiteFamilies.Load("TestSuiteIntro.xml");
                }
                return testSuiteFamilies;
            }
        }

        /// <summary>
        /// Loads test suite configuration.
        /// </summary>
        /// <param name="testSuiteInfo">The information of a test suite</param>
        public void LoadTestSuiteConfig(TestSuiteInfo testSuiteInfo)
        {
            testSuiteDir = testSuiteInfo.TestSuiteFolder + "\\";
            try
            {
                appConfig = AppConfig.LoadConfig(
                    testSuiteInfo.TestSuiteName,
                    testSuiteInfo.TestSuiteVersion,
                    testSuiteDir,
                    installDir);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format(StringResource.ConfigLoadError, e.Message));
            }

            try
            {
                filter = TestCaseFilter.LoadFromXml(appConfig.RuleDefinitions);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format(StringResource.LoadFilterError, e.Message));
            }
            appConfig.InitDefaultConfigurations();

            LastRuleSelectionFilename = testSuiteInfo.LastProfile;
        }

        /// <summary>
        /// Loads test suite assembly.
        /// </summary>
        public void LoadTestSuiteAssembly()
        {
            try
            {
                appConfig.GetAdapterMethods();
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }

                throw new Exception(string.Format(StringResource.LoadAdapterError, sb.ToString()));
            }
            catch (Exception e)
            {
                throw new Exception(string.Format(StringResource.LoadAdapterError, e.Message));
            }
            testSuite = new TestSuite();
            try
            {
                testSuite.LoadFrom(appConfig.TestSuiteAssembly);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format(StringResource.LoadAssemblyError, e.Message));
            }
            if (appConfig.TestCategory != null)
            {
                try
                {
                    testSuite.AppendCategoryByConfigFile(appConfig.TestCategory);
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format(StringResource.AppendCategoryError, e.Message));
                }
            }

        }

        /// <summary>
        /// Loads PTF configuration.
        /// </summary>
        public void LoadPtfconfig()
        {
            try
            {
                ptfconfig = new PtfConfig(appConfig.PtfConfigFiles, appConfig.DefaultPtfConfigFiles);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format(StringResource.LoadPtfconfigError, e.Message));
            }
        }

        #endregion

        #region Auto-detection
        /// <summary>
        /// Initializes the auto-detection plug-in.
        /// </summary>
        public void InitializeDetector()
        {
            Microsoft.Protocols.TestManager.Detector.UtilCallBackFunctions.GetPropertyValue = (string name) =>
            {
                var property = ptfconfig.GetPropertyNodeByName(name);
                if (property != null) return property.Value;
                return null;
            };

            Microsoft.Protocols.TestManager.Detector.UtilCallBackFunctions.GetPropertiesByFile = (filename) =>
                {
                    if (!ptfconfig.FileProperties.ContainsKey(filename))
                        return null;
                    return ptfconfig.FileProperties[filename];
                };
            detector = new Detector();
            detector.Load(appConfig.DetectorAssemblyPath);
        }

        /// <summary>
        /// Gets properties required for auto-detection.
        /// </summary>
        /// <returns>A PrerequisitView object</returns>
        public PrerequisitView GetPrerequisits()
        {
            Microsoft.Protocols.TestManager.Detector.Prerequisites p = detector.GetPrerequisits();
            prerequisits = new PrerequisitView()
            {
                Summary = p.Summary,
                Title = p.Title,
                Properties = new List<PrerequisitProperty>()
            };
            foreach (var i in p.Properties)
            {
                prerequisits.Properties.Add(new PrerequisitProperty()
                {
                    PropertyName = i.Key,
                    PropertyValues = i.Value
                });
            }
            return prerequisits;
        }
        /// <summary>
        /// Gets the auto-detection steps.
        /// </summary>
        /// <returns>A list of the steps</returns>
        public List<Microsoft.Protocols.TestManager.Detector.DetectingItem> GetDetectSteps()
        {
            detectSteps = detector.GetDetectSteps();
            return detectSteps;
        }

        /// <summary>
        /// Sets the property values required for auto-detection.
        /// </summary>
        /// <returns>Returns true if succeeded, otherwise false.</returns>
        public bool SetPrerequisits()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (var p in prerequisits.Properties)
            {
                properties.Add(p.PropertyName, p.Value);
            };
            return detector.SetPrerequisits(properties);

        }


        private StreamWriter logWriter;
        private int stepIndex;
        private string detectorLog;
        /// <summary>
        /// Start the auto-detection
        /// </summary>
        /// <param name="callback">The call back function when the detection finished.</param>
        public void StartDetection(DetectionCallback callback)
        {
            stepIndex = 0;
            detectorLog = Path.Combine(appConfig.AppDataDirectory, "Detector_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".log");
            logWriter = new StreamWriter(detectorLog);
            detector.DetectLogCallback = (msg, style) =>
                {
                    if (stepIndex == detectSteps.Count) return;
                    var item = detectSteps[stepIndex];
                    item.Style = style;
                    switch (style)
                    {
                        case Microsoft.Protocols.TestManager.Detector.LogStyle.Default:
                            detectSteps[stepIndex].DetectingStatus = Microsoft.Protocols.TestManager.Detector.DetectingStatus.Detecting;
                            break;
                        case Microsoft.Protocols.TestManager.Detector.LogStyle.Error:
                            stepIndex++;
                            item.DetectingStatus = Microsoft.Protocols.TestManager.Detector.DetectingStatus.Error;
                            break;
                        case Microsoft.Protocols.TestManager.Detector.LogStyle.StepFailed:
                            stepIndex++;
                            item.DetectingStatus = Microsoft.Protocols.TestManager.Detector.DetectingStatus.Failed;
                            break;
                        case Microsoft.Protocols.TestManager.Detector.LogStyle.StepSkipped:
                            stepIndex++;
                            item.DetectingStatus = Microsoft.Protocols.TestManager.Detector.DetectingStatus.Skipped;
                            break;
                        case Microsoft.Protocols.TestManager.Detector.LogStyle.StepNotFound: 
                            stepIndex++;
                            item.DetectingStatus = Microsoft.Protocols.TestManager.Detector.DetectingStatus.NotFound;
                            break;
                        case Microsoft.Protocols.TestManager.Detector.LogStyle.StepPassed:
                            stepIndex++;
                            item.DetectingStatus = Microsoft.Protocols.TestManager.Detector.DetectingStatus.Finished;
                            break;
                        default:
                            item.DetectingStatus = Microsoft.Protocols.TestManager.Detector.DetectingStatus.Pending;
                            break;
                    }
                    logWriter.WriteLine("[{0}] {1}", DateTime.Now.ToString(), msg);
                    logWriter.Flush();
                };
            detector.BeginDetection((outcome) =>
                {
                    if (stepIndex < detectSteps.Count) detectSteps[stepIndex].DetectingStatus = TestManager.Detector.DetectingStatus.Pending;
                    callback(outcome);
                    logWriter.Close();
                    logWriter = null;
                });
        }

        /// <summary>
        /// Stop the auto-detection
        /// </summary>
        public void StopDetection(Action callback)
        {
            detector.DetectLogCallback = null;
            detectSteps[stepIndex].DetectingStatus = TestManager.Detector.DetectingStatus.Canceling;
            detector.StopDetection(callback);
            if (stepIndex < detectSteps.Count) detectSteps[stepIndex].DetectingStatus = TestManager.Detector.DetectingStatus.Pending;
            if (logWriter != null)
            {
                logWriter.Close();
                logWriter.Dispose();
                logWriter = null;
            }
            stepIndex = 0;
        }

        /// <summary>
        /// Get a object to show on the UI as the detection summary.
        /// </summary>
        /// <returns>A object to show in the content control.</returns>
        public object GetDetectionSummary()
        {
            return detector.GetDetectionSummary();
        }

        /// <summary>
        /// Apply the test case selection rules detected by the plug-in.
        /// </summary>
        public void ApplyDetectedRules()
        {
            foreach (var rule in detector.GetRules())
            {
                Rule r = filter.FindRuleByName(rule.Name);
                if (r == null) throw new Exception(string.Format("Cannot find rule by name {0}.", rule.Name));
                switch (rule.Status)
                {
                    case Microsoft.Protocols.TestManager.Detector.RuleStatus.Selected:
                        r.SelectStatus = RuleSelectStatus.Selected;
                        r.Status = RuleSupportStatus.Selected;
                        break;
                    case Microsoft.Protocols.TestManager.Detector.RuleStatus.NotSupported:
                        r.SelectStatus = RuleSelectStatus.NotSelected;
                        r.Status = RuleSupportStatus.NotSupported;
                        break;
                    case Microsoft.Protocols.TestManager.Detector.RuleStatus.Unknown:
                        r.SelectStatus = RuleSelectStatus.NotSelected;
                        r.Status = RuleSupportStatus.Unknown;
                        break;
                    default:
                        r.SelectStatus = RuleSelectStatus.NotSelected;
                        r.Status = RuleSupportStatus.Default;
                        break;
                }
            }
        }

        /// <summary>
        /// Opens the auto-detection log
        /// </summary>
        public void OpenDetectionLog()
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(detectorLog);
            p.Start();
        }

        /// <summary>
        /// Apply the property values detected by the plug-in.
        /// </summary>
        public void ApplyDetectedValues()
        {
            var properties = detector.GetDetectedProperty();
            foreach (var property in properties)
            {
                PtfProperty p = ptfconfig.GetPropertyNodeByName(property.Key);
                if (p == null) throw new Exception(string.Format("Cannot find property by name {0}.", property.Key));

                if (property.Value == null || property.Value.Count == 0) p.Value = null;
                else if (property.Value.Count == 1) p.Value = property.Value[0];
                else
                {
                    p.ChoiceItems = property.Value;
                    p.Value = property.Value[0];
                }
            }
        }

        #endregion

        #region Test case filter
        /// <summary>
        /// Gets the TestCaseFilter object for current test suite.
        /// </summary>
        /// <returns>A TestCaseFilter object</returns>
        public TestCaseFilter GetFilter()
        {
            return filter;
        }

        private List<TestCase> selectedCases = null;
        /// <summary>
        /// Gets a filtered test case list.
        /// </summary>
        /// <returns>Test case list</returns>
        public List<TestCase> GetSelectedCaseList()
        {
            selectedCases = filter.FilterTestCaseList(testSuite.TestCaseList);
            return selectedCases;
        }

        /// <summary>
        /// Gets current TestSuite object.
        /// </summary>
        /// <returns>A TestSuite object</returns>
        public TestSuite GetTestSuite()
        {
            return testSuite;
        }

        /// <summary>
        /// The number of the selected test cases.
        /// </summary>
        public int SelectedCaseCount
        {
            get { return selectedCases.Count; }
        }

        #endregion

        #region PTF Properties
        List<string> hiddenProperties = null;

        /// <summary>
        /// Gets the properties to hide from the plug-in.
        /// </summary>
        public void HideProperties()
        {
            List<Microsoft.Protocols.TestManager.Detector.CaseSelectRule> selectedRules = new List<Microsoft.Protocols.TestManager.Detector.CaseSelectRule>();
            foreach (string rule in filter.GetRuleList(true))
            {
                selectedRules.Add(new Microsoft.Protocols.TestManager.Detector.CaseSelectRule()
                {
                    Name = rule,
                    Status = Microsoft.Protocols.TestManager.Detector.RuleStatus.Selected
                });

            }
            foreach (string rule in filter.GetRuleList(false))
            {
                selectedRules.Add(new Microsoft.Protocols.TestManager.Detector.CaseSelectRule()
                {
                    Name = rule,
                    Status = Microsoft.Protocols.TestManager.Detector.RuleStatus.NotSupported
                });
            }
            hiddenProperties = detector.GetHiddenProperties(selectedRules);
        }

        private PtfPropertyView ptfPropertyView;

        /// <summary>
        /// Creates a PtfPropertyView for the UI.
        /// </summary>
        /// <returns>A PtfPropertyView object</returns>
        public PtfPropertyView CreatePtfPropertyView()
        {
            ptfPropertyView = ptfconfig.CreatePtfPropertyView(hiddenProperties);
            if (appConfig.PropertyGroupOrder != null) ptfPropertyView.SortItems(appConfig.PropertyGroupOrder);
            return ptfPropertyView;
        }

        /// <summary>
        /// The number of the properties whos value is different from the default.
        /// </summary>
        public int ChangedPropertyCount
        {
            get
            {
                int count = 0;
                Stack<PtfPropertyView> propertyStack = new Stack<PtfPropertyView>();
                propertyStack.Push(ptfPropertyView);
                while (propertyStack.Count > 0)
                {
                    PtfPropertyView p = propertyStack.Pop();
                    if (p.Count > 0)
                    {
                        foreach (var child in p) propertyStack.Push(child);
                    }
                    else
                    {
                        if (p.Value != p.DefaultValue) count++;
                    }
                }
                return count;
            }
        }

        /// <summary>
        /// Saves the ptfconfig file back to the bin folder.
        /// </summary>
        public void SavePtfconfigToBinFolder()
        {
            ptfconfig.Save(Path.Combine(appConfig.TestSuiteDirectory, "Bin"));
        }

        /// <summary>
        /// Saves the ptfconfig files back to the source code folder.
        /// </summary>
        public void SavePtfconfigToSourceCode()
        {
            for (int k = 0; k < ptfconfig.ConfigFileNames.Count; k++)
            {
                string fileName = System.IO.Path.GetFileName(ptfconfig.ConfigFileNames[k]);
                foreach (var dest in appConfig.PtfConfigFilesInSource)
                {
                    if (System.IO.Path.GetFileName(dest) == fileName)
                    {
                        RemoveReadonly(dest);
                        ptfconfig.XmlDocList[k].Save(dest);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Adapter

        /// <summary>
        /// Gets PtfAdapterView for the UI.
        /// </summary>
        /// <returns></returns>
        public List<PtfAdapterView> GetAdaptersView()
        {
            SetAdapterConfig();
            return appConfig.PredefinedAdapters;
        }


        public event ContentModifiedEventHandler AdapterConfigurationChanged;

        /// <summary>
        /// Applies the changes in the PtfAdapterView in the ptfconfig XML Documents.
        /// </summary>
        public void ApplyAdaptersConfig()
        {
            foreach (PtfAdapterView adapter in appConfig.PredefinedAdapters)
            {
                ptfconfig.ApplyAdapterConfig(adapter.AdapterConfig);
            }
        }


        private void SetAdapterConfig()
        {
            foreach (var item in ptfconfig.adapterTable)
            {
                string name = item.Key;
                XmlNode xmlNode = item.Value;
                string type = xmlNode.Attributes["xsi:type"].Value;
                PtfAdapterView adapter;
                adapter = appConfig.PredefinedAdapters.FirstOrDefault(i => i.Name == name);
                if (adapter == null) continue;

                adapter.ContentModified += AdapterConfigurationChanged;
                switch (type)
                {
                    case "powershell":
                        {
                            string scriptDir = xmlNode.Attributes["scriptdir"].Value;
                            adapter.PowerShellAdapter = new PowerShellAdapterNode(name, adapter.FriendlyName, scriptDir);
                            adapter.Type = AdapterType.PowerShell;
                        }
                        break;
                    case "interactive":
                        {
                            adapter.InteractiveAdapter = new InteractiveAdapterNode(name, adapter.FriendlyName);
                            adapter.Type = AdapterType.Interactive;
                        }
                        break;
                    case "managed":
                        {
                            string adaptertype = xmlNode.Attributes["adaptertype"].Value;
                            adapter.ManagedAdapter = new ManagedAdapterNode(name, adapter.FriendlyName, adaptertype);
                            adapter.Type = AdapterType.Managed;
                        }
                        break;
                    case "script":
                        {
                            string scriptDir = xmlNode.Attributes["scriptdir"].Value;
                            adapter.ScriptAdapter = new ScriptAdapterNode(name, adapter.FriendlyName, scriptDir);
                            adapter.Type = AdapterType.Script;
                        }
                        break;
                }
            }
        }

        #endregion

        #region Save & Load settings
        /// <summary>
        /// Loads the configurations from a saved profile.
        /// </summary>
        /// <param name="filename">File name</param>
        public void LoadProfileSettings(string filename)
        {
            using (ProfileUtil profile = ProfileUtil.LoadProfile(filename))
            {
                if (!profile.VerifyVersion(appConfig.TestSuiteName, appConfig.TestSuiteVersion))
                {
                    if (profile.Info != null)
                    {
                        throw new InvalidDataException(string.Format(
                            StringResource.ProfileNotMatchError,
                            profile.Info.TestSuiteName, profile.Info.Version,
                            appConfig.TestSuiteName, appConfig.TestSuiteVersion
                            ));
                    }
                    else
                    {
                        throw new InvalidDataException(StringResource.InvalidProfile);
                    }
                }
                string desCfgDir = System.IO.Path.Combine(appConfig.TestSuiteDirectory, "Bin");
                profile.SavePtfCfgTo(desCfgDir);
                filter.LoadProfile(profile.ProfileStream);
                ImportPlaylist(profile.PlaylistStream);
                GetSelectedCaseList();

                int sel, notfound;
                ApplyPlaylist(out sel, out notfound);
            }
        }

        /// <summary>
        /// Saves the configurations as a profile.
        /// </summary>
        /// <param name="filename">File name</param>
        public void SaveProfileSettings(string filename)
        {
            string profileName = System.IO.Path.GetFileNameWithoutExtension(filename);
            string[] ptfConfigFiles = Directory.GetFiles(
                Path.Combine(appConfig.TestSuiteDirectory, "Bin"), "*.ptfconfig", SearchOption.TopDirectoryOnly);
            using (ProfileUtil profile = ProfileUtil.CreateProfile(filename, appConfig.TestSuiteName, appConfig.TestSuiteVersion))
            {
                foreach (string settingsConfigFile in ptfConfigFiles)
                {
                    profile.AddPtfCfg(settingsConfigFile);
                }
                filter.SaveProfile(profile.profileStream);
                ExportPlaylist(profile.PlaylistStream, true);
            }
        }

        /// <summary>
        /// Save the rule selection of current run.
        /// </summary>
        public void SaveLastProfile()
        {
            SaveProfileSettings(LastRuleSelectionFilename);
        }

        #endregion

        #region Run test cases
        /// <summary>
        /// Initializes the test engine
        /// </summary>
        public void InitializeTestEngine()
        {
            testEngine = new TestEngine(appConfig.VSTestPath)
            {
                WorkingDirectory = testSuiteDir,
                TestAssemblies = appConfig.TestSuiteAssembly,
                TestSetting = appConfig.TestSetting,
                PipeName = appConfig.PipeName,
            };
            testEngine.InitializeLogger(selectedCases);
        }

        /// <summary>
        /// Occurs when the test run finished.
        /// </summary>
        public event TestFinishedEvent TestFinished
        {
            add { if (testEngine != null) testEngine.TestFinished += value; }
            remove { if (testEngine != null)testEngine.TestFinished -= value; }
        }

        /// <summary>
        /// Retrives the object of the logger.
        /// </summary>
        /// <returns>Logger object.</returns>
        public Logger GetLogger()
        {
            return testEngine.GetLogger();
        }

        /// <summary>
        /// Runs all filtered test cases.
        /// </summary>
        public void RunAllTestCases()
        {
            if (appConfig.TestCategory == null)
            {
                testEngine.BeginRunByFilter(filter.GetFilterExpression());
            }
            else
            {
                RunByCases(GetSelectedCaseList());
            }
        }

        /// <summary>
        /// Runs specific test cases.
        /// </summary>
        /// <param name="testcases">Test case</param>
        public void RunByCases(List<TestCase> testcases)
        {
            Stack<TestCase> caseStack = new Stack<TestCase>();
            foreach (var test in testcases) caseStack.Push(test);
            testEngine.BeginRunByCase(caseStack);
        }


        public void SyncRunByCases(List<TestCase> testcases)
        {
            Stack<TestCase> caseStack = new Stack<TestCase>();
            foreach (var test in testcases) caseStack.Push(test);
            testEngine.RunByCase(caseStack);
        }


        public void AbortExecution()
        {
            testEngine.AbortExecution();
        }

        /// <summary>
        /// Filters test cases by keyword in name.
        /// </summary>
        /// <param name="keyword">Keyword</param>
        public void FilterByKeyword(string keyword)
        {
            testEngine.FilterByKeyword(keyword);
        }

        /// <summary>
        /// Removes the filter and show all selected test cases.
        /// </summary>
        public void RemoveFilter()
        {
            testEngine.RemoveFilter();
        }
        #endregion

        #region Save test results

        /// <summary>
        /// Export test cases to a Visual Studio playlist.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="checkedOnly">True if only export checked cases. False export all test cases.</param>
        public void ExportPlaylist(string filename, bool checkedOnly)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                ExportPlaylist(stream, checkedOnly);
            }
        }

        /// <summary>
        /// Export test cases to a Visual Studio playlist.
        /// </summary>
        /// <param name="stream">A Stream</param>
        /// <param name="checkedOnly">True if only export checked cases. False export all test cases.</param>
        public void ExportPlaylist(Stream stream, bool checkedOnly)
        {
            XmlWriter writer = XmlWriter.Create(stream);
            writer.WriteStartElement("Playlist");
            writer.WriteStartAttribute("Version");
            writer.WriteValue("1.0");
            writer.WriteEndAttribute();
            foreach (var testcase in selectedCases)
            {
                if (checkedOnly & !testcase.IsChecked) continue;
                writer.WriteStartElement("Add");
                writer.WriteStartAttribute("Test");
                writer.WriteValue(testcase.FullName);
                writer.WriteEndAttribute();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
        }

        private List<string> caselistCache;

        /// <summary>
        /// Import a Play List
        /// </summary>
        /// <param name="stream">A Stream</param>
        public void ImportPlaylist(Stream stream)
        {
            caselistCache = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            settings.DtdProcessing = DtdProcessing.Prohibit;
            XmlReader xmlReader = XmlReader.Create(stream, settings);
            doc.Load(xmlReader);
            XmlNode Playlist = doc.SelectSingleNode("Playlist");
            foreach (XmlNode node in Playlist.SelectNodes("Add"))
            {
                string name = node.Attributes["Test"].Value;
                caselistCache.Add(name);
            }
        }

        /// <summary>
        /// Import a Play List
        /// </summary>
        /// <param name="filename">Filename</param>
        public void ImportPlaylist(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                ImportPlaylist(stream);
            }
        }

        /// <summary>
        /// Check the test cases in the playlist.
        /// </summary>
        /// <param name="numberOfChecked">Number of checked cases.</param>
        /// <param name="numberOfNotfound">Number of not found test cases in the list.</param>
        public void ApplyPlaylist(out int numberOfChecked, out int numberOfNotfound)
        {
            numberOfChecked = 0;
            numberOfNotfound = 0;
            TestCaseGroup.HoldUpdatingHeader();
            foreach (var testcase in selectedCases)
            {
                if(!caselistCache.Contains(testcase.Name) && !testcase.IsChecked)
                    testcase.IsChecked = false;
            }
            TestCaseGroup.ResumeUpdatingHeader();

            foreach (string name in caselistCache)
            {
                var testcase = selectedCases.FirstOrDefault((v) => v.FullName == name);
                if (testcase != null)
                {
                    if(!testcase.IsChecked)
                    {
                        testcase.IsChecked = true;
                    }
                    numberOfChecked++;
                }
                else
                {
                    numberOfNotfound++;
                }
            }

        }

        /// <summary>
        /// Generates plain text case list.
        /// </summary>
        /// <param name="passed">Include passed test cases</param>
        /// <param name="failed">Include failed test cases</param>
        /// <param name="inconclusive">Include inconclusive test cases</param>
        /// <param name="notrun">Include not run test cases</param>
        /// <returns>A list of CaseListItem</returns>
        public List<CaseListItem> GenerateTextCaseListItems(bool passed, bool failed, bool inconclusive, bool notrun)
        {
            List<CaseListItem> items = new List<CaseListItem>();
            foreach (var i in selectedCases)
            {
                if (i.Status == TestCaseStatus.Passed && passed ||
                    i.Status == TestCaseStatus.Failed && failed ||
                    i.Status == TestCaseStatus.NotRun && notrun ||
                    i.Status == TestCaseStatus.Other && inconclusive)
                {
                    items.Add(new CaseListItem(i.Name, i.Status));
                }
            }
            return items;
        }

        /// <summary>
        /// The way of sorting items in the test case list
        /// </summary>
        public enum SortBy { Name, Outcome };

        /// <summary>
        /// Convert the list to plain text report
        /// </summary>
        /// <param name="items">Case list</param>
        /// <param name="showOutcome">shows test case outcome in the list</param>
        /// <param name="sortby">The way of sorting items in the test case list</param>
        /// <param name="separator">The style of the text file</param>
        /// <returns>Plain text report</returns>
        public static string GeneratePlainTextReport(List<CaseListItem> items, bool showOutcome, SortBy sortby, CaseListItem.Separator separator)
        {
            if (sortby == SortBy.Name)
            {
                items.Sort((x, y) => { return string.Compare(x.Name, y.Name); });
            }
            else
            {
                items.Sort((x, y) => { return string.Compare(x.Outcome, y.Outcome); });
            }
            StringBuilder sb = new StringBuilder();
            foreach (var i in items)
            {
                sb.AppendLine(i.FormatText(showOutcome, separator));
            }
            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// Changes the read only attribute of a file.
        /// </summary>
        /// <param name="file">The file to be removed the read only attribute</param>
        public static void RemoveReadonly(string file)
        {
            FileAttributes attributes = File.GetAttributes(file);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(file, attributes & ~FileAttributes.ReadOnly);
            }
        }

        /// <summary>
        /// Parse the file content to get the case status
        /// Result format in file: "Result":"Result: Passed"
        /// </summary>
        public static bool ParseFileGetStatus(string filePath, out TestCaseStatus status)
        {
            status = TestCaseStatus.NotRun;

            string content = File.ReadAllText(filePath);
            int startIndex = content.IndexOf(AppConfig.ResultKeyword);
            startIndex += AppConfig.ResultKeyword.Length;
            int endIndex = content.IndexOf("\"", startIndex);
            string statusStr = content.Substring(startIndex, endIndex - startIndex);
            switch (statusStr)
            {
                case AppConfig.HtmlLogStatusPassed:
                    status = TestCaseStatus.Passed;
                    break;
                case AppConfig.HtmlLogStatusFailed:
                    status = TestCaseStatus.Failed;
                    break;
                case AppConfig.HtmlLogStatusInconclusive:
                    status = TestCaseStatus.Other;
                    break;
                default:
                    // The file name format is not correct
                    return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Items for the plain text report
    /// </summary>
    public class CaseListItem
    {
        /// <summary>
        /// The separator in the case list
        /// </summary>
        public enum Separator { Space, Comma };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the test case</param>
        /// <param name="status">The status of the test case</param>
        public CaseListItem(string name, TestCaseStatus status)
        {
            Name = name;
            Outcome = status == TestCaseStatus.Other ? "Inconclusive" : status.ToString();
        }
        /// <summary>
        /// The outcome of the test case
        /// </summary>
        public string Outcome;

        /// <summary>
        /// The name of the test case
        /// </summary>
        public string Name;

        /// <summary>
        /// Generates a line in the plain text case list.
        /// </summary>
        /// <param name="showOutcome">Shows the outcome</param>
        /// <param name="separator">The separator</param>
        /// <returns>An item in the report</returns>
        public string FormatText(bool showOutcome, Separator separator)
        {
            if (!showOutcome) return Name;
            if (separator == Separator.Space) return string.Format("{0} {1}", Outcome, Name);
            return string.Format("{0},{1}", Outcome, Name);
        }
    }
}
