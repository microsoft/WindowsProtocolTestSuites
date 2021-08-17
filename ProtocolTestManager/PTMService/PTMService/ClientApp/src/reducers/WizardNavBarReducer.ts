import {
  SET_SELECTED_STEP,
  WizardNavBarActionTypes
} from '../actions/WizardNavBarAction'

export interface WizardNavBarState {
  lastStep: number
}

const initialWizardNavBarState: WizardNavBarState = {
  lastStep: 1
}

export const getWizardNavBarReducer = (state = initialWizardNavBarState, action: WizardNavBarActionTypes): WizardNavBarState => {
  switch (action.type) {
    case SET_SELECTED_STEP:
      return {
        ...state,
        lastStep: action.activeStep
      }
    default:
      return state
  }
}
