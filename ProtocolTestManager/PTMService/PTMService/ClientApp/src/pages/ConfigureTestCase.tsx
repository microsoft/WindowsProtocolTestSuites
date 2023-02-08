// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Link, PrimaryButton, Stack } from '@fluentui/react'
import { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard'
import { PropertyGroupsActions } from '../actions/PropertyGroupsAction'
import { InvalidAppStateNotification } from '../components/InvalidAppStateNotification'
import { PropertyGroupView } from '../components/PropertyGroupView'
import { StepPanel } from '../components/StepPanel'
import { useWindowSize } from '../components/UseWindowSize'
import { WizardNavBar } from '../components/WizardNavBar'
import { getNavSteps } from '../model/DefaultNavSteps'
import { Property } from '../model/Property'
import { PropertyGroupsDataSrv } from '../services/PropertyGroups'
import { AppState } from '../store/configureStore'

export function ConfigureTestCase (props: StepWizardProps) {
  const dispatch = useDispatch()
  const testSuiteInfo = useSelector((state: AppState) => state.testSuiteInfo)
  const configuration = useSelector((state: AppState) => state.configurations)
  const propertyGroups = useSelector((state: AppState) => state.propertyGroups)
  const configureMethod = useSelector((state: AppState) => state.configureMethod)
  const wizardProps: StepWizardChildProps = props as StepWizardChildProps
  const navSteps = getNavSteps(wizardProps, configureMethod)
  const wizard = WizardNavBar(wizardProps, navSteps)
  const winSize = useWindowSize()

  useEffect(() => {
    if (!propertyGroups.updated || propertyGroups.propertyGroups.length === 0) {
      dispatch(PropertyGroupsDataSrv.getPropertyGroups())
    }
  }, [dispatch])

  if (testSuiteInfo.selectedTestSuite === undefined || configuration.selectedConfiguration === undefined) {
    return <InvalidAppStateNotification
      testSuite={testSuiteInfo.selectedTestSuite}
      configuration={configuration.selectedConfiguration}
      wizard={wizard}
      wizardProps={wizardProps} />
  }

  const onPropertyValueChange = (updatedProperty: Property) => {
    dispatch(PropertyGroupsActions.updatedEditingPropertyGroupAction(updatedProperty))
  }

  const onEditingPropertyGroupChange = (index: number) => {
    dispatch(PropertyGroupsActions.updatePropertyGroupsAction())
    dispatch(PropertyGroupsActions.setEditingPropertyGroupAction(index))
  }

  const onPreviousButtonClick = () => {
    dispatch(PropertyGroupsActions.updatePropertyGroupsAction())
    wizardProps.previousStep()
  }

  const onNextButtonClick = () => {
    dispatch(PropertyGroupsActions.updatePropertyGroupsAction())
    dispatch(PropertyGroupsDataSrv.setPropertyGroups(() => {
      if (propertyGroups.errorMsg === undefined) {
        wizardProps.nextStep()
      }
    }))
  }

  return (
    <div>
      <StepPanel leftNav={wizard} isLoading={propertyGroups.isLoading} errorMsg={propertyGroups.errorMsg}>
        <Stack style={{ paddingLeft: 10 }}>
          <Stack horizontal style={{ paddingBottom: 20 }}>
            <div style={{ fontWeight: 'bold' }}>Groups</div>
            <div style={{ paddingLeft: winSize.width * 0.10, fontWeight: 'bold' }}>{propertyGroups.editingPropertyGroup?.Name} Properties</div>
            <div style={{ paddingLeft: winSize.width * 0.13, fontSize: 'small', fontWeight: 'normal', alignSelf: 'center' }}>*The value is modified compared to the latest run</div>
          </Stack>
          <Stack horizontal>
            <Stack style={{ paddingTop: 20, width: 220, height: winSize.height - 160, overflowY: 'auto' }} tokens={{ childrenGap: 20 }} >
              {
                propertyGroups.propertyGroups.map((propertyGroup, index) => {
                  return (
                    <div key={index} style={{ alignSelf: 'start' }}>
                      <Link style={{ fontSize: 'large', fontWeight: 'bold', color: propertyGroups.editingPropertyGroupIndex === index ? '#005A9E' : '#525251' }}
                        disabled={propertyGroups.editingPropertyGroupIndex === index}
                        onClick={() => onEditingPropertyGroupChange(index)}>
                        {propertyGroup.Name}
                      </Link>
                    </div>
                  )
                })
              }
            </Stack>
            <div style={{ paddingLeft: 30, width: winSize.width, height: winSize.height - 160, overflowY: 'auto' }}>
              {
                propertyGroups.editingPropertyGroup !== undefined
                  ? <PropertyGroupView
                    winSize={winSize}
                    latestPropertyGroup={propertyGroups.latestPropertyGroups[propertyGroups.editingPropertyGroupIndex]}
                    propertyGroup={propertyGroups.editingPropertyGroup}
                    onValueChange={onPropertyValueChange} />
                  : null
              }
            </div>
          </Stack>
          <div className='buttonPanel'>
            <Stack horizontal horizontalAlign="end" tokens={{ childrenGap: 10 }} >
              <PrimaryButton text="Previous" onClick={onPreviousButtonClick} disabled={propertyGroups.isPosting} />
              <PrimaryButton text="Next" onClick={onNextButtonClick} disabled={propertyGroups.isPosting} />
            </Stack>
          </div>
        </Stack>
      </StepPanel>
    </div >
  )
};
