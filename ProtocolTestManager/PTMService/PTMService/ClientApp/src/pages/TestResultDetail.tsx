// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { DetailsList, DetailsListLayoutMode, Dialog, DialogFooter, DialogType, IGroup, IObjectWithKey, Label, PrimaryButton, SearchBox, Selection, SelectionMode, SelectionZone, Spinner, SpinnerSize, Stack } from '@fluentui/react';
import { useBoolean, useForceUpdate } from '@uifabric/react-hooks';
import { useCallback, useEffect, useMemo, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useHistory } from 'react-router-dom';
import { TestCaseResultActions } from '../actions/TestCaseResultActions';
import { TestResultsActions } from '../actions/TestResultsActions';
import { FullWidthPanel } from '../components/FullWidthPanel';
import { StackGap10, StackGap5 } from '../components/StackStyle';
import { useWindowSize } from '../components/UseWindowSize';
import { TestCaseOverview, TestCaseResult } from '../model/TestCaseResult';
import { SelectedTestCasesDataSrv } from '../services/SelectedTestCases';
import { TestCaseResultDataSrv } from '../services/TestCaseResult';
import { TestResultsDataSrv } from '../services/TestResults';
import { AppState } from '../store/configureStore';

type GroupKeys = {
    KeyName: string,
    Keys: { key: string, name: string }[],
    KeyComparer: (result: TestCaseOverview, key: string) => boolean
}

const groupByTestCaseStateKeys: GroupKeys = {
    KeyName: 'State',
    Keys: [
        {
            key: 'Passed',
            name: 'Passed'
        },
        {
            key: 'Failed',
            name: 'Failed'
        },
        {
            key: 'Inconclusive',
            name: 'Inconclusive'
        },
        {
            key: 'Running',
            name: 'Running'
        },
        {
            key: 'NotRun',
            name: 'Not Run'
        }
    ],
    KeyComparer: (result, key) => result.State === key
}

const isValidFilterPhrase = (filterPhrase: string | undefined) => {
    return filterPhrase !== undefined && filterPhrase !== null && filterPhrase !== '';
};

const filterByNameFunc = (filterPhrase: string | undefined) => (item: TestCaseOverview) => {
    return isValidFilterPhrase(filterPhrase) ?
        item.FullName.toLocaleLowerCase().indexOf(filterPhrase!.toLocaleLowerCase()) >= 0
        : true;
};

type SelectionItem = TestCaseOverview & IObjectWithKey;

const getSelectionItems = (testCaseResults: TestCaseOverview[]) => {
    return testCaseResults.map((result: TestCaseOverview) => {
        return {
            ...result,
            key: result.FullName
        };
    });
};

type TestCaseResultViewProps = {
    winSize: { width: number, height: number },
    result: TestCaseResult | undefined
}

const lineHeader = /\d{4}\-\d{2}\-\d{2} \d{2}:\d{2}:\d{2}\.\d{3} \[(?<kind>\w+)\]/g;

const getLineBackgroundColor = (kind: string) => {
    if (kind.includes('Failed')) {
        return 'red';
    } else if (kind.includes('Inconclusive')) {
        return 'yellow';
    } else if (kind.includes('Succeeded')) {
        return 'green';
    }

    return 'transparent';
};

const renderOutputLines = (lines: string[]) => {
    return lines.reduce((res: { elements: JSX.Element[], index: number }, currentLine) => {
        lineHeader.lastIndex = 0;
        const matches = lineHeader.exec(currentLine);
        if (matches !== null && matches.groups !== undefined) {
            return {
                elements: [...res.elements, <p key={res.index} style={{ overflowWrap: 'normal', backgroundColor: getLineBackgroundColor(matches.groups.kind) }}>{currentLine}</p>],
                index: res.index + 1
            };
        }
        else {
            return {
                elements: [...res.elements, <p key={res.index} style={{ overflowWrap: 'normal' }}>&nbsp;&nbsp;&nbsp;&nbsp;{currentLine}</p>],
                index: res.index + 1
            };
        }
    }, { elements: [], index: 0 }).elements;
};

