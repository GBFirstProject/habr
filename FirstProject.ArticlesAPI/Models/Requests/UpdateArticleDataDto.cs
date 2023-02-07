namespace FirstProject.ArticlesAPI.Models.Requests
{
    public class UpdateArticleDataDto
    {
        public string Title { get; set; }        
        public string TextHtml { get; set; }
        public string ImageUrl { get; set; }        
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Hubs { get; set; } = new List<string>();
    }
}
