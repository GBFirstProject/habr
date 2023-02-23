using AutoMapper;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Services;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.ArticlesAPI.Controllers
{
    [Route("api/[controller]")]
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService, ILogger<BaseController> logger, IMapper mapper) : base(logger, mapper)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Выбирает ТОП тэги за последний месяц. По умолчанию 10 штук.
        /// </summary>
        /// <param name="cancellation"></param>
        /// <param name="count">Необязательный параметр. Указать количество при необходимости</param>
        /// <returns></returns>
        [HttpGet("top")]
        public async Task<IActionResult> GetTopTagsForLastMonthAsync(CancellationToken cancellation, int count = 10)
        {
            try
            {
                var tagDtos = await _tagService.GetTopTagsForLastMonthAsync(count, cancellation);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = tagDtos
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
