// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TestCase } from './TestCase'

export type TestCasesViewType = 'InCategory' | 'OutCategory' | 'NoCategory' | undefined

export type CapabilitiesTestCasesFilterType = 'Name' | 'TestCategory' | 'Class'

export const TestCasesViewsConfig = new Map<string, TestCasesViewType>([
  ['0', 'InCategory'],
  ['1', 'OutCategory'],
  ['2', 'NoCategory']
])

export interface CapabilitiesFileInfo {
  Id: number
  Name: string
  Description?: string
  TestSuiteName: string
  TestSuiteVersion: string
  TestSuiteFullName: string
}

export interface NamedConfigHierarchy {
  Name: string
}

export interface ConfigCategory extends NamedConfigHierarchy {
  TestCases: Map<string, string>
}

export interface ConfigGroup extends NamedConfigHierarchy {
  Categories: ConfigCategory[]
}

export interface ConfigTestCase {
  Name: string
  Categories: string[]
  CategoriesCount: number
}

export interface ConfigTestCaseCategoryInfo {
  GroupName: string
  CategoryName: string
}

export interface ConfigTestCasesInfo{
  TestsInCurrentCategory: string[]
  TestsInOtherCategories: string[]
  TestsNotInAnyCategory: string[]
}

export interface ConfigMetadata {
  Testsuite: string
  Version: string
}

export interface CapabilitiesConfig {
  Metadata: ConfigMetadata
  Groups: ConfigGroup[]
  TestCases: ConfigTestCase[]
  TestCasesMap: Map<string, ConfigTestCase>
}

export interface TestCaseWithCategories {
  Test: TestCase
  LowerCaseCategories: Set<string>
}

export interface TestCaseFilterInfo {
  IsFiltered: boolean
  FilterBy: CapabilitiesTestCasesFilterType
  FilterText: string
  FilterOutput: string[]
}

export interface ConfigTestCasesFilterInfo {
  TestsInCurrentCategory: TestCaseFilterInfo
  TestsInOtherCategories: TestCaseFilterInfo
  TestsNotInAnyCategory: TestCaseFilterInfo
}

export interface JsonInfo {
  Name: string
  Json: string
}
