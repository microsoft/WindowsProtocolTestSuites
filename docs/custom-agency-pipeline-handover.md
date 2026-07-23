# Handover: Build a Custom Agency Compute Pipeline for WindowsProtocolTestSuites

**Audience:** A Claude Code session running **with the `WindowsProtocolTestSuites` repository open** (org `microsoft`, project `WindowsProtocolTestSuites`).
**Author of this handover:** A session running in the orchestration repo `Code Generation WPTS` (the daily AI-codegen driver). That session has Eng Hub MCP access; **you (the receiving session) probably do not**, so every fact you need from the Eng Hub docs is embedded verbatim/summarized below. You should not need to fetch anything to execute this.

**Status:** Plan validated with the human. Decisions are locked. The original go/no-go blocker is **resolved**: the team is going with **Way A only — work-item assignment / `agency remote create`** — which the docs confirm honors `agency-preferences.yml`. Two small "get-the-right-value" items remain before merge (see §6); neither blocks design. Draft artifacts are in §7 and contain `TODO` markers tied to those items.

---

## 0. VERIFICATION LOG — receiving session (WPTS repo), 2026-06-18

> Added by the WPTS-repo session that executed this handover. Items below are **verified against a real Agency run** unless marked *untested*.

**Built & registered**
- Pipeline definition **195772** — "Agency Custom Compute" — in ADO folder `\WindowsProtocolTestSuites\Code Generation`, YAML `.azuredevops/pipelines/agency-custom-compute.yml`, on branch `ai-work/agency-custom-compute` (5 commits; **not yet merged to `main`**).
- `.azuredevops/policies/agency-preferences.yml`: `projectId: b67fd756-1c65-48ef-9824-eeb3cb9b2728`, `pipelineId: 195772`, `pipelineTrialMode: true`.
- 3 queue-time UI variables set (override on): `Agency.Consent.WindowsProtocolTestSuites=true`, `Agency_Context` (empty), `system.connection.accessTokenScope=vso.agentpools vso.build_execute vso.code vso.packaging`.

**Open items resolved**
- **§6.1** project GUID = `b67fd756-1c65-48ef-9824-eeb3cb9b2728` (ADO REST projects API).
- **§6.2** — clarified by the `agency` CLI itself: for **`agency remote create`, compute is selected by FLAGS, not by `agency-preferences.yml`**. A bare `create` uses *default* compute. Force the custom pipeline with `--pipeline-id 195772 --pipeline-ref ai-work/agency-custom-compute --pipeline-trial-mode false` (v1 `prjob`). The v2 `create job --compute pipeline --pipeline-id` path currently returns **500** on validate — avoid. `agency-preferences.yml` governs only the **work-item-assignment** path (see §0 "work-item" below).
- **§6.3** — runtime field is **`Agency_Context.WorkingBranch`** (PascalCase; e.g. `copilot/api-…`). NOT `azDoContext.sourceBranchName` (that was a CLI-validate echo with a different shape). **However**, the verified run shows the `Agency@3` task checks out / commits to the working branch *itself* — our custom clone landed on `main` and the PR was still correct. So the §6.3 checkout is **optional** (belt-and-suspenders for retries/iterations). If wired: `$branch = ($env:AGENCY_CONTEXT | ConvertFrom-Json).WorkingBranch; git -C $dst checkout $branch`.
- **Consent variable value** = `"true"` (Eng Hub "Customizing Agency Compute" + run).

