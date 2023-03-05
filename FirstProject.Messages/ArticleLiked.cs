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
        /// Идентификатор сообщения
        /// </summary>
        public string Username { get; set; }


        public ArticleLiked(Guid articleId, string userId)
        {
            ArticleId = articleId;
            Username = userId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
