using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.ArticlesAPI.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Tag> _tagRepository;

        public TagService(IRepository<Article> articleRepository, IRepository<Tag> tagRepository)
        {
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<TagDto>> GetTopTagsForLastMonthAsync(int count, CancellationToken cancellation)
        {            
            var tags = await _tagRepository.GetAllAsync(cancellation);
            var tagArticlesCounts = await _articleRepository.Query()
                .Where(a => a.TimePublished >= DateTime.UtcNow.AddMonths(-1))
                .SelectMany(a => a.Tags)
                .GroupBy(h => h)
                .ToDictionaryAsync(g => g.Key, g => g.Count(), cancellation);

            var topTags = tags
                .Select(t => new TagDto
                {
                    TagName = t.TagName,
                    ArticlesCount = tagArticlesCounts.GetValueOrDefault(t, 0)
                })
                .OrderByDescending(t => t.ArticlesCount)
                .Take(count);

            return topTags;
        }
    }

}
