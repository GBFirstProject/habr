using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    [Table("LeadData")]
    public class LeadData : BaseModel<Guid>
    {
        public Article Article { get; set; }
        public string TextHtml { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
    }
}
