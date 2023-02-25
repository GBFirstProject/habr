using AutoMapper;
using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Models.Enums;
using FirstProject.ArticlesAPI.Models.Requests;
using FirstProject.ArticlesAPI.Utility;

namespace FirstProject.ArticlesAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Article, FullArticleDTO>()
                .ForMember(a => a.TimePublished, opt => opt.MapFrom(source => DateTime.Parse(source.TimePublished.ToString())))
                .ForMember(a => a.AuthorNickName, opt => opt.MapFrom(a => a.Author.NickName))
                .ForMember(a => a.FullTextHtml, opt => opt.MapFrom(a => a.TextHtml))
                .ForMember(a => a.PreviewTextHtml, opt => opt.MapFrom(a => HtmlSanitizeExtension.FilterHtmlToWhitelist(a.LeadData.TextHtml)))
                .ForMember(a => a.ReadingCount, opt => opt.MapFrom(a => a.Statistics.ReadingCount))
                .ForMember(a => a.HubrId, opt => opt.MapFrom(a => a.hubrId))
                .ReverseMap();
            CreateMap<Article, PreviewArticleDTO>()
                .ForMember(a => a.TimePublished, opt => opt.MapFrom(source => DateTime.Parse(source.TimePublished.ToString())))
                .ForMember(a => a.AuthorNickName, opt => opt.MapFrom(a => a.Author.NickName))
                .ForMember(a => a.Text, opt => opt.MapFrom(a => HtmlSanitizeExtension.FilterHtmlToWhitelist(a.LeadData.TextHtml)))                
                .ForMember(a => a.ImageURL, opt => opt.MapFrom(a => a.LeadData.ImageUrl == null ? a.MetaData.ShareImageUrl : a.LeadData.ImageUrl))
                .ForMember(a => a.ReadingCount, opt => opt.MapFrom(a => a.Statistics.ReadingCount))
                .ForMember(a => a.HubrId, opt => opt.MapFrom(a => a.hubrId));
            CreateMap<CreateArticleRequest, Article>()
                .ForMember(dest => dest.LeadData, opt => opt.MapFrom(src => new LeadData { ImageUrl = src.ImageUrl }))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(tag => new Tag { TagName = tag })))
                .ForMember(dest => dest.Hubs, opt => opt.MapFrom(src => src.Hubs.Select(hub => new Hub { Title = hub })));                
        }
    }
}
