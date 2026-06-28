// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;
using SOWI.vCard.Services;
using SOWI.vCard.Tests.Fixtures;

namespace SOWI.vCard.Tests.Integration
{
    /// <summary>
    /// Tests für Phase 6: Stream-/Datei-API, NAME, KEY-Binärdaten.
    /// </summary>
    public class Phase6StreamAndFinalizeTests
    {
        private readonly VCardService _service = VCardService.CreateDefault();

        [Fact]
        public async Task ParseAsync_FromMemoryStream_ParsesVCard()
        {
            await using var stream = new MemoryStream(
                System.Text.Encoding.UTF8.GetBytes(VCardFixtures.VCard30));

            var card = await this._service.ParseAsync(stream);

            Assert.Equal("3.0", card.Version);
            Assert.Equal("Dr. Erika Mustermann", card.FullName);
        }

        [Fact]
        public async Task ParseDocumentAsync_FromStream_ReturnsAllCards()
        {
            const string document =
                VCardFixtures.VCard21 + "\r\n" + VCardFixtures.VCard30;

            await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(document));

            var cards = await this._service.ParseDocumentAsync(stream);

            Assert.Equal(2, cards.Count);
        }

        [Fact]
        public async Task ParseFileAsync_AndSerializeToFileAsync_RoundTrips()
        {
            var inputPath = Path.GetTempFileName();
            var outputPath = Path.GetTempFileName();

            try
            {
                await File.WriteAllTextAsync(inputPath, VCardFixtures.VCard40);

                var card = await this._service.ParseFileAsync(inputPath);
                card.Note = "Datei-Roundtrip-Test";

                await this._service.SerializeToFileAsync(card, outputPath);

                var roundTrip = await this._service.ParseFileAsync(outputPath);

                Assert.Equal(card.FullName, roundTrip.FullName);
                Assert.Equal(card.Note, roundTrip.Note);
            }
            finally
            {
                File.Delete(inputPath);
                File.Delete(outputPath);
            }
        }

        [Fact]
        public void Parse_VCard30_Name_SetsNameProperty()
        {
            const string vCard =
                "BEGIN:VCARD\r\n" +
                "VERSION:3.0\r\n" +
                "FN:Test\r\n" +
                "SOURCE:http://mustermann.de/vCard.vcf\r\n" +
                "NAME:Mustermann vCard\r\n" +
                "END:VCARD";

            var card = this._service.Parse(vCard);

            Assert.Equal("http://mustermann.de/vCard.vcf", card.Source);
            Assert.Equal("Mustermann vCard", card.Name);
        }

        [Fact]
        public void RoundTrip_VCard40_KeyDataUri_PreservesBinaryKey()
        {
            var card = new VCard
            {
                Version = "4.0",
                FullName = "Test",
                Key = new PublicKey
                {
                    Version = "4.0",
                    Type = "application/pgp-keys",
                    Data = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF },
                },
            };

            var roundTrip = this._service.Parse(this._service.Serialize(card));

            Assert.NotNull(roundTrip.Key?.Data);
            Assert.Equal(card.Key!.Data, roundTrip.Key!.Data);
        }

        [Fact]
        public void RoundTrip_VCard30_KeyBase64_PreservesBinaryKey()
        {
            var card = new VCard
            {
                Version = "3.0",
                FullName = "Test",
                Key = new PublicKey
                {
                    Version = "3.0",
                    Type = "PGP",
                    Encoding = "b",
                    Data = new byte[] { 1, 2, 3 },
                },
            };

            var roundTrip = this._service.Parse(this._service.Serialize(card));

            Assert.NotNull(roundTrip.Key?.Data);
            Assert.Equal(new byte[] { 1, 2, 3 }, roundTrip.Key!.Data);
        }
    }
}
