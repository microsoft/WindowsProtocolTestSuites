// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Link, Stack, Pivot, PivotItem } from '@fluentui/react'
import { ConfigGroup, ConfigCategory, TestCasesViewsConfig } from '../model/CapabilitiesFileInfo'
import { useDispatch, useSelector } from 'react-redux'
import { AppState } from '../store/configureStore'
import { CapabilitiesConfigActions } from '../actions/CapabilitiesConfigAction'
import { CapabilitiesTestCasesTreePanel } from '../components/CapabilitiesTestCasesTreePanel'

export function CapabilitiesGroupsPanel (props: any) {
  const dispatch = useDispatch()
  const capabilitiesConfigState = useSelector((state: AppState) => state.capabilitiesConfig)

  const selectedGroup = capabilitiesConfigState.selectedGroup
  const selectedCategoryInGroup = capabilitiesConfigState.selectedCategoryInGroup
  const testCasesInfo = capabilitiesConfigState.testCasesInfo
  const selectedTestCasesInfo = capabilitiesConfigState.selectedTestCasesInfo

  const onGroupLinkClicked = (g: ConfigGroup): any => {
    dispatch(CapabilitiesConfigActions.selectCapabilitiesConfigGroup(g))
  }

  const onCategoryLinkClicked = (c: ConfigCategory): any => {
    dispatch(CapabilitiesConfigActions.selectCapabilitiesConfigCategory(c))
  }

  const onTestCasesSelected = (values: string[]): void => {
    dispatch(CapabilitiesConfigActions.selectCapabilitiesTestCases(values))
  }

  const onTestCasesViewPivoted = (item: PivotItem | undefined) => {
    if (item && item.props.itemKey) {
      const view = TestCasesViewsConfig.get(item.props.itemKey)
      dispatch(CapabilitiesConfigActions.selectCapabilitiesTestCasesView(view))
    }
  }

  const getGroupLinkColor = (g: ConfigGroup): string => {
    let color = '#525251'

    if (selectedGroup !== undefined) {
      if (selectedGroup.Name === g.Name) {
        color = '#005A9E'
      }
    }

    return color
  }

  const getCategoryLinkColor = (c: ConfigCategory): string => {
    let color = '#525251'

    if (selectedCategoryInGroup !== undefined) {
      if (selectedCategoryInGroup.Name === c.Name) {
        color = '#005A9E'
      }
    }

    return color
  }

  return (
        <div>
            <Stack style={{ paddingLeft: 10 }}>
                <Stack horizontal style={{ paddingBottom: 10 }}>
                  <div style={{ width: 240, fontWeight: 'bold' }}>Groups</div>

                  {
                      selectedGroup !== undefined
                        ? <div style={{ width: 240, paddingLeft: 10, fontWeight: 'bold' }}>
                          Categories
                          {
                             <span> (Group: {selectedGroup.Name})</span>
                          }
                      </div>
                        : ''
                  }

                  {
                      selectedCategoryInGroup !== undefined
                        ? <div style={{ paddingLeft: 10, fontWeight: 'bold' }}>
                              Test Cases
                              {
                                  <span> (Category: {selectedCategoryInGroup.Name})</span>
                              }
                          </div>
                        : ''
                  }

                </Stack>

                <Stack horizontal>
                  <div style={{ width: 240 }}>
                    {
                        capabilitiesConfigState.groups.length > 0
                          ? capabilitiesConfigState.groups.map((g, index) => {
                            return (
                                    <div key={index} style={{ alignSelf: 'start' }}>
                                      ~&nbsp;
                                      <Link style={{ fontSize: '14px', fontWeight: 'bold', color: getGroupLinkColor(g) }} onClick={() => onGroupLinkClicked(g)}>
                                            {g.Name}
                                      </Link>
                                    </div>
                            )
                          })
                          : <span style={{ fontSize: '13px' }}>No groups have been defined for this file.</span>
                    }
                    </div>

                  {
                      selectedGroup !== undefined
                        ? <div style={{ width: 240, paddingLeft: 10, overflowY: 'auto' }}>
                          <div style={{ borderLeft: '2px solid #bae7ff', paddingLeft: 20 }}>
                              {
                                  selectedGroup.Categories.length > 0
                                    ? selectedGroup.Categories.map((c, index) => {
                                      return (
                                              <div key={index} style={{ alignSelf: 'start' }}>
                                                  -&nbsp;
                                                  <Link style={{ fontSize: '14px', color: getCategoryLinkColor(c) }} onClick={() => onCategoryLinkClicked(c)}>
                                                      {c.Name}
                                                  </Link>
                                              </div>
                                      )
                                    })
                                    : <span style={{ fontSize: '13px' }}>No categories have been defined for this group.</span>
                              }
                          </div>
                      </div>
                        : ''
                  }

                  {
                      selectedCategoryInGroup !== undefined
                        ? <div style={{ paddingLeft: 10, overflowY: 'auto' }}>
                              <div style={{ borderLeft: '2px solid #bae7ff', paddingLeft: 20 }}>
                                  <Pivot linkFormat="tabs" onLinkClick={(item, _) => onTestCasesViewPivoted(item)}>
                                      <PivotItem headerText="in this category" itemIcon="Sunny" itemCount={testCasesInfo.TestsInCurrentCategory.length} itemKey="0">
                                          <CapabilitiesTestCasesTreePanel testCases={testCasesInfo.TestsInCurrentCategory} selectedTestCases={selectedTestCasesInfo.TestsInCurrentCategory} onChecked={(values) => onTestCasesSelected(values)} />
                                      </PivotItem>
                                      <PivotItem headerText="in other categories but not in this" itemIcon="SunAdd" itemCount={testCasesInfo.TestsInOtherCategories.length} itemKey="1">
                                          <CapabilitiesTestCasesTreePanel testCases={testCasesInfo.TestsInOtherCategories} selectedTestCases={selectedTestCasesInfo.TestsInOtherCategories} onChecked={(values) => onTestCasesSelected(values)} />
                                      </PivotItem>
                                      <PivotItem headerText="not in any category" itemIcon="WarningSolid" itemCount={testCasesInfo.TestsNotInAnyCategory.length} itemKey="2">
                                          <CapabilitiesTestCasesTreePanel testCases={testCasesInfo.TestsNotInAnyCategory} selectedTestCases={selectedTestCasesInfo.TestsNotInAnyCategory} onChecked={(values) => onTestCasesSelected(values)} />
                                      </PivotItem>
                                  </Pivot>
                              </div>
                           </div>
                        : ''
                  }
                </Stack>
          </Stack>
        </div>
  )
};
