// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Link, PrimaryButton, Stack } from '@fluentui/react';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard';
import { PropertyGroupsActions } from '../actions/PropertyGroupsActions';
import { LoadingPanel } from '../components/LoadingPanel';
import { PropertyGroupView } from '../components/PropertyGroupView';
import { StepPanel } from '../components/StepPanel';
import { useWindowSize } from '../components/UseWindowSize';
import { WizardNavBar } from '../components/WizardNavBar';
import { getNavSteps } from '../model/DefaultNavSteps';
import { Property } from '../model/Property';
import { PropertyGroupsDataSrv } from '../services/PropertyGroups';
import { AppState } from '../store/configureStore';

export function ConfigureTestCase(props: StepWizardProps) {
    const dispatch = useDispatch();
    const propertyGroups = useSelector((state: AppState) => state.propertyGroups);
    const configureMethod = useSelector((state: AppState) => state.configureMethod);

    useEffect(() => {
        dispatch(PropertyGroupsDataSrv.getPropertyGroups());
    }, [dispatch]);

    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;
    const navSteps = getNavSteps(wizardProps, configureMethod);
    const wizard = WizardNavBar(wizardProps, navSteps);
    const winSize = useWindowSize();

    const onPropertyValueChange = (updatedProperty: Property) => {
        dispatch(PropertyGroupsActions.updatedEditingPropertyGroupAction(updatedProperty));
    };

    const onEditingPropertyGroupChange = (index: number) => {
        dispatch(PropertyGroupsActions.setEditingPropertyGroupAction(index));
    };

    const onPreviousButtonClick = () => {
        dispatch(PropertyGroupsActions.updatePropertyGroupsAction());
        wizardProps.previousStep();
    };

    const onNextButtonClick = () => {
        dispatch(PropertyGroupsDataSrv.setPropertyGroups((data: any) => {
            if (propertyGroups.errorMsg === undefined) {
                wizardProps.nextStep();
            }
        }));
    };

    return (
        <div>
            <StepPanel leftNav={wizard} isLoading={propertyGroups.isLoading} errorMsg={propertyGroups.errorMsg}>
                <Stack style={{ padding: 10 }}>
                    <Stack horizontal style={{ paddingBottom: 20 }}>
                        <div style={{ fontWeight: 'bold' }}>Groups</div>
                        <div style={{ paddingLeft: winSize.width * 0.10, fontWeight: 'bold' }}>{propertyGroups.editingPropertyGroup?.Name} Properties</div>
                        <div style={{ paddingLeft: winSize.width * 0.13, fontSize: 'small', fontWeight: 'normal', alignSelf: 'center' }}>*The value is modified compared to the latest run</div>
                    </Stack>
                    <Stack horizontal>
                        <Stack style={{ paddingTop: 20, width: 220, height: winSize.height - 160, overflowY: 'auto' }} tokens={{ childrenGap: 20 }} >
                            {
                                propertyGroups.propertyGroups.map((propertyGroup, index) => {
                                    return (
                                        <div style={{ alignSelf: 'start' }}>
                                            <Link key={index} style={{ fontSize: 'large', fontWeight: 'bold' }}
                                                disabled={propertyGroups.editingPropertyGroupIndex === index}
                                                onClick={() => onEditingPropertyGroupChange(index)}>
                                                {propertyGroup.Name}
                                            </Link>
                                        </div>
                                    );
                                })
                            }
                        </Stack>
                        <div style={{ paddingLeft: 30, width: winSize.width, height: winSize.height - 160, overflowY: 'auto' }}>
                            {
                                propertyGroups.editingPropertyGroup === undefined ?
                                    <LoadingPanel /> :
                                    <PropertyGroupView
                                        winSize={winSize}
                                        latestPropertyGroup={propertyGroups.latestPropertyGroups[propertyGroups.editingPropertyGroupIndex]}
                                        propertyGroup={propertyGroups.editingPropertyGroup!}
                                        onValueChange={onPropertyValueChange}
                                    />
                            }
                        </div>
                    </Stack>
                    <div style={{ borderTop: '1px solid #d9d9d9', backgroundColor: '#f5f5f5', paddingTop: 7, paddingRight: 45, height: 40 }}>
                        <Stack horizontal horizontalAlign="end" tokens={{ childrenGap: 10 }} >
                            <PrimaryButton text="Previous" onClick={onPreviousButtonClick} disabled={propertyGroups.isPosting} />
                            <PrimaryButton text="Next" onClick={onNextButtonClick} disabled={propertyGroups.isPosting} />
                        </Stack>
                    </div>
                </Stack>
            </StepPanel>
        </div >
    );
};