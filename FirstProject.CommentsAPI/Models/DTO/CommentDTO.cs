﻿namespace FirstProject.CommentsAPI.Models.DTO
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
    }
}
