// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GET_TESTSUITEINFO_FAILURE, GET_TESTSUITEINFO_REQUEST, GET_TESTSUITEINFO_SUCCESS, SET_SELECTED_TESTSUITE, TestSuiteInfoActionTypes } from '../actions/TestSuiteInfoAction'
import { TestSuite } from '../model/TestSuite'

export interface TestSuiteInfoState {
  isLoading: boolean
  errorMsg?: string
  selectedTestSuite?: TestSuite
  pageDate: any
}

const initialTestSuitesState: TestSuiteInfoState = {
  isLoading: true,
  errorMsg: undefined,
  selectedTestSuite: undefined,
  pageDate: undefined
}

export const getTestSuiteInfoReducer = (state = initialTestSuitesState, action: TestSuiteInfoActionTypes): TestSuiteInfoState => {
  switch (action.type) {
    case GET_TESTSUITEINFO_REQUEST:
      return {
        ...state,
        isLoading: true,
        errorMsg: undefined
      }
    case GET_TESTSUITEINFO_SUCCESS:
      return {
        ...state,
        isLoading: false
      }
    case GET_TESTSUITEINFO_FAILURE:
      return {
        ...state,
        isLoading: false,
        errorMsg: action.errorMsg
      }
    case SET_SELECTED_TESTSUITE:
      return {
        ...state,
        isLoading: true,
        selectedTestSuite: action.selectedTestSuite
      }

    default:
      return state
  }
}
