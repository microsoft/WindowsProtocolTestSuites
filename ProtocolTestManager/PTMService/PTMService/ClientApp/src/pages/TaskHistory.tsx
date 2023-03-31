// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { DetailsList, DetailsListLayoutMode, Dialog, DialogFooter, DialogType, IColumn, PrimaryButton, SearchBox, SelectionMode, Spinner, SpinnerSize, Stack, Toggle } from '@fluentui/react'
import { Pagination } from '@uifabric/experiments'
import { useBoolean } from '@uifabric/react-hooks'
import { useCallback, useEffect, useMemo, useRef, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useHistory } from 'react-router-dom'
import { TestResultsActions } from '../actions/TestResultsAction'
import { FullWidthPanel } from '../components/FullWidthPanel'
import { StackGap10, StackGap5 } from '../components/StackStyle'
import { useWindowSize } from '../components/UseWindowSize'
import { TestCaseState } from '../model/TestCaseResult'
import { TestResultOverview, TestResultState } from '../model/TestResult'
import { TestSuite } from '../model/TestSuite'
import { ConfigurationsDataSrv } from '../services/Configurations'
import { SelectedTestCasesDataSrv } from '../services/SelectedTestCases'
import { TestResultsDataSrv } from '../services/TestResults'
import { TestSuitesDataSrv } from '../services/TestSuites'
import { ProfileDataSrv } from '../services/ProfileService'
import { AppState } from '../store/configureStore'

type NumberValueKeysOf<T> = { [P in keyof T]-?: T[P] extends number ? P : never }[keyof T]

function getDict<TItem extends Pick<TItem, NumberValueKeysOf<TItem>>>(items: TItem[], keyName: NumberValueKeysOf<TItem>) {
  return items.reduce((dict: { [key: number]: TItem }, item) => {
    const keyValue: number = item[keyName]
    dict[keyValue] = item
    return dict
  }, {})
}

interface ListColumnsProps {
  onRenderId: (testResultId: number) => JSX.Element
  onRenderTestSuite: (configurationId: number) => JSX.Element
  isTestSuiteRemoved: (configurationId: number) => boolean
  onRenderConfiguration: (configurationId: number) => JSX.Element
  onRenderStatus: (status: TestResultState) => JSX.Element
  onRenderCount: (kind: TestCaseState | 'Total', count: number | undefined) => JSX.Element,
  onAbort: (testResultId: number) => void
  onRerun: (testResultId: number, configurationId: number) => void,
  onViewResult: (testResultId: number) => void
  onExportProfile: (testResultId: number) => void,
  onRemove: (testResultId: number) => void
}

