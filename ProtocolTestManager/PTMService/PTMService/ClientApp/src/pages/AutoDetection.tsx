// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  Link,
  DetailsList,
  IColumn,
  Label,
  PrimaryButton,
  Stack,
  Spinner,
  SpinnerSize
} from '@fluentui/react'
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard'
import { PopupModal } from '../components/PopupModal'
import { StepPanel } from '../components/StepPanel'
import { HeaderMenuHeight, LeftPanelWidth, WizardNavBar } from '../components/WizardNavBar'
import { getNavSteps } from '../model/DefaultNavSteps'
import { AppState } from '../store/configureStore'
import { useDispatch, useSelector } from 'react-redux'
import { useWindowSize } from '../components/UseWindowSize'
import { LoadingPanel } from '../components/LoadingPanel'
import { Property } from '../model/Property'
import { useEffect, useState, CSSProperties, ReactElement } from 'react'
import { AutoDetectionDataSrv } from '../services/AutoDetection'
import { SelectionMode } from '@uifabric/experiments/lib/Utilities'
import { AutoDetectionActions } from '../actions/AutoDetectionAction'
import { DetectingItem, DetectionStatus } from '../model/AutoDetectionData'
import { useBoolean, useForceUpdate } from '@uifabric/react-hooks'
import { useRef } from 'react'
import { useMemo } from 'react'
import { PropertyGroupView } from '../components/PropertyGroupView'
import { PropertyGroup } from '../model/PropertyGroup'

