using HabrParser.Models.APIArticles;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.APIArticles
{
    [Table("Tags")]
    public class Tag : BaseModel<int>
    {        
        public string TagName { get; set; } = string.Empty;
        public List<Article> Articles { get; set; } = new List<Article>();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Tag)) return false;
            var objToCompare = (Tag)obj;
            return TagName.Equals(objToCompare.TagName);
        }
    }
}
