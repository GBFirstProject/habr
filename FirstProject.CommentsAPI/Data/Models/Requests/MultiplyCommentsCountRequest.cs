namespace FirstProject.CommentsAPI.Data.Models.Requests
{
    public class MultiplyCommentsCountRequest
    {
        public Guid[] ArticleIds { get; set; } = null!;
    }
}
