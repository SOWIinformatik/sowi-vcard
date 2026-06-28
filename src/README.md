# SOWI vCard


SOWI vCard Komponente, parser für das vCard Dateiformat.
- Parsing den vCard String zum vCard Data Model.   
- Parsing das vCard Data Model zum vCard String.   


## Spezifikation vCard

vCard definiert nach der Referenzspezifikation RFC 6350 (August 2011) [RFC 6350: vCard Format Specification (rfc-editor.org)](https://www.rfc-editor.org/rfc/rfc6350.html)

Eine vCard wird als einfache, unformatierte Textdatei gespeichert. Diese besteht aus einem oder mehreren vCard-Objekten, die durch die Begrenzungen `BEGIN:VCARD` und `END:VCARD` eingeschlossen werden. Alle vCards müssen die `VERSION`-Eigenschaft enthalten, die die vCard-Version spezifiziert. `VERSION` muss direkt auf `BEGIN` folgen (außer im vCard-2.1-Standard, wo es überall innerhalb der vCard vorkommen darf). Die Inhalte der vCard bestehen aus einzelnen Eigenschaften und deren Attributen. Die Eigenschaften können in beliebiger Reihenfolge wie folgt definiert werden:

```
EIGENSCHAFT[;PARAMETER]:Attribut[;Attribut]
```

1))unterstützt und erforderlich

2)) unterstützt, nicht erforderlich

3)) als Eigenschaft nicht länger unterstützt, da jetzt als Parameter innerhalb anderer Eigenschaft nutzbar

4)) Sonderfall, unterstützt

