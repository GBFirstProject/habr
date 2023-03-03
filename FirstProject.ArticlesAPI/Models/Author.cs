using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    [Table("Authors")]
    public class Author : BaseModel<Guid>
    {
        public int hubrId { get; set; }        
        [Required]        
        public string NickName { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; } = string.Empty;
        public float Rating { get; set; } = 0;
        public List<Contact> Contacts { get; set; }// = new List<Contact>();
        public string? Logo { get; set; }
        public string? Title { get; set; }
        public string? Link { get; set; }
        public List<Article> Articles { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Author)) return false;
            var objToCompare = (Author)obj;
            return NickName.Equals(objToCompare.NickName) &&
                  FirstName.Equals(objToCompare.FirstName) &&
                  hubrId.Equals(objToCompare.hubrId);
        }
    }
}
