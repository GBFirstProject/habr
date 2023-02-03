using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models.Habr.Original
{
    [Table("Hub")]
    public class Hub
    {
        public string Id { get; set; }
        public string? Alias { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string? TitleHtml { get; set; }
        public bool IsProfiled { get; set; }
        //public object? RelatedData { get; set; }
        public string? RelatedData { get; set; }
        //поля, доступные на хабре, но, вероятно, не нужные в нашем проекте
        //public HubRefs hubRefs { get; set; }
        //public HubIds hubIds { get; set; }
        //public PagesCount pagesCount { get; set; }
        public bool IsLoading { get; set; }
        //public Route? Route { get; set; }
    }
}
