/* eslint-disable @typescript-eslint/indent */
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from '.'
import { CapabilitiesActions, CapabilitiesFileActionTypes } from '../actions/CapabilitiesAction'
import { CapabilitiesConfigActions, CapabilitiesConfigActionTypes } from '../actions/CapabilitiesConfigAction'
import { CreateCapabilitiesFileRequest } from '../model/CreateCapabilitiesFileRequest'
import { UpdateCapabilitiesFileRequest } from '../model/UpdateCapabilitiesFileRequest'
import { FilterParams } from '../model/FilterCapabilitiesTestCasesRequest'
import { SaveCapabilitiesFileRequest } from '../model/SaveCapabilitiesFileRequest'
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
    getCapabilitiesConfig: (id: number): AppThunkAction<CapabilitiesConfigActionTypes> => async (dispatch) => {
        await FetchService({
            url: `api/capabilities/${id}`,
            method: RequestMethod.GET,
            dispatch,
            onRequest: CapabilitiesConfigActions.getCapabilitiesConfigAction_Request,
            onComplete: CapabilitiesConfigActions.getCapabilitiesConfigAction_Success,
            onError: CapabilitiesConfigActions.getCapabilitiesConfigAction_Failure
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
    },
    downloadFilteredCapabilitiesFile: (id: number) => {
        const url = `api/capabilities/download/filtered/${id}`
        const link = document.createElement('a')
        link.href = url
        link.click()
    },
    saveCapabilitiesFile: (id: number, request: SaveCapabilitiesFileRequest, callback: () => void): AppThunkAction<CapabilitiesFileActionTypes> => async (dispatch) => {
        const postData = new FormData()
        postData.append('CapabilitiesFileJson', request.CapabilitiesFileJson)

        await FetchService({
            url: `api/management/capabilities/save/${id}`,
            method: RequestMethod.POST,
            body: postData,
            headers: {},
            dispatch,
            onRequest: CapabilitiesConfigActions.saveCapabilitiesConfigAction_Request,
            onComplete: CapabilitiesConfigActions.saveCapabilitiesConfigAction_Success,
            onError: CapabilitiesConfigActions.saveCapabilitiesConfigAction_Failure,
            onCompleteCallback: callback
        })
    },
    filterCapabilitiesTestCases: (parameters: FilterParams, callback: () => void): AppThunkAction<CapabilitiesFileActionTypes> => async (dispatch) => {
        const postData = new FormData()
        postData.append('RequestJson', JSON.stringify(parameters))

        await FetchService({
            url: 'api/capabilities/filter',
            method: RequestMethod.POST,
            body: postData,
            headers: {},
            dispatch,
            onRequest: CapabilitiesConfigActions.filterTestCasesConfigAction_Request,
            onComplete: CapabilitiesConfigActions.filterTestCasesConfigAction_Success,
            onError: CapabilitiesConfigActions.filterTestCasesConfigAction_Failure,
            onCompleteCallback: callback
        })
    }
}
