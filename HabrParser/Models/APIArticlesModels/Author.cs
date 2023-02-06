using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.APIArticles
{
    [Table("Authors")]
    public class Author : BaseModel<int>
    {
        public int hubrId { get; set; }
        [Required]
        public string NickName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;        
        public string LastName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        public string? Logo { get; set; }
        public string? Title { get; set; }
        public string? Link { get; set; }
        public List<Article> Articles { get; set; }
    }
}
