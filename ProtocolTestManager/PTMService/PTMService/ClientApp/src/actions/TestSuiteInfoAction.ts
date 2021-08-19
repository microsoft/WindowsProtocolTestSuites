// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TestSuite } from '../model/TestSuite'

// define action consts
export const GET_TESTSUITEINFO_REQUEST = 'TESTSUITEINFO/GET_TESTSUITEINFO_REQUEST'
export const GET_TESTSUITEINFO_SUCCESS = 'TESTSUITEINFO/GET_TESTSUITEINFO_SUCCESS'
export const GET_TESTSUITEINFO_FAILURE = 'TESTSUITEINFO/GET_TESTSUITEINFO_FAILURE'

export const SET_SELECTED_TESTSUITE = 'TESTSUITEINFO/SET_SELECTED_TESTSUITE'

// define action types
interface GetTestSuiteInfoActionRequestType { type: typeof GET_TESTSUITEINFO_REQUEST }
interface GetTestSuiteInfoActionSuccessType { type: typeof GET_TESTSUITEINFO_SUCCESS }
interface GetTestSuiteInfoActionFailureType { type: typeof GET_TESTSUITEINFO_FAILURE, errorMsg: string }
interface SetSelectedTestSuiteActionType { type: typeof SET_SELECTED_TESTSUITE, selectedTestSuite: TestSuite }

export type TestSuiteInfoActionTypes = GetTestSuiteInfoActionRequestType
| GetTestSuiteInfoActionSuccessType
| GetTestSuiteInfoActionFailureType
| SetSelectedTestSuiteActionType

// define actions
export const TestSuiteInfoActions = {
  getTestSuiteInfoAction_Request: (): TestSuiteInfoActionTypes => {
    return {
      type: GET_TESTSUITEINFO_REQUEST
    }
  },
  getTestSuiteInfoAction_Success: (): TestSuiteInfoActionTypes => {
    return {
      type: GET_TESTSUITEINFO_SUCCESS
    }
  },
  getTestSuiteInfoAction_Failure: (error: string): TestSuiteInfoActionTypes => {
    return {
      type: GET_TESTSUITEINFO_FAILURE,
      errorMsg: error
    }
  },
  setSelectedTestSuiteAction: (selectedTestSuite: TestSuite): TestSuiteInfoActionTypes => {
    return {
      type: SET_SELECTED_TESTSUITE,
      selectedTestSuite: selectedTestSuite
    }
  }
}
