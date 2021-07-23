// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GET_DETECTION_STEPS_REQUEST, GET_DETECTION_STEPS_SUCCESS, GET_PREREQUISITE_REQUEST, GET_PREREQUISITE_SUCCESS, TestSuiteAutoDetectionActionTypes, UPDATE_PREREQUISITE } from "../actions/AutoDetectionAction";
import { Prerequisite, DetectionSteps } from "../model/AutoDetectionData";
import { Property } from "../model/Property";

export interface AutoDetectState {
    isPrerequisiteLoading: boolean;
    isDetectionStepsLoading: boolean;
    errorMsg?: string;
    prerequisite?: Prerequisite;
    detectionSteps?: DetectionSteps;
}

const initialAutoDetectState: AutoDetectState = {
    isPrerequisiteLoading: false,
    isDetectionStepsLoading: false,
    errorMsg: undefined,
    prerequisite: undefined,
    detectionSteps: undefined
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
            action.payload.Properties.map(p => {
                p.Value = p.Choices[0];
            })
            return {
                ...state,
                isPrerequisiteLoading: false,
                errorMsg: undefined,
                prerequisite: action.payload
            }
        case UPDATE_PREREQUISITE:
            return {
                ...state,
                isPrerequisiteLoading: false,
                prerequisite: action.payload
            };

        case GET_DETECTION_STEPS_REQUEST:
            return {
                ...state,
                isDetectionStepsLoading: true,
                errorMsg: undefined,
                prerequisite: undefined
            }

        case GET_DETECTION_STEPS_SUCCESS:
            return {
                ...state,
                isDetectionStepsLoading: false,
                errorMsg: undefined,
                detectionSteps: action.payload
            }
        default:
            return state;
    }
}