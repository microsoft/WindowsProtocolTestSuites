// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
    GET_ALLTESTCASES_REQUEST,
    GET_ALLTESTCASES_SUCCESS,
    GET_ALLTESTCASES_FAILURE,
    CREATE_RUNREQUEST_REQUEST,
    CREATE_RUNREQUEST_SUCCESS,
    CREATE_RUNREQUEST_FAILURE,
    SelectedTestCasesActionTypes,
} from '../actions/SelectedTestCasesAction';

export interface SelectedTestCasesState {
    isLoading: boolean;
    isPosting: boolean;
    errorMsg?: string;
    allTestCases: string[];
    testResultId: number | undefined;
}

const initialSelectedTestCasesState: SelectedTestCasesState = {
    isLoading: false,
    isPosting: false,
    errorMsg: undefined,
    allTestCases: [],
    testResultId: undefined,
};

export const getSelectedTestCasesReducer = (state = initialSelectedTestCasesState, action: SelectedTestCasesActionTypes): SelectedTestCasesState => {
    switch (action.type) {
        case GET_ALLTESTCASES_REQUEST:
            return {
                ...state,
                isLoading: true,
                errorMsg: undefined,
            };

        case GET_ALLTESTCASES_SUCCESS:
            return {
                ...state,
                isLoading: false,
                allTestCases: action.payload
            };

        case GET_ALLTESTCASES_FAILURE:
            return {
                ...state,
                isLoading: false,
                allTestCases: [],
                errorMsg: action.errorMsg
            }

        case CREATE_RUNREQUEST_REQUEST:
            return {
                ...state,
                isPosting: true,
                errorMsg: undefined,
                testResultId: undefined,
            };

        case CREATE_RUNREQUEST_SUCCESS:
            return {
                ...state,
                isPosting: false,
                testResultId: action.payload
            };

        case CREATE_RUNREQUEST_FAILURE:
            return {
                ...state,
                isPosting: false,
                errorMsg: action.errorMsg,
                testResultId: undefined,
            };

        default:
            return state;
    }
};