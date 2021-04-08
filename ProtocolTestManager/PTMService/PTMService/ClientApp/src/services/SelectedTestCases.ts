// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FetchService, RequestMethod } from '.';
import { SelectedTestCasesActions, SelectedTestCasesActionTypes } from '../actions/SelectedTestCasesActions';
import { RunRequest } from '../model/RunRequest';
import { AppThunkAction } from '../store/configureStore';

export const SelectedTestCasesDataSrv = {
    getAllTestCases: (): AppThunkAction<SelectedTestCasesActionTypes> => async (dispatch, getState) => {
        const state = getState();

        let configurationId = state.configurations.selectedConfiguration?.Id;
        await FetchService({
            url: `api/configuration/${configurationId}/test`,
            method: RequestMethod.GET,
            dispatch,
            onRequest: SelectedTestCasesActions.getAllTestCasesAction_Request,
            onComplete: SelectedTestCasesActions.getAllTestCasesAction_Success,
            onError: SelectedTestCasesActions.getAllTestCasesAction_Failure
        });
    },
    createRunRequest: (requestedTestCases: string[], completeCallback: (data: any) => void): AppThunkAction<SelectedTestCasesActionTypes> => async (dispatch, getState) => {
        const state = getState();

        let configurationId = state.configurations.selectedConfiguration?.Id;
        const runRequest: RunRequest = {
            ConfigurationId: configurationId!,
            SelectedTestCases: requestedTestCases
        };
        await FetchService({
            url: `api/run`,
            method: RequestMethod.POST,
            dispatch,
            body: JSON.stringify(runRequest),
            onRequest: SelectedTestCasesActions.createRunRequestAction_Request,
            onComplete: SelectedTestCasesActions.createRunRequestAction_Success,
            onError: SelectedTestCasesActions.createRunRequestAction_Failure,
        }).then(completeCallback);
    },
    abortRunRequest: (testResultId: number): AppThunkAction<SelectedTestCasesActionTypes> => async (dispatch, getState) => {
        await FetchService({
            url: `api/run/${testResultId}`,
            method: RequestMethod.PUT,
            dispatch,
            onRequest: SelectedTestCasesActions.abortRunRequestAction_Request,
            onComplete: SelectedTestCasesActions.abortRunRequestAction_Success,
            onError: SelectedTestCasesActions.abortRunRequestAction_Failure
        });
    },
};

