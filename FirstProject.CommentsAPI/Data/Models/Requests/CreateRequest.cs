namespace FirstProject.CommentsAPI.Data.Models.Requests
{
    public class CreateRequest
    {
        public Guid ArticleId { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid ReplyTo { get; set; } = Guid.Empty;
    }
}
