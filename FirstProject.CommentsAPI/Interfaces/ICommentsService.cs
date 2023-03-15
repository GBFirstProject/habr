using FirstProject.CommentsAPI.Data.Models.DTO;

namespace FirstProject.CommentsAPI.Interfaces
{
    public interface ICommentsService
    {
        Task<IEnumerable<CommentJsonDTO>> GetCommentsByArticleId(Guid articleId, int index, int count, CancellationToken cts);
        Task<int> GetCommentsCountByArticleId(Guid articleId, CancellationToken cts);
        Task<int> GetRootCommentsCountByArticleId(Guid articleId, CancellationToken cts);
        Task<Dictionary<Guid, int>> GetCommentsCountByArticleId(Guid[] articleIds, CancellationToken cts);
        Task<CommentDTO> CreateComment(Guid articleId, string username, string content, Guid replyTo, CancellationToken cts);
        Task<CommentDTO> LikeComment(Guid commentId, string username, CancellationToken cts);
        Task<CommentDTO> DislikeComment(Guid commentId, string username, CancellationToken cts);
        Task<CommentDTO> ChangeContentComment(Guid commentId, string content, CancellationToken cts);
        Task<bool> DeleteComment(Guid id, CancellationToken cts);
        Task<string> GetUsernameByCommentId(Guid commentId, CancellationToken cts);
    }
}
