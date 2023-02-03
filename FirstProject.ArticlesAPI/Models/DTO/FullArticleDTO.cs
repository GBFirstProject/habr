namespace FirstProject.ArticlesAPI.Models.DTO
{
    public class FullArticleDTO
    {
        public Guid Id { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string TextHtml { get; set; }
        public string ImageUrl { get; set; }
        public long TimePublished { get; set; }
        public bool CommentsEnabled { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Hubs { get; set; } = new List<string>();
    }
}
