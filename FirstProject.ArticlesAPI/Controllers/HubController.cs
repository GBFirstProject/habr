using AutoMapper;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Services;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.ArticlesAPI.Controllers
{    
    [Route("api/[controller]")]
    public class HubController : BaseController
    {
        private readonly IHubService _hubService;        

        public HubController(IHubService hubService, ILogger<HubController> logger, IMapper mapper) : base(logger, mapper) 
        {
            _hubService = hubService;
        }

        [HttpGet("top")]
        public async Task<IActionResult> GetTopHubsLastMonth(CancellationToken cancellation, int count = 10)
        {
            try
            {
                var hubs = await _hubService.GetTopHubsLastMonth(count, cancellation);
                return Ok(new ResponseDTO()
                {
                    IsSuccess = true,
                    Result = hubs
                });                
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }

}
