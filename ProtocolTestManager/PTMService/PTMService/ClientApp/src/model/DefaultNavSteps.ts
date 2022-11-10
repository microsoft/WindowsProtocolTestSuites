// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { StepWizardChildProps } from 'react-step-wizard'
import { ConfigurationMethod_AutoDetection } from '../pages/ConfigureMethod'
import { ConfigureMethodState } from '../reducers/ConfigureMethodReducer'
import { StepNavItemInfo } from './StepNavItemInfo'

export const RunSteps = {
  SELECT_TEST_SUITE: 1,
  TEST_SUITE_INTRODUCTION: 2,
  SELECT_CONFIGURATION: 3,
  CONFIGURE_METHOD: 4,
  AUTO_DETECTION: 5,
  DETECTION_RESULT: 6,
  FILTER_TEST_CASE: 7,
  CONFIGURE_TEST_CASE: 8,
  CONFIGURE_ADAPTER: 9,
  RUN_SELECTED_TEST_CASE: 10
}

const DefaultNavSteps: StepNavItemInfo[] = [
  {
    Caption: 'Select Test Suite',
    TargetStep: RunSteps.SELECT_TEST_SUITE,
    IsEnabled: true,
    IsActive: true
  },
  {
    Caption: 'Test Suite Introduction',
    TargetStep: RunSteps.TEST_SUITE_INTRODUCTION,
    IsEnabled: false
  },
  {
    Caption: 'Select Configuration',
    TargetStep: RunSteps.SELECT_CONFIGURATION,
    IsEnabled: false
  },
  {
    Caption: 'Configure Method',
    TargetStep: RunSteps.CONFIGURE_METHOD,
    IsEnabled: false
  },
  {
    Caption: 'Auto-Detection',
    TargetStep: RunSteps.AUTO_DETECTION,
    IsEnabled: false
  },
  {
    Caption: 'Detection Result',
    TargetStep: RunSteps.DETECTION_RESULT,
    IsEnabled: false
  },
  {
    Caption: 'Filter Test Cases',
    TargetStep: RunSteps.FILTER_TEST_CASE,
    IsEnabled: false
  },
  {
    Caption: 'Configure Test Cases',
    TargetStep: RunSteps.CONFIGURE_TEST_CASE,
    IsEnabled: false
  },
  {
    Caption: 'Configure Adapters',
    TargetStep: RunSteps.CONFIGURE_ADAPTER,
    IsEnabled: false
  },
  {
    Caption: 'Run Selected Test Cases',
    TargetStep: RunSteps.RUN_SELECTED_TEST_CASE,
    IsEnabled: false
  }
]

export function getNavSteps (wizardProps: StepWizardChildProps, configureMethod?: ConfigureMethodState) {
  return DefaultNavSteps.map(item => {
    if (item.TargetStep < wizardProps.currentStep) {
      if (item.TargetStep === RunSteps.AUTO_DETECTION || item.TargetStep === RunSteps.DETECTION_RESULT) {
        if (configureMethod?.selectedMethod === ConfigurationMethod_AutoDetection) {
          return {
            ...item,
            IsActive: false,
            IsEnabled: true
          }
        } else {
          return {
            ...item,
            IsActive: false,
            IsEnabled: false
          }
        }
      } else {
        return {
          ...item,
          IsActive: false,
          IsEnabled: true
        }
      }
    } else if (item.TargetStep === wizardProps.currentStep) {
      return {
        ...item,
        IsEnabled: true,
        IsActive: true
      }
    } else {
      return {
        ...item,
        IsActive: false,
        IsEnabled: false
      }
    }
  })
}
