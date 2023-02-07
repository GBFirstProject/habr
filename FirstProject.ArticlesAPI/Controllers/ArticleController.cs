﻿using AutoMapper;
using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.ArticlesAPI.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articlesService;
        private readonly ILogger<ArticleController> _logger;
        private readonly IMapper _mapper;

        public ArticleController(IArticleService articlesService, ILogger<ArticleController> logger, IMapper mapper)
        {
            _articlesService = articlesService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение полной статьи по Id
        /// </summary>
        /// <param name="articleId">Guid статьи</param>
        /// <param name="token"></param>
        /// <returns>Полная статья</returns>
        [HttpGet]
        public async Task<IActionResult> GetArticleById(Guid articleId, CancellationToken token)
        {
            try
            {
                var entries = await _articlesService.GetArticleByIdAsync(articleId, token);

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
        private IActionResult Error(Exception ex)
        {
            _logger.LogError(ex, "Исключение");
            var response = new ResponseDTO()
            {
                IsSuccess = false,
                DisplayMessage = "Создано исключение"
            };
            response.ErrorMessage = ex.Message;
            return Ok(response);
        }
    }
}
