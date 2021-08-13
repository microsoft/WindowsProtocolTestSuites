// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FetchService, RequestMethod } from '.'
import { TestCaseResultActions, TestCaseResultActionTypes } from '../actions/TestCaseResultAction'
import { AppThunkAction } from '../store/configureStore'

export const TestCaseResultDataSrv = {
  getTestCaseResult: (testCaseName: string): AppThunkAction<TestCaseResultActionTypes> => async (dispatch, getState) => {
    const state = getState()

    const testResultId = state.testResults.selectedTestResultId
    await FetchService({
      url: `api/testresult/${testResultId}/testcase?testCaseName=${testCaseName}`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: TestCaseResultActions.getTestCaseResultAction_Request,
      onComplete: TestCaseResultActions.getTestCaseResultAction_Success,
      onError: TestCaseResultActions.getTestCaseResultAction_Failure
    })
  }
}
