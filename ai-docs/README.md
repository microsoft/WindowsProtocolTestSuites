# AI Documentation Index — Windows Protocol Test Suites

This directory contains AI-friendly documentation for the Windows Protocol Test Suites repository. It is intended for AI contributors and new engineers who need to understand the codebase rapidly and contribute effectively.

## Files

| File | Description |
|---|---|
| [architecture-overview.md](architecture-overview.md) | High-level component map, technology stack, how pieces fit together, Mermaid diagrams |
| [protosdk-reference.md](protosdk-reference.md) | Per-protocol breakdown of ProtoSDK, key base classes, encoding/decoding patterns |
| [test-suites-guide.md](test-suites-guide.md) | Overview of every test suite, test structure, inheritance hierarchy, how to add a test |
| [protocol-specifications.md](protocol-specifications.md) | Per-protocol MS-XXXX reference: purpose, codebase location, key concepts, spec links |
| [ptm-guide.md](ptm-guide.md) | Protocol Test Manager (PTM): architecture, web API, how to add test suite support |
| [build-and-setup.md](build-and-setup.md) | Prerequisites, build commands, environment setup, how to run tests |
| [contribution-guide.md](contribution-guide.md) | Step-by-step guide for AI contributors: branching, code placement, conventions, PR process |

## Quick Start for AI Contributors

1. **Load relevant skills first** — before touching any code, check [`.github/skills/`](../.github/skills/) for a skill matching your domain. Skills contain mandatory workflows, domain classification rules, and reusable component maps that prevent common mistakes. See the [Skills & Instructions](#skills--instructions-critical-for-ai-contributors) section below.
2. Read [architecture-overview.md](architecture-overview.md) to understand what the repo does and how the components relate.
3. Identify which protocol you are working on (e.g., SMB2, RDP, Kerberos) and read the relevant section of [protocol-specifications.md](protocol-specifications.md).
4. Find the corresponding ProtoSDK implementation in [protosdk-reference.md](protosdk-reference.md) and the test suite in [test-suites-guide.md](test-suites-guide.md).
5. Follow [build-and-setup.md](build-and-setup.md) to build the affected test suite.
6. Follow [contribution-guide.md](contribution-guide.md) for branch naming, commit conventions, and PR requirements.

---

## Skills & Instructions (Critical for AI Contributors)

This repository ships two types of AI guidance that **must be consulted before implementing anything**. They are located in [`.github/instructions/`](../.github/instructions/) and [`.github/skills/`](../.github/skills/).

### Instructions — Always-On Rules

Located in [`.github/instructions/`](../.github/instructions/), these files apply globally to every task:

| File | Purpose |
|---|---|
| [`skills.instructions.md`](../.github/instructions/skills.instructions.md) | Explains the skills system and mandates the discover → load → implement workflow. **Read this first.** |
| [`mslearn.instructions.md`](../.github/instructions/mslearn.instructions.md) | Directs AI to use `microsoft_docs_search`, `microsoft_docs_fetch`, and `microsoft_code_sample_search` MCP tools when working with C#, .NET, ASP.NET Core, and other Microsoft technologies. These tools surface documentation that may be newer or more specific than training data. |

**Why they matter:** Instructions enforce project-wide rules that override default AI behavior. Without reading them, an AI contributor may search for test cases in the wrong domain, duplicate existing infrastructure, or miss the requirement to validate against live Microsoft documentation.

### Skills — Domain-Specific Expert Guidance

Located in [`.github/skills/`](../.github/skills/), each subdirectory is a skill with a `SKILL.md` entry point. Skills use **progressive disclosure** — load only what you need:

| Layer | What it contains | When to load |
|---|---|---|
| `SKILL.md` | Mandatory workflow, domain classification rules, component inventory | Always, before starting any task in that domain |
| `references/<topic>.md` | Deep protocol-specific patterns, base classes, code examples | On-demand, once domain is classified |

#### Available Skills

| Skill | Trigger Keywords | What it provides |
|---|---|---|
| [`file-server`](../.github/skills/file-server/SKILL.md) | FileServer, SMB, SMB2, SMB3, CIFS, MS-SMB2, MS-FSCC, MS-FSA, MS-DFSC, MS-FSRVP, MS-RSVD, MS-SQOS, file sharing | **Critical domain classification rules** (FSA vs SMB2 vs DFSC etc.), reusable adapter/base class inventory, test templates, test execution patterns. Covers 10 sub-domains via `references/`. |
| [`generate-protocol-diff`](../.github/skills/generate-protocol-diff/SKILL.md) | Protocol diff, spec comparison, protocol version changes | Guidance for diffing protocol specification versions to identify changes requiring new or updated test cases. |

#### How to Use a Skill

```
1. List .github/skills/ to discover available skills
2. Read .github/skills/<skill-name>/SKILL.md
3. Follow the mandatory workflow in SKILL.md — do not skip steps
4. Load references/<topic>.md only for the classified sub-domain
```

> **Why skills exist:** Protocol test suites have subtle domain boundaries. For example, a scenario from an MS-SMB2 document section may belong to the FSA test domain (not SMB2) if the concepts involve `File*` or `Fs*` structures from MS-FSCC. Without the `file-server` skill's classification rules, an AI will place tests in the wrong folder, duplicate existing code, and break test execution. Skills encode lessons learned from real contribution mistakes.

## Repository Root

`c:\Users\jomitiran\source\repos\WindowsProtocolTestSuites`

## Key Entry Points

| What you want | Where to look |
|---|---|
| SMB2 protocol logic | `ProtoSDK/MS-SMB2/` |
| FileServer test cases | `TestSuites/FileServer/src/SMB2/TestSuite/` |
| RDP protocol logic | `ProtoSDK/MS-RDPBCGR/`, `ProtoSDK/MS-RDPEGFX/`, etc. |
| RDP test cases | `TestSuites/RDP/Client/src/TestSuite/`, `TestSuites/RDP/Server/src/TestSuite/` |
| Kerberos logic | `ProtoSDK/KerberosLib/` |
| Kerberos test cases | `TestSuites/Kerberos/src/TestSuite/` |
| SMBD (RDMA) logic | `ProtoSDK/MS-SMBD/`, `ProtoSDK/RDMA/`, `ProtoSDK/RdmaLinux/` |
| PTM web service | `ProtocolTestManager/PTMService/PTMService/` |
| PTM kernel logic | `ProtocolTestManager/PTMService/PTMKernelService/` |
| Environment scripts | `CommonScripts/` |
| Prerequisite installer | `InstallPrerequisites/` |
| CI/CD pipeline definitions | `pipelines/` |
