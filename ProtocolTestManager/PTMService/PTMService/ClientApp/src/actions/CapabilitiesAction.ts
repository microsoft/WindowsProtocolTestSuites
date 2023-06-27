// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { CapabilitiesFileInfo } from '../model/CapabilitiesFileInfo'

// define action consts
export const GET_CAPABILITIES_FILES_REQUEST = 'CAPABILITIES/GET_CAPABILITIES_FILES_REQUEST'
export const GET_CAPABILITIES_FILES_SUCCESS = 'CAPABILITIES/GET_CAPABILITIES_FILES_SUCCESS'
export const GET_CAPABILITIES_FILES_FAILURE = 'CAPABILITIES/GET_CAPABILITIES_FILES_FAILURE'

export const CREATE_CAPABILITIES_FILE_REQUEST = 'CAPABILITIES/CREATE_CAPABILITIES_FILE_REQUEST'
export const CREATE_CAPABILITIES_FILE_SUCCESS = 'CAPABILITIES/CREATE_CAPABILITIES_FILE_SUCCESS'
export const CREATE_CAPABILITIES_FILE_FAILURE = 'CAPABILITIES/CREATE_CAPABILITIES_FILE_FAILURE'

export const UPDATE_CAPABILITIES_FILE_REQUEST = 'CAPABILITIES/UPDATE_CAPABILITIES_FILE_REQUEST'
export const UPDATE_CAPABILITIES_FILE_SUCCESS = 'CAPABILITIES/UPDATE_CAPABILITIES_FILE_SUCCESS'
export const UPDATE_CAPABILITIES_FILE_FAILURE = 'CAPABILITIES/UPDATE_CAPABILITIES_FILE_FAILURE'

export const REMOVE_CAPABILITIES_FILE_REQUEST = 'CAPABILITIES/REMOVE_CAPABILITIES_FILE_REQUEST'
export const REMOVE_CAPABILITIES_FILE_SUCCESS = 'CAPABILITIES/REMOVE_CAPABILITIES_FILE_SUCCESS'
export const REMOVE_CAPABILITIES_FILE_FAILURE = 'CAPABILITIES/REMOVE_CAPABILITIES_FILE_FAILURE'

export const SET_SEARCHTEXT = 'CAPABILITIES/SET_SEARCHTEXT'

// define action types
interface GetCapabilitiesFilesActionRequestType { type: typeof GET_CAPABILITIES_FILES_REQUEST }
interface GetCapabilitiesFilesActionSuccessType { type: typeof GET_CAPABILITIES_FILES_SUCCESS, payload: CapabilitiesFileInfo[] }
interface GetCapabilitiesFilesActionFailureType { type: typeof GET_CAPABILITIES_FILES_FAILURE, errorMsg: string }

interface CreateCapabilitiesFileActionRequestType { type: typeof CREATE_CAPABILITIES_FILE_REQUEST }
interface CreateCapabilitiesFileActionSuccessType { type: typeof CREATE_CAPABILITIES_FILE_SUCCESS, payload: number }
interface CreateCapabilitiesFileActionFailureType { type: typeof CREATE_CAPABILITIES_FILE_FAILURE, errorMsg: string }

interface UpdateCapabilitiesFileActionRequestType { type: typeof UPDATE_CAPABILITIES_FILE_REQUEST }
interface UpdateCapabilitiesFileActionSuccessType { type: typeof UPDATE_CAPABILITIES_FILE_SUCCESS }
interface UpdateCapabilitiesFileActionFailureType { type: typeof UPDATE_CAPABILITIES_FILE_FAILURE, errorMsg: string }

interface RemoveCapabilitiesFileActionRequestType { type: typeof REMOVE_CAPABILITIES_FILE_REQUEST }
interface RemoveCapabilitiesFileActionSuccessType { type: typeof REMOVE_CAPABILITIES_FILE_SUCCESS }
interface RemoveCapabilitiesFileActionFailureType { type: typeof REMOVE_CAPABILITIES_FILE_FAILURE, errorMsg: string }

interface SetSearchTextActionType { type: typeof SET_SEARCHTEXT, searchText: string }

export type CapabilitiesFileActionTypes = CreateCapabilitiesFileActionRequestType
| CreateCapabilitiesFileActionSuccessType
| CreateCapabilitiesFileActionFailureType
| GetCapabilitiesFilesActionRequestType
| GetCapabilitiesFilesActionSuccessType
| GetCapabilitiesFilesActionFailureType
| UpdateCapabilitiesFileActionRequestType
| UpdateCapabilitiesFileActionSuccessType
| UpdateCapabilitiesFileActionFailureType
| RemoveCapabilitiesFileActionRequestType
| RemoveCapabilitiesFileActionSuccessType
| RemoveCapabilitiesFileActionFailureType
| SetSearchTextActionType

// define actions
export const CapabilitiesActions = {
  getCapabilitiesFilesAction_Request: (): CapabilitiesFileActionTypes => {
    return {
      type: GET_CAPABILITIES_FILES_REQUEST
    }
  },
  getCapabilitiesFilesAction_Success: (capabilitiesFiles: CapabilitiesFileInfo[]): CapabilitiesFileActionTypes => {
    return {
      type: GET_CAPABILITIES_FILES_SUCCESS,
      payload: capabilitiesFiles
    }
  },
  getCapabilitiesFilesAction_Failure: (error: string): CapabilitiesFileActionTypes => {
    return {
      type: GET_CAPABILITIES_FILES_FAILURE,
      errorMsg: error
    }
  },
  createCapabilitiesFileAction_Request: (): CapabilitiesFileActionTypes => {
    return {
      type: CREATE_CAPABILITIES_FILE_REQUEST
    }
  },
  createCapabilitiesFileAction_Success: (id: number): CapabilitiesFileActionTypes => {
    return {
      type: CREATE_CAPABILITIES_FILE_SUCCESS,
      payload: id
    }
  },
  createCapabilitiesFileAction_Failure: (error: string): CapabilitiesFileActionTypes => {
    return {
      type: CREATE_CAPABILITIES_FILE_FAILURE,
      errorMsg: error
    }
  },
  updateCapabilitiesFileAction_Request: (): CapabilitiesFileActionTypes => {
    return {
      type: UPDATE_CAPABILITIES_FILE_REQUEST
    }
  },
  updateCapabilitiesFileAction_Success: (): CapabilitiesFileActionTypes => {
    return {
      type: UPDATE_CAPABILITIES_FILE_SUCCESS
    }
  },
  updateCapabilitiesFileAction_Failure: (error: string): CapabilitiesFileActionTypes => {
    return {
      type: UPDATE_CAPABILITIES_FILE_FAILURE,
      errorMsg: error
    }
  },
  removeCapabilitiesFileAction_Request: (): CapabilitiesFileActionTypes => {
    return {
      type: REMOVE_CAPABILITIES_FILE_REQUEST
    }
  },
  removeCapabilitiesFileAction_Success: (): CapabilitiesFileActionTypes => {
    return {
      type: REMOVE_CAPABILITIES_FILE_SUCCESS
    }
  },
  removeCapabilitiesFileAction_Failure: (error: string): CapabilitiesFileActionTypes => {
    return {
      type: REMOVE_CAPABILITIES_FILE_FAILURE,
      errorMsg: error
    }
  },
  setSearchTextAction: (filter: string): CapabilitiesFileActionTypes => {
    return {
      type: SET_SEARCHTEXT,
      searchText: filter
    }
  }
}
