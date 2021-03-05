import { StepWizardChildProps } from "react-step-wizard";
import { StepNavItemInfo } from "./StepNavItemInfo";

const DefaultNavSteps: StepNavItemInfo[] = [
    {
        Caption: 'Select Test Suite',
        TargetStep: 1,
        IsEnabled: true,
        IsActive: true
    },
    {
        Caption: 'Select Configuration',
        TargetStep: 2,
        IsEnabled: false,
    },
    {
        Caption: 'Configure Method',
        TargetStep: 3,
        IsEnabled: false,
    },
    {
        Caption: 'Auto-Detection',
        TargetStep: 4,
        IsEnabled: false,
    },
    {
        Caption: 'Detection Result',
        TargetStep: 5,
        IsEnabled: false,
    },
    {
        Caption: 'Filter Test Case',
        TargetStep: 6,
        IsEnabled: false,
    },
    {
        Caption: 'Configure Test Case',
        TargetStep: 7,
        IsEnabled: false,
    },
    {
        Caption: 'Configure Adapter',
        TargetStep: 8,
        IsEnabled: false,
    },
    {
        Caption: 'Run Selected Test Case',
        TargetStep: 9,
        IsEnabled: false,
    },
];

export function getNavSteps(wizardProps: StepWizardChildProps) {
    return DefaultNavSteps.map(item => {
        if (item.TargetStep <= wizardProps.currentStep) {
            return {
                ...item,
                IsEnabled: true
            }
        } else if (item.TargetStep === wizardProps.currentStep) {
            return {
                ...item,
                IsEnabled: true,
                IsActive: true
            }
        } else {
            return {
                ...item,
                IsActive: false,
                IsEnabled: false
            }
        }
    });
}