// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React from 'react'
import { Route, Switch, Redirect } from 'react-router-dom'
import HomePage from './pages/HomePage'

import { Header } from './components/Header'
import { TestSuiteRunWizard } from './pages/TestSuiteRunWizard'
import { TaskHistory } from './pages/TaskHistory'
import { TestResultDetail } from './pages/TestResultDetail'
import './css/index.css'
import { IStackStyles, IStackTokens, Stack } from '@fluentui/react'
import { useWindowSize } from './components/UseWindowSize'
import { Management } from './pages/Management'

export default function App () {
  const winSize = useWindowSize()

  const stackStyles: IStackStyles = {
    root: {
      overflow: 'hidden'
    }
  }

  return (
        <Stack verticalFill styles={stackStyles}>
            <Header />
            <Switch>
                <Route exact path='/' component={HomePage} />
                <Route exact path='/Tasks/NewRun' component={TestSuiteRunWizard} />
                <Route exact path='/Tasks/History' component={TaskHistory} />
                <Route exact path='/Tasks/TestResult' component={TestResultDetail} />
                <Route exact path='/Management' component={Management} />
                <Redirect from="*" to="/" />
            </Switch>
        </Stack>
  )
}