**End-to-end run — PASSED** (build `150080074`, def 195772, branch `ai-work/agency-custom-compute`):
`Checkout` **skipped** (skipSourceSync honored) → `Custom clone` **succeeded** → `Run Agency` **succeeded` → draft PR created with the exact requested change. Managed pool `Agency-Prod-Default` acquired, **no deadlock, no consent block**.

**Untested — work-item-assignment path** (the eventual daily-driver end state): requires `agency-preferences.yml` **merged to `main`** (work-item path reads prefs from the repo's branch, not from `--pipeline-ref`), then on the work item: link the repo (artifact link **or** tag `copilot:repo=microsoft/WindowsProtocolTestSuites/WindowsProtocolTestSuites@main`), **assign "GitHub Copilot"**, and — while `pipelineTrialMode: true` — also tag **`agency:pipelineTrialMode=true`** (else trial mode falls back to default compute). The work-item UI experience is noted as private-preview/limited in Eng Hub.

---

## 1. TL;DR — what you are building and why

The default Agency (ADO SWE Agent) managed compute is **failing at the repository clone step** when it runs against `WindowsProtocolTestSuites`. The product team is investigating the root cause, but we are not waiting on them. Instead we are standing up a **customer-owned ("custom compute") Agency pipeline** that takes over the environment — most importantly, it performs its **own clone** of the repo using an identity we control, so the agent run no longer depends on the failing built-in sync.

Concretely you will:

1. Add a **custom Agency pipeline YAML** to the `WindowsProtocolTestSuites` repo that `extends` the 1ES Agency pipeline template, runs on the **Agency-managed pool**, sets `skipSourceSync: true`, and clones the repo itself in `preAgentSteps`.
2. Add **`.azuredevops/policies/agency-preferences.yml`** to the same repo so Agency knows to use your pipeline instead of default compute.
3. Register the pipeline in ADO and set three **queue-time variables in the pipeline UI** (not in YAML).
4. Validate end-to-end in **trial mode** (so it only affects opted-in work items), then roll out.

> **Invocation path is decided: Way A — work-item assignment / `agency remote create`.** The docs confirm this flow reads the target repo's `agency-preferences.yml` and routes to your custom pipeline (with `pipelineTrialMode` honored). So authoring the files described here **is sufficient** for the custom pipeline to be selected — there is no REST-API uncertainty to resolve. (The current driver in the orchestration repo still uses the REST API and is being migrated to Way A as a *separate* workstream; that migration does not affect the work in this doc.)

---

## 2. System context (how the current automation works)

There are **two repositories** and they are easy to confuse:

| Role | Repo | Org / Project / Repo | URL |
|---|---|---|---|
| **Orchestration / driver** (where this handover was written) | `Code Generation WPTS` | `microsoft` / `WindowsProtocolTestSuites` / `Code Generation WPTS` | `https://dev.azure.com/microsoft/WindowsProtocolTestSuites/_git/Code%20Generation%20WPTS` |
| **Target — the repo Agency operates on** (where YOU work) | `WindowsProtocolTestSuites` | `microsoft` / `WindowsProtocolTestSuites` / `WindowsProtocolTestSuites` | `https://dev.azure.com/microsoft/WindowsProtocolTestSuites/_git/WindowsProtocolTestSuites` |

Other coordinates worth knowing:
- **Work items** live in a *different* project: `microsoft` / **`OS`** (area path `OS\ImPaCT\TEC\Data and Protocols`). The code repo is in project `WindowsProtocolTestSuites`. This split is intentional and normal in the `microsoft` org.
- `dev.azure.com/microsoft` and `microsoft.visualstudio.com` are **the same organization** (`microsoft`), just modern vs. legacy URL forms. There is **no cross-org boundary** anywhere in this system. (An earlier suggestion floated a "cross-org PAT" — it was based on a misreading; ignore it.)

### Current flow (in the orchestration repo)
The driver pipeline `.azuredevops/pipelines/wpts-ai-codegen.yml` runs daily (8am PST), and for each pending work item, `scripts/process-work-items.ps1`:
1. Creates branch `ai-work/{workItemId}` from `main` in the WPTS repo (via the Refs REST API — no clone).
2. Sets the work item state to `Started`.
3. **Calls the Agency REST API** `POST {agencyApiUrl}/jobs` to do the implementation + PR.
4. Polls `GET {agencyApiUrl}/jobs/{jobId}` until done.
5. Triggers the test pipeline and posts results to the PR.
6. On Agency API failure, falls back to assigning the work item to **GitHub Copilot**.

