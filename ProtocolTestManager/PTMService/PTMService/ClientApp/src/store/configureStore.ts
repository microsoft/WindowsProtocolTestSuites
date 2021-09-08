// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { applyMiddleware, createStore, combineReducers } from 'redux'
import thunkMiddleware from 'redux-thunk'
import { connectRouter, routerMiddleware } from 'connected-react-router'
import { appReducers } from '../reducers'
import { composeWithDevTools } from 'redux-devtools-extension'
import { createBrowserHistory } from 'history'

export const history = createBrowserHistory()

const rootReducer = combineReducers({
  ...appReducers,
  router: connectRouter(history)
})

export type AppState = ReturnType<typeof rootReducer>

export function configureStore (initialState?: AppState) {
  const middleware = [
    thunkMiddleware,
    routerMiddleware(history)
  ]

  const enhancers = []
  const windowIfDefined = typeof window === 'undefined' ? null : window as any // eslint-disable-line @typescript-eslint/no-explicit-any
  if (windowIfDefined && windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__) {
    enhancers.push(windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__())
  }

  return createStore(
    rootReducer,
    // @ts-expect-error See https://github.com/reduxjs/redux/pull/4078
    initialState,
    composeWithDevTools(applyMiddleware(...middleware), ...enhancers)
  )
}

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export type AppThunkAction<TAction> = (dispatch: (action: TAction) => void, getState: () => AppState) => void
