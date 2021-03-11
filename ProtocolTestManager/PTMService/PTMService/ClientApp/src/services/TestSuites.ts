// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from ".";
import { TestSuiteActions, TestSuitesActionTypes } from "../actions/TestSuitesActions";
import { AppThunkAction } from "../store/configureStore";


export const TestSuitesDataSrv = {
    getTestSuiteList: (): AppThunkAction<TestSuitesActionTypes> => async (dispatch, getState) => {
        // const state = getState();
        await FetchService({
            url: 'api/testsuite',
            method: RequestMethod.GET,
            dispatch,
            onRequest: TestSuiteActions.getTestSuitesAction_Request,
            onComplete: TestSuiteActions.getTestSuitesAction_Success,
            onError: TestSuiteActions.getTestSuitesAction_Failure
        });
    }
};
