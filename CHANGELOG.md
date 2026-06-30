# Changelog

Alle wesentlichen Änderungen an **SOWI.vCard** werden in dieser Datei dokumentiert.<br>
Das Format orientiert sich an [Keep a Changelog](https://keepachangelog.com/de/1.1.0/); Versionen folgen dem SOWI-Schema `YY.MM.DD`.

## [26.6.30] — 2026-06-30

### Hinzugefügt

- Branch Protection für `main` (Repository Ruleset, Solo-Maintainer-Auslegung)
- Skripte unter `scripts/github/` für Ruleset und GitHub Releases
- Dokumentation CI/CD (Anhang E Branch Protection, Release-Test)

### Geändert

- CI/CD-Dokumentation an Ist-Stand (`ci.yml`, Environment `production` ohne Required reviewers)
- Release-Pipeline: NuGet-Artefakt aus flachem Download-Pfad

### Veröffentlichung

- NuGet: [SOWI.vCard 26.6.30](https://www.nuget.org/packages/SOWI.vCard/26.6.30)
- GitHub Actions Release-Run: [28441095511](https://github.com/SOWIinformatik/sowi-vcard/actions/runs/28441095511)

## [26.6.29] — 2026-06-29

### Hinzugefügt

- Erste Veröffentlichung auf [nuget.org](https://www.nuget.org/packages/SOWI.vCard)
- GitHub Actions: CI (`ci.yml`), Release (`release.yml`) mit Trusted Publishing (OIDC)
- Repository-Variable `NUGET_USER`, Environment `production`

### Veröffentlichung

- NuGet: [SOWI.vCard 26.6.29](https://www.nuget.org/packages/SOWI.vCard/26.6.29)
- GitHub Actions Release-Run: [28351585999](https://github.com/SOWIinformatik/sowi-vcard/actions/runs/28351585999)

## [26.6.28] — 2026-06-28

### Hinzugefügt

- Initiale Bibliothek: Parser und Serializer für vCard 2.1, 3.0 und 4.0 (RFC 6350)
- xUnit-Tests, Outlook-/Apple-Fixtures, Round-Trip- und Integrations tests
- NuGet-Metadaten, automatische Versionierung über `Directory.Build.props`
