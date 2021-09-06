// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from '.'
import { AdapterActions, TestSuiteAdapterActionTypes } from '../actions/ConfigureAdapterAction'
import { Adapter } from '../model/Adapter'
import { AppThunkAction } from '../store/configureStore'

export const AdapterDataSrv = {
  getAdapters: (configureId: number): AppThunkAction<TestSuiteAdapterActionTypes> => async (dispatch) => {
    // const state = getState();
    await FetchService({
      url: `api/configuration/${configureId}/adapter`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: AdapterActions.getAdapterAction_Request,
      onComplete: AdapterActions.getAdapterAction_Success,
      onError: AdapterActions.getAdapterAction_Failure
    })
  },
  setAdapters: (configureId: number, adapters: Adapter[], completeCallback: (data: any) => void): AppThunkAction<TestSuiteAdapterActionTypes> => async (dispatch) => {
    // const state = getState();
    await FetchService({
      url: `api/configuration/${configureId}/adapter`,
      method: RequestMethod.PUT,
      body: JSON.stringify(adapters),
      dispatch,
      onRequest: AdapterActions.setAdapterAction_Request,
      onComplete: AdapterActions.setAdapterAction_Success,
      onError: AdapterActions.setAdapterAction_Failure,
      onCompleteCallback: completeCallback
    })
  }
}
