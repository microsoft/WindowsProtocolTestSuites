// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { CommandBarButton, ContextualMenu, DefaultButton, DetailsList, DetailsListLayoutMode, Dialog, DialogFooter, DialogType, IColumn, Label, PrimaryButton, SearchBox, SelectionMode, Stack, TextField, Dropdown } from '@fluentui/react'
import { useBoolean } from '@uifabric/react-hooks'
import { useDispatch, useSelector } from 'react-redux'
import { useHistory } from 'react-router-dom'
import { StackGap10, StackGap5 } from '../components/StackStyle'
import { useCallback, useEffect, useState } from 'react'
import { CapabilitiesActions } from '../actions/CapabilitiesAction'
import { CapabilitiesFileInfo } from '../model/CapabilitiesFileInfo'
import { PopupModal } from '../components/PopupModal'
import { AppState } from '../store/configureStore'
import { ManagementDataSrv } from '../services/Management'
import { CapabilitiesDataSrv } from '../services/Capabilities'

type TestSuiteCapabilitiesDialogKind = 'Create' | 'Update' | 'Remove'

interface TestSuiteCapabilitiesDialogContentProps {
  kind: TestSuiteCapabilitiesDialogKind
  type: DialogType
  title: string
}

export function ListCapabilitiesConfig (props: any) {
  const dispatch = useDispatch()
  const history = useHistory()
  const capabilitiesListState = useSelector((state: AppState) => state.capabilitiesList)
  const managementState = useSelector((state: AppState) => state.management)
  const [hideDialog, { toggle: toggleHideDialog }] = useBoolean(true)
  const [dialogContentProps, setDialogContentProps] = useState<TestSuiteCapabilitiesDialogContentProps | undefined>(undefined)
  const [currentCapabilitiesFileName, setCurrentCapabilitiesFileName] = useState('')
  const [currentCapabilitiesFileDescription, setCurrentCapabilitiesFileDescription] = useState('')
  const [newCapabilitiesFileTestSuiteId, setNewCapabilitiesFileTestSuiteId] = useState<number>(0)
  const [capabilitiesFileErrorMsg, setCapabilitiesFileErrorMsg] = useState('')
  const [currentCapabilitiesFile, setCurrentCapabilitiesFile] = useState<CapabilitiesFileInfo | undefined>(undefined)
  const [isWarningDialogOpen, { setTrue: showWarningDialog, setFalse: hideWarningDialog }] = useBoolean(false)

  useEffect(() => {
    dispatch(ManagementDataSrv.getTestSuiteList())
  }, [dispatch])

  useEffect(() => {
    dispatch(CapabilitiesDataSrv.getCapabilitiesFiles())
  }, [dispatch])

  // Reset the states each time the dialog is hidden.
  useEffect(() => {
    if (currentCapabilitiesFile != null) {
      if (hideDialog) {
        setCurrentCapabilitiesFile(undefined)
        setCurrentCapabilitiesFileName('')
        setCurrentCapabilitiesFileDescription('')
        setNewCapabilitiesFileTestSuiteId(0)
      }
    }
  }, [hideDialog])

  const testSuitesDropdownOptions =
        managementState.displayList.map(testSuite => {
          return {
            key: testSuite.Id,
            text: `${testSuite.Name} - ${testSuite.Version}`
          }
        })

  const createDialogContentProps: TestSuiteCapabilitiesDialogContentProps = {
    kind: 'Create',
    type: DialogType.normal,
    title: 'Create Capabilities File'
  }

  const updateDialogContentProps: TestSuiteCapabilitiesDialogContentProps = {
    kind: 'Update',
    type: DialogType.normal,
    title: 'Update Capabilities File'
  }

  const removeDialogContentProps: TestSuiteCapabilitiesDialogContentProps = {
    kind: 'Remove',
    type: DialogType.normal,
    title: 'Remove Capabilities File'
  }

  const modalProps = {
    isBlocking: true,
    styles: { main: { innerWidth: 600, maxWidth: 650 } },
    dragOptions: {
      moveMenuItemText: 'Move',
      closeMenuItemText: 'Close',
      menu: ContextualMenu
    }
  }

  const getCapabilitiesGridColumns = (props: {
    onDownload: (id: number) => void
    onConfigure: (id: number) => void
    onUpdate: (id: number) => void
    onRemove: (id: number) => void
  }): IColumn[] => {
    return [
      {
        key: 'Name',
        name: 'Name',
        ariaLabel: 'Name of the capabilities configuration file',
        fieldName: 'Name',
        minWidth: 200,
        maxWidth: 300,
        isResizable: true
      },
      {
        key: 'TestSuiteFullName',
        name: 'Test Suite',
        ariaLabel: 'Name & version of the test suite for the capabilities configuration file',
        fieldName: 'TestSuiteFullName',
        minWidth: 200,
        maxWidth: 300,
        isResizable: true
      },
      {
        key: 'Action',
        name: 'Action',
        minWidth: 200,
        isRowHeader: true,
        isResizable: true,
        data: 'string',
        isPadded: true,
        onRender: (item: CapabilitiesFileInfo, index, column) => {
          return <Stack horizontal tokens={StackGap10}>
                <CommandBarButton iconProps={{ iconName: 'Download' }} text="Download" onClick={() => { props.onDownload(item.Id) }} />
                <CommandBarButton iconProps={{ iconName: 'ConfigurationSolid' }} text="Configure" onClick={() => { props.onConfigure(item.Id) }} />
                <CommandBarButton iconProps={{ iconName: 'UpgradeAnalysis' }} text="Update Metadata" onClick={() => { props.onUpdate(item.Id) }} />
                <CommandBarButton iconProps={{ iconName: 'RemoveFromTrash' }} text="Remove" onClick={() => { props.onRemove(item.Id) }} />
            </Stack>
        }
      }
    ]
  }

  const getCapabilitiesConfigById = (id: number) =>
    capabilitiesListState.displayList.find((value) => value.Id === id)

  const columns = getCapabilitiesGridColumns({
    onDownload: (id: number) => {
      const foundCapabilitiesFile = getCapabilitiesConfigById(id)
      if (foundCapabilitiesFile != null) {
        setCurrentCapabilitiesFile(foundCapabilitiesFile)
        setCurrentCapabilitiesFileName(foundCapabilitiesFile.Name)
        setCurrentCapabilitiesFileDescription(foundCapabilitiesFile.Description ?? '')
        dispatch(CapabilitiesDataSrv.downloadCapabilitiesFile(id))
      }
    },
    onConfigure: (id: number) => {
      const foundCapabilitiesFile = getCapabilitiesConfigById(id)
      if (foundCapabilitiesFile != null) {
        dispatch(() => history.push(`/Capabilities/Configure/${foundCapabilitiesFile.Id}`, { from: 'ListCapabilitiesConfig' }))
      }
    },
    onUpdate: (id: number) => {
      const foundCapabilitiesFile = getCapabilitiesConfigById(id)
      if (foundCapabilitiesFile != null) {
        setCurrentCapabilitiesFile(foundCapabilitiesFile)
        setCurrentCapabilitiesFileName(foundCapabilitiesFile.Name)
        setCurrentCapabilitiesFileDescription(foundCapabilitiesFile.Description ?? '')
        setDialogContentProps(updateDialogContentProps)
        toggleHideDialog()
      }
    },
    onRemove: (id: number) => {
      const foundCapabilitiesFile = getCapabilitiesConfigById(id)
      if (foundCapabilitiesFile != null) {
        setCurrentCapabilitiesFile(foundCapabilitiesFile)
        setCurrentCapabilitiesFileName(foundCapabilitiesFile.Name)
        setCurrentCapabilitiesFileDescription(foundCapabilitiesFile.Description ?? '')
        setDialogContentProps(removeDialogContentProps)
        toggleHideDialog()
      }
    }
  })

  const onFieldChange = useCallback(
    (element: ElementType, newValue?: string) => {
      switch (element) {
        case ElementType.Name:
          setCurrentCapabilitiesFileName(newValue!)
          break
        case ElementType.Description:
          setCurrentCapabilitiesFileDescription(newValue!)
          break
        default:
          break
      }
    }, [])

  const onSearchChanged = (newValue: string): void => {
    dispatch(CapabilitiesActions.setSearchTextAction(newValue))
  }

  const onCapabilitiesFileCreate = () => {
    if (currentCapabilitiesFileName && (newCapabilitiesFileTestSuiteId !== 0)) {
      setCapabilitiesFileErrorMsg('')
      dispatch(CapabilitiesDataSrv.createCapabilitiesFile({
        CapabilitiesFileName: currentCapabilitiesFileName,
        CapabilitiesFileDescription: currentCapabilitiesFileDescription,
        TestSuiteId: newCapabilitiesFileTestSuiteId
      }, () => {
        toggleHideDialog()
        setCurrentCapabilitiesFile(undefined)
        setCurrentCapabilitiesFileName('')
        setCurrentCapabilitiesFileDescription('')
        setNewCapabilitiesFileTestSuiteId(0)
        dispatch(CapabilitiesDataSrv.getCapabilitiesFiles())
      }))
    } else {
      if (!currentCapabilitiesFileName) {
        setCapabilitiesFileErrorMsg('Capabilities File Name can\'t be empty')
        return
      }
      if (newCapabilitiesFileTestSuiteId === 0) {
        setCapabilitiesFileErrorMsg('Kindly select a Test Suite to target')
        return
      }

      setCapabilitiesFileErrorMsg('')
    }
  }

  const onCapabilitiesFileUpdate = () => {
    if (currentCapabilitiesFile !== undefined && currentCapabilitiesFileName) {
      setCapabilitiesFileErrorMsg('')
      dispatch(CapabilitiesDataSrv.updateCapabilitiesFile(currentCapabilitiesFile.Id, {
        CapabilitiesFileName: currentCapabilitiesFileName,
        CapabilitiesFileDescription: currentCapabilitiesFileDescription
      }, () => {
        toggleHideDialog()
        setCurrentCapabilitiesFile(undefined)
        setCurrentCapabilitiesFileName('')
        setCurrentCapabilitiesFileDescription('')
        dispatch(CapabilitiesDataSrv.getCapabilitiesFiles())
      }))
    } else {
      if (!currentCapabilitiesFileName) {
        setCapabilitiesFileErrorMsg('Capabilities File Name can\'t be empty')
        return
      }

      setCapabilitiesFileErrorMsg('')
    }
  }

  const onCapabilitiesFileRemove = () => {
    if (currentCapabilitiesFile !== undefined) {
      dispatch(CapabilitiesDataSrv.removeCapabilitiesFile(currentCapabilitiesFile.Id,
        () => {
          toggleHideDialog()
          setCurrentCapabilitiesFile(undefined)
          setCurrentCapabilitiesFileName('')
          setCurrentCapabilitiesFileDescription('')
          setCapabilitiesFileErrorMsg('')
          dispatch(CapabilitiesDataSrv.getCapabilitiesFiles())
        }))
    }
  }

  return (
        <div>
            <Stack horizontal horizontalAlign="end" style={{ paddingLeft: 10, paddingRight: 10, paddingTop: 10 }} tokens={StackGap10}>
              <SearchBox placeholder="Filter by name"
                  onChange={(_, newValue) => onSearchChanged(newValue ?? '')} />
                <PrimaryButton
                    iconProps={{ iconName: 'Add' }}
                    allowDisabledFocus
                    text='New Capabilities File'
                    onClick={() => {
                      setDialogContentProps(createDialogContentProps)
                      toggleHideDialog()
                    }} />
            </Stack>
            <hr style={{ border: '1px solid #d9d9d9' }} />
            <div style={{ padding: 10 }}>
                <DetailsList
                    items={capabilitiesListState.displayList}
                    compact={true}
                    columns={columns}
                    selectionMode={SelectionMode.none}
                    layoutMode={DetailsListLayoutMode.justified}
                    isHeaderVisible={true} />
            </div>
            <Dialog
                hidden={hideDialog}
                onDismiss={toggleHideDialog}
                dialogContentProps={dialogContentProps}
                modalProps={modalProps}>
                {
                    dialogContentProps !== undefined && dialogContentProps.kind !== 'Remove'
                      ? <Stack tokens={StackGap10}>
                            <TextField
                                label="Capabilities File Name"
                                value={currentCapabilitiesFileName}
                                disabled={capabilitiesListState.isProcessing}
                              onChange={(event: any, newValue?: string) => { onFieldChange(ElementType.Name, newValue) }} />

                          {
                              dialogContentProps.kind === 'Create'
                                ? <Dropdown
                                      key={props.key}
                                      style={{ alignSelf: 'center' }}
                                      ariaLabel={'Select Test Suite'}
                                      placeholder='Select Test Suite'
                                      options={testSuitesDropdownOptions}
                                      onChange={(_, newValue, __) => { const value: any = newValue?.key; setNewCapabilitiesFileTestSuiteId(value) }} />
                                : null
                          }

                            <TextField
                                label="Capabilities File Description"
                                multiline={true}
                                value={currentCapabilitiesFileDescription}
                                disabled={capabilitiesListState.isProcessing}
                                errorMessage={capabilitiesFileErrorMsg || capabilitiesListState.errorMsg}
                                onChange={(event: any, newValue?: string) => { onFieldChange(ElementType.Description, newValue) }} />
                        </Stack>
                      : <div style={{ fontSize: 'large' }}>
                            <Stack horizontalAlign='start' tokens={StackGap5}>
                                <Label>Do you want to remove the following capabilities file?</Label>
                                <Stack horizontal><div style={{ paddingRight: 5 }}>ID:</div>{currentCapabilitiesFile?.Id}</Stack>
                                <Stack horizontal><div style={{ paddingRight: 5 }}>File:</div>{currentCapabilitiesFile?.Name}</Stack>
                                <Stack horizontal><div style={{ paddingRight: 5 }}>Description:</div>{currentCapabilitiesFile?.Description}</Stack>
                            </Stack>
                        </div>
                }
                {
                    dialogContentProps !== undefined
                      ? <DialogFooter>
                            {
                                dialogContentProps.kind === 'Create'
                                  ? <PrimaryButton
                                        onClick={onCapabilitiesFileCreate}
                                        text={capabilitiesListState.isProcessing ? 'Creating' : 'Create'}
                                        disabled={capabilitiesListState.isProcessing} />
                                  : null
                          }
                          {
                              dialogContentProps.kind === 'Update'
                                ? <PrimaryButton
                                      onClick={onCapabilitiesFileUpdate}
                                      text={capabilitiesListState.isProcessing ? 'Updating' : 'Update'}
                                      disabled={capabilitiesListState.isProcessing} />
                                : null
                          }
                            <DefaultButton
                                onClick={toggleHideDialog}
                                text="Close"
                                disabled={capabilitiesListState.isProcessing} />
                            {
                                dialogContentProps.kind === 'Remove'
                                  ? <PrimaryButton
                                        onClick={onCapabilitiesFileRemove}
                                        style={{ backgroundColor: '#ce3939' }}
                                        text={capabilitiesListState.isProcessing ? 'Removing' : 'Remove'}
                                        disabled={capabilitiesListState.isProcessing}
                                    />
                                  : null
                            }
                        </DialogFooter>
                      : null
                }
            </Dialog>
            <PopupModal isOpen={isWarningDialogOpen} header={'Warning'} onClose={hideWarningDialog} text={capabilitiesListState.errorMsg} />
        </div>
  )
}

enum ElementType {
  Name,
  Description,
}
