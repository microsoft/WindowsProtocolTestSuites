// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// define action consts
export const CREATE_RUNREQUEST_REQUEST = 'SELECTEDTESTCASES/CREATE_RUNREQUEST_REQUEST'
export const CREATE_RUNREQUEST_SUCCESS = 'SELECTEDTESTCASES/CREATE_RUNREQUEST_SUCCESS'
export const CREATE_RUNREQUEST_FAILURE = 'SELECTEDTESTCASES/CREATE_RUNREQUEST_FAILURE'

export const ABORT_RUNREQUEST_REQUEST = 'SELECTEDTESTCASES/ABORT_RUNREQUEST_REQUEST'
export const ABORT_RUNREQUEST_SUCCESS = 'SELECTEDTESTCASES/ABORT_RUNREQUEST_SUCCESS'
export const ABORT_RUNREQUEST_FAILURE = 'SELECTEDTESTCASES/ABORT_RUNREQUEST_FAILURE'

export const REMOVE_RUNREQUEST_REQUEST = 'SELECTEDTESTCASES/REMOVE_RUNREQUEST_REQUEST'
export const REMOVE_RUNREQUEST_SUCCESS = 'SELECTEDTESTCASES/REMOVE_RUNREQUEST_SUCCESS'
export const REMOVE_RUNREQUEST_FAILURE = 'SELECTEDTESTCASES/REMOVE_RUNREQUEST_FAILURE'

// define action types
interface CreateRunRequestActionRequestType { type: typeof CREATE_RUNREQUEST_REQUEST; }
interface CreateRunRequestActionSuccessType { type: typeof CREATE_RUNREQUEST_SUCCESS; payload: number; }
interface CreateRunRequestActionFailureType { type: typeof CREATE_RUNREQUEST_FAILURE; errorMsg: string; }

interface AbortRunRequestActionRequestType { type: typeof ABORT_RUNREQUEST_REQUEST, payload: number }
interface AbortRunRequestActionSuccessType { type: typeof ABORT_RUNREQUEST_SUCCESS }
interface AbortRunRequestActionFailureType { type: typeof ABORT_RUNREQUEST_FAILURE, errorMsg: string }

interface RemoveRunRequestActionRequestType { type: typeof REMOVE_RUNREQUEST_REQUEST, payload: number }
interface RemoveRunRequestActionSuccessType { type: typeof REMOVE_RUNREQUEST_SUCCESS }
interface RemoveRunRequestActionFailureType { type: typeof REMOVE_RUNREQUEST_FAILURE, errorMsg: string }

export type SelectedTestCasesActionTypes =
  CreateRunRequestActionRequestType |
  CreateRunRequestActionSuccessType |
  CreateRunRequestActionFailureType |
  AbortRunRequestActionRequestType |
  AbortRunRequestActionSuccessType |
  AbortRunRequestActionFailureType |
  RemoveRunRequestActionRequestType |
  RemoveRunRequestActionSuccessType |
  RemoveRunRequestActionFailureType;  

// define actions
export const SelectedTestCasesActions = {
  createRunRequestAction_Request: (): SelectedTestCasesActionTypes => {
    return {
      type: CREATE_RUNREQUEST_REQUEST
    }
  },
  createRunRequestAction_Success: (testResultId: number): SelectedTestCasesActionTypes => {
    return {
      type: CREATE_RUNREQUEST_SUCCESS,
      payload: testResultId
    }
  },
  createRunRequestAction_Failure: (error: string): SelectedTestCasesActionTypes => {
    return {
      type: CREATE_RUNREQUEST_FAILURE,
      errorMsg: error
    }
  },
  abortRunRequestAction_Request: (testResultId: number): SelectedTestCasesActionTypes => {
    return {
      type: ABORT_RUNREQUEST_REQUEST,
      payload: testResultId
    }
  },
  abortRunRequestAction_Success: (): SelectedTestCasesActionTypes => {
    return {
      type: ABORT_RUNREQUEST_SUCCESS
    }
  },
  abortRunRequestAction_Failure: (error: string): SelectedTestCasesActionTypes => {
    return {
      type: ABORT_RUNREQUEST_FAILURE,
      errorMsg: error
    }
  },
  removeRunRequestAction_Request: (testResultId: number): SelectedTestCasesActionTypes => {
    return {
      type: REMOVE_RUNREQUEST_REQUEST,
      payload: testResultId
    }
  },
  removeRunRequestAction_Success: (): SelectedTestCasesActionTypes => {
    return {
      type: REMOVE_RUNREQUEST_SUCCESS
    }
  },
  removeRunRequestAction_Failure: (error: string): SelectedTestCasesActionTypes => {
    return {
      type: REMOVE_RUNREQUEST_FAILURE,
      errorMsg: error
    }
  }
}
