namespace FirstProject.ArticlesAPI.Models.DTO
{
    public class SearchPreviewResultDTO
    {
        public IEnumerable<PreviewArticleDTO> ResultData { get; set; }
        public int Count { get; set; }
    }
}
