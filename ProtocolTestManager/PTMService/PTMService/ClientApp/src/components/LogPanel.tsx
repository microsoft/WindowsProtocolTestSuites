// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import * as React from 'react'
import { ReactElement, useCallback, useEffect, useRef, useState } from 'react'
import { DefaultButton, Stack } from '@fluentui/react'

interface LogPanelProps {
  log: string
  canDock: boolean
  isOpen: boolean
  onToggle: (open: boolean) => void
  title?: string
  windowHeight?: number
  headerHeight?: number
  isDetecting?: boolean
}

const STATUS_STYLES: Record<string, React.CSSProperties> = {
  'Failed': { paddingLeft: 5, color: '#a30000' },
  'Cancelled': { paddingLeft: 5, color: '#a30000' },
  'Finished': { paddingLeft: 5, color: '#006100' },
  'Detecting': { paddingLeft: 5, color: '#0000ff' },
  'Canceling': { paddingLeft: 5, color: '#0000ff' },
  'NotStart': { paddingLeft: 5 },
  'Pending': { paddingLeft: 5 }
}

export function LogPanel({ log, canDock, isOpen, onToggle, title = 'Detection Log' }: LogPanelProps): ReactElement {
  const [hasUnreadLogLines, setHasUnreadLogLines] = useState<boolean>(false)
  const [isLogPinnedToBottom, setIsLogPinnedToBottom] = useState<boolean>(true)
  const logContentRef = useRef<HTMLDivElement>(null)
  const previousLogLengthRef = useRef<number>(0)

  useEffect(() => {
    if (!logContentRef.current) return

    const newLogLength = log.length
    const hasNewContent = newLogLength > previousLogLengthRef.current

    if (hasNewContent) {
      previousLogLengthRef.current = newLogLength
      
      if (isLogPinnedToBottom) {
        setHasUnreadLogLines(false)
        setTimeout(() => {
          if (logContentRef.current) {
            logContentRef.current.scrollTop = logContentRef.current.scrollHeight
          }
        }, 0)
      } else {
        setHasUnreadLogLines(true)
      }
    }
  }, [log, isLogPinnedToBottom])

  const handleScroll = useCallback(() => {
    if (!logContentRef.current) return
    
    const { scrollTop, scrollHeight, clientHeight } = logContentRef.current
    const isAtBottom = scrollHeight - scrollTop - clientHeight < 10
    
    if (isAtBottom && hasUnreadLogLines) {
      setHasUnreadLogLines(false)
    }
    
    setIsLogPinnedToBottom(isAtBottom)
  }, [hasUnreadLogLines])

  if (!canDock && !isOpen) {
    return <DefaultButton 
      text={`${hasUnreadLogLines ? '(!) ' : ''}${isOpen ? 'Hide Log' : 'Show Log'}`}
      onClick={() => onToggle(!isOpen)}
      styles={{ root: { marginTop: 10 } }}
    />
  }

  const panelStyle: React.CSSProperties = canDock 
    ? {
        width: 400,
        height: '100%',
        maxHeight: '100%',
        borderLeft: '1px solid #e0e0e0',
        overflowY: 'hidden',
        display: 'flex',
        flexDirection: 'column',
        backgroundColor: '#f5f5f5',
        flexShrink: 0
      }
    : {
        width: '100%',
        height: 300,
        maxHeight: 300,
        borderTop: '1px solid #e0e0e0',
        overflowY: 'hidden',
        display: isOpen ? 'flex' : 'none',
        flexDirection: 'column',
        backgroundColor: '#f5f5f5',
        marginTop: 10
      }

  const contentStyle: React.CSSProperties = {
    flex: 1,
    overflowY: 'auto',
    overflowX: 'hidden',
    padding: 10,
    fontFamily: 'monospace',
    fontSize: 12,
    lineHeight: 1.4,
    whiteSpace: 'pre-wrap',
    wordWrap: 'break-word'
  }

  const headerStyle: React.CSSProperties = {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: '8px 10px',
    borderBottom: '1px solid #d0d0d0',
    backgroundColor: '#fff',
    minHeight: 32
  }

  return (
    <div style={panelStyle}>
      <div style={headerStyle}>
        <span style={{ fontWeight: 'bold', fontSize: 13 }}>
          {hasUnreadLogLines && '(!) '}{title}
        </span>
        {!canDock && (
          <DefaultButton
            text={isOpen ? 'Hide Log' : 'Show Log'}
            onClick={() => onToggle(!isOpen)}
            styles={{ root: { marginLeft: 'auto' } }}
          />
        )}
      </div>
      <div 
        ref={logContentRef}
        style={contentStyle}
        onScroll={handleScroll}
      >
        {log || 'Waiting for logs...'}
      </div>
    </div>
  )
}
