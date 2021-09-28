// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TestSuite } from '../model/TestSuite'

// define action consts
export const GET_TESTSUITES_REQUEST = 'TESTSUITEINFO/GET_TESTSUITES_REQUEST'
export const GET_TESTSUITES_SUCCESS = 'TESTSUITEINFO/GET_TESTSUITES_SUCCESS'
export const GET_TESTSUITES_FAILURE = 'TESTSUITEINFO/GET_TESTSUITES_FAILURE'

// define action types
interface GetTestSuitesActionRequestType { type: typeof GET_TESTSUITES_REQUEST }
interface GetTestSuitesActionSuccessType { type: typeof GET_TESTSUITES_SUCCESS, payload: TestSuite[] }
interface GetTestSuitesActionFailureType { type: typeof GET_TESTSUITES_FAILURE, errorMsg: string }
export type TestSuitesActionTypes = GetTestSuitesActionRequestType | GetTestSuitesActionSuccessType | GetTestSuitesActionFailureType

// define actions
export const TestSuiteActions = {
  getTestSuitesAction_Request: (): TestSuitesActionTypes => {
    return {
      type: GET_TESTSUITES_REQUEST
    }
  },
  getTestSuitesAction_Success: (testSuites: TestSuite[]): TestSuitesActionTypes => {
    return {
      type: GET_TESTSUITES_SUCCESS,
      payload: testSuites
    }
  },
  getTestSuitesAction_Failure: (error: string): TestSuitesActionTypes => {
    return {
      type: GET_TESTSUITES_FAILURE,
      errorMsg: error
    }
  }
}
