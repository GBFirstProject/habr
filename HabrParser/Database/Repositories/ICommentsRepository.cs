using HabrParser.Models.APIComments;

namespace HabrParser.Database.Repositories
{
    public interface ICommentsRepository
    {
        Task<Comment> CreateComment(Comment comment, CancellationToken cts);
        Task<bool> CommentsAlreadyExists(Guid articleId, CancellationToken cts);
    }
}
