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
            var detector = GetAutoDetection(configurationId);

            var prerequisitView = detector.GetPrerequisites();

            return prerequisitView;
        }

        public bool SetPrerequisites(List<Property> prerequisitProperties, int configurationId)
        {
            var detector = GetAutoDetection(configurationId);

            return detector.SetPrerequisits(prerequisitProperties);
        }

        public List<DetectingItem> GetDetectedSteps(int configurationId)
        {
            var detector = GetAutoDetection(configurationId);

            return detector.GetDetectedSteps();
        }

        public DetectionOutcome GetDetectionOutcome(int configurationId)
        {
            var detector = GetAutoDetection(configurationId);

            return detector.GetDetectionOutcome();
        }

        public void Reset(int configurationId)
        {
            var detector = GetAutoDetection(configurationId);

            detector.Reset();
        }

        public void StartDetection(int configurationId, DetectionCallback callback)
        {
            var detector = GetAutoDetection(configurationId);

            detector.StartDetection(callback);
        }

        public void StopDetection(int configurationId, Action callback)
        {
            var detector = GetAutoDetection(configurationId);

            detector.StopDetection(callback);
        }

        public object GetDetectionSummary(int configurationId)
        {
            var detector = GetAutoDetection(configurationId);

            return detector.GetDetectionSummary();
        }
    }
}