﻿using AutoMapper;
using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Exceptions;
using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Models.Enums;
using FirstProject.ArticlesAPI.Models.Requests;
using FirstProject.ArticlesAPI.Services.Interfaces;
using FirstProject.ArticlesAPI.Utility;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.ArticlesAPI.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<Hub> _hubsRepository;
        private readonly IRepository<Statistics> _statisticsRepository;
        private readonly IRepository<Tag> _tagsRepository;
        private readonly IRepository<LeadData> _leadDataRepository;

        private readonly IMapper _mapper;
        public ArticleService(IRepository<Article> articleRepository, 
                                IRepository<Author> authorRepository, 
                                IRepository<Hub> hubsRepository, 
                                IRepository<Statistics> statisticsRepository,
                                IRepository<Tag> tagsRepository,
                                IRepository<LeadData> leadDataRepository,
                                IMapper mapper)
        {            
            (_articleRepository, 
                _authorRepository, 
                _hubsRepository, 
                _statisticsRepository,
                _tagsRepository,
                _leadDataRepository,
                _mapper) = 
                (articleRepository,
                authorRepository, 
                hubsRepository,
                statisticsRepository,
                tagsRepository,
                leadDataRepository,
                mapper);
        }

        public async Task<FullArticleDTO> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Article article = await ArticleById(id, cancellationToken).ConfigureAwait(false);
            return _mapper.Map<FullArticleDTO>(article);
        }
        public async Task<PreviewArticleDTO> GetPreviewArticleByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Article article = await ArticleById(id, cancellationToken).ConfigureAwait(false);
            if (article.LeadData.ImageUrl == null)
            {
                article.LeadData.ImageUrl = article.MetaData.ShareImageUrl;
            }
            return _mapper.Map<PreviewArticleDTO>(article);
        }
        private async Task<Article> ArticleById(Guid id, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.Query()
                .Include(article => article.Author)
                .Include(article => article.Hubs)
                .Include(article => article.Tags)
                .Include(article => article.LeadData)
                .Include(article => article.Statistics)
                .Include(article => article.MetaData)
                .FirstOrDefaultAsync(article => article.Id == id, cancellationToken);
            if (article == null)
            {
                throw new DataNotFoundException(nameof(Article), id);
            }
            return article;
        }


        public async Task<Guid> CreateArticleAsync(CreateArticleRequest articleDto, CancellationToken cancellationToken)
        {
            var article = _mapper.Map<Article>(articleDto);
            article.Author ??= new Author { NickName = "UNKNOWN" };
            article.Statistics = new Statistics(); 
            for (int i = 0; i < article.Tags.Count; i++)
            {
                Task<Tag> tag = AddTag(article.Tags[i], cancellationToken);
                article.Tags[i] = tag.Result;
            }
            for (int i = 0; i < article.Hubs.Count; i++)
            {
                Task <Hub> hub = AddHub(article.Hubs[i], cancellationToken);
                article.Hubs[i] = hub.Result;
            }
            await _articleRepository.AddAsync(article, cancellationToken);
            await _articleRepository.SaveChangesAsync(cancellationToken);            
            var articleModel = _mapper.Map<FullArticleDTO>(article);            
            return articleModel.Id;
        }       

        public async Task<bool> DeleteArticleAsync(Guid id, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetByIdAsync(cancellationToken, id);
            _articleRepository.Delete(article);
            await _articleRepository.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<FullArticleDTO> UpdateArticleDataAsync(UpdateArticleDTO updateArticleDataDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PreviewArticleDTO>> GetPreviewArticles(PagingParameters paging, CancellationToken cancellationToken)
        {
            Task<List<Article>> articles = _articleRepository.Query()
                .AsNoTracking()
                .AsSplitQuery()
                .Include(a => a.Author)
                .Include(a => a.LeadData)
                .Include(a => a.Statistics)
                .Include(a => a.Tags)
                .Include(a => a.Hubs)
                .Include(a => a.MetaData)
                .Where(a => a.TimePublished > DateTime.Now.AddMonths(-1))
                .OrderBy(on => on.TimePublished)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync(cancellationToken);
            foreach(Article article in articles.Result)
            {
                if(article.LeadData.ImageUrl == null)
                {
                    article.LeadData.ImageUrl = article.MetaData.ShareImageUrl;
                }
            }
            var articlePreviews = _mapper
                .Map<IEnumerable<PreviewArticleDTO>>(articles.Result);
            return articlePreviews;
        }

        public int GetArticlesCount(CancellationToken cancellationToken)
        {
            Task<List<Article>> articles = _articleRepository.Query()
                .AsNoTracking()
                .AsSplitQuery()
                .Where(a => a.TimePublished > DateTime.Now.AddMonths(-1))
                .OrderBy(on => on.TimePublished)
                .ToListAsync(cancellationToken);
            if (articles == null) return 0;
            else return articles.Result.Count;
        }

        private async Task<Tag> AddTag(Tag tag, CancellationToken cancellation)
        {
            Task<Tag> existTag = _tagsRepository.Query()
                .FirstOrDefaultAsync(t => t.TagName == tag.TagName); 
            if (existTag.Result == null)
            {

                _tagsRepository.AddAsync(tag, cancellation);
                return tag;
            }
            else
            {
                return existTag.Result;
            }
        }
        private async Task<Hub> AddHub(Hub hub, CancellationToken cancellation)
        {
            Task<Hub> existHub = _hubsRepository.Query()
                .FirstOrDefaultAsync(h => h.Title == hub.Title);
            if (existHub.Result == null)
            {
                _hubsRepository.AddAsync(hub, cancellation);
                return hub;
            }
            else
            {
                return existHub.Result;
            }
        }
    }
}
