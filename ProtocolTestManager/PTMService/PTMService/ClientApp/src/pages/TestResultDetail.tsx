// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Checkbox, DetailsList, DetailsListLayoutMode, Dialog, DialogFooter, DialogType, Dropdown, IContextualMenuProps, IDropdownOption, IGroup, IObjectWithKey, Label, MarqueeSelection, MessageBar, MessageBarType, PrimaryButton, SearchBox, Selection, SelectionMode, SelectionZone, Spinner, SpinnerSize, Stack } from '@fluentui/react'
import { useBoolean, useConst, useForceUpdate } from '@uifabric/react-hooks'
import { useCallback, useEffect, useMemo, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useHistory } from 'react-router-dom'
import { TestCaseResultActions } from '../actions/TestCaseResultAction'
import { TestResultsActions } from '../actions/TestResultsAction'
import { FullWidthPanel } from '../components/FullWidthPanel'
import { StackGap10, StackGap5 } from '../components/StackStyle'
import { useWindowSize } from '../components/UseWindowSize'
import { TestCaseOverview, TestCaseResult, TestCaseState } from '../model/TestCaseResult'
import { ReportFormat, ReportRequest, TestResult } from '../model/TestResult'
import { SelectedTestCasesDataSrv } from '../services/SelectedTestCases'
import { TestCaseResultDataSrv } from '../services/TestCaseResult'
import { TestResultsDataSrv } from '../services/TestResults'
import { ProfileDataSrv } from '../services/ProfileService'
import { AppState } from '../store/configureStore'
import { ContextualMenuControl, ContextualMenuItemProps } from '../components/ContextualMenuControl'

interface GroupKeys {
  KeyName: string
  Keys: Array<{ key: string, name: string }>
  KeyComparer: (result: TestCaseOverview, key: string) => boolean
}

const groupByTestCaseStateKeys: GroupKeys = {
  KeyName: 'State',
  Keys: [
    {
      key: 'Passed',
      name: 'Passed'
    },
    {
      key: 'Failed',
      name: 'Failed'
    },
    {
      key: 'Inconclusive',
      name: 'Inconclusive'
    },
    {
      key: 'Running',
      name: 'Running'
    },
    {
      key: 'NotRun',
      name: 'Not Run'
    }
  ],
  KeyComparer: (result, key) => result.State === key
}

const isValidFilterPhrase = (filterPhrase: string | undefined) => {
  return filterPhrase !== undefined && filterPhrase !== null && filterPhrase !== ''
}

const filterByNameFunc = (filterPhrase: string | undefined) => (item: TestCaseOverview) => {
  return isValidFilterPhrase(filterPhrase)
    ? item.FullName.toLocaleLowerCase().includes(filterPhrase!.toLocaleLowerCase())
    : true
}

type SelectionItem = TestCaseOverview & IObjectWithKey

const getSelectionItems = (testCaseResults: TestCaseOverview[]) => {
  return testCaseResults.map((result: TestCaseOverview) => {
    return {
      ...result,
      key: result.FullName
    }
  })
}

const hasFailedCases = (results: TestCaseOverview[]) => {
  return results.length === 0
    ? false
    : results.some(item => item.State === 'Failed')
}

interface TestCaseResultViewProps {
  winSize: { width: number, height: number },
  result: TestCaseResult | undefined
}

const lineHeader = /\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3} \[(?<kind>\w+)\]/g

const getLineBackgroundColor = (kind: string) => {
  if (kind.includes('Failed')) {
    return '#FF9E9E'
  } else if (kind.includes('Inconclusive')) {
    return '#FFFF00'
  } else if (kind.includes('Succeeded')) {
    return '#61FF61'
  }

  return 'transparent'
}

const renderOutputLines = (lines: string[]) => {
  return lines.reduce((res: JSX.Element[], currentLine) => {
    lineHeader.lastIndex = 0
    const matches = lineHeader.exec(currentLine)
    if (matches !== null && matches.groups !== undefined) {
      return [...res, <p key={res.length} style={{ overflowWrap: 'normal', backgroundColor: getLineBackgroundColor(matches.groups.kind) }}>{currentLine}</p>]
    } else {
      return [...res, <p key={res.length} style={{ overflowWrap: 'normal' }}>&nbsp;&nbsp;&nbsp;&nbsp;{currentLine}</p>]
    }
  }, [])
}

