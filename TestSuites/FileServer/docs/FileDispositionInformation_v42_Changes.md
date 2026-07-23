# MS-FSA v42.0 FileDispositionInformation Test Implementation

## Specification Change Summary

**Protocol**: MS-FSA  
**Section**: 2.1.5.15.3 - FileDispositionInformation  
**Version**: v41.0 → v42.0  
**Change Type**: Modified  

### Key Change

The specification v42.0 adds a new requirement when marking a directory for deletion:

**New Behavior (v42.0):**
```
If Open.Stream.StreamType is DirectoryStream:
    Set Open.ChangeNotifyDirectoryMarkedDeleted to TRUE.  // <-- NEW LINE
    For each ChangeNotifyEntry in Volume.ChangeNotifyList...
```

This flag (`Open.ChangeNotifyDirectoryMarkedDeleted`) is set to TRUE before processing change notification entries when a directory stream is marked for deletion via FileDispositionInformation.

## Test Implementation Changes

### 1. Model-Based Tests (FSAModel)

**File**: `TestSuites/FileServer/src/FSAModel/Model/QueryAndSetFileInformation/QueryAndSetFileInformationModel.cs`

**Change**: Added requirement capture for the new DirectoryStream behavior in `SetFileDispositionInfo` method:

```csharp
//If Open.Stream.StreamType is DirectoryStream, set Open.ChangeNotifyDirectoryMarkedDeleted to TRUE
if (gStreamType == StreamType.DirectoryStream)
{
    Helper.CaptureRequirement(4014, @"[In FileDispositionInformation,Pseudocode for the operation is as follows:]
        If InputBuffer.DeletePending is TRUE:If Open.Stream.StreamType is DirectoryStream:Set Open.ChangeNotifyDirectoryMarkedDeleted to TRUE.");
}
```

**Requirement ID**: 4014 - New requirement to capture the DirectoryStream deletion flag behavior

### 2. Traditional Tests (FSA)

**File**: `TestSuites/FileServer/src/FSA/TestSuite/FileInformation/FileInfo_Set_FileDispositionInformation.cs` (NEW)

**Test Cases Added**:

#### 2.1 FileInfo_Set_FileDispositionInformation_File
- **Category**: Positive
- **Description**: Verifies that FileDispositionInformation can be set on a data file
- **Steps**:
  1. Create a new data file with DELETE access
  2. Set FileDispositionInformation with DeletePending = TRUE
  3. Verify operation succeeds (STATUS_SUCCESS)

#### 2.2 FileInfo_Set_FileDispositionInformation_Directory
- **Category**: Positive
- **Description**: Verifies that FileDispositionInformation can be set on a directory and that the new v42.0 behavior is documented
- **Steps**:
  1. Create a new directory with DELETE access
  2. Set FileDispositionInformation with DeletePending = TRUE
  3. Verify operation succeeds (STATUS_SUCCESS)
- **Documentation**: Test includes comment referencing MS-FSA 2.1.5.15.3 (v42.0) about the `Open.ChangeNotifyDirectoryMarkedDeleted` flag

#### 2.3 FileInfo_Set_FileDispositionInformation_AccessDenied
- **Category**: UnexpectedFields (Negative)
- **Description**: Verifies that STATUS_ACCESS_DENIED is returned when DELETE access is not granted
- **Steps**:
  1. Create a file without DELETE access (GENERIC_READ | GENERIC_WRITE only)
  2. Attempt to set FileDispositionInformation with DeletePending = TRUE
  3. Verify operation fails with STATUS_ACCESS_DENIED

#### 2.4 FileInfo_Set_FileDispositionInformation_ReadonlyFile
- **Category**: UnexpectedFields (Negative)
- **Description**: Verifies that STATUS_CANNOT_DELETE is returned for readonly files
- **Steps**:
  1. Create a file with READONLY attribute
  2. Attempt to set FileDispositionInformation with DeletePending = TRUE
  3. Verify operation fails with STATUS_CANNOT_DELETE

## Test Coverage

The implementation provides comprehensive test coverage for FileDispositionInformation:

| Requirement | Test Case | Type |
|-------------|-----------|------|
| Basic deletion on file | FileInfo_Set_FileDispositionInformation_File | Positive |
| Basic deletion on directory with v42.0 flag | FileInfo_Set_FileDispositionInformation_Directory | Positive |
| DELETE access required | FileInfo_Set_FileDispositionInformation_AccessDenied | Negative |
| Readonly files cannot be deleted | FileInfo_Set_FileDispositionInformation_ReadonlyFile | Negative |
| DirectoryStream deletion flag (Requirement 4014) | Model-based test | Model |

## Observable Behavior

The specification change adds an internal state flag (`Open.ChangeNotifyDirectoryMarkedDeleted`) that is set but not directly observable from test perspective. The flag likely affects subsequent operations or cleanup scenarios not visible in this section of the specification.

The existing v41.0 behavior for change notification (completing with STATUS_DELETE_PENDING) remains unchanged in v42.0. The new flag is set **before** processing change notification entries.

## Validation

- ✅ Model-based test updated with new requirement (4014)
- ✅ Traditional tests added for all scenarios
- ✅ Build verification passed
- ✅ Test categories use correct values (UnexpectedFields for negative tests)
- ✅ Documentation includes v42.0 specification reference

## Related Files Modified

1. `TestSuites/FileServer/src/FSAModel/Model/QueryAndSetFileInformation/QueryAndSetFileInformationModel.cs` - Added requirement 4014
2. `TestSuites/FileServer/src/FSA/TestSuite/FileInformation/FileInfo_Set_FileDispositionInformation.cs` - New test file with 4 test cases

## Build Status

✅ All builds passed successfully after implementation

## Notes for Test Execution

- Tests require DELETE access permissions on test share
- Directory deletion tests verify empty directory deletion only (non-empty directories should fail with STATUS_DIRECTORY_NOT_EMPTY per existing spec)
- Readonly file test may behave differently on different file systems (NTFS, ReFS, FAT32)
