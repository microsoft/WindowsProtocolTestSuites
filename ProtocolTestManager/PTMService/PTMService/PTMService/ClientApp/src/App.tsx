// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import HomePage from './pages/HomePage';

import { Header } from './components/Header';
import { TestSuiteRunWizard } from './pages/TestSuiteRunWizard';
import { TaskHistory } from './pages/TaskHistory';
import './css/index.css';

export default function App() {
    return (
        <div>
            <Header />
            <Switch>
                <Route exact path='/' component={HomePage} />
                <Route exact path='/Tasks/NewRun' component={TestSuiteRunWizard} />
                <Route exact path='/Tasks/History' component={TaskHistory} />
                <Redirect from="*" to="/" />
            </Switch>
        </div>
    )
}