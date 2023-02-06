using HabrParser.Models.APIArticles;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.APIArticles
{
    [Table("Tags")]
    public class Tag : BaseModel<int>
    {        
        public string TagName { get; set; } = string.Empty;
        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
