using System.ComponentModel.DataAnnotations;

namespace FirstProject.ArticleAPI.Models
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public long Data { get; set; }
        [Required]
        public int CategoryId { get; set; }

        public string Content { get; set; }
        public string ImageUrl { get; set; }
    }
}
