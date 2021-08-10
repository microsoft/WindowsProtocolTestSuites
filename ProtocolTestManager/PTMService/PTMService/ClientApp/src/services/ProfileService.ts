// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FetchService, RequestMethod } from '.';
import { AppThunkAction } from '../store/configureStore';
import { ConfigureMethodActions, TestSuiteConfigureMethodActionTypes } from '../actions/ConfigureMethodAction';
import { ProfileUploadRequest } from "../model/ProfileUploadRequest";
import { ProfileExportRequest } from "../model/ProfileExportRequest";

const downloadProfile = (fileName: string, blob: Blob | undefined) => {
    if (blob === undefined) {
        return;
    }

    const url = window.URL.createObjectURL(new Blob([blob]));
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', fileName)
    link.click();
};

const getFileNameWithExtension = (id: string) => {
    let datetime = new Date();
    return `ProfileExport${datetime.getTime()}${id}.ptm`;
}

const getHeaders = () => {
    return { 'Accept': 'application/xml', 'Content-Type': 'application/json' };
}

export const ProfileDataSrv = {
    saveProfile: (selectedTestCases: string[]): AppThunkAction<TestSuiteConfigureMethodActionTypes> => async (dispatch, getState) => {
        const state = getState();
        const testsuiteId = state.testSuiteInfo.selectedTestSuite?.Id;
        const configurationId = state.configurations.selectedConfiguration?.Id

        const request: ProfileExportRequest = {
            TestSuiteId: testsuiteId!,
            ConfigurationId: configurationId!,
            SelectedTestCases: selectedTestCases
        };

        await FetchService({
            url: `api/testsuite/profile/export/`,
            headers: getHeaders(),
            method: RequestMethod.POST,
            body: JSON.stringify(request),
            dispatch,
            onRequest: ConfigureMethodActions.saveProfileAction_Request,
            onComplete: ConfigureMethodActions.saveProfileAction_Success,
            onError: ConfigureMethodActions.saveProfileAction_Failure
        }).then((data: Blob | undefined) =>  downloadProfile(getFileNameWithExtension(`${configurationId}`), data));
    },
    saveProfileByResultId: (testResultId: number): AppThunkAction<TestSuiteConfigureMethodActionTypes> => async (dispatch) => {
        await FetchService({
            url: `api/testsuite/${testResultId}/profile/export`,
            headers: getHeaders(),
            method: RequestMethod.POST,
            dispatch,
            onRequest: ConfigureMethodActions.saveProfileAction_Request,
            onComplete: ConfigureMethodActions.saveProfileAction_Success,
            onError: ConfigureMethodActions.saveProfileAction_Failure
        }).then((data: Blob | undefined) =>  downloadProfile(getFileNameWithExtension(`${testResultId}`), data));
    },
    importProfile: (request: ProfileUploadRequest, callback: (data: boolean) => void): AppThunkAction<TestSuiteConfigureMethodActionTypes> => async (dispatch) => {
        const postData = new FormData();
        postData.append('Package', request.Package);
        postData.append('TestSuiteId', request.TestSuiteId.toString());
        postData.append('ConfigurationId', request.ConfigurationId.toString());

        await FetchService({
            url: `api/testsuite/${request.TestSuiteId}/profile/${request.ConfigurationId}`,
            method: RequestMethod.POST,
            body: postData,
            headers: { },
            dispatch,
            onRequest: ConfigureMethodActions.importProfileAction_Request,
            onComplete: ConfigureMethodActions.importProfileAction_Success,
            onError: ConfigureMethodActions.importProfileAction_Failure,
        }).then(callback);
    }
};