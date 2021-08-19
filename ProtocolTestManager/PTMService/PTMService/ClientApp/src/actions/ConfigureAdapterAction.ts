// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Adapter, AdapterChangedEvent } from '../model/Adapter'

export const GET_ADAPTERS_REQUEST = 'ADAPTER/GET_ADAPTERS_REQUEST'
export const GET_ADAPTERS_SUCCESS = 'ADAPTER/GET_ADAPTERS_SUCCESS'
export const GET_ADAPTERS_FAILURE = 'ADAPTER/GET_ADAPTERS_FAILURE'

export const SET_ADAPTERS_REQUEST = 'ADAPTER/SET_ADAPTERS_REQUEST'
export const SET_ADAPTERS_SUCCESS = 'ADAPTER/SET_ADAPTERS_SUCCESS'
export const SET_ADAPTERS_FAILURE = 'ADAPTER/SET_ADAPTERS_FAILURE'

export const ON_ADAPTERS_CHANGED = 'ADAPTER/ON_ADAPTERS_CHANGED'
export const SET_ERROR_MESSAGE = 'ADAPTER/SET_ERROR_MESSAGE'

// define action types
interface GetTSAdaptersActionRequestType { type: typeof GET_ADAPTERS_REQUEST, configureId: number }
interface GetTSAdaptersActionSuccessType { type: typeof GET_ADAPTERS_SUCCESS, payload: Adapter[] }
interface GetTSAdaptersActionFailureType { type: typeof GET_ADAPTERS_FAILURE, errorMsg: string }

interface SetTSAdaptersActionRequestType { type: typeof SET_ADAPTERS_REQUEST }
interface SetTSAdaptersActionSuccessType { type: typeof SET_ADAPTERS_SUCCESS }
interface SetTSAdaptersActionFailureType { type: typeof SET_ADAPTERS_FAILURE, errorMsg: string }

interface OnAdapterChangedActionType { type: typeof ON_ADAPTERS_CHANGED, changedEvent: AdapterChangedEvent }
interface SetErrorMessageActionType { type: typeof SET_ERROR_MESSAGE, errorMsg?: string }

export type TestSuiteAdapterActionTypes = GetTSAdaptersActionRequestType
| GetTSAdaptersActionSuccessType
| GetTSAdaptersActionFailureType
| SetTSAdaptersActionRequestType
| SetTSAdaptersActionSuccessType
| SetTSAdaptersActionFailureType
| OnAdapterChangedActionType
| SetErrorMessageActionType

// define actions
export const AdapterActions = {
  getAdapterAction_Request: (id: number): TestSuiteAdapterActionTypes => {
    return {
      type: GET_ADAPTERS_REQUEST,
      configureId: id
    }
  },
  getAdapterAction_Success: (adapters: Adapter[]): TestSuiteAdapterActionTypes => {
    return {
      type: GET_ADAPTERS_SUCCESS,
      payload: adapters
    }
  },
  getAdapterAction_Failure: (error: string): TestSuiteAdapterActionTypes => {
    return {
      type: GET_ADAPTERS_FAILURE,
      errorMsg: error
    }
  },
  setAdapterAction_Request: (): TestSuiteAdapterActionTypes => {
    return {
      type: SET_ADAPTERS_REQUEST
    }
  },
  setAdapterAction_Success: (): TestSuiteAdapterActionTypes => {
    return {
      type: SET_ADAPTERS_SUCCESS
    }
  },
  setAdapterAction_Failure: (error: string): TestSuiteAdapterActionTypes => {
    return {
      type: SET_ADAPTERS_FAILURE,
      errorMsg: error
    }
  },
  onAdapterChanged: (event: AdapterChangedEvent): TestSuiteAdapterActionTypes => {
    return {
      type: ON_ADAPTERS_CHANGED,
      changedEvent: event
    }
  },
  setErrorMessage: (errorMessage?: string): TestSuiteAdapterActionTypes => {
    return {
      type: SET_ERROR_MESSAGE,
      errorMsg: errorMessage
    }
  }
}
