namespace FirstProject.CommentsAPI.Data.Models.DTO
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; } = new();
        public int Dislikes { get; set; } = new();
        public Guid ReplyTo { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(CommentDTO))
            {
                return false;
            }

            var dto = (CommentDTO)obj;

            return Id.Equals(dto.Id) &&
                   Username.Equals(dto.Username) &&
                   ArticleId.Equals(dto.ArticleId) &&
                   Content.Equals(dto.Content) &&
                   CreatedAt.Equals(dto.CreatedAt) &&
                   Likes.Equals(dto.Likes) &&
                   Dislikes.Equals(dto.Dislikes);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Username, ArticleId, Content, CreatedAt, Likes, Dislikes);
        }
    }
}
