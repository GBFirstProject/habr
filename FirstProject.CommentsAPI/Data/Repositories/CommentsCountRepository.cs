using FirstProject.CommentsAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.CommentsAPI.Data.Repositories
{
    public class CommentsCountRepository : ICommentsCountRepository
    {
        private readonly CommentsDbContext _context;

        public CommentsCountRepository(CommentsDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCount(Guid articleId, CancellationToken cts)
        {
            try
            {
                if (articleId == Guid.Empty)
                {
                    throw new ArgumentException("Article Id was empty");
                }

                var entry = await _context.CommentsCount
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.ArticleId == articleId, cts);

                if (entry == null)
                {
                    throw new Exception("Article Id not found");
                }

                return entry.Count;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> DecreaseCount(Guid articleId, CancellationToken cts)
        {
            try
            {
                if (articleId == Guid.Empty)
                {
                    throw new ArgumentException("Article Id was empty");
                }

                var entry = await _context.CommentsCount
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.ArticleId == articleId, cts);

                if (entry == null)
                {
                    throw new Exception("Article Id not found");
                }

                if (entry.Count > 0)
                {
                    entry.Count--;
                }
                await _context.SaveChangesAsync(cts);

                return entry.Count;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> IncreaseCount(Guid articleId, CancellationToken cts)
        {
            try
            {
                if (articleId == Guid.Empty)
                {
                    throw new ArgumentException("Article Id was empty");
                }

                var entry = await _context.CommentsCount
                    .FirstOrDefaultAsync(s => s.ArticleId == articleId, cts);

                if (entry == null)
                {
                    entry = new()
                    {
                        ArticleId = articleId,
                        Count = 0
                    };
                    await _context.CommentsCount.AddAsync(entry, cts);
                }

                entry.Count++;
                await _context.SaveChangesAsync(cts);

                return entry.Count;
            }
            catch
            {
                throw;
            }
        }
    }
}
