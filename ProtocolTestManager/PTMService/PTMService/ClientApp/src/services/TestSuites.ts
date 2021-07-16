// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from ".";
import { TestSuiteActions, TestSuitesActionTypes } from "../actions/TestSuitesAction";
import { FilterTestCaseActions, FilterTestCaseActionTypes } from "../actions/FilterTestCaseAction";
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
    },
    getTestSuiteTestCases: (): AppThunkAction<FilterTestCaseActionTypes> => async (dispatch, getState) => {
        const state = getState();
        const testsuiteId = state.testsuites.selectedTestSuite?.Id
        await FetchService({
            url: `api/testsuite/${testsuiteId}`,
            method: RequestMethod.GET,
            dispatch,
            onRequest: FilterTestCaseActions.getTestSuiteTestCasesAction_Request,
            onComplete: FilterTestCaseActions.getTestSuiteTestCasesAction_Success,
            onError: FilterTestCaseActions.getTestSuiteTestCasesAction_Failure
        });
    }
};
