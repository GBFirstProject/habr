﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.Messages
{/// <summary>
/// Интерфейс для сообщения о лайке статьи
/// </summary>
    public interface IArticleLiked
    {
        /// <summary>
        /// Идентификатор статьи
        /// </summary>
        public Guid ArticleId{ get; }
        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public Guid UserId{ get;} 
    }
}
