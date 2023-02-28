using AutoMapper;
using FirstProject.CommentsAPI.Models;
using FirstProject.CommentsAPI.Models.DTO;
using FirstProject.CommentsAPI.Models.Requests;

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

            config.CreateMap<CreateRequest, CommentDTO>();
        }
    }
}
