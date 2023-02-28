using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.CommentsAPI.Models
{
    [Table("comments")]
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ArticleId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public List<string> Likes { get; set; } = new();

        [Required]
        public List<string> Dislikes { get; set; } = new();

        public Guid ReplyTo { get; set; }
    }
}
