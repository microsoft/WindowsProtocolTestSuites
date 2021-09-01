// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Property } from "./Property";

export interface Prerequisite {
  Title: string
  Summary: string
  Properties: Property[]
}

export type DetectionStepStatus = 'Pending' | 'Detecting' | 'Finished' | 'Skipped' | 'NotFound' | 'Failed' | 'Error' | 'Canceling' | 'Cancelled'

export interface DetectingItem {
  Name: string
  Status: DetectionStepStatus
}

export interface DetectionStepsResponse {
  DetectingContent: string
  DetectingStatus: DetectionStepStatus
}

export interface DetectionResult {
  Status: DetectionStatus
  Exception: string
}

export interface DetectionResultResponse {
  Result: DetectionResult
  DetectionSteps: DetectionStepsResponse[]
}

export interface DetectionSteps {
  DetectingItems: DetectingItem[]
  LogFileName: string
  Result: DetectionResult
}

export enum DetectionStatus {
  // Detection not start.
  NotStart = 'NotStart',

  // Detection in progress.
  InProgress = 'InProgress',

  // Detection finished.
  Finished = 'Finished',

  // Detection step skipped.
  Skipped = 'Skipped',

  // Detection step not found.
  NotFound = 'NotFound',

  // Error occurred running detection step.
  Error = 'Error'
}
