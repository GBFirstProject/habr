using System.IO;
using System.Linq;
using FirstProject.ArticlesAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

namespace FirstProject.ArticlesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticleImageController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Генерирует изображение по заголовку статьи и тэгам
        /// сохраняет созданное изображение на сервере и передает url созданного изображения
        /// </summary>
        /// <param name="articleData"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult GenerateImage(Article articleData)
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

                // Save the image to the server
                var fileName = $"{articleData.Id}.jpg";
                var filePath = $"~/Images/{fileName}";
                var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);

                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
                using (var stream = new FileStream(absolutePath, FileMode.Create))
                {
                    data.SaveTo(stream);
                }

                return Ok(new { imageUrl = filePath });
            }
        }
    }
}
