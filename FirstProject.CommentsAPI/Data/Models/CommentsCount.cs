using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.CommentsAPI.Data.Models
{
    [Table("comments-count")]
    public class CommentsCount
    {
        [Key]
        public Guid ArticleId { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
