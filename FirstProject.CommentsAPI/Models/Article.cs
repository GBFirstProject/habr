using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.CommentsAPI.Models
{
    public class Article
    {        
        public Guid ArticleId { get; set; }

        public Guid UserId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime PublishedAt { get; set; }

        public string? Link { get; set; }

        public string? ImageLink { get; set; }
        public List<Guid>? Likes { get; set; }
    }
}
