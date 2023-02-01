using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.Messages
{ /// <summary>
  /// Класс сообщения о комментировании статьи
  /// </summary>
    public class ArticleCommented : IArticleCommented
    {
        
        /// <summary>
        /// Идентификатор статьи
        /// </summary>
        public Guid ArticleId{ get; set; }
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public Guid UserId{ get; set; }

    }
}
