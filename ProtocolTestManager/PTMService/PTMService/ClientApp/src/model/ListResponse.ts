// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TestResultOverview } from './TestResult'

export interface ListResponse {
  PageCount: number
  TestResults: TestResultOverview[]
}
