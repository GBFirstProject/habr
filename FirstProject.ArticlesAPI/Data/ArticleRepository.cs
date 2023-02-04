using AutoMapper;
using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Models;

namespace FirstProject.ArticlesAPI.Data
{
    public class ArticleRepository : IArticlesRepository
    {
        private readonly ArticlesDBContext _dbContext;
        private readonly IMapper _mapper;

        public ArticleRepository(ArticlesDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Create(Article data)
        {
            throw new NotImplementedException();
        }

        public void Delete(Article data)
        {
            throw new NotImplementedException();
        }

        public void Edit(Article data)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Article> FindAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Article>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Article> GetArticleById(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Update(Article data)
        {
            throw new NotImplementedException();
        }
    }
}
