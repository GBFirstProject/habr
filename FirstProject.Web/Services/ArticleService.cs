using FirstProject.Web.Models;
using FirstProject.Web.Models.Dto;
using FirstProject.Web.Services.Interfaces;
using Newtonsoft.Json.Linq;
using static FirstProject.Web.StaticDetails;
using System;

namespace FirstProject.Web.Services
{
    public class ArticleService : BaseService, IArticleService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ArticleService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> CreateArticleAsync<T>(ArticleDto articleDto)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = articleDto,
                Url = StaticDetails.ArticleAPIBase + "/api/articles",
                AccessToken = ""
            });
        }

        public async Task<T> DeleteArticleAsync<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.ArticleAPIBase + "/api/articles/" + id,
                AccessToken = ""
            });
        }

        public async Task<T> GetAllArticlesAsync<T>()
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ArticleAPIBase + "/api/articles",
                AccessToken = ""
            });
        }

        public async Task<T> GetArticleByIdAsync<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ArticleAPIBase + "/api/articles/" + id,
                AccessToken = ""
            });
        }

        public async Task<T> UpdateArticleAsync<T>(ArticleDto articleDto)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = articleDto,
                Url = StaticDetails.ArticleAPIBase + "/api/articles",
                AccessToken = ""
            });
        }
    }
}
