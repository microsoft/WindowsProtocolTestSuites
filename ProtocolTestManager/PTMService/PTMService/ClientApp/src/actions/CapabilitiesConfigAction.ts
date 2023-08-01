// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ConfigGroup, ConfigCategory, TestCasesViewType } from '../model/CapabilitiesFileInfo'

// define action consts
export const GET_CAPABILITIES_CONFIG_REQUEST = 'CAPABILITIES/GET_CAPABILITIES_CONFIG_REQUEST'
export const GET_CAPABILITIES_CONFIG_SUCCESS = 'CAPABILITIES/GET_CAPABILITIES_CONFIG_SUCCESS'
export const GET_CAPABILITIES_CONFIG_FAILURE = 'CAPABILITIES/GET_CAPABILITIES_CONFIG_FAILURE'

export const SELECT_CAPABILITIES_CONFIG_GROUP = 'CAPABILITIES/SELECT_CAPABILITIES_CONFIG_GROUP'
export const SELECT_CAPABILITIES_CONFIG_CATEGORY = 'CAPABILITIES/SELECT_CAPABILITIES_CONFIG_CATEGORY'

export const ADD_CAPABILITIES_CONFIG_GROUP = 'CAPABILITIES/ADD_CAPABILITIES_CONFIG_GROUP'
export const UPDATE_CAPABILITIES_CONFIG_GROUP = 'CAPABILITIES/UPDATE_CAPABILITIES_CONFIG_GROUP'
export const REMOVE_CAPABILITIES_CONFIG_GROUP = 'CAPABILITIES/REMOVE_CAPABILITIES_CONFIG_GROUP'
export const ADD_CAPABILITIES_CONFIG_CATEGORY = 'CAPABILITIES/ADD_CAPABILITIES_CONFIG_CATEGORY'
export const UPDATE_CAPABILITIES_CONFIG_CATEGORY = 'CAPABILITIES/UPDATE_CAPABILITIES_CONFIG_CATEGORY'
export const REMOVE_CAPABILITIES_CONFIG_CATEGORY = 'CAPABILITIES/REMOVE_CAPABILITIES_CONFIG_CATEGORY'

export const SELECT_CAPABILITIES_TESTCASES_VIEW = 'CAPABILITIES/SELECT_CAPABILITIES_TESTCASES_VIEW'
export const SELECT_CAPABILITIES_TESTCASES = 'CAPABILITIES/SELECT_CAPABILITIES_TESTCASES'

export const ADD_SELECTED_TESTCASES_TO_SELECTED_CATEGORY = 'CAPABILITIES/ADD_SELECTED_TESTCASES_TO_SELECTED_CATEGORY'
export const REMOVE_SELECTED_TESTCASES_FROM_SELECTED_CATEGORY = 'CAPABILITIES/REMOVE_SELECTED_TESTCASES_FROM_SELECTED_CATEGORY'

export const SAVE_CAPABILITIES_CONFIG_REQUEST = 'CAPABILITIES/SAVE_CAPABILITIES_CONFIG_REQUEST'
export const SAVE_CAPABILITIES_CONFIG_SUCCESS = 'CAPABILITIES/SAVE_CAPABILITIES_CONFIG_SUCCESS'
export const SAVE_CAPABILITIES_CONFIG_FAILURE = 'CAPABILITIES/SAVE_CAPABILITIES_CONFIG_FAILURE'

// define action types
interface GetCapabilitiesConfigActionRequestType { type: typeof GET_CAPABILITIES_CONFIG_REQUEST }
interface GetCapabilitiesConfigActionSuccessType { type: typeof GET_CAPABILITIES_CONFIG_SUCCESS, payload: string }
interface GetCapabilitiesConfigActionFailureType { type: typeof GET_CAPABILITIES_CONFIG_FAILURE, errorMsg: string }

interface SelectCapabilitiesConfigGroupActionType { type: typeof SELECT_CAPABILITIES_CONFIG_GROUP, payload: ConfigGroup }
interface SelectCapabilitiesConfigCategoryActionType { type: typeof SELECT_CAPABILITIES_CONFIG_CATEGORY, payload: ConfigCategory }

interface AddCapabilitiesConfigGroupActionType { type: typeof ADD_CAPABILITIES_CONFIG_GROUP, payload: string }
interface UpdateCapabilitiesConfigGroupActionType { type: typeof UPDATE_CAPABILITIES_CONFIG_GROUP, payload: string }
interface RemoveCapabilitiesConfigGroupActionType { type: typeof REMOVE_CAPABILITIES_CONFIG_GROUP, payload: string }
interface AddCapabilitiesConfigCategoryActionType { type: typeof ADD_CAPABILITIES_CONFIG_CATEGORY, payload: string }
interface UpdateCapabilitiesConfigCategoryActionType { type: typeof UPDATE_CAPABILITIES_CONFIG_CATEGORY, payload: string }
interface RemoveCapabilitiesConfigCategoryActionType { type: typeof REMOVE_CAPABILITIES_CONFIG_CATEGORY, payload: string }

interface SelectCapabilitiesTestCasesViewActionType { type: typeof SELECT_CAPABILITIES_TESTCASES_VIEW, payload: TestCasesViewType }
interface SelectCapabilitiesTestCasesActionType { type: typeof SELECT_CAPABILITIES_TESTCASES, payload: string[] }

interface AddSelectedTestCasesToSelectedCategoryActionType { type: typeof ADD_SELECTED_TESTCASES_TO_SELECTED_CATEGORY }

interface RemoveSelectedTestCasesFromSelectedCategoryActionType { type: typeof REMOVE_SELECTED_TESTCASES_FROM_SELECTED_CATEGORY }

