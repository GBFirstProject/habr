namespace FirstProject.ArticlesAPI.Models.DTO
{
    public class LikeResultDTO
    {
        public Guid ArticleId { get; set; }
        public List<Guid> Likes { get; set; } = new List<Guid>();
        public List<Guid> Dislikes { get; set; } = new List<Guid>();
    }
}
