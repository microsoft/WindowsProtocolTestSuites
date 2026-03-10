# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Picks one ADO work item, invokes GitHub Copilot CLI, then creates a branch and PR.

.DESCRIPTION
    1. Queries Azure DevOps for committed work items assigned to the current user.
    2. Picks the first work item (oldest by ID):
       - Creates a feature branch from the target branch (or reuses existing).
       - Runs Copilot CLI non-interactively with the work item content as the prompt,
         including an instruction to use the relevant repository skill.
       - Commits changes, pushes the branch, and creates a PR (or updates existing).
    3. Marks the work item as Completed in ADO.

    Designed to be called once per pipeline run. Each run processes exactly one
    work item, marks it complete, then exits. The next pipeline run picks the next item.

    Authentication uses Azure CLI credentials (run 'az login' before using this script).

.PARAMETER Organization
    Azure DevOps organization URL. Auto-detected from git remote if not specified.

.PARAMETER Project
    Azure DevOps project name. Auto-detected from git remote if not specified.

.PARAMETER TargetBranch
    The branch PRs will target. Default: main.

.PARAMETER AreaPath
    Azure DevOps area path to filter work items. Default: OS\ImPaCT\TEC\Data and Protocols.

.PARAMETER Skill
    Copilot skill to invoke. If empty, auto-detected from .github/skills/.

.PARAMETER CompletedState
    The ADO work item state to set after successful processing. Default: Completed.

.PARAMETER DryRun
    Preview what would be done without making changes.

.EXAMPLE
    .\Invoke-CopilotFromAdo.ps1
    .\Invoke-CopilotFromAdo.ps1 -TargetBranch develop -Skill file-server
    .\Invoke-CopilotFromAdo.ps1 -CompletedState Resolved
    .\Invoke-CopilotFromAdo.ps1 -DryRun
#>

[CmdletBinding()]
param(
    [string]$Organization,
    [string]$Project,
    [string]$TargetBranch = "main",
    [string]$AreaPath = "OS\ImPaCT\TEC\Data and Protocols",
    [string]$Skill,
    [string]$CompletedState = "Completed",
    [switch]$DryRun
)

Set-StrictMode -Version Latest
# Use "Continue" so PS 5.1 does not treat native-command stderr (e.g. az CLI
# warnings) as terminating errors. All error handling is explicit via
# $LASTEXITCODE checks and throw statements.
$ErrorActionPreference = "Continue"

# -----------------------------------------------------------------------
# Helpers
# -----------------------------------------------------------------------

function Write-Step    { param([string]$Msg) Write-Host "`n> $Msg" -ForegroundColor Cyan }
function Write-Ok      { param([string]$Msg) Write-Host "  [OK] $Msg" -ForegroundColor Green }
function Write-Warn    { param([string]$Msg) Write-Host "  [WARN] $Msg" -ForegroundColor Yellow }
function Write-Fail    { param([string]$Msg) Write-Host "  [FAIL] $Msg" -ForegroundColor Red }

function Install-PrerequisiteIfMissing {
    param(
        [string]$Command,
        [string]$WingetId,
        [string]$DisplayName
    )

    if (Get-Command $Command -ErrorAction SilentlyContinue) {
        Write-Ok "$DisplayName found"
        return
    }

    Write-Warn "$DisplayName not found - installing via winget..."

    if (-not (Get-Command winget -ErrorAction SilentlyContinue)) {
        throw "winget is required to auto-install $DisplayName but was not found. " +
              "Install $DisplayName manually, or install winget from https://aka.ms/getwinget"
    }

    winget install --id $WingetId --accept-source-agreements --accept-package-agreements --silent 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to install $DisplayName via 'winget install --id $WingetId'. Install it manually."
    }

    # Refresh PATH so the newly installed binary is discoverable in this session
    $machinePath = [Environment]::GetEnvironmentVariable("Path", "Machine")
    $userPath    = [Environment]::GetEnvironmentVariable("Path", "User")
    $env:Path    = "$machinePath;$userPath"

    if (-not (Get-Command $Command -ErrorAction SilentlyContinue)) {
        throw "$DisplayName was installed but '$Command' is still not on PATH. Restart your terminal and try again."
    }
    Write-Ok "$DisplayName installed successfully"
}

