using AutoMapper;
using Azure.Core;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Models.Requests;
using FirstProject.ArticlesAPI.Services;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FirstProject.ArticlesAPI.Controllers
{
    /// <summary>
    /// Сервис работы со статьями
    /// </summary>
    [Route("api/articles")]    
    public class ArticleController : BaseController
    {
        private const string ID = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private const string ROLE = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

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
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetArticleById(Guid articleId, CancellationToken token)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
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
        /// Постраничный вывод предварительных статей, созданных за последний месяц
        /// </summary>
        /// <param name="paging">параметры страницы</param>
        /// <param name="token">отмена</param>
        /// <returns></returns>        
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
        /// <param name="tag"></param>
        /// <param name="token"></param>
        /// <returns></returns>        
        [HttpGet("get-by-tag")]
        public async Task<IActionResult> GetArticlesPreviewByTag([FromQuery] PagingParameters paging, string tag, CancellationToken token)
        {
            try
            {
                var searchArticlesResult = await _articlesService.GetPreviewArticlesByTagLastMonthAsync(tag, paging, token);                
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = searchArticlesResult
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
        /// <param name="hub"></param>
        /// <param name="token"></param>
        /// <returns></returns>        
        [HttpGet("get-by-hub")]
        public async Task<IActionResult> GetArticlesPreviewByHub([FromQuery] PagingParameters paging, string hub, CancellationToken token)
        {
            try
            {
                var searchArticlesResult = await _articlesService.GetPreviewArticlesByHubLastMonthAsync(hub, paging, token);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = searchArticlesResult
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Получение количества статей созданных за последний месяц
        /// ДЛЯ ОТЛАДКИ В ПОЛЕ DisplayMessage будет отображаться токен авторизации
        /// </summary>
        /// <returns></returns>        
        [HttpGet("get-articles-count")]
        public async Task<IActionResult> GetArticlesCountAsync(CancellationToken token)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                int articlesCount = await _articlesService.GetArticlesCount(token);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    DisplayMessage = accessToken,
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
        [Authorize(Roles = "User,Moderator,Admin")]
        [HttpPost("add-article")]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleRequest request, CancellationToken cancellation)
        {            
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var userId = User.Claims.Where(u => u.Type == ID)?.FirstOrDefault()?.Value;
                var userNickName = User.Claims.Where(s => s.Type == "UserName")?.FirstOrDefault()?.Value;         
                if(request.AuthorId != Guid.Parse(userId))
                {
                    throw new UnauthorizedAccessException("ID пользователя в DTO не сооветствует токену авторизации");
                }
                request.AuthorNickName = userNickName;
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

        /// <summary>
        /// Обновляет статью 
        /// </summary>
        /// <param name="id">id обновляемой статьи</param>
        /// <param name="updateRequest">тело обновленной статьи</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>        
        [Authorize(Roles = "User,Moderator,Admin")]
        [HttpPut("update-article")]        
        public async Task<IActionResult> UpdateArticle(Guid id, [FromBody] UpdateArticleRequest updateRequest, CancellationToken cancellation)
        {
            try
            {
                var userId = User.Claims.Where(u => u.Type == ID)?.FirstOrDefault()?.Value;                
                var updatedArticle = await _articlesService.UpdateArticleDataAsync(updateRequest, Guid.Parse(userId), cancellation);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = updatedArticle
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        /// <summary>
        /// Удаляет статью 
        /// </summary>
        /// <param name="id">id статьи на удаление</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [Authorize(Roles = "User,Moderator,Admin")]
        [HttpDelete("delete-article")]        
        public async Task<IActionResult> DeleteArticle(Guid id, CancellationToken cancellation)
        {
            try
            {
                var userId = User.Claims.Where(u => u.Type == ID)?.FirstOrDefault()?.Value;                
                await _articlesService.DeleteArticleAsync(id, Guid.Parse(userId), cancellation);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Возвращает заголовки статей автора по ID для личного кабинета
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>        
        [HttpGet("titlesByAuthorId")]
        public async Task<IActionResult> GetArticlesTitlesByAuthorId([FromQuery] Guid authorId, CancellationToken cancellation)
        {
            try
            {
                var articlesByAuthor = await _articlesService.GetArticlesTitlesByAuthorId(authorId, cancellation);

                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = articlesByAuthor
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Лайк статьи
        /// </summary>
        /// <param name="articleId">ID статьи, которую лайкаем</param>        
        /// <param name="cts"></param>
        /// <returns>Изменненый комментарий</returns>
        [Authorize(Roles = "User,Moderator,Admin")]
        [HttpPut("like")]
        public async Task<IActionResult> LikeArticle(Guid articleId, CancellationToken cts)
        {
            try
            {
                //var userId = Guid.Parse(User.Claims.FirstOrDefault(s => s.Type == ID)!.Value);                
                var userId = User.Claims.Where(u => u.Type == ID)?.FirstOrDefault()?.Value;                
                var result = await _articlesService.LikeArticle(articleId, Guid.Parse(userId), cts);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = result
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Дизлайк статьи
        /// </summary>
        /// <param name="articleId">ID статьи, которую дизлайкаем</param>        
        /// <param name="cts"></param>
        /// <returns>Измененный комментарий</returns>
        [Authorize(Roles = "User,Moderator,Admin")]
        [HttpPut("dislike")]
        public async Task<IActionResult> DislikeArticle(Guid articleId, CancellationToken cts)
        {
            try
            {
                //var userId = Guid.Parse(User.Claims.FirstOrDefault(s => s.Type == ID)!.Value);
                var userId = User.Claims.Where(u => u.Type == ID)?.FirstOrDefault()?.Value;                
                var result = await _articlesService.DislikeArticle(articleId, Guid.Parse(userId), cts);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = result
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Получает лучшую статью. Пока - самую просматриваемую
        /// </summary>
        /// <param name="token">токен отмены</param>
        /// <returns></returns>
        [HttpGet("get-best-article")]
        public async Task<IActionResult> GetBestArticle(CancellationToken token)
        {
            try
            {
                var entity = await _articlesService.GetBestArticlePreview(token);
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
        /// Список заголовков статей с ИД и автор ИД, у которых IsPublished = false
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Authorize(Roles = "Moderator,Admin")]
        [HttpGet("get-articles-for-moderation")]
        public async Task<IActionResult> GetArticlesForModeration(CancellationToken token)
        {
            var role = User.Claims.FirstOrDefault(s => s.Type == ROLE)!.Value;
            var userId = Guid.Parse(User.Claims.FirstOrDefault(s => s.Type == ID)!.Value);
            if (role != "Admin" && role != "Moderator")
            {
                return Ok(new ResponseDTO()
                {
                    DisplayMessage = "Этот пользователь не имеет прав просматривать статьи для модерации",
                    IsSuccess = false,
                    Result = null
                });
            }
            try
            {
                var entity = await _articlesService.GetUnpublishedArticlesForModeration(token);
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
        /// Метод для модератора/админа, который одобряет статью, делают ей IsPublished = true
        /// меняет дату публикацию на ту, которая была в момент одобрения
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        [Authorize(Roles = "Moderator,Admin")]
        [HttpPut("approve-article")]
        public async Task<IActionResult> ApproveArticle(Guid articleId, CancellationToken cts)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(s => s.Type == ROLE)!.Value;
                var userId = Guid.Parse(User.Claims.FirstOrDefault(s => s.Type == ID)!.Value);
                if (role != "Admin" && role != "Moderator")
                {
                    return Ok(new ResponseDTO()
                    {
                        DisplayMessage = "Этот пользователь не имеет прав просматривать статьи для модерации",
                        IsSuccess = false,
                        Result = null
                    });
                }
                await _articlesService.ApproveArticleAsync(articleId, cts);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = null
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Модератор отклоняет статью
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        [Authorize(Roles = "Moderator,Admin")]
        [HttpPut("reject-article")]
        public async Task<IActionResult> RejectArticle(Guid articleId, CancellationToken cts)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(s => s.Type == ROLE)!.Value;
                var userId = Guid.Parse(User.Claims.FirstOrDefault(s => s.Type == ID)!.Value);
                if (role != "Admin" && role != "Moderator")
                {
                    return Ok(new ResponseDTO()
                    {
                        DisplayMessage = "Этот пользователь не имеет прав просматривать статьи для модерации",
                        IsSuccess = false,
                        Result = null
                    });
                }
                await _articlesService.RejectArticleAsync(articleId, cts);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = null
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Получает превью статей по ключевому слову
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="keyword">ключевое слово для поиска</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("get-by-keyword")]
        public async Task<IActionResult> GetArticlesPreviewByKeyword([FromQuery] PagingParameters paging, string keyword, CancellationToken token)
        {
            try
            {
                var searchArticlesResult = await _articlesService.GetPreviewArticlesByKeywordLastMonthAsync(keyword, paging, token);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = searchArticlesResult
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
