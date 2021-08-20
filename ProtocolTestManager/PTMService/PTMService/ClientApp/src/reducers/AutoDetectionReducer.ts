// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  GET_AUTO_DETECTION_PREREQUISITE_REQUEST,
  GET_AUTO_DETECTION_PREREQUISITE_SUCCESS,
  GET_AUTO_DETECTION_PREREQUISITE_FAILURE,
  GET_AUTO_DETECTION_STEPS_REQUEST,
  GET_AUTO_DETECTION_STEPS_SUCCESS,
  GET_AUTO_DETECTION_STEPS_FAILURE,
  UPDATE_AUTO_DETECTION_STEPS_REQUEST,
  UPDATE_AUTO_DETECTION_STEPS_SUCCESS,
  UPDATE_AUTO_DETECTION_STEPS_FAILURE,
  START_AUTO_DETECTION_REQUEST,
  START_AUTO_DETECTION_FAILURE,
  START_AUTO_DETECTION_SUCCESS,
  STOP_AUTO_DETECTION_REQUEST,
  STOP_AUTO_DETECTION_FAILURE,
  STOP_AUTO_DETECTION_SUCCESS,
  APPLY_AUTO_DETECTION_RESULT_REQUEST,
  APPLY_AUTO_DETECTION_RESULT_FAILURE,
  UPDATE_AUTO_DETECTION_PREREQUISITE,
  GET_AUTO_DETECTION_LOG_REQUEST,
  GET_AUTO_DETECTION_LOG_FAILURE,
  SET_AUTO_DETECTION_LOG,
  SET_SHOW_WARNING,
  TestSuiteAutoDetectionActionTypes
} from '../actions/AutoDetectionAction'
import { Prerequisite, DetectionSteps } from '../model/AutoDetectionData'

export interface AutoDetectionState {
  isPrerequisiteLoading: boolean
  isDetectionStepsLoading: boolean
  errorMsg?: string
  showWarning: boolean
  log?: string
  prerequisite?: Prerequisite
  detectionSteps?: DetectionSteps
}

const initialAutoDetectionState: AutoDetectionState = {
  isPrerequisiteLoading: false,
  isDetectionStepsLoading: false,
  errorMsg: undefined,
  showWarning: false,
  log: undefined,
  prerequisite: undefined,
  detectionSteps: undefined
}

export const getAutoDetectionReducer = (state = initialAutoDetectionState, action: TestSuiteAutoDetectionActionTypes): AutoDetectionState => {
  switch (action.type) {
    case GET_AUTO_DETECTION_PREREQUISITE_REQUEST:
      return {
        ...state,
        isPrerequisiteLoading: true,
        errorMsg: undefined,
        prerequisite: undefined
      }

    case GET_AUTO_DETECTION_PREREQUISITE_SUCCESS:
      return {
        ...state,
        isPrerequisiteLoading: false,
        errorMsg: undefined,
        prerequisite: action.payload
      }

    case GET_AUTO_DETECTION_PREREQUISITE_FAILURE:
      return {
        ...state,
        isPrerequisiteLoading: false,
        errorMsg: action.errorMsg
      }

    case UPDATE_AUTO_DETECTION_PREREQUISITE:
      if (state.prerequisite !== undefined) {
        const updatedProperties = state.prerequisite.Properties.map((item) => {
          if (item.Name === action.payload.Name) {
            return { ...item, Value: action.payload.Value }
          } else {
            return item
          }
        })
        return {
          ...state,
          prerequisite: { ...state.prerequisite, Properties: updatedProperties }
        }
      } else {
        return state
      }

    case START_AUTO_DETECTION_REQUEST:
    case STOP_AUTO_DETECTION_REQUEST:
      return {
        ...state,
        isDetectionStepsLoading: true,
        errorMsg: undefined
      }

    case START_AUTO_DETECTION_SUCCESS:
    case STOP_AUTO_DETECTION_SUCCESS:
      return {
        ...state,
        isDetectionStepsLoading: false,
        errorMsg: undefined
      }

    case START_AUTO_DETECTION_FAILURE:
    case STOP_AUTO_DETECTION_FAILURE:
      return {
        ...state,
        isDetectionStepsLoading: false,
        errorMsg: action.errorMsg
      }

    case APPLY_AUTO_DETECTION_RESULT_REQUEST:
    case GET_AUTO_DETECTION_LOG_REQUEST:
      return {
        ...state,
        errorMsg: undefined
      }

    case APPLY_AUTO_DETECTION_RESULT_FAILURE:
    case GET_AUTO_DETECTION_LOG_FAILURE:
      return {
        ...state,
        errorMsg: action.errorMsg
      }

    case SET_AUTO_DETECTION_LOG:
      return {
        ...state,
        log: action.payload
      }

    case SET_SHOW_WARNING:
      return {
        ...state,
        showWarning: action.payload
      }

    case GET_AUTO_DETECTION_STEPS_REQUEST:
      return {
        ...state,
        isDetectionStepsLoading: true,
        errorMsg: undefined,
        showWarning: false
      }

    case GET_AUTO_DETECTION_STEPS_SUCCESS:
      return {
        ...state,
        isDetectionStepsLoading: false,
        errorMsg: undefined,
        detectionSteps: action.payload
      }

    case GET_AUTO_DETECTION_STEPS_FAILURE:
      return {
        ...state,
        isDetectionStepsLoading: false,
        errorMsg: action.errorMsg
      }

    case UPDATE_AUTO_DETECTION_STEPS_REQUEST:
      return {
        ...state,
        errorMsg: undefined,
        showWarning: true
      }

    case UPDATE_AUTO_DETECTION_STEPS_SUCCESS:
      return {
        ...state,
        errorMsg: undefined,
        detectionSteps: action.payload
      }

    case UPDATE_AUTO_DETECTION_STEPS_FAILURE:
      return {
        ...state,
        errorMsg: action.errorMsg
      }

    default:
      return state
  }
}
