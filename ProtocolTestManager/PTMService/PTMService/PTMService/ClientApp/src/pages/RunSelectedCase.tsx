// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React from 'react';
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard';
import { StepPanel } from '../components/StepPanel';
import { WizardNavBar } from '../components/WizardNavBar';
import { getNavSteps } from '../model/DefaultNavSteps';

export function RunSelectedCase(props: StepWizardProps) {
    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;

    // need check if need disable Auto-Detection and DetectionResult step
    const navSteps = getNavSteps(wizardProps);
    const wizard = WizardNavBar(wizardProps, navSteps);
    
    return (
        <div>
            <StepPanel leftNav={wizard} isLoading={false} errorMsg={''} >
                <div>RunSelectedCase</div>
            </StepPanel>
        </div>
    )
};