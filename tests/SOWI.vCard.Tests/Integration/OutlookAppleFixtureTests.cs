// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Abstractions;
using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;
using SOWI.vCard.Serialization;
using SOWI.vCard.Services;
using SOWI.vCard.Tests.Fixtures;
using SOWI.vCard.Tests.Helpers;

namespace SOWI.vCard.Tests.Integration
{
    /// <summary>
    /// Integrationstests mit Outlook- und Apple-vCard-Fixtures.
    /// </summary>
    public class OutlookAppleFixtureTests
    {
        private readonly IVCardService _service = VCardService.CreateDefault();

        [Fact]
        public void Parse_OutlookContact_SetsCoreProperties()
        {
            var card = this._service.Parse(OutlookAppleVCardFixtures.OutlookContact);

            Assert.Equal("3.0", card.Version);
            Assert.Equal("Hans Müller", card.FullName);
            Assert.Equal("Müller", card.PersonName.LastName);
            Assert.Contains("Beispiel AG", card.Organization);
            Assert.Equal("Projektleiter", card.Title);
            Assert.Equal("hans.mueller@beispiel.ch", card.Emails[0].Address);
            Assert.Equal(2, card.PhoneNumbers.Count);
            Assert.Single(card.Addresses);
            Assert.Equal("Bahnhofstrasse 1", card.Addresses[0].Street);
            Assert.Equal("Zürich", card.Addresses[0].Locality);
            Assert.Contains("Interner Kontakt", card.Note);
            Assert.NotEmpty(card.Extensions);
        }

        [Fact]
        public void Parse_AppleContact_SetsCoreProperties()
        {
            var card = this._service.Parse(OutlookAppleVCardFixtures.AppleContact);

            Assert.Equal("3.0", card.Version);
            Assert.Equal("Anna Marie Schmid", card.FullName);
            Assert.Equal("Anna", card.PersonName.FirstName);
            Assert.Equal("Marie", card.PersonName.MiddleName);
            Assert.NotNull(card.Birthday);
            Assert.Equal(1990, card.Birthday!.Value.Year);
            Assert.Equal(4, card.Birthday.Value.Month);
            Assert.Equal(12, card.Birthday.Value.Day);
            Assert.Equal("anna.schmid@example.com", card.Emails[0].Address);
            Assert.Single(card.PhoneNumbers);
            Assert.Equal("http://www.example.com", card.Url);
            Assert.Equal(2, card.Extensions.Count);
            Assert.Contains(card.Extensions.Keys, k => k.StartsWith("X-ABUID", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void RoundTrip_OutlookContact_PreservesCoreDataAndExtensions()
        {
            var original = this._service.Parse(OutlookAppleVCardFixtures.OutlookContact);

            var roundTrip = this._service.Parse(this._service.Serialize(original));

            VCardEquivalenceAssertions.AssertCoreContactDataEquivalent(original, roundTrip);
            VCardEquivalenceAssertions.AssertExtensionsPreserved(original, roundTrip);
        }

        [Fact]
        public void RoundTrip_AppleContact_PreservesCoreDataAndExtensions()
        {
            var original = this._service.Parse(OutlookAppleVCardFixtures.AppleContact);

            var roundTrip = this._service.Parse(this._service.Serialize(original));

            VCardEquivalenceAssertions.AssertCoreContactDataEquivalent(original, roundTrip);
            VCardEquivalenceAssertions.AssertExtensionsPreserved(original, roundTrip);
            Assert.NotNull(roundTrip.Birthday);
            Assert.Equal(original.Birthday, roundTrip.Birthday);
        }

        [Theory]
        [InlineData("OutlookContact.vcf")]
        [InlineData("AppleContact.vcf")]
        public void Parse_FixtureFile_ParsesSuccessfully(string fileName)
        {
            var text = OutlookAppleVCardFixtures.ReadFixtureFile(fileName);
            var card = this._service.Parse(text);

            Assert.False(string.IsNullOrEmpty(card.FullName));
            Assert.Equal("3.0", card.Version);
        }

        [Theory]
        [InlineData("OutlookContact.vcf")]
        [InlineData("AppleContact.vcf")]
        public void RoundTrip_FixtureFile_PreservesCoreData(string fileName)
        {
            var text = OutlookAppleVCardFixtures.ReadFixtureFile(fileName);
            var original = this._service.Parse(text);

            var roundTrip = this._service.Parse(this._service.Serialize(original));

            VCardEquivalenceAssertions.AssertCoreContactDataEquivalent(original, roundTrip);
            VCardEquivalenceAssertions.AssertExtensionsPreserved(original, roundTrip);
        }
    }
}
