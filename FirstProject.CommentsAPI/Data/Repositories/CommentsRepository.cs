using AutoMapper;
using FirstProject.CommentsAPI.Data.Models;
using FirstProject.CommentsAPI.Data.Models.DTO;
using FirstProject.CommentsAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace FirstProject.CommentsAPI.Data.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly CommentsDbContext _context;
        private readonly IMapper _mapper;

        public CommentsRepository(CommentsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentDTO> CreateComment(CommentDTO comment, CancellationToken cts)
        {

            try
            {
                if (comment == null)
                {
                    throw new ArgumentNullException("Comment was null");
                }

                if (comment.Username == string.Empty)
                {
                    throw new ArgumentException("Username was empty");
                }

                if (comment.ArticleId == Guid.Empty)
                {
                    throw new ArgumentException("Article Id was empty");
                }

                if (comment.Content == string.Empty)
                {
                    throw new ArgumentException("Content was empty");
                }

                var entry = _mapper.Map<Comment>(comment);
                entry.CreatedAt = DateTime.UtcNow;

                await _context.AddAsync(entry, cts);

                await _context.SaveChangesAsync(cts);

                return _mapper.Map<CommentDTO>(entry);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByArticleId(Guid articleId, int index, int count, CancellationToken cts)
        {
            try
            {
                if (articleId == Guid.Empty)
                {
                    throw new ArgumentException("Article Id was empty");
                }

                var entries = await _context.Comments
                    .AsNoTracking()
                    .AsSplitQuery()
                    .OrderByDescending(s => s.CreatedAt)
                    .Where(s => s.ArticleId == articleId)
                    .Where(s => s.ReplyTo == Guid.Empty)
                    .Skip(index)
                    .Take(count)
                    .FromCacheAsync(cts);

                return _mapper.Map<IEnumerable<CommentDTO>>(entries);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentReplies(Guid commentId, CancellationToken cts)
        {
            try
            {
                if (commentId == Guid.Empty)
                {
                    throw new ArgumentException("Comment Id was empty");
                }

                var entries = await _context.Comments
                    .AsNoTracking()
                    .AsSplitQuery()
                    .OrderByDescending(s => s.CreatedAt)
                    .Where(s => s.ReplyTo == commentId)
                    .FromCacheAsync(cts);

                return _mapper.Map<IEnumerable<CommentDTO>>(entries);
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> GetCommentsCountByArticleId(Guid articleId, CancellationToken cts)
        {
            try
            {
                if (articleId == Guid.Empty)
                {
                    throw new ArgumentException("Article Id was empty");
                }

                var result = await _context.Comments
                    .AsNoTracking()
                    .CountAsync(s => s.ArticleId == articleId, cts);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Dictionary<Guid, int>> GetCommentsCountByArticleId(Guid[] articleIds, CancellationToken cts)
        {
            try
            {
                if (!articleIds.Any())
                {
                    throw new ArgumentException("Article Ids was empty");
                }

                Dictionary<Guid, int> result = new();

                foreach (var articleId in articleIds)
                {
                    var entry = await _context.Comments
                        .AsNoTracking()
                        .CountAsync(s => s.ArticleId == articleId, cts);

                    result.Add(articleId, entry);
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<CommentDTO> LikeComment(Guid commentId, string username, CancellationToken cts)
        {
            try
            {
                if (commentId == Guid.Empty)
                {
                    throw new ArgumentException("Comment Id was empty");
                }

                if (username == string.Empty)
                {
                    throw new ArgumentException("User Id was empty");
                }

                var entry = await _context.Comments.FirstOrDefaultAsync(s => s.Id == commentId, cts);
                if (entry == null)
                {
                    throw new Exception("Comment not found");
                }

                if (entry.Dislikes.Contains(username))
                {
                    entry.Dislikes.Remove(username);
                }

                if (!entry.Likes.Contains(username))
                {
                    entry.Likes.Add(username);
                }
                else
                {
                    entry.Likes.Remove(username);
                }

                await _context.SaveChangesAsync(cts);

                return _mapper.Map<CommentDTO>(entry);
            }
            catch
            {
                throw;
            }
        }

        public async Task<CommentDTO> DislikeComment(Guid commentId, string username, CancellationToken cts)
        {
            try
            {
                if (commentId == Guid.Empty)
                {
                    throw new ArgumentException("Comment Id was empty");
                }

                if (username == string.Empty)
                {
                    throw new ArgumentException("User Id was empty");
                }

                var entry = await _context.Comments.FirstOrDefaultAsync(s => s.Id == commentId, cts);
                if (entry == null)
                {
                    throw new Exception("Comment not found");
                }

                if (entry.Likes.Contains(username))
                {
                    entry.Likes.Remove(username);
                }

                if (!entry.Dislikes.Contains(username))
                {
                    entry.Dislikes.Add(username);
                }
                else
                {
                    entry.Dislikes.Remove(username);
                }

                await _context.SaveChangesAsync(cts);

                return _mapper.Map<CommentDTO>(entry);
            }
            catch
            {
                throw;
            }
        }

        public async Task<CommentDTO> ChangeContentComment(Guid commentId, string content, CancellationToken cts)
        {
            try
            {
                if (commentId == Guid.Empty)
                {
                    throw new ArgumentException("Comment Id was empty");
                }

                if (content == string.Empty)
                {
                    throw new ArgumentException("Content was empty");
                }

                var entry = await _context.Comments.FirstOrDefaultAsync(s => s.Id == commentId, cts);
                if (entry == null)
                {
                    throw new Exception("Comment not found");
                }

                entry.Content = content;

                await _context.SaveChangesAsync(cts);

                return _mapper.Map<CommentDTO>(entry);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Guid> DeleteComment(Guid commentId, CancellationToken cts)
        {
            try
            {
                if (commentId == Guid.Empty)
                {
                    throw new ArgumentException("Comment Id was empty");
                }

                var entry = await _context.Comments.FirstOrDefaultAsync(s => s.Id == commentId, cts);
                if (entry == null)
                {
                    return Guid.Empty;
                }

                _context.Comments.Remove(entry);

                await _context.SaveChangesAsync(cts);

                return entry.ArticleId;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetUsernameByCommentId(Guid commentId, CancellationToken cts)
        {
            try
            {
                if (commentId == Guid.Empty)
                {
                    throw new ArgumentException("Comment Id was empty");
                }

                var entry = await _context.Comments.FirstOrDefaultAsync(s => s.Id == commentId, cts);
                if (entry == null)
                {
                    throw new Exception("Comment not found");
                }

                return entry.Username;
            }
            catch
            {
                throw;
            }
        }
    }
}
