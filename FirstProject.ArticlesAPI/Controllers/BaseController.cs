using AutoMapper;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.ArticlesAPI.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult Error(Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO
            {
                IsSuccess = false,
                ErrorMessage = "Internal server error"
            });
        }
        
        protected readonly ILogger<BaseController> _logger;
        protected readonly IMapper _mapper;

        public BaseController(ILogger<BaseController> logger, IMapper mapper)
        {            
            _logger = logger;
            _mapper = mapper;
        }
    }
}
