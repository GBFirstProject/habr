﻿using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Models.Enums;
using FirstProject.ArticlesAPI.Models.Requests;
using System.Collections;

namespace FirstProject.ArticlesAPI.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<IEnumerable<PreviewArticleDTO>> GetPreviewArticles(PagingParameters paging, CancellationToken cancellationToken);
        public Task<FullArticleDTO> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<PreviewArticleDTO> GetPreviewArticleByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<Guid> CreateArticleAsync(CreateArticleRequest articleDto, CancellationToken cancellationToken);
        public Task<FullArticleDTO> UpdateArticleDataAsync(UpdateArticleDTO updateArticleDataDto, CancellationToken cancellationToken);        
        public Task<bool> DeleteArticleAsync(Guid id, CancellationToken cancellationToken);        
        public int GetArticlesCount(CancellationToken cancellationToken);
        public Task<SearchPreviewResultDTO> GetPreviewArticleByHubLastMonthAsync(string hub, PagingParameters paging, CancellationToken cancellationToken);
        public Task<SearchPreviewResultDTO> GetPreviewArticleByTagLastMonthAsync(string tag, PagingParameters paging, CancellationToken cancellationToken);
    }
}
