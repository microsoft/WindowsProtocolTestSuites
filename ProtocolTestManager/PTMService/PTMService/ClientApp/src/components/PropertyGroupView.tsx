/* eslint-disable react/react-in-jsx-scope */
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Dropdown, Label, Stack, TextField, TooltipDelay, TooltipHost } from '@fluentui/react'
import { FunctionComponent, ReactElement, useLayoutEffect, useState } from 'react'
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
  width: number
}

interface CommonPropertyProps {
  property: Property
  onValueChange: (property: Property) => void
}

interface ChoosablePropertyProps {
  property: Required<Property>
  onValueChange: (property: Property) => void
}

function CommonProperty (props: CommonPropertyProps): ReactElement {
  return (
    <TooltipHost
      style={{ alignSelf: 'center' }}
      key={props.property.Key ?? props.property.Name}
      content={props.property.Description}
      delay={TooltipDelay.zero}>
      <TextField
        key={props.property.Key ?? props.property.Name}
        ariaLabel={`Input a value for ${props.property.Name}`}
        style={{ alignSelf: 'stretch', minWidth: 480 }}
        value={props.property.Value}
        onChange={(_, newValue) => props.onValueChange({ ...props.property, Value: newValue! })}
      />
    </TooltipHost>
  )
}

function ChoosableProperty (props: ChoosablePropertyProps): ReactElement {
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
        style={{ alignSelf: 'center', minWidth: 480 }}
        ariaLabel={`Select an option for ${props.property.Name}`}
        placeholder='Select an option'
        defaultSelectedKey={props.property.Value?.toLowerCase()}
        options={dropdownOptions}
        onChange={(_, newValue, __) => props.onValueChange({ ...props.property, Value: newValue?.text! })}
      />
    </TooltipHost>
  )
}

function PropertyName (props: PropertyNameProps): ReactElement {
  const { latestProperty, property } = props
  if (latestProperty === undefined || latestProperty.Value === property.Value) {
    return (
      <Label style={{ width: props.width }}>
        <div style={{ paddingLeft: 5 }}>{property.Name}</div>
      </Label>
    )
  } else {
    return (
      <Label style={{ width: props.width, backgroundSize: '120', backgroundColor: '#004578', color: 'white' }}>
        <div style={{ paddingLeft: 5 }}>{property.Name}*</div>
      </Label>
    )
  }
}

export const PropertyGroupView: FunctionComponent<PropertyGroupViewProps> = (props: PropertyGroupViewProps) => {
  const [propertyNameLabelWidthMaximum, setPropertyNameLabelWidthMaximum] = useState<number>(0)

  useLayoutEffect(() => {
    const max = props.propertyGroup.Items.reduce((currMax, curr) => {
      const calculatedWidth = (curr.Name.length + 1) * 9
      return Math.max(calculatedWidth, currMax)
    }, 0)

    setPropertyNameLabelWidthMaximum(max)
  }, [props])

  return (
    <div style={{ borderLeft: '2px solid #bae7ff', paddingLeft: 20, minHeight: props.winSize.height - 180 }}>
      {
        props.propertyGroup.Items.map((item, index) => {
          return <Stack
            horizontal
            key={item.Key ?? item.Name}
            style={{
              padding: 8,
              backgroundColor: `rgba(238, 238, 238, ${index % 2 === 1 ? 0 : 1})`
            }}
            tokens={{ childrenGap: 20 }}>
            <PropertyName
              latestProperty={props.latestPropertyGroup?.Items[index]}
              property={item}
              width={propertyNameLabelWidthMaximum} />
            {
              item.Choices !== undefined && item.Choices !== null && item.Choices.length > 1
                ? <ChoosableProperty property={{ ...item, Choices: item.Choices }} onValueChange={props.onValueChange} />
                : <CommonProperty property={item} onValueChange={props.onValueChange} />
            }
          </Stack>
        })
      }
    </div>
  )
}
