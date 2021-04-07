// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { DefaultButton, DetailsList, DetailsListLayoutMode, Dropdown, Fabric, IColumn, IDropdownOption, IObjectWithKey, Label, MarqueeSelection, PrimaryButton, SearchBox, Selection, Stack } from '@fluentui/react';
import { useForceUpdate } from '@uifabric/react-hooks';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useHistory } from 'react-router-dom';
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard';
import { StackGap10 } from '../components/StackStyle';
import { StepPanel } from '../components/StepPanel';
import { useWindowSize } from '../components/UseWindowSize';
import { WizardNavBar } from '../components/WizardNavBar';
import { getNavSteps } from '../model/DefaultNavSteps';
import { SelectedTestCasesDataSrv } from '../services/SelectedTestCases';
import { AppState } from '../store/configureStore';

interface ListItem extends IObjectWithKey {
    Name: string;
}

const getListItems = (testCases: string[]): ListItem[] => {
    return testCases.map(testCaseName => {
        return {
            key: testCaseName,
            Name: testCaseName
        };
    });
};

function copyAndSort<T>(items: T[], columnKey: string, isSortedDescending?: boolean): T[] {
    const key = columnKey as keyof T;
    return items.slice(0).sort((a: T, b: T) => ((isSortedDescending ? a[key] < b[key] : a[key] > b[key]) ? 1 : -1));
}

interface FilterByDropdownOption extends IDropdownOption {
    filterFunc: (filterPhrase: string | undefined) => (item: ListItem) => boolean;
    filterPlaceholder: string;
}

const isValidFilterPhrase = (filterPhrase: string | undefined) => {
    return filterPhrase !== undefined && filterPhrase !== null && filterPhrase !== '';
};

const filterByNameFunc = (filterPhrase: string | undefined) => (item: ListItem) => {
    return isValidFilterPhrase(filterPhrase) ?
        item.Name.toLocaleLowerCase().indexOf(filterPhrase!.toLocaleLowerCase()) >= 0
        : true;
};

const filterByDropdownOptions: FilterByDropdownOption[] = [
    {
        key: 'Name',
        text: 'Name',
        filterFunc: filterByNameFunc,
        filterPlaceholder: 'Filter by Name...'
    }
];

type FilterByDropdownProps = {
    options: FilterByDropdownOption[],
    onOptionChange: (newOption: FilterByDropdownOption) => void
}

function FilterByDropdown(props: FilterByDropdownProps) {
    return (
        props.options.length > 0
            ? <Stack horizontal tokens={StackGap10}>
                <Label style={{ alignSelf: 'center' }}>Filter By</Label>
                <Dropdown
                    style={{ alignSelf: 'center', minWidth: 80 }}
                    defaultSelectedKey={props.options[0].key}
                    options={props.options}
                    onChange={(_, newValue, __) => props.onOptionChange(newValue as FilterByDropdownOption)}
                />
            </Stack>
            : null
    );
}

