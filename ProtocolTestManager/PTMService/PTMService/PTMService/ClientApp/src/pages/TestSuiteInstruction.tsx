// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { PrimaryButton } from '@fluentui/react';
import React from 'react';
import { StepWizardChildProps } from 'react-step-wizard';
import { StepPanel } from '../components/StepPanel';
import { WizardNavBar } from '../components/WizardNavBar';
import { DefaultNavSteps } from '../model/DefaultNavSteps';

export function TestSuiteInstruction(props: any) {
    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;

    const wizard = WizardNavBar(wizardProps, DefaultNavSteps);

    return (
        <StepPanel leftNav={wizard} isLoading={false} >
            <div>TestSuiteInstruction</div>
            <PrimaryButton onClick={() => wizardProps.previousStep()}>Preview</PrimaryButton>
            <PrimaryButton onClick={() => wizardProps.nextStep()}>Next</PrimaryButton>
        </StepPanel>
    )
};