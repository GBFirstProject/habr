using AutoMapper;
using FirstProject.CommentsAPI.Interfaces;
using FirstProject.CommentsAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.CommentsAPI.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsAPIController : ControllerBase
    {
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
        public async Task<IActionResult> CreateComment([FromBody] CommentDTO request, CancellationToken cts)
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
        /// Изменение комментария
        /// </summary>
        /// <param name="request">Изменяемый комментарий</param>
        /// <param name="cts"></param>
        /// <returns>Изменненый комментарий</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromBody] CommentDTO request, CancellationToken cts)
        {
            try
            {
                var result = await _repository.UpdateComment(request, cts);
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
                var result = await _repository.DeleteComment(guid, cts);

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
    }
}
