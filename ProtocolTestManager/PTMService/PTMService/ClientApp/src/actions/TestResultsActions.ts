// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ListResponse } from "../model/ListResponse";
import { TestResult, TestResultSummary } from "../model/TestResult";

// define action consts
export const LIST_TESTRESULTS_REQUEST = 'TESTRESULTS/LIST_TESTRESULTS_REQUEST';
export const LIST_TESTRESULTS_SUCCESS = 'TESTRESULTS/LIST_TESTRESULTS_SUCCESS';
export const LIST_TESTRESULTS_FAILURE = 'TESTRESULTS/LIST_TESTRESULTS_FAILURE';

export const SET_PAGENUMBER = 'TESTRESULTS/SET_PAGENUMBER';

export const SET_SELECTEDTESTRESULT = 'TESTRESULTS/SET_SELECTEDTESTRESULT';
export const CLEAR_SELECTEDTESTRESULT = 'TESTRESULTS/CLEAR_SELECTEDTESTRESULT';

export const GET_TESTRESULTDETAIL_REQUEST = 'TESTRESULTS/GET_TESTRESULTDETAIL_REQUEST';
export const GET_TESTRESULTDETAIL_SUCCESS = 'TESTRESULTS/GET_TESTRESULTDETAIL_SUCCESS';
export const GET_TESTRESULTDETAIL_FAILURE = 'TESTRESULTS/GET_TESTRESULTDETAIL_FAILURE';

// define action types
interface ListTestResultsActionRequestType { type: typeof LIST_TESTRESULTS_REQUEST; payload: number; }
interface ListTestResultsActionSuccessType { type: typeof LIST_TESTRESULTS_SUCCESS; payload: ListResponse; }
interface ListTestResultsActionFailureType { type: typeof LIST_TESTRESULTS_FAILURE; errorMsg: string; }

interface SetPageNumberActionType { type: typeof SET_PAGENUMBER; payload: number; }

interface SetSelectedTestResultActionType { type: typeof SET_SELECTEDTESTRESULT; testResultId: number; summary: TestResultSummary; }
interface ClearSelectedTestResultActionType { type: typeof CLEAR_SELECTEDTESTRESULT; }

interface GetTestResultDetailActionRequestType { type: typeof GET_TESTRESULTDETAIL_REQUEST; payload: number; }
interface GetTestResultDetailActionSuccessType { type: typeof GET_TESTRESULTDETAIL_SUCCESS; payload: TestResult; }
interface GetTestResultDetailActionFailureType { type: typeof GET_TESTRESULTDETAIL_FAILURE; errorMsg: string; }

export type TestResultsActionTypes =
    ListTestResultsActionRequestType |
    ListTestResultsActionSuccessType |
    ListTestResultsActionFailureType |
    SetPageNumberActionType |
    SetSelectedTestResultActionType |
    ClearSelectedTestResultActionType |
    GetTestResultDetailActionRequestType |
    GetTestResultDetailActionSuccessType |
    GetTestResultDetailActionFailureType;

// define actions
export const TestResultsActions = {
    listTestResultsAction_Request: (pageNumber: number): TestResultsActionTypes => {
        return {
            type: LIST_TESTRESULTS_REQUEST,
            payload: pageNumber
        };
    },
    listTestResultsAction_Success: (listResponse: ListResponse): TestResultsActionTypes => {
        return {
            type: LIST_TESTRESULTS_SUCCESS,
            payload: listResponse
        };
    },
    listTestResultsAction_Failure: (error: string): TestResultsActionTypes => {
        return {
            type: LIST_TESTRESULTS_FAILURE,
            errorMsg: error
        };
    },
    setPageNumberAction: (pageNumber: number): TestResultsActionTypes => {
        return {
            type: SET_PAGENUMBER,
            payload: pageNumber
        }
    },
    setSelectedTestResultAction: (testResultId: number, summary: TestResultSummary): TestResultsActionTypes => {
        return {
            type: SET_SELECTEDTESTRESULT,
            testResultId: testResultId,
            summary: summary
        };
    },
    clearSelectedTestResultAction: (): TestResultsActionTypes => {
        return {
            type: CLEAR_SELECTEDTESTRESULT,
        };
    },
    getTestResultDetailAction_Request: (testResultId: number): TestResultsActionTypes => {
        return {
            type: GET_TESTRESULTDETAIL_REQUEST,
            payload: testResultId,
        };
    },
    getTestResultDetailAction_Success: (testResult: TestResult): TestResultsActionTypes => {
        return {
            type: GET_TESTRESULTDETAIL_SUCCESS,
            payload: testResult
        };
    },
    getTestResultDetailAction_Failure: (error: string): TestResultsActionTypes => {
        return {
            type: GET_TESTRESULTDETAIL_FAILURE,
            errorMsg: error
        };
    }
};