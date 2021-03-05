// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GET_TESTSUITE_CONFIGURATIONS_FAILURE, GET_TESTSUITE_CONFIGURATIONS_REQUEST, GET_TESTSUITE_CONFIGURATIONS_SUCCESS, SET_SEARCHTEXT, TestSuiteConfigurationActionTypes } from "../actions/TestSuiteConfigurationAction";
import { Configuration } from "../model/Configuration";

export interface ConfigurationState {
    isLoading: boolean;
    errorMsg?: string;
    searchText?: string;
    configurationList: Configuration[];
    displayList: Configuration[];
    selectedConfiguration?: Configuration;
}

const initialConfigurationState: ConfigurationState = {
    isLoading: false,
    errorMsg: undefined,
    configurationList: [],
    displayList: [],
    selectedConfiguration: undefined,
}

export const getConfigurationReducer = (state = initialConfigurationState, action: TestSuiteConfigurationActionTypes): ConfigurationState => {
    switch (action.type) {
        case GET_TESTSUITE_CONFIGURATIONS_REQUEST:
            return {
                ...state,
                isLoading: true,
                errorMsg: undefined,
                configurationList: [],
                displayList: [],
            }
        case GET_TESTSUITE_CONFIGURATIONS_SUCCESS:
            return {
                ...state,
                isLoading: false,
                configurationList: action.payload,
                displayList: filterConfiguration(action.payload, state.searchText),
            }
        case GET_TESTSUITE_CONFIGURATIONS_FAILURE:
            return {
                ...state,
                isLoading: false,
                configurationList: [],
                displayList: [],
                errorMsg: action.errorMsg
            }
        case SET_SEARCHTEXT:
            return {
                ...state,
                isLoading: false,
                displayList: filterConfiguration(state.configurationList, action.searchText)
            }

        default:
            return state;
    }
}

function filterConfiguration(orignalList: Configuration[], searchText?: string): Configuration[] {
    if (!searchText) {
        return orignalList;
    }

    const newList: Configuration[] = [];
    const lowerSearchText = searchText!.toLowerCase();
    orignalList.map((item) => {
        if ((item.Name.toLowerCase().indexOf(lowerSearchText!) >= 0) || (item.Description.toLowerCase().indexOf(lowerSearchText!)) >= 0) {
            newList.push(item);
        }
    });

    return newList;
}