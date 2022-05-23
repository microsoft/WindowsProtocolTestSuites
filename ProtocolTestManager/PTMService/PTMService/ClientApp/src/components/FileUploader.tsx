// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Label, Stack, IStackStyles, IconButton, Text, getTheme } from '@fluentui/react'
import styled from '@emotion/styled'
import { StackGap5 } from './StackStyle'
import { ReactElement, useRef, useState } from 'react'

export interface IFile {
  FileName: string
  File: Blob
}

export interface FileUploaderProp {
  onSuccess: (files: IFile[]) => void
  label?: string
  suffix?: string[]
  iconName?: string
  className?: string
  disabled?: boolean
  placeholder?: string
  maxFileCount?: number
  onProcess?: (percentage: number) => void
}

export function FileUploader(props: FileUploaderProp): ReactElement {
  const containerStackStyles: IStackStyles = {
    root: {
      width: '100%'
    }
  }

  const [files, setFiles] = useState<IFile[]>()
  const [errorMsg, setErrorMsg] = useState<string | undefined>(undefined)
  const fileRef = useRef<HTMLInputElement>(null)

  const onIconClick = (): void => {
    if (fileRef.current != null) {
      fileRef.current.click()
    }
  }

  const generateErrorMessage = (): string | undefined => {
    if ((props.suffix == null) || props.suffix.length === 0) {
      return undefined
    }
    return 'All files must end with "' + props.suffix.join(', ') + '"'
  }

  const checkFileType = (files: IFile[]): boolean => {
    if (props.suffix === undefined || props.suffix === null || props.suffix.length === 0) {
      return true
    }

    const flag = files.reduce((prevFlag: boolean, currFile: IFile) => {
      return props.suffix !== undefined
        ? prevFlag && props.suffix.some(itemSuffix => currFile.FileName.endsWith(itemSuffix))
        : prevFlag
    }, true)

    return flag
  }

  const convertFileListToIFiles = (fileList: FileList): IFile[] => {
    return [...(new Array(fileList.length)).keys()].map(index => {
      return {
        FileName: fileList[index].name,
        File: fileList[index]
      }
    })
  }

  const onFileChange = (): void => {
    if (fileRef.current?.files != null) {
      // Select none files
      if (fileRef.current.files.length <= 0) {
        return
      }

      // Select files exceeding maximum file count
      if (props.maxFileCount !== undefined && fileRef.current.files.length > props.maxFileCount) {
        setErrorMsg(`Only ${props.maxFileCount} files can be uploaded!`)
        return
      }

      // Validate files
      const addedFiles: IFile[] = convertFileListToIFiles(fileRef.current.files)

      if (!checkFileType(addedFiles)) {
        const typeErrorMsg = generateErrorMessage()
        if (typeErrorMsg !== undefined && typeErrorMsg.length > 0) {
          setErrorMsg(typeErrorMsg)
          setFiles([])
        }
      } else {
        setErrorMsg(undefined)
        setFiles(addedFiles)
        props.onSuccess(addedFiles)
        return
      }
    } else {
      setErrorMsg('Unknown error')
    }

    props.onSuccess([])
  }

  return (<div className={props.className}>
    <Label>{props.label}</Label>
    <div style={{ borderStyle: 'solid', borderWidth: 1, borderColor: 'grey' }}>
      <FlexContainer>
        <Stack horizontal wrap styles={containerStackStyles} horizontalAlign="space-between">
          <Stack horizontal wrap tokens={StackGap5}>
            <div style={{ marginTop: 8, marginBottom: 8, paddingLeft: 6 }}>
              {
                ((files != null) && files.length > 0)
                  ? files.map((file, index) => {
                    return <span key={index}>
                      {file.FileName}{(index === (files.length - 1)) ? '' : ';'}&nbsp;&nbsp;
                    </span>
                  })
                  : <span>{props.placeholder}</span>
              }
            </div>
          </Stack>
        </Stack>
        <FlexAutoItem>
          <div>
            <input
              type="file"
              multiple
              ref={fileRef}
              disabled={props.disabled}
              onChange={onFileChange}
              style={{ display: 'none' }}
            />
            <IconButton
              className="icon"
              aria-label="Browse File"
              iconProps={{ iconName: 'DocumentSet' }}
              disabled={props.disabled}
              onClick={onIconClick}
            />
          </div>
        </FlexAutoItem>
      </FlexContainer>
    </div>
    <div>
      <Text variant="small" style={{ color: getTheme().palette.red }}>
        {errorMsg}
      </Text>
    </div>
  </div>)
}

export const FlexContainer = styled.div`
    display       : flex;
    flex-direction: row;
`

export const FlexAutoItem = styled.div`
    display       : flex;
    flex-direction: row;
`
