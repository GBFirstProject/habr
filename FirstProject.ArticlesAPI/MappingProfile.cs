using AutoMapper;
using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Models.Requests;

namespace FirstProject.ArticlesAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Article, FullArticleDTO>();
            CreateMap<Article, PreviewArticleDTO>();
            CreateMap<CreateArticleRequest, FullArticleDTO>();            
        }
    }
}
