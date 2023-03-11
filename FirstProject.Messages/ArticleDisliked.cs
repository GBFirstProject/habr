using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.Messages
{  /// <summary>
   /// Класс сообщения о дизлайке статьи
   /// </summary>
    public class ArticleDisliked : MessageBase
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

        public ArticleDisliked(Guid articleId, string username, Guid articleAuthorId)
        {
            ArticleId = articleId;
            Username = username;
            CreatedAt = DateTime.UtcNow;
            ArticleAuthorId=articleAuthorId;
        }
    }
}
