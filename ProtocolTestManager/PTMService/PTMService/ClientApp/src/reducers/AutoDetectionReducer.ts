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
  START_AUTO_DETECTION_SUCCESS,
  START_AUTO_DETECTION_FAILURE,
  STOP_AUTO_DETECTION_REQUEST,
  STOP_AUTO_DETECTION_SUCCESS,
  STOP_AUTO_DETECTION_FAILURE,
  APPLY_AUTO_DETECTION_RESULT_REQUEST,
  APPLY_AUTO_DETECTION_RESULT_FAILURE,
  UPDATE_AUTO_DETECTION_PREREQUISITE,
  GET_AUTO_DETECTION_LOG_REQUEST,
  GET_AUTO_DETECTION_LOG_FAILURE,
  SET_AUTO_DETECTION_LOG,
  APPEND_AUTO_DETECTION_LOG_CHUNK,
  RESET_AUTO_DETECTION_LOG_STREAM,
  TestSuiteAutoDetectionActionTypes
} from '../actions/AutoDetectionAction'
import { Prerequisite, DetectionSteps, DetectionStatus } from '../model/AutoDetectionData'
import { createAsyncHandlers } from '../utils/AsyncReducerFactory'

export interface AutoDetectionState {
  isPrerequisiteLoading: boolean
  isDetectionStepsLoading: boolean
  errorMsg?: string
  log: string
  logRunId?: string
  logOffset: number
  isLogStreamComplete: boolean
  prerequisite?: Prerequisite
  detectionSteps?: DetectionSteps
  detecting: boolean
  canceling: boolean
}

const initialAutoDetectionState: AutoDetectionState = {
  isPrerequisiteLoading: false,
  isDetectionStepsLoading: false,
  errorMsg: undefined,
  log: '',
  logRunId: undefined,
  logOffset: 0,
  isLogStreamComplete: false,
  prerequisite: undefined,
  detectionSteps: undefined,
  detecting: false,
  canceling: false
}

// Reducer handler factories for common async patterns
const prerequisiteHandlers = createAsyncHandlers<AutoDetectionState, 'isPrerequisiteLoading', 'prerequisite', 'errorMsg'>('isPrerequisiteLoading', 'prerequisite', 'errorMsg')
const stepsHandlers = createAsyncHandlers<AutoDetectionState, 'isDetectionStepsLoading', 'detectionSteps', 'errorMsg'>('isDetectionStepsLoading', 'detectionSteps', 'errorMsg')

export const getAutoDetectionReducer = (state = initialAutoDetectionState, action: TestSuiteAutoDetectionActionTypes): AutoDetectionState => {
  switch (action.type) {
    // Prerequisite handlers
    case GET_AUTO_DETECTION_PREREQUISITE_REQUEST:
      return prerequisiteHandlers.request(state)
    case GET_AUTO_DETECTION_PREREQUISITE_SUCCESS:
      return prerequisiteHandlers.success(state, action.payload)
    case GET_AUTO_DETECTION_PREREQUISITE_FAILURE:
      return prerequisiteHandlers.failure(state, action.errorMsg)

    case UPDATE_AUTO_DETECTION_PREREQUISITE:
      if (state.prerequisite !== undefined) {
        const updatedProperties = state.prerequisite.Properties.map((item) =>
          item.Name === action.payload.Name ? { ...item, Value: action.payload.Value } : item
        )
        return {
          ...state,
          prerequisite: { ...state.prerequisite, Properties: updatedProperties }
        }
      }
      return state

    // Start/Stop detection handlers
    case START_AUTO_DETECTION_REQUEST:
      return { ...state, isDetectionStepsLoading: true, errorMsg: undefined }
    case START_AUTO_DETECTION_SUCCESS:
      return { ...state, isDetectionStepsLoading: false, errorMsg: undefined }
    case START_AUTO_DETECTION_FAILURE:
      return { ...state, isDetectionStepsLoading: false, errorMsg: action.errorMsg }

    case STOP_AUTO_DETECTION_REQUEST:
      return { ...state, isDetectionStepsLoading: true, canceling: true, errorMsg: undefined }
    case STOP_AUTO_DETECTION_SUCCESS:
      return { ...state, isDetectionStepsLoading: false, canceling: true, errorMsg: undefined }
    case STOP_AUTO_DETECTION_FAILURE:
      return { ...state, isDetectionStepsLoading: false, canceling: false, errorMsg: action.errorMsg }

    // Result/Log request handlers
    case APPLY_AUTO_DETECTION_RESULT_REQUEST:
    case GET_AUTO_DETECTION_LOG_REQUEST:
      return { ...state, errorMsg: undefined }

    case APPLY_AUTO_DETECTION_RESULT_FAILURE:
    case GET_AUTO_DETECTION_LOG_FAILURE:
      return { ...state, errorMsg: action.errorMsg }

    // Log content handlers
    case SET_AUTO_DETECTION_LOG:
      return { ...state, log: action.payload }

    case APPEND_AUTO_DETECTION_LOG_CHUNK:
      if ((state.logRunId !== undefined && action.payload.RunId !== state.logRunId) ||
          action.payload.Offset !== state.logOffset) {
        return state
      }
      return {
        ...state,
        log: `${state.log}${action.payload.Content}`,
        logRunId: action.payload.RunId,
        logOffset: action.payload.NextOffset,
        isLogStreamComplete: action.payload.IsComplete
      }

    case RESET_AUTO_DETECTION_LOG_STREAM:
      return {
        ...state,
        log: '',
        logRunId: action.payload,
        logOffset: 0,
        isLogStreamComplete: false
      }

    // Get/Update steps handlers
    case GET_AUTO_DETECTION_STEPS_REQUEST:
      return { ...state, isDetectionStepsLoading: true, errorMsg: undefined }

    case GET_AUTO_DETECTION_STEPS_SUCCESS:
      return {
        ...state,
        isDetectionStepsLoading: false,
        errorMsg: undefined,
        detectionSteps: action.payload,
        detecting: action.payload.Result.Status === DetectionStatus.InProgress,
        canceling: state.canceling ? action.payload.Result.Status === DetectionStatus.InProgress : state.canceling
      }

    case GET_AUTO_DETECTION_STEPS_FAILURE:
      return { ...state, isDetectionStepsLoading: false, errorMsg: action.errorMsg }

    case UPDATE_AUTO_DETECTION_STEPS_REQUEST:
      return { ...state, errorMsg: undefined }

    case UPDATE_AUTO_DETECTION_STEPS_SUCCESS:
      return {
        ...state,
        errorMsg: undefined,
        detectionSteps: action.payload,
        detecting: action.payload.Result.Status === DetectionStatus.InProgress,
        canceling: state.canceling ? action.payload.Result.Status === DetectionStatus.InProgress : state.canceling
      }

    case UPDATE_AUTO_DETECTION_STEPS_FAILURE:
      return { ...state, errorMsg: action.errorMsg }

    default:
      return state
  }
}
