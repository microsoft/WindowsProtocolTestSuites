export interface ProfileExportRequest {
  /// <summary>
  /// Test suite id.
  /// </summary>
  TestSuiteId: number

  /// <summary>
  // Configuration id.
  /// </summary>
  ConfigurationId: number

  /// <summary>
  /// Profile package.
  /// </summary>
  SelectedTestCases: string[]

  /// <summary>
  /// Test result id.
  /// </summary>
  TestResultId?: number

  /// <summary>
  /// File name.
  /// </summary>
  FileName?: string
}
