using AutoMapper;
using FirstProject.CommentsAPI.Data.Models.DTO;
using FirstProject.CommentsAPI.Data.Models.Requests;
using FirstProject.CommentsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.CommentsAPI.Controllers
{
    [Authorize]
    [Route("api/comments")]
    [ApiController]
    public class CommentsAPIController : ControllerBase
    {
        private const string ID = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private const string ROLE = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        private readonly ICommentsService _service;
        private readonly ILogger<CommentsAPIController> _logger;

        public CommentsAPIController(
            ICommentsService service,
            ILogger<CommentsAPIController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка комментариев по Guid статьи
        /// </summary>
        /// <param name="articleId">Guid статьи</param>
        /// <param name="index">Начальный индекс</param>
        /// <param name="count">Количество</param>
        /// <param name="cts"></param>
        /// <returns>Список комментариев</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCommentsByArticleId(Guid articleId, int index, int count, CancellationToken cts)
        {
            try
            {
                var entries = await _service.GetCommentsByArticleId(articleId, index, count, cts);

                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = entries
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Получение количества комментариев по Guid статьи
        /// </summary>
        /// <param name="articleId">Guid статьи</param>
        /// <param name="cts"></param>
        /// <returns>Количество комментариев</returns>
        [AllowAnonymous]
        [HttpGet("getcount")]
        public async Task<IActionResult> GetCommentsCountByArticleId(string articleId, CancellationToken cts)
        {
            try
            {
                var guid = Guid.Parse(articleId);
                var result = await _service.GetCommentsCountByArticleId(guid, cts);

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
        /// Создание комментария
        /// </summary>
        /// <param name="request">Создаваемый комментарий</param>
        /// <param name="cts"></param>
        /// <returns>Созданный комментарий</returns>
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateRequest request, CancellationToken cts)
        {
            try
            {
                var username = User.Claims.FirstOrDefault(s => s.Type == ID)!.Value;

                var result = await _service.CreateComment(request.ArticleId, username, request.Content, request.ReplyTo, cts);

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
        /// Лайк комментария
        /// </summary>
        /// <param name="request">Id комментария</param>
        /// <param name="cts"></param>
        /// <returns>Измененный комментарий</returns>
        [HttpPut("like")]
        public async Task<IActionResult> LikeComment([FromBody] GradeRequest request, CancellationToken cts)
        {
            try
            {
                var username = User.Claims.FirstOrDefault(s => s.Type == ID)!.Value;

                var result = await _service.LikeComment(request.CommentId, username, cts);

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
        /// Дизлайк комментария
        /// </summary>
        /// <param name="request">Id комментария</param>
        /// <param name="cts"></param>
        /// <returns>Измененный комментарий</returns>
        [HttpPut("dislike")]
        public async Task<IActionResult> DislikeComment([FromBody] GradeRequest request, CancellationToken cts)
        {
            try
            {
                var username = User.Claims.FirstOrDefault(s => s.Type == ID)!.Value;

                var result = await _service.DislikeComment(request.CommentId, username, cts);

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
        /// Изменение текста комментария
        /// </summary>
        /// <param name="request">Запрос на изменение</param>
        /// <param name="cts"></param>
        /// <returns>Измененный комментарий</returns>
        [HttpPut("change")]
        public async Task<IActionResult> ChangeContentComment([FromBody] ChangeContentRequest request, CancellationToken cts)
        {
            try
            {
                if (!await IsHasRights(request.CommentId, cts))
                {
                    return Unauthorized();
                }

                var result = await _service.ChangeContentComment(request.CommentId, request.Content, cts);
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
        /// Удаление комментария
        /// </summary>
        /// <param name="id">Guid комментария</param>
        /// <param name="cts"></param>
        /// <returns>Результат операции</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteComment(string id, CancellationToken cts)
        {
            try
            {
                var guid = Guid.Parse(id);
                if (!await IsHasRights(guid, cts))
                {
                    return Unauthorized();
                }

                var result = await _service.DeleteComment(guid, cts);

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

        private IActionResult Error(Exception ex)
        {
            _logger.LogError(ex, "Вызвано исключение");
            var response = new ResponseDTO()
            {
                IsSuccess = false,
                DisplayMessage = "Вызвано исключение"
            };
            response.ErrorMessages.Add(ex.Message);
            return Ok(response);
        }

        private async Task<bool> IsHasRights(Guid commentId, CancellationToken cts)
        {
            var role = User.Claims.FirstOrDefault(s => s.Type == ROLE)!.Value;
            var username = User.Claims.FirstOrDefault(s => s.Type == ID)!.Value;

            if (role != "Admin" && role != "Moderator")
            {
                var owner_username = await _service.GetUsernameByCommentId(commentId, cts);
                if (username != owner_username)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