| Name           | 2.1  | 3.0  | 4.0  | Beschreibung                                                 | Beispiel                                                     |
| -------------- | :--: | :--: | :--: | ------------------------------------------------------------ | ------------------------------------------------------------ |
| `ADR`          |  2)  |  2)  |  2)  | Strukturierte Darstellung der physischen Anschrift des vCard-Objekts. | `ADR;TYPE=home:;;Heidestrasse 17;Koeln;;51147;Germany`       |
| `AGENT`        |  2)  |  2)  |      | Informationen über eine andere Person, die im Namen des vCard-Objekts handeln soll. Typischerweise ist das ein Vertreter, Assistent oder Sekretär. Hier kann entweder ein URL oder eine eingebettete vCard angegeben werden. | `AGENT:http://de.wikipedia.org/wiki/007`                     |
| `ANNIVERSARY`  |      |      |  2)  | Jahrestag (gemeint ist i. A. der Hochzeitstag) der Person.   | `ANNIVERSARY:20140812` oder `ANNIVERSARY:2014-08-12`         |
| `BDAY`         |  2)  |  2)  |  2)  | Geburtsdatum der mit der vCard verbundenen Person.           | **2.1**, **3.0**: `BDAY:1964-08-12` <br />**4.0**: `BDAY:19640812` oder `BDAY:1964-08-12` |
| `BEGIN`        |  1)  |  1)  |  1)  | Jede vCard muss mit dieser Eigenschaft beginnen.             | `BEGIN:VCARD`                                                |
| `CALADRURI`    |      |      |  2)  | URL für das Senden einer Terminanforderung zur Verwendung des Kalenders der Person. | `CALADRURI:http://example.org/kalender/emuster`              |
| `CALURI`       |      |      |  2)  | URL zum Kalender der Person.                                 | `CALURI:http://example.org/kalender/emuster`                 |
| `CATEGORIES`   |  2)  |  2)  |  2)  | Liste von Eigenschaften, die das Objekt der vCard beschreiben. | `CATEGORIES:swimmer,biker`                                   |
| `CLASS`        |      |  2)  |      | Sensibilität der in der vCard enthaltenen Daten.             | `CLASS:public`                                               |
| `CLIENTPIDMAP` |      |      |  2)  | UUID (Universally Unique Identifier) für die Synchronisation verschiedener Revisionsstände derselben vCard. | `CLIENTPIDMAP:1;urn:uuid:3df403f4-5924-4bb7-b077-3c711d9eb34b` |
| `EMAIL`        |  2)  |  2)  |  2)  | E-Mail-Adresse zur Kommunikation mit dem vCard-Objekt.       | `EMAIL:erika@mustermann.de`                                  |
| `END`          |  1)  |  1)  |  1)  | Jede vCard muss mit dieser Eigenschaft enden.                | `END:VCARD`                                                  |
| `FBURL`        |      |      |  2)  | URL, der beschreibt, ob auf dem Kalender der Person „frei“ oder „besetzt“ angezeigt wird. | `FBURL:http://example.org/fb/emuster`                        |
| `FN`           |  2)  |  1)  |  1)  | Formatierte Zeichenfolge mit dem vollständigen Namen des vCard-Objekts. | `FN:Dr. Erika Mustermann`                                    |
| `GENDER`       |      |      |  2)  | Geschlecht (biologisch) und Geschlechtsidentität der Person. | `GENDER:F` `GENDER:O;intersex`                               |
| `GEO`          |  2)  |  2)  |  2)  | Längen- und Breitengrad.                                     | **2.1**, **3.0**: `GEO:50.858;7.0885` <br />**4.0**: `GEO:geo:50.858\,7.0885` |
| `IMPP`         |      |  4)  |  2)  | Definiert ein Instant-Messenger-Handle. *Dieses Objekt wurde vor Gültigkeit der vCard-Version 3.0 durch einen separaten RFC eingeführt. Deshalb kann vCards 3,0 diese Eigenschaft nutzen, auch wenn sie nicht Teil der Spezifikation 3.0 ist.* | `IMPP:aim:erika@mustermann.de`                               |
| `KEY`          |  2)  |  2)  |  2)  | Öffentlicher Schlüssel, der dem vCard-Objekt zugeordnet ist. Es kann zu einem externen URL verwiesen werden, Klartext angegeben werden oder ein Base64-kodierter Textblock in die vCard eingebettet werden. | **2.1**: `KEY;PGP:http://example.org/key.pgp` <br />**2.1**: `KEY;PGP;ENCODING=BASE64:[base64-data]` <br />**3.0**: `KEY;TYPE=PGP:http://example.org/key.pgp` <br />**3.0**: `KEY;TYPE=PGP;ENCODING=B:[base64-data]` <br />**4.0**: `KEY;MEDIATYPE=application/pgp-keys:http://example.org/key.pgp` <br />**4.0**: `KEY:data:application/pgp-keys;base64\,[base64-data]` |
| `KIND`         |      |      |  2)  | Art des Objekts, das die vCard beschreibt: Eine Person (`individual`), eine Organisation (`organization`) oder eine Gruppe (`group`). | `KIND:individual`                                            |
| `LABEL`        |  2)  |  2)  |  3)  | Stellt den eigentlichen Text dar, der auf einem physischen Adressetikett zur Adressierung an das mit der vCard verbundene Objekt vorhanden ist (ähnlich der `ADR`-Eigenschaft).  *Nicht unterstützt in Version 4.0. Stattdessen ist diese Information im `LABEL`-Parameter der `ADR`-Eigenschaft gespeichert.* | `LABEL;TYPE=HOME:Heidestrasse 17\n51147 Koeln\nDeutschland`  |
| `LANG`         |      |      |  2)  | Sprache, die die Person spricht.                             | `LANG:de-DE`                                                 |
| `LOGO`         |  2)  |  2)  |  2)  | Logo der Organisation, mit der die Person in Beziehung steht, der die vCard gehört. Es kann auf einen externen URL verwiesen oder ein Base64-kodierter Textblock in die vCard eingebettet werden. | **2.1**: `LOGO;PNG:http://example.org/logo.png` <br />**2.1**: `LOGO;PNG;ENCODING=BASE64:[base64-data]` <br />**3.0**: `LOGO;TYPE=PNG:http://example.org/logo.png` <br />**3.0**: `LOGO;TYPE=PNG;ENCODING=b:[base64-data]` <br />**4.0**: `LOGO;MEDIATYPE=image/png:http://example.org/logo.png` <br />**4.0**: `LOGO:data:image/png;base64,[base64-data]` |
| `MAILER`       |  2)  |  2)  |      | Art des genutzten E-Mail-Programms.                          | `MAILER:Thunderbird`                                         |
| `MEMBER`       |      |      |  2)  | Definiert das Objekt, das die vCard repräsentiert, als Teil einer Gruppe. Zulässige Werte sind:ein „mailto:“-URL, der eine E-Mail-Adresse enthältein UUID (Universally Unique Identifier), der auf die eigene vCard des Mitglieds verweistUm diese Eigenschaft verwenden zu können, muss die `KIND`-Eigenschaft auf „group“ gesetzt werden. | `MEMBER:urn:uuid:550e8400-e29b-11d4-a716-446655440000`       |
| `N`            |  1)  |  1)  |  2)  | Strukturierte Darstellung von Namen der Person, Ort oder Sache, der das vCard-Objekt zugeordnet ist. (N:Nachname;Vorname;zusätzliche Vornamen;Präfix;Suffix) | `N:Mustermann;Erika;;Dr.;`                                   |
| `NAME`         |      |  2)  |      | Textdarstellung der `SOURCE`-Eigenschaft.                    |                                                              |
| `NICKNAME`     |      |  2)  |  2)  | Ein oder mehrere Alternativnamen für das Objekt, das von der vCard repräsentiert wird. | `NICKNAME:Erica,Ehrenreiche`                                 |
| `NOTE`         |  2)  |  2)  |  2)  | Zusätzliche Informationen oder Kommentar, welche mit der vCard in Verbindung stehen. | `NOTE:Ich bin eine fiktive Person aus Deutschland\nund ich bin bundesweit bekannt.` |
| `ORG`          |  2)  |  2)  |  2)  | Name und gegebenenfalls Einheit(en) der Organisation, der das vCard-Objekt zugeordnet ist. Diese Eigenschaft basiert auf den Attributen „Organization Name“ und „Organization Unit“ des X.520-Standards. | `ORG:Google;GMail Team;Spam Detection Squad`                 |
| `PHOTO`        |  2)  |  2)  |  2)  | Bild oder Fotografie der mit der vCard verbundenen Person. Es kann auf einen externen URL verwiesen oder ein Base64 Textblock in die vCard eingebettet werden. | **2.1**: `PHOTO;JPEG:http://example.org/photo.jpg` <br />**2.1**: `PHOTO;JPEG;ENCODING=BASE64:[base64-data]` <br />**3.0**: `PHOTO;TYPE=JPEG:http://example.org/photo.jpg` <br />**3.0**: `PHOTO;TYPE=JPEG;ENCODING=b:[base64-data]` <br />**4.0**: `PHOTO;MEDIATYPE=image/jpeg:http://example.org/photo.jpg` <br />**4.0**: `PHOTO:data:image/jpeg;base64,[base64-data]` |
| `PRODID`       |      |  2)  |  2)  | Kennung für das Produkt, mit dem das vCard-Objekt erstellt wurde. | `PRODID:-//ONLINE DIRECTORY//NONSGML Version 1//DE`          |
| `PROFILE`      |  2)  |  2)  |      | Legt fest, dass die vCard eine vCard ist.                    | `PROFILE:VCARD`                                              |
| `RELATED`      |      |      |  2)  | Andere Einheit, zu der die Person Verbindung hat. Zulässige Werte sind: ein „mailto:“-URL,  der eine E-Mail-Adresse enthält ein UUID (Universally Unique Identifier), der auf die eigene vCard des Mitglieds verweist | `RELATED;TYPE=friend:urn:uuid:550e8400-e29b-11d4-a716-446655440000` |
| `REV`          |  2)  |  2)  |  2)  | Zeitstempel der letzten Aktualisierung der vCard.            | `REV:20140301T221110Z`                                       |
| `ROLE`         |  2)  |  2)  |  2)  | Rolle, Beruf oder Wirtschaftskategorie des vCard-Objekts innerhalb einer Organisation. | `ROLE:Executive`                                             |
| `SORT-STRING`  |  2)  |  2)  |  3)  | Zeichenkette, die die Sortierreihenfolge der vCard in Anwendungen beschreibt. 3)*Nicht unterstützt in Version 4.0. Stattdessen wird diese Information im `SORT-AS`-Parameter der `N`- und/oder `ORG`-Eigenschaft gespeichert.* | `SORT-STRING:Mustermann`                                     |
| `SOUND`        |  2)  |  2)  |  2)  | Gibt standardmäßig die Aussprache der `FN`-Eigenschaft des vCard-Objekts an, wenn diese Eigenschaft nicht mit anderen Eigenschaften verknüpft ist. Es kann auf einen externen URL verwiesen oder ein Base64 Textblock in die vCard eingebettet werden. | **2.1**: `SOUND;OGG:http://example.org/sound.ogg` <br />**2.1**: `SOUND;OGG;ENCODING=BASE64:[base64-data]` <br />**3.0**: `SOUND;TYPE=OGG:http://example.org/sound.ogg` <br />**3.0**: `SOUND;TYPE=OGG;ENCODING=b:[base64-data]` <br />**4.0**: `SOUND;MEDIATYPE=audio/ogg:http://example.org/sound.ogg` <br />**4.0**: `SOUND:data:audio/ogg;base64\,[base64-data]` |
| `SOURCE`       |  2)  |  2)  |  2)  | URL, der verwendet werden kann, um die neueste Version dieser vCard zu erhalten. | `SOURCE:http://mustermann.de/vCard.vcf`                      |
| `TEL`          |  2)  |  2)  |  2)  | Normalform einer numerischen Zeichenkette für eine Telefonnummer zur telefonischen Kommunikation mit dem vCard-Objekt. | `TEL;TYPE=CELL,HOME:(0170) 1234567` `TEL;TYPE=VOICE,HOME:+4141310xxyy` |
| `TITLE`        |  2)  |  2)  |  2)  | Angabe der Stellenbezeichnung, funktionellen Stellung oder Funktion der mit dem vCard-Objekt verbunden Person innerhalb einer Organisation. | `TITLE:V.P. Research and Development`                        |
| `TZ`           |  2)  |  2)  |  2)  | Zeitzone des vCard Objekts.                                  | **2.1**, **3.0**: `TZ:+0100` <br />**4.0**: `TZ:Europe/Berlin` |
| `UID`          |  2)  |  2)  |  2)  | UUID (Universally Unique Identifier), der eine persistente, global eindeutige Kennung des verbundenen Objekts darstellt. | `UID:urn:uuid:550e8400-e29b-11d4-a716-44665544ffff`          |
| `URL`          |  2)  |  2)  |  2)  | URL zu einer Website, die die Person repräsentiert.          | `URL:http://www.mustermann.de`                               |
| `VERSION`      |  1)  |  1)  |  1)  | Version der vCard-Spezifikation. In den Versionen 3.0 und 4.0 muss diese auf die `BEGIN`-Eigenschaft folgen. | `VERSION:3.0`                                                |
| `XML`          |      |      |  2)  | Beliebige mit der vCard verbundene XML-Daten. Wird verwendet, wenn die vCard in XML-codiert ist (xCard-Standard) und das XML-Dokument Elemente enthält, die nicht Teil des xCard-Standards sind. | `XML:<b>kein XML-Element des xCard-Standards</b>`            |

