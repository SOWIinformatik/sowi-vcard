// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;
using SOWI.vCard.Serialization;

namespace SOWI.vCard.Tests.Serialization
{
    /// <summary>
    /// Unit-Tests für <see cref="VCardSerializer"/>.
    /// </summary>
    public class VCardSerializerTests
    {
        private readonly VCardSerializer _serializer = new();

        [Fact]
        public void Serialize_NullVCard_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this._serializer.Serialize(null!));
        }

        [Fact]
        public void SerializeDocument_NullList_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this._serializer.SerializeDocument(null!));
        }

        [Fact]
        public void SerializeDocument_EmptyList_ReturnsEmptyString()
        {
            var result = this._serializer.SerializeDocument(Array.Empty<VCard>());

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Serialize_MinimalVCard_ContainsRequiredLines()
        {
            var card = new VCard
            {
                Version = "4.0",
                FullName = "Test User",
            };

            var text = this._serializer.Serialize(card);

            Assert.StartsWith("BEGIN:VCARD\r\n", text);
            Assert.Contains("VERSION:4.0\r\n", text);
            Assert.Contains("FN:Test User\r\n", text);
            Assert.Contains("END:VCARD\r\n", text);
        }

        [Fact]
        public void Serialize_VCard30_PhoneNumber_WritesTypeParameter()
        {
            var card = new VCard
            {
                Version = "3.0",
                FullName = "Test",
                PhoneNumbers =
                {
                    new PhoneNumber { Type = "WORK,VOICE", Number = "+41 44 123 45 67" },
                },
            };

            var text = this._serializer.Serialize(card);

            Assert.Contains("TEL;TYPE=WORK,VOICE:+41 44 123 45 67", text);
        }

        [Fact]
        public void Serialize_VCard40_PhoneNumber_WritesTelUri()
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
                        Number = "+49-221-9999123",
                        UseUriValue = true,
                    },
                },
            };

            var text = this._serializer.Serialize(card);

            Assert.Contains("TEL;TYPE=work,voice;VALUE=uri:tel:+49-221-9999123", text);
        }

        [Fact]
        public void Serialize_VCard40_GeoLocation_WritesGeoUri()
        {
            var card = new VCard
            {
                Version = "4.0",
                FullName = "Test",
                Geo = new GeoLocation { Latitude = 50.858, Longitude = 7.0885, IsUriFormat = true },
            };

            var text = this._serializer.Serialize(card);

            Assert.Contains("GEO:geo:50.8580\\,7.0885", text);
        }

        [Fact]
        public void Serialize_VCard30_Birthday_WritesIsoDate()
        {
            var card = new VCard
            {
                Version = "3.0",
                FullName = "Test",
                Birthday = new DateTime(1990, 4, 12),
            };

            var text = this._serializer.Serialize(card);

            Assert.Contains("BDAY:1990-04-12", text);
        }

        [Fact]
        public void Serialize_VCard40_Birthday_WritesCompactDate()
        {
            var card = new VCard
            {
                Version = "4.0",
                FullName = "Test",
                Birthday = new DateTime(1990, 4, 12),
            };

            var text = this._serializer.Serialize(card);

            Assert.Contains("BDAY:19900412", text);
        }

        [Fact]
        public void SerializeDocument_MultipleCards_JoinsWithCrLf()
        {
            var cards = new[]
            {
                new VCard { Version = "3.0", FullName = "A", Uid = "uid-a" },
                new VCard { Version = "4.0", FullName = "B", Uid = "uid-b" },
            };

            var text = this._serializer.SerializeDocument(cards);

            Assert.Contains("FN:A", text);
            Assert.Contains("FN:B", text);
            Assert.Equal(2, text.Split("BEGIN:VCARD", StringSplitOptions.None).Length - 1);
        }

        [Fact]
        public void Serialize_Extensions_WritesOriginalLines()
        {
            var card = new VCard
            {
                Version = "4.0",
                FullName = "Test",
            };
            card.Extensions["X-ASSISTANT"] = "X-ASSISTANT:John Doe";

            var text = this._serializer.Serialize(card);

            Assert.Contains("X-ASSISTANT:John Doe", text);
        }
    }
}
