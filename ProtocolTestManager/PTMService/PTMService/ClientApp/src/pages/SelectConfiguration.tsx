// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ActionButton, ContextualMenu, DefaultButton, DetailsList, DetailsListLayoutMode, Dialog, DialogFooter, DialogType, IColumn, IStackTokens, Link, PrimaryButton, SearchBox, SelectionMode, Stack, TextField } from '@fluentui/react';
import { useBoolean } from '@uifabric/react-hooks';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { StepWizardChildProps } from 'react-step-wizard';
import { ConfigurationActions } from '../actions/TestSuiteConfigurationAction';
import { StepPanel } from '../components/StepPanel';
import { WizardNavBar } from '../components/WizardNavBar';
import { Configuration } from '../model/Configuration';
import { getNavSteps } from '../model/DefaultNavSteps';
import { ConfigurationsDataSrv } from '../services/Configurations';
import { AppState } from '../store/configureStore';

export function SelectConfiguration(props: any) {
    const gapStackTokens: IStackTokens = {
        childrenGap: 10,
    };

    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;

    const navSteps = getNavSteps(wizardProps);

    const wizard = WizardNavBar(wizardProps, navSteps);
    const dispatch = useDispatch();

    const testSuiteInfo = useSelector((state: AppState) => state.testsuites);
    const configurations = useSelector((state: AppState) => state.configurations);

    if (testSuiteInfo.selectedTestSuite === undefined) {
        return (
            <StepPanel leftNav={wizard} isLoading={false} >
                <div>No Test Suite selected, please go to <Link onClick={() => { wizardProps.firstStep() }}>Start page</Link></div>
            </StepPanel>
        )
    }

    //#region  Create New Configuration Dialog
    const [hideDialog, { toggle: toggleHideDialog }] = useBoolean(true);
    const dialogContentProps = {
        type: DialogType.normal,
        title: 'New Configuration',
    };

    const modalProps = {
        isBlocking: true,
        styles: { main: { maxWidth: 450 } },
        dragOptions: {
            moveMenuItemText: 'Move',
            closeMenuItemText: 'Close',
            menu: ContextualMenu,
        }
    }

    const onConfigurationCreate = () => {

        toggleHideDialog();
    }
    //#endregion

    const columns = getConfigurationGridColumns((configureId) => {
        // apply selected configure id
        // navigate to Run selected test case page
    });

    const onSearchChanged = (newValue: string): void => {
        dispatch(ConfigurationActions.setSearchTextAction(newValue));
    };

    useEffect(() => {
        dispatch(ConfigurationsDataSrv.getConfigurations(testSuiteInfo.selectedTestSuite!.Id));
    }, [dispatch])

    return (
        <div>
            <StepPanel leftNav={wizard} isLoading={configurations.isLoading} errorMsg={configurations.errorMsg} >
                <Stack horizontal horizontalAlign="end" style={{ paddingLeft: 10, paddingRight: 10 }} tokens={gapStackTokens}>
                    <SearchBox placeholder="Search" onSearch={onSearchChanged} />
                    <PrimaryButton iconProps={{ iconName: 'Add' }} allowDisabledFocus onClick={() => {
                        toggleHideDialog();
                    }}>
                        New
                </PrimaryButton>
                </Stack>
                <hr style={{ border: "1px solid #d9d9d9" }} />
                <DetailsList
                    items={configurations.displayList}
                    compact={true}
                    columns={columns}
                    selectionMode={SelectionMode.none}
                    layoutMode={DetailsListLayoutMode.justified}
                    isHeaderVisible={true}
                />
            </StepPanel>
            <Dialog
                hidden={hideDialog}
                onDismiss={toggleHideDialog}
                dialogContentProps={dialogContentProps}
                modalProps={modalProps}
            >
                <Stack tokens={gapStackTokens}>
                    <TextField
                        label="Configuration Name"
                    />
                    <TextField
                        label="Description"
                    />
                </Stack>
                <DialogFooter>
                    <PrimaryButton onClick={onConfigurationCreate} text="Create" />
                    <DefaultButton onClick={toggleHideDialog} text="Close" />
                </DialogFooter>
            </Dialog>
        </div>

    )
};

function getConfigurationGridColumns(onSelect: (configureId: number) => void): IColumn[] {
    return [
        {
            key: 'Name',
            name: 'Name',
            ariaLabel: 'Name of the Configuration',
            fieldName: 'Name',
            minWidth: 200,
            maxWidth: 300,
            isResizable: true,
        },
        {
            key: 'Description',
            name: 'Description',
            fieldName: 'Description',
            minWidth: 300,
            isRowHeader: true,
            isResizable: true,
            data: 'string',
            isPadded: true,
        },
        {
            key: 'Action',
            name: 'Action',
            minWidth: 100,
            isRowHeader: true,
            isResizable: true,
            data: 'string',
            isPadded: true,
            onRender: (item: Configuration, index, column) => {
                return <Stack>
                    <PrimaryButton onClick={() => { onSelect(item.Id) }}>Select</PrimaryButton>
                </Stack>
            }
        }
    ];
}