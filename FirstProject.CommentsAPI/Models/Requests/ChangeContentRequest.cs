﻿namespace FirstProject.CommentsAPI.Models.Requests
{
    public class ChangeContentRequest
    {
        public Guid CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
