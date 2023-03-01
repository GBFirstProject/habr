using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    [Table("Hubs")]
    public class Hub : BaseModel<int>
    {
        public int hubrId { get; set; }
        /// <summary>
        /// псевдоним, как он указан в базе habr.com
        /// </summary>
        public string Alias { get; set; } = string.Empty;
        /// <summary>
        /// collective, corporative, etc.
        /// </summary>
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// Текст хаба непосредственно
        /// </summary>
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
        public override string ToString()
        {
            return Title;
        }
    }
}
