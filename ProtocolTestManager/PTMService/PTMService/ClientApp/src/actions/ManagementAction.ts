// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TestSuite } from "../model/TestSuite";

// define action consts
export const GET_TESTSUITES_REQUEST = 'MANAGEMENT/GET_TESTSUITES_REQUEST';
export const GET_TESTSUITES_SUCCESS = 'MANAGEMENT/GET_TESTSUITES_SUCCESS';
export const GET_TESTSUITES_FAILURE = 'MANAGEMENT/GET_TESTSUITES_FAILURE';

export const INSTALL_TESTSUITE_REQUEST = 'MANAGEMENT/INSTALL_TESTSUITE_REQUEST';
export const INSTALL_TESTSUITE_SUCCESS = 'MANAGEMENT/INSTALL_TESTSUITE_SUCCESS';
export const INSTALL_TESTSUITE_FAILURE = 'MANAGEMENT/INSTALL_TESTSUITE_FAILURE';

export const SET_SEARCHTEXT = 'MANAGEMENT/SET_SEARCHTEXT'

// define action types
interface GetTestSuitesActionRequestType { type: typeof GET_TESTSUITES_REQUEST; }
interface GetTestSuitesActionSuccessType { type: typeof GET_TESTSUITES_SUCCESS; payload: TestSuite[]; }
interface GetTestSuitesActionFailureType { type: typeof GET_TESTSUITES_FAILURE; errorMsg: string; }

interface InstallTestSuiteActionRequestType { type: typeof INSTALL_TESTSUITE_REQUEST; }
interface InstallTestSuiteActionSuccessType { type: typeof INSTALL_TESTSUITE_SUCCESS; payload: number; }
interface InstallTestSuiteActionFailureType { type: typeof INSTALL_TESTSUITE_FAILURE; errorMsg: string; }

interface SetSearchTextActionType { type: typeof SET_SEARCHTEXT; searchText: string }

export type ManagementActionTypes = GetTestSuitesActionRequestType
    | GetTestSuitesActionSuccessType
    | GetTestSuitesActionFailureType
    | InstallTestSuiteActionRequestType
    | InstallTestSuiteActionSuccessType
    | InstallTestSuiteActionFailureType
    | SetSearchTextActionType;

// define actions
export const ManagementActions = {
    getTestSuitesAction_Request: (): ManagementActionTypes => {
        return {
            type: GET_TESTSUITES_REQUEST
        }
    },
    getTestSuitesAction_Success: (testSuites: TestSuite[]): ManagementActionTypes => {
        return {
            type: GET_TESTSUITES_SUCCESS,
            payload: testSuites
        }
    },
    getTestSuitesAction_Failure: (error: string): ManagementActionTypes => {
        return {
            type: GET_TESTSUITES_FAILURE,
            errorMsg: error
        }
    },
    installTestSuite_Request: (): ManagementActionTypes => {
        return {
            type: INSTALL_TESTSUITE_REQUEST
        }
    },
    installTestSuite_Success: (id: number): ManagementActionTypes => {
        return {
            type: INSTALL_TESTSUITE_SUCCESS,
            payload: id
        }
    },
    installTestSuite_Failure: (error: string): ManagementActionTypes => {
        return {
            type: INSTALL_TESTSUITE_FAILURE,
            errorMsg: error
        }
    },
    setSearchTextAction: (filter: string): ManagementActionTypes => {
        return {
            type: SET_SEARCHTEXT,
            searchText: filter
        }
    },
}