/* eslint-disable @typescript-eslint/indent */
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from '.'
import { CapabilitiesActions, CapabilitiesFileActionTypes } from '../actions/CapabilitiesAction'
import { CreateCapabilitiesFileRequest } from '../model/CreateCapabilitiesFileRequest'
import { UpdateCapabilitiesFileRequest } from '../model/UpdateCapabilitiesFileRequest'
import { AppThunkAction } from '../store/configureStore'

export const CapabilitiesDataSrv = {
    getCapabilitiesFiles: (): AppThunkAction<CapabilitiesFileActionTypes> => async (dispatch) => {
        await FetchService({
            url: 'api/capabilities',
            method: RequestMethod.GET,
            dispatch,
            onRequest: CapabilitiesActions.getCapabilitiesFilesAction_Request,
            onComplete: CapabilitiesActions.getCapabilitiesFilesAction_Success,
            onError: CapabilitiesActions.getCapabilitiesFilesAction_Failure
        })
    },
    createCapabilitiesFile: (request: CreateCapabilitiesFileRequest, callback: () => void): AppThunkAction<CapabilitiesFileActionTypes> => async (dispatch) => {
        const postData = new FormData()
        postData.append('TestSuiteId', request.TestSuiteId?.toString())
        postData.append('CapabilitiesFileName', request.CapabilitiesFileName)
        if (request.CapabilitiesFileDescription) {
          postData.append('CapabilitiesFileDescription', request.CapabilitiesFileDescription)
        }

        await FetchService({
            url: 'api/management/capabilities',
            method: RequestMethod.POST,
            body: postData,
            headers: {},
            dispatch,
            onRequest: CapabilitiesActions.createCapabilitiesFileAction_Request,
            onComplete: CapabilitiesActions.createCapabilitiesFileAction_Success,
            onError: CapabilitiesActions.createCapabilitiesFileAction_Failure,
            onCompleteCallback: callback
        })
    },
    updateCapabilitiesFile: (id: number, request: UpdateCapabilitiesFileRequest, callback: () => void): AppThunkAction<CapabilitiesFileActionTypes> => async (dispatch) => {
        const postData = new FormData()
        postData.append('CapabilitiesFileName', request.CapabilitiesFileName)
        postData.append('CapabilitiesFileDescription', request.CapabilitiesFileDescription)

        await FetchService({
            url: `api/management/capabilities/${id}`,
            method: RequestMethod.POST,
            body: postData,
            headers: {},
            dispatch,
            onRequest: CapabilitiesActions.updateCapabilitiesFileAction_Request,
            onComplete: CapabilitiesActions.updateCapabilitiesFileAction_Success,
            onError: CapabilitiesActions.updateCapabilitiesFileAction_Failure,
            onCompleteCallback: callback
        })
    },
    removeCapabilitiesFile: (id: number, callback: () => void): AppThunkAction<CapabilitiesFileActionTypes> => async (dispatch) => {
        await FetchService({
            url: `api/management/capabilities/${id}`,
            method: RequestMethod.DELETE,
            headers: {},
            dispatch,
            onRequest: CapabilitiesActions.removeCapabilitiesFileAction_Request,
            onComplete: CapabilitiesActions.removeCapabilitiesFileAction_Success,
            onError: CapabilitiesActions.removeCapabilitiesFileAction_Failure,
            onCompleteCallback: callback
        })
    },
    downloadCapabilitiesFile: (id: number) => {
        const url = `api/capabilities/download/${id}`
        const link = document.createElement('a')
        link.href = url
        link.click()
    }
}
