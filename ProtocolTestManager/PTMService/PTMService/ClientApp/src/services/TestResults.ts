// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FetchService, RequestMethod } from '.'
import { TestResultsActions, TestResultsActionTypes } from '../actions/TestResultsAction'
import { ReportFormat, ReportRequest, TestResult } from '../model/TestResult'
import { AppThunkAction } from '../store/configureStore'

const downloadBlob = (fileName: string, blob: Blob | undefined) => {
  if (blob === undefined) {
    return
  }

  const url = window.URL.createObjectURL(new Blob([blob]))
  const link = document.createElement('a')
  link.href = url
  link.setAttribute('download', fileName)
  link.click()
}

const getRequestHeaders = (format: ReportFormat) => {
  let mimeType = 'text/plain'
  switch (format) {
    case 'Plain':
      mimeType = 'text/plain'
      break

    case 'Json':
      mimeType = 'text/plain'
      break

    case 'XUnit':
      mimeType = 'application/xml'
      break
  }

  return {
    Accept: mimeType,
    'Content-Type': 'application/json'
  }
}

const getFileNameWithExtension = (fileName: string, format: ReportFormat) => {
  let extension = 'txt'
  switch (format) {
    case 'Plain':
      extension = 'txt'
      break

    case 'Json':
      extension = 'json'
      break

    case 'XUnit':
      extension = 'xml'
      break
  }

  return `${fileName}.${extension}`
}

export const TestResultsDataSrv = {
  listTestResults: (): AppThunkAction<TestResultsActionTypes> => async (dispatch, getState) => {
    const state = getState()

    const pageSize = state.testResults.pageSize
    const pageNumber = state.testResults.pageNumber
    const showAll = state.testResults.showRemovedTestSuites
    const query = state.testResults.query

    await FetchService({
      url: `api/testresult?pageSize=${pageSize}&pageNumber=${pageNumber}` +
        (query === undefined ? '' : `&query=${query}`) +
        (!showAll ? '' : `&showAll=${showAll}`),
      method: RequestMethod.GET,
      dispatch,
      onRequest: TestResultsActions.listTestResultsAction_Request,
      onComplete: TestResultsActions.listTestResultsAction_Success,
      onError: TestResultsActions.listTestResultsAction_Failure
    })
  },
  getTestResultDetail: (testResultId: number, completeCallback?: (result: TestResult) => void): AppThunkAction<TestResultsActionTypes> => async (dispatch, getState) => {
    await FetchService({
      url: `api/testresult/${testResultId}`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: TestResultsActions.getTestResultDetailAction_Request,
      onComplete: TestResultsActions.getTestResultDetailAction_Success,
      onError: TestResultsActions.getTestResultDetailAction_Failure,
      onCompleteCallback: completeCallback
    })
  },
  getTestRunReport: (testResultId: number, reportRequest: ReportRequest): AppThunkAction<TestResultsActionTypes> => async (dispatch, getState) => {
    await FetchService({
      url: `api/testresult/${testResultId}/report`,
      method: RequestMethod.POST,
      headers: getRequestHeaders(reportRequest.Format),
      body: JSON.stringify(reportRequest),
      dispatch,
      onRequest: TestResultsActions.getTestRunReportAction_Request,
      onComplete: TestResultsActions.getTestRunReportAction_Success,
      onError: TestResultsActions.getTestRunReportAction_Failure,
      onCompleteCallback: (data: Blob | undefined) => downloadBlob(getFileNameWithExtension(`${testResultId}`, reportRequest.Format), data)
    })
  }
}
