// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


export const SET_CONFIGUREMETHOD_REQUEST = 'CONFIGUREMETHOD/SET_CONFIGUREMETHOD_REQUEST'

// define action types
interface SetTSConfigureMethodType { type: typeof SET_CONFIGUREMETHOD_REQUEST; selectedMethod: string; }

export type TestSuiteConfigureMethodActionTypes = SetTSConfigureMethodType;

// define actions
export const ConfigureMethodActions = {
    setConfigureMethod: (key: string): TestSuiteConfigureMethodActionTypes => {
        return {
            type: SET_CONFIGUREMETHOD_REQUEST,
            selectedMethod: key,
        }
    },
}