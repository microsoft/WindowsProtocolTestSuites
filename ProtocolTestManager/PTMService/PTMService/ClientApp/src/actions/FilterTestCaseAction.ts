// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RuleData, SelectedRuleGroup } from '../model/RuleGroup'
import { TestCase } from '../model/TestCase'
import { TestSuite } from '../model/TestSuite'

// define action consts
export const GET_FILTERTESTCASE_RULES_REQUEST = 'FILTERTESTCASE/GET_CONFIGURATION_RULES_REQUEST'
export const GET_FILTERTESTCASE_RULES_SUCCESS = 'FILTERTESTCASE/GET_CONFIGURATION_RULES_SUCCESS'
export const GET_FILTERTESTCASE_RULES_FAILURE = 'FILTERTESTCASE/GET_CONFIGURATION_RULES_FAILURE'

export const GET_TESTSUITETESTCASES_REQUEST = 'FILTERTESTCASE/GET_TESTSUITETESTCASES_REQUEST'
export const GET_TESTSUITETESTCASES_SUCCESS = 'FILTERTESTCASE/GET_TESTSUITETESTCASES_SUCCESS'
export const GET_TESTSUITETESTCASES_FAILURE = 'FILTERTESTCASE/GET_TESTSUITETESTCASES_FAILURE'

export const SET_RULES_REQUEST = 'FILTERTESTCASE/SET_RULES_REQUEST'
export const SET_RULES_SUCCESS = 'FILTERTESTCASE/SET_RULES_SUCCESS'
export const SET_RULES_FAILURE = 'FILTERTESTCASE/SET_RULES_FAILURE'

export const SET_SELECTED_RULES = 'FILTERTESTCASE/SET_SELECTED_RULES'

// define action types
interface GetTSRulesActionRequestType { type: typeof GET_FILTERTESTCASE_RULES_REQUEST }
interface GetTSRulesActionSuccessType { type: typeof GET_FILTERTESTCASE_RULES_SUCCESS, payload: RuleData }
interface GetTSRulesActionFailureType { type: typeof GET_FILTERTESTCASE_RULES_FAILURE, errorMsg: string }

interface GetTestSuiteTestCasesActionRequestType { type: typeof GET_TESTSUITETESTCASES_REQUEST }
interface GetTestSuiteTestCasesActionSuccessType { type: typeof GET_TESTSUITETESTCASES_SUCCESS, payload: TestCase[] }
interface GetTestSuiteTestCasesActionFailureType { type: typeof GET_TESTSUITETESTCASES_FAILURE, errorMsg: string }

interface SetTSRulesActionRequestType { type: typeof SET_RULES_REQUEST }
interface SetTSRulesActionSuccessType { type: typeof SET_RULES_SUCCESS }
interface SetTSRulesActionFailureType { type: typeof SET_RULES_FAILURE, errorMsg: string }

interface SetSelectedRulesActionType { type: typeof SET_SELECTED_RULES, payload: SelectedRuleGroup }

export type FilterTestCaseActionTypes = GetTSRulesActionRequestType
| GetTSRulesActionSuccessType
| GetTSRulesActionFailureType
| GetTestSuiteTestCasesActionRequestType
| GetTestSuiteTestCasesActionSuccessType
| GetTestSuiteTestCasesActionFailureType
| SetTSRulesActionRequestType
| SetTSRulesActionSuccessType
| SetTSRulesActionFailureType
| SetSelectedRulesActionType

// define actions
export const FilterTestCaseActions = {
  getFilterRulesAction_Success: (groups: RuleData): FilterTestCaseActionTypes => {
    return {
      type: GET_FILTERTESTCASE_RULES_SUCCESS,
      payload: groups
    }
  },
  getFilterRuleAction_Request: (): FilterTestCaseActionTypes => {
    return {
      type: GET_FILTERTESTCASE_RULES_REQUEST
    }
  },
  getFilterRuleAction_Failure: (error: string): FilterTestCaseActionTypes => {
    return {
      type: GET_FILTERTESTCASE_RULES_FAILURE,
      errorMsg: error
    }
  },
  getTestSuiteTestCasesAction_Request: (): FilterTestCaseActionTypes => {
    return {
      type: GET_TESTSUITETESTCASES_REQUEST
    }
  },
  getTestSuiteTestCasesAction_Success: (testSuite: TestSuite): FilterTestCaseActionTypes => {
    return {
      type: GET_TESTSUITETESTCASES_SUCCESS,
      payload: testSuite.TestCases ?? []
    }
  },
  getTestSuiteTestCasesAction_Failure: (error: string): FilterTestCaseActionTypes => {
    return {
      type: GET_TESTSUITETESTCASES_FAILURE,
      errorMsg: error
    }
  },
  setRulesAction_Request: (): FilterTestCaseActionTypes => {
    return {
      type: SET_RULES_REQUEST
    }
  },
  setRulesAction_Success: (): FilterTestCaseActionTypes => {
    return {
      type: SET_RULES_SUCCESS
    }
  },
  setRulesAction_Failure: (error: string): FilterTestCaseActionTypes => {
    return {
      type: SET_RULES_FAILURE,
      errorMsg: error
    }
  },
  setSelectedRuleAction: (info: SelectedRuleGroup): FilterTestCaseActionTypes => {
    return {
      type: SET_SELECTED_RULES,
      payload: info
    }
  }
}
