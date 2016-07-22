// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#include "Rdma.h"



using namespace System;
using namespace System::Runtime::InteropServices;


// Begin SMBD client Namespace
BEGIN_RDMA_NAMESPACE
/// <summary>
/// Deconstructor
/// </summary>
RdmaMemoryWindow::~RdmaMemoryWindow()
{
}

// =================================================================
// private

/// <summary>
/// Constructor
/// </summary>
/// <param name="memoryWindow">Memory window entity with NDSPI type</param>
RdmaMemoryWindow::RdmaMemoryWindow(INDMemoryWindow *memoryWindow)
{
	_memoryWindow = memoryWindow;
}

END_RDMA_NAMESPACE