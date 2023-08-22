/* eslint-disable @typescript-eslint/indent */
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
    GET_CAPABILITIES_CONFIG_REQUEST,
    GET_CAPABILITIES_CONFIG_SUCCESS,
    GET_CAPABILITIES_CONFIG_FAILURE,
    SELECT_CAPABILITIES_CONFIG_GROUP,
    SELECT_CAPABILITIES_CONFIG_CATEGORY,
    SELECT_CAPABILITIES_TESTCASES_VIEW,
    SELECT_CAPABILITIES_TESTCASES,
    ADD_CAPABILITIES_CONFIG_GROUP,
    UPDATE_CAPABILITIES_CONFIG_GROUP,
    REMOVE_CAPABILITIES_CONFIG_GROUP,
    ADD_CAPABILITIES_CONFIG_CATEGORY,
    UPDATE_CAPABILITIES_CONFIG_CATEGORY,
    REMOVE_CAPABILITIES_CONFIG_CATEGORY,
    ADD_SELECTED_TESTCASES_TO_SELECTED_CATEGORY,
    REMOVE_SELECTED_TESTCASES_FROM_SELECTED_CATEGORY,
    CapabilitiesConfigActionTypes,
    SAVE_CAPABILITIES_CONFIG_REQUEST,
    SAVE_CAPABILITIES_CONFIG_SUCCESS,
    SAVE_CAPABILITIES_CONFIG_FAILURE
} from '../actions/CapabilitiesConfigAction'
import {
    TestCasesViewType, CapabilitiesConfig, NamedConfigHierarchy, ConfigGroup, ConfigMetadata,
    ConfigCategory, ConfigTestCase, ConfigTestCaseCategoryInfo, ConfigTestCasesInfo
} from '../model/CapabilitiesFileInfo'
import { TestCase } from '../model/TestCase'
export interface CapabilitiesConfigState {
    isLoading: boolean
    isProcessing: boolean
    errorMsg?: string
    metadata: ConfigMetadata
    groups: ConfigGroup[]
    selectedGroup?: ConfigGroup
    categoriesInGroup: ConfigCategory[]
    selectedCategoryInGroup?: ConfigCategory
    testCases: Map<string, ConfigTestCase>
    testCasesInfo: ConfigTestCasesInfo
    selectedTestCasesInfo: ConfigTestCasesInfo
    selectedTestCasesInCurrentView: string[]
    testCasesView: TestCasesViewType
    hasUnsavedChanges: boolean
}

const initialCapabilitiesConfigState: CapabilitiesConfigState = {
    isLoading: false,
    isProcessing: false,
    errorMsg: undefined,
    metadata: {
        Testsuite: '',
        Version: ''
    },
    groups: [],
    selectedGroup: undefined,
    categoriesInGroup: [],
    selectedCategoryInGroup: undefined,
    testCases: new Map<string, ConfigTestCase>(),
    testCasesInfo: {
        TestsInCurrentCategory: [],
        TestsInOtherCategories: [],
        TestsNotInAnyCategory: []
    },
    selectedTestCasesInfo: {
        TestsInCurrentCategory: [],
        TestsInOtherCategories: [],
        TestsNotInAnyCategory: []
    },
    selectedTestCasesInCurrentView: [],
    testCasesView: 'InCategory',
    hasUnsavedChanges: false
}

const compare = (a: NamedConfigHierarchy, b: NamedConfigHierarchy): number => {
    const first = a?.Name ? a.Name.trim().toLowerCase() : ''
    const second = b?.Name ? b.Name.trim().toLowerCase() : ''
    if (first < second) {
        return -1
    } else if (first > second) {
        return 1
    } else {
        return 0
    }
}

const dedupeAndSort = (data: string[]): string[] => {
    return [...new Set(data)].sort()
}

