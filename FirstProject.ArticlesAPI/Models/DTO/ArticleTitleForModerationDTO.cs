namespace FirstProject.ArticlesAPI.Models.DTO
{
    public class ArticleTitleForModerationDTO
    {
        public Guid AuthorId { get; set; }
        public Guid ArticleId { get; set; }
        public string Title { get; set; }
    }
}
