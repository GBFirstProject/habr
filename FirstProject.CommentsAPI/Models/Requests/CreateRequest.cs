namespace FirstProject.CommentsAPI.Models.Requests
{
    public class CreateRequest
    {
        public Guid UserId { get; set; }
        public Guid ArticleId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
