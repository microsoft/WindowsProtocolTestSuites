// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/**
 * Factory function to generate reducer case handlers for async request/success/failure patterns.
 * Eliminates repetitive switch statements in Redux reducers.
 * 
 * Usage:
 *   const handlers = createAsyncHandlers('isLoading', 'data', 'errorMsg')
 *   case REQUEST: return handlers.request(state)
 *   case SUCCESS: return handlers.success(state, action.payload)
 *   case FAILURE: return handlers.failure(state, action.payload)
 */
export function createAsyncHandlers<S extends Record<string, any>, K1 extends keyof S, K2 extends keyof S, K3 extends keyof S>(
  loadingKey: K1,
  dataKey: K2,
  errorKey: K3
) {
  return {
    request: (state: S): S => ({
      ...state,
      [loadingKey]: true,
      [errorKey]: undefined
    }) as S,
    
    success: (state: S, payload: any): S => ({
      ...state,
      [loadingKey]: false,
      [dataKey]: payload,
      [errorKey]: undefined
    }) as S,
    
    failure: (state: S, payload: any): S => ({
      ...state,
      [loadingKey]: false,
      [errorKey]: payload
    }) as S
  }
}

/**
 * Variant for multiple loading states (e.g., isPrerequisiteLoading, isStepsLoading).
 * 
 * Usage:
 *   const prerequisiteHandlers = createAsyncHandlers('isPrerequisiteLoading', 'prerequisite', 'errorMsg')
 */
export function createAsyncHandlersMulti<S extends Record<string, any>, K1 extends keyof S & string, K2 extends keyof S & string, K3 extends keyof S & string>(
  loadingKey: K1,
  dataKey: K2,
  errorKey: K3
) {
  return createAsyncHandlers<S, K1, K2, K3>(loadingKey, dataKey, errorKey)
}
