export interface ProfileUploadRequest {
  // Test suite id.
  TestSuiteId: number

  // Configuration id.
  ConfigurationId: number

  // Profile package.
  Package: Blob
}
