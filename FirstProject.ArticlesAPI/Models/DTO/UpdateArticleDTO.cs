namespace FirstProject.ArticlesAPI.Models.DTO
{
    public class UpdateArticleDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public Guid AuthorId { get; set; }
        public List<Guid> HubIds { get; set; }
        public List<string> Tags { get; set; }
        public string LeadImageUrl { get; set; }
        public string ShareImageUrl { get; set; }
    }

}
