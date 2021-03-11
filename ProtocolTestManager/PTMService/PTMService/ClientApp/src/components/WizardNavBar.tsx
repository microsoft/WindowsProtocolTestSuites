// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import styled from '@emotion/styled';
import { IStackTokens, Link, Stack } from '@fluentui/react';
import React from 'react';
import { StepWizardChildProps } from "react-step-wizard";
import { StepNavItemInfo } from '../model/StepNavItemInfo';
import { useWindowSize } from './UseWindowSize';

export function WizardNavBar(wizardProps: StepWizardChildProps, navSteps: StepNavItemInfo[]) {
    const navStepItems = navSteps.map((item: StepNavItemInfo, index: number) => {
        if (item.IsEnabled) {
            return <Link key={index} underline={false} onClick={() => wizardProps.goToStep(item.TargetStep)}>{item.Caption}</Link>
        } else {
            return <Link key={index} underline={false} disabled={true}>{item.Caption}</Link>
        }
    });

    const verticalGapStackTokens: IStackTokens = {
        childrenGap: 10,
    };

    const winSize = useWindowSize();


    return (<LeftPanel style={{ height: winSize.height -10 }}>
        <Stack verticalFill verticalAlign="start" tokens={verticalGapStackTokens} >
            {navStepItems}
        </Stack>
    </LeftPanel>);
}

const leftPanelWidth = 200;
export const LeftPanel = styled.div`
    float: left;
    width: ${leftPanelWidth}px;
    margin-top:auto;
    margin-bottom:auto;
    z-index:999;
    border-right-color: #bae7ff;
    border-style: none solid none  none ;
    border-width: 2px;
    padding-left: 10px;
`;

export const RightPanel = styled.div`
    margin-Left: ${leftPanelWidth + 10}px;
    height: 100%;
    z-index:999;
    padding-left: 10px;
`;