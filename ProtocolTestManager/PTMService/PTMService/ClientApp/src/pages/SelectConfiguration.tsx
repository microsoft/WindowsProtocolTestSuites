// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ContextualMenu, DefaultButton, DetailsList, DetailsListLayoutMode, Dialog, DialogFooter, DialogType, IColumn, Link, PrimaryButton, SearchBox, SelectionMode, Stack, TextField } from '@fluentui/react'
import { useBoolean } from '@uifabric/react-hooks'
import { useCallback, useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { StepWizardChildProps } from 'react-step-wizard'
import { ConfigureMethodActions } from '../actions/ConfigureMethodAction'
import { DetectionResultActions } from '../actions/DetectionResultAction'
import { PropertyGroupsActions } from '../actions/PropertyGroupsAction'
import { ConfigurationActions } from '../actions/TestSuiteConfigurationAction'
import { WizardNavBarActions } from '../actions/WizardNavBarAction'
import { StackGap10 } from '../components/StackStyle'
import { StepPanel } from '../components/StepPanel'
import { WizardNavBar } from '../components/WizardNavBar'
import { Configuration } from '../model/Configuration'
import { getNavSteps, RunSteps } from '../model/DefaultNavSteps'
import { ConfigurationsDataSrv } from '../services/Configurations'
import { AppState } from '../store/configureStore'

export function SelectConfiguration(props: any) {
    const dispatch = useDispatch()
    const testSuiteInfo = useSelector((state: AppState) => state.testSuiteInfo)
    const configurations = useSelector((state: AppState) => state.configurations)
    const detectResult = useSelector((state: AppState) => state.detectResult)
    const [hideDialog, { toggle: toggleHideDialog }] = useBoolean(true)
    const [configurationName, setConfigurationName] = useState('')
    const [configurationDescription, setConfigurationDescription] = useState('')
    const [configurationErrorMsg, setConfigurationErrorMsg] = useState('')

    useEffect(() => {
        if (testSuiteInfo.selectedTestSuite != null) {
            dispatch(ConfigurationsDataSrv.getConfigurations(testSuiteInfo.selectedTestSuite.Id))
        }
    }, [dispatch, configurations.lastCreateId, testSuiteInfo.selectedTestSuite])

    const onFieldChange = useCallback(
        (element: ElementType, newValue?: string) => {
            switch (element) {
                case ElementType.Name:
                    setConfigurationName(newValue!)
                    break
                case ElementType.Description:
                    setConfigurationDescription(newValue!)
                    break
                default:
                    break
            }
        }, [])

    const wizardProps: StepWizardChildProps = props as StepWizardChildProps
    const navSteps = getNavSteps(wizardProps)
    const wizard = WizardNavBar(wizardProps, navSteps)

    if (testSuiteInfo.selectedTestSuite === undefined) {
        return (
            <StepPanel leftNav={wizard} isLoading={false} >
                <div>No Test Suite selected, please go to <Link onClick={() => { wizardProps.firstStep() }}>Start page</Link></div>
            </StepPanel>
        )
    }

    const dialogContentProps = {
        type: DialogType.normal,
        title: 'New Configuration'
    }

    const modalProps = {
        isBlocking: true,
        styles: { main: { maxWidth: 650 } },
        dragOptions: {
            moveMenuItemText: 'Move',
            closeMenuItemText: 'Close',
            menu: ContextualMenu
        }
    }

    const onConfigurationCreate = () => {
        if (configurationName != null && configurationName !== '') {
            dispatch(ConfigurationsDataSrv.createConfiguration({
                Name: configurationName,
                Description: configurationDescription,
                TestSuiteId: testSuiteInfo.selectedTestSuite!.Id,
                IsConfigured: false,
            }))
            toggleHideDialog()
            setConfigurationName('')
            setConfigurationDescription('')
            setConfigurationErrorMsg('')
        } else {
            setConfigurationErrorMsg('Configuration Name can\'t be null')
        }
    }

    const getCurrentConfiguration = (id: number) => {
        const foundConfiguration = configurations.displayList.find((value) => value.Id === id)
        return foundConfiguration
    }

    const columns = getConfigurationGridColumns({
        onRun: id => {
            const foundConfiguration = getCurrentConfiguration(id)
            if (foundConfiguration != null) {
                dispatch(PropertyGroupsActions.setUpdatedAction(false))
                dispatch(ConfigurationActions.setSelectedConfigurationAction(foundConfiguration))
                if (id !== configurations.selectedConfiguration?.Id) {
                    dispatch(WizardNavBarActions.setWizardNavBarAction(wizardProps.currentStep))
                    // Resets configureMethod and detectionResult after switching configurations
                    dispatch(ConfigureMethodActions.setConfigurationMethodAction())
                    dispatch(DetectionResultActions.resetDetectionResultAction())
                }
                if (detectResult.detectionResult === undefined) {
                    dispatch(ConfigureMethodActions.setConfigurationMethodAction())
                }
                // go to run step
                wizardProps.goToStep(RunSteps.RUN_SELECTED_TEST_CASE)
            }
        },
        onEdit: id => {
            const foundConfiguration = getCurrentConfiguration(id)
            if (foundConfiguration != null) {
                dispatch(PropertyGroupsActions.setUpdatedAction(false))
                dispatch(ConfigurationActions.setSelectedConfigurationAction(foundConfiguration))
                if (id !== configurations.selectedConfiguration?.Id) {
                    dispatch(WizardNavBarActions.setWizardNavBarAction(wizardProps.currentStep))
                    // Resets configureMethod and detectionResult after switching configurations
                    dispatch(ConfigureMethodActions.setConfigurationMethodAction())
                    dispatch(DetectionResultActions.resetDetectionResultAction())
                }
                // go to next step
                wizardProps.nextStep()
            }
        }
    })

    const onSearchChanged = (newValue: string): void => {
        dispatch(ConfigurationActions.setSearchTextAction(newValue))
    }

    return (
        <div>
            <StepPanel leftNav={wizard} isLoading={configurations.isLoading} errorMsg={configurations.errorMsg} >
                <Stack horizontal horizontalAlign="end" style={{ paddingLeft: 10, paddingRight: 10 }} tokens={StackGap10}>
                    <SearchBox placeholder="Search" onSearch={onSearchChanged} />
                    <PrimaryButton iconProps={{ iconName: 'Add' }} allowDisabledFocus onClick={() => {
                        toggleHideDialog()
                    }}>
                        New
                    </PrimaryButton>
                </Stack>
                <hr style={{ border: '1px solid #d9d9d9' }} />
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
                <Stack tokens={StackGap10}>
                    <TextField
                        label="Configuration Name"
                        value={configurationName}
                        errorMessage={configurationErrorMsg}
                        onChange={(event: any, newValue?: string) => { onFieldChange(ElementType.Name, newValue) }}
                    />
                    <TextField
                        label="Description"
                        multiline={true}
                        value={configurationDescription}
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
    onRun: (configureId: number) => void
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
            isResizable: true
        },
        {
            key: 'Description',
            name: 'Description',
            fieldName: 'Description',
            minWidth: 300,
            isRowHeader: true,
            isResizable: true,
            data: 'string',
            isPadded: true
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
                return <Stack horizontal tokens={StackGap10}>
                    <PrimaryButton disabled={!item.IsConfigured} onClick={() => { props.onRun(item.Id!) }}>Run</PrimaryButton>
                    <PrimaryButton onClick={() => { props.onEdit(item.Id!) }}>Edit</PrimaryButton>
                </Stack>
            }
        }
    ]
}

enum ElementType {
    Name,
    Description,
}
