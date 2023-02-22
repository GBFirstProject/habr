using System.IO;
using System.Linq;
using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Services.Interfaces;
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
        private readonly IArticleImageService _articleImageService;
        private readonly IRepository<Article> _articleRepository;

        public ArticleImageController(IWebHostEnvironment webHostEnvironment, 
            IArticleImageService articleImageService,
            IRepository<Article> articleRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _articleImageService = articleImageService;
            _articleRepository = articleRepository;
        }

        /// <summary>
        /// Генерирует изображение по заголовку статьи и тэгам
        /// сохраняет созданное изображение на сервере и передает url созданного изображения
        /// </summary>
        /// <param name="id">Guid искомой статьи</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrGenerateImage(Guid id, CancellationToken cancellation)
        {
            // Retrieve article data from the database using the article ID
            var article = _articleRepository.GetByIdAsync(cancellation, id);
            if (article.Result == null)
            {
                return NotFound();
            }

            // Generate or get the image using the ArticleImageService
            var imageBytes = _articleImageService.GetImageBytes(article.Result);

            // Return the image as a file
            return File(imageBytes, "image/jpeg");        
        }
    }
}
