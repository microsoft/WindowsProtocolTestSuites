# Windows Protocol Test Suites — Claude Code Instructions

## Codebase Overview

This is the **Windows Protocol Test Suites** repository — a .NET-based interoperability testing framework for Microsoft Open Specification protocols.

### Key Components

| Directory | Purpose |
|---|---|
| `ProtoSDK/` | Protocol library — message structures, encode/decode, send/receive for each protocol |
| `TestSuites/` | Individual test suites per protocol family (FileServer, RDP, Kerberos, SMBD, BranchCache, ADFamily, MS-AZOD, MS-ADFSPIP, MS-ADOD, MS-WSP, MS-XCA) |
| `ProtocolTestManager/` | PTMService — web UI to configure and run test cases |
| `CommonScripts/` | Shared deployment scripts |
| `InstallPrerequisites/` | Scripts to install required software dependencies |

### Primary Languages
- **C#** (.NET 8.0) — main protocol SDK and test suite code
- **C++** — ADFamily and MS-SMBD components
- **PowerShell** — build scripts (`build.ps1`), environment setup, batch runners

### Build
```powershell
cd TestSuites/<SuiteName>/src
./build.ps1
```

---

## Workflow — Required for Every Task

Follow these steps **before starting any task**:

### 1. Pull and update main branch
```bash
git checkout main
git pull origin main
```

### 2. Create a feature branch off main
Branch naming convention: `brucewayne/<short-descriptive-name>`

```bash
git checkout -b brucewayne/<appropriate-name-for-the-task>
```

Use kebab-case for the branch name. Examples:
- `brucewayne/fix-smb2-query-directory`
- `brucewayne/add-rdp-test-cases`
- `brucewayne/update-fileserver-user-guide`

### 3. Complete the task

Work on the branch. Follow the existing code conventions and the contribution guidelines in `CONTRIBUTING.md`:
- Run impacted test cases and verify they pass
- Update relevant documentation if adding new test cases or configurations
- Write clear, focused commit messages (e.g., `Fix SMB2 negotiate response handling`)

### 4. Open a Pull Request to main

```bash
gh pr create --base main --title "<concise title>" --body "$(cat <<'EOF'
## Summary
- <bullet point summary of changes>

## Test plan
- [ ] Ran impacted test cases — all pass
- [ ] Documentation updated (if applicable)

🤖 Generated with [Claude Code](https://claude.com/claude-code)
EOF
)"
```

---

## Contributing Guidelines (from CONTRIBUTING.md)

- PRs must include a clear description of the change
- If adding new test cases, update the corresponding **Test Design Specification**
- If adding new configuration, update the corresponding **User Guide**
- Follow existing code conventions throughout the codebase
