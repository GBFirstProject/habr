﻿using Duende.Bff;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FirstProject.Web.Controllers
{
    [BffApi]
    public class ArticlesController : ControllerBase
    {
        private ILogger _logger;
        
        public ArticlesController(ILogger<ArticlesController> loger)
        {
            _logger = loger;
        }
        
        [HttpGet]
        [Route("api")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");


            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            var client = new HttpClient(httpClientHandler           );

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync(
                Config.ArticlesAPIBase + "/api/article/testAdmin");

            return new JsonResult(content);
        }

        [HttpGet]
        [Route("api/q")]
        public async Task<IActionResult> GetQ()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            _logger.LogInformation(accessToken,
                DateTime.UtcNow.ToLongTimeString());

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            var client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync(
                Config.ArticlesAPIBase + "/api/article/test");

            return new JsonResult(content);
        }
    }

}
