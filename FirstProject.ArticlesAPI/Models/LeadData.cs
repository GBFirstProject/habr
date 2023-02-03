using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    [Table("LeadData")]
    public class LeadData : BaseModel<Guid>
    {
        [Required]
        [ForeignKey("ArticleId")]
        public int ArticleId { get; set; }
        public Article Article { get; set; } = new Article();
        public string TextHtml { get; set; } = string.Empty;        
        public string ImageUrl { get; set; } = string.Empty;
    }
}
