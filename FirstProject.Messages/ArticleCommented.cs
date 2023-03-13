using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.Messages
{ /// <summary>
  /// Класс сообщения о комментировании статьи
  /// </summary>
    public class ArticleCommented : MessageBase
    {

        /// <summary>
        /// Идентификатор статьи
        /// </summary>
        public Guid ArticleId { get; set; }
        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public string Username { get; set; }
    
        public ArticleCommented(Guid articleId, string username)
        {
            ArticleId = articleId;
            Username = username;
            CreatedAt = DateTime.UtcNow;
           
        }

        public ArticleCommented()
        {
            
        }
    }
}
