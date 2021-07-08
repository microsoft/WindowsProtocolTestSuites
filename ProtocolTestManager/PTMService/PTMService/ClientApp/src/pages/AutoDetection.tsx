// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Link, PrimaryButton, Stack } from '@fluentui/react';
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard';
import { StepPanel } from '../components/StepPanel';
import { WizardNavBar } from '../components/WizardNavBar';
import { getNavSteps } from '../model/DefaultNavSteps';
import { AppState } from '../store/configureStore';
import { useDispatch, useSelector } from 'react-redux';

export function AutoDetection(props: StepWizardProps) {
    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;
    const navSteps = getNavSteps(wizardProps);
    const wizard = WizardNavBar(wizardProps, navSteps);
    const dispatch = useDispatch();
    const autoDetection = useSelector((state: AppState) => state.autoDetection);

    const onPreviousButtonClick = () => {
        //dispatch(PropertyGroupsActions.updatePropertyGroupsAction());
        //wizardProps.previousStep();
    };

    const onNextButtonClick = () => {
        //dispatch(PropertyGroupsActions.updatePropertyGroupsAction());
        //dispatch(PropertyGroupsDataSrv.setPropertyGroups(() => {
        //    if (propertyGroups.errorMsg === undefined) {
        //        wizardProps.nextStep();
        //    }
        //}));
    };

    const isPreviousButtonDisabled = () => { return true };
    const isNextButtonDisabled = () => { return true };
    const getNextButtonText = () => { return 'Detect' };

    return (
        <div>
            <StepPanel leftNav={wizard} isLoading={autoDetection.isPrerequisiteLoading || autoDetection.isDetectionStepsLoading} errorMsg={''} >
                <Stack style={{ paddingLeft: 10 }}>
                    <Stack>
                    </Stack>
                    //Prerequiste
                    //Space
                    //Detection Steps
                    <div className='buttonPanel'>
                        <Stack horizontal horizontalAlign="end" tokens={{ childrenGap: 10 }} >
                            <PrimaryButton text="Previous" onClick={onPreviousButtonClick} disabled={isPreviousButtonDisabled()} />
                            <PrimaryButton text={getNextButtonText()} onClick={onNextButtonClick} disabled={isNextButtonDisabled()} />
                        </Stack>
                    </div>
                </Stack>
            </StepPanel>
        </div>
    )
};