// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FunctionComponent, useState, useEffect } from "react"
import { Rule, SelectedRuleGroup } from "../model/RuleGroup";
import CheckboxTree, {Node} from 'react-checkbox-tree';
import 'react-checkbox-tree/lib/react-checkbox-tree.css';
import { Icon } from '@fluentui/react/lib/Icon';

type TreePanelProps = {
    groupName: string
    rules: Rule[]
    checked: string[]
    selectAction: (data:SelectedRuleGroup) => void;
}

const getGroup = (rules: Rule[]): Node[] => {
    return rules.map(rule => {
        if (rule.Rules) {
            return {className:"treeNode", value: rule.Name, label: rule.DisplayName, children: getGroup(rule.Rules) }
        }
        if (rule.Categories) {
            let nodes = rule.Categories.map(curr => { return { value: curr, label: curr } })
            return {className:"treeNode", value: rule.Name, label: rule.DisplayName, children: nodes }
        }
        return {className:"treeNode", value: rule.Name, label: rule.DisplayName }
    })
}

const getItems = (rules: Rule[]): string[] => {
    let results: string[] = []
    rules.forEach(rule => {
        if (rule.Rules) {

            results.push(...getItems(rule.Rules))
        }
        results.push(rule.Name)
    })
    return results;
}
const getExpanded = (rules: Rule[]): string[] => {
    let groups: string[] = getItems(rules)
    return groups.concat('all')
}

export const TreePanel: FunctionComponent<TreePanelProps> = (props) => {
    let data = getGroup(props.rules);
    let expandedNode = getExpanded(props.rules);
    const [checked, setChecked] = useState<Array<string>>(props.checked);
    const [expanded, setExpanded] = useState<Array<string>>(expandedNode);

    useEffect(() => {
        if (JSON.stringify(checked) != JSON.stringify(props.checked)) {
            let data = { Name: props.groupName, Selected: checked }
            props.selectAction(data);              
        }
    }, [checked]);

    useEffect(() => {
        if(JSON.stringify(checked) != JSON.stringify(props.checked))
        {
            setChecked(props.checked);
        }       
    }, [props.checked])

    return (
        <div style={{border: '1px solid rgba(0, 0, 0, 0.35)', padding:5+'px'}}>
            <CheckboxTree nodes={data}
                checked={checked}
                expanded={expanded}
                onCheck={curr => setChecked(curr)}
                onExpand={expanded => setExpanded(expanded)}
                showNodeIcon={false}
                icons={{
                    check: <Icon iconName="CheckboxComposite" />,
                    uncheck: <Icon iconName="Checkbox" />,
                    halfCheck: <i className="ms-Icon ms-Icon--CheckboxIndeterminateCombo"/>,
                    expandClose: <Icon iconName="CaretHollow" />,
                    expandOpen: <Icon iconName="CaretSolid" />
                }} />
        </div>
    )
}
