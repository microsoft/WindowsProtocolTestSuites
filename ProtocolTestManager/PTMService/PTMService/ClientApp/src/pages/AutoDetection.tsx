// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  Link,
  DetailsList,
  IColumn,
  Label,
  PrimaryButton,
  SelectionMode,
  Stack,
  Spinner,
  SpinnerSize
} from '@fluentui/react'
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard'
import { PopupModal } from '../components/PopupModal'
import { StepPanel } from '../components/StepPanel'
import { LogPanel } from '../components/LogPanel'
import { HeaderMenuHeight, WizardNavBar } from '../components/WizardNavBar'
import { getNavSteps } from '../model/DefaultNavSteps'
import { AppState } from '../store/configureStore'
import { useDispatch, useSelector } from 'react-redux'
import { useWindowSize } from '../components/UseWindowSize'
import { LoadingPanel } from '../components/LoadingPanel'
import { Property } from '../model/Property'
import { CSSProperties, ReactElement, useCallback, useEffect, useLayoutEffect, useMemo, useRef, useState } from 'react'
import { AutoDetectionDataSrv } from '../services/AutoDetection'
import { AutoDetectionActions } from '../actions/AutoDetectionAction'
import { WizardNavBarActions } from '../actions/WizardNavBarAction'
import { DetectingItem, DetectionStatus, DetectionStepStatus } from '../model/AutoDetectionData'
import { useBoolean } from '@uifabric/react-hooks'
import { PropertyGroupView } from '../components/PropertyGroupView'
import { PropertyGroup } from '../model/PropertyGroup'
import { PropertyGroupsActions } from '../actions/PropertyGroupsAction'
import { AutoDetectionState } from '../reducers/AutoDetectionReducer'
import { InvalidAppStateNotification } from '../components/InvalidAppStateNotification'
import { DetectionResultActions } from '../actions/DetectionResultAction'

const getStyle = (status: DetectionStepStatus): CSSProperties => {
  if (status === 'Failed' || status === 'Cancelled') {
    return { paddingLeft: 5, color: '#a30000' }
  } else if (status === 'Finished') {
    return { paddingLeft: 5, color: '#006100' }
  } else if (status === 'Detecting' || status === 'Canceling') {
    return { paddingLeft: 5, color: '#0000ff' }
  } else {
    return { paddingLeft: 5 }
  }
}

const renderDetectingContent = (item: Property): ReactElement => {
  return (
        <Label>
            <div style={{ paddingLeft: 5 }} >{item.Name}</div>
        </Label>
  )
}

