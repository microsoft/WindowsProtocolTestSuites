// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FetchService, RequestMethod } from '.';
import { TestResultsActions, TestResultsActionTypes } from '../actions/TestResultsActions';
import { AppThunkAction } from '../store/configureStore';

export const TestResultsDataSrv = {
    listTestResults: (): AppThunkAction<TestResultsActionTypes> => async (dispatch, getState) => {
        const state = getState();

        const pageSize = state.testResults.pageSize;
        const pageNumber = state.testResults.pageNumber;
        await FetchService({
            url: `api/testresult?pageSize=${pageSize}&pageNumber=${pageNumber}`,
            method: RequestMethod.GET,
            dispatch,
            onRequest: TestResultsActions.listTestResultsAction_Request,
            onComplete: TestResultsActions.listTestResultsAction_Success,
            onError: TestResultsActions.listTestResultsAction_Failure
        });
    },
    getTestResultDetail: (testResultId: number): AppThunkAction<TestResultsActionTypes> => async (dispatch, getState) => {
        await FetchService({
            url: `api/testresult/${testResultId}`,
            method: RequestMethod.GET,
            dispatch,
            onRequest: TestResultsActions.getTestResultDetailAction_Request,
            onComplete: TestResultsActions.getTestResultDetailAction_Success,
            onError: TestResultsActions.getTestResultDetailAction_Failure
        });
    }
};