> **Decision — invocation is Way A only.** The flow above (REST `POST /jobs`) is the *current* implementation, which is **Way B**. The team has decided to move to **Way A — work-item assignment / `agency remote create`** — because Way A is the documented path that honors `agency-preferences.yml` custom-compute selection. Migrating the driver from Way B → Way A is a **separate workstream in the orchestration repo** and is *not* part of this handover. For your purposes: invocation will be Way A, so the custom pipeline you author here **will** be selected.

Key endpoint and identity facts (from the driver):
- Agency API base: `https://copilotswe.app.prod.gitops.startclean.microsoft.com/api/agency`
- Agency API auth: Entra token via `az account get-access-token --resource api://81bbac67-d541-4a6d-a48b-b1c0f9a57888` (tenant `72f988bf-86f1-41af-91ab-2d7cd011db47`), service connection `AgencyWPTS`, SP client id from variable group `CodeGenerationGroup` (`ServicePrincipal`).
- The **job request** Agency receives (shape that matters for you):
  ```
  organization      = microsoft
  project           = WindowsProtocolTestSuites
  repository        = WindowsProtocolTestSuites
  sourceBranch      = ai-work/{workItemId}     # the working branch, pre-created
  targetBranch      = main
  createPullRequest = true / publishPullRequest = true
  ```
  → **Agency operates on `WindowsProtocolTestSuites` and works on branch `ai-work/{id}`.** Your custom clone must end up checked out on that branch (see §6.3 / §7).

**The human confirmed:** the pipeline service principal / build identity "has access to all repos already," so cloning the WPTS repo from inside the pipeline with `System.AccessToken` is expected to work.

---

## 3. Authoritative references (Eng Hub)

These are the source-of-truth docs. Owners can answer the open questions in §6.

- **Pipeline Template Parameters** — every parameter of `1ES.Agency.PipelineTemplate.yml`:
  `https://eng.ms/docs/coreai/devdiv/one-engineering-system-1es/1es-jacekcz/startrightgitops/ado-swe-agent/agency/azuredevops/pipeline-template`
- **Customizing Agency Compute** — the setup walkthrough:
  `https://eng.ms/docs/coreai/devdiv/one-engineering-system-1es/1es-jacekcz/startrightgitops/ado-swe-agent/agency/azuredevops/running-agency`
- **Doc owners / escalation:** dradboia@, snehatuli@, shkumari@, hagoel@, kewilli@, ajeyaraj@, prmcdonald@ (all @microsoft.com)
- Feature status: **Public Preview** — "ready for use but has some rough edges … minor breaking changes may occur to the pipeline execution model." Pin template refs and expect churn.

---

## 4. Hard constraints (from the docs — do not violate)

