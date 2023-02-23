using System.IO;
using System.Linq;
using AutoMapper;
using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace FirstProject.ArticlesAPI.Controllers
{    
    [Route("api/[controller]")]
    public class ArticleImageController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IArticleImageService _articleImageService;
        private readonly IRepository<Article> _articleRepository;

        public ArticleImageController(IWebHostEnvironment webHostEnvironment, 
            IArticleImageService articleImageService,
            IRepository<Article> articleRepository,
            ILogger<ArticleImageController> logger,
            IMapper mapper) : base (logger, mapper)
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
        [HttpPost("get-article-image-by-id")]
        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrGenerateImage(Guid id, CancellationToken cancellation)
        {
            try
            {
                // Retrieve article data from the database using the article ID
                //var article = _articleRepository.GetByIdAsync(cancellation, id);
                var article = await _articleRepository.Query()                    
                    .Include(article => article.Hubs)
                    .Include(article => article.Tags)                    
                    .FirstOrDefaultAsync(article => article.Id == id, cancellation);
                if (article == null)
                {
                    return Ok(new ResponseDTO()
                    {
                        IsSuccess = false,
                        Result = "статья не найдена"
                    });
                }                

                // Generate or get the image using the ArticleImageService
                var imageBytes = _articleImageService.GetImageBytes(article);

                // Return the image as a file
                return File(imageBytes, "image/jpeg");
            }
            catch(Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
