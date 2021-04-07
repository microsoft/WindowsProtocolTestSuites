// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FilterTestCaseActionTypes, GET_FILTERTESTCASE_RULES_REQUEST, GET_FILTERTESTCASE_RULES_SUCCESS, GET_FILTERTESTCASE_RULES_FAILURE, SET_SELECTED_RULES, GET_TESTSUITETESTCASES_REQUEST, GET_TESTSUITETESTCASES_SUCCESS, GET_TESTSUITETESTCASES_FAILURE, SET_RULES_REQUEST, SET_RULES_SUCCESS, SET_RULES_FAILURE } from "../actions/FilterTestCaseAction";
import { Configuration } from "../model/Configuration";
import { RuleGroup, SelectedRuleGroup } from "../model/RuleGroup";
import { TestCase } from "../model/TestCase";

export interface FilterTestCaseState {
    isLoading: boolean;
    isPosting: boolean;
    errorMsg?: string;
    ruleGroup: RuleGroup[];
    listTestCases: TestCase[];
    selectedConfiguration?: Configuration;
    listSelectedCases?: string[];
    selectedRules: SelectedRuleGroup[];
}

const initialFilterTestCaseState: FilterTestCaseState = {
    isLoading: false,
    isPosting: false,
    errorMsg: undefined,
    ruleGroup: [],
    listTestCases: [],
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
        case GET_TESTSUITETESTCASES_REQUEST:
            return {
                ...state,
                isLoading: true,
                errorMsg: undefined,
                listTestCases: []
            }
        case GET_TESTSUITETESTCASES_SUCCESS:

            return {
                ...state,
                isLoading: false,
                listTestCases: action.payload
            }
        case GET_TESTSUITETESTCASES_FAILURE:
            return {
                ...state,
                isLoading: false,
                listTestCases: [],
                errorMsg: action.errorMsg
            }
        case SET_SELECTED_RULES:
            let currSelectedRules = state.selectedRules.map(item =>
                item.Name == action.payload.Name
                    ? action.payload
                    : item
            )
            let match: string[] = []

            currSelectedRules.forEach(g=>match.push(...g.Selected));
            
            let filterCases = state.listTestCases.filter(x=>
                {
                    return x.Category && x.Category.some(r=>match.indexOf(r)>=0)
                }).map(e => {return e.Name })
            return {
                ...state,
                selectedRules: currSelectedRules,
                listSelectedCases:filterCases
            }
        case SET_RULES_REQUEST:
            return {
                ...state,
                isPosting: true,
                errorMsg: undefined,
            }
        case SET_RULES_SUCCESS:
            return {
                ...state,
                isPosting: false,
            }
        case SET_RULES_FAILURE:
            return {
                ...state,
                isPosting: false,
                errorMsg: action.errorMsg
            }
        default:
            return state;
    }
}
