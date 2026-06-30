// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Abstractions;
using SOWI.vCard.Services;
using SOWI.vCard.Tests.Fixtures;
using SOWI.vCard.Tests.Helpers;

namespace SOWI.vCard.Tests.Integration
{
    /// <summary>
    /// Round-Trip-Tests: Parse → Serialize → Parse mit vollständiger Datenprüfung.
    /// </summary>
    public class RoundTripTests
    {
        private readonly IVCardService _service = VCardService.CreateDefault();

        [Fact]
        public void RoundTrip_VCard21_ReadmeExample_PreservesCoreData()
        {
            var original = this._service.Parse(VCardFixtures.VCard21);
            original.Uid = "roundtrip-uid-21";

            var roundTrip = this._service.Parse(this._service.Serialize(original));

            VCardEquivalenceAssertions.AssertReadmeExampleEquivalent(original, roundTrip);
        }

        [Fact]
        public void RoundTrip_VCard30_ReadmeExample_PreservesCoreData()
        {
            var original = this._service.Parse(VCardFixtures.VCard30);
            original.Uid = "roundtrip-uid-30";

            var roundTrip = this._service.Parse(this._service.Serialize(original));

            VCardEquivalenceAssertions.AssertReadmeExampleEquivalent(original, roundTrip);
        }

        [Fact]
        public void RoundTrip_VCard40_ReadmeExample_PreservesCoreData()
        {
            var original = this._service.Parse(VCardFixtures.VCard40);
            original.Uid = "roundtrip-uid-40";

            var roundTrip = this._service.Parse(this._service.Serialize(original));

            VCardEquivalenceAssertions.AssertReadmeExampleEquivalent(original, roundTrip);
        }

        [Fact]
        public void RoundTrip_VCard30_ReadmeExample_PreservesUrlAndAddressLabel()
        {
            var original = this._service.Parse(VCardFixtures.VCard30);

            var roundTrip = this._service.Parse(this._service.Serialize(original));

            Assert.Equal("http://de.wikipedia.org/", roundTrip.Url);
            Assert.Single(roundTrip.Addresses);
            Assert.Equal("Heidestraße 17", roundTrip.Addresses[0].Street);
            Assert.Equal("Köln", roundTrip.Addresses[0].Locality);
        }

        [Fact]
        public void RoundTrip_VCard40_ReadmeExample_PreservesAdrLabelAndTelUri()
        {
            var original = this._service.Parse(VCardFixtures.VCard40);

            var roundTrip = this._service.Parse(this._service.Serialize(original));

            Assert.Single(roundTrip.Addresses);
            Assert.Contains("Heidestraße 17", roundTrip.Addresses[0].Label);
            Assert.Equal(2, roundTrip.PhoneNumbers.Count);
            Assert.True(roundTrip.PhoneNumbers[0].UseUriValue);
        }

        [Fact]
        public void RoundTrip_DocumentWithMultipleCards_PreservesAllCards()
        {
            const string document = VCardFixtures.VCard30 + "\r\n" + VCardFixtures.VCard40;
            var cards = this._service.ParseDocument(document);

            var merged = this._service.SerializeDocument(cards);
            var roundTrip = this._service.ParseDocument(merged);

            Assert.Equal(2, roundTrip.Count);
            VCardEquivalenceAssertions.AssertReadmeExampleEquivalent(cards[0], roundTrip[0]);
            VCardEquivalenceAssertions.AssertReadmeExampleEquivalent(cards[1], roundTrip[1]);
        }
    }
}
