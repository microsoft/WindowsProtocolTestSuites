export interface ProfileUploadRequest {
    /// <summary>
    /// Test suite id.
    /// </summary>
    TestSuiteId: number;

    /// <summary>
    // Configuration id.
    /// </summary>
    ConfigurationId: number;

    /// <summary>
    /// Profile package.
    /// </summary>
    Package: Blob;
}