### Zusätzliche vCard-Eigenschaften

In verschiedenen separaten Spezifikationen werden zusätzliche vCard-Eigenschaften definiert.

| Name            | Spezifikation | Beschreibung                                                 | Beispiel                                        |
| :-------------- | :-----------: | :----------------------------------------------------------- | :---------------------------------------------- |
| `BIRTHPLACE`    |   RFC 6474    | Geburtsort der Person                                        | `BIRTHPLACE;VALUE=text:Berlin\, Germany`        |
| `DEATHDATE`     |   RFC 6474    | Sterbedatum der Person                                       | `DEATHDATE:20740812`                            |
| `DEATHPLACE`    |   RFC 6474    | Sterbeort der Person                                         | `DEATHPLACE;VALUE=uri:geo:54.76,13.63`          |
| `EXPERTISE`     |   RFC 6715    | Fachgebiet, über dessen Kenntnis die Person verfügt          | `EXPERTISE;LEVEL=expert:Computer Science`       |
| `HOBBY`         |   RFC 6715    | Freizeitbeschäftigung, der die Person nachgeht               | `HOBBY;LEVEL=high:knitting`                     |
| `IMPP`          |   RFC 4770    | Handle zu einem Instant Messenger. Dieses wurde in die offizielle vCard-Spezifikation in Version 4.0 aufgenommen. | `IMPP:aim:erikamustermann@aol.com`              |
| `INTEREST`      |   RFC 6715    | Freizeitbeschäftigung, für die sich die Person interessiert, an der sie aber nicht zwangsläufig teilnimmt. | `INTEREST;LEVEL=high:baseball`                  |
| `ORG-DIRECTORY` |   RFC 6715    | URI, der den Arbeitsplatz der Person repräsentiert; damit können Informationen über Mitarbeiter der Person eingeholt werden | `ORG-DIRECTORY:http://www.firma.de/mitarbeiter` |

