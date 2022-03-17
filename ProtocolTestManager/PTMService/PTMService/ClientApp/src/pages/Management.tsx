// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { CommandBarButton, ContextualMenu, DefaultButton, DetailsList, DetailsListLayoutMode, Dialog, DialogFooter, DialogType, IColumn, Label, PrimaryButton, SearchBox, SelectionMode, Stack, TextField } from '@fluentui/react'
import { useBoolean } from '@uifabric/react-hooks'
import { useCallback, useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useHistory } from 'react-router-dom'
import { ManagementActions } from '../actions/ManagementAction'
import { WizardNavBarActions } from '../actions/WizardNavBarAction'
import { TestSuiteInfoActions } from '../actions/TestSuiteInfoAction'
import { FileUploader, IFile } from '../components/FileUploader'
import { PopupModal } from '../components/PopupModal'
import { StackGap10, StackGap5 } from '../components/StackStyle'
import { TestSuite } from '../model/TestSuite'
import { ManagementDataSrv } from '../services/Management'
import { AppState } from '../store/configureStore'
import { RunSteps } from '../model/DefaultNavSteps'

type TestSuiteManagementDialogKind = 'Install' | 'Update' | 'Remove'

interface TestSuiteManagementDialogContentProps {
  kind: TestSuiteManagementDialogKind
  type: DialogType
  title: string
}

