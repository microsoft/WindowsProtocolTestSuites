// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TestCase } from './TestCase'

export interface CapabilitiesFileInfo {
  Id: number
  Name: string
  Description?: string
  TestSuiteName: string
  TestSuiteVersion: string
  TestSuiteFullName: string
}

export interface CapabilitiesFile extends CapabilitiesFileInfo {
  TestCases?: TestCase[]
}
