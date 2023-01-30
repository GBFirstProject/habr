using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    [Table ("Users")]
    public class User
    {
        public Guid Id { get; set; }    
        public string Name { get; set; }
    }
}
