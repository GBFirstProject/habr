using AutoMapper;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Models.Requests;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.ArticlesAPI.Controllers
{
    /// <summary>
    /// Сервис работы со статьями
    /// </summary>
    [Authorize]
    [Route("api/articles")]    
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articlesService;
        
        /// <summary>
        /// Конструктор сервиса работы со статьями
        /// </summary>
        /// <param name="articlesService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public ArticleController(IArticleService articlesService, ILogger<ArticleController> logger, IMapper mapper) : base(logger,mapper) 
        {
            _articlesService = articlesService;            
        }

        /// <summary>
        /// Получение полной статьи по Id
        /// </summary>
        /// <param name="articleId">Guid статьи</param>
        /// <param name="token"></param>
        /// <returns>Полная статья</returns>
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        /// Постраничный вывод предварительных статей, созданных за последний месяц
        /// </summary>
        /// <param name="paging">параметры страницы</param>
        /// <param name="token">отмена</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetArticlesPreview([FromQuery] PagingParameters paging, CancellationToken token)
        {
            try
            {
                var articles = await _articlesService.GetPreviewArticles(paging, token);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = articles
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// выборка статей из базы по тэгам
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="tags"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("get-by-tags")]
        public async Task<IActionResult> GetArticlesPreviewByTag([FromQuery] PagingParameters paging, string[] tags, CancellationToken token)
        {
            try
            {
                var articles = await _articlesService.GetPreviewArticles(paging, token);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = articles
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// выборка статей из базы по хабам
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="hubs"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("get-by-hubs")]
        public async Task<IActionResult> GetArticlesPreviewByHub([FromQuery] PagingParameters paging, string[] hubs, CancellationToken token)
        {
            try
            {
                var articles = await _articlesService.GetPreviewArticles(paging, token);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = articles
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Получение количества статей созданных за последний месяц
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("get-articles-count")]
        public IActionResult GetArticlesCount(CancellationToken token)
        {
            try
            {
                int articlesCount = _articlesService.GetArticlesCount(token);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = articlesCount
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Добавляет статью
        /// </summary>
        /// <param name="request">тело статьи</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("add-article")]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleRequest request, CancellationToken cancellation)
        {
            try
            {
                var articleId = await _articlesService.CreateArticleAsync(request, cancellation);

                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = articleId
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }                
    }
}