function Resolve-AdoFromRemote {
    $remoteUrl = git --no-pager remote get-url origin 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to get git remote URL. Ensure you are inside a git repository."
    }

    # https://dev.azure.com/{org}/{project}/_git/{repo}
    if ($remoteUrl -match 'https://dev\.azure\.com/(?<org>[^/]+)/(?<project>[^/]+)/_git/(?<repo>[^/\s]+)') {
        return @{
            Organization = "https://dev.azure.com/$($Matches.org)"
            Project      = $Matches.project
            Repository   = $Matches.repo
        }
    }
    # https://{org}.visualstudio.com/{collection}/{project}/_git/{repo}
    elseif ($remoteUrl -match 'https://(?<org>[^.]+)\.visualstudio\.com/(?<coll>[^/]*)/(?<project>[^/]+)/_git/(?<repo>[^/\s]+)') {
        return @{
            Organization = "https://$($Matches.org).visualstudio.com/$($Matches.coll)"
            Project      = $Matches.project
            Repository   = $Matches.repo
        }
    }
    else {
        throw "Could not parse ADO details from remote URL: $remoteUrl"
    }
}

function Get-AvailableSkills {
    $root = git rev-parse --show-toplevel 2>$null
    if (-not $root) { return @() }
    $dir = Join-Path (Join-Path $root ".github") "skills"
    if (Test-Path $dir) {
        return @(Get-ChildItem -Path $dir -Directory | Select-Object -ExpandProperty Name)
    }
    return @()
}

function Get-ActiveWorkItems {
    param([string]$Org, [string]$Proj, [string]$Area)

    $where = "[System.State] = 'Committed' AND [System.Tags] CONTAINS 'WPTS AI Task'"
    if ($Area) {
        $where += " AND [System.AreaPath] UNDER '$Area'"
    }

    $wiql = "SELECT [System.Id], [System.Title], [System.WorkItemType] " +
            "FROM WorkItems " +
            "WHERE $where " +
            "ORDER BY [System.Id] ASC"

    $json = az boards query --wiql $wiql --org $Org --project $Proj --output json 2>$null
    if ($LASTEXITCODE -ne 0) {
        $err = az boards query --wiql $wiql --org $Org --project $Proj 2>&1
        throw "Work item query failed: $err"
    }

    if (-not $json) { return @() }
    return ($json | ConvertFrom-Json)
}

function Set-WorkItemState {
    param([int]$Id, [string]$Org, [string]$State)

    $json = az boards work-item update --id $Id --state $State --org $Org --output json 2>$null
    if ($LASTEXITCODE -ne 0) {
        $err = az boards work-item update --id $Id --state $State --org $Org 2>&1
        throw "Failed to update work item #$Id to state '$State': $err"
    }
    return ($json | ConvertFrom-Json)
}

function Get-WorkItemDetail {
    param([int]$Id, [string]$Org)

    $json = az boards work-item show --id $Id --org $Org --output json 2>$null
    if ($LASTEXITCODE -ne 0) {
        $err = az boards work-item show --id $Id --org $Org 2>&1
        throw "Failed to fetch work item #$Id`: $err"
    }

    $wi = $json | ConvertFrom-Json
    $f  = $wi.fields

    $desc = if ($f.PSObject.Properties.Match('System.Description').Count) { $f.'System.Description' } else { $null }

    return @{
        Id           = $Id
        Title        = $f.'System.Title'
        Description  = $desc
        WorkItemType = $f.'System.WorkItemType'
        Tags         = $f.'System.Tags'
    }
}

function New-BranchName {
    param([hashtable]$WI)
    $slug = ($WI.Title -replace '[^a-zA-Z0-9]+', '-' -replace '^-|-$', '').ToLower()
    if ($slug.Length -gt 60) { $slug = $slug.Substring(0, 60).TrimEnd('-') }
    return "copilot/ado-$($WI.Id)-$slug"
}

