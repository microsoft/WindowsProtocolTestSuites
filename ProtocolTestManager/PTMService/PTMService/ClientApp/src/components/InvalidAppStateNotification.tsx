// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Link } from '@fluentui/react'
import { StepWizardChildProps } from 'react-step-wizard'
import { Configuration } from '../model/Configuration'
import { TestSuite } from '../model/TestSuite'
import { StepPanel } from './StepPanel'

interface InvalidAppStateNotificationProps {
  testSuite?: TestSuite
  configuration?: Configuration
  wizard: JSX.Element
  wizardProps: StepWizardChildProps
}

export const InvalidAppStateNotification: React.FunctionComponent<InvalidAppStateNotificationProps> = (props) => {
  const { testSuite, configuration, wizard, wizardProps } = props

  if (testSuite === undefined) {
    return (
            <StepPanel leftNav={wizard} isLoading={false} >
                <div>No Test Suite is selected, please go to <Link onClick={() => { wizardProps.firstStep() }}>Start page</Link></div>
            </StepPanel>
    )
  } else if (configuration === undefined) {
    return (
            <StepPanel leftNav={wizard} isLoading={false} >
                <div>No configuration is selected, please go to <Link onClick={() => { wizardProps.firstStep() }}>Start page</Link></div>
            </StepPanel>
    )
  } else {
    return (
            <StepPanel leftNav={wizard} isLoading={false} >
                <div>It is impossible here, InvalidAppStateNotification FunctionComponent should only be called when the selected test suite or configuration is undefined!</div>
            </StepPanel>
    )
  }
}