// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export interface Adapter {
  // Adapter name
  Name: string
  // Adapter displayname
  DisplayName: string
  // Customize Adapter implementation type, only
  AdapterType: string
  // Supported Kinds
  SupportedKinds: AdapterKind[]
  // Adapter Kind
  Kind: AdapterKind
  // PowerShell adapter Script Directory
  ScriptDirectory: string
  // Shell adapter Script Directory
  ShellScriptDirectory: string
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
  ScriptDirectory,
  ShellScriptDirectory
}

export interface AdapterChangedEvent {
  Field: ChangedField
  NewValue: string | number | undefined
  Adapter: string
}
