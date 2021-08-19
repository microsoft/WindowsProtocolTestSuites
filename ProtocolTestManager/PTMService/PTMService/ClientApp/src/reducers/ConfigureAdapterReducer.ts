// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GET_ADAPTERS_FAILURE, GET_ADAPTERS_REQUEST, GET_ADAPTERS_SUCCESS, ON_ADAPTERS_CHANGED, SET_ADAPTERS_FAILURE, SET_ADAPTERS_REQUEST, SET_ADAPTERS_SUCCESS, SET_ERROR_MESSAGE, TestSuiteAdapterActionTypes } from '../actions/ConfigureAdapterAction'
import { Adapter, AdapterKind, ChangedField } from '../model/Adapter'

export interface AdapterState {
  isLoading: boolean
  isPosting: boolean
  errorMsg?: string
  adapterList: Adapter[]
}

const initialAdapterState: AdapterState = {
  isLoading: false,
  isPosting: false,
  errorMsg: undefined,
  adapterList: []
}

export const getAdapterReducer = (state = initialAdapterState, action: TestSuiteAdapterActionTypes): AdapterState => {
  switch (action.type) {
    case GET_ADAPTERS_REQUEST:
      return {
        ...state,
        isLoading: true,
        errorMsg: undefined,
        adapterList: []
      }
    case GET_ADAPTERS_SUCCESS:
      return {
        ...state,
        isLoading: false,
        adapterList: action.payload
      }
    case GET_ADAPTERS_FAILURE:
      return {
        ...state,
        isLoading: false,
        adapterList: [],
        errorMsg: action.errorMsg
      }
    case ON_ADAPTERS_CHANGED:
      const changedAdapter = state.adapterList.find((adapter) => adapter.Name === action.changedEvent.Adapter)

      if (changedAdapter !== undefined) {
        switch (action.changedEvent.Field) {
          case ChangedField.AdapterType:
            changedAdapter.AdapterType = '' + action.changedEvent.NewValue
            break
          case ChangedField.AdapterKind:
            const adapterKindKey = action.changedEvent.NewValue! as keyof typeof AdapterKind
            changedAdapter.Kind = AdapterKind[adapterKindKey]
            break
          case ChangedField.ScriptDirectory:
            changedAdapter.ScriptDirectory = '' + action.changedEvent.NewValue
            break
          case ChangedField.ShellScriptDirectory:
            changedAdapter.ShellScriptDirectory = '' + action.changedEvent.NewValue
            break
        }
      }
      return {
        ...state
      }
    case SET_ERROR_MESSAGE:
      return {
        ...state,
        errorMsg: action.errorMsg
      }
    case SET_ADAPTERS_REQUEST:
      return {
        ...state,
        isPosting: true,
        errorMsg: undefined
      }
    case SET_ADAPTERS_SUCCESS:
      return {
        ...state,
        isPosting: false
      }
    case SET_ADAPTERS_FAILURE:
      return {
        ...state,
        isPosting: false,
        errorMsg: action.errorMsg
      }
    default:
      return state
  }
}
