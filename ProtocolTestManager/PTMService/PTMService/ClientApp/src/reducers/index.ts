// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { getAdapterReducer } from "./ConfigureAdapterReducer";
import { getPropertyGroupsReducer } from "./PropertyGroupsReducer";
import { getConfigurationReducer } from "./TestSuiteConfigurationReducer";
import { getTestSuitesReducer } from "./TestSuitesReducer";

export const appReducers = {
    testsuites: getTestSuitesReducer,
    configurations: getConfigurationReducer,
    // autoDetection:
    // detectResult:
    // filterTestCase:
    propertyGroups: getPropertyGroupsReducer,
    configureAdapter: getAdapterReducer,
    // runCase:
};
