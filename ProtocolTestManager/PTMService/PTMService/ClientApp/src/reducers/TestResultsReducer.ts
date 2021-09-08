// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  GET_TESTSUITES_REQUEST,
  GET_TESTSUITES_SUCCESS,
  GET_TESTSUITES_FAILURE,
  TestSuitesActionTypes
} from '../actions/TestSuitesAction'

import {
  GET_TESTSUITE_CONFIGURATIONS_REQUEST,
  GET_TESTSUITE_CONFIGURATIONS_SUCCESS,
  GET_TESTSUITE_CONFIGURATIONS_FAILURE,
  TestSuiteConfigurationActionTypes
} from '../actions/TestSuiteConfigurationAction'

import {
  SAVE_PROFILE_REQUEST,
  SAVE_PROFILE_SUCCESS,
  SAVE_PROFILE_FAILURE,
  TestSuiteConfigureMethodActionTypes
} from '../actions/ConfigureMethodAction'

import {
  CREATE_RUNREQUEST_REQUEST,
  CREATE_RUNREQUEST_SUCCESS,
  CREATE_RUNREQUEST_FAILURE,
  ABORT_RUNREQUEST_REQUEST,
  ABORT_RUNREQUEST_SUCCESS,
  ABORT_RUNREQUEST_FAILURE,
  SelectedTestCasesActionTypes
} from '../actions/SelectedTestCasesAction'

import {
  LIST_TESTRESULTS_REQUEST,
  LIST_TESTRESULTS_SUCCESS,
  LIST_TESTRESULTS_FAILURE,
  SET_SHOWREMOVEDTESTSUITES,
  SET_PAGENUMBER,
  SET_QUERY,
  SET_SELECTEDTESTRESULT,
  CLEAR_SELECTEDTESTRESULT,
  GET_TESTRESULTDETAIL_REQUEST,
  GET_TESTRESULTDETAIL_SUCCESS,
  GET_TESTRESULTDETAIL_FAILURE,
  GET_TESTRUNREPORT_REQUEST,
  GET_TESTRUNREPORT_SUCCESS,
  GET_TESTRUNREPORT_FAILURE,
  TestResultsActionTypes
} from '../actions/TestResultsAction'
import { Configuration } from '../model/Configuration'
import { TestResult, TestResultOverview, TestResultSummary } from '../model/TestResult'
import { TestSuite } from '../model/TestSuite'

export interface TestResultsState {
  isLoading: boolean
  isPosting: boolean
  isDownloading: boolean
  errorMsg?: string
  allTestSuites: TestSuite[]
  showRemovedTestSuites: boolean
  allConfigurations: Array<Required<Configuration>>
  pageNumber: number
  pageCount: number
  currentPageResults: TestResultOverview[]
  pageSize: 15
  query: string | undefined
  selectedTestResultId: number | undefined
  selectedTestResultSummary: TestResultSummary | undefined
  selectedTestResult: TestResult | undefined
}

const initialTestResultsState: TestResultsState = {
  isLoading: false,
  isPosting: false,
  isDownloading: false,
  errorMsg: undefined,
  allTestSuites: [],
  showRemovedTestSuites: false,
  allConfigurations: [],
  pageNumber: 0,
  pageCount: 1,
  currentPageResults: [],
  pageSize: 15,
  query: undefined,
  selectedTestResultId: undefined,
  selectedTestResultSummary: undefined,
  selectedTestResult: undefined
}

const getUpdatedRequiredConfigurations = (state: TestResultsState, newConfigurations: Configuration[]): Array<Required<Configuration>> => {
  if (state.allConfigurations.length === 0) {
    return newConfigurations.map(item => {
      return {
        ...item,
        Id: item.Id!
      }
    })
  }

  const updatedRequiredConf = state.allConfigurations.map((item) => {
    const updatedList = newConfigurations.filter((i) => i.Id === item.Id)
    if (updatedList.length > 0) {
      return {
        ...updatedList[0],
        Id: updatedList[0].Id!
      }
    } else {
      return item
    }
  })

  const newRequiredConf = newConfigurations.filter((item) => updatedRequiredConf.reduce((res: boolean, curr) => res && curr.Id !== item.Id, true))
    .map(item => {
      return {
        ...item,
        Id: item.Id!
      }
    })

  return [...updatedRequiredConf, ...newRequiredConf]
}

