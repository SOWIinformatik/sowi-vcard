// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;
using SOWI.vCard.Services;

namespace SOWI.vCard.Tests.Parsing
{
    /// <summary>
    /// Tests für KEY, RFC-Erweiterungen, X-Properties und Photo-Provider.
    /// </summary>
    public class ExtendedPropertyTests
    {
        private readonly VCardService _service = VCardService.CreateDefault();

        [Fact]
        public void Parse_VCard40_KeyUrl_ParsesPublicKey()
        {
            const string vCard =
                "BEGIN:VCARD\r\n" +
                "VERSION:4.0\r\n" +
                "FN:Test\r\n" +
                "KEY;MEDIATYPE=application/pgp-keys:http://example.org/key.pgp\r\n" +
                "END:VCARD";

            var card = this._service.Parse(vCard);

            Assert.NotNull(card.Key);
            Assert.Equal("4.0", card.Key!.Version);
            Assert.Equal("application/pgp-keys", card.Key.Type);
            Assert.Equal("http://example.org/key.pgp", card.Key.Value);
        }

        [Fact]
        public void RoundTrip_VCard30_KeyUrl_PreservesKey()
        {
            var card = new VCard
            {
                Version = "3.0",
                FullName = "Test",
                Key = new PublicKey
                {
                    Version = "3.0",
                    Type = "PGP",
                    Value = "http://example.org/key.pgp",
                },
            };

            var roundTrip = this._service.Parse(this._service.Serialize(card));

            Assert.NotNull(roundTrip.Key);
            Assert.Equal("PGP", roundTrip.Key!.Type);
            Assert.Equal("http://example.org/key.pgp", roundTrip.Key.Value);
        }

        [Fact]
        public void Parse_VCard40_RfcExtensions_SetsProperties()
        {
            const string vCard =
                "BEGIN:VCARD\r\n" +
                "VERSION:4.0\r\n" +
                "FN:Test\r\n" +
                "BIRTHPLACE;VALUE=text:Berlin\\, Germany\r\n" +
                "DEATHDATE:20740812\r\n" +
                "DEATHPLACE;VALUE=uri:geo:54.76,13.63\r\n" +
                "EXPERTISE;LEVEL=expert:Computer Science\r\n" +
                "HOBBY;LEVEL=high:knitting\r\n" +
                "INTEREST;LEVEL=high:baseball\r\n" +
                "ORG-DIRECTORY:http://www.firma.de/mitarbeiter\r\n" +
                "END:VCARD";

            var card = this._service.Parse(vCard);

            Assert.Equal("Berlin, Germany", card.Birthplace);
            Assert.NotNull(card.DeathDate);
            Assert.Equal(2074, card.DeathDate!.Value.Year);
            Assert.Equal("geo:54.76,13.63", card.DeathPlace);
            Assert.Single(card.Expertises);
            Assert.Equal("expert", card.Expertises[0].Level);
            Assert.Equal("Computer Science", card.Expertises[0].Text);
            Assert.Single(card.Hobbies);
            Assert.Single(card.Interests);
            Assert.Equal("http://www.firma.de/mitarbeiter", card.OrgDirectory);
        }

        [Fact]
        public void Parse_XExtension_StoresInExtensionsDictionary()
        {
            const string vCard =
                "BEGIN:VCARD\r\n" +
                "VERSION:4.0\r\n" +
                "FN:Test\r\n" +
                "X-ASSISTANT:John Doe\r\n" +
                "X-MANAGER:Jane Doe\r\n" +
                "END:VCARD";

            var card = this._service.Parse(vCard);

            Assert.Equal(2, card.Extensions.Count);
            Assert.Contains(card.Extensions.Values, v => v == "X-ASSISTANT:John Doe");
            Assert.Contains(card.Extensions.Values, v => v == "X-MANAGER:Jane Doe");
            Assert.Empty(card.Others);
        }

        [Fact]
        public void RoundTrip_XExtension_PreservesExtensions()
        {
            var card = new VCard
            {
                Version = "4.0",
                FullName = "Test",
            };
            card.Extensions["X-ASSISTANT"] = "X-ASSISTANT:John Doe";

            var roundTrip = this._service.Parse(this._service.Serialize(card));

            Assert.Contains(roundTrip.Extensions.Values, v => v == "X-ASSISTANT:John Doe");
        }
    }
}
