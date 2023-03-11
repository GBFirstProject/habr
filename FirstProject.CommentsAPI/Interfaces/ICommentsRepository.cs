using FirstProject.CommentsAPI.Data.Models.DTO;

namespace FirstProject.CommentsAPI.Interfaces
{
    public interface ICommentsRepository
    {
        Task<CommentDTO> CreateComment(CommentDTO comment, CancellationToken cts);
        Task<IEnumerable<CommentDTO>> GetCommentsByArticleId(Guid articleId, int index, int count, CancellationToken cts);
        Task<IEnumerable<CommentDTO>> GetCommentReplies(Guid commentId, CancellationToken cts);
        Task<int> GetCommentsCountByArticleId(Guid articleId, CancellationToken cts);
        Task<Dictionary<Guid, int>> GetCommentsCountByArticleId(Guid[] articleIds, CancellationToken cts);

        Task<CommentDTO> LikeComment(Guid commentId, string username, CancellationToken cts);
        Task<CommentDTO> DislikeComment(Guid commentId, string username, CancellationToken cts);
        Task<CommentDTO> ChangeContentComment(Guid commentId, string content, CancellationToken cts);
        Task<Guid> DeleteComment(Guid commentId, CancellationToken cts);

        Task<string> GetUsernameByCommentId(Guid commentId, CancellationToken cts);
    }
}