export function RunSelectedCase(props: StepWizardProps) {
    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;
    const navSteps = getNavSteps(wizardProps);
    const wizard = WizardNavBar(wizardProps, navSteps);
    const winSize = useWindowSize();

    const history = useHistory();

    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(SelectedTestCasesDataSrv.getAllTestCases());
    }, [dispatch]);

    const [filterPhrase, setFilterPhrase] = useState<string | undefined>(undefined);
    const [filterByDropdownOption, setFilterByDropdownOption] = useState<FilterByDropdownOption>(filterByDropdownOptions[0]);
    const [filteredTestCases, setFilteredTestCases] = useState<ListItem[]>([]);

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
    ]);

    const [selectedItems, setSelectedItems] = useState<string[]>([]);

    const [selection, setSelection] = useState(() => {
        const s: Selection<IObjectWithKey> = new Selection({
            onSelectionChanged: () => {
                setSelectedItems(s.getSelection().map(item => item.key as string));
                setSelection(s);
                forceUpdate();
            }
        });
        s.setItems(filteredTestCases, true);
        return s;
    });

    const [runAllClicked, setRunAllClicked] = useState(false);
    const [runSelectedClicked, setRunSelectedClicked] = useState(false);

    const selectedTestCases = useSelector((state: AppState) => state.selectedTestCases);

    const allListItems = useMemo(() => getListItems(selectedTestCases.allTestCases), [selectedTestCases.allTestCases]);

    const listColumnsRef = useRef<IColumn[]>();
    listColumnsRef.current = listColumns;

    const filteredTestCasesRef = useRef<ListItem[]>();
    filteredTestCasesRef.current = filteredTestCases;

    const forceUpdate = useForceUpdate();

    useEffect(() => {
        if (!isValidFilterPhrase(filterPhrase)) {
            setFilteredTestCases(allListItems);
        }
    }, [filterPhrase, allListItems]);

    const onColumnHeaderClick = useCallback((column: IColumn) => {
        const currColumn: IColumn = listColumnsRef.current!.filter(currCol => column.key === currCol.key)[0]
        const newColumns = listColumnsRef.current!.map((newCol: IColumn) => {
            if (newCol.key === currColumn.key) {
                return {
                    ...currColumn,
                    isSorted: true,
                    isSortedDescending: !currColumn.isSortedDescending,
                };
            } else {
                return {
                    ...newCol,
                    isSorted: false,
                    isSortedDescending: true
                };
            }
        });

        const newItems = copyAndSort(filteredTestCasesRef.current!, currColumn.fieldName!, !currColumn.isSortedDescending);
        setListColumns(newColumns);
        setFilteredTestCases(newItems);
    }, []);

    const onFilterPhraseChanged = (filterPhrase: string | undefined) => {
        setFilterPhrase(filterPhrase);
        selection.setAllSelected(false);

        setFilteredTestCases(allListItems.filter(filterByDropdownOption.filterFunc(filterPhrase)));
    };

    const onFilterPhraseClear = () => {
        setFilterPhrase(undefined);
        selection.setAllSelected(false);

        setFilteredTestCases(allListItems);
    };

    const onRunAllCasesClick = () => {
        if (selectedTestCases.allTestCases.length === 0) {
            return;
        }

        setRunAllClicked(true);
        dispatch(SelectedTestCasesDataSrv.createRunRequest(selectedTestCases.allTestCases, () => {
            history.push('/Tasks/History', { from: 'RunSelectedCase' });
        }));
    };

    const getRunAllButtonText = () => {
        if (runAllClicked && selectedTestCases.isPosting) {
            return "Processing...";
        }
        else {
            return "Run all cases";
        }
    }

    const onRunSelectedCasesClick = () => {
        if (selectedItems.length === 0) {
            return;
        }

        setRunSelectedClicked(true);
        dispatch(SelectedTestCasesDataSrv.createRunRequest(selectedItems, () => {
            history.push('/Tasks/History', { from: 'RunSelectedCase' });
        }));
    };

    const getRunSelectedButtonText = () => {
        if (runSelectedClicked && selectedTestCases.isPosting) {
            return "Processing..."
        }
        else {
            return `Run selected cases (${selectedItems.length}/${selectedTestCases.allTestCases.length})`;
        }
    }

    return (
        <StepPanel leftNav={wizard} isLoading={selectedTestCases.isLoading} errorMsg={selectedTestCases.errorMsg}>
            <div>
                <div>
                    <Stack style={{ padding: 10 }}>
                        <Fabric style={{ paddingBottom: 20 }}>
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
                            <hr style={{ border: "1px solid #d9d9d9" }} />
                            <div style={{ height: winSize.height - 210, overflowY: 'auto' }}>
                                <MarqueeSelection selection={selection}>
                                    <DetailsList
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
                        <div style={{ borderTop: '1px solid #d9d9d9', backgroundColor: '#f5f5f5', paddingTop: 7, paddingLeft: 45, paddingRight: 45, height: 40 }}>
                            <Stack horizontal>
                                <Stack horizontalAlign='start' style={{ paddingRight: '61%' }}>
                                    <PrimaryButton text='Previous' disabled={selectedTestCases.isPosting} onClick={() => wizardProps.previousStep()} />
                                </Stack>
                                <Stack horizontal horizontalAlign='end' tokens={{ childrenGap: 25 }}>
                                    <DefaultButton text={getRunAllButtonText()} disabled={selectedTestCases.isPosting || selectedTestCases.allTestCases.length === 0} onClick={onRunAllCasesClick} />
                                    <DefaultButton style={{ width: 240 }} text={getRunSelectedButtonText()} disabled={selectedTestCases.isPosting || selectedItems.length === 0} onClick={onRunSelectedCasesClick} />
                                </Stack>
                            </Stack>
                        </div>
                    </Stack>
                </div>
            </div>
        </StepPanel>
    );
};