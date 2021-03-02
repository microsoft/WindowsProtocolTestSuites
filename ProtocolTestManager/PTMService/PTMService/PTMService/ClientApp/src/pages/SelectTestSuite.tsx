// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { PrimaryButton } from '@fluentui/react';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { StepWizardChildProps } from 'react-step-wizard';
import { StepPanel } from '../components/StepPanel';
import { WizardNavBar } from '../components/WizardNavBar';
import { DefaultNavSteps } from '../model/DefaultNavSteps';
import { TestSuitesActionCreators } from '../services/TestSuites';
import { AppState } from '../store/configureStore';

export function SelectTestSuite(props: any) {
    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;
    const navSteps = DefaultNavSteps.map(item => {
        if (item.TargetStep <= 1) {
            return {
                ...item,
                IsEnabled: true
            }
        } else if (item.TargetStep == 2) {
            return {
                ...item,
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
    const wizard = WizardNavBar(wizardProps, navSteps);

    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(TestSuitesActionCreators.getTestSuiteList());
    }, [dispatch])

    const testSuites = useSelector((state: AppState) => state.testsuites);

    return (
        <StepPanel leftNav={wizard} isLoading={testSuites.isLoading} errorMsg={testSuites.errorMsg}>
            {
                testSuites.testSuiteList.map((item, index) => {
                    return <div key={index} style={{ borderStyle: 'solid', borderWidth: 1, borderRadius: 5, borderColor: '#595959', padding: 10 }}>
                        <div>
                            <h2>{item.Name}</h2>
                            <p>{item.Description}</p>
                            <p>{item.Version}</p>
                            <PrimaryButton onClick={() => wizardProps.nextStep()}></PrimaryButton>
                        </div>
                    </div>
                })
            }
        </StepPanel>
    )
};