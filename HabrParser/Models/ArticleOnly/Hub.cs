using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.ArticleOnly
{
    [Table ("Hub")]
    public class Hub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HubId { get; set; }
        public int Id { get; set; }
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
