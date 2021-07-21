// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Link, PrimaryButton, Stack } from '@fluentui/react';
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard';
import { StepPanel } from '../components/StepPanel';
import { WizardNavBar } from '../components/WizardNavBar';
import { getNavSteps } from '../model/DefaultNavSteps';
import { AppState } from '../store/configureStore';
import { useDispatch, useSelector } from 'react-redux';
import { PropertyGroupsActions } from '../actions/PropertyGroupsActions';
import { PropertyGroupsDataSrv } from '../services/PropertyGroups';
import { useWindowSize } from '../components/UseWindowSize';
import { LoadingPanel } from '../components/LoadingPanel';
import { PropertyGroupView } from '../components/PropertyGroupView';
import { Property } from '../model/Property';
import { useEffect } from 'react';
import { TestSuitesDataSrv } from '../services/TestSuites';

export function AutoDetection(props: StepWizardProps) {
    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;
    const propertyGroups = useSelector((state: AppState) => state.propertyGroups);
    const testSuites = useSelector((state: AppState) => state.testsuites);
    const navSteps = getNavSteps(wizardProps);
    const wizard = WizardNavBar(wizardProps, navSteps);
    const dispatch = useDispatch();
    const autoDetection = useSelector((state: AppState) => state.autoDetection);
    const winSize = useWindowSize();

    useEffect(() => {
        dispatch(TestSuitesDataSrv.getAutoDetectionPrerequisite());
    }, [dispatch]);

    useEffect(() => {
        if (!propertyGroups.updated) {
            dispatch(TestSuitesDataSrv.getAutoDetectionPrerequisite());
        }
    }, [dispatch, propertyGroups.updated]);

    console.log('*******************');
    console.log(propertyGroups);
    console.log(testSuites);
    const onPreviousButtonClick = () => {
        dispatch(PropertyGroupsActions.updatePropertyGroupsAction());
        wizardProps.previousStep();
    };

    const onPropertyValueChange = (updatedProperty: Property) => {
        dispatch(PropertyGroupsActions.updatedEditingPropertyGroupAction(updatedProperty));
    };

    // TODO.
    const onNextButtonClick = () => {
        dispatch(PropertyGroupsActions.updatePropertyGroupsAction());
        dispatch(PropertyGroupsDataSrv.setPropertyGroups(() => {
            if (propertyGroups.errorMsg === undefined) {
                wizardProps.nextStep();
            }
        }));
    };

    const isPreviousButtonDisabled = () => { return true };
    const isNextButtonDisabled = () => { return false };
    const getNextButtonText = () => { return 'Detect' };

    return (
        <div>
            <StepPanel leftNav={wizard} isLoading={autoDetection.isPrerequisiteLoading || autoDetection.isDetectionStepsLoading} errorMsg={''} >
                <Stack style={{ paddingLeft: 10 }}>
                    <Stack>
                    </Stack>
                    //Prerequiste
                    <div style={{ paddingLeft: 30, width: winSize.width, height: winSize.height - 160, overflowY: 'auto' }}>
                        {

                                
                        }
                    </div>
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