function TestCaseResultView(props: TestCaseResultViewProps) {
    return (
        props.result !== undefined
            ? <Stack tokens={StackGap10}>
                <Label style={{ fontWeight: 'bold', color: '#337ab7', alignSelf: 'center' }}>{props.result.Overview.FullName}</Label>
                <Label>Start Time: {props.result.StartTime}</Label>
                <Label>End Time: {props.result.EndTime}</Label>
                <Label>State: {props.result.Overview.State}</Label>
                {
                    props.result.Output !== null
                        ? <div style={{ border: '1px solid #d9d9d9', height: props.winSize.height - 300, overflowY: 'auto' }}>
                            <div style={{ paddingLeft: 5 }}>
                                {renderOutputLines(props.result.Output.split('\n').map((line) => line.trim()))}
                            </div>
                        </div>
                        : null
                }
            </Stack >
            : <div>Please select a test case result to view...</div>
    );
}

export function TestResultDetail(props: any) {
    const winSize = useWindowSize();

    const history = useHistory();

    const dispatch = useDispatch();

    const [dialogHidden, { toggle: toggleDialogHidden }] = useBoolean(true);

    const [filterPhrase, setFilterPhrase] = useState<string | undefined>(undefined);
    const [filteredResults, setFilteredResults] = useState<SelectionItem[] | undefined>(undefined);

    const [groupKeys, setGroupKeys] = useState(groupByTestCaseStateKeys);

    const [groupCollapsedStatuses, setGroupCollapsedStatuses] = useState(() => {
        let statuses: { [key: string]: boolean } = {};
        groupKeys.Keys.forEach((item) => {
            statuses[item.key] = false;
        })

        return statuses;
    });

    const [selectedItems, setSelectedItems] = useState<TestCaseOverview[] | undefined>(undefined);

    const testResults = useSelector((state: AppState) => state.testResults);
    const testCaseResult = useSelector((state: AppState) => state.testCaseResult);

    const testCaseResults = useMemo(() => testResults.selectedTestResult?.Results ?? [], [testResults.selectedTestResult]);

    const forceUpdate = useForceUpdate();

    useEffect(() => {
        if (testResults.selectedTestResultId !== undefined) {
            dispatch(TestResultsDataSrv.getTestResultDetail(testResults.selectedTestResultId));
        }
    }, [dispatch, testResults.selectedTestResultId]);

    useEffect(() => {
        const handler = () => {
            const selectedTestResult = testResults.selectedTestResult;
            const updateStatus = selectedTestResult !== undefined && (selectedTestResult?.Overview.Status === 'Created' || selectedTestResult?.Overview.Status === 'Running');
            if (testResults.selectedTestResultId !== undefined) {
                if (updateStatus) {
                    dispatch(TestResultsDataSrv.getTestResultDetail(testResults.selectedTestResultId));
                }
            }
        };
        const interval = setInterval(handler, 5000);

        return () => clearInterval(interval);
    }, [dispatch, testResults.selectedTestResultId, testResults.selectedTestResult])

    useEffect(() => {
        if (selectedItems !== undefined && selectedItems?.length === 1) {
            dispatch(TestCaseResultDataSrv.getTestCaseResult(selectedItems[0].FullName));
        }
    }, [dispatch, selectedItems]);

    const getUpdatedGroups = useCallback((resultList: TestCaseOverview[]) => {
        const updatedResult = groupKeys.Keys.reduce((result: { groups: IGroup[], startIndex: number }, currentKey) => {
            const caseCount = resultList.filter(item => groupKeys.KeyComparer(item, currentKey.key)).length;
            if (caseCount === 0) {
                return result;
            }
            else {
                return {
                    groups: [...result.groups, {
                        key: currentKey.key,
                        name: currentKey.name,
                        startIndex: result.startIndex,
                        count: caseCount,
                        isCollapsed: Object.getOwnPropertyDescriptor(groupCollapsedStatuses, currentKey.key)?.value
                    }],
                    startIndex: result.startIndex + caseCount
                }
            }
        }, { groups: [], startIndex: 0 });

        return updatedResult.groups;

    }, [groupKeys, groupCollapsedStatuses]);

    const getUpdatedGroupedItems = useCallback((resultList: TestCaseOverview[]) => {
        const updatedList = groupKeys.Keys.flatMap((key) => {
            return resultList.filter(item => groupKeys.KeyComparer(item, key.key))
                .sort((a, b) => a.FullName.localeCompare(b.FullName));
        });

        return updatedList;
    }, [groupKeys]);

    const updateFilteredResults = useCallback((filterPhrase: string | undefined) => {
        const groupedResults = isValidFilterPhrase(filterPhrase)
            ? getUpdatedGroupedItems(testCaseResults.filter(filterByNameFunc(filterPhrase)))
            : getUpdatedGroupedItems(testCaseResults);

        setFilteredResults(groupedResults);
    }, [testCaseResults, getUpdatedGroupedItems]);

    useEffect(() => {
        if (testResults.selectedTestResult !== undefined) {
            updateFilteredResults(filterPhrase);
        }
    }, [testResults.selectedTestResult, filterPhrase, updateFilteredResults]);

    const [selection, setSelection] = useState<Selection<IObjectWithKey>>(() => {
        const s = new Selection<SelectionItem>({
            onSelectionChanged: () => {
                setSelectedItems(s.getSelection());
                setSelection(s);
                forceUpdate();
            }
        });

        s.setItems(getSelectionItems(testCaseResults), true);

        return s;
    });

    const onFilterPhraseChanged = (filterPhrase: string | undefined) => {
        setFilterPhrase(filterPhrase);
        selection.setAllSelected(false);
    };

    const onFilterPhraseClear = () => {
        setFilterPhrase(undefined);
        selection.setAllSelected(false);
    };

    const onToggleGroupCollapse = (group: IGroup) => {
        groupCollapsedStatuses[group.key] = !group.isCollapsed;
        setGroupCollapsedStatuses({ ...groupCollapsedStatuses });
    };

    const onExportProfile = () => {

    };

    const onGoBackClick = () => {
        dispatch(TestCaseResultActions.clearSelectedTestCaseResultAction());
        dispatch(TestResultsActions.clearSelectedTestResultAction());
        dispatch(() => history.push('/Tasks/History', { from: 'TestResultDetail' }));
    };

    const onDialogAbortButtonClick = () => {
        if (testResults.selectedTestResultId !== undefined) {
            dispatch(SelectedTestCasesDataSrv.abortRunRequest(testResults.selectedTestResultId));
        }

        dispatch(() => toggleDialogHidden());
    };

    return (
        <div>
            <FullWidthPanel isLoading={testCaseResult.isLoading} errorMsg={testCaseResult.errorMsg} >
                <Stack horizontal tokens={{ childrenGap: 20 }}>
                    <div style={{ maxWidth: winSize.width * 0.30, height: winSize.height - 140 }}>
                        <SelectionZone selection={selection} selectionMode={SelectionMode.single}>
                            <Stack tokens={StackGap10}>
                                <SearchBox
                                    style={{ width: 280 }}
                                    placeholder='Filter by Name...'
                                    onChange={(_, newValue) => onFilterPhraseChanged(newValue)}
                                    onReset={onFilterPhraseClear}
                                />
                                <div style={{ width: winSize.width * 0.30, height: winSize.height - 180, overflowY: 'auto' }}>
                                    {
                                        filteredResults !== undefined
                                            ? <DetailsList
                                                items={filteredResults}
                                                columns={[
                                                    {
                                                        key: 'Name',
                                                        name: 'Name',
                                                        minWidth: 360,
                                                        isRowHeader: true,
                                                        isResizable: true,
                                                        onRender: (item: TestCaseOverview | undefined, index: number | undefined) => {
                                                            if (item != undefined && index != undefined) {
                                                                const startIndex = item.FullName.lastIndexOf('.') + 1;
                                                                const caseName = item.FullName.substring(startIndex);
                                                                return <div>{caseName}</div>
                                                            }
                                                            else {
                                                                return null;
                                                            }
                                                        }
                                                    }
                                                ]}
                                                layoutMode={DetailsListLayoutMode.justified}
                                                selection={selection}
                                                selectionMode={SelectionMode.single}
                                                selectionPreservedOnEmptyClick={true}
                                                setKey='set'
                                                getKey={(item: TestCaseOverview) => item.FullName}
                                                groups={getUpdatedGroups(filteredResults)}
                                                groupProps={{
                                                    headerProps: {
                                                        onToggleCollapse: onToggleGroupCollapse
                                                    }
                                                }}
                                                compact={true}
                                                isHeaderVisible={false}
                                            />
                                            : <div>
                                                <Spinner size={SpinnerSize.medium} />
                                                <p style={{ color: '#fa8c16' }}>Loading...</p>
                                            </div>
                                    }
                                </div>
                            </Stack>
                        </SelectionZone>
                    </div>
                    <div style={{ borderLeft: '2px solid #bae7ff', minHeight: winSize.height - 140 }}>
                        <div style={{ paddingLeft: 20, paddingRight: 15, maxWidth: winSize.width * 0.82 }}>
                            <TestCaseResultView
                                winSize={winSize}
                                result={testCaseResult.selectedTestCaseResult} />
                        </div>
                    </div>
                </Stack>
                <hr style={{ border: "1px solid #d9d9d9" }} />
                <div className='buttonPanel'>
                    <Stack horizontal horizontalAlign='end' tokens={StackGap10}>
                        {
                            testResults.selectedTestResult === undefined
                                ? null
                                : testResults.selectedTestResult.Overview.Status === 'Running'
                                    ? <PrimaryButton style={{ backgroundColor: '#ff4949' }} onClick={toggleDialogHidden}>Abort</PrimaryButton>
                                    : null
                        }
                        <PrimaryButton disabled onClick={onExportProfile}>Export Profile</PrimaryButton>
                        <PrimaryButton onClick={onGoBackClick}>Go Back</PrimaryButton>
                    </Stack>
                </div>
            </FullWidthPanel>
            <Dialog
                hidden={dialogHidden}
                onDismiss={toggleDialogHidden}
                dialogContentProps={{
                    type: DialogType.normal,
                    title: `Abort Test Run ${testResults.selectedTestResultId}?`
                }}
                modalProps={{
                    isBlocking: true
                }}>
                <div style={{ fontSize: 'large' }}>
                    <Stack horizontalAlign='start' tokens={StackGap5}>
                        <Stack horizontal><div style={{ paddingRight: 5 }}>TestSuite:</div>{testResults.selectedTestResultSummary?.TestSuite.Name + ' ' + testResults.selectedTestResultSummary?.TestSuite.Version}</Stack>
                        <Stack horizontal><div style={{ paddingRight: 5 }}>Configuration:</div>{testResults.selectedTestResultSummary?.Configuration}</Stack>
                        <Stack horizontal><div style={{ paddingRight: 5 }}>Status:</div><div>{testResults.selectedTestResult?.Overview.Status}</div></Stack>
                    </Stack>
                </div>
                <DialogFooter>
                    <PrimaryButton onClick={toggleDialogHidden} text="Cancel" />
                    <PrimaryButton style={{ backgroundColor: '#ff4949' }} onClick={onDialogAbortButtonClick} text="Abort" />
                </DialogFooter>
            </Dialog>
        </div>
    );
}