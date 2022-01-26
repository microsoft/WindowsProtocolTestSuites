// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FilterTestCaseActionTypes, GET_FILTERTESTCASE_RULES_REQUEST, GET_FILTERTESTCASE_RULES_SUCCESS, GET_FILTERTESTCASE_RULES_FAILURE, SET_SELECTED_RULES, GET_TESTSUITETESTCASES_REQUEST, GET_TESTSUITETESTCASES_SUCCESS, GET_TESTSUITETESTCASES_FAILURE, SET_RULES_REQUEST, SET_RULES_SUCCESS, SET_RULES_FAILURE } from '../actions/FilterTestCaseAction'
import { Configuration } from '../model/Configuration'
import { AllNode, RuleGroup, SelectedRuleGroup, Rule } from '../model/RuleGroup'
import { TestCase } from '../model/TestCase'

export interface FilterTestCaseState {
    isCasesLoading: boolean;
    isRulesLoading: boolean;
    isPosting: boolean;
    errorMsg?: string;
    ruleGroup: RuleGroup[];
    listTestCases: TestCase[];
    selectedConfiguration?: Configuration;
    listSelectedCases: TestCase[];
    selectedRules: SelectedRuleGroup[];
    targetFilterIndex: number;
    mappingFilterIndex: number;
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
    selectedRules: [],
    targetFilterIndex: -1,
    mappingFilterIndex: -1
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
                const selectedRules = action.payload.SelectedRules.find(x => x.Name === group.Name)
                const currSelected = (selectedRules?.Rules?.map(s => s.Name.replace(group.Name, AllNode.value))) ?? []
                return { Name: group.Name, Selected: currSelected }
            })
            return {
                ...state,
                isRulesLoading: false,
                ruleGroup: action.payload.AllRules,
                targetFilterIndex: action.payload.TargetFilterIndex,
                mappingFilterIndex: action.payload.MappingFilterIndex,
                selectedRules: initialSelected,
                listSelectedCases: getFilteredTestCases(initialSelected, action.payload.AllRules, state.listTestCases)
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
                listSelectedCases: getFilteredTestCases(state.selectedRules, state.ruleGroup, action.payload)
            }
        case GET_TESTSUITETESTCASES_FAILURE:
            return {
                ...state,
                isCasesLoading: false,
                listTestCases: [],
                errorMsg: action.errorMsg
            }
        case SET_SELECTED_RULES:
            const currSelectedRules = state.selectedRules.map(item =>
                item.Name === action.payload.Name
                    ? action.payload
                    : item
            )
            if (state.targetFilterIndex !== -1 && state.mappingFilterIndex !== -1) {
                const targetFilterName = state.ruleGroup[state.targetFilterIndex].Name
                if (action.payload.Name === targetFilterName) {
                    const tmp = currSelectedRules[state.targetFilterIndex].Selected.map(x => x.substring(x.indexOf('%') + 1))
                    const features = getFeatures(state.ruleGroup[state.mappingFilterIndex].Rules, AllNode.value)
                    const selectedFeature = features.filter(x => {
                        return tmp.some(t => { return x.includes(t) })
                    })
                    currSelectedRules[state.mappingFilterIndex].Selected = selectedFeature
                }
                const mappingFilterName = state.ruleGroup[state.mappingFilterIndex].Name
                if (action.payload.Name === mappingFilterName) {
                    const tmp = currSelectedRules[state.mappingFilterIndex].Selected.map(x => {
                        const curr = x.substring(0, x.indexOf('%')).split('.').pop()
                        const fKey = x.substring(x.indexOf('%') + 1)
                        return `${fKey}%${curr}`
                    })
                    const features = getFeatures(state.ruleGroup[state.targetFilterIndex].Rules, AllNode.value)
                    const selectedFeature = features.filter(x => {
                        return tmp.some(t => { return x.includes(t) })
                    })
                    currSelectedRules[state.targetFilterIndex].Selected = selectedFeature
                }
            }
            return {
                ...state,
                selectedRules: currSelectedRules,
                listSelectedCases: getFilteredTestCases(currSelectedRules, state.ruleGroup, state.listTestCases)
            }
        case SET_RULES_REQUEST:
            return {
                ...state,
                isPosting: true,
                errorMsg: undefined
            }
        case SET_RULES_SUCCESS:
            return {
                ...state,
                isPosting: false
            }
        case SET_RULES_FAILURE:
            return {
                ...state,
                isPosting: false,
                errorMsg: action.errorMsg
            }
        default:
            return state
    }
}

function getMappedCategories(rules: Rule[] | undefined, parent: string): { [key: string]: string[] | undefined } {
    if (rules !== undefined) {
        return rules.reduce((dict: { [key: string]: string[] | undefined }, rule) => {
            const curr = parent ? `${parent}.${rule.Name}` : rule.Name
            if (rule.Rules) {
                return { ...dict, ...getMappedCategories(rule.Rules, curr) }
            } else {
                dict[curr] = rule.Categories
                return dict
            }
        }, {})
    }
    return {}
}

function getFilteredTestCases(currSelectedRules: SelectedRuleGroup[], ruleGroup: RuleGroup[], listTestCases: TestCase[]): TestCase[] {
    return currSelectedRules.reduce((filteredCases, g) => {
        const currRule = ruleGroup.find(o => o.Name === g.Name)?.Rules
        const ruleDict = getMappedCategories(currRule, AllNode.value)
        const cleanSelected = g.Selected.map(e => {
            if (e.indexOf('%') > 0) {
                return e.substring(0, e.indexOf('%'))
            } else {
                return e
            }
        })
        const uniqueRules = [...new Set(cleanSelected)]
        const positiveCategories = uniqueRules.flatMap(s => ruleDict[s]).filter(s => s?.charAt(0) != "!");

        const negativeCategories = uniqueRules.flatMap(s => ruleDict[s]).filter(s => s?.charAt(0) == "!").flatMap(s => s?.substring(1));

        if (negativeCategories.length > 0) {
            return filteredCases.filter(x => {
                return x.Category && (!x.Category.some(r => negativeCategories.includes(r)) || x.Category.some(r => positiveCategories.includes(r)))
            })
        }
        return filteredCases.filter(x => {
            return x.Category && x.Category.some(r => positiveCategories.includes(r))
        })
    }, listTestCases)
}

function getFeatures(rules: Rule[], parent: string): string[] {
    const results: string[] = []
    rules.forEach(rule => {
        const curr = parent ? `${parent}.${rule.Name}` : rule.Name
        if (rule.Rules) {
            results.push(...getFeatures(rule.Rules, curr))
        }
        if (rule.MappingRules) {
            rule.MappingRules.forEach(c => { results.push(`${curr}%${c}`) })
        }
    })
    return results
}