1. **The pipeline YAML MUST live in the repo Agency operates on** (`WindowsProtocolTestSuites`). The docs state this twice as *Important*: "Do not define the pipeline in a separate repository. Agency resolves the pipeline definition from your repository's branch, so the pipeline definition must be co-located with your code." → This is why the work goes in the WPTS repo and **not** in the orchestration repo.
2. **`agency-preferences.yml` MUST be at `.azuredevops/policies/agency-preferences.yml`** in that same repo.
3. **The three pipeline variables must be set in the ADO pipeline UI as queue-time-settable — NEVER in YAML.** If they're in YAML, "Agency will fail to invoke the pipeline."
4. **Pool must not be your PR-policy pool.** Reusing the same pool that runs PR validation can **deadlock** (Agency holds an agent waiting on a PR build that's waiting for an agent from the same exhausted pool). The orchestration driver currently uses `TestSuiteBuildESPoolTME-CentralUS`; do not point the Agency pipeline at that pool. (We avoid the issue entirely by using the Agency-managed pool — see §5.)
5. **Running `agency copilot …` CLI directly in your own pipeline is explicitly NOT supported.** Custom compute means you control the *environment*; Agency still drives the agent via its task. Don't try to invoke the agent yourself.
6. **A BYO pool, if ever used, must be stateless** ("Fresh agent every time") and use a `1ES PT - Starter` / `1ESPT MMS` image. (Not needed for our chosen approach.)

### Documented execution order of the template
The template produces one stage / one job with steps in this exact order:
1. **Update Build Number** — internal; parses `Agency_Context`, updates the build number.
2. **`checkout: none`** — always runs, to suppress ADO's default checkout (even when `skipSourceSync: false`).
3. **Clone/Sync repo** — clones/syncs the repo into `agentContextRoot` using `System.AccessToken`. **Skipped when `skipSourceSync: true`.**
4. **`preAgentSteps`** — your custom setup steps. ← our custom clone goes here.
5. **Agency task** — runs the agent (`repoRoot = agentContextRoot`).
6. **`postAgentSteps`** — your custom cleanup steps.
7. **Publish AgencyArtifact** — uploads the log dir; runs with `condition: always()`.

---

## 5. Decisions already made (locked with the human)

| # | Decision | Rationale |
|---|---|---|
| D1 | **Author in the `WindowsProtocolTestSuites` repo.** | Required by constraint §4.1. Human confirmed they have write access. |
| D2 | **Pool = `useAgencyManagedPool: true`, `os: windows`.** | The doc's **recommended Option A**: shared `Agency-Prod-Default` pool, no provisioning, no per-project pool authorization, and no deadlock risk with PR policies. (Caveat: throttled to ~20 concurrent jobs/user — fine for the daily batch. If that ceiling becomes a problem, switch to a dedicated stateless BYO pool — *not* the PR-policy pool.) `windows` matches the driver's current `windows-latest`. |
| D3 | **`skipSourceSync: true` + custom clone in `preAgentSteps`, authenticating with `System.AccessToken`.** | The default clone is failing and the product team is still investigating; making the pipeline self-sufficient for cloning decouples us from that. Build identity already has repo access. If the product team later fixes built-in sync, simply remove `skipSourceSync` and the custom step. |

---

## 6. OPEN ITEMS — resolve these before merge

### 6.1 Get the project GUID for `agency-preferences.yml`
`pipelineConfig.projectId` needs the **GUID** of the `WindowsProtocolTestSuites` project (not the name).

Resolve with either:
```bash
# REST (needs a bearer token with vso.project scope; System.AccessToken works in-pipeline)
GET https://dev.azure.com/microsoft/_apis/projects/WindowsProtocolTestSuites?api-version=7.0
# → use the ".id" field
```
or
```bash
az devops project show --project WindowsProtocolTestSuites --org https://dev.azure.com/microsoft --query id -o tsv
```

### 6.2 ✅ RESOLVED — invocation is Way A, which honors `agency-preferences.yml`
The original open question was whether the **REST API** `POST /jobs` path reads `agency-preferences.yml`. **That is now moot: the team has decided to use Way A only** (work-item assignment / `agency remote create`). The Customizing Agency Compute doc states for Way A that *"Agency reads the `agency-preferences.yml` configuration from the repository"* and, when `pipelineTrialMode` is off, *"Agency always invokes your custom pipeline."*

- **Implication for you (WPTS-repo session):** none — authoring the two files (§7) is sufficient for the custom pipeline to be selected.
- **Implication for the orchestration repo:** the driver currently uses the REST API (Way B) and must be migrated to Way A; that is tracked separately and is **out of scope** for this handover.

### 6.3 `Agency_Context` schema — the working-branch field name
With `skipSourceSync: true`, you skip the built-in Clone/Sync step, which normally **clones *and* checks out the working branch** (`ai-work/{id}`). Your custom clone must replicate that checkout. The working branch is inside the `Agency_Context` JSON (doc: "Contains a JSON payload with the problem context, **working branch**, and API base URL"), but the **exact field name is not documented here.**

**Action:** Get the `Agency_Context` JSON shape from the Agency team or by inspecting a real run's logged value (it's populated at runtime), then fix the `checkout` line in the `preAgentSteps` script (the `TODO` in §7). Until confirmed, do **not** rely on a guessed field name.

---