function TestCaseResultView (props: TestCaseResultViewProps) {
  return (
    props.result !== undefined
      ? <Stack tokens={StackGap10}>
        <Label style={{ fontWeight: 'bold', color: '#337ab7', alignSelf: 'center' }}>{props.result.Overview.FullName}</Label>
        <Label>Start Time: {props.result.StartTime}</Label>
        <Label>End Time: {props.result.EndTime}</Label>
        <Label>State: {props.result.Overview.State}</Label>
        {
          props.result.Output !== null
            ? <div style={{ border: '1px solid #d9d9d9', height: props.winSize.height - 300, overflowY: 'auto' }}>
              <div style={{ paddingLeft: 5 }}>
                {renderOutputLines(props.result.Output.split('\n').map((line) => line.trim()))}
              </div>
            </div>
            : null
        }
      </Stack >
      : <div>Please select a test case result to view...</div>
  )
}

interface SelectedTestCasesViewProps {
  winSize: { width: number, height: number },
  results: TestCaseOverview[]
}

function SelectedTestCasesView (props: SelectedTestCasesViewProps) {
  return (
    <div>
      <Label style={{ fontWeight: 'bold', color: '#337ab7', fontSize: 'large' }}>Selected {props.results.length} Test Case Results:</Label>
      <div style={{ border: '1px solid #d9d9d9', height: props.winSize.height - 180, overflowY: 'auto' }}>
        <ul>
          {
            props.results.sort((a, b) => a.FullName.localeCompare(b.FullName))
              .map((item, index) => {
                return <li key={index}>{item.FullName}</li>
              })
          }
        </ul>
      </div>
    </div>
  )
}

interface RerunContextualMenuProps {
  testResult: TestResult
  selectedItems: TestCaseOverview[] | undefined
  onRerun: (kind: 'All' | 'Failed' | 'Selected') => () => void
}

function RerunContextualMenu (props: RerunContextualMenuProps) {
  const menuProps = useMemo<IContextualMenuProps>(() => ({
    shouldFocusOnMount: true,
    items: [
      {
        key: 'RerunAll',
        text: 'Rerun All Cases',
        disabled: props.testResult.Results.length === 0,
        onClick: props.onRerun('All')
      },
      {
        key: 'RerunFailed',
        text: 'Rerun Failed Cases',
        disabled: !hasFailedCases(props.testResult.Results),
        onClick: props.onRerun('Failed')
      },
      {
        key: 'RerunSelected',
        text: 'Rerun Selected Cases',
        disabled: props.selectedItems === undefined || props.selectedItems.length === 0,
        onClick: props.onRerun('Selected')
      }
    ]
  }), [props])

  return <PrimaryButton menuProps={menuProps}>Rerun</PrimaryButton>
}

