using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    public class Article : BaseModel<Guid>
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = "ru";
        [Required]
        public string TextHtml { get; set; } = string.Empty;
        [Required]
        public DateTimeOffset? TimePublished { get; set; } = DateTime.UtcNow;
        public bool CommentsEnabled { get; set; } = true;
        public string ImageLink { get; set; } = string.Empty;
        public int? LeadDataId { get; set; }
        [ForeignKey("LeadDataId")]
        //public LeadData LeadData { get; set; }
        public int? AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        //public Author Author { get; set; }
        public List<Tag> Tags { get; set;} = new List<Tag>();
        
    }    
}
