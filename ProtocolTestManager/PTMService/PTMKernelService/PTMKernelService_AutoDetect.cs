// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using System.Threading;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService
    {
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public int CreateAutoDetector(int configurationId)
        {
            var configuration = GetConfiguration(configurationId);

            var testSuite = configuration.TestSuite;

            cacheLock.EnterWriteLock();
            try
            {
                var autoDetector = AutoDetection.Create(testSuite);
                AutoDetectionPool.AddOrUpdate(configurationId, _ => autoDetector, (_, _) => autoDetector);
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }

            return configurationId;
        }

        public IAutoDetection GetAutoDetection(int configurationId)
        {
            cacheLock.EnterUpgradeableReadLock();
            try
            {
                if (!AutoDetectionPool.ContainsKey(configurationId))
                {
                    CreateAutoDetector(configurationId);
                }

                return AutoDetectionPool[configurationId];
            }
            finally
            {
                cacheLock.ExitUpgradeableReadLock();
            }
        }

        public PrerequisiteView GetPrerequisites(int configurationId)
        {
            return GetAutoDetection(configurationId).GetPrerequisites();
        }

        public bool SetPrerequisites(List<Property> prerequisitProperties, int configurationId)
        {
            return GetAutoDetection(configurationId).SetPrerequisits(prerequisitProperties);
        }

        public List<DetectingItem> GetDetectedSteps(int configurationId)
        {
            return GetAutoDetection(configurationId).GetDetectedSteps();
        }

        public DetectionOutcome GetDetectionOutcome(int configurationId)
        {
            return GetAutoDetection(configurationId).GetDetectionOutcome();
        }

        public void Reset(int configurationId)
        {
            GetAutoDetection(configurationId).Reset();
        }

        public void StartDetection(int configurationId, DetectionCallback callback)
        {
            GetAutoDetection(configurationId).StartDetection(callback);
        }

        public void StopDetection(int configurationId, Action callback)
        {
            GetAutoDetection(configurationId).StopDetection(callback);
        }

        public void ApplyDetectionResult(int configurationId)
        {
            GetAutoDetection(configurationId).ApplyDetectionResult();
        }

        public object GetDetectionSummary(int configurationId)
        {
            return GetAutoDetection(configurationId).GetDetectionSummary();
        }

        public string GetDetectionLog(int configurationId)
        {
            return GetAutoDetection(configurationId).GetDetectionLog();
        }
    }
}