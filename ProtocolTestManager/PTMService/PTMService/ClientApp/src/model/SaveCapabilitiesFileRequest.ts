// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  ConfigGroup, ConfigMetadata
} from '../model/CapabilitiesFileInfo'

interface Metadata {
  testsuite: string
  version: string
}

interface Category {
  name: string
}

interface Group {
  name: string
  categories: Category[]
}

interface TestCase {
  name: string
  categories: string[]
}

interface Capabilities {
  metadata: Metadata
  groups: Group[]
  testcases: TestCase[]
}

interface CapabilitiesFile {
  capabilities: Capabilities
}

export interface SaveCapabilitiesFileRequest {
  CapabilitiesFileJson: string
}

export function buildCapabilitiesFileRequest (metadata: ConfigMetadata, groups: ConfigGroup[],
  testCasesInNoCategory: string[]): SaveCapabilitiesFileRequest {
  const groupsToSave: Group[] = []
  let testCasesToSave: TestCase[] = []

  const testCasesInCategories = new Map<string, TestCase>()
  groups.forEach(g => {
    const categories: Category[] = []

    g.Categories.forEach(c => {
      categories.push({
        name: c.Name
      })

      c.TestCases.forEach((value: string, key: string) => {
        if (testCasesInCategories.has(key)) {
          testCasesInCategories.get(key)!.categories.push(`${g.Name}.${c.Name}`)
        } else {
          testCasesInCategories.set(key,
            {
              name: value,
              categories: [`${g.Name}.${c.Name}`]
            })
        }
      })
    })

    groupsToSave.push({
      name: g.Name,
      categories: categories
    })
  })

  testCasesToSave = Array.from(testCasesInCategories.values())
  testCasesInNoCategory.forEach(t => {
    testCasesToSave.push({
      name: t,
      categories: []
    })
  })

  const capabilitiesFile: CapabilitiesFile = {
    capabilities: {
      metadata: {
        testsuite: metadata.Testsuite,
        version: metadata.Version
      },
      groups: groupsToSave,
      testcases: testCasesToSave
    }
  }

  const json = JSON.stringify(capabilitiesFile)

  return {
    CapabilitiesFileJson: json
  }
}
