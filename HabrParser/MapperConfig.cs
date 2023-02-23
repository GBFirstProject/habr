using AutoMapper;
using HabrParser.Models.APIArticles;

namespace HabrParser
{
    internal static class MapperConfig
    {
        public static void RegisterMaps(this IMapperConfigurationExpression config)
        {
            config.CreateMap<Models.Article, Models.ArticleOnly.ParsedArticle>();
            config.CreateMap<Models.Author, Models.ArticleOnly.Author>();
            config.CreateMap<Models.Flow, Models.ArticleOnly.Flow>();
            config.CreateMap<Models.Hub, Models.ArticleOnly.Hub>();
            config.CreateMap<Models.LeadData, Models.ArticleOnly.LeadData>();
            config.CreateMap<Models.Tag, Models.ArticleOnly.Tag>();

            config.CreateMap<string, Guid>().ConvertUsing(s => LocalConverters.Str2Guid(s));

            config.CreateMap<HabrParser.Models.Article, Article>()
                .ForMember(m => m.hubrId, opt => opt.MapFrom(a => a.id))
                .ForMember(m => m.Title, opt => opt.MapFrom(a => a.titleHtml))
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.Language, opt => opt.MapFrom(a => LocalConverters.StrToLanguage(a.lang)))
                .ForMember(m => m.AuthorNickName, opt => opt.MapFrom(a => a.author.alias));
            config.CreateMap<Models.Contact, Contact>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            config.CreateMap<Models.Author, Author>()
                .ForMember(a => a.NickName, opt => opt.MapFrom(a => a.alias))
                .ForMember(a => a.FirstName, opt => opt.MapFrom(a => LocalConverters.FirstNameFromStr(a.fullname)))
                .ForMember(a => a.LastName, opt => opt.MapFrom(a => LocalConverters.LastNameFromStr(a.fullname)))
                .ForMember(a => a.hubrId, opt => opt.MapFrom(a => a.id))
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.AvatarUrl, opt => opt.MapFrom(a => a.avatarUrl.Replace("//", "http://")));
            config.CreateMap<Models.Hub, Hub>()
                .ForMember(a => a.hubrId, opt => opt.MapFrom(a => a.id))
                .ForMember(m => m.Id, opt => opt.Ignore());
            config.CreateMap<Models.LeadData, LeadData>()
                .ForMember(m => m.Id, opt => opt.Ignore());
            config.CreateMap<Models.Metadata, Metadata>()
                .ForMember(m => m.Id, opt => opt.Ignore());
            config.CreateMap<Models.Tag, Tag>()
                .ForMember(t => t.TagName, opt => opt.MapFrom(t => t.titleHtml))
                .ForMember(m => m.Id, opt => opt.Ignore());
            config.CreateMap<Models.Statistics, Statistics>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
