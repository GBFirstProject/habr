using AutoMapper;
using FirstProject.CommentsAPI.Interfaces;
using FirstProject.CommentsAPI.Models;
using FirstProject.CommentsAPI.Models.DTO;
using FirstProject.Messages;

using Microsoft.EntityFrameworkCore;

namespace FirstProject.CommentsAPI.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly CommentsDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _notificationServiceUrl;

        public CommentsRepository(CommentsDbContext context, IMapper mapper, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _httpClientFactory=httpClientFactory;
            _notificationServiceUrl = configuration.GetConnectionString("NotificationService");
        }

        public async Task<CommentDTO> CreateComment(CommentDTO comment, CancellationToken cts)
        {
            try
            {
                if (comment == null)
                {
                    throw new ArgumentNullException("Comment was null");
                }

                if (comment.UserId == Guid.Empty)
                {
                    throw new ArgumentException("User Id was empty");
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
                entry.Id = Guid.NewGuid();
                entry.CreatedAt = DateTime.UtcNow;

                await _context.AddAsync(entry, cts);

                await _context.SaveChangesAsync(cts);


                /////
                var client = _httpClientFactory.CreateClient();

                var message = new ArticleCommented(comment.ArticleId, comment.UserId, comment.CreatedAt);

                var request = new HttpRequestMessage(HttpMethod.Post, _notificationServiceUrl + "/Notifications" + "/commented");

                request.Content = JsonContent.Create(message);

                await client.SendAsync(request);
                /////

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

                var entries = await _context.Comments.Where(s => s.ArticleId == articleId).ToListAsync(cts);
                entries.Sort((a, b) => b.CreatedAt.CompareTo(a.CreatedAt));

                if (index + count > entries.Count)
                {
                    var result_entries = entries.GetRange(index, entries.Count - index);
                    return _mapper.Map<IEnumerable<CommentDTO>>(result_entries);
                }
                else
                {
                    var result_entries = entries.GetRange(index, count);
                    return _mapper.Map<IEnumerable<CommentDTO>>(result_entries);
                }
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

                var entries = await _context.Comments.Where(s => s.ArticleId == articleId).ToListAsync(cts);

                return entries.Count;
            }
            catch
            {
                throw;
            }
        }

        public async Task<CommentDTO> LikeComment(Guid commentId, Guid userId, CancellationToken cts)
        {
            try
            {
                if (commentId == Guid.Empty)
                {
                    throw new ArgumentException("Comment Id was empty");
                }

                if (userId == Guid.Empty)
                {
                    throw new ArgumentException("User Id was empty");
                }

                var entry = await _context.Comments.FirstOrDefaultAsync(s => s.Id == commentId, cts);
                if (entry == null)
                {
                    throw new Exception("Comment not found");
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

                await _context.SaveChangesAsync(cts);

                return _mapper.Map<CommentDTO>(entry);
            }
            catch
            {
                throw;
            }
        }

        public async Task<CommentDTO> DislikeComment(Guid commentId, Guid userId, CancellationToken cts)
        {
            try
            {
                if (commentId == Guid.Empty)
                {
                    throw new ArgumentException("Comment Id was empty");
                }

                if (userId == Guid.Empty)
                {
                    throw new ArgumentException("User Id was empty");
                }

                var entry = await _context.Comments.FirstOrDefaultAsync(s => s.Id == commentId, cts);
                if (entry == null)
                {
                    throw new Exception("Comment not found");
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

        public async Task<bool> DeleteComment(Guid commentId, CancellationToken cts)
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
                    return false;
                }

                _context.Comments.Remove(entry);

                await _context.SaveChangesAsync(cts);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Guid> GetUserIdByCommentId(Guid commentId, CancellationToken cts)
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

                return entry.UserId;
            }
            catch
            {
                throw;
            }
        }
    }
}
