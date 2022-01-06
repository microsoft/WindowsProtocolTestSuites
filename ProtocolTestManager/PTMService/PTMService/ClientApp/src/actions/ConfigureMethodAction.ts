// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export const SET_CONFIGURATION_METHOD = 'CONFIGUREMETHOD/SET_CONFIGURATION_METHOD'

export const SAVE_PROFILE_REQUEST = 'CONFIGUREMETHOD/SAVE_PROFILE_REQUEST'
export const SAVE_PROFILE_SUCCESS = 'CONFIGUREMETHOD/SAVE_PROFILE_SUCCESS'
export const SAVE_PROFILE_FAILURE = 'CONFIGUREMETHOD/SAVE_PROFILE_FAILURE'

export const IMPORT_PROFILE_REQUEST = 'CONFIGUREMETHOD/IMPORT_PROFILE_REQUEST'
export const IMPORT_PROFILE_SUCCESS = 'CONFIGUREMETHOD/IMPORT_PROFILE_SUCCESS'
export const IMPORT_PROFILE_FAILURE = 'CONFIGUREMETHOD/IMPORT_PROFILE_FAILURE'

// define action types
interface SetConfigurationMethodType { type: typeof SET_CONFIGURATION_METHOD, selectedMethod?: string }

// save profile actions
interface SaveProfileActionRequestType { type: typeof SAVE_PROFILE_REQUEST, testResultId: number, selectedTestCases?: string[] }
interface SaveProfileActionSuccessType { type: typeof SAVE_PROFILE_SUCCESS, payload: string }
interface SaveProfileActionFailureType { type: typeof SAVE_PROFILE_FAILURE, errorMsg: string }

// import profile actions
interface ImportProfileActionRequestType { type: typeof IMPORT_PROFILE_REQUEST }
interface ImportProfileActionSuccessType { type: typeof IMPORT_PROFILE_SUCCESS, payload: boolean }
interface ImportProfileActionFailureType { type: typeof IMPORT_PROFILE_FAILURE, errorMsg: string }

export type TestSuiteConfigureMethodActionTypes =
  SetConfigurationMethodType |
  SaveProfileActionRequestType |
  SaveProfileActionSuccessType |
  SaveProfileActionFailureType |
  ImportProfileActionRequestType |
  ImportProfileActionSuccessType |
  ImportProfileActionFailureType

// define actions
export const ConfigureMethodActions = {
  setConfigurationMethodAction: (key?: string): TestSuiteConfigureMethodActionTypes => {
    return {
      type: SET_CONFIGURATION_METHOD,
      selectedMethod: key
    }
  },
  saveProfileAction_Request: (testResultId: number, selectedTestCases?: string[]): TestSuiteConfigureMethodActionTypes => {
    return {
      type: SAVE_PROFILE_REQUEST,
      testResultId: testResultId,
      selectedTestCases: selectedTestCases
    }
  },
  saveProfileAction_Success: (fileLocation: string): TestSuiteConfigureMethodActionTypes => {
    return {
      type: SAVE_PROFILE_SUCCESS,
      payload: fileLocation
    }
  },
  saveProfileAction_Failure: (errorMsg: string): TestSuiteConfigureMethodActionTypes => {
    return {
      type: SAVE_PROFILE_FAILURE,
      errorMsg: errorMsg
    }
  },
  importProfileAction_Request: (): TestSuiteConfigureMethodActionTypes => {
    return {
      type: IMPORT_PROFILE_REQUEST
    }
  },
  importProfileAction_Success: (resp: boolean): TestSuiteConfigureMethodActionTypes => {
    return {
      type: IMPORT_PROFILE_SUCCESS,
      payload: resp
    }
  },
  importProfileAction_Failure: (err: string): TestSuiteConfigureMethodActionTypes => {
    return {
      type: IMPORT_PROFILE_FAILURE,
      errorMsg: err
    }
  }
}
