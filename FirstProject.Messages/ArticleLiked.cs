using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.Messages
{
    /// <summary>
    /// Класс сообщения о лайке статьи
    /// </summary>
    public class ArticleLiked : MessageBase
    {
        /// <summary>
        /// Идентификатор статьи
        /// </summary>
        public Guid ArticleId { get; set; }
        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Идентификатор автора статьи
        /// </summary>
        public Guid ArticleAuthorId { get; set; }

        public ArticleLiked(Guid articleId, string userId, Guid articleAuthorId)
        {
            ArticleId = articleId;
            Username = userId;
            CreatedAt = DateTime.UtcNow;
            ArticleAuthorId=articleAuthorId;
        }
    }
}