## 7. Draft artifacts (adapt, then commit on a branch off `main`)

> Git workflow: **branch from `main`** for these changes (e.g. `users/<alias>/agency-custom-compute`); do not commit straight to `main`.

### 7.1 `.azuredevops/policies/agency-preferences.yml`
```yaml
# Tells Agency to use our customer-owned pipeline for compute when operating on this repo.
# metadata
name: Agency custom pipeline configuration
description: Configure Agency to use customer-owned pipeline for compute

# filters
resource: repository

# configuration
configuration:
  agencyPreferences:
    pipelineConfig:
      organization: microsoft
      projectId: <PROJECT_GUID>          # OPEN ITEM 6.1
      pipelineId: <PIPELINE_ID>          # from §8 step 2 (the registered definition id)
      pipelineTrialMode: true            # SAFE ROLLOUT: only work items tagged
                                         # agency:pipelineTrialMode=true use this pipeline.
                                         # Remove (or set false) after validation.
```

### 7.2 Custom pipeline YAML (suggested path: `.azuredevops/pipelines/agency-custom-compute.yml`)
```yaml
# Custom Agency compute pipeline for WindowsProtocolTestSuites.
# Lives in the repo Agency operates on (required). Selected via
# .azuredevops/policies/agency-preferences.yml in this same repo.

trigger: none            # Agency triggers this pipeline; no CI trigger.

resources:
  repositories:
    - repository: 1esPipelines
      type: git
      name: 1ESPipelineTemplates/1ESPipelineTemplates
      ref: refs/tags/canary        # pin to a stable tag once validated (canary == latest/unstable)

extends:
  template: v1/1ES.Agency.PipelineTemplate.yml@1esPipelines
  parameters:
    pool:
      useAgencyManagedPool: true   # D2: shared Agency-Prod-Default pool (recommended)
      os: windows

    agentContextRoot: $(Build.SourcesDirectory)   # working tree the agent operates on

    skipSourceSync: true           # D3: we clone the repo ourselves below

    preAgentSteps:
      - pwsh: |
          $ErrorActionPreference = 'Stop'
          $dst = "$(Build.SourcesDirectory)"

          # checkout: none has already run, so $dst should be empty (git clone requires an empty target).
          $repo = "https://dev.azure.com/microsoft/WindowsProtocolTestSuites/_git/WindowsProtocolTestSuites"

          Write-Host "Cloning $repo into $dst"
          # Authenticate with the build identity via an Authorization header.
          # Do NOT embed a PAT in the URL (it can leak into logs).
          git -c http.extraHeader="AUTHORIZATION: bearer $(System.AccessToken)" clone $repo $dst
          if ($LASTEXITCODE -ne 0) { throw "git clone failed with exit code $LASTEXITCODE" }

          # OPEN ITEM 6.3: check out Agency's working branch (ai-work/{id}).
          # Replace <workingBranchField> with the confirmed field name from Agency_Context.
          # $ctx = $env:AGENCY_CONTEXT | ConvertFrom-Json
          # $branch = $ctx.<workingBranchField>
          # Write-Host "Checking out working branch: $branch"
          # git -C $dst checkout $branch
          # if ($LASTEXITCODE -ne 0) { throw "git checkout '$branch' failed" }

          # OPTIONAL: large-repo mitigations if clone proves slow/unstable
          #   - shallow:    git ... clone --depth 1 --no-single-branch $repo $dst
          #   - partial:    git ... clone --filter=blob:none $repo $dst
          #   - LFS:        git -C $dst lfs pull        (only if the repo uses LFS)
          #   - submodules: git -C $dst submodule update --init --recursive
        displayName: 'Custom clone (WindowsProtocolTestSuites)'
        env:
          SYSTEM_ACCESSTOKEN: $(System.AccessToken)
          AGENCY_CONTEXT: $(Agency_Context)

    # postAgentSteps: []           # add cleanup/notifications here if needed later

    # If 1ES network isolation is enforced and the agent or clone must reach
    # additional hosts, allow them here (dev.azure.com internal traffic is normally fine):
    # settings:
    #   networkIsolationAdditionalDomainAllowList:
    #     - <host>
```

