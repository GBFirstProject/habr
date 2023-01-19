using FirstProject.Web.Models.Dto;
using FirstProject.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FirstProject.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> ArticleIndex()
        {
            List<ArticleDto> list = new();
            var response = await _articleService.GetAllArticlesAsync<ResponseDto>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ArticleDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> ArticleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ArticleCreate(ArticleDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _articleService.CreateArticleAsync<ResponseDto>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ArticleIndex));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ArticleEdit(int articleId)
        {
            var response = await _articleService.GetArticleByIdAsync<ResponseDto>(articleId);
            if (response != null && response.IsSuccess)
            {
                ArticleDto model = JsonConvert.DeserializeObject<ArticleDto>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ArticleEdit(ArticleDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _articleService.UpdateArticleAsync<ResponseDto>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ArticleIndex));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ArticleDelete(int articleId)
        {
            var response = await _articleService.GetArticleByIdAsync<ResponseDto>(articleId);
            if (response != null && response.IsSuccess)
            {
                ArticleDto model = JsonConvert.DeserializeObject<ArticleDto>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ArticleDelete(ArticleDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _articleService.DeleteArticleAsync<ResponseDto>(model.ArticleId);
                {
                    return RedirectToAction(nameof(ArticleIndex));
                }
            }
            return View(model);
        }
    }
}
