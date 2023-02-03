using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models.Habr.Original
{
    [Table("Flow")]
    public class Flow
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Alias { get; set; }
        public string? Title { get; set; }
        public string? TitleHtml { get; set; }
        //public Route? Route { get; set; }
        //public Updates updates { get; set; }
        public List<Flow>? Flows { get; set; }
    }
}
