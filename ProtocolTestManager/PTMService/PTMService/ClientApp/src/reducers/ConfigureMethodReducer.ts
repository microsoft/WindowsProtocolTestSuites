// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { SET_CONFIGUREMETHOD_REQUEST, TestSuiteConfigureMethodActionTypes } from "../actions/ConfigureMethodAction";

export interface ConfigureMethodState {
    isLoading: boolean;
    errorMsg?: string;
    selectedMethod?: string;
}

const initialConfigureMethodState: ConfigureMethodState = {
    isLoading: false,
    errorMsg: undefined,
    selectedMethod: undefined,
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

        default:
            return state;
    }
}
