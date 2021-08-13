// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect, useState } from 'react'

const resizeEventName = 'resize'
export function useWindowSize () {
  const [winSize, setWinSize] = useState({ width: 800, height: 600 })

  useEffect(() => {
    function handleWinResize () {
      setWinSize({
        width: window.innerWidth,
        height: window.innerHeight
      })
    }

    window.addEventListener(resizeEventName, handleWinResize)
    handleWinResize()

    return () => {
      window.removeEventListener(resizeEventName, handleWinResize)
    }
  }, [])

  return winSize
}
