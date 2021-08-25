// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Prerequisite, DetectionSteps, DetectionResultResponse } from '../model/AutoDetectionData'
import { Property } from '../model/Property'

export const GET_AUTO_DETECTION_PREREQUISITE_REQUEST = 'AUTO_DETECTION/GET_AUTO_DETECTION_PREREQUISITE_REQUEST'
export const GET_AUTO_DETECTION_PREREQUISITE_SUCCESS = 'AUTO_DETECTION/GET_AUTO_DETECTION_PREREQUISITE_SUCCESS'
export const GET_AUTO_DETECTION_PREREQUISITE_FAILURE = 'AUTO_DETECTION/GET_AUTO_DETECTION_PREREQUISITE_FAILURE'

export const GET_AUTO_DETECTION_STEPS_REQUEST = 'AUTO_DETECTION/GET_AUTO_DETECTION_STEPS_REQUEST'
export const GET_AUTO_DETECTION_STEPS_SUCCESS = 'AUTO_DETECTION/GET_AUTO_DETECTION_STEPS_SUCCESS'
export const GET_AUTO_DETECTION_STEPS_FAILURE = 'AUTO_DETECTION/GET_AUTO_DETECTION_STEPS_FAILURE'

export const UPDATE_AUTO_DETECTION_STEPS_REQUEST = 'AUTO_DETECTION/UPDATE_AUTO_DETECTION_STEPS_REQUEST'
export const UPDATE_AUTO_DETECTION_STEPS_SUCCESS = 'AUTO_DETECTION/UPDATE_AUTO_DETECTION_STEPS_SUCCESS'
export const UPDATE_AUTO_DETECTION_STEPS_FAILURE = 'AUTO_DETECTION/UPDATE_AUTO_DETECTION_STEPS_FAILURE'

export const START_AUTO_DETECTION_REQUEST = 'AUTO_DETECTION/START_AUTO_DETECTION_REQUEST'
export const START_AUTO_DETECTION_SUCCESS = 'AUTO_DETECTION/START_AUTO_DETECTION_SUCCESS'
export const START_AUTO_DETECTION_FAILURE = 'AUTO_DETECTION/START_AUTO_DETECTION_FAILURE'

export const STOP_AUTO_DETECTION_REQUEST = 'AUTO_DETECTION/STOP_AUTO_DETECTION_REQUEST'
export const STOP_AUTO_DETECTION_SUCCESS = 'AUTO_DETECTION/STOP_AUTO_DETECTION_SUCCESS'
export const STOP_AUTO_DETECTION_FAILURE = 'AUTO_DETECTION/STOP_AUTO_DETECTION_FAILURE'

export const APPLY_AUTO_DETECTION_RESULT_REQUEST = 'AUTO_DETECTION/APPLY_AUTO_DETECTION_RESULT_REQUEST'
export const APPLY_AUTO_DETECTION_RESULT_SUCCESS = 'AUTO_DETECTION/APPLY_AUTO_DETECTION_RESULT_SUCCESS'
export const APPLY_AUTO_DETECTION_RESULT_FAILURE = 'AUTO_DETECTION/APPLY_AUTO_DETECTION_RESULT_FAILURE'

export const UPDATE_AUTO_DETECTION_PREREQUISITE = 'AUTO_DETECTION/UPDATE_AUTO_DETECTION_PREREQUISITE'

export const GET_AUTO_DETECTION_LOG_REQUEST = 'AUTO_DETECTION/GET_AUTO_DETECTION_LOG_REQUEST'
export const GET_AUTO_DETECTION_LOG_SUCCESS = 'AUTO_DETECTION/GET_AUTO_DETECTION_LOG_SUCCESS'
export const GET_AUTO_DETECTION_LOG_FAILURE = 'AUTO_DETECTION/GET_AUTO_DETECTION_LOG_FAILURE'

export const SET_AUTO_DETECTION_LOG = 'AUTO_DETECTION/SET_AUTO_DETECTION_LOG'

// define action types
interface GetAutoDetectionPrerequisiteActionRequestType { type: typeof GET_AUTO_DETECTION_PREREQUISITE_REQUEST }
interface GetAutoDetectionPrerequisiteActionSuccessType { type: typeof GET_AUTO_DETECTION_PREREQUISITE_SUCCESS, payload: Prerequisite }
interface GetAutoDetectionPrerequisiteActionFailureType { type: typeof GET_AUTO_DETECTION_PREREQUISITE_FAILURE, errorMsg: string }

