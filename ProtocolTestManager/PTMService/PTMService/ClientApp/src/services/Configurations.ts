// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from '.'
import { ConfigurationActions, TestSuiteConfigurationActionTypes } from '../actions/TestSuiteConfigurationAction'
import { FilterTestCaseActions, FilterTestCaseActionTypes } from '../actions/FilterTestCaseAction'
import { Configuration } from '../model/Configuration'
import { AppThunkAction } from '../store/configureStore'
import { AllNode } from '../model/RuleGroup'

export const ConfigurationsDataSrv = {
  getConfigurations: (testsuiteId: number): AppThunkAction<TestSuiteConfigurationActionTypes> => async (dispatch) => {
    // const state = getState();
    await FetchService({
      url: `api/configuration?testsuiteId=${testsuiteId}`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: ConfigurationActions.getConfigurationAction_Request,
      onComplete: ConfigurationActions.getConfigurationAction_Success,
      onError: ConfigurationActions.getConfigurationAction_Failure
    })
  },
  createConfiguration: (configuration: Configuration): AppThunkAction<TestSuiteConfigurationActionTypes> => async (dispatch) => {
    // const state = getState();
    await FetchService({
      url: 'api/configuration',
      method: RequestMethod.POST,
      body: JSON.stringify(configuration),
      dispatch,
      onRequest: ConfigurationActions.createConfiguration_Request,
      onComplete: ConfigurationActions.createConfiguration_Success,
      onError: ConfigurationActions.createConfiguration_Failure
    })
  },
  getRules: (): AppThunkAction<FilterTestCaseActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    await FetchService({
      url: `api/configuration/${configurationId}/rule`,
      method: RequestMethod.GET,
      dispatch,
      onRequest: FilterTestCaseActions.getFilterRuleAction_Request,
      onComplete: FilterTestCaseActions.getFilterRulesAction_Success,
      onError: FilterTestCaseActions.getFilterRuleAction_Failure
    })
  },
  setRules: (completeCallback: () => void): AppThunkAction<FilterTestCaseActionTypes> => async (dispatch, getState) => {
    const state = getState()
    const configurationId = state.configurations.selectedConfiguration?.Id
    const selectedRules = state.filterInfo.selectedRules
    const data = state.filterInfo.ruleGroup.map(g => {
      const curr = selectedRules.find(s => s.Name === g.Name)
      const rules = curr?.Selected.map(r => { return { Name: r.replace(AllNode.value, g.Name) } })
      return { Name: g.Name, Rules: rules }
    })
    await FetchService({
      url: `api/configuration/${configurationId}/rule`,
      method: RequestMethod.PUT,
      body: JSON.stringify(data),
      dispatch,
      onRequest: FilterTestCaseActions.setRulesAction_Request,
      onComplete: FilterTestCaseActions.setRulesAction_Success,
      onError: FilterTestCaseActions.setRulesAction_Failure,
      onCompleteCallback: completeCallback
    })
  }
}