export function TestResultDetail (props: any) {
  const winSize = useWindowSize()

  const history = useHistory()

  const dispatch = useDispatch()

  const [abortDialogHidden, { toggle: toggleAbortDialogHidden }] = useBoolean(true)
  const [reportDialogHidden, { toggle: toggleReportDialogHidden }] = useBoolean(true)

  const reportOutcomes = useConst<TestCaseState[]>(['Passed', 'Failed', 'Inconclusive'])
  const reportFormatOptions = useConst<IDropdownOption[]>(() => {
    const reportFormats: ReportFormat[] = ['Plain', 'Json', 'XUnit']
    return reportFormats.map(format => { return { key: format, text: format } })
  })

  const [filterPhrase, setFilterPhrase] = useState<string | undefined>(undefined)
  const [filteredResults, setFilteredResults] = useState<SelectionItem[] | undefined>(undefined)
  const [reportedStates, setReportedStates] = useState<TestCaseState[]>([])
  const [reportFormat, setReportFormat] = useState<ReportFormat>('Plain')
  const [reportDialogShowMsg, setReportDialogShowMsg] = useState(true)

  const [groupKeys, setGroupKeys] = useState(groupByTestCaseStateKeys)

  const [groupCollapsedStatuses, setGroupCollapsedStatuses] = useState(() => {
    const statuses: { [key: string]: boolean } = {}
    groupKeys.Keys.forEach((item) => {
      statuses[item.key] = false
    })

    return statuses
  })

  const [selectedItems, setSelectedItems] = useState<TestCaseOverview[] | undefined>(undefined)

  const testResults = useSelector((state: AppState) => state.testResults)
  const testCaseResult = useSelector((state: AppState) => state.testCaseResult)

  const testCaseResults = useMemo(() => testResults.selectedTestResult?.Results ?? [], [testResults.selectedTestResult])

  const forceUpdate = useForceUpdate()

  useEffect(() => {
    if (testResults.selectedTestResultId !== undefined) {
      dispatch(TestResultsDataSrv.getTestResultDetail(testResults.selectedTestResultId))
    }
  }, [dispatch, testResults.selectedTestResultId])

  useEffect(() => {
    setReportDialogShowMsg(testResults.errorMsg !== undefined)
  }, [testResults.errorMsg])

  useEffect(() => {
    if (testResults.selectedTestResult !== undefined) {
      const newStates = reportOutcomes.filter(state => testResults.selectedTestResult?.Results.some(res => res.State === state))
      setReportedStates(newStates)
    }
  }, [testResults.selectedTestResult, reportOutcomes])

  useEffect(() => {
    const handler = () => {
      const selectedTestResult = testResults.selectedTestResult
      const updateStatus = selectedTestResult !== undefined && (selectedTestResult?.Overview.Status === 'Created' || selectedTestResult?.Overview.Status === 'Running')
      if (testResults.selectedTestResultId !== undefined) {
        if (updateStatus) {
          dispatch(TestResultsDataSrv.getTestResultDetail(testResults.selectedTestResultId))
        }
      }
    }
    const interval = setInterval(handler, 5000)

    return () => clearInterval(interval)
  }, [dispatch, testResults.selectedTestResultId, testResults.selectedTestResult])

  useEffect(() => {
    if (selectedItems !== undefined && selectedItems?.length === 1) {
      dispatch(TestCaseResultDataSrv.getTestCaseResult(selectedItems[0].FullName))
    }
  }, [dispatch, selectedItems])

  const getUpdatedGroups = useCallback((resultList: TestCaseOverview[]) => {
    const updatedResult = groupKeys.Keys.reduce((result: { groups: IGroup[], startIndex: number }, currentKey) => {
      const caseCount = resultList.filter(item => groupKeys.KeyComparer(item, currentKey.key)).length
      if (caseCount === 0) {
        return result
      } else {
        return {
          groups: [...result.groups, {
            key: currentKey.key,
            name: currentKey.name,
            startIndex: result.startIndex,
            count: caseCount,
            isCollapsed: Object.getOwnPropertyDescriptor(groupCollapsedStatuses, currentKey.key)?.value
          }],
          startIndex: result.startIndex + caseCount
        }
      }
    }, { groups: [], startIndex: 0 })

    return updatedResult.groups
  }, [groupKeys, groupCollapsedStatuses])

  const getUpdatedGroupedItems = useCallback((resultList: TestCaseOverview[]) => {
    const updatedList = groupKeys.Keys.flatMap((key) => {
      return resultList.filter(item => groupKeys.KeyComparer(item, key.key))
        .sort((a, b) => a.FullName.localeCompare(b.FullName))
    })

    return updatedList
  }, [groupKeys])

  const updateFilteredResults = useCallback((filterPhrase: string | undefined) => {
    const groupedResults = isValidFilterPhrase(filterPhrase)
      ? getUpdatedGroupedItems(testCaseResults.filter(filterByNameFunc(filterPhrase)))
      : getUpdatedGroupedItems(testCaseResults)

    setFilteredResults(groupedResults)
  }, [testCaseResults, getUpdatedGroupedItems])

  useEffect(() => {
    if (testResults.selectedTestResult !== undefined) {
      updateFilteredResults(filterPhrase)
    }
  }, [testResults.selectedTestResult, filterPhrase, updateFilteredResults])

  const [selection, setSelection] = useState<Selection<IObjectWithKey>>(() => {
    const s = new Selection<SelectionItem>({
      onSelectionChanged: () => {
        setSelectedItems(s.getSelection())
        setSelection(s)
        forceUpdate()
      }
    })

    s.setItems(getSelectionItems(testCaseResults), true)

    return s
  })

  const onFilterPhraseChanged = (filterPhrase: string | undefined) => {
    setFilterPhrase(filterPhrase)
    selection.setAllSelected(false)
  }

  const onFilterPhraseClear = () => {
    setFilterPhrase(undefined)
    selection.setAllSelected(false)
  }

  const onToggleGroupCollapse = (group: IGroup) => {
    groupCollapsedStatuses[group.key] = !group.isCollapsed
    setGroupCollapsedStatuses({ ...groupCollapsedStatuses })
  }

  const onRerunClick = (kind: 'All' | 'Failed' | 'Selected') => () => {
    if (testResults.selectedTestResult?.Results === undefined) {
      return
    }

    const completeCallback = () => {
      history.push('/Tasks/History', { from: 'TestResultDetail' })
    }

    const results = testResults.selectedTestResult.Results
    const confId = testResults.selectedTestResult.Overview.ConfigurationId
    switch (kind) {
      case 'All': {
        dispatch(SelectedTestCasesDataSrv.createRunRequest(results.map(item => item.FullName), confId, completeCallback))
        return
      }

      case 'Failed': {
        dispatch(SelectedTestCasesDataSrv.createRunRequest(results.filter(item => item.State === 'Failed').map(item => item.FullName), confId, completeCallback))
        return
      }

      case 'Selected': {
        if (selectedItems === undefined || selectedItems.length === 0) {
          return
        }

        dispatch(SelectedTestCasesDataSrv.createRunRequest(selectedItems.map(item => item.FullName), confId, completeCallback))
      }
    }
  }

  const onExportProfile = (kind: 'All' | 'Selected') => () => {
    if (testResults.selectedTestResult?.Results === undefined) {
      return
    }

    switch (kind) {
      case 'All': {
        dispatch(ProfileDataSrv.saveProfile(testResults.selectedTestResult.Overview.Id))
        return
      }

      case 'Selected': {
        if (selectedItems === undefined || selectedItems.length === 0) {
          return
        }

        dispatch(ProfileDataSrv.saveProfile(testResults.selectedTestResult.Overview.Id, selectedItems.map(item => item.FullName)))
      }
    }
  }

  const getExportProfileMenuItems = (): ContextualMenuItemProps[] => {
    return [
      {
        key: 'ExportAll',
        text: 'Export All Test Cases to Profile',
        disabled: testCaseResults.length === 0,
        menuAction: onExportProfile('All')
      },
      {
        key: 'ExportSelected',
        text: 'Export Selected Test Cases to Profile',
        disabled: selectedItems === undefined || selectedItems.length === 0,
        menuAction: onExportProfile('Selected')
      }
    ]
  }

  const onGoBackClick = () => {
    dispatch(TestCaseResultActions.clearSelectedTestCaseResultAction())
    dispatch(TestResultsActions.clearSelectedTestResultAction())
    dispatch(() => history.push('/Tasks/History', { from: 'TestResultDetail' }))
  }

  const onAbortDialogAbortButtonClick = () => {
    if (testResults.selectedTestResultId !== undefined) {
      dispatch(SelectedTestCasesDataSrv.abortRunRequest(testResults.selectedTestResultId))
    }

    dispatch(() => toggleAbortDialogHidden())
  }

  const getReportedTestCases = useCallback(() => {
    if (testResults.selectedTestResult !== undefined) {
      return reportedStates.flatMap(state => testResults.selectedTestResult!.Results.filter(res => res.State === state).map(res => res.FullName))
    } else {
      return []
    }
  }, [testResults.selectedTestResult, reportedStates])

  const onReportDialogCheckboxChange = useCallback((state: TestCaseState, checked: boolean) => {
    if (reportedStates.includes(state)) {
      if (!checked) {
        const newStates = reportedStates.filter(s => s !== state)
        setReportedStates(newStates)
      }
    } else {
      if (checked) {
        setReportedStates([...reportedStates, state])
      }
    }
  }, [reportedStates])

  const onReportDialogExportButtonClick = () => {
    if (testResults.selectedTestResultId !== undefined) {
      const reportRequest: ReportRequest = {
        TestCases: getReportedTestCases(),
        Format: reportFormat
      }

      dispatch(TestResultsDataSrv.getTestRunReport(testResults.selectedTestResultId, reportRequest))
    }
  }

  return (
    <div>
      <FullWidthPanel isLoading={testCaseResult.isLoading} errorMsg={testCaseResult.errorMsg} >
        <Stack horizontal tokens={{ childrenGap: 20 }}>
          <div style={{ maxWidth: winSize.width * 0.30, height: winSize.height - 140 }}>
            <SelectionZone selection={selection} selectionMode={SelectionMode.multiple}>
              <Stack tokens={StackGap10}>
                <SearchBox
                  style={{ width: 280 }}
                  placeholder='Filter by Name...'
                  onChange={(_, newValue) => onFilterPhraseChanged(newValue)}
                  onReset={onFilterPhraseClear}
                />
                <div style={{ width: winSize.width * 0.30, height: winSize.height - 180, overflowY: 'auto' }}>
                  {
                    filteredResults !== undefined
                      ? <MarqueeSelection selection={selection}>
                        <DetailsList
                          items={filteredResults}
                          columns={[
                            {
                              key: 'Name',
                              name: 'Name',
                              minWidth: 360,
                              isRowHeader: true,
                              isResizable: true,
                              onRender: (item: TestCaseOverview | undefined, index: number | undefined) => {
                                if (item !== undefined && index !== undefined) {
                                  const startIndex = item.FullName.lastIndexOf('.') + 1
                                  const caseName = item.FullName.substring(startIndex)
                                  return <div>{caseName}</div>
                                } else {
                                  return null
                                }
                              }
                            }
                          ]}
                          layoutMode={DetailsListLayoutMode.justified}
                          selection={selection}
                          selectionMode={SelectionMode.multiple}
                          selectionPreservedOnEmptyClick={true}
                          setKey='set'
                          getKey={(item: TestCaseOverview) => item.FullName}
                          groups={getUpdatedGroups(filteredResults)}
                          groupProps={{
                            headerProps: {
                              onToggleCollapse: onToggleGroupCollapse
                            }
                          }}
                          compact={true}
                          isHeaderVisible={false}
                        />
                      </MarqueeSelection>
                      : testResults.selectedTestResultId !== undefined
                        ? <div>
                          <Spinner size={SpinnerSize.medium} />
                          <p style={{ color: '#ab5f0e' }}>Loading...</p>
                        </div>
                        : <p>Please select a test result on the Task History page.</p>
                  }
                </div>
              </Stack>
            </SelectionZone>
          </div>
          <div style={{ borderLeft: '2px solid #bae7ff', minHeight: winSize.height - 140 }}>
            <div style={{ paddingLeft: 20, width: winSize.width * 0.66 }}>
              {
                selectedItems !== undefined && selectedItems.length > 1
                  ? <SelectedTestCasesView
                    winSize={winSize}
                    results={selectedItems} />
                  : <TestCaseResultView
                    winSize={winSize}
                    result={testCaseResult.selectedTestCaseResult} />
              }
            </div>
          </div>
        </Stack>
        <hr style={{ border: '1px solid #d9d9d9' }} />
        <div className='buttonPanel'>
          <Stack horizontal horizontalAlign='end' tokens={StackGap10}>
            {
              testResults.selectedTestResult === undefined
                ? null
                : testResults.selectedTestResult.Overview.Status === 'Running'
                  ? <PrimaryButton style={{ backgroundColor: '#ce3939' }} onClick={toggleAbortDialogHidden}>Abort</PrimaryButton>
                  : null
            }
            {
              testResults.selectedTestResultSummary !== undefined && !testResults.selectedTestResultSummary.TestSuite.Removed
                ? <Stack horizontal horizontalAlign='end' tokens={StackGap10}>
                  {
                    testResults.selectedTestResult === undefined
                      ? null
                      : testResults.selectedTestResult.Overview.Status !== 'Created' && testResults.selectedTestResult.Overview.Status !== 'Running'
                        ? <RerunContextualMenu
                          testResult={testResults.selectedTestResult}
                          selectedItems={selectedItems}
                          onRerun={onRerunClick} />
                        : null
                  }
                  {
                    testResults.selectedTestResult === undefined
                      ? null
                      : testResults.selectedTestResult.Overview.Status === 'Failed' || testResults.selectedTestResult.Overview.Status === 'Finished'
                        ? <PrimaryButton disabled={testResults.isDownloading} onClick={toggleReportDialogHidden}>Export Report</PrimaryButton>
                        : null
                  }
                  {
                    testResults.selectedTestResult === undefined
                      ? null
                      : <ContextualMenuControl text="Export Profile" shouldFocusOnMount={true} menuItems={getExportProfileMenuItems()} />
                  }
                </Stack>
                : null
            }
            <PrimaryButton onClick={onGoBackClick}>Go Back</PrimaryButton>
          </Stack>
        </div>
      </FullWidthPanel>
      <Dialog
        hidden={abortDialogHidden}
        onDismiss={toggleAbortDialogHidden}
        dialogContentProps={{
          type: DialogType.normal,
          title: `Abort Test Run ${testResults.selectedTestResultId}?`
        }}
        modalProps={{
          isBlocking: true
        }}>
        <div style={{ fontSize: 'large' }}>
          <Stack horizontalAlign='start' tokens={StackGap5}>
            <Stack horizontal><div style={{ paddingRight: 5 }}>TestSuite:</div>{testResults.selectedTestResultSummary?.TestSuite.Name + ' ' + testResults.selectedTestResultSummary?.TestSuite.Version}</Stack>
            <Stack horizontal><div style={{ paddingRight: 5 }}>Configuration:</div>{testResults.selectedTestResultSummary?.Configuration}</Stack>
            <Stack horizontal><div style={{ paddingRight: 5 }}>Status:</div><div>{testResults.selectedTestResult?.Overview.Status}</div></Stack>
          </Stack>
        </div>
        <DialogFooter>
          <PrimaryButton onClick={toggleAbortDialogHidden} text="Cancel" />
          <PrimaryButton style={{ backgroundColor: '#ce3939' }} onClick={onAbortDialogAbortButtonClick} text="Abort" />
        </DialogFooter>
      </Dialog>
      <Dialog
        hidden={reportDialogHidden}
        onDismiss={toggleReportDialogHidden}
        dialogContentProps={{
          type: DialogType.normal,
          title: `Export Report for Test Run ${testResults.selectedTestResultId}?`
        }}
        modalProps={{
          isBlocking: true
        }}>
        <div style={{ fontSize: 'large' }}>
          <Stack horizontalAlign='start' tokens={StackGap10}>
            {
              !reportDialogShowMsg
                ? null
                : <MessageBar
                  messageBarType={MessageBarType.error}
                  onDismiss={() => setReportDialogShowMsg(false)}>
                  {testResults.errorMsg}
                </MessageBar>
            }
            <Label>Outcome</Label>
            {
              testResults.selectedTestResult !== undefined
                ? reportOutcomes.map(state => {
                  const isCheckboxEnabled = testResults.selectedTestResult?.Results.some(res => res.State === state)
                  return <Checkbox
                    key={state}
                    disabled={!isCheckboxEnabled}
                    label={`${state} Test Cases`}
                    checked={isCheckboxEnabled && reportedStates.includes(state)}
                    onChange={(_, checked) => onReportDialogCheckboxChange(state, checked!)}
                  />
                })
                : null
            }
            <Stack horizontal tokens={StackGap10}>
              <Label>Format</Label>
              <Dropdown
                style={{ alignSelf: 'center', minWidth: 180 }}
                ariaLabel='Report format'
                placeholder='Select a report format'
                selectedKey={reportFormat}
                options={reportFormatOptions}
                onChange={(_, newValue, __) => setReportFormat(newValue?.text! as ReportFormat)}
              />
            </Stack>
          </Stack>
        </div>
        <DialogFooter>
          <PrimaryButton disabled={reportedStates.length === 0 || testResults.isDownloading} onClick={onReportDialogExportButtonClick} text="Export" />
          <PrimaryButton disabled={testResults.isDownloading} onClick={toggleReportDialogHidden} text="Cancel" />
        </DialogFooter>
      </Dialog>
    </div>
  )
}
