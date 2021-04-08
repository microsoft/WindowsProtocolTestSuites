// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GET_TESTSUITES_REQUEST, GET_TESTSUITES_SUCCESS, GET_TESTSUITES_FAILURE, ManagementActionTypes, SET_SEARCHTEXT, INSTALL_TESTSUITE_REQUEST, INSTALL_TESTSUITE_SUCCESS, INSTALL_TESTSUITE_FAILURE } from "../actions/ManagementAction";
import { TestSuite } from "../model/TestSuite";

export interface TestSuitesState {
    isLoading: boolean;
    isInstalling: boolean;
    errorMsg?: string;
    testSuiteList: TestSuite[];
    displayList: TestSuite[];
    selectedTestSuite?: TestSuite;
    searchText?: string;
}

const initialTestSuitesState: TestSuitesState = {
    isLoading: false,
    isInstalling: false,
    errorMsg: undefined,
    testSuiteList: [],
    displayList: [],
    selectedTestSuite: undefined,
    searchText: undefined,
}

export const getManagementReducer = (state = initialTestSuitesState, action: ManagementActionTypes): TestSuitesState => {
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
                testSuiteList: action.payload,
                displayList: filterConfiguration(action.payload, state.searchText),
            }
        case GET_TESTSUITES_FAILURE:
            return {
                ...state,
                isLoading: false,
                testSuiteList: [],
                errorMsg: action.errorMsg
            }
        case INSTALL_TESTSUITE_REQUEST:
            return {
                ...state,
                isLoading: false,
                isInstalling: true,
                errorMsg: undefined,
            }
        case INSTALL_TESTSUITE_SUCCESS:
            return {
                ...state,
                isLoading: false,
                isInstalling: false,
                errorMsg: undefined,
            }
        case INSTALL_TESTSUITE_FAILURE:
            return {
                ...state,
                isLoading: false,
                isInstalling: false,
                errorMsg: action.errorMsg,
            }
        case SET_SEARCHTEXT:
            return {
                ...state,
                isLoading: false,
                displayList: filterConfiguration(state.testSuiteList, action.searchText)
            }
        default:
            return state;
    }
}

function filterConfiguration(orignalList: TestSuite[], searchText?: string): TestSuite[] {
    if (!searchText) {
        return orignalList;
    }

    const newList: TestSuite[] = [];
    const lowerSearchText = searchText!.toLowerCase();
    orignalList.map((item) => {
        if ((item.Name.toLowerCase().indexOf(lowerSearchText!) >= 0) || (item.Name.toLowerCase().indexOf(lowerSearchText!)) >= 0) {
            newList.push(item);
        }
    });

    return newList;
}