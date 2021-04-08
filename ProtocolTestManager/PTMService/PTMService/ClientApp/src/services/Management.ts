// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from ".";
import { ManagementActions, ManagementActionTypes } from "../actions/ManagementAction";
import { InstallRequest } from "../model/InstallRequest";
import { AppThunkAction } from "../store/configureStore";


export const ManagementDataSrv = {
    getTestSuiteList: (): AppThunkAction<ManagementActionTypes> => async (dispatch) => {
        await FetchService({
            url: 'api/testsuite',
            method: RequestMethod.GET,
            dispatch,
            onRequest: ManagementActions.getTestSuitesAction_Request,
            onComplete: ManagementActions.getTestSuitesAction_Success,
            onError: ManagementActions.getTestSuitesAction_Failure
        });
    },
    installTestSuite: (request: InstallRequest, callback: () => void): AppThunkAction<ManagementActionTypes> => async (dispatch) => {
        const postData = new FormData();
        postData.append('Package', request.Package);
        postData.append('TestSuiteName', request.TestSuiteName);
        if (request.Description) {
            postData.append('Description', request.Description);
        }

        await FetchService({
            url: 'api/management/testsuite',
            method: RequestMethod.POST,
            body: postData,
            headers: { },
            dispatch,
            onRequest: ManagementActions.installTestSuite_Request,
            onComplete: ManagementActions.installTestSuite_Success,
            onError: ManagementActions.installTestSuite_Failure,
        }).then(callback);
    },
};