interface GetAutoDetectionStepsActionRequestType { type: typeof GET_AUTO_DETECTION_STEPS_REQUEST }
interface GetAutoDetectionStepsActionSuccessType { type: typeof GET_AUTO_DETECTION_STEPS_SUCCESS, payload: DetectionSteps }
interface GetAutoDetectionStepsActionFailureType { type: typeof GET_AUTO_DETECTION_STEPS_FAILURE, errorMsg: string }

interface UpdateAutoDetectionStepsActionRequestType { type: typeof UPDATE_AUTO_DETECTION_STEPS_REQUEST }
interface UpdateAutoDetectionStepsActionSuccessType { type: typeof UPDATE_AUTO_DETECTION_STEPS_SUCCESS, payload: DetectionSteps }
interface UpdateAutoDetectionStepsActionFailureType { type: typeof UPDATE_AUTO_DETECTION_STEPS_FAILURE, errorMsg: string }

interface StartAutoDetectionActionRequestType { type: typeof START_AUTO_DETECTION_REQUEST }
interface StartAutoDetectionActionSuccessType { type: typeof START_AUTO_DETECTION_SUCCESS }
interface StartAutoDetectionActionFailureType { type: typeof START_AUTO_DETECTION_FAILURE, errorMsg: string }

interface StopAutoDetectionActionRequestType { type: typeof STOP_AUTO_DETECTION_REQUEST }
interface StopAutoDetectionActionSuccessType { type: typeof STOP_AUTO_DETECTION_SUCCESS }
interface StopAutoDetectionActionFailureType { type: typeof STOP_AUTO_DETECTION_FAILURE, errorMsg: string }

interface ApplyAutoDetectionResultActionRequestType { type: typeof APPLY_AUTO_DETECTION_RESULT_REQUEST }
interface ApplyAutoDetectionResultActionSuccessType { type: typeof APPLY_AUTO_DETECTION_RESULT_SUCCESS }
interface ApplyAutoDetectionResultActionFailureType { type: typeof APPLY_AUTO_DETECTION_RESULT_FAILURE, errorMsg: string }

interface UpdateAutoDetectionPrerequisiteActionType { type: typeof UPDATE_AUTO_DETECTION_PREREQUISITE, payload: Property };

interface GetAutoDetectionLogActionRequestType { type: typeof GET_AUTO_DETECTION_LOG_REQUEST };
interface GetAutoDetectionLogActionSuccessType { type: typeof GET_AUTO_DETECTION_LOG_SUCCESS, payload: Blob };
interface GetAutoDetectionLogActionFailureType { type: typeof GET_AUTO_DETECTION_LOG_FAILURE, errorMsg: string };

interface SetAutoDetectionLogActionType { type: typeof SET_AUTO_DETECTION_LOG, payload: string }

export type TestSuiteAutoDetectionActionTypes = GetAutoDetectionPrerequisiteActionRequestType
  | GetAutoDetectionPrerequisiteActionSuccessType
  | GetAutoDetectionPrerequisiteActionFailureType
  | GetAutoDetectionStepsActionRequestType
  | GetAutoDetectionStepsActionSuccessType
  | GetAutoDetectionStepsActionFailureType
  | UpdateAutoDetectionStepsActionRequestType
  | UpdateAutoDetectionStepsActionSuccessType
  | UpdateAutoDetectionStepsActionFailureType
  | StartAutoDetectionActionRequestType
  | StartAutoDetectionActionSuccessType
  | StartAutoDetectionActionFailureType
  | StopAutoDetectionActionRequestType
  | StopAutoDetectionActionSuccessType
  | StopAutoDetectionActionFailureType
  | ApplyAutoDetectionResultActionRequestType
  | ApplyAutoDetectionResultActionSuccessType
  | ApplyAutoDetectionResultActionFailureType
  | UpdateAutoDetectionPrerequisiteActionType
  | GetAutoDetectionLogActionRequestType
  | GetAutoDetectionLogActionSuccessType
  | GetAutoDetectionLogActionFailureType
  | SetAutoDetectionLogActionType

