// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import styled from '@emotion/styled'
import { StepWizardChildProps } from 'react-step-wizard'
import { StepNavItemInfo } from '../model/StepNavItemInfo'
import { useWindowSize } from './UseWindowSize'
import { useSelector } from 'react-redux'
import { AppState } from '../store/configureStore'
import { RunSteps } from '../model/DefaultNavSteps'
import * as ConfigureMethod from '../pages/ConfigureMethod'

export function WizardNavBar (wizardProps: StepWizardChildProps, navSteps: StepNavItemInfo[]) {
  const wizardState = useSelector((state: AppState) => state.wizard)
  const configureMethod = useSelector((state: AppState) => state.configureMethod)
  const detectResult = useSelector((state: AppState) => state.detectResult)

  const navStepItems = navSteps.map((item: StepNavItemInfo, index: number) => {
    let isEnabledStep = item.TargetStep <= wizardState.lastStep
    if (item.TargetStep === RunSteps.AUTO_DETECTION || item.TargetStep === RunSteps.DETECTION_RESULT) {
      if (RunSteps.CONFIGURE_METHOD < wizardState.lastStep &&
                wizardProps.currentStep < wizardState.lastStep &&
                configureMethod &&
                configureMethod.selectedMethod &&
                configureMethod.selectedMethod === ConfigureMethod.ConfigurationMethod_AutoDetection) {
        isEnabledStep = true
      } else {
        isEnabledStep = item.IsEnabled
      }

      if (item.TargetStep === RunSteps.DETECTION_RESULT && detectResult.detectionResult === undefined) {
        isEnabledStep = false
      }
    }
    if (item.IsActive) {
      return <SetupStep
                stepStatus='Running'
                tabIndex={0}
                key={index}>
                {item.Caption}
            </SetupStep>
    } else if (isEnabledStep) {
      return <SetupStep
                stepStatus='Complete'
                onKeyDown={(evt) => {
                  if (evt.key === 'Enter' || evt.key === ' ') {
                    wizardProps.goToStep(item.TargetStep)
                  }
                }}
                tabIndex={0}
                key={index}
                onClick={() => wizardProps.goToStep(item.TargetStep)}>
                {item.Caption}
            </SetupStep>
    } else {
      return <SetupStep
                stepStatus='NotStart'
                tabIndex={-1}
                key={index}>
                {item.Caption}
            </SetupStep>
    }
  })

  const winSize = useWindowSize()

  return (<LeftPanel style={{ height: winSize.height - HeaderMenuHeight }}>
        <Wizard>
            <VLine stepCount={navStepItems.length} />
            {navStepItems}
        </Wizard>
    </LeftPanel>)
}

export const LeftPanelWidth = 250
// header height 50 + 20 padding
export const HeaderMenuHeight = 72

export const LeftPanel = styled.div`
    position: absolute;
    top: 0px;
    left: 0px;
    width: ${LeftPanelWidth}px;
    z-index:999;
    border-right-color: #7565;
    border-style: none solid none none;
    border-width: 2px;
    padding-left: 5px;
    padding-Top: 20px;
    background-color: #f1f1f1;
    font-size:larger;
    font-weight: 500;
`

export const RightPanel = styled.div`
    padding-Top: 20px;
    & {
        margin-Left: ${LeftPanelWidth + 10}px;
        height: 100%;
        z-index: 999;
    }
    &::after {
        clear: "both";
    }
`

export const Wizard = styled.div`
    margin-left: 40px;
    position: relative;
`

export const VLine = styled.div<{ stepCount: number }>`
    width: 3px;
    background-color: #69c0ff;
    position: absolute;
    margin-left: -21px;
    height: ${props => (props.stepCount - 1) * 56}px;
`

type StepStatus = 'Running' | 'Complete' | 'NotStart'

const getStepColor = (stepStatus: StepStatus) => {
  switch (stepStatus) {
    case 'Running': return '#116dc3'
    case 'Complete': return '#297808'
    case 'NotStart': return '#6e6e6e'
  }
}

const getStepContentColor = (stepStatus: Exclude<StepStatus, 'NotStart'>) => {
  switch (stepStatus) {
    case 'Running': return '#003a8c'
    case 'Complete': return '#092b00'
  }
}

const getStepContent = (stepStatus: StepStatus) => {
  switch (stepStatus) {
    case 'Running': return '▶'
    case 'Complete': return '✔'
    case 'NotStart': return ''
  }
}

export const SetupStep = styled.div<{ stepStatus: StepStatus }>`
    padding-bottom: 30px;
    position: relative;
    ${props => props.stepStatus === 'Complete' ? 'cursor: pointer;' : ''}
    color: ${props => getStepColor(props.stepStatus)};
    &:before {
        content: '${props => getStepContent(props.stepStatus)}';
        ${props => props.stepStatus === 'NotStart' ? '' : `color: ${getStepContentColor(props.stepStatus)};`}
        display: block;
        width: 1.5em;
        height: 1.5em;
        text-align: center;
        border-radius: 50%;
        background-color: ${props => getStepColor(props.stepStatus)};
        position: absolute;
        margin-left:-35px;
    }
`