const getConfigTestCasesInfo = (testCases: Map<string, ConfigTestCase>,
    selectedCategory: ConfigCategory | undefined): ConfigTestCasesInfo => {
    const testsNotInAnyCategory: string[] = []
    const testsInOtherCategories: string[] = []

    if (!selectedCategory) {
        return {
            TestsInCurrentCategory: [],
            TestsInOtherCategories: [],
            TestsNotInAnyCategory: []
        }
    }

    testCases.forEach((value: ConfigTestCase, key: string) => {
        if (value.CategoriesCount === 0) {
            testsNotInAnyCategory.push(value.Name)
        } else if (!selectedCategory.TestCases.has(key)) {
            testsInOtherCategories.push(value.Name)
        }
    })

    const result = {
        TestsInCurrentCategory: dedupeAndSort(Array.from(selectedCategory.TestCases.values())),
        TestsInOtherCategories: dedupeAndSort(testsInOtherCategories),
        TestsNotInAnyCategory: dedupeAndSort(testsNotInAnyCategory)
    }

    return result
}

const addSelectedTestCasesToCategory =
    (category: ConfigCategory | undefined, testCases: Map<string, ConfigTestCase>, selectedTestCases: string[]):
        [ConfigCategory | undefined, Map<string, ConfigTestCase>] => {
    if (category) {
        selectedTestCases.forEach(t => {
            const testCaseNameLower = t.toLowerCase().trim()

            if (!category.TestCases.has(testCaseNameLower)) {
               category.TestCases.set(testCaseNameLower, t)

                if (testCases.has(testCaseNameLower)) {
                    testCases.get(testCaseNameLower)!.CategoriesCount += 1
                }
            }
        })
    }

    return [category, testCases]
}

const removeSelectedTestCasesFromCategory =
    (category: ConfigCategory | undefined, testCases: Map<string, ConfigTestCase>, selectedTestCases: string[]):
        [ConfigCategory | undefined, Map<string, ConfigTestCase>] => {
        if (category) {
            selectedTestCases.forEach(t => {
                const testCaseNameLower = t.toLowerCase().trim()

                if (category.TestCases.has(testCaseNameLower)) {
                    category.TestCases.delete(testCaseNameLower)

                    if (testCases.has(testCaseNameLower)) {
                        testCases.get(testCaseNameLower)!.CategoriesCount -= 1
                    }
                }
            })
        }

        return [category, testCases]
    }

const removeGroup =
    (groupName: string, groups: ConfigGroup[], testCases: Map<string, ConfigTestCase>):
        [Map<string, ConfigTestCase>] => {
        const groupIndex = groups.findIndex(g =>
            g?.Name?.toLocaleLowerCase()?.trim() === groupName?.toLocaleLowerCase()?.trim()
        )
        if (groupIndex !== -1) {
            const group = groups[groupIndex]
            group.Categories.forEach(category => {
                category.TestCases.forEach((value: string, key: string) => {
                    if (testCases.has(key)) {
                        testCases.get(key)!.CategoriesCount -= 1
                    }
                })
            })

            groups.splice(groupIndex, 1)
        }

        return [testCases]
    }

const removeCategory =
    (categoryName: string, group: ConfigGroup | undefined, testCases: Map<string, ConfigTestCase>):
        [Map<string, ConfigTestCase>] => {
        if (group) {
            const categoryIndex = group?.Categories.findIndex(c =>
                c?.Name?.toLocaleLowerCase()?.trim() === categoryName?.toLocaleLowerCase()?.trim()
            )
            if (categoryIndex !== -1) {
                const category = group.Categories[categoryIndex]
                category.TestCases.forEach((value: string, key: string) => {
                    if (testCases.has(key)) {
                        testCases.get(key)!.CategoriesCount -= 1
                    }
                })

                group.Categories.splice(categoryIndex, 1)
            }
        }

        return [testCases]
    }

