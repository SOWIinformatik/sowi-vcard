// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

// Ignore Spelling: Timezone Geo UID Pid Timestamp

using SOWI.vCard.Domain.ValueObjects;

namespace SOWI.vCard.Domain
{
    /// <summary>
    /// Aggregate Root: repräsentiert eine vCard gemäss RFC 6350.<br />
    /// Siehe src/README.md für Property-Referenz.
    /// </summary>
    public class VCard
    {
        /// <summary>
        /// Persistente, global eindeutige Kennung (UID).
        /// </summary>
        public string Uid { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// vCard-Version (VERSION), z. B. 3.0 oder 4.0.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Formatierter vollständiger Name (FN).
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Strukturierter Name (N).
        /// </summary>
        public PersonName PersonName { get; set; } = new();

        /// <summary>
        /// Alternativname (NICKNAME).
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// Foto (PHOTO).
        /// </summary>
        public Photo? Photo { get; set; }

        /// <summary>
        /// Geburtsdatum (BDAY).
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Adressen (ADR).
        /// </summary>
        public List<Address> Addresses { get; set; } = new();

        /// <summary>
        /// Telefonnummern (TEL).
        /// </summary>
        public List<PhoneNumber> PhoneNumbers { get; set; } = new();

        /// <summary>
        /// E-Mail-Adressen (EMAIL).
        /// </summary>
        public List<Email> Emails { get; set; } = new();

        /// <summary>
        /// Stellenbezeichnung (TITLE).
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Rolle (ROLE).
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Organisation (ORG).
        /// </summary>
        public string Organization { get; set; } = string.Empty;

        /// <summary>
        /// Website (URL).
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Notiz (NOTE).
        /// </summary>
        public string Note { get; set; } = string.Empty;

        /// <summary>
        /// Jahrestag (ANNIVERSARY).
        /// </summary>
        public DateTime? Anniversary { get; set; }

        /// <summary>
        /// Geschlecht (GENDER).
        /// </summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Kategorien (CATEGORIES).
        /// </summary>
        public List<string> Categories { get; set; } = new();

        /// <summary>
        /// Zeitzone (TZ).
        /// </summary>
        public string Timezone { get; set; } = string.Empty;

        /// <summary>
        /// Geo-Koordinaten (GEO).
        /// </summary>
        public GeoLocation Geo { get; set; } = new();

        /// <summary>
        /// Sprachen (LANG).
        /// </summary>
        public List<string> Languages { get; set; } = new();

        /// <summary>
        /// Verwandte Personen (RELATED).
        /// </summary>
        public List<RelatedPerson> Related { get; set; } = new();

        /// <summary>
        /// Erstellungszeitpunkt (CREATED).
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Letzte Änderung (REV).
        /// </summary>
        public DateTime? Revision { get; set; }

        /// <summary>
        /// Verarbeitungszeitstempel (DTSTAMP).
        /// </summary>
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// Sensibilität (CLASS).
        /// </summary>
        public string Classification { get; set; } = string.Empty;

        /// <summary>
        /// Öffentlicher Schlüssel (KEY).
        /// </summary>
        public PublicKey? Key { get; set; }

        /// <summary>
        /// Geburtsort (BIRTHPLACE, RFC 6474).
        /// </summary>
        public string Birthplace { get; set; } = string.Empty;

        /// <summary>
        /// Sterbedatum (DEATHDATE, RFC 6474).
        /// </summary>
        public DateTime? DeathDate { get; set; }

        /// <summary>
        /// Sterbeort (DEATHPLACE, RFC 6474).
        /// </summary>
        public string DeathPlace { get; set; } = string.Empty;

        /// <summary>
        /// Fachgebiete (EXPERTISE, RFC 6715).
        /// </summary>
        public List<LeveledText> Expertises { get; set; } = new();

        /// <summary>
        /// Hobbys (HOBBY, RFC 6715).
        /// </summary>
        public List<LeveledText> Hobbies { get; set; } = new();

        /// <summary>
        /// Interessen (INTEREST, RFC 6715).
        /// </summary>
        public List<LeveledText> Interests { get; set; } = new();

        /// <summary>
        /// Organisationsverzeichnis-URI (ORG-DIRECTORY, RFC 6715).
        /// </summary>
        public string OrgDirectory { get; set; } = string.Empty;

        /// <summary>
        /// X-Erweiterungen (Properties mit Präfix X-).
        /// </summary>
        public Dictionary<string, string> Extensions { get; set; } = new();

        /// <summary>
        /// Nicht zugeordnete Properties (unbekannte Standard-Properties).
        /// </summary>
        public Dictionary<string, string> Others { get; set; } = new();

        /// <summary>
        /// Produktkennung (PRODID).
        /// </summary>
        public string ProductIdentifier { get; set; } = string.Empty;

        /// <summary>
        /// Sortierstring (SORT-STRING).
        /// </summary>
        public string SortString { get; set; } = string.Empty;

        /// <summary>
        /// Client-PID-Maps (CLIENTPIDMAP).
        /// </summary>
        public List<ClientPidMap> ClientPidMaps { get; set; } = new();

        /// <summary>
        /// Free/Busy-URL (FBURL).
        /// </summary>
        public string FreeBusyUrl { get; set; } = string.Empty;

        /// <summary>
        /// Kalender-Adress-URI (CALADRURI).
        /// </summary>
        public string CalendarAddressUri { get; set; } = string.Empty;

        /// <summary>
        /// Kalender-URI (CALURI).
        /// </summary>
        public string CalendarUri { get; set; } = string.Empty;

        /// <summary>
        /// Instant-Messenger-Handle (IMPP).
        /// </summary>
        public List<string> Impps { get; set; } = new();

        /// <summary>
        /// Quell-URL der vCard (SOURCE).
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Textdarstellung der SOURCE-Property (NAME, vCard 3.0).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Art des Objekts (KIND): individual, organization, group.
        /// </summary>
        public string Kind { get; set; } = string.Empty;

        /// <summary>
        /// Gruppenmitglieder (MEMBER), nur bei KIND=group.
        /// </summary>
        public List<string> Members { get; set; } = new();

        /// <summary>
        /// Vertreter/Assistent (AGENT).
        /// </summary>
        public string Agent { get; set; } = string.Empty;

        /// <summary>
        /// Logo (LOGO).
        /// </summary>
        public Photo? Logo { get; set; }

        /// <summary>
        /// Klang/Aussprache (SOUND).
        /// </summary>
        public Photo? Sound { get; set; }

        /// <summary>
        /// Eigenständige Adressetiketten (LABEL, vCard 2.1/3.0).
        /// </summary>
        public List<PostalLabel> PostalLabels { get; set; } = new();

        /// <summary>
        /// E-Mail-Programm (MAILER, vCard 2.1/3.0).
        /// </summary>
        public string Mailer { get; set; } = string.Empty;

        /// <summary>
        /// Profil-Kennung (PROFILE, vCard 2.1/3.0).
        /// </summary>
        public string Profile { get; set; } = string.Empty;

        /// <summary>
        /// Zusätzliche XML-Daten (XML, vCard 4.0).
        /// </summary>
        public string Xml { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string ToString() => $"{this.FullName} ({this.Version})";
    }
}
