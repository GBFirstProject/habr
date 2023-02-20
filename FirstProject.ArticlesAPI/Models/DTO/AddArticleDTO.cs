using FirstProject.ArticlesAPI.Models.Enums;

namespace FirstProject.ArticlesAPI.Models.DTO
{
    public class AddArticleDTO
    {
        public Guid Id { get; set; }
        public int hubrId { get; set; }
        public string Title { get; set; }
        public ArticleLanguage Language { get; set; } = ArticleLanguage.Russian;
        public string TextHtml { get; set; }
        public DateTimeOffset? TimePublished { get; set; }
        public bool CommentsEnabled { get; set; }
        public Guid LeadDataId { get; set; }
        public Guid MetaDataId { get; set; }
        public int AuthorId { get; set; }
        public Guid StatisticsId { get; set; }
        public List<TagDTO> Tags { get; set; }
        public List<HubDTO> Hubs { get; set; }
    }
}