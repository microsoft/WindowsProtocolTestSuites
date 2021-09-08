// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from '.'
import { DetectionResultActions, TestSuiteDetectionResultActionTypes } from '../actions/DetectionResultAction'
import { AppThunkAction } from '../store/configureStore'

export const DetectionResultSrv = {
  getDetectionResult: (): AppThunkAction<TestSuiteDetectionResultActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/configuration/${configurationId}/autodetect/summary`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: DetectionResultActions.getDetectionSummaryAction_Request,
      onComplete: DetectionResultActions.getDetectionSummaryAction_Success,
      onError: DetectionResultActions.getDetectionSummaryAction_Failure
    })
  }
}
