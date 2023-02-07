using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.CommentsAPI.Models
{
    [Table("comments")]
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ArticleId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public List<Guid> Likes { get; set; } = new();

        [Required]
        public List<Guid> Dislikes { get; set; } = new();
    }
}
