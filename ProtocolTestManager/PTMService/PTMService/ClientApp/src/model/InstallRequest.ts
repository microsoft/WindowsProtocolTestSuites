// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export interface InstallRequest {
  /// <summary>
  /// Test suite name.
  /// </summary>
  TestSuiteName: string

  /// <summary>
  /// Test suite package.
  /// </summary>
  Package: Blob

  /// <summary>
  /// Description.
  /// </summary>
  Description: string
}
