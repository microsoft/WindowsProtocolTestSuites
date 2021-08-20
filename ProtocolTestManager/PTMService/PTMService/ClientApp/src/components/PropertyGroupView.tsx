/* eslint-disable react/react-in-jsx-scope */
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Dropdown, Label, TextField, TooltipDelay, TooltipHost } from '@fluentui/react'
import { FunctionComponent, ReactElement } from 'react'
import { Property } from '../model/Property'
import { PropertyGroup } from '../model/PropertyGroup'

interface PropertyGroupViewProps {
  winSize: { width: number, height: number }
  latestPropertyGroup?: PropertyGroup
  propertyGroup: PropertyGroup
  onValueChange: (property: Property) => void
}

interface PropertyNameProps {
  latestProperty?: Property
  property: Property
}

interface CommonPropertyProps {
  property: Property
  onValueChange: (property: Property) => void
}

interface ChoosablePropertyProps {
  property: Required<Property>
  onValueChange: (property: Property) => void
}

function CommonProperty(props: CommonPropertyProps): ReactElement {
  return (
    <TooltipHost
      style={{ alignSelf: 'center' }}
      key={props.property.Key ?? props.property.Name}
      content={props.property.Description}
      delay={TooltipDelay.zero}>
      <TextField
        key={props.property.Key ?? props.property.Name}
        style={{ alignSelf: 'stretch', minWidth: 360 }}
        value={props.property.Value}
        onChange={(_, newValue) => props.onValueChange({ ...props.property, Value: newValue! })}
      />
    </TooltipHost>
  )
}

function ChoosableProperty(props: ChoosablePropertyProps): ReactElement {
  const dropdownOptions = props.property.Choices.map(choice => {
    return {
      key: choice.toLowerCase(),
      text: choice
    }
  })

  return (
    <TooltipHost
      style={{ alignSelf: 'center' }}
      key={props.property.Key ?? props.property.Name}
      content={props.property.Description}
      delay={TooltipDelay.zero}>
      <Dropdown
        key={props.property.Key ?? props.property.Name}
        style={{ alignSelf: 'center', minWidth: 360 }}
        placeholder='Select an option'
        defaultSelectedKey={props.property.Value?.toLowerCase()}
        options={dropdownOptions}
        onChange={(_, newValue, __) => props.onValueChange({ ...props.property, Value: newValue?.text! })}
      />
    </TooltipHost>
  )
}

function PropertyName(props: PropertyNameProps): ReactElement {
  const { latestProperty, property } = props
  if (latestProperty === undefined || latestProperty.Value === property.Value) {
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

export const PropertyGroupView: FunctionComponent<PropertyGroupViewProps> = (props: PropertyGroupViewProps) => {
  return (
    <div style={{ borderLeft: '2px solid #bae7ff', paddingLeft: 20, minHeight: props.winSize.height - 180 }}>
      {
        props.propertyGroup.Items.map((item, index) => {
          return <div
            key={item.Key ?? item.Name}
            style={{
              padding: 8,
              display: 'flex',
              backgroundColor: index % 2 === 0 ? '#EEEEEE' : 'none',
              flexDirection: 'row'
            }} >
            <div style={{ flex: '30em' }}>
              <PropertyName latestProperty={props.latestPropertyGroup?.Items[index]} property={item} />
            </div>
            <div style={{ flex: '80em', paddingLeft: 20, paddingRight: 60 }}>
              {
                item.Choices !== undefined && item.Choices !== null && item.Choices.length > 1
                  ? <ChoosableProperty property={{ ...item, Choices: item.Choices }} onValueChange={props.onValueChange} />
                  : <CommonProperty property={item} onValueChange={props.onValueChange} />
              }
            </div>
          </div>
        })
      }
    </div>
  )
}
