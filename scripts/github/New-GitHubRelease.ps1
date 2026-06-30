# SOWI Informatik, www.sowi.ch
# Franz Schönbächler
#
# Erstellt ein GitHub Release mit Release Notes aus CHANGELOG.md.
# Voraussetzung: GitHub CLI (gh), Angemeldung mit repo-Berechtigung.
#
# Verwendung:
#   gh auth login
#   .\scripts\github\New-GitHubRelease.ps1 -Version 26.6.30
#   .\scripts\github\New-GitHubRelease.ps1 -Version 26.6.30 -TargetCommit e5cd8a4 -WhatIf

[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Parameter(Mandatory = $true)]
    [string] $Version,

    [string] $Repository = '',
    [string] $TargetCommit = 'HEAD',
    [string] $ChangelogPath = '',
    [switch] $Draft
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$scriptDirectory = $PSScriptRoot
if ([string]::IsNullOrWhiteSpace($scriptDirectory)) {
    $scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
}

$repositoryRoot = Split-Path (Split-Path $scriptDirectory -Parent) -Parent

if ([string]::IsNullOrWhiteSpace($ChangelogPath)) {
    $ChangelogPath = Join-Path $repositoryRoot 'CHANGELOG.md'
}

function Get-GhExecutable {
    $command = Get-Command gh -ErrorAction SilentlyContinue
    if ($command) {
        return $command.Source
    }

    $candidate = Join-Path ${env:ProgramFiles} 'GitHub CLI\gh.exe'
    if (Test-Path -LiteralPath $candidate) {
        return $candidate
    }

    throw 'GitHub CLI (gh) nicht gefunden. Installation: winget install GitHub.cli'
}

function Get-RepositoryFromGitRemote {
    $remoteUrl = git -C $repositoryRoot remote get-url origin 2>$null
    if (-not $remoteUrl) {
        throw 'Repository nicht ermittelbar. Parameter -Repository angeben.'
    }

    if ($remoteUrl -match 'github\.com[:/](?<owner>[^/]+)/(?<repo>[^/.]+)') {
        return "$($Matches.owner)/$($Matches.repo)"
    }

    throw "Ungültige origin-URL: $remoteUrl"
}

function Get-ChangelogSection {
    param(
        [Parameter(Mandatory = $true)]
        [string] $Path,

        [Parameter(Mandatory = $true)]
        [string] $ReleaseVersion
    )

    if (-not (Test-Path -LiteralPath $Path)) {
        throw "CHANGELOG nicht gefunden: $Path"
    }

    $lines = Get-Content -LiteralPath $Path -Encoding UTF8
    $headerPattern = "^## \[$([regex]::Escape($ReleaseVersion))\]"
    $startIndex = -1

    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match $headerPattern) {
            $startIndex = $i
            break
        }
    }

    if ($startIndex -lt 0) {
        throw "Kein Abschnitt ## [$ReleaseVersion] in $Path gefunden."
    }

    $sectionLines = @()
    for ($i = $startIndex + 1; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match '^## \[') {
            break
        }

        $sectionLines += $lines[$i]
    }

    $notes = ($sectionLines -join "`n").Trim()
    if ([string]::IsNullOrWhiteSpace($notes)) {
        throw "Release Notes für [$ReleaseVersion] sind leer."
    }

    return $notes
}

$gh = Get-GhExecutable
if (-not $Repository) {
    $Repository = Get-RepositoryFromGitRemote
}

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
    throw 'Nicht bei GitHub angemeldet. Bitte zuerst ausführen: gh auth login'
}

$releaseNotes = Get-ChangelogSection -Path $ChangelogPath -ReleaseVersion $Version
$title = "SOWI.vCard $Version"

Write-Host "Repository:     $Repository"
Write-Host "Version (Tag):  $Version"
Write-Host "Target commit:  $TargetCommit"
Write-Host "Title:          $title"

if ($PSCmdlet.ShouldProcess($Repository, "GitHub Release $Version erstellen")) {
    $tempFile = [System.IO.Path]::GetTempFileName()
    try {
        [System.IO.File]::WriteAllText($tempFile, $releaseNotes, [System.Text.UTF8Encoding]::new($false))

        $ghArgs = @(
            'release', 'create', $Version,
            '--repo', $Repository,
            '--target', $TargetCommit,
            '--title', $title,
            '--notes-file', $tempFile
        )

        if ($Draft) {
            $ghArgs += '--draft'
        }

        & $gh @ghArgs
        if ($LASTEXITCODE -ne 0) {
            throw 'gh release create fehlgeschlagen.'
        }
    }
    finally {
        Remove-Item -LiteralPath $tempFile -Force -ErrorAction SilentlyContinue
    }

    Write-Host ''
    Write-Host "Release erstellt: https://github.com/$Repository/releases/tag/$Version"
}