// define actions
export const AutoDetectionActions = {
  getAutoDetectionPrerequisiteAction_Request: (): TestSuiteAutoDetectionActionTypes => {
    return {
      type: GET_AUTO_DETECTION_PREREQUISITE_REQUEST
    }
  },
  getAutoDetectionPrerequisiteAction_Success: (data: Prerequisite): TestSuiteAutoDetectionActionTypes => {
    return {
      type: GET_AUTO_DETECTION_PREREQUISITE_SUCCESS,
      payload: data
    }
  },
  getAutoDetectionPrerequisiteAction_Failure: (error: string): TestSuiteAutoDetectionActionTypes => {
    return {
      type: GET_AUTO_DETECTION_PREREQUISITE_FAILURE,
      errorMsg: error
    }
  },
  getAutoDetectionStepsAction_Request: (): TestSuiteAutoDetectionActionTypes => {
    return {
      type: GET_AUTO_DETECTION_STEPS_REQUEST
    }
  },
  getAutoDetectionStepsAction_Success: (data: DetectionResultResponse): TestSuiteAutoDetectionActionTypes => {
    return {
      type: GET_AUTO_DETECTION_STEPS_SUCCESS,
      payload: {
        ...{} as DetectionSteps,
        DetectingItems: data.DetectionSteps.map(e => { return { Name: e.DetectingContent, Status: e.DetectingStatus } }),
        Result: data.Result
      }
    }
  },
  getAutoDetectionStepsAction_Failure: (error: string): TestSuiteAutoDetectionActionTypes => {
    return {
      type: GET_AUTO_DETECTION_STEPS_FAILURE,
      errorMsg: error
    }
  },
  updateAutoDetectionStepsAction_Request: (): TestSuiteAutoDetectionActionTypes => {
    return {
      type: UPDATE_AUTO_DETECTION_STEPS_REQUEST
    }
  },
  updateAutoDetectionStepsAction_Success: (data: DetectionResultResponse): TestSuiteAutoDetectionActionTypes => {
    return {
      type: UPDATE_AUTO_DETECTION_STEPS_SUCCESS,
      payload: {
        ...{} as DetectionSteps,
        DetectingItems: data.DetectionSteps.map(e => { return { Name: e.DetectingContent, Status: e.DetectingStatus } }),
        Result: data.Result
      }
    }
  },
  updateAutoDetectionStepsAction_Failure: (error: string): TestSuiteAutoDetectionActionTypes => {
    return {
      type: UPDATE_AUTO_DETECTION_STEPS_FAILURE,
      errorMsg: error
    }
  },
  startAutoDetectionAction_Request: (): TestSuiteAutoDetectionActionTypes => {
    return {
      type: START_AUTO_DETECTION_REQUEST
    }
  },
  startAutoDetectionAction_Success: (): TestSuiteAutoDetectionActionTypes => {
    return {
      type: START_AUTO_DETECTION_SUCCESS
    }
  },
  startAutoDetectionAction_Failure: (error: string): TestSuiteAutoDetectionActionTypes => {
    return {
      type: START_AUTO_DETECTION_FAILURE,
      errorMsg: error
    }
  },
  stopAutoDetectionAction_Request: (): TestSuiteAutoDetectionActionTypes => {
    return {
      type: STOP_AUTO_DETECTION_REQUEST
    }
  },
  stopAutoDetectionAction_Success: (): TestSuiteAutoDetectionActionTypes => {
    return {
      type: STOP_AUTO_DETECTION_SUCCESS
    }
  },
  stopAutoDetectionAction_Failure: (error: string): TestSuiteAutoDetectionActionTypes => {
    return {
      type: STOP_AUTO_DETECTION_FAILURE,
      errorMsg: error
    }
  },
  updateAutoDetectionPrerequisiteAction: (property: Property): TestSuiteAutoDetectionActionTypes => {
    return {
      type: UPDATE_AUTO_DETECTION_PREREQUISITE,
      payload: property
    }
  },
  getAutoDetectionLogAction_Request: (): TestSuiteAutoDetectionActionTypes => {
    return {
      type: GET_AUTO_DETECTION_LOG_REQUEST,
    }
  },
  getAutoDetectionLogAction_Success: (log: Blob): TestSuiteAutoDetectionActionTypes => {
    return {
      type: GET_AUTO_DETECTION_LOG_SUCCESS,
      payload: log
    }
  },
  getAutoDetectionLogAction_Failure: (error: string): TestSuiteAutoDetectionActionTypes => {
    return {
      type: GET_AUTO_DETECTION_LOG_FAILURE,
      errorMsg: error
    }
  },
  setAutoDetectionLogAction: (log: string): TestSuiteAutoDetectionActionTypes => {
    return {
      type: SET_AUTO_DETECTION_LOG,
      payload: log
    }
  },
  applyAutoDetectionResultAction_Request: (): TestSuiteAutoDetectionActionTypes => {
    return {
      type: APPLY_AUTO_DETECTION_RESULT_REQUEST
    }
  },
  applyAutoDetectionResultAction_Success: (): TestSuiteAutoDetectionActionTypes => {
    return {
      type: APPLY_AUTO_DETECTION_RESULT_SUCCESS
    }
  },
  applyAutoDetectionResultAction_Failure: (error: string): TestSuiteAutoDetectionActionTypes => {
    return {
      type: APPLY_AUTO_DETECTION_RESULT_FAILURE,
      errorMsg: error
    }
  }
}
