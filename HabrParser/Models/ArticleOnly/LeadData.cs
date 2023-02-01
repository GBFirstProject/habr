using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.ArticleOnly
{
    //[Keyless]
    [Table ("LeadData")]
    public class LeadData
    {

        public int LeadDataId { get; set; }
        [ForeignKey(nameof(ParsedArticle))]
        public Guid ArticleId { get; set; }
        public string? TextHtml { get; set; }
        //public object? ImageUrl { get; set; }
        public string? ImageUrl { get; set; }
        //public object? ButtonTextHtml { get; set; }
        public string? ButtonTextHtml { get; set; }
        //public object? Image { get; set; }
        public string? Image { get; set; }
    }
}
