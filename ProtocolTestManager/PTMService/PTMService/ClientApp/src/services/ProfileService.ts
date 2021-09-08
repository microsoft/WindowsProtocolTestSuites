// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FetchService, RequestMethod } from '.'
import { AppThunkAction } from '../store/configureStore'
import { ConfigureMethodActions, TestSuiteConfigureMethodActionTypes } from '../actions/ConfigureMethodAction'
import { ProfileUploadRequest } from '../model/ProfileUploadRequest'
import { ProfileExportRequest } from '../model/ProfileExportRequest'

const downloadProfile = (fileName: string, blob: Blob | undefined) => {
  if (blob === undefined) {
    return
  }

  const url = window.URL.createObjectURL(new Blob([blob]))
  const link = document.createElement('a')
  link.href = url
  link.setAttribute('download', fileName)
  link.click()
}

const getFileNameWithExtension = (id: string) => {
  const datetime = new Date()
  return `ProfileExported${datetime.getTime()}${id}.ptm`
}

export const ProfileDataSrv = {
  saveProfile: (testResultId: number, selectedTestCases?: string[]): AppThunkAction<TestSuiteConfigureMethodActionTypes> => async (dispatch) => {
    await FetchService({
      url: `api/testresult/${testResultId}/profile`,
      headers: { Accept: 'application/xml', 'Content-Type': 'application/json' },
      method: RequestMethod.POST,
      body: JSON.stringify({ SelectedTestCases: selectedTestCases } as ProfileExportRequest),
      dispatch,
      onRequest: ConfigureMethodActions.saveProfileAction_Request,
      onComplete: ConfigureMethodActions.saveProfileAction_Success,
      onError: ConfigureMethodActions.saveProfileAction_Failure,
      onCompleteCallback: (data: Blob | undefined) => downloadProfile(getFileNameWithExtension(`${testResultId}`), data)
    })
  },
  importProfile: (request: ProfileUploadRequest, callback: (data: boolean | undefined) => void): AppThunkAction<TestSuiteConfigureMethodActionTypes> => async (dispatch) => {
    const postData = new FormData()
    postData.append('Package', request.Package)

    await FetchService({
      url: `api/testsuite/${request.TestSuiteId}/profile?configurationId=${request.ConfigurationId}`,
      method: RequestMethod.POST,
      body: postData,
      headers: {},
      dispatch,
      onRequest: ConfigureMethodActions.importProfileAction_Request,
      onComplete: ConfigureMethodActions.importProfileAction_Success,
      onError: ConfigureMethodActions.importProfileAction_Failure,
      onCompleteCallback: callback
    })
  }
}
