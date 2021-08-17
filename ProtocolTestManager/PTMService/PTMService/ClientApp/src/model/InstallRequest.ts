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
