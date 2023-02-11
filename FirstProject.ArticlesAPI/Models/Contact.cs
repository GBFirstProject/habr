using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    [Table("Contacts")]
    public class Contact : BaseModel<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public Author Author { get; set; }// = new Author();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Contact)) return false;
            var objToCompare = (Contact)obj;

            return Title.Equals(objToCompare.Title) &&
                   Url.Equals(objToCompare.Url);
        }
    }
}