### vCard-Erweiterungen

vCard unterstützt individuelle Erweiterungen, diese beginnen mit dem Präfix `X-`. Einige von ihnen sind:

| Erweiterung                                               |       Nutzung als        |   Datenformat    | Verwendung                                                   |
| :-------------------------------------------------------- | :----------------------: | :--------------: | :----------------------------------------------------------- |
| Unterstützt von verschiedenen Programmen                  |                          |                  |                                                              |
| `X-ABUID`                                                 |          Objekt          |                  | UUID (Universally Unique Identifier) für diesen Eintrag im Apple-Programm Kontakte |
| `X-ANNIVERSARY`                                           |          Objekt          |    YYYY-MM-DD    | beliebiges Jubiläum (zusätzlich zu `BDAY`, Geburtstag)       |
| `X-ASSISTANT`                                             |          Objekt          |   Zeichenkette   | Assistenzname (anstelle von `AGENT`)                         |
| `X-MANAGER`                                               |          Objekt          |   Zeichenkette   | Name des Managers                                            |
| `X-SPOUSE`                                                |          Objekt          |   Zeichenkette   | Name des Ehepartners                                         |
| `X-GENDER`                                                |          Objekt          |   Zeichenkette   | Geschlecht `Male` oder `Female`                              |
| `X-AIM`                                                   |          Objekt          |   Zeichenkette   | Kontaktinformationen für Instant Messaging (IM); `TYPE`-Parameter wie für `TEL` |
| `X-ICQ`                                                   |          Objekt          |   Zeichenkette   |                                                              |
| `X-GOOGLE-TALK`                                           |          Objekt          |   Zeichenkette   |                                                              |
| `X-JABBER`                                                |          Objekt          |   Zeichenkette   |                                                              |
| `X-MSN`                                                   |          Objekt          |   Zeichenkette   |                                                              |
| `X-YAHOO`                                                 |          Objekt          |   Zeichenkette   |                                                              |
| `X-TWITTER`                                               |          Objekt          |   Zeichenkette   |                                                              |
| `X-SKYPE`, `X-SKYPE-USERNAME`                             |          Objekt          |   Zeichenkette   |                                                              |
| `X-GADUGADU`                                              |          Objekt          |   Zeichenkette   |                                                              |
| `X-GROUPWISE`                                             |          Objekt          |   Zeichenkette   |                                                              |
| `X-MS-IMADDRESS`                                          |          Objekt          |   Zeichenkette   | IM-Adresse im VCF-Attachment von Microsoft Outlook (bei Rechtsklick auf den Kontakt-Eintrag → Vollständigen Kontakt senden → Im Internetformat [vCard]) |
| `X-MS-CARDPICTURE`                                        |          Objekt          |   Zeichenkette   | genutzt als `PHOTO` oder `LOGO`; enthält das Kontaktbild aus der Kontaktkarte von Microsoft Outlook |
| `X-PHONETIC-FIRST-NAME`, `X-PHONETIC-LAST-NAME`           |          Objekt          |   Zeichenkette   | alternative Schreibweisen, zur Unterstützung bei der Aussprache unbekannter Namen |
| Eingeführt und genutzt von Mozilla; genutzt von Evolution |                          |                  |                                                              |
| `X-MOZILLA-HTML`                                          |          Objekt          |  `TRUE`/`FALSE`  | Mail-Empfänger bevorzugt HTML-formatierte E-Mails            |
| `X-MOZILLA-PROPERTY`                                      |          Objekt          |   Zeichenkette   | Thunderbird-spezifische Einstellungen, die wie folgt codiert werden: `X-MOZILLA-PROPERTY:PropertyName;PropertyValue` |
| Eingeführt und genutzt von Evolution                      |                          |                  |                                                              |
| `X-EVOLUTION-ANNIVERSARY`                                 |          Objekt          |    YYYY-MM-DD    | beliebiges Jubiläum (zusätzlich zu `BDAY`, Geburtstag)       |
| `X-EVOLUTION-ASSISTANT`                                   |          Objekt          |   Zeichenkette   | Assistenzname (anstelle von `AGENT`)                         |
| `X-EVOLUTION-BLOG-URL`                                    |          Objekt          | Zeichenkette/URL | Blog-URL                                                     |
| `X-EVOLUTION-FILE-AS`                                     |          Objekt          |   Zeichenkette   | Datei unter anderem Namen (zusätzlich zu `N`, Namensbestandteile; und `FN`, vollständiger Name) |
| `X-EVOLUTION-MANAGER`                                     |          Objekt          |   Zeichenkette   | Name des Managers                                            |
| `X-EVOLUTION-SPOUSE`                                      |          Objekt          |   Zeichenkette   | Name des Ehepartners                                         |
| `X-EVOLUTION-VIDEO-URL`                                   |          Objekt          | Zeichenkette/URL | Videochat-Adresse                                            |
| `X-EVOLUTION-CALLBACK`                                    | `TEL TYPE` Parameterwert |        –         | Rückrufnummer                                                |
| `X-EVOLUTION-RADIO`                                       | `TEL TYPE` Parameterwert |        –         | Funk-Kontaktinformation                                      |
| `X-EVOLUTION-TELEX`                                       | `TEL TYPE` Parameterwert |        –         | Telex-Kontaktinformation                                     |
| `X-EVOLUTION-TTYTDD`                                      | `TEL TYPE` Parameterwert |        –         | TTY-Schreibtelefon-Kontaktinformation *(Telecommunications device for the deaf)* |
| Eingeführt und genutzt von Kontact und KAddressbook       |                          |                  |                                                              |
| `X-KADDRESSBOOK-BlogFeed`                                 |          Objekt          | Zeichenkette/URL | Blog-URL                                                     |
| `X-KADDRESSBOOK-X-Anniversary`                            |          Objekt          |    ISO-Datum     | beliebiges Jubiläum (zusätzlich zu `BDAY`, Geburtstag)       |
| `X-KADDRESSBOOK-X-AssistantsName`                         |          Objekt          |   Zeichenkette   | Assistenzname (anstelle von `AGENT`)                         |
| `X-KADDRESSBOOK-X-IMAddress`                              |          Objekt          |   Zeichenkette   | IM-Adresse                                                   |
| `X-KADDRESSBOOK-X-ManagersName`                           |          Objekt          |   Zeichenkette   | Name des Managers                                            |
| `X-KADDRESSBOOK-X-Office`                                 |          Objekt          |   Zeichenkette   | Bürobezeichnung                                              |
| `X-KADDRESSBOOK-X-Profession`                             |          Objekt          |   Zeichenkette   | Beruf                                                        |
| `X-KADDRESSBOOK-X-SpouseName`                             |          Objekt          |   Zeichenkette   | Name des Ehepartners                                         |