---

## 8. Step-by-step implementation checklist

**Phase 0 — Resolve open items**
- [ ] §6.1 Get the `WindowsProtocolTestSuites` **project GUID**.
- [ ] §6.2 ✅ Resolved — Way A (work-item assignment) honors `agency-preferences.yml`; no action needed for the WPTS-repo work. (Driver migration to Way A is tracked in the orchestration repo.)
- [ ] §6.3 Confirm the **`Agency_Context` working-branch field name**.
- [ ] Confirm `1ESPipelineTemplates` repo is accessible in the `microsoft` org (it is, org-wide — sanity check only).

**Phase 1 — Author files (branch off `main` in the WPTS repo)**
- [ ] Add `.azuredevops/pipelines/agency-custom-compute.yml` (§7.2), with the §6.3 checkout line filled in.
- [ ] Add `.azuredevops/policies/agency-preferences.yml` (§7.1) with `pipelineTrialMode: true` and the real `projectId`. Leave `pipelineId` as a placeholder until Phase 2, then update.

**Phase 2 — Register pipeline + UI variables**
- [ ] Create a new pipeline **definition** in `microsoft/WindowsProtocolTestSuites` pointing at `agency-custom-compute.yml`. Record the **`pipelineId`** (a.k.a. `definitionId` — in the pipeline URL `…/_build?definitionId={id}`). Put it into `agency-preferences.yml`.
- [ ] In the pipeline UI → **Variables**, add each of these and check **"Let users override this value when running this pipeline"** (queue-time settable). **Do not put them in YAML:**
  - `Agency.Consent.WindowsProtocolTestSuites`  → authorizes Agency to invoke the pipeline for this repo.
  - `Agency_Context`  → Agency populates at runtime (JSON: problem context, working branch, API base URL).
  - `system.connection.accessTokenScope`  → set to the default scopes `vso.agentpools vso.build_execute vso.code vso.packaging` (must include `vso.code` for the clone).
- [ ] First run on the managed pool may prompt for **pool consent** — approve if asked.
- [ ] Sanity check the build service identity has **Read** on the repo (same project, so normally yes).

**Phase 3 — Validate in trial mode**
- [ ] Tag a **test work item** with `agency:pipelineTrialMode=true`.
- [ ] Invoke Agency for that work item (via the driver, or `agency remote create … --pipeline-ref <your-branch>` to test the pipeline branch before merge).
- [ ] Confirm the run executes steps in the documented order (§4), the **custom clone succeeds**, the agent runs, and a **PR is opened on WPTS** from `ai-work/{id}` → `main`.
- [ ] Inspect the **AgencyArtifact** logs (published on `always()`).
- [ ] Confirm **no pool deadlock** with PR-policy builds.

**Phase 4 — Roll out**
- [ ] Merge the branch to `main` (PR).
- [ ] Once confident, remove `pipelineTrialMode` (or set `false`) so **all** work items use the custom pipeline.
- [ ] Coordinate with the orchestration-repo owner: the driver must invoke via **Way A** (work-item assignment / `agency remote create`) for `agency-preferences.yml` to apply. Confirm that migration is in place so production (non-trial) work items route through the custom pipeline.

---

## 9. Gotchas / pitfalls

