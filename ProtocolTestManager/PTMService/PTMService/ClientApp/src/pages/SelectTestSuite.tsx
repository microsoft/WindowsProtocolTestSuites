// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { StepWizardChildProps } from 'react-step-wizard';
import { TestSuiteActions } from '../actions/TestSuitesActions';
import { StepPanel } from '../components/StepPanel';
import { TestSuiteInfo } from '../components/TestSuiteInfo';
import { WizardNavBar } from '../components/WizardNavBar';
import { getNavSteps } from '../model/DefaultNavSteps';
import { TestSuite } from '../model/TestSuite';
import { TestSuitesDataSrv } from '../services/TestSuites';
import { AppState } from '../store/configureStore';

export function SelectTestSuite(props: any) {
    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;

    const navSteps = getNavSteps(wizardProps);
    const wizard = WizardNavBar(wizardProps, navSteps);

    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(TestSuitesDataSrv.getTestSuiteList());
    }, [dispatch])

    const testSuites = useSelector((state: AppState) => state.testsuites);

    const onSelectTestSuite = (testSuite: TestSuite) => {
        dispatch(TestSuiteActions.setSelectedTestSuiteAction(testSuite));
        wizardProps.nextStep();
    }

    return (
        <StepPanel leftNav={wizard} isLoading={testSuites.isLoading} errorMsg={testSuites.errorMsg}>
            {
                testSuites.testSuiteList.map((item, index) => {
                    return <TestSuiteInfo key={index} TestSuiteName={item.Name} Description={item.Description} Version={item.Version} OnSelect={() => onSelectTestSuite(item)}></TestSuiteInfo>
                })
            }
        </StepPanel>
    )
};