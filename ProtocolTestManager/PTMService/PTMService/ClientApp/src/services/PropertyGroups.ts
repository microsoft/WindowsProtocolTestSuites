// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FetchService, RequestMethod } from '.'
import { PropertyGroupsActions, PropertyGroupsActionTypes } from '../actions/PropertyGroupsAction'
import { AppThunkAction } from '../store/configureStore'

export const PropertyGroupsDataSrv = {
  getPropertyGroups: (): AppThunkAction<PropertyGroupsActionTypes> => async (dispatch, getState) => {
    const state = getState()

    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/configuration/${configurationId}/property`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: PropertyGroupsActions.getPropertyGroupsAction_Request,
      onComplete: PropertyGroupsActions.getPropertyGroupsAction_Success,
      onError: PropertyGroupsActions.getPropertyGroupsAction_Failure
    })
  },
  setPropertyGroups: (completeCallback: () => void): AppThunkAction<PropertyGroupsActionTypes> => async (dispatch, getState) => {
    const state = getState()
    if (!state.propertyGroups.updated) {
      completeCallback()
      return
    }

    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/configuration/${configurationId}/property`,
      method: RequestMethod.PUT,
      dispatch,
      body: JSON.stringify(state.propertyGroups.propertyGroups),
      onRequest: PropertyGroupsActions.setPropertyGroupsAction_Request,
      onComplete: PropertyGroupsActions.setPropertyGroupsAction_Success,
      onError: PropertyGroupsActions.setPropertyGroupsAction_Failure,
      onCompleteCallback: completeCallback
    })
  }
}
