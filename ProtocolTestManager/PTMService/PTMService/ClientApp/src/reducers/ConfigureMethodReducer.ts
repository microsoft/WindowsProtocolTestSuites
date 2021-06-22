// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { 
    SET_CONFIGUREMETHOD_REQUEST, 
    TestSuiteConfigureMethodActionTypes,
    SAVE_PROFILE_REQUEST,
    SAVE_PROFILE_SUCCESS,
    SAVE_PROFILE_FAILURE,
    IMPORT_PROFILE_REQUEST,
    IMPORT_PROFILE_SUCCESS,
    IMPORT_PROFILE_FAILURE } from "../actions/ConfigureMethodAction";

export interface ConfigureMethodState {
    isLoading: boolean;
    errorMsg?: string;
    selectedMethod?: string;
    isPosting: boolean;
    isProfileImport: boolean;
    isProfileUploading: boolean;
    profileLocation?: any;
}

const initialConfigureMethodState: ConfigureMethodState = {
    isLoading: false,
    errorMsg: undefined,
    selectedMethod: undefined,
    isPosting: false,
    profileLocation: undefined,
    isProfileImport: false,
    isProfileUploading: false,
}

export const getConfigureMethodReducer = (state = initialConfigureMethodState, action: TestSuiteConfigureMethodActionTypes): ConfigureMethodState => {
    switch (action.type) {
        case SET_CONFIGUREMETHOD_REQUEST:
            return {
                ...state,
                isLoading: true,
                errorMsg: undefined,
                selectedMethod: action.selectedMethod,
            }
        
        case SAVE_PROFILE_REQUEST:
            return {
                ...state,
                isPosting: true,
                errorMsg: undefined
            }

        case SAVE_PROFILE_SUCCESS:
            return {
                ...state,
                isPosting: false,
                profileLocation: action.payload
            }

        case SAVE_PROFILE_FAILURE:
            return {
                ...state,
                isPosting: false,
                errorMsg: action.errorMsg
            }

        case IMPORT_PROFILE_REQUEST:
            return {
                ...state,
                isPosting: true,
                isProfileUploading: true,
                errorMsg: undefined
            }

        case IMPORT_PROFILE_SUCCESS:
            return {
                ...state,
                isPosting: false,
                isProfileUploading: false,
                isProfileImport: action.payload
            }

        case IMPORT_PROFILE_FAILURE:
            return {
                ...state,
                isPosting: false,
                isProfileUploading: false,
                isProfileImport: false,
                errorMsg: action.errorMsg
            }

        default:
            return state;
    }
}
