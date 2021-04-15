// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FunctionComponent, useState, useEffect } from "react"
import { AllNode, Rule, SelectedRuleGroup } from "../model/RuleGroup";
import CheckboxTree, { Node } from 'react-checkbox-tree';
import 'react-checkbox-tree/lib/react-checkbox-tree.css';
import { Icon } from '@fluentui/react/lib/Icon';

type TreePanelProps = {
    groupName: string
    rules: Rule[]
    checked: string[]
    selectAction: (data: SelectedRuleGroup) => void;
}

const getGroup = (rules: Rule[], parent: string): Node[] => {
    return rules.map(rule => {
        let curr = parent ? parent + "." + rule.Name : rule.Name
        if (rule.Rules) {
            return { className: "treeNode", value: curr, label: rule.DisplayName, categories: rule.Categories, children: getGroup(rule.Rules, curr) }
        }
        return { className: "treeNode", value: curr, label: rule.DisplayName, categories: rule.Categories }
    })
}

const createGroupItems = (rules: Rule[]): any[] => {
    const groups: any[] = getGroup(rules, AllNode.value)
    return [
        {
            value: AllNode.value, label: AllNode.lable, children: groups
        }
    ]
}

const getItems = (rules: Rule[], parent: string): string[] => {
    const results: string[] = []
    rules.forEach(rule => {
        let curr = parent ? parent + "." + rule.Name : rule.Name
        if (rule.Rules) {

            results.push(...getItems(rule.Rules, curr))
        }
        results.push(curr)
    })
    return results;
}
const getExpanded = (rules: Rule[]): string[] => {
    const groups: string[] = getItems(rules, AllNode.value)
    return groups.concat(AllNode.value)
}

export const TreePanel: FunctionComponent<TreePanelProps> = (props) => {
    const data = createGroupItems(props.rules);
    const expandedNode = getExpanded(props.rules);
    const [checked, setChecked] = useState<Array<string>>(props.checked);
    const [expanded, setExpanded] = useState<Array<string>>(expandedNode);

    useEffect(() => {
        if (JSON.stringify(checked) != JSON.stringify(props.checked)) {
            const data = { Name: props.groupName, Selected: checked }
            props.selectAction(data);
        }
    }, [checked]);

    useEffect(() => {
        if (JSON.stringify(checked) != JSON.stringify(props.checked)) {
            setChecked(props.checked);
        }
    }, [props.checked])

    return (
        <div style={{ border: '1px solid rgba(0, 0, 0, 0.35)', padding: 5 + 'px' }}>
            <CheckboxTree nodes={data}
                checked={checked}
                expanded={expanded}
                onCheck={curr => setChecked(curr)}
                onExpand={expanded => setExpanded(expanded)}
                showNodeIcon={false}
                icons={{
                    check: <Icon iconName="CheckboxComposite" />,
                    uncheck: <Icon iconName="Checkbox" />,
                    halfCheck: <i className="ms-Icon ms-Icon--CheckboxIndeterminateCombo" />,
                    expandClose: <Icon iconName="CaretHollow" />,
                    expandOpen: <Icon iconName="CaretSolid" />
                }} />
        </div>
    )
}
