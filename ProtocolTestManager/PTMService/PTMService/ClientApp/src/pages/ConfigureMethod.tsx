// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { PrimaryButton, Stack, Dialog, DialogFooter, DefaultButton, DialogType, ContextualMenu } from '@fluentui/react'
import { useBoolean } from '@uifabric/react-hooks'
import { CSSProperties, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard'
import { ConfigureMethodActions } from '../actions/ConfigureMethodAction'
import { WizardNavBarActions } from '../actions/WizardNavBarAction'
import { StackGap10 } from '../components/StackStyle'
import { StepPanel } from '../components/StepPanel'
import { WizardNavBar } from '../components/WizardNavBar'
import { FileUploader, IFile } from '../components/FileUploader'
import { getNavSteps, RunSteps } from '../model/DefaultNavSteps'
import { AppState } from '../store/configureStore'
import { ProfileDataSrv } from '../services/ProfileService'
import { PropertyGroupsActions } from '../actions/PropertyGroupsAction'

export const ConfigurationMethod_AutoDetection = 'AutoDetection'
export const ConfigurationMethod_Manual = 'Manual'
export const ConfigurationMethod_Profile = 'Profile'

export function ConfigureMethod(props: StepWizardProps) {
  const dispatch = useDispatch()

  const [hideDialog, { toggle: toggleHideDialog }] = useBoolean(true)
  const [hideAutoDetectionWarningDialog, { toggle: toggleAutoDetectionWarningDialog }] = useBoolean(true)
  const [importingErrMsg, setImportingErrMsg] = useState('')
  const wizardProps: StepWizardChildProps = props as StepWizardChildProps
  const navSteps = getNavSteps(wizardProps)
  const [file, setFile] = useState<IFile>()

  const items: MethodItemProp[] = [{
    Title: 'Run Auto-Detection',
    Key: ConfigurationMethod_AutoDetection,
    Description: 'Run Auto-Detection to retrieve capabilities of System Under Test(SUT) which are used to configure the test suite and select test cases automatically.'
  }, {
    Title: 'Do Manual Configuration',
    Key: ConfigurationMethod_Manual,
    Description: 'Don\'t run Auto-Detection. Configure the test suite and select test cases manually'
  }, {
    Title: 'Load Profile',
    Key: ConfigurationMethod_Profile,
    Description: 'Protocol Test Manager Profile contains information about configuration of test suite and selected test cases.\r\n You could load an existing profile to get the saved configuration'
  }]

  const onItemClicked = (key: string) => {
    switch (key) {
      case ConfigurationMethod_AutoDetection:
        toggleAutoDetectionWarningDialog()
        return
      case ConfigurationMethod_Manual:
        dispatch(WizardNavBarActions.setWizardNavBarAction(wizardProps.currentStep))
        dispatch(ConfigureMethodActions.setConfigurationMethodAction(key))
        wizardProps.goToStep(RunSteps.FILTER_TEST_CASE)
        return
      case ConfigurationMethod_Profile:
        dispatch(PropertyGroupsActions.setUpdatedAction(false))
        dispatch(ConfigureMethodActions.setConfigurationMethodAction(key))
        toggleHideDialog()
    }
  }

  const configureMethod = useSelector((state: AppState) => state.configureMethod)
  const configurations = useSelector((state: AppState) => state.configurations)
  const testSuites = useSelector((state: AppState) => state.testSuiteInfo)

  const dialogContentProps = {
    type: DialogType.normal,
    title: 'Load Profile'
  }

  const autoDetectionDialogContentProps = {
    type: DialogType.normal,
    title: 'Warning: '
  }

  const modalProps = {
    isBlocking: true,
    styles: { main: { innerWidth: 600, maxWidth: 650 } },
    dragOptions: {
      moveMenuItemText: 'Move',
      closeMenuItemText: 'Close',
      menu: ContextualMenu
    }
  }

  const autoDetectionWarningModalProps = {
    isBlocking: true,
    dragOptions: {
      moveMenuItemText: 'Move',
      closeMenuItemText: 'Close',
      menu: ContextualMenu
    }
  }

  const onFileUploadSuccess = (files: IFile[]): void => {
    if (files.length > 0) {
      setFile(files[0])
    }
  }

  const onRunAutoDetection = () => {
    dispatch(WizardNavBarActions.setWizardNavBarAction(wizardProps.currentStep))
    dispatch(ConfigureMethodActions.setConfigurationMethodAction(ConfigurationMethod_AutoDetection))
    wizardProps.goToStep(RunSteps.AUTO_DETECTION)
  }

  const onConfigurationCreate = () => {
    if (file != null) {
      setImportingErrMsg('')
      const testSuiteId = testSuites.selectedTestSuite?.Id
      const configId = configurations.selectedConfiguration?.Id
      dispatch(ProfileDataSrv.importProfile({
        Package: file.File,
        TestSuiteId: testSuiteId ?? 0,
        ConfigurationId: configId ?? 0
      }, (data: boolean) => {
        if (data === undefined && configureMethod.errorMsg === undefined) {
          setImportingErrMsg('Profile did not work. Try again.')
        }

        if (configureMethod.errorMsg) {
          setImportingErrMsg(configureMethod.errorMsg)
        } else if (data) {
          setImportingErrMsg('')
          toggleHideDialog()
          wizardProps.goToStep(RunSteps.RUN_SELECTED_TEST_CASE)
        }
      }))
    } else {
      setImportingErrMsg('Profile file cannot be empty')
    }
  }

  return (
    <div>
      <StepPanel leftNav={WizardNavBar(wizardProps, navSteps)} >
        <Stack style={{ padding: 10 }}>
          {
            items.map((item, index) => {
              return <div key={index} style={{ paddingBottom: 50 }}>
                <MethodItem
                  Title={item.Title}
                  Key={item.Key}
                  Description={item.Description}
                  Disabled={item.Disabled}
                  onClick={() => { onItemClicked(item.Key) }}
                ></MethodItem>
              </div>
            })
          }
          <Stack horizontal horizontalAlign="end" tokens={StackGap10} >
            <PrimaryButton text="Previous" onClick={() => wizardProps.previousStep()} />
          </Stack>
        </Stack>
      </StepPanel>

      <Dialog
        hidden={hideDialog}
        onDismiss={toggleHideDialog}
        dialogContentProps={dialogContentProps}
        modalProps={modalProps}
      >
        <Stack tokens={StackGap10}>
          <p style={{ color: 'red', padding: 3 }}>{importingErrMsg}</p>
          <FileUploader
            label="Package"
            onSuccess={onFileUploadSuccess}
            maxFileCount={1}
            suffix={['.ptm']}
            placeholder="Select .ptm file"
          />
        </Stack>
        <DialogFooter>
          <PrimaryButton onClick={onConfigurationCreate} text={configureMethod.isProfileUploading ? 'Uploading...' : 'Load Profile'} disabled={configureMethod.isProfileUploading} />
          <DefaultButton onClick={toggleHideDialog} text="Close" disabled={configureMethod.isProfileUploading} />
        </DialogFooter>
      </Dialog>

      <Dialog
        hidden={hideAutoDetectionWarningDialog}
        onDismiss={toggleAutoDetectionWarningDialog}
        dialogContentProps={autoDetectionDialogContentProps}
        modalProps={autoDetectionWarningModalProps}
      >
        <Stack>
          <div>Run Auto-Detection may have impact to SUT's states (PTM may create directories or files, may establish connections with SUT and send packets, etc).</div>
          <div>Do you want to continue?</div>
        </Stack>
        <DialogFooter>
          <PrimaryButton onClick={onRunAutoDetection} text={'Yes'} />
          <DefaultButton onClick={toggleAutoDetectionWarningDialog} text="No" />
        </DialogFooter>
      </Dialog>
    </div>
  )
};

interface MethodItemProp {
  Title: string
  Key: string
  Description: string
  Disabled?: boolean
  onClick?: () => void
}

function MethodItem(props: MethodItemProp) {
  const divStyle: CSSProperties | undefined = props.Disabled ? { color: 'grey' } : { cursor: 'pointer' }
  const divOnClicked = props.Disabled ? undefined : props.onClick

  return (<div className="card" style={divStyle}>
    <Stack className="container" tokens={StackGap10} onClick={divOnClicked}>
      <div>
        <div className="subject">{props.Title}</div>
        <div className="description">{props.Description}</div>
      </div>
    </Stack>
  </div>)
}
