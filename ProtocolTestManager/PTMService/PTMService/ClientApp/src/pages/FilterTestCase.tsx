// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect } from 'react';
import { StepWizardChildProps } from 'react-step-wizard';
import { StackGap10 } from '../components/StackStyle';
import { StepPanel } from '../components/StepPanel';
import { useWindowSize } from '../components/UseWindowSize';
import { HeaderMenuHeight, WizardNavBar } from '../components/WizardNavBar';
import { getNavSteps } from '../model/DefaultNavSteps';
import { useDispatch, useSelector } from 'react-redux';
import { AppState } from '../store/configureStore';
import { RuleListPanel } from '../components/RuleListPanel';
import { ConfigurationsDataSrv } from '../services/Configurations';
import { SelectedRuleGroup } from "../model/RuleGroup";

import { PrimaryButton, Stack } from '@fluentui/react';
export function FilterTestCase(props: any) {
    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;

    const dispatch = useDispatch();
    const filterInfo = useSelector((state: AppState) => state.filterInfo);
    const configureMethod = useSelector((state: AppState) => state.configureMethod);

    const navSteps = getNavSteps(wizardProps, configureMethod);
    const wizard = WizardNavBar(wizardProps, navSteps);
    const winSize = useWindowSize();

    useEffect(() => {
        dispatch(ConfigurationsDataSrv.getRuleGroups());
    }, [dispatch])

    const onPreviousButtonClick = () => {
        wizardProps.previousStep();
    };

    const onNextButtonClick = () => {
        wizardProps.nextStep();
    };

    const checkedAction = (data: SelectedRuleGroup) => {
        dispatch(ConfigurationsDataSrv.setSelectedRule(data));
    }
    return (
        <StepPanel leftNav={wizard} isLoading={filterInfo.isLoading} errorMsg={filterInfo.errorMsg} >
            <div >
                <Stack horizontal style={{ paddingLeft: 10, paddingRight: 10 }} >
                    <div style={{ width: 350 }}>Filter</div>
                    <div>Selected Test Cases {filterInfo.listSelectedCases?.length}</div>
                </Stack>
                <hr style={{ border: "1px solid #d9d9d9" }} />
                <Stack>
                    <Stack horizontal>
                        <Stack tokens={{ childrenGap: 10 }}>
                            <RuleListPanel ruleGroups={filterInfo.ruleGroup} selected={filterInfo.selectedRules} checkedAction={checkedAction} />
                        </Stack>
                        <div style={{ width: 25 }} />
                        <Stack style={{ height: winSize.height - HeaderMenuHeight - 100, width: 100 + '%', overflowY: 'auto' }}>
                            {filterInfo.listSelectedCases && filterInfo.listSelectedCases.map(curr => <div key={curr}>{curr}</div>)}
                        </Stack>
                    </Stack>

                </Stack>
            </div>
            <div className='buttonPanel'>
                <Stack
                    horizontal
                    horizontalAlign="end" tokens={StackGap10} style={{ padding: 20 + 'px' }}>
                    <PrimaryButton onClick={() => onPreviousButtonClick()}>Previous</PrimaryButton>
                    <PrimaryButton onClick={() => onNextButtonClick()}>Next</PrimaryButton>
                </Stack>
            </div>
        </StepPanel>
    )
};