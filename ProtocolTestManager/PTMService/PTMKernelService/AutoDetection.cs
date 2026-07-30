// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    public class AutoDetection : IAutoDetection
    {
        private readonly ReaderWriterLockSlim stateLocker = new();
        private readonly ReaderWriterLockSlim logLocker = new();
        private readonly object lifecycleLocker = new();
        private static readonly AsyncLocal<DetectionRun> activeDetectionRun = new();

        private Exception detectedException = null;

        private List<DetectingItem> detectSteps;

        private ITestSuite TestSuite { get; set; }

        private IConfiguration Configuration { get; set; }

        private PtfConfig PtfConfig { get; set; }

        private IValueDetector valueDetector = null;

        private readonly PrerequisiteView prerequisiteView = null;

        private string detectorAssemblyFileName = string.Empty;

        private Assembly detectorAssembly = null;

        private AssemblyLoadContext alc = null;

        private string detectorInstanceTypeName = string.Empty;

        private DetectionStatus detectionStatus = DetectionStatus.NotStart;

        private DetectionRun currentRun;

        private sealed class DetectionRun
        {
            public DetectionRun(AutoDetection owner, IValueDetector detector)
            {
                Owner = owner;
                Detector = detector;
            }

            public AutoDetection Owner { get; }

            public IValueDetector Detector { get; }

            public CancellationTokenSource Cancellation { get; } = new();

            public string Id { get; set; } = string.Empty;

            public string LogPath { get; set; } = string.Empty;

            public StreamWriter LogWriter { get; set; }

            public Task Task { get; set; }

            public int StepIndex { get; set; }

            public int LogClosed;

            public int Retired;
        }

        private AutoDetection(IConfiguration configuration)
        {
            TestSuite = configuration.TestSuite;
            Configuration = configuration;
            UtilCallBackFunctions.WriteLog = RouteDetectorLog;

            InitializeDetector();

            detectSteps = ValueDetector.GetDetectionSteps();

            Prerequisites p = GetPrerequisitsInValueDetectorAssembly();
            prerequisiteView = new PrerequisiteView()
            {
                Summary = p.Summary,
                Title = p.Title,
                Properties = []
            };
            foreach (var i in p.Properties)
            {
                prerequisiteView.Properties.Add(new Property()
                {
                    Name = i.Key,
                    Value = ((i.Value != null) && (i.Value.Count > 0)) ? i.Value[0] : null,
                    Choices = i.Value
                });
            }
        }

        public static AutoDetection Create(IConfiguration configuration)
        {
            var instance = new AutoDetection(configuration);

            return instance;
        }

        protected IValueDetector ValueDetector
        {
            get
            {
                stateLocker.EnterUpgradeableReadLock();
                try
                {
                    if (valueDetector == null)
                    {
                        stateLocker.EnterWriteLock();
                        try
                        {
                            if (valueDetector == null)
                            {
                                // Create an instance
                                valueDetector = detectorAssembly.CreateInstance(detectorInstanceTypeName) as IValueDetector;
                            }
                        }
                        finally
                        {
                            stateLocker.ExitWriteLock();
                        }
                    }
                }
                finally
                {
                    stateLocker.ExitUpgradeableReadLock();
                }

                return valueDetector;
            }
        }

        /// <summary>
        /// Loads the auto-detect plug-in from assembly file.
        /// </summary>
        /// <param name="detectorAssemblyFileName">File name</param>
        public void Load(string detectorAssemblyFileName)
        {
            // Get CustomerInterface
            Type interfaceType = typeof(IValueDetector);

            string assemblyDirPath = Directory.GetParent(detectorAssemblyFileName).FullName;
            alc = new CollectibleAssemblyLoadContext(detectorAssemblyFileName, AutoDetectionConsts.ignoredAssemblies, AutoDetectionConsts.mixedAssemblies);

            alc.Resolving += (context, assemblyName) =>
            {
                string assemblyPath = Path.Combine(assemblyDirPath, $"{assemblyName.Name}.dll");
                if (File.Exists(assemblyPath))
                {
                    return context.LoadFromAssemblyPath(assemblyPath);
                }

                return null;
            };

            Assembly assembly = alc.LoadFromAssemblyPath(detectorAssemblyFileName);

            Type[] types = assembly.GetTypes();

            // Find a class that implement Customer Interface
            foreach (Type type in types)
            {
                if (type.IsClass && interfaceType.IsAssignableFrom(type) == true)
                {
                    detectorInstanceTypeName = type.FullName;
                    break;
                }
            }

            detectorAssembly = assembly;
        }

        public void InitializeDetector()
        {
            var ptfConfigStorage = Configuration.StorageRoot.GetNode(ConfigurationConsts.PtfConfig);
            PtfConfig = new PtfConfig(ptfConfigStorage.GetFiles().ToList());

            UtilCallBackFunctions.GetPropertyValue = (string name) =>
            {
                var property = this.PtfConfig.GetPropertyNodeByName(name);
                if (property != null) return property.Value;
                return null;
            };

            UtilCallBackFunctions.GetPropertiesByFile = (filename) =>
            {
                if (!this.PtfConfig.FileProperties.ContainsKey(filename))
                    return null;
                return this.PtfConfig.FileProperties[filename];
            };

            detectorAssemblyFileName = TestSuite.GetDetectorAssembly();

            Load(detectorAssemblyFileName);
        }

        #region Get/Set Prerequisites

        /// <summary>
        /// Gets the properties required for auto-detection.
        /// </summary>
        /// <returns>Prerequisites object.</returns>
        public PrerequisiteView GetPrerequisites()
        {
            stateLocker.EnterReadLock();
            try
            {
                return prerequisiteView;
            }
            finally
            {
                stateLocker.ExitReadLock();
            }
        }

        /// <summary>
        /// Sets the property values required for auto-detection.
        /// </summary>
        /// <returns>Returns true if succeeded, otherwise false.</returns>
        public bool SetPrerequisits(List<Property> prerequisiteProperties)
        {
            Dictionary<string, string> properties = [];
            foreach (var p in prerequisiteProperties)
            {
                properties.Add(p.Name, p.Value);
            };

            stateLocker.EnterWriteLock();
            try
            {
                prerequisiteView.Properties = prerequisiteProperties;
            }
            finally
            {
                stateLocker.ExitWriteLock();
            }

            return SetPrerequisitesInValueDetectorAssembly(properties);
        }

        #endregion

        /// <summary>
        /// Gets a list of the detection steps.
        /// </summary>
        /// <returns>A list of the detection steps.</returns>
        public List<DetectingItem> GetDetectedSteps()
        {
            stateLocker.EnterReadLock();
            try
            {
                return detectSteps;
            }
            finally
            {
                stateLocker.ExitReadLock();
            }
        }

        public DetectionOutcome GetDetectionOutcome()
        {
            return new DetectionOutcome(GetDetectionStatus(), detectedException);
        }

        public string GetDetectionLog()
        {
            var run = GetCurrentRun();
            try
            {
                if (run != null && !string.IsNullOrEmpty(run.LogPath) && File.Exists(run.LogPath))
                {
                    return File.ReadAllText(run.LogPath);
                }
                return string.Empty;
            }
            catch (IOException)
            {
                return $"{run?.LogPath} is being processed, please wait and try again.";
            }
        }

        public DetectionLogChunk GetDetectionLogChunk(long offset)
        {
            var normalizedOffset = Math.Max(0, offset);
            var run = GetCurrentRun();
            if (run == null)
            {
                return new DetectionLogChunk
                {
                    Offset = normalizedOffset,
                    NextOffset = normalizedOffset,
                    IsComplete = true
                };
            }

            logLocker.EnterReadLock();
            try
            {
                if (string.IsNullOrEmpty(run.LogPath) || !File.Exists(run.LogPath))
                {
                    return new DetectionLogChunk
                    {
                        RunId = run.Id,
                        Offset = normalizedOffset,
                        NextOffset = normalizedOffset,
                        Content = string.Empty,
                        IsComplete = Volatile.Read(ref run.LogClosed) == 1
                    };
                }

                using var logStream = new FileStream(run.LogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                if (normalizedOffset > logStream.Length)
                {
                    normalizedOffset = 0;
                }

                logStream.Seek(normalizedOffset, SeekOrigin.Begin);
                using var streamReader = new StreamReader(logStream);
                var content = streamReader.ReadToEnd();
                var nextOffset = logStream.Position;

                return new DetectionLogChunk
                {
                    RunId = run.Id,
                    Offset = normalizedOffset,
                    NextOffset = nextOffset,
                    Content = content,
                    IsComplete = Volatile.Read(ref run.LogClosed) == 1 && nextOffset >= logStream.Length
                };
            }
            catch (IOException)
            {
                return new DetectionLogChunk
                {
                    RunId = run.Id,
                    Offset = normalizedOffset,
                    NextOffset = normalizedOffset,
                    Content = string.Empty,
                    IsComplete = false
                };
            }
            finally
            {
                logLocker.ExitReadLock();
            }
        }

        #region Detection
        /// <summary>
        /// Reset AutoDetection settings
        /// </summary>
        public void Reset()
        {
            lock (lifecycleLocker)
            {
                ResetCore();
            }
        }

        private void ResetCore()
        {
            var previousRun = GetCurrentRun();
            CloseLogger(previousRun);

            stateLocker.EnterWriteLock();
            try
            {
                if (valueDetector != null)
                {
                    var detectorIsStillRunning =
                        previousRun?.Task != null &&
                        !previousRun.Task.IsCompleted &&
                        ReferenceEquals(previousRun.Detector, valueDetector);
                    if (!detectorIsStillRunning)
                    {
                        valueDetector.Dispose();
                    }
                    else
                    {
                        Volatile.Write(ref previousRun.Retired, 1);
                    }
                    valueDetector = null;
                }
            }
            finally
            {
                stateLocker.ExitWriteLock();
            }

            if (previousRun?.Task?.IsCompleted == true)
            {
                previousRun.Cancellation.Dispose();
            }

            var newDetectSteps = ValueDetector.GetDetectionSteps();

            stateLocker.EnterWriteLock();
            try
            {
                detectSteps = newDetectSteps;
            }
            finally
            {
                stateLocker.ExitWriteLock();
            }
            SetDetectionStatus(DetectionStatus.NotStart);
            detectedException = null;
        }

        /// <summary>
        /// Begins the auto-detection.
        /// </summary>
        /// <param name="DetectionEvent">Callback function when the detection finished.</param>
        public void StartDetection(DetectionCallback callback)
        {
            lock (lifecycleLocker)
            {
                if (GetDetectionStatus() == DetectionStatus.InProgress)
                {
                    return;
                }

                StartDetectionCore();
            }
        }

        public string StartDetection(List<Property> prerequisiteProperties, DetectionCallback callback)
        {
            lock (lifecycleLocker)
            {
                if (GetDetectionStatus() == DetectionStatus.InProgress)
                {
                    // Controller currently treats null as a handled failure result; avoid propagating an exception.
                    return null;
                }

                ResetCore();
                if (!SetPrerequisits(prerequisiteProperties))
                {
                    return null;
                }

                return StartDetectionCore();
            }
        }

        /// <summary>
        /// Stop the auto-detection
        /// </summary>
        public void StopDetection(Action callback)
        {
            lock (lifecycleLocker)
            {
                StopDetectionCore();
            }
        }

        #endregion

        /// <summary>
        /// Gets an object represents the detection summary.
        /// </summary>
        /// <returns>An object</returns>
        public List<ResultItemMap> GetDetectionSummary()
        {
            return ValueDetector.GetSUTSummary();
        }

        #region Apply Detection Summary to xml
        /// <summary>
        /// Apply the test case selection rules detected by the plug-in.
        /// </summary>
        /// <param name="ruleGroupsBySelectedRules">The rule groups by selected rules.</param>
        /// <param name="targetFilterIndex">Target filter index</param>
        /// <param name="mappingFilterIndex">Mapping filter index</param>
        public void ApplyDetectedRules(out IEnumerable<Common.Types.RuleGroup> ruleGroupsBySelectedRules, int targetFilterIndex, int mappingFilterIndex)
        {
            // Get the filter.
            var filter = TestSuite.GetTestCaseFilter();
            // create mapping table for the filter.
            CreateMappingTableForTestCaseFilter(filter, targetFilterIndex, mappingFilterIndex);
            // Update selected rules for the filter.
            foreach (var rule in ValueDetector.GetSelectedRules())
            {
                Kernel.Rule r = filter.FindRuleByName(rule.Name);
                if (r == null) throw new Exception(string.Format("Cannot find rule by name {0}.", rule.Name));
                switch (rule.Status)
                {
                    case Microsoft.Protocols.TestManager.Detector.RuleStatus.Selected:
                        r.SelectStatus = Kernel.RuleSelectStatus.Selected;
                        r.Status = RuleSupportStatus.Selected;
                        break;
                    case Microsoft.Protocols.TestManager.Detector.RuleStatus.NotSupported:
                        r.SelectStatus = Kernel.RuleSelectStatus.UnSelected;
                        r.Status = RuleSupportStatus.NotSupported;
                        break;
                    case Microsoft.Protocols.TestManager.Detector.RuleStatus.Unknown:
                        r.SelectStatus = Kernel.RuleSelectStatus.UnSelected;
                        r.Status = RuleSupportStatus.Unknown;
                        break;
                    default:
                        r.SelectStatus = Kernel.RuleSelectStatus.UnSelected;
                        r.Status = RuleSupportStatus.Default;
                        break;
                }
            }
            // Update filter to ruleGroups
            var ruleGroups = new List<Common.Types.RuleGroup>();
            foreach (var group in filter)
            {
                Common.Types.RuleGroup ruleGroup = new()
                {
                    Name = group.Name,
                    DisplayName = group.Name,
                    Rules = []
                };
                AddItems(ruleGroup.Rules, group);
                ruleGroups.Add(ruleGroup);
            }
            // Update the selected rule groups.
            ruleGroupsBySelectedRules = ruleGroups;
        }

        public void CreateMappingTableForTestCaseFilter(TestCaseFilter filter, int targetFilterIndex, int mappingFilterIndex)
        {
            if (targetFilterIndex == -1 ||
                mappingFilterIndex == -1)
            {
                return;
            }
            else
            {
                Dictionary<string, List<Kernel.Rule>> featureMappingTableForKernel = [];
                Dictionary<string, List<Kernel.Rule>> reverseMappingTableForKernel = [];
                Kernel.RuleGroup targetFilterGroup = filter[targetFilterIndex];
                Kernel.RuleGroup mappingFilterGroup = filter[mappingFilterIndex];
                Dictionary<string, Kernel.Rule> mappingRuleTable = CreateRuleTableFromRuleGroupForKernel(mappingFilterGroup);
                Dictionary<string, Kernel.Rule> targetRuleTable = CreateRuleTableFromRuleGroupForKernel(targetFilterGroup);

                var testCaseList = TestSuite.GetTestCases(null);

                foreach (TestManager.Common.TestCaseInfo testCase in testCaseList)
                {
                    List<string> categories = testCase.Category.ToList();
                    foreach (string target in targetRuleTable.Keys)
                    {
                        if (categories.Contains(target))
                        {
                            foreach (string category in categories)
                            {
                                if (!category.Equals(target))
                                {
                                    mappingRuleTable.TryGetValue(category, out Kernel.Rule currentRule);
                                    if (currentRule == null)
                                    {
                                        continue;
                                    }
                                    UpdateMappingTableForKernel(featureMappingTableForKernel, target, currentRule);
                                    // Add item to reverse mapping table
                                    UpdateMappingTableForKernel(reverseMappingTableForKernel, category, targetRuleTable[target]);
                                }
                            }
                            break;
                        }
                    }
                }

                targetFilterGroup.featureMappingTable = featureMappingTableForKernel;
                targetFilterGroup.mappingRuleGroup = mappingFilterGroup;

                mappingFilterGroup.reverseFeatureMappingTable = reverseMappingTableForKernel;
                mappingFilterGroup.targetRuleGroup = targetFilterGroup;
            }
        }

        private Dictionary<string, Kernel.Rule> CreateRuleTableFromRuleGroupForKernel(Kernel.RuleGroup ruleGroup)
        {
            Dictionary<string, Kernel.Rule> ruleTable = [];
            Stack<Kernel.Rule> ruleStack = new();
            foreach (Kernel.Rule r in ruleGroup) ruleStack.Push(r);
            while (ruleStack.Count > 0)
            {
                Kernel.Rule r = ruleStack.Pop();
                if (r.CategoryList.Count != 0 &&
                    !ruleTable.ContainsKey(r.CategoryList[0]))
                {
                    ruleTable.Add(r.CategoryList[0], r);
                }
                foreach (Kernel.Rule childRule in r) ruleStack.Push(childRule);
            }
            return ruleTable;
        }

        private void UpdateMappingTableForKernel(Dictionary<string, List<Kernel.Rule>> mappingTable, string target, Kernel.Rule currentRule)
        {
            if (mappingTable.ContainsKey(target))
            {
                if (!mappingTable[target].Contains(currentRule))
                {
                    mappingTable[target].Add(currentRule);
                }
            }
            else
            {
                mappingTable[target] = [currentRule];
            }
        }

        private void AddItems(IList<Common.Types.Rule> displayRules, List<Kernel.Rule> rules)
        {
            foreach (var rule in rules)
            {
                Common.Types.Rule displayRule = new()
                {
                    DisplayName = rule.Name,
                    Name = rule.Name,
                    Categories = rule.CategoryList.ToArray(),
                    SelectStatus = rule.SelectStatus == Kernel.RuleSelectStatus.Selected ? Common.Types.RuleSelectStatus.Selected : (rule.SelectStatus == Kernel.RuleSelectStatus.Partial ? Common.Types.RuleSelectStatus.Partial : Common.Types.RuleSelectStatus.UnSelected),
                };

                if (rule.Count > 0)
                {
                    AddItems(displayRule, rule);
                }
                displayRules.Add(displayRule);
            }
        }

        public void ApplyDetectedValues(ref IEnumerable<PropertyGroup> properties)
        {
            ValueDetector.GetDetectedProperty(out Dictionary<string, List<string>> propertiesByDetector);
            List<PropertyGroup> updatedPropertyGroupList = [];
            foreach (var ptfconfigProperty in properties)
            {
                PropertyGroup newPropertyGroup = new()
                {
                    Name = ptfconfigProperty.Name,
                    Items = ptfconfigProperty.Items,
                };

                foreach (var item in ptfconfigProperty.Items)
                {
                    var propertyFromDetctor = propertiesByDetector.Where(i => i.Key == item.Key);
                    if (propertyFromDetctor.Count() > 0)
                    {
                        var detectorPropertyValue = propertyFromDetctor.FirstOrDefault().Value;
                        var newProperty = newPropertyGroup.Items.Where(i => i.Key == item.Key).FirstOrDefault();
                        if (detectorPropertyValue.Count() == 1)
                        {
                            newProperty.Value = detectorPropertyValue[0];
                        }
                        else if (detectorPropertyValue.Count() > 0)
                        {
                            newProperty.Choices = detectorPropertyValue;
                            newProperty.Value = detectorPropertyValue[0];
                        }
                    }
                }

                updatedPropertyGroupList.Add(newPropertyGroup);
            }
            properties = updatedPropertyGroupList.ToArray();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the properties required for auto-detection.
        /// </summary>
        /// <returns>Prerequisites object.</returns>
        private Prerequisites GetPrerequisitsInValueDetectorAssembly()
        {
            return ValueDetector.GetPrerequisites();
        }

        /// <summary>
        /// Sets the values of the properties required for auto-detection.
        /// </summary>
        /// <param name="properties">Name - value map.</param>
        /// <returns>Returns true if provided values are enough, otherwise returns false.</returns>
        private bool SetPrerequisitesInValueDetectorAssembly(Dictionary<string, string> properties)
        {
            return ValueDetector.SetPrerequisiteProperties(properties);
        }

        /// <summary>
        /// Gets a list of properties to hide.
        /// </summary>
        /// <param name="rules">Test case selection rules</param>
        /// <returns>A list of properties to hide.</returns>
        public List<string> GetHiddenPropertiesInValueDetectorAssembly(List<CaseSelectRule> rules)
        {
            return ValueDetector.GetHiddenProperties(rules);
        }

        private string StartDetectionCore()
        {
            var run = new DetectionRun(this, ValueDetector);
            var token = run.Cancellation.Token;
            DetectContext context = new((instanceId, stepId, logStyle) =>
            {
                if (token.IsCancellationRequested || !instanceId.Equals(run.Id) || !IsCurrentRun(run))
                {
                    return;
                }

                run.StepIndex = stepId;
                SetDetectStepCurrentStatus(run, logStyle switch
                {
                    LogStyle.Default => DetectingStatus.Detecting,
                    LogStyle.Error => DetectingStatus.Error,
                    LogStyle.StepFailed => DetectingStatus.Failed,
                    LogStyle.StepSkipped => DetectingStatus.Skipped,
                    LogStyle.StepNotFound => DetectingStatus.NotFound,
                    LogStyle.StepPassed => DetectingStatus.Finished,
                    _ => DetectingStatus.Finished,
                });
            }, token, TestSuite.StorageRoot.AbsolutePath);

            run.Id = context.Id;
            run.LogPath = Path.Combine(
                TestSuite.StorageRoot.AbsolutePath,
                "Detector_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".log");
            run.LogWriter = new StreamWriter(run.LogPath);
            run.Task = new Task(() => ExecuteDetection(run, context));

            stateLocker.EnterWriteLock();
            try
            {
                currentRun = run;
                detectedException = null;
                detectionStatus = DetectionStatus.InProgress;
            }
            finally
            {
                stateLocker.ExitWriteLock();
            }

            run.Task.Start();
            return run.Id;
        }

        private void ExecuteDetection(DetectionRun run, DetectContext context)
        {
            activeDetectionRun.Value = run;
            try
            {
                run.Cancellation.Token.ThrowIfCancellationRequested();
                var succeeded = run.Detector.RunDetection(context);
                if (run.Cancellation.IsCancellationRequested)
                {
                    TryCompleteRun(run, DetectionStatus.Error, null, DetectingStatus.Cancelled, "Auto-detection was cancelled.");
                }
                else
                {
                    TryCompleteRun(
                        run,
                        succeeded ? DetectionStatus.Finished : DetectionStatus.Error,
                        null,
                        succeeded ? null : DetectingStatus.Error,
                        succeeded ? "Auto-detection completed." : "Auto-detection failed.");
                }
            }
            catch (OperationCanceledException) when (run.Cancellation.IsCancellationRequested)
            {
                TryCompleteRun(run, DetectionStatus.Error, null, DetectingStatus.Cancelled, "Auto-detection was cancelled.");
            }
            catch (Exception ex)
            {
                TryCompleteRun(
                    run,
                    DetectionStatus.Error,
                    ex,
                    DetectingStatus.Error,
                    $"Auto-detection failed:{Environment.NewLine}{ex}");
            }
            finally
            {
                activeDetectionRun.Value = null;
                if (Volatile.Read(ref run.Retired) == 1 || !IsCurrentRun(run))
                {
                    run.Detector.Dispose();
                    run.Cancellation.Dispose();
                }
            }
        }

        private void TryCompleteRun(
            DetectionRun run,
            DetectionStatus status,
            Exception exception,
            DetectingStatus? stepStatus,
            string message)
        {
            lock (lifecycleLocker)
            {
                if (!IsCurrentRun(run))
                {
                    return;
                }

                WriteDetectionLog(run, message);
                if (stepStatus.HasValue)
                {
                    SetDetectStepCurrentStatus(run, stepStatus.Value);
                }
                CloseLogger(run);

                stateLocker.EnterWriteLock();
                try
                {
                    detectedException = exception;
                    detectionStatus = status;
                }
                finally
                {
                    stateLocker.ExitWriteLock();
                }
            }
        }

        private static void RouteDetectorLog(string message, bool startNewLine, LogStyle style)
        {
            var run = activeDetectionRun.Value;
            run?.Owner.HandleDetectorLog(run, message);
        }

        private void HandleDetectorLog(DetectionRun run, string message)
        {
            if (!IsCurrentRun(run))
            {
                return;
            }

            WriteDetectionLog(run, message);
        }

        private void WriteDetectionLog(DetectionRun run, string message)
        {
            logLocker.EnterWriteLock();
            try
            {
                if (run.LogWriter != null)
                {
                    run.LogWriter.WriteLine("[{0}] {1}", DateTime.Now, message);
                    run.LogWriter.Flush();
                }
            }
            finally
            {
                logLocker.ExitWriteLock();
            }
        }

        private void StopDetectionCore()
        {
            var run = GetCurrentRun();
            if (run != null)
            {
                TryCompleteRun(run, DetectionStatus.Error, null, DetectingStatus.Cancelled, "Auto-detection was cancelled.");
                run.Cancellation.Cancel();
            }
        }

        private void SetDetectionStatus(DetectionStatus status)
        {
            stateLocker.EnterWriteLock();
            try
            {
                detectionStatus = status;
            }
            finally
            {
                stateLocker.ExitWriteLock();
            }
        }

        private DetectionStatus GetDetectionStatus()
        {
            stateLocker.EnterReadLock();
            try
            {
                return detectionStatus;
            }
            finally
            {
                stateLocker.ExitReadLock();
            }
        }

        private DetectionRun GetCurrentRun()
        {
            stateLocker.EnterReadLock();
            try
            {
                return currentRun;
            }
            finally
            {
                stateLocker.ExitReadLock();
            }
        }

        private bool IsCurrentRun(DetectionRun run)
        {
            stateLocker.EnterReadLock();
            try
            {
                return ReferenceEquals(currentRun, run);
            }
            finally
            {
                stateLocker.ExitReadLock();
            }
        }

        private void CloseLogger(DetectionRun run)
        {
            if (run == null)
            {
                return;
            }

            logLocker.EnterWriteLock();
            try
            {
                if (run.LogWriter != null)
                {
                    run.LogWriter.Close();
                    run.LogWriter.Dispose();
                    run.LogWriter = null;
                }
                Volatile.Write(ref run.LogClosed, 1);
            }
            finally
            {
                logLocker.ExitWriteLock();
            }
        }

        private void SetDetectStepCurrentStatus(DetectionRun run, DetectingStatus detectingStatus)
        {
            if (!IsCurrentRun(run))
            {
                return;
            }

            stateLocker.EnterWriteLock();
            try
            {
                if (run.StepIndex >= 0 && run.StepIndex < detectSteps.Count)
                {
                    detectSteps[run.StepIndex].DetectingStatus = detectingStatus;
                }
            }
            finally
            {
                stateLocker.ExitWriteLock();
            }
        }

        #endregion
    }
}