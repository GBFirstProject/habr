using System.ComponentModel.DataAnnotations;

namespace FirstProject.ArticlesAPI.Models
{
    public class Article
    {
        [Required]
        public int ArticleId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }

        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
    }
}
