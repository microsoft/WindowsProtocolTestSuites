// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  getTheme,
  Modal,
  IconButton,
  TextField,
  mergeStyleSets,
  FontWeights,
  IIconProps,
  IButtonStyles
} from '@fluentui/react'
import { ReactElement } from 'react'

export interface PopupModalProp {
  isOpen: boolean
  header?: string
  onClose: () => void
  text?: string
}

export function PopupModal (props: PopupModalProp): ReactElement {
  return (<Modal
                isOpen={props.isOpen}
                containerClassName={contentStyles.container}
            >
              <div className={contentStyles.header}>
                <span>{props.header}</span>
                <IconButton
                  styles={iconButtonStyles}
                  iconProps={cancelIcon}
                  ariaLabel='Close'
                  onClick={props.onClose}
                />
              </div>
              <div className={contentStyles.body}>
                  <TextField multiline readOnly autoAdjustHeight ariaLabel='Popup content' resizable={false} defaultValue={props.text} />
              </div>
            </Modal>)
}

const theme = getTheme()
const contentStyles = mergeStyleSets({
  container: {
    display: 'flex',
    flexFlow: 'column nowrap',
    alignItems: 'stretch',
    minWidth: '50%'
  },
  header: [
    theme.fonts.xxLarge,
    {
      flex: '1 1 auto',
      borderTop: `4px solid ${theme.palette.themePrimary}`,
      color: theme.palette.neutralPrimary,
      display: 'flex',
      alignItems: 'center',
      fontWeight: FontWeights.semibold,
      padding: '12px 12px 14px 24px'
    }
  ],
  body: {
    flex: '4 4 auto',
    padding: '0 24px 24px 24px',
    overflowY: 'hidden',
    selectors: {
      p: { margin: '14px 0' },
      'p:first-child': { marginTop: 0 },
      'p:last-child': { marginBottom: 0 }
    }
  }
})

const cancelIcon: IIconProps = { iconName: 'Cancel' }

const iconButtonStyles: Partial<IButtonStyles> = {
  root: {
    color: theme.palette.neutralPrimary,
    marginLeft: 'auto',
    marginTop: '4px',
    marginRight: '2px'
  },
  rootHovered: {
    color: theme.palette.neutralDark
  }
}
