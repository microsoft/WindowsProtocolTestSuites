// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ComboBox, IComboBox, IComboBoxOption, Link, PrimaryButton, Stack, TextField } from '@fluentui/react';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard';
import { StackGap10, StackGap5 } from '../components/StackStyle';
import { StepPanel } from '../components/StepPanel';
import { useWindowSize } from '../components/UseWindowSize';
import { HeaderMenuHeight, WizardNavBar } from '../components/WizardNavBar';
import { AdapterChangedEvent, AdapterKind, ChangedField } from '../model/Adapter';
import { getNavSteps } from '../model/DefaultNavSteps';
import { AdapterDataSrv } from '../services/ConfigureAdapter';
import { AppState } from '../store/configureStore';
import '../css/configureAdapter.css';
import { AdapterActions } from '../actions/ConfigureAdapterAction';

export function ConfigureAdapter(props: StepWizardProps) {
    const dispatch = useDispatch();
    const configuration = useSelector((state: AppState) => state.configurations);
    const adapters = useSelector((state: AppState) => state.configureAdapter);
    const configureMethod = useSelector((state: AppState) => state.configureMethod);

    useEffect(() => {
        if (configuration.selectedConfiguration) {
            dispatch(AdapterDataSrv.getAdapters(configuration.selectedConfiguration!.Id!));
        }
    }, [dispatch, configuration.selectedConfiguration])

    const wizardProps: StepWizardChildProps = props as StepWizardChildProps;
    const navSteps = getNavSteps(wizardProps, configureMethod);
    const wizard = WizardNavBar(wizardProps, navSteps);
    const winSize = useWindowSize();

    if (configuration.selectedConfiguration === undefined) {
        return (
            <StepPanel leftNav={wizard} isLoading={false} >
                <div>No configuration selected, please go to <Link onClick={() => { wizardProps.firstStep() }}>Start page</Link></div>
            </StepPanel>
        )
    }

    const onAdapterChanged = (event: AdapterChangedEvent) => {
        dispatch(AdapterActions.onAdapterChanged(event));
    }

    const adapterList = adapters.adapterList.map((item, index) => {
        return <AdapterItem key={index} Title={item.Name} AdapterType={item.AdapterType} Kind={item.Kind} ScriptDirectory={item.ScriptDirectory} onChange={onAdapterChanged} />
    })

    const onNextStepClicked = () => {
        if (verifyAdapterSettings()) {
            AdapterActions.setErrorMessage();
            dispatch(AdapterDataSrv.setAdapters(configuration.selectedConfiguration!.Id!, adapters.adapterList, (data: any) => {
                wizardProps.nextStep();
            }));
        } else {
            dispatch(AdapterActions.setErrorMessage(`[${(new Date()).toLocaleTimeString()}]: Adapter\'s required field is not set`));
        }
    }

    const verifyAdapterSettings = (): boolean => {
        for (const item of adapters.adapterList) {
            if ((item.Kind === AdapterKind.Managed) && (!item.AdapterType)) {
                return false;
            } else if ((item.Kind === AdapterKind.Shell || item.Kind === AdapterKind.PowerShell) && (!item.ScriptDirectory)) {
                return false;
            }
        }

        return true;
    }

    return (
        <div>
            <StepPanel leftNav={wizard} isLoading={adapters.isLoading} errorMsg={adapters.errorMsg} >
                <div style={{ height: winSize.height - HeaderMenuHeight - 50, overflowY: 'scroll' }}>
                    {adapterList}
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
    Title: string;
    AdapterType: string;
    Kind: AdapterKind;
    ScriptDirectory: string;
    onChange: (event: AdapterChangedEvent) => void;
}

function AdapterItem(props: AdapterItemProp) {
    const INITIAL_OPTIONS: IComboBoxOption[] = [];
    const [adapterType, setAdapterType] = useState<string | number | undefined>(props.Kind ? props.Kind : "");
    const [paramAdatperType, setParamAdapterType] = useState<string>(props.AdapterType ? props.AdapterType : "");
    const [paramScriptDirectory, setParamScriptDirectory] = useState<string>(props.ScriptDirectory ? props.ScriptDirectory : "");
    const [errorMsg, setErrorMsg] = useState('');

    for (const [propertyKey, propertyValue] of Object.entries(AdapterKind)) {
        if (!Number.isNaN(Number(propertyKey))) {
            continue;
        }

        INITIAL_OPTIONS.push({ key: propertyValue, text: propertyKey });
    }

    const onChange = React.useCallback(
        (ev: React.FormEvent<IComboBox>, option?: IComboBoxOption): void => {
            setAdapterType(option?.key);
            checkRequiredField('' + option?.key, paramAdatperType, paramScriptDirectory);
            props.onChange({
                Field: ChangedField.AdapterKind,
                NewValue: option?.key,
                Adapter: props.Title
            });
        },
        [setAdapterType, props],
    );

    const onChangeParamAdatperType = React.useCallback(
        (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string) => {
            setParamAdapterType(newValue || '');

            checkRequiredField('' + adapterType, newValue!, paramScriptDirectory);

            props.onChange({
                Field: ChangedField.AdapterType,
                NewValue: newValue,
                Adapter: props.Title
            });
        },
        [props],
    );

    const onChangeParamScriptDirectory = React.useCallback(
        (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string) => {
            setParamScriptDirectory(newValue || '');

            checkRequiredField('' + adapterType, paramAdatperType!, newValue!);

            props.onChange({
                Field: ChangedField.ScriptDirectory,
                NewValue: newValue,
                Adapter: props.Title
            });
        },
        [props],
    );

    const checkRequiredField = (_adapterType: string, _paramAdapterType: string, _paramScriptDirectory: string) => {
        if ((_adapterType === AdapterKind.Managed) && (!_paramAdapterType)) {
            setErrorMsg('Adapter Type Can\'t be null');
        } else if ((_adapterType === AdapterKind.Shell || _adapterType === AdapterKind.PowerShell) && (!_paramScriptDirectory)) {
            setErrorMsg('Script Directory Can\'t be null');
        } else {
            setErrorMsg('');
        }
    }

    let description = undefined;
    let paramsDiv = undefined;
    switch (adapterType) {
        case AdapterKind.Interactive:
            description = <div>
                <div>Interactive Adapter pops up a dialog when one of the following method is called.</div>
                <div>You need to do the operation manually and enter the results in the dialog box.</div>
            </div>;
            paramsDiv = <div style={{ fontStyle: 'italic' }}>Interactive adapter has no configurations</div>;
            break;
        case AdapterKind.Managed:
            description = <div>
                <div>Managed Adapter uses managed code to implement the methods in the adapter.</div>
                <div>Usually, you do not need to change the configuration for managed adapter.</div>
            </div>;
            paramsDiv = <div>
                <Stack horizontalAlign="start" horizontal tokens={StackGap10}>
                    <div style={{ fontWeight: 'bold' }}>Adapter Type: </div>
                    <TextField value={paramAdatperType} className='input' onChange={onChangeParamAdatperType} errorMessage={errorMsg} />
                </Stack>
            </div>;
            break;
        case AdapterKind.PowerShell:
            description = <div>
                <div>PowerShell Adapter uses PowerShell scripts to implement the methods in the adapter.</div>
                <div>One PowerShell script file for each method.</div>
            </div>;
            paramsDiv = <div className='parameters'>
                <div>You need to configure the location of the scripts.</div>
                <Stack horizontalAlign="start" horizontal tokens={StackGap10}>
                    <div style={{ fontWeight: 'bold' }}>Script Directory: </div>
                    <TextField value={paramScriptDirectory} className='input' onChange={onChangeParamScriptDirectory} errorMessage={errorMsg} />
                </Stack>
            </div>;
            break;
        case AdapterKind.Shell:
            description = <div>
                <div>Shell Adapter uses shell script to implement the methods in the adapter.</div>
                <div>One .sh file for each method.</div>
            </div>;
            paramsDiv = <div className='parameters'>
                <div>You need to configure the location of the scripts.</div>
                <Stack horizontalAlign="start" horizontal tokens={StackGap10}>
                    <div style={{ fontWeight: 'bold' }}>Script Directory: </div>
                    <TextField value={paramScriptDirectory} className='input' onChange={onChangeParamScriptDirectory} errorMessage={errorMsg} />
                </Stack>
            </div>;
            break;
        default:
            console.log('Not support yet')
            break;
    };

    useEffect(() => {
        checkRequiredField('' + adapterType, paramAdatperType, paramScriptDirectory);
    }, []);

    return (<div className="adapterItem">
        <Stack tokens={StackGap5}>
            <div className='title'>
                {props.Title}
            </div>
            <Stack horizontalAlign="start" horizontal>
                <div>Type:</div>
                <ComboBox className='input'
                    allowFreeform={false}
                    autoComplete={'on'}
                    options={INITIAL_OPTIONS}
                    selectedKey={adapterType}
                    onChange={onChange}
                />
            </Stack>
            <hr />
            {description}
            {paramsDiv}
        </Stack>
    </div>);
}