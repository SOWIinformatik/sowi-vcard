// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

namespace SOWI.vCard.Tests.Fixtures
{
    /// <summary>
    /// vCard-Beispiele aus src/README.md.
    /// </summary>
    internal static class VCardFixtures
    {
        /// <summary>
        /// README-Beispiel vCard 2.1.
        /// </summary>
        internal const string VCard21 =
            "BEGIN:VCARD\r\n" +
            "VERSION:2.1\r\n" +
            "N:Mustermann;Erika;;Dr.;\r\n" +
            "FN:Dr. Erika Mustermann\r\n" +
            "ORG:Wikimedia\r\n" +
            "ROLE:Kommunikation\r\n" +
            "TITLE:Redaktion & Gestaltung\r\n" +
            "PHOTO;JPEG:http://commons.wikimedia.org/wiki/File:Erika_Mustermann_2010.jpg\r\n" +
            "TEL;WORK;VOICE:(0221) 9999123\r\n" +
            "TEL;HOME;VOICE:(0221) 1234567\r\n" +
            "ADR;HOME:;;Heidestrasse 17;Koeln;;51147;Deutschland\r\n" +
            "EMAIL;PREF;INTERNET:erika@mustermann.de\r\n" +
            "REV:20140301T221110Z\r\n" +
            "END:VCARD";

        /// <summary>
        /// README-Beispiel vCard 3.0.
        /// </summary>
        internal const string VCard30 =
            "BEGIN:VCARD\r\n" +
            "VERSION:3.0\r\n" +
            "N:Mustermann;Erika;;Dr.;\r\n" +
            "FN:Dr. Erika Mustermann\r\n" +
            "ORG:Wikimedia\r\n" +
            "ROLE:Kommunikation\r\n" +
            "TITLE:Redaktion & Gestaltung\r\n" +
            "PHOTO;VALUE=URL;TYPE=JPEG:http://commons.wikimedia.org/wiki/File:Erika_Mustermann_2010.jpg\r\n" +
            "TEL;TYPE=WORK,VOICE:+49 221 9999123\r\n" +
            "TEL;TYPE=HOME,VOICE:+49 221 1234567\r\n" +
            "ADR;TYPE=HOME:;;Heidestraße 17;Köln;;51147;Germany\r\n" +
            "EMAIL;TYPE=PREF,INTERNET:erika@mustermann.de\r\n" +
            "URL:http://de.wikipedia.org/\r\n" +
            "REV:2014-03-01T22:11:10Z\r\n" +
            "END:VCARD";

        /// <summary>
        /// README-Beispiel vCard 4.0.
        /// </summary>
        internal const string VCard40 =
            "BEGIN:VCARD\r\n" +
            "VERSION:4.0\r\n" +
            "N:Mustermann;Erika;;Dr.;\r\n" +
            "FN:Dr. Erika Mustermann\r\n" +
            "ORG:Wikimedia\r\n" +
            "ROLE:Kommunikation\r\n" +
            "TITLE:Redaktion & Gestaltung\r\n" +
            "PHOTO;MEDIATYPE=image/jpeg:http://commons.wikimedia.org/wiki/File:Erika_Mustermann_2010.jpg\r\n" +
            "TEL;TYPE=work,voice;VALUE=uri:tel:+49-221-9999123\r\n" +
            "TEL;TYPE=home,voice;VALUE=uri:tel:+49-221-1234567\r\n" +
            "ADR;TYPE=home;LABEL=\"Heidestraße 17\\n51147 Köln\\nDeutschland\":;;Heidestraße 17;Köln;;51147;Germany\r\n" +
            "EMAIL:erika@mustermann.de\r\n" +
            "REV:20140301T221110Z\r\n" +
            "END:VCARD";
    }
}
