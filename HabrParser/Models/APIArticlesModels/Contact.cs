using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.APIArticles
{
    [Table ("Contacts")]
    public class Contact : BaseModel<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public Author Author { get; set; } = new Author();
    }
}
