// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from '.'
import { AutoDetectionActions, TestSuiteAutoDetectionActionTypes } from '../actions/AutoDetectionAction'
import { AutoDetectionState } from '../reducers/AutoDetectionReducer'
import { AppThunkAction } from '../store/configureStore'

export const AutoDetectionDataSrv = {
  getAutoDetectionPrerequisite: (): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/configuration/${configurationId}/autodetect/prerequisites`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: AutoDetectionActions.getAutoDetectionPrerequisiteAction_Request,
      onComplete: AutoDetectionActions.getAutoDetectionPrerequisiteAction_Success,
      onError: AutoDetectionActions.getAutoDetectionPrerequisiteAction_Failure
    })
  },
  getAutoDetectionSteps: (completeCallback: (autoDetection: AutoDetectionState) => void): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/configuration/${configurationId}/autodetect/detectionsteps`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: AutoDetectionActions.getAutoDetectionStepsAction_Request,
      onComplete: AutoDetectionActions.getAutoDetectionStepsAction_Success,
      onError: AutoDetectionActions.getAutoDetectionStepsAction_Failure,
      onCompleteCallback: () => {
        const currState = getState()
        completeCallback(currState.autoDetection)
      }
    })
  },
  updateAutoDetectionSteps: (completeCallback: (autoDetection: AutoDetectionState) => void): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/configuration/${configurationId}/autodetect/detectionsteps`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: AutoDetectionActions.updateAutoDetectionStepsAction_Request,
      onComplete: AutoDetectionActions.updateAutoDetectionStepsAction_Success,
      onError: AutoDetectionActions.updateAutoDetectionStepsAction_Failure,
      onCompleteCallback: () => {
        const currState = getState()
        completeCallback(currState.autoDetection)
      }
    })
  },
  getAutoDetectionLog: (completeCallback: () => void): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/configuration/${configurationId}/autodetect/log`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: AutoDetectionActions.getAutoDetectionLogAction_Request,
      onComplete: AutoDetectionActions.getAutoDetectionLogAction_Success,
      onError: AutoDetectionActions.getAutoDetectionLogAction_Failure,
      headers: { 'Content-Type': 'text/plain' },
      onCompleteCallback: async (logBlob: Blob | undefined) => {
        if (logBlob !== undefined) {
          const logText = await logBlob.text()
          dispatch(AutoDetectionActions.setAutoDetectionLogAction(logText))
          completeCallback()
        }
      }
    })
  },
  startAutoDetection: (completeCallback: () => void): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    const body = state.autoDetection.prerequisite?.Properties
    await FetchService({
      url: `api/configuration/${configurationId}/autodetect/start`,
      method: RequestMethod.POST,
      dispatch,
      onRequest: AutoDetectionActions.startAutoDetectionAction_Request,
      onComplete: AutoDetectionActions.startAutoDetectionAction_Success,
      onError: AutoDetectionActions.startAutoDetectionAction_Failure,
      body: JSON.stringify(body),
      onCompleteCallback: completeCallback
    })
  },
  stopAutoDetection: (): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/configuration/${configurationId}/autodetect/stop`,
      method: RequestMethod.POST,
      dispatch,
      onRequest: AutoDetectionActions.stopAutoDetectionAction_Request,
      onComplete: AutoDetectionActions.stopAutoDetectionAction_Success,
      onError: AutoDetectionActions.stopAutoDetectionAction_Failure
    })
  },
  applyDetectionResult: (completeCallback: () => void): AppThunkAction<TestSuiteAutoDetectionActionTypes> => async (dispatch, getState) => {
    console.log('applyDetectionResult')
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/configuration/${configurationId}/autodetect/apply`,
      method: RequestMethod.POST,
      dispatch,
      onRequest: AutoDetectionActions.applyAutoDetectionResultAction_Request,
      onComplete: AutoDetectionActions.applyAutoDetectionResultAction_Success,
      onError: AutoDetectionActions.applyAutoDetectionResultAction_Failure,
      onCompleteCallback: completeCallback
    })
  }
}
