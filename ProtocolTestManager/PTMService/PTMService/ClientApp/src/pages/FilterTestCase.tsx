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
import { CapabilitiesTreePanel } from '../components/CapabilitiesTreePanel'
import { ConfigurationsDataSrv } from '../services/Configurations'
import { FilterTestCaseActions } from '../actions/FilterTestCaseAction'
import { TestSuitesDataSrv } from '../services/TestSuites'
import { SelectedRuleGroup } from '../model/RuleGroup'
import { IStackItemTokens, IStackTokens, PrimaryButton, Stack, Dropdown } from '@fluentui/react'
import { ConfigurationMethod_AutoDetection } from './ConfigureMethod'
import { InvalidAppStateNotification } from '../components/InvalidAppStateNotification'
import { CapabilitiesDataSrv } from '../services/Capabilities'

export const FilterTestCase: React.FC<any> = (props: any) => {
  const wizardProps: StepWizardChildProps = props as StepWizardChildProps

  const dispatch = useDispatch()
  const testSuiteInfo = useSelector((state: AppState) => state.testSuiteInfo)
  const configuration = useSelector((state: AppState) => state.configurations)
  const filterInfo = useSelector((state: AppState) => state.filterInfo)
  const configureMethod = useSelector((state: AppState) => state.configureMethod)
  const detectionResult = useSelector((state: AppState) => state.detectResult)
  const capabilitiesListState = useSelector((state: AppState) => state.capabilitiesList)

  const navSteps = getNavSteps(wizardProps, configureMethod)
  const wizard = WizardNavBar(wizardProps, navSteps)

  useEffect(() => {
    dispatch(ConfigurationsDataSrv.getRules())
    dispatch(TestSuitesDataSrv.getTestSuiteTestCases())
    dispatch(CapabilitiesDataSrv.getCapabilitiesFiles())
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

  const buildCapabilitiesDropdownOptions = () => {
    const testSuiteName = testSuiteInfo.selectedTestSuite?.TestSuiteName ?? ''
    const testSuiteVersion = testSuiteInfo.selectedTestSuite?.Version ?? ''
    const options = capabilitiesListState.displayList
      .filter(c => c.TestSuiteName === testSuiteName && c.TestSuiteVersion === testSuiteVersion)
      .map(c => {
        return {
          key: c.Id,
          text: `${c.Name}`
        }
      })

    options.unshift({
      key: -1,
      text: '(None)'
    })

    return options
  }

  const onCategoriesSelected = (values: string[]): void => {
    dispatch(FilterTestCaseActions.selectCapabilitiesFileCategoriesAction(values))
  }

  const onCategoriesExpanded = (values: string[]): void => {
    dispatch(FilterTestCaseActions.expandCapabilitiesFileCategoriesAction(values))
  }

  const setCapabilitiesFileId = (value: any) => {
    dispatch(FilterTestCaseActions.selectCapabilitiesFileAction(value))

    if (value !== -1) {
      dispatch(TestSuitesDataSrv.getCapabilitiesConfig(value))
    }
  }

  return (
        <StepPanel leftNav={wizard} isLoading={filterInfo.isRulesLoading || filterInfo.isCasesLoading} errorMsg={filterInfo.errorMsg} >
            <Stack tokens={stackTokens} verticalFill>
                <Stack.Item grow style={{ overflowY: 'hidden' }}>
                    <Stack horizontal tokens={stackTokens}>
                        <Stack.Item grow tokens={stackItemTokens} style={{ overflowY: 'auto' }}>

                        <Dropdown
                            key={props.key}
                            style={{ alignSelf: 'center' }}
                            placeholder='Kindly select a capabilities file'
                            label='Using the following capabilities file:'
                            options={buildCapabilitiesDropdownOptions()}
                            selectedKey={filterInfo.selectedCapabilitiesFileId}
                            onChange={(_, newValue, __) => { const value: any = newValue?.key; setCapabilitiesFileId(value) }} />
                          {
                              filterInfo.selectedCapabilitiesFileId === -1
                                ? <RuleListPanel ruleGroups={filterInfo.ruleGroup} selected={filterInfo.selectedRules} checkedAction={checkedAction} />
                                : <CapabilitiesTreePanel groups={filterInfo.groups} onChecked={(values) => onCategoriesSelected(values)}
                                      selectedCategories={filterInfo.selectedCategories}
                                      onExpanded={(values) => onCategoriesExpanded(values)}
                                      expandedCategories={filterInfo.expandedCategoriesByFile.get(filterInfo.selectedCapabilitiesFileId)!}                                  />
                            }

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
