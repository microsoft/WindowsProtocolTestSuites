// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel
{
    public interface IAutoDetection
    {
        void InitializeDetector(int testSuiteId);

        PrerequisiteView GetPrerequisites();

        bool SetPrerequisits(List<PrerequisiteProperty> prerequisitProperties);

        List<DetectingItem> GetDetectedSteps(bool resetSteps = false);

        void StartDetection(DetectionCallback callback);

        void StopDetection(Action callback);

        object GetDetectionSummary();
    }
}