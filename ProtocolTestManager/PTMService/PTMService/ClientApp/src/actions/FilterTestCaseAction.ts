// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RuleGroup, SelectedRuleGroup } from "../model/RuleGroup";

// define action consts
export const GET_FILTERTESTCASE_RULES_REQUEST = 'FILTERTESTCASE/GET_CONFIGURATION_RULES_REQUEST';
export const GET_FILTERTESTCASE_RULES_SUCCESS = 'FILTERTESTCASE/GET_CONFIGURATION_RULES_SUCCESS';
export const GET_FILTERTESTCASE_RULES_FAILURE = 'FILTERTESTCASE/GET_CONFIGURATION_RULES_FAILURE';

export const SET_SELECTED_RULES = 'FILTERTESTCASE/SET_SELECTED_RULES'; 
export const SET_SELECTED_TESTCASES = 'FILTERTESTCASE/SET_SELECTED_TESTCASES';

// define action types
interface GetTSRulesActionRequestType { type: typeof GET_FILTERTESTCASE_RULES_REQUEST; }
interface GetTSRulesActionSuccessType { type: typeof GET_FILTERTESTCASE_RULES_SUCCESS; payload: RuleGroup[]; }
interface GetTSRulesActionFailureType { type: typeof GET_FILTERTESTCASE_RULES_FAILURE; errorMsg: string; }

interface SetSelectedRulesActionType { type: typeof SET_SELECTED_RULES; payload: SelectedRuleGroup; }
interface SetSelectedTestCasesActionType { type: typeof SET_SELECTED_TESTCASES; payload: string[]; }
export type FilterTestCaseActionTypes = GetTSRulesActionRequestType
    | GetTSRulesActionSuccessType
    | GetTSRulesActionFailureType
    | SetSelectedRulesActionType
    | SetSelectedTestCasesActionType;

// define actions
export const FilterTestCaseActions = {    
    getFilterRulesAction_Success: (groups: RuleGroup[]): FilterTestCaseActionTypes => {
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
    setSelectedRuleAction:(info:SelectedRuleGroup):FilterTestCaseActionTypes=> {
        return {
            type: SET_SELECTED_RULES,
            payload: info
        }
    },
    setAffectedRules_Success:(info:SelectedRuleGroup):FilterTestCaseActionTypes=> {
        return {
            type: SET_SELECTED_RULES,
            payload: info
        }
    },
    setTestCasesAction_Success:(testCases:string[]):FilterTestCaseActionTypes=> {
        return {
            type: SET_SELECTED_TESTCASES,
            payload: testCases
        }
    }
}