function ConvertFrom-Html {
    param([string]$Html)
    if (-not $Html) { return "" }
    return ($Html -replace '<[^>]+>', ' ' -replace '\s+', ' ').Trim()
}

function Add-PrLabel {
    param([string]$Org, [string]$ProjectId, [string]$RepoId, [int]$PrId, [string]$Label)

    $token = az account get-access-token --resource "499b84ac-1321-427f-aa17-267ca6975798" --query accessToken -o tsv 2>$null
    if (-not $token) { return $false }

    $uri = "$Org/$ProjectId/_apis/git/repositories/$RepoId/pullRequests/$PrId/labels?api-version=7.1-preview.1"
    $body = @{ name = $Label } | ConvertTo-Json
    $headers = @{ Authorization = "Bearer $token"; "Content-Type" = "application/json" }

    try {
        $null = Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $body
        return $true
    }
    catch {
        return $false
    }
}

function Build-CopilotPrompt {
    param([hashtable]$WI, [string[]]$Skills)

    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.Add("Azure DevOps Work Item #$($WI.Id).")
    $lines.Add("Type: $($WI.WorkItemType).")
    $lines.Add("Title: $($WI.Title).")

    $desc = ConvertFrom-Html $WI.Description
    if ($desc) { $lines.Add("Description: $desc") }

    if ($WI.Tags) { $lines.Add("Tags: $($WI.Tags).") }

    if ($Skills.Count -gt 0) {
        $list = ($Skills | ForEach-Object { $_ }) -join ", "
        $lines.Add("Instructions: Implement the changes described in this work item. " +
                    "This repository has the following skills available: $list. " +
                    "Use the appropriate skill(s) to assist with the implementation. " +
                    "Invoke the skill first before making any changes. " +
                    "Use the microsoft-learn MCP tools (microsoft_docs_search, microsoft_docs_fetch, microsoft_code_sample_search) " +
                    "to look up official Microsoft protocol documentation for additional context on the protocol specifications. " +
                    "Use subagents (via the task tool) extensively for faster results: " +
                    "launch parallel explore agents to investigate multiple aspects of the codebase simultaneously, " +
                    "use task agents for builds and tests, and use general-purpose agents for complex multi-step implementation work. " +
                    "Prefer parallel subagent execution over sequential single-threaded work whenever possible.")
    }
    else {
        $lines.Add("Instructions: Implement the changes described in this work item. " +
                    "Use the microsoft-learn MCP tools (microsoft_docs_search, microsoft_docs_fetch, microsoft_code_sample_search) " +
                    "to look up official Microsoft protocol documentation for additional context on the protocol specifications. " +
                    "Use subagents (via the task tool) extensively for faster results: " +
                    "launch parallel explore agents to investigate multiple aspects of the codebase simultaneously, " +
                    "use task agents for builds and tests, and use general-purpose agents for complex multi-step implementation work. " +
                    "Prefer parallel subagent execution over sequential single-threaded work whenever possible.")
    }

    return $lines -join "`n"
}

# -----------------------------------------------------------------------
# Main
# -----------------------------------------------------------------------

Write-Host ""
Write-Host "========================================================" -ForegroundColor Magenta
Write-Host "  ADO Work Item -> Copilot CLI -> Branch & PR (1 item) " -ForegroundColor Magenta
Write-Host "========================================================" -ForegroundColor Magenta

# --- Resolve ADO details ------------------------------------------------
Write-Step "Resolving Azure DevOps details from git remote..."
$ado = Resolve-AdoFromRemote

if (-not $Organization) { $Organization = $ado.Organization }
if (-not $Project)      { $Project      = $ado.Project }
$RepoName = $ado.Repository

Write-Ok "Organization : $Organization"
Write-Ok "Project      : $Project"
Write-Ok "Repository   : $RepoName"

# --- Prerequisites: install if missing ------------------------------------
Write-Step "Checking prerequisites (auto-installing if missing)..."

Install-PrerequisiteIfMissing -Command "git"     -WingetId "Git.Git"         -DisplayName "Git"
Install-PrerequisiteIfMissing -Command "az"      -WingetId "Microsoft.AzureCLI" -DisplayName "Azure CLI"
Install-PrerequisiteIfMissing -Command "copilot" -WingetId "GitHub.Copilot"  -DisplayName "GitHub Copilot CLI"

