using FirstProject.ArticleAPI.Models.Dto;
using FirstProject.ArticleAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.ArticleAPI.Controllers
{
    [Route("api/articles")]
    public class ArticleAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IArticleRepository _articleRepository;

        public ArticleAPIController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
            this._response = new ResponseDto();
        }
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                IEnumerable<ArticleDto> articleDtos = await _articleRepository.GetArticles();
                _response.Result = articleDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<object> Get(int id)
        {
            try
            {
                ArticleDto articleDto = await _articleRepository.GetArticleById(id);
                _response.Result = articleDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPost]
        public async Task<object> Post([FromBody] ArticleDto articleDto)
        {
            try
            {
                ArticleDto article = await _articleRepository.CreateUpdateArticle(articleDto);
                _response.Result = article;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPut]
        public async Task<object> Put([FromBody] ArticleDto articleDto)
        {
            try
            {
                ArticleDto article = await _articleRepository.CreateUpdateArticle(articleDto);
                _response.Result = article;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<object> Delete(int id)
        {
            try
            {
                bool isSuccess = await _articleRepository.DeleteArticle(id);
                _response.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}

