// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ContextualMenuItemType, IContextualMenuProps, IContextualMenuItem, PrimaryButton } from '@fluentui/react'
import { useMemo } from 'react'

export interface ContextualMenuItemProps {
  key: string
  text: string
  disabled: boolean
  menuAction: () => void
}

interface ContextualMenuProps {
  text: string
  shouldFocusOnMount: boolean
  menuItems: ContextualMenuItemProps[]
}

export const ContextualMenuControl: React.FunctionComponent<ContextualMenuProps> = (props) => {
  const menuItems: IContextualMenuItem[] = useMemo(() => props.menuItems.flatMap((item: ContextualMenuItemProps, index: number) => {
    return index !== props.menuItems.length - 1
      ? [
        {
          key: item.key,
          text: item.text,
          disabled: item.disabled,
          onClick: item.menuAction
        },
        {
          key: `${item.key}Divider`,
          itemType: ContextualMenuItemType.Divider
        }
      ]
      : [
        {
          key: item.key,
          text: item.text,
          disabled: item.disabled,
          onClick: item.menuAction
        }
      ]
  }), [props])

  const menuProps = useMemo<IContextualMenuProps>(() => ({
    shouldFocusOnMount: props.shouldFocusOnMount,
    items: menuItems
  }), [props])

  return (
    <div>
      <PrimaryButton text={props.text} menuProps={menuProps} />
    </div>
  )
}
