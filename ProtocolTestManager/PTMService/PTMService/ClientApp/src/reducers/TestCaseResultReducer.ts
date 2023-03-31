// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
  REMOVE_RUNREQUEST_REQUEST,
  REMOVE_RUNREQUEST_SUCCESS,
  REMOVE_RUNREQUEST_FAILURE,  
  SelectedTestCasesActionTypes
} from '../actions/SelectedTestCasesAction'

import {
  GET_TESTRESULTDETAIL_REQUEST,
  GET_TESTRESULTDETAIL_SUCCESS,
  GET_TESTRESULTDETAIL_FAILURE,
  GET_TESTRUNREPORT_REQUEST,
  GET_TESTRUNREPORT_SUCCESS,
  GET_TESTRUNREPORT_FAILURE,
  TestResultsActionTypes
} from '../actions/TestResultsAction'

import {
  GET_TESTCASERESULT_REQUEST,
  GET_TESTCASERESULT_SUCCESS,
  GET_TESTCASERESULT_FAILURE,
  CLEAR_SELECTEDTESTCASERESULT,
  TestCaseResultActionTypes
} from '../actions/TestCaseResultAction'

import { TestCaseResult } from '../model/TestCaseResult'

export interface TestCaseResultState {
  isLoading: boolean
  isPosting: boolean
  errorMsg?: string
  selectedTestCaseResult: TestCaseResult | undefined
}

const initialTestCaseResultState: TestCaseResultState = {
  isLoading: false,
  isPosting: false,
  errorMsg: undefined,
  selectedTestCaseResult: undefined
}

export const getTestCaseResultReducer = (state = initialTestCaseResultState, action: TestCaseResultActionTypes | TestSuiteConfigureMethodActionTypes | SelectedTestCasesActionTypes | TestResultsActionTypes): TestCaseResultState => {
  switch (action.type) {
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

    case REMOVE_RUNREQUEST_REQUEST:
      return {
        ...state,
        isLoading: true,
        errorMsg: undefined
      }

    case REMOVE_RUNREQUEST_SUCCESS:
      return {
        ...state,
        isLoading: false
      }

    case REMOVE_RUNREQUEST_FAILURE:
      return {
        ...state,
        isLoading: false,
        errorMsg: action.errorMsg
      }

    case GET_TESTRESULTDETAIL_REQUEST:
      return {
        ...state,
        isPosting: true,
        errorMsg: undefined
      }

    case GET_TESTRESULTDETAIL_SUCCESS:
      return {
        ...state,
        isPosting: false
      }

    case GET_TESTRESULTDETAIL_FAILURE:
      return {
        ...state,
        isPosting: false,
        errorMsg: action.errorMsg
      }

    case GET_TESTRUNREPORT_REQUEST:
      return {
        ...state,
        isPosting: true,
        errorMsg: undefined
      }

    case GET_TESTRUNREPORT_SUCCESS:
      return {
        ...state,
        isPosting: false
      }

    case GET_TESTRUNREPORT_FAILURE:
      return {
        ...state,
        isPosting: false,
        errorMsg: action.errorMsg
      }

    case GET_TESTCASERESULT_REQUEST:
      return {
        ...state,
        isPosting: true,
        errorMsg: undefined
      }

    case GET_TESTCASERESULT_SUCCESS:
      return {
        ...state,
        isPosting: false,
        selectedTestCaseResult: action.payload
      }

    case GET_TESTCASERESULT_FAILURE:
      return {
        ...state,
        isPosting: false,
        errorMsg: action.errorMsg
      }

    case CLEAR_SELECTEDTESTCASERESULT:
      return {
        ...state,
        selectedTestCaseResult: undefined
      }

    default:
      return state
  }
}