### Internet Media Type (MIME-Typ)

Der standardisierte Internet Media Type einer vCard lautet ab Version 4.0 der vCard-Spezifikation:

```
text/vcard
```

## Versionen 3 und 4

VCard Version 3 und VCard Version 4 sind unterschiedliche Spezifikationen für elektronische Visitenkarten.   
Hier sind einige wichtige Unterschiede zwischen ihnen:

| Was                           | Version 3.0                                                  | Version 4.0                                                  |
| :---------------------------- | :----------------------------------------------------------- | :----------------------------------------------------------- |
| **Das Format**                | Verwendet ein einfaches textbasiertes Format mit einem festen Satz von Eigenschaften. | Verwendet ebenfalls ein textbasiertes Format und erweitert die Spezifikation um zusätzliche Eigenschaften und eine flexiblere Modellierung. |
| **Erweiterungen**             | Unterstützt Erweiterungen über benutzerdefinierte Eigenschaften mit dem Präfix `X-`. | Unterstützt Erweiterungen über benutzerdefinierte Eigenschaften mit dem Präfix `X-` sowie zusätzliche standardisierte Eigenschaften. |
| **Geografische Position**     | Unterstützt die Angabe der geografischen Position unter Verwendung der GEO-Eigenschaft im Format `Breitengrad;Längengrad`. | Unterstützt die Angabe der geografischen Position unter Verwendung der GEO-Eigenschaft als URI im Format `geo:`. |
| **Datums- und Zeitformate**   | Verwendet ein einfaches textbasiertes Format für Datums- und Zeitangaben, z. B. “JJJJMMTT”. | Unterstützt komplexere Datums- und Zeitformate, wie z.B. “JJJJ-MM-TT” und “JJJJ-MM-DTThh:mm:ssZ”. |
| **Zeichenkodierung**          | Unterstützt eine begrenzte Anzahl von Zeichenkodierungen, z. B. US-ASCII. | Unterstützt eine breitere Palette von Zeichenkodierungen, einschließlich UTF-8. |
| **Multivalued Eigenschaften** | Einige Eigenschaften, wie z. B. EMAIL und ADR, können mehrere Werte haben, sind aber nicht ausdrücklich als mehrwertig definiert. | Einige Eigenschaften, wie z. B. EMAIL und ADR, können mehrere Werte haben und sind ausdrücklich als mehrwertig definiert. |

