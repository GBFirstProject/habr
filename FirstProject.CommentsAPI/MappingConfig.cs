using AutoMapper;
using FirstProject.CommentsAPI.Models.DTO;
using FirstProject.CommentsAPI.Models;

namespace FirstProject.CommentsAPI
{
    public static class MappingConfig
    {
        public static void RegisterMaps(this IMapperConfigurationExpression config)
        {
            config.CreateMap<CommentDTO, Comment>().ReverseMap();
        }
    }
}
