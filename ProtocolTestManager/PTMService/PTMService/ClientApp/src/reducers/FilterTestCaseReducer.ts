// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FilterTestCaseActionTypes, GET_FILTERTESTCASE_RULES_REQUEST, GET_FILTERTESTCASE_RULES_SUCCESS, GET_FILTERTESTCASE_RULES_FAILURE, SET_SELECTED_RULES, GET_TESTSUITETESTCASES_REQUEST, GET_TESTSUITETESTCASES_SUCCESS, GET_TESTSUITETESTCASES_FAILURE, SET_RULES_REQUEST, SET_RULES_SUCCESS, SET_RULES_FAILURE } from '../actions/FilterTestCaseAction'
import { Configuration } from '../model/Configuration'
import { AllNode, RuleGroup, SelectedRuleGroup, Rule, MapItem } from '../model/RuleGroup'
import { TestCase } from '../model/TestCase'

export interface FilterTestCaseState {
  isCasesLoading: boolean
  isRulesLoading: boolean
  isPosting: boolean
  errorMsg?: string
  ruleGroup: RuleGroup[]
  listTestCases: TestCase[]
  selectedConfiguration?: Configuration
  listSelectedCases?: string[]
  selectedRules: SelectedRuleGroup[]
  targetFilterIndex: number
  mappingFilterIndex: number
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
          const features = getItems(state.ruleGroup[state.mappingFilterIndex].Rules, AllNode.value)
          const selectedFeature = features.filter(x => {
            return tmp.some(t => { return x.includes(t) })
          })
          currSelectedRules[state.mappingFilterIndex].Selected = selectedFeature
        }
        const mappingFilterName = state.ruleGroup[state.mappingFilterIndex].Name
        if (action.payload.Name === mappingFilterName) {
          const tmp = currSelectedRules[state.mappingFilterIndex].Selected.map(x => {
            const curr = x.substring(0, x.indexOf('%')).split('.').pop()
            const fkey = x.substring(x.indexOf('%') + 1)
            return `${fkey}%${curr}`
          })
          const features = getItems(state.ruleGroup[state.targetFilterIndex].Rules, AllNode.value)
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

function getMapCategories (rules: Rule[] | undefined, parent: string): MapItem {
  if (rules !== undefined) {
    return rules.reduce((dict: MapItem, rule) => {
      const curr = parent ? `${parent}.${rule.Name}` : rule.Name
      if (rule.Rules != null) {
        return { ...dict, ...getMapCategories(rule.Rules, curr) }
      } else {
        dict[curr] = rule.Categories
        return dict
      }
    }, {})
  }
  return {}
}

function getFilteredTestCases (currSelectedRules: SelectedRuleGroup[], ruleGroup: RuleGroup[], listTestCases: TestCase[]): string[] {
  let filteredCases = listTestCases
  currSelectedRules.forEach(g => {
    const match: string[] = []
    const currRule = ruleGroup.find(o => o.Name === g.Name)?.Rules
    const mapitems = getMapCategories(currRule, AllNode.value)
    const cleanSelected = g.Selected.map(e => {
      if (e.indexOf('%') > 0) {
        return e.substring(0, e.indexOf('%'))
      } else {
        return e
      }
    })
    const unique = [...new Set(cleanSelected)]
    unique.forEach(s => {
      match.push(...(mapitems[s] ?? []))
    })

    filteredCases = filteredCases.filter(x => {
      return x.Category && x.Category.some(r => match.includes(r))
    })
  })

  return filteredCases.map(e => { return e.Name })
}

function getItems (rules: Rule[], parent: string): string[] {
  const results: string[] = []
  rules.forEach(rule => {
    const curr = parent ? `${parent}.${rule.Name}` : rule.Name
    if (rule.Rules != null) {
      results.push(...getItems(rule.Rules, curr))
    }
    if (rule.MappingRules != null) {
      rule.MappingRules.map(c => { results.push(`${curr}%${c}`) })
    }
    results.push(curr)
  })
  return results
}