## Beispiele

### vCard 2.1

```
BEGIN:VCARD
VERSION:2.1
N:Mustermann;Erika;;Dr.;
FN:Dr. Erika Mustermann
ORG:Wikimedia
ROLE:Kommunikation
TITLE:Redaktion & Gestaltung
PHOTO;JPEG:http://commons.wikimedia.org/wiki/File:Erika_Mustermann_2010.jpg
TEL;WORK;VOICE:(0221) 9999123
TEL;HOME;VOICE:(0221) 1234567
ADR;HOME:;;Heidestrasse 17;Koeln;;51147;Deutschland
EMAIL;PREF;INTERNET:erika@mustermann.de
REV:20140301T221110Z
END:VCARD
```

### vCard 3.0

```
BEGIN:VCARD
VERSION:3.0
N:Mustermann;Erika;;Dr.;
FN:Dr. Erika Mustermann
ORG:Wikimedia
ROLE:Kommunikation
TITLE:Redaktion & Gestaltung
PHOTO;VALUE=URL;TYPE=JPEG:http://commons.wikimedia.org/wiki/File:Erika_Mustermann_2010.jpg
TEL;TYPE=WORK,VOICE:+49 221 9999123
TEL;TYPE=HOME,VOICE:+49 221 1234567
ADR;TYPE=HOME:;;Heidestraße 17;Köln;;51147;Germany
EMAIL;TYPE=PREF,INTERNET:erika@mustermann.de
URL:http://de.wikipedia.org/
REV:2014-03-01T22:11:10Z
END:VCARD
```

