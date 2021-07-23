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
            onComplete: AutoDetectActions.GetAutoDetectPrerequisiteAction_Success,
            onError: AutoDetectActions.GetAutoDetectPrerequisiteAction_Failure
        });
    },
    updateAutoDetectionPrerequisite: (): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
        const state = getState();
        const configurationId = state.configurations.selectedConfiguration?.Id;
        await FetchService({
            url: `api/testsuite/${configurationId}/autodetect/prerequisites`,
            method: RequestMethod.GET,
            dispatch,
            onRequest: AutoDetectActions.GetAutoDetectPrerequisiteAction_Request,
            onComplete: AutoDetectActions.GetAutoDetectPrerequisiteAction_Success,
            onError: AutoDetectActions.GetAutoDetectPrerequisiteAction_Failure
        });
    },

    getAutoDetectionSteps: (): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
        const state = getState();
        const configurationId = state.configurations.selectedConfiguration?.Id;
        await FetchService({
            url: `api/testsuite/${configurationId}/autodetect/detectionsteps`,
            method: RequestMethod.GET,
            dispatch,
            onRequest: AutoDetectActions.GetAutoDetectStepsAction_Request,
            onComplete: AutoDetectActions.GetAutoDetectStepsAction_Success,
            onError: AutoDetectActions.GetAutoDetectStepsAction_Failure
        });
    },

    startAutoDetection: (): AppThunkAction<FilterTestCaseActionTypes> => async (dispatch, getState) => {
        const state = getState();
        const configurationId = state.configurations.selectedConfiguration?.Id;
        await FetchService({
            url: `api/testsuite/${configurationId}/autodetect/start`,
            method: RequestMethod.POST,
            dispatch,
            onRequest: AutoDetectActions.PostAutoDetectStart_Request,
            onComplete: AutoDetectActions.PostAutoDetectStart_Success,
            onError: AutoDetectActions.PostAutoDetectStart_Failure
        });
    },

    stopAutoDetection: (): AppThunkAction<FilterTestCaseActionTypes> => async (dispatch, getState) => {
        const state = getState();
        const configurationId = state.configurations.selectedConfiguration?.Id;
        await FetchService({
            url: `api/testsuite/${configurationId}/autodetect/stop`,
            method: RequestMethod.POST,
            dispatch,
            onRequest: AutoDetectActions.PostAutoDetectStop_Request,
            onComplete: AutoDetectActions.PostAutoDetectStop_Success,
            onError: AutoDetectActions.PostAutoDetectStop_Failure
        });
    },
    pullAutoDetectionSteps: (): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
        const state = getState();
        const configurationId = state.configurations.selectedConfiguration?.Id;
        await FetchService({
            url: `api/testsuite/${configurationId}/autodetect/detectionsteps`,
            method: RequestMethod.GET,
            dispatch,
            onRequest: AutoDetectActions.StartPolling,
            onComplete: AutoDetectActions.GetAutoDetectStepsAction_Success,
            onError: AutoDetectActions.GetAutoDetectStepsAction_Failure
        });
    },
};
