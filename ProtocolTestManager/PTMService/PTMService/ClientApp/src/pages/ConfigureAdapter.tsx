// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ComboBox, IComboBox, IComboBoxOption, Link, PrimaryButton, Stack, TextField } from '@fluentui/react'
import React, { useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard'
import { StackGap10, StackGap5 } from '../components/StackStyle'
import { StepPanel } from '../components/StepPanel'
import { useWindowSize } from '../components/UseWindowSize'
import { HeaderMenuHeight, WizardNavBar } from '../components/WizardNavBar'
import { AdapterChangedEvent, AdapterKind, ChangedField } from '../model/Adapter'
import { getNavSteps } from '../model/DefaultNavSteps'
import { AdapterDataSrv } from '../services/ConfigureAdapter'
import { AppState } from '../store/configureStore'
import '../css/configureAdapter.css'
import { AdapterActions } from '../actions/ConfigureAdapterAction'
import { InvalidAppStateNotification } from '../components/InvalidAppStateNotification'

export function ConfigureAdapter (props: StepWizardProps) {
  const dispatch = useDispatch()
  const testSuiteInfo = useSelector((state: AppState) => state.testSuiteInfo)
  const configuration = useSelector((state: AppState) => state.configurations)
  const adapters = useSelector((state: AppState) => state.configureAdapter)
  const configureMethod = useSelector((state: AppState) => state.configureMethod)

  useEffect(() => {
    if (configuration.selectedConfiguration != null) {
      dispatch(AdapterDataSrv.getAdapters(configuration.selectedConfiguration.Id!))
    }
  }, [dispatch, configuration.selectedConfiguration])

  const wizardProps: StepWizardChildProps = props as StepWizardChildProps
  const navSteps = getNavSteps(wizardProps, configureMethod)
  const wizard = WizardNavBar(wizardProps, navSteps)
  const winSize = useWindowSize()

  if (testSuiteInfo.selectedTestSuite === undefined || configuration.selectedConfiguration === undefined) {
    return <InvalidAppStateNotification
        testSuite={testSuiteInfo.selectedTestSuite}
        configuration={configuration.selectedConfiguration}
        wizard={wizard}
        wizardProps={wizardProps} />
  }

  const onAdapterChanged = (event: AdapterChangedEvent) => {
    dispatch(AdapterActions.onAdapterChanged(event))
  }

  const adapterList = adapters.adapterList.map((item, index) => {
    return <AdapterItem
            key={index}
            Name={item.Name}
            DisplayName={item.DisplayName}
            AdapterType={item.AdapterType}
            Kind={item.Kind}
            ScriptDirectory={item.ScriptDirectory}
            SupportedKinds={item.SupportedKinds}
            ShellScriptDirectory={item.ShellScriptDirectory}
            onChange={onAdapterChanged} />
  })

  const onNextStepClicked = () => {
    if (verifyAdapterSettings()) {
      AdapterActions.setErrorMessage()
      dispatch(AdapterDataSrv.setAdapters(configuration.selectedConfiguration!.Id!, adapters.adapterList, (data: any) => {
        wizardProps.nextStep()
      }))
    } else {
      dispatch(AdapterActions.setErrorMessage(`[${(new Date()).toLocaleTimeString()}]: Adapter\'s required field is not set`))
    }
  }

  const verifyAdapterSettings = (): boolean => {
    for (const item of adapters.adapterList) {
      if ((item.Kind === AdapterKind.Managed) && (!item.AdapterType)) {
        return false
      } else if ((item.Kind === AdapterKind.Shell && (!item.ShellScriptDirectory)) || (item.Kind === AdapterKind.PowerShell && (!item.ScriptDirectory))) {
        return false
      }
    }

    return true
  }

  return (
        <div>
            <StepPanel leftNav={wizard} isLoading={adapters.isLoading} errorMsg={adapters.errorMsg} >
                <div style={{ height: winSize.height - HeaderMenuHeight - 45, overflowY: 'scroll' }}>
                    {adapterList.length > 0 ? adapterList : <h1>There is no Adapter for current test suite, just click Next button to continue.</h1>}
                </div>
                <div className='buttonPanel'>
                    <Stack horizontal horizontalAlign='end' tokens={StackGap10}>
                        <PrimaryButton text="Previous" onClick={() => wizardProps.previousStep()} disabled={adapters.isPosting} />
                        <PrimaryButton text="Next" onClick={onNextStepClicked} disabled={adapters.isPosting} />
                    </Stack>
                </div>
            </StepPanel>
        </div>
  )
};

interface AdapterItemProp {
  Name: string
  DisplayName: string
  AdapterType: string
  Kind: AdapterKind
  ScriptDirectory: string
  SupportedKinds: AdapterKind[]
  ShellScriptDirectory: string
  onChange: (event: AdapterChangedEvent) => void
}

function AdapterItem (props: AdapterItemProp) {
  const INITIAL_OPTIONS: IComboBoxOption[] = []
  const [adapterKind, setAdapterKind] = useState<AdapterKind>(props.Kind)
  const [paramAdapterType, setParamAdapterType] = useState<string>(props.AdapterType ? props.AdapterType : '')
  const [paramScriptDirectory, setParamScriptDirectory] = useState<string>(props.ScriptDirectory ? props.ScriptDirectory : '')
  const [paramShellScriptDirectory, setParamShellScriptDirectory] = useState<string>(props.ShellScriptDirectory ? props.ShellScriptDirectory : '')
  const [errorMsg, setErrorMsg] = useState('')

  // Add all supported kinds of plugin to INITIAL_OPTIONS
  if (props.SupportedKinds.length > 0) {
    for (const key in props.SupportedKinds) {
      const value = props.SupportedKinds[key]
      INITIAL_OPTIONS.push({ key: value, text: value })
    }
    // The PTFCONFIG file's adapterKind is not in INITIAL_OPTIONS which are the adapter kinds supported by plugin, so we use INITIAL_OPTIONS[0] as default selected adapter kind.
    if (INITIAL_OPTIONS.filter((x) => x.key === ('' + adapterKind)).length === 0) {
      if (INITIAL_OPTIONS.length > 0) {
        setAdapterKind(AdapterKind[INITIAL_OPTIONS[0].key as keyof typeof AdapterKind])
      }
    }
  } else {
    // Plugin doesn't show any adapter kind, so we list all adapter types for current AdapterItem.
    for (const [propertyKey, propertyValue] of Object.entries(AdapterKind)) {
      if (!Number.isNaN(Number(propertyKey))) {
        continue
      }

      INITIAL_OPTIONS.push({ key: propertyValue, text: propertyKey })
    }
  }

  const onChange = React.useCallback(
    (ev: React.FormEvent<IComboBox>, option?: IComboBoxOption): void => {
      const kind = AdapterKind[option?.key as keyof typeof AdapterKind]
      setAdapterKind(kind)
      checkRequiredField(kind, paramAdapterType, paramScriptDirectory, paramShellScriptDirectory)
      props.onChange({
        Field: ChangedField.AdapterKind,
        NewValue: option?.key,
        Adapter: props.Name
      })
    },
    [setAdapterKind, props]
  )

  const onChangeParamManagedAdapterType = React.useCallback(
    (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string) => {
      setParamAdapterType(newValue || '')

      checkRequiredField(adapterKind, newValue!, paramScriptDirectory, paramShellScriptDirectory)

      props.onChange({
        Field: ChangedField.AdapterType,
        NewValue: newValue,
        Adapter: props.Name
      })
    },
    [props]
  )

  const onChangeParamScriptDirectory = React.useCallback(
    (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string) => {
      setParamScriptDirectory(newValue || '')

      checkRequiredField(adapterKind, paramAdapterType, newValue!, '')

      props.onChange({
        Field: ChangedField.ScriptDirectory,
        NewValue: newValue,
        Adapter: props.Name
      })
    },
    [props]
  )

  const onChangeParamShellScriptDirectory = React.useCallback(
    (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string) => {
      setParamShellScriptDirectory(newValue || '')

      checkRequiredField(adapterKind, paramAdapterType, '', newValue!)

      props.onChange({
        Field: ChangedField.ShellScriptDirectory,
        NewValue: newValue,
        Adapter: props.Name
      })
    },
    [props]
  )

  const checkRequiredField = (_adapterKind: AdapterKind, _paramAdapterType: string, _paramScriptDirectory: string, _paramShellScriptDirectory: string) => {
    if ((_adapterKind === AdapterKind.Managed) && (!_paramAdapterType)) {
      setErrorMsg('Adapter Type Can\'t be null')
    } else if ((_adapterKind === AdapterKind.PowerShell && !_paramScriptDirectory) || (_adapterKind === AdapterKind.Shell && !_paramShellScriptDirectory)) {
      setErrorMsg('Script Directory Can\'t be null')
    } else {
      setErrorMsg('')
    }
  }

  let description
  let paramsDiv
  switch (adapterKind) {
    case AdapterKind.Interactive:
      description = <div>
                <div>Interactive Adapter pops up a dialog when one of the following method is called.</div>
                <div>You need to do the operation manually and enter the results in the dialog box.</div>
            </div>
      paramsDiv = <div style={{ fontStyle: 'italic' }}>Interactive adapter has no configurations</div>
      break
    case AdapterKind.Managed:
      description = <div>
                <div>Managed Adapter uses managed code to implement the methods in the adapter.</div>
                <div>Usually, you do not need to change the configuration for managed adapter.</div>
            </div>
      paramsDiv = <div>
                <Stack horizontalAlign="start" horizontal tokens={StackGap10}>
                    <div style={{ fontWeight: 'bold' }}>Adapter Type: </div>
                    <TextField ariaLabel='Managed adapter type' value={paramAdapterType} className='input' onChange={onChangeParamManagedAdapterType} errorMessage={errorMsg} />
                </Stack>
            </div>
      break
    case AdapterKind.PowerShell:
      description = <div>
                <div>PowerShell Adapter uses PowerShell scripts to implement the methods in the adapter.</div>
                <div>One PowerShell script file for each method.</div>
            </div>
      paramsDiv = <div className='parameters'>
                <div>You need to configure the location of the scripts.</div>
                <Stack horizontalAlign="start" horizontal tokens={StackGap10}>
                    <div style={{ fontWeight: 'bold' }}>Script Directory: </div>
                    <TextField ariaLabel='PowerShell adapter script directory' value={paramScriptDirectory} className='input' onChange={onChangeParamScriptDirectory} errorMessage={errorMsg} />
                </Stack>
            </div>
      break
    case AdapterKind.Shell:
      description = <div>
                <div>Shell Adapter uses shell script to implement the methods in the adapter.</div>
                <div>One .sh file for each method.</div>
            </div>
      paramsDiv = <div className='parameters'>
                <div>You need to configure the location of the scripts.</div>
                <Stack horizontalAlign="start" horizontal tokens={StackGap10}>
                    <div style={{ fontWeight: 'bold' }}>Script Directory: </div>
                    <TextField ariaLabel='Shell adapter script directory' value={paramShellScriptDirectory} className='input' onChange={onChangeParamShellScriptDirectory} errorMessage={errorMsg} />
                </Stack>
            </div>
      break
    default:
      console.log('Not support yet')
      break
  };

  useEffect(() => {
    checkRequiredField(adapterKind, paramAdapterType, paramScriptDirectory, paramShellScriptDirectory)
  }, [])

  return (<div className="adapterItem">
        <Stack tokens={StackGap5}>
            <div className='title'>
                {props.DisplayName}
            </div>
            <Stack horizontalAlign="start" horizontal>
                <div>Type:</div>
                <ComboBox className='input'
                    allowFreeform={false}
                    autoComplete={'on'}
                    ariaLabel='Select an adapter type'
                    options={INITIAL_OPTIONS}
                    selectedKey={adapterKind}
                    onChange={onChange}
                />
            </Stack>
            <hr />
            {description}
            {paramsDiv}
        </Stack>
    </div>)
}
