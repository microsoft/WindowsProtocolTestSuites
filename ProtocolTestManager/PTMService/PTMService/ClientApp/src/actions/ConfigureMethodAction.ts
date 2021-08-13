// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export const SET_CONFIGUREMETHOD_REQUEST = 'CONFIGUREMETHOD/SET_CONFIGUREMETHOD_REQUEST'

export const SAVE_PROFILE_REQUEST = 'TESTRESULTS/SAVE_PROFILE_REQUEST'
export const SAVE_PROFILE_SUCCESS = 'TESTRESULTS/SAVE_PROFILE_SUCCESS'
export const SAVE_PROFILE_FAILURE = 'TESTRESULTS/SAVE_PROFILE_FAILURE'

export const IMPORT_PROFILE_REQUEST = 'TESTRESULTS/IMPORT_PROFILE_REQUEST'
export const IMPORT_PROFILE_SUCCESS = 'TESTRESULTS/IMPORT_PROFILE_SUCCESS'
export const IMPORT_PROFILE_FAILURE = 'TESTRESULTS/IMPORT_PROFILE_FAILURE'

// define action types
interface SetTSConfigureMethodType { type: typeof SET_CONFIGUREMETHOD_REQUEST; selectedMethod: string; }

// save profile actions
interface SetSaveProfileActionRequestType { type: typeof SAVE_PROFILE_REQUEST; payload: number; }
interface SetSaveProfileActionSuccessType { type: typeof SAVE_PROFILE_SUCCESS; payload: string; }
interface SetSaveProfileActionFailureType { type: typeof SAVE_PROFILE_FAILURE; errorMsg: string; }

// import profile actions
interface ImportProfileActionRequestType { type: typeof IMPORT_PROFILE_REQUEST; filename: string; testSuiteId: number; }
interface ImportProfileActionSuccessType { type: typeof IMPORT_PROFILE_SUCCESS; payload: boolean; }
interface ImportProfileActionFailureType { type: typeof IMPORT_PROFILE_FAILURE; errorMsg: string; }

export type TestSuiteConfigureMethodActionTypes =
    SetTSConfigureMethodType |
    SetSaveProfileActionRequestType |
    SetSaveProfileActionSuccessType |
    SetSaveProfileActionFailureType |
    ImportProfileActionRequestType |
    ImportProfileActionSuccessType |
    ImportProfileActionFailureType;

// define actions
export const ConfigureMethodActions = {
  setConfigureMethod: (key: string): TestSuiteConfigureMethodActionTypes => {
    return {
      type: SET_CONFIGUREMETHOD_REQUEST,
      selectedMethod: key
    }
  },
  saveProfileAction_Request: (testsuiteId: number): TestSuiteConfigureMethodActionTypes => {
    return {
      type: SAVE_PROFILE_REQUEST,
      payload: testsuiteId
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
  importProfileAction_Request: (filename: string, testSuiteId: number): TestSuiteConfigureMethodActionTypes => {
    return {
      type: IMPORT_PROFILE_REQUEST,
      filename: filename,
      testSuiteId: testSuiteId
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
