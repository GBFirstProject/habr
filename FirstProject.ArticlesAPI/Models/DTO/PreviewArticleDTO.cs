namespace FirstProject.ArticlesAPI.Models.DTO
{
    public class PreviewArticleDTO
    {
        public Guid Id { get; set; }
        public int AuthorId { get; set; }
        public string AuthorNickName { get; set; }
        public string Title { get; set; }        
        public string Text { get; set; }
        public long TimePublished { get; set; }
        public int ReadingCount { get; set; }
        public List<string> Tags { get; set; }
    }
}
