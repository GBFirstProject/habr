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

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Hub)) return false;
            var objToCompare = (Hub)obj;
            return Alias.Equals(objToCompare.Alias) &&
                  Type.Equals(objToCompare.Type) &&
                  Title.Equals(objToCompare.Title) &&
                  hubrId.Equals(objToCompare.hubrId);
        }
    }
}
