// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export interface FilterParams {
  TestCases: string[]
  FilterByCategory: boolean
  Filter: string
  TestSuiteName: string
  TestSuiteVersion: string
}

export interface FilterResult {
  TestCases: string[]
  FilterByCategory: boolean
  Filter: string
}
