namespace FirstProject.CommentsAPI.Models.Requests
{
    public class CreateRequest
    {
        public Guid ArticleId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