### vCard 4.0

```
BEGIN:VCARD
VERSION:4.0
N:Mustermann;Erika;;Dr.;
FN:Dr. Erika Mustermann
ORG:Wikimedia
ROLE:Kommunikation
TITLE:Redaktion & Gestaltung
PHOTO;MEDIATYPE=image/jpeg:http://commons.wikimedia.org/wiki/File:Erika_Mustermann_2010.jpg
TEL;TYPE=work,voice;VALUE=uri:tel:+49-221-9999123
TEL;TYPE=home,voice;VALUE=uri:tel:+49-221-1234567
ADR;TYPE=home;LABEL="Heidestraße 17\n51147 Köln\nDeutschland":;;Heidestraße 17;Köln;;51147;Germany
EMAIL:erika@mustermann.de
REV:20140301T221110Z
END:VCARD
```

## Anwendung

Die öffentliche API der Bibliothek ist die Fassade `VCardService` über das Interface `IVCardService`.<br />
Empfohlener Einstiegspunkt: `VCardService.CreateDefault()`.

### vCard parsen und serialisieren

```csharp
using SOWI.vCard.Abstractions;
using SOWI.vCard.Services;

// Standard-Instanz mit Parser und Serializer
IVCardService service = VCardService.CreateDefault();

// vCard-Text einlesen (einzelner Block)
string vCardText = File.ReadAllText("kontakt.vcf");
var card = service.Parse(vCardText);

Console.WriteLine(card.FullName);          // z. B. Dr. Erika Mustermann
Console.WriteLine(card.Version);           // z. B. 4.0
Console.WriteLine(card.Emails[0].Address); // z. B. erika@mustermann.de

// Zurück in vCard-Text serialisieren
string output = service.Serialize(card);
File.WriteAllText("kontakt-export.vcf", output);
```

