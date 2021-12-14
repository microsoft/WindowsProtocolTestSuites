// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { DetectionSummary } from '../model/DetectionResult';

export const GET_DETECTION_SUMMARY_REQUEST = 'DETECTION_RESULT/GET_DETECTION_SUMMARY_REQUEST';
export const GET_DETECTION_SUMMARY_SUCCESS = 'DETECTION_RESULT/GET_DETECTION_SUMMARY_SUCCESS';
export const GET_DETECTION_SUMMARY_FAILURE = 'DETECTION_RESULT/GET_DETECTION_SUMMARY_FAILURE';
export const SET_DETECTION_SUMMARY = 'SET_DETECTION_SUMMARY';

// define action types
interface GetDetectionSummaryActionRequestType { type: typeof GET_DETECTION_SUMMARY_REQUEST; }
interface GetDetectionSummaryActionSuccessType { type: typeof GET_DETECTION_SUMMARY_SUCCESS; payload: DetectionSummary; }
interface GetDetectionSummaryActionFailureType { type: typeof GET_DETECTION_SUMMARY_FAILURE; errorMsg: string; }
interface SetDetectionSummaryActionType { type: typeof SET_DETECTION_SUMMARY; }

export type TestSuiteDetectionResultActionTypes =
    GetDetectionSummaryActionRequestType
    | GetDetectionSummaryActionSuccessType
    | GetDetectionSummaryActionFailureType
    | SetDetectionSummaryActionType

// define actions
export const DetectionResultActions = {
    getDetectionSummaryAction_Request: (): TestSuiteDetectionResultActionTypes => {
        return {
            type: GET_DETECTION_SUMMARY_REQUEST
        }
    },
    getDetectionSummaryAction_Success: (data: DetectionSummary): TestSuiteDetectionResultActionTypes => {
        return {
            type: GET_DETECTION_SUMMARY_SUCCESS,
            payload: data
        }
    },
    getDetectionSummaryAction_Failure: (error: string): TestSuiteDetectionResultActionTypes => {
        return {
            type: GET_DETECTION_SUMMARY_FAILURE,
            errorMsg: error
        }
    },
    setDetectionSummaryAction: (): TestSuiteDetectionResultActionTypes => {
        return {
            type: SET_DETECTION_SUMMARY
        }
    },
}