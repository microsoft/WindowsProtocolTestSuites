// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Link } from '@fluentui/react'
import { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useHistory } from 'react-router-dom'
import { StepWizardChildProps } from 'react-step-wizard'
import { ConfigureMethodActions } from '../actions/ConfigureMethodAction'
import { DetectionResultActions } from '../actions/DetectionResultAction'
import { TestSuiteInfoActions } from '../actions/TestSuiteInfoAction'
import { WizardNavBarActions } from '../actions/WizardNavBarAction'
import { StepPanel } from '../components/StepPanel'
import { TestSuiteInfo } from '../components/TestSuiteInfo'
import { WizardNavBar } from '../components/WizardNavBar'
import { getNavSteps } from '../model/DefaultNavSteps'
import { TestSuite } from '../model/TestSuite'
import { TestSuitesDataSrv } from '../services/TestSuites'
import { AppState } from '../store/configureStore'

export function SelectTestSuite(props: any) {
  const wizardProps: StepWizardChildProps = props as StepWizardChildProps
  const testSuiteInfo = useSelector((state: AppState) => state.testSuiteInfo)
  const navSteps = getNavSteps(wizardProps)
  const wizard = WizardNavBar(wizardProps, navSteps)

  const dispatch = useDispatch()
  const history = useHistory()

  useEffect(() => {
    dispatch(TestSuitesDataSrv.getTestSuiteList())
    if (history.action === 'PUSH') {
      dispatch(WizardNavBarActions.setWizardNavBarAction(wizardProps.currentStep))
    }
  }, [dispatch])

  const testSuites = useSelector((state: AppState) => state.testSuites)

  const onSelectTestSuite = (testSuite: TestSuite) => {
    dispatch(TestSuiteInfoActions.setSelectedTestSuiteAction(testSuite))
    if (testSuite.Id !== testSuiteInfo.selectedTestSuite?.Id) {
        dispatch(WizardNavBarActions.setWizardNavBarAction(wizardProps.currentStep))
        // Resets configureMethod and detectionResult after switching testSuite
        dispatch(ConfigureMethodActions.setConfigurationMethodAction())
        dispatch(DetectionResultActions.resetDetectionResultAction())
    }
    wizardProps.nextStep()
  }

  const testSuitesList = testSuites.testSuiteList.map((item, index) => {
    return <TestSuiteInfo
      key={index}
      TestSuiteName={item.Name}
      Description={item.Description}
      Version={item.Version}
      OnSelect={() => onSelectTestSuite(item)} />
  })

  return (
    <StepPanel leftNav={wizard} isLoading={testSuites.isLoading} errorMsg={testSuites.errorMsg}>
      {
        (testSuitesList.length === 0)
          ? <div>
            No test suite found, please go to <Link onClick={() => history.push('/Management')}>Management</Link> page to install test suite
          </div>
          : testSuitesList
      }
    </StepPanel>
  )
};