export const getCapabilitiesConfigReducer = (state = initialCapabilitiesConfigState, action: CapabilitiesConfigActionTypes): CapabilitiesConfigState => {
    switch (action.type) {
        case GET_CAPABILITIES_CONFIG_REQUEST:
            return {
                ...state,
                isLoading: true,
                errorMsg: undefined,
                groups: []
            }

        case GET_CAPABILITIES_CONFIG_SUCCESS:
            {
                const config = parseCapabilitiesConfigJson(action.payload)

                const selectedGroup = config.Groups.length > 0 ? config.Groups[0] : undefined
                const categoriesInGroup = config.Groups.length > 0 ? config.Groups[0].Categories.sort(compare) : []
                const selectedCategoryInGroup = categoriesInGroup.length > 0 ? categoriesInGroup[0] : undefined
                const testCasesInfo =
                    getConfigTestCasesInfo(config.TestCasesMap, selectedCategoryInGroup)

                return {
                    ...state,
                    isLoading: false,
                    errorMsg: undefined,
                    metadata: config.Metadata,
                    groups: config.Groups.sort(compare),
                    selectedGroup: selectedGroup,
                    categoriesInGroup: categoriesInGroup,
                    selectedCategoryInGroup: selectedCategoryInGroup,
                    testCases: config.TestCasesMap,
                    testCasesInfo: testCasesInfo,
                    hasUnsavedChanges: false
                }
            }

        case GET_CAPABILITIES_CONFIG_FAILURE:
            return {
                ...state,
                isLoading: false,
                errorMsg: action.errorMsg,
                groups: []
            }

        case SELECT_CAPABILITIES_CONFIG_GROUP:
            {
                const categoriesInGroup = action.payload.Categories.sort(compare)
                const selectedCategoryInGroup =
                    categoriesInGroup.length > 0 ? categoriesInGroup[0] : undefined
                const testCasesInfo = getConfigTestCasesInfo(state.testCases, selectedCategoryInGroup)

                return {
                    ...state,
                    errorMsg: undefined,
                    selectedGroup: action.payload,
                    categoriesInGroup: categoriesInGroup,
                    selectedCategoryInGroup: selectedCategoryInGroup,
                    testCasesInfo: testCasesInfo
                }
            }

        case SELECT_CAPABILITIES_CONFIG_CATEGORY:
            {
                const selectedCategoryInGroup = action.payload
                const testCasesInfo =
                    getConfigTestCasesInfo(state.testCases, selectedCategoryInGroup)

                return {
                    ...state,
                    errorMsg: undefined,
                    selectedCategoryInGroup: selectedCategoryInGroup,
                    testCasesInfo: testCasesInfo
                }
            }

        case SELECT_CAPABILITIES_TESTCASES:
            {
                let selected = state.selectedTestCasesInfo
                switch (state.testCasesView) {
                    case 'InCategory':
                        selected = {
                            ...state.selectedTestCasesInfo,
                            TestsInCurrentCategory: action.payload
                        }
                        break
                    case 'OutCategory':
                        selected = {
                            ...state.selectedTestCasesInfo,
                            TestsInOtherCategories: action.payload
                        }
                        break
                    case 'NoCategory':
                        selected = {
                            ...state.selectedTestCasesInfo,
                            TestsNotInAnyCategory: action.payload
                        }
                        break
                }

                return {
                    ...state,
                    errorMsg: undefined,
                    selectedTestCasesInfo: selected,
                    selectedTestCasesInCurrentView: action.payload
                }
            }

        case SELECT_CAPABILITIES_TESTCASES_VIEW:
            {
                return {
                    ...state,
                    errorMsg: undefined,
                    testCasesView: action.payload
                }
            }

        case ADD_CAPABILITIES_CONFIG_GROUP:
            {
                const newGroup = {
                    Name: action.payload,
                    Categories: []
                }

                return {
                    ...state,
                    errorMsg: undefined,
                    groups: [...state.groups, newGroup].sort(compare),
                    selectedGroup: newGroup,
                    categoriesInGroup: [],
                    selectedCategoryInGroup: undefined,
                    hasUnsavedChanges: true
                }
            }

        case UPDATE_CAPABILITIES_CONFIG_GROUP:
            {
                state.selectedGroup!.Name = action.payload

                return {
                    ...state,
                    errorMsg: undefined,
                    groups: [...state.groups].sort(compare),
                    hasUnsavedChanges: true
                }
            }

        case REMOVE_CAPABILITIES_CONFIG_GROUP:
            {
                const groups = state.groups
                const testCases = state.testCases
                const [updatedTestCases] =
                    removeGroup(action.payload, groups, testCases)

                const newSelectedGroup = state.groups.length > 0 ? state.groups[0] : undefined
                const categoriesInGroup = newSelectedGroup?.Categories.sort(compare) ?? []
                const newSelectedCategoryInGroup = categoriesInGroup.length > 0 ? categoriesInGroup[0] : undefined
                const testCasesInfo =
                    getConfigTestCasesInfo(updatedTestCases, newSelectedCategoryInGroup)

                return {
                    ...state,
                    errorMsg: undefined,
                    selectedGroup: newSelectedGroup,
                    selectedCategoryInGroup: newSelectedCategoryInGroup,
                    categoriesInGroup: categoriesInGroup,
                    testCases: updatedTestCases,
                    testCasesInfo: testCasesInfo,
                    hasUnsavedChanges: true
                }
            }

        case ADD_CAPABILITIES_CONFIG_CATEGORY:
            {
                const newCategory = {
                    Name: action.payload,
                    TestCases: new Map<string, string>()
                }

                const selectedCategoryInGroup = newCategory
                const testCasesInfo =
                    getConfigTestCasesInfo(state.testCases, selectedCategoryInGroup)

                if (state.selectedGroup) {
                    state.selectedGroup.Categories =
                        [...state.selectedGroup.Categories, newCategory].sort(compare)
                }
                return {
                    ...state,
                    errorMsg: undefined,
                    categoriesInGroup: [...state.categoriesInGroup, newCategory].sort(compare),
                    selectedCategoryInGroup: selectedCategoryInGroup,
                    testCasesInfo: testCasesInfo,
                    hasUnsavedChanges: true
                }
            }

        case UPDATE_CAPABILITIES_CONFIG_CATEGORY:
            {
                const selected =
                    state.selectedGroup?.Categories.find(c => c.Name === state.selectedCategoryInGroup!.Name)
                selected!.Name = action.payload
                state.selectedCategoryInGroup!.Name = action.payload

                return {
                    ...state,
                    errorMsg: undefined,
                    categoriesInGroup: [...state.categoriesInGroup].sort(compare),
                    hasUnsavedChanges: true
                }
            }

        case REMOVE_CAPABILITIES_CONFIG_CATEGORY:
            {
                const testCases = state.testCases
                const selectedGroup = state.selectedGroup
                const [updatedTestCases] =
                    removeCategory(action.payload, selectedGroup, testCases)

                const categoriesInGroup = selectedGroup?.Categories.sort(compare) ?? []
                const newSelectedCategoryInGroup =
                    categoriesInGroup.length > 0 ? categoriesInGroup[0] : undefined
                const testCasesInfo =
                    getConfigTestCasesInfo(updatedTestCases, newSelectedCategoryInGroup)

                return {
                    ...state,
                    errorMsg: undefined,
                    selectedCategoryInGroup: newSelectedCategoryInGroup,
                    categoriesInGroup: categoriesInGroup,
                    testCases: updatedTestCases,
                    testCasesInfo: testCasesInfo,
                    hasUnsavedChanges: true
                }
            }

        case ADD_SELECTED_TESTCASES_TO_SELECTED_CATEGORY:
            {
                const selectedTestCases = state.selectedTestCasesInCurrentView
                const selectedCategory = state.selectedCategoryInGroup
                const testCases = state.testCases

                const [updatedCategory, updatedTestCases] =
                    addSelectedTestCasesToCategory(selectedCategory, testCases, selectedTestCases)
                const testCasesInfo =
                    getConfigTestCasesInfo(updatedTestCases, updatedCategory)

                return {
                    ...state,
                    errorMsg: undefined,
                    selectedCategoryInGroup: updatedCategory,
                    testCases: updatedTestCases,
                    testCasesInfo: testCasesInfo,
                    hasUnsavedChanges: true
                }
            }

        case REMOVE_SELECTED_TESTCASES_FROM_SELECTED_CATEGORY:
            {
                const selectedTestCases = state.selectedTestCasesInCurrentView
                const selectedCategory = state.selectedCategoryInGroup
                const testCases = state.testCases

                const [updatedCategory, updatedTestCases] =
                    removeSelectedTestCasesFromCategory(selectedCategory, testCases, selectedTestCases)
                const testCasesInfo =
                    getConfigTestCasesInfo(updatedTestCases, updatedCategory)

                return {
                    ...state,
                    errorMsg: undefined,
                    selectedCategoryInGroup: updatedCategory,
                    testCases: updatedTestCases,
                    testCasesInfo: testCasesInfo,
                    hasUnsavedChanges: true
                }
            }

        case SAVE_CAPABILITIES_CONFIG_REQUEST:
            return {
                ...state,
                isLoading: true,
                errorMsg: undefined
            }

        case SAVE_CAPABILITIES_CONFIG_SUCCESS:
            {
                return {
                    ...state,
                    isLoading: false,
                    errorMsg: undefined,
                    hasUnsavedChanges: false
                }
            }

        case SAVE_CAPABILITIES_CONFIG_FAILURE:
            return {
                ...state,
                isLoading: false,
                errorMsg: action.errorMsg
            }

        default:
            return state
    }
}

