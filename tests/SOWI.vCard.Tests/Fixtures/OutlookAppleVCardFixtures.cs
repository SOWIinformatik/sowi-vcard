// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Tests.Fixtures
{
    /// <summary>
    /// Typische vCard-Exporte aus Microsoft Outlook und Apple Kontakte (vCard 3.0).
    /// </summary>
    internal static class OutlookAppleVCardFixtures
    {
        /// <summary>
        /// Typischer Outlook-MIMEDIR-Export (Microsoft 365 / Outlook Desktop).
        /// </summary>
        internal const string OutlookContact =
            "BEGIN:VCARD\r\n" +
            "VERSION:3.0\r\n" +
            "PRODID:-//Microsoft Corporation//Outlook MIMEDIR//EN\r\n" +
            "N:Müller;Hans;;;\r\n" +
            "FN:Hans Müller\r\n" +
            "ORG:Beispiel AG;IT\r\n" +
            "TITLE:Projektleiter\r\n" +
            "EMAIL;TYPE=INTERNET;CHARSET=utf-8:hans.mueller@beispiel.ch\r\n" +
            "TEL;TYPE=WORK,VOICE:+41 44 123 45 67\r\n" +
            "TEL;TYPE=CELL:+41 79 123 45 67\r\n" +
            "ADR;TYPE=WORK:;;Bahnhofstrasse 1;Zürich;;8001;Schweiz\r\n" +
            "NOTE:Interner Kontakt\\nBitte vor Termin anrufen\r\n" +
            "UID:040000008200E00074C5B7101A82E0080000000000000000001000000A1B2C3D4E5F6789012345678901234\r\n" +
            "REV:2024-05-15T08:30:00Z\r\n" +
            "X-MS-OL-DESIGN;CHARSET=utf-8:<card xmlns=\"http://schemas.microsoft.com/office/outlook/12/outlookcard.xsd\">\r\n" +
            "END:VCARD";

        /// <summary>
        /// Typischer Apple-Kontakte-Export (macOS / iOS).
        /// </summary>
        internal const string AppleContact =
            "BEGIN:VCARD\r\n" +
            "VERSION:3.0\r\n" +
            "PRODID:-//Apple Inc.//macOS 14.5//EN\r\n" +
            "N:Schmid;Anna;Marie;;\r\n" +
            "FN:Anna Marie Schmid\r\n" +
            "BDAY:1990-04-12\r\n" +
            "EMAIL;TYPE=INTERNET;TYPE=HOME:anna.schmid@example.com\r\n" +
            "TEL;TYPE=IPHONE:+41 79 555 12 34\r\n" +
            "ADR;TYPE=HOME;TYPE=pref:;;Seestrasse 5;Bern;;3000;Schweiz\r\n" +
            "URL;TYPE=pref:http://www.example.com\r\n" +
            "NOTE:Erstellt in Kontakte\r\n" +
            "X-ABUID:ABCD1234-5678-90EF-GHIJ-KLMNOPQRSTUV\r\n" +
            "X-APPLE-OMU-URL:https://example.com/card\r\n" +
            "END:VCARD";

        internal static string ReadFixtureFile(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "Files", fileName);
            return File.ReadAllText(path);
        }
    }
}
