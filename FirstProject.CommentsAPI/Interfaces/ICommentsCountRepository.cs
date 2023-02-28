namespace FirstProject.CommentsAPI.Interfaces
{
    public interface ICommentsCountRepository
    {
        Task<int> GetCount(Guid articleId, CancellationToken cts);
        Task<int> IncreaseCount(Guid articleId, CancellationToken cts);
        Task<int> DecreaseCount(Guid articleId, CancellationToken cts);
    }
}
