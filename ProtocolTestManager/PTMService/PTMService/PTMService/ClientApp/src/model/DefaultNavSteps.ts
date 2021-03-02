import { StepNavItemInfo } from "./StepNavItemInfo";

export const DefaultNavSteps: StepNavItemInfo[] = [
    {
        Caption: 'Select Test Suite',
        TargetStep: 1,
        IsEnabled: true, 
        IsActive: true
    },
    {
        Caption: 'Test Suite Introduction',
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