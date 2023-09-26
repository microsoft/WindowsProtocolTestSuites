// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  FilterTestCaseActionTypes, GET_FILTERTESTCASE_RULES_REQUEST, GET_FILTERTESTCASE_RULES_SUCCESS,
  GET_FILTERTESTCASE_RULES_FAILURE, SET_SELECTED_RULES, GET_TESTSUITETESTCASES_REQUEST,
  GET_TESTSUITETESTCASES_SUCCESS, GET_TESTSUITETESTCASES_FAILURE, SET_RULES_REQUEST, SET_RULES_SUCCESS,
  SET_RULES_FAILURE,
  SELECT_CAPABILITIES_FILE,
  SELECT_CAPABILITIES_FILE_CATEGORIES,
  EXPAND_CAPABILITIES_FILE_CATEGORIES,
  GET_CAPABILITIES_CONFIG_REQUEST,
  GET_CAPABILITIES_CONFIG_SUCCESS,
  GET_CAPABILITIES_CONFIG_FAILURE
} from '../actions/FilterTestCaseAction'
import {
  REMOVE_CAPABILITIES_FILE_SUCCESS,
  CapabilitiesFileActionTypes
} from '../actions/CapabilitiesAction'
import { Configuration } from '../model/Configuration'
import { AllNode, RuleGroup, SelectedRuleGroup, Rule } from '../model/RuleGroup'
import { TestCase } from '../model/TestCase'
import { ConfigGroup, TestCaseWithCategories, CapabilitiesConfig } from '../model/CapabilitiesFileInfo'

export interface FilterTestCaseState {
  isCasesLoading: boolean
  isRulesLoading: boolean
  isPosting: boolean
  errorMsg?: string
  ruleGroup: RuleGroup[]
  listTestCases: TestCase[]
  listTestCasesMap: Map<string, TestCase>
  selectedConfiguration?: Configuration
  listSelectedCases: TestCase[]
  selectedRules: SelectedRuleGroup[]
  targetFilterIndex: number
  mappingFilterIndex: number
  selectedCapabilitiesFileId: number
  groups: ConfigGroup[]
  testCasesByFileWithCategories: Map<number, Map<string, TestCaseWithCategories>>
  selectedCategoriesByFile: Map<number, string[]>
  selectedCategories: string[]
  expandedCategoriesByFile: Map<number, string[]>
}

const initialFilterTestCaseState: FilterTestCaseState = {
  isCasesLoading: false,
  isRulesLoading: false,
  isPosting: false,
  errorMsg: undefined,
  ruleGroup: [],
  listTestCases: [],
  listTestCasesMap: new Map<string, TestCase>(),
  selectedConfiguration: undefined,
  listSelectedCases: [],
  selectedRules: [],
  targetFilterIndex: -1,
  mappingFilterIndex: -1,
  selectedCapabilitiesFileId: -1,
  groups: [],
  testCasesByFileWithCategories: new Map<number, Map<string, TestCaseWithCategories>>(),
  selectedCategoriesByFile: new Map<number, string[]>(),
  selectedCategories: [],
  expandedCategoriesByFile: new Map<number, string[]>()
}

export const getFilterTestCaseReducer = (state = initialFilterTestCaseState, action: FilterTestCaseActionTypes | CapabilitiesFileActionTypes): FilterTestCaseState => {
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
        listSelectedCases: getFilteredTestCases(initialSelected, action.payload.AllRules, state.listTestCases, state)
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
    {
      const listTestCases = action.payload
      listTestCases.forEach(t => {
        state.listTestCasesMap.set(t.FullName.toLowerCase(), t)
      })

      return {
        ...state,
        isCasesLoading: false,
        listTestCases: listTestCases,
        listSelectedCases: getFilteredTestCases(state.selectedRules, state.ruleGroup, action.payload, state)
      }
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
        listSelectedCases: getFilteredTestCases(currSelectedRules, state.ruleGroup, state.listTestCases, state)
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
    case SELECT_CAPABILITIES_FILE:
    {
      const fileId = action.payload
      let listSelectedCases = state.listSelectedCases

      if (fileId === -1) {
        state.selectedCapabilitiesFileId = -1
        listSelectedCases =
              getFilteredTestCases(state.selectedRules, state.ruleGroup, state.listTestCases, state)
      }

      return {
        ...state,
        listSelectedCases: listSelectedCases,
        selectedCapabilitiesFileId: fileId
      }
    }
    case SELECT_CAPABILITIES_FILE_CATEGORIES:
    {
      const fileId = state.selectedCapabilitiesFileId
      const testCases =
          getFilteredTestCasesForCapabilitiesFile(state, action.payload)

      state.selectedCategoriesByFile.set(fileId, action.payload)
      return {
        ...state,
        selectedCategories: action.payload,
        listSelectedCases: testCases
      }
    }
    case EXPAND_CAPABILITIES_FILE_CATEGORIES:
    {
      const fileId = state.selectedCapabilitiesFileId
      state.expandedCategoriesByFile.set(fileId, action.payload)

      return {
        ...state
      }
    }
    case GET_CAPABILITIES_CONFIG_REQUEST:
      return {
        ...state,
        errorMsg: undefined,
        groups: []
      }
    case GET_CAPABILITIES_CONFIG_SUCCESS:
    {
      const fileId = state.selectedCapabilitiesFileId
      let categories = new Array<string>()
      let testCases = new Array<TestCase>()
      const [groups, testCasesWithCategories] =
        parseCapabilitiesConfigJson(action.payload, state.listTestCasesMap)

      if (fileId !== -1) {
        state.testCasesByFileWithCategories.set(fileId, testCasesWithCategories)

        // If there are currently no categories selected, pre-select categories based on
        // existing selected test cases. This is useful for transferring auto-detected test
        // cases to the capabilities file's categories.
        if (!state.selectedCategoriesByFile.has(fileId)) {
          categories = inferCategories(testCasesWithCategories, state.listTestCases)
          state.selectedCategoriesByFile.set(fileId, categories)
        } else {
          categories = state.selectedCategoriesByFile.get(fileId)!
        }

        testCases =
            categories.length === 0
              ? []
              : getFilteredTestCasesForCapabilitiesFile(state, categories)
      }

      return {
        ...state,
        groups: groups,
        selectedCategories: categories,
        listSelectedCases: testCases
      }
    }
    case GET_CAPABILITIES_CONFIG_FAILURE:
      return {
        ...state,
        errorMsg: action.errorMsg,
        groups: []
      }
    case REMOVE_CAPABILITIES_FILE_SUCCESS:
    {
      state.selectedCapabilitiesFileId = -1
      const listSelectedCases = getFilteredTestCases(state.selectedRules, state.ruleGroup, state.listTestCases, state)

      return {
        ...state,
        selectedCategories: [],
        listSelectedCases: listSelectedCases
      }
    }
    default:
      return state
  }
}

