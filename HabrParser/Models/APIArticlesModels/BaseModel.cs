using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.APIArticles
{
    public abstract class BaseModel<T>    
    {
        [Required]
        [Key]//, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
    }
}