const getListColumns = (props: ListColumnsProps): IColumn[] => {
  return [
    {
      key: 'Id',
      name: 'ID',
      fieldName: 'Id',
      minWidth: 80,
      maxWidth: 100,
      isRowHeader: true,
      isResizable: false,
      onRender: (item: TestResultOverview) => props.onRenderId(item.Id)
    },
    {
      key: 'TestSuite',
      name: 'Test Suite',
      fieldName: 'TestSuite',
      minWidth: 160,
      isResizable: true,
      data: 'string',
      isPadded: true,
      onRender: (item: TestResultOverview) => props.onRenderTestSuite(item.ConfigurationId)
    },
    {
      key: 'Configuration',
      name: 'Configuration',
      fieldName: 'Configuration',
      minWidth: 160,
      isResizable: true,
      data: 'string',
      isPadded: true,
      onRender: (item: TestResultOverview) => props.onRenderConfiguration(item.ConfigurationId)
    },
    {
      key: 'Status',
      name: 'Status',
      fieldName: 'Status',
      minWidth: 80,
      maxWidth: 100,
      isResizable: false,
      data: 'string',
      isPadded: true,
      onRender: (item: TestResultOverview) => props.onRenderStatus(item.Status)
    },
    {
      key: 'Total',
      name: 'Total',
      minWidth: 80,
      maxWidth: 100,
      isResizable: false,
      data: 'string',
      isPadded: true,
      onRender: (item: TestResultOverview) => props.onRenderCount('Total', item.Total)
    },
    {
      key: 'Passed',
      name: 'Passed',
      minWidth: 80,
      maxWidth: 100,
      isResizable: false,
      data: 'string',
      isPadded: true,
      onRender: (item: TestResultOverview) => props.onRenderCount('Passed', item.Passed)
    },
    {
      key: 'Failed',
      name: 'Failed',
      minWidth: 80,
      maxWidth: 100,
      isResizable: false,
      data: 'string',
      isPadded: true,
      onRender: (item: TestResultOverview) => props.onRenderCount('Failed', item.Failed)
    },
    {
      key: 'Inconclusive',
      name: 'Inconclusive',
      minWidth: 80,
      maxWidth: 100,
      isResizable: false,
      data: 'string',
      isPadded: true,
      onRender: (item: TestResultOverview) => props.onRenderCount('Inconclusive', item.Inconclusive)
    },
    {
      key: 'Not Run',
      name: 'Not Run',
      minWidth: 80,
      maxWidth: 100,
      isResizable: false,
      data: 'string',
      isPadded: true,
      onRender: (item: TestResultOverview) => props.onRenderCount('NotRun', item.NotRun)
    },
    {
      key: 'Action',
      name: 'Action',
      minWidth: 380,
      maxWidth: 380,
      isResizable: false,
      data: 'string',
      isPadded: true,
      onRender: (item: TestResultOverview, _, __) => {
        return (
          <Stack horizontal tokens={StackGap10}>
            {
              item.Status === 'Created' || item.Status === 'Running'
                ? <PrimaryButton style={{ backgroundColor: '#ce3939' }} onClick={() => { props.onAbort(item.Id) }}>Abort</PrimaryButton>
                : null
            }
            {
              !props.isTestSuiteRemoved(item.ConfigurationId)
                ? <Stack horizontal tokens={StackGap10}>
                  {
                    item.Status !== 'Created' && item.Status !== 'Running'
                      ? <PrimaryButton onClick={() => { props.onRerun(item.Id, item.ConfigurationId) }}>Rerun</PrimaryButton>
                      : null
                  }
                  <PrimaryButton onClick={() => { props.onExportProfile(item.Id) }}>Export Profile</PrimaryButton>
                </Stack>
                : null
            }
            {
              item.Status !== 'Created'
                ? <PrimaryButton onClick={() => { props.onViewResult(item.Id) }}>View Result</PrimaryButton>
                : null
            }
            {
                !props.isTestSuiteRemoved(item.ConfigurationId)
                    ? <Stack horizontal tokens={StackGap10}>
                        {
                            item.Status !== 'Running'
                                ? <PrimaryButton onClick={() => { props.onRemove(item.Id) }}>Remove</PrimaryButton>
                                : null
                        }
                    </Stack>
                    : null
            }
          </Stack>
        )
      }
    }
  ]
}

const getTestCaseStateColor = (kind: TestCaseState | 'Total') => {
  switch (kind) {
    case 'Passed':
      return '#005700'

    case 'Failed':
      return '#8A0000'

    case 'Inconclusive':
      return '#7A5000'

    case 'NotRun':
      return '#545454'

    case 'Total':
    default:
      return '#000000'
  }
}

const renderCount = (kind: TestCaseState | 'Total', count: number | undefined) => {
  return <div style={{ color: getTestCaseStateColor(kind), fontSize: 'large' }}>{count}</div>
}

const getTestResultStateColor = (kind: TestResultState) => {
  switch (kind) {
    case 'Created':
      return '#525252'

    case 'Running':
      return '#000000'

    case 'Finished':
      return '#006600'

    case 'Failed':
      return '#B30000'
  }
}

const renderStatus = (status: TestResultState) => {
  return <div style={{ color: getTestResultStateColor(status), fontSize: 'large' }}>{status}</div>
}

