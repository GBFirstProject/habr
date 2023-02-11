﻿using AutoMapper;
using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Models.Requests;

namespace FirstProject.ArticlesAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Article, FullArticleDTO>()
                .ForMember(a => a.TimePublished, opt => opt.MapFrom(source => source.TimePublished.Value.Ticks))
                .ForMember(a => a.AuthorNickName, opt => opt.MapFrom(a => a.Author.NickName))
                .ForMember(a => a.FullTextHtml, opt => opt.MapFrom(a => a.TextHtml))
                .ForMember(a => a.PreviewTextHtml, opt => opt.MapFrom(a => a.LeadData.TextHtml))
                .ForMember(a => a.ReadingCount, opt => opt.MapFrom(a => a.Statistics.ReadingCount))
                .ForMember(a => a.ImageUrl, opt => opt.MapFrom(a => a.MetaData.ShareImageUrl))
                .ReverseMap();
            CreateMap<Article, PreviewArticleDTO>()
                .ForMember(a => a.TimePublished, opt => opt.MapFrom(source => source.TimePublished.Value.Ticks))
                .ForMember(a => a.AuthorNickName, opt => opt.MapFrom(a => a.Author.NickName))
                .ForMember(a => a.Text, opt => opt.MapFrom(a => a.LeadData.TextHtml))
                .ForMember(a => a.ReadingCount, opt => opt.MapFrom(a => a.Statistics.ReadingCount));
            CreateMap<CreateArticleRequest, FullArticleDTO>();            
        }
    }
}