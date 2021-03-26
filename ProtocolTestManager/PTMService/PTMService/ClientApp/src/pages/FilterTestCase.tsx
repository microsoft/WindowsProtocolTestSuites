// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React from 'react';
import { useSelector } from 'react-redux';
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard';
import { StepPanel } from '../components/StepPanel';
import { WizardNavBar } from '../components/WizardNavBar';
import { getNavSteps } from '../model/DefaultNavSteps';
import { AppState } from '../store/configureStore';

export function FilterTestCase(props: StepWizardProps) {

    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;
    const navSteps = getNavSteps(wizardProps);
    const wizard = WizardNavBar(wizardProps, navSteps);
    const configuration = useSelector((state: AppState) => state.configurations);
    
    return (
        <div>
            <StepPanel leftNav={wizard} isLoading={false} errorMsg={''} >
                <div>FilterTestCase</div>
            </StepPanel>
        </div>
    )
};