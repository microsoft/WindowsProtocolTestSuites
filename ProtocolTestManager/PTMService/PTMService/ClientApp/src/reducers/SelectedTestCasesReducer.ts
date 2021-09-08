// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  CREATE_RUNREQUEST_REQUEST,
  CREATE_RUNREQUEST_SUCCESS,
  CREATE_RUNREQUEST_FAILURE,
  SelectedTestCasesActionTypes
} from '../actions/SelectedTestCasesAction'

export interface SelectedTestCasesState {
  isPosting: boolean
  errorMsg?: string
  testResultId: number | undefined
}

const initialSelectedTestCasesState: SelectedTestCasesState = {
  isPosting: false,
  errorMsg: undefined,
  testResultId: undefined
}

export const getSelectedTestCasesReducer = (state = initialSelectedTestCasesState, action: SelectedTestCasesActionTypes): SelectedTestCasesState => {
  switch (action.type) {
    case CREATE_RUNREQUEST_REQUEST:
      return {
        ...state,
        isPosting: true,
        errorMsg: undefined,
        testResultId: undefined
      }

    case CREATE_RUNREQUEST_SUCCESS:
      return {
        ...state,
        isPosting: false,
        testResultId: action.payload
      }

    case CREATE_RUNREQUEST_FAILURE:
      return {
        ...state,
        isPosting: false,
        errorMsg: action.errorMsg,
        testResultId: undefined
      }

    default:
      return state
  }
}
