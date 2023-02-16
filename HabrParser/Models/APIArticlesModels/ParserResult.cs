using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.APIArticles
{    
    [Table("ParserResult")]
    public class ParserResult
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParserResultId { get; set; }
        public int LastArticleId { get; set; }
        /// <summary>
        /// ID для первых 50 000
        /// </summary>
        public int LastArticleId1 { get; set; }
        /// <summary>
        /// ID для первых 50 000 по 100 000
        /// </summary>
        public int LastArticleId50 { get; set; }
        /// <summary>
        /// ID для статей с 100 000 по 150 000
        /// </summary>
        public int LastArticleId100 { get; set; }
        /// <summary>
        /// ID для статей с 150 000 по 200 000
        /// </summary>
        public int LastArticleId150 { get; set; }
        /// <summary>
        /// ID для статей с 200 000 по 250 000
        /// </summary>
        public int LastArticleId200 { get; set; }
        /// <summary>
        /// ID для статей с 250 000 по 300 000
        /// </summary>
        public int LastArticleId250 { get; set; }
        /// <summary>
        /// ID для статей с 300 000 по 350 000
        /// </summary>
        public int LastArticleId300 { get; set; }
        /// <summary>
        /// ID для статей с 350 000 по 400 000
        /// </summary>
        public int LastArticleId350 { get; set; }
        /// <summary>
        /// ID для статей с 400 000 по 450 000
        /// </summary>
        public int LastArticleId400 { get; set; }
        /// <summary>
        /// ID для статей с 450 000 по 500 000
        /// </summary>
        public int LastArticleId450 { get; set; }
        /// <summary>
        /// ID для статей с 500 000 по 550 000
        /// </summary>
        public int LastArticleId500 { get; set; }
        /// <summary>
        /// ID для статей с 550 000 по 600 000
        /// </summary>
        public int LastArticleId550 { get; set; }
        /// <summary>
        /// ID для статей с 600 000 по 650 000
        /// </summary>
        public int LastArticleId600 { get; set; }
        /// <summary>
        /// ID для статей с 650 000 по 700 000
        /// </summary>
        public int LastArticleId650 { get; set; }
        /// <summary>
        /// ID для статей с 700 000 по 715 000
        /// </summary>
        public int LastArticleId700 { get; set; }
    }
}
