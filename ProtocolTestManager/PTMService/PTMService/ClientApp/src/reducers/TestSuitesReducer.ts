// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GET_TESTSUITES_FAILURE, GET_TESTSUITES_REQUEST, GET_TESTSUITES_SUCCESS, SET_SELECTED_TESTSUITE, TestSuitesActionTypes } from "../actions/TestSuitesActions";
import { TestSuite } from "../model/TestSuite";

export interface TestSuitesState {
    isLoading: boolean;
    errorMsg?: string;
    testSuiteList: TestSuite[];
    selectedTestSuite?: TestSuite;
}

const initialTestSuitesState: TestSuitesState = {
    isLoading: false,
    errorMsg: undefined,
    testSuiteList: [],
    selectedTestSuite: undefined,
}

export const getTestSuitesReducer = (state = initialTestSuitesState, action: TestSuitesActionTypes): TestSuitesState => {
    switch (action.type) {
        case GET_TESTSUITES_REQUEST:
            return {
                ...state,
                isLoading: true,
                errorMsg: undefined,
                testSuiteList: []
            }
        case GET_TESTSUITES_SUCCESS:
            return {
                ...state,
                isLoading: false,
                testSuiteList: action.payload
            }
        case GET_TESTSUITES_FAILURE:
            return {
                ...state,
                isLoading: false,
                testSuiteList: [],
                errorMsg: action.errorMsg
            }
        case SET_SELECTED_TESTSUITE:
            return {
                ...state,
                selectedTestSuite: action.selectedTestSuite,
            }
        default:
            return state;
    }
}