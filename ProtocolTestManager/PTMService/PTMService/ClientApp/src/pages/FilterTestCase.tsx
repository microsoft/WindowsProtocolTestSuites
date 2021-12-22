// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect } from 'react'
import { StepWizardChildProps } from 'react-step-wizard'
import { StackGap10 } from '../components/StackStyle'
import { StepPanel } from '../components/StepPanel'
import { WizardNavBar } from '../components/WizardNavBar'
import { getNavSteps, RunSteps } from '../model/DefaultNavSteps'
import { useDispatch, useSelector } from 'react-redux'
import { AppState } from '../store/configureStore'
import { RuleListPanel } from '../components/RuleListPanel'
import { ConfigurationsDataSrv } from '../services/Configurations'
import { FilterTestCaseActions } from '../actions/FilterTestCaseAction'
import { TestSuitesDataSrv } from '../services/TestSuites'
import { SelectedRuleGroup } from '../model/RuleGroup'
import { IStackItemTokens, IStackTokens, PrimaryButton, Stack } from '@fluentui/react'
import { ConfigurationMethod_AutoDetection } from './ConfigureMethod'
import { PropertyGroupsActions } from '../actions/PropertyGroupsAction'
import { InvalidAppStateNotification } from '../components/InvalidAppStateNotification'

export const FilterTestCase: React.FC<any> = (props: any) => {
    const wizardProps: StepWizardChildProps = props as StepWizardChildProps

    const dispatch = useDispatch()
    const testSuiteInfo = useSelector((state: AppState) => state.testSuiteInfo)
    const configuration = useSelector((state: AppState) => state.configurations)
    const filterInfo = useSelector((state: AppState) => state.filterInfo)
    const configureMethod = useSelector((state: AppState) => state.configureMethod)
    const detectionResult = useSelector((state: AppState) => state.detectResult)

    const navSteps = getNavSteps(wizardProps, configureMethod)
    const wizard = WizardNavBar(wizardProps, navSteps)

    useEffect(() => {
        dispatch(ConfigurationsDataSrv.getRules())
        dispatch(TestSuitesDataSrv.getTestSuiteTestCases())
    }, [dispatch])

    if (testSuiteInfo.selectedTestSuite === undefined || configuration.selectedConfiguration === undefined) {
        return <InvalidAppStateNotification
            testSuite={testSuiteInfo.selectedTestSuite}
            configuration={configuration.selectedConfiguration}
            wizard={wizard}
            wizardProps={wizardProps} />
    }

    const onPreviousButtonClick: React.MouseEventHandler<unknown> = () => {
        if (configureMethod?.selectedMethod === ConfigurationMethod_AutoDetection && detectionResult.detectionResult !== undefined) {
            wizardProps.previousStep()
        } else {
            wizardProps.goToStep(RunSteps.CONFIGURE_METHOD)
        }
    }

    const onNextButtonClick: React.MouseEventHandler<unknown> = () => {
        dispatch(ConfigurationsDataSrv.setRules(() => {
            wizardProps.nextStep()
        }))
    }

    const checkedAction = (data: SelectedRuleGroup): void => {
        dispatch(FilterTestCaseActions.setSelectedRuleAction(data))
    }

    const stackTokens: IStackTokens = {
        maxHeight: '100%'
    }

    const stackItemTokens: IStackItemTokens = {
        padding: '0 10px'
    }

    return (
        <StepPanel leftNav={wizard} isLoading={filterInfo.isRulesLoading || filterInfo.isCasesLoading} errorMsg={filterInfo.errorMsg} >
            <Stack tokens={stackTokens} verticalFill>
                <Stack.Item grow style={{ overflowY: 'hidden' }}>
                    <Stack horizontal tokens={stackTokens}>
                        <Stack.Item grow tokens={stackItemTokens} style={{ overflowY: 'auto' }}>
                            <RuleListPanel ruleGroups={filterInfo.ruleGroup} selected={filterInfo.selectedRules} checkedAction={checkedAction} />
                        </Stack.Item>
                        <Stack.Item grow tokens={stackItemTokens} style={{ overflowY: 'auto' }}>
                            <div>Selected Test Cases {filterInfo.listSelectedCases.length}</div>
                            {filterInfo.listSelectedCases.map(curr => <div key={curr.Name}>{curr.Name}</div>)}
                        </Stack.Item>
                    </Stack>
                </Stack.Item>
                <Stack.Item disableShrink className='buttonPanel'>
                    <Stack
                        horizontal
                        horizontalAlign="end" tokens={StackGap10}>
                        <PrimaryButton onClick={onPreviousButtonClick} disabled={filterInfo.isPosting}>Previous</PrimaryButton>
                        <PrimaryButton onClick={onNextButtonClick} disabled={filterInfo.isPosting}>Next</PrimaryButton>
                    </Stack>
                </Stack.Item>
            </Stack>
        </StepPanel>
    )
}
