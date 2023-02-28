using AutoMapper;
using FirstProject.CommentsAPI.Data.Models;
using FirstProject.CommentsAPI.Data.Models.DTO;
using FirstProject.CommentsAPI.Interfaces;

namespace FirstProject.CommentsAPI.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _comments;
        private readonly ICommentsCountRepository _commentsCount;
        private readonly IMapper _mapper;

        public CommentsService(ICommentsRepository comments, ICommentsCountRepository commentsCount, IMapper mapper)
        {
            _comments = comments;
            _commentsCount = commentsCount;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentJsonDTO>> GetCommentsByArticleId(Guid articleId, int index, int count, CancellationToken cts)
        {
            try
            {
                var result = await _comments.GetCommentsByArticleId(articleId, index, count, cts);

                List<CommentJsonDTO> comments = await GenerateCommentJson(result, Guid.Empty, cts);

                return comments;
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

                await _commentsCount.IncreaseCount(articleId, cts);

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

                if (result != Guid.Empty)
                {
                    await _commentsCount.DecreaseCount(result, cts);
                }

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
                comment.Replies.AddRange(await GenerateCommentJson(result.Where(s => s.ReplyTo == entry.Id), entry.Id, cts));

                comments.Add(comment);
            }
            return comments;
        }
    }
}
