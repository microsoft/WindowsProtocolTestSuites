// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
    GET_CAPABILITIES_FILES_REQUEST,
    GET_CAPABILITIES_FILES_SUCCESS,
    GET_CAPABILITIES_FILES_FAILURE,
    CREATE_CAPABILITIES_FILE_REQUEST,
    CREATE_CAPABILITIES_FILE_SUCCESS,
    CREATE_CAPABILITIES_FILE_FAILURE,
    UPDATE_CAPABILITIES_FILE_REQUEST,
    UPDATE_CAPABILITIES_FILE_SUCCESS,
    UPDATE_CAPABILITIES_FILE_FAILURE,
    REMOVE_CAPABILITIES_FILE_REQUEST,
    REMOVE_CAPABILITIES_FILE_SUCCESS,
    REMOVE_CAPABILITIES_FILE_FAILURE,
    SET_SEARCHTEXT,
    CapabilitiesFileActionTypes
} from '../actions/CapabilitiesAction'
import { CapabilitiesFileInfo } from '../model/CapabilitiesFileInfo'

export interface CapabilitiesFilesState {
    isLoading: boolean
    isProcessing: boolean
    errorMsg?: string
    capabilitiesConfigList: CapabilitiesFileInfo[]
    displayList: CapabilitiesFileInfo[]
    selectedTestSuite?: CapabilitiesFileInfo
    searchText?: string
}

const initialCapabilitiesFilesState: CapabilitiesFilesState = {
    isLoading: false,
    isProcessing: false,
    errorMsg: undefined,
    capabilitiesConfigList: [],
    displayList: [],
    selectedTestSuite: undefined,
    searchText: undefined
}

export const getCapabilitiesListReducer = (state = initialCapabilitiesFilesState, action: CapabilitiesFileActionTypes): CapabilitiesFilesState => {
    switch (action.type) {
        case GET_CAPABILITIES_FILES_REQUEST:
            return {
                ...state,
                isLoading: true,
                errorMsg: undefined,
                capabilitiesConfigList: []
            }

        case GET_CAPABILITIES_FILES_SUCCESS:
            return {
                ...state,
                isLoading: false,
                capabilitiesConfigList: action.payload,
                displayList: refineCapabilitiesList(action.payload, state.searchText)
            }

        case GET_CAPABILITIES_FILES_FAILURE:
            return {
                ...state,
                isLoading: false,
                capabilitiesConfigList: [],
                errorMsg: action.errorMsg
            }

        case CREATE_CAPABILITIES_FILE_REQUEST:
        case UPDATE_CAPABILITIES_FILE_REQUEST:
        case REMOVE_CAPABILITIES_FILE_REQUEST:
            return {
                ...state,
                isLoading: false,
                isProcessing: true,
                errorMsg: undefined
            }

        case CREATE_CAPABILITIES_FILE_SUCCESS:
        case UPDATE_CAPABILITIES_FILE_SUCCESS:
        case REMOVE_CAPABILITIES_FILE_SUCCESS:
            return {
                ...state,
                isLoading: false,
                isProcessing: false,
                errorMsg: undefined
            }

        case CREATE_CAPABILITIES_FILE_FAILURE:
        case UPDATE_CAPABILITIES_FILE_FAILURE:
        case REMOVE_CAPABILITIES_FILE_FAILURE:
            return {
                ...state,
                isLoading: false,
                isProcessing: false,
                errorMsg: action.errorMsg
            }

        case SET_SEARCHTEXT:
            return {
                ...state,
                isLoading: false,
                displayList: refineCapabilitiesList(state.capabilitiesConfigList, action.searchText)
            }

        default:
            return state
    }
}

function refineCapabilitiesList(originalList: CapabilitiesFileInfo[], searchText?: string): CapabilitiesFileInfo[] {
    originalList.forEach(c => { c.TestSuiteFullName = `${c.TestSuiteName} - ${c.TestSuiteVersion}` })

    if (!searchText) {
        return originalList
    }

    const lowerSearchText = searchText.toLowerCase()
    const newList = originalList.reduce((prevList: CapabilitiesFileInfo[], curr) => {
        if (curr.Name.toLowerCase().includes(lowerSearchText)) {
            return [...prevList, curr]
        } else {
            return prevList
        }
    }, [])

    return newList
}