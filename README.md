# SOWI.vCard

**.NET-Bibliothek zum Parsen und Serialisieren von vCard-Dateien** (`.vcf`) — Versionen **2.1**, **3.0** und **4.0** gemäss [RFC 6350](https://www.rfc-editor.org/rfc/rfc6350.html).

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
| **Mehrfach-vCards** | Dokumente mit mehreren `BEGIN:VCARD`-Blöcken |
| **Versionen** | 2.1, 3.0, 4.0 mit versionsspezifischen Strategien |
| **Erweiterbar** | `X-`-Properties und unbekannte Felder werden erhalten |

---

## Installation

### NuGet

```bash
dotnet add package SOWI.vCard
```

### Aus dem Quellcode

```bash
git clone https://github.com/SOWIinformatik/sowi-vcard.git
cd SOWI.vCard
dotnet build
dotnet test
```

---

## Schnellstart

```csharp
using SOWI.vCard.Abstractions;
using SOWI.vCard.Services;

IVCardService service = VCardService.CreateDefault();

// Parsen
string vCardText = await File.ReadAllTextAsync("kontakt.vcf");
var card = service.Parse(vCardText);

Console.WriteLine(card.FullName);          // z. B. Dr. Erika Mustermann
Console.WriteLine(card.Version);           // z. B. 4.0
Console.WriteLine(card.Emails[0].Address); // z. B. erika@mustermann.de

// Serialisieren
string output = service.Serialize(card);
await File.WriteAllTextAsync("kontakt-export.vcf", output);
```

### Mehrere vCards in einer Datei

```csharp
IReadOnlyList<VCard> cards = service.ParseDocument(vCardText);
string merged = service.SerializeDocument(cards);
```

### Neues vCard-Objekt erstellen

```csharp
using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;

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
services.AddSingleton<IVCardLineReader, VCardLineReader>();
services.AddSingleton<IVCardParser, VCardParser>();
services.AddSingleton<IVCardSerializer, VCardSerializer>();
services.AddSingleton<IVCardService, VCardService>();
```

Weitere Beispiele (Photo-Provider, async Datei-I/O): [`src/README.md`](src/README.md).

---

## Projektstruktur

```text
SOWI.vCard/
├── src/
│   ├── Domain/           # VCard-Aggregat und Value Objects
│   ├── Abstractions/     # IVCardParser, IVCardSerializer, IVCardService
│   ├── Parsing/          # RFC-Parser, Property-Handler, Version-Strategien
│   ├── Serialization/    # Serializer und Property-Writer
│   └── Services/         # VCardService (Fassade)
├── tests/
│   └── SOWI.vCard.Tests/ # Unit- und Integrationstests
└── docs/
    └── SOWI.vCard.Architecture.md
```

---

## Entwicklung

```bash
# Solution bauen
dotnet build SOWI.vCard.slnx

# Tests ausführen
dotnet test

# NuGet-Paket erstellen (Ausgabe: ../Packages/)
dotnet pack src/SOWI.vCard.csproj -c Release
```

---

## Dokumentation

| Dokument | Inhalt |
| -------- | ------ |
| [`src/README.md`](src/README.md) | RFC-Property-Referenz, vCard-Beispiele, API-Details |
| [`docs/SOWI.vCard.Architecture.md`](docs/SOWI.vCard.Architecture.md) | Architektur, Design-Patterns, Coding Standards |
| [`docs/SOWI.vCard.CICD.md`](docs/SOWI.vCard.CICD.md) | CI/CD, Versionierung, Release-Prozess |

---

## Lizenz

Dieses Projekt steht unter der [MIT-Lizenz](LICENSE.md).

Copyright © 2026 [SOWI Informatik](https://www.sowi.ch)
