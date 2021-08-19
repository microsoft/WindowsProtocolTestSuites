// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GET_DETECTION_STEPS_REQUEST, GET_DETECTION_STEPS_SUCCESS, GET_PREREQUISITE_REQUEST, GET_PREREQUISITE_SUCCESS, START_POLLING, START_POLLING_Failure, START_POLLING_Success, STOP_POLLING, TestSuiteAutoDetectionActionTypes, UPDATE_PREREQUISITE } from '../actions/AutoDetectionAction'
import { Prerequisite, DetectionSteps, PrerequisiteProperty } from '../model/AutoDetectionData'

export interface AutoDetectState {
  isPrerequisiteLoading: boolean;
  isDetectionStepsLoading: boolean;
  errorMsg?: string;
  prerequisite?: Prerequisite;
  detectionSteps?: DetectionSteps;
  isPolling: boolean;
}

const initialAutoDetectState: AutoDetectState = {
  isPrerequisiteLoading: false,
  isDetectionStepsLoading: false,
  errorMsg: undefined,
  prerequisite: undefined,
  detectionSteps: undefined,
  isPolling: false
}

export const getAutoDetectReducer = (state = initialAutoDetectState, action: TestSuiteAutoDetectionActionTypes): AutoDetectState => {
  switch (action.type) {
    case GET_PREREQUISITE_REQUEST:
      return {
        ...state,
        isPrerequisiteLoading: true,
        errorMsg: undefined,
        prerequisite: undefined
      }
    case GET_PREREQUISITE_SUCCESS:
      action.payload.Properties.map((p: PrerequisiteProperty) => {
        if (p.Value === undefined && p.Choices && p.Choices.length > 0) {
          p.Value = p.Choices[0]
        }
        return p
      })

      return {
        ...state,
        isPrerequisiteLoading: false,
        errorMsg: undefined,
        prerequisite: action.payload
      }
    case UPDATE_PREREQUISITE:
      const updatedItems = state.prerequisite?.Properties.map((item) => {
        if (item.Name === action.payload.Name) {
          item.Value = action.payload.Value
          return item
        }
      })
      return {
        ...state,
        isPrerequisiteLoading: false
      }

    case GET_DETECTION_STEPS_REQUEST:
      return {
        ...state,
        isDetectionStepsLoading: true,
        errorMsg: undefined
      }

    case GET_DETECTION_STEPS_SUCCESS:
      return {
        ...state,
        isDetectionStepsLoading: false,
        errorMsg: undefined,
        detectionSteps: action.payload
      }

    case START_POLLING:
      return {
        ...state,
        isDetectionStepsLoading: false,
        errorMsg: undefined
      }

    case START_POLLING_Success:
      return {
        ...state,
        isDetectionStepsLoading: false,
        errorMsg: undefined,
        detectionSteps: action.payload
      }

    case START_POLLING_Failure:
      return {
        ...state,
        errorMsg: action.errorMsg
      }

    case START_POLLING:
      return { ...state, isPolling: true }
    case STOP_POLLING:
      return { ...state, isPolling: false }
    default:
      return state
  }
}
