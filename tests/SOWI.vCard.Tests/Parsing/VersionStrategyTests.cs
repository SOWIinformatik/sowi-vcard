// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;
using SOWI.vCard.Services;
using SOWI.vCard.Tests.Fixtures;

namespace SOWI.vCard.Tests.Parsing
{
    /// <summary>
    /// Tests für versionsspezifische Parse- und Serialize-Strategien (Phase 3).
    /// </summary>
    public class VersionStrategyTests
    {
        private readonly VCardService _service = VCardService.CreateDefault();

        [Fact]
        public void Parse_VCard40_TelUri_PreservesTelPrefix()
        {
            var card = this._service.Parse(VCardFixtures.VCard40);

            Assert.Equal(2, card.PhoneNumbers.Count);
            Assert.True(card.PhoneNumbers[0].UseUriValue);
            Assert.Equal("tel:+49-221-9999123", card.PhoneNumbers[0].Number);
            Assert.Equal("work,voice", card.PhoneNumbers[0].Type);
        }

        [Fact]
        public void Serialize_VCard40_TelUri_WritesValueUriParameter()
        {
            var card = new VCard
            {
                Version = "4.0",
                FullName = "Test",
                PhoneNumbers =
                {
                    new PhoneNumber
                    {
                        Type = "work,voice",
                        Number = "tel:+49-221-9999123",
                        UseUriValue = true,
                    },
                },
            };

            var text = this._service.Serialize(card);

            Assert.Contains("TEL;TYPE=work,voice;VALUE=uri:tel:+49-221-9999123", text);
        }

        [Fact]
        public void Parse_VCard40_GeoUri_ParsesCoordinates()
        {
            const string vCard =
                "BEGIN:VCARD\r\n" +
                "VERSION:4.0\r\n" +
                "FN:Test\r\n" +
                "GEO:geo:50.1234\\,8.5678\r\n" +
                "END:VCARD";

            var card = this._service.Parse(vCard);

            Assert.True(card.Geo.IsUriFormat);
            Assert.Equal(50.1234, card.Geo.Latitude, 4);
            Assert.Equal(8.5678, card.Geo.Longitude, 4);
        }

        [Fact]
        public void RoundTrip_VCard40_GeoUri_PreservesCoordinates()
        {
            var card = new VCard
            {
                Version = "4.0",
                FullName = "Test",
                Geo = new GeoLocation
                {
                    Latitude = 50.1234,
                    Longitude = 8.5678,
                    IsUriFormat = true,
                    Separator = ",",
                },
            };

            var roundTrip = this._service.Parse(this._service.Serialize(card));

            Assert.Equal(50.1234, roundTrip.Geo.Latitude, 4);
            Assert.Equal(8.5678, roundTrip.Geo.Longitude, 4);
        }

        [Fact]
        public void Parse_VCard40_Impp_AddsToImppsList()
        {
            const string vCard =
                "BEGIN:VCARD\r\n" +
                "VERSION:4.0\r\n" +
                "FN:Test\r\n" +
                "IMPP:xmpp:erika@mustermann.de\r\n" +
                "END:VCARD";

            var card = this._service.Parse(vCard);

            Assert.Single(card.Impps);
            Assert.Equal("xmpp:erika@mustermann.de", card.Impps[0]);
        }

        [Fact]
        public void Serialize_VCard40_Impp_WritesImppLine()
        {
            var card = new VCard
            {
                Version = "4.0",
                FullName = "Test",
                Impps = { "xmpp:erika@mustermann.de" },
            };

            var text = this._service.Serialize(card);

            Assert.Contains("IMPP:xmpp:erika@mustermann.de", text);
        }

        [Fact]
        public void Parse_VCard40_LogoAndSound_ParsesMediaProperties()
        {
            const string vCard =
                "BEGIN:VCARD\r\n" +
                "VERSION:4.0\r\n" +
                "FN:Test\r\n" +
                "LOGO;MEDIATYPE=image/png:https://example.org/logo.png\r\n" +
                "SOUND;MEDIATYPE=audio/ogg:https://example.org/name.ogg\r\n" +
                "END:VCARD";

            var card = this._service.Parse(vCard);

            Assert.NotNull(card.Logo);
            Assert.Equal("image/png", card.Logo!.Type);
            Assert.Equal("https://example.org/logo.png", card.Logo.Value);
            Assert.NotNull(card.Sound);
            Assert.Equal("audio/ogg", card.Sound!.Type);
        }
    }
}
