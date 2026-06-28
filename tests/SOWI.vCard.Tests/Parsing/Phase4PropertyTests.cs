// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;
using SOWI.vCard.Services;
using SOWI.vCard.Tests.Fixtures;

namespace SOWI.vCard.Tests.Parsing
{
    /// <summary>
    /// Tests für Phase 4: ADR LABEL, Property-Handler, weitere Standard-Properties.
    /// </summary>
    public class Phase4PropertyTests
    {
        private readonly VCardService _service = VCardService.CreateDefault();

        [Fact]
        public void Parse_VCard40_ReadmeExample_ParsesAdrLabel()
        {
            var card = this._service.Parse(VCardFixtures.VCard40);

            Assert.Single(card.Addresses);
            var address = card.Addresses[0];
            Assert.Equal("home", address.Type);
            Assert.Contains("Heidestraße 17", address.Label);
            Assert.Contains("Köln", address.Label);
        }

        [Fact]
        public void RoundTrip_VCard40_AdrLabel_PreservesLabel()
        {
            var card = new VCard
            {
                Version = "4.0",
                FullName = "Test",
                Addresses =
                {
                    new Address
                    {
                        Type = "home",
                        Label = "Heidestraße 17\n51147 Köln\nDeutschland",
                        Street = "Heidestraße 17",
                        Locality = "Köln",
                        PostalCode = "51147",
                        Country = "Germany",
                    },
                },
            };

            var roundTrip = this._service.Parse(this._service.Serialize(card));

            Assert.Equal(card.Addresses[0].Label, roundTrip.Addresses[0].Label);
            Assert.Equal(card.Addresses[0].Street, roundTrip.Addresses[0].Street);
        }

        [Fact]
        public void Parse_VCard30_StandaloneLabel_AddsPostalLabel()
        {
            const string vCard =
                "BEGIN:VCARD\r\n" +
                "VERSION:3.0\r\n" +
                "FN:Test\r\n" +
                "LABEL;TYPE=HOME:Heidestrasse 17\\n51147 Koeln\\nDeutschland\r\n" +
                "END:VCARD";

            var card = this._service.Parse(vCard);

            Assert.Single(card.PostalLabels);
            Assert.Equal("HOME", card.PostalLabels[0].Type);
            Assert.Contains("Heidestrasse 17", card.PostalLabels[0].Text);
        }

        [Fact]
        public void Parse_VCard30_MailerAndProfile_SetsProperties()
        {
            const string vCard =
                "BEGIN:VCARD\r\n" +
                "VERSION:3.0\r\n" +
                "FN:Test\r\n" +
                "MAILER:Thunderbird\r\n" +
                "PROFILE:VCARD\r\n" +
                "END:VCARD";

            var card = this._service.Parse(vCard);

            Assert.Equal("Thunderbird", card.Mailer);
            Assert.Equal("VCARD", card.Profile);
        }

        [Fact]
        public void Parse_VCard40_Xml_SetsXmlProperty()
        {
            const string vCard =
                "BEGIN:VCARD\r\n" +
                "VERSION:4.0\r\n" +
                "FN:Test\r\n" +
                "XML:<b>kein XML-Element des xCard-Standards</b>\r\n" +
                "END:VCARD";

            var card = this._service.Parse(vCard);

            Assert.Equal("<b>kein XML-Element des xCard-Standards</b>", card.Xml);
        }

        [Fact]
        public void Serialize_VCard30_PostalLabel_WritesLabelLine()
        {
            var card = new VCard
            {
                Version = "3.0",
                FullName = "Test",
                PostalLabels =
                {
                    new PostalLabel { Type = "HOME", Text = "Heidestrasse 17\n51147 Koeln" },
                },
            };

            var text = this._service.Serialize(card);

            Assert.Contains("LABEL;TYPE=HOME:Heidestrasse 17\\n51147 Koeln", text);
        }
    }
}
