#!/bin/bash
#
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#
# Argument 1: If set, just list all filtered test cases instead of running tests actually.

InvocationPath=$(dirname "$0")

Filter=""

DryRun=$1

Cmd="$InvocationPath/RunTestCasesByFilter.sh \"$Filter\" \"$DryRun\""

eval $Cmd
