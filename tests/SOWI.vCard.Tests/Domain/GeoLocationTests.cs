// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;
using SOWI.vCard.Services;

namespace SOWI.vCard.Tests.Domain
{
    /// <summary>
    /// Tests für Domänenregeln und bekannte Bugfixes.
    /// </summary>
    public class GeoLocationTests
    {
        [Fact]
        public void Serialize_IncludesSouthernHemisphereGeo()
        {
            var card = new VCard
            {
                Version = "3.0",
                FullName = "Test",
                Geo = new GeoLocation { Latitude = -33.8688, Longitude = 151.2093 },
            };

            var text = VCardService.CreateDefault().Serialize(card);

            Assert.Contains("GEO:", text);
            Assert.Contains("-33.8688", text);
            Assert.Contains("151.2093", text);
        }
    }
}
