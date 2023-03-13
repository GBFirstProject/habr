using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.Messages
{
    public class ArticleRejected:MessageBase
    {
        public Guid ArticleId { get; set; }
        public ArticleRejected(Guid articleId)
        {

            ArticleId=articleId;

        }
        public ArticleRejected()
        {
            
        }
    }
}
