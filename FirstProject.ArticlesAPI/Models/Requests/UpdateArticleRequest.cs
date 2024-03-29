﻿namespace FirstProject.ArticlesAPI.Models.Requests
{
    public class UpdateArticleRequest
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; }
        public string TextHtml { get; set; }
        /// <summary>
        /// it`s for Lead Data
        /// If ImageUrl will be empty. Should generate a automatic one
        /// </summary>
        public string ImageUrl { get; set; }
        public bool CommentsEnabled { get; set; }
        public bool IsPublished { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Hubs { get; set; } = new List<string>();
    }
}
