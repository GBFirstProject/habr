using HabrParser.Models.APIArticles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabrParser.Models.APIArticles
{
    [Table("Statistics")]
    public class Statistics : BaseModel<Guid>
    {
        public int CommentsCount { get; set; }
        public int FavoritesCount { get; set; }
        public int ReadingCount { get; set; }
        public int Score { get; set; }
        public int VotesCount { get; set; }
        public int VotesCountPlus { get; set; }
        public int VotesCountMinus { get; set; }
        public Article Article { get; set; }
    }
}
