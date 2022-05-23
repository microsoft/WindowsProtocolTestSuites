// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { StackGap10 } from '../components/StackStyle'
import React, { useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useHistory } from 'react-router-dom'
import { StepWizardChildProps } from 'react-step-wizard'
import { StepPanel } from '../components/StepPanel'
import { WizardNavBar } from '../components/WizardNavBar'
import { getNavSteps } from '../model/DefaultNavSteps'
import { TestSuitesDataSrv } from '../services/TestSuites'
import { AppState } from '../store/configureStore'
import { useWindowSize } from '../components/UseWindowSize'
import { LoadingPanel } from '../components/LoadingPanel'
import { PrimaryButton, Stack, Link } from '@fluentui/react'

export function TestSuiteIntroduction (props: any) {
  const wizardProps: StepWizardChildProps = props as StepWizardChildProps

  const navSteps = getNavSteps(wizardProps)
  const wizard = WizardNavBar(wizardProps, navSteps)
  const winSize = useWindowSize()
  const dispatch = useDispatch()
  const history = useHistory()
  const testSuiteInfo = useSelector((state: AppState) => state.testSuiteInfo)
  const [documentTitle, setDocumentTitle] = useState('')
  const [homeDocTitle, setHomeDocTitle] = useState('')

  useEffect(() => {
    if (testSuiteInfo.selectedTestSuite != null) {
      dispatch(TestSuitesDataSrv.getTestSuiteIntroduction())
    }
  }, [dispatch])

  if (testSuiteInfo.selectedTestSuite === undefined) {
    return (
            <StepPanel leftNav={wizard} isLoading={false} >
                <div>No Test Suite selected, please go to <Link onClick={() => { wizardProps.firstStep() }}>Start page</Link></div>
            </StepPanel>
    )
  }
  const onPreviousButtonClick = () => {
    wizardProps.previousStep()
  }

  const onNextButtonClick = () => {
    wizardProps.nextStep()
  }

  const onBackClick = () => {
    history.goBack()
  }

  const isIFrame = (input: HTMLElement | null): input is HTMLIFrameElement =>
    input !== null && input.tagName === 'IFRAME'

  const pageLoaded = () => {
    const docUserGuide = document.getElementById('UserGuide')
    if (isIFrame(docUserGuide) && (docUserGuide.contentWindow != null)) {
      const currTitle = docUserGuide.contentDocument?.title || ''
      setDocumentTitle(currTitle)
      if (homeDocTitle == '') {
        setHomeDocTitle(currTitle)
      }
    }
  }
  const iframeUrl = `./api/testsuite/${testSuiteInfo.selectedTestSuite.Id}/userguide/index.html`
  const ariaLabel = `User guide for ${documentTitle}`
  return (
        <StepPanel leftNav={wizard} isLoading={testSuiteInfo.isLoading} errorMsg={testSuiteInfo.errorMsg}>
            <Stack horizontal style={{ paddingLeft: 10, paddingRight: 10 }} >
                <h3 style={{ width: winSize.width * 0.80 }}>{documentTitle}</h3>
                {homeDocTitle != documentTitle && <div><PrimaryButton onClick={() => onBackClick()} >Back</PrimaryButton></div>}
            </Stack>
            {homeDocTitle == '' && testSuiteInfo.errorMsg == undefined &&
                <LoadingPanel />
            }
            {testSuiteInfo.errorMsg == undefined &&
                <iframe id="UserGuide" aria-label={ariaLabel} role="none" style={{ border: 'aliceblue', height: winSize.height - 190, width: 100 + '%', overflowY: 'auto' }} src={iframeUrl} onLoad={pageLoaded} />
            }
            {testSuiteInfo.errorMsg == undefined &&
                <div className='buttonPanel'>
                    <Stack
                        horizontal
                        horizontalAlign="end" tokens={StackGap10}>
                        <PrimaryButton onClick={() => onPreviousButtonClick()} >Previous</PrimaryButton>
                        <PrimaryButton onClick={() => onNextButtonClick()} >Next</PrimaryButton>
                    </Stack>
                </div>
            }
        </StepPanel>
  )
};
