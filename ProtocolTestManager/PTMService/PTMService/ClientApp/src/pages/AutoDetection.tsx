// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { DetailsList, Dropdown, IColumn, Label, Link, PrimaryButton, Stack, TextField, TooltipDelay, TooltipHost } from '@fluentui/react';
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
import React, { useEffect, useState } from 'react';
import { TestSuitesDataSrv } from '../services/TestSuites';
import { SelectionMode } from '@uifabric/experiments/lib/Utilities';
import { AutoDetectActions } from '../actions/AutoDetectionAction';
import { Prerequisite } from '../model/AutoDetectionData';

export function AutoDetection(props: StepWizardProps) {
    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;
    const testSuites = useSelector((state: AppState) => state.testsuites);
    const prerequisite = useSelector((state: AppState) => state.autoDetection);
    const navSteps = getNavSteps(wizardProps);
    const wizard = WizardNavBar(wizardProps, navSteps);
    const dispatch = useDispatch();
    const autoDetection = useSelector((state: AppState) => state.autoDetection);
    const winSize = useWindowSize();
    const [detectingTimes, setDetectingTimes] = useState(0);

    useEffect(() => {
        dispatch(TestSuitesDataSrv.getAutoDetectionPrerequisite());
    }, [dispatch]);

    useEffect(() => {
        dispatch(TestSuitesDataSrv.getAutoDetectionSteps());
    }, [dispatch]);

    useEffect(() => {
        console.log('useEffect');
        console.log(detectingTimes);

        if (isAutoDetectFinished()) {
            return;
        }

        const timer = setTimeout(() => {
            setDetectingTimes(detectingTimes - 1);
            dispatch(TestSuitesDataSrv.pullAutoDetectionSteps());
            }, 1000);
       
        return () => clearTimeout(timer);
    }, [detectingTimes])

    const onPreviousButtonClick = () => {
        wizardProps.previousStep();
    };

    const onPropertyValueChange = (updatedProperty: Property) => {

        console.log(updatedProperty);
        prerequisite.prerequisite?.Properties.map(p => {
            if (p.Name === updatedProperty.Name) {
                p.Value = updatedProperty.Value;
            }
        });

        dispatch(AutoDetectActions.UpdateAutoDetectPrerequisiteAction(prerequisite.prerequisite!));
    };

    const isAutoDetectStarted = () => {
        const isStarted = prerequisite.detectionSteps?.DetectingItems.some(item => {
            if (item.Status != 'Pending') {
                return true;
            }
            return false;
        })
        return isStarted;
    }

    const isAutoDetectFinished = () => {
        const hasPending = prerequisite.detectionSteps?.DetectingItems.some(item => {
            if (item.Status === 'Pending') {
                return true;
            }
            return false;
        })
        return !hasPending;
    }

    const onNextButtonClick = () => {
        if (isAutoDetectFinished()) {
            // Next page
            wizardProps.nextStep();
        }
        else if (isAutoDetectStarted()) {
            // Cancel
            dispatch(TestSuitesDataSrv.stopAutoDetection());
        }
        else {
            dispatch(TestSuitesDataSrv.startAutoDetection());
            setDetectingTimes(100);
        }
    };

    const isPreviousButtonDisabled = () => {
        if (isAutoDetectStarted()) {
            return true;
        }
        else {
            return false;
        }
    };

    const isNextButtonDisabled = () => { return false };
    const getNextButtonText = () => {
        if (isAutoDetectFinished()) {
            return 'Next';
        }
        else if (isAutoDetectStarted()) {
            return 'Cancel';
        }
        else {
            return 'Detect';
        }
        
    };

    const StepColumns = (): IColumn[] => {
        return [{
            key: 'DetectingContent',
            name: 'DetectingContent',
            fieldName: 'DetectingContent',
            minWidth: 240,
            isRowHeader: true,
            isResizable: true,
            onRender: (item: Property, index: number | undefined) => {
                return (
                    <Label>
                        <div style={{ paddingLeft: 5 }}>{item.Name}</div>
                    </Label>
                );
            }
        },
            {
                key: 'DetectingStatus',
                name: 'DetectingStatus',
                fieldName: 'DetectingStatus',
                minWidth: 480,
                isResizable: true,
                isPadded: true,
                onRender: (item: any) => {
                    let style = { };

                    if (item.Status === 'Failed') {
                        style = { paddingLeft: 5, color: 'red' };
                    }
                    else if (item.Status === 'Finished') {
                        style = { paddingLeft: 5, color: 'green' };
                    }
                    else {
                        style = { paddingLeft: 5 };
                    }
                    return (
                        <Label>
                            <div style={ style }>{item.Status}</div>
                        </Label>
                    );
                }
        }];
    }

    const getListColumns = (props: { onRenderName: (prop: Property, index: number) => JSX.Element, onRenderValue: (prop: Property) => JSX.Element }): IColumn[] => {
        return [{
            key: 'Name',
            name: 'Name',
            fieldName: 'Name',
            minWidth: 240,
            isRowHeader: true,
            isResizable: true,
            onRender: (item: Property, index: number | undefined) => props.onRenderName(item, index!)
        },
        {
            key: 'Value',
            name: 'Value',
            fieldName: 'Value',
            minWidth: 480,
            isResizable: true,
            isPadded: true,
            onRender: (item: Property) => props.onRenderValue(item)
        }];
    };

    const listColumns = getListColumns({
        onRenderName: (item: Property, index: number) => {
            const latestProperty = prerequisite.prerequisite?.Properties[index];
            if (latestProperty?.Value === item.Value) {
                return (
                    <Label>
                        <div style={{ paddingLeft: 5 }}>{item.Name}</div>
                    </Label>
                );
            }
            else {
                return (
                    <Label style={{ backgroundSize: '120', backgroundColor: '#004578', color: 'white' }}>
                        <div style={{ paddingLeft: 5 }}>{item.Name}*</div>
                    </Label>
                );
            }
        },
        onRenderValue: (item: Property) => item.Choices?.length && item.Choices?.length > 1 ? RenderChoosableProperty(item)! : RenderCommonProperty(item)
    });

    function RenderChoosableProperty(property: Property) {
        if (property.Choices === undefined) {
            return;
        }

        const dropdownOptions = property.Choices.map(choice => {
            return {
                key: choice.toLowerCase(),
                text: choice
            };
        });

        return (
            <TooltipHost
                style={{ alignSelf: 'center' }}
                key={prerequisite.prerequisite?.Title + property.Key + property.Name}
                content={property.Description}
                delay={TooltipDelay.zero}>
                <Stack horizontal tokens={{ childrenGap: 10 }}>
                    <Dropdown
                        style={{ alignSelf: 'center', minWidth: 360 }}
                        placeholder='Select an option'
                        defaultSelectedKey={property.Value?.toLowerCase()}
                        options={dropdownOptions}
                        onChange={(_, newValue, __) => onPropertyValueChange({ ...property, Value: newValue?.text! })}
                    />
                </Stack>
            </TooltipHost>
        );
    }

    function RenderCommonProperty(property: Property) {
        return (
            <TooltipHost
                style={{ alignSelf: 'center' }}
                key={prerequisite.prerequisite?.Title + property.Key + property.Name}
                content={property.Description}
                delay={TooltipDelay.zero}>
                <Stack horizontal tokens={{ childrenGap: 10 }}>
                    <TextField
                        style={{ alignSelf: 'stretch', minWidth: 360 }}
                        value={property.Value}
                        onChange={(_, newValue) => onPropertyValueChange({ ...property, Value: newValue! })}
                    />
                </Stack>
            </TooltipHost>
        );
    }



    return (
        <div>
            <StepPanel leftNav={wizard} isLoading={autoDetection.isPrerequisiteLoading || autoDetection.isDetectionStepsLoading} errorMsg={''} >
                <Stack style={{ paddingLeft: 10 }}>
                    <Stack>
                        {prerequisite.prerequisite?.Summary}
                    </Stack>

                    <div style={{ paddingLeft: 30, width: winSize.width, height: winSize.height/2-180, overflowY: 'auto' }}>
                        {
                            prerequisite.isPrerequisiteLoading ?
                                <LoadingPanel /> :
                                <div>
                                    <Stack horizontal style={{ paddingTop: 20 }} horizontalAlign='start' tokens={{ childrenGap: 10 }}>
                                        <div style={{ borderLeft: '2px solid #bae7ff', minHeight: 200 }}>
                                            <DetailsList
                                                columns={listColumns}
                                                items={prerequisite.prerequisite?.Properties ? prerequisite.prerequisite?.Properties: []}
                                                compact
                                                selectionMode={SelectionMode.none}
                                                isHeaderVisible={false}
                                            />
                                        </div>
                                    </Stack>
                                </div>
                                
                        }
                    </div>
                    <div style={{ paddingLeft: 30, width: winSize.width, height: winSize.height / 2, overflowY: 'auto' }}>
                        {
                            prerequisite.isDetectionStepsLoading ?
                                <LoadingPanel /> :
                                <div>
                                    <Stack horizontal style={{ paddingTop: 20 }} horizontalAlign='start' tokens={{ childrenGap: 10 }}>
                                        <div style={{ borderLeft: '2px solid #bae7ff', minHeight: 200 }}>
                                            <DetailsList
                                                columns={StepColumns()}
                                                items={prerequisite.detectionSteps?.DetectingItems ? prerequisite.detectionSteps?.DetectingItems : []}
                                                compact
                                                selectionMode={SelectionMode.none}
                                                isHeaderVisible={false}
                                            />
                                        </div>
                                    </Stack>
                                </div>

                        }
                    </div>
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