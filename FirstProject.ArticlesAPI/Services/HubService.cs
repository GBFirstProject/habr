using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.ArticlesAPI.Services
{
    public class HubService : IHubService
    {
        private readonly IRepository<Hub> _hubRepository;
        private readonly IRepository<Article> _articleRepository;

        public HubService(IRepository<Hub> hubRepository, IRepository<Article> articleRepository)
        {
            _hubRepository = hubRepository;
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<HubDto>> GetTopHubsLastMonth(int count, CancellationToken cancellation)
        {
            var hubs = await _hubRepository.GetAllAsync(cancellation);
            var hubArticlesCounts = await _articleRepository.Query()
                .Where(a => a.TimePublished >= DateTime.UtcNow.AddMonths(-1))
                .SelectMany(a => a.Hubs)
                .GroupBy(h => h)
                .ToDictionaryAsync(g => g.Key, g => g.Count(), cancellation);

            var topHubs = hubs
                .Select(h => new HubDto
                {
                    Title = h.Title,
                    ArticlesCount = hubArticlesCounts.GetValueOrDefault(h, 0)
                })
                .OrderByDescending(h => h.ArticlesCount)
                .Take(count);

            return topHubs;
        }
    }

}
