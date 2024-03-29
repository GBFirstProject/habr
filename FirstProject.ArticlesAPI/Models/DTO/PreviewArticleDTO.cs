﻿namespace FirstProject.ArticlesAPI.Models.DTO
{
    public class PreviewArticleDTO
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorNickName { get; set; }
        public string Title { get; set; }        
        public string Text { get; set; }
        public string ImageURL { get; set; }    
        public DateTime TimePublished { get; set; }
        public int ReadingCount { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Hubs { get; set; }
        public List<Guid> Likes { get; set; } = new List<Guid>();
        public List<Guid> Dislikes { get; set; } = new List<Guid>();
        /// <summary>
        /// номер статьи на хабре для отладки
        /// </summary>
        public int HubrId { get; set; }
    }
}
