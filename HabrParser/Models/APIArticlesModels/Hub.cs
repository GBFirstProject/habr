using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.APIArticles
{
    [Table("Hubs")]
    public class Hub : BaseModel<int>
    {
        public int hubrId { get; set; }
        public string Alias { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
