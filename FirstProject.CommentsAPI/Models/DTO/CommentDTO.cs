namespace FirstProject.CommentsAPI.Models.DTO
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ArticleId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<Guid> Likes { get; set; } = new();
        public List<Guid> Dislikes { get; set; } = new();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(CommentDTO))
            {
                return false;
            }

            var dto = (CommentDTO)obj;

            return Id.Equals(dto.Id) &&
                   UserId.Equals(dto.UserId) &&
                   ArticleId.Equals(dto.ArticleId) &&
                   Content.Equals(dto.Content) &&
                   CreatedAt.Equals(dto.CreatedAt) &&
                   Enumerable.SequenceEqual(Likes, dto.Likes) &&
                   Enumerable.SequenceEqual(Dislikes, dto.Dislikes);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, UserId, ArticleId, Content, CreatedAt, Likes, Dislikes);
        }
    }
}
