using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    [Table("Hubs")]
    public class Hub
    {
        public string Alias { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
}
