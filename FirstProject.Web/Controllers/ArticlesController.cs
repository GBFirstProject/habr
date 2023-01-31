using Duende.Bff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FirstProject.WebNew.Controllers
{
    [Route("api")]
    [BffApi]
    public class ArticlesController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }

}
