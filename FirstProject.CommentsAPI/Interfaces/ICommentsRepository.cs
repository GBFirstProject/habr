using FirstProject.CommentsAPI.Models;
using FirstProject.CommentsAPI.Models.DTO;

namespace FirstProject.CommentsAPI.Interfaces
{
    public interface ICommentsRepository
    {
        Task<CommentDTO> CreateComment(CommentDTO comment, CancellationToken cts);
        Task<IEnumerable<CommentDTO>> GetCommentsByArticleId(Guid articleId, int index, int count, CancellationToken cts);
        Task<int> GetCommentsCountByArticleId(Guid articleId, CancellationToken cts);
        Task<CommentDTO> UpdateComment(CommentDTO comment, CancellationToken cts);
        Task<bool> DeleteComment(Guid id, CancellationToken cts);
    }
}
