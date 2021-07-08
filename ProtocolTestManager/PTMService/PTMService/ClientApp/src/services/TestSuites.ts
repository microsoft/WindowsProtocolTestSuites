// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from ".";
import { TestSuiteActions, TestSuitesActionTypes } from "../actions/TestSuitesActions";
import { FilterTestCaseActions, FilterTestCaseActionTypes } from "../actions/FilterTestCaseAction";
import { AutoDetectActions, TestSuiteAutoDetectionActionTypes } from "../actions/AutoDetectionAction";
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
    },
    getAutoDetectionPrerequisite: (): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
        const state = getState();
        const configurationId = state.configurations.selectedConfiguration?.Id;
        await FetchService({
            url: `api/testsuite/${configurationId}/autodetect/prerequisites`,
            method: RequestMethod.GET,
            dispatch,
            onRequest: AutoDetectActions.GetAutoDetectPrerequisiteAction_Request,
            onComplete: FilterTestCaseActions.getFilterRulesAction_Success,
            onError: FilterTestCaseActions.getFilterRuleAction_Failure
        });
    }
};
