// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Abstractions;
using SOWI.vCard.Domain.Exceptions;
using SOWI.vCard.Services;
using SOWI.vCard.Tests.Fixtures;

namespace SOWI.vCard.Tests.Integration
{
    /// <summary>
    /// Integrationstests mit README-Beispielen aus src/README.md.
    /// </summary>
    public class ReadmeExampleTests
    {
        private readonly IVCardService _service = VCardService.CreateDefault();

        [Fact]
        public void Parse_VCard21_SetsCoreProperties()
        {
            var card = this._service.Parse(VCardFixtures.VCard21);

            Assert.Equal("2.1", card.Version);
            Assert.Equal("Dr. Erika Mustermann", card.FullName);
            Assert.Equal("Mustermann", card.PersonName.LastName);
            Assert.Equal("Erika", card.PersonName.FirstName);
            Assert.Equal("Dr.", card.PersonName.Prefix);
            Assert.Equal("Wikimedia", card.Organization);
            Assert.Equal(2, card.PhoneNumbers.Count);
            Assert.Single(card.Addresses);
            Assert.Single(card.Emails);
        }

        [Fact]
        public void Parse_VCard30_SetsCoreProperties()
        {
            var card = this._service.Parse(VCardFixtures.VCard30);

            Assert.Equal("3.0", card.Version);
            Assert.Equal("Dr. Erika Mustermann", card.FullName);
            Assert.Equal("http://de.wikipedia.org/", card.Url);
            Assert.Equal(2, card.PhoneNumbers.Count);
        }

        [Fact]
        public void Parse_VCard40_SetsCoreProperties()
        {
            var card = this._service.Parse(VCardFixtures.VCard40);

            Assert.Equal("4.0", card.Version);
            Assert.Equal("Dr. Erika Mustermann", card.FullName);
            Assert.Equal("erika@mustermann.de", card.Emails[0].Address);
        }

        [Fact]
        public void Parse_EmptyText_ThrowsVCardParseException()
        {
            Assert.Throws<VCardParseException>(() => this._service.Parse(string.Empty));
        }

        [Fact]
        public void ParseDocument_VCard21_ReturnsSingleCard()
        {
            var cards = this._service.ParseDocument(VCardFixtures.VCard21);

            Assert.Single(cards);
            Assert.Equal("2.1", cards[0].Version);
        }

        [Fact]
        public void RoundTrip_VCard21_PreservesFullNameAndVersion()
        {
            var original = this._service.Parse(VCardFixtures.VCard21);
            original.Uid = "test-uid-roundtrip";

            var serialized = this._service.Serialize(original);
            var roundTrip = this._service.Parse(serialized);

            Assert.Equal(original.FullName, roundTrip.FullName);
            Assert.Equal(original.Version, roundTrip.Version);
            Assert.Equal(original.Uid, roundTrip.Uid);
        }
    }
}
