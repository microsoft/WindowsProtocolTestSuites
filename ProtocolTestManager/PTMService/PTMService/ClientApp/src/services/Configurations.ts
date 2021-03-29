// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from ".";
import { ConfigurationActions, TestSuiteConfigurationActionTypes } from "../actions/TestSuiteConfigurationAction";
import { FilterTestCaseActions, FilterTestCaseActionTypes } from "../actions/FilterTestCaseAction";
import { Configuration } from "../model/Configuration";
import { AppThunkAction } from "../store/configureStore";
import { SelectedRuleGroup } from "../model/RuleGroup";

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
        });
    },
    createConfiguration: (configuration: Configuration): AppThunkAction<TestSuiteConfigurationActionTypes> => async (dispatch) => {
        // const state = getState();
        await FetchService({
            url: `api/configuration`,
            method: RequestMethod.POST,
            body: JSON.stringify(configuration),
            dispatch,
            onRequest: ConfigurationActions.createConfiguration_Request,
            onComplete: ConfigurationActions.createConfiguration_Success,
            onError: ConfigurationActions.createConfiguration_Failure
        });
    },
    getRuleGroups: (): AppThunkAction<FilterTestCaseActionTypes> => async (dispatch, getState) => {
        const state = getState();
        const configurationId = state.configurations.selectedConfiguration?.Id
        await FetchService({
            url: `api/configuration/${configurationId}/rule`,
            method: RequestMethod.GET,
            dispatch,
            onRequest: FilterTestCaseActions.getFilterRuleAction_Request,
            onComplete: FilterTestCaseActions.getFilterRulesAction_Success,
            onError: FilterTestCaseActions.getFilterRuleAction_Failure
        });
    },
    setSelectedRule: (info: SelectedRuleGroup): AppThunkAction<FilterTestCaseActionTypes> => async (dispatch, getState) => {
        dispatch(FilterTestCaseActions.setSelectedRuleAction(info))

        const state = getState();
        const configurationId = state.configurations.selectedConfiguration?.Id
        // get map
        if (info.Name != "Priority") {
            let data = [{ Name: info.Name, rules:info.Selected.map(curr=>{return {Name:curr}}) }]

            await FetchService({
                url: `api/configuration/${configurationId}/rule`,
                method: RequestMethod.PUT,
                body: JSON.stringify(data),
                dispatch,
                onComplete: FilterTestCaseActions.setAffectedRules_Success,
                onError: FilterTestCaseActions.getFilterRuleAction_Failure
            })
        }

        await FetchService({
            url: `api/configuration/${configurationId}/test`,
            method: RequestMethod.GET,
            dispatch,
            onComplete: FilterTestCaseActions.setTestCasesAction_Success,
            onError: FilterTestCaseActions.getFilterRuleAction_Failure
        });
    }
};
