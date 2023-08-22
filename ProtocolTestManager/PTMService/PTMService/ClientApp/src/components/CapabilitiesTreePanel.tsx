// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FunctionComponent, useCallback, useState, useEffect, useRef } from 'react'
import CheckboxTree, { Node } from 'react-checkbox-tree'
import { Icon } from '@fluentui/react/lib/Icon'
import { ConfigGroup, ConfigCategory } from '../model/CapabilitiesFileInfo'

interface CapabilitiesTreePanelProps {
  groups: ConfigGroup[]
  selectedCategories: string[]
  expandedCategories: string[]
  onChecked: (values: string[]) => void
  onExpanded: (values: string[]) => void
}

const mapCategoryToNode = (group: ConfigGroup, category: ConfigCategory) => {
  return {
    value: `${group.Name.toLowerCase()}.${category.Name.toLowerCase()}`, label: category.Name, children: null, showCheckbox: true
  }
}

const mapGroupToNode = (group: ConfigGroup) => {
  const children = group.Categories.map(c => mapCategoryToNode(group, c))

  return {
    value: `(${group.Name.toLowerCase()})`, label: group.Name, children: children, showCheckbox: true
  }
}

const createNodes = (groups: ConfigGroup[]) => {
  const nodes = groups.map(g => mapGroupToNode(g))

  return [{ value: '(All)', label: '(All)', children: nodes, showCheckbox: true }]
}

export const CapabilitiesTreePanel: FunctionComponent<CapabilitiesTreePanelProps> = (props) => {
  const data: any = createNodes(props.groups)
  const [checked, setChecked] = useState<string[]>(props.selectedCategories)

  // UseEffect here helps keeps 'checked' in sync with 'props.selectedCategories'.
  // When it's re-rendered in quick succession, useState doesn't update on the re-render.
  useEffect(() => {
    setChecked(props.selectedCategories)
  }, [props.selectedCategories])

  const [expanded, setExpanded] = useState<string[]>(props.expandedCategories ?? ['(All)'])

  const trackChecked = (checked: string[], onChecked: (values: string[]) => void) => {
    setChecked(checked)
    onChecked(checked)
  }

  const trackExpanded = (expanded: string[], onExpanded: (values: string[]) => void) => {
    setExpanded(expanded)
    onExpanded(expanded)
  }

  return (
    props.groups.length > 0
      ? <div style={{ border: '1px solid rgba(0, 0, 0, 0.35)', padding: '5px' }}>
                <CheckboxTree nodes={data}
                    checked={checked}
                    expanded={expanded}
                    onClick={curr => { }}
                    onCheck={curr => trackChecked(curr, props.onChecked)}
                    onExpand={expanded => trackExpanded(expanded, props.onExpanded)}
                    showNodeIcon={false}
                    icons={{
                      check: <Icon aria-label='Checked' iconName="CheckboxComposite" />,
                      uncheck: <Icon aria-label='Not checked' iconName="Checkbox" />,
                      halfCheck: <i aria-label='Partially checked' role='img' className="ms-Icon ms-Icon--CheckboxIndeterminateCombo" />,
                      expandClose: <Icon aria-label='Collapsed' iconName="CaretHollow" />,
                      expandOpen: <Icon aria-label='Expanded' iconName="CaretSolid" />
                    }} />
        </div>
      : <div>
            <span style={{ fontSize: '13px', borderTop: '5px', borderLeft: '5px' }}>There are no categories defined in the selected file.</span>
        </div>
  )
}
