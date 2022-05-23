// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Spinner, SpinnerSize } from '@fluentui/react'

export function LoadingPanel () {
  return (<div style={{ marginTop: 20, marginLeft: 20, float: 'left' }}>
        <Spinner size={SpinnerSize.large} />
        <p style={{ color: '#ab5f0e' }}>Loading...</p>
    </div>)
}
