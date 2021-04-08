// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { CommandBarButton, ContextualMenu, DefaultButton, DetailsList, DetailsListLayoutMode, Dialog, DialogFooter, DialogType, IColumn, PrimaryButton, SearchBox, SelectionMode, Stack, TextField } from '@fluentui/react';
import { useBoolean } from '@uifabric/react-hooks';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ManagementActions } from '../actions/ManagementAction';
import { FileUploader, IFile } from '../components/FileUploader';
import { StackGap10 } from '../components/StackStyle';
import { TestSuite } from '../model/TestSuite';
import { ManagementDataSrv } from '../services/Management';
import { AppState } from '../store/configureStore';

export function Management(props: any) {
    const dispatch = useDispatch();
    const [hideDialog, { toggle: toggleHideDialog }] = useBoolean(true);
    const [testSuiteName, setTestSuiteName] = useState('');
    const [testSuiteDescription, setTestSuiteDescription] = useState('');
    const [testSuiteErrorMsg, setTestSuiteErrorMsg] = useState('');
    const [file, setFile] = useState<IFile>();

    useEffect(() => {
        dispatch(ManagementDataSrv.getTestSuiteList());
    }, [dispatch])

    const onFieldChange = useCallback(
        (element: ElementType, newValue?: string) => {
            switch (element) {
                case ElementType.Name:
                    setTestSuiteName(newValue!);
                    break;
                case ElementType.Description:
                    setTestSuiteDescription(newValue!);
                    break;
                default:
                    break;
            }
        }, []);

    const managementState = useSelector((state: AppState) => state.management);

    const dialogContentProps = {
        type: DialogType.normal,
        title: 'Install Test Suite',
    };

    const modalProps = {
        isBlocking: true,
        styles: { main: { innerWidth: 600, maxWidth: 650 } },
        dragOptions: {
            moveMenuItemText: 'Move',
            closeMenuItemText: 'Close',
            menu: ContextualMenu,
        }
    }

    const onConfigurationCreate = () => {
        if (testSuiteName && file) {
            dispatch(ManagementDataSrv.installTestSuite({
                TestSuiteName: testSuiteName,
                Description: testSuiteDescription,
                Package: file!.File
            }, () => {
                toggleHideDialog();
                setTestSuiteName('');
                setTestSuiteDescription('');
                setTestSuiteErrorMsg('');
                dispatch(ManagementDataSrv.getTestSuiteList());
            }))
        } else {
            if (!testSuiteName) {
                setTestSuiteErrorMsg(`TestSuite Name can't be null`)
                return;
            }
            if (!file) {
                setTestSuiteErrorMsg(`TestSuite package can't be null`)
                return;
            }
            setTestSuiteErrorMsg('')
        }
    }

    const getCurrentConfigure = (id: number) => {
        const findedConfigure = managementState.displayList.find((value) => value.Id === id);
        return findedConfigure;
    }

    const columns = getConfigurationGridColumns({
        onReRun: (id => {
            const findedConfigure = getCurrentConfigure(id);
            if (findedConfigure) {
                //dispatch(ManagementActions.setSelectedConfigurationAction(findedConfigure));
            }
        }),
        onUpdate: (id => {
            const findedConfigure = getCurrentConfigure(id);
            if (findedConfigure) {
                //dispatch(ManagementActions.setSelectedConfigurationAction(findedConfigure));
            }
        }),
        onRemove: (id => {

        })
    });

    const onSearchChanged = (newValue: string): void => {
        dispatch(ManagementActions.setSearchTextAction(newValue));
    };

    const onFileUploadSuccess = (files: IFile[]): void => {
        if (files.length > 0) {
            setFile(files[0]);
        }
    }

    return (
        <div>
            <Stack horizontal horizontalAlign="end" style={{ paddingLeft: 10, paddingRight: 10, paddingTop: 10 }} tokens={StackGap10}>
                <SearchBox placeholder="Search" onSearch={onSearchChanged} />
                <PrimaryButton iconProps={{ iconName: 'Add' }} allowDisabledFocus onClick={() => {
                    toggleHideDialog();
                }}>
                    Install Test Suite
                </PrimaryButton>
            </Stack>
            <hr style={{ border: "1px solid #d9d9d9" }} />
            <div style={{ padding: 10 }}>
                <DetailsList
                    items={managementState.displayList}
                    compact={true}
                    columns={columns}
                    selectionMode={SelectionMode.none}
                    layoutMode={DetailsListLayoutMode.justified}
                    isHeaderVisible={true}
                />
            </div>
            <Dialog
                hidden={hideDialog}
                onDismiss={toggleHideDialog}
                dialogContentProps={dialogContentProps}
                modalProps={modalProps}
            >
                <Stack tokens={StackGap10}>
                    <TextField
                        label="Test Suite Name"
                        value={testSuiteName}
                        onChange={(event: any, newValue?: string) => { onFieldChange(ElementType.Name, newValue) }}
                    />
                    <FileUploader
                        label="Package"
                        onSuccess={onFileUploadSuccess}
                        maxFileCount={1} suffix={['.zip', '.gz']}
                        placeholder="Please click to upload test suite package"
                    />
                    <TextField
                        label="Description"
                        multiline={true}
                        value={testSuiteDescription}
                        errorMessage={testSuiteErrorMsg}
                        onChange={(event: any, newValue?: string) => { onFieldChange(ElementType.Description, newValue) }}
                    />
                </Stack>
                <DialogFooter>
                    <PrimaryButton onClick={onConfigurationCreate} text={managementState.isInstalling ? "Installing" : "Create"} disabled={managementState.isInstalling} />
                    <DefaultButton onClick={toggleHideDialog} text="Close" disabled={managementState.isInstalling} />
                </DialogFooter>
            </Dialog>
        </div>
    )
};

function getConfigurationGridColumns(props: {
    onReRun: (configureId: number) => void,
    onUpdate: (configureId: number) => void,
    onRemove: (configureId: number) => void,
}): IColumn[] {
    return [
        {
            key: 'Name',
            name: 'Test Suite',
            ariaLabel: 'Name of the Test Suite',
            fieldName: 'Name',
            minWidth: 200,
            maxWidth: 300,
            isResizable: true,
        },
        {
            key: 'Version',
            name: 'Version',
            fieldName: 'Version',
            minWidth: 200,
            maxWidth: 200,
            isRowHeader: true,
            isResizable: true,
            data: 'string',
            isPadded: true,
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
            minWidth: 300,
            isRowHeader: true,
            isResizable: true,
            data: 'string',
            isPadded: true,
            onRender: (item: TestSuite, index, column) => {
                return <Stack horizontal tokens={StackGap10}>
                    <CommandBarButton iconProps={{ iconName: 'Rerun' }} text="ReRun" disabled={true} onClick={() => { props.onReRun(item.Id!) }} />
                    <CommandBarButton iconProps={{ iconName: 'UpgradeAnalysis' }} text="Update" disabled={true} onClick={() => { props.onUpdate(item.Id!) }} />
                    <CommandBarButton iconProps={{ iconName: 'RemoveFromTrash' }} text="Remove" disabled={true} onClick={() => { props.onRemove(item.Id!) }} />
                </Stack>
            }
        }
    ];
}

enum ElementType {
    Name,
    Description,
}