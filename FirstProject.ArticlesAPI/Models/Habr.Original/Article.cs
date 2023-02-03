using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models.Habr.Original
{
    [Table("Articles")]
    public class Article
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Column]
        public DateTime? TimePublished { get; set; }
        [Column]
        public bool IsCorporative { get; set; }
        [Column]
        public string? Lang { get; set; }
        [Column]
        public string? TitleHtml { get; set; }
        public int? LeadDataId { get; set; }
        [ForeignKey("LeadDataId")]
        public LeadData? LeadData { get; set; }
        [Column]
        public string? EditorVersion { get; set; }
        [Column]
        public string? PostType { get; set; }
        //public List<object>? PostLabels { get; set; }
        //public List<string>? PostLabels { get; set; }
        [Column]
        public Author? Author { get; set; }
        [Column]
        public List<Hub>? Hubs { get; set; }
        [Column]
        public List<Flow>? Flows { get; set; }
        [Column]
        //public object? RelatedData { get; set; }
        public string? RelatedData { get; set; }
        [Column]
        public string? TextHtml { get; set; }
        [Column]
        public List<Tag>? Tags { get; set; }

        //поля, доступные на хабре, но, вероятно, не нужные в нашем проекте
        //public Statistics statistics { get; set; }
        //public Metadata metadata { get; set; }        
        //public List<object>? Polls { get; set; }
        //public List<string>? Polls { get; set; }
        [Column]
        public bool CommentsEnabled { get; set; }
        [Column]
        public bool RulesRemindEnabled { get; set; }
        [Column]
        public bool VotesEnabled { get; set; }
        [Column]
        public string? Status { get; set; }
        [Column]
        //public object? PlannedPublishTime { get; set; }
        public DateTime? PlannedPublishTime { get; set; }
        [Column]
        //public object? @Checked { get; set; }
        public bool? Checked { get; set; }
        [Column]
        public bool HasPinnedComments { get; set; }
        [Column]
        //public object? Format { get; set; }
        public string? Format { get; set; }
        [Column]
        public bool IsEditorial { get; set; }
        [Column]
        public string? ImageLink { get; set; }
        [Column]
        public List<User>? Likes { get; set; }
    }
}
