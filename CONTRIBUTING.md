# Mitwirken an SOWI.vCard

Vielen Dank für dein Interesse an **SOWI.vCard** — einer .NET-Bibliothek zum Parsen und Serialisieren von vCard-Dateien (2.1, 3.0, 4.0).

Dieses Dokument ist der **kurze Einstieg** für Beiträge von aussen. Details zu Architektur, CI/CD und Releases stehen in den verlinkten Dokumenten.

---

## Wie du helfen kannst

### Fehler melden

Bitte erstelle ein [GitHub Issue](https://github.com/SOWIinformatik/sowi-vcard/issues) mit:

- **Beobachtung:** Was passiert ist?
- **Erwartung:** Was hättest du erwartet?
- **Reproduktion:** Schritte oder minimaler Code / `.vcf`-Ausschnitt
- **Version:** NuGet-Version oder Commit, .NET-Version

Anonymisierte Kontaktdaten in Beispiel-`.vcf`-Dateien verwenden.

### Verbesserungen vorschlagen

Für grössere Änderungen (neue Properties, API-Erweiterungen, Breaking Changes) vor der Implementierung kurz ein Issue eröffnen — das spart Umwege beim Review.

Typische Beiträge:

- Parser-/Serializer-Korrekturen (RFC-Konformität)
- Neue oder erweiterte Properties
- Tests und Fixtures (z. B. Outlook-, Apple-`.vcf`)
- Dokumentation

---

## Pull Request einreichen

1. Repository **forken** und lokal klonen.
2. Branch von `main` erstellen:
   - `feature/kurze-beschreibung` — neue Funktionen
   - `fix/kurze-beschreibung` — Fehlerbehebungen
   - `hotfix/kurze-beschreibung` — kritische Korrekturen
3. Lokal bauen und testen:

   ```bash
   dotnet build SOWI.vCard.slnx -c Release
   dotnet test SOWI.vCard.slnx -c Release --no-build
   ```

4. **Pull Request** nach `main` öffnen.

### Was wir beim Review prüfen

| Thema | Erwartung |
| ----- | --------- |
| CI | Check `build` muss grün sein |
| Tests | Neue Logik mit sinnvollen Unit- oder Integrationstests abdecken |
| RFC | Verhalten an RFC 6350 bzw. versionsspezifische Strategien anlehnen |
| Stil | `.editorconfig` und bestehende Muster im Repository |
| Scope | Ein PR pro thematischem Change |

Direkte Commits auf `main` sind durch Branch Protection nicht möglich.

---

## Vor dem Coden lesen

| Dokument | Inhalt |
| -------- | ------ |
| [`docs/SOWI.vCard.Architecture.md`](docs/SOWI.vCard.Architecture.md) | Schichten, Patterns, Coding Standards |
| [`src/README.md`](src/README.md) | RFC-Property-Referenz, API-Details |
| [`docs/SOWI.vCard.CICD.md`](docs/SOWI.vCard.CICD.md) | Branches, CI, Releases (für Maintainer) |

**Architektur in Kürze:** Konsumenten nutzen `VCardService` als Fassade. Parsing und Serialisierung sind getrennt; die **Domain** enthält keine I/O- oder Parser-Details. Neue Properties gehören in die passende Schicht (Handler, Value Object, Tests).

---

## Commit-Nachrichten

Kurz und sachlich, auf Deutsch oder Englisch — konsistent im PR. Beispiele:

- `Fix: Escaping bei Semikolon in FN-Property`
- `Feature: Unterstützung für IMPP-Property (vCard 4.0)`

---

## Lizenz

Mit dem Einreichen eines Beitrags stimmst du zu, dass deine Änderungen unter der [MIT-Lizenz](LICENSE.md) des Projekts veröffentlicht werden.

---

Copyright © 2026 [SOWI Informatik](https://www.sowi.ch)
