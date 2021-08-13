// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import styled from '@emotion/styled'

export const FixedActionBar = styled.div`
    display: flex;
    justify-content: center;
    align-items: center;
    position: fixed;
    left: 0;
    right: 0;
    height: 50px;
    bottom: 0;
    background: #eee;
    z-index:999;
`

export const FixedContent = styled.div`
    margin-bottom: 50px;
`

export const Split = styled.div`
    width: 1px;
    height:23px;
    background-color: #434343;
    margin-left:2px;
    margin-right:2px;
`
