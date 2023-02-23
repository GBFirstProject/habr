using FirstProject.ArticlesAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    [Table("Articles")]
    public class Article : BaseModel<Guid>
    {
        public int hubrId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public ArticleLanguage Language { get; set; } = ArticleLanguage.Russian;
        [Required]
        public string TextHtml { get; set; } = string.Empty;
        [Required]
        public DateTimeOffset? TimePublished { get; set; } = DateTime.UtcNow;
        public bool CommentsEnabled { get; set; } = true;        
        public Guid LeadDataId { get; set; }
        [ForeignKey("LeadDataId")]
        public LeadData LeadData { get; set; } = new LeadData();
        public Guid MetaDataId { get; set; }
        [ForeignKey("MetaDataId")]
        public Metadata MetaData { get; set; } = new Metadata();
        public Guid AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; } = new Author();
        public Guid StatisticsId { get; set; }
        [ForeignKey("StatisticsId")]
        public Statistics Statistics { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Hub> Hubs { get; set; } = new List<Hub>();
        public bool IsPublished { get; set; } = false;
        /// <summary>
        /// последняя версия - в базе храним только ник автора, а самого автора в другом сервисе
        /// </summary>
        public string AuthorNickName { get; set; }

    }
}
