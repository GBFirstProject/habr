namespace FirstProject.ArticlesAPI.Models.DTO
{
    public class FullArticleDTO
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorNickName { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string PreviewTextHtml { get; set; }
        /// <summary>
        /// URL изображения для preview
        /// </summary>
        public string ImageURL { get; set; }
        public string FullTextHtml { get; set; }        
        public DateTime TimePublished { get; set; }
        public bool CommentsEnabled { get; set; }
        public int ReadingCount { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Hubs { get; set; } = new List<string>();
        public List<Guid> Likes { get; set; } = new List<Guid>();
        public List<Guid> Dislikes { get; set; } = new List<Guid>();

        /// <summary>
        /// номер статьи на хабре для отладки
        /// </summary>
        public int HubrId { get; set; }
    }
}
