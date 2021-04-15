// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FilterTestCaseActionTypes, GET_FILTERTESTCASE_RULES_REQUEST, GET_FILTERTESTCASE_RULES_SUCCESS, GET_FILTERTESTCASE_RULES_FAILURE, SET_SELECTED_RULES, GET_TESTSUITETESTCASES_REQUEST, GET_TESTSUITETESTCASES_SUCCESS, GET_TESTSUITETESTCASES_FAILURE, SET_RULES_REQUEST, SET_RULES_SUCCESS, SET_RULES_FAILURE } from "../actions/FilterTestCaseAction";
import { Configuration } from "../model/Configuration";
import { AllNode, RuleGroup, SelectedRuleGroup, Rule, MapItem } from "../model/RuleGroup";
import { TestCase } from "../model/TestCase";

export interface FilterTestCaseState {
    isCasesLoading: boolean;
    isRulesLoading: boolean;
    isPosting: boolean;
    errorMsg?: string;
    ruleGroup: RuleGroup[];
    listTestCases: TestCase[];
    selectedConfiguration?: Configuration;
    listSelectedCases?: string[];
    selectedRules: SelectedRuleGroup[];
}

const initialFilterTestCaseState: FilterTestCaseState = {
    isCasesLoading: false,
    isRulesLoading: false,
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
                isRulesLoading: true,
                errorMsg: undefined,
                ruleGroup: []
            }
        case GET_FILTERTESTCASE_RULES_SUCCESS:
            const initialSelected = action.payload.AllRules.map(group => {

                const selectedRules = action.payload.SelectedRules.find(x => x.Name == group.Name)
                const currSelected = selectedRules?.Rules?.map(s => s.Name.replace(group.Name, AllNode.value)) || []
                return { Name: group.Name, Selected: currSelected }
            })
            return {
                ...state,
                isRulesLoading: false,
                ruleGroup: action.payload.AllRules,
                selectedRules: initialSelected,
                listSelectedCases: getSelectCases(initialSelected, action.payload.AllRules, state.listTestCases)
            }
        case GET_FILTERTESTCASE_RULES_FAILURE:
            return {
                ...state,
                isRulesLoading: false,
                ruleGroup: [],
                errorMsg: action.errorMsg
            }
        case GET_TESTSUITETESTCASES_REQUEST:
            return {
                ...state,
                isCasesLoading: true,
                errorMsg: undefined,
                listTestCases: []
            }
        case GET_TESTSUITETESTCASES_SUCCESS:

            return {
                ...state,
                isCasesLoading: false,
                listTestCases: action.payload,
                listSelectedCases: getSelectCases(state.selectedRules, state.ruleGroup, action.payload)
            }
        case GET_TESTSUITETESTCASES_FAILURE:
            return {
                ...state,
                isCasesLoading: false,
                listTestCases: [],
                errorMsg: action.errorMsg
            }
        case SET_SELECTED_RULES:
            let currSelectedRules = state.selectedRules.map(item =>
                item.Name == action.payload.Name
                    ? action.payload
                    : item
            )
            let list = getSelectCases(currSelectedRules, state.ruleGroup, state.listTestCases)
            return {
                ...state,
                selectedRules: currSelectedRules,
                listSelectedCases: list
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

function getMapCategories(rules: Rule[] | undefined, parent: string): MapItem {
    if (rules !== undefined) {
        return rules.reduce((dict: MapItem, rule) => {
            const curr = parent ? parent + "." + rule.Name : rule.Name
            if (rule.Rules) {
                return { ...dict, ...getMapCategories(rule.Rules, curr) };
            }
            else {
                dict[curr] = rule.Categories;
                return dict;
            }
        }, {});
    }
    return {};
}

function getSelectCases(currSelectedRules: SelectedRuleGroup[], ruleGroup: RuleGroup[], listTestCases: TestCase[]):string[] {
    let match: string[] = []

    currSelectedRules.forEach(g => {
        const currRule = ruleGroup.find(o => o.Name == g.Name)?.Rules;
        const mapitems = getMapCategories(currRule, AllNode.value)
        g.Selected.forEach(s => {
            match.push(...mapitems[s] || [])
        })
    });

    return listTestCases.filter(x => {
        return x.Category && x.Category.some(r => match.indexOf(r) >= 0)
    }).map(e => { return e.Name })
}