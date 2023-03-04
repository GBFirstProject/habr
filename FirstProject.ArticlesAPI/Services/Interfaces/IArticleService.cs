using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Models.Enums;
using FirstProject.ArticlesAPI.Models.Requests;
using System.Collections;

namespace FirstProject.ArticlesAPI.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<List<PreviewArticleDTO>> GetPreviewArticles(PagingParameters paging, CancellationToken cancellationToken);
        public Task<FullArticleDTO> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<PreviewArticleDTO> GetPreviewArticleByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<PreviewArticleDTO> GetBestArticlePreview(CancellationToken cancellationToken);
        public Task<Guid> CreateArticleAsync(CreateArticleRequest articleDto, CancellationToken cancellationToken);
        public Task<FullArticleDTO> UpdateArticleDataAsync(UpdateArticleRequest updateArticleDataDto, Guid userId, CancellationToken cancellationToken);        
        public Task<bool> DeleteArticleAsync(Guid id, Guid userId, CancellationToken cancellationToken);        
        public Task<int> GetArticlesCount(CancellationToken cancellationToken);
        public Task<SearchPreviewResultDTO> GetPreviewArticlesByHubLastMonthAsync(string hub, PagingParameters paging, CancellationToken cancellationToken);
        public Task<SearchPreviewResultDTO> GetPreviewArticlesByTagLastMonthAsync(string tag, PagingParameters paging, CancellationToken cancellationToken);
        public Task<List<ArticleTitleDTO>> GetArticlesTitlesByAuthorId(Guid authorId, CancellationToken cancellationToken);
        /// <summary>
        /// Список статей для модерации (IsPublished = false)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<ArticleTitleForModerationDTO>> GetUnpublishedArticlesForModeration(CancellationToken cancellationToken);
        public Task<FullArticleDTO> LikeArticle(Guid articleId, Guid userId, CancellationToken cts);
        public Task<FullArticleDTO> DislikeArticle(Guid articleId, Guid userId, CancellationToken cts);
    }
}
