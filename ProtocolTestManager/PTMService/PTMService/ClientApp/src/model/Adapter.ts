// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export interface Adapter {
    // Adapter name
    Name: string;
    // Customize Adapter implementation type, only 
    AdapterType: string;
    // Adapter Kind
    Kind: AdapterKind;
    // Managed adapter
    ScriptDirectory: string;
}

export enum AdapterKind {
    Managed = 'Managed',
    PowerShell = 'PowerShell',
    Shell = 'Shell',
    Interactive = 'Interactive',
}

export enum ChangedField {
    AdapterKind,
    AdapterType,
    ScriptDirectory
}

export interface AdapterChangedEvent {
    Field: ChangedField;
    NewValue: string | number | undefined;
    Adapter: string;
}