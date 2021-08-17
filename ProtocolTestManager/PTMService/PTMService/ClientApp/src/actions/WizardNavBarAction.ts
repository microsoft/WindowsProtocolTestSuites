// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// define action consts
export const SET_SELECTED_STEP = 'WIZARDNAVBAR/SET_SELECTED_STEP'

// define action types
interface SetWizardNavBarActionType { type: typeof SET_SELECTED_STEP, activeStep: number }
export type WizardNavBarActionTypes = SetWizardNavBarActionType

// define actions
export const WizardNavBarActions = {
  setWizardNavBarAction: (step: number): WizardNavBarActionTypes => {
    return {
      type: SET_SELECTED_STEP,
      activeStep: step
    }
  }
}
