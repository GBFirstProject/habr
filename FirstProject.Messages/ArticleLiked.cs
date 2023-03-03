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
    public class ArticleLiked 
    {
        /// <summary>
        /// Идентификатор статьи
        /// </summary>
        public Guid ArticleId { get; set; }
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Дата сообщения
        /// </summary>
        public DateTime CreatedAt { get; set; }

        public ArticleLiked(Guid articleId, Guid userId, DateTime createdAt)
        {
            ArticleId= articleId;
            UserId= userId;
            CreatedAt = createdAt;
        }
    }
}
