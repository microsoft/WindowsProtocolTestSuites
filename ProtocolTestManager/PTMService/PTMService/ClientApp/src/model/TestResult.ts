// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TestCaseOverview } from './TestCaseResult'
import { TestSuite } from './TestSuite'

export type TestResultState = 'Created' | 'Running' | 'Failed' | 'Finished'

export interface TestResultOverview {
  Id: number
  Status: TestResultState
  ConfigurationId: number
  Total?: number
  NotRun?: number
  Running?: number
  Passed?: number
  Failed?: number
  Inconclusive?: number
}

export interface TestResult {
  Overview: TestResultOverview
  Results: TestCaseOverview[]
}

export interface TestResultSummary {
  TestSuite: TestSuite
  Configuration: string
}

export type ReportFormat = 'Plain' | 'Json' | 'XUnit'

export interface ReportRequest {
  TestCases: string[]
  Format: ReportFormat
}
