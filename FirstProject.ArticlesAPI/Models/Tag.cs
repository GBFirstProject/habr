using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    [Table("Tags")]
    public class Tag : BaseModel<int>
    {
        public string TitleHtml { get; set; } = string.Empty;
    }
}
