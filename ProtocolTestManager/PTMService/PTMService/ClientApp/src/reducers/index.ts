// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { getAdapterReducer } from "./ConfigureAdapterReducer";
import { getPropertyGroupsReducer } from "./PropertyGroupsReducer";
import { getConfigurationReducer } from "./TestSuiteConfigurationReducer";
import { getSelectedTestCasesReducer } from "./SelectedTestCasesReducer";
import { getTestSuitesReducer } from "./TestSuitesReducer";
import { getManagementReducer } from "./Management";
import { getConfigureMethodReducer } from "./ConfigureMethodReducer";
import { getFilterTestCaseReducer } from "./FilterTestCaseReducer";
import { getTestCaseResultReducer } from "./TestCaseResultReducer";
import { getTestResultsReducer } from "./TestResultsReducer";

export const appReducers = {
    testsuites: getTestSuitesReducer,
    configurations: getConfigurationReducer,
    configureMethod: getConfigureMethodReducer,
    // autoDetection:
    // detectResult:
    // filterTestCase:
    propertyGroups: getPropertyGroupsReducer,
    configureAdapter: getAdapterReducer,
    // runCase:
    filterInfo: getFilterTestCaseReducer,
    selectedTestCases: getSelectedTestCasesReducer,
    testResults: getTestResultsReducer,
    testCaseResult: getTestCaseResultReducer,
    management: getManagementReducer,
};
