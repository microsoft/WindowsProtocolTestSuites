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
export interface DetectionSteps {
    DetectingItems: DetectingItem[];
    LogFileName: string;
}


//StartAutoDetection: api/testsuite/${configurationId}/autodetect/start
export interface StartDetectorRequest {
    //Prerequisites: Prerequisites;
}

//StopAutoDetection: api/testsuite/${configurationId}/autodetect/stop

//GetDetectionResult: api/testsuite/${configurationId}/autodetect/summary
export interface DetectionSummary {
    

}
