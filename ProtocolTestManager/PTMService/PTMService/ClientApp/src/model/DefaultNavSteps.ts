// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { StepWizardChildProps } from "react-step-wizard";
import { StepNavItemInfo } from "./StepNavItemInfo";

export const RunSteps = {
    SELECT_TEST_SUITE: 1,
    SELECT_CONFIGURATION: 2,
    CONFIGURE_METHOD: 3,
    AUTO_DETECTION: 4,
    DETECTION_RESULT: 5,
    FILTERTESTCASE: 6,
    CONFIGURE_TEST_CASE: 7,
    CONFIGURE_ADAPTER: 8,
    RUN_SELECTED_TEST_CASE: 9,
}

const DefaultNavSteps: StepNavItemInfo[] = [
    {
        Caption: 'Select Test Suite',
        TargetStep: RunSteps.SELECT_TEST_SUITE,
        IsEnabled: true,
        IsActive: true
    },
    {
        Caption: 'Select Configuration',
        TargetStep: RunSteps.SELECT_CONFIGURATION,
        IsEnabled: false,
    },
    {
        Caption: 'Configure Method',
        TargetStep: RunSteps.CONFIGURE_METHOD,
        IsEnabled: false,
    },
    {
        Caption: 'Auto-Detection',
        TargetStep: RunSteps.AUTO_DETECTION,
        IsEnabled: false,
    },
    {
        Caption: 'Detection Result',
        TargetStep: RunSteps.DETECTION_RESULT,
        IsEnabled: false,
    },
    {
        Caption: 'Filter Test Case',
        TargetStep: RunSteps.FILTERTESTCASE,
        IsEnabled: false,
    },
    {
        Caption: 'Configure Test Case',
        TargetStep: RunSteps.CONFIGURE_TEST_CASE,
        IsEnabled: false,
    },
    {
        Caption: 'Configure Adapter',
        TargetStep: RunSteps.CONFIGURE_ADAPTER,
        IsEnabled: false,
    },
    {
        Caption: 'Run Selected Test Case',
        TargetStep: RunSteps.RUN_SELECTED_TEST_CASE,
        IsEnabled: false,
    },
];

export function getNavSteps(wizardProps: StepWizardChildProps) {
    return DefaultNavSteps.map(item => {
        if (item.TargetStep <= wizardProps.currentStep) {
            return {
                ...item,
                IsEnabled: true
            }
        } else if (item.TargetStep === wizardProps.currentStep) {
            return {
                ...item,
                IsEnabled: true,
                IsActive: true
            }
        } else {
            return {
                ...item,
                IsActive: false,
                IsEnabled: false
            }
        }
    });
}