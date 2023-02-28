using AutoMapper;
using FirstProject.CommentsAPI.Data.Models;
using FirstProject.CommentsAPI.Data.Models.DTO;
using FirstProject.CommentsAPI.Data.Models.Requests;

namespace FirstProject.CommentsAPI
{
    public static class MappingConfig
    {
        public static void RegisterMaps(this IMapperConfigurationExpression config)
        {
            config.CreateMap<CommentDTO, Comment>();

            config.CreateMap<Comment, CommentDTO>()
                .ForMember(dto => dto.Likes, exp => exp.MapFrom(model => model.Likes.Count))
                .ForMember(dto => dto.Dislikes, exp => exp.MapFrom(model => model.Dislikes.Count));

            config.CreateMap<CommentDTO, CommentJsonDTO>();
        }
    }
}
