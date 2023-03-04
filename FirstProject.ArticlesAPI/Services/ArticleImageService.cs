using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Services.Interfaces;
using SkiaSharp;
using System.Net;

namespace FirstProject.ArticlesAPI.Services
{
    public class ArticleImageService : IArticleImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticleImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public byte[] GenerateImage(Article articleData)
        {
            // Set the image size and background color
            var width = 600;
            var height = 400;
            var backgroundColor = SKColors.White;

            // Create a new SKBitmap instance with the specified size and color
            using (var bitmap = new SKBitmap(width, height))
            {
                // Fill the background with the specified color
                using (var canvas = new SKCanvas(bitmap))
                {
                    canvas.Clear(backgroundColor);
                }

                // Add the title text to the image
                using (var paint = new SKPaint())
                {
                    paint.TextSize = 24;
                    paint.Color = SKColors.Black;
                    paint.IsAntialias = true;
                    paint.Typeface = SKTypeface.FromFamilyName("Arial");

                    string titleText = articleData.Title;
                    var titlePosition = new SKPoint(10, 10);
                    var titleTextBlob = SKTextBlob.Create(titleText, paint.ToFont());
                    using (var canvas = new SKCanvas(bitmap))
                    {
                        canvas.DrawText(titleTextBlob.ToString(), titlePosition, paint);
                    }
                }

                // Add the main themes text to the image
                using (var paint = new SKPaint())
                {
                    paint.TextSize = 18;
                    paint.Color = SKColors.Gray;
                    paint.IsAntialias = true;
                    paint.Typeface = SKTypeface.FromFamilyName("Arial");

                    var mainThemesText = string.Join(", ", articleData.Tags.Select(tag => tag.TagName));
                    var mainThemesPosition = new SKPoint(10, 50);
                    using (var textBlob = SKTextBlob.Create(mainThemesText, paint.ToFont()))
                    {
                        using (var canvas = new SKCanvas(bitmap))
                        {
                            canvas.DrawText(textBlob.ToString(), mainThemesPosition, paint);
                        }
                    }
                }                

                // Save the image to a file
                var fileName = $"{Guid.NewGuid()}.jpg";
                var filePath = Path.Combine("images", fileName);
                if(Directory.Exists("images"))
                {
                    Directory.CreateDirectory("images");
                }
                //var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
                using (var stream = new FileStream(filePath, FileMode.CreateNew))
                {
                    bitmap.Encode(SKEncodedImageFormat.Jpeg, 100).SaveTo(stream);
                }

                // Read the image bytes and return them
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    return bytes;
                }
            }
        }

        public byte[] GetImageBytes(Article articleData)
        {
            // Check if an image URL exists for the specified articleData            
            if (articleData == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(articleData.LeadData.ImageUrl))
            {
                return GenerateImage(articleData);
            }

            // Load the image bytes from the URL
            using (var client = new WebClient())
            {
                return client.DownloadData(articleData.LeadData.ImageUrl);
            }
        }
    }
}
