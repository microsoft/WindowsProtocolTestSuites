// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  GET_TESTSUITES_REQUEST,
  GET_TESTSUITES_SUCCESS,
  GET_TESTSUITES_FAILURE,
  INSTALL_TESTSUITE_REQUEST,
  INSTALL_TESTSUITE_SUCCESS,
  INSTALL_TESTSUITE_FAILURE,
  UPDATE_TESTSUITE_REQUEST,
  UPDATE_TESTSUITE_SUCCESS,
  UPDATE_TESTSUITE_FAILURE,
  REMOVE_TESTSUITE_REQUEST,
  REMOVE_TESTSUITE_SUCCESS,
  REMOVE_TESTSUITE_FAILURE,
  SET_SEARCHTEXT,
  ManagementActionTypes
} from '../actions/ManagementAction'
import { TestSuite } from '../model/TestSuite'

export interface TestSuitesState {
  isLoading: boolean
  isProcessing: boolean
  errorMsg?: string
  testSuiteList: TestSuite[]
  displayList: TestSuite[]
  selectedTestSuite?: TestSuite
  searchText?: string
}

const initialTestSuitesState: TestSuitesState = {
  isLoading: false,
  isProcessing: false,
  errorMsg: undefined,
  testSuiteList: [],
  displayList: [],
  selectedTestSuite: undefined,
  searchText: undefined
}

export const getManagementReducer = (state = initialTestSuitesState, action: ManagementActionTypes): TestSuitesState => {
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
        testSuiteList: action.payload,
        displayList: filterTestSuites(action.payload, state.searchText)
      }

    case GET_TESTSUITES_FAILURE:
      return {
        ...state,
        isLoading: false,
        testSuiteList: [],
        errorMsg: action.errorMsg
      }

    case INSTALL_TESTSUITE_REQUEST:
    case UPDATE_TESTSUITE_REQUEST:
    case REMOVE_TESTSUITE_REQUEST:
      return {
        ...state,
        isLoading: false,
        isProcessing: true,
        errorMsg: undefined
      }

    case INSTALL_TESTSUITE_SUCCESS:
    case UPDATE_TESTSUITE_SUCCESS:
    case REMOVE_TESTSUITE_SUCCESS:
      return {
        ...state,
        isLoading: false,
        isProcessing: false,
        errorMsg: undefined
      }

    case INSTALL_TESTSUITE_FAILURE:
    case UPDATE_TESTSUITE_FAILURE:
    case REMOVE_TESTSUITE_FAILURE:
      return {
        ...state,
        isLoading: false,
        isProcessing: false,
        errorMsg: action.errorMsg
      }

    case SET_SEARCHTEXT:
      return {
        ...state,
        isLoading: false,
        displayList: filterTestSuites(state.testSuiteList, action.searchText)
      }

    default:
      return state
  }
}

function filterTestSuites (originalList: TestSuite[], searchText?: string): TestSuite[] {
  if (!searchText) {
    return originalList.filter(t => !t.Removed)
  }

  const lowerSearchText = searchText.toLowerCase()
  const newList = originalList.reduce((prevList: TestSuite[], curr) => {
    if (!curr.Removed) {
      if (curr.Name.toLowerCase().includes(lowerSearchText)) {
        return [...prevList, curr]
      } else {
        return prevList
      }
    } else {
      return prevList
    }
  }, [])

  return newList
}
