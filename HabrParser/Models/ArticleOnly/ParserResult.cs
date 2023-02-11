using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.ArticleOnly
{    
    [Table("ParserResult")]
    public class ParserResult
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParserResultId { get; set; }
        public int LastArtclelId { get; set; }
    }
}
