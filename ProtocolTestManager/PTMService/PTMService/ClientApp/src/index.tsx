// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'react-devtools'
import { initializeIcons } from '@uifabric/icons'
import * as React from 'react'
import * as ReactDOM from 'react-dom'
import { Provider } from 'react-redux'
import { ConnectedRouter } from 'connected-react-router'
import { configureStore, history } from './store/configureStore'
import App from './App'
import registerServiceWorker from './registerServiceWorker'

// Initialize Fabric's icons
initializeIcons()

// Get the application-wide store instance, prepopulating with state from the server where available.
const store = configureStore()

ReactDOM.render(
    <Provider store={store}>
        <ConnectedRouter history={history}>
            <App />
        </ConnectedRouter>
    </Provider>,
    document.getElementById('root'))

registerServiceWorker()
