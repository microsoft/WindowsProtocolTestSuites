// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RequestMethod, FetchService } from ".";
import { ConfigurationActions, TestSuiteConfigurationActionTypes } from "../actions/TestSuiteConfigurationAction";
import { AppThunkAction } from "../store/configureStore";


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
    }
};
