﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.Messages
{
    public class ArticleApproved:MessageBase
    {
        public Guid ArticleId { get; set; }
        public ArticleApproved(Guid articleId)
        {

            ArticleId=articleId;

        }
    }
}
