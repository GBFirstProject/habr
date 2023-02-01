using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.Messages
{ /// <summary>
  /// Интерфейс сообщения о комментировании статьи
  /// </summary>
    public interface IArticleCommented
    {/// <summary>
     /// Идентификатор сообщения
     /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// Идентификатор статьи
        /// </summary>
        public Guid ArticleId{ get; }

        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public Guid UserId{ get; }   
    }
}
