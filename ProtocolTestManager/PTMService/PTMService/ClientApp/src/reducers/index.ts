// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { getAdapterReducer } from './ConfigureAdapterReducer';
import { getPropertyGroupsReducer } from './PropertyGroupsReducer';
import { getConfigurationReducer } from './TestSuiteConfigurationReducer';
import { getSelectedTestCasesReducer } from './SelectedTestCasesReducer';
import { getTestSuitesReducer } from './TestSuitesReducer';
import { getManagementReducer } from './ManagementReducer';
import { getConfigureMethodReducer } from './ConfigureMethodReducer';
import { getAutoDetectionReducer } from './AutoDetectionReducer';
import { getDetectionResultReducer } from './DetectionResultReducer';
import { getFilterTestCaseReducer } from './FilterTestCaseReducer';
import { getTestCaseResultReducer } from './TestCaseResultReducer';
import { getTestResultsReducer } from './TestResultsReducer';
import { getTestSuiteInfoReducer } from './TestSuiteInfoReducer';
import { getWizardNavBarReducer } from './WizardNavBarReducer';

export const appReducers = {
  testSuites: getTestSuitesReducer,
  testSuiteInfo: getTestSuiteInfoReducer,
  configurations: getConfigurationReducer,
  configureMethod: getConfigureMethodReducer,
  autoDetection: getAutoDetectionReducer,
  detectResult: getDetectionResultReducer,
  propertyGroups: getPropertyGroupsReducer,
  configureAdapter: getAdapterReducer,
  filterInfo: getFilterTestCaseReducer,
  selectedTestCases: getSelectedTestCasesReducer,
  testResults: getTestResultsReducer,
  testCaseResult: getTestCaseResultReducer,
  management: getManagementReducer,
  wizard: getWizardNavBarReducer
}
