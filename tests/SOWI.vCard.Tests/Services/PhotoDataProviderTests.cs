// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain.ValueObjects;
using SOWI.vCard.Services;

namespace SOWI.vCard.Tests.Services
{
    /// <summary>
    /// Tests für PhotoDataProvider und <see cref="VCardService.GetPhotoBytes"/>.
    /// </summary>
    public class PhotoDataProviderTests
    {
        [Fact]
        public void GetPhotoBytes_WithEmbeddedData_ReturnsPhotoData()
        {
            var service = VCardService.CreateDefault(includePhotoProvider: true);
            var photo = new Photo { Data = new byte[] { 1, 2, 3 } };

            var bytes = service.GetPhotoBytes(photo);

            Assert.Equal(new byte[] { 1, 2, 3 }, bytes);
        }

        [Fact]
        public void GetPhotoBytes_WithoutData_UsesProviderFallback()
        {
            var service = VCardService.CreateDefault(includePhotoProvider: true);
            var provider = new FilePhotoDataProvider();
            var expected = provider.LoadDefaultPortrait();

            if (expected == null)
            {
                return;
            }

            var bytes = service.GetPhotoBytes(new Photo { Value = "https://example.org/photo.jpg" });

            Assert.NotNull(bytes);
            Assert.Equal(expected.Length, bytes!.Length);
        }

        [Fact]
        public void GetPhotoBytes_WithoutProvider_ReturnsNullForUrlPhoto()
        {
            var service = VCardService.CreateDefault(includePhotoProvider: false);

            var bytes = service.GetPhotoBytes(new Photo { Value = "https://example.org/photo.jpg" });

            Assert.Null(bytes);
        }
    }
}