function toPascalCase (value: string): string {
    if (!value) {
        return value
    }

    value = value.toLocaleLowerCase()
    const specialCases = new Map<string, string>()
    specialCases.set('testcases', 'TestCases')
    if (specialCases.has(value)) {
        return specialCases.get(value) ?? ''
    } else {
        return value.charAt(0).toUpperCase() + value.slice(1)
    }
}

function changeKeyCasing (current: any) {
    if (current && Array.isArray(current)) {
        const output: any = current.map(c => changeKeyCasing(c))
        return output
    } else if (current && typeof current === 'object') {
        const output: any = {}
        for (const [k, v] of Object.entries(current)) {
            const newKey = toPascalCase(k)
            output[newKey] = changeKeyCasing(v)
        }
        return output
    }

    return current
}

function getDepupedCategoryInfo(testCase: ConfigTestCase): ConfigTestCaseCategoryInfo[] {
    const map = new Map<string, ConfigTestCaseCategoryInfo>()

    testCase.Categories.forEach(c => {
        if (c) {
            const parts = c.split('.', 2)
            if (parts.length >= 2) {
                const groupName = parts[0]
                const categoryName = parts[1]
                const groupNameLower = groupName?.trim()?.toLocaleLowerCase()
                const categoryNameLower = categoryName?.trim()?.toLocaleLowerCase()

                const key = `${groupNameLower}.${categoryNameLower}`
                if (!map.has(key)) {
                    map.set(key, {
                        CategoryName: categoryName,
                        GroupName: groupName
                    })
                } else {
                    console.log(`category ${categoryName} in group, ${groupName} for test case, ${testCase.Name} is duplicated.`)
                }
            } else {
                console.log(`category, ${c} in test case, ${testCase.Name} is not properly formatted.`)
            }
        }
    })

    return Array.from(map.values())
}

