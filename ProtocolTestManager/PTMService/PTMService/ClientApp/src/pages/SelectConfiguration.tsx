// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ContextualMenu, DefaultButton, DetailsList, DetailsListLayoutMode, Dialog, DialogFooter, DialogType, IColumn, IStackTokens, Link, PrimaryButton, SearchBox, SelectionMode, Stack, TextField } from '@fluentui/react';
import { useBoolean } from '@uifabric/react-hooks';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { StepWizardChildProps } from 'react-step-wizard';
import { ConfigurationActions } from '../actions/TestSuiteConfigurationAction';
import { StepPanel } from '../components/StepPanel';
import { WizardNavBar } from '../components/WizardNavBar';
import { Configuration } from '../model/Configuration';
import { getNavSteps, RunSteps } from '../model/DefaultNavSteps';
import { ConfigurationsDataSrv } from '../services/Configurations';
import { AppState } from '../store/configureStore';

export function SelectConfiguration(props: any) {
    const dispatch = useDispatch();
    const testSuiteInfo = useSelector((state: AppState) => state.testsuites);
    const configurations = useSelector((state: AppState) => state.configurations);
    const [hideDialog, { toggle: toggleHideDialog }] = useBoolean(true);
    const [configureName, setConfigureName] = useState('');
    const [configureDescription, setConfigureDescription] = useState('');
    const [configureErrorMsg, setConfigureErrorMsg] = useState('');
    const [descriptionErrorMsg, setDescriptionErrorMsg] = useState('');

    useEffect(() => {
        if (testSuiteInfo.selectedTestSuite) {
            dispatch(ConfigurationsDataSrv.getConfigurations(testSuiteInfo.selectedTestSuite!.Id));
        }
    }, [configurations.lastCreateId])

    const onFieldChange = useCallback(
        (element: ElementType, newValue?: string) => {
            switch (element) {
                case ElementType.Name:
                    setConfigureName(newValue!);
                    break;
                case ElementType.Description:
                    setConfigureDescription(newValue!);
                    break;
                default:
                    break;
            }
        }, []);

    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;
    const navSteps = getNavSteps(wizardProps);
    const wizard = WizardNavBar(wizardProps, navSteps);

    if (testSuiteInfo.selectedTestSuite === undefined) {
        return (
            <StepPanel leftNav={wizard} isLoading={false} >
                <div>No Test Suite selected, please go to <Link onClick={() => { wizardProps.firstStep() }}>Start page</Link></div>
            </StepPanel>
        )
    }

    //#region  Create New Configuration Dialog

    const dialogContentProps = {
        type: DialogType.normal,
        title: 'New Configuration',
    };

    const modalProps = {
        isBlocking: true,
        styles: { main: { maxWidth: 650 } },
        dragOptions: {
            moveMenuItemText: 'Move',
            closeMenuItemText: 'Close',
            menu: ContextualMenu,
        }
    }

    const onConfigurationCreate = () => {
        if (configureName && configureDescription) {
            dispatch(ConfigurationsDataSrv.createConfiguration({
                Name: configureName,
                Description: configureDescription,
                TestSuiteId: testSuiteInfo.selectedTestSuite!.Id
            }))
            toggleHideDialog();
            setConfigureName('');
            setConfigureDescription('');
            setConfigureErrorMsg('');
            setDescriptionErrorMsg('');
        } else {
            if (configureName) {
                setConfigureErrorMsg('')
            } else {
                setConfigureErrorMsg(`Configuration Name can't be null`)
            }
            if (configureDescription) {
                setDescriptionErrorMsg('')
            } else {
                setDescriptionErrorMsg(`Description can't be null`)
            }
        }
    }
    //#endregion

    const getCurrentConfigure = (id: number) => {
        const findedConfigure = configurations.displayList.find((value) => value.Id === id);
        return findedConfigure;
    }

    const columns = getConfigurationGridColumns({
        onRun: (id => {
            const findedConfigure = getCurrentConfigure(id);
            if (findedConfigure) {
                dispatch(ConfigurationActions.setSelectedConfigurationAction(findedConfigure));
                // go to run step
                wizardProps.goToStep(RunSteps.RUN_SELECTED_TEST_CASE);
            }
        }),
        onEdit: (id => {
            const findedConfigure = getCurrentConfigure(id);
            if (findedConfigure) {
                dispatch(ConfigurationActions.setSelectedConfigurationAction(findedConfigure));
                // go to next step
                wizardProps.nextStep();
            }
        })
    });

    const onSearchChanged = (newValue: string): void => {
        dispatch(ConfigurationActions.setSearchTextAction(newValue));
    };

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
                        value={configureName}
                        errorMessage={configureErrorMsg}
                        onChange={(event: any, newValue?: string) => { onFieldChange(ElementType.Name, newValue) }}
                    />
                    <TextField
                        label="Description"
                        multiline={true}
                        value={configureDescription}
                        errorMessage={descriptionErrorMsg}
                        onChange={(event: any, newValue?: string) => { onFieldChange(ElementType.Description, newValue) }}
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

function getConfigurationGridColumns(props: {
    onRun: (configureId: number) => void,
    onEdit: (configureId: number) => void
}): IColumn[] {
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
            minWidth: 200,
            isRowHeader: true,
            isResizable: true,
            data: 'string',
            isPadded: true,
            onRender: (item: Configuration, index, column) => {
                return <Stack horizontal tokens={gapStackTokens}>
                    <PrimaryButton onClick={() => { props.onRun(item.Id!) }}>Run</PrimaryButton>
                    <PrimaryButton onClick={() => { props.onEdit(item.Id!) }}>Edit</PrimaryButton>
                </Stack>
            }
        }
    ];
}

const gapStackTokens: IStackTokens = {
    childrenGap: 10,
};

enum ElementType {
    Name,
    Description,
}