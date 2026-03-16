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
Branch naming convention: `ai-work/<short-descriptive-name>`

```bash
git checkout -b ai-work/<appropriate-name-for-the-task>
```

Use kebab-case for the branch name. Examples:
- `ai-work/fix-smb2-query-directory`
- `ai-work/add-rdp-test-cases`
- `ai-work/update-fileserver-user-guide`

### 3. Complete the task

Work on the branch. Follow the existing code conventions and the contribution guidelines in `CONTRIBUTING.md`:
- Run impacted test cases and verify they pass
- Write clear, focused commit messages (e.g., `Fix SMB2 negotiate response handling`)

#### Documentation updates when adding new tests

When adding new test cases, **all of the following must be updated** before committing:

| What changed | What to update |
|---|---|
| New test case(s) added to any suite | `TestSuites/<SuiteName>/docs/Test_Design_Spec/` — add the test case entry (name, category, scenario description, pass criteria) |
| New configuration property added | `TestSuites/<SuiteName>/docs/User_Guide/` — document the property, its valid values, and its effect |
| New protocol sub-domain or feature area | `ai-docs/test-suites-guide.md` — add or update the feature area table for that suite |
| New MS-XXXX protocol referenced in code | `ai-docs/protocol-specifications.md` — add the protocol entry with spec link, codebase paths, and key concepts |
| New reusable adapter, base class, or SDK component | `ai-docs/protosdk-reference.md` or `ai-docs/test-suites-guide.md` — document the component so AI contributors know to reuse it |
| New skill reference file added to `.github/skills/` | `ai-docs/README.md` — add the skill to the Available Skills table |

> **Why this matters:** AI contributors rely on `ai-docs/` to discover existing components. If a new adapter or base class is undocumented, future AI will re-implement it from scratch, creating duplicates. Keeping docs in sync is as important as the code itself.

### 4. Commit and open a Pull Request to main

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

### 5. New instructions received while on a feature branch

If new instructions are given while already on a feature branch, **always confirm with the user** before proceeding:

> "You are currently on branch `ai-work/<name>`. Do you want me to continue on this branch, or start fresh from main on a new branch?"

Only proceed once the user has confirmed which path to take.

---

## Contributing Guidelines (from CONTRIBUTING.md)

- PRs must include a clear description of the change
- If adding new test cases, update the corresponding **Test Design Specification**
- If adding new configuration, update the corresponding **User Guide**
- Follow existing code conventions throughout the codebase
