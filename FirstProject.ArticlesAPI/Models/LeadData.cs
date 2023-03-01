using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    [Table("LeadData")]
    public class LeadData : BaseModel<Guid>
    {
        public Article Article { get; set; }
        /// <summary>
        /// Сокращенное содержание статьи. Генерируется из первого абзаца с удалением лишних тэгов
        /// </summary>
        public string TextHtml { get; set; } = string.Empty;
        /// <summary>
        /// Надо генерировать изображение и ссылку, если не указано превью изображение
        /// </summary>
        public string? ImageUrl { get; set; } = string.Empty;
        /// <summary>
        /// не используется
        /// </summary>
        public string? Image { get; set; } = string.Empty;
    }
}
