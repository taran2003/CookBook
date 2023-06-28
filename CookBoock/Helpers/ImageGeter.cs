#if ANDROID
using Microsoft.Maui.Graphics.Platform;
#endif

using System.Net;

namespace CookBoock.Helpers
{
    static class ImageGeter
    {
        public static Microsoft.Maui.Graphics.IImage GetSmallImage(Stream stream)
        {
            Microsoft.Maui.Graphics.IImage image = null;
#if ANDROID
            image = PlatformImage.FromStream(stream);
#endif
            image = image.Downsize(100, true);
            return image;
            //return null;  
        }

        public static Microsoft.Maui.Graphics.IImage GetImage(Stream stream)
        {
            Microsoft.Maui.Graphics.IImage image = null;
#if ANDROID
            image = PlatformImage.FromStream(stream);
#endif
            return image;
            //return null;  
        }

        public static Stream GetImageStreamFromUrl(string url)
        {
            var imageBytes = new HttpClient().GetByteArrayAsync(url).Result;
            return new MemoryStream(imageBytes);
        }

        public static Microsoft.Maui.Graphics.IImage GetImageFromUrl(string url)
        {
            Microsoft.Maui.Graphics.IImage image = null;
            var imageBytes = new HttpClient().GetByteArrayAsync(url).Result;
#if ANDROID
            image = PlatformImage.FromStream(new MemoryStream(imageBytes));
#endif
            image = image.Downsize(500, true);
            return image;
        }
    }
}
