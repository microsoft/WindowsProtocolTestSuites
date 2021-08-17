// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TestSuite } from '../model/TestSuite'

// define action consts
export const GET_TESTSUITES_REQUEST = 'MANAGEMENT/GET_TESTSUITES_REQUEST'
export const GET_TESTSUITES_SUCCESS = 'MANAGEMENT/GET_TESTSUITES_SUCCESS'
export const GET_TESTSUITES_FAILURE = 'MANAGEMENT/GET_TESTSUITES_FAILURE'

export const INSTALL_TESTSUITE_REQUEST = 'MANAGEMENT/INSTALL_TESTSUITE_REQUEST'
export const INSTALL_TESTSUITE_SUCCESS = 'MANAGEMENT/INSTALL_TESTSUITE_SUCCESS'
export const INSTALL_TESTSUITE_FAILURE = 'MANAGEMENT/INSTALL_TESTSUITE_FAILURE'

export const UPDATE_TESTSUITE_REQUEST = 'MANAGEMENT/UPDATE_TESTSUITE_REQUEST'
export const UPDATE_TESTSUITE_SUCCESS = 'MANAGEMENT/UPDATE_TESTSUITE_SUCCESS'
export const UPDATE_TESTSUITE_FAILURE = 'MANAGEMENT/UPDATE_TESTSUITE_FAILURE'

export const REMOVE_TESTSUITE_REQUEST = 'MANAGEMENT/REMOVE_TESTSUITE_REQUEST'
export const REMOVE_TESTSUITE_SUCCESS = 'MANAGEMENT/REMOVE_TESTSUITE_SUCCESS'
export const REMOVE_TESTSUITE_FAILURE = 'MANAGEMENT/REMOVE_TESTSUITE_FAILURE'

export const SET_SEARCHTEXT = 'MANAGEMENT/SET_SEARCHTEXT'

// define action types
interface GetTestSuitesActionRequestType { type: typeof GET_TESTSUITES_REQUEST }
interface GetTestSuitesActionSuccessType { type: typeof GET_TESTSUITES_SUCCESS, payload: TestSuite[] }
interface GetTestSuitesActionFailureType { type: typeof GET_TESTSUITES_FAILURE, errorMsg: string }

interface InstallTestSuiteActionRequestType { type: typeof INSTALL_TESTSUITE_REQUEST }
interface InstallTestSuiteActionSuccessType { type: typeof INSTALL_TESTSUITE_SUCCESS, payload: number }
interface InstallTestSuiteActionFailureType { type: typeof INSTALL_TESTSUITE_FAILURE, errorMsg: string }

interface UpdateTestSuiteActionRequestType { type: typeof UPDATE_TESTSUITE_REQUEST }
interface UpdateTestSuiteActionSuccessType { type: typeof UPDATE_TESTSUITE_SUCCESS }
interface UpdateTestSuiteActionFailureType { type: typeof UPDATE_TESTSUITE_FAILURE, errorMsg: string }

interface RemoveTestSuiteActionRequestType { type: typeof REMOVE_TESTSUITE_REQUEST }
interface RemoveTestSuiteActionSuccessType { type: typeof REMOVE_TESTSUITE_SUCCESS }
interface RemoveTestSuiteActionFailureType { type: typeof REMOVE_TESTSUITE_FAILURE, errorMsg: string }

interface SetSearchTextActionType { type: typeof SET_SEARCHTEXT, searchText: string }

export type ManagementActionTypes = GetTestSuitesActionRequestType
| GetTestSuitesActionSuccessType
| GetTestSuitesActionFailureType
| InstallTestSuiteActionRequestType
| InstallTestSuiteActionSuccessType
| InstallTestSuiteActionFailureType
| UpdateTestSuiteActionRequestType
| UpdateTestSuiteActionSuccessType
| UpdateTestSuiteActionFailureType
| RemoveTestSuiteActionRequestType
| RemoveTestSuiteActionSuccessType
| RemoveTestSuiteActionFailureType
| SetSearchTextActionType

// define actions
export const ManagementActions = {
  getTestSuitesAction_Request: (): ManagementActionTypes => {
    return {
      type: GET_TESTSUITES_REQUEST
    }
  },
  getTestSuitesAction_Success: (testSuites: TestSuite[]): ManagementActionTypes => {
    return {
      type: GET_TESTSUITES_SUCCESS,
      payload: testSuites
    }
  },
  getTestSuitesAction_Failure: (error: string): ManagementActionTypes => {
    return {
      type: GET_TESTSUITES_FAILURE,
      errorMsg: error
    }
  },
  installTestSuiteAction_Request: (): ManagementActionTypes => {
    return {
      type: INSTALL_TESTSUITE_REQUEST
    }
  },
  installTestSuiteAction_Success: (id: number): ManagementActionTypes => {
    return {
      type: INSTALL_TESTSUITE_SUCCESS,
      payload: id
    }
  },
  installTestSuiteAction_Failure: (error: string): ManagementActionTypes => {
    return {
      type: INSTALL_TESTSUITE_FAILURE,
      errorMsg: error
    }
  },
  updateTestSuiteAction_Request: (): ManagementActionTypes => {
    return {
      type: UPDATE_TESTSUITE_REQUEST
    }
  },
  updateTestSuiteAction_Success: (): ManagementActionTypes => {
    return {
      type: UPDATE_TESTSUITE_SUCCESS
    }
  },
  updateTestSuiteAction_Failure: (error: string): ManagementActionTypes => {
    return {
      type: UPDATE_TESTSUITE_FAILURE,
      errorMsg: error
    }
  },
  removeTestSuiteAction_Request: (): ManagementActionTypes => {
    return {
      type: REMOVE_TESTSUITE_REQUEST
    }
  },
  removeTestSuiteAction_Success: (): ManagementActionTypes => {
    return {
      type: REMOVE_TESTSUITE_SUCCESS
    }
  },
  removeTestSuiteAction_Failure: (error: string): ManagementActionTypes => {
    return {
      type: REMOVE_TESTSUITE_FAILURE,
      errorMsg: error
    }
  },
  setSearchTextAction: (filter: string): ManagementActionTypes => {
    return {
      type: SET_SEARCHTEXT,
      searchText: filter
    }
  }
}
