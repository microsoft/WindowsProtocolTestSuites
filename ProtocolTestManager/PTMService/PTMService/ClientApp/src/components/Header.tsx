// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Icon, IStackStyles } from '@fluentui/react'
import { Stack } from 'office-ui-fabric-react'
import React from 'react'
import { Link, NavLink } from 'react-router-dom'

export function Header () {
  const headerStackTokens: IStackStyles = {
    root: {
      color: '#FFF'
    }
  }

  return (
    <div className="header">
      <Stack horizontal horizontalAlign="space-between" styles={headerStackTokens} >
        <Stack horizontal horizontalAlign="start">
          <div className="title">
            <Link to="/">PTM Service</Link>
          </div>
          <div className="menu">
            <NavLink activeClassName="active" exact to="/Tasks/NewRun">Run TestSuite</NavLink>
            <NavLink activeClassName="active" exact to="/Tasks/History">View History</NavLink>
            <NavLink activeClassName="active" exact to="/Management">Management</NavLink>
          </div>
        </Stack>
        <Stack horizontal horizontalAlign="end">
          <div className="rightIcon">
            <Link to="/Help" title="Help">
              <Icon iconName="Help" />
            </Link>
          </div>
          <div className="rightIcon">
            <a href="mailto:prototest@microsoft.com" title="Contact us">
              <Icon iconName="Mail" />
            </a>
          </div>
        </Stack>
      </Stack>
    </div>
  )
}
