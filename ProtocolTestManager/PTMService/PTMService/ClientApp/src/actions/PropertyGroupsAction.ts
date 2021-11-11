// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Property } from '../model/Property'
import { PropertyGroup } from '../model/PropertyGroup'

// define action consts
export const GET_PROPERTYGROUPS_REQUEST = 'PROPERTYGROUPS/GET_PROPERTYGROUPS_REQUEST'
export const GET_PROPERTYGROUPS_SUCCESS = 'PROPERTYGROUPS/GET_PROPERTYGROUPS_SUCCESS'
export const GET_PROPERTYGROUPS_FAILURE = 'PROPERTYGROUPS/GET_PROPERTYGROUPS_FAILURE'

export const UPDATE_EDITINGPROPERTYGROUP = 'PROPERTYGROUPS/UPDATE_EDITINGPROPERTYGROUP'
export const SET_EDITINGPROPERTYGROUP = 'PROPERTYGROUPS/SET_EDITINGPROPERTYGROUP'
export const UPDATE_PROPERTYGROUPS = 'PROPERTYGROUPS/UPDATE_PROPERTYGROUPS'

export const SET_UPDATED = 'PROPERTYGROUPS/SET_UPDATED'

export const SET_PROPERTYGROUPS_REQUEST = 'PROPERTYGROUPS/SET_PROPERTYGROUPS_REQUEST'
export const SET_PROPERTYGROUPS_SUCCESS = 'PROPERTYGROUPS/SET_PROPERTYGROUPS_SUCCESS'
export const SET_PROPERTYGROUPS_FAILURE = 'PROPERTYGROUPS/SET_PROPERTYGROUPS_FAILURE'

// define action types
interface GetPropertyGroupsActionRequestType { type: typeof GET_PROPERTYGROUPS_REQUEST }
interface GetPropertyGroupsActionSuccessType { type: typeof GET_PROPERTYGROUPS_SUCCESS, payload: PropertyGroup[] }
interface GetPropertyGroupsActionFailureType { type: typeof GET_PROPERTYGROUPS_FAILURE, errorMsg: string }

interface UpdateEditingPropertyGroupActionType { type: typeof UPDATE_EDITINGPROPERTYGROUP, payload: Property };
interface SetEditingPropertyGroupActionType { type: typeof SET_EDITINGPROPERTYGROUP, payload: number }
interface UpdatePropertyGroupsActionType { type: typeof UPDATE_PROPERTYGROUPS }

interface SetUpdatedActionType { type: typeof SET_UPDATED, payload: boolean }

interface SetPropertyGroupsActionRequestType { type: typeof SET_PROPERTYGROUPS_REQUEST }
interface SetPropertyGroupsActionSuccessType { type: typeof SET_PROPERTYGROUPS_SUCCESS }
interface SetPropertyGroupsActionFailureType { type: typeof SET_PROPERTYGROUPS_FAILURE, errorMsg: string }

export type PropertyGroupsActionTypes =
  GetPropertyGroupsActionRequestType |
  GetPropertyGroupsActionSuccessType |
  GetPropertyGroupsActionFailureType |
  UpdateEditingPropertyGroupActionType |
  SetEditingPropertyGroupActionType |
  UpdatePropertyGroupsActionType |
  SetUpdatedActionType |
  SetPropertyGroupsActionRequestType |
  SetPropertyGroupsActionSuccessType |
  SetPropertyGroupsActionFailureType

// define actions
export const PropertyGroupsActions = {
  getPropertyGroupsAction_Request: (): PropertyGroupsActionTypes => {
    return {
      type: GET_PROPERTYGROUPS_REQUEST
    }
  },
  getPropertyGroupsAction_Success: (propertyGroups: PropertyGroup[]): PropertyGroupsActionTypes => {
    return {
      type: GET_PROPERTYGROUPS_SUCCESS,
      payload: propertyGroups
    }
  },
  getPropertyGroupsAction_Failure: (error: string): PropertyGroupsActionTypes => {
    return {
      type: GET_PROPERTYGROUPS_FAILURE,
      errorMsg: error
    }
  },
  updatedEditingPropertyGroupAction: (property: Property): PropertyGroupsActionTypes => {
    return {
      type: UPDATE_EDITINGPROPERTYGROUP,
      payload: property
    }
  },
  setEditingPropertyGroupAction: (index: number): PropertyGroupsActionTypes => {
    return {
      type: SET_EDITINGPROPERTYGROUP,
      payload: index
    }
  },
  updatePropertyGroupsAction: (): PropertyGroupsActionTypes => {
    return {
      type: UPDATE_PROPERTYGROUPS
    }
  },
  setUpdatedAction: (updated: boolean): PropertyGroupsActionTypes => {
    return {
      type: SET_UPDATED,
      payload: updated
    }
  },
  setPropertyGroupsAction_Request: (): PropertyGroupsActionTypes => {
    return {
      type: SET_PROPERTYGROUPS_REQUEST
    }
  },
  setPropertyGroupsAction_Success: (): PropertyGroupsActionTypes => {
    return {
      type: SET_PROPERTYGROUPS_SUCCESS
    }
  },
  setPropertyGroupsAction_Failure: (error: string): PropertyGroupsActionTypes => {
    return {
      type: SET_PROPERTYGROUPS_FAILURE,
      errorMsg: error
    }
  }
}
