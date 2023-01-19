using AutoMapper;
using FirstProject.ArticleAPI.DbContexts;
using FirstProject.ArticleAPI.Models;
using FirstProject.ArticleAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.ArticleAPI.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public ArticleRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ArticleDto> CreateUpdateArticle(ArticleDto articleDto)
        {
            Article article = _mapper.Map<ArticleDto, Article>(articleDto);
            if (article.ArticleId > 0)
            {
                _db.Articles.Update(article);
            }
            else
            {
                _db.Articles.Add(article);
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<Article, ArticleDto>(article);
        }

        public async Task<bool> DeleteArticle(int articleId)
        {
            try
            {
                Article article = await _db.Articles.FirstOrDefaultAsync(u => u.ArticleId == articleId);
                if (article == null)
                {
                    return false;
                }
                _db.Articles.Remove(article);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ArticleDto> GetArticleById(int articleId)
        {
            Article article = await _db.Articles.Where(x => x.ArticleId == articleId).FirstOrDefaultAsync();
            return _mapper.Map<ArticleDto>(article);
        }

        public async Task<IEnumerable<ArticleDto>> GetArticles()
        {
            List<Article> articleList = await _db.Articles.ToListAsync();
            return _mapper.Map<List<ArticleDto>>(articleList);

        }
    }
}
