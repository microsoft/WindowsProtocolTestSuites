// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GET_DETECTION_SUMMARY_REQUEST, GET_DETECTION_SUMMARY_SUCCESS, GET_DETECTION_SUMMARY_FAILURE, RESET_DETECTION_RESULT, TestSuiteDetectionResultActionTypes } from "../actions/DetectionResultAction";
import { DetectionSummary } from '../model/DetectionResult';

export interface DetectionResultState {
    isDetectionResultLoading: boolean;
    errorMsg?: string;
    detectionResult?: DetectionSummary;
}

const initialAutoDetectState: DetectionResultState = {
    isDetectionResultLoading: false,
    errorMsg: undefined,
    detectionResult: undefined
}

export const getDetectionResultReducer = (state = initialAutoDetectState, action: TestSuiteDetectionResultActionTypes): DetectionResultState => {
    switch (action.type) {
        case GET_DETECTION_SUMMARY_REQUEST:
            return {
                ...state,
                isDetectionResultLoading: true,
                errorMsg: undefined,
                detectionResult: undefined
            }

        case GET_DETECTION_SUMMARY_SUCCESS:
            return {
                ...state,
                isDetectionResultLoading: false,
                errorMsg: undefined,
                detectionResult: action.payload
            }

        case GET_DETECTION_SUMMARY_FAILURE:
            return {
                ...state,
                isDetectionResultLoading: false,
                errorMsg: action.errorMsg
            }

        case RESET_DETECTION_RESULT:
            return {
                ...state,
                detectionResult: undefined,
                isDetectionResultLoading: false,
                errorMsg: undefined
            }

        default:
            return state;
    }
}