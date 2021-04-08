// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Spinner, SpinnerSize, Label, Stack, IStackStyles, IconButton, Text, getTheme } from '@fluentui/react';
import styled from '@emotion/styled';
import { StackGap5 } from './StackStyle';
import React, { useRef, useState } from 'react';

export interface IFile {
    FileName: string;
    File: Blob;
}

export interface FileUploaderProp {
    onSuccess: (files: IFile[]) => void;
    label?: string;
    suffix?: string[];
    iconName?: string;
    className?: string;
    disabled?: boolean;
    placeholder?: string;
    maxFileCount?: number;
    onProcess?: (percentage: number) => void;
}

export function FileUploader(props: FileUploaderProp) {
    const containerStackStyles: IStackStyles = {
        root: {
            width: `100%`
        }
    };

    const [files, setFiles] = useState<IFile[]>();
    const [errorMsg, setErrorMsg] = useState('');
    const fileRef = useRef<HTMLInputElement>(null);

    const _onIconClicked = (): void => {
        if(fileRef.current){
            fileRef.current.click();
        }
    }

    const _generateErrorMessag = (): string => {
        if (!props.suffix || props.suffix.length === 0) {
            return '';
        }
        return 'Document must end with "' + props.suffix.join(', ') + '"';
    }

    const checkFileType = (files: IFile[]): boolean => {
        if (!props.suffix || props.suffix.length === 0) {
            return true;
        }
        if (props.suffix === undefined || props.suffix === null || props.suffix.length === 0) {
            return true;
        }

        let flag: boolean = true;
        files.forEach((file) => {
            let fileEndWithSuffix = false;
            props.suffix!.forEach((itemSuffix: string) => {
                fileEndWithSuffix = file.FileName.endsWith(itemSuffix) || fileEndWithSuffix;
            });
            flag = flag && fileEndWithSuffix;
        });

        return flag;
    }

    const ConvertFileListToIFiles = (fileList: FileList): IFile[] => {
        const files: IFile[] = [];
        // tslint:disable-next-line: prefer-for-of
        for (let i = 0; i < fileList.length; i++) {
            const file: IFile = {
                FileName: fileList[i].name,
                File: fileList[i]
            };
            files.push(file);
        }
        return files;
    }

    const _onFileChange = (): void => {
        let successSelectedFiles: IFile[] = [];
        if (fileRef.current && fileRef.current.files) {
            // Select none files
            if (fileRef.current.files.length <= 0) {
                return;
            }
            // Check file validation
            const addedFiles: IFile[] = ConvertFileListToIFiles(fileRef.current.files);

            if (!checkFileType(addedFiles)) {
                const typeErrorMsg: string = _generateErrorMessag();
                if (typeErrorMsg.length > 0) {
                    setErrorMsg(typeErrorMsg);
                    setFiles([]);
                }
            } else {
                setFiles(addedFiles);
                setErrorMsg('');
                successSelectedFiles = addedFiles;
            }
        } else {
            const unknownErrorMsg: string = 'Unknown error';
            setErrorMsg(unknownErrorMsg);
        }

        props.onSuccess(successSelectedFiles);
    }

    return (<div className={props.className}>
        <Label>{props.label}</Label>
        <div style={{ borderStyle: 'solid', borderWidth: 1, borderColor: 'grey' }}>
            <FlexContainer>
                <Stack horizontal wrap styles={containerStackStyles} horizontalAlign="space-between">
                    <Stack horizontal wrap tokens={StackGap5}>
                        <div style={{ marginTop: 8, marginBottom: 8, paddingLeft: 6 }}>
                            {
                                (files && files.length > 0) ?
                                    files.map((file, index) => {
                                        return <span key={index}>
                                            {file.FileName}{(index === (files.length - 1)) ? '' : ';'}&nbsp;&nbsp;
                                </span>;
                                    }) :
                                    <span>{props.placeholder}</span>
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
                            onChange={_onFileChange}
                            style={{ display: 'none' }}
                        />
                        <IconButton
                            className="icon"
                            iconProps={{ iconName: 'DocumentSet' }}
                            onClick={_onIconClicked}
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
    </div>);
}

export const FlexContainer = styled.div`
    display       : flex;
    flex-direction: row;
`;

export const FlexAutoItem = styled.div`
    display       : flex;
    flex-direction: row;
`;