export function AutoDetection(props: StepWizardProps) {
  const wizardProps: StepWizardChildProps = props as StepWizardChildProps

  const [isAutoDetectionWarningDialogOpen, { setTrue: showAutoDetectionWarningDialog, setFalse: hideAutoDetectionWarningDialog }] = useBoolean(false)
  const [isAutoDetectionLogDialogOpen, { setTrue: showAutoDetectionLogDialog, setFalse: hideAutoDetectionLogDialog }] = useBoolean(false)
  const autoDetection = useSelector((state: AppState) => state.autoDetection)
  const autoDetectionLog = useMemo(() => autoDetection.log, [autoDetection])
  const prerequisitePropertyGroup = useMemo<PropertyGroup>(() => { return { Name: 'Prerequisite Properties', Items: autoDetection.prerequisite?.Properties ?? [] } }, [autoDetection])
  const navSteps = getNavSteps(wizardProps)
  const wizard = WizardNavBar(wizardProps, navSteps)
  const dispatch = useDispatch()

  const winSize = useWindowSize()
  const [detectingTimes, setDetectingTimes] = useState(-999)
  const [detecting, setDetecting] = useState(false)
  const prerequisiteSummaryRef = useRef<HTMLDivElement>(null)
  const [headerHeight, setHeaderHeight] = useState<number>(HeaderMenuHeight)

  const forceUpdate = useForceUpdate()

  useEffect(() => {
    dispatch(AutoDetectionDataSrv.getAutoDetectionPrerequisite())
  }, [dispatch])

  useEffect(() => {
    dispatch(AutoDetectionDataSrv.getAutoDetectionSteps())
  }, [dispatch])

  useEffect(() => {
    if (detectingTimes === -999 || shouldAutoDetectionStop()) {
      if (autoDetection.detectionSteps?.Result.Status === DetectionStatus.Error && autoDetection.showWarning) {
        showAutoDetectionWarningDialog()
      }
      setDetecting(false)
      return
    }

    const timer = setTimeout(() => {
      setDetectingTimes(detectingTimes - 1)
      dispatch(AutoDetectionDataSrv.updateAutoDetectionSteps())
    }, 1000)

    return () => clearTimeout(timer)
  }, [dispatch, autoDetection, detectingTimes])

  useEffect(() => {
    const timer = setTimeout(() => {
      if (prerequisiteSummaryRef.current !== null) {
        setHeaderHeight(prerequisiteSummaryRef.current.offsetHeight + HeaderMenuHeight)
        if (autoDetection.prerequisite?.Summary && prerequisiteSummaryRef.current.offsetHeight === 0) {
          forceUpdate()
        }
      }
    })

    return () => clearTimeout(timer)
  }, [autoDetection, prerequisiteSummaryRef])

  const onPreviousButtonClick = () => {
    wizardProps.previousStep()
  }

  const onPropertyValueChange = (updatedProperty: Property) => {
    dispatch(AutoDetectionActions.updateAutoDetectionPrerequisiteAction(updatedProperty))
  }

  // This function is to determine if detection status is suitable for running.
  const shouldAutoDetectionStop = () => {
    const statusToStop = autoDetection.detectionSteps?.Result.Status === DetectionStatus.Finished ||
      autoDetection.detectionSteps?.Result.Status === DetectionStatus.Error ||
      autoDetection.detectionSteps?.Result.Status === DetectionStatus.NotStart

    // We should get steps for 3 times no matter the status.
    if (detectingTimes >= 98) {
      return false
    }

    return statusToStop
  }

  const onNextButtonClick = () => {
    if (autoDetection.detectionSteps?.Result.Status === DetectionStatus.Finished) {
      dispatch(AutoDetectionDataSrv.applyDetectionResult(() => {
        // Next page
        wizardProps.nextStep()
      }))
    }
  }

  const onDetectButtonClick = () => {
    if (detecting) {
      // Cancel
      dispatch(AutoDetectionDataSrv.stopAutoDetection())
      setDetectingTimes(-999)
      setDetecting(false)
    } else {
      // Start detect
      dispatch(AutoDetectionDataSrv.startAutoDetection())
      setDetectingTimes(100)
      setDetecting(true)
    }
  }

  const isPreviousButtonDisabled = (): boolean => detecting

  const isDetectButtonDisabled = (): boolean => autoDetection.detectionSteps?.Result.Status === DetectionStatus.InProgress

  const isNextButtonDisabled = (): boolean => autoDetection.detectionSteps?.Result.Status !== DetectionStatus.Finished

  const getDetectButtonText = (): string => detecting ? 'Cancel' : 'Detect'

  const onFailClick = () => dispatch(AutoDetectionDataSrv.getAutoDetectionLog(showAutoDetectionLogDialog))

  const onCloseAutoDetectionWarningDialogClick = () => {
    dispatch(AutoDetectionActions.setShowWarningAction(false))
    hideAutoDetectionWarningDialog()
  }

  const getStepColumns = (): IColumn[] => {
    const getStyle = (status: string): CSSProperties => {
      if (status === 'Failed') {
        return { paddingLeft: 5, color: 'red' }
      } else if (status === 'Finished') {
        return { paddingLeft: 5, color: 'green' }
      } else {
        return { paddingLeft: 5 }
      }
    }

    const renderDetectingContent = (item: Property, index: number | undefined): ReactElement => {
      return (
        <Label>
          <div style={{ paddingLeft: 5 }} >{item.Name}</div>
        </Label>
      )
    }

    const renderDetectingStatus = (item: DetectingItem): ReactElement => {
      return (
        <Label>
          <div style={getStyle(item.Status)} >
            {
              ((status) => {
                if (status === 'Failed') {
                  return (
                    <Link
                      underline
                      style={getStyle(status)}
                      onClick={onFailClick}>
                      {status}
                    </Link>
                  )
                } else if ((status === 'Pending' || status === 'Detecting') && detecting) {
                  return (
                    <Stack horizontal>
                      <Spinner size={SpinnerSize.medium} />
                      <div style={getStyle(status)}>{status}</div>
                    </Stack>
                  )
                } else {
                  return (<div style={getStyle(status)}>{status}</div>)
                }
              })(item.Status)
            }
          </div>
        </Label>
      )
    }

    return [{
      key: 'DetectingContent',
      name: 'DetectingContent',
      fieldName: 'DetectingContent',
      minWidth: 240,
      isRowHeader: true,
      isResizable: true,
      onRender: renderDetectingContent
    },
    {
      key: 'DetectingStatus',
      name: 'DetectingStatus',
      fieldName: 'DetectingStatus',
      minWidth: 480,
      isResizable: true,
      isPadded: true,
      onRender: renderDetectingStatus
    }]
  }

  return (
    <div>
      <StepPanel leftNav={wizard} isLoading={autoDetection.isPrerequisiteLoading || autoDetection.isDetectionStepsLoading} errorMsg={autoDetection.errorMsg} >
        <Stack style={{ paddingLeft: 10 }}>
          <div ref={prerequisiteSummaryRef}>
            <Stack style={{ paddingLeft: 10 }}>
              {autoDetection.prerequisite?.Summary.split('\n').map((line) => <p key={line}>{line.trim()}</p>)}
            </Stack>
          </div>
          <div style={{ paddingLeft: 30, maxHeight: (winSize.height - headerHeight) / 2, overflowY: 'auto' }}>
            {
              autoDetection.isPrerequisiteLoading
                ? <LoadingPanel />
                : <PropertyGroupView
                  winSize={{ ...winSize, height: (winSize.height - headerHeight) / 2 }}
                  latestPropertyGroup={prerequisitePropertyGroup}
                  propertyGroup={prerequisitePropertyGroup}
                  onValueChange={onPropertyValueChange} />
            }
          </div>
          <div style={{ paddingLeft: 30, paddingTop: 20, maxHeight: (winSize.height - headerHeight) / 2, overflowY: 'auto' }}>
            {
              autoDetection.isDetectionStepsLoading
                ? <LoadingPanel />
                : <div>
                  <Stack horizontal style={{ paddingTop: 20 }} horizontalAlign='start' tokens={{ childrenGap: 10 }}>
                    <div style={{ borderLeft: '2px solid #bae7ff', minHeight: 200 }}>
                      <DetailsList
                        columns={getStepColumns()}
                        items={autoDetection.detectionSteps?.DetectingItems ?? []}
                        compact
                        selectionMode={SelectionMode.none}
                        isHeaderVisible={false}
                      />
                    </div>
                  </Stack>
                </div>
            }
          </div>
          <div className='buttonPanel' style={{ position: 'fixed', left: LeftPanelWidth + 20, bottom: 10, right: 0 }}>
            <Stack horizontal horizontalAlign="end" tokens={{ childrenGap: 10 }} >
              <PrimaryButton text="Previous" onClick={onPreviousButtonClick} disabled={isPreviousButtonDisabled()} />
              <PrimaryButton text={getDetectButtonText()} onClick={onDetectButtonClick} disabled={isDetectButtonDisabled()} />
              <PrimaryButton text="Next" onClick={onNextButtonClick} disabled={isNextButtonDisabled()} />
            </Stack>
          </div>
        </Stack>
      </StepPanel>
      <PopupModal isOpen={isAutoDetectionWarningDialogOpen} header={'Warning'} onClose={onCloseAutoDetectionWarningDialogClick} text={autoDetection.detectionSteps?.Result.Exception} />
      <PopupModal isOpen={isAutoDetectionLogDialogOpen} header={'Log'} onClose={hideAutoDetectionLogDialog} text={autoDetectionLog} />
    </div >
  )
};
