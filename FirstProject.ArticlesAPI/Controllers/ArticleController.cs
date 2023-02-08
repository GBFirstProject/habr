using AutoMapper;
using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.ArticlesAPI.Controllers
{
    /// <summary>
    /// Сервис работы со статьями
    /// </summary>
    [Route("api/articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articlesService;
        private readonly ILogger<ArticleController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор сервиса работы со статьями
        /// </summary>
        /// <param name="articlesService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public ArticleController(IArticleService articlesService, ILogger<ArticleController> logger, IMapper mapper)
        {
            _articlesService = articlesService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение полной статьи по Id
        /// </summary>
        /// <param name="articleId">Guid статьи</param>
        /// <param name="token"></param>
        /// <returns>Полная статья</returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetArticleById(Guid articleId, CancellationToken token)
        {
            try
            {
                var entity = await _articlesService.GetArticleByIdAsync(articleId, token);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = entity
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Получение сокращенной версии статьи по Id
        /// </summary>
        /// <param name="articleId">Guid статьи</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>Полная статья</returns>
        [HttpGet("get-preview-by-id")]
        public async Task<IActionResult> GetPreviewArticleById(Guid articleId, CancellationToken token)
        {
            try
            {
                var entity = await _articlesService.GetPreviewArticleByIdAsync(articleId, token);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = entity
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Тест сваггера
        /// </summary>
        /// <returns>тестовая preview статья</returns>
        [HttpGet("get-test-article")]
        public IActionResult GetTestArticle(CancellationToken token)
        {
            return Ok(new ResponseDTO()
            {
                IsSuccess = true,
                Result = new PreviewArticleDTO { Text = "test", TimePublished = 0, Title = "test_title" }
            });
        }
        private IActionResult Error(Exception ex)
        {
            _logger.LogError(ex, "Исключение");
            var response = new ResponseDTO()
            {
                IsSuccess = false,
                DisplayMessage = "Создано исключение"
            };
            response.ErrorMessage = ex.Message;
            return Ok(response);
        }
    }
}