export const getTestResultsReducer = (state = initialTestResultsState, action: TestResultsActionTypes | TestSuitesActionTypes | TestSuiteConfigurationActionTypes | TestSuiteConfigureMethodActionTypes | SelectedTestCasesActionTypes): TestResultsState => {
  switch (action.type) {
    case GET_TESTSUITES_REQUEST:
      return {
        ...state,
        isPosting: true,
        errorMsg: undefined
      }

    case GET_TESTSUITES_SUCCESS:
      return {
        ...state,
        isPosting: false,
        allTestSuites: action.payload
      }

    case GET_TESTSUITES_FAILURE:
      return {
        ...state,
        isPosting: false,
        errorMsg: action.errorMsg
      }

    case GET_TESTSUITE_CONFIGURATIONS_REQUEST:
      return {
        ...state,
        isPosting: true,
        errorMsg: undefined
      }

    case GET_TESTSUITE_CONFIGURATIONS_SUCCESS:
      return {
        ...state,
        isPosting: false,
        allConfigurations: getUpdatedRequiredConfigurations(state, action.payload)
      }

    case GET_TESTSUITE_CONFIGURATIONS_FAILURE:
      return {
        ...state,
        isPosting: false,
        errorMsg: action.errorMsg
      }

    case SAVE_PROFILE_REQUEST:
      return {
        ...state,
        isPosting: true,
        errorMsg: undefined
      }

    case SAVE_PROFILE_SUCCESS:
      return {
        ...state,
        isPosting: false
      }

    case SAVE_PROFILE_FAILURE:
      return {
        ...state,
        isPosting: false,
        errorMsg: action.errorMsg
      }

    case CREATE_RUNREQUEST_REQUEST:
      return {
        ...state,
        isLoading: true,
        errorMsg: undefined
      }

    case CREATE_RUNREQUEST_SUCCESS:
      return {
        ...state,
        isLoading: false
      }

    case CREATE_RUNREQUEST_FAILURE:
      return {
        ...state,
        isLoading: false,
        errorMsg: action.errorMsg
      }

    case ABORT_RUNREQUEST_REQUEST:
      return {
        ...state,
        isLoading: true,
        errorMsg: undefined
      }

    case ABORT_RUNREQUEST_SUCCESS:
      return {
        ...state,
        isLoading: false
      }

    case ABORT_RUNREQUEST_FAILURE:
      return {
        ...state,
        isLoading: false,
        errorMsg: action.errorMsg
      }

    case LIST_TESTRESULTS_REQUEST:
      return {
        ...state,
        isPosting: true,
        errorMsg: undefined
      }

    case LIST_TESTRESULTS_SUCCESS:
      return {
        ...state,
        isPosting: false,
        pageNumber: action.payload.PageCount === 0 ? 0 : (state.pageNumber > action.payload.PageCount - 1 ? action.payload.PageCount - 1 : state.pageNumber),
        pageCount: action.payload.PageCount,
        currentPageResults: action.payload.TestResults
      }

    case LIST_TESTRESULTS_FAILURE:
      return {
        ...state,
        isPosting: false,
        errorMsg: action.errorMsg
      }

    case SET_SHOWREMOVEDTESTSUITES:
      return {
        ...state,
        showRemovedTestSuites: action.payload
      }

    case SET_PAGENUMBER:
      return {
        ...state,
        pageNumber: action.payload
      }

    case SET_QUERY:
      return {
        ...state,
        query: action.payload
      }

    case SET_SELECTEDTESTRESULT:
      return {
        ...state,
        selectedTestResultId: action.testResultId,
        selectedTestResultSummary: action.summary
      }

    case CLEAR_SELECTEDTESTRESULT:
      return {
        ...state,
        selectedTestResultId: undefined,
        selectedTestResultSummary: undefined,
        selectedTestResult: undefined
      }

    case GET_TESTRESULTDETAIL_REQUEST:
      return {
        ...state,
        isLoading: true,
        errorMsg: undefined
      }

    case GET_TESTRESULTDETAIL_SUCCESS:
      return {
        ...state,
        isLoading: false,
        selectedTestResult: action.payload
      }

    case GET_TESTRESULTDETAIL_FAILURE:
      return {
        ...state,
        isLoading: false,
        errorMsg: action.errorMsg
      }

    case GET_TESTRUNREPORT_REQUEST:
      return {
        ...state,
        isDownloading: true,
        errorMsg: undefined
      }

    case GET_TESTRUNREPORT_SUCCESS:
      return {
        ...state,
        isDownloading: false
      }

    case GET_TESTRUNREPORT_FAILURE:
      return {
        ...state,
        isDownloading: false,
        errorMsg: action.errorMsg
      }

    default:
      return state
  }
}
