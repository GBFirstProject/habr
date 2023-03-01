using AutoMapper;
using Azure.Core;
using FirstProject.ArticlesAPI.Data;
using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Exceptions;
using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Models.Enums;
using FirstProject.ArticlesAPI.Models.Requests;
using FirstProject.ArticlesAPI.Services.Interfaces;
using FirstProject.ArticlesAPI.Utility;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System.Linq;

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

        public async Task<bool> DeleteArticleAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetByIdAsync(cancellationToken, id);
            // Check that the article exists and that the user is its author
            if (article == null)
            {
                throw new ArgumentException($"Статья {article.Id} не найдена");
            }
            if (article.AuthorId != userId)
            {
                throw new UnauthorizedAccessException($"Пользователь {userId} не является автором статьи {article.Id}");
            }
            _articleRepository.Delete(article);
            await _articleRepository.SaveChangesAsync(cancellationToken);
            return true;
        }
                
        public async Task<FullArticleDTO> UpdateArticleDataAsync(UpdateArticleRequest updateArticleDataDto, Guid userId, CancellationToken cancellationToken)
        {
            // Get the article by its ID
            var article = await _articleRepository.Query()
                .Include(a => a.Author)
                .Include(a => a.Tags)
                .Include(a => a.Hubs)
                .Include(a => a.LeadData)
                .FirstOrDefaultAsync(a => a.Id == updateArticleDataDto.ArticleId, cancellationToken);

            // Check that the article exists and that the user is its author
            if (article == null)
            {
                throw new ArgumentException($"Статья {updateArticleDataDto.ArticleId} не найдена");
            }
            if (article.AuthorId != userId)
            {
                throw new UnauthorizedAccessException($"Пользователь {userId} не является автором статьи {updateArticleDataDto.ArticleId}");
            }

            // Update the article properties based on the DTO
            article.Title = updateArticleDataDto.Title;
            article.TextHtml = updateArticleDataDto.TextHtml;
            article.LeadData.TextHtml = LeadDataUtilityService.GetLeadDataDescription(updateArticleDataDto.TextHtml);
            article.LeadData.ImageUrl = string.IsNullOrEmpty(updateArticleDataDto.ImageUrl)
                ? GenerateAutomaticImageUrl()
                : updateArticleDataDto.ImageUrl;
            article.CommentsEnabled = updateArticleDataDto.CommentsEnabled;
            article.IsPublished = updateArticleDataDto.IsPublished;

            // Update the article's tags and hubs
            article.Tags.Clear();
            article.Tags.AddRange(await GetOrCreateTagsAsync(updateArticleDataDto.Tags, cancellationToken));
            article.Hubs.Clear();
            article.Hubs.AddRange(await GetOrCreateHubsAsync(updateArticleDataDto.Hubs, cancellationToken));

            // Save the changes to the database
            await _articleRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<FullArticleDTO>(article);            
        }

        private async Task<List<Tag>> GetOrCreateTagsAsync(List<string> tagNames, CancellationToken cancellationToken)
        {
            var existingTags = await _tagsRepository.Query()
                .Where(t => tagNames.Contains(t.TagName))
                .ToListAsync(cancellationToken);

            var newTagNames = tagNames.Except(existingTags.Select(t => t.TagName));
            var newTags = newTagNames.Select(t => new Tag { TagName = t }).ToList();

            await _tagsRepository.AddRangeAsync(newTags, cancellationToken);
            await _tagsRepository.SaveChangesAsync(cancellationToken);

            return existingTags.Concat(newTags).ToList();
        }

        private async Task<List<Hub>> GetOrCreateHubsAsync(List<string> hubNames, CancellationToken cancellationToken)
        {
            var existingHubs = await _hubsRepository.Query()
                .Where(h => hubNames.Contains(h.Title))
                .ToListAsync(cancellationToken);

            var newHubNames = hubNames.Except(existingHubs.Select(h => h.Title));
            var newHubs = newHubNames.Select(h => new Hub { Title = h }).ToList();

            await _hubsRepository.AddRangeAsync(newHubs, cancellationToken);
            await _hubsRepository.SaveChangesAsync(cancellationToken);

            return existingHubs.Concat(newHubs).ToList();
        }

        private string GenerateAutomaticImageUrl()
        {
            // Generate a random URL for the image
            return $"https://example.com/images/{Guid.NewGuid()}.jpg";
        }
        public async Task<List<PreviewArticleDTO>> GetPreviewArticles(PagingParameters paging, CancellationToken cancellationToken)
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
                .Where(a => a.TimePublished > DateTime.UtcNow.AddMonths(-1) && a.IsPublished == true)
                .OrderByDescending(on => on.TimePublished)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)                
                .ToListAsync(cancellationToken);
            
            var articlePreviews = _mapper
                .Map<IEnumerable<PreviewArticleDTO>>(articles.Result);
            return articlePreviews.ToList();
        }
        public async Task<SearchPreviewResultDTO> GetPreviewArticleByHubLastMonthAsync(string hub, PagingParameters paging, CancellationToken cancellationToken)
        {
            var hubEntity = await _hubsRepository
                .Query()                
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(h => h.Title == hub, cancellationToken);
            if (hubEntity == null)
            {
                var previews = new List<PreviewArticleDTO>();
                return new SearchPreviewResultDTO()
                {
                    ResultData = previews,
                    Count = 0
                };
            }
            Task<List<Article>> articles = _articleRepository
                .Query()
                .AsNoTracking()
                .AsSplitQuery()
                .Include(a => a.Author)
                .Include(a => a.LeadData)
                .Include(a => a.Statistics)
                .Include(a => a.Tags)
                .Include(a => a.Hubs)
                .Include(a => a.MetaData)
                .Where(a => a.Hubs.Contains(hubEntity) && a.TimePublished >= DateTimeOffset.UtcNow.AddMonths(-1) && a.IsPublished == true)
                .OrderByDescending(on => on.TimePublished)
                .ToListAsync(cancellationToken);

            var articlePreviews = _mapper
                .Map<IEnumerable<PreviewArticleDTO>>(articles.Result);
            return new SearchPreviewResultDTO()
            {
                ResultData = articlePreviews
                    .Skip((paging.PageNumber - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .ToList(),
                Count = articlePreviews.Count()
            };
        }
        public async Task<SearchPreviewResultDTO> GetPreviewArticleByTagLastMonthAsync(string tag, PagingParameters paging, CancellationToken cancellationToken)
        {
            var tagEntity = await _tagsRepository
                .Query()
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(t => t.TagName == tag, cancellationToken);
            if (tagEntity == null)
            {
                var previews = new List<PreviewArticleDTO>();
                return new SearchPreviewResultDTO()
                {
                    ResultData = previews,
                    Count = 0
                };                
            }
            Task<List<Article>> articles = _articleRepository
                .Query()
                .AsNoTracking()
                .AsSplitQuery()
                .Include(a => a.Author)
                .Include(a => a.LeadData)
                .Include(a => a.Statistics)
                .Include(a => a.Tags)
                .Include(a => a.Hubs)
                .Include(a => a.MetaData)
                .Where(a => a.Tags.Contains(tagEntity) && a.TimePublished >= DateTimeOffset.UtcNow.AddMonths(-1) && a.IsPublished == true)
                .OrderByDescending(on => on.TimePublished)
                .ToListAsync(cancellationToken);

            var articlePreviews = _mapper
                .Map<IEnumerable<PreviewArticleDTO>>(articles.Result);
            return new SearchPreviewResultDTO()
            {
                ResultData = articlePreviews
                    .Skip((paging.PageNumber - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .ToList(),
                Count = articlePreviews.Count()
            };
        }

        public int GetArticlesCount(CancellationToken cancellationToken)
        {
            Task<List<Article>> articles = _articleRepository.Query()
                .AsNoTracking()
                .AsSplitQuery()
                .Where(a => a.TimePublished > DateTime.Now.AddMonths(-1))                
                .ToListAsync(cancellationToken);
            if (articles == null) return 0;
            else return articles.Result.Count;
        }
        public async Task<List<ArticlesByAuthorDTO>> GetArticlesTitlesByAuthorId(Guid authorId, CancellationToken cancellationToken)
        {
            Task<List<Article>> articles = _articleRepository.Query()
               .AsNoTracking()
               .AsSplitQuery()
               .Where(a => a.AuthorId == authorId)
               .OrderByDescending(on => on.TimePublished)
               .ToListAsync(cancellationToken);

            var articlesByAuthor = _mapper
                .Map<List<ArticlesByAuthorDTO>>(articles.Result);
            return articlesByAuthor;
        }

        private async Task<Tag> AddTag(Tag tag, CancellationToken cancellation)
        {
            Task<Tag?> existTag = _tagsRepository.Query()
                .FirstOrDefaultAsync(t => t.TagName == tag.TagName); 
            if (existTag.Result == null)
            {

                var newTag = await _tagsRepository.AddAsync(tag, cancellation);
                return newTag;
            }
            else
            {
                return existTag.Result;
            }
        }
        private async Task<Hub> AddHub(Hub hub, CancellationToken cancellation)
        {
            Task<Hub?> existHub = _hubsRepository.Query()
                .FirstOrDefaultAsync(h => h.Title == hub.Title);
            if (existHub.Result == null)
            {
                var newHub = await _hubsRepository.AddAsync(hub, cancellation);
                return newHub;
            }
            else
            {
                return existHub.Result;
            }
        }

        public async Task<FullArticleDTO> LikeArticle(Guid articleId, Guid userId, CancellationToken cts)
        {
            try
            {
                if (articleId == Guid.Empty)
                {
                    throw new ArgumentException("Id статьи не указан");
                }

                if (userId == Guid.Empty)
                {
                    throw new ArgumentException("Id пользователя не указан");
                }

                var entry = await _articleRepository.Query().FirstOrDefaultAsync(a => a.Id == articleId, cts);
                if (entry == null)
                {
                    throw new Exception("Статья не найдена");
                }

                if (entry.Dislikes.Contains(userId))
                {
                    entry.Dislikes.Remove(userId);
                }

                if (!entry.Likes.Contains(userId))
                {
                    entry.Likes.Add(userId);
                }
                else
                {
                    entry.Likes.Remove(userId);
                }

                await _articleRepository.SaveChangesAsync(cts);

                return _mapper.Map<FullArticleDTO>(entry);
            }
            catch
            {
                throw;
            }
        }

        public async Task<FullArticleDTO> DislikeArticle(Guid articleId, Guid userId, CancellationToken cts)
        {
            try
            {
                if (articleId == Guid.Empty)
                {
                    throw new ArgumentException("Id статьи не указан");
                }

                if (userId == Guid.Empty)
                {
                    throw new ArgumentException("Id пользователя не указан");
                }

                var entry = await _articleRepository.Query().FirstOrDefaultAsync(a => a.Id == articleId, cts);
                if (entry == null)
                {
                    throw new Exception("Статья не найдена");
                }

                if (entry.Likes.Contains(userId))
                {
                    entry.Likes.Remove(userId);
                }

                if (!entry.Dislikes.Contains(userId))
                {
                    entry.Dislikes.Add(userId);
                }
                else
                {
                    entry.Dislikes.Remove(userId);
                }

                await _articleRepository.SaveChangesAsync(cts);

                return _mapper.Map<FullArticleDTO>(entry);
            }
            catch
            {
                throw;
            }
        }
    }
}
