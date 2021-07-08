// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GET_PREREQUISITE_REQUEST, TestSuiteAutoDetectionActionTypes } from "../actions/AutoDetectionAction";
import { Prerequisite, DetectionSteps } from "../model/AutoDetectionData";

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
        default:
            return state;
    }
}