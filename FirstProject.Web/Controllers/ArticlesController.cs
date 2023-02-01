using Duende.Bff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FirstProject.Web.Controllers
{
    [BffApi]
    public class ArticlesController : ControllerBase
    {
        [HttpGet]
        [Route("api")]
        [Authorize(Roles = "User")]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [HttpGet]
        [Route("api/q")]
        [Authorize]
        public IActionResult GetQ()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }

}