export function Management(props: any) {
  const history = useHistory()
  const dispatch = useDispatch()
  const managementState = useSelector((state: AppState) => state.management)
  const [isWarningDialogOpen, { setTrue: showWarningDialog, setFalse: hideWarningDialog }] = useBoolean(false)
  const [hideDialog, { toggle: toggleHideDialog }] = useBoolean(true)
  const [dialogContentProps, setDialogContentProps] = useState<TestSuiteManagementDialogContentProps | undefined>(undefined)
  const [currentTestSuite, setCurrentTestSuite] = useState<TestSuite | undefined>(undefined)
  const [testSuiteName, setTestSuiteName] = useState('')
  const [testSuiteDescription, setTestSuiteDescription] = useState('')
  const [testSuiteErrorMsg, setTestSuiteErrorMsg] = useState('')
  const [file, setFile] = useState<IFile>()

  useEffect(() => {
    dispatch(ManagementDataSrv.getTestSuiteList())
  }, [dispatch])

  useEffect(() => {
    if (managementState.errorMsg !== undefined) {
      showWarningDialog()
    }
  }, [managementState])

  const onFieldChange = useCallback(
    (element: ElementType, newValue?: string) => {
      switch (element) {
        case ElementType.Name:
          setTestSuiteName(newValue!)
          break
        case ElementType.Description:
          setTestSuiteDescription(newValue!)
          break
        default:
          break
      }
    }, [])

  const installDialogContentProps: TestSuiteManagementDialogContentProps = {
    kind: 'Install',
    type: DialogType.normal,
    title: 'Install Test Suite'
  }

  const updateDialogContentProps: TestSuiteManagementDialogContentProps = {
    kind: 'Update',
    type: DialogType.normal,
    title: 'Update Test Suite'
  }

  const removeDialogContentProps: TestSuiteManagementDialogContentProps = {
    kind: 'Remove',
    type: DialogType.normal,
    title: 'Remove Test Suite'
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

  const onTestSuiteInstall = () => {
    if (testSuiteName && (file != null)) {
      dispatch(ManagementDataSrv.installTestSuite({
        TestSuiteName: testSuiteName,
        Description: testSuiteDescription,
        Package: file.File
      }, () => {
        toggleHideDialog()
        setCurrentTestSuite(undefined)
        setTestSuiteName('')
        setTestSuiteDescription('')
        setTestSuiteErrorMsg('')
        dispatch(ManagementDataSrv.getTestSuiteList())
      }))
    } else {
      if (!testSuiteName) {
        setTestSuiteErrorMsg('TestSuite Name can\'t be null')
        return
      }
      if (file == null) {
        setTestSuiteErrorMsg('TestSuite package can\'t be null')
        return
      }
      setTestSuiteErrorMsg('')
    }
  }

  const onTestSuiteUpdate = () => {
    if (currentTestSuite !== undefined && testSuiteName && (file != null)) {
      setTestSuiteErrorMsg('')
      dispatch(ManagementDataSrv.updateTestSuite(currentTestSuite.Id, {
        TestSuiteName: testSuiteName,
        Description: testSuiteDescription,
        Package: file.File
      }, () => {
        toggleHideDialog()
        setCurrentTestSuite(undefined)
        setTestSuiteName('')
        setTestSuiteDescription('')
        dispatch(ManagementDataSrv.getTestSuiteList())
      }))
    } else {
      if (!testSuiteName) {
        setTestSuiteErrorMsg('TestSuite Name can\'t be null')
        return
      }
      if (file == null) {
        setTestSuiteErrorMsg('TestSuite package can\'t be null')
        return
      }
      setTestSuiteErrorMsg('')
    }
  }

  const onTestSuiteRemove = () => {
    if (currentTestSuite !== undefined) {
      dispatch(ManagementDataSrv.removeTestSuite(currentTestSuite.Id,
        () => {
          toggleHideDialog()
          setCurrentTestSuite(undefined)
          setTestSuiteName('')
          setTestSuiteDescription('')
          setTestSuiteErrorMsg('')
          dispatch(ManagementDataSrv.getTestSuiteList())
        }))
    }
  }

  const getCurrentTestSuite = (id: number) => managementState.displayList.find((value) => value.Id === id)

  const columns = getTestSuiteGridColumns({
    onRerun: (id: number) => {
      const foundTestSuite = getCurrentTestSuite(id)
      if (foundTestSuite != null) {
        dispatch(TestSuiteInfoActions.setSelectedTestSuiteAction(foundTestSuite))
        dispatch(WizardNavBarActions.setWizardNavBarAction(RunSteps.SELECT_CONFIGURATION))
        dispatch(() => history.push('/Tasks/NewRun#SelectConfiguration', { from: 'Management' }))
      }
    },
    onUpdate: (id: number) => {
      const foundTestSuite = getCurrentTestSuite(id)
      if (foundTestSuite != null) {
        setCurrentTestSuite(foundTestSuite)
        setTestSuiteName(foundTestSuite.Name)
        setTestSuiteDescription(foundTestSuite.Description ?? '')
        setDialogContentProps(updateDialogContentProps)
        toggleHideDialog()
      }
    },
    onRemove: (id: number) => {
      const foundTestSuite = getCurrentTestSuite(id)
      if (foundTestSuite != null) {
        setCurrentTestSuite(foundTestSuite)
        setTestSuiteName(foundTestSuite.Name)
        setTestSuiteDescription(foundTestSuite.Description ?? '')
        setDialogContentProps(removeDialogContentProps)
        toggleHideDialog()
      }
    }
  })

  const onSearchChanged = (newValue: string): void => {
    dispatch(ManagementActions.setSearchTextAction(newValue))
  }

  const onFileUploadSuccess = (files: IFile[]): void => {
    if (files.length > 0) {
      setFile(files[0])
    }
  }

  return (
    <div>
      <Stack horizontal horizontalAlign="end" style={{ paddingLeft: 10, paddingRight: 10, paddingTop: 10 }} tokens={StackGap10}>
        <SearchBox placeholder="Search" onSearch={onSearchChanged} />
        <PrimaryButton
          iconProps={{ iconName: 'Add' }}
          allowDisabledFocus
          text='Install Test Suite'
          onClick={() => {
            setDialogContentProps(installDialogContentProps)
            toggleHideDialog()
          }} />
      </Stack>
      <hr style={{ border: '1px solid #d9d9d9' }} />
      <div style={{ padding: 10 }}>
        <DetailsList
          items={managementState.displayList}
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
                label="Test Suite Name"
                value={testSuiteName}
                disabled={managementState.isProcessing}
                onChange={(event: any, newValue?: string) => { onFieldChange(ElementType.Name, newValue) }} />
              <FileUploader
                label="Package"
                onSuccess={onFileUploadSuccess}
                disabled={managementState.isProcessing}
                maxFileCount={1}
                suffix={['.zip', '.gz']}
                placeholder="Please click to upload test suite package" />
              <TextField
                label="Description"
                multiline={true}
                value={testSuiteDescription}
                disabled={managementState.isProcessing}
                errorMessage={testSuiteErrorMsg}
                onChange={(event: any, newValue?: string) => { onFieldChange(ElementType.Description, newValue) }} />
            </Stack>
            : <div style={{ fontSize: 'large' }}>
              <Stack horizontalAlign='start' tokens={StackGap5}>
                <Label>Do you want to remove the following test suite?</Label>
                <Stack horizontal><div style={{ paddingRight: 5 }}>ID:</div>{currentTestSuite?.Id}</Stack>
                <Stack horizontal><div style={{ paddingRight: 5 }}>TestSuite:</div>{testSuiteName + ' ' + currentTestSuite?.Version}</Stack>
                <Stack horizontal><div style={{ paddingRight: 5 }}>Description:</div>{testSuiteDescription}</Stack>
              </Stack>
            </div>
        }
        {
          dialogContentProps !== undefined
            ? <DialogFooter>
              {
                dialogContentProps.kind === 'Install'
                  ? <PrimaryButton
                    onClick={onTestSuiteInstall}
                    text={managementState.isProcessing ? 'Installing' : 'Install'}
                    disabled={managementState.isProcessing} />
                  : null
              }
              {
                dialogContentProps.kind === 'Update'
                  ? <PrimaryButton
                    onClick={onTestSuiteUpdate}
                    text={managementState.isProcessing ? 'Updating' : 'Update'}
                    disabled={managementState.isProcessing} />
                  : null
              }
              <DefaultButton
                onClick={toggleHideDialog}
                text="Close"
                disabled={managementState.isProcessing} />
              {
                dialogContentProps.kind === 'Remove'
                  ? <PrimaryButton
                    onClick={onTestSuiteRemove}
                    style={{ backgroundColor: '#ce3939' }}
                    text={managementState.isProcessing ? 'Removing' : 'Remove'}
                    disabled={managementState.isProcessing}
                  />
                  : null
              }
            </DialogFooter>
            : null
        }
      </Dialog>
      <PopupModal isOpen={isWarningDialogOpen} header={'Warning'} onClose={hideWarningDialog} text={managementState.errorMsg} />
    </div>
  )
};

function getTestSuiteGridColumns(props: {
  onRerun: (id: number) => void
  onUpdate: (id: number) => void
  onRemove: (id: number) => void
}): IColumn[] {
  return [
    {
      key: 'Name',
      name: 'Test Suite',
      ariaLabel: 'Name of the Test Suite',
      fieldName: 'Name',
      minWidth: 200,
      maxWidth: 300,
      isResizable: true
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
      isPadded: true
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
      minWidth: 300,
      isRowHeader: true,
      isResizable: true,
      data: 'string',
      isPadded: true,
      onRender: (item: TestSuite, index, column) => {
        return <Stack horizontal tokens={StackGap10}>
          <CommandBarButton iconProps={{ iconName: 'Rerun' }} text="Rerun" onClick={() => { props.onRerun(item.Id) }} />
          <CommandBarButton iconProps={{ iconName: 'UpgradeAnalysis' }} text="Update" onClick={() => { props.onUpdate(item.Id) }} />
          <CommandBarButton iconProps={{ iconName: 'RemoveFromTrash' }} text="Remove" onClick={() => { props.onRemove(item.Id) }} />
        </Stack>
      }
    }
  ]
}

enum ElementType {
  Name,
  Description,
}
