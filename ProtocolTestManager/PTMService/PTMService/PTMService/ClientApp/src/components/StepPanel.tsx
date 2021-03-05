// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { MessageBar, MessageBarButton, MessageBarType } from "@fluentui/react"
import React, { FunctionComponent, useState } from "react"
import { LoadingPanel } from "./LoadingPanel"
import { RightPanel } from "./WizardNavBar"

type StepPanelProps = {
    isLoading: boolean,
    leftNav: JSX.Element,
    infoMsg?: string,
    warningMsg?: string,
    errorMsg?: string,
}

export const StepPanel: FunctionComponent<StepPanelProps> = (props) => {
    const [showMsg, setShowMsg] = useState(true);

    let infoMessageBar = undefined;
    let warningMessageBar = undefined;
    let errorMessageBar = undefined;
    if (!!props.errorMsg) {
        errorMessageBar = <MessageBar key={1}
            messageBarType={MessageBarType.error}
            isMultiline={false}
            onDismiss={() => setShowMsg(false)}
            dismissButtonAriaLabel="Close"
        >
            {props.errorMsg}
        </MessageBar>
    }

    if (!!props.warningMsg) {
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

    if (!!props.infoMsg) {
        infoMessageBar = <MessageBar key={3}
            messageBarType={MessageBarType.success}
            onDismiss={() => setShowMsg(false)}
            dismissButtonAriaLabel="Close"
            isMultiline={false}
        >
            {props.infoMsg}
        </MessageBar>
    };

    return (
        <div>
            {props.leftNav}
            <RightPanel>
                {
                    (((errorMessageBar || warningMessageBar || infoMessageBar) !== undefined) && showMsg) ? <div>{errorMessageBar || warningMessageBar || infoMessageBar}</div> : <div>
                        {
                            props.isLoading ? <LoadingPanel /> : <div>
                                {props.children}
                            </div>
                        }
                    </div>
                }
            </RightPanel>
        </div >

    )
}