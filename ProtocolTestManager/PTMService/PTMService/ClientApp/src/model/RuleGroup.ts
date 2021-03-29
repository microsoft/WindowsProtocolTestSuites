// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export interface RuleGroup {
    DisplayName?: string;

    Name: string;

    Rules:Rule[]
}
export interface Rule {
    DisplayName?: string;

    Name: string;

    Rules?: Rule[];    
    Categories?: string[]
}
export interface SelectedRuleGroup {
    Name: string;

    Selected: string[]
}

export interface GroupItem {
    key: string;

    name: string;
    
    rules:Rule[];

    checked: string[];

    action:(data: any) => void;
}