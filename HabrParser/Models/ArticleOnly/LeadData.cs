using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.ArticleOnly
{
    //[Keyless]
    [Table ("LeadData")]
    public class LeadData
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeadDataId { get; set; }
        [ForeignKey("ArticleId")]
        public int ArticleId { get; set; }
        public ParsedArticle ParsedArticle { get; set; }
        public string? TextHtml { get; set; }
        //public object? ImageUrl { get; set; }
        public string? ImageUrl { get; set; }
        //public object? ButtonTextHtml { get; set; }
        public string? ButtonTextHtml { get; set; }
        //public object? Image { get; set; }
        public string? Image { get; set; }        
    }
}