### Mehrere vCards in einer Datei

```csharp
IVCardService service = VCardService.CreateDefault();
string document = File.ReadAllText("adressbuch.vcf");

IReadOnlyList<VCard> cards = service.ParseDocument(document);
foreach (var card in cards)
{
    Console.WriteLine($"{card.FullName} ({card.Version})");
}

string merged = service.SerializeDocument(cards);
```

### Photo-Daten laden (optional)

Für URL-basierte Photos ohne eingebettete Binärdaten kann ein `IPhotoDataProvider` registriert werden.<br />
`CreateDefault(includePhotoProvider: true)` lädt bei Bedarf das eingebettete Standard-Portrait aus `Resources/Portrait.jpg`.

```csharp
var service = VCardService.CreateDefault(includePhotoProvider: true);
var card = service.Parse(vCardText);

byte[]? photoBytes = service.GetPhotoBytes(card.Photo);
if (photoBytes != null)
{
    File.WriteAllBytes("portrait.jpg", photoBytes);
}
```

### Eigene Instanz (Dependency Injection)

```csharp
using SOWI.vCard.Abstractions;
using SOWI.vCard.Parsing;
using SOWI.vCard.Serialization;
using SOWI.vCard.Services;

IVCardLineReader lineReader = new VCardLineReader();
IVCardParser parser = new VCardParser(lineReader);
IVCardSerializer serializer = new VCardSerializer();
IPhotoDataProvider photoProvider = new FilePhotoDataProvider();

var service = new VCardService(parser, serializer, photoProvider);
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

### Stream und Datei (asynchron)

Für Datei-I/O stehen asynchrone Methoden auf `IVCardService` bereit.<br />
Reines String-Parsing bleibt synchron.

```csharp
IVCardService service = VCardService.CreateDefault();

// Aus Stream lesen
await using var stream = File.OpenRead("kontakt.vcf");
var card = await service.ParseAsync(stream);

// Direkt aus Datei
var cardFromFile = await service.ParseFileAsync("kontakt.vcf");

// In Datei schreiben
await service.SerializeToFileAsync(card, "kontakt-export.vcf");
```

Weitere Details zur Architektur: `docs/SOWI.vCard.Architecture.md`.

## Quelle

[vCard – Wikipedia](https://de.wikipedia.org/wiki/VCard)  

https://www.iana.org/assignments/vcard-elements/vcard-elements.xhtml#properties
