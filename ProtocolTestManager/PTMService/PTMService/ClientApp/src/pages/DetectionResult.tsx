// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { StepWizardChildProps, StepWizardProps } from 'react-step-wizard';
import { StepPanel } from '../components/StepPanel';
import { WizardNavBar } from '../components/WizardNavBar';
import { getNavSteps, RunSteps } from '../model/DefaultNavSteps';
import * as ConfigureMethod from './ConfigureMethod';
import { DetectedResult, DetectionSummary, ResultItem, ResultItemMap } from '../model/DetectionResult';
import { StackGap10 } from '../components/StackStyle';
import { PrimaryButton, Stack, Nav, INavLink, INavLinkGroup, IIconProps, IRenderGroupHeaderProps } from '@fluentui/react';
import { AppState } from '../store/configureStore';
import { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { DetectionResultSrv } from '../services/DetectionResult';

const getDetectionSummary = (detectionSummary: DetectionSummary) => {
  const currentDate = new Date();
  let pageHTML = '<head><style>table,td {border: 1px solid #333;background-color:#b6d5f8;}thead,tfoot {background-color: #4a97ef;color: #fff;}</style></head><h1>Auto Detection Result Report-' + currentDate + '</h1><table><thead><tr><th>Detection Category</th><th>Detection Item</th><th>Detection Result</th></tr></thead><tbody>';
  detectionSummary.ResultItemMapList.forEach((resultItemMap) => {
    pageHTML += '<tr><td width="300px" rowspan="' + resultItemMap.ResultItemList.length + '"><b>' + resultItemMap.Header + '</b><br/>Detect Description:' + resultItemMap.Description + '</td>';
    let i = 0;
    resultItemMap.ResultItemList.forEach((resultItem) => {
      if (i > 0) pageHTML += '<tr>';
      pageHTML += '<td>' + resultItem.Name + '</td><td>' + resultItem.DetectedResult + '</td>';
      if (i > 0) pageHTML += '</tr>';
      i++;
    });
    pageHTML += '</tr>';
  });
  pageHTML += '</tbody></table>';
  const blob = new Blob([pageHTML], { type: 'data:attachment/text' });
  if (blob === undefined) {
    return;
  }

  const url = window.URL.createObjectURL(new Blob([blob]));
  const link = document.createElement('a');
  link.href = url;
  const dateStr = currentDate.toISOString().replace(/:/g, '-').replace('T', '-').replace('Z', '').replace('.', '-').replace(/ /g, '-');
  link.setAttribute('download', 'AutoDetectionResult-' + dateStr + '.html');
  link.click();
}

export function DetectionResult (props: StepWizardProps) {
  const wizardProps: StepWizardChildProps = props as StepWizardChildProps;
  const configureMethod = useSelector((state: AppState) => state.configureMethod);
  const detectionResult = useSelector((state: AppState) => state.detectResult);
  const navSteps = getNavSteps(wizardProps, configureMethod);
  const wizard = WizardNavBar(wizardProps, navSteps);
  const dispatch = useDispatch();
  const [description, setDescription] = useState<string>('');
  const [seletecdItem, setSelectedItem] = useState<string>('');
  const exportDetectionResult = () => {
    if (detectionResult.detectionResult) {
      getDetectionSummary(detectionResult.detectionResult);
    }
    else {
      alert('No detection result!');
    }
  };

  useEffect(() => {
    dispatch(DetectionResultSrv.getDetectionResult());
  }, [dispatch])

  const onPreviousButtonClick = () => {
    if (configureMethod && configureMethod.selectedMethod === ConfigureMethod.ConfigureMethod_AutoDetection) {
      wizardProps.previousStep();
    } else {
      wizardProps.goToStep(RunSteps.CONFIGURE_METHOD);
    }
  };

  const onNextButtonClick = () => {
    wizardProps.nextStep();
  };

  const onLinkClick = (ev?: React.MouseEvent<HTMLElement>, item?: INavLink): void => {
    if (item) {
      if (item.key) setSelectedItem(item.key);

      const data = item.data as ResultItem;
      const result = data.DetectedResult;
      let text = '';
      if (result === 'Supported') {
        text = data.Name + ' is found supported after detection'
      }
      else if (result === 'UnSupported') {
        text = data.Name + ' is found not supported after detection'
      }
      else {
        text = 'Detection failed'
      }

      setDescription(text);
    }
  }

  const iconMap: { [id: string]: IIconProps; } = {
    Supported: {
      iconName: 'SkypeCircleCheck',
      styles: {
        root: {
          color: 'green'
        }
      }
    },
    UnSupported: {
      iconName: 'StatusErrorFull',
      styles: {
        root: {
          color: 'red'
        }
      }
    },
    DetectFail: {
      iconName: 'IncidentTriangle',
      styles: {
        root: {
          color: 'orange'
        }
      }
    },
  };
  let navLinkGroups: INavLinkGroup[] = [];
  if (detectionResult.detectionResult !== undefined && detectionResult.detectionResult.ResultItemMapList !== undefined) {
    navLinkGroups = detectionResult.detectionResult.ResultItemMapList.map(itemMap => {
      return {
        name: itemMap.Header,
        links: itemMap.ResultItemList.map((item, index) => {
          return {
            name: item.Name,
            url: '',
            key: item.Name + index,
            iconProps: iconMap[item.DetectedResult],
            data: item
          }
        }),
        collapseByDefault: true,
        groupData: itemMap
      };
    });
  }

  const onMouseEnterGroupHeader = (props?: IRenderGroupHeaderProps) => {
    if (props) {
      setDescription((navLinkGroups.filter(n => n.name === props.name)[0].groupData as ResultItemMap).Description);
    }
  };

  return (
    <StepPanel leftNav={wizard} isLoading={detectionResult.isDetectionResultLoading} errorMsg={detectionResult.errorMsg} >
      <Stack style={{ height: '100%' }}>
        <Stack style={{ height: '100%' }}>
          <Nav
            onLinkClick={onLinkClick}
            selectedKey={seletecdItem}
            onRenderGroupHeader={(props, defaultRender) => <div onMouseEnter={() => onMouseEnterGroupHeader(props)}>{defaultRender!(props)}</div>}
            groups={navLinkGroups}
          />
        </Stack>
        <Stack>
          <div>{description}</div>
          <div className='buttonPanel'>
            <Stack
              horizontal
              horizontalAlign="end" tokens={StackGap10}>
              <PrimaryButton onClick={() => exportDetectionResult()}>Export Detection Result</PrimaryButton>
              <PrimaryButton onClick={() => onPreviousButtonClick()}>Previous</PrimaryButton>
              <PrimaryButton onClick={() => onNextButtonClick()}>Next</PrimaryButton>
            </Stack>
          </div>
        </Stack>
      </Stack>
    </StepPanel>
  )
};