interface SaveCapabilitiesConfigRequestActionType { type: typeof SAVE_CAPABILITIES_CONFIG_REQUEST }
interface SaveCapabilitiesConfigSuccessActionType { type: typeof SAVE_CAPABILITIES_CONFIG_SUCCESS }
interface SaveCapabilitiesConfigFailureActionType { type: typeof SAVE_CAPABILITIES_CONFIG_FAILURE, errorMsg: string }

export type CapabilitiesConfigActionTypes = GetCapabilitiesConfigActionRequestType
| GetCapabilitiesConfigActionSuccessType
| GetCapabilitiesConfigActionFailureType
| SelectCapabilitiesConfigGroupActionType
| SelectCapabilitiesConfigCategoryActionType
| AddCapabilitiesConfigGroupActionType
| UpdateCapabilitiesConfigGroupActionType
| RemoveCapabilitiesConfigGroupActionType
| AddCapabilitiesConfigCategoryActionType
| UpdateCapabilitiesConfigCategoryActionType
| RemoveCapabilitiesConfigCategoryActionType
| SelectCapabilitiesTestCasesViewActionType
| SelectCapabilitiesTestCasesActionType
| AddSelectedTestCasesToSelectedCategoryActionType
| RemoveSelectedTestCasesFromSelectedCategoryActionType
| SaveCapabilitiesConfigRequestActionType
| SaveCapabilitiesConfigSuccessActionType
| SaveCapabilitiesConfigFailureActionType

// define actions
export const CapabilitiesConfigActions = {
  getCapabilitiesConfigAction_Request: (): CapabilitiesConfigActionTypes => {
    return {
      type: GET_CAPABILITIES_CONFIG_REQUEST
    }
  },
  getCapabilitiesConfigAction_Success: (capabilitiesConfigJson: string): CapabilitiesConfigActionTypes => {
    return {
      type: GET_CAPABILITIES_CONFIG_SUCCESS,
      payload: capabilitiesConfigJson
    }
  },
  getCapabilitiesConfigAction_Failure: (error: string): CapabilitiesConfigActionTypes => {
    return {
      type: GET_CAPABILITIES_CONFIG_FAILURE,
      errorMsg: error
    }
  },
  selectCapabilitiesConfigGroup: (group: ConfigGroup): CapabilitiesConfigActionTypes => {
    return {
      type: SELECT_CAPABILITIES_CONFIG_GROUP,
      payload: group
    }
  },
  selectCapabilitiesConfigCategory: (category: ConfigCategory): CapabilitiesConfigActionTypes => {
    return {
      type: SELECT_CAPABILITIES_CONFIG_CATEGORY,
      payload: category
    }
  },
  addCapabilitiesConfigGroup: (groupName: string): CapabilitiesConfigActionTypes => {
    return {
      type: ADD_CAPABILITIES_CONFIG_GROUP,
      payload: groupName
    }
  },
  updateCapabilitiesConfigGroup: (groupName: string): CapabilitiesConfigActionTypes => {
    return {
      type: UPDATE_CAPABILITIES_CONFIG_GROUP,
      payload: groupName
    }
  },
  removeCapabilitiesConfigGroup: (groupName: string): CapabilitiesConfigActionTypes => {
    return {
      type: REMOVE_CAPABILITIES_CONFIG_GROUP,
      payload: groupName
    }
  },
  addCapabilitiesConfigCategory: (categoryName: string): CapabilitiesConfigActionTypes => {
    return {
      type: ADD_CAPABILITIES_CONFIG_CATEGORY,
      payload: categoryName
    }
  },
  updateCapabilitiesConfigCategory: (categoryName: string): CapabilitiesConfigActionTypes => {
    return {
      type: UPDATE_CAPABILITIES_CONFIG_CATEGORY,
      payload: categoryName
    }
  },
  removeCapabilitiesConfigCategory: (categoryName: string): CapabilitiesConfigActionTypes => {
    return {
      type: REMOVE_CAPABILITIES_CONFIG_CATEGORY,
      payload: categoryName
    }
  },
  selectCapabilitiesTestCasesView: (view: TestCasesViewType): CapabilitiesConfigActionTypes => {
    return {
      type: SELECT_CAPABILITIES_TESTCASES_VIEW,
      payload: view
    }
  },
  selectCapabilitiesTestCases: (selected: string[]): CapabilitiesConfigActionTypes => {
    return {
      type: SELECT_CAPABILITIES_TESTCASES,
      payload: selected
    }
  },
  addTestCasesToSelectedCategory: (): CapabilitiesConfigActionTypes => {
    return {
      type: ADD_SELECTED_TESTCASES_TO_SELECTED_CATEGORY
    }
  },
  removeSelectedTestCasesFromSelectedCategory: (): CapabilitiesConfigActionTypes => {
    return {
      type: REMOVE_SELECTED_TESTCASES_FROM_SELECTED_CATEGORY
    }
  },
  saveCapabilitiesConfigAction_Request: (): CapabilitiesConfigActionTypes => {
    return {
      type: SAVE_CAPABILITIES_CONFIG_REQUEST
    }
  },
  saveCapabilitiesConfigAction_Success: (): CapabilitiesConfigActionTypes => {
    return {
      type: SAVE_CAPABILITIES_CONFIG_SUCCESS
    }
  },
  saveCapabilitiesConfigAction_Failure: (error: string): CapabilitiesConfigActionTypes => {
    return {
      type: SAVE_CAPABILITIES_CONFIG_FAILURE,
      errorMsg: error
    }
  }
}
