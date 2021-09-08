// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  SET_SELECTED_RULES,
  FilterTestCaseActionTypes
} from '../actions/FilterTestCaseAction'

import {
  GET_PROPERTYGROUPS_REQUEST,
  GET_PROPERTYGROUPS_SUCCESS,
  GET_PROPERTYGROUPS_FAILURE,
  UPDATE_EDITINGPROPERTYGROUP,
  SET_EDITINGPROPERTYGROUP,
  UPDATE_PROPERTYGROUPS,
  SET_UPDATED,
  SET_PROPERTYGROUPS_REQUEST,
  SET_PROPERTYGROUPS_SUCCESS,
  SET_PROPERTYGROUPS_FAILURE,
  PropertyGroupsActionTypes
} from '../actions/PropertyGroupsAction'
import { Property } from '../model/Property'
import { PropertyGroup } from '../model/PropertyGroup'

export interface PropertyGroupsState {
  isLoading: boolean
  errorMsg?: string
  updated: boolean
  isPosting: boolean
  latestPropertyGroups: PropertyGroup[]
  propertyGroups: PropertyGroup[]
  editingPropertyGroupIndex: number
  editingPropertyGroup: PropertyGroup | undefined
}

const initialPropertyGroupsState: PropertyGroupsState = {
  isLoading: false,
  errorMsg: undefined,
  updated: false,
  isPosting: false,
  latestPropertyGroups: [],
  propertyGroups: [],
  editingPropertyGroupIndex: 0,
  editingPropertyGroup: undefined
}

const getUpdatedEditingPropertyGroup = (state: PropertyGroupsState, updatedProperty: Property) => {
  const updatedItems = state.editingPropertyGroup!.Items.map((item) => {
    if (item.Name === updatedProperty.Name) {
      return updatedProperty
    }

    return item
  })

  return {
    ...state.editingPropertyGroup!,
    Items: updatedItems
  }
}

const getUpdatedPropertyGroups = (state: PropertyGroupsState) => {
  return state.propertyGroups.map((group) => {
    if (group.Name === state.editingPropertyGroup?.Name) {
      return state.editingPropertyGroup
    }

    return group
  })
}

export const getPropertyGroupsReducer = (state = initialPropertyGroupsState, action: PropertyGroupsActionTypes | FilterTestCaseActionTypes): PropertyGroupsState => {
  switch (action.type) {
    case SET_SELECTED_RULES:
      return {
        ...state,
        updated: false
      }

    case GET_PROPERTYGROUPS_REQUEST:
      return {
        ...state,
        isLoading: true,
        errorMsg: undefined,
        latestPropertyGroups: [],
        propertyGroups: [],
        editingPropertyGroupIndex: 0,
        editingPropertyGroup: undefined
      }

    case GET_PROPERTYGROUPS_SUCCESS:
      return {
        ...state,
        isLoading: false,
        latestPropertyGroups: action.payload,
        propertyGroups: action.payload,
        editingPropertyGroupIndex: 0,
        editingPropertyGroup: action.payload[0]
      }

    case GET_PROPERTYGROUPS_FAILURE:
      return {
        ...state,
        isLoading: false,
        errorMsg: action.errorMsg
      }

    case UPDATE_EDITINGPROPERTYGROUP:
      return {
        ...state,
        updated: true,
        editingPropertyGroup: getUpdatedEditingPropertyGroup(state, action.payload)
      }

    case SET_EDITINGPROPERTYGROUP:
      return {
        ...state,
        editingPropertyGroupIndex: action.payload,
        editingPropertyGroup: state.propertyGroups[action.payload]
      }

    case UPDATE_PROPERTYGROUPS:
      return {
        ...state,
        updated: true,
        propertyGroups: getUpdatedPropertyGroups(state)
      }

    case SET_UPDATED:
      return {
        ...state,
        updated: action.payload
      }

    case SET_PROPERTYGROUPS_REQUEST:
      return {
        ...state,
        isPosting: true,
        errorMsg: undefined
      }

    case SET_PROPERTYGROUPS_SUCCESS:
      return {
        ...state,
        isPosting: false
      }

    case SET_PROPERTYGROUPS_FAILURE:
      return {
        ...state,
        isPosting: false,
        errorMsg: action.errorMsg
      }

    default:
      return state
  }
}
