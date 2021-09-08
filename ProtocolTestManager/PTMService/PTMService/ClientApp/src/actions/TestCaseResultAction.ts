// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TestCaseResult } from '../model/TestCaseResult'

// define action consts
export const GET_TESTCASERESULT_REQUEST = 'TESTCASERESULT/GET_TESTCASERESULT_REQUEST'
export const GET_TESTCASERESULT_SUCCESS = 'TESTCASERESULT/GET_TESTCASERESULT_SUCCESS'
export const GET_TESTCASERESULT_FAILURE = 'TESTCASERESULT/GET_TESTCASERESULT_FAILURE'

export const CLEAR_SELECTEDTESTCASERESULT = 'TESTCASERESULT/CLEAR_SELECTEDTESTCASERESULT'

// define action types
interface GetTestCaseResultActionRequestType { type: typeof GET_TESTCASERESULT_REQUEST, payload: string }
interface GetTestCaseResultActionSuccessType { type: typeof GET_TESTCASERESULT_SUCCESS, payload: TestCaseResult }
interface GetTestCaseResultActionFailureType { type: typeof GET_TESTCASERESULT_FAILURE, errorMsg: string }

interface ClearSelectedTestCaseResultActionType { type: typeof CLEAR_SELECTEDTESTCASERESULT }

export type TestCaseResultActionTypes =
    GetTestCaseResultActionRequestType |
    GetTestCaseResultActionSuccessType |
    GetTestCaseResultActionFailureType |
    ClearSelectedTestCaseResultActionType

// define actions
export const TestCaseResultActions = {
  getTestCaseResultAction_Request: (testCaseName: string): TestCaseResultActionTypes => {
    return {
      type: GET_TESTCASERESULT_REQUEST,
      payload: testCaseName
    }
  },
  getTestCaseResultAction_Success: (testCaseResult: TestCaseResult): TestCaseResultActionTypes => {
    return {
      type: GET_TESTCASERESULT_SUCCESS,
      payload: testCaseResult
    }
  },
  getTestCaseResultAction_Failure: (error: string): TestCaseResultActionTypes => {
    return {
      type: GET_TESTCASERESULT_FAILURE,
      errorMsg: error
    }
  },
  clearSelectedTestCaseResultAction: (): TestCaseResultActionTypes => {
    return {
      type: CLEAR_SELECTEDTESTCASERESULT
    }
  }
}
