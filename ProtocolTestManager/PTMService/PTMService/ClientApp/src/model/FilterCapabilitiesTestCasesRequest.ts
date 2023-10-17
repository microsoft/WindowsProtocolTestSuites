// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { CapabilitiesTestCasesFilterType } from '../model/CapabilitiesFileInfo'

export interface FilterParams {
  TestCases: string[]
  FilterType: CapabilitiesTestCasesFilterType
  Filter: string
  TestSuiteName: string
  TestSuiteVersion: string
}

export interface FilterResult {
  TestCases: string[]
  FilterType: CapabilitiesTestCasesFilterType
  Filter: string
}
