// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "RDMANdv2.h"

using namespace System;
using namespace System::Runtime::InteropServices;

BEGIN_RDMA_NAMESPACE

RdmaMemoryWindow::~RdmaMemoryWindow()
{
    if (_memoryWindow != NULL)
    {
        _memoryWindow->Release();
        _memoryWindow = NULL;
    }
}

RdmaMemoryWindow::RdmaMemoryWindow(IND2MemoryWindow *memoryWindow, unsigned __int64 invalidateResultId)
{
    _memoryWindow = memoryWindow;
    _invalidateResultId = invalidateResultId;
}

END_RDMA_NAMESPACE
