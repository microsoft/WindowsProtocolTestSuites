/* eslint-disable react/react-in-jsx-scope */
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Dropdown, Label, Stack, TextField, TooltipDelay, TooltipHost } from '@fluentui/react'
import { FunctionComponent } from 'react'
import { Property } from '../model/Property'
import { PropertyGroup } from '../model/PropertyGroup'

type PropertyGroupViewProps = {
  winSize: { width: number, height: number },
  latestPropertyGroup: PropertyGroup,
  propertyGroup: PropertyGroup,
  onValueChange: (property: Property) => void
}

type PropertyNameProps = {
  property: Property,
  index: number
}

type PropertyViewProps = {
  property: Property
}

export const PropertyGroupView: FunctionComponent<PropertyGroupViewProps> = (props: PropertyGroupViewProps) => {
  const onValueChange = props.onValueChange

  function CommonProperty(viewProps: PropertyViewProps) {
    const property = viewProps.property

    return (
      <TooltipHost
        style={{ alignSelf: 'center' }}
        key={props.propertyGroup.Name + property.Key + property.Name}
        content={property.Description}
        delay={TooltipDelay.zero}>
        <Stack horizontal tokens={{ childrenGap: 10 }}>
          <TextField
            key={property.Key}
            style={{ alignSelf: 'stretch', minWidth: 360 }}
            value={property.Value}
            onChange={(_, newValue) => onValueChange({ ...property, Value: newValue! })}
          />
        </Stack>
      </TooltipHost>
    )
  }

  function ChoosableProperty(viewProps: PropertyViewProps) {
    const property = viewProps.property

    if (property.Choices === undefined) {
      return null
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
            key={property.Key}
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

  function PropertyView(viewProps: PropertyViewProps) {
    const property = viewProps.property

    return property.Choices?.length
      ? <ChoosableProperty property={property} />
      : <CommonProperty property={property} />
  }

  function PropertyName(nameProps: PropertyNameProps) {
    const { property, index } = nameProps

    const latestProperty = props.latestPropertyGroup.Items[index]
    if (latestProperty.Value === property.Value) {
      return (
        <Label>
          <div style={{ paddingLeft: 5 }}>{property.Name}</div>
        </Label>
      )
    } else {
      return (
        <Label style={{ backgroundSize: '120', backgroundColor: '#004578', color: 'white' }}>
          <div style={{ paddingLeft: 5 }}>{property.Name}*</div>
        </Label>
      )
    }
  }

  return (
    <div style={{ borderLeft: '2px solid #bae7ff', paddingLeft: 20, minHeight: props.winSize.height - 180 }}>
      {
        props.propertyGroup.Items.map((item, index) => {
          return <div key={item.Name}
            style={{
              padding: 8,
              display: 'flex',
              backgroundColor: index % 2 === 0 ? '#EEEEEE' : 'none',
              flexDirection: 'row'
            }} >
            <PropertyName property={item} index={index} />
            <PropertyView property={item} />
          </div>
        })
      }
    </div>
  )
}
