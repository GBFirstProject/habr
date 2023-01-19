using AutoMapper;
using FirstProject.ArticleAPI.Models;
using FirstProject.ArticleAPI.Models.Dto;

namespace FirstProject.ArticleAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ArticleDto, Article>();
                config.CreateMap<Article, ArticleDto>();
            });

            return mappingConfig;
        }
    }
}
