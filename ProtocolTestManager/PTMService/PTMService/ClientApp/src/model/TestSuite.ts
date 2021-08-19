// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TestCase } from './TestCase'

export interface TestSuite {
  Id: number
  Name: string
  Version: string
  Description?: string
  Removed: boolean
  TestCases?: TestCase[]
}