function getMappedCategories (rules: Rule[] | undefined, parent: string): { [key: string]: string[] | undefined } {
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

function inferCategories(testCasesWithCategories: Map<string, TestCaseWithCategories>,
  selectedTestCases: TestCase[]) {
  const inferredCategories = new Set<string>()
  selectedTestCases.forEach(t => {
    const testCaseNameLower = t.FullName.toLowerCase()
    const testCaseWithCategories = testCasesWithCategories.get(testCaseNameLower)
    if (testCaseWithCategories && testCaseWithCategories.LowerCaseCategories.size > 0) {
      testCaseWithCategories.LowerCaseCategories.forEach(c => {
        if (!inferredCategories.has(c)) {
          inferredCategories.add(c)
        }
      })
    }
  })

  return Array.from(inferredCategories)
}

function getFilteredTestCasesForCapabilitiesFile(state: FilterTestCaseState,
  selectedCategories: string[]) {
  const fileId = state.selectedCapabilitiesFileId
  const testCasesWithCategories = state.testCasesByFileWithCategories.get(fileId)
  const categories = selectedCategories.filter(p => !p.startsWith('('))
    .map(c => c.toLowerCase())
  const testCases =
        Array.from(testCasesWithCategories!
          .values())
          .filter(t => categories.some(c => t.LowerCaseCategories.has(c)))
          .map(t => t.Test)

  return testCases
}

function getFilteredTestCases (currSelectedRules: SelectedRuleGroup[], ruleGroup: RuleGroup[], listTestCases: TestCase[],
  state: FilterTestCaseState): TestCase[] {
  if (state.selectedCapabilitiesFileId !== -1) {
    const selectedCategories = state.selectedCategoriesByFile.get(state.selectedCapabilitiesFileId)

    return getFilteredTestCasesForCapabilitiesFile(state, selectedCategories!)
  }

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

function toPascalCase (value: string): string {
  if (!value) {
    return value
  }

  value = value.toLocaleLowerCase()
  const specialCases = new Map<string, string>()
  specialCases.set('testcases', 'TestCases')
  if (specialCases.has(value)) {
    return specialCases.get(value) ?? ''
  } else {
    return value.charAt(0).toUpperCase() + value.slice(1)
  }
}

function changeKeyCasing (current: any) {
  if (current && Array.isArray(current)) {
    const output: any = current.map(c => changeKeyCasing(c))
    return output
  } else if (current && typeof current === 'object') {
    const output: any = {}
    for (const [k, v] of Object.entries(current)) {
      const newKey = toPascalCase(k)
      output[newKey] = changeKeyCasing(v)
    }
    return output
  }

  return current
}

function parseCapabilitiesConfigJson (input: any, testCasesMap: Map<string, TestCase>):
[ConfigGroup[], Map<string, TestCaseWithCategories>] {
  const json = changeKeyCasing(JSON.parse(input.Json))

  const result: CapabilitiesConfig = json.Capabilities
  const testCasesWithCategories = new Map<string, TestCaseWithCategories>()

  result.TestCases.forEach(t => {
    const testCaseNameLower = t.Name.toLowerCase()
    const testCase = testCasesMap.get(testCaseNameLower)
    const categories = t.Categories.map(c => c.toLowerCase())
    testCasesWithCategories.set(testCaseNameLower,
      {
        Test: testCase!,
        LowerCaseCategories: new Set(categories)
      }
    )
  })

  return [result.Groups, testCasesWithCategories]
}
