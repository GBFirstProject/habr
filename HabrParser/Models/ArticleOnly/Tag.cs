using HabrParser.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.ArticleOnly
{
    [Table ("Tag")]
    public class Tag
    {
        public Tag(int tagId, int id, string? titleHtml)
        {
            TagId = tagId;
            Id = id;
            TitleHtml = titleHtml;
        }
        public Tag(int tagId, ArticlesDBContext dBContext)
        {
            var existTag = dBContext.Tags.FirstOrDefault(t => t.TagId == tagId);
            if(existTag != null)
            {
                TagId = existTag.TagId;
                Id = existTag.Id;
                TitleHtml = existTag.TitleHtml;
            }
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }
        public int Id { get; set; }
        public string? TitleHtml { get; set; }        
    }
}
