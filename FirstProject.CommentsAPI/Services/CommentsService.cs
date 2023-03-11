using AutoMapper;
using FirstProject.CommentsAPI.Data.Models.DTO;
using FirstProject.CommentsAPI.Interfaces;
using FirstProject.CommentsAPI.Utils;
using FirstProject.Messages;

namespace FirstProject.CommentsAPI.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _comments;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly CommentsCache _cache;

        public CommentsService(
            ICommentsRepository comments, 
            IMapper mapper, 
            INotificationService notificationService, 
            CommentsCache cache)
        {
            _comments = comments;
            _mapper = mapper;
            _notificationService = notificationService;
            _cache = cache;
        }

        public async Task<IEnumerable<CommentJsonDTO>> GetCommentsByArticleId(Guid articleId, int index, int count, CancellationToken cts)
        {
            try
            {
                var key = $"{articleId}-{count}-{index}";
                var result = _cache.Get(key);

                if (result == null)
                {
                    var entries = (await _comments.GetCommentsByArticleId(articleId, index, count, cts)).ToList();

                    for (int i = 0; i < entries.Count; i++)
                    {
                        var temp = (await _comments.GetCommentReplies(entries[i].Id, cts)).ToList();
                        entries.AddRange(temp);
                    }

                    List<CommentJsonDTO> json = await GenerateCommentJson(entries, Guid.Empty, cts);

                    _cache.Add(key, json);

                    return json;
                }

                return result;
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
                var result = await _comments.GetCommentsCountByArticleId(articleId, cts);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Dictionary<Guid,int>> GetCommentsCountByArticleId(Guid[] articleIds, CancellationToken cts)
        {
            try
            {
                var result = await _comments.GetCommentsCountByArticleId(articleIds, cts);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<CommentDTO> CreateComment(Guid articleId, string username, string content, Guid replyTo, CancellationToken cts)
        {
            try
            {
                CommentDTO entry = new()
                {
                    ArticleId = articleId,
                    Username = username,
                    Content = content,
                    ReplyTo = replyTo
                };
                var result = await _comments.CreateComment(entry, cts);
                _cache.Drop(articleId);

                var message = new ArticleCommented(articleId, username);
                _notificationService.SendMessage(message, cts);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteComment(Guid id, CancellationToken cts)
        {
            try
            {
                var result = await _comments.DeleteComment(id, cts);
                _cache.Drop(result);

                return true;
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
                var result = await _comments.LikeComment(commentId, username, cts);
                _cache.Drop(result.ArticleId);

                return result;
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
                var result = await _comments.DislikeComment(commentId, username, cts);
                _cache.Drop(result.ArticleId);

                return result;
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
                var result = await _comments.ChangeContentComment(commentId, content, cts);
                _cache.Drop(result.ArticleId);

                return result;
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
                var result = await _comments.GetUsernameByCommentId(commentId, cts);

                return result;
            }
            catch
            {
                throw;
            }
        }

        private async Task<List<CommentJsonDTO>> GenerateCommentJson(IEnumerable<CommentDTO> result, Guid currentEntry, CancellationToken cts)
        {
            List<CommentJsonDTO> comments = new();
            foreach (var entry in result.Where(s => s.ReplyTo == currentEntry))
            {
                var comment = _mapper.Map<CommentJsonDTO>(entry);
                comment.Replies.AddRange(await GenerateCommentJson(result, entry.Id, cts));

                comments.Add(comment);
            }
            return comments;
        }
    }
}