function parseCapabilitiesConfigJson (input: string): CapabilitiesConfig {
    const json = changeKeyCasing(JSON.parse(input))

    const result: CapabilitiesConfig = json.Capabilities

    // Initialize test cases collection for each category.
    result.Groups.forEach(g => {
        g.Categories.forEach(c => {
            c.TestCases = new Map<string, string>()
        })
    })

    const groupsHash = new Map<string, Map<string, ConfigCategory>>()
    result.Groups.forEach(g => {
        const groupNameLower = g.Name?.trim()?.toLocaleLowerCase()
        groupsHash.set(groupNameLower, new Map<string, ConfigCategory>())
        g.Categories.forEach(gc => {
            const categoryNameLower = gc.Name?.trim()?.toLocaleLowerCase()
            groupsHash.get(groupNameLower)?.set(categoryNameLower, gc)
        })
    })

    result.TestCasesMap = new Map<string, ConfigTestCase>()
    result.TestCases.forEach(t => {
        // Initialize categories count for each test case.
        t.CategoriesCount = t.Categories.length

        // Build maps for test cases list on the categories and on the full test cases list on the root node.
        const testCaseNameLower = t.Name.trim().toLowerCase()
        const categoryInfo = getDepupedCategoryInfo(t)

        if (!result.TestCasesMap.has(testCaseNameLower)) {
            result.TestCasesMap.set(testCaseNameLower, t)
        }

        categoryInfo.forEach(c => {
            const groupNameLower = c.GroupName?.trim()?.toLocaleLowerCase()
            const categoryNameLower = c.CategoryName?.trim()?.toLocaleLowerCase()

            if (groupsHash.has(groupNameLower) &&
                groupsHash.get(groupNameLower)?.has(categoryNameLower)) {
                groupsHash.get(groupNameLower)?.get(categoryNameLower)?.TestCases.set(testCaseNameLower, t.Name)
            } else {
                console.log(`category, ${c.CategoryName} in group, ${c.GroupName} for test case, ${t.Name} could not be found.`)
            }
        })
    })

    return result
}
