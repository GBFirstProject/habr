namespace FirstProject.CommentsAPI.Models.Requests
{
    public class UpdateRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ArticleId { get; set; }
        public string Content { get; set; } = string.Empty;
        public List<Guid> Likes { get; set; } = new();
        public List<Guid> Dislikes { get; set; } = new();
    }
}
