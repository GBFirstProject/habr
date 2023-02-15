namespace FirstProject.ArticlesAPI.Models.Requests
{
    public class CreateArticleRequest
    {
        public int AuthroId { get; set; }
        public string Title { get; set; }
        public string Language { get; set; } = "Russian";
        public string TextHtml { get; set; }
        public string ImageUrl { get; set; }
        public bool CommentsEnabled { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Hubs { get; set; } = new List<string>();

    }
}