# --- Azure CLI login -----------------------------------------------------
Write-Step "Verifying Azure CLI authentication..."
$azAccount = az account show --output json 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Warn "Not logged in - launching 'az login'..."
    az login 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "Azure CLI login failed. Run 'az login' manually." }
    $azAccount = az account show --output json 2>$null
}
Write-Ok "Azure CLI authenticated as $((($azAccount | ConvertFrom-Json).user).name)"

# --- azure-devops extension ----------------------------------------------
$exts = az extension list --output json 2>$null | ConvertFrom-Json
if (-not ($exts | Where-Object { $_.name -eq 'azure-devops' })) {
    Write-Warn "Installing azure-devops extension..."
    az extension add --name azure-devops --yes 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "Failed to install azure-devops extension." }
}
Write-Ok "azure-devops extension ready"

# --- Skills --------------------------------------------------------------
if ($Skill) {
    $skills = @($Skill)
}
else {
    $skills = Get-AvailableSkills
}
if ($skills.Count -gt 0) { Write-Ok "Skills: $($skills -join ', ')" }
else                      { Write-Warn "No skills detected" }

# --- Fetch work items ----------------------------------------------------
$workItems = Get-ActiveWorkItems -Org $Organization -Proj $Project -Area $AreaPath

if (-not $workItems -or $workItems.Count -eq 0) {
    Write-Ok "No committed work items assigned to you. Nothing to do!"
    exit 0
}

Write-Ok "Found $($workItems.Count) committed work item(s) - picking the first one"

# --- Process single work item --------------------------------------------
$wiRef = $workItems[0]
$wiId  = if ($wiRef.id) { $wiRef.id } else { $wiRef.fields.'System.Id' }

Write-Host "`n--------------------------------------------------------" -ForegroundColor DarkGray
Write-Step "Processing work item #$wiId"

