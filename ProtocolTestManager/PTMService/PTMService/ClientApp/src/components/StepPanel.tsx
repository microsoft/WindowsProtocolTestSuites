// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { MessageBar, MessageBarButton, MessageBarType } from '@fluentui/react'
import { FunctionComponent, useEffect, useState } from 'react'
import { LoadingPanel } from './LoadingPanel'
import { useWindowSize } from './UseWindowSize'
import { HeaderMenuHeight, RightPanel } from './WizardNavBar'

interface StepPanelProps {
  isLoading?: boolean
  leftNav: JSX.Element
  infoMsg?: string
  warningMsg?: string
  errorMsg?: string
}

export const StepPanel: FunctionComponent<StepPanelProps> = (props) => {
  const [showMsg, setShowMsg] = useState(true)
  const winSize = useWindowSize()

  let infoMessageBar
  let warningMessageBar
  let errorMessageBar
  if (props.errorMsg) {
    errorMessageBar = <MessageBar key={1}
            messageBarType={MessageBarType.error}
            isMultiline={false}
            onDismiss={() => setShowMsg(false)}
            dismissButtonAriaLabel="Close"
        >
            {props.errorMsg}
        </MessageBar>
  }

  if (props.warningMsg) {
    warningMessageBar = <MessageBar key={2}
            messageBarType={MessageBarType.warning}
            isMultiline={false}
            onDismiss={() => setShowMsg(false)}
            dismissButtonAriaLabel="Close"
            actions={
                <div>
                    <MessageBarButton>Action</MessageBarButton>
                </div>
            }
        >
            {props.warningMsg}
        </MessageBar>
  }

  if (props.infoMsg) {
    infoMessageBar = <MessageBar key={3}
            messageBarType={MessageBarType.success}
            onDismiss={() => setShowMsg(false)}
            dismissButtonAriaLabel="Close"
            isMultiline={false}
        >
            {props.infoMsg}
        </MessageBar>
  };

  useEffect(() => {
    setShowMsg(true)
  }, [props.errorMsg, props.infoMsg, props.warningMsg])

  const showMessageBar = (((errorMessageBar ?? warningMessageBar ?? infoMessageBar) !== undefined) && showMsg)
  return (
        <div style={{ height: winSize.height - (HeaderMenuHeight - 20) }}>
            <RightPanel>
                {
                    showMessageBar ? <div>{errorMessageBar ?? warningMessageBar ?? infoMessageBar}</div> : undefined
                }
                {
                    <div>
                        {
                            props.isLoading
                              ? <LoadingPanel />
                              : <div style={{ height: winSize.height - HeaderMenuHeight - (showMessageBar ? 50 : 0), overflowY: 'auto' }}>
                                    {props.children}
                                </div>
                        }
                    </div>
                }
            </RightPanel>
            {props.leftNav}
        </div>
  )
}
