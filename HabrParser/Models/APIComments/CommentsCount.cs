using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.APIComments
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
