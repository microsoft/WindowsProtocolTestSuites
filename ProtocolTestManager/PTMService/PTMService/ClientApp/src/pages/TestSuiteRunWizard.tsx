// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import StepWizard from 'react-step-wizard'
import { SelectTestSuite } from './SelectTestSuite'
import { TestSuiteIntroduction } from './TestSuiteIntroduction'
import { SelectConfiguration } from './SelectConfiguration'
import { ConfigureMethod } from './ConfigureMethod'
import { AutoDetection } from './AutoDetection'
import { DetectionResult } from './DetectionResult'
import { FilterTestCase } from './FilterTestCase'
import { ConfigureTestCase } from './ConfigureTestCase'
import { RunSelectedCase } from './RunSelectedCase'
import { ConfigureAdapter } from './ConfigureAdapter'
import { useDispatch, useSelector } from 'react-redux'
import { WizardNavBarActions } from '../actions/WizardNavBarAction'
import { AppState } from '../store/configureStore'

export function TestSuiteRunWizard () {
  const wizardMethod = useSelector((state: AppState) => state.wizard)
  const dispatch = useDispatch()
  const onStepChange = (stepChange: {
    previousStep: number
    activeStep: number
  }) => {
    if (wizardMethod.lastStep < stepChange.activeStep) {
      dispatch(WizardNavBarActions.setWizardNavBarAction(stepChange.activeStep))
    }
  }
  return (
        <div>
            <StepWizard
                isHashEnabled={true}
                isLazyMount
                onStepChange={onStepChange}
            >
                <SelectTestSuite hashKey={'SelectTestSuite'} />
                <TestSuiteIntroduction hashKey={'TestSuiteIntroduction'} />
                <SelectConfiguration hashKey={'SelectConfiguration'} />
                <ConfigureMethod hashKey={'ConfigureMethod'} />
                <AutoDetection hashKey={'AutoDetection'} />
                <DetectionResult hashKey={'DetectionResult'} />
                <FilterTestCase hashKey={'FilterTestCase'} />
                <ConfigureTestCase hashKey={'ConfigureTestCase'} />
                <ConfigureAdapter hashKey={'ConfigureAdapter'} />
                <RunSelectedCase hashKey={'RunSelectedCase'} />
            </StepWizard>
        </div>
  )
};