export function AutoDetection (props: StepWizardProps) {
  const wizardProps: StepWizardChildProps = props as StepWizardChildProps

  const [isAutoDetectionWarningDialogOpen, { setTrue: showAutoDetectionWarningDialog, setFalse: hideAutoDetectionWarningDialog }] = useBoolean(false)
  const testSuiteInfo = useSelector((state: AppState) => state.testSuiteInfo)
  const configuration = useSelector((state: AppState) => state.configurations)
  const autoDetection = useSelector((state: AppState) => state.autoDetection)
  const autoDetectionLog = useMemo(() => autoDetection.log ?? '', [autoDetection.log])
  const prerequisitePropertyGroup = useMemo<PropertyGroup>(() => { return { Name: 'Prerequisite Properties', Items: autoDetection.prerequisite?.Properties ?? [] } }, [autoDetection])
  const navSteps = getNavSteps(wizardProps)
  const wizard = WizardNavBar(wizardProps, navSteps)
  const dispatch = useDispatch()

  const winSize = useWindowSize()
  const canDockLogPanel = winSize.width >= 1400
  const [headerHeight, setHeaderHeight] = useState<number>(0)
  const [prerequisitePropertiesHeight, setPrerequisitePropertiesHeight] = useState<number>(0)
  const [isLogPanelOpen, setIsLogPanelOpen] = useState<boolean>(canDockLogPanel)

  useEffect(() => {
    dispatch(AutoDetectionDataSrv.getAutoDetectionPrerequisite())
  }, [dispatch])

  if (testSuiteInfo.selectedTestSuite === undefined || configuration.selectedConfiguration === undefined) {
    return <InvalidAppStateNotification
            testSuite={testSuiteInfo.selectedTestSuite}
            configuration={configuration.selectedConfiguration}
            wizard={wizard}
            wizardProps={wizardProps} />
  }

  const autoDetectionStepsUpdateCallback = useCallback((currAutoDetection: AutoDetectionState) => {
    if (currAutoDetection.detectionSteps?.Result.Status !== DetectionStatus.InProgress) {
      if (currAutoDetection.detectionSteps?.Result.Status === DetectionStatus.Error) {
        if (currAutoDetection.detectionSteps.Result.Exception !== null && currAutoDetection.detectionSteps.Result.Exception !== '') {
          showAutoDetectionWarningDialog()
        }
      }
    }
  }, [showAutoDetectionWarningDialog])

  useEffect(() => {
    dispatch(AutoDetectionDataSrv.getAutoDetectionSteps(autoDetectionStepsUpdateCallback))
  }, [dispatch, autoDetectionStepsUpdateCallback])

  useEffect(() => {
    setIsLogPanelOpen(canDockLogPanel)
  }, [canDockLogPanel])

  useEffect(() => {
    if (!autoDetection.detecting && autoDetection.isLogStreamComplete) {
      return
    }

    let cancelled = false
    let timer: ReturnType<typeof setTimeout> | undefined

    const pollAutoDetection = async () => {
      if (autoDetection.detecting) {
        dispatch(AutoDetectionDataSrv.updateAutoDetectionSteps(autoDetectionStepsUpdateCallback))
      }
      if (!cancelled) {
        dispatch(AutoDetectionDataSrv.getAutoDetectionLogChunk(() => { }))
      }
      if (!cancelled) {
        timer = setTimeout(pollAutoDetection, 1000)
      }
    }

    void pollAutoDetection()
    return () => {
      cancelled = true
      if (timer !== undefined) {
        clearTimeout(timer)
      }
    }
  }, [dispatch, autoDetection.detecting, autoDetection.isLogStreamComplete, autoDetectionStepsUpdateCallback])

  useLayoutEffect(() => {
    if (autoDetection.prerequisite?.Summary !== undefined) {
      const lineCount = autoDetection.prerequisite.Summary.split('\n').length
      const summaryHeight = 43 * lineCount

      setHeaderHeight(HeaderMenuHeight + summaryHeight)
    }
  }, [autoDetection])

  useLayoutEffect(() => {
    if (autoDetection.prerequisite?.Properties !== undefined) {
      const initialHeight = (winSize.height - headerHeight) * 0.45
      const calculatedHeight = 48 * autoDetection.prerequisite.Properties.length

      setPrerequisitePropertiesHeight(Math.min(initialHeight, calculatedHeight))
    }
  }, [autoDetection])

  const onPreviousButtonClick = () => {
    wizardProps.previousStep()
  }

  const onPropertyValueChange = (updatedProperty: Property) => {
    dispatch(AutoDetectionActions.updateAutoDetectionPrerequisiteAction(updatedProperty))
  }

  const onNextButtonClick = () => {
    if (autoDetection.detectionSteps?.Result.Status === DetectionStatus.Finished) {
      dispatch(PropertyGroupsActions.setUpdatedAction(false))
      dispatch(AutoDetectionDataSrv.applyDetectionResult(() => {
        // Next page
        wizardProps.nextStep()
      }))
    }
  }

  const onDetectButtonClick = () => {
    dispatch(WizardNavBarActions.setWizardNavBarAction(wizardProps.currentStep))
    dispatch(DetectionResultActions.resetDetectionResultAction())
    if (autoDetection.detecting) {
      // Cancel
      dispatch(AutoDetectionDataSrv.stopAutoDetection())
    } else {
      if (!canDockLogPanel) {
        setIsLogPanelOpen(true)
      }
      // Start detection
      dispatch(AutoDetectionDataSrv.startAutoDetection(() => {
        dispatch(AutoDetectionDataSrv.updateAutoDetectionSteps(autoDetectionStepsUpdateCallback))
      }))
    }
  }

  const isNextButtonDisabled = (): boolean => autoDetection.detecting || autoDetection.detectionSteps?.Result.Status !== DetectionStatus.Finished

  const isDetectButtonDisabled = (): boolean => autoDetection.canceling

  const getDetectButtonText = (): string => autoDetection.detecting ? 'Cancel' : 'Detect'

  const onToggleLogPanelClick = () => {
    setIsLogPanelOpen(isOpen => !isOpen)
  }

  const onCloseAutoDetectionWarningDialogClick = () => {
    hideAutoDetectionWarningDialog()
  }

  const renderDetectingStatus = useCallback((item: DetectingItem): ReactElement => {
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
                                        onClick={() => setIsLogPanelOpen(true)}>
                                        {status}
                                    </Link>
                            )
                          } else if (status === 'Finished') {
                            return (
                                    <Link
                                        underline
                                        style={getStyle(status)}
                                        onClick={() => setIsLogPanelOpen(true)}>
                                        {status}
                                    </Link>
                            )
                          } else if ((status === 'Pending' || status === 'Detecting') && autoDetection.detecting) {
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
  }, [autoDetection.detecting])

  const stepColumns = useMemo<IColumn[]>(() => [{
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
  }], [renderDetectingStatus])

  const showDockedLogPanel = canDockLogPanel && isLogPanelOpen

  return (
        <div>
            <StepPanel leftNav={wizard} isLoading={autoDetection.isPrerequisiteLoading || autoDetection.isDetectionStepsLoading} errorMsg={autoDetection.errorMsg} >
                <Stack horizontal={showDockedLogPanel} tokens={{ childrenGap: 16 }} style={{ height: showDockedLogPanel ? winSize.height - HeaderMenuHeight : undefined, paddingLeft: 10, paddingRight: 20 }}>
                    <Stack style={{ height: showDockedLogPanel ? '100%' : undefined, minWidth: 0, flexGrow: 1 }}>
                        <Stack style={{ paddingLeft: 30 }}>
                            {autoDetection.prerequisite?.Summary.split('\n').map((line) => <p key={line}>{line.trim()}</p>)}
                        </Stack>
                        <div style={{ paddingLeft: 30, height: prerequisitePropertiesHeight, overflowY: 'auto' }}>
                            {
                                autoDetection.isPrerequisiteLoading
                                  ? <LoadingPanel />
                                  : <PropertyGroupView
                                        winSize={{ ...winSize, height: prerequisitePropertiesHeight }}
                                        propertyGroup={prerequisitePropertyGroup}
                                        onValueChange={onPropertyValueChange} />
                            }
                        </div>
                        <div style={{ paddingTop: 10, paddingLeft: 30, height: winSize.height - (headerHeight + prerequisitePropertiesHeight) - 55, overflowY: 'auto' }}>
                            {
                                autoDetection.isDetectionStepsLoading
                                  ? <LoadingPanel />
                                  : <div>
                                        <Stack horizontal style={{ paddingTop: 20 }} horizontalAlign='start' tokens={{ childrenGap: 10 }}>
                                            <div style={{ borderLeft: '2px solid #bae7ff', minHeight: 200 }}>
                                                <DetailsList
                                                    columns={stepColumns}
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
                        <div className='buttonPanel'>
                            <Stack horizontal horizontalAlign="end" tokens={{ childrenGap: 10 }} >
                                {!canDockLogPanel && (
                                    <PrimaryButton
                                        text={isLogPanelOpen ? 'Hide Log' : 'Show Log'}
                                        onClick={onToggleLogPanelClick}
                                        disabled={autoDetection.detectionSteps?.Result.Status === DetectionStatus.NotStart && !isLogPanelOpen}
                                    />
                                )}
                                <PrimaryButton text={getDetectButtonText()} onClick={onDetectButtonClick} disabled={isDetectButtonDisabled()} />
                                <PrimaryButton text="Previous" onClick={onPreviousButtonClick} />
                                <PrimaryButton text="Next" onClick={onNextButtonClick} disabled={isNextButtonDisabled()} />
                            </Stack>
                        </div>
                    </Stack>
                    {
                        isLogPanelOpen
                          ? <LogPanel
                                log={autoDetectionLog}
                                canDock={canDockLogPanel}
                                isOpen={isLogPanelOpen}
                                onToggle={onToggleLogPanelClick}
                                windowHeight={winSize.height}
                                headerHeight={headerHeight}
                                isDetecting={autoDetection.detecting}
                            />
                          : undefined
                    }
                </Stack>
            </StepPanel>
            <PopupModal isOpen={isAutoDetectionWarningDialogOpen} header={'Warning'} onClose={onCloseAutoDetectionWarningDialogClick} text={autoDetection.detectionSteps?.Result.Exception} />
        </div >
  )
};
