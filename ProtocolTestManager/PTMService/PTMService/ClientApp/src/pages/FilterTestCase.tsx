// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect } from 'react';
import { StepWizardChildProps } from 'react-step-wizard';
import { StackGap10 } from '../components/StackStyle';
import { StepPanel } from '../components/StepPanel';
import { useWindowSize } from '../components/UseWindowSize';
import { HeaderMenuHeight, WizardNavBar } from '../components/WizardNavBar';
import { getNavSteps, RunSteps } from '../model/DefaultNavSteps';
import { useDispatch, useSelector } from 'react-redux';
import { AppState } from '../store/configureStore';
import { RuleListPanel } from '../components/RuleListPanel';
import { ConfigurationsDataSrv } from '../services/Configurations';
import { FilterTestCaseActions } from "../actions/FilterTestCaseAction";
import { TestSuitesDataSrv } from '../services/TestSuites';
import { SelectedRuleGroup } from "../model/RuleGroup";

import { PrimaryButton, Stack } from '@fluentui/react';
import { ConfigureMethod_AutoDetection } from './ConfigureMethod';
export function FilterTestCase(props: any) {
    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;

    const dispatch = useDispatch();
    const filterInfo = useSelector((state: AppState) => state.filterInfo);
    const configureMethod = useSelector((state: AppState) => state.configureMethod);

    const navSteps = getNavSteps(wizardProps, configureMethod);
    const wizard = WizardNavBar(wizardProps, navSteps);
    const winSize = useWindowSize();

    useEffect(() => {
        dispatch(ConfigurationsDataSrv.getRules());
        dispatch(TestSuitesDataSrv.getTestSuiteTestCases());
    }, [dispatch])

    const onPreviousButtonClick = () => {
        if (configureMethod && configureMethod.selectedMethod && configureMethod.selectedMethod === ConfigureMethod_AutoDetection) {
            wizardProps.previousStep();
        } else {
            wizardProps.goToStep(RunSteps.CONFIGURE_METHOD);
        }
    };

    const onNextButtonClick = () => {
        dispatch(ConfigurationsDataSrv.setRules(() => {
            wizardProps.nextStep();
        }));
    };

    const checkedAction = (data: SelectedRuleGroup) => {
        dispatch(FilterTestCaseActions.setSelectedRuleAction(data));
    }
    return (
        <StepPanel leftNav={wizard} isLoading={filterInfo.isRulesLoading || filterInfo.isCasesLoading} errorMsg={filterInfo.errorMsg} >
            <div>
                <Stack horizontal style={{ paddingLeft: 10, paddingRight: 10 }} >
                    <div style={{ width: winSize.width * 0.30, }}>Filter</div>
                    <div>Selected Test Cases {filterInfo.listSelectedCases?.length}</div>
                </Stack>
                <hr style={{ border: "1px solid #d9d9d9" }} />
                <Stack horizontal style={{ paddingLeft: 10, paddingRight: 10 }}>
                    <Stack style={{ minWidth: winSize.width * 0.30, }} tokens={StackGap10}>
                        <RuleListPanel ruleGroups={filterInfo.ruleGroup} selected={filterInfo.selectedRules} checkedAction={checkedAction} />
                    </Stack>
                    <div style={{ width: 25 }} />
                    <Stack style={{ height: winSize.height - HeaderMenuHeight - 90, width: 100 + '%', overflowY: 'auto' }}>
                        {filterInfo.listSelectedCases && filterInfo.listSelectedCases.map(curr => <div key={curr}>{curr}</div>)}
                    </Stack>
                </Stack>
            </div>
            <div className='buttonPanel'>
                <Stack
                    horizontal
                    horizontalAlign="end" tokens={StackGap10}>
                    <PrimaryButton onClick={() => onPreviousButtonClick()} disabled={filterInfo.isPosting}>Previous</PrimaryButton>
                    <PrimaryButton onClick={() => onNextButtonClick()} disabled={filterInfo.isPosting}>Next</PrimaryButton>
                </Stack>
            </div>
        </StepPanel>
    )
};