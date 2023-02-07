using AutoMapper;
using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Exceptions;
using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.Enums;
using FirstProject.ArticlesAPI.Models.Requests;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.ArticlesAPI.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<Hub> _hubsRepository;
        private readonly IMapper _mapper;
        public ArticleService(IRepository<Article> articleRepository, 
                                IRepository<Author> authorRepository, 
                                IRepository<Hub> hubsRepository, 
                                IMapper mapper)
        {            
            (_articleRepository, _authorRepository, _hubsRepository, _mapper) = (articleRepository,
                authorRepository, hubsRepository, mapper);
        }

        public async Task<Article> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.Query()
                .Include(article => article.Author)
                .Include(article => article.Hubs)
                .Include(article => article.Tags)
                .FirstOrDefaultAsync(article => article.Id == id, cancellationToken);
            if (article == null)
            {
                throw new DataNotFoundException(nameof(Article), id);
            }            
            await _articleRepository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<Article>(article);
        }
        
        public async Task<Article> CreateArticleAsync(CreateArticleRequest articleDto, CancellationToken cancellationToken)
        {
            var article = _mapper.Map<Article>(articleDto);           
            await _articleRepository.AddAsync(article, cancellationToken);
            await _articleRepository.SaveChangesAsync(cancellationToken);
            var articleModel = _mapper.Map<Article>(article);            
            return articleModel;
        }
        public async Task<bool> DeleteArticleAsync(Guid id, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetByIdAsync(cancellationToken, id);
            _articleRepository.Delete(article);
            await _articleRepository.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<Article> UpdateArticleDataAsync(UpdateArticleDataDto updateArticleDataDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
