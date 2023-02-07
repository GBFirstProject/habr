using AutoMapper;
using FirstProject.CommentsAPI.Interfaces;
using FirstProject.CommentsAPI.Models.DTO;
using FirstProject.CommentsAPI.Models.Requests;
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

        private readonly ICommentsRepository _repository;
        private readonly ILogger<CommentsAPIController> _logger;
        private readonly IMapper _mapper;

        public CommentsAPIController(ICommentsRepository repository, ILogger<CommentsAPIController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
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
                var entries = await _repository.GetCommentsByArticleId(articleId, index, count, cts);

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
                var result = await _repository.GetCommentsCountByArticleId(guid, cts);

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
                var result = await _repository.CreateComment(_mapper.Map<CommentDTO>(request), cts);

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
        /// <returns>Изменненый комментарий</returns>
        [HttpPut("like")]
        public async Task<IActionResult> LikeComment([FromBody] GradeRequest request, CancellationToken cts)
        {
            try
            {
                var result = await _repository.LikeComment(request.CommentId, Guid.Empty, cts);
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
                var result = await _repository.DislikeComment(request.CommentId,Guid.Empty, cts);
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

                var result = await _repository.ChangeContentComment(request.CommentId, request.Content, cts);
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
        /// <param name="commentId">Guid комментария</param>
        /// <param name="cts"></param>
        /// <returns>Результат операции</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteComment(Guid commentId, CancellationToken cts)
        {
            try
            {
                if (!await IsHasRights(commentId, cts))
                {
                    return Unauthorized();
                }

                var result = await _repository.DeleteComment(commentId, cts);

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
            var userId = Guid.Parse(User.Claims.FirstOrDefault(s => s.Type == ID)!.Value);

            if (role != "Admin" && role != "Moderator")
            {
                var ownerId = await _repository.GetUserIdByCommentId(commentId, cts);
                if (userId != ownerId)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
