// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FilterTestCaseActionTypes, GET_FILTERTESTCASE_RULES_REQUEST, GET_FILTERTESTCASE_RULES_SUCCESS, GET_FILTERTESTCASE_RULES_FAILURE, SET_SELECTED_RULES, SET_SELECTED_TESTCASES } from "../actions/FilterTestCaseAction";
import { Configuration } from "../model/Configuration";
import { RuleGroup, SelectedRuleGroup } from "../model/RuleGroup";

export interface FilterTestCaseState {
    isLoading: boolean;
    errorMsg?: string;
    ruleGroup: RuleGroup[];
    selectedConfiguration?: Configuration;
    listSelectedCases?: string[];
    selectedRules: SelectedRuleGroup[];
}

const initialFilterTestCaseState: FilterTestCaseState = {
    isLoading: false,
    errorMsg: undefined,
    ruleGroup: [],
    selectedConfiguration: undefined,
    listSelectedCases: [],
    selectedRules: []
}

export const getFilterTestCaseReducer = (state = initialFilterTestCaseState, action: FilterTestCaseActionTypes): FilterTestCaseState => {
    switch (action.type) {
        case GET_FILTERTESTCASE_RULES_REQUEST:
            return {
                ...state,
                isLoading: true,
                errorMsg: undefined,
                ruleGroup: []
            }
        case GET_FILTERTESTCASE_RULES_SUCCESS:
            let initialSelected = action.payload.map(group => { return { Name: group.Name, Selected: [] } })
            return {
                ...state,
                isLoading: false,
                ruleGroup: action.payload,
                selectedRules: initialSelected
            }
        case GET_FILTERTESTCASE_RULES_FAILURE:
            return {
                ...state,
                isLoading: false,
                ruleGroup: [],
                errorMsg: action.errorMsg
            }
        case SET_SELECTED_RULES:
            return {
                ...state,
                selectedRules: state.selectedRules.map(item =>
                    item.Name == action.payload.Name
                        ? action.payload
                        : item
                )
            }
        case SET_SELECTED_TESTCASES:
            return {
                ...state,
                isLoading: false,
                listSelectedCases: action.payload
            }        
        default:
            return state;
    }
}
