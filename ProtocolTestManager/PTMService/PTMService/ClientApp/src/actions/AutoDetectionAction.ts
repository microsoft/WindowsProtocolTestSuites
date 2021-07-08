// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Prerequisite, DetectionSteps } from "../model/AutoDetectionData";

export const GET_PREREQUISITE_REQUEST = 'AUTO_DETECT/GET_PREREQUISITE_REQUEST';
export const GET_PREREQUISITE_SUCCESS = 'AUTO_DETECT/GET_PREREQUISITE_SUCCESS';
export const GET_PREREQUISITE_FAILURE = 'AUTO_DETECT/GET_PREREQUISITE_FAILURE';

export const GET_DETECTION_STEPS_REQUEST = 'AUTO_DETECT/GET_DETECTION_STEPS_REQUEST';
export const GET_DETECTION_STEPS_SUCCESS = 'AUTO_DETECT/GET_DETECTION_STEPS_SUCCESS';
export const GET_DETECTION_STEPS_FAILURE = 'AUTO_DETECT/GET_DETECTION_STEPS_FAILURE';

export const START_AUTO_DETECTION_REQUEST = 'AUTO_DETECT/START_AUTO_DETECTION_REQUEST';
export const START_AUTO_DETECTION_SUCCESS = 'AUTO_DETECT/START_AUTO_DETECTION_SUCCESS';
export const START_AUTO_DETECTION_FAILURE = 'AUTO_DETECT/START_AUTO_DETECTION_FAILURE';

export const STOP_AUTO_DETECTION_REQUEST = 'AUTO_DETECT/STOP_AUTO_DETECTION_REQUEST';
export const STOP_AUTO_DETECTION_SUCCESS = 'AUTO_DETECT/STOP_AUTO_DETECTION_SUCCESS';
export const STOP_AUTO_DETECTION_FAILURE = 'AUTO_DETECT/STOP_AUTO_DETECTION_FAILURE';

export const GET_DETECTION_SUMMARY_REQUEST = 'AUTO_DETECT/GET_DETECTION_SUMMARY_REQUEST';
export const GET_DETECTION_SUMMARY_SUCCESS = 'AUTO_DETECT/GET_DETECTION_SUMMARY_SUCCESS';
export const GET_DETECTION_SUMMARY_FAILURE = 'AUTO_DETECT/GET_DETECTION_SUMMARY_FAILURE';

// define action types
interface GetAutoDetectPrerequisiteActionRequestType { type: typeof GET_PREREQUISITE_REQUEST; }
interface GetAutoDetectPrerequisiteActionSuccessType { type: typeof GET_PREREQUISITE_SUCCESS; payload: Prerequisite; }
interface GetAutoDetectPrerequisiteActionFailureType { type: typeof GET_PREREQUISITE_FAILURE; errorMsg: string; }

interface GetAutoDetectionStepsActionRequestType { type: typeof GET_DETECTION_STEPS_REQUEST; }
interface GetAutoDetectionStepsActionSuccessType { type: typeof GET_DETECTION_STEPS_SUCCESS; payload: DetectionSteps; }
interface GetAutoDetectionStepsActionFailureType { type: typeof GET_DETECTION_STEPS_FAILURE; errorMsg: string; }

interface StartAutoDetectionActionRequestType { type: typeof START_AUTO_DETECTION_REQUEST; }
interface StartAutoDetectionActionSuccessType { type: typeof START_AUTO_DETECTION_SUCCESS; }
interface StartAutoDetectionActionFailureType { type: typeof START_AUTO_DETECTION_FAILURE; errorMsg: string; }

interface StopAutoDetectionActionRequestType { type: typeof STOP_AUTO_DETECTION_REQUEST; }
interface StopAutoDetectionActionSuccessType { type: typeof STOP_AUTO_DETECTION_SUCCESS; }
interface StopAutoDetectionActionFailureType { type: typeof STOP_AUTO_DETECTION_FAILURE; errorMsg: string; }



export type TestSuiteAutoDetectionActionTypes = GetAutoDetectPrerequisiteActionRequestType
    | GetAutoDetectPrerequisiteActionSuccessType
    | GetAutoDetectPrerequisiteActionFailureType
    | GetAutoDetectionStepsActionRequestType
    | GetAutoDetectionStepsActionSuccessType
    | GetAutoDetectionStepsActionFailureType
    | StartAutoDetectionActionRequestType
    | StartAutoDetectionActionSuccessType
    | StartAutoDetectionActionFailureType
    | StopAutoDetectionActionRequestType
    | StopAutoDetectionActionSuccessType
    | StopAutoDetectionActionFailureType

// define actions
export const AutoDetectActions = {
    GetAutoDetectPrerequisiteAction_Request: (): TestSuiteAutoDetectionActionTypes => {
        return {
            type: GET_PREREQUISITE_REQUEST
        }
    },
    GetAutoDetectPrerequisiteAction_Success: (data: Prerequisite): TestSuiteAutoDetectionActionTypes => {
        return {
            type: GET_PREREQUISITE_SUCCESS,
            payload: data
        }
    },
    GetAutoDetectPrerequisiteAction_Failure: (error: string): TestSuiteAutoDetectionActionTypes => {
        return {
            type: GET_PREREQUISITE_FAILURE,
            errorMsg: error
        }
    },
}