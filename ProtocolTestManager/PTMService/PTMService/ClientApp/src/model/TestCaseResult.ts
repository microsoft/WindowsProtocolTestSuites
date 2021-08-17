// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export type TestCaseState = 'NotRun' | 'Running' | 'Passed' | 'Failed' | 'Inconclusive'

export interface TestCaseOverview {
  FullName: string
  State: TestCaseState
}

export interface TestCaseResult {
  Overview: TestCaseOverview
  StartTime: string
  EndTime: string
  Output: string
}
