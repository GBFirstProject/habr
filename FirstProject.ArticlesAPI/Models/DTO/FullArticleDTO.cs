namespace FirstProject.ArticlesAPI.Models.DTO
{
    public class FullArticleDTO
    {
        public Guid Id { get; set; }
        public int AuthorId { get; set; }
        public string AuthorNickName { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string PreviewTextHtml { get; set; }
        public string FullTextHtml { get; set; }        
        public long TimePublished { get; set; }
        public bool CommentsEnabled { get; set; }
        public int ReadingCount { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Hubs { get; set; } = new List<string>();
    }
}
