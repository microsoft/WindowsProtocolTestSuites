// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  GET_TESTSUITES_FAILURE,
  GET_TESTSUITES_REQUEST,
  GET_TESTSUITES_SUCCESS,
  TestSuitesActionTypes
} from '../actions/TestSuitesAction'
import { TestSuite } from '../model/TestSuite'

export interface TestSuitesState {
  isLoading: boolean
  errorMsg?: string
  testSuiteList: TestSuite[]
}

const initialTestSuitesState: TestSuitesState = {
  isLoading: false,
  errorMsg: undefined,
  testSuiteList: []

}

export const getTestSuitesReducer = (state = initialTestSuitesState, action: TestSuitesActionTypes): TestSuitesState => {
  switch (action.type) {
    case GET_TESTSUITES_REQUEST:
      return {
        ...state,
        isLoading: true,
        errorMsg: undefined,
        testSuiteList: []
      }

    case GET_TESTSUITES_SUCCESS:
      return {
        ...state,
        isLoading: false,
        testSuiteList: action.payload.filter(t => !t.Removed)
      }

    case GET_TESTSUITES_FAILURE:
      return {
        ...state,
        isLoading: false,
        testSuiteList: [],
        errorMsg: action.errorMsg
      }

    default:
      return state
  }
}
