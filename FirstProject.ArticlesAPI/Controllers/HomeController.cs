using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers;

[Route("api/article")]
public class HomeController : ControllerBase
{
    public HomeController()
    {
    }

    [HttpGet("testadmin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> TestAdmin()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        return new JsonResult(accessToken);
    }

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        return new JsonResult(accessToken);
    }

}