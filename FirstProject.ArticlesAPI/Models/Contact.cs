using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    [Table ("Contacts")]
    public class Contact : BaseModel<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
