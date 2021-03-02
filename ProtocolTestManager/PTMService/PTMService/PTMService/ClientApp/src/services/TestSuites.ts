// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from ".";
import { TestSuiteActions, TestSuitesActionTypes } from "../actions/TestSuitesActions";
import { AppThunkAction } from "../store/configureStore";


export const TestSuitesActionCreators = {
    getTestSuiteList: (): AppThunkAction<TestSuitesActionTypes> => async (dispatch, getState) => {
        await FetchService('api/testsuite',
            RequestMethod.GET,
            () => {
                dispatch(TestSuiteActions.getTestSuitesAction_Request())
            },
            (data: any) => {
                dispatch(TestSuiteActions.getTestSuitesAction_Success(data))
            },
            (error: any) => {
                dispatch(TestSuiteActions.getTestSuitesAction_Failure(error))
            })
    }
};

