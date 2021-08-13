// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { DetailsList, Dropdown, IColumn, Label, SelectionMode, Stack, TextField, TooltipDelay, TooltipHost } from '@fluentui/react'
import { FunctionComponent } from 'react'
import { Property } from '../model/Property'
import { PropertyGroup } from '../model/PropertyGroup'

type PropertyGroupViewProps = {
    winSize: { width: number, height: number },
    latestPropertyGroup: PropertyGroup,
    propertyGroup: PropertyGroup,
    onValueChange: (property: Property) => void
}

const getListColumns = (props: { onRenderName: (prop: Property, index: number) => JSX.Element, onRenderValue: (prop: Property) => JSX.Element }): IColumn[] => {
  return [{
    key: 'Name',
    name: 'Name',
    fieldName: 'Name',
    minWidth: 240,
    isRowHeader: true,
    isResizable: true,
    onRender: (item: Property, index: number | undefined) => props.onRenderName(item, index!)
  },
  {
    key: 'Value',
    name: 'Value',
    fieldName: 'Value',
    minWidth: 480,
    isResizable: true,
    isPadded: true,
    onRender: (item: Property) => props.onRenderValue(item)
  }]
}

export const PropertyGroupView: FunctionComponent<PropertyGroupViewProps> = (props: PropertyGroupViewProps) => {
  const onValueChange = props.onValueChange

  function RenderCommonProperty (property: Property) {
    return (
            <TooltipHost
                style={{ alignSelf: 'center' }}
                key={props.propertyGroup.Name + property.Key + property.Name}
                content={property.Description}
                delay={TooltipDelay.zero}>
                <Stack horizontal tokens={{ childrenGap: 10 }}>
                    <TextField
                        style={{ alignSelf: 'stretch', minWidth: 360 }}
                        value={property.Value}
                        onChange={(_, newValue) => onValueChange({ ...property, Value: newValue! })}
                    />
                </Stack>
            </TooltipHost>
    )
  }

  function RenderChoosableProperty (property: Property) {
    if (property.Choices === undefined) {
      return
    }

    const dropdownOptions = property.Choices.map(choice => {
      return {
        key: choice.toLowerCase(),
        text: choice
      }
    })

    return (
            <TooltipHost
                style={{ alignSelf: 'center' }}
                key={props.propertyGroup.Name + property.Key + property.Name}
                content={property.Description}
                delay={TooltipDelay.zero}>
                <Stack horizontal tokens={{ childrenGap: 10 }}>
                    <Dropdown
                        style={{ alignSelf: 'center', minWidth: 360 }}
                        placeholder='Select an option'
                        defaultSelectedKey={property.Value?.toLowerCase()}
                        options={dropdownOptions}
                        onChange={(_, newValue, __) => onValueChange({ ...property, Value: newValue?.text! })}
                    />
                </Stack>
            </TooltipHost>
    )
  }

  const listColumns = getListColumns({
    onRenderName: (item: Property, index: number) => {
      const latestProperty = props.latestPropertyGroup.Items[index]
      if (latestProperty.Value === item.Value) {
        return (
                    <Label>
                        <div style={{ paddingLeft: 5 }}>{item.Name}</div>
                    </Label>
        )
      } else {
        return (
                    <Label style={{ backgroundSize: '120', backgroundColor: '#004578', color: 'white' }}>
                        <div style={{ paddingLeft: 5 }}>{item.Name}*</div>
                    </Label>
        )
      }
    },
    onRenderValue: (item: Property) => item.Choices?.length ? RenderChoosableProperty(item)! : RenderCommonProperty(item)
  })

  return (
        <div>
            <Stack horizontal style={{ paddingTop: 20 }} horizontalAlign='start' tokens={{ childrenGap: 10 }}>
                <div style={{ borderLeft: '2px solid #bae7ff', minHeight: props.winSize.height - 180 }}>
                    <DetailsList
                        columns={listColumns}
                        items={props.propertyGroup.Items}
                        compact
                        selectionMode={SelectionMode.none}
                        isHeaderVisible={false}
                    />
                </div>
            </Stack>
        </div>
  )
}