$branch = $null
try {
    # 1 - Full details
    $wi = Get-WorkItemDetail -Id $wiId -Org $Organization
    Write-Ok "[$($wi.WorkItemType)] $($wi.Title)"

    if ($DryRun) {
        $prompt = Build-CopilotPrompt -WI $wi -Skills $skills
        Write-Warn "[DRY RUN] Prompt that would be sent to Copilot:"
        Write-Host $prompt -ForegroundColor DarkGray
        Write-Warn "[DRY RUN] Work item #$wiId would be marked as '$CompletedState' after processing."
        exit 0
    }

    # 2 - Ensure clean starting point on target branch
    Write-Step "Switching to $TargetBranch and pulling latest..."
    git checkout $TargetBranch  2>&1 | Out-Null
    git pull origin $TargetBranch 2>&1 | Out-Null
    Write-Ok "Up to date on $TargetBranch"

    # 3 - Create or reuse feature branch
    $branch = New-BranchName -WI $wi
    Write-Step "Preparing branch: $branch"

    $localExists  = git branch --list $branch 2>&1
    $remoteExists = git ls-remote --heads origin $branch 2>&1

    if ($localExists) {
        git checkout $branch 2>&1 | Out-Null
        Write-Ok "Switched to existing local branch"
    }
    elseif ($remoteExists) {
        git checkout -b $branch "origin/$branch" 2>&1 | Out-Null
        Write-Ok "Checked out existing remote branch"
    }
    else {
        git checkout -b $branch 2>&1 | Out-Null
        Write-Ok "Created new branch"
    }

    # 4 - Launch Copilot CLI with the work-item prompt (non-interactive)
    $prompt = Build-CopilotPrompt -WI $wi -Skills $skills

    Write-Step "Running Copilot CLI (non-interactive)..."

    copilot -p $prompt --allow-all

    # 5 - Check for changes
    $changes = git status --porcelain 2>&1
    if (-not $changes) {
        Write-Warn "No file changes after Copilot session - skipping PR."
        git checkout $TargetBranch 2>&1 | Out-Null

        # Still mark as completed so it isn't picked up again
        Write-Step "Marking work item #$wiId as '$CompletedState'..."
        Set-WorkItemState -Id $wiId -Org $Organization -State $CompletedState
        Write-Ok "Work item #$wiId marked as '$CompletedState'"
        exit 0
    }

    # 6 - Commit
    Write-Step "Committing changes..."
    git add -A
    git commit -m "feat: implement ADO #$wiId - $($wi.Title)`n`nCo-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>" 2>&1 | Out-Null
    Write-Ok "Changes committed"

    # 7 - Push
    Write-Step "Pushing branch..."
    git push origin $branch 2>&1
    if ($LASTEXITCODE -ne 0) { throw "git push failed" }
    Write-Ok "Branch pushed to origin"

    # 8 - Create PR in Azure DevOps (skip if one already exists for this branch)
    Write-Step "Checking for existing PR..."
    $existingPr = az repos pr list `
        --repository $RepoName `
        --source-branch $branch `
        --target-branch $TargetBranch `
        --status active `
        --org $Organization `
        --project $Project `
        --output json 2>$null | ConvertFrom-Json

    if ($existingPr -and $existingPr.Count -gt 0) {
        $prId = $existingPr[0].pullRequestId
        Write-Ok "PR #$prId already exists - pushed updates to existing PR"

        # Ensure work item is linked to existing PR
        az repos pr work-item add --id $prId --work-items $wiId --org $Organization --output none 2>&1 | Out-Null
    }
    else {
        Write-Step "Creating pull request..."
        $prTitle = "ADO #$wiId`: $($wi.Title)"
        $prDesc  = "Implements Azure DevOps work item [#$wiId].`n`n" +
                   "**$($wi.WorkItemType):** $($wi.Title)`n`n" +
                   "Automated via Copilot CLI."

        $prJson = az repos pr create `
            --repository $RepoName `
            --source-branch $branch `
            --target-branch $TargetBranch `
            --title $prTitle `
            --description $prDesc `
            --org $Organization `
            --project $Project `
            --output json 2>&1

        if ($LASTEXITCODE -ne 0) { throw "PR creation failed: $prJson" }

        $pr = $prJson | ConvertFrom-Json
        $prId = $pr.pullRequestId
        Write-Ok "PR #$prId created -> $($pr.repository.webUrl)/pullrequest/$prId"

        # 9 - Add AI-Generated label via REST API
        Write-Step "Adding AI-Generated label to PR #$prId..."
        $labeled = Add-PrLabel -Org $Organization -ProjectId $pr.repository.project.id -RepoId $pr.repository.id -PrId $prId -Label "AI-Generated"
        if ($labeled) { Write-Ok "Label applied" } else { Write-Warn "Could not apply label" }

        # 10 - Link work item to the PR
        Write-Step "Linking work item #$wiId to PR #$prId..."
        az repos pr work-item add --id $prId --work-items $wiId --org $Organization --output none 2>&1 | Out-Null
        if ($LASTEXITCODE -eq 0) {
            Write-Ok "Work item #$wiId linked to PR #$prId"
        }
        else {
            Write-Warn "Could not link work item to PR"
        }
    }

    # 11 - Mark work item as completed in ADO
    Write-Step "Marking work item #$wiId as '$CompletedState'..."
    Set-WorkItemState -Id $wiId -Org $Organization -State $CompletedState
    Write-Ok "Work item #$wiId marked as '$CompletedState'"

    # 12 - Back to target branch
    git checkout $TargetBranch 2>&1 | Out-Null
}
catch {
    Write-Fail "Error on work item #$wiId`: $_"
    # Best-effort return to clean state
    git reset --hard HEAD         2>&1 | Out-Null
    git checkout $TargetBranch    2>&1 | Out-Null
    if ($branch) {
        git branch -D $branch 2>&1 | Out-Null
    }
    exit 1
}

# --- Done ----------------------------------------------------------------
Write-Host "`n--------------------------------------------------------" -ForegroundColor DarkGray
Write-Step "Done - work item #$wiId processed and marked as '$CompletedState'."
