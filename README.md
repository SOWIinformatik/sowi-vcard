# SOWI.vCard

**.NET-Bibliothek zum Parsen und Serialisieren von vCard-Dateien** (`.vcf`) — Versionen **2.1**, **3.0** und **4.0** (v4.0 gemäss [RFC 6350](https://www.rfc-editor.org/rfc/rfc6350.html), ältere Formate mit versionsspezifischen Strategien).

Entwickelt von [SOWI Informatik](https://www.sowi.ch).

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.md)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/SOWI.vCard)](https://www.nuget.org/packages/SOWI.vCard)

---

## Überblick

SOWI.vCard ist eine schlanke Format-Bibliothek ohne externe Abhängigkeiten. Sie wandelt vCard-Text in ein typisiertes Domänenmodell um und zurück — geeignet für Kontaktimport, Adressbuch-Export, CardDAV-Anbindungen und ähnliche Szenarien.

| Funktion | Beschreibung |
| -------- | ------------ |
| **Parsen** | vCard-String, Stream oder Datei → `VCard`-Objekt |
| **Serialisieren** | `VCard`-Objekt → RFC-konformen Text |
| **Mehrfach-vCards** | Dokumente mit mehreren `BEGIN:VCARD`-Blöcken (`ParseDocument` / `SerializeDocument`) |
| **Versionen** | 2.1, 3.0, 4.0 mit versionsspezifischen Strategien |
| **Erweiterbar** | `X-`-Properties → `VCard.Extensions`; unbekannte Standard-Properties → `VCard.Others` |

---

## Installation

### NuGet

```bash
dotnet add package SOWI.vCard
```

### Aus dem Quellcode

```bash
git clone https://github.com/SOWIinformatik/sowi-vcard.git
cd sowi-vcard
dotnet build SOWI.vCard.slnx
dotnet test SOWI.vCard.slnx
```

---

## Schnellstart

Empfohlener Einstiegspunkt: `VCardService.CreateDefault()`.

```csharp
using SOWI.vCard.Abstractions;
using SOWI.vCard.Services;

IVCardService service = VCardService.CreateDefault();

// Parsen (String)
string vCardText = await File.ReadAllTextAsync("kontakt.vcf");
var card = service.Parse(vCardText);

Console.WriteLine(card.FullName);          // z. B. Dr. Erika Mustermann
Console.WriteLine(card.Version);           // z. B. 4.0
Console.WriteLine(card.Emails[0].Address); // z. B. erika@mustermann.de

// Serialisieren
string output = service.Serialize(card);
await File.WriteAllTextAsync("kontakt-export.vcf", output);
```

Alternativ direkt über die Fassade (async Datei-I/O):

```csharp
var cardFromFile = await service.ParseFileAsync("kontakt.vcf");
await service.SerializeToFileAsync(cardFromFile, "kontakt-export.vcf");
```

### Mehrere vCards in einer Datei

```csharp
using SOWI.vCard.Abstractions;
using SOWI.vCard.Domain;
using SOWI.vCard.Services;

IVCardService service = VCardService.CreateDefault();
string document = await File.ReadAllTextAsync("adressbuch.vcf");

IReadOnlyList<VCard> cards = service.ParseDocument(document);
string merged = service.SerializeDocument(cards);
```

### Neues vCard-Objekt erstellen

```csharp
using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;
using SOWI.vCard.Services;

var card = new VCard
{
    Version = "4.0",
    FullName = "Max Mustermann",
    PersonName = new PersonName { LastName = "Mustermann", FirstName = "Max" },
    Organization = "Beispiel AG",
    Emails = { new Email { Address = "max@example.org" } },
};

string vCardText = VCardService.CreateDefault().Serialize(card);
```

### Dependency Injection

```csharp
using SOWI.vCard.Abstractions;
using SOWI.vCard.Parsing;
using SOWI.vCard.Serialization;
using SOWI.vCard.Services;

services.AddSingleton<IVCardLineReader, VCardLineReader>();
services.AddSingleton<IVCardParser, VCardParser>();
services.AddSingleton<IVCardSerializer, VCardSerializer>();
services.AddSingleton<IVCardService, VCardService>();
```

Weitere Beispiele (Photo-Provider, Stream-I/O): [`src/README.md`](src/README.md).

---

## Projektstruktur

```text
sowi-vcard/                         # GitHub-Repository (Projektname: SOWI.vCard)
├── .github/workflows/
│   ├── ci.yml                      # Build, Test, Pack (Artefakte)
│   └── release.yml                 # Manuell: NuGet-Publish
├── src/
│   ├── SOWI.vCard.csproj
│   ├── Domain/                     # VCard-Aggregat, Value Objects, Exceptions
│   ├── Abstractions/               # IVCardParser, IVCardSerializer, IVCardService, …
│   ├── Parsing/                    # Parser, PropertyHandlers, VersionStrategies
│   ├── Serialization/              # VCardSerializer, VCardPropertyWriters
│   ├── Services/                   # VCardService (Fassade), FilePhotoDataProvider
│   └── README.md                   # RFC-Property-Referenz
├── tests/
│   └── SOWI.vCard.Tests/           # Unit- und Integrationstests
├── docs/
│   ├── SOWI.vCard.Architecture.md
│   └── SOWI.vCard.CICD.md
├── Directory.Build.props
├── SOWI.vCard.slnx
├── LICENSE.md
└── .editorconfig
```

---

## Entwicklung

```bash
# Solution bauen
dotnet build SOWI.vCard.slnx

# Tests ausführen
dotnet test SOWI.vCard.slnx

# NuGet-Paket lokal erstellen (Ausgabe: ../Packages/)
dotnet pack src/SOWI.vCard.csproj -c Release
```

CI-Pipeline (GitHub Actions) und Release-Prozess: [`docs/SOWI.vCard.CICD.md`](docs/SOWI.vCard.CICD.md).

---

## Dokumentation

| Dokument | Inhalt |
| -------- | ------ |
| [`src/README.md`](src/README.md) | RFC-Property-Referenz, vCard-Beispiele, API-Details |
| [`docs/SOWI.vCard.Architecture.md`](docs/SOWI.vCard.Architecture.md) | Ist-Architektur, Design-Prinzipien, Coding Standards |
| [`docs/SOWI.vCard.CICD.md`](docs/SOWI.vCard.CICD.md) | CI/CD, Versionierung, Release-Prozess |

---

## Lizenz

Dieses Projekt steht unter der [MIT-Lizenz](LICENSE.md).

Copyright © 2026 [SOWI Informatik](https://www.sowi.ch)