export function TaskHistory(props: any) {
  const winSize = useWindowSize()

  const history = useHistory()

  const dispatch = useDispatch()

  const [dialogHidden, { toggle: toggleDialogHidden }] = useBoolean(true)

  const [removalDialogHidden, { toggle: toggleRemovalDialogHidden }] = useBoolean(true)

  const [testResultToAbort, setTestResultToAbort] = useState<TestResultOverview | undefined>(undefined)

  const [testResultToRemove, setTestResultToRemove] = useState<TestResultOverview | undefined>(undefined)

  const [tempQuery, setTempQuery] = useState<string | undefined>(undefined)

  const selectedTestCases = useSelector((state: AppState) => state.selectedTestCases)
  const testResults = useSelector((state: AppState) => state.testResults)

  const testSuiteDict = useMemo(() => getDict(testResults.allTestSuites, 'Id'), [testResults.allTestSuites])
  const configurationDict = useMemo(() => getDict(testResults.allConfigurations, 'Id'), [testResults.allConfigurations])

  const latestTestResultId = useMemo(() => selectedTestCases.testResultId, [selectedTestCases.testResultId])

  const allTestSuitesRef = useRef<TestSuite[]>()
  allTestSuitesRef.current = testResults.allTestSuites

  useEffect(() => {
    const handler = () => {
      dispatch(TestSuitesDataSrv.getTestSuiteList())
      allTestSuitesRef.current?.forEach((testSuite) => {
        dispatch(ConfigurationsDataSrv.getConfigurations(testSuite.Id))
      })
      dispatch(TestResultsDataSrv.listTestResults())
    }

    dispatch(TestResultsActions.clearSelectedTestResultAction())
    handler()
    const interval = setInterval(handler, 5000)

    return () => clearInterval(interval)
  }, [dispatch])

  useEffect(() => {
    const pageNumbers = document.getElementsByClassName("ms-Pagination-pageNumber")
    if (pageNumbers.length > 0) {
      for (let index = 0; index < pageNumbers.length; index++) {
        const pageNumber = pageNumbers.item(index)
        if (pageNumber) {
          const selectedValue = pageNumber.getAttribute("aria-selected")
          pageNumber.removeAttribute("aria-selected")
          if (selectedValue) {
            pageNumber.setAttribute("data-selected", selectedValue)
          }
        }
      }
    }
  })

  const onDialogAbortButtonClick = () => {
    if (testResultToAbort !== undefined) {
      dispatch(SelectedTestCasesDataSrv.abortRunRequest(testResultToAbort.Id, () => {
        dispatch(TestResultsDataSrv.listTestResults())
      }))
    }

    dispatch(() => toggleDialogHidden())
  }

  const onDialogRemoveButtonClick = () => {
    if (testResultToRemove !== undefined) {
      dispatch(SelectedTestCasesDataSrv.removeRunRequest(testResultToRemove.Id, () => {
        dispatch(TestResultsDataSrv.listTestResults())
      }))
    }

    dispatch(() => toggleRemovalDialogHidden())
  }

  const renderId = useCallback((testResultId: number) => {
    if (latestTestResultId !== undefined && latestTestResultId === testResultId) {
      return <Stack horizontal>
        <div style={{ color: '#ce3939', fontSize: 'large', fontWeight: 'bold' }}>New!</div>
        <div style={{ paddingLeft: 5, fontSize: 'large' }}>{testResultId}</div>
      </Stack>
    } else {
      return <div style={{ fontSize: 'large' }}>{testResultId}</div>
    }
  }, [latestTestResultId])

  const renderTestSuite = useCallback((configurationId: number) => {
    if (configurationDict[configurationId] === undefined) {
      return <Spinner size={SpinnerSize.xSmall} />
    }

    const testSuite = testSuiteDict[configurationDict[configurationId].TestSuiteId]
    if (testSuite.Removed) {
      return <div style={{ color: 'rgba(0, 0, 0, 0.5)', fontSize: 'large', textDecorationLine: 'line-through' }}>
        {`${testSuite.Name} ${testSuite.Version}`}
      </div>
    } else {
      return <div style={{ fontSize: 'large' }}>
        {`${testSuite.Name} ${testSuite.Version}`}
      </div>
    }
  }, [testSuiteDict, configurationDict])

  const isTestSuiteRemoved = useCallback((configurationId: number) => {
    if (configurationDict[configurationId] === undefined) {
      return true
    }

    return testSuiteDict[configurationDict[configurationId].TestSuiteId].Removed
  }, [testSuiteDict, configurationDict])

  const renderConfiguration = useCallback((configurationId: number) => {
    if (configurationDict[configurationId] === undefined) {
      return <Spinner size={SpinnerSize.xSmall} />
    }

    return <div style={{ fontSize: 'large' }}>
      {configurationDict[configurationId].Name}
    </div>
  }, [configurationDict])

  const renderDialogContent = useCallback((testResult: TestResultOverview | undefined) => {
    return (
      <div>
        {
          testResult !== undefined
            ? <div style={{ fontSize: 'large' }}>
              <Stack tokens={StackGap5}>
                <Stack horizontal><div style={{ paddingRight: 5 }}>TestSuite:</div>{renderTestSuite(testResult.ConfigurationId)}</Stack>
                <Stack horizontal><div style={{ paddingRight: 5 }}>Configuration:</div>{renderConfiguration(testResult.ConfigurationId)}</Stack>
                <Stack horizontal><div style={{ paddingRight: 5 }}>Status:</div><div>{testResult.Status}</div></Stack>
              </Stack>
            </div>
            : null
        }
      </div>
    )
  }, [renderTestSuite, renderConfiguration])

  const getTestResultSummary = useCallback((testResultId: number) => {
    const testResult = testResults.currentPageResults.filter(item => item.Id === testResultId)[0]
    return {
      TestSuite: testSuiteDict[configurationDict[testResult.ConfigurationId].TestSuiteId],
      Configuration: configurationDict[testResult.ConfigurationId].Name
    }
  }, [testResults.currentPageResults, testSuiteDict, configurationDict])

  const columns = getListColumns({
    onRenderId: renderId,
    onRenderTestSuite: renderTestSuite,
    isTestSuiteRemoved: isTestSuiteRemoved,
    onRenderConfiguration: renderConfiguration,
    onRenderStatus: renderStatus,
    onRenderCount: renderCount,
    onAbort: (testResultId: number) => {
      setTestResultToAbort(testResults.currentPageResults.filter(item => item.Id === testResultId)[0])
      toggleDialogHidden()
    },
    onRerun: (testResultId: number, configurationId: number) => {
      dispatch(TestResultsDataSrv.getTestResultDetail(testResultId, (result) => {
        dispatch(SelectedTestCasesDataSrv.createRunRequest(result.Results.map(item => item.FullName), configurationId))
      }))
    },
    onViewResult: (testResultId: number) => {
      dispatch(TestResultsActions.setSelectedTestResultAction(testResultId, getTestResultSummary(testResultId)))
      dispatch(() => history.push('/Tasks/TestResult', { from: 'TaskHistory' }))
    },
    onExportProfile: (testResultId: number) => {
      dispatch(ProfileDataSrv.saveProfile(testResultId))
    },
    onRemove: (testResultId: number) => {
      setTestResultToRemove(testResults.currentPageResults.filter(item => item.Id === testResultId)[0])
      toggleRemovalDialogHidden()
    }
  })

  const onSearchChanged = (query: string | undefined) => {
    dispatch(TestResultsActions.setQueryAction(query))
    dispatch(TestResultsDataSrv.listTestResults())
  }

  const onSearchClear = () => {
    dispatch(TestResultsActions.setQueryAction(undefined))
    dispatch(TestResultsDataSrv.listTestResults())
  }

  const onChangePageNumber = (pageNumber: number) => {
    dispatch(TestResultsActions.setPageNumberAction(pageNumber))
    dispatch(TestResultsDataSrv.listTestResults())
  }

  return (
    <div>
      <FullWidthPanel isLoading={testResults.isLoading} errorMsg={testResults.errorMsg} >
        <Toggle
          label="Show results of removed test suites"
          inlineLabel
          checked={testResults.showRemovedTestSuites}
          onChange={(_, checked) => {
            dispatch(TestResultsActions.setShowRemovedTestSuitesAction(checked ?? false))
            dispatch(TestResultsDataSrv.listTestResults())
          }} />
        <Stack horizontal horizontalAlign="end" style={{ marginTop: -34, paddingRight: 10 }} tokens={StackGap10}>
          <SearchBox
            style={{ minWidth: 300 }}
            placeholder="Input your query phrase..."
            onChange={(_, newValue) => setTempQuery(newValue)}
            onSearch={onSearchChanged}
            onClear={onSearchClear} />
          <PrimaryButton onClick={() => onSearchChanged(tempQuery)}>Search</PrimaryButton>
        </Stack>
        <hr style={{ border: '1px solid #d9d9d9' }} />
        <div style={{ height: winSize.height - 180, overflowY: 'auto' }}>
          {
            testResults.allTestSuites.length > 0 && testResults.allConfigurations.length > 0 && testResults.currentPageResults.length > 0
              ? <DetailsList
                items={testResults.currentPageResults}
                columns={columns}
                selectionMode={SelectionMode.none}
                layoutMode={DetailsListLayoutMode.justified}
                isHeaderVisible={true}
              />
              : testResults.pageCount > 0
                ? <div>
                  <Spinner size={SpinnerSize.medium} />
                  <p style={{ color: '#ab5f0e' }}>Loading...</p>
                </div>
                : <p>There are no results found currently.</p>
          }
        </div>
        <hr style={{ border: '1px solid #d9d9d9' }} />
        <div className='buttonPanel'>
          <Stack horizontal horizontalAlign='center' tokens={StackGap10}>
            <Pagination
              styles={{
                pageNumber: {
                  fontSize: 'large',
                  marginTop: -10
                }
              }}
              firstPageAriaLabel="First page"
              previousPageAriaLabel="Previous page"
              nextPageAriaLabel="Next page"
              lastPageAriaLabel="Last page"
              pageAriaLabel='Page'
              selectedAriaLabel='Selected'
              selectedPageIndex={testResults.pageNumber}
              pageCount={testResults.pageCount}
              onPageChange={onChangePageNumber}
              format='buttons'
            />
          </Stack>
        </div>
      </FullWidthPanel>
      <Dialog
        hidden={dialogHidden}
        onDismiss={toggleDialogHidden}
        dialogContentProps={{
          type: DialogType.normal,
          title: `Abort Test Run ${testResultToAbort?.Id}?`
        }}
        modalProps={{
          isBlocking: true
        }}>
        <Stack horizontalAlign='start'>
          {renderDialogContent(testResultToAbort)}
        </Stack>
        <DialogFooter>
          <PrimaryButton onClick={toggleDialogHidden} text="Cancel" />
          <PrimaryButton style={{ backgroundColor: '#ff4949' }} onClick={onDialogAbortButtonClick} text="Abort" />
        </DialogFooter>
      </Dialog>

      <Dialog
        hidden={removalDialogHidden}
        onDismiss={toggleRemovalDialogHidden}
        dialogContentProps={{
          type: DialogType.normal,
          title: `Are you sure you want to remove this test result?`
        }}
        modalProps={{
          isBlocking: true
        }}>
        <Stack horizontalAlign='start'>
          {renderDialogContent(testResultToRemove)}
        </Stack>
        <DialogFooter>
          <PrimaryButton onClick={toggleRemovalDialogHidden} text="Cancel" />
          <PrimaryButton style={{ backgroundColor: '#ff4949' }} onClick={onDialogRemoveButtonClick} text="Remove" />
        </DialogFooter>
      </Dialog>

    </div>
  )
}
