namespace FirstProject.CommentsAPI.Models.DTO
{
    public class CommentDTO
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid ArticleId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public List<Guid> Likes { get; set; } = new();
        public List<Guid> Dislikes { get; set; } = new();
    }
}
