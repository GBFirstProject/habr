namespace FirstProject.CommentsAPI.Data.Models.DTO
{
    public class CommentJsonDTO
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public List<CommentJsonDTO> Replies { get; set; } = new();
    }
}
