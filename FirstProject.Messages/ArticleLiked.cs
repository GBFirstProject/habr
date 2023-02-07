﻿using System;
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
        /// Идентификатор отправителя
        /// </summary>
        public Guid UserId { get; set; }
    }
}
