// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React, { FunctionComponent } from "react"
import { IGroupHeaderProps, GroupHeader, IGroup, GroupedList } from '@fluentui/react';
import { TreePanel } from './TreePanel';
import { RuleGroup, SelectedRuleGroup, GroupItem } from "../model/RuleGroup";
import { useWindowSize } from './UseWindowSize';
import { HeaderMenuHeight } from './WizardNavBar';

type RuleListPanelProps = {
    ruleGroups: RuleGroup[]
    selected: SelectedRuleGroup[]
    checkedAction: (data: SelectedRuleGroup) => void;
}

const onRenderCell = (nestingDepth?: number, item?: GroupItem, itemIndex?: number): React.ReactNode => {
    return item ? (
        <TreePanel groupName={item.name} rules={item.rules} checked={item.checked} selectAction={item.action} />
    ) : null;
};


const onRenderHeader = (props?: IGroupHeaderProps): JSX.Element => {
    return (
        <GroupHeader styles={{ "check": { "display": "none" }, "headerCount": { "display": "none" }, "title": { "font-weight": "normal", "fontSize": "13px" } }} {...props} />
    );
}

export const RuleListPanel: FunctionComponent<RuleListPanelProps> = (props) => {
    const winSize = useWindowSize();
    const groups: IGroup[] = props.ruleGroups.map((group, index) => { return { key: group.Name, name: group.DisplayName || group.Name, startIndex: index, count: 1, level: 0 } });
    const items: GroupItem[] = props.ruleGroups.map(group => { return { key: group.Name, name: group.Name, rules: group.Rules, checked: props.selected.find(e => e.Name == group.Name)?.Selected || [], action: props.checkedAction } })
    return (
        <div style={{ height: winSize.height - HeaderMenuHeight - 100, overflowY: 'auto' }}>
            <GroupedList items={items} onRenderCell={onRenderCell} groupProps={{ onRenderHeader }} groups={groups} />
        </div>
    )
}
