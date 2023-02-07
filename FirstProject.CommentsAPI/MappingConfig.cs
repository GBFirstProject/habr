using AutoMapper;
using FirstProject.CommentsAPI.Models.DTO;
using FirstProject.CommentsAPI.Models;
using FirstProject.CommentsAPI.Models.Requests;

namespace FirstProject.CommentsAPI
{
    public static class MappingConfig
    {
        public static void RegisterMaps(this IMapperConfigurationExpression config)
        {
            config.CreateMap<CommentDTO, Comment>().ReverseMap();
            config.CreateMap<CreateRequest, CommentDTO>();
        }
    }
}
