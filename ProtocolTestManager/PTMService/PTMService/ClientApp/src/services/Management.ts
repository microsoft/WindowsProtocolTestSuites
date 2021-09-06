// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from '.'
import { ManagementActions, ManagementActionTypes } from '../actions/ManagementAction'
import { InstallRequest } from '../model/InstallRequest'
import { AppThunkAction } from '../store/configureStore'

export const ManagementDataSrv = {
  getTestSuiteList: (): AppThunkAction<ManagementActionTypes> => async (dispatch) => {
    await FetchService({
      url: 'api/testsuite',
      method: RequestMethod.GET,
      dispatch,
      onRequest: ManagementActions.getTestSuitesAction_Request,
      onComplete: ManagementActions.getTestSuitesAction_Success,
      onError: ManagementActions.getTestSuitesAction_Failure
    })
  },
  installTestSuite: (request: InstallRequest, callback: () => void): AppThunkAction<ManagementActionTypes> => async (dispatch) => {
    const postData = new FormData()
    postData.append('Package', request.Package)
    postData.append('TestSuiteName', request.TestSuiteName)
    if (request.Description) {
      postData.append('Description', request.Description)
    }

    await FetchService({
      url: 'api/management/testsuite',
      method: RequestMethod.POST,
      body: postData,
      headers: {},
      dispatch,
      onRequest: ManagementActions.installTestSuiteAction_Request,
      onComplete: ManagementActions.installTestSuiteAction_Success,
      onError: ManagementActions.installTestSuiteAction_Failure,
      onCompleteCallback: callback
    })
  },
  updateTestSuite: (id: number, request: InstallRequest, callback: () => void): AppThunkAction<ManagementActionTypes> => async (dispatch) => {
    const postData = new FormData()
    postData.append('Package', request.Package)
    postData.append('TestSuiteName', request.TestSuiteName)
    if (request.Description) {
      postData.append('Description', request.Description)
    }

    await FetchService({
      url: `api/management/testsuite/${id}`,
      method: RequestMethod.POST,
      body: postData,
      headers: {},
      dispatch,
      onRequest: ManagementActions.updateTestSuiteAction_Request,
      onComplete: ManagementActions.updateTestSuiteAction_Success,
      onError: ManagementActions.updateTestSuiteAction_Failure,
      onCompleteCallback: callback
    })
  },
  removeTestSuite: (id: number, callback: () => void): AppThunkAction<ManagementActionTypes> => async (dispatch) => {
    await FetchService({
      url: `api/management/testsuite/${id}`,
      method: RequestMethod.DELETE,
      headers: {},
      dispatch,
      onRequest: ManagementActions.removeTestSuiteAction_Request,
      onComplete: ManagementActions.removeTestSuiteAction_Success,
      onError: ManagementActions.removeTestSuiteAction_Failure,
      onCompleteCallback: callback
    })
  }
}
