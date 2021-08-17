// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export enum DetectedResult {
    DetectFail = 'DetectFail',
    Supported = 'Supported',
    UnSupported = 'UnSupported'
}

export interface ResultItem {
    DetectedResult: DetectedResult;
    ImageUrl: string;
    Name: string;
}

export interface ResultItemMap {
    Header: string;
    Description: string;
    ResultItemList: ResultItem[];
}

export interface DetectionSummary {
    ResultItemMapList: ResultItemMap[];
}
