using System.ComponentModel.DataAnnotations;

namespace FirstProject.Web.Models.Dto
{
    public class ArticleDto
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public long Data { get; set; }
        public int CategoryId { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
    }
}
