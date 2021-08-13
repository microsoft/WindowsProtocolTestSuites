// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from '.'
import { FilterTestCaseActionTypes } from '../actions/FilterTestCaseAction'
import { AutoDetectActions, TestSuiteAutoDetectionActionTypes } from '../actions/AutoDetectionAction'
import { AppThunkAction } from '../store/configureStore'

export const AutoDetectionDataSrv = {
  getAutoDetectionPrerequisite: (): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/testsuite/${configurationId}/autodetect/prerequisites`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: AutoDetectActions.GetAutoDetectPrerequisiteAction_Request,
      onComplete: AutoDetectActions.GetAutoDetectPrerequisiteAction_Success,
      onError: AutoDetectActions.GetAutoDetectPrerequisiteAction_Failure
    })
  },

  getAutoDetectionSteps: (): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/testsuite/${configurationId}/autodetect/detectionsteps`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: AutoDetectActions.GetAutoDetectStepsAction_Request,
      onComplete: AutoDetectActions.GetAutoDetectStepsAction_Success,
      onError: AutoDetectActions.GetAutoDetectStepsAction_Failure
    })
  },

  startAutoDetection: (): AppThunkAction<FilterTestCaseActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    const body = state.autoDetection.prerequisite?.Properties
    await FetchService({
      url: `api/testsuite/${configurationId}/autodetect/start`,
      method: RequestMethod.POST,
      dispatch,
      onRequest: AutoDetectActions.PostAutoDetectStart_Request,
      onComplete: AutoDetectActions.PostAutoDetectStart_Success,
      onError: AutoDetectActions.PostAutoDetectStart_Failure,
      body: JSON.stringify(body)
    })
  },

  stopAutoDetection: (): AppThunkAction<FilterTestCaseActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/testsuite/${configurationId}/autodetect/stop`,
      method: RequestMethod.POST,
      dispatch,
      onRequest: AutoDetectActions.PostAutoDetectStop_Request,
      onComplete: AutoDetectActions.PostAutoDetectStop_Success,
      onError: AutoDetectActions.PostAutoDetectStop_Failure
    })
  },
  pullAutoDetectionSteps: (): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/testsuite/${configurationId}/autodetect/detectionsteps`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: AutoDetectActions.StartPolling,
      onComplete: AutoDetectActions.GetAutoDetectStepsAction_Success,
      onError: AutoDetectActions.GetAutoDetectStepsAction_Failure
    })
  },
  applyDetectionResult: (completeCallback?: () => void): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
    console.log('applyDetectionResult')
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/testsuite/${configurationId}/autodetect/apply`,
      method: RequestMethod.POST,
      dispatch,
      onRequest: AutoDetectActions.ApplyDetectionResult_Request,
      onComplete: AutoDetectActions.ApplyDetectionResult_Success,
      onError: AutoDetectActions.ApplyDetectionResult_Failure
    }).then(completeCallback)
  }
}
