# SOWI Informatik, www.sowi.ch
# Franz Schönbächler
#
# Wendet das Repository Ruleset für Branch Protection auf main an.
# Konfiguration: PR-Pflicht, CI-Check build, Konversations-Auflösung; ohne Review-Pflicht
# und ohne Up-to-date-Zwang (Solo-Maintainer, öffentliches Repository).
# Voraussetzung: GitHub CLI (gh) mit Admin-Rechten auf dem Repository.
#
# Verwendung:
#   gh auth login
#   .\scripts\github\Apply-BranchProtection.ps1
#   .\scripts\github\Apply-BranchProtection.ps1 -Repository SOWIinformatik/sowi-vcard -WhatIf

[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [string] $Repository = '',
    [string] $RulesetPath = '',
    [string] $RulesetName = 'SOWI.vCard main'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$scriptDirectory = $PSScriptRoot
if ([string]::IsNullOrWhiteSpace($scriptDirectory)) {
    $scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
}

if ([string]::IsNullOrWhiteSpace($RulesetPath)) {
    $RulesetPath = Join-Path $scriptDirectory 'branch-protection-ruleset.json'
}

function Get-GhExecutable {
    $command = Get-Command gh -ErrorAction SilentlyContinue
    if ($command) {
        return $command.Source
    }

    $candidates = @(
        (Join-Path ${env:ProgramFiles} 'GitHub CLI\gh.exe'),
        (Join-Path ${env:LOCALAPPDATA} 'Programs\GitHub CLI\gh.exe')
    )

    foreach ($candidate in $candidates) {
        if (Test-Path -LiteralPath $candidate) {
            return $candidate
        }
    }

    throw 'GitHub CLI (gh) nicht gefunden. Installation: winget install GitHub.cli'
}

function Get-RepositoryFromGitRemote {
    $repositoryRoot = Split-Path (Split-Path $scriptDirectory -Parent) -Parent
    $remoteUrl = git -C $repositoryRoot remote get-url origin 2>$null
    if (-not $remoteUrl) {
        throw 'Repository nicht ermittelbar. Parameter -Repository angeben (z. B. SOWIinformatik/sowi-vcard).'
    }

    if ($remoteUrl -match 'github\.com[:/](?<owner>[^/]+)/(?<repo>[^/.]+)') {
        return "$($Matches.owner)/$($Matches.repo)"
    }

    throw "Ungültige origin-URL: $remoteUrl"
}

function Invoke-GhApi {
    param(
        [Parameter(Mandatory = $true)]
        [string] $Gh,

        [Parameter(Mandatory = $true)]
        [string[]] $Args
    )

    $output = & $Gh @Args 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw ($output | Out-String).Trim()
    }

    return $output
}

$gh = Get-GhExecutable
if (-not $Repository) {
    $Repository = Get-RepositoryFromGitRemote
}

Write-Host "Repository: $Repository"
Write-Host "Ruleset:    $RulesetName"

$authExitCode = 0
$previousErrorAction = $ErrorActionPreference
$ErrorActionPreference = 'Continue'
try {
    & $gh auth status 1>$null 2>$null
    $authExitCode = $LASTEXITCODE
}
finally {
    $ErrorActionPreference = $previousErrorAction
}

if ($authExitCode -ne 0) {
    throw @"
Nicht bei GitHub angemeldet. Bitte zuerst ausführen:

  gh auth login

Erforderliche Berechtigung: Repository-Administration (Rulesets anlegen/ändern).
"@
}

if (-not (Test-Path -LiteralPath $RulesetPath)) {
    throw "Ruleset-Datei nicht gefunden: $RulesetPath"
}

$rulesetBody = Get-Content -LiteralPath $RulesetPath -Raw -Encoding UTF8
$null = $rulesetBody | ConvertFrom-Json

$existingRulesetsJson = Invoke-GhApi -Gh $gh -Args @(
    'api',
    "repos/$Repository/rulesets"
)

$existingRulesetId = $null
foreach ($entry in ($existingRulesetsJson | ConvertFrom-Json)) {
    if ($entry.name -eq $RulesetName) {
        $existingRulesetId = [string]$entry.id
        break
    }
}

if ($PSCmdlet.ShouldProcess($Repository, 'Branch Protection Ruleset anwenden')) {
    if ($existingRulesetId) {
        Write-Host "Aktualisiere bestehendes Ruleset (ID $existingRulesetId) ..."
        $tempFile = [System.IO.Path]::GetTempFileName()
        try {
            [System.IO.File]::WriteAllText($tempFile, $rulesetBody, [System.Text.UTF8Encoding]::new($false))
            Invoke-GhApi -Gh $gh -Args @(
                'api',
                '--method', 'PUT',
                "repos/$Repository/rulesets/$existingRulesetId",
                '--input', $tempFile
            ) | Out-Null
        }
        finally {
            Remove-Item -LiteralPath $tempFile -Force -ErrorAction SilentlyContinue
        }
    }
    else {
        Write-Host 'Erstelle neues Ruleset ...'
        $tempFile = [System.IO.Path]::GetTempFileName()
        try {
            [System.IO.File]::WriteAllText($tempFile, $rulesetBody, [System.Text.UTF8Encoding]::new($false))
            Invoke-GhApi -Gh $gh -Args @(
                'api',
                '--method', 'POST',
                "repos/$Repository/rulesets",
                '--input', $tempFile
            ) | Out-Null
        }
        finally {
            Remove-Item -LiteralPath $tempFile -Force -ErrorAction SilentlyContinue
        }
    }

    Write-Host ''
    Write-Host 'Branch Protection erfolgreich angewendet.'
    Write-Host "Prüfen: https://github.com/$Repository/settings/rules"
}
