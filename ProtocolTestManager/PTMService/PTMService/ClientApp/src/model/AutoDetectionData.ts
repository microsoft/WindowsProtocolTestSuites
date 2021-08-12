// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export interface PrerequisitProperty {
    Name: string;
    Value: string;
    Choices: string[];
}

//GetPrerequisitesRequest: api/testsuite/${configurationId}/autodetect/prerequisites
//GetPrerequisitesResponse:
export interface Prerequisite {
    Title: string;
    Summary: string;
    Properties: PrerequisitProperty[];
}


export interface DetectingItem {
    Name: string;
    Status: string;
}

//GetDetectionStepsRequest:api/testsuite/${configurationId}/autodetect/detectionsteps
//GetDetectionStepsResponse:
export interface DetectionStepsResponse {
    DetectingContent: string;
    DetectingStatus: string;
}

export interface DetectionResult {
    Status: DetectionStatus;
    Exception: string;
}

export interface DetectionResultResponse{
    Result: DetectionResult;
    DetectionSteps: DetectionStepsResponse[];
}

export interface DetectionSteps {
    DetectingItems: DetectingItem[];
    LogFileName: string;
    Result: DetectionResult;
}


//StartAutoDetection: api/testsuite/${configurationId}/autodetect/start
export interface StartDetectorRequest {
    //Prerequisites: Prerequisites;
}

//StopAutoDetection: api/testsuite/${configurationId}/autodetect/stop

//GetDetectionResult: api/testsuite/${configurationId}/autodetect/summary
export interface DetectionSummary {


}

export enum DetectionStatus {
    /// <summary>
    /// Detection not start.
    /// </summary>
    NotStart = "NotStart",

    /// <summary>
    /// Detection in progress.
    /// </summary>
    InProgress = "InProgress",

    /// <summary>
    /// Detection finished.
    /// </summary>
    Finished = "Finished",

    /// <summary>
    /// Detection step skipped.
    /// </summary>
    Skipped = "Skipped",

    /// <summary>
    /// Detection step not found.
    /// </summary>
    NotFound = "NotFound",

    /// <summary>
    /// Error occured running detection step
    /// </summary>
    Error = "Error"
}