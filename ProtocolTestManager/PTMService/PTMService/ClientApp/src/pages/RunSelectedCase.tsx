// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ContextualMenu, DefaultButton, DetailsList, DetailsListLayoutMode, Dialog, DialogFooter, DialogType, Dropdown, Fabric, IColumn, IDropdownOption, IObjectWithKey, Label, Link, MarqueeSelection, PrimaryButton, SearchBox, Selection, Stack } from '@fluentui/react'
import { useBoolean, useForceUpdate } from '@uifabric/react-hooks'
import { useCallback, useEffect, useMemo, useRef, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useHistory } from 'react-router-dom'
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard'
import { StackGap10 } from '../components/StackStyle'
import { StepPanel } from '../components/StepPanel'
import { useWindowSize } from '../components/UseWindowSize'
import { WizardNavBar } from '../components/WizardNavBar'
import { getNavSteps } from '../model/DefaultNavSteps'
import { ConfigurationsDataSrv } from '../services/Configurations'
import { TestSuitesDataSrv } from '../services/TestSuites'
import { SelectedTestCasesDataSrv } from '../services/SelectedTestCases'
import { AppState } from '../store/configureStore'
import { TestCase } from '../model/TestCase'
import { ContextualMenuControl, ContextualMenuItemProps } from '../components/ContextualMenuControl'
import { FileUploader, IFile } from '../components/FileUploader'
import { InvalidAppStateNotification } from '../components/InvalidAppStateNotification'

interface ListItem extends IObjectWithKey {
  Name: string
  FullName: string
}

const getListItems = (testCases: TestCase[]): ListItem[] => {
  return testCases.map(testCase => {
    return {
      key: testCase.FullName,
      Name: testCase.Name,
      FullName: testCase.FullName
    }
  })
}

function copyAndSort<T> (items: T[], columnKey: string, isSortedDescending?: boolean): T[] {
  const key = columnKey as keyof T
  return items.slice(0).sort((a: T, b: T) => ((isSortedDescending ? a[key] < b[key] : a[key] > b[key]) ? 1 : -1))
}

interface FilterByDropdownOption extends IDropdownOption {
  filterFunc: (filterPhrase: string | undefined) => (item: ListItem) => boolean
  filterPlaceholder: string
}

const isValidFilterPhrase = (filterPhrase: string | undefined) => {
  return filterPhrase !== undefined && filterPhrase !== null && filterPhrase !== ''
}

const filterByNameFunc = (filterPhrase: string | undefined) => (item: ListItem) => {
  return isValidFilterPhrase(filterPhrase)
    ? item.Name.toLocaleLowerCase().includes(filterPhrase!.toLocaleLowerCase())
    : true
}

const filterByDropdownOptions: FilterByDropdownOption[] = [
  {
    key: 'Name',
    text: 'Name',
    filterFunc: filterByNameFunc,
    filterPlaceholder: 'Filter by Name...'
  }
]

interface FilterByDropdownProps {
  options: FilterByDropdownOption[]
  onOptionChange: (newOption: FilterByDropdownOption) => void
}

function FilterByDropdown (props: FilterByDropdownProps) {
  return (
    props.options.length > 0
      ? <Stack horizontal tokens={StackGap10}>
        <Label style={{ alignSelf: 'center' }}>Filter By</Label>
        <Dropdown
          ariaLabel='Name of the property to be filtered'
          style={{ alignSelf: 'center', minWidth: 80 }}
          defaultSelectedKey={props.options[0].key}
          options={props.options}
          onChange={(_, newValue, __) => props.onOptionChange(newValue as FilterByDropdownOption)}
        />
      </Stack>
      : null
  )
}

const downloadPlaylist = (pageHTML: string) => {
  const currentDate = new Date()
  const blob = new Blob([pageHTML], { type: 'data:attachment/text' })
  if (blob === undefined) {
    return
  }

  const url = window.URL.createObjectURL(new Blob([blob]))
  const link = document.createElement('a')
  link.href = url
  const dateStr = currentDate.toISOString().replace(/:/g, '-').replace('T', '-').replace('Z', '').replace('.', '-').replace(/ /g, '-')
  link.setAttribute('download', 'ptmservice-' + dateStr + '.playlist')
  link.click()
}

