// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ListResponse } from '../model/ListResponse'
import { ReportRequest, TestResult, TestResultSummary } from '../model/TestResult'

// define action consts
export const LIST_TESTRESULTS_REQUEST = 'TESTRESULTS/LIST_TESTRESULTS_REQUEST'
export const LIST_TESTRESULTS_SUCCESS = 'TESTRESULTS/LIST_TESTRESULTS_SUCCESS'
export const LIST_TESTRESULTS_FAILURE = 'TESTRESULTS/LIST_TESTRESULTS_FAILURE'

export const SET_SHOWREMOVEDTESTSUITES = 'TESTRESULTS/SET_SHOWREMOVEDTESTSUITES'

export const SET_PAGENUMBER = 'TESTRESULTS/SET_PAGENUMBER'
export const SET_QUERY = 'TESTRESULTS/SET_QUERY'

export const SET_SELECTEDTESTRESULT = 'TESTRESULTS/SET_SELECTEDTESTRESULT'
export const CLEAR_SELECTEDTESTRESULT = 'TESTRESULTS/CLEAR_SELECTEDTESTRESULT'

export const GET_TESTRESULTDETAIL_REQUEST = 'TESTRESULTS/GET_TESTRESULTDETAIL_REQUEST'
export const GET_TESTRESULTDETAIL_SUCCESS = 'TESTRESULTS/GET_TESTRESULTDETAIL_SUCCESS'
export const GET_TESTRESULTDETAIL_FAILURE = 'TESTRESULTS/GET_TESTRESULTDETAIL_FAILURE'

export const GET_TESTRUNREPORT_REQUEST = 'TESTRESULTS/GET_TESTRUNREPORT_REQUEST'
export const GET_TESTRUNREPORT_SUCCESS = 'TESTRESULTS/GET_TESTRUNREPORT_SUCCESS'
export const GET_TESTRUNREPORT_FAILURE = 'TESTRESULTS/GET_TESTRUNREPORT_FAILURE'

// define action types
interface ListTestResultsActionRequestType { type: typeof LIST_TESTRESULTS_REQUEST, payload: number }
interface ListTestResultsActionSuccessType { type: typeof LIST_TESTRESULTS_SUCCESS, payload: ListResponse }
interface ListTestResultsActionFailureType { type: typeof LIST_TESTRESULTS_FAILURE, errorMsg: string }

interface SetShowRemovedTestSuitesActionType { type: typeof SET_SHOWREMOVEDTESTSUITES, payload: boolean }

interface SetPageNumberActionType { type: typeof SET_PAGENUMBER, payload: number }
interface SetQueryActionType { type: typeof SET_QUERY, payload: string | undefined }

interface SetSelectedTestResultActionType { type: typeof SET_SELECTEDTESTRESULT, testResultId: number, summary: TestResultSummary }
interface ClearSelectedTestResultActionType { type: typeof CLEAR_SELECTEDTESTRESULT }

interface GetTestResultDetailActionRequestType { type: typeof GET_TESTRESULTDETAIL_REQUEST, payload: number }
interface GetTestResultDetailActionSuccessType { type: typeof GET_TESTRESULTDETAIL_SUCCESS, payload: TestResult }
interface GetTestResultDetailActionFailureType { type: typeof GET_TESTRESULTDETAIL_FAILURE, errorMsg: string }

interface GetTestRunReportActionRequestType { type: typeof GET_TESTRUNREPORT_REQUEST, testResultId: number, reportRequest: ReportRequest }
interface GetTestRunReportActionSuccessType { type: typeof GET_TESTRUNREPORT_SUCCESS }
interface GetTestRunReportActionFailureType { type: typeof GET_TESTRUNREPORT_FAILURE, errorMsg: string }

export type TestResultsActionTypes =
    ListTestResultsActionRequestType |
    ListTestResultsActionSuccessType |
    ListTestResultsActionFailureType |
    SetShowRemovedTestSuitesActionType |
    SetPageNumberActionType |
    SetQueryActionType |
    SetSelectedTestResultActionType |
    ClearSelectedTestResultActionType |
    GetTestResultDetailActionRequestType |
    GetTestResultDetailActionSuccessType |
    GetTestResultDetailActionFailureType |
    GetTestRunReportActionRequestType |
    GetTestRunReportActionSuccessType |
    GetTestRunReportActionFailureType

// define actions
export const TestResultsActions = {
  listTestResultsAction_Request: (pageNumber: number): TestResultsActionTypes => {
    return {
      type: LIST_TESTRESULTS_REQUEST,
      payload: pageNumber
    }
  },
  listTestResultsAction_Success: (listResponse: ListResponse): TestResultsActionTypes => {
    return {
      type: LIST_TESTRESULTS_SUCCESS,
      payload: listResponse
    }
  },
  listTestResultsAction_Failure: (error: string): TestResultsActionTypes => {
    return {
      type: LIST_TESTRESULTS_FAILURE,
      errorMsg: error
    }
  },
  setShowRemovedTestSuitesAction: (shown: boolean): TestResultsActionTypes => {
    return {
      type: SET_SHOWREMOVEDTESTSUITES,
      payload: shown
    }
  },
  setPageNumberAction: (pageNumber: number): TestResultsActionTypes => {
    return {
      type: SET_PAGENUMBER,
      payload: pageNumber
    }
  },
  setQueryAction: (query: string | undefined): TestResultsActionTypes => {
    return {
      type: SET_QUERY,
      payload: query
    }
  },
  setSelectedTestResultAction: (testResultId: number, summary: TestResultSummary): TestResultsActionTypes => {
    return {
      type: SET_SELECTEDTESTRESULT,
      testResultId: testResultId,
      summary: summary
    }
  },
  clearSelectedTestResultAction: (): TestResultsActionTypes => {
    return {
      type: CLEAR_SELECTEDTESTRESULT
    }
  },
  getTestResultDetailAction_Request: (testResultId: number): TestResultsActionTypes => {
    return {
      type: GET_TESTRESULTDETAIL_REQUEST,
      payload: testResultId
    }
  },
  getTestResultDetailAction_Success: (testResult: TestResult): TestResultsActionTypes => {
    return {
      type: GET_TESTRESULTDETAIL_SUCCESS,
      payload: testResult
    }
  },
  getTestResultDetailAction_Failure: (error: string): TestResultsActionTypes => {
    return {
      type: GET_TESTRESULTDETAIL_FAILURE,
      errorMsg: error
    }
  },
  getTestRunReportAction_Request: (testResultId: number, reportRequest: ReportRequest): TestResultsActionTypes => {
    return {
      type: GET_TESTRUNREPORT_REQUEST,
      testResultId: testResultId,
      reportRequest: reportRequest
    }
  },
  getTestRunReportAction_Success: (): TestResultsActionTypes => {
    return {
      type: GET_TESTRUNREPORT_SUCCESS
    }
  },
  getTestRunReportAction_Failure: (error: string): TestResultsActionTypes => {
    return {
      type: GET_TESTRUNREPORT_FAILURE,
      errorMsg: error
    }
  }
}
