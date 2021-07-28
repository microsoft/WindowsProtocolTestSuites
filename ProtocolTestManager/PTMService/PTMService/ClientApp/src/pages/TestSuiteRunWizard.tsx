// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import StepWizard from "react-step-wizard";
import { SelectTestSuite } from "./SelectTestSuite";
import { SelectConfiguration } from "./SelectConfiguration";
import { ConfigureMethod } from "./ConfigureMethod";
import { AutoDetection } from "./AutoDetection";
import { DetectionResult } from "./DetectionResult";
import { FilterTestCase } from "./FilterTestCase";
import { ConfigureTestCase } from "./ConfigureTestCase";
import { RunSelectedCase } from "./RunSelectedCase";
import { ConfigureAdapter } from "./ConfigureAdapter";

export function TestSuiteRunWizard() {
    return (
        <div>
            <StepWizard
                isHashEnabled={true}
                isLazyMount
            >
                <SelectTestSuite hashKey={'SelectTestSuite'} />
                <SelectConfiguration hashKey={'SelectConfiguration'} />
                <ConfigureMethod hashKey={'ConfigureMethod'} />
                <AutoDetection hashKey={'AutoDetection'} />
                <DetectionResult hashKey={'DetectionResult'} />
                <FilterTestCase hashKey={'FilterTestCase'} />
                <ConfigureTestCase hashKey={'ConfigureTestCase'} />
                <ConfigureAdapter hashKey={'ConfigureAdapter'} />
                <RunSelectedCase hashKey={'RunSelectedCase'} />
            </StepWizard>
        </div>
    )
};