const exportPlaylist = (items: string[]) => {
  let pageHTML: string = '<?xml version="1.0" encoding="utf-8"?><Playlist Version="1.0">'
  items.forEach((item) => {
    pageHTML += '<Add Test="' + item + '" />'
  })
  pageHTML += '</Playlist>'
  downloadPlaylist(pageHTML)
}

export function RunSelectedCase (props: StepWizardProps) {
  const dispatch = useDispatch()

  const testSuiteInfo = useSelector((state: AppState) => state.testSuiteInfo)
  const configuration = useSelector((state: AppState) => state.configurations)
  const filterInfo = useSelector((state: AppState) => state.filterInfo)
  const selectedTestCases = useSelector((state: AppState) => state.selectedTestCases)
  const configureMethod = useSelector((state: AppState) => state.configureMethod)

  const [filterPhrase, setFilterPhrase] = useState<string | undefined>(undefined)
  const [filterByDropdownOption, setFilterByDropdownOption] = useState<FilterByDropdownOption>(filterByDropdownOptions[0])
  const [filteredTestCases, setFilteredTestCases] = useState<ListItem[]>([])

  const [listColumns, setListColumns] = useState<IColumn[]>(() => [
    {
      key: 'Name',
      name: 'Name',
      fieldName: 'Name',
      minWidth: 360,
      isRowHeader: true,
      isResizable: true,
      isSorted: false,
      isSortedDescending: false,
      data: 'string',
      onColumnClick: (_, column) => onColumnHeaderClick(column)
    }
  ])

  const [selectedItems, setSelectedItems] = useState<string[]>([])

  const [selection, setSelection] = useState(() => {
    const s: Selection<IObjectWithKey> = new Selection({
      onSelectionChanged: () => {
        setSelectedItems(s.getSelection().map(item => item.key as string))
        setSelection(s)
        forceUpdate()
      }
    })
    s.setItems(filteredTestCases, true)
    return s
  })

  const [runAllClicked, setRunAllClicked] = useState(false)
  const [runSelectedClicked, setRunSelectedClicked] = useState(false)
  const [hideDialog, { toggle: toggleHideDialog }] = useBoolean(true)
  const [isUploadingPlaylist, setIsUploadingPlaylist] = useState(false)
  const [showSuccess, setShowSuccess] = useState(false)
  const [importingErrMsg, setImportingErrMsg] = useState<string | undefined>(undefined)
  const [file, setFile] = useState<IFile>()

  const onFileUploadSuccess = (files: IFile[]): void => {
    if (files.length > 0) {
      setFile(files[0])
    }
  }

  const onImportPlaylist = () => {
    if (file != null) {
      setIsUploadingPlaylist(true)
      const reader = new FileReader()
      const parser = new DOMParser()
      reader.onloadend = (event) => {
        const tests: string[] = []
        const readerData = event.target?.result?.toString()
        const playlistData = parser.parseFromString(readerData ?? '', 'text/xml')
        playlistData.querySelectorAll('Add').forEach((i) => {
          const selectedKey: string = i.getAttribute('Test')?.toString() ?? ''
          if (selectedKey !== '') tests.push(selectedKey)
        })
        // Disable onSelectionChanged to avoid long rendering time.
        selection.setChangeEvents(false, true)
        // Select the imported items.
        const selectionItems = selection.getItems().map(item => item.key)
        tests.forEach(test => { if (selectionItems.includes(test)) selection.setKeySelected(test, true, false) })
        // Enable onSelectionChanged.
        selection.setChangeEvents(true, false)
        // Update selection.
        setSelectedItems(selection.getSelection().map(item => item.key as string))
        setSelection(selection)
        // Update states.
        setShowSuccess(true)
        setImportingErrMsg(undefined)
        setIsUploadingPlaylist(false)
        // Force update.
        forceUpdate()
      }
      reader.readAsText(file.File)
    } else {
      setImportingErrMsg('Playlist file cannot be empty')
    }
  }

  const dialogContentProps = {
    type: DialogType.normal,
    title: 'Import Playlist'
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

  const listColumnsRef = useRef<IColumn[]>()
  listColumnsRef.current = listColumns

  const filteredTestCasesRef = useRef<ListItem[]>()
  filteredTestCasesRef.current = filteredTestCases

  const allListItems = useMemo(() => getListItems(filterInfo.listSelectedCases), [filterInfo.listSelectedCases])
  const forceUpdate = useForceUpdate()

  const wizardProps: StepWizardChildProps = props as StepWizardChildProps
  const navSteps = getNavSteps(wizardProps, configureMethod)
  const wizard = WizardNavBar(wizardProps, navSteps)
  const winSize = useWindowSize()

  const history = useHistory()

  useEffect(() => {
    dispatch(ConfigurationsDataSrv.getRules())
    dispatch(TestSuitesDataSrv.getTestSuiteTestCases())
  }, [dispatch])

  useEffect(() => {
    if (!isValidFilterPhrase(filterPhrase)) {
      setFilteredTestCases(allListItems)
    }
  }, [filterPhrase, allListItems])

  if (testSuiteInfo.selectedTestSuite === undefined || configuration.selectedConfiguration === undefined) {
    return <InvalidAppStateNotification
      testSuite={testSuiteInfo.selectedTestSuite}
      configuration={configuration.selectedConfiguration}
      wizard={wizard}
      wizardProps={wizardProps} />
  }

  const onColumnHeaderClick = useCallback((column: IColumn) => {
    const currColumn: IColumn = listColumnsRef.current!.filter(currCol => column.key === currCol.key)[0]
    const newColumns = listColumnsRef.current!.map((newCol: IColumn) => {
      if (newCol.key === currColumn.key) {
        return {
          ...currColumn,
          isSorted: true,
          isSortedDescending: !currColumn.isSortedDescending
        }
      } else {
        return {
          ...newCol,
          isSorted: false,
          isSortedDescending: true
        }
      }
    })

    const newItems = copyAndSort(filteredTestCasesRef.current!, currColumn.fieldName!, !currColumn.isSortedDescending)
    setListColumns(newColumns)
    setFilteredTestCases(newItems)
  }, [])

  const onFilterPhraseChanged = (filterPhrase: string | undefined) => {
    setFilterPhrase(filterPhrase)
    selection.setAllSelected(false)

    setFilteredTestCases(allListItems.filter(filterByDropdownOption.filterFunc(filterPhrase)))
  }

  const onFilterPhraseClear = () => {
    setFilterPhrase(undefined)
    selection.setAllSelected(false)

    setFilteredTestCases(allListItems)
  }

  const onRunAllCasesClick = () => {
    if (filterInfo.listSelectedCases.length === 0) {
      return
    }

    setRunAllClicked(true)
    dispatch(SelectedTestCasesDataSrv.createRunRequest(filterInfo.listSelectedCases.map(e => e.FullName), undefined, () => {
      history.push('/Tasks/History', { from: 'RunSelectedCase' })
    }))
  }

  const getRunAllButtonText = () => {
    if (runAllClicked && selectedTestCases.isPosting) {
      return 'Processing...'
    } else {
      return 'Run all cases'
    }
  }

  const onRunSelectedCasesClick = () => {
    if (selectedItems.length === 0) {
      return
    }

    setRunSelectedClicked(true)
    dispatch(SelectedTestCasesDataSrv.createRunRequest(selectedItems, undefined, () => {
      history.push('/Tasks/History', { from: 'RunSelectedCase' })
    }))
  }

  const getRunSelectedButtonText = () => {
    if (runSelectedClicked && selectedTestCases.isPosting) {
      return 'Processing...'
    } else {
      return `Run selected cases (${selectedItems.length}/${filterInfo.listSelectedCases.length})`
    }
  }

  const onShowImportDialog = () => {
    setImportingErrMsg(undefined)
    setIsUploadingPlaylist(false)
    setShowSuccess(false)
    toggleHideDialog()
    setFile(undefined)
  }

  const exportTestCases = () => {
    if (filterInfo.listSelectedCases.length === 0) {
      return
    }
    exportPlaylist(filterInfo.listSelectedCases.map(e => e.FullName))
  }

  const exportCheckedTestCases = () => {
    if (selectedItems.length === 0) {
      return
    }
    exportPlaylist(selectedItems)
  }

  const getMenuItems = (): ContextualMenuItemProps[] => {
    return [
      {
        key: 'ExportAll',
        text: 'Export All Test Cases to Playlist',
        disabled: filterInfo.listSelectedCases.length === 0,
        menuAction: exportTestCases
      },
      {
        key: 'ExportSelected',
        text: 'Export Selected Test Cases to Playlist',
        disabled: selectedItems.length === 0,
        menuAction: exportCheckedTestCases
      },
      {
        key: 'Import',
        text: 'Import Playlist...',
        disabled: false,
        menuAction: onShowImportDialog
      }
    ]
  }

  return (
    <StepPanel leftNav={wizard} isLoading={filterInfo.isCasesLoading} errorMsg={filterInfo.errorMsg ?? selectedTestCases.errorMsg}>
      <Stack style={{ paddingLeft: 10 }}>
        <Fabric>
          <Stack horizontal tokens={StackGap10}>
            <FilterByDropdown
              options={filterByDropdownOptions}
              onOptionChange={(newValue) => setFilterByDropdownOption(newValue)} />
            <SearchBox
              style={{ width: 280 }}
              placeholder={filterByDropdownOption.filterPlaceholder}
              onChange={(_, newValue) => onFilterPhraseChanged(newValue)}
              onReset={onFilterPhraseClear}
            />
            {
              !isValidFilterPhrase(filterPhrase)
                ? null
                : <Label>{`Number of test cases after filter applied: ${filteredTestCases.length}`}</Label>
            }
          </Stack>
          <hr style={{ border: '1px solid #d9d9d9' }} />
          <div style={{ height: winSize.height - 170, overflowY: 'auto' }}>
            <MarqueeSelection selection={selection}>
              <DetailsList
                ariaLabelForSelectAllCheckbox='Select all test cases in the list'
                items={filteredTestCases}
                setKey="set"
                columns={listColumns}
                layoutMode={DetailsListLayoutMode.justified}
                selection={selection}
                selectionPreservedOnEmptyClick={true}
                compact
              />
            </MarqueeSelection>
          </div>
        </Fabric>
        <div className='buttonPanel'>
          <Stack horizontal horizontalAlign='end' tokens={StackGap10}>
            <PrimaryButton text='Previous' disabled={selectedTestCases.isPosting} onClick={() => wizardProps.previousStep()} />
            <ContextualMenuControl text="Import/Export Playlist" shouldFocusOnMount={true} menuItems={getMenuItems()} />
            <PrimaryButton style={{ width: 240 }} text={getRunSelectedButtonText()} disabled={selectedTestCases.isPosting || selectedItems.length === 0} onClick={onRunSelectedCasesClick} />
            <PrimaryButton text={getRunAllButtonText()} disabled={selectedTestCases.isPosting || filterInfo.listSelectedCases.length === 0} onClick={onRunAllCasesClick} />
          </Stack>
        </div>
      </Stack>
      <Dialog
        hidden={hideDialog}
        onDismiss={toggleHideDialog}
        dialogContentProps={dialogContentProps}
        modalProps={modalProps}
      >
        {
          showSuccess
            ? <div>Import playlist successfully!</div>
            : <Stack tokens={StackGap10}>
              <p style={{ color: 'red', padding: 3 }}>{importingErrMsg}</p>
              <FileUploader
                label="Package"
                onSuccess={onFileUploadSuccess}
                maxFileCount={1}
                disabled={isUploadingPlaylist}
                suffix={['.playlist']}
                placeholder="Select .playlist file"
              />
            </Stack>
        }
        <DialogFooter>
          {!showSuccess &&
            <PrimaryButton onClick={onImportPlaylist} text={isUploadingPlaylist ? 'Uploading...' : 'Import Playlist'} disabled={isUploadingPlaylist} />}
          <DefaultButton onClick={toggleHideDialog} text="Close" disabled={isUploadingPlaylist} />
        </DialogFooter>
      </Dialog>
    </StepPanel>
  )
};
