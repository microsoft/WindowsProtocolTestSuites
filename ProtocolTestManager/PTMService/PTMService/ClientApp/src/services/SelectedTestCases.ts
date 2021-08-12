// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FetchService, RequestMethod } from '.';
import { SelectedTestCasesActions, SelectedTestCasesActionTypes } from '../actions/SelectedTestCasesAction';
import { RunRequest } from '../model/RunRequest';
import { AppThunkAction } from '../store/configureStore';

export const SelectedTestCasesDataSrv = {
    getAllTestCases: (): AppThunkAction<SelectedTestCasesActionTypes> => (dispatch, getState) => {
        const state = getState();

        if (state.filterInfo.isRulesLoading || state.filterInfo.isCasesLoading) {
            dispatch(SelectedTestCasesActions.getAllTestCasesAction_Request());
        }
        else {
            dispatch(SelectedTestCasesActions.getAllTestCasesAction_Success(state.filterInfo.listSelectedCases||[]));
        }
    },

    createRunRequest: (requestedTestCases: string[], configurationId?: number, completeCallback?: () => void): AppThunkAction<SelectedTestCasesActionTypes> => async (dispatch, getState) => {
        const state = getState();

        const confId = configurationId ?? state.configurations.selectedConfiguration?.Id;
        const runRequest: RunRequest = {
            ConfigurationId: confId!,
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
    abortRunRequest: (testResultId: number, completeCallback?: () => void): AppThunkAction<SelectedTestCasesActionTypes> => async (dispatch, getState) => {
        await FetchService({
            url: `api/run/${testResultId}`,
            method: RequestMethod.PUT,
            dispatch,
            onRequest: SelectedTestCasesActions.abortRunRequestAction_Request,
            onComplete: SelectedTestCasesActions.abortRunRequestAction_Success,
            onError: SelectedTestCasesActions.abortRunRequestAction_Failure
        }).then(completeCallback);
    },
};

