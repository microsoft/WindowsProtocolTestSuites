/* eslint-disable @typescript-eslint/indent */
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
  ContextualMenu, DefaultButton, Dialog, DialogFooter, DialogType, PrimaryButton, SearchBox, Stack,
    TextField, IStackItemTokens, IStackTokens, MessageBar, MessageBarType, Label
} from '@fluentui/react'
import { CommandBar, ICommandBarItemProps } from '@fluentui/react/lib/CommandBar'
import { useBoolean } from '@uifabric/react-hooks'
import { StackGap5, StackGap10 } from '../components/StackStyle'
import { CapabilitiesGroupsPanel } from '../components/CapabilitiesGroupsPanel'
import { ReactNode, useCallback, useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { CapabilitiesDataSrv } from '../services/Capabilities'
import { CapabilitiesConfigActions } from '../actions/CapabilitiesConfigAction'
import { buildCapabilitiesFileRequest } from '../model/SaveCapabilitiesFileRequest'
import { AppState } from '../store/configureStore'

type CapabilitiesConfigDialogContentKind = 'Group' | 'Category'
type CapabilitiesConfigDialogActionKind = 'CreateUpdate' | 'Remove'

interface CapabilitiesConfigDialogContentProps {
  kind: CapabilitiesConfigDialogContentKind
  actionKind: CapabilitiesConfigDialogActionKind
  title: string
  action: (name: string) => void
  primaryButtonText: (isProcessing: Boolean) => string
}

export function ConfigureCapabilitiesConfig (props: any) {
  const capabilitiesConfigState = useSelector((state: AppState) => state.capabilitiesConfig)
  const selectedGroup = capabilitiesConfigState.selectedGroup
  const selectedCategoryInGroup = capabilitiesConfigState.selectedCategoryInGroup
  const testCasesInfo = capabilitiesConfigState.testCasesInfo
  const selectedTestCasesInfo = capabilitiesConfigState.selectedTestCasesInfo
  const selectedTestCasesView = capabilitiesConfigState.testCasesView
  const [hideDialog, { toggle: toggleHideDialog }] = useBoolean(true)
  const [showErrorMessageBar, setShowErrorMessageBar] = useState(false)
  const [groupName, setGroupName] = useState('')
  const [categoryName, setCategoryName] = useState('')
  const [errorMsg, setErrorMsg] = useState('')
  const dispatch = useDispatch()
  const [dialogContentProps, setDialogContentProps] = useState<CapabilitiesConfigDialogContentProps | undefined>(undefined)
  const stackTokens: IStackTokens = {
    maxHeight: '100%'
  }

  const stackItemTokens: IStackItemTokens = {
    padding: '0 10px'
  }

  useEffect(() => {
    const id: number = props.match.params.id
    dispatch(CapabilitiesDataSrv.getCapabilitiesConfig(id))
  }, [dispatch])

  // Show the message bar when an error message is available.
  useEffect(() => {
      if (capabilitiesConfigState.errorMsg) {
          setShowErrorMessageBar(true)
      } else {
          setShowErrorMessageBar(false)
      }
  }, [capabilitiesConfigState.errorMsg])

  const modalProps = {
    isBlocking: true,
    styles: { main: { innerWidth: 600, maxWidth: 650 } },
    dragOptions: {
      moveMenuItemText: 'Move',
      closeMenuItemText: 'Close',
      menu: ContextualMenu
    }
  }

  const onGroupNameChanged =
      useCallback((newValue?: string) => { setGroupName(newValue ?? '') }, [])

  const onCategoryNameChanged =
      useCallback((newValue?: string) => { setCategoryName(newValue ?? '') }, [])

  const containsInvalidCharacters = (name: string): Boolean => {
    const re = /^[A-Za-z0-9\-._ ]+$/ // Allow alphanumeric, space, hyphen, underscores, spaces and periods.
    if (!re.test(name)) {
      return true
    }

    return false
  }

  const groupNameValid = (name: string, currentValue: string): Boolean => {
    if (!name) {
      setErrorMsg('Group name cannot be empty.')
      return false
    }

    if (name.toLocaleLowerCase() !== (currentValue ?? '').toLocaleLowerCase()) {
      const existingGroupNames = capabilitiesConfigState.groups.map(g => g.Name)
      if (existingGroupNames.some(g => g.toLocaleLowerCase().trim() === name.toLocaleLowerCase().trim())) {
        setErrorMsg(`Group, '${name}' already exists.`)
        return false
      }
    }

    if (containsInvalidCharacters(name) === true) {
      setErrorMsg('Group names can contain only alphanumeric, hyphen, underscore, space and period characters.')
      return false
    }

    return true
  }

  const categoryNameValid = (name: string, currentValue: string): Boolean => {
    if (!name) {
      setErrorMsg('Category name cannot be empty.')
      return false
    }

    const selectedGroupName = capabilitiesConfigState.selectedGroup?.Name ?? ''
    if (name.toLocaleLowerCase() !== (currentValue ?? '').toLocaleLowerCase()) {
      const existingCategoryNames = capabilitiesConfigState.categoriesInGroup.map(c => c.Name)
      if (existingCategoryNames.some(c => c.toLocaleLowerCase().trim() === name.toLocaleLowerCase().trim())) {
        setErrorMsg(`Category, '${name}' already exists within the group, '${selectedGroupName}'.`)
        return false
      }
    }

    if (containsInvalidCharacters(name) === true) {
      setErrorMsg('Category names can contain only alphanumeric, hyphen, underscore, space and period characters.')
      return false
    }

    return true
  }

  const onGroupCreate = (name: string) => {
    if (groupNameValid(name, '') === true) {
      dispatch(CapabilitiesConfigActions.addCapabilitiesConfigGroup(name))
      toggleHideDialog()
      setGroupName('')
      setErrorMsg('')
    }
  }

  const onGroupUpdate = (name: string) => {
    const currentValue = capabilitiesConfigState.selectedGroup?.Name ?? ''
    if (groupNameValid(name, currentValue) === true) {
      dispatch(CapabilitiesConfigActions.updateCapabilitiesConfigGroup(name))
      toggleHideDialog()
      setGroupName('')
      setErrorMsg('')
    }
  }

  const onCategoryCreate = (name: string) => {
    if (categoryNameValid(name, '') === true) {
      dispatch(CapabilitiesConfigActions.addCapabilitiesConfigCategory(name))
      toggleHideDialog()
      setCategoryName('')
      setErrorMsg('')
    }
  }

  const onCategoryUpdate = (name: string) => {
    const currentValue = capabilitiesConfigState.selectedCategoryInGroup?.Name ?? ''
    if (categoryNameValid(name, currentValue) === true) {
      dispatch(CapabilitiesConfigActions.updateCapabilitiesConfigCategory(name))
      toggleHideDialog()
      setCategoryName('')
      setErrorMsg('')
    }
  }

  const onCategoryRemove = (name: string) => {
    dispatch(CapabilitiesConfigActions.removeCapabilitiesConfigCategory(name))
    toggleHideDialog()
    setCategoryName('')
    setErrorMsg('')
  }

  const onGroupRemove = (name: string) => {
    dispatch(CapabilitiesConfigActions.removeCapabilitiesConfigGroup(name))
    toggleHideDialog()
    setGroupName('')
    setErrorMsg('')
  }

  const onTestCasesAdd = () => {
    dispatch(CapabilitiesConfigActions.addTestCasesToSelectedCategory())
  }

  const onTestCasesRemove = () => {
    dispatch(CapabilitiesConfigActions.removeSelectedTestCasesFromSelectedCategory())
  }

  const onDownload = () => {
    const id: number = props.match.params.id
    dispatch(CapabilitiesDataSrv.downloadCapabilitiesFile(id))
  }

  const onSave = () => {
    const id: number = props.match.params.id
    const request = buildCapabilitiesFileRequest(capabilitiesConfigState.metadata,
      capabilitiesConfigState.groups, capabilitiesConfigState.testCasesInfo.TestsNotInAnyCategory)
    dispatch(CapabilitiesDataSrv.saveCapabilitiesFile(id, request,
      () => {

      }))
  }

  const createGroupDialogContentProps: CapabilitiesConfigDialogContentProps = {
    kind: 'Group',
    title: 'Create New Group',
    action: onGroupCreate,
    primaryButtonText: (isProcessing: Boolean) => isProcessing ? 'Creating' : 'Create',
    actionKind: 'CreateUpdate'
  }

  const updateGroupDialogContentProps: CapabilitiesConfigDialogContentProps = {
    kind: 'Group',
    title: 'Update Group',
    action: onGroupUpdate,
    primaryButtonText: (isProcessing: Boolean) => isProcessing ? 'Updating' : 'Update',
    actionKind: 'CreateUpdate'
  }

  const removeGroupDialogContentProps: CapabilitiesConfigDialogContentProps = {
    kind: 'Group',
    title: 'Remove Group',
    action: onGroupRemove,
    primaryButtonText: (isProcessing: Boolean) => isProcessing ? 'Removing' : 'Remove',
    actionKind: 'Remove'
  }

  const createCategoryDialogContentProps: CapabilitiesConfigDialogContentProps = {
    kind: 'Category',
    title: 'Create New Category',
    action: onCategoryCreate,
    primaryButtonText: (isProcessing: Boolean) => isProcessing ? 'Creating' : 'Create',
    actionKind: 'CreateUpdate'
  }

  const updateCategoryDialogContentProps: CapabilitiesConfigDialogContentProps = {
    kind: 'Category',
    title: 'Update Category',
    action: onCategoryUpdate,
    primaryButtonText: (isProcessing: Boolean) => isProcessing ? 'Updating' : 'Update',
    actionKind: 'CreateUpdate'
  }

  const removeCategoryDialogContentProps: CapabilitiesConfigDialogContentProps = {
    kind: 'Category',
    title: 'Remove Category',
    action: onCategoryRemove,
    primaryButtonText: (isProcessing: Boolean) => isProcessing ? 'Removing' : 'Remove',
    actionKind: 'Remove'
  }

  const buildMenuItems = (): ICommandBarItemProps[] => {
    const saveMenu: any = {
      key: 'save',
      text: 'Save',
      iconProps: { iconName: 'Save' },
      onClick: onSave
    }

    if (!capabilitiesConfigState.hasUnsavedChanges) {
        saveMenu.disabled = true
    }

    const downloadMenu = {
      key: 'download',
      text: 'Download',
      iconProps: { iconName: 'Download' },
      onClick: onDownload
    }

    const fileMenu = {
      key: 'file',
      text: 'File',
      iconProps: { iconName: 'FileTemplate' },
      subMenuProps: {
        items: [
          saveMenu,
          downloadMenu
        ]
      }
    }

    const groupsMenu = {
      key: 'groups',
      text: 'Groups',
      iconProps: { iconName: 'GroupedList' },
      subMenuProps: {
        items: [
          {
            key: 'new_group',
            text: 'New Group',
            iconProps: { iconName: 'Add' },
            onClick: () => {
              setDialogContentProps(createGroupDialogContentProps)
              toggleHideDialog()
            }
          }
        ]
      }
    }

    const commandBarMenus: any[] = [fileMenu, groupsMenu]

    if (selectedGroup !== undefined) {
      groupsMenu.subMenuProps.items.push(
        {
          key: 'edit_group',
          text: `Edit Group (${selectedGroup.Name})`,
          iconProps: { iconName: 'Edit' },
          onClick: () => {
            setGroupName(capabilitiesConfigState.selectedGroup?.Name ?? '')
            setDialogContentProps(updateGroupDialogContentProps)
            toggleHideDialog()
          }
        }
      )

      groupsMenu.subMenuProps.items.push(
        {
          key: 'delete_group',
          text: `Delete Group (${selectedGroup.Name})`,
          iconProps: { iconName: 'Delete' },
          onClick: () => {
            setGroupName(capabilitiesConfigState.selectedGroup?.Name ?? '')
            setDialogContentProps(removeGroupDialogContentProps)
            toggleHideDialog()
          }
        }
      )

      // Categories
      const categoriesMenu = {
        key: 'categories',
        text: 'Categories',
        iconProps: { iconName: 'Merge' },
        subMenuProps: {
          items: [
            {
              key: 'new_category',
              text: `New Category (for group: ${selectedGroup.Name})`,
              iconProps: { iconName: 'Add' },
              onClick: () => {
                setDialogContentProps(createCategoryDialogContentProps)
                toggleHideDialog()
              }
            }
          ]
        }
      }

      if (selectedCategoryInGroup !== undefined) {
        categoriesMenu.subMenuProps.items.push(
          {
            key: 'edit_category',
            text: `Edit Category (${selectedCategoryInGroup.Name})`,
            iconProps: { iconName: 'Edit' },
            onClick: () => {
              setCategoryName(capabilitiesConfigState.selectedCategoryInGroup?.Name ?? '')
              setDialogContentProps(updateCategoryDialogContentProps)
              toggleHideDialog()
            }
          }
        )

        categoriesMenu.subMenuProps.items.push(
          {
            key: 'delete_category',
            text: `Delete Category (${selectedCategoryInGroup.Name})`,
            iconProps: { iconName: 'Delete' },
            onClick: () => {
              setCategoryName(capabilitiesConfigState.selectedCategoryInGroup?.Name ?? '')
              setDialogContentProps(removeCategoryDialogContentProps)
              toggleHideDialog()
            }
          }
        )
      }

      commandBarMenus.push(categoriesMenu)

      // Switch on selectedTestCasesView
      if (selectedTestCasesView !== undefined) {
        const testCasesMenu: any = {
          key: 'testCases',
          text: 'Test Cases',
          iconProps: { iconName: 'TestSuite' },
          subMenuProps: {
            items: []
          }
        }

        if (selectedTestCasesView === 'InCategory') {
          testCasesMenu.subMenuProps.items.push(
            {
              key: 'delete_group',
              text: `Remove the selected test case(s) from category, '${selectedCategoryInGroup?.Name}'`,
              iconProps: { iconName: 'DependencyRemove' },
              disabled: selectedTestCasesInfo.TestsInCurrentCategory.length === 0,
              onClick: onTestCasesRemove
            }
          )
        } else {
          let disabled = true
          let onClick = () => {}

          if (selectedTestCasesView === 'OutCategory') {
            disabled = selectedTestCasesInfo.TestsInOtherCategories.length === 0
            onClick = onTestCasesAdd
          } else if (selectedTestCasesView === 'NoCategory') {
            disabled = selectedTestCasesInfo.TestsNotInAnyCategory.length === 0
            onClick = onTestCasesAdd
          }

          testCasesMenu.subMenuProps.items.push(
            {
              key: 'delete_group',
              text: `Add the selected test case(s) to category, '${selectedCategoryInGroup?.Name}'`,
              iconProps: { iconName: 'DependencyAdd' },
              disabled: disabled,
              onClick: onClick
            }
          )
        }

        commandBarMenus.push(testCasesMenu)
      }
    }

    return commandBarMenus
  }

  const reset = () => {
    setGroupName('')
    setCategoryName('')
    setErrorMsg('')
  }

  const onDialogDismiss = () => {
      toggleHideDialog()
      reset()
  }

  const dialog = () => {
    return <Dialog
          hidden={hideDialog}
          onDismiss={onDialogDismiss}
          dialogContentProps={{
            type: DialogType.normal,
            title: dialogContentProps?.title
          }}
          modalProps={modalProps}>
          {
            dialogContentProps?.actionKind === 'CreateUpdate'
              ? dialogContentProps?.kind === 'Group'
                ? <Stack tokens={StackGap10}>
                    <TextField
                        label="Group Name"
                        value={groupName}
                        disabled={capabilitiesConfigState.isProcessing}
                        errorMessage={errorMsg || capabilitiesConfigState.errorMsg}
                        onChange={(event: any, newValue?: string) => { onGroupNameChanged(newValue) }} />
                </Stack>
                : <Stack tokens={StackGap10}>
                    <TextField label="Group" readOnly defaultValue={selectedGroup?.Name} />
                    <TextField
                    label="Category Name"
                    value={categoryName}
                    disabled={capabilitiesConfigState.isProcessing}
                    errorMessage={errorMsg || capabilitiesConfigState.errorMsg}
                    onChange={(event: any, newValue?: string) => { onCategoryNameChanged(newValue) }} />
                        </Stack>
                : dialogContentProps?.kind === 'Group'
                    ? <div style={{ fontSize: 'large' }}>
                        <Stack horizontalAlign='start' tokens={StackGap5}>
                            <Label>Do you want to remove the following group?</Label>
                            <Stack horizontal><div style={{ paddingRight: 5 }}>Group:</div>{selectedGroup?.Name}</Stack>
                        </Stack>
                    </div>
                    : <div style={{ fontSize: 'large' }}>
                        <Stack horizontalAlign='start' tokens={StackGap5}>
                            <Label>Do you want to remove the following category?</Label>
                            <Stack horizontal><div style={{ paddingRight: 5 }}>Group:</div>{selectedGroup?.Name}</Stack>
                            <Stack horizontal><div style={{ paddingRight: 5 }}>Category:</div>{selectedCategoryInGroup?.Name}</Stack>
                        </Stack>
                    </div>
          }

        <DialogFooter>
              <PrimaryButton
                onClick={() =>
                  dialogContentProps?.kind === 'Group'
                    ? dialogContentProps?.action(groupName)
                    : dialogContentProps?.action(categoryName)
                 }
                  text={dialogContentProps?.primaryButtonText(capabilitiesConfigState.isProcessing)}
                  disabled={capabilitiesConfigState.isProcessing} />
              <DefaultButton
                  onClick={toggleHideDialog}
                  text="Close"
                  disabled={capabilitiesConfigState.isProcessing} />
          </DialogFooter>
      </Dialog>
  }

  return (
        <div>
          <Stack horizontal style={{ width: '100%', paddingLeft: 10, paddingRight: 10, paddingTop: 10 }} tokens={StackGap10}>
              <CommandBar items={buildMenuItems()} style={{ width: 480 }} />

              {
                  // selectedCategoryInGroup !== undefined
                  //  ? <SearchBox placeholder="Filter test cases (You can review filtering rules using the help link above)" style={{ width: 500 }} />
                  //  : ''
              }

          </Stack>
          <hr style={{ border: '1px solid #d9d9d9' }} />

          {
              showErrorMessageBar
                ? <MessageBar
                      messageBarType={MessageBarType.blocked}
                      isMultiline={false}
                      dismissButtonAriaLabel="Close"
                      onDismiss={() => setShowErrorMessageBar(false)}
                      truncated={true}>
                      <b>{capabilitiesConfigState.errorMsg}</b>
                  </MessageBar>
                : ''
          }

            <CapabilitiesGroupsPanel />
          {dialog()}
      </div>
  )
}
