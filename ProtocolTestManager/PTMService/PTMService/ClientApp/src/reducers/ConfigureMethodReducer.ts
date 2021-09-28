// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  SET_CONFIGURATION_METHOD,
  SAVE_PROFILE_REQUEST,
  SAVE_PROFILE_SUCCESS,
  SAVE_PROFILE_FAILURE,
  IMPORT_PROFILE_REQUEST,
  IMPORT_PROFILE_SUCCESS,
  IMPORT_PROFILE_FAILURE,
  TestSuiteConfigureMethodActionTypes
} from '../actions/ConfigureMethodAction'

export interface ConfigureMethodState {
  errorMsg?: string
  selectedMethod?: string
  isPosting: boolean
  isProfileImported: boolean
  isUploadingProfile: boolean
  profileLocation?: string
}

const initialConfigureMethodState: ConfigureMethodState = {
  errorMsg: undefined,
  selectedMethod: undefined,
  isPosting: false,
  isProfileImported: false,
  isUploadingProfile: false,
  profileLocation: undefined
}

export const getConfigureMethodReducer = (state = initialConfigureMethodState, action: TestSuiteConfigureMethodActionTypes): ConfigureMethodState => {
  switch (action.type) {
    case SET_CONFIGURATION_METHOD:
      return {
        ...state,
        errorMsg: undefined,
        selectedMethod: action.selectedMethod
      }

    case SAVE_PROFILE_REQUEST:
      return {
        ...state,
        isPosting: true,
        errorMsg: undefined
      }

    case SAVE_PROFILE_SUCCESS:
      return {
        ...state,
        isPosting: false,
        profileLocation: action.payload
      }

    case SAVE_PROFILE_FAILURE:
      return {
        ...state,
        isPosting: false,
        errorMsg: action.errorMsg
      }

    case IMPORT_PROFILE_REQUEST:
      return {
        ...state,
        isPosting: true,
        isUploadingProfile: true,
        isProfileImported: false,
        errorMsg: undefined
      }

    case IMPORT_PROFILE_SUCCESS:
      return {
        ...state,
        isPosting: false,
        isUploadingProfile: false,
        isProfileImported: action.payload
      }

    case IMPORT_PROFILE_FAILURE:
      return {
        ...state,
        isPosting: false,
        isUploadingProfile: false,
        isProfileImported: false,
        errorMsg: action.errorMsg
      }

    default:
      return state
  }
}