- **`git clone` needs an empty target.** `checkout: none` (always run by the template) should leave `$(Build.SourcesDirectory)` empty, so cloning into it works. If anything pre-populates it, the clone fails — clone into a temp dir and move, or `git init` + `fetch` instead.
- **Never embed a PAT/token in the clone URL.** Use `git -c http.extraHeader="AUTHORIZATION: bearer $(System.AccessToken)"`. ADO masks secret vars in logs, but URL-embedded creds are an anti-pattern and can still leak via error output.
- **Prefer `System.AccessToken` over a PAT.** A PAT is a long-lived secret; 1ES/Agency favor identity-based auth. The build identity already has access — don't reintroduce a PAT unless §6.2's resolution forces it.
- **Don't author in the orchestration repo.** It will not be picked up (constraint §4.1). Everything here goes in `WindowsProtocolTestSuites`.
- **Variables in YAML break invocation.** The three Phase-2 variables must be UI/queue-time only.
- **Pool deadlock.** Don't point this at the PR-policy pool. Managed pool avoids it.
- **Template ref churn.** `refs/tags/canary` tracks the latest (possibly unstable) template. Pin to a stable tag after validation; re-test on template bumps (Public Preview).
- **`skipSourceSync` ⇒ you own the branch checkout.** It's not just the clone — the built-in step you skipped also did the working-branch checkout. Missing that (§6.3) means the agent runs against the wrong ref.

---

## 10. Definition of done

- Custom pipeline YAML + `agency-preferences.yml` merged to `main` in `WindowsProtocolTestSuites`.
- A trial-mode work item produced a successful end-to-end run: custom clone → agent → PR on WPTS, with no deadlock and logs published.
- Driver invokes via Way A (work-item assignment / `agency remote create`), so production (non-trial) work items route through the custom pipeline.
- `pipelineTrialMode` removed (or `false`) for full rollout.

---

## 11. Quick reference — parameters used (from the Pipeline Template Parameters doc)

| Parameter | Type / default | Meaning |
|---|---|---|
| `agentContextRoot` | string (none) | Working tree for the agent; passed as `repoRoot` to the Agency task and `AGENCY_CONTEXTROOT` to the sync step. Typically `$(Build.SourcesDirectory)`. |
| `preAgentSteps` | stepList `[]` | Steps injected **after** source sync, **before** the Agency task. Our custom clone lives here. |
| `postAgentSteps` | stepList `[]` | Steps after the Agency task (cleanup/notifications). |
| `skipSourceSync` | boolean `false` | When `true`, skips the built-in Clone/Sync step. Use when you handle checkout yourself or need a custom strategy. `checkout: none` still always runs first. |
| `agentStep` | object `{ verbosity: '' }` | Agency task config. `verbosity` (e.g. `debug`, `agency=trace`) replaces deprecated `logLevel`; `timeoutInMinutes` default `120`. |
| `logPath` | string `''` | Agency log dir; defaults to `$(Build.StagingDirectory)/agency-logs`; source for the AgencyArtifact; always prepended to `globalOptions` as `--log-dir`. |
| `mcpConfiguration` | object `{}` | Per-MCP `serviceConnection` map (WIF auth), mapped to indexed task inputs (max 50). Key must exactly match the MCP name in agent config or auth silently fails. Not needed unless you add MCP servers. |
| `pool` | object `{}` | `useAgencyManagedPool: true` + `os` (our choice), **or** `name`/`image`/`demands` for BYO (mutually exclusive with `useAgencyManagedPool`). |
| `settings` | object `{}` | Passed to `1ES.Unofficial.PipelineTemplate.yml`. `networkIsolationMode` (default `Enforce`), `networkIsolationPolicy`, `networkIsolationAdditionalDomainAllowList`. |
| `stageName` | string `'AgencyStage'` | Stage name override. |
| `customBuildTags` | object `[]` | Extra build tags (`1ES.PT.Agency` is always added). |

### `agency-preferences.yml` config keys
| Key | Meaning |
|---|---|
| `organization` | ADO org name (`microsoft`). |
| `projectId` | **GUID** of the project (not name) — §6.1. |
| `pipelineId` | The pipeline definition id (a.k.a. `definitionId`) to invoke. Its YAML must be in the repo Agency operates on. |
| `pipelineTrialMode` | Optional; `true` ⇒ pipeline used only for work items tagged `agency:pipelineTrialMode=true`. Defaults `false`. |

### Testing a pipeline branch before merge
`agency remote create --org microsoft --project <proj> --repo WindowsProtocolTestSuites --prompt "<task>" --pipeline-ref <your-branch>` runs Agency against your pipeline **branch** instead of the default branch — use it to validate before merging.
