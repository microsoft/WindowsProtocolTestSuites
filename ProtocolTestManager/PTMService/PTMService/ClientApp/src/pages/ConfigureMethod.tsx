// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { PrimaryButton, Stack } from '@fluentui/react';
import { CSSProperties } from 'react';
import { useDispatch } from 'react-redux';
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard';
import { ConfigureMethodActions } from '../actions/ConfigureMethodAction';
import { StackGap10 } from '../components/StackStyle';
import { StepPanel } from '../components/StepPanel';
import { WizardNavBar } from '../components/WizardNavBar';
import { getNavSteps, RunSteps } from '../model/DefaultNavSteps';

export const ConfigureMethod_AutoDetection = "AutoDetection";
export const ConfigureMethod_Manual = "Manual";
export const ConfigureMethod_Profile = "Profile";

export function ConfigureMethod(props: StepWizardProps) {
    const dispatch = useDispatch();
    
    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;
    const navSteps = getNavSteps(wizardProps);
    
    const items: MethodItemProp[] = [{
        Title: 'Run Auto-Detection',
        Key: ConfigureMethod_AutoDetection,
        Description: 'Run Auto-Detection to retrieve capabilities of System Under Test(SUT) which are used to configure the test suite and select test cases automatically.',
        Disabled: true,
    }, {
        Title: 'Do Manual Configuration',
        Key: ConfigureMethod_Manual,
        Description: 'Don\'t run Auto-Detection. Configure the test suite and select test cases manually',
    }, {
        Title: 'Load Profile',
        Key: ConfigureMethod_Profile,
        Description: 'Protocol Test Manager Profile contains information about configuration of test suite and selected test cases.\r\n You could load an existing profile to get the saved configuration',
        Disabled: true,
    }];

    const onItemClicked = (key: string) => {
        dispatch(ConfigureMethodActions.setConfigureMethod(key));

        switch (key) {
            case ConfigureMethod_AutoDetection:
                wizardProps.goToStep(RunSteps.AUTO_DETECTION);
                return;
            case ConfigureMethod_Manual:
                wizardProps.goToStep(RunSteps.FILTERTESTCASE);
                return;
            case ConfigureMethod_Profile:
                wizardProps.goToStep(RunSteps.RUN_SELECTED_TEST_CASE);
                return;
        }
    }

    return (
        <div>
            <StepPanel leftNav={WizardNavBar(wizardProps, navSteps)} >
                <Stack style={{ padding: 10 }}>
                    {
                        items.map((item, index) => {
                            return <div key={index} style={{ paddingBottom: 50 }}>
                                <MethodItem
                                    Title={item.Title}
                                    Key={item.Key}
                                    Description={item.Description}
                                    Disabled={item.Disabled}
                                    onClick={() => { onItemClicked(item.Key) }}
                                ></MethodItem>
                            </div>
                        })
                    }
                    <Stack horizontal horizontalAlign="end" tokens={StackGap10} >
                        <PrimaryButton text="Previous" onClick={() => wizardProps.previousStep()} />
                    </Stack>
                </Stack>
            </StepPanel>
        </div>
    )
};

interface MethodItemProp {
    Title: string;
    Key: string;
    Description: string;
    Disabled?: boolean;
    onClick?: () => void;
}

function MethodItem(props: MethodItemProp) {
    const divStyle: CSSProperties | undefined = props.Disabled ? { color: 'grey' } : { cursor: 'pointer' };
    const divOnClicked = props.Disabled ? undefined : props.onClick;

    return (<div className="card" style={divStyle}>
        <Stack className="container" tokens={StackGap10} onClick={divOnClicked}>
            <div>
                <div className="subject">{props.Title}</div>
                <div className="description">{props.Description}</div>
            </div>
        </Stack>
